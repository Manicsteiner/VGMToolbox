using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VGMToolbox.format.iso
{
    public class UnsupportedDiscFormat : IVolume
    {
        public const string FORMAT_DESCRIPTION = "Unknown/Unsupported";

        public long VolumeBaseOffset { set; get; }
        public string FormatDescription { set; get; }
        public VolumeDataType VolumeType { set; get; }
        public string VolumeIdentifier { set; get; }
        public bool IsRawDump { set; get; }
        public IDirectoryStructure[] Directories { set; get; }

        public void Initialize(FileStream isoStream, long offset, bool isRawDump)
        {
            this.VolumeBaseOffset = offset;
            this.FormatDescription = FORMAT_DESCRIPTION;
            this.VolumeType = VolumeDataType.Data;
            this.VolumeIdentifier = String.Empty;
            this.IsRawDump = isRawDump;
            this.Directories = null;
        }

        public void ExtractAll(ref Dictionary<string, FileStream> streamCache, string destintionFolder, bool extractAsRaw)
        {

        }
    }
}
