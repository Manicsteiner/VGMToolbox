using System.ComponentModel;

using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.nds
{
    class SdatExtractorWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct SdatExtractorStruct : IVgmtWorkerStruct
        {
            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public SdatExtractorWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pSdatExtractorStruct, 
            DoWorkEventArgs e)
        {        
            string outputDir = SdatUtil.ExtractSdat(pPath);           
        }        
    }
}
