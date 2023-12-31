﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3603
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Xml.Serialization;

namespace VGMToolbox.tools.extract
{
    // 
    // This source code was auto-generated by xsd, Version=2.0.50727.3038.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class OffsetFinderTemplate : IComparable, ISerializablePreset
    {

        private Header headerField;

        private SearchParameters searchParametersField;

        private string notesOrWarningsField;

        /// <remarks/>
        public Header Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public SearchParameters SearchParameters
        {
            get
            {
                return this.searchParametersField;
            }
            set
            {
                this.searchParametersField = value;
            }
        }

        /// <remarks/>
        public string NotesOrWarnings
        {
            get
            {
                return this.notesOrWarningsField;
            }
            set
            {
                this.notesOrWarningsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class cutParameters
    {

        private CutStyle cutStyleField;

        private string staticCutSizeField;

        private string cutSizeAtOffsetField;

        private string cutSizeOffsetSizeField;

        private Endianness cutSizeOffsetEndianessField;

        private bool cutSizeOffsetEndianessFieldSpecified;

        private string cutSizeMultiplierField;

        private bool useModOffsetForTerminatorStringField;

        private bool useModOffsetForTerminatorStringFieldSpecified;

        private string modOffsetForTerminatorStringDivisorField;

        private string modOffsetForTerminatorStringResultField;

        private string terminatorStringField;

        private bool treatTerminatorStringAsHexField;

        private bool treatTerminatorStringAsHexFieldSpecified;

        private bool includeTerminatorInSizeField;

        private bool includeTerminatorInSizeFieldSpecified;

        /// <remarks/>
        public CutStyle CutStyle
        {
            get
            {
                return this.cutStyleField;
            }
            set
            {
                this.cutStyleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string StaticCutSize
        {
            get
            {
                return this.staticCutSizeField;
            }
            set
            {
                this.staticCutSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string CutSizeAtOffset
        {
            get
            {
                return this.cutSizeAtOffsetField;
            }
            set
            {
                this.cutSizeAtOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string CutSizeOffsetSize
        {
            get
            {
                return this.cutSizeOffsetSizeField;
            }
            set
            {
                this.cutSizeOffsetSizeField = value;
            }
        }

        /// <remarks/>
        public Endianness CutSizeOffsetEndianess
        {
            get
            {
                return this.cutSizeOffsetEndianessField;
            }
            set
            {
                this.cutSizeOffsetEndianessField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CutSizeOffsetEndianessSpecified
        {
            get
            {
                return this.cutSizeOffsetEndianessFieldSpecified;
            }
            set
            {
                this.cutSizeOffsetEndianessFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string CutSizeMultiplier
        {
            get
            {
                return this.cutSizeMultiplierField;
            }
            set
            {
                this.cutSizeMultiplierField = value;
            }
        }

        /// <remarks/>
        public bool UseModOffsetForTerminatorString
        {
            get
            {
                return this.useModOffsetForTerminatorStringField;
            }
            set
            {
                this.useModOffsetForTerminatorStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UseModOffsetForTerminatorStringSpecified
        {
            get
            {
                return this.useModOffsetForTerminatorStringFieldSpecified;
            }
            set
            {
                this.useModOffsetForTerminatorStringFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string ModOffsetForTerminatorStringDivisor
        {
            get
            {
                return this.modOffsetForTerminatorStringDivisorField;
            }
            set
            {
                this.modOffsetForTerminatorStringDivisorField = value;
            }
        }

        /// <remarks/>
        public string ModOffsetForTerminatorStringResult
        {
            get
            {
                return this.modOffsetForTerminatorStringResultField;
            }
            set
            {
                this.modOffsetForTerminatorStringResultField = value;
            }
        }

        /// <remarks/>
        public string TerminatorString
        {
            get
            {
                return this.terminatorStringField;
            }
            set
            {
                this.terminatorStringField = value;
            }
        }

        /// <remarks/>
        public bool TreatTerminatorStringAsHex
        {
            get
            {
                return this.treatTerminatorStringAsHexField;
            }
            set
            {
                this.treatTerminatorStringAsHexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TreatTerminatorStringAsHexSpecified
        {
            get
            {
                return this.treatTerminatorStringAsHexFieldSpecified;
            }
            set
            {
                this.treatTerminatorStringAsHexFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IncludeTerminatorInSize
        {
            get
            {
                return this.includeTerminatorInSizeField;
            }
            set
            {
                this.includeTerminatorInSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludeTerminatorInSizeSpecified
        {
            get
            {
                return this.includeTerminatorInSizeFieldSpecified;
            }
            set
            {
                this.includeTerminatorInSizeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    public enum CutStyle
    {

        /// <remarks/>
        @static,

        /// <remarks/>
        offset,

        /// <remarks/>
        terminator,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SearchParameters
    {

        private string searchStringField;

        private bool treatSearchStringAsHexField;

        private string startingOffsetField;

        private bool useModOffsetForSearchStringField;

        private bool useModOffsetForSearchStringFieldSpecified;

        private string modOffsetForSearchStringDivisorField;

        private string modOffsetForSearchStringResultField;

        private string searchStringOffsetField;

        private string outputFileExtensionField;

        private string minimumSizeForCuttingField;

        private cutParameters cutParametersField;

        private bool addExtraBytesField;

        private string addExtraByteSizeField;

        /// <remarks/>
        public string SearchString
        {
            get
            {
                return this.searchStringField;
            }
            set
            {
                this.searchStringField = value;
            }
        }

        /// <remarks/>
        public bool TreatSearchStringAsHex
        {
            get
            {
                return this.treatSearchStringAsHexField;
            }
            set
            {
                this.treatSearchStringAsHexField = value;
            }
        }

        /// <remarks/>
        public string StartingOffset
        {
            get
            {
                return this.startingOffsetField;
            }
            set
            {
                this.startingOffsetField = value;
            }
        }

        /// <remarks/>
        public bool UseModOffsetForSearchString
        {
            get
            {
                return this.useModOffsetForSearchStringField;
            }
            set
            {
                this.useModOffsetForSearchStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UseModOffsetForSearchStringSpecified
        {
            get
            {
                return this.useModOffsetForSearchStringFieldSpecified;
            }
            set
            {
                this.useModOffsetForSearchStringFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string ModOffsetForSearchStringDivisor
        {
            get
            {
                return this.modOffsetForSearchStringDivisorField;
            }
            set
            {
                this.modOffsetForSearchStringDivisorField = value;
            }
        }

        /// <remarks/>
        public string ModOffsetForSearchStringResult
        {
            get
            {
                return this.modOffsetForSearchStringResultField;
            }
            set
            {
                this.modOffsetForSearchStringResultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string SearchStringOffset
        {
            get
            {
                return this.searchStringOffsetField;
            }
            set
            {
                this.searchStringOffsetField = value;
            }
        }

        /// <remarks/>
        public string OutputFileExtension
        {
            get
            {
                return this.outputFileExtensionField;
            }
            set
            {
                this.outputFileExtensionField = value;
            }
        }

        /// <remarks/>
        public string MinimumSizeForCutting
        {
            get
            {
                return this.minimumSizeForCuttingField;
            }
            set
            {
                this.minimumSizeForCuttingField = value;
            }
        }

        /// <remarks/>
        public cutParameters CutParameters
        {
            get
            {
                return this.cutParametersField;
            }
            set
            {
                this.cutParametersField = value;
            }
        }

        /// <remarks/>
        public bool AddExtraBytes
        {
            get
            {
                return this.addExtraBytesField;
            }
            set
            {
                this.addExtraBytesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string AddExtraByteSize
        {
            get
            {
                return this.addExtraByteSizeField;
            }
            set
            {
                this.addExtraByteSizeField = value;
            }
        }
    }
}