using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.util;

namespace VGMToolbox.format.util
{
    public struct ExtractXaStruct
    {
        private string path;
        private bool addRiffHeader;
        private bool patchByte0x11;
        private uint silentFramesCount;

        public string Path
        {
            set { path = value; }
            get { return path; }
        }
        public bool AddRiffHeader
        {
            set { addRiffHeader = value; }
            get { return addRiffHeader; }
        }
        public bool PatchByte0x11
        {
            set { patchByte0x11 = value; }
            get { return patchByte0x11; }
        }
        public uint SilentFramesCount
        {
            set { silentFramesCount = value; }
            get { return silentFramesCount; }
        }

        public bool FilterAgainstBlockId
        {
            set;
            get;
        }

        public bool DoTwoPass { set; get;}

        public bool UseSilentBlocksForEof { set; get; }
        public bool UseEndOfTrackMarkerForEof { set; get; }
    }

    public struct CdxaWriterStruct
    {
        public FileStream FileWriter { set; get; }
        public long CurrentChunkOffset { set; get; }
        public bool NonSilentBlockDetected { set; get; }
    }

    public class CdxaUtil
    {
        const long EMPTY_BLOCK_OFFSET_FLAG = -1;
        const float MINIMUM_BLOCK_DISTANCE_PERCENTAGE = 0.15f;
        
