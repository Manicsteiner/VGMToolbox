using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class S98V1 : IFormat
    {
        private static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x53, 0x39, 0x38, 0x31 }; // S981
        private const string FORMAT_ABBREVIATION = "S98V1";

        public const string ASCII_TAG = "[S98]";
        protected const string TAG_UTF8_INDICATOR = "utf8=";
        protected const int TAG_IDENTIFIER_LENGTH = 5;

        protected string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        protected struct S98Device
        { 
            public byte[] DeviceType;
            public byte[] Clock;
            public byte[] Pan;
            public byte[] Reserve;
        }
        protected const int S98DEVICE_TYPE_OFFSET = 0x00;
        protected const int S98DEVICE_TYPE_LENGTH = 0x04;

        protected const int S98DEVICE_CLOCK_OFFSET = 0x04;
        protected const int S98DEVICE_CLOCK_LENGTH = 0x04;

        protected const int S98DEVICE_PAN_OFFSET = 0x08;
        protected const int S98DEVICE_PAN_LENGTH = 0x04;

        protected const int S98DEVICE_RESERVED_OFFSET = 0x0C;
        protected const int S98DEVICE_RESERVED_LENGTH = 0x04;

        // ALL VERSIONS

        protected const int SIG_OFFSET = 0x00;
        protected const int SIG_LENGTH = 0x03;

        protected const int VERSION_OFFSET = 0x03;
        protected const int VERSION_LENGTH = 0x01;

        protected const int TIMER_INFO_OFFSET = 0x04;
        protected const int TIMER_INFO_LENGTH = 0x04;

        protected const int TIMER_INFO2_OFFSET = 0x08;
        protected const int TIMER_INFO2_LENGTH = 0x04;

        protected const int COMPRESSING_OFFSET = 0x0C;
        protected const int COMPRESSING_LENGTH = 0x04;

        protected const int SONG_NAME_OFFSET_OFFSET = 0x10;
        protected const int SONG_NAME_OFFSET_LENGTH = 0x04;

        protected const int DUMP_DATA_OFFSET_OFFSET = 0x14;
        protected const int DUMP_DATA_OFFSET_LENGTH = 0x04;

        protected const int LOOP_POINT_OFFSET_OFFSET = 0x18;
        protected const int LOOP_POINT_OFFSET_LENGTH = 0x04;

        // V1 ONLY        
        private const int V1_RESERVED_OFFSET = 0x1C;
        private const int V1_RESERVED_LENGTH = 0x24;        

        // V2 ONLY
        private const int V2_COMPRESSED_DATA_OFFEST_OFFSET = 0x1C;
        private const int V2_COMPRESSED_DATA_OFFEST_LENGTH = 0x04;
        
        // ALL
        protected byte[] asciiSignature;
        protected byte[] version;
        protected byte[] timerInfo;
        protected byte[] timerInfo2;
        protected byte[] compressing;
        protected byte[] songNameOffset;
        protected byte[] dumpDataOffset;
        protected byte[] loopPointOffset;
        protected byte[] data;

        public byte[] AsciiSignature { get { return this.asciiSignature; } }
        public byte[] Version { get { return this.version; } }
        public byte[] TimerInfo { get { return this.timerInfo; } }
        public byte[] TimerInfo2 { get { return this.timerInfo2; } }
        public byte[] Compressing { get { return this.compressing; } }
        public byte[] SongNameOffset { get { return this.songNameOffset; } }
        public byte[] DumpDataOffset { get { return this.dumpDataOffset; } }
        public byte[] LoopPointOffset { get { return this.loopPointOffset; } }
        public byte[] Data { get { return this.data; } }

        // V1
        private byte[] v1Reserved;        
        public byte[] V1Reserved { get { return this.v1Reserved; } }        

        protected Dictionary<string, string> tagHash = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        protected Dictionary<int, string> deviceHash = new Dictionary<int, string>();

        #region METHODS

        public byte[] getAsciiSignature(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
        }
        public byte[] getVersion(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, VERSION_OFFSET, VERSION_LENGTH);
        }
        public byte[] getTimerInfo(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, TIMER_INFO_OFFSET, TIMER_INFO_LENGTH);
        }
        public byte[] getTimerInfo2(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, TIMER_INFO2_OFFSET, TIMER_INFO2_LENGTH);
        }
        public byte[] getCompressing(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, COMPRESSING_OFFSET, COMPRESSING_LENGTH);
        }
        public byte[] getSongNameOffset(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SONG_NAME_OFFSET_OFFSET, SONG_NAME_OFFSET_LENGTH);
        }
        public byte[] getDumpDataOffset(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, DUMP_DATA_OFFSET_OFFSET, DUMP_DATA_OFFSET_LENGTH);
        }
        public byte[] getLoopPointOffset(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, LOOP_POINT_OFFSET_OFFSET, LOOP_POINT_OFFSET_LENGTH);
        }
        public byte[] getV1Reserved(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, V1_RESERVED_OFFSET, V1_RESERVED_LENGTH);
        }

        public virtual void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath = pFilePath;
            this.asciiSignature = this.getAsciiSignature(pStream);
            this.version = this.getVersion(pStream);
            this.timerInfo = this.getTimerInfo(pStream);
            this.timerInfo2 = this.getTimerInfo2(pStream);
            this.compressing = this.getCompressing(pStream);
            this.songNameOffset = this.getSongNameOffset(pStream);
            this.dumpDataOffset = this.getDumpDataOffset(pStream);
            this.loopPointOffset = this.getLoopPointOffset(pStream);
            
            int v1DataOffset = BitConverter.ToInt32(this.dumpDataOffset, 0);
            this.v1Reserved = this.getV1Reserved(pStream);
            this.data = ParseFile.ParseSimpleOffset(pStream, v1DataOffset, (int) pStream.Length - v1DataOffset);

            this.initDeviceHash();
            this.initializeTagHash(pStream);
        }

        private void initializeTagHash(Stream pStream)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            tagHash.Add("S98 Version", enc.GetString(this.version));
            
            // Song Name
            if (BitConverter.ToUInt32(this.songNameOffset, 0) != 0)
            {
                Int32 songOffset = BitConverter.ToInt32(this.songNameOffset, 0);
                Int32 songLength = ParseFile.GetSegmentLength(pStream, songOffset, new byte[] { 0x00 });
                byte[] songName = ParseFile.ParseSimpleOffset(pStream, songOffset, songLength);
                
                tagHash.Add("Song Name", enc.GetString(songName));
                tagHash.Add("Uncompressed Data Size", "0x" + BitConverter.ToInt32(this.compressing, 0).ToString("X2"));
                tagHash.Add("Offset [Song Name]", "0x" + BitConverter.ToInt32(this.songNameOffset, 0).ToString("X2"));
            }
 
            // Offsets            
            tagHash.Add("Offset [Dump Data]", "0x" + BitConverter.ToInt32(this.dumpDataOffset, 0).ToString("X2"));
            tagHash.Add("Offset [Loop Point]", "0x" + BitConverter.ToInt32(this.loopPointOffset, 0).ToString("X2"));

        }
        
        private void initDeviceHash()
        {
            deviceHash.Add(0, "None");
            deviceHash.Add(1, "PSG (YM2149)");
            deviceHash.Add(2, "OPN (YM2203)");
            deviceHash.Add(3, "OPN2 (YM2612)");
            deviceHash.Add(4, "OPNA (YM2608)");
            deviceHash.Add(5, "OPM (YM2151)");
            deviceHash.Add(6, "OPLL (YM2413)");
            deviceHash.Add(7, "OPL (YM3526)");
            deviceHash.Add(8, "OPL2 (YM3812)");
            deviceHash.Add(9, "OPL3 (YMF262)");
            deviceHash.Add(15, "PSG (AY-3-8910)");
            deviceHash.Add(16, "DCSG (SN76489)");
        }

        public virtual void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();

            pChecksum.Update(this.version);
            pChecksum.Update(this.timerInfo);
            pChecksum.Update(this.timerInfo2);                
            pChecksum.Update(this.compressing);

            pChecksum.Update(this.data);
            
            // ADD LOOP POINT AS LOOP OFFSET - DATA OFFSET?
        }

        public virtual void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            pChecksum.Update(this.version);
            pChecksum.Update(this.timerInfo);
            pChecksum.Update(this.timerInfo2);
            pChecksum.Update(this.compressing);

            pMd5CryptoStream.Write(this.version, 0, this.version.Length);
            pMd5CryptoStream.Write(this.timerInfo, 0, this.timerInfo.Length);
            pMd5CryptoStream.Write(this.timerInfo2, 0, this.timerInfo2.Length);
            pMd5CryptoStream.Write(this.compressing, 0, this.compressing.Length);

            pSha1CryptoStream.Write(this.version, 0, this.version.Length);
            pSha1CryptoStream.Write(this.timerInfo, 0, this.timerInfo.Length);
            pSha1CryptoStream.Write(this.timerInfo2, 0, this.timerInfo2.Length);
            pSha1CryptoStream.Write(this.compressing, 0, this.compressing.Length);

            pChecksum.Update(this.data);
            pMd5CryptoStream.Write(this.data, 0, this.data.Length);
            pSha1CryptoStream.Write(this.data, 0, this.data.Length);

            // ADD LOOP POINT AS LOOP OFFSET - DATA OFFSET?
        }

        public virtual byte[] GetAsciiSignature()
        {
            return S98V1.ASCII_SIGNATURE;
        }

        public string GetFileExtensions()
        {
            return null;
        }

        public virtual string GetFormatAbbreviation()
        {
            return S98V1.FORMAT_ABBREVIATION;
        }

        public bool IsFileLibrary() { return false; }

        public bool HasMultipleFileExtensions()
        {
            return false;
        }

        public Dictionary<string, string> GetTagHash()
        {
            return this.tagHash;
        }

        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }

        #endregion        
    }
}
