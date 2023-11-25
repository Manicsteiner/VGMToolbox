using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Mpeg2Stream : MpegStream
    {
        public const string DefaultAudioExtension = ".m2a";
        public const string DefaultVideoExtension = ".m2v";

        public Mpeg2Stream(string path) :
            base(path)
        {
            this.FileExtensionAudio = DefaultAudioExtension;
            this.FileExtensionVideo = DefaultVideoExtension;

            base.BlockIdDictionary[BitConverter.ToUInt32(MpegStream.PacketStartBytes, 0)] = new BlockSizeStruct(PacketSizeType.Static, 0xE); // Pack Header
        }

        protected int GetStandardPesHeaderSize(Stream readStream, long currentOffset)
        {
            byte checkBytes;
            OffsetDescription od = new OffsetDescription();

            od.OffsetByteOrder = Constants.BigEndianByteOrder;
            od.OffsetSize = "1";
            od.OffsetValue = "8";

            checkBytes = (byte)ParseFile.GetVaryingByteValueAtRelativeOffset(readStream, od, currentOffset);

            return checkBytes + 3;
        }

        protected override int GetAudioPacketHeaderSize(Stream readStream, long currentOffset)
        {
            int packetSize = GetStandardPesHeaderSize(readStream, currentOffset);
            return packetSize;
        }
        protected override int GetVideoPacketHeaderSize(Stream readStream, long currentOffset)
        {
            return GetStandardPesHeaderSize(readStream, currentOffset);
        }

        protected override string GetAudioFileExtension(Stream readStream, long currentOffset)
        {
            return DefaultAudioExtension;
        }

        protected ulong DecodePresentationTimeStamp(byte[] EncodedTimeStampBytes)
        {
            ulong decodedTimeStamp = 0;
            ulong encodedTimeStamp = 0;

            if (EncodedTimeStampBytes.Length != 5)
            {
                throw new FormatException("Encoded time stamp must be 5 bytes.");
            }

            // convert to ulong from bytes
            encodedTimeStamp = EncodedTimeStampBytes[0];
            encodedTimeStamp &= 0x0F;
            encodedTimeStamp <<= 32;
            encodedTimeStamp += (ulong)(EncodedTimeStampBytes[1] << 24);
            encodedTimeStamp += (ulong)(EncodedTimeStampBytes[2] << 16);
            encodedTimeStamp += (ulong)(EncodedTimeStampBytes[3] << 8);                        
            encodedTimeStamp += EncodedTimeStampBytes[4];
            
            decodedTimeStamp |= (encodedTimeStamp >> 3) & (0x0007ul << 30); // top 3 bits, shifted left by 3, other bits zeroed out
            decodedTimeStamp |= (encodedTimeStamp >> 2) & (0x7ffful << 15); // middle 15 bits
            decodedTimeStamp |= (encodedTimeStamp >> 1) & (0x7ffful << 0); // bottom 15 bits

            return decodedTimeStamp;
        }
    }
}
