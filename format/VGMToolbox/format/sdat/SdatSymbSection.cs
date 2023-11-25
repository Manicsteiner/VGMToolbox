using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format.sdat
{
    public class SdatSymbSection
    {
        public SdatSymbSection() { }

        private static readonly byte[] NULL_BYTE_ARRAY = new byte[] { 0x00 }; // NULL

        public const int STD_HEADER_SIGNATURE_OFFSET = 0x00;
        public const int STD_HEADER_SIGNATURE_LENGTH = 4;

        public const int STD_HEADER_SECTION_SIZE_OFFSET = 0x04;
        public const int STD_HEADER_SECTION_SIZE_LENGTH = 4;

        // offsets, releative to this section
        public const int SYMB_RECORD_SEQ_OFFSET_OFFSET = 0x08;
        public const int SYMB_RECORD_SEQ_OFFSET_LENGTH = 4;

        public const int SYMB_RECORD_SEQARC_OFFSET_OFFSET = 0x0C;
        public const int SYMB_RECORD_SEQARC_OFFSET_LENGTH = 4;

        public const int SYMB_RECORD_BANK_OFFSET_OFFSET = 0x10;
        public const int SYMB_RECORD_BANK_OFFSET_LENGTH = 4;

        public const int SYMB_RECORD_WAVEARC_OFFSET_OFFSET = 0x14;
        public const int SYMB_RECORD_WAVEARC_OFFSET_LENGTH = 4;

        public const int SYMB_RECORD_PLAYER_OFFSET_OFFSET = 0x18;
        public const int SYMB_RECORD_PLAYER_OFFSET_LENGTH = 4;

        public const int SYMB_RECORD_GROUP_OFFSET_OFFSET = 0x1C;
        public const int SYMB_RECORD_GROUP_OFFSET_LENGTH = 4;

        // conflict between specs here!
        // http://tahaxan.arcnor.com/index.php?option=com_content&task=view&id=38&Itemid=36
        // http://kiwi.ds.googlepages.com/sdat.html

        public const int SYMB_RECORD_PLAYER2_OFFSET_OFFSET = 0x20;
        public const int SYMB_RECORD_PLAYER2_OFFSET_LENGTH = 4;

        public const int SYMB_RECORD_STRM_OFFSET_OFFSET = 0x24;
        public const int SYMB_RECORD_STRM_OFFSET_LENGTH = 4;

        public const int SYMB_RECORD_UNUSED_OFFSET = 0x28;
        public const int SYMB_RECORD_UNUSED_LENGTH = 24;

        // THESE SHOULD EXIST FOR EACH SECTION
        public const int SYMB_ENTRY_NUM_FILES_OFFSET = 0x00;        // RELATIVE TO SECTION OFFSET
        public const int SYMB_ENTRY_NUM_FILES_LENGTH = 4;

        public const int SYMB_ENTRY_FILE_NAME_SIZE = 4;

        // !!!!! Add stuff here for subrecords for SEQARC
        public const int SYMB_ENTRY_SEQARC_COUNT_OFFSET = 0x00;
        public const int SYMB_ENTRY_SEQARC_COUNT_LENGTH = 4;

        public const int SYMB_ENTRY_SEQARC_SUBENTRY_FILENAME_OFFSET = 0x00;
        public const int SYMB_ENTRY_SEQARC_SUBENTRY_FILENAME_LENGTH = 4;

        public const int SYMB_ENTRY_SEQARC_SUBENTRY_SUBREC_OFFSET_OFFSET = 0x04;
        public const int SYMB_ENTRY_SEQARC_SUBENTRY_SUBREC_OFFSET_LENGTH = 4;

        /////////////
        // Variables
        /////////////        

        // private
        private byte[] stdHeaderSignature;
        private byte[] stdHeaderSectionSize;

        private byte[] symbRecordSeqOffset;
        private byte[] symbRecordSeqArcOffset;
        private byte[] symbRecordBankOffset;
        private byte[] symbRecordWaveArcOffset;
        private byte[] symbRecordPlayerOffset;
        private byte[] symbRecordGroupOffset;
        private byte[] symbRecordPlayer2Offset;
        private byte[] symbRecordStrmOffset;

        private byte[][] symbSeqSubRecords;
        private byte[][] symbSeqFileNames;

        private byte[][] symbSeqArcFileNames;

        private byte[][] symbBankSubRecords;
        private byte[][] symbBankFileNames;
        private byte[][] symbWaveArcSubRecords;
        private byte[][] symbWaveArcFileNames;
        private byte[][] symbPlayerSubRecords;
        private byte[][] symbPlayerFileNames;
        private byte[][] symbGroupSubRecords;
        private byte[][] symbGroupFileNames;
        private byte[][] symbPlayer2SubRecords;
        private byte[][] symbPlayer2FileNames;
        private byte[][] symbStrmSubRecords;
        private byte[][] symbStrmFileNames;

        private SdatSymbolRec sdatSymbolRecSeq;
        
        private SdatSymbolRec sdatSymbolRecSeqArc;
        
        private SdatSymbolRec sdatSymbolRecBank;
        private SdatSymbolRec sdatSymbolRecWaveArc;
        private SdatSymbolRec sdatSymbolRecPlayer;
        private SdatSymbolRec sdatSymbolRecGroup;
        private SdatSymbolRec sdatSymbolRecPlayer2;
        private SdatSymbolRec sdatSymbolRecStrm;

        public struct SdatSymbolRec
        {
            public byte[] nCount;
            public byte[][] nEntryOffsets;
        }

        // public
        public byte[] StdHeaderSignature { get { return stdHeaderSignature; } }
        public byte[] StdHeaderSectionSize { get { return stdHeaderSectionSize; } }

        public byte[] SymbRecordSeqOffset { get { return symbRecordSeqOffset; } }
        public byte[] SymbRecordSeqArcOffset { get { return symbRecordSeqArcOffset; } }
        public byte[] SymbRecordBankOffset { get { return symbRecordBankOffset; } }
        public byte[] SymbRecordWaveArcOffset { get { return symbRecordWaveArcOffset; } }
        public byte[] SymbRecordPlayerOffset { get { return symbRecordPlayerOffset; } }
        public byte[] SymbRecordGroupOffset { get { return symbRecordGroupOffset; } }
        public byte[] SymbRecordPlayer2Offset { get { return symbRecordPlayer2Offset; } }
        public byte[] SymbRecordStrmOffset { get { return symbRecordStrmOffset; } }

        public string[] SymbSeqFileNames { get { return getSymbFileNames(symbSeqFileNames); } }
        public string[] SymbSeqArcFileNames { get { return getSymbFileNames(symbSeqArcFileNames); } }
        public string[] SymbBankFileNames { get { return getSymbFileNames(symbBankFileNames); } }
        public string[] SymbWaveArcFileNames { get { return getSymbFileNames(symbWaveArcFileNames); } }
        public string[] SymbPlayerFileNames { get { return getSymbFileNames(symbPlayerFileNames); } }
        public string[] SymbGroupFileNames { get { return getSymbFileNames(symbGroupFileNames); } }
        public string[] SymbPlayer2FileNames { get { return getSymbFileNames(symbPlayer2FileNames); } }
        public string[] SymbStrmFileNames { get { return getSymbFileNames(symbStrmFileNames); } }

        private string[] getSymbFileNames(byte[][] pFileNames)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            int titleCount = pFileNames.GetLength(0);
            string[] values = new string[pFileNames.GetLength(0)];

            for (int i = 0; i < titleCount; i++)
            {
                if (pFileNames[i] != null)
                {
                    values[i] = enc.GetString(pFileNames[i]);
                }
            }

            return values;
        }

        ////////////
        // METHODS
        ////////////
        public byte[] getStdHeaderSignature(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + STD_HEADER_SIGNATURE_OFFSET,
                STD_HEADER_SIGNATURE_LENGTH);
        }
        public byte[] getStdHeaderSectionSize(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + STD_HEADER_SECTION_SIZE_OFFSET,
                STD_HEADER_SECTION_SIZE_LENGTH);
        }

        public byte[] getSymbRecordSeqOffset(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + SYMB_RECORD_SEQ_OFFSET_OFFSET,
                SYMB_RECORD_SEQ_OFFSET_LENGTH);
        }
        public byte[] getSymbRecordSeqArcOffset(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + SYMB_RECORD_SEQARC_OFFSET_OFFSET,
                SYMB_RECORD_SEQARC_OFFSET_LENGTH);
        }
        public byte[] getSymbRecordBankOffset(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + SYMB_RECORD_BANK_OFFSET_OFFSET,
                SYMB_RECORD_BANK_OFFSET_LENGTH);
        }
        public byte[] getSymbRecordWaveArcOffset(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + SYMB_RECORD_WAVEARC_OFFSET_OFFSET,
                SYMB_RECORD_WAVEARC_OFFSET_LENGTH);
        }
        public byte[] getSymbRecordPlayerOffset(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + SYMB_RECORD_PLAYER_OFFSET_OFFSET,
                SYMB_RECORD_PLAYER_OFFSET_LENGTH);
        }
        public byte[] getSymbRecordGroupOffset(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + SYMB_RECORD_GROUP_OFFSET_OFFSET,
                SYMB_RECORD_GROUP_OFFSET_LENGTH);
        }
        public byte[] getSymbRecordPlayer2Offset(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + SYMB_RECORD_PLAYER2_OFFSET_OFFSET,
                SYMB_RECORD_PLAYER2_OFFSET_LENGTH);
        }
        public byte[] getSymbRecordStrmOffset(Stream pStream, int pSectionOffset)
        {
            return ParseFile.ParseSimpleOffset(pStream, pSectionOffset + SYMB_RECORD_STRM_OFFSET_OFFSET,
                SYMB_RECORD_STRM_OFFSET_LENGTH);
        }

        public void getSeqArcSymbRecord(Stream pStream, int pSectionOffset, int pSubSectionOffset,
            ref byte[][] pSymbFileNames, ref SdatSymbolRec pSdatSymbolRec)
        {
            if (pSubSectionOffset > 0)
            {
                pSdatSymbolRec = new SdatSymbolRec();

                pSdatSymbolRec.nCount = ParseFile.ParseSimpleOffset(pStream,
                    pSectionOffset + pSubSectionOffset + SYMB_ENTRY_SEQARC_COUNT_OFFSET,
                    SYMB_ENTRY_SEQARC_COUNT_LENGTH);

                int subRecordCount = BitConverter.ToInt32(pSdatSymbolRec.nCount, 0);

                pSymbFileNames = new byte[subRecordCount][];

                pSdatSymbolRec.nEntryOffsets = new byte[subRecordCount][];

                for (int i = 1; i <= subRecordCount; i++)
                {
                    pSdatSymbolRec.nEntryOffsets[i - 1] = ParseFile.ParseSimpleOffset(pStream,
                        pSectionOffset + pSubSectionOffset + SYMB_ENTRY_NUM_FILES_OFFSET + (SYMB_ENTRY_FILE_NAME_SIZE * i) + (SYMB_ENTRY_SEQARC_SUBENTRY_SUBREC_OFFSET_LENGTH * (i - 1)),
                        SYMB_ENTRY_NUM_FILES_LENGTH);

                    int fileOffset = BitConverter.ToInt32(pSdatSymbolRec.nEntryOffsets[i - 1], 0);

                    if (fileOffset > 0)
                    {
                        int fileLength = ParseFile.GetSegmentLength(pStream, pSectionOffset + fileOffset, NULL_BYTE_ARRAY);
                        pSymbFileNames[i - 1] = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + fileOffset, fileLength);
                    }
                    else
                    {
                        pSymbFileNames[i - 1] = null;
                    }
                }
            }
        }

        public void getSymbRecord(Stream pStream, int pSectionOffset, int pSubSectionOffset,
            ref byte[][] pSymbFileNames, ref SdatSymbolRec pSdatSymbolRec)
        {
            if (pSubSectionOffset > 0)
            {
                pSdatSymbolRec = new SdatSymbolRec();

                pSdatSymbolRec.nCount = ParseFile.ParseSimpleOffset(pStream,
                    pSectionOffset + pSubSectionOffset + SYMB_ENTRY_NUM_FILES_OFFSET,
                    SYMB_ENTRY_NUM_FILES_LENGTH);

                int subRecordCount = BitConverter.ToInt32(pSdatSymbolRec.nCount, 0);

                pSymbFileNames = new byte[subRecordCount][];

                pSdatSymbolRec.nEntryOffsets = new byte[subRecordCount][];

                for (int i = 1; i <= subRecordCount; i++)
                {
                    pSdatSymbolRec.nEntryOffsets[i - 1] = ParseFile.ParseSimpleOffset(pStream,
                        pSectionOffset + pSubSectionOffset + SYMB_ENTRY_NUM_FILES_OFFSET + (SYMB_ENTRY_FILE_NAME_SIZE * i),
                        SYMB_ENTRY_NUM_FILES_LENGTH);

                    int fileOffset = BitConverter.ToInt32(pSdatSymbolRec.nEntryOffsets[i - 1], 0);

                    if (fileOffset > 0)
                    {
                        int fileLength = ParseFile.GetSegmentLength(pStream, pSectionOffset + fileOffset, NULL_BYTE_ARRAY);
                        pSymbFileNames[i - 1] = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + fileOffset, fileLength);
                    }
                    else
                    {
                        pSymbFileNames[i - 1] = null;
                    }
                }
            }
        }

        public void Initialize(Stream pStream, int pSectionOffset)
        {            
            int intSymbRecordOffset;

            stdHeaderSignature = getStdHeaderSignature(pStream, pSectionOffset);
            stdHeaderSectionSize = getStdHeaderSectionSize(pStream, pSectionOffset);

            // SEQ
            symbRecordSeqOffset = getSymbRecordSeqOffset(pStream, pSectionOffset);
            intSymbRecordOffset = BitConverter.ToInt32(symbRecordSeqOffset, 0);
            getSymbRecord(pStream, pSectionOffset, intSymbRecordOffset,
                ref symbSeqFileNames, ref sdatSymbolRecSeq);

            // @TODO: Get subrecords for each SEQARC
            symbRecordSeqArcOffset = getSymbRecordSeqArcOffset(pStream, pSectionOffset);
            intSymbRecordOffset = BitConverter.ToInt32(symbRecordSeqArcOffset, 0);
            getSeqArcSymbRecord(pStream, pSectionOffset, intSymbRecordOffset,
                ref symbSeqArcFileNames, ref sdatSymbolRecSeqArc);

            // BANK
            symbRecordBankOffset = getSymbRecordBankOffset(pStream, pSectionOffset);
            intSymbRecordOffset = BitConverter.ToInt32(symbRecordBankOffset, 0);
            getSymbRecord(pStream, pSectionOffset, intSymbRecordOffset,
                ref symbBankFileNames, ref sdatSymbolRecBank);


            // WAVEARC
            symbRecordWaveArcOffset = getSymbRecordWaveArcOffset(pStream, pSectionOffset);
            intSymbRecordOffset = BitConverter.ToInt32(symbRecordWaveArcOffset, 0);
            getSymbRecord(pStream, pSectionOffset, intSymbRecordOffset,
                ref symbWaveArcFileNames, ref sdatSymbolRecWaveArc);

            // PLAYER
            symbRecordPlayerOffset = getSymbRecordPlayerOffset(pStream, pSectionOffset);
            intSymbRecordOffset = BitConverter.ToInt32(symbRecordPlayerOffset, 0);
            getSymbRecord(pStream, pSectionOffset, intSymbRecordOffset,
                ref symbPlayerFileNames, ref sdatSymbolRecPlayer);

            // GROUP
            symbRecordGroupOffset = getSymbRecordGroupOffset(pStream, pSectionOffset);
            intSymbRecordOffset = BitConverter.ToInt32(symbRecordGroupOffset, 0);
            getSymbRecord(pStream, pSectionOffset, intSymbRecordOffset,
                ref symbGroupFileNames, ref sdatSymbolRecGroup);

            // PLAYER2
            symbRecordPlayer2Offset = getSymbRecordPlayer2Offset(pStream, pSectionOffset);
            intSymbRecordOffset = BitConverter.ToInt32(symbRecordPlayer2Offset, 0);
            getSymbRecord(pStream, pSectionOffset, intSymbRecordOffset,
                ref symbPlayer2FileNames, ref sdatSymbolRecPlayer2);

            // STRM
            symbRecordStrmOffset = getSymbRecordStrmOffset(pStream, pSectionOffset);
            intSymbRecordOffset = BitConverter.ToInt32(symbRecordStrmOffset, 0);
            getSymbRecord(pStream, pSectionOffset, intSymbRecordOffset,
                ref symbStrmFileNames, ref sdatSymbolRecStrm);
        }

    }
}
