using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.util;

namespace VGMToolbox.format.sdat
{
    public class Swar :IFormat
    {
        public static readonly byte[] ASCII_SIGNATURE =
            new byte[] { 0x53, 0x57, 0x41, 0x52, 0xFF, 0xFE, 0x00, 0x01 }; // SWAR

        private const string FORMAT_ABBREVIATION = "SWAR";
        public const string FILE_EXTENSION = ".swar";

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

        // SWAR specific info
        byte[] dataBytes;
        UInt32 swarHeaderSize;
        byte[] reserved; // 8 * 32bit blocks
        UInt32 numberofSamples;
        UInt32[] sampleOffsets;

        public UInt32[] SampleOffsets { get { return sampleOffsets; } }

        // FILE INFO
        public const int STD_HEADER_SIGNATURE_OFFSET = 0x00;
        public const int STD_HEADER_SIGNATURE_LENGTH = 0x08;

        public const int STD_HEADER_FILE_SIZE_OFFSET = 0x08;
        public const int STD_HEADER_FILE_SIZE_LENGTH = 4;

        public const int STD_HEADER_HEADER_SIZE_OFFSET = 0x0C;
        public const int STD_HEADER_HEADER_SIZE_LENGTH = 2;

        public const int STD_HEADER_NUMBER_OF_SECTIONS_OFFSET = 0x0E;
        public const int STD_HEADER_NUMBER_OF_SECTIONS_LENGTH = 2;

        // SWAR INFO
        public const int SWAR_HEADER_SIGNATURE_OFFSET = 0x10;
        public const int SWAR_HEADER_SIGNATURE_LENGTH = 0x04;

        public const int SWAR_HEADER_SIZE_OFFSET = 0x14;
        public const int SWAR_HEADER_SIZE_LENGTH = 4;

        public const int SWAR_HEADER_RESERVED_OFFSET = 0x18;
        public const int SWAR_HEADER_RESERVED_LENGTH = 0x20;

        public const int SWAR_HEADER_NUM_SAMPLES_OFFSET = 0x38;
        public const int SWAR_HEADER_NUM_SAMPLES_LENGTH = 0x04;

        public const int SWAR_HEADER_SAMPLE_OFFSETS_OFFSET = 0x3C;
        public const int SWAR_HEADER_SAMPLE_OFFSETS_LENGTH = 0x04;

        public Swar() { }

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath = pFilePath;

            this.magicBytes = ParseFile.ParseSimpleOffset(pStream, STD_HEADER_SIGNATURE_OFFSET, STD_HEADER_SIGNATURE_LENGTH);
            this.fileSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, STD_HEADER_FILE_SIZE_OFFSET, STD_HEADER_FILE_SIZE_LENGTH), 0);
            this.fileHeaderSize = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(pStream, STD_HEADER_HEADER_SIZE_OFFSET, STD_HEADER_HEADER_SIZE_LENGTH), 0);
            this.numberOfBlocks = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(pStream, STD_HEADER_NUMBER_OF_SECTIONS_OFFSET, STD_HEADER_NUMBER_OF_SECTIONS_LENGTH), 0);

            this.dataBytes = ParseFile.ParseSimpleOffset(pStream, SWAR_HEADER_SIGNATURE_OFFSET, SWAR_HEADER_SIGNATURE_LENGTH);
            this.swarHeaderSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, SWAR_HEADER_SIZE_OFFSET, SWAR_HEADER_SIZE_LENGTH), 0);
            this.reserved = ParseFile.ParseSimpleOffset(pStream, SWAR_HEADER_RESERVED_OFFSET, SWAR_HEADER_RESERVED_LENGTH);
            this.numberofSamples = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, SWAR_HEADER_NUM_SAMPLES_OFFSET, SWAR_HEADER_NUM_SAMPLES_LENGTH), 0);

            this.sampleOffsets = new UInt32[this.numberofSamples];

            for (int i = 0; i < this.sampleOffsets.Length; i++)
            {
                this.sampleOffsets[i] = 
                    BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, SWAR_HEADER_SAMPLE_OFFSETS_OFFSET + (SWAR_HEADER_SAMPLE_OFFSETS_LENGTH * i), SWAR_HEADER_SAMPLE_OFFSETS_LENGTH), 0);
            }
        }

        #region IFormat requirements

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

        public bool HasMultipleFileExtensions() { return false; }

        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }

        public Dictionary<string, string> GetTagHash() { return this.tagHash; }
        public void GetDatFileCrc32(ref Crc32 pChecksum) { pChecksum.Reset();}
        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream) { }
        
        #endregion
    }
}
