using System.ComponentModel;

using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.xsf
{
    class XsfCompressedProgramExtractorWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {

        public struct XsfCompressedProgramExtractorStruct : IVgmtWorkerStruct
        {            
            public bool includeExtension;
            public bool stripGsfHeader;
            public bool extractReservedSection;

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public XsfCompressedProgramExtractorWorker() : base() { }

        protected override void DoTaskForFile(string pPath,
            IVgmtWorkerStruct pXsfCompressedProgramExtractorStruct, DoWorkEventArgs e)
        {
            XsfCompressedProgramExtractorStruct xsfCompressedProgramExtractorStruct =
                (XsfCompressedProgramExtractorStruct)pXsfCompressedProgramExtractorStruct;
            
            // Extract Compressed Data
            Xsf2ExeStruct xsf2ExeStruct = new Xsf2ExeStruct();
            xsf2ExeStruct.IncludeExtension = xsfCompressedProgramExtractorStruct.includeExtension;
            xsf2ExeStruct.StripGsfHeader = xsfCompressedProgramExtractorStruct.stripGsfHeader;
            XsfUtil.ExtractCompressedDataSection(pPath, xsf2ExeStruct);

            // Extract Reserved Section
            if (xsfCompressedProgramExtractorStruct.extractReservedSection)
            {
                XsfUtil.ExtractReservedSection(pPath, xsf2ExeStruct);
            }           
        }        
    }
}
