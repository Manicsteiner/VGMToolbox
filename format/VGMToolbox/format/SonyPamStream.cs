using System;
using System.Collections.Generic;
using System.IO;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class SonyPamStream : SonyPmfStream
    {
        new public const string DefaultAudioExtension = ".at3";
        public const string Ac3AudioExtension = ".ac3";

        new public static readonly byte[] Atrac3Bytes = new byte[] { 0x1E, 0x60, 0x14 };
        
        public const string M2vVideoExtension = ".m2v";
        public static readonly byte[] M2vBytes = new byte[] { 0x00, 0x00, 0x01, 0xB3 };

        public static readonly byte[] PamAudioStreamInfoStartBytes = new byte[] { 0x00, 0x00, 0x00, 0xBD };

        public SonyPamStream(string path)
            : base(path)
        {
            this.UsesSameIdForMultipleAudioTracks = true;
            this.FileExtensionAudio = Atrac3AudioExtension;
            this.FileExtensionVideo = AvcVideoExtension;

            base.BlockIdDictionary[BitConverter.ToUInt32(Mpeg2Stream.PacketStartBytes, 0)] = new BlockSizeStruct(PacketSizeType.Static, 0xE); // Pack Header
            base.BlockIdDictionary[BitConverter.ToUInt32(new byte[] { 0x00, 0x00, 0x01, 0xBD }, 0)] = new BlockSizeStruct(PacketSizeType.SizeBytes, 2); // Audio Stream, two bytes following equal length (Big Endian)
        }

        protected override long GetStartOffset(Stream readStream, long currentOffset) 
        {
            long startOffset = 0;

            uint seekOffsets = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(readStream, 0x90, 4));
            uint seekCount = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(readStream, 0x94, 4));

            if (seekOffsets > 0)
            { 
                startOffset = seekOffsets + (seekCount * 0x0C);
            }

            return startOffset;
        }

        protected override string GetAudioFileExtension(Stream readStream, long currentOffset)
        {
            string fileExtension;
            byte streamId = this.GetStreamId(readStream, currentOffset);

            if (streamId < 0x20)
            {
                fileExtension = Atrac3AudioExtension;
            }
            else if ((streamId >= 0x30) && (streamId < 0x40))
            {
                fileExtension = Ac3AudioExtension;
            }
            else if ((streamId >= 0x40) && (streamId < 0x50))
            {
                fileExtension = LpcmAudioExtension;
            }
            else
            {
                fileExtension = ".bin";
            }

            return fileExtension;
        }
        protected override string GetVideoFileExtension(Stream readStream, long currentOffset)
        {
            string fileExtension;
            byte[] checkBytes;
            int videoHeaderSize = this.GetVideoPacketHeaderSize(readStream, currentOffset);
            
            checkBytes = ParseFile.ParseSimpleOffset(readStream, (currentOffset + videoHeaderSize + 6), 4);

            if (ParseFile.CompareSegment(checkBytes, 0, AvcBytes))
            {
                fileExtension = AvcVideoExtension;
            }
            else if (ParseFile.CompareSegment(checkBytes, 0, M2vBytes))
            {
                fileExtension = M2vVideoExtension;
            }
            else
            {
                fileExtension = ".bin";
            }

            return fileExtension;
        }

        protected override void DoFinalTasks(FileStream sourceFileStream, Dictionary<uint, FileStream> outputFiles, bool addHeader)
        {
            byte[] headerBytes;
            byte[] aa3HeaderBytes;
            uint headerBlock;
            string sourceFile;

            long streamInfoOffset;
            byte streamIdByte;
            byte streamIdCheckByte;
            byte channelCount;
            GenhCreationStruct gcStruct;
            string genhFile;

            foreach (uint streamId in outputFiles.Keys)
            {
                if (this.IsThisAnAudioBlock(BitConverter.GetBytes(streamId)))
                {
                    if (outputFiles[streamId].Name.EndsWith(Atrac3AudioExtension))
                    {
                        headerBytes = ParseFile.ParseSimpleOffset(outputFiles[streamId], 0, 0x8);

                        // remove all header chunks
                        string cleanedFile = FileUtil.RemoveAllChunksFromFile(outputFiles[streamId], headerBytes);

                        // close stream and rename file
                        sourceFile = outputFiles[streamId].Name;

                        outputFiles[streamId].Close();
                        outputFiles[streamId].Dispose();

                        File.Delete(sourceFile);
                        File.Move(cleanedFile, sourceFile);

                        // add header
                        if (addHeader)
                        {
                            Array.Reverse(headerBytes);
                            headerBlock = BitConverter.ToUInt32(headerBytes, 4);

                            string headeredFile = Path.ChangeExtension(sourceFile, Atrac3Plus.FileExtension);
                            aa3HeaderBytes = Atrac3Plus.GetAa3Header(headerBlock);
                            FileUtil.AddHeaderToFile(aa3HeaderBytes, sourceFile, headeredFile);

                            File.Delete(sourceFile);
                        }
                    }
                    else if (addHeader && 
                             outputFiles[streamId].Name.EndsWith(LpcmAudioExtension))
                    {
                        // get this block's stream id
                        streamIdByte = BitConverter.GetBytes(streamId)[0];
                        
                        // get location of first audio stream info block
                        streamInfoOffset = ParseFile.GetNextOffset(sourceFileStream, 0, PamAudioStreamInfoStartBytes);

                        // find matching info block
                        while ((streamInfoOffset > -1))
                        {
                            streamIdCheckByte = ParseFile.ParseSimpleOffset(sourceFileStream, streamInfoOffset + 0x4, 1)[0];

                            if (streamIdCheckByte == streamIdByte)
                            {
                                // get channel count
                                channelCount = ParseFile.ParseSimpleOffset(sourceFileStream, streamInfoOffset + 0x11, 1)[0];

                                // close stream and build GENH file
                                sourceFile = outputFiles[streamId].Name;

                                outputFiles[streamId].Close();
                                outputFiles[streamId].Dispose();

                                gcStruct = new GenhCreationStruct();
                                gcStruct.Format = "0x03";
                                gcStruct.HeaderSkip = "0";
                                gcStruct.Interleave = "0x2";
                                gcStruct.Channels = channelCount.ToString();
                                gcStruct.Frequency = "48000";
                                gcStruct.NoLoops = true;

                                genhFile = GenhUtil.CreateGenhFile(sourceFile, gcStruct);

                                // delete original file
                                if (!String.IsNullOrEmpty(genhFile))
                                {
                                    File.Delete(sourceFile);
                                }

                                break;
                            }

                            streamInfoOffset = ParseFile.GetNextOffset(sourceFileStream, streamInfoOffset + 1, PamAudioStreamInfoStartBytes);
                        }
                    }
                }
            }
        }
    }
}
