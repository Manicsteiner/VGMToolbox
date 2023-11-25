using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class SsfMakeAdvancedWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct SsfMakeAdvancedStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }
        }

        public SsfMakeAdvancedWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pTaskStruct,
            DoWorkEventArgs e)
        { 
        
        }
    }
}
