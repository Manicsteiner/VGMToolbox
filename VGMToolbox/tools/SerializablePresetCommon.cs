using System;
using System.Xml.Serialization;

namespace VGMToolbox.tools
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Header
    {

        private string formatNameField;

        private string authorField;

        /// <remarks/>
        public string FormatName
        {
            get
            {
                return this.formatNameField;
            }
            set
            {
                this.formatNameField = value;
            }
        }

        /// <remarks/>
        public string Author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    public enum Endianness
    {

        /// <remarks/>
        little,

        /// <remarks/>
        big,
    }
}
