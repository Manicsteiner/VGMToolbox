using System.ComponentModel;

using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.nds
{
    class SdatFinderWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct SdatFinderStruct : IVgmtWorkerStruct
        {
            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public SdatFinderWorker(): base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pSdatFinderStruct, 
            DoWorkEventArgs e)
        {
            string[] outputPaths = SdatUtil.ExtractSdatsFromFile(pPath, null);                           
        }        
    }
}
