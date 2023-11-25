using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.extract
{
    public class ExtractNintendoU8ArchiveWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct U8ArchiveUnpackerStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }
        }

        public ExtractNintendoU8ArchiveWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pTaskStruct,
            DoWorkEventArgs e)
        {
            U8ArchiveUnpackerStruct ws = (U8ArchiveUnpackerStruct)pTaskStruct;
            NintendoU8Archive u8;

            if (NintendoU8Archive.IsU8File(pPath))
            {
                u8 = new NintendoU8Archive(pPath);
                u8.ExtractAll();

                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("[{0}]{1}", Path.GetFileName(pPath), Environment.NewLine);
                ReportProgress(this.progress, this.progressStruct);
            }
            else
            {
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("[{0}] is not a U8 File.{1}", Path.GetFileName(pPath), Environment.NewLine);
                ReportProgress(this.progress, this.progressStruct);            
            }
        }       
    }
}
