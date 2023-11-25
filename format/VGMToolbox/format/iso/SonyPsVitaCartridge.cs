using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VGMToolbox.format;

namespace VGMToolbox.format.iso
{
    public class SonyPsVitaCartridge
    {
        public static readonly byte[] STANDARD_IDENTIFIER = new byte[] {0x53, 0x6F, 0x6E, 0x79, 0x20, 0x43, 0x6F, 0x6D, 
                                                                      0x70, 0x75, 0x74, 0x65, 0x72, 0x20, 0x45, 0x6E, 
                                                                      0x74, 0x65, 0x72, 0x74, 0x61, 0x69, 0x6E, 0x6D, 
                                                                      0x65, 0x6E, 0x74, 0x20, 0x49, 0x6E, 0x63, 0x2E}; // Sony Computer Entertainment Inc.
        public const uint IDENTIFIER_OFFSET = 0x00;
        public const long EXFAT_HEADER_OFFSET = 0x8000;
    }
}
