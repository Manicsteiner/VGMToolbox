using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class SsfMakeWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        private static readonly string SSFTOOL_FOLDER_PATH =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "ssf"));
        private static readonly string SEQEXT_SOURCE_PATH = Path.Combine(SSFTOOL_FOLDER_PATH, "seqext.py");
        private static readonly string TONEXT_SOURCE_PATH = Path.Combine(SSFTOOL_FOLDER_PATH, "tonext.py");
        private static readonly string SSFTIME_SOURCE_PATH = Path.Combine(SSFTOOL_FOLDER_PATH, "ssftime.py");

        private static readonly string SSFMAKE_FOLDER_PATH =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "ssf"), "ssfmake"));
        private static readonly string SSFMAKE_SOURCE_PATH = Path.Combine(SSFMAKE_FOLDER_PATH, "ssfmake.py");

        private static readonly string SSFINFO_FOLDER_PATH =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "ssf"), "ssfinfo"));
        private static readonly string SSFINFO_SOURCE_PATH = Path.Combine(SSFINFO_FOLDER_PATH, "ssfinfo.py");

        private static readonly string WORKING_FOLDER =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "working_ssf"));
        private static readonly string WORKING_FOLDER_SEEK =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "working_ssf_seek"));
        private static readonly string SSFMAKE_DESTINATION_PATH =
            Path.Combine(WORKING_FOLDER, "ssfmake.py");
        private static readonly string SSFINFO_DESTINATION_PATH =
            Path.Combine(WORKING_FOLDER, "ssfinfo.py");
        private static readonly string BIN2PSF_SOURCE_PATH =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "bin2psf.exe"));
        private static readonly string BIN2PSF_DESTINATION_PATH =
            Path.GetFullPath(Path.Combine(WORKING_FOLDER, "bin2psf.exe"));        
        private static readonly string PSFPOINT_SOURCE_PATH =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "psfpoint.exe"));
        private static readonly string PSFPOINT_DESTINATION_PATH =
            Path.GetFullPath(Path.Combine(WORKING_FOLDER, "psfpoint.exe"));

        private readonly string OUTPUT_FOLDER =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "rips"), "ssfs"));

        private const int LINE_NUM_SEQ_BANK = 130;
        private const int LINE_NUM_SEQ_TRACK = 131;
        private const int LINE_NUM_VOLUME = 132;
        private const int LINE_NUM_MIXER_BANK = 133;
        private const int LINE_NUM_MIXER_NUMBER = 134;
        private const int LINE_NUM_EFFECT = 135;
        private const int LINE_NUM_USE_DSP = 136;
        
        private const int LINE_NUM_DRIVER = 139;
        private const int LINE_NUM_MAP = 140;
        private const int LINE_NUM_TONE_DATA = 141;
        private const int LINE_NUM_SEQ_DATA = 142;
        private const int LINE_NUM_DSP_PROGRAM = 143;

        private const int LINE_NUM_OUT_FILE = 145;

        private const string FILE_EXTENSION_SEQUENCE = ".SEQ";
        private const string FILE_EXTENSION_TONE = ".BIN";
        private const string FILE_EXTENSION_DSP = ".EXB";

        private int fileCount;
        private int maxFiles;
        VGMToolbox.util.ProgressStruct progressStruct = new VGMToolbox.util.ProgressStruct();

        public struct SsfMakeStruct
        {            
            public string sequenceBank;
            public string sequenceTrack;
            public string volume;
            public string mixerBank;
            public string mixerNumber;
            public string effectNumber;
            public string useDsp;
            
            public string driver;
            public string map;

            public string sourcePath;
            public string outputFolder;
            public string dspFile;
            public string customMapFile;
            
            public bool findData;
            public bool TryAllCombinations { set; get; }
        }

        public SsfMakeWorker()
        {
            fileCount = 0;
            maxFiles = 0;
            this.progressStruct = new VGMToolbox.util.ProgressStruct();

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }
 
        private void makeSsfs(SsfMakeStruct pSsfMakeStruct, DoWorkEventArgs e)
        {
            string[] uniqueSeqFiles;
            string[] uniqueToneFiles;

            if (!CancellationPending)
            {
                if (pSsfMakeStruct.findData)
                {
                    // extract the data using seqext.py and tonext.py
                    this.extractData(pSsfMakeStruct, e);

                    // grab DSP data from source dir
                    string exbFileName;
                    string[] exbFiles = Directory.GetFiles(pSsfMakeStruct.sourcePath, "*.EXB");
                    
                    foreach (string f in exbFiles)
                    { 
                        exbFileName = Path.GetFileName(f);
                        File.Copy(f, Path.Combine(WORKING_FOLDER_SEEK, exbFileName), true);
                    }

                    // set source path to folder with our extracted data
                    pSsfMakeStruct.sourcePath = WORKING_FOLDER_SEEK;
                }


                // get list of unique files
                uniqueSeqFiles = this.getUniqueFileNames(pSsfMakeStruct.sourcePath, "*.SEQ");
                uniqueToneFiles = this.getUniqueFileNames(pSsfMakeStruct.sourcePath, "*.BIN");
                
                if (uniqueSeqFiles.Length > 0)
                {
                    if (pSsfMakeStruct.TryAllCombinations && (uniqueToneFiles.Length > 0))
                    {
                        this.maxFiles = uniqueSeqFiles.Length * uniqueToneFiles.Length;
                    }
                    else
                    {
                        this.maxFiles = uniqueSeqFiles.Length;
                    }

                    this.buildSsfs(uniqueSeqFiles, uniqueToneFiles, pSsfMakeStruct, e);
                    this.timeSsfs(Path.Combine(OUTPUT_FOLDER, pSsfMakeStruct.outputFolder));
                }

                if (Directory.Exists(WORKING_FOLDER_SEEK))
                {
                    Directory.Delete(WORKING_FOLDER_SEEK, true);
                }
            }
            else
            {
                e.Cancel = true;
                return;
            }

            return;
        }

        private string[] getUniqueFileNames(string pSourceDirectory, string mask)
        {
            int fileCount = 0;
            int i = 0;
            string[] ret = new string[0];

            if (!Directory.Exists(pSourceDirectory))
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = String.Format("ERROR: Directory {0} not found.", pSourceDirectory);
                ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
            }
            else
            {
                fileCount = Directory.GetFiles(pSourceDirectory, mask, SearchOption.TopDirectoryOnly).Length;

                if (fileCount > 0)
                {
                    ret = new string[fileCount];
                }

                foreach (string f in Directory.GetFiles(pSourceDirectory, mask))
                {
                    ret[i] = f;
                    i++;
                }
            }

            return ret;
        }

        private void buildSsfs(string[] pUniqueSqFiles, string[] pUniqueToneFiles, 
            SsfMakeStruct pSsfMakeStruct, DoWorkEventArgs e)
        {
            int progress = 0;
            StringBuilder ssfmakeArguments = new StringBuilder();

            // string filePrefixPath;
            string filePrefix;
            
            string anticipatedTonePath;
            string anticipatedDspPath;

            string potentialFilePath;
            string potentialFileName;

            string toneFileName;

            int totalTracks;

            string ripOutputFolder = Path.Combine(OUTPUT_FOLDER, pSsfMakeStruct.outputFolder);

            // check for more seq than tones/dsp and try to match based on file name (Riglordsaga)
            if (pSsfMakeStruct.findData)
            {
                foreach (string f in pUniqueSqFiles)
                {
                    anticipatedTonePath = Path.ChangeExtension(f, FILE_EXTENSION_TONE);
                    anticipatedDspPath = Path.ChangeExtension(f, FILE_EXTENSION_DSP);

                    if (!File.Exists(anticipatedTonePath))
                    {
                        filePrefix = Path.GetFileNameWithoutExtension(f);
                        potentialFileName = filePrefix.Substring(0, filePrefix.IndexOf('_') + 1) + "000" + FILE_EXTENSION_TONE; // TRY XXXX_000.BIN
                        potentialFilePath = Path.Combine(Path.GetDirectoryName(f), potentialFileName);

                        if (File.Exists(potentialFilePath))
                        {
                            File.Copy(potentialFilePath, anticipatedTonePath);
                        }
                    }

                    if (!File.Exists(anticipatedDspPath))
                    {
                        filePrefix = Path.GetFileNameWithoutExtension(f);
                        potentialFileName = filePrefix.Substring(0, filePrefix.IndexOf('_')) + FILE_EXTENSION_DSP; // TRY XXXX.EXB
                        potentialFilePath = Path.Combine(Path.GetDirectoryName(f), potentialFileName);

                        if (File.Exists(potentialFilePath))
                        {
                            File.Copy(potentialFilePath, anticipatedDspPath);
                        }
                    }
                }
            } // if (pSsfMakeStruct.findData)

            foreach (string seqFile in pUniqueSqFiles)
            {
                if (!CancellationPending)
                {
                    if (pSsfMakeStruct.TryAllCombinations)
                    {
                        foreach (string toneFile in pUniqueToneFiles)
                        {
                            try
                            {
                                // report progress
                                progress = (++this.fileCount * 100) / maxFiles;
                                this.progressStruct.Clear();
                                this.progressStruct.FileName = seqFile;
                                ReportProgress(progress, this.progressStruct);

                                toneFileName = toneFile;

                                if (!String.IsNullOrEmpty(pSsfMakeStruct.dspFile) ||
                                    File.Exists(Path.ChangeExtension(seqFile, FILE_EXTENSION_DSP)))
                                {
                                    pSsfMakeStruct.useDsp = "1";
                                }

                                if (String.IsNullOrEmpty(pSsfMakeStruct.customMapFile))
                                {
                                    pSsfMakeStruct.map = getMapFile(pSsfMakeStruct, seqFile, toneFileName);
                                }
                                else
                                {
                                    pSsfMakeStruct.map = Path.GetFileName(pSsfMakeStruct.customMapFile);
                                }
                                
                                if (!String.IsNullOrEmpty(pSsfMakeStruct.map))
                                {
                                    totalTracks = this.getNumberOfTracks(seqFile);

                                    if (this.prepareWorkingDir(pSsfMakeStruct, seqFile, toneFileName))
                                    {
                                        this.customizeScript(pSsfMakeStruct, seqFile, toneFileName, totalTracks);
                                        this.executeScript();
                                        this.moveSsfToRipFolder(ripOutputFolder, totalTracks);
                                    }

                                    Directory.Delete(WORKING_FOLDER, true);
                                }
                                else
                                {
                                    this.progressStruct.Clear();
                                    this.progressStruct.FileName = seqFile;
                                    this.progressStruct.ErrorMessage = "[ERROR - NO SUITABLE MAP FILE FOUND]: " + seqFile + Environment.NewLine;
                                    ReportProgress(progress, this.progressStruct);
                                }
                            }
                            catch (Exception ex2)
                            {
                                this.progressStruct.Clear();
                                this.progressStruct.FileName = seqFile;
                                this.progressStruct.ErrorMessage = ex2.Message;
                                ReportProgress(progress, this.progressStruct);
                            }
                        }

                    }
                    else
                    {
                        try
                        {
                            // report progress
                            progress = (++this.fileCount * 100) / maxFiles;
                            this.progressStruct.Clear();
                            this.progressStruct.FileName = seqFile;
                            ReportProgress(progress, this.progressStruct);

                            // filePrefix = Path.GetFileNameWithoutExtension(seqFile);
                            // filePrefixPath = Path.Combine(Path.GetDirectoryName(seqFile), filePrefix);
                            toneFileName = Path.ChangeExtension(seqFile, FILE_EXTENSION_TONE);

                            if (!String.IsNullOrEmpty(pSsfMakeStruct.dspFile) ||
                                File.Exists(Path.ChangeExtension(seqFile, FILE_EXTENSION_DSP)))
                            {
                                pSsfMakeStruct.useDsp = "1";
                            }

                            if (String.IsNullOrEmpty(pSsfMakeStruct.customMapFile))
                            {
                                pSsfMakeStruct.map = getMapFile(pSsfMakeStruct, seqFile, toneFileName);
                            }
                            else
                            {
                                pSsfMakeStruct.map = Path.GetFileName(pSsfMakeStruct.customMapFile);
                            }

                            if (!String.IsNullOrEmpty(pSsfMakeStruct.map))
                            {
                                totalTracks = this.getNumberOfTracks(seqFile);

                                if (this.prepareWorkingDir(pSsfMakeStruct, seqFile, toneFileName))
                                {
                                    this.customizeScript(pSsfMakeStruct, seqFile, toneFileName, totalTracks);
                                    this.executeScript();
                                    this.moveSsfToRipFolder(ripOutputFolder, totalTracks);
                                }

                                Directory.Delete(WORKING_FOLDER, true);
                            }
                            else
                            {
                                this.progressStruct.Clear();
                                this.progressStruct.FileName = seqFile;
                                this.progressStruct.ErrorMessage = "[ERROR - NO SUITABLE MAP FILE FOUND]: " + seqFile + Environment.NewLine;
                                ReportProgress(progress, this.progressStruct);
                            }
                        }
                        catch (Exception ex2)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.FileName = seqFile;
                            this.progressStruct.ErrorMessage = ex2.Message;
                            ReportProgress(progress, this.progressStruct);
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                } // if (!CancellationPending)

            } // foreach (string f in pUniqueSqFiles)
        }

        private string getMapFile(SsfMakeStruct pSsfMakeStruct, string seqFileName, string toneFileName)
        {
            FileInfo fi;
            string ret = null;

            fi = new FileInfo(seqFileName);
            long seqSize = fi.Length;

            fi = new FileInfo(toneFileName);
            long toneSize = fi.Length;

            if (pSsfMakeStruct.useDsp.Equals("0"))
            {
                if ((toneSize <= 0x6E000) && (seqSize <= 0x07000))
                {
                    ret = "GEN_SEQ07000.MAP";
                }                
                else if ((toneSize <= 0x6D000) && (seqSize <= 0x08000))
                {
                    ret = "GEN_SEQ08000.MAP";
                }
                else if ((toneSize <= 0x65000) && (seqSize <= 0x10000))
                {
                    ret = "GEN_SEQ10000.MAP";
                }
                else if ((toneSize <= 0x55000) && (seqSize <= 0x20000))
                {
                    ret = "GEN_SEQ20000.MAP";
                }
            }
            else // will require multi run, but try to use tone size as a guess
            {
                if (toneSize <= 0x44FC0)
                {
                    ret = "GEN_DSP20040.MAP";
                }
                else if (toneSize <= 0x54FC0)
                {
                    ret = "GEN_DSP10040.MAP";
                }
                else if (toneSize <= 0x5CFC0)
                {
                    ret = "GEN_DSP8040.MAP";
                }
                else if (toneSize <= 0x60FC0)
                {
                    ret = "GEN_DSP4040.MAP";
                }
            }

            return ret;
        }

        private bool prepareWorkingDir(SsfMakeStruct pSsfMakeStruct, string seqFileName, string toneFileName)
        {
            bool ret = false;
            string userFilePath;

            try
            {
                // create working dir
                if (!Directory.Exists(WORKING_FOLDER))
                {
                    Directory.CreateDirectory(WORKING_FOLDER);
                }

                // copy needed files
                string mapFileSourcePath;

                if (String.IsNullOrEmpty(pSsfMakeStruct.customMapFile))
                {
                    mapFileSourcePath = Path.Combine(SSFMAKE_FOLDER_PATH, pSsfMakeStruct.map);
                }
                else
                {
                    mapFileSourcePath = pSsfMakeStruct.customMapFile;
                }
                
                string mapFileDestinationPath = Path.Combine(WORKING_FOLDER, pSsfMakeStruct.map);

                File.Copy(mapFileSourcePath, mapFileDestinationPath, true);
                File.Copy(SSFMAKE_SOURCE_PATH, SSFMAKE_DESTINATION_PATH, true);
                File.Copy(SSFINFO_SOURCE_PATH, SSFINFO_DESTINATION_PATH, true);
                File.Copy(BIN2PSF_SOURCE_PATH, BIN2PSF_DESTINATION_PATH, true);
                File.Copy(PSFPOINT_SOURCE_PATH, PSFPOINT_DESTINATION_PATH, true);

                userFilePath = Path.GetFullPath(pSsfMakeStruct.driver);
                File.Copy(userFilePath, Path.Combine(WORKING_FOLDER, Path.GetFileName(userFilePath)), true);

                userFilePath = toneFileName;
                File.Copy(userFilePath, Path.Combine(WORKING_FOLDER, Path.GetFileName(userFilePath)), true);

                userFilePath = seqFileName;
                File.Copy(userFilePath, Path.Combine(WORKING_FOLDER, Path.GetFileName(userFilePath)), true);

                if (String.IsNullOrEmpty(pSsfMakeStruct.dspFile))
                {
                    userFilePath = Path.ChangeExtension(seqFileName, FILE_EXTENSION_DSP);
                    if (File.Exists(userFilePath))
                    {
                        File.Copy(userFilePath, Path.Combine(WORKING_FOLDER, Path.GetFileName(userFilePath)), true);
                    }
                }
                else
                {
                    File.Copy(pSsfMakeStruct.dspFile, Path.Combine(WORKING_FOLDER, Path.GetFileName(pSsfMakeStruct.dspFile)), true);
                }

                ret = true;
            }
            catch (Exception _ex)
            {
                Directory.Delete(WORKING_FOLDER, true);

                progressStruct.Clear();
                progressStruct.ErrorMessage = String.Format("ERROR: {0}", _ex.Message);
                ReportProgress(0, progressStruct);
            }

            return ret;
        }

        private bool customizeScript(SsfMakeStruct pSsfMakeStruct, string seqFileName, string toneFileName, int pTotalTracks)
        {            
            bool ret = true;
            int lineNumber;
            string inputLine;

            string workingFile = SSFMAKE_DESTINATION_PATH + ".tmp";
            string dspFileName = Path.GetFileName(Path.ChangeExtension(seqFileName, FILE_EXTENSION_DSP));
            string outputFileNameWithoutExtension = Path.GetFileNameWithoutExtension(seqFileName);

            // open reader
            StreamReader reader = new StreamReader(File.Open(SSFMAKE_DESTINATION_PATH, FileMode.Open, FileAccess.Read));
            
            // open writer for temp file
            StreamWriter writer = new StreamWriter(File.Open(workingFile, FileMode.Create, FileAccess.Write));

            lineNumber = 1;
            while ((inputLine = reader.ReadLine()) != null)
            {
                switch (lineNumber)
                {
                    case LINE_NUM_SEQ_BANK:
                        writer.WriteLine(String.Format("bank      = {0}    # sequence bank number", pSsfMakeStruct.sequenceBank));
                        break;
                    case LINE_NUM_SEQ_TRACK:
                        writer.WriteLine(String.Format("track     = {0}    # sequence track number", pSsfMakeStruct.sequenceTrack));
                        break;
                    case LINE_NUM_VOLUME:
                        writer.WriteLine(String.Format("volume    = {0}    # volume (reduce if clipping)", pSsfMakeStruct.volume));
                        break;
                    case LINE_NUM_MIXER_BANK:
                        writer.WriteLine(String.Format("mixerbank = {0}    # mixer bank number (usually same as sequence bank number)", pSsfMakeStruct.mixerBank));
                        break;
                    case LINE_NUM_MIXER_NUMBER:
                        writer.WriteLine(String.Format("mixern    = {0}    # mixer number (usually 0)", pSsfMakeStruct.mixerNumber));
                        break;
                    case LINE_NUM_EFFECT:
                        writer.WriteLine(String.Format("effect    = {0}    # effect number (usually 0)", pSsfMakeStruct.effectNumber));
                        break;
                    case LINE_NUM_USE_DSP:
                        writer.WriteLine(String.Format("use_dsp   = {0}       # 1: use DSP, 0: do not use DSP", pSsfMakeStruct.useDsp));
                        break;
        
                    case LINE_NUM_DRIVER:
                        writer.WriteLine(String.Format("ndrv = '{0}'    # sound driver", Path.GetFileName(pSsfMakeStruct.driver)));
                        break;
                    case LINE_NUM_MAP:
                        writer.WriteLine(String.Format("nmap = '{0}'    # sound area map", pSsfMakeStruct.map));
                        break;
                    case LINE_NUM_TONE_DATA:
                        writer.WriteLine(String.Format("nbin = '{0}'    # tone data", Path.GetFileName(toneFileName)));
                        break;
                    case LINE_NUM_SEQ_DATA:
                        writer.WriteLine(String.Format("nseq = '{0}'    # sequence data", Path.GetFileName(seqFileName)));
                        break;
                    case LINE_NUM_DSP_PROGRAM:
                        if (String.IsNullOrEmpty(pSsfMakeStruct.dspFile))
                        {
                            if (File.Exists(Path.GetFullPath(Path.Combine(WORKING_FOLDER, dspFileName))))
                            {
                                writer.WriteLine(String.Format("nexb = '{0}'    # DSP program", Path.GetFileName(dspFileName)));
                            }
                            else
                            {
                                writer.WriteLine(String.Format("nexb = '{0}'    # DSP program", String.Empty));
                            }
                        }
                        else
                        {
                            writer.WriteLine(String.Format("nexb = '{0}'    # DSP program", Path.GetFileName(pSsfMakeStruct.dspFile)));
                        }
                        break;

                    case LINE_NUM_OUT_FILE:
                        if (pSsfMakeStruct.TryAllCombinations)
                        {
                            outputFileNameWithoutExtension = 
                                String.Format("SEQ[{0}]_TON[{1}]", Path.GetFileNameWithoutExtension(seqFileName), Path.GetFileNameWithoutExtension(toneFileName));
                        }

                        if (pTotalTracks > 1)
                        {
                            writer.WriteLine(String.Format("nout = '{0}.ssflib'    # output file name (if .ssflib, create ssflib and minissfs for each track in the bank)", outputFileNameWithoutExtension));                        
                        }
                        else
                        {
                            writer.WriteLine(String.Format("nout = '{0}.ssf'    # output file name (if .ssflib, create ssflib and minissfs for each track in the bank)", outputFileNameWithoutExtension));                        
                        }
                        break;
                
                    default:
                        writer.WriteLine(inputLine);
                        break;
                }
                
                lineNumber++;
            }
            
            // close reader
            reader.Close();
            reader.Dispose();
            
            // close writer
            writer.Close();
            writer.Dispose();

            // delete original
            File.Delete(SSFMAKE_DESTINATION_PATH);
            // rename edited temp file
            File.Move(workingFile, SSFMAKE_DESTINATION_PATH);

            return ret;
        }

        private bool executeScript()
        {
            bool ret = false;
            bool isSuccess;

            string ssfMakeOutput;
            string ssfMakeError = String.Empty;
            string arguments = String.Format(" {0}", Path.GetFileName(SSFMAKE_DESTINATION_PATH));

            using (Process ssfMakeProcess = new Process())
            {
                try
                {
                    ssfMakeProcess.StartInfo = new ProcessStartInfo("python.exe", arguments);
                    ssfMakeProcess.StartInfo.WorkingDirectory = WORKING_FOLDER;
                    ssfMakeProcess.StartInfo.UseShellExecute = false;
                    ssfMakeProcess.StartInfo.CreateNoWindow = true;
                    ssfMakeProcess.StartInfo.RedirectStandardOutput = true;
                    ssfMakeProcess.StartInfo.RedirectStandardError = true;
                    isSuccess = ssfMakeProcess.Start();
                    ssfMakeOutput = ssfMakeProcess.StandardOutput.ReadToEnd();
                    ssfMakeError = ssfMakeProcess.StandardError.ReadToEnd();
                    ssfMakeProcess.WaitForExit();

                    this.progressStruct.Clear();
                    this.progressStruct.GenericMessage = ssfMakeOutput + Environment.NewLine;
                    this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                    ret = true && isSuccess;
                }
                catch (Exception _e)
                {
                    ret = false;
                    this.progressStruct.Clear();
                    this.progressStruct.ErrorMessage = _e.Message;
                    ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
                }
                finally
                {
                    if (!String.IsNullOrEmpty(ssfMakeError))
                    {
                        ret = false;
                        this.progressStruct.Clear();
                        this.progressStruct.ErrorMessage = "[ERROR - SSFMAKE]" + ssfMakeError + Environment.NewLine;
                        ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);                    
                    }
                }
            }

            return ret;
        }

        private void moveSsfToRipFolder(string ripOutputFolder, int pTotalTracks)
        {
            string destinationFile;

            string[] sourceFiles;

            if (!Directory.Exists(ripOutputFolder))
            {
                Directory.CreateDirectory(ripOutputFolder);
            }

            if (pTotalTracks > 1)
            {
                sourceFiles = Directory.GetFiles(WORKING_FOLDER, "*.MINISSF");

                foreach (string f in sourceFiles)
                {
                    destinationFile = Path.Combine(ripOutputFolder, Path.GetFileName(f));
                    File.Copy(f, destinationFile, false);
                }

                sourceFiles = Directory.GetFiles(WORKING_FOLDER, "*.SSFLIB");

                foreach (string f in sourceFiles)
                {
                    destinationFile = Path.Combine(ripOutputFolder, Path.GetFileName(f));
                    File.Copy(f, destinationFile, false);
                }
            }
            else 
            {
                sourceFiles = Directory.GetFiles(WORKING_FOLDER, "*.SSF");

                foreach (string f in sourceFiles)
                {
                    destinationFile = Path.Combine(ripOutputFolder, Path.GetFileName(f));
                    File.Copy(f, destinationFile, false);
                }            
            }
        }

        private void extractData(SsfMakeStruct pSsfMakeStruct, DoWorkEventArgs e)
        {
            string destinationPath;
            string arguments;
            bool isSuccess;
            string seekOutput;
            Process extractionProcess = null;

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

            foreach (string file in Directory.GetFiles(pSsfMakeStruct.sourcePath))
            {
                if (!CancellationPending)
                {                
                    try
                    {
                        // copy to working dir
                        destinationPath = Path.Combine(WORKING_FOLDER_SEEK, Path.GetFileName(file));
                        File.Copy(file, destinationPath, true);

                        // extract SEQ
                        arguments = String.Format(" {0} {1} .",
                            Path.GetFileName(seqextScriptPath), Path.GetFileName(file));
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
                        this.progressStruct.GenericMessage = String.Format("[SEQEXT - {0}]", Path.GetFileName(file)) +
                            Environment.NewLine + seekOutput;
                        this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);


                        // extract TONE
                        arguments = String.Format(" {0} {1} .",
                            Path.GetFileName(tonextScriptPath), Path.GetFileName(file));
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
                        this.progressStruct.GenericMessage = String.Format("[TONEXT - {0}]", Path.GetFileName(file)) +
                            Environment.NewLine + seekOutput + Environment.NewLine;
                        this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
                       
                        // delete the original
                        File.Delete(destinationPath);
                    }
                    catch (Exception _e)
                    {
                        this.progressStruct.Clear();
                        this.progressStruct.ErrorMessage = _e.Message;
                        ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
                    }
                    finally
                    {
                        if (extractionProcess != null)
                        {
                            extractionProcess.Dispose();
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            
            } // foreach (string file in Directory.GetFiles(pSsfMakeStruct.sourcePath))

            // delete the scripts
            File.Delete(seqextScriptPath);
            File.Delete(tonextScriptPath);
        }

        private void timeSsfs(string ripOutputFolder)
        {
            string arguments;
            bool isSuccess;

            // copy ssftime.py to rip folder
            string ssftimeDestinationPath = 
                Path.Combine(ripOutputFolder, Path.GetFileName(SSFTIME_SOURCE_PATH));
            File.Copy(SSFTIME_SOURCE_PATH, ssftimeDestinationPath, true);

            // copy psfpoint.exe to rip folder
            string psfpointDestinationPath =
                Path.Combine(ripOutputFolder, Path.GetFileName(PSFPOINT_SOURCE_PATH));
            File.Copy(PSFPOINT_SOURCE_PATH, psfpointDestinationPath, true);

            using (Process timingProcess = new Process())
            {
                // time .ssf
                arguments = String.Format(" {0} {1} .",
                    Path.GetFileName(ssftimeDestinationPath), "*.ssf");
                
                timingProcess.StartInfo = new ProcessStartInfo("python.exe", arguments);
                timingProcess.StartInfo.WorkingDirectory = ripOutputFolder;
                timingProcess.StartInfo.UseShellExecute = false;
                timingProcess.StartInfo.CreateNoWindow = true;
                isSuccess = timingProcess.Start();
                timingProcess.WaitForExit();
                timingProcess.Close();

                // time .minissf
                arguments = String.Format(" {0} {1} .",
                    Path.GetFileName(ssftimeDestinationPath), "*.minissf");
                
                timingProcess.StartInfo = new ProcessStartInfo("python.exe", arguments);
                timingProcess.StartInfo.WorkingDirectory = ripOutputFolder;
                timingProcess.StartInfo.UseShellExecute = false;
                timingProcess.StartInfo.CreateNoWindow = true;
                isSuccess = timingProcess.Start();
                timingProcess.WaitForExit();
            }

            // delete ssftime.py and psfpoint.exe
            File.Delete(ssftimeDestinationPath);
            File.Delete(psfpointDestinationPath);
        }

        private int getNumberOfTracks(string pSequencePath)
        {
            int ret = 1;

            using (FileStream fs = File.OpenRead(pSequencePath))
            {
                fs.Position = 1;
                ret = fs.ReadByte();            
            }

            return ret;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            SsfMakeStruct ssfMakeStruct = (SsfMakeStruct)e.Argument;
            
            if (XsfUtil.IsPythonPresentInPath())
            {            
                this.makeSsfs(ssfMakeStruct, e);
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
