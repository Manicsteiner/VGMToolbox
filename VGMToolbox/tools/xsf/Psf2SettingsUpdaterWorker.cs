using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class Psf2SettingsUpdaterWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        private static readonly string PSF2_PROGRAMS_FOLDER =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "psf2"));
        private static readonly string MKPSF2_SOURCE_PATH =
            Path.Combine(PSF2_PROGRAMS_FOLDER, "mkpsf2.exe");
        
        public struct Psf2SettingsUpdaterStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }
            public Psf2.Psf2IniSqIrxStruct IniSettings { set; get; }
            public bool RemoveEmptySettings { set; get; }
        }
        
        public Psf2SettingsUpdaterWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pPsf2SettingsUpdaterStruct, 
            DoWorkEventArgs e)
        {
            Psf2SettingsUpdaterStruct iniStruct = (Psf2SettingsUpdaterStruct)pPsf2SettingsUpdaterStruct;            
            string workingFolder = String.Empty;

            //////////////////////
            // copy existing tags
            //////////////////////
            string formatString = XsfUtil.GetXsfFormatString(pPath);
            Dictionary<string, string> tagHash;
            Xsf psf2File, newPsf2File;

            if (!String.IsNullOrEmpty(formatString) &&
                formatString.Equals(Xsf.FormatNamePsf2))
            {
                try
                {
                    using (FileStream fs = File.Open(pPath, FileMode.Open, FileAccess.Read))
                    {
                        // initialize file
                        psf2File = new Xsf();
                        psf2File.Initialize(fs, pPath);

                        // copy tags
                        tagHash = psf2File.GetTagHash();
                    }

                    ///////////////
                    // unpack Psf2
                    ///////////////
                    workingFolder = Path.Combine(Path.GetDirectoryName(pPath), Path.GetFileNameWithoutExtension(pPath));
                    string unpackFolder = Path.Combine(workingFolder, "unpack_dir");
                    XsfUtil.UnpackPsf2(pPath, unpackFolder);

                    ///////////////////
                    // parse .ini file
                    ///////////////////
                    Psf2.Psf2IniSqIrxStruct originalIni;
                    string[] originalIniFiles = Directory.GetFiles(unpackFolder, "*.ini");

                    if (originalIniFiles.Length > 0)
                    {

                        using (FileStream iniFs = File.Open(originalIniFiles[0], FileMode.Open, FileAccess.Read))
                        {
                            originalIni = Psf2.ParseClsIniFile(iniFs);
                        }

                        ////////////////////
                        // update .ini file
                        ////////////////////
                        iniStruct.IniSettings = UpdateClsIniFile(iniStruct.IniSettings, originalIni, iniStruct.RemoveEmptySettings);
                        File.Delete(originalIniFiles[0]);
                        Psf2.WriteClsIniFile(iniStruct.IniSettings, originalIniFiles[0]);

                        ///////////////
                        // repack Psf2
                        ///////////////
                        string psf2FileName = Path.GetFileName(pPath);

                        ///////////////////
                        // copy mkpsf2.exe
                        ///////////////////
                        string mkpsf2Destination = Path.Combine(workingFolder, Path.GetFileName(MKPSF2_SOURCE_PATH));
                        File.Copy(MKPSF2_SOURCE_PATH, mkpsf2Destination, true);

                        //////////////////
                        // run mkpsf2.exe
                        //////////////////
                        string arguments = String.Format(" \"{0}\" \"{1}\"", psf2FileName, unpackFolder);
                        Process makePsf2Process = new Process();
                        makePsf2Process.StartInfo = new ProcessStartInfo(mkpsf2Destination, arguments);
                        makePsf2Process.StartInfo.UseShellExecute = false;
                        makePsf2Process.StartInfo.CreateNoWindow = true;
                        makePsf2Process.StartInfo.WorkingDirectory = workingFolder;
                        bool isSuccess = makePsf2Process.Start();
                        makePsf2Process.WaitForExit();
                        makePsf2Process.Close();
                        makePsf2Process.Dispose();

                        ////////////////
                        // replace tags
                        ////////////////
                        string newPsf2FilePath = Path.Combine(workingFolder, psf2FileName);

                        using (FileStream newFs = File.Open(newPsf2FilePath, FileMode.Open, FileAccess.Read))
                        {
                            // initialize new file
                            newPsf2File = new Xsf();
                            newPsf2File.Initialize(newFs, newPsf2FilePath);
                        }

                        // update to use old tag hash
                        newPsf2File.TagHash = tagHash;
                        newPsf2File.UpdateTags();

                        /////////////////////////
                        // replace original file
                        /////////////////////////                    
                        File.Copy(newPsf2FilePath, pPath, true);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    // delete working folder
                    if (Directory.Exists(workingFolder))
                    {
                        Directory.Delete(workingFolder, true);
                    }
                }
            }
        }

        private Psf2.Psf2IniSqIrxStruct UpdateClsIniFile(
            Psf2.Psf2IniSqIrxStruct newIniFile, 
            Psf2.Psf2IniSqIrxStruct oldIniFile, 
            bool removeEmptyEntries)
        {
            Psf2.Psf2IniSqIrxStruct ret = new Psf2.Psf2IniSqIrxStruct();

            ret.SqFileName = GetUpdatedValue(oldIniFile.SqFileName, newIniFile.SqFileName, false);
            ret.HdFileName = GetUpdatedValue(oldIniFile.HdFileName, newIniFile.HdFileName, false);
            ret.BdFileName = GetUpdatedValue(oldIniFile.BdFileName, newIniFile.BdFileName, false);

            ret.SequenceNumber = GetUpdatedValue(oldIniFile.SequenceNumber, newIniFile.SequenceNumber, removeEmptyEntries);
            ret.TimerTickInterval = GetUpdatedValue(oldIniFile.TimerTickInterval, newIniFile.TimerTickInterval, removeEmptyEntries);
            ret.Reverb = GetUpdatedValue(oldIniFile.Reverb, newIniFile.Reverb, removeEmptyEntries);
            ret.Depth = GetUpdatedValue(oldIniFile.Depth, newIniFile.Depth, removeEmptyEntries);
            ret.Tempo = GetUpdatedValue(oldIniFile.Tempo, newIniFile.Tempo, removeEmptyEntries);
            ret.Volume = GetUpdatedValue(oldIniFile.Volume, newIniFile.Volume, removeEmptyEntries);

            return ret;
        }

        private string GetUpdatedValue(string oldIniValue, string newIniValue, bool removeEmptyEntries)
        {
            string ret;

            if (!String.IsNullOrEmpty(newIniValue) || removeEmptyEntries)
            {
                ret = newIniValue;
            }
            else
            {
                ret = oldIniValue;
            }

            return ret;
        }
    }
}
