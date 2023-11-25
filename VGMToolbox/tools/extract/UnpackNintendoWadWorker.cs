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
    public class UnpackNintendoWadWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct WadUnpackerStruct : IVgmtWorkerStruct
        {
            public bool UnpackExtractedU8Files { set; get; }
            public bool ExtractAllFiles { set; get; }

            public string[] SourcePaths {set;get;}
        }

        public UnpackNintendoWadWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pTaskStruct,
            DoWorkEventArgs e)
        {
            WadUnpackerStruct ws = (WadUnpackerStruct)pTaskStruct;

            NintendoWad wad;
            NintendoU8Archive u8;
            string[] extractedFiles;

            if (NintendoWad.IsWadFile(pPath))
            {
                //-------------
                // Extract WAD
                //-------------
                wad = new NintendoWad(pPath);

                if (ws.ExtractAllFiles)
                {
                    extractedFiles = wad.ExtractAll();
                }
                else
                {
                    extractedFiles = wad.ExtractContent();
                }

                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("[{0}]{1}    WAD Unpacked.{1}", Path.GetFileName(pPath), Environment.NewLine);
                ReportProgress(this.progress, this.progressStruct);            

                //---------------------
                // Extract U8 Archives
                //---------------------
                if (ws.UnpackExtractedU8Files)
                {
                    foreach (string f in extractedFiles)
                    {
                        if (NintendoU8Archive.IsU8File(f))
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.GenericMessage = String.Format("    Extracting U8: {0}{1}", Path.GetFileName(f), Environment.NewLine);
                            ReportProgress(this.progress, this.progressStruct);            
                            
                            u8 = new NintendoU8Archive(f);
                            u8.ExtractAll();
                        }
                    }
                }
            }
        }   
    }
}