        public static void ExtractXaFiles(ExtractXaStruct pExtractXaStruct)
        {
            Dictionary<UInt32, CdxaWriterStruct> bwDictionary = new Dictionary<UInt32, CdxaWriterStruct>();
            List<UInt32> bwKeys;

            long offset;
            byte[] trackId;            
            byte[] buffer = new byte[Cdxa.XA_BLOCK_SIZE];
            UInt32 trackKey;
            
            byte[] tempTrackId;
            UInt32 tempTrackKey;

            long previousOffset;
            long distanceBetweenBlocks;
            long distanceCeiling = EMPTY_BLOCK_OFFSET_FLAG;
            Dictionary<long, int> distanceFrequency = new Dictionary<long, int>();
            
            CdxaWriterStruct workingStruct = new CdxaWriterStruct();

            string outputFileName;
            string outputDirectory = Path.Combine(Path.GetDirectoryName(pExtractXaStruct.Path),
                Path.GetFileNameWithoutExtension(pExtractXaStruct.Path));

            int totalPasses = 1;
            bool doFileWrite = false;

            if (pExtractXaStruct.DoTwoPass)
            {
                totalPasses = 2; 
            }

            using (FileStream fs = File.OpenRead(pExtractXaStruct.Path))
            {
                for (int currentPass = 1; currentPass <= totalPasses; currentPass++)
                {
                    // turn on write flag
                    if (currentPass == totalPasses)
                    {
                        doFileWrite = true;
                    }
                    
                    // get first offset to start the party
                    offset = ParseFile.GetNextOffset(fs, 0, Cdxa.XA_SIG);

                    if (offset != -1) // we have found an XA sig
                    {
                        if (!Directory.Exists(outputDirectory))
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }

                        while ((offset != -1) && ((offset + Cdxa.XA_BLOCK_SIZE) <= fs.Length))
                        {
                            trackId = ParseFile.ParseSimpleOffset(fs, offset + Cdxa.XA_TRACK_OFFSET, Cdxa.XA_TRACK_SIZE);
                            trackKey = GetTrackKey(trackId);
                            //trackKey = ParseFile.ReadUintBE(fs, offset + Cdxa.XA_TRACK_OFFSET) & 0xFFFF0000;
                            
                            if (pExtractXaStruct.UseEndOfTrackMarkerForEof &&
                                (
                                  ((trackId[2] & Cdxa.XA_END_OF_TRACK_MARKER) == Cdxa.XA_END_OF_TRACK_MARKER) ||
                                  ((trackId[2] & Cdxa.XA_END_OF_AUDIO_MARKER) == Cdxa.XA_END_OF_AUDIO_MARKER)
                                ))
                            { 
                                // close the track
                                tempTrackId = trackId;
                                tempTrackId[2] = Cdxa.XA_CHUNK_ID_DIGITS;
                                tempTrackKey = GetTrackKey(tempTrackId);

                                if (bwDictionary.ContainsKey(tempTrackKey))
                                {                                    
                                    // close up this file, we're done.
                                    if (doFileWrite)
                                    {
                                        // write the block
                                        // set offset, doing this way because of boxing issues
                                        workingStruct = bwDictionary[tempTrackKey];
                                        workingStruct.CurrentChunkOffset = offset;
                                        bwDictionary[tempTrackKey] = workingStruct;

                                        // get the next block
                                        buffer = ParseFile.ParseSimpleOffset(fs, offset, Cdxa.XA_BLOCK_SIZE);

                                        // patch if needed
                                        if (pExtractXaStruct.PatchByte0x11)
                                        {
                                            buffer[0x11] = 0x00;
                                        }

                                        // write the block
                                        bwDictionary[tempTrackKey].FileWriter.Write(buffer, 0, buffer.Length);

                                        // set flag that a non-silent block was found
                                        if (!bwDictionary[tempTrackKey].NonSilentBlockDetected)
                                        {
                                            // doing this way because of boxing issues
                                            workingStruct = bwDictionary[tempTrackKey];
                                            workingStruct.NonSilentBlockDetected = true;
                                            bwDictionary[tempTrackKey] = workingStruct;
                                        }
                                        
                                        FixHeaderAndCloseWriter(bwDictionary[tempTrackKey].FileWriter, pExtractXaStruct,
                                            bwDictionary[tempTrackKey].NonSilentBlockDetected);
                                    }
                                    
                                    bwDictionary.Remove(tempTrackKey);
                                }

                                offset += Cdxa.XA_BLOCK_SIZE;
                            }
                            else if ((pExtractXaStruct.FilterAgainstBlockId) && 
                                     ((trackId[2] & 0x0F) != Cdxa.XA_AUDIO_MASK))
                            {
                                offset = ParseFile.GetNextOffset(fs, offset + 1, Cdxa.XA_SIG);
                            }
                            else
                            {
                                // check distance between blocks for possible split
                                if ((doFileWrite) &&
                                    (bwDictionary.ContainsKey(trackKey)) &&
                                    (distanceCeiling != EMPTY_BLOCK_OFFSET_FLAG) &&
                                    (bwDictionary[trackKey].CurrentChunkOffset != EMPTY_BLOCK_OFFSET_FLAG))
                                {
                                    previousOffset = bwDictionary[trackKey].CurrentChunkOffset;
                                    distanceBetweenBlocks = offset - previousOffset;

                                    if (distanceBetweenBlocks > distanceCeiling)
                                    {
                                        // close up this file, we're done.
                                        FixHeaderAndCloseWriter(bwDictionary[trackKey].FileWriter, pExtractXaStruct, 
                                            bwDictionary[trackKey].NonSilentBlockDetected);
                                        bwDictionary.Remove(trackKey);
                                    }
                                }                                
                                
                                // First Block only
                                if (!bwDictionary.ContainsKey(trackKey))
                                {
                                    outputFileName = GetOutputFileName(pExtractXaStruct, trackId);
                                    workingStruct = new CdxaWriterStruct();
                                    workingStruct.CurrentChunkOffset = EMPTY_BLOCK_OFFSET_FLAG;
                                    workingStruct.NonSilentBlockDetected = false;

                                    if (doFileWrite)
                                    {
                                        workingStruct.FileWriter = File.Open(outputFileName, FileMode.Create, FileAccess.ReadWrite);
                                    }
                                    
                                    bwDictionary.Add(trackKey, workingStruct);

                                    if (doFileWrite && pExtractXaStruct.AddRiffHeader)
                                    {
                                        bwDictionary[trackKey].FileWriter.Write(Cdxa.XA_RIFF_HEADER, 0, Cdxa.XA_RIFF_HEADER.Length);
                                    }
                                }

                                // calculate distance between blocks for possible split
                                if ((!doFileWrite) &&
                                    (bwDictionary[trackKey].CurrentChunkOffset != EMPTY_BLOCK_OFFSET_FLAG))
                                {
                                    previousOffset = bwDictionary[trackKey].CurrentChunkOffset;
                                    distanceBetweenBlocks = offset - previousOffset;

                                    if (!distanceFrequency.ContainsKey(distanceBetweenBlocks))
                                    {
                                        distanceFrequency.Add(distanceBetweenBlocks, 1);
                                    }
                                    else
                                    {
                                        distanceFrequency[distanceBetweenBlocks]++;
                                    }
                                }

                                // set offset, doing this way because of boxing issues
                                workingStruct = bwDictionary[trackKey];
                                workingStruct.CurrentChunkOffset = offset;
                                bwDictionary[trackKey] = workingStruct;

                                // get the next block
                                buffer = ParseFile.ParseSimpleOffset(fs, offset, Cdxa.XA_BLOCK_SIZE);
                                
                                // check if this is a "silent" block
                                if ((pExtractXaStruct.UseSilentBlocksForEof) && IsSilentBlock(buffer, pExtractXaStruct))
                                {
                                    if (doFileWrite)
                                    {
                                        // close up this file, we're done.
                                        FixHeaderAndCloseWriter(bwDictionary[trackKey].FileWriter, pExtractXaStruct,
                                            bwDictionary[trackKey].NonSilentBlockDetected);
                                    }
                                    
                                    bwDictionary.Remove(trackKey);
                                }
                                else if (doFileWrite)
                                {
                                    // patch if needed
                                    if (pExtractXaStruct.PatchByte0x11)
                                    {
                                        buffer[0x11] = 0x00;
                                    }

                                    // write the block
                                    bwDictionary[trackKey].FileWriter.Write(buffer, 0, buffer.Length);
                                    
                                    // set flag that a non-silent block was found
                                    if (!bwDictionary[trackKey].NonSilentBlockDetected)
                                    {
                                        // doing this way because of boxing issues
                                        workingStruct = bwDictionary[trackKey];
                                        workingStruct.NonSilentBlockDetected = true;
                                        bwDictionary[trackKey] = workingStruct;
                                    }
                                }

                                offset += Cdxa.XA_BLOCK_SIZE;
                            }
                        }

                        // fix header and close writers
                        bwKeys = new List<UInt32>(bwDictionary.Keys);
                        foreach (UInt32 keyname in bwKeys)
                        {
                            if (doFileWrite)
                            {
                                FixHeaderAndCloseWriter(bwDictionary[keyname].FileWriter, pExtractXaStruct,
                                    bwDictionary[keyname].NonSilentBlockDetected);
                            }
                            
                            bwDictionary.Remove(keyname);
                        }

                        // get statistical mode of distance between blocks
                        if (!doFileWrite)
                        {
                            distanceCeiling = GetDistanceCeiling(distanceFrequency);
                        }
                    }
                    else
                    {
                        // no CD-XA found
                        break;
                    }
                }
            }                
        }

