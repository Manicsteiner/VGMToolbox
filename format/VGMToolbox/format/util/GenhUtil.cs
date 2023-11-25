using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format.util
{
    public struct GenhCreationStruct
    {
        public bool doCreation;
        public bool doEdit;
        public bool doExtract;
        
        public string Format;
        public string HeaderSkip;

        public string Interleave;
        public bool UseInterleaveOffset { set; get; }
        public OffsetDescription InterleaveOffsetDescription { set; get; }

        public string Channels;
        public bool UseChannelsOffset { set; get; }
        public OffsetDescription ChannelsOffsetDescription { set; get; }

        public string Frequency;
        public bool UseFrequencyOffset { set; get; }
        public OffsetDescription FrequencyOffsetDescription { set; get; }

        public string LoopStart;
        public bool UseLoopStartOffset { set; get; }
        public OffsetDescription LoopStartOffsetDescription { set; get; }
        public bool DoLoopStartBytesToSamples { set; get; }

        public string LoopEnd;
        public bool UseLoopEndOffset { set; get; }
        public OffsetDescription LoopEndOffsetDescription { set; get; }
        public bool DoLoopEndBytesToSamples { set; get; }

        public string TotalSamples;
        public bool UseTotalSamplesOffset { set; get; }
        public OffsetDescription TotalSamplesOffsetDescription { set; get; }
        public bool DoTotalSamplesBytesToSamples { set; get; }

        public bool NoLoops;
        public bool UseFileEnd;
        public bool FindLoop;

        public string SkipSamples;
        public byte SkipSamplesMode;        

        public byte Atrac3StereoMode;
        public byte XmaStreamMode;
        public string RawDataSize;

        public string CoefRightChannel;
        public string CoefLeftChannel;
        public byte CoefficientType;

        public bool OutputHeaderOnly;
        public string[] SourcePaths;
    }

    public sealed class GenhUtil
    {
        public const int UNKNOWN_SAMPLE_COUNT = -1;
        private const int SONY_ADPCM_LOOP_HACK_BYTE_COUNT = 0x30;
        private const string GENH_BATCH_FILE_NAME = "vgmt_genh_copy.bat";

        private GenhUtil() { }

        public static bool IsGenhFile(string path)
        {
            bool ret = false;

            using (FileStream typeFs = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                Type dataType = FormatUtil.getObjectType(typeFs);

                if (dataType != null && dataType.Name.Equals("Genh"))
                {
                    ret = true;
                }
            }

            return ret;
        }
        
        public static string CreateGenhFile(string pSourcePath, GenhCreationStruct pGenhCreationStruct)
        {
            string ret = String.Empty;
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            long testCoefficient;
 
            int dspInterleaveType =
                GetDspInterleave(pGenhCreationStruct.Interleave, pGenhCreationStruct.Channels);

            //--------------
            // looping info
            //--------------
            if (pGenhCreationStruct.NoLoops)
            {
                pGenhCreationStruct.LoopStart = Genh.EMPTY_SAMPLE_COUNT;
                pGenhCreationStruct.LoopEnd = Genh.EMPTY_SAMPLE_COUNT;
            }
            else if (pGenhCreationStruct.UseFileEnd)
            {
                pGenhCreationStruct.LoopEnd = GetTotalSamples(pSourcePath, pGenhCreationStruct);
            }
            else if (pGenhCreationStruct.FindLoop)
            {
                string loopStartFound;
                string loopEndFound;

                if (((pGenhCreationStruct.Format.Equals("0")) || (pGenhCreationStruct.Format.Equals("14"))) &&
                    (GetPsAdpcmLoop(pSourcePath, pGenhCreationStruct, out loopStartFound,
                        out loopEndFound)))
                {
                    pGenhCreationStruct.LoopStart = loopStartFound;
                    pGenhCreationStruct.LoopEnd = loopEndFound;
                }
                else
                {
                    pGenhCreationStruct.LoopStart = Genh.EMPTY_SAMPLE_COUNT;
                    pGenhCreationStruct.LoopEnd = GetTotalSamples(pSourcePath, pGenhCreationStruct);
                }
            }

            // set total samples if no loop end found
            if (pGenhCreationStruct.LoopEnd == Genh.EMPTY_SAMPLE_COUNT)
            {
                pGenhCreationStruct.TotalSamples = GetTotalSamples(pSourcePath, pGenhCreationStruct);
            }

            //---------------------
            // offset based values
            //---------------------
            if (pGenhCreationStruct.UseLoopStartOffset || 
                pGenhCreationStruct.UseLoopEndOffset ||
                pGenhCreationStruct.UseInterleaveOffset ||
                pGenhCreationStruct.UseChannelsOffset ||
                pGenhCreationStruct.UseFrequencyOffset ||
                pGenhCreationStruct.UseTotalSamplesOffset)
            {
                int formatId = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Format);
                long interleave = VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Interleave);
                long channels = VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Channels);

                using (FileStream inputFs = File.Open(pSourcePath, FileMode.Open, FileAccess.Read))
                {
                    //------------
                    // Interleave
                    //------------
                    if (pGenhCreationStruct.UseInterleaveOffset)
                    {
                        interleave = ParseFile.GetVaryingByteValueAtAbsoluteOffset(inputFs, pGenhCreationStruct.InterleaveOffsetDescription);
                        pGenhCreationStruct.Interleave = ((int)interleave).ToString();
                    }

                    //----------
                    // Channels
                    //----------
                    if (pGenhCreationStruct.UseChannelsOffset)
                    {
                        channels = ParseFile.GetVaryingByteValueAtAbsoluteOffset(inputFs, pGenhCreationStruct.ChannelsOffsetDescription);
                        pGenhCreationStruct.Channels = ((int)channels).ToString();
                    }

                    //-----------
                    // Frequency
                    //-----------
                    if (pGenhCreationStruct.UseFrequencyOffset)
                    {
                        long frequency = ParseFile.GetVaryingByteValueAtAbsoluteOffset(inputFs, pGenhCreationStruct.FrequencyOffsetDescription);
                        pGenhCreationStruct.Frequency = ((int)frequency).ToString();
                    }

                    //------------
                    // Loop Start
                    //------------
                    if (pGenhCreationStruct.UseLoopStartOffset)
                    {
                        long loopStart = ParseFile.GetVaryingByteValueAtAbsoluteOffset(inputFs, pGenhCreationStruct.LoopStartOffsetDescription);

                        if (pGenhCreationStruct.DoLoopStartBytesToSamples)
                        {
                            loopStart = BytesToSamples(formatId, (int)loopStart, (int)channels, (int)interleave);
                        }

                        pGenhCreationStruct.LoopStart = ((int)loopStart).ToString();
                    }

                    //----------
                    // Loop End
                    //----------
                    if (pGenhCreationStruct.UseLoopEndOffset)
                    {
                        long loopEnd = ParseFile.GetVaryingByteValueAtAbsoluteOffset(inputFs, pGenhCreationStruct.LoopEndOffsetDescription);

                        if (pGenhCreationStruct.DoLoopEndBytesToSamples)
                        {
                            loopEnd = BytesToSamples(formatId, (int)loopEnd, (int)channels, (int)interleave);
                        }
                        
                        pGenhCreationStruct.LoopEnd = ((int)loopEnd).ToString();
                    }

                    //---------------
                    // Total Samples 
                    //---------------
                    if (pGenhCreationStruct.UseTotalSamplesOffset)
                    {
                        long totalSamples = ParseFile.GetVaryingByteValueAtAbsoluteOffset(inputFs, pGenhCreationStruct.TotalSamplesOffsetDescription);

                        if (pGenhCreationStruct.DoTotalSamplesBytesToSamples)
                        {
                            totalSamples = BytesToSamples(formatId, (int)totalSamples, (int)channels, (int)interleave);
                        }

                        pGenhCreationStruct.TotalSamples = ((int)totalSamples).ToString();
                    }
                }
            }

            FileInfo fi = new FileInfo(Path.GetFullPath(pSourcePath));
            UInt32 fileLength = (UInt32)fi.Length;
            string outputFilePath;

            if (pGenhCreationStruct.OutputHeaderOnly)
            {
                outputFilePath = Path.ChangeExtension(pSourcePath, Genh.FILE_EXTENSION_HEADER);
            }
            else
            {
                outputFilePath = Path.ChangeExtension(pSourcePath, Genh.FILE_EXTENSION);
            }
            
            // write the file
            using (FileStream outputFs = File.Open(outputFilePath, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(outputFs))
                {
                    bw.Write(Genh.ASCII_SIGNATURE);
                    bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Channels));
                    bw.Write((Int32)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Interleave));
                    bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Frequency));
                    bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.LoopStart));

                    if (!String.IsNullOrEmpty(pGenhCreationStruct.LoopEnd))
                    {
                        bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.LoopEnd));
                    }
                    else
                    {
                        bw.Write(new byte[] {0x00, 0x00, 0x00, 0x00});
                    }

                    bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Format));
                    bw.Write((UInt32)(VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.HeaderSkip) + Genh.GENH_HEADER_SIZE));
                    bw.Write((UInt32)Genh.GENH_HEADER_SIZE);

                    if (VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Format) == 12)
                    {
                        //testCoefficient = VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.CoefLeftChannel) + Genh.GENH_HEADER_SIZE;
                        
                        bw.Write((UInt32)(VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.CoefLeftChannel) + Genh.GENH_HEADER_SIZE));
                        bw.Write((UInt32)(VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.CoefRightChannel) + Genh.GENH_HEADER_SIZE));
                        bw.Write((UInt32)dspInterleaveType);
                        bw.Write((UInt32)pGenhCreationStruct.CoefficientType);

                        if (pGenhCreationStruct.CoefficientType == 1 || pGenhCreationStruct.CoefficientType == 3) // split coefficients (aka Capcom Hack)
                        {
                            bw.Write((UInt32)(VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.CoefLeftChannel) + Genh.GENH_HEADER_SIZE + 0x10));
                            bw.Write((UInt32)(VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.CoefRightChannel) + Genh.GENH_HEADER_SIZE + 0x10));
                        }
                        else
                        {
                            bw.Write(new byte[] { 0x00 });
                        }
                    }

                    // Total Samples
                    if (!String.IsNullOrWhiteSpace(pGenhCreationStruct.TotalSamples))
                    {
                        bw.BaseStream.Position = Genh.TOTAL_SAMPLES_OFFSET;
                        bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.TotalSamples));
                    }

                    // Skip Samples
                    if ((pGenhCreationStruct.SkipSamplesMode != Genh.SKIP_SAMPLES_MODE_AUTODETECT) && 
                        !String.IsNullOrWhiteSpace(pGenhCreationStruct.SkipSamples))
                    {
                        bw.BaseStream.Position = Genh.SKIP_SAMPLES_OFFSET;
                        bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.SkipSamples));

                        // Skip Samples Mode
                        bw.BaseStream.Position = Genh.SKIP_SAMPLES_MODE_OFFSET;
                        bw.Write((byte)pGenhCreationStruct.SkipSamplesMode);
                    }

                    // ATRAC3 Stereo Mode
                    bw.BaseStream.Position = Genh.ATRAC3_STEREO_MODE_OFFSET;
                    bw.Write((byte)pGenhCreationStruct.Atrac3StereoMode);

                    // XMA Stream Mode
                    bw.BaseStream.Position = Genh.XMA_STREAM_MODE_OFFSET;
                    bw.Write((byte)pGenhCreationStruct.XmaStreamMode);

                    // RAW DATA SIZE
                    bw.BaseStream.Position = Genh.RAW_DATA_SIZE_OFFSET;
                    bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.RawDataSize == null ? "0" : pGenhCreationStruct.RawDataSize));

                    // Original File Size
                    bw.BaseStream.Position = Genh.ORIG_FILENAME_OFFSET;
                    bw.Write(enc.GetBytes(Path.GetFileName(pSourcePath).Trim()));

                    bw.BaseStream.Position = Genh.ORIG_FILESIZE_OFFSET;
                    bw.Write(fileLength);

                    // GENH Version
                    bw.BaseStream.Position = Genh.GENH_VERSION_OFFSET;
                    bw.Write(Genh.CURRENT_VERSION);

                    bw.BaseStream.Position = 0xFFC;
                    bw.Write(0x0);

                    // create batch script or add input file
                    if (pGenhCreationStruct.OutputHeaderOnly)
                    {
                        AddCopyItemToBatchFile(Path.GetDirectoryName(pSourcePath), pSourcePath, outputFilePath);
                    }
                    else
                    {
                        using (FileStream inputFs = File.Open(pSourcePath, FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader br = new BinaryReader(inputFs))
                            {
                                byte[] data = new byte[Constants.FileReadChunkSize];
                                int bytesRead = 0;

                                while ((bytesRead = br.Read(data, 0, data.Length)) > 0)
                                {
                                    bw.Write(data, 0, bytesRead);
                                }
                            }
                        }                        
                    }
                }

                ret = outputFilePath;
            }

            return ret;
        }

        public static int GetDspInterleave(string pGenhInterleave, string GenhChannels)
        {
            int dspInterleave = 0;
            int genhInterleave = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhInterleave);
            int genhChannels = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(GenhChannels);

            // Calculating the Interleave Type for DSP
            if (genhInterleave >= 8)
            {
                dspInterleave = 0; // Normal Interleave Layout
            }

            if (genhInterleave <= 7)
            {
                dspInterleave = 1; // Sub-FrameInterleave
            }

            if (genhChannels == 1)
            {
                dspInterleave = 2; // No layout (mono fies or whatever
            }

            return dspInterleave;
        }

        public static bool CanConvertBytesToSamples(long pFormatId)
        {
            bool ret = true;

            if (pFormatId == 8 ||  // MPEG
                pFormatId == 20 || // XMA
                pFormatId == 21 || // XMA2
                pFormatId == 22)   // FFMPEG
            {
                ret = false;
            }

            return ret;
        }

        public static bool CanConvertBytesToSamples(string pFormatId)
        {
            long value = Convert.ToInt32(pFormatId);
            return CanConvertBytesToSamples(value);
        }

        public static string GetTotalSamples(string pSourcePath, GenhCreationStruct pGenhCreationStruct)
        {
            int formatId = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Format);
            int headerSkip = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.HeaderSkip);
            int channels = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Channels);
            int interleave = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Interleave);
            int loopEnd = -1;

            FileInfo fi = new FileInfo(Path.GetFullPath(pSourcePath));
            int fileLength = (int)fi.Length;
            
            loopEnd = BytesToSamples(formatId, (fileLength - headerSkip), channels, interleave);
            
            return loopEnd.ToString();
        }

        public static int BytesToSamples(int formatId, int byteValue, int channels, int interleave)
        { 
            int sampleCount = UNKNOWN_SAMPLE_COUNT;
            int frames, lastFrame;

            switch (formatId)
            {
                case 0x00: // "0x00 - PlayStation 4-bit ADPCM"
                case 0x0E: // "0x0E - PlayStation 4-bit ADPCM (with bad flags)
                    sampleCount = (byteValue / 16 / channels * 28);
                    break;

                case 0x01: // "0x01 - XBOX 4-bit IMA ADPCM"
                    sampleCount = (byteValue / 36 / channels * 64);
                    break;

                case 0x02: // "0x02 - GameCube ADP/DTK 4-bit ADPCM"
                    sampleCount = (byteValue / 32 * 28);
                    break;

                case 0x03: // "0x03 - PCM RAW (Big Endian)"
                case 0x04: // "0x04 - PCM RAW (Little Endian)
                    sampleCount = (byteValue / 2 / channels);
                    break;

                case 0x05: // "0x05 - PCM RAW (8-Bit)"
                case 0x0D: // "0x0D - PCM RAW (8-Bit unsigned)"
                    sampleCount = (byteValue / channels);
                    break;

                case 0x06: // "0x06 - Squareroot-delta-exact 8-bit DPCM"
                    sampleCount = (byteValue / 1 / channels);
                    break;

                case 0x08: // "0x08 - MPEG Layer Audio File (MP1/2/3)"
                    // Not Implemented
                    break;

                case 0x07: // "0x07 - Interleaved DVI 4-Bit IMA ADPCM"
                case 0x09: // "0x09 - 4-bit IMA ADPCM"
                case 0x0A: // "0x0A - Yamaha AICA 4-bit ADPCM"    
                case 0x11: // "0x11 - Apple Quicktime 4-bit IMA ADPCM"    
                    sampleCount = (byteValue / channels * 2);
                    break;

                case 0x0B: // 0x0B - Microsoft 4-bit ADPCM
                    frames = byteValue / interleave;
                    lastFrame = byteValue - (frames * interleave);
                    sampleCount = (frames * (interleave - (14 - 2))) + (lastFrame - (14 - 2));
                    break;

                case 0x0C: // "0x0C - Nintendo GameCube DSP 4-bit ADPCM"
                    sampleCount = (byteValue / channels / 8 * 14);
                    break;

                case 0x0F: // "0x0F - Microsoft 4-bit IMA ADPCM"
                    sampleCount = (byteValue / 0x800) * (0x800 - 4 * channels) * 2 / channels + ((byteValue % 0x800) != 0 ? (byteValue % 0x800 - 4 * channels) * 2 / channels : 0);
                    break;

                case 0x12: // 0x12 - ATRAC3 
                    sampleCount = (byteValue / interleave) * 1024;
                    break;

                case 0x13: // 0x13 - ATRAC3+ 
                    sampleCount = (byteValue / interleave) * 2048;
                    break;

                default:
                    sampleCount = UNKNOWN_SAMPLE_COUNT;
                    break;

            }

            return sampleCount;
        }

        public static bool GetPsAdpcmLoop(string pSourcePath, 
            GenhCreationStruct pGenhCreationStruct, out string pLoopStart, out string pLoopEnd)
        {
            bool ret = false;
            pLoopStart = Genh.EMPTY_SAMPLE_COUNT;
            pLoopEnd = Genh.EMPTY_SAMPLE_COUNT;

            long loopStart = -1;
            long loopEnd = -1;
            
            long loopCheckBytesOffset;
            byte[] possibleLoopBytes;

            long headerSkip = VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.HeaderSkip);
            int channels = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Channels);
            int interleave = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(pGenhCreationStruct.Interleave);
            string fullIncomingPath = Path.GetFullPath(pSourcePath);

            byte[] checkByte = new byte[1];

            FileInfo fi = new FileInfo(Path.GetFullPath(fullIncomingPath));
            if ((fi.Length - headerSkip) % 0x10 != 0)
            { 
                throw new Exception(String.Format("Error processing <{0}> Length of file minus the header skip value is not divisible by 0x10.  This is not a proper PS ADPCM rip.", fullIncomingPath));
            }
            
            using (BinaryReader br = new BinaryReader(File.OpenRead(fullIncomingPath)))            
            {   
                // Loop Start
                br.BaseStream.Position = headerSkip + 0x01;
                
                while (br.BaseStream.Position < fi.Length)
                {
                    br.Read(checkByte, 0, checkByte.Length);

                    if (checkByte[0] == 0x06)
                    {
                        loopStart = br.BaseStream.Position - 2 - headerSkip;

                        break;
                    }
                    else
                    {
                        br.BaseStream.Position += 0x10 - 0x01;
                    }
                }

                // Loop End
                br.BaseStream.Position = fi.Length - 0x0F;
                
                while (br.BaseStream.Position > headerSkip)
                {
                    br.Read(checkByte, 0, checkByte.Length);

                    if (checkByte[0] == 0x03)
                    {
                        loopEnd = br.BaseStream.Position + 0x0E - headerSkip;

                        //if (channels > 1)
                        //{
                        //    loopEnd -= interleave;
                        //}

                        break;
                    }
                    else if (br.BaseStream.Position >= 0x11)
                    {
                        br.BaseStream.Position -= 0x11;
                    }
                    else
                    {
                        br.BaseStream.Position = headerSkip;
                    }
                }

                // if loop end found but start not found, try alternate method
                if ((loopEnd >= 0) && (loopStart < 0))
                {
                    loopCheckBytesOffset = loopEnd + headerSkip - SONY_ADPCM_LOOP_HACK_BYTE_COUNT;
                    possibleLoopBytes = ParseFile.ParseSimpleOffset(
                                                br.BaseStream,
                                                loopCheckBytesOffset, 
                                                SONY_ADPCM_LOOP_HACK_BYTE_COUNT - 0x10);
                    loopStart = ParseFile.GetNextOffset(br.BaseStream, 0, possibleLoopBytes, true);

                    if ((loopStart > 0) && (loopStart < loopCheckBytesOffset))
                    {
                        loopStart += SONY_ADPCM_LOOP_HACK_BYTE_COUNT - headerSkip;
                    }

                }
            }
            
            if (loopStart >= 0)
            {
                pLoopStart = (loopStart / 16 / channels * 28).ToString();
                ret = true;
            }
            
            if (loopEnd >= 0)
            {
                pLoopEnd = (loopEnd / 16 / channels * 28).ToString();
                ret = true;
            }

            return ret;
        }

        public static string ExtractGenhFile(string pSourcePath, bool extractHeaderToFile, bool outputExtractionLog, bool outputExtractionFile)
        {
            string outputFileName = null;
            string headerOutputFileName = null;

            if (IsGenhFile(pSourcePath))
            {
                using (FileStream fs = File.Open(pSourcePath, FileMode.Open, FileAccess.Read))
                {
                    Genh genhFile = new Genh();
                    genhFile.Initialize(fs, pSourcePath);
                    Int32 headerLength = BitConverter.ToInt32(genhFile.HeaderLength, 0);
                    Int32 originalFileSize = BitConverter.ToInt32(genhFile.OriginalFileSize, 0);
                    string originalFileName = System.Text.Encoding.ASCII.GetString(genhFile.OriginalFileName);
                    
                    outputFileName = Path.Combine(Path.GetDirectoryName(pSourcePath), originalFileName).Trim();                    
                    ParseFile.ExtractChunkToFile(fs, headerLength, originalFileSize, outputFileName, outputExtractionLog, outputExtractionFile);                    

                    if (extractHeaderToFile)
                    {
                        headerOutputFileName = Path.Combine(Path.GetDirectoryName(pSourcePath), String.Format("{0}{1}", originalFileName.Trim(), Genh.FILE_EXTENSION_HEADER)).Trim();
                        ParseFile.ExtractChunkToFile(fs, 0, headerLength, headerOutputFileName, outputExtractionLog, outputExtractionFile);
                    }

                    FileInfo fi = new FileInfo(outputFileName);
                    if (fi.Length != (long)originalFileSize)
                    {
                        throw new IOException(String.Format("Extracted file <{0}> size did not match size in header of <{1}>: 0x{2}{3}", outputFileName, 
                            pSourcePath, originalFileSize.ToString("X8"), Environment.NewLine));
                    }
                }            
            }

            return outputFileName;
        }

        public static string AddCopyItemToBatchFile(string destinationFolder, string originalFileName,
            string headerFileName)
        {
            string batchOutputPath = Path.Combine(destinationFolder, GENH_BATCH_FILE_NAME);
            string copyStatementFormat = "copy /b \"{0}\" + \"{1}\" \"{2}\"";

            using (StreamWriter sw = new StreamWriter(File.Open(batchOutputPath, FileMode.Append, FileAccess.Write)))
            {
                sw.WriteLine(String.Format(copyStatementFormat, Path.GetFileName(headerFileName), Path.GetFileName(originalFileName), Path.GetFileName(Path.ChangeExtension(originalFileName, Genh.FILE_EXTENSION))));
            }

            return batchOutputPath;
        }

        public static GenhCreationStruct GetGenhCreationStruct(Genh genhItem)
        { 
            GenhCreationStruct gcStruct = new GenhCreationStruct();

            int formatValue = BitConverter.ToInt32(genhItem.Identifier, 0);
            gcStruct.Format = formatValue.ToString();
            gcStruct.HeaderSkip = "0x" + ((BitConverter.ToInt32(genhItem.AudioStart, 0) - BitConverter.ToInt32(genhItem.HeaderLength, 0))).ToString("X4");
            gcStruct.Interleave = "0x" + BitConverter.ToInt32(genhItem.Interleave, 0).ToString("X4");
            gcStruct.Channels = BitConverter.ToInt32(genhItem.Channels, 0).ToString();        
            gcStruct.Frequency = BitConverter.ToInt32(genhItem.Frequency, 0).ToString();
            gcStruct.TotalSamples = BitConverter.ToInt32(genhItem.TotalSamples, 0).ToString();

            gcStruct.SkipSamplesMode = genhItem.SkipSamplesMode;
            gcStruct.SkipSamples = BitConverter.ToInt32(genhItem.SkipSamples, 0).ToString();

            gcStruct.Atrac3StereoMode = genhItem.Atrac3StereoMode;
            gcStruct.XmaStreamMode = genhItem.XmaStreamMode;
            gcStruct.RawDataSize = BitConverter.ToInt32(genhItem.RawStreamSize, 0).ToString();

            int loopStartValue = BitConverter.ToInt32(genhItem.LoopStart, 0);
            if (loopStartValue > -1)
            {
                gcStruct.LoopStart = loopStartValue.ToString();
            }
            else
            {
                gcStruct.LoopStart = String.Empty;          
            }

            int loopEndValue = BitConverter.ToInt32(genhItem.LoopEnd, 0);
            if (loopEndValue > 0)
            {
                gcStruct.LoopEnd = loopEndValue.ToString();
                gcStruct.UseLoopEndOffset = true;
            }
            else
            {
                gcStruct.LoopEnd = String.Empty;
                gcStruct.UseLoopEndOffset = false;            
            }

            if (formatValue == 12)
            {
                gcStruct.CoefficientType = genhItem.CoefficientType[0];

                if (gcStruct.CoefficientType == 1 || gcStruct.CoefficientType == 3) // Split Coefficients (aka Capcom Hack)
                {
                    gcStruct.CoefRightChannel = "0x" + (BitConverter.ToUInt32(genhItem.RightCoef, 0) - Genh.GENH_HEADER_SIZE - 0x10).ToString("X4");
                    gcStruct.CoefLeftChannel = "0x" + (BitConverter.ToUInt32(genhItem.LeftCoef, 0) - Genh.GENH_HEADER_SIZE - 0x10).ToString("X4");
                }
                else
                {
                    gcStruct.CoefRightChannel = "0x" + (BitConverter.ToUInt32(genhItem.RightCoef, 0) - Genh.GENH_HEADER_SIZE).ToString("X4");
                    gcStruct.CoefLeftChannel = "0x" + (BitConverter.ToUInt32(genhItem.LeftCoef, 0) - Genh.GENH_HEADER_SIZE).ToString("X4");                
                }
            }
            
            return gcStruct;
        }
    }
}
