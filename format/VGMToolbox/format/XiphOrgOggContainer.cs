using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VGMToolbox.format
{
    public class XiphOrgOggContainer
    {
        public static readonly byte[] MAGIC_BYTES = new byte[] { 0x4F, 0x67, 0x67, 0x53 };

        public const byte PAGE_TYPE_CONTINUATION = 1;
        public const byte PAGE_TYPE_BEGIN_STREAM = 2;
        public const byte PAGE_TYPE_END_STREAM = 4;

        public XiphOrgOggContainer() { }
    
    }
}
