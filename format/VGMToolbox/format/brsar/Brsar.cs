using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;
using VGMToolbox.util;

namespace VGMToolbox.format.brsar
{
    class Brsar //: IFormat
    {
        public static readonly byte[] ASCII_SIGNATURE =
            new byte[] { 0x52, 0x53, 0x41, 0x52, 0xFE, 0xFF, 0x01, 0x04 }; // RSAR
        public const string ASCII_SIGNATURE_STRING = "52534152FEFF0104";

        private const string FORMAT_ABBREVIATION = "BRSAR";
        private const string HEX_PREFIX = "0x";
        public const string SDAT_FILE_EXTENSION = ".brsar";

        public const string SEQUENCE_FILE_EXTENSION = ".rseq";

        private static readonly byte[] EMPTY_WAVEARC = new byte[] { 0xFF, 0xFF };
        public const int NO_SEQUENCE_RESTRICTION = -1;

        ///////////////////////////////////
        // Standard Header Information
        /// ///////////////////////////////
        public const int STD_HEADER_SIGNATURE_OFFSET = 0x00;
        public const int STD_HEADER_SIGNATURE_LENGTH = 8;

        public const int STD_HEADER_FILE_SIZE_OFFSET = 0x08;
        public const int STD_HEADER_FILE_SIZE_LENGTH = 4;

        public const int STD_HEADER_HEADER_SIZE_OFFSET = 0x0C;
        public const int STD_HEADER_HEADER_SIZE_LENGTH = 2;

        public const int STD_HEADER_NUMBER_OF_SECTIONS_OFFSET = 0x0E;
        public const int STD_HEADER_NUMBER_OF_SECTIONS_LENGTH = 2;

        public byte[] StdHeaderSignature { set; get; }
        public byte[] StdHeaderFileSize { set; get; }
        public byte[] StdHeaderHeaderSize { set; get; }
        public byte[] StdHeaderNumberOfSections { set; get; }

        ////////////////
        // BRSAR HEADER
        ////////////////

        // SYMB
        public const int BRSAR_HEADER_SYMB_OFFSET_OFFSET = 0x10;
        public const int BRSAR_HEADER_SYMB_OFFSET_LENGTH = 4;
        public const int BRSAR_HEADER_SYMB_SIZE_OFFSET = 0x14;
        public const int BRSAR_HEADER_SYMB_SIZE_LENGTH = 4;

        public byte[] BrsarHeaderSymbOffset { set; get; }
        public byte[] BrsarHeaderSymbSize { set; get; }

        public BrsarSymbSection SymbSection { set; get; }
        
        // INFO
        public const int BRSAR_HEADER_INFO_OFFSET_OFFSET = 0x18;
        public const int BRSAR_HEADER_INFO_OFFSET_LENGTH = 4;
        public const int BRSAR_HEADER_INFO_SIZE_OFFSET = 0x1C;
        public const int BRSAR_HEADER_INFO_SIZE_LENGTH = 4;

        public byte[] BrsarHeaderInfoOffset { set; get; }
        public byte[] BrsarHeaderInfoSize { set; get; }

        // FILE
        public const int BRSAR_HEADER_FILE_OFFSET_OFFSET = 0x20;
        public const int BRSAR_HEADER_FILE_OFFSET_LENGTH = 4;
        public const int BRSAR_HEADER_FILE_SIZE_OFFSET = 0x24;
        public const int BRSAR_HEADER_FILE_SIZE_LENGTH = 4;

        public byte[] BrsarHeaderFileOffset { set; get; }
        public byte[] BrsarHeaderFileSize { set; get; }









        // File Path
        public string FilePath { set; get; }

        // Tag Hash
        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.FilePath = pFilePath;
            
            ///////////////////
            // standard header
            ///////////////////
            this.StdHeaderSignature = ParseFile.ParseSimpleOffset(pStream, STD_HEADER_SIGNATURE_OFFSET, STD_HEADER_SIGNATURE_LENGTH);
            this.StdHeaderFileSize = ParseFile.ParseSimpleOffset(pStream, STD_HEADER_FILE_SIZE_OFFSET, STD_HEADER_FILE_SIZE_LENGTH);
            this.StdHeaderHeaderSize = ParseFile.ParseSimpleOffset(pStream, STD_HEADER_HEADER_SIZE_OFFSET, STD_HEADER_HEADER_SIZE_LENGTH);
            this.StdHeaderNumberOfSections = ParseFile.ParseSimpleOffset(pStream, STD_HEADER_NUMBER_OF_SECTIONS_OFFSET, STD_HEADER_NUMBER_OF_SECTIONS_LENGTH);
            
