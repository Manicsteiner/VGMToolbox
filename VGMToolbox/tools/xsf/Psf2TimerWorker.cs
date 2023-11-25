using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.xsf
{
    class Psf2TimerWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        Dictionary<string, string> extractedLibHash;

        private const string BATCH_FILE_NAME = "!timing_batch.bat";

        public struct Psf2TimerStruct : IVgmtWorkerStruct
        {                        
            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public Psf2TimerWorker() :
            base()
        {            
            extractedLibHash = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pPsf2TimerStruct, 
            DoWorkEventArgs e)
        {
            string outputSqFileName = null;
            string[] libPaths;            
            string[] sqFiles;
            string[] iniFiles;
            Psf2.Psf2IniSqIrxStruct psf2IniStruct;
            
            string filePath;
            string fileDir;
            string fileName;
            string outputDir;
            string libOutputDir;

            Ps2SequenceData.Ps2SqTimingStruct psf2Time;
            int minutes;
            double seconds;
            int sequenceNumber = 0;
            
            StringBuilder batchFile = new StringBuilder();
            string batchFilePath;

            string formatString = XsfUtil.GetXsfFormatString(pPath);

            if (!String.IsNullOrEmpty(formatString) && (formatString.Equals(Xsf.FormatNamePsf2)))
            {
                filePath = Path.GetFullPath(pPath);
                fileDir = Path.GetDirectoryName(filePath);
                fileName = Path.GetFileNameWithoutExtension(filePath);
                            
                outputDir = XsfUtil.UnpackPsf2(filePath);

                // parse ini
                iniFiles = Directory.GetFiles(outputDir, "PSF2.INI", SearchOption.AllDirectories);

                if (iniFiles.Length > 0)
                {
                    using (FileStream iniFs = File.Open(iniFiles[0], FileMode.Open, FileAccess.Read))
                    {
                        // parse ini file to get SQ info
                        psf2IniStruct = Psf2.ParseClsIniFile(iniFs);
                    }

                    using (FileStream fs = File.OpenRead(pPath))
                    {
                        Psf2 psf2File = new Psf2();
                        psf2File.Initialize(fs, pPath);

                        // check for libs
                        libPaths = psf2File.GetLibPathArray();
                    }

                    // copy the SQ file out (should only be one)
                    sqFiles = Directory.GetFiles(outputDir, psf2IniStruct.SqFileName, SearchOption.AllDirectories);

                    if (sqFiles.Length > 0)
                    {
                        if (!String.IsNullOrEmpty(psf2IniStruct.SequenceNumber))
                        {
                            sequenceNumber = int.Parse(psf2IniStruct.SequenceNumber);
                            outputSqFileName = String.Format("{0}_n={1}.SQ", outputDir, psf2IniStruct.SequenceNumber);
                        }
                        else
                        {
                            outputSqFileName = String.Format("{0}.SQ", outputDir);
                        }
                        File.Copy(sqFiles[0], outputSqFileName, true);
                    }
                    else // miniPSF2
                    {
                        // unpack each lib, looking for the needed file
                        foreach (string libPath in libPaths)
                        {
                            fileDir = Path.GetDirectoryName(libPath);
                            fileName = Path.GetFileNameWithoutExtension(libPath);
                            libOutputDir = Path.Combine(fileDir, fileName);

                            if (!extractedLibHash.ContainsKey(libPath))
                            {
                                libOutputDir = XsfUtil.UnpackPsf2(libPath);
                                extractedLibHash.Add(libPath, libOutputDir.ToUpper());
                            }

                            // look for the file in this lib
                            sqFiles = Directory.GetFiles(libOutputDir, psf2IniStruct.SqFileName, SearchOption.AllDirectories);

                            if (sqFiles.Length > 0)
                            {
                                if (!String.IsNullOrEmpty(psf2IniStruct.SequenceNumber))
                                {
                                    sequenceNumber = int.Parse(psf2IniStruct.SequenceNumber);
                                    outputSqFileName = String.Format("{0}_n={1}.SQ", outputDir, psf2IniStruct.SequenceNumber);
                                }
                                else
                                {
                                    outputSqFileName = String.Format("{0}.SQ", outputDir);
                                }

                                File.Copy(sqFiles[0], outputSqFileName, true);
                                break;
                            }

                            // delete the unpkpsf2 output folder
                            if (Directory.Exists(libOutputDir))
                            {
                                Directory.Delete(libOutputDir, true);
                            }

                        } // foreach (string libPath in libPaths)
                    }

                    // get time and add to script
                    if (!String.IsNullOrEmpty(outputSqFileName))
                    {
                        psf2Time = XsfUtil.GetTimeForPsf2File(outputSqFileName, sequenceNumber);

                        File.Delete(outputSqFileName);  // delete SQ file

                        minutes = (int)(psf2Time.TimeInSeconds / 60d);
                        seconds = (psf2Time.TimeInSeconds - (minutes * 60));
                        // seconds = Math.Ceiling(seconds);

                        // shouldn't be needed without Math.Ceiling call, but whatever
                        if (seconds >= 60)
                        {
                            minutes++;
                            seconds -= 60d;
                        }

                        batchFile.AppendFormat("psfpoint.exe -length=\"{0}:{1}\" -fade=\"{2}\" \"{3}\"",
                            minutes.ToString(), seconds.ToString().PadLeft(2, '0'),
                            psf2Time.FadeInSeconds.ToString(), Path.GetFileName(pPath));
                        batchFile.Append(Environment.NewLine);

                        batchFilePath = Path.Combine(Path.GetDirectoryName(pPath), BATCH_FILE_NAME);

                        if (!File.Exists(batchFilePath))
                        {
                            using (FileStream cfs = File.Create(batchFilePath)) { };
                        }

                        using (StreamWriter sw = new StreamWriter(File.Open(batchFilePath, FileMode.Append, FileAccess.Write)))
                        {
                            sw.Write(batchFile.ToString());
                        }

                        // report warnings
                        if (!String.IsNullOrEmpty(psf2Time.Warnings))
                        {
                            this.progressStruct.Clear();
                            progressStruct.GenericMessage = String.Format("{0}{1}  WARNINGS{2}    {3}", pPath, Environment.NewLine, Environment.NewLine, psf2Time.Warnings);
                            ReportProgress(this.Progress, progressStruct);
                        }
                    }
                } // if (iniFiles.Length > 0)

                // delete the unpkpsf2 output folder
                if ((Directory.Exists(outputDir)) &&
                    (!extractedLibHash.ContainsValue(outputDir.ToUpper())))
                {
                    Directory.Delete(outputDir, true);
                }

            } // if (psf2File.getFormat().Equals(Xsf.FORMAT_NAME_PSF2) && (!psf2File.IsFileLibrary()))
        }    

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            base.OnDoWork(e);

            // delete lib folders
            foreach (string k in extractedLibHash.Keys)
            {
                if (Directory.Exists(extractedLibHash[k]))
                {
                    Directory.Delete(extractedLibHash[k], true);
                }
            }
        }        
    }
}
