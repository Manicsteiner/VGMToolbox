using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class HudsonHvqm4VideoStream
    {
        public static readonly byte[] HVQM4_13_SIGNATURE = new byte[] { 0x48, 0x56, 0x51, 0x4D, 
                                                                        0x34, 0x20, 0x31, 0x2E, 
                                                                        0x33, 0x00, 0x00, 0x00, 
                                                                        0x00, 0x00, 0x00, 0x00 };

        public static readonly byte[] HVQM4_15_SIGNATURE = new byte[] { 0x48, 0x56, 0x51, 0x4D, 
                                                                        0x34, 0x20, 0x31, 0x2E, 
                                                                        0x35, 0x00, 0x00, 0x00, 
                                                                        0x00, 0x00, 0x00, 0x00 };


        public const string DefaultAudioExtension = ".ima";
        public const string DefaultVideoExtension = ".h4m";

        public const string DefaultAudioExtensionRaw = "raw.ima";
        public const string DefaultVideoExtensionRaw = "raw.video.h4m";

        public struct BlockHeader
        {
            public uint BlockSize { set; get; }
            public uint AudioFrameCount { set; get; }
            public uint VideoFrameCount { set; get; }
            public uint Unknown { set; get; }

            public void Clear()
            {
                this.BlockSize = 0;
                this.AudioFrameCount = 0;
                this.VideoFrameCount = 0;
                this.Unknown = 0;
            }
            public long GetSize()
            {
                return 0x10;
            }
        }

        public struct FrameHeader
        {
            public ushort FrameId1 { set; get; }
            public ushort FrameId2 { set; get; }
            public uint FrameSize { set; get; }

            public void Clear()
            {
                this.FrameId1 = 0;
                this.FrameId2 = 0;
                this.FrameSize = 0;
            }
            public long GetSize()
            {
                return 8;
            }

            public bool IsAudioFrame(bool isFirstAudioFrame)
            {
                bool ret = false;

                if (FrameId1 == 0 && ((isFirstAudioFrame && (FrameId2 == 3 || FrameId2 == 1)) ||
                                      (!isFirstAudioFrame && FrameId2 == 2)))
                {
                    ret = true;
                }

                return ret;
            }

            public bool IsVideoFrame(bool isFirstVideoFrame, VersionType revision)
            {
                bool ret = false;
                
                if (FrameId1 == 1 && ((revision == VersionType.HVQM4_13 && FrameId2 == 0x10) ||
                                      (revision == VersionType.HVQM4_13 && FrameId2 == 0x30) ||
                                      (isFirstVideoFrame && FrameId2 == 0x10) ||
                                      (!isFirstVideoFrame && FrameId2 == 0x20)))
                {
                    ret = true;
                }
                
                return ret;
            }
        }

        //  thanks to hcs for this info!!!
        public enum VersionType
        {
            HVQM4_13,
            HVQM4_15,
        };

        public VersionType FileRevision { set; get; }
        public uint HeaderSize { set; get; }    // 0x10-0x13 
        public uint BodySize { set; get; }      // 0x14-0x17
        public uint Blocks { set; get; }         // 0x18-0x1B
        public uint AudioFrames { set; get; }   // 0x1C-0x1F
        public uint VideoFrames { set; get; }   // 0x20-0x23
        public uint Unk24 { set; get; }          // 0x24-0x27 (0x8257,0x8256)
        public uint Duration { set; get; }       // 0x28-0x2B
        public uint Unk2C { set; get; }          // 0x2C-0x2F (0)
        public uint AudioFrameSize { set; get; } // 0x30-0x33
        public ushort Hres { set; get; }         // 0x34-0x35
        public ushort Vres { set; get; }         // 0x36-0x37
        public ushort Unk38 { set; get; }        // 0x38-0x3B (0x0202)
        public byte Unk3A { set; get; }          // 0x3A (0 or 0x12)
        public byte Unk3B { set; get; }          // 0x3B (0)
        public byte AudioChannels { set; get; } // 0x3C
        public byte AudioBitdepth { set; get; } // 0x3D
        public ushort Pad { set; get; }          // 0x3E-0x3F (0)
        public uint AudioSampleRate { set; get; }   // 0x40-0x43

        public byte[] FullHeader { set; get; }

        public string FilePath { get; set; }
        public string FileExtensionAudio { get; set; }
        public string FileExtensionVideo { get; set; }

        public bool IsVideoPresent { set; get; }
        public bool IsAudioPresent { set; get; }

        public HudsonHvqm4VideoStream(string path)
        {
            this.FilePath = path;
            this.FileExtensionAudio = DefaultAudioExtension;
            this.FileExtensionVideo = DefaultVideoExtension;

            this.FullHeader = null;
            this.IsAudioPresent = true;
            this.IsVideoPresent = true;
        }

        private void writeChunkToStream(
            byte[] chunk,
            string chunkId,
            Dictionary<string, FileStream> streamWriters,
            string fileExtension)
        {
            string destinationFile;

            if (!streamWriters.ContainsKey(chunkId))
            {
                destinationFile = Path.Combine(Path.GetDirectoryName(this.FilePath),
                    String.Format("{0}.{1}{2}", Path.GetFileNameWithoutExtension(this.FilePath), chunkId, fileExtension));
                streamWriters[chunkId] = File.Open(destinationFile, FileMode.Create, FileAccess.ReadWrite);
            }

            streamWriters[chunkId].Write(chunk, 0, chunk.Length);
        }


        public void ParseHeader(Stream inStream, long currentOffset)
        { 
            this.HeaderSize = ParseFile.ReadUintBE(inStream, currentOffset + 0x10);     // 0x10-0x13 
            this.BodySize = ParseFile.ReadUintBE(inStream, currentOffset + 0x14);       // 0x14-0x17
            this.Blocks = ParseFile.ReadUintBE(inStream, currentOffset + 0x18);         // 0x18-0x1B
            this.AudioFrames = ParseFile.ReadUintBE(inStream, currentOffset + 0x1C);    // 0x1C-0x1F
            this.VideoFrames = ParseFile.ReadUintBE(inStream, currentOffset + 0x20);    // 0x20-0x23
            this.Unk24  = ParseFile.ReadUintBE(inStream, currentOffset + 0x24);         // 0x24-0x27 (0x8257,0x8256)
            this.Duration = ParseFile.ReadUintBE(inStream, currentOffset + 0x28);       // 0x28-0x2B
            this.Unk2C = ParseFile.ReadUintBE(inStream, currentOffset + 0x2C);          // 0x2C-0x2F (0)
            this.AudioFrameSize = ParseFile.ReadUintBE(inStream, currentOffset + 0x30); // 0x30-0x33
            this.Hres = ParseFile.ReadUshortBE(inStream, currentOffset + 0x34);         // 0x34-0x35
            this.Vres = ParseFile.ReadUshortBE(inStream, currentOffset + 0x36);         // 0x36-0x37
            this.Unk38 = ParseFile.ReadUshortBE(inStream, currentOffset + 0x38);        // 0x38-0x3B (0x0202)
            this.Unk3A = ParseFile.ReadByte(inStream, currentOffset + 0x3A);            // 0x3A (0 or 0x12)
            this.Unk3B = ParseFile.ReadByte(inStream, currentOffset + 0x3B);            // 0x3B (0)
            this.AudioChannels = ParseFile.ReadByte(inStream, currentOffset + 0x3C);    // 0x3C
            this.AudioBitdepth = ParseFile.ReadByte(inStream, currentOffset + 0x3D);    // 0x3D
            this.Pad = ParseFile.ReadUshortBE(inStream, currentOffset + 0x3E);          // 0x3E-0x3F (0)
            this.AudioSampleRate = ParseFile.ReadUintBE(inStream, currentOffset + 0x40);// 0x40-0x43

            // save off original header
            this.FullHeader = ParseFile.ParseSimpleOffset(inStream, currentOffset, (int)this.HeaderSize);

            #region AUDIO/VIDEO features checks
            // set video present flags
            if (this.VideoFrames == 0)
            {
                if (this.Hres == 0)
                {
                    throw new Exception("Video frame count greater than 0, but horizontal resolution equals zero.");
                }
                else if (this.Vres == 0)
                {
                    throw new Exception("Video frame count greater than 0, but vertical resolution equals zero.");
                }
                else
                {
                    this.IsVideoPresent = true;
                }
            }

            // set audio present flags
            if (this.AudioFrames > 0)
            {
                if (this.AudioFrameSize == 0)
                {
                    throw new Exception("Audio frame count greater than 0, but audio frame size equals zero.");
                }
                else if (this.AudioChannels == 0)
                {
                    throw new Exception("Audio frame count greater than 0, but audio channel count equals zero.");
                }
                else if (this.AudioBitdepth == 0)
                {
                    throw new Exception("Audio frame count greater than 0, but audio bit depth equals zero.");
                }
                else if (this.AudioSampleRate == 0)
                {
                    throw new Exception("Audio frame count greater than 0, but audio sample rate equals zero.");
                }
                else
                {
                    this.IsAudioPresent = true;
                }
            }
            #endregion
        }

        public void ParseBlockHeader(Stream inStream, long currentOffset, ref BlockHeader bHeader)
        {
            bHeader.Clear();            
            bHeader.BlockSize = ParseFile.ReadUintBE(inStream, currentOffset);
            bHeader.AudioFrameCount = ParseFile.ReadUintBE(inStream, currentOffset + 4);
            bHeader.VideoFrameCount = ParseFile.ReadUintBE(inStream, currentOffset + 8);
            bHeader.Unknown = ParseFile.ReadUintBE(inStream, currentOffset + 0xC);
        }

        public void ParseFrameHeader(Stream inStream, long currentOffset, ref FrameHeader fHeader)
        {
            fHeader.Clear();
            fHeader.FrameId1 = ParseFile.ReadUshortBE(inStream, currentOffset);
            fHeader.FrameId2 = ParseFile.ReadUshortBE(inStream, currentOffset + 2);
            fHeader.FrameSize = ParseFile.ReadUintBE(inStream, currentOffset + 4);
        }

        protected void DoFinalTasks(Dictionary<string, FileStream> outputFiles)
        {
            foreach (string streamId in outputFiles.Keys)
            {
                outputFiles[streamId].Close();
                outputFiles[streamId].Dispose();
            }
        }

        public virtual void DemultiplexStreams(MpegStream.DemuxOptionsStruct demuxOptions)
        {
            long currentOffset = -1;
            long fileSize;

            byte[] dummy;
            
            uint blocksProcessed = 0;
            uint audioFramesProcessed = 0;
            uint videoFramesProcessed = 0;
            uint bytesProcessed = 0;
            uint expectedBytesProcessed = 0;

            BlockHeader blockHeader = new BlockHeader();
            FrameHeader frameHeader = new FrameHeader();

            bool isFirstVideoFrame = true;
            bool isFirstAudioFrame = true;

            byte[] fullChunk;
            
            uint audioChunkSize;
            uint audioChunkSamples;

            long blockStart;
            long frameStart;

            Dictionary<string, FileStream> streamWriters = new Dictionary<string, FileStream>();

            try
            {
                using (FileStream fs = File.OpenRead(this.FilePath))
                {
                    fileSize = fs.Length;

                    #region HEADER CHECK
                    // check header
                    dummy = ParseFile.ParseSimpleOffset(fs, 0, 0x10);

                    if (ParseFile.CompareSegment(dummy, 0, HVQM4_13_SIGNATURE))
                    {
                        this.FileRevision = VersionType.HVQM4_13;
                        currentOffset = 0;
                    }
                    else if (ParseFile.CompareSegment(dummy, 0, HVQM4_15_SIGNATURE))
                    {
                        this.FileRevision = VersionType.HVQM4_15;
                        currentOffset = 0;
                    }
                    else
                    {
                        throw new Exception("HVQM4 signature not found at offset 0x00");
                    }
                    #endregion

                    // parse file
                    if (currentOffset >= 0)
                    {
                        // get header
                        this.ParseHeader(fs, 0);
                        currentOffset = this.HeaderSize + 4;

                        // process file
                        while ((currentOffset < fileSize) && 
                               (blocksProcessed < this.Blocks)) 
                        {
                            // reset flags
                            isFirstVideoFrame = true;
                            isFirstAudioFrame = true;

                            audioFramesProcessed = 0;
                            videoFramesProcessed = 0;

                            //--------------
                            // parse block
                            //--------------
                            blockStart = currentOffset;
                            blocksProcessed++;

                            // parse block header
                            this.ParseBlockHeader(fs, currentOffset, ref blockHeader);
                            currentOffset += blockHeader.GetSize();
                            bytesProcessed = 0;

                            while (bytesProcessed < blockHeader.BlockSize)
                            {
                                frameStart = currentOffset;
                                
                                // verify we haven't processed too much
                                if (audioFramesProcessed > blockHeader.AudioFrameCount)
                                {
                                    throw new Exception(String.Format("Processed more audio frames than expected for block starting at 0x{0}", blockStart.ToString("X8")));
                                }
                                else if (videoFramesProcessed > blockHeader.VideoFrameCount)
                                {
                                    throw new Exception(String.Format("Processed more video frames than expected for block starting at 0x{0}", blockStart.ToString("X8")));
                                }

                                // parse frame header
                                this.ParseFrameHeader(fs, currentOffset, ref frameHeader);
                                currentOffset += frameHeader.GetSize();
                                bytesProcessed += (uint)frameHeader.GetSize();

                                //---------------
                                // process frame
                                //---------------
                                // audio chunk
                                if (demuxOptions.ExtractAudio && frameHeader.IsAudioFrame(isFirstAudioFrame))
                                {
                                    audioChunkSamples = ParseFile.ReadUintBE(fs, currentOffset);
                                    audioChunkSize = (audioChunkSamples * this.AudioChannels) / 2;

                                    // get full frame for now
                                    fullChunk = ParseFile.ParseSimpleOffset(fs, currentOffset, (int)frameHeader.FrameSize);
                                    
                                    // different frames have different IMA info
                                    //if (isFirstAudioFrame)
                                    //{
                                    //    fullChunk = ParseFile.ParseSimpleOffset(fs, currentOffset + 6, (int)audioChunkSize);
                                    //}
                                    //else
                                    //{
                                    //    fullChunk = ParseFile.ParseSimpleOffset(fs, currentOffset + 4, (int)audioChunkSize);
                                    //}
                                    
                                    this.writeChunkToStream(fullChunk, "audio", streamWriters, this.FileExtensionAudio);

                                    isFirstAudioFrame = false;
                                    audioFramesProcessed++;
                                }

                                // video chunk
                                else if (demuxOptions.ExtractVideo && frameHeader.IsVideoFrame(isFirstVideoFrame, this.FileRevision))
                                {
                                    fullChunk = ParseFile.ParseSimpleOffset(fs, currentOffset, (int)frameHeader.FrameSize);
                                    this.writeChunkToStream(fullChunk, "video", streamWriters, this.FileExtensionVideo);

                                    isFirstVideoFrame = false;
                                    videoFramesProcessed++;
                                }

                                //unknown
                                else
                                {
                                    fullChunk = ParseFile.ParseSimpleOffset(fs, currentOffset, (int)frameHeader.FrameSize);
                                    this.writeChunkToStream(fullChunk, "unknown", streamWriters, ".bin");
                                }

                                // update number of bytes processed
                                bytesProcessed += frameHeader.FrameSize;

                                // move to next frame
                                currentOffset += frameHeader.FrameSize;
                            
                            } // while (bytesProcessed < blockHeader.BlockSize)

                            // verify proper number of bytes processed
                            if (blocksProcessed < this.Blocks)
                            {
                                bytesProcessed = bytesProcessed + (uint)(blockHeader.GetSize() + 4);
                                expectedBytesProcessed = ParseFile.ReadUintBE(fs, currentOffset);

                                if (expectedBytesProcessed != bytesProcessed)
                                {
                                    throw new Exception(String.Format(
                                        "Bytes processed 0x{0}does not match expected bytes processed {1} for block starting at 0x{2}.",
                                        bytesProcessed.ToString("X8"),
                                        expectedBytesProcessed.ToString("X8"),
                                        blockStart.ToString("X8")));
                                }
                                else
                                {
                                    currentOffset += 4;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(String.Format("Cannot find MVHD header.{0}", Environment.NewLine));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.DoFinalTasks(streamWriters);
            }
        }
    }
}
