using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class XsfRecompressDataWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct XsfRecompressDataStruct : IVgmtWorkerStruct
        {
            public int CompressionLevel;
            public bool RecompressFolders;

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public XsfRecompressDataWorker() : base() { }

        protected override void DoTaskForDirectory(string pPath, IVgmtWorkerStruct pTaskStruct,
            DoWorkEventArgs e)
        {
            XsfRecompressDataStruct xsfRecompressDataStruct = (XsfRecompressDataStruct) pTaskStruct;
            
            foreach (string d in Directory.GetDirectories(pPath))
            {
                if (!CancellationPending)
                {
                    this.DoTaskForDirectory(d, pTaskStruct, e);
                }
                else
                {
                    e.Cancel = true;
                    break;
                }
            }
            foreach (string f in Directory.GetFiles(pPath))
            {
                if (!CancellationPending)
                {
                    // Report Progress
                    base.progress = (++base.fileCount * 100) / base.maxFiles;
                    base.progressStruct.Clear();
                    base.progressStruct.FileName = f;
                    ReportProgress(base.progress, base.progressStruct);

                    // perform task
                    try
                    {
                        this.DoTaskForFile(f, pTaskStruct, e);
                    }
                    catch (Exception ex)
                    {
                        base.progressStruct.Clear();
                        base.progressStruct.ErrorMessage =
                            String.Format(CultureInfo.CurrentCulture, "Error processing <{0}>.  Error received: ", f) + ex.Message + Environment.NewLine;
                        ReportProgress(base.progress, base.progressStruct);
                    }
                    finally
                    {
                        this.DoFinally();
                    }
                }
                else
                {
                    e.Cancel = true;
                    break;
                }
            }

            // recompress folder if requested
            if (xsfRecompressDataStruct.RecompressFolders &&
                (((IList<string>)xsfRecompressDataStruct.SourcePaths)).Contains(pPath))
            {                
                string recompressedFolder = Path.Combine(pPath, XsfUtil.RecompressedSubfolderName);

                if (Directory.Exists(recompressedFolder))
                {
                    string[] sourceFiles = Directory.GetFiles(pPath, "*.*", SearchOption.TopDirectoryOnly);
                    string recompressedFolderFileName;

                    foreach (string f in sourceFiles)
                    {
                        recompressedFolderFileName = Path.Combine(recompressedFolder, Path.GetFileName(f));

                        if (!File.Exists(recompressedFolderFileName))
                        {
                            File.Copy(f, recompressedFolderFileName);
                        }
                    }

                    string destinationArchive = Path.Combine(pPath, String.Format("{0}.{1}", Path.GetFileName(pPath), "7z"));

                    if (File.Exists(destinationArchive))
                    {
                        destinationArchive = Path.Combine(pPath, String.Format("{0}_{1}.{2}", Path.GetFileName(pPath), new Random().Next(0xFF).ToString("X2"), "7z"));
                    }
                    
                    CompressionUtil.CompressFolderWith7zip(recompressedFolder, destinationArchive);
                }
            }
        }
        
        protected override void DoTaskForFile(string pPath,
            IVgmtWorkerStruct pXsfRecompressDataStruct, DoWorkEventArgs e)
        {
            XsfRecompressDataStruct xsfRecompressDataStruct =
                (XsfRecompressDataStruct)pXsfRecompressDataStruct;

            XsfRecompressStruct xsfRecompressStruct = new XsfRecompressStruct();
            xsfRecompressStruct.CompressionLevel = xsfRecompressDataStruct.CompressionLevel;

            string outputPath = XsfUtil.ReCompressDataSection(pPath, xsfRecompressStruct);
        }    
    }
}
