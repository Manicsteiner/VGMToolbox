using System.ComponentModel;

using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.xsf
{
    class UnpkPsf2Worker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct UnpkPsf2Struct : IVgmtWorkerStruct
        {
            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public UnpkPsf2Worker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pUnpkPsf2Struct, 
            DoWorkEventArgs e)
        {
            XsfUtil.UnpackPsf2(pPath);
        }            
    }
}
