using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format.sdat
{
    public class Swav
    {
        public struct SwavInfo
        {
            public uint WaveType;
            public uint Loop;
            public UInt16 SampleRate;
            public UInt16 Time;
            public UInt16 LoopOffset;
            public UInt32 NonLoopLength;
        }

        public static readonly byte[] ASCII_SIGNATURE =
            new byte[] { 0x53, 0x57, 0x41, 0x56, 0xFF, 0xFE, 0x00, 0x01 }; // SWAV
        public static readonly byte[] DATA_SIGNATURE =
            new byte[] { 0x44, 0x41, 0x54, 0x41 }; // SWAV
        public static uint SWAV_INFO_SIZE = 12;

        public const string FILE_EXTENSION = ".swav";

        private const string FORMAT_ABBREVIATION = "SWAV";

        string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        // Tag Hash
        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        // general file chunk
        byte[] magicBytes;
        UInt32 fileSize;
        UInt16 fileHeaderSize; // always 16
        UInt16 numberOfBlocks; // always 1

        // SWAV chunk
        byte[] dataBytes;
        UInt32 swavHeaderSize;
        SwavInfo swavInfo;
        byte[] data;
                
        // FILE INFO
        private const int STD_HEADER_SIGNATURE_OFFSET = 0x00;
        private const int STD_HEADER_SIGNATURE_LENGTH = 0x08;

        private const int STD_HEADER_FILE_SIZE_OFFSET = 0x08;
        private const int STD_HEADER_FILE_SIZE_LENGTH = 4;

        private const int STD_HEADER_HEADER_SIZE_OFFSET = 0x0C;
        private const int STD_HEADER_HEADER_SIZE_LENGTH = 2;

        private const int STD_HEADER_NUMBER_OF_SECTIONS_OFFSET = 0x0E;
        private const int STD_HEADER_NUMBER_OF_SECTIONS_LENGTH = 2;

        // SWAV Stuff
        private const int SWAV_HEADER_SIGNATURE_OFFSET = 0x10;
        private const int SWAV_HEADER_SIGNATURE_LENGTH = 0x04;

        private const int SWAV_HEADER_SIZE_OFFSET = 0x14;
        private const int SWAV_HEADER_SIZE_LENGTH = 4;

        private const int SWAV_HEADER_INFO_OFFSET = 0x14;
        private const int SWAV_HEADER_INFO_LENGTH = 4;

        private const int SWAV_HEADER_DATA_OFFSET = 0x20;

        // SWAV INFO stuff
        private const int SWAV_INFO_WAVETYPE_OFFSET = 0x00;
        private const int SWAV_INFO_WAVETYPE_LENGTH = 0x01;

        private const int SWAV_INFO_LOOPFLAG_OFFSET = 0x01;
        private const int SWAV_INFO_LOOPFLAG_LENGTH = 0x01;

        private const int SWAV_INFO_SAMPLERATE_OFFSET = 0x02;
        private const int SWAV_INFO_SAMPLERATE_LENGTH = 0x02;

        private const int SWAV_INFO_SAMPLETIME_OFFSET = 0x04;
        private const int SWAV_INFO_SAMPLETIME_LENGTH = 0x02;

        private const int SWAV_INFO_LOOPOFFSET_OFFSET = 0x06;
        private const int SWAV_INFO_LOOPOFFSET_LENGTH = 0x02;

        private const int SWAV_INFO_NONLOOPLENGTH_OFFSET = 0x08;
        private const int SWAV_INFO_NONLOOPLENGTH_LENGTH = 0x04;

        public static SwavInfo GetSwavInfo(Stream pStream, long pOffset)
        {
            SwavInfo ret = new SwavInfo();

            ret.WaveType = 
                (uint) ParseFile.ParseSimpleOffset(pStream, pOffset + SWAV_INFO_WAVETYPE_OFFSET, SWAV_INFO_WAVETYPE_LENGTH)[0];
            ret.Loop =
                (uint)ParseFile.ParseSimpleOffset(pStream, pOffset + SWAV_INFO_LOOPFLAG_OFFSET, SWAV_INFO_LOOPFLAG_LENGTH)[0];
            ret.SampleRate =
                BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(pStream, pOffset + SWAV_INFO_SAMPLERATE_OFFSET, SWAV_INFO_SAMPLERATE_LENGTH), 0);
            ret.Time =
                BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(pStream, pOffset + SWAV_INFO_SAMPLETIME_OFFSET, SWAV_INFO_SAMPLETIME_LENGTH), 0);
            ret.LoopOffset =
                BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(pStream, pOffset + SWAV_INFO_LOOPOFFSET_OFFSET, SWAV_INFO_LOOPOFFSET_LENGTH), 0);
            ret.NonLoopLength =
                BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, pOffset + SWAV_INFO_NONLOOPLENGTH_OFFSET, SWAV_INFO_NONLOOPLENGTH_OFFSET), 0);

            return ret;
        }
    }
}
