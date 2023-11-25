using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class Psf2toPsf2LibWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        private static readonly string PSF2_PROGRAMS_FOLDER =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "psf2"));
        private static readonly string UNPKPSF2_SOURCE_PATH =
            Path.Combine(PSF2_PROGRAMS_FOLDER, "unpkpsf2.exe");
        private static readonly string MKPSF2_SOURCE_PATH =
            Path.Combine(PSF2_PROGRAMS_FOLDER, "mkpsf2.exe");

        private static readonly string PSFPOINT_SOURCE_PATH =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "psfpoint.exe"));
        private static readonly string PSFPOINT_BATCH_FILE = "psfpoint_batch.bat";

        private static readonly string PSF2LIB_WORKING_FOLDER =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "working_psf2lib"));
        private static readonly string PSF2LIB_WORKING_SUBFOLDER =
            Path.GetFullPath(Path.Combine(PSF2LIB_WORKING_FOLDER, "lib"));
        
        private static readonly string PSF2_WORKING_FOLDER =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "working_psf2"));

        private static readonly string PSF2_RIPS_FOLDER =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "rips"), "psf2s"));

        private const string BD_HASH_KEY = "BD";
        private const string HD_HASH_KEY = "HD";
        private const string SQ_HASH_KEY = "SQ";

        VGMToolbox.util.ProgressStruct progressStruct;
        private int fileCount;
        private int maxFiles;
        private Dictionary<string, Dictionary<string, string>> fileNameHash;


        public Psf2toPsf2LibWorker()
        {
            fileCount = 0;
            maxFiles = 0;
            fileNameHash = new Dictionary<string, Dictionary<string, string>>();
            fileNameHash.Add(BD_HASH_KEY, new Dictionary<string, string>());
            fileNameHash.Add(HD_HASH_KEY, new Dictionary<string, string>());
            fileNameHash.Add(SQ_HASH_KEY, new Dictionary<string, string>());

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        public struct Psf2ToPsf2LibStruct
        {
            public string sourcePath;
            public string libraryName;
        }

        private void buildPsf2Lib(Psf2ToPsf2LibStruct pPsf2ToPsf2LibStruct, DoWorkEventArgs e)
        {
            bool isSuccess;
            
            if (!Directory.Exists(pPsf2ToPsf2LibStruct.sourcePath))
            {
                this.progressStruct = new VGMToolbox.util.ProgressStruct();
                this.progressStruct.NewNode = null;
                this.progressStruct.FileName = null;
                this.progressStruct.ErrorMessage =
                    String.Format("ERROR: Directory <{0}> Not Found.", pPsf2ToPsf2LibStruct.sourcePath);
                ReportProgress(0, this.progressStruct);
            }
            else
            {
                // Get Max Files
                this.maxFiles = Directory.GetFiles(pPsf2ToPsf2LibStruct.sourcePath, "*.psf2").Length;
                
                pPsf2ToPsf2LibStruct.libraryName = 
                    FileUtil.CleanFileName(pPsf2ToPsf2LibStruct.libraryName) + 
                    ".psf2lib";
                
                foreach (string f in 
                    Directory.GetFiles(pPsf2ToPsf2LibStruct.sourcePath, "*.psf2"))
                {
                    // report progress
                    int progress = (++this.fileCount * 100) / maxFiles;
                    this.progressStruct = new VGMToolbox.util.ProgressStruct();
                    this.progressStruct.NewNode = null;
                    this.progressStruct.FileName = f;
                    ReportProgress(progress, this.progressStruct);                    
                                        
                    using (FileStream fs = File.OpenRead(f))
                    {
                        // check type
                        Type dataType = FormatUtil.getObjectType(fs);

                        if (dataType != null && dataType.Name.Equals("Xsf"))
                        {
                            Xsf psf2File = new Xsf();
                            psf2File.Initialize(fs, f);

                            if (psf2File.GetFormat().Equals(Xsf.FormatNamePsf2))
                            {
                                // copy tags
                                Dictionary<string, string> tagHash = psf2File.GetTagHash();

                                // unpack files
                                string outputDir = Path.Combine(PSF2_WORKING_FOLDER, 
                                    Path.GetFileNameWithoutExtension(f));
                                isSuccess = this.unpackPsf2File(f, outputDir);
                                
                                if (isSuccess)
                                {
                                    // move non.ini files to lib working dir
                                    isSuccess = moveNonIniFiles(outputDir, PSF2LIB_WORKING_SUBFOLDER);

                                    // make simple psf2 with just pointer .ini
                                    isSuccess = makeMiniPsf2(outputDir);

                                    if (isSuccess)
                                    {
                                        // update tag script
                                        addTagsToTagScript(f, tagHash, pPsf2ToPsf2LibStruct.libraryName);
                                    }
                                }
                                                                                                                           
                            } // if (psf2File.getFormat().Equals(Xsf.FORMAT_NAME_PSF2))
                        
                        } // if (dataType != null && dataType.Name.Equals("Xsf"))

                        fs.Close();
                    } // using (FileStream fs = File.OpenRead(f))
                
                } // foreach (string f in Directory.GetFiles(pPsf2ToPsf2LibStruct.sourcePath, "*.psf2"))

                // combine files into lib
                makePsf2Lib(PSF2LIB_WORKING_SUBFOLDER, pPsf2ToPsf2LibStruct.libraryName);

                // execute tagging script
                executeBatchScript();

                // move completed psf2s to rip folder
                doCleanup(pPsf2ToPsf2LibStruct);
            }
        }

        private bool unpackPsf2File(string pSourcePath, string pOutputPath)
        {
            int progress;
            bool isSuccess = false;
            Process unpkPsf2Process = null;
                        
            try
            {
                // create dir
                if (!Directory.Exists(pOutputPath))
                {
                    Directory.CreateDirectory(pOutputPath);
                }
                
                // call unpkpsf2.exe
                string arguments = String.Format(" \"{0}\" \"{1}\"", pSourcePath, pOutputPath);
                unpkPsf2Process = new Process();
                unpkPsf2Process.StartInfo = new ProcessStartInfo(UNPKPSF2_SOURCE_PATH, arguments);
                unpkPsf2Process.StartInfo.UseShellExecute = false;
                unpkPsf2Process.StartInfo.CreateNoWindow = true;
                isSuccess = unpkPsf2Process.Start();
                unpkPsf2Process.WaitForExit();
                unpkPsf2Process.Close();
                unpkPsf2Process.Dispose();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                
                if ((unpkPsf2Process != null) && (!unpkPsf2Process.HasExited))
                {
                    unpkPsf2Process.Close();
                    unpkPsf2Process.Dispose();
                }

                progress = (fileCount * 100) / maxFiles;
                this.progressStruct = new VGMToolbox.util.ProgressStruct();
                this.progressStruct.NewNode = null;
                this.progressStruct.ErrorMessage = String.Format("Error processing <{0}>.  Error received: ", pSourcePath) + ex.Message;
                ReportProgress(progress, this.progressStruct);
            }

            return isSuccess;
        }

        private bool moveNonIniFiles(string pSourceDirectory, string pDestinationDirectory)
        {
            bool isSuccess = true;
            Crc32 crc32Generator = new Crc32();
            string fileDestinationPath;
            string fileExtension;
            string fileName;

            Psf2.Psf2IniSqIrxStruct psf2IniStruct;

            if (!Directory.Exists(pDestinationDirectory))
            {
                Directory.CreateDirectory(pDestinationDirectory);
            }

            // get .ini and parse
            string iniPath = Path.Combine(pSourceDirectory, "psf2.ini");
            using (FileStream fs = File.OpenRead(iniPath))
            {
                psf2IniStruct = Psf2.ParseClsIniFile(fs);
                fs.Close();
            }

            foreach (string f in Directory.GetFiles(pSourceDirectory, "*.*", SearchOption.AllDirectories))
            {
                fileDestinationPath = String.Empty;
                
                // check if it is one of the SQ/BD/HD files
                fileName = Path.GetFileName(f).ToUpper();
                fileExtension = Path.GetExtension(f).ToUpper();

                if (fileName.Equals(Path.GetFileName(psf2IniStruct.BdFileName))) 
                {
                    psf2IniStruct.BdFileName = this.getFileNameForChecksum(f, BD_HASH_KEY) + Psf2.FILE_EXTENSION_BD;
                    fileDestinationPath = Path.Combine(pDestinationDirectory, psf2IniStruct.BdFileName);
                }
                else if (fileName.Equals(Path.GetFileName(psf2IniStruct.HdFileName))) 
                {
                    psf2IniStruct.HdFileName = this.getFileNameForChecksum(f, HD_HASH_KEY) + Psf2.FILE_EXTENSION_HD;
                    fileDestinationPath = Path.Combine(pDestinationDirectory, psf2IniStruct.HdFileName);
                }
                else if (fileName.Equals(Path.GetFileName(psf2IniStruct.SqFileName))) 
                {
                    psf2IniStruct.SqFileName = this.getFileNameForChecksum(f, SQ_HASH_KEY) + Psf2.FILE_EXTENSION_SQ;
                    fileDestinationPath = Path.Combine(pDestinationDirectory, psf2IniStruct.SqFileName);
                }
                else if (fileExtension != ".INI")
                {
                    fileDestinationPath = Path.Combine(pDestinationDirectory, Path.GetFileName(f));
                }

                if (!String.IsNullOrEmpty(fileDestinationPath))
                {
                    try
                    {
                        File.Copy(f, fileDestinationPath, true);
                    }
                    catch (Exception _e)
                    {
                        isSuccess = false;

                        int progress = (fileCount * 100) / maxFiles;
                        this.progressStruct.Clear();
                        this.progressStruct.ErrorMessage = String.Format("Error processing <{0}>.  Error received: ", f) + _e.Message;
                        ReportProgress(progress, this.progressStruct);

                        break;
                    }

                    File.Delete(f);               
                }                                
            }

            // rebuild .ini file
            Psf2.WriteClsIniFile(psf2IniStruct, iniPath);

            return isSuccess;         
        }

        private bool makeMiniPsf2(string pSourceDirectory)
        {
            Process makePsf2Process = null;
            bool isSuccess = false;
            string outputFilePrefix = Path.GetFileName(pSourceDirectory);

            string mkpsf2Destination = Path.Combine(PSF2_WORKING_FOLDER, Path.GetFileName(MKPSF2_SOURCE_PATH));

            try
            {
                if (!File.Exists(mkpsf2Destination))
                {
                    File.Copy(MKPSF2_SOURCE_PATH, mkpsf2Destination, true);
                }

                // run makepsf2                
                string arguments = String.Format(" \"{0}.minipsf2\" \"{1}\"", outputFilePrefix, pSourceDirectory);
                makePsf2Process = new Process();
                makePsf2Process.StartInfo = new ProcessStartInfo(mkpsf2Destination, arguments);
                makePsf2Process.StartInfo.UseShellExecute = false;
                makePsf2Process.StartInfo.CreateNoWindow = true;
                makePsf2Process.StartInfo.WorkingDirectory = PSF2_WORKING_FOLDER;
                isSuccess = makePsf2Process.Start();
                makePsf2Process.WaitForExit();
                makePsf2Process.Close();
                makePsf2Process.Dispose();

                Directory.Delete(pSourceDirectory, true);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                
                if ((makePsf2Process != null))
                {
                    makePsf2Process.Close();
                    makePsf2Process.Dispose();
                }

                int progress = (fileCount * 100) / maxFiles;
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = String.Format("Error creating <{0}.minipsf2>.  Error received: ", outputFilePrefix) + ex.Message;
                ReportProgress(progress, this.progressStruct);            
            }
            
            return isSuccess;
        }

        private bool makePsf2Lib(string pSourceDirectory, string pOutputFileName)
        {
            bool isSuccess = false;
            Process makePsf2Process = null;

            string mkpsf2Destination = Path.Combine(PSF2LIB_WORKING_FOLDER, Path.GetFileName(MKPSF2_SOURCE_PATH));
            string outputFileSource = Path.Combine(PSF2LIB_WORKING_FOLDER, pOutputFileName);
            string outputFileDestination = Path.Combine(PSF2_WORKING_FOLDER, pOutputFileName);

            if (!Directory.Exists(Path.GetDirectoryName(mkpsf2Destination)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(mkpsf2Destination));
            }
            
            if (!File.Exists(mkpsf2Destination))
            {
                File.Copy(MKPSF2_SOURCE_PATH, mkpsf2Destination, true);
            }

            try
            {
                // run makepsf2                
                string arguments = String.Format(" \"{0}\" \"{1}\"", pOutputFileName, pSourceDirectory);
                makePsf2Process = new Process();
                makePsf2Process.StartInfo = new ProcessStartInfo(mkpsf2Destination, arguments);
                makePsf2Process.StartInfo.UseShellExecute = false;
                makePsf2Process.StartInfo.CreateNoWindow = true;
                makePsf2Process.StartInfo.WorkingDirectory = PSF2LIB_WORKING_FOLDER;
                isSuccess = makePsf2Process.Start();
                makePsf2Process.WaitForExit();
                makePsf2Process.Close();
                makePsf2Process.Dispose();

                Directory.Delete(pSourceDirectory, true);

                File.Move(outputFileSource, outputFileDestination);
                File.Delete(mkpsf2Destination);
            }
            catch (Exception ex)
            {
                isSuccess = false;

                if ((makePsf2Process != null))
                {
                    makePsf2Process.Close();
                    makePsf2Process.Dispose();
                }

                int progress = (fileCount * 100) / maxFiles;
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = 
                    String.Format("Error creating <{0}>.  Error received: ", pOutputFileName) + ex.Message +
                    Environment.NewLine;
                ReportProgress(progress, this.progressStruct);                        
            }

            return isSuccess;
        }

        private bool addTagsToTagScript(string pFileName, 
            Dictionary<string, string> pTagHash, string pLibName)
        {
            bool isSuccess = true;
            string batchFilePath = Path.Combine(PSF2_WORKING_FOLDER, PSFPOINT_BATCH_FILE);

            try
            {
                using (StreamWriter sw = new StreamWriter(File.Open(batchFilePath, FileMode.Append, FileAccess.Write)))
                {
                    foreach (string k in pTagHash.Keys)
                    {
                        sw.WriteLine("psfpoint.exe -{0}=\"{1}\" \"{2}\"",
                            k, pTagHash[k], Path.GetFileNameWithoutExtension(pFileName) + ".minipsf2");
                    }

                    // add lib
                    sw.WriteLine("psfpoint.exe -{0}=\"{1}\" \"{2}\"",
                        "_lib", pLibName, Path.GetFileNameWithoutExtension(pFileName) + ".minipsf2");
                    sw.WriteLine();

                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                
                int progress = (fileCount * 100) / maxFiles;
                this.progressStruct = new VGMToolbox.util.ProgressStruct();
                this.progressStruct.NewNode = null;
                this.progressStruct.ErrorMessage = String.Format("Error adding batch file info for <{0}>.  Error received: ", pFileName) + ex.Message;
                ReportProgress(progress, this.progressStruct);                
            }

            return isSuccess;
        }

        private void executeBatchScript()
        {
            Process psfProcess;

            try
            {
                // copy psfpoint.exe
                string psfpointDestinationPath = Path.Combine(PSF2_WORKING_FOLDER, Path.GetFileName(PSFPOINT_SOURCE_PATH));
                File.Copy(PSFPOINT_SOURCE_PATH, psfpointDestinationPath, true);

                // execute script
                psfProcess = new Process();
                psfProcess.StartInfo = new ProcessStartInfo(Path.Combine(PSF2_WORKING_FOLDER, PSFPOINT_BATCH_FILE));
                psfProcess.StartInfo.UseShellExecute = false;
                psfProcess.StartInfo.WorkingDirectory = PSF2_WORKING_FOLDER;
                psfProcess.StartInfo.CreateNoWindow = true;
                psfProcess.Start();
                psfProcess.WaitForExit();
                psfProcess.Close();
                psfProcess.Dispose();

                // delete file
                File.Delete(psfpointDestinationPath);
            }
            catch (Exception ex)
            {
                int progress = (fileCount * 100) / maxFiles;
                this.progressStruct = new VGMToolbox.util.ProgressStruct();
                this.progressStruct.NewNode = null;
                this.progressStruct.ErrorMessage = 
                    "Error executing batch script.  Error received: " + ex.Message +
                    Environment.NewLine;
                ReportProgress(progress, this.progressStruct);
            }
        }

        private void doCleanup(Psf2ToPsf2LibStruct pPsf2ToPsf2LibStruct)
        {
            try
            {
                // remove mkpsf2
                File.Delete(Path.Combine(PSF2_WORKING_FOLDER, Path.GetFileName(MKPSF2_SOURCE_PATH)));

                // copy files to rip folder
                string destinationFolder =
                    Path.Combine(PSF2_RIPS_FOLDER, Path.GetFileNameWithoutExtension(pPsf2ToPsf2LibStruct.libraryName));

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                foreach (string f in Directory.GetFiles(PSF2_WORKING_FOLDER))
                {
                    File.Move(f, Path.Combine(destinationFolder, Path.GetFileName(f)));
                }

                // delete working folders
                Directory.Delete(PSF2_WORKING_FOLDER, true);
                Directory.Delete(PSF2LIB_WORKING_FOLDER, true);
            }
            catch (Exception ex)
            {
                int progress = (fileCount * 100) / maxFiles;
                this.progressStruct = new VGMToolbox.util.ProgressStruct();
                this.progressStruct.NewNode = null;
                this.progressStruct.ErrorMessage = "Error cleaning up.  Error received: " + ex.Message;
                ReportProgress(progress, this.progressStruct);            
            }
        }

        private string getFileNameForChecksum(string pFileName, string pHashKey)
        {
            string checksumKey;
            string newFileName;
            string ret;
            
            using (FileStream fs = File.OpenRead(pFileName))
            {
                checksumKey = String.Format("{0}_{1}_{2}", ChecksumUtil.GetCrc32OfFullFile(fs), ChecksumUtil.GetMd5OfFullFile(fs),
                    ChecksumUtil.GetSha1OfFullFile(fs));
            }

            if (this.fileNameHash[pHashKey].ContainsKey(checksumKey))
            {
                ret = this.fileNameHash[pHashKey][checksumKey];
            }
            else
            {
                newFileName = this.fileNameHash[pHashKey].Count.ToString("X8");
                this.fileNameHash[pHashKey].Add(checksumKey, newFileName);
                ret = newFileName;
            }
            
            return ret;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Psf2ToPsf2LibStruct psf2ToPsf2LibStruct = (Psf2ToPsf2LibStruct)e.Argument;
            this.buildPsf2Lib(psf2ToPsf2LibStruct, e);
        }
    }
}
