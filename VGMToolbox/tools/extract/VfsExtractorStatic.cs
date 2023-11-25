using System;
using System.Xml.Serialization;

namespace VGMToolbox.tools.extract
{
    public partial class VfsExtractorSettings : IComparable, ISerializablePreset
    {
        public VfsExtractorSettings()
        {
            this.headerField = new Header();
            this.headerParametersField = new HeaderParameters();
            this.fileRecordParametersField = new FileRecordParameters();
            this.notesOrWarningsField = String.Empty;
        }

        public override string ToString()
        {
            string ret = null;

            if ((this.headerField != null) && (!String.IsNullOrEmpty(this.headerField.FormatName)))
            {
                ret = this.headerField.FormatName;
            }

            return ret;
        }

        public int CompareTo(object obj)
        {
            if (obj is VfsExtractorSettings)
            {
                VfsExtractorSettings o = (VfsExtractorSettings)obj;

                return this.Header.FormatName.CompareTo(o.Header.FormatName);
            }

            throw new ArgumentException("object is not a VfsExtractorSettings");
        }
    }
}
