using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class BinkStream
    {
        public enum BinkType { Version01, Version02 };

        const string DefaultFileExtensionAudioMulti = ".audio.multi.bik";
        const string DefaultFileExtensionAudioSplit = ".audio.split.bik";
        const string DefaultFileExtensionVideo = ".video.bik";

        public uint FrameCount { set; get; }
        public uint AudioTrackCount { set; get; }
        public uint[] AudioTrackIds { set; get; }
        public FrameOffsetStruct[] FrameOffsetList { set; get; }
        public BinkType BinkVersion { set; get; }
        public byte[] MagicBytes { set; get; }


        public byte[] FullHeader { set; get; }

        public uint[][] NewFrameOffsetsAudio { set; get; }
        public uint[] NewFrameOffsetsVideo { set; get; }

        public uint MaxVideoFrameSize { set; get; }
        public uint MaxAudioFrameSize { set; get; }

        public string FilePath { get; set; }
        public string FileExtensionAudio { get; set; }
        public string FileExtensionVideo { get; set; }

        public static readonly byte[] BINK01_HEADER = new byte[] { 0x42, 0x49, 0x4B };
        public static readonly byte[] BINK02_HEADER = new byte[] { 0x4B, 0x42, 0x32 };

        public struct FrameOffsetStruct
        {
            public uint FrameOffset { get; set; }
            public bool IsKeyFrame { get; set; }
        }

        public BinkStream(string filePath)
        {
            this.FilePath = filePath;
            this.FileExtensionAudio = BinkStream.DefaultFileExtensionAudioMulti;
            this.FileExtensionVideo = BinkStream.DefaultFileExtensionVideo;
        }

        private int getIndexForSplitAudioTrackFileName(string splitAudioFileName)
        {
            int index = -1;
            string audioTrackIdString;
            int fileExtensionLocation;
            uint audioTrackId;

            // get track ID
            fileExtensionLocation = splitAudioFileName.IndexOf(BinkStream.DefaultFileExtensionAudioSplit, 0);
            audioTrackIdString = splitAudioFileName.Substring(fileExtensionLocation - 8, 8);
            audioTrackId = (uint)ByteConversion.GetLongValueFromString("0x" + audioTrackIdString);

            for (int i = 0; i < this.AudioTrackIds.Length; i++)
            {
                if (this.AudioTrackIds[i] == audioTrackId)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private void writeChunkToStream(
            byte[] chunk,
            uint chunkId,
            Dictionary<uint, FileStream> streamWriters,
            string fileExtension)
        {
            string destinationFile;

            if (!streamWriters.ContainsKey(chunkId))
            {
                destinationFile = Path.Combine(Path.GetDirectoryName(this.FilePath),
                    String.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(this.FilePath), chunkId.ToString("X8"), fileExtension));
                streamWriters[chunkId] = File.Open(destinationFile, FileMode.Create, FileAccess.ReadWrite);
            }

            streamWriters[chunkId].Write(chunk, 0, chunk.Length);
        }

        public void ParseHeader(FileStream inStream, long offsetToHeader)
        {
            long frameOffsetOffset;
            long audioIdOffet;
            int fullHeaderSize;
            byte versionId;

            this.MagicBytes = ParseFile.ParseSimpleOffset(inStream, offsetToHeader, 3);
            if (ParseFile.CompareSegment(this.MagicBytes, 0, BINK01_HEADER))
            {
                this.BinkVersion = BinkType.Version01;
            }
            else if (ParseFile.CompareSegment(this.MagicBytes, 0, BINK02_HEADER))
            {
                versionId = ParseFile.ReadByte(inStream, offsetToHeader + 3);

                if (versionId < 0x69) // version "i"
                {
                    this.BinkVersion = BinkType.Version01;
                }
                else // version "a" through "h"
                {
                    this.BinkVersion = BinkType.Version02;
                }                
            }
            else
            {
                throw new FormatException("Unrecognized Magic Bytes for Bink.");
            }

            this.FrameCount = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(inStream, offsetToHeader + 8, 4), 0);
            this.AudioTrackCount = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(inStream, offsetToHeader + 0x28, 4), 0);

            // get Audio Track Ids
            this.AudioTrackIds = new uint[this.AudioTrackCount];

            for (uint i = 0; i < this.AudioTrackCount; i++)
            {
                audioIdOffet = offsetToHeader + 0x2C + (this.AudioTrackCount * 8) + (i * 4);

                if (this.BinkVersion == BinkType.Version02)
                {
                    audioIdOffet += 4;
                }

                this.AudioTrackIds[i] = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(inStream, audioIdOffet, 4), 0);
            }

            // get frame offsets
            this.FrameOffsetList = new FrameOffsetStruct[this.FrameCount];

            for (uint i = 0; i < this.FrameCount; i++)
            {
                frameOffsetOffset = offsetToHeader + 0x2C + (this.AudioTrackCount * 0xC) + (i * 4);

                if (this.BinkVersion == BinkType.Version02)
                {
                    frameOffsetOffset += 4;
                }

                this.FrameOffsetList[i].FrameOffset = ParseFile.ReadUintLE(inStream, frameOffsetOffset);

                if ((this.FrameOffsetList[i].FrameOffset & 1) == 1)
                {
                    this.FrameOffsetList[i].IsKeyFrame = true;
                    this.FrameOffsetList[i].FrameOffset &= 0xFFFFFFFE; // mask off bit 0
                }
            }

            // get full header
            fullHeaderSize = (int)(offsetToHeader + 0x2C + (this.AudioTrackCount * 0xC) + (this.FrameCount * 4) + 4);

            if (this.BinkVersion == BinkType.Version02)
            {
                fullHeaderSize += 4;
            }

            this.FullHeader = ParseFile.ParseSimpleOffset(inStream, offsetToHeader, fullHeaderSize);

        }

        protected virtual void DoFinalTasks(Dictionary<uint, FileStream> streamWriters,
            MpegStream.DemuxOptionsStruct demuxOptions)
        {
            string sourceFile;
            string headeredFile;

            int headerSize;
            byte[] headerBytes;
            byte[] frameOffsetBytes;

            uint previousFrameOffset;
            uint frameOffset;

            uint maxFrameSize;
            uint fileLength;

            int audioTrackIndex;

            uint frameStartLocation;
            byte[] dummyValues = new byte[4];

            foreach (uint key in streamWriters.Keys)
            {
                try
                {
                    if (demuxOptions.AddHeader)
                    {
                        //////////////////////
                        // Multi-Track Audio
                        //////////////////////
                        if (streamWriters[key].Name.EndsWith(BinkStream.DefaultFileExtensionAudioMulti))
                        {
                            //////////////////////////
                            // update original header
                            //////////////////////////
                            headerBytes = new byte[this.FullHeader.Length];
                            Array.Copy(this.FullHeader, headerBytes, this.FullHeader.Length);

                            // set frame start location
                            frameStartLocation = 0x2C + (this.AudioTrackCount * 0xC);

                            if (this.BinkVersion == BinkType.Version02)
                            {
                                frameStartLocation += 4;
                            }

                            // set file size
                            fileLength = (uint)(streamWriters[key].Length + headerBytes.Length - 8);

                            // set video size to minimum
                            Array.Copy(BitConverter.GetBytes((uint)1), 0, headerBytes, 0x14, 4);
                            Array.Copy(BitConverter.GetBytes((uint)1), 0, headerBytes, 0x18, 4);

                            // insert file length                            
                            Array.Copy(BitConverter.GetBytes(fileLength), 0, headerBytes, 4, 4);

                            // insert frame offsets
                            previousFrameOffset = 0;
                            frameOffset = 0;
                            maxFrameSize = 0;

                            for (uint i = 0; i < this.FrameCount; i++)
                            {
                                // set previous offset
                                previousFrameOffset = frameOffset;
                                frameOffset = this.NewFrameOffsetsAudio[0][i];

                                if (this.FrameOffsetList[i].IsKeyFrame)
                                {
                                    // add key frame bit
                                    frameOffsetBytes = BitConverter.GetBytes(frameOffset | 1);
                                }
                                else
                                {
                                    frameOffsetBytes = BitConverter.GetBytes(frameOffset);
                                }

                                // insert offset
                                Array.Copy(frameOffsetBytes, 0, headerBytes, (frameStartLocation + (i * 4)), 4);

                                // calculate max frame size
                                if ((frameOffset - previousFrameOffset) > maxFrameSize)
                                {
                                    maxFrameSize = frameOffset - previousFrameOffset;
                                }
                            }

                            // Add last frame offset (EOF)
                            fileLength = (uint)(streamWriters[key].Length + headerBytes.Length);
                            Array.Copy(BitConverter.GetBytes(fileLength), 0, headerBytes, (frameStartLocation + (this.FrameCount * 4)), 4);

                            // insert max frame size
                            if ((fileLength - frameOffset) > maxFrameSize)
                            {
                                maxFrameSize = fileLength - frameOffset;
                            }

                            Array.Copy(BitConverter.GetBytes(maxFrameSize), 0, headerBytes, 0xC, 4);

                            // append to file
                            sourceFile = streamWriters[key].Name;
                            headeredFile = sourceFile + ".headered";

                            streamWriters[key].Close();
                            streamWriters[key].Dispose();

                            FileUtil.AddHeaderToFile(headerBytes, streamWriters[key].Name, headeredFile);
                            File.Delete(streamWriters[key].Name);
                            File.Move(headeredFile, streamWriters[key].Name);
                        }

                        //////////////////////
                        // Split Track Audio
                        //////////////////////
                        else if (streamWriters[key].Name.EndsWith(BinkStream.DefaultFileExtensionAudioSplit))
                        {
                            //////////////////////////
                            // update original header
                            //////////////////////////
                            headerBytes = new byte[this.FullHeader.Length];
                            Array.Copy(this.FullHeader, headerBytes, this.FullHeader.Length);

                            // set video size to minimum
                            Array.Copy(BitConverter.GetBytes((uint)1), 0, headerBytes, 0x14, 4);
                            Array.Copy(BitConverter.GetBytes((uint)1), 0, headerBytes, 0x18, 4);

                            // get track info
                            audioTrackIndex = this.getIndexForSplitAudioTrackFileName(streamWriters[key].Name);

                            // resize header since all audio info except this track will be removied
                            headerSize = (int)(0x2C + 0xC + ((this.FrameCount + 1) * 4));

                            if (this.BinkVersion == BinkType.Version02)
                            {
                                headerSize += 4;
                            }

                            Array.Resize(ref headerBytes, headerSize);

                            // insert audio info for this track
                            headerBytes[0x28] = 1;
                            Array.Copy(this.FullHeader, 0x2C + (audioTrackIndex * 4), headerBytes, 0x2C, 4);
                            
                            // only one audio track, audio track id must equal zero
                            if (this.BinkVersion == BinkType.Version01)
                            {
                                Array.Copy(this.FullHeader, 0x2C + (this.AudioTrackCount * 4) + (audioTrackIndex * 4), headerBytes, 0x30, 4);
                                Array.Copy(BitConverter.GetBytes((uint)0), 0, headerBytes, 0x34, 4);
                            }
                            else if (this.BinkVersion == BinkType.Version02)
                            {
                                Array.Copy(this.FullHeader, 0x30 + (audioTrackIndex * 4), headerBytes, 0x30, 4);
                                Array.Copy(this.FullHeader, 0x30 + (this.AudioTrackCount * 4) + (audioTrackIndex * 4), headerBytes, 0x34, 4);
                                Array.Copy(BitConverter.GetBytes((uint)0), 0, headerBytes, 0x38, 4);
                            }
                            else
                            {
                                throw new FormatException("Unsupported Bink type for split audio streams.");
                            }

                            // set file size
                            fileLength = (uint)(streamWriters[key].Length + headerBytes.Length - 8);
                            Array.Copy(BitConverter.GetBytes(fileLength), 0, headerBytes, 4, 4);

                            // insert frame offsets
                            previousFrameOffset = 0;
                            frameOffset = 0;
                            maxFrameSize = 0;

                            frameStartLocation = 0x2C + 0xC;

                            if (this.BinkVersion == BinkType.Version02)
                            {
                                frameStartLocation += 4;
                            }

                            for (uint i = 0; i < this.FrameCount; i++)
                            {
                                // set previous offset
                                previousFrameOffset = frameOffset;
                                frameOffset = this.NewFrameOffsetsAudio[audioTrackIndex][i];

                                if (this.FrameOffsetList[i].IsKeyFrame)
                                {
                                    // add key frame bit
                                    frameOffsetBytes = BitConverter.GetBytes(this.NewFrameOffsetsAudio[audioTrackIndex][i] | 1);
                                }
                                else
                                {
                                    frameOffsetBytes = BitConverter.GetBytes(this.NewFrameOffsetsAudio[audioTrackIndex][i]);
                                }

                                // insert offset
                                Array.Copy(frameOffsetBytes, 0, headerBytes, (frameStartLocation + (i * 4)), 4);

                                // calculate max frame size
                                if ((frameOffset - previousFrameOffset) > maxFrameSize)
                                {
                                    maxFrameSize = frameOffset - previousFrameOffset;
                                }

                            }

                            // Add last frame offset (EOF)
                            fileLength = (uint)(streamWriters[key].Length + headerBytes.Length);
                            Array.Copy(BitConverter.GetBytes(fileLength), 0, headerBytes, (frameStartLocation + (this.FrameCount * 4)), 4);

                            // insert max frame size
                            if ((fileLength - frameOffset) > maxFrameSize)
                            {
                                maxFrameSize = fileLength - frameOffset;
                            }

                            Array.Copy(BitConverter.GetBytes(maxFrameSize), 0, headerBytes, 0xC, 4);

                            // append to file
                            sourceFile = streamWriters[key].Name;
                            headeredFile = sourceFile + ".headered";

                            streamWriters[key].Close();
                            streamWriters[key].Dispose();

                            FileUtil.AddHeaderToFile(headerBytes, streamWriters[key].Name, headeredFile);
                            File.Delete(streamWriters[key].Name);
                            File.Move(headeredFile, streamWriters[key].Name);
                        }

                        //////////////////////
                        // Video
                        //////////////////////
                        else if (streamWriters[key].Name.EndsWith(BinkStream.DefaultFileExtensionVideo))
                        {
                            //////////////////////////
                            // update original header
                            //////////////////////////
                            headerBytes = new byte[this.FullHeader.Length];
                            Array.Copy(this.FullHeader, headerBytes, this.FullHeader.Length);

                            // resize header since audio info will be removied
                            headerSize = (int)(0x2C + ((this.FrameCount + 1) * 4));

                            if (this.BinkVersion == BinkType.Version02)
                            {
                                headerSize += 4;
                            }

                            Array.Resize(ref headerBytes, headerSize);

                            // set file size
                            fileLength = (uint)(streamWriters[key].Length + headerBytes.Length - 8);
                            Array.Copy(BitConverter.GetBytes(fileLength), 0, headerBytes, 4, 4);

                            // set audio track count to zero
                            headerBytes[0x28] = 0;

                            // insert frame offsets
                            previousFrameOffset = 0;
                            frameOffset = 0;
                            maxFrameSize = 0;

                            frameStartLocation = 0x2C;

                            if (this.BinkVersion == BinkType.Version02)
                            {
                                frameStartLocation += 4;
                            }

                            for (uint i = 0; i < this.FrameCount; i++)
                            {
                                // set previous offset
                                previousFrameOffset = frameOffset;
                                frameOffset = this.NewFrameOffsetsVideo[i];

                                if (this.FrameOffsetList[i].IsKeyFrame)
                                {
                                    // add key frame bit
                                    frameOffsetBytes = BitConverter.GetBytes(this.NewFrameOffsetsVideo[i] | 1);
                                }
                                else
                                {
                                    frameOffsetBytes = BitConverter.GetBytes(this.NewFrameOffsetsVideo[i]);
                                }

                                // insert frame offset
                                Array.Copy(frameOffsetBytes, 0, headerBytes, (frameStartLocation + (i * 4)), 4);

                                // calculate max frame size
                                if ((frameOffset - previousFrameOffset) > maxFrameSize)
                                {
                                    maxFrameSize = frameOffset - previousFrameOffset;
                                }
                            }

                            // Add last frame offset (EOF)
                            fileLength = (uint)(streamWriters[key].Length + headerBytes.Length);
                            Array.Copy(BitConverter.GetBytes(fileLength), 0, headerBytes, (frameStartLocation + (this.FrameCount * 4)), 4);

                            // insert max frame size
                            if ((fileLength - frameOffset) > maxFrameSize)
                            {
                                maxFrameSize = fileLength - frameOffset;
                            }

                            Array.Copy(BitConverter.GetBytes(maxFrameSize), 0, headerBytes, 0xC, 4);

                            // append to file
                            sourceFile = streamWriters[key].Name;
                            headeredFile = sourceFile + ".headered";

                            streamWriters[key].Close();
                            streamWriters[key].Dispose();

                            FileUtil.AddHeaderToFile(headerBytes, streamWriters[key].Name, headeredFile);
                            File.Delete(streamWriters[key].Name);
                            File.Move(headeredFile, streamWriters[key].Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (streamWriters[key] != null)
                    {
                        throw new Exception(String.Format("Exception building header for file: {0}{1}{2}", streamWriters[key].Name, ex.Message, Environment.NewLine));
                    }
                    else
                    {
                        throw new Exception(String.Format("Exception building header for file: {0}{1}{2}", "UNKNOWN", ex.Message, Environment.NewLine));
                    }
                }
                finally
                {
                    // close streams if open
                    if (streamWriters[key] != null &&
                        streamWriters[key].CanRead)
                    {
                        streamWriters[key].Close();
                        streamWriters[key].Dispose();
                    }
                }
            }
        }

        public virtual void DemultiplexStreams(MpegStream.DemuxOptionsStruct demuxOptions)
        {
            long currentOffset = 0;
            long fileSize;
            long packetEndOffset;

            uint audioPacketSize;
            byte[] audioPacket;
            byte[] emptyAudioPacket = new byte[] { 0x00, 0x00, 0x00, 0x00 };

            uint videoPacketSize;
            byte[] videoPacket;

            long currentPacketOffset;

            Dictionary<uint, FileStream> streamOutputWriters = new Dictionary<uint, FileStream>();

            // set audio extension based on flags
            if (demuxOptions.SplitAudioStreams)
            {
                this.FileExtensionAudio = BinkStream.DefaultFileExtensionAudioSplit;
            }
            else
            {
                this.FileExtensionAudio = BinkStream.DefaultFileExtensionAudioMulti;
            }

            try
            {
                using (FileStream fs = File.OpenRead(this.FilePath))
                {
                    fileSize = fs.Length;
                    currentOffset = 0;

                    // parse the header
                    this.ParseHeader(fs, currentOffset);

                    // setup new offsets and first frame
                    this.NewFrameOffsetsVideo = new uint[this.FrameCount];
                    this.NewFrameOffsetsVideo[0] = this.FrameOffsetList[0].FrameOffset - (this.AudioTrackCount * 0xC); // subtract audio frame header info

                    // setup audio frames
                    this.NewFrameOffsetsAudio = new uint[this.AudioTrackCount][];

                    if (demuxOptions.SplitAudioStreams)
                    {
                        this.FileExtensionAudio = BinkStream.DefaultFileExtensionAudioSplit;

                        for (uint i = 0; i < this.AudioTrackCount; i++)
                        {
                            this.NewFrameOffsetsAudio[i] = new uint[this.FrameCount];
                            this.NewFrameOffsetsAudio[i][0] = this.FrameOffsetList[0].FrameOffset - ((this.AudioTrackCount - 1) * 0xC); // only need one audio header area
                        }
                    }
                    else if (this.AudioTrackCount > 0)
                    {
                        this.FileExtensionAudio = BinkStream.DefaultFileExtensionAudioMulti;
                        this.NewFrameOffsetsAudio[0] = new uint[this.FrameCount];
                        this.NewFrameOffsetsAudio[0][0] = this.FrameOffsetList[0].FrameOffset; // all header info stays
                    }

                    //////////////////////
                    // process each frame
                    //////////////////////
                    for (uint frameId = 0; frameId < this.FrameCount; frameId++)
                    {
                        try
                        {
                            currentPacketOffset = 0;

                            //// check to see if final frame offset is file size frame
                            //if (this.FrameOffsetList[frameId].FrameOffset + currentPacketOffset >= fileSize)
                            //{
                            //    break;
                            //}

                            if (demuxOptions.SplitAudioStreams)
                            {
                                //////////////////
                                // extract audio  - separate tracks
                                //////////////////
                                for (uint audioTrackId = 0; audioTrackId < this.AudioTrackCount; audioTrackId++)
                                {
                                    audioPacketSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, this.FrameOffsetList[frameId].FrameOffset + currentPacketOffset, 4), 0);
                                    audioPacketSize += 4;

                                    if (demuxOptions.ExtractAudio)
                                    {
                                        audioPacket = ParseFile.ParseSimpleOffset(fs, this.FrameOffsetList[frameId].FrameOffset + currentPacketOffset, (int)audioPacketSize);
                                        this.writeChunkToStream(audioPacket, this.AudioTrackIds[audioTrackId], streamOutputWriters, this.FileExtensionAudio);
                                    }

                                    currentPacketOffset += audioPacketSize; // goto next packet                        

                                    // update audio frame id
                                    if ((frameId + 1) < this.FrameCount)
                                    {
                                        this.NewFrameOffsetsAudio[audioTrackId][frameId + 1] = this.NewFrameOffsetsAudio[audioTrackId][frameId] + audioPacketSize;
                                    }
                                }
                            }
                            else
                            {
                                ////////////////////////////////////
                                // extract audio  - combine tracks
                                ////////////////////////////////////
                                for (uint audioTrackId = 0; audioTrackId < this.AudioTrackCount; audioTrackId++)
                                {
                                    audioPacketSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, this.FrameOffsetList[frameId].FrameOffset + currentPacketOffset, 4), 0);
                                    audioPacketSize += 4;

                                    if ((demuxOptions.ExtractAudio))
                                    {
                                        audioPacket = ParseFile.ParseSimpleOffset(fs, this.FrameOffsetList[frameId].FrameOffset + currentPacketOffset, (int)audioPacketSize);
                                        this.writeChunkToStream(audioPacket, 0, streamOutputWriters, this.FileExtensionAudio);
                                    }

                                    currentPacketOffset += audioPacketSize; // goto next packet                        
                                }

                                // update audio frame id
                                if ((this.AudioTrackCount > 0) && ((frameId + 1) < this.FrameCount))
                                {
                                    this.NewFrameOffsetsAudio[0][frameId + 1] = this.NewFrameOffsetsAudio[0][frameId] + (uint)currentPacketOffset;
                                }
                            }

                            /////////////////
                            // extract video
                            /////////////////
                            if (frameId == (this.FrameCount - 1)) // last frame
                            {
                                packetEndOffset = fileSize;
                            }
                            else
                            {
                                packetEndOffset = this.FrameOffsetList[frameId + 1].FrameOffset;
                            }

                            videoPacketSize = (uint)(packetEndOffset - (this.FrameOffsetList[frameId].FrameOffset + currentPacketOffset));

                            if (demuxOptions.ExtractVideo)
                            {
                                // parse video packet
                                videoPacket = ParseFile.ParseSimpleOffset(fs, this.FrameOffsetList[frameId].FrameOffset + currentPacketOffset, (int)videoPacketSize);
                                this.writeChunkToStream(videoPacket, 0xFFFF, streamOutputWriters, this.FileExtensionVideo);
                            }

                            // update video frame offset
                            if ((frameId + 1) < this.NewFrameOffsetsVideo.Length)
                            {
                                this.NewFrameOffsetsVideo[frameId + 1] = this.NewFrameOffsetsVideo[frameId] + (uint)videoPacketSize;
                            }
                        }
                        catch (Exception fex)
                        {
                            throw new Exception(String.Format("Exception processing frame 0x{0} at offset 0x{1}: {2}{3}", frameId.ToString("X"), this.FrameOffsetList[frameId].FrameOffset.ToString("X"), fex.Message, Environment.NewLine));
                        }
                    } // for (uint frameId = 0; frameId < this.FrameCount; frameId++)

                } // using (FileStream fs = File.OpenRead(this.FilePath))
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Exception processing block at offset 0x{0}: {1}{2}", currentOffset.ToString("X"), ex.Message, Environment.NewLine));
            }
            finally
            {
                // clean up
                this.DoFinalTasks(streamOutputWriters, demuxOptions);
            }
        }
    }

}

