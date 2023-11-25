using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format.sdat
{
    public class SdatFatSection
    {
        // Section Header
        public const int FAT_HEADER_SIGNATURE_OFFSET = 0x00;
        public const int FAT_HEADER_SIGNATURE_LENGTH = 4;

        public const int FAT_HEADER_SECTION_SIZE_OFFSET = 0x04;
        public const int FAT_HEADER_SECTION_SIZE_LENGTH = 4;

        public const int FAT_HEADER_NUMBER_OF_FILES_OFFSET = 0x08;
        public const int FAT_HEADER_NUMBER_OF_FILES_LENGTH = 4;

        // FAT Record - Offsets are relative
        public const int FAT_RECORD_FILE_OFFSET_OFFSET = 0x00;
        public const int FAT_RECORD_FILE_OFFSET_LENGTH = 4;

        public const int FAT_RECORD_FILE_SIZE_OFFSET = 0x04;
        public const int FAT_RECORD_FILE_SIZE_LENGTH = 4;

        public const int FAT_RECORD_RESERVED_OFFSET = 0x08;
        public const int FAT_RECORD_RESERVED_LENGTH = 8;

        # region STRUCT

        public struct SdatFatRec
        {
            public byte[] nOffset;
            public byte[] nSize;
            public byte[] reserved;
        }

        # endregion

        #region VARIABLES

        private byte[] fatHeaderSignature;
        private byte[] fatHeaderSectionSize;
        private byte[] fatHeaderNumberOfFiles;
        private SdatFatRec[] sdatFatRecs;

        public byte[] FatHeaderSignature { get { return fatHeaderSignature; } }
        public byte[] FatHeaderSectionSize { get { return fatHeaderSectionSize; } }
        public byte[] FatHeaderNumberOfFiles { get { return fatHeaderNumberOfFiles; } }
        public SdatFatRec[] SdatFatRecs { get { return sdatFatRecs; } }

        #endregion

        # region METHODS

        private void getSdatFatRecs(Stream pStream, int pSectionOffset)
        {
            // build this to get the offset past the header
            int fatHeaderSize = FAT_HEADER_SIGNATURE_LENGTH + FAT_HEADER_SECTION_SIZE_LENGTH +
                FAT_HEADER_NUMBER_OF_FILES_LENGTH;

            // set the size of the record to perform a single read
            int fatRecordSize = FAT_RECORD_FILE_OFFSET_LENGTH + FAT_RECORD_FILE_SIZE_LENGTH +
                FAT_RECORD_RESERVED_LENGTH;

            int numberOfFatRecs = BitConverter.ToInt32(this.fatHeaderNumberOfFiles, 0);
            this.sdatFatRecs = new SdatFatRec[numberOfFatRecs];

            for (int i = 0; i < numberOfFatRecs; i++)
            {
                // get our offset for the current record
                int offsetAfterHeader = (i == 0 ? 0 : (i * fatRecordSize));

                // get our current record
                byte[] tempBytes = ParseFile.ParseSimpleOffset(pStream,
                    pSectionOffset + fatHeaderSize + offsetAfterHeader,
                    fatRecordSize);

                // initialize struct
                sdatFatRecs[i] = new SdatFatRec();
                sdatFatRecs[i].nOffset = new byte[FAT_RECORD_FILE_OFFSET_LENGTH];
                sdatFatRecs[i].nSize = new byte[FAT_RECORD_FILE_SIZE_LENGTH];
                sdatFatRecs[i].reserved = new byte[FAT_RECORD_RESERVED_LENGTH];

                // assign bytes to struct
                Array.Copy(tempBytes, FAT_RECORD_FILE_OFFSET_OFFSET,
                    sdatFatRecs[i].nOffset, 0, FAT_RECORD_FILE_OFFSET_LENGTH);
                Array.Copy(tempBytes, FAT_RECORD_FILE_SIZE_OFFSET,
                    sdatFatRecs[i].nSize, 0, FAT_RECORD_FILE_SIZE_LENGTH);
                Array.Copy(tempBytes, FAT_RECORD_RESERVED_OFFSET,
                    sdatFatRecs[i].reserved, 0, FAT_RECORD_RESERVED_LENGTH);
            }

        }

        public void Initialize(Stream pStream, int pSectionOffset)
        {
            fatHeaderSignature = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + FAT_HEADER_SIGNATURE_OFFSET,
                FAT_HEADER_SIGNATURE_LENGTH);
            fatHeaderSectionSize = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + FAT_HEADER_SECTION_SIZE_OFFSET,
                FAT_HEADER_SECTION_SIZE_LENGTH);
            fatHeaderNumberOfFiles = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + FAT_HEADER_NUMBER_OF_FILES_OFFSET,
                FAT_HEADER_NUMBER_OF_FILES_LENGTH);

            getSdatFatRecs(pStream, pSectionOffset);
        }

        # endregion

    }
}