            Array.Reverse(this.StdHeaderFileSize);
            Array.Reverse(this.StdHeaderHeaderSize);
            Array.Reverse(this.StdHeaderNumberOfSections);

            ////////////////
            // brsar header
            ////////////////
            
            // SYMB
            BrsarHeaderSymbOffset = ParseFile.ParseSimpleOffset(pStream, BRSAR_HEADER_SYMB_OFFSET_OFFSET, BRSAR_HEADER_SYMB_OFFSET_LENGTH);
            BrsarHeaderSymbSize = ParseFile.ParseSimpleOffset(pStream, BRSAR_HEADER_SYMB_SIZE_OFFSET, BRSAR_HEADER_SYMB_SIZE_LENGTH);

            Array.Reverse(this.BrsarHeaderSymbOffset);
            Array.Reverse(this.BrsarHeaderSymbSize);

            // INFO
            BrsarHeaderInfoOffset = ParseFile.ParseSimpleOffset(pStream, BRSAR_HEADER_INFO_OFFSET_OFFSET, BRSAR_HEADER_INFO_OFFSET_LENGTH);
            BrsarHeaderInfoSize = ParseFile.ParseSimpleOffset(pStream, BRSAR_HEADER_INFO_SIZE_OFFSET, BRSAR_HEADER_INFO_SIZE_LENGTH);

            Array.Reverse(this.BrsarHeaderInfoOffset);
            Array.Reverse(this.BrsarHeaderInfoSize);

            // FILE
            BrsarHeaderFileOffset = ParseFile.ParseSimpleOffset(pStream, BRSAR_HEADER_FILE_OFFSET_OFFSET, BRSAR_HEADER_FILE_OFFSET_LENGTH);
            BrsarHeaderFileSize = ParseFile.ParseSimpleOffset(pStream, BRSAR_HEADER_FILE_SIZE_OFFSET, BRSAR_HEADER_FILE_SIZE_LENGTH);

            Array.Reverse(this.BrsarHeaderFileOffset);
            Array.Reverse(this.BrsarHeaderFileSize);

            // SYMB Section
            if (BitConverter.ToUInt32(this.BrsarHeaderSymbSize, 0) > 0)
            {
                SymbSection = new BrsarSymbSection();
                SymbSection.Initialize(pStream, BitConverter.ToInt32(this.BrsarHeaderSymbOffset, 0));
            }


            this.initializeTagHash();
        }

        private void initializeTagHash()
        {
            #region initializeTagHash - BRSAR

            tagHash.Add("File Size", HEX_PREFIX + BitConverter.ToUInt32(this.StdHeaderFileSize, 0).ToString("X4"));
            tagHash.Add("Header Size", HEX_PREFIX + BitConverter.ToUInt16(this.StdHeaderHeaderSize, 0).ToString("X2"));
            tagHash.Add("Number of Sections", HEX_PREFIX + BitConverter.ToUInt16(this.StdHeaderNumberOfSections, 0).ToString("X2"));
            
            tagHash.Add("SYMB Offset, Length",
                HEX_PREFIX + BitConverter.ToUInt32(this.BrsarHeaderSymbOffset, 0).ToString("X4") + ", " +
                HEX_PREFIX + BitConverter.ToUInt32(this.BrsarHeaderSymbSize, 0).ToString("X4"));
            
            tagHash.Add("INFO Offset, Length",
                HEX_PREFIX + BitConverter.ToUInt32(this.BrsarHeaderInfoOffset, 0).ToString("X4") + ", " +
                HEX_PREFIX + BitConverter.ToUInt32(this.BrsarHeaderInfoSize, 0).ToString("X4"));

            tagHash.Add("FILE Offset, Length",
                HEX_PREFIX + BitConverter.ToUInt32(this.BrsarHeaderFileOffset, 0).ToString("X4") + ", " +
                HEX_PREFIX + BitConverter.ToUInt32(this.BrsarHeaderFileSize, 0).ToString("X4"));
            #endregion

            #region SYMB info
            
            if (SymbSection != null)
            {
                tagHash.Add("SYMB - Section Size", HEX_PREFIX + BitConverter.ToUInt32(SymbSection.StdHeaderSectionSize, 0).ToString("X4"));
            }

            #endregion
        }

        #region IFormat Required Functions

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
        public void GetDatFileCrc32(ref Crc32 pChecksum) { pChecksum.Reset(); }
        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream) { }

        #endregion
    }
}
