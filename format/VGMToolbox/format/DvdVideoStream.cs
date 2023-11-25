using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class DvdVideoStream : Mpeg2Stream
    {
        public const string Ac3AudioExtension = ".ac3";
        public const string DtsAudioExtension = ".dts";
        public const string SubFileExtension = ".sub";
        public const string LpcmFileExtension = ".lpcm";

        static readonly byte[] Ac3Bytes = new byte[] { 0x0B, 0x77 };
        static readonly byte[] DtsBytes = new byte[] { 0x7F, 0xFE, 0x80, 0x01 };

        public DvdVideoStream(string path) :
            base(path)
        {
            this.UsesSameIdForMultipleAudioTracks = true;
            this.FileExtensionAudio = DefaultAudioExtension;
            this.FileExtensionVideo = DefaultVideoExtension;

            base.BlockIdDictionary[BitConverter.ToUInt32(Mpeg2Stream.PacketStartBytes, 0)] = new BlockSizeStruct(PacketSizeType.Static, 0xE); // Pack Header
        }

        protected override bool IsThisAnAudioBlock(byte[] blockToCheck)
        {
            return ((blockToCheck[3] >= 0xBD) &&
                    (blockToCheck[3] <= 0xDF) &&
                    (blockToCheck[3] != 0xBE) &&
                    (blockToCheck[3] != 0xBF));
        }
        protected override byte GetStreamId(Stream readStream, long currentOffset)
        {
            byte streamId;

            streamId = ParseFile.ParseSimpleOffset(readStream,
                currentOffset + 6 + this.GetAudioPacketHeaderSize(readStream, currentOffset), 1)[0];

            return streamId;
        }

        protected override int GetAudioPacketHeaderSize(Stream readStream, long currentOffset)
        {
            int packetSize = GetStandardPesHeaderSize(readStream, currentOffset);

            return packetSize;
        }
        protected override int GetAudioPacketSubHeaderSize(Stream readStream, long currentOffset, byte streamId)
        {
            int subHeaderSize = 0;
            string streamFileExtension = this.StreamIdFileType[streamId];

            switch (streamFileExtension)
            { 
                case Ac3AudioExtension:
                case DtsAudioExtension:
                    subHeaderSize = 4;
                    break;
                case LpcmFileExtension:
                    subHeaderSize = 7;
                    break;
                default:
                    subHeaderSize = 0;
                    break;
            }

            return subHeaderSize;
        }
        protected override int GetVideoPacketHeaderSize(Stream readStream, long currentOffset)
        {
            return GetStandardPesHeaderSize(readStream, currentOffset);
        }

        protected override string GetAudioFileExtension(Stream readStream, long currentOffset)
        {
            string fileExtension;
            byte streamId = this.GetStreamId(readStream, currentOffset);

            if ((streamId >= 0x20) && (streamId <= 0x3b))
            {
                fileExtension = SubFileExtension;
            }
            else if ((streamId >= 0x80) && (streamId <= 0x87))
            {
                fileExtension = Ac3AudioExtension;
            }
            else if ((streamId >= 0x88) && (streamId <= 0x8f))
            {
                fileExtension = DtsAudioExtension;
            }
            else if ((streamId >= 0xA0) && (streamId <= 0xA7))
            {
                fileExtension = LpcmFileExtension;
            }
            else
            {
                fileExtension = ".bin";
            }
            
            //byte[] streamHead = ParseFile.ParseSimpleOffset(readStream,
            //    currentOffset + 6 + this.GetAudioPacketHeaderSize(readStream, currentOffset) + 4, 4);
            
            //byte[] blockSizeArray = ParseFile.ParseSimpleOffset(readStream, currentOffset + 4, 2);
            //Array.Reverse(blockSizeArray);
            //uint blockSize = (uint)BitConverter.ToUInt16(blockSizeArray, 0);

            //long ac3SyncLocation = ParseFile.GetNextOffset(readStream, currentOffset, Ac3Bytes);
            //if (ac3SyncLocation > 0)
            //{
            //    ac3SyncLocation -= currentOffset;
            //}

            ////if ((streamHead[0] == 0x0B) && (streamHead[1] == 0x77))
            ////{
            ////    fileExtension = Ac3AudioExtension;
            ////}
            //if (ParseFile.CompareSegment(streamHead, 0, DtsBytes))
            //{
            //    fileExtension = DtsAudioExtension;
            //}
            //else if ((ac3SyncLocation > 0) && (ac3SyncLocation < blockSize))
            //{
            //    fileExtension = Ac3AudioExtension;
            //}         
            //else
            //{
            //    fileExtension = ".bin";
            //}

            return fileExtension;
        }
    }
}
