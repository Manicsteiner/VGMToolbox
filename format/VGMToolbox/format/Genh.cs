using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Genh : IFormat
    {
        public static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x47, 0x45, 0x4E, 0x48 };
        private const string FORMAT_ABBREVIATION = "GENH";
        
        public static readonly byte[] CURRENT_VERSION = new byte[] { 0x56, 0x47, 0x30, 0x33 };
        
        public const UInt32 GENH_HEADER_SIZE = 0x1000;
        public const string FILE_EXTENSION = ".genh";
        public const string FILE_EXTENSION_HEADER = ".genh.header";
        public const string EMPTY_SAMPLE_COUNT = "-1";


        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x04;

        private const int CHANNELS_OFFSET = 0x04;
        private const int CHANNELS_LENGTH = 0x04;

        private const int INTERLEAVE_OFFSET = 0x08;
        private const int INTERLEAVE_LENGTH = 0x04;

        private const int FREQUENCY_OFFSET = 0x0C;
        private const int FREQUENCY_LENGTH = 0x04;

        private const int LOOP_START_OFFSET = 0x10;
        private const int LOOP_START_LENGTH = 0x04;

        private const int LOOP_END_OFFSET = 0x14;
        private const int LOOP_END_LENGTH = 0x04;

        private const int IDENTIFIER_OFFSET = 0x18;
        private const int IDENTIFIER_LENGTH = 0x04;

        private const int AUDIO_START_OFFSET = 0x1C;
        private const int AUDIO_START_LENGTH = 0x04;

        private const int HEADER_LENGTH_OFFSET = 0x20;
        private const int HEADER_LENGTH_LENGTH = 0x04;

        private const int LEFT_COEF_OFFSET = 0x24;
        private const int LEFT_COEF_LENGTH = 0x04;

        private const int RIGHT_COEF_OFFSET = 0x28;
        private const int RIGHT_COEF_LENGTH = 0x04;

        private const int DSP_INTERLEAVE_OFFSET = 0x2C;
        private const int DSP_INTERLEAVE_LENGTH = 0x04;

        private const int COEFFICIENT_TYPE_OFFSET = 0x30;
        private const int COEFFICIENT_TYPE_LENGTH = 0x01;

        private const int SPLIT_COEF1_OFFSET = 0x34;
        private const int SPLIT_COEF1_LENGTH = 0x04;

        private const int SPLIT_COEF2_OFFSET = 0x38;
        private const int SPLIT_COEF2_LENGTH = 0x04;
       
        public const int ORIG_FILENAME_OFFSET = 0x200;
        private const int ORIG_FILENAME_LENGTH = 0x100;

        public const int ORIG_FILESIZE_OFFSET = 0x300;
        private const int ORIG_FILESIZE_LENGTH = 0x04;

        public const int GENH_VERSION_OFFSET = 0x304;
        private const int GENH_VERSION_LENGTH = 0x04;

        //--------------------------------------------
        // Updates from March, 2017 - BEGIN
        //--------------------------------------------
        public  const int TOTAL_SAMPLES_OFFSET = 0x40;
        private const int TOTAL_SAMPLES_LENGTH = 0x04;

        public const int SKIP_SAMPLES_OFFSET = 0x44;
        private const int SKIP_SAMPLES_LENGTH = 0x04;

        public const int SKIP_SAMPLES_MODE_OFFSET = 0x48;
        private const int SKIP_SAMPLES_MODE_LENGTH = 0x01;
        public const byte SKIP_SAMPLES_MODE_AUTODETECT = 0;
        public const byte SKIP_SAMPLES_MODE_FORCE = 1;

        public const int ATRAC3_STEREO_MODE_OFFSET = 0x49;
        private const int ATRAC3_STEREO_MODE_LENGTH = 0x01;

        public const byte ATRAC3_STEREO_MODE_AUTODETECT_VALUE = 0x00;
        public const string ATRAC3_STEREO_MODE_AUTODETECT_DESCRIPTION = "Autodetect";
        public const byte ATRAC3_STEREO_MODE_FORCE_JOINT_STEREO_VALUE = 0x01;
        public const string ATRAC3_STEREO_MODE_FORCE_JOINT_STEREO_DESCRIPTION = "Joint Stereo";
        public const byte ATRAC3_STEREO_MODE_FORCE_FULL_STEREO_VALUE = 0x02;
        public const string ATRAC3_STEREO_MODE_FORCE_FULL_STEREO_DESCRIPTION = "Full Stereo";

        public const int XMA_STREAM_MODE_OFFSET = 0x4A;
        private const int XMA_STREAM_MODE_LENGTH = 0x01;

        public const byte XMA_STREAM_MODE_DEFAULT_VALUE = 0x00;
        public const string XMA_STREAM_MODE_DEFAULT_DESCRIPTION = "Default";
        public const byte XMA_STREAM_MODE_SINGLE_VALUE = 0x01;
        public const string XMA_STREAM_MODE_SINGLE_DESCRIPTION = "Single";

        public const int RAW_DATA_SIZE_OFFSET = 0x50;
        private const int RAW_DATA_SIZE_LENGTH = 0x04;
        //--------------------------------------------
        // Updates from March, 2017 - END
        //--------------------------------------------

        private byte[] asciiSignature;
        private byte[] channels;
        private byte[] interleave;
        private byte[] frequency;
        private byte[] loopStart;
        private byte[] loopEnd;
        private byte[] identifier;
        private byte[] audioStart;
        private byte[] headerLength;
        private byte[] originalFileName;
        private byte[] originalFileSize;                
        private byte[] versionNumber;

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public byte[] AsciiSignature { get { return asciiSignature; } }
        public byte[] Channels { get { return channels; } }
        public byte[] Interleave { get { return interleave; } }
        public byte[] Frequency { get { return frequency; } }
        public byte[] LoopStart { get { return loopStart; } }
        public byte[] LoopEnd { get { return loopEnd; } }
        public byte[] Identifier { get { return identifier; } }
        public byte[] AudioStart { get { return audioStart; } }
        public byte[] HeaderLength { get { return headerLength; } }
        public byte[] OriginalFileName { get { return originalFileName; } }
        public byte[] OriginalFileSize { get { return originalFileSize; } }
        public byte[] VersionNumber { get { return versionNumber; } }
        public byte[] LeftCoef { get; set; }
        public byte[] RightCoef { get; set; }
        public byte[] DspInterleave { get; set; }
        public byte[] CoefficientType { get; set; }
        public byte[] SplitCoef1 { get; set; }
        public byte[] SplitCoef2 { get; set; }

        public byte[] TotalSamples { get; set; }
        public byte[] SkipSamples { get; set; }
        public byte SkipSamplesMode { get; set; }
        public byte Atrac3StereoMode { get; set; }
        public byte XmaStreamMode { get; set; }
        public byte[] RawStreamSize { get; set; }

        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        #region IFormat

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath = pFilePath;            
            this.asciiSignature = ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
            this.channels = ParseFile.ParseSimpleOffset(pStream, CHANNELS_OFFSET, CHANNELS_LENGTH);
            this.interleave = ParseFile.ParseSimpleOffset(pStream, INTERLEAVE_OFFSET, INTERLEAVE_LENGTH);
            this.frequency = ParseFile.ParseSimpleOffset(pStream, FREQUENCY_OFFSET, FREQUENCY_LENGTH);
            this.loopStart = ParseFile.ParseSimpleOffset(pStream, LOOP_START_OFFSET, LOOP_START_LENGTH);
            this.loopEnd = ParseFile.ParseSimpleOffset(pStream, LOOP_END_OFFSET, LOOP_END_LENGTH);
            this.identifier = ParseFile.ParseSimpleOffset(pStream, IDENTIFIER_OFFSET, IDENTIFIER_LENGTH);
            this.audioStart = ParseFile.ParseSimpleOffset(pStream, AUDIO_START_OFFSET, AUDIO_START_LENGTH);
            this.headerLength = ParseFile.ParseSimpleOffset(pStream, HEADER_LENGTH_OFFSET, HEADER_LENGTH_LENGTH);
            this.originalFileName = ParseFile.ParseSimpleOffset(pStream, ORIG_FILENAME_OFFSET, ORIG_FILENAME_LENGTH);
            this.originalFileSize = ParseFile.ParseSimpleOffset(pStream, ORIG_FILESIZE_OFFSET, ORIG_FILESIZE_LENGTH);
            this.versionNumber = ParseFile.ParseSimpleOffset(pStream, GENH_VERSION_OFFSET, GENH_VERSION_LENGTH);

            this.LeftCoef = ParseFile.ParseSimpleOffset(pStream, LEFT_COEF_OFFSET, LEFT_COEF_LENGTH);
            this.RightCoef = ParseFile.ParseSimpleOffset(pStream, RIGHT_COEF_OFFSET, RIGHT_COEF_LENGTH);
            this.DspInterleave  = ParseFile.ParseSimpleOffset(pStream, DSP_INTERLEAVE_OFFSET, DSP_INTERLEAVE_LENGTH);
            this.CoefficientType = ParseFile.ParseSimpleOffset(pStream, COEFFICIENT_TYPE_OFFSET, COEFFICIENT_TYPE_LENGTH);
            this.SplitCoef1  = ParseFile.ParseSimpleOffset(pStream, SPLIT_COEF1_OFFSET, SPLIT_COEF1_LENGTH);
            this.SplitCoef2 = ParseFile.ParseSimpleOffset(pStream, SPLIT_COEF2_OFFSET, SPLIT_COEF2_LENGTH);

            this.TotalSamples = ParseFile.ParseSimpleOffset(pStream, TOTAL_SAMPLES_OFFSET, TOTAL_SAMPLES_LENGTH);
            this.SkipSamples = ParseFile.ParseSimpleOffset(pStream, SKIP_SAMPLES_OFFSET, SKIP_SAMPLES_LENGTH);
            this.SkipSamplesMode = ParseFile.ReadByte(pStream, SKIP_SAMPLES_MODE_OFFSET);
            this.Atrac3StereoMode = ParseFile.ReadByte(pStream, ATRAC3_STEREO_MODE_OFFSET);
            this.XmaStreamMode = ParseFile.ReadByte(pStream, XMA_STREAM_MODE_OFFSET);
            this.RawStreamSize = ParseFile.ParseSimpleOffset(pStream, RAW_DATA_SIZE_OFFSET, RAW_DATA_SIZE_LENGTH);

            this.initializeTagHash();
        }

        private void initializeTagHash()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            tagHash.Add("GENH Version", enc.GetString(FileUtil.ReplaceNullByteWithSpace(this.versionNumber)));
            tagHash.Add("Channels", String.Format("0x{0}", BitConverter.ToUInt32(this.channels, 0).ToString("X4")));
            tagHash.Add("Interleave", String.Format("0x{0}", BitConverter.ToUInt32(this.interleave, 0).ToString("X4")));
            tagHash.Add("Frequency", String.Format("0x{0}", BitConverter.ToUInt32(this.frequency, 0).ToString("X4")));
            tagHash.Add("Loop Start", String.Format("0x{0}", BitConverter.ToUInt32(this.loopStart, 0).ToString("X4")));
            tagHash.Add("Loop End", String.Format("0x{0}", BitConverter.ToUInt32(this.loopEnd, 0).ToString("X4")));
            tagHash.Add("Identifier", String.Format("0x{0}", BitConverter.ToUInt32(this.identifier, 0).ToString("X4")));
            tagHash.Add("Audio Start", String.Format("0x{0}", BitConverter.ToUInt32(this.audioStart, 0).ToString("X4")));
            tagHash.Add("GENH Header Length", String.Format("0x{0}", BitConverter.ToUInt32(this.headerLength, 0).ToString("X4")));            
            tagHash.Add("Original File Name", enc.GetString(FileUtil.ReplaceNullByteWithSpace(this.originalFileName)));
            tagHash.Add("Original File Size", String.Format("0x{0}", BitConverter.ToUInt32(this.originalFileSize, 0).ToString("X4")));            
        }

        public byte[] GetAsciiSignature()
        {
            return ASCII_SIGNATURE;
        }
        public string GetFileExtensions()
        {
            return null;
        }
        public string GetFormatAbbreviation()
        {
            return FORMAT_ABBREVIATION;
        }
        public bool IsFileLibrary() { return false; }
        public bool HasMultipleFileExtensions()
        {
            return false;
        }
        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }
        public Dictionary<string, string> GetTagHash()
        {
            return this.tagHash;
        }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();

            using (FileStream fs = File.Open(this.filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    br.BaseStream.Position = (long)BitConverter.ToUInt32(this.headerLength, 0);

                    byte[] data = new byte[Constants.FileReadChunkSize];
                    int bytesRead;

                    while ((bytesRead = br.Read(data, 0, data.Length)) > 0)
                    {
                        pChecksum.Update(data, 0, bytesRead);
                    }
                }               
            }
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            using (FileStream fs = File.Open(this.filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    br.BaseStream.Position = (long)BitConverter.ToUInt32(this.headerLength, 0);

                    byte[] data = new byte[Constants.FileReadChunkSize];
                    int bytesRead;

                    while ((bytesRead = br.Read(data, 0, data.Length)) > 0)
                    {
                        pChecksum.Update(data, 0, bytesRead);
                        pMd5CryptoStream.Write(data, 0, bytesRead);
                        pSha1CryptoStream.Write(data, 0, bytesRead);
                    }
                }
            }
        }

        #endregion
    }
}
