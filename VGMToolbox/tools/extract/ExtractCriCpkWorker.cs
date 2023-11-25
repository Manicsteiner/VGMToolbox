using System;
using System.ComponentModel;
using System.IO;

using VGMToolbox.format;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.extract
{
    class ExtractCriCpkWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public const string CRI_CPK_EXTRACTION_FOLDER = "VGMT_CPK_EXTRACT_{0}";
        
        public struct ExtractCriCpkStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }
        }

        public ExtractCriCpkWorker() :
            base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pExtractCriCpkStruct, DoWorkEventArgs e)
        {
            ExtractCriCpkStruct extractCriCpkStruct = (ExtractCriCpkStruct)pExtractCriCpkStruct;
            
            using (FileStream fs = File.Open(pPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                CriCpkArchive cpk = new CriCpkArchive();
                cpk.Initialize(fs, 0, false);
                cpk.ExtractAll();
            }
        }                              
    }
}
