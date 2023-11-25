using System.ComponentModel;

using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.xsf
{
    class VabSplitterWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct VabSplitterStruct : IVgmtWorkerStruct
        {
            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public VabSplitterWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pUnpkPsf2Struct, 
            DoWorkEventArgs e)
        {
            XsfUtil.SplitVab(pPath, true, true);
        }     
    }
}
