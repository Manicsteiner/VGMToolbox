using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.other
{
    class InternalNameFileRenamerWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct InternalNameFileRenamerWorkerStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }            
            public string OutputFolder { set; get; }

            public string Offset { set; get; }
            public string TerminatorBytes { set; get; }
            public string NameLength { set; get; }

            public bool MaintainOriginalExtension { set; get; }
        }

        public InternalNameFileRenamerWorker() : 
            base() 
        {
            this.progressCounterIncrementer = 10;
        }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pInternalNameFileRenamerWorkerStruct,
            DoWorkEventArgs e)
        {
            InternalNameFileRenamerWorkerStruct taskStruct = (InternalNameFileRenamerWorkerStruct)pInternalNameFileRenamerWorkerStruct;

            byte[] terminatorBytes = null;
            int nameLength = 0;

            if (taskStruct.TerminatorBytes != null)
            { 
                terminatorBytes = ByteConversion.GetBytesFromHexString(taskStruct.TerminatorBytes);
            }

            if (!String.IsNullOrEmpty(taskStruct.NameLength))
            {
                nameLength = (int)ByteConversion.GetLongValueFromString(taskStruct.NameLength);
            }

            FileUtil.RenameFileUsingInternalName(pPath, 
                ByteConversion.GetLongValueFromString(taskStruct.Offset), 
                nameLength,
                terminatorBytes, 
                taskStruct.MaintainOriginalExtension);        
        }
    }
}
