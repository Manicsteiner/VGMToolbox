using System;
using System.Collections.Generic;
using System.IO;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class SonyPmfStream : Mpeg2Stream
    {
        new public const string DefaultAudioExtension = ".at3";

        public const string Atrac3AudioExtension = ".at3";
        public const string LpcmAudioExtension = ".lpcm";
        public const string SubTitleExtension = ".subs";

        public const byte LpcmStreamId = 0x40;

        public static readonly byte[] Atrac3Bytes = new byte[] { 0x1E, 0x60, 0x04 };
        public static readonly byte[] LpcmBytes = new byte[] { 0x1E, 0x61, 0x80, 0x40 };

        public const string AvcVideoExtension = ".264";
        public static readonly byte[] AvcBytes = new byte[] { 0x00, 0x00, 0x00, 0x01 };

        public SonyPmfStream(string path)
            : base(path)
        {
            this.UsesSameIdForMultipleAudioTracks = true;
            this.FileExtensionAudio = DefaultAudioExtension;
            this.FileExtensionVideo = AvcVideoExtension;

            base.BlockIdDictionary[BitConverter.ToUInt32(Mpeg2Stream.PacketStartBytes, 0)] = new BlockSizeStruct(PacketSizeType.Static, 0xE); // Pack Header
            base.BlockIdDictionary[BitConverter.ToUInt32(new byte[] { 0x00, 0x00, 0x01, 0xBD }, 0)] = new BlockSizeStruct(PacketSizeType.SizeBytes, 2); // Audio Stream, two bytes following equal length (Big Endian)
        }

        protected override long GetStartOffset(Stream readStream, long currentOffset)
        {
            long startOffset = 0;

            uint seekOffsets = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(readStream, 0x86, 4));
            uint seekCount = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(readStream, 0x8A, 4));

            if (seekOffsets > 0)
            {
                startOffset = seekOffsets + (seekCount * 0x0A);
            }

            return startOffset;
        }

        protected override int GetAudioPacketHeaderSize(Stream readStream, long currentOffset)
        {            
            byte checkBytes;
            OffsetDescription od = new OffsetDescription();

            od.OffsetByteOrder = Constants.BigEndianByteOrder;
            od.OffsetSize = "1";
            od.OffsetValue = "8";

            checkBytes = (byte)ParseFile.GetVaryingByteValueAtRelativeOffset(readStream, od, currentOffset);

            return checkBytes + 7;           
        }
        protected override int GetAudioPacketSubHeaderSize(Stream readStream, long currentOffset, byte streamId)
        {
            int subHeaderSize = 0;
            string streamFileExtension = this.StreamIdFileType[streamId];

            switch (streamFileExtension)
            {
                case SubTitleExtension:     // leave timing data attached for post-processing
                    subHeaderSize = -0xC;
                    break;
                default:
                    subHeaderSize = 0;
                    break;
            }

            return subHeaderSize;
        }
        protected override bool IsThisAnAudioBlock(byte[] blockToCheck)
        {
            return (blockToCheck[3] == 0xBD);
        }
        protected override bool IsThisAVideoBlock(byte[] blockToCheck)
        {
            return ((blockToCheck[3] >= 0xE0) && (blockToCheck[3] <= 0xEF));
        }

        protected override string GetAudioFileExtension(Stream readStream, long currentOffset)
        {
            string fileExtension;
            byte streamId = this.GetStreamId(readStream, currentOffset);

            if (streamId < 0x20)
            {
                fileExtension = Atrac3AudioExtension;
            }
            else if ((streamId >= 0x40) && (streamId < 0x50))
            {
                fileExtension = LpcmAudioExtension;
            }
            else if ((streamId >= 0x80) && (streamId < 0x9F))
            {
                fileExtension = SubTitleExtension;
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
            else
            {
                fileExtension = ".bin";
            }

            return fileExtension;
        }

        protected override byte GetStreamId(Stream readStream, long currentOffset) 
        {
            byte streamId;
            byte sizeValue;
            int offsetToCheck;

            sizeValue = ParseFile.ParseSimpleOffset(readStream, currentOffset + 8, 1)[0];
            offsetToCheck = sizeValue + 6 + 7 - 4;
            streamId = ParseFile.ParseSimpleOffset(readStream, currentOffset + offsetToCheck, 1)[0];

            return streamId;
        }

        protected void ConvertSubtitles(uint streamId, FileStream subtitleStream)
        {
            long subsLength = subtitleStream.Length;
            long currentOffset = 0;

            byte[] encodedPresentationTimeStamp;
            ulong decodedTimeStamp;
            ushort subtitlePacketSize;

            byte[] PNG_HEADER = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            byte[] PNG_END = new byte[] { 0x49, 0x45, 0x4E, 0x44 };

            long pngStartOffset;
            long pngEndOffset;
            long pngSize;
            uint pngCount = 0;

            string baseDirectory = Path.GetDirectoryName(subtitleStream.Name);
            baseDirectory = Path.Combine(baseDirectory, String.Format("{0}_PNGs", Path.GetFileNameWithoutExtension(subtitleStream.Name)));
            string destinationFile;

            while (currentOffset < subsLength)
            {
                // decode time stamp
                encodedPresentationTimeStamp = ParseFile.ParseSimpleOffset(subtitleStream, currentOffset, 5);
                decodedTimeStamp = this.DecodePresentationTimeStamp(encodedPresentationTimeStamp);

                // get subtitle packet size
                subtitlePacketSize = ParseFile.ReadUshortBE(subtitleStream, currentOffset + 0xC);

                // extract PNG
                pngStartOffset = ParseFile.GetNextOffset(subtitleStream, currentOffset + 0x1E, PNG_HEADER);
                pngEndOffset = ParseFile.GetNextOffset(subtitleStream, pngStartOffset, PNG_END) + 4;
                pngSize = pngEndOffset - pngStartOffset;

                if (pngSize > (subtitlePacketSize - 0x14))
                {
                    throw new Exception("Warning, PNG size exceeds packet size.");
                }

                destinationFile = Path.Combine(baseDirectory, String.Format("{0}.png", pngCount.ToString("D8")));
                ParseFile.ExtractChunkToFile(subtitleStream, pngStartOffset, pngSize, destinationFile);
                pngCount++;

                // write timestamp and PNG to file

                // move to next block
                currentOffset += 0xE + subtitlePacketSize;
            }
        }

        protected override void DoFinalTasks(FileStream sourceFileStream, Dictionary<uint, FileStream> outputFiles, bool addHeader)
        {
            byte[] headerBytes;
            byte[] aa3HeaderBytes;
            uint headerBlock;
            string sourceFile;

            foreach (uint streamId in outputFiles.Keys)
            {
                if (this.IsThisAnAudioBlock(BitConverter.GetBytes(streamId)) &&
                    outputFiles[streamId].Name.EndsWith(Atrac3AudioExtension))
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

                        string headeredFile = Path.ChangeExtension(sourceFile, Atrac3Plus.FileExtensionPsp);
                        aa3HeaderBytes = Atrac3Plus.GetAa3Header(headerBlock);
                        FileUtil.AddHeaderToFile(aa3HeaderBytes, sourceFile, headeredFile);

                        File.Delete(sourceFile);
                    }
                }
                //else if (this.IsThisAnAudioBlock(BitConverter.GetBytes(streamId)) &&
                //    outputFiles[streamId].Name.EndsWith(SubTitleExtension))
                //{
                //    this.ConvertSubtitles(streamId, outputFiles[streamId]);

                //}
            }
        }
    }
}
