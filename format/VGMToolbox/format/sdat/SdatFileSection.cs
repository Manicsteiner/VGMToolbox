using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format.sdat
{
    public class SdatFileSection
    {
        public const int FILE_HEADER_SIGNATURE_OFFSET = 0x00;
        public const int FILE_HEADER_SIGNATURE_LENGTH = 4;

        public const int FILE_HEADER_SECTION_SIZE_OFFSET = 0x04;
        public const int FILE_HEADER_SECTION_SIZE_LENGTH = 4;

        public const int FILE_HEADER_NUMBER_OF_FILES_OFFSET = 0x08;
        public const int FILE_HEADER_NUMBER_OF_FILES_LENGTH = 4;

        public const int FILE_HEADER_RESERVED_OFFSET = 0x0C;
        public const int FILE_HEADER_RESERVED_LENGTH = 4;

        #region VARIABLES

        private byte[] fileHeaderSignature;
        private byte[] fileHeaderSectionSize;
        private byte[] fileHeaderNumberOfFiles;
        private byte[] fileHeaderReservedSection;

        public byte[] FileHeaderSignature { get { return fileHeaderSignature; } }
        public byte[] FileHeaderSectionSize { get { return fileHeaderSectionSize; } }
        public byte[] FileHeaderNumberOfFiles { get { return fileHeaderNumberOfFiles; } }
        public byte[] FileHeaderReservedSection { get { return fileHeaderReservedSection; } }

        #endregion

        #region METHODS

        public void Initialize(Stream pStream, int pSectionOffset)
        {
            fileHeaderSignature = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + FILE_HEADER_SIGNATURE_OFFSET,
                FILE_HEADER_SIGNATURE_LENGTH);
            fileHeaderSectionSize = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + FILE_HEADER_SECTION_SIZE_OFFSET,
                FILE_HEADER_SECTION_SIZE_LENGTH);
            fileHeaderNumberOfFiles = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + FILE_HEADER_NUMBER_OF_FILES_OFFSET,
                FILE_HEADER_NUMBER_OF_FILES_LENGTH);
            fileHeaderReservedSection = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + FILE_HEADER_RESERVED_OFFSET,
                FILE_HEADER_RESERVED_LENGTH);
        }

        #endregion
    }
}
