using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.examine
{
    class ExamineSearchForFileWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {

        public struct ExamineSearchForFileStruct : IVgmtWorkerStruct
        {
            public bool ExtractFile;
            public bool CaseSensitive;
            public string[] SearchStrings;
            public string OutputFolder;

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public ExamineSearchForFileWorker() : base() { }

        protected override void DoTaskForFile(string pPath,
            IVgmtWorkerStruct pExamineSearchForFileStruct, DoWorkEventArgs e)
        {
            ExamineSearchForFileStruct examineSearchForFileStruct =
                (ExamineSearchForFileStruct)pExamineSearchForFileStruct;

            StringBuilder filePaths = new StringBuilder();
            StringBuilder searchStringHeader = new StringBuilder();
            string[] compressedFilePaths;
            string searchString;

            foreach (string s in examineSearchForFileStruct.SearchStrings)
            {                                              
                if (examineSearchForFileStruct.CaseSensitive)
                {
                    searchString = s;
                }
                else
                {
                    searchString = s.ToUpper();
                }

                if ((examineSearchForFileStruct.CaseSensitive &&
                    Path.GetFileName(pPath).Contains(searchString)) ||
                    (!examineSearchForFileStruct.CaseSensitive &&
                    Path.GetFileName(pPath).ToUpper().Contains(searchString)))
                {
                    filePaths.AppendFormat("---= Search String: [{0}] =---{1}", s, Environment.NewLine);
                    filePaths.Append(pPath);
                    filePaths.Append(Environment.NewLine);

                    // copy to destination folder
                    File.Copy(pPath, Path.Combine(examineSearchForFileStruct.OutputFolder, Path.GetFileName(pPath)));
                }
                else // check to see if this is an archive
                {
                    compressedFilePaths = CompressionUtil.GetFileList(pPath);

                    // Extract if inside a compressed file
                    if (compressedFilePaths != null)
                    {
                        foreach (string f in compressedFilePaths)
                        {
                            if ((examineSearchForFileStruct.CaseSensitive &&
                                f.Contains(searchString)) ||
                                (!examineSearchForFileStruct.CaseSensitive &&
                                f.ToUpper().Contains(searchString)))
                            {
                                filePaths.AppendFormat("---= Search String: [{0}] =---{1}", s, Environment.NewLine);
                                filePaths.AppendFormat("{0} ({1})", pPath, f);
                                filePaths.Append(Environment.NewLine);

                                if (examineSearchForFileStruct.ExtractFile)
                                {
                                    CompressionUtil.ExtractFileFromArchive(pPath, f, examineSearchForFileStruct.OutputFolder);
                                }
                            }
                        }
                    }
                }

                this.progressStruct.Clear();
                progressStruct.GenericMessage = filePaths.ToString();
                ReportProgress(this.Progress, progressStruct);
            }
        }
    }        
}
