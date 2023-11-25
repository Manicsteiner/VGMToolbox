using System;
using System.Xml.Serialization;

namespace VGMToolbox.tools.stream
{
    public partial class XmaConverterSettings : IComparable, ISerializablePreset
    {
        public XmaConverterSettings()
        { 
            this.headerField = new Header();
            this.useXmaParseField = false;
            this.xmaParseParametersField = new XmaParseParameters();
            this.addRiffHeaderField = false;
            this.riffParametersField = new RiffParameters();
            this.createPosFileField = false;
            this.posFileParametersField = new PosFileParameters();
            this.wavConversionParametersField = new WavConversionParameters();
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
            if (obj is XmaConverterSettings)
            {
                XmaConverterSettings o = (XmaConverterSettings)obj;

                return this.Header.FormatName.CompareTo(o.Header.FormatName);
            }

            throw new ArgumentException("object is not a XmaConverterSettings");
        }
    }
}
