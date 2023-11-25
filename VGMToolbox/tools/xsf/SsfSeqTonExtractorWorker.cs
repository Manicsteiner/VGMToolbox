using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class SsfSeqTonExtractorWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        private static readonly string SSFTOOL_FOLDER_PATH =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "ssf"));
        private static readonly string SEQEXT_SOURCE_PATH = Path.Combine(SSFTOOL_FOLDER_PATH, "seqext.py");
        private static readonly string TONEXT_SOURCE_PATH = Path.Combine(SSFTOOL_FOLDER_PATH, "tonext.py");
        private static readonly string WORKING_FOLDER_SEEK =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "working_ssf_seqtonext"));

        private readonly string PROGRAM_PATH =
            Path.Combine(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "psf2"), "unpkpsf2.exe");

        public struct SsfSeqTonExtractorStruct : IVgmtWorkerStruct
        {
            public bool extractToSubFolder;

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public SsfSeqTonExtractorWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pSsfSeqTonExtractorStruct, 
            DoWorkEventArgs e)
        {
            SsfSeqTonExtractorStruct ssfSeqTonExtractorStruct = (SsfSeqTonExtractorStruct)pSsfSeqTonExtractorStruct;
            
            string destinationPath;
            string outputFolder;
            string[] extractedFiles;
            string arguments;
            bool isSuccess;
            string seekOutput;
            Process extractionProcess = null;

            try
            {
                // create temp folder
                if (!Directory.Exists(WORKING_FOLDER_SEEK))
                {
                    Directory.CreateDirectory(WORKING_FOLDER_SEEK);
                }

                // copy scripts
                string seqextScriptPath = Path.Combine(WORKING_FOLDER_SEEK, Path.GetFileName(SEQEXT_SOURCE_PATH));
                string tonextScriptPath = Path.Combine(WORKING_FOLDER_SEEK, Path.GetFileName(TONEXT_SOURCE_PATH));

                File.Copy(SEQEXT_SOURCE_PATH, seqextScriptPath, true);
                File.Copy(TONEXT_SOURCE_PATH, tonextScriptPath, true);

                // copy to working dir
                destinationPath = Path.Combine(WORKING_FOLDER_SEEK, Path.GetFileName(pPath));
                File.Copy(pPath, destinationPath, true);

                // extract SEQ
                arguments = String.Format(" {0} {1} .",
                    Path.GetFileName(seqextScriptPath), Path.GetFileName(pPath));
                extractionProcess = new Process();
                extractionProcess.StartInfo = new ProcessStartInfo("python.exe", arguments);
                extractionProcess.StartInfo.WorkingDirectory = WORKING_FOLDER_SEEK;
                extractionProcess.StartInfo.UseShellExecute = false;
                extractionProcess.StartInfo.CreateNoWindow = true;
                extractionProcess.StartInfo.RedirectStandardOutput = true;
                isSuccess = extractionProcess.Start();
                seekOutput = extractionProcess.StandardOutput.ReadToEnd();
                extractionProcess.WaitForExit();
                extractionProcess.Close();

                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("[SEQEXT - {0}]", Path.GetFileName(pPath)) +
                    Environment.NewLine + seekOutput;
                this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);


                // extract TONE
                arguments = String.Format(" {0} {1} .",
                    Path.GetFileName(tonextScriptPath), Path.GetFileName(pPath));
                extractionProcess = new Process();
                extractionProcess.StartInfo = new ProcessStartInfo("python.exe", arguments);
                extractionProcess.StartInfo.WorkingDirectory = WORKING_FOLDER_SEEK;
                extractionProcess.StartInfo.UseShellExecute = false;
                extractionProcess.StartInfo.CreateNoWindow = true;
                extractionProcess.StartInfo.RedirectStandardOutput = true;
                isSuccess = extractionProcess.Start();
                seekOutput = extractionProcess.StandardOutput.ReadToEnd();
                extractionProcess.WaitForExit();

                extractionProcess.Close();
                extractionProcess.Dispose();

                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("[TONEXT - {0}]", Path.GetFileName(pPath)) +
                    Environment.NewLine + seekOutput + Environment.NewLine;
                this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                // delete the originals
                File.Delete(destinationPath);
                File.Delete(seqextScriptPath);
                File.Delete(tonextScriptPath);

                // copy any output to output folder
                if (ssfSeqTonExtractorStruct.extractToSubFolder)
                {
                    outputFolder = Path.Combine(Path.GetDirectoryName(pPath), Path.GetFileNameWithoutExtension(pPath));
                }
                else
                {
                    outputFolder = Path.GetDirectoryName(pPath);
                }
                
                extractedFiles = Directory.GetFiles(WORKING_FOLDER_SEEK);

                if (extractedFiles.Length > 0)
                {
                    if (File.Exists(outputFolder))
                    {
                        outputFolder += "_" + new Random().Next().ToString();
                    }
                    
                    if (!Directory.Exists(outputFolder))
                    {
                        Directory.CreateDirectory(outputFolder);
                    }

                    foreach (string file in extractedFiles)
                    {
                        File.Copy(file, Path.Combine(outputFolder, Path.GetFileName(file)), true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (extractionProcess != null)
                {
                    extractionProcess.Dispose();
                }

                if (Directory.Exists(WORKING_FOLDER_SEEK))
                {
                    Directory.Delete(WORKING_FOLDER_SEEK, true);
                }
            }
        }    

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            if (XsfUtil.IsPythonPresentInPath())
            {
                base.OnDoWork(e);
            }
            else
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = "ERROR: A directory containing the Python Interpreter (python.exe) cannot be found in your PATH environment variable.";
                ReportProgress(0, this.progressStruct);
            }
        }         
    
    }
}
