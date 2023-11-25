using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.util;

namespace mkpsf2fe
{
    class MkPsf2Worker : BackgroundWorker
    {

        private readonly string WORKING_FOLDER = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "working");
        private readonly string MODULES_FOLDER = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "modules");
        private readonly string PROGRAMS_FOLDER = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "programs");
        private readonly string OUTPUT_FOLDER = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "psf2s");
        
        private int fileCount = 0;
        private int maxFiles = 0;

        public struct MkPsf2Struct
        {
            public string sourcePath;
            public string modulePath;
            
            public string tickInterval;
            public string reverb;
            public string depth;
            public string tempo;
            public string volume;
        }

        public MkPsf2Worker()
        {
            fileCount = 0;
            maxFiles = 0;
            
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }


        private void makePsf2s(MkPsf2Struct pMkPsf2Struct, DoWorkEventArgs e)
        {
            string[] uniqueSqFiles;

            if (!CancellationPending)
            {
                // get list of unique files
                uniqueSqFiles = this.getUniqueFileNames(pMkPsf2Struct.sourcePath);
                if (uniqueSqFiles != null)
                {
                    this.maxFiles = uniqueSqFiles.Length;
                    this.buildPsf2s(uniqueSqFiles, pMkPsf2Struct, e);
                }
            }
            else
            {
                e.Cancel = true;
                return;
            }
                             
            return;
        }

        private string[] getUniqueFileNames(string pSourceDirectory)
        {
            int fileCount = 0;
            int i = 0;
            string[] ret = null;

            VGMToolbox.util.ProgressStruct vProgressStruct = new VGMToolbox.util.ProgressStruct();

            if (!Directory.Exists(pSourceDirectory))
            {
                vProgressStruct = new VGMToolbox.util.ProgressStruct();
                vProgressStruct.NewNode = null;
                vProgressStruct.ErrorMessage = String.Format("ERROR: Directory {0} not found.", pSourceDirectory);
                ReportProgress(Constants.ProgressMessageOnly, vProgressStruct);
            }
            else
            {
                fileCount = Directory.GetFiles(pSourceDirectory, "*.SQ").Length;

                if (fileCount > 0)
                {
                    ret = new string[fileCount];
                }

                foreach (string f in Directory.GetFiles(pSourceDirectory, "*.SQ"))
                {
                    ret[i] = f;
                    i++;
                }
            }

            return ret;
        }

        private void buildPsf2s(string[] pUniqueSqFiles, MkPsf2Struct pMkPsf2Struct, 
            DoWorkEventArgs e)
        {
            Process makePsf2Process;
            VGMToolbox.util.ProgressStruct vProgressStruct = new VGMToolbox.util.ProgressStruct();

            string makePsf2SourcePath = Path.Combine(PROGRAMS_FOLDER, "mkpsf2.exe");
            string makePsf2DestinationPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "mkpsf2.exe");

            try
            {
                Directory.CreateDirectory(WORKING_FOLDER);

                // copy generic modules to working directory
                File.Copy(Path.Combine(MODULES_FOLDER, "psf2.irx"), Path.Combine(WORKING_FOLDER, "psf2.irx"), true);
                File.Copy(Path.Combine(MODULES_FOLDER, "sq.irx"), Path.Combine(WORKING_FOLDER, "sq.irx"), true);

                // copy source modules to working directory
                File.Copy(Path.Combine(pMkPsf2Struct.modulePath, "LIBSD.IRX"), Path.Combine(WORKING_FOLDER, "LIBSD.IRX"), true);
                File.Copy(Path.Combine(pMkPsf2Struct.modulePath, "MODHSYN.IRX"), Path.Combine(WORKING_FOLDER, "MODHSYN.IRX"), true);
                File.Copy(Path.Combine(pMkPsf2Struct.modulePath, "MODMIDI.IRX"), Path.Combine(WORKING_FOLDER, "MODMIDI.IRX"), true);

                // copy program
                File.Copy(makePsf2SourcePath, makePsf2DestinationPath, true);
            }
            catch (Exception ex)
            {
                vProgressStruct = new VGMToolbox.util.ProgressStruct();
                vProgressStruct.NewNode = null;
                vProgressStruct.FileName = null;
                vProgressStruct.ErrorMessage = ex.Message;
                ReportProgress(0, vProgressStruct);

                return;
            }
            
            foreach (string f in pUniqueSqFiles)
            {
                if (!CancellationPending)
                {
                    StringBuilder sqArguments = new StringBuilder();
                    
                    // report progress
                    int progress = (++this.fileCount * 100) / maxFiles;
                    vProgressStruct = new VGMToolbox.util.ProgressStruct();
                    vProgressStruct.NewNode = null;
                    vProgressStruct.FileName = f;
                    ReportProgress(progress, vProgressStruct);

                    try
                    {

                        // copy data files to working directory                    
                        string filePrefix = Path.GetFileNameWithoutExtension(f);
                        string sourceDirectory = Path.GetDirectoryName(f);

                        string bdFileName = filePrefix + ".bd";
                        string hdFileName = filePrefix + ".hd";
                        string sqFileName = filePrefix + ".sq";

                        string sourceBdFile = Path.Combine(sourceDirectory, bdFileName);
                        string sourceHdFile = Path.Combine(sourceDirectory, hdFileName);
                        string sourceSqFile = Path.Combine(sourceDirectory, sqFileName);

                        string destinationBdFile = Path.Combine(WORKING_FOLDER, bdFileName);
                        string destinationHdFile = Path.Combine(WORKING_FOLDER, hdFileName);
                        string destinationSqFile = Path.Combine(WORKING_FOLDER, sqFileName);

                        File.Copy(sourceBdFile, destinationBdFile);
                        File.Copy(sourceHdFile, destinationHdFile);
                        File.Copy(sourceSqFile, destinationSqFile);

                        // write ini file
                        string iniPath = Path.Combine(WORKING_FOLDER, "psf2.ini");
                        StreamWriter sw = File.CreateText(iniPath);
                        sw.WriteLine("libsd.irx");
                        sw.WriteLine("modhsyn.irx");
                        sw.WriteLine("modmidi.irx");

                        // build sq.irx arguments                    
                        sqArguments.Append(String.IsNullOrEmpty(pMkPsf2Struct.reverb.Trim()) ?
                            " -r=5" : String.Format(" -r={0}", pMkPsf2Struct.reverb.Trim()));
                        sqArguments.Append(String.IsNullOrEmpty(pMkPsf2Struct.depth.Trim()) ?
                            " -d=16383" : String.Format(" -d={0}", pMkPsf2Struct.depth.Trim()));

                        sqArguments.Append(String.IsNullOrEmpty(pMkPsf2Struct.tickInterval.Trim()) ?
                            String.Empty : String.Format(" -u={0}", pMkPsf2Struct.tickInterval.Trim()));
                        sqArguments.Append(String.IsNullOrEmpty(pMkPsf2Struct.tempo.Trim()) ?
                            String.Empty : String.Format(" -t={0}", pMkPsf2Struct.tempo.Trim()));
                        sqArguments.Append(String.IsNullOrEmpty(pMkPsf2Struct.volume.Trim()) ?
                            String.Empty : String.Format(" -v={0}", pMkPsf2Struct.volume.Trim()));

                        sqArguments.Append(String.Format(" -s={0} -h={1} -b={2}",
                            sqFileName, hdFileName, bdFileName));

                        //sw.WriteLine(String.Format("sq.irx -r=5 -d=16383 -s={0} -h={1} -b={2}",
                        //    sqFileName, hdFileName, bdFileName));

                        sw.WriteLine(String.Format("sq.irx {0}", sqArguments.ToString()));
                        sw.Close();
                        sw.Dispose();

                        // run makepsf2                
                        string arguments = String.Format(" {0}.psf2 {1}", filePrefix, WORKING_FOLDER);
                        makePsf2Process = new Process();
                        makePsf2Process.StartInfo = new ProcessStartInfo(makePsf2DestinationPath, arguments);
                        makePsf2Process.StartInfo.UseShellExecute = false;
                        makePsf2Process.StartInfo.CreateNoWindow = true;
                        bool isSuccess = makePsf2Process.Start();
                        makePsf2Process.WaitForExit();

                        if (isSuccess)
                        {
                            vProgressStruct = new VGMToolbox.util.ProgressStruct();
                            vProgressStruct.NewNode = null;
                            vProgressStruct.GenericMessage = String.Format("{0}.psf2 created.", filePrefix) +
                                Environment.NewLine;
                            ReportProgress(Constants.ProgressMessageOnly, vProgressStruct);

                            File.Move(filePrefix + ".psf2", Path.Combine(OUTPUT_FOLDER, filePrefix + ".psf2"));
                        }

                        File.Delete(destinationBdFile);
                        File.Delete(destinationHdFile);
                        File.Delete(destinationSqFile);
                        File.Delete(iniPath);
                    }
                    catch (Exception ex2)
                    {
                        vProgressStruct = new VGMToolbox.util.ProgressStruct();
                        vProgressStruct.NewNode = null;
                        vProgressStruct.FileName = f;
                        vProgressStruct.ErrorMessage = ex2.Message;
                        ReportProgress(progress, vProgressStruct);
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }

            } // foreach

            try
            {
                Directory.Delete(WORKING_FOLDER, true);
                File.Delete(makePsf2DestinationPath);
            }
            catch (Exception ex3)
            {
                vProgressStruct = new VGMToolbox.util.ProgressStruct();
                vProgressStruct.NewNode = null;
                vProgressStruct.FileName = null;
                vProgressStruct.ErrorMessage = ex3.Message;
                ReportProgress(100, vProgressStruct);
            }
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            MkPsf2Struct mkPsf2StructStruct = (MkPsf2Struct)e.Argument;

            this.makePsf2s(mkPsf2StructStruct, e);
        }    
    }

}
