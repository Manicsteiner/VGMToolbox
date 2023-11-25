using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.stream
{
    class XmashMashWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        #region CONSTANTS

        // xmash
        public const string XMASH_PATH = "external\\xma\\xmash.exe";
        public static readonly string XMASH_FULL_PATH =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            XMASH_PATH);
        public const string XMASH_OUTPUT_PATH = "xmash_out_vgmt";

        // to wav
        public const string TOWAV_PATH = "external\\xma\\ToWav.exe";
        public static readonly string TOWAV_FULL_PATH =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            TOWAV_PATH);

        // sox
        public const string SOX_PATH = "external\\sox";
        public static readonly string SOX_FOLDER = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            SOX_PATH);
        public static readonly string SOX_FULL_PATH =
            Path.Combine(SOX_FOLDER, "sox.exe");

        private const string WORKING_FOLDER_NAME = "_vgmt_xmash_mash";
        private const string XMASH_OUTPUT_EXTENSION = ".xma";
        private const string TOWAV_OUTPUT_EXTENSION = ".wav";

        private const string XMASH_ERROR_TEXT = "failure.";

        #endregion

        private ArrayList executableFilesInWorkingFolder { set; get; }

        public XmashMashWorker()
        {
            this.executableFilesInWorkingFolder = new ArrayList();
        }

        public struct XmaMashMashStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }
            public string OutputFolder { set; get; }

            public bool IgnoreXmashFailure { set; get; }

            public bool ReinterleaveMultichannel { set; get; }
            
            
            public bool ShowExeOutput { set; get; }
        }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pXmaMashMashStruct,
            DoWorkEventArgs e)
        {
            XmaMashMashStruct taskStruct = (XmaMashMashStruct)pXmaMashMashStruct;
            string workingFolder;
            string[] workingFiles;
            string workingSourceFile;

            string consoleOutput = String.Empty;
            string consoleError = String.Empty;

            bool errorFound = false;

            //------------------
            // output file name
            //------------------
            this.ShowOutput(pPath, String.Format("[{0}]", Path.GetFileName(pPath)), false);

            //----------------------
            // build working folder
            //----------------------
            workingFolder = this.createWorkingFolder(pPath, taskStruct);

            //------------------------------------
            // copy source file to working folder
            //------------------------------------
            workingSourceFile = Path.Combine(workingFolder, Path.GetFileName(pPath));
            File.Copy(pPath, workingSourceFile, true);

                       
            // set working file
            workingFiles = new string[1];
            workingFiles[0] = workingSourceFile;

            #region XMASH
            //---------------------------
            // xmash.exe
            //---------------------------            
            this.ShowOutput(pPath, "---- calling xmash.exe", false);

            // call xmash and set output as working_file for next step
            workingFiles = this.callXmash(workingFolder, workingFiles[0], taskStruct, out consoleOutput, out consoleError);

            // show output
            if (taskStruct.ShowExeOutput && !String.IsNullOrEmpty(consoleOutput))
            {
                this.ShowOutput(pPath, consoleOutput, false);
            }
            
            // check for errors
            if (!String.IsNullOrEmpty(consoleError) || consoleOutput.Contains(XMASH_ERROR_TEXT))
            {
                errorFound = true;
                
                if (!String.IsNullOrEmpty(consoleError))
                {
                    this.ShowOutput(pPath, consoleError, true);
                }

                if (taskStruct.IgnoreXmashFailure)
                {
                    errorFound = false;
                    workingFiles = new string[1] { workingSourceFile };
                }

                if (consoleOutput.Contains(XMASH_ERROR_TEXT))
                {
                    this.ShowOutput(pPath, consoleOutput, errorFound);
                }
            }
            #endregion

            #region TOWAV
            //-----------
            // ToWav.exe
            //-----------
            if (!errorFound)
            {
                this.ShowOutput(pPath, "---- calling ToWav.exe", false);

                // call ToWav.exe and set working files for next step
                workingFiles = this.callToWav(workingFolder, workingFiles, taskStruct, out consoleOutput, out consoleError);

                // show output
                if (taskStruct.ShowExeOutput && !String.IsNullOrEmpty(consoleOutput))
                {
                    this.ShowOutput(pPath, consoleOutput, false);
                }

                // dispay ToWav.exe error
                if (!String.IsNullOrEmpty(consoleError))
                {
                    this.ShowOutput(pPath, consoleError, true);
                    errorFound = true;
                }
            }            
            #endregion

            #region REINTERLEAVE OUTPUT (SOX)
            //-----------
            // sox.exe
            //-----------
            if (!errorFound && taskStruct.ReinterleaveMultichannel && (workingFiles.Length > 1))
            {
                this.ShowOutput(pPath, "---- calling sox.exe", false);

                // call ToWav.exe and set working files for next step
                workingFiles[0] = this.callSox(pPath, workingFolder, workingFiles, taskStruct, out consoleOutput, out consoleError);

                // show output
                if (taskStruct.ShowExeOutput && !String.IsNullOrEmpty(consoleOutput))
                {
                    this.ShowOutput(pPath, consoleOutput, false);
                }

                // dispay sox.exe error
                if (!String.IsNullOrEmpty(consoleError))
                {
                    this.ShowOutput(pPath, consoleError, true);
                    errorFound = true;
                }
            }
            #endregion

            //----------------------
            // clean working folder
            //----------------------
            this.cleanWorkingFolder(pPath, workingSourceFile, taskStruct);

        }

        protected override void  DoFinalTask(IVgmtWorkerStruct pTaskStruct)
        {
            string[] executablesToDelete;

            executablesToDelete = (string[])this.executableFilesInWorkingFolder.ToArray(typeof(string));

            foreach (string exe in executablesToDelete)
            {
                File.Delete(exe);
            }
        }

        //--------------------------
        // Working Folder functions
        //--------------------------
        private string getWorkingFolderPath(string processingFilePath, XmaMashMashStruct taskStruct)
        {
            string workingFolder;

            if (String.IsNullOrEmpty(taskStruct.OutputFolder))
            {
                workingFolder = Path.Combine(Path.GetDirectoryName(processingFilePath), WORKING_FOLDER_NAME);
            }
            else
            {
                workingFolder = taskStruct.OutputFolder;
            }

            return workingFolder;
        }

        private string createWorkingFolder(string processingFilePath, XmaMashMashStruct taskStruct)
        {
            string workingFolder = getWorkingFolderPath(processingFilePath, taskStruct);

            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder);
            }

            return workingFolder;
        }

        private void cleanWorkingFolder(string processingFilePath, string workingFile, XmaMashMashStruct taskStruct)
        {
            string workingFolder = getWorkingFolderPath(processingFilePath, taskStruct);
            string[] xmashXmaFiles;

            // delete working copy of source file
            if (File.Exists(workingFile))
            {
                File.Delete(workingFile);
            }

            // delete xmash output
            xmashXmaFiles = Directory.GetFiles(workingFolder, String.Format("*{0}", XMASH_OUTPUT_EXTENSION), SearchOption.TopDirectoryOnly);

            foreach (string xmaFile in xmashXmaFiles)
            {
                File.Delete(xmaFile);
            }

            //// delete xmash.exe
            //string xmashWorkingPath = Path.Combine(workingFolder, Path.GetFileName(XMASH_FULL_PATH));

            //if (File.Exists(xmashWorkingPath))
            //{
            //    File.Delete(xmashWorkingPath);
            //}

            // delete towav.exe
            //string toWavWorkingPath = Path.Combine(workingFolder, Path.GetFileName(TOWAV_FULL_PATH));

            //if (File.Exists(toWavWorkingPath))
            //{
            //    File.Delete(toWavWorkingPath);
            //}

            // delete sox.exe, etc...
            //string soxWorkingPath = Path.Combine(workingFolder, Path.GetFileName(SOX_FULL_PATH));

            //if (File.Exists(soxWorkingPath))
            //{
            //    File.Delete(soxWorkingPath);
            //}

            //dllFiles = Directory.GetFiles(workingFolder, "*.dll", SearchOption.TopDirectoryOnly);

            //foreach (string dllFile in dllFiles)
            //{
            //    File.Delete(dllFile);
            //}
        }

        //---------------
        // External Apps
        //---------------
        private string[] callXmash(
            string workingFolder,
            string workingFile,
            XmaMashMashStruct taskStruct,
            out string consoleOutput,
            out string consoleError)
        {
            string xmashWorkingPath;
            string[] xmashOutputFilePaths;
            Process xmashProcess;
            StringBuilder parameters = new StringBuilder();

            // copy to working folder
            xmashWorkingPath = Path.Combine(workingFolder, Path.GetFileName(XMASH_FULL_PATH));

            if (!File.Exists(xmashWorkingPath))
            {
                File.Copy(XMASH_FULL_PATH, xmashWorkingPath, false);
                this.executableFilesInWorkingFolder.Add(xmashWorkingPath);
            }

            // build parameters
            parameters.AppendFormat(" \"{0}\"", Path.GetFileName(workingFile)); // Filename

            // call function
            xmashProcess = new Process();
            xmashProcess.StartInfo = new ProcessStartInfo(xmashWorkingPath);
            xmashProcess.StartInfo.WorkingDirectory = workingFolder;
            xmashProcess.StartInfo.Arguments = parameters.ToString();
            xmashProcess.StartInfo.UseShellExecute = false;
            xmashProcess.StartInfo.CreateNoWindow = true;

            xmashProcess.StartInfo.RedirectStandardError = true;
            xmashProcess.StartInfo.RedirectStandardOutput = true;

            bool isSuccess = xmashProcess.Start();
            consoleOutput = xmashProcess.StandardOutput.ReadToEnd();
            consoleError = xmashProcess.StandardError.ReadToEnd();

            xmashProcess.WaitForExit();
            xmashProcess.Close();
            xmashProcess.Dispose();

            // get list of output xma files
            xmashOutputFilePaths = Directory.GetFiles(workingFolder, String.Format("{0}_*{1}", Path.GetFileName(workingFile), XMASH_OUTPUT_EXTENSION), SearchOption.TopDirectoryOnly);

            return xmashOutputFilePaths;
        }

        
        private string[] callToWav(
            string workingFolder,
            string[] workingFiles,
            XmaMashMashStruct taskStruct,
            out string consoleOutput,
            out string consoleError)
        {
            string toWavWorkingPath;
            ArrayList toWavOutputFiles = new ArrayList();
            string[] toWavOutputFilePaths;
            string testFileName;

            Process toWavProcess;
            StringBuilder parameters = new StringBuilder();

            consoleOutput = String.Empty;
            consoleError = String.Empty;

            // copy to working folder
            toWavWorkingPath = Path.Combine(workingFolder, Path.GetFileName(TOWAV_FULL_PATH));

            if (!File.Exists(toWavWorkingPath))
            {
                File.Copy(TOWAV_FULL_PATH, toWavWorkingPath, true);
                this.executableFilesInWorkingFolder.Add(toWavWorkingPath);
            }

            foreach (string workingFile in workingFiles)
            {
                // build parameters            
                parameters.AppendFormat(" \"{0}\"", Path.GetFileName(workingFile)); // Filename

                // call function
                toWavProcess = new Process();
                toWavProcess.StartInfo = new ProcessStartInfo(toWavWorkingPath);
                toWavProcess.StartInfo.WorkingDirectory = workingFolder;
                toWavProcess.StartInfo.Arguments = parameters.ToString();
                toWavProcess.StartInfo.UseShellExecute = false;
                toWavProcess.StartInfo.CreateNoWindow = true;

                toWavProcess.StartInfo.RedirectStandardError = true;
                toWavProcess.StartInfo.RedirectStandardOutput = true;

                bool isSuccess = toWavProcess.Start();
                consoleOutput += toWavProcess.StandardOutput.ReadToEnd();
                consoleError += toWavProcess.StandardError.ReadToEnd();

                toWavProcess.WaitForExit();
                toWavProcess.Close();
                toWavProcess.Dispose();

                // update output file list
                testFileName = Path.ChangeExtension(workingFile, TOWAV_OUTPUT_EXTENSION);

                if (File.Exists(testFileName))
                {
                    // one file in, one file out
                    toWavOutputFiles.Add(testFileName);
                }
                else if (taskStruct.IgnoreXmashFailure)
                { 
                    // multichannel in, mutliple files out (but not parsable by XMASH)                
                    testFileName = Path.Combine(Path.GetDirectoryName(workingFile),
                        String.Format("{0} Fl Fr{1}", Path.GetFileNameWithoutExtension(workingFile), TOWAV_OUTPUT_EXTENSION));

                    if (File.Exists(testFileName))
                    {
                        // one file in, one file out
                        toWavOutputFiles.Add(testFileName);
                    }

                    testFileName = Path.Combine(Path.GetDirectoryName(workingFile),
                        String.Format("{0} C LFE{1}", Path.GetFileNameWithoutExtension(workingFile), TOWAV_OUTPUT_EXTENSION));

                    if (File.Exists(testFileName))
                    {
                        // one file in, one file out
                        toWavOutputFiles.Add(testFileName);
                    }

                    testFileName = Path.Combine(Path.GetDirectoryName(workingFile),
                        String.Format("{0} Sl Sr{1}", Path.GetFileNameWithoutExtension(workingFile), TOWAV_OUTPUT_EXTENSION));

                    if (File.Exists(testFileName))
                    {
                        // one file in, one file out
                        toWavOutputFiles.Add(testFileName);
                    }

                }
            }

            // build output path
            toWavOutputFilePaths = (string[])toWavOutputFiles.ToArray(typeof(string));

            return toWavOutputFilePaths;
        }

        private string callSox(
            string baseFilePath,
            string workingFolder,
            string[] workingFiles,
            XmaMashMashStruct taskStruct,
            out string consoleOutput,
            out string consoleError)
        {
            string soxWorkingPath;
            string mergedOutputFileName;
            string[] soxFiles;
            Process soxProcess;
            StringBuilder parameters = new StringBuilder();

            consoleOutput = String.Empty;
            consoleError = String.Empty;

            // loop through required support files
            soxFiles = Directory.GetFiles(SOX_FOLDER);

            foreach (string sFile in soxFiles)
            {
                if (!sFile.Equals(SOX_FULL_PATH))
                {
                    // build path to working folder
                    soxWorkingPath = Path.Combine(workingFolder, Path.GetFileName(sFile));

                    // copy to working folder
                    if (!File.Exists(soxWorkingPath))
                    {
                        File.Copy(sFile, soxWorkingPath, false);
                        this.executableFilesInWorkingFolder.Add(soxWorkingPath);
                    }                
                }
            }

            // copy sox to working folder
            soxWorkingPath = Path.Combine(workingFolder, Path.GetFileName(SOX_FULL_PATH));

            if (!File.Exists(soxWorkingPath))
            {
                File.Copy(SOX_FULL_PATH, soxWorkingPath, true);
                this.executableFilesInWorkingFolder.Add(soxWorkingPath);
            }

            // build parameters
            parameters.AppendFormat(" -M");

            foreach (string workingFile in workingFiles)
            {
                // build parameters            
                parameters.AppendFormat(" \"{0}\"", Path.GetFileName(workingFile)); // Filename
            }

            mergedOutputFileName = String.Format("{0}.merged.wav", Path.GetFileName(baseFilePath));
            parameters.AppendFormat(" \"{0}\"", mergedOutputFileName);

            // call function
            soxProcess = new Process();
            soxProcess.StartInfo = new ProcessStartInfo(soxWorkingPath);
            soxProcess.StartInfo.WorkingDirectory = workingFolder;
            soxProcess.StartInfo.Arguments = parameters.ToString();
            soxProcess.StartInfo.UseShellExecute = false;
            soxProcess.StartInfo.CreateNoWindow = true;

            soxProcess.StartInfo.RedirectStandardError = true;
            soxProcess.StartInfo.RedirectStandardOutput = true;

            bool isSuccess = soxProcess.Start();
            consoleOutput += soxProcess.StandardOutput.ReadToEnd();
            consoleError += soxProcess.StandardError.ReadToEnd();

            soxProcess.WaitForExit();
            soxProcess.Close();
            soxProcess.Dispose();
            
            // build output path
            mergedOutputFileName = Path.Combine(workingFolder, mergedOutputFileName);

            return mergedOutputFileName;
        }

    }
}