        private static void FixHeaderAndCloseWriter(FileStream pFs, ExtractXaStruct pExtractXaStruct,
            bool nonSilentBlockFound)
        {
            string filename = pFs.Name;
            
            if (pExtractXaStruct.AddRiffHeader)
            {
                UInt32 xaFileSize = (UInt32)pFs.Length;

                // add file size
                pFs.Position = Cdxa.FILESIZE_OFFSET;
                pFs.Write(BitConverter.GetBytes(xaFileSize - 8), 0, 4);

                // add data size
                pFs.Position = Cdxa.DATA_LENGTH_OFFSET;
                pFs.Write(BitConverter.GetBytes((uint)(xaFileSize - Cdxa.XA_RIFF_HEADER.Length)), 0, 4);
            }

            pFs.Close();
            pFs.Dispose();

            if (!nonSilentBlockFound)
            {
                File.Delete(filename);
            }
        }

        private static bool IsSilentBlock(byte[] pCdxaBlock, ExtractXaStruct pExtractXaStruct)
        {
            bool ret = false;
            int silentFrameCount = 0;
            long bufferOffset = 0;

            while ((bufferOffset = ParseFile.GetNextOffset(pCdxaBlock, bufferOffset, Cdxa.XA_SILENT_FRAME)) > -1)
            {
                silentFrameCount++;
                bufferOffset += 1;
            }

            // did we find enough silent frames?
            if (silentFrameCount >= pExtractXaStruct.SilentFramesCount)
            {
                ret = true;
            }

            return ret;
        }

        private static string GetOutputFileName(ExtractXaStruct pExtractXaStruct, byte[] pTrackId)
        {
            string outputDirectory = Path.Combine(Path.GetDirectoryName(pExtractXaStruct.Path),
                Path.GetFileNameWithoutExtension(pExtractXaStruct.Path));
            string outputFileName = Path.GetFileNameWithoutExtension(pExtractXaStruct.Path) + "_" + ParseFile.ByteArrayToString(pTrackId) + Cdxa.XA_FILE_EXTENSION;

            int fileCount = Directory.GetFiles(outputDirectory, String.Format("{0}*", Path.GetFileNameWithoutExtension(outputFileName)), SearchOption.TopDirectoryOnly).Length;
            outputFileName = String.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(outputFileName), fileCount.ToString("X4"), Cdxa.XA_FILE_EXTENSION);

            //if (outputFileName.Equals("BGM2_01016401_0005.xa"))
            //{
            //    int x = 1;
            //}

            string ret = Path.Combine(outputDirectory, outputFileName);
            
            return ret;
        }

        private static long GetDistanceCeiling(Dictionary<long, int> distanceFrequencyList)
        {
            long distanceCeiling = EMPTY_BLOCK_OFFSET_FLAG;
            long totalBlockCount = 0;

            // get total number of blocks found (not entirely correct, because of the first one skipped)
            foreach (long key in distanceFrequencyList.Keys)
            {
                totalBlockCount += distanceFrequencyList[key];
            }

            // determine max value based on percentage of hits
            foreach (long key in distanceFrequencyList.Keys)
            {
                if ((key > distanceCeiling) &&
                    ((((float)distanceFrequencyList[key]/(float)totalBlockCount) >= MINIMUM_BLOCK_DISTANCE_PERCENTAGE)))
                {
                    distanceCeiling = key;
                }
            }

            return distanceCeiling;
        }

        private static uint GetTrackKey(byte[] trackIdBytes)
        {
            uint ret = 0;

            //ret |= (uint)(trackIdBytes[0] << 24);
            //ret |= (uint)(trackIdBytes[0] << 16);

            ret = BitConverter.ToUInt32(trackIdBytes, 0);
            ret &= 0xFF00FFFF;



            return ret;
        }
    }
}
