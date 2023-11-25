using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format.brsar
{
    class BrsarSymbSection
    {
        public BrsarSymbSection() { }

        private static readonly byte[] NULL_BYTE_ARRAY = new byte[] { 0x00 }; // NULL

        ///////////////////
        // standard header
        ///////////////////
        public const int STD_HEADER_SIGNATURE_OFFSET = 0x00;
        public const int STD_HEADER_SIGNATURE_LENGTH = 4;
        
        public const int STD_HEADER_SECTION_SIZE_OFFSET = 0x04;
        public const int STD_HEADER_SECTION_SIZE_LENGTH = 4;

        public const int SYMB_RECORD_OFFSET_OFFSET = 0x08;
        public const int SYMB_RECORD_OFFSET_LENGTH = 4;

        public byte[] StdHeaderSignature { set; get; }
        public byte[] StdHeaderSectionSize { set; get; }



        public void Initialize(Stream pStream, int pSectionOffset)
        {
            ///////////////////
            // standard header
            ///////////////////
            this.StdHeaderSignature = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + STD_HEADER_SIGNATURE_OFFSET, STD_HEADER_SIGNATURE_LENGTH);
            this.StdHeaderSectionSize = ParseFile.ParseSimpleOffset(pStream, pSectionOffset + STD_HEADER_SECTION_SIZE_OFFSET, STD_HEADER_SECTION_SIZE_LENGTH);

            Array.Reverse(this.StdHeaderSignature);
            Array.Reverse(this.StdHeaderSectionSize);
        }
    }
}
