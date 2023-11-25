using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class MkPsf2Worker : BackgroundWorker, IVgmtBackgroundWorker
    {
        private const int NO_SEQ_NUM_FOUND = -1;

        private readonly string WORKING_FOLDER =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "working_psf2"));
        private readonly string MODULES_FOLDER =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "psf2"), "modules"));
        private readonly string PROGRAMS_FOLDER =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"), "psf2"));
        private readonly string OUTPUT_FOLDER =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "rips"), "psf2s"));

        private const long PSF2_CSL_SEQ_COUNT_OFFSET = 0x3C;
        private const long PSF2_CSL_SEQ_OFFSET_BEGIN = 0x40;

        public const string PSF2LIB_FILE_EXTENSION = ".psf2lib";

        private ProgressStruct progressStruct;
        private int progress = 0;
        private int fileCount = 0;
        private int maxFiles = 0;

        private bool[] validSequenceArray;

        public struct MkPsf2Struct
        {
            public string sourcePath;
            public string modulePath;
            public string outputFolder;

            public string tickInterval;
            public string reverb;
            public string depth;
            public string tempo;
            public string volume;

            public bool TryCombinations { set; get; }
            public bool CreatePsf2lib { set; get; }
            public string Psf2LibName { set; get; }
        }

        public MkPsf2Worker()
        {
            progressStruct = new ProgressStruct();
            fileCount = 0;
            maxFiles = 0;

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }


        private void makePsf2s(MkPsf2Struct pMkPsf2Struct, DoWorkEventArgs e)
        {
            string[] uniqueSqFiles;
            string[] uniqueHdFiles;

            if (!CancellationPending)
            {
                // get list of unique files
                uniqueSqFiles = this.getUniqueFileNames(pMkPsf2Struct.sourcePath, "*.SQ");
                uniqueHdFiles = this.getUniqueFileNames(pMkPsf2Struct.sourcePath, "*.HD");

                if (uniqueSqFiles != null)
                {
                    if (pMkPsf2Struct.TryCombinations)
                    {
                        this.maxFiles = uniqueSqFiles.Length * uniqueHdFiles.Length;
                    }
                    else
                    {
                        this.maxFiles = uniqueSqFiles.Length;
                    }

                    // verify we do not have more than one HD/BD pair if a psf2lib is desired
                    if (pMkPsf2Struct.CreatePsf2lib)
                    {
                        this.maxFiles++;
                        
                        if (uniqueHdFiles.Length > 1)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.ErrorMessage = String.Format("ERROR: More than 1 HD/BD pair detected, please only include 1 HD/BD pair when making .psf2lib files{0}", Environment.NewLine);
                            ReportProgress(this.progress, this.progressStruct);
                            return;                    
                        }
                    }
                    this.buildPsf2s(uniqueSqFiles, uniqueHdFiles, pMkPsf2Struct, e);
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
            string[] ret = null;

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

        private void buildPsf2s(string[] pUniqueSqFiles, string[] pUniqueHdFiles, MkPsf2Struct pMkPsf2Struct, 
            DoWorkEventArgs e)
        {
            string makePsf2SourcePath = Path.Combine(PROGRAMS_FOLDER, "mkpsf2.exe");
            string makePsf2DestinationPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "mkpsf2.exe");

            // setup working directory
            try
            {
                Directory.CreateDirectory(WORKING_FOLDER);

                // copy program
                File.Copy(makePsf2SourcePath, makePsf2DestinationPath, true);
            }
            catch (Exception ex)
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = ex.Message;
                ReportProgress(0, this.progressStruct);

                return;
            }
            
            // build psf2lib
            try
            {
                // make psf2lib first
                if (pMkPsf2Struct.CreatePsf2lib)
                {
                    this.makePsf2File(pMkPsf2Struct, null, pUniqueHdFiles[0], Path.ChangeExtension(pUniqueHdFiles[0], ".bd"),
                        makePsf2DestinationPath);
                }
            }
            catch (Exception psf2libEx)
            {
                this.progressStruct.Clear();
                this.progressStruct.FileName = pMkPsf2Struct.Psf2LibName;
                this.progressStruct.ErrorMessage = psf2libEx.Message;
                ReportProgress(this.progress, this.progressStruct);            
            }

            foreach (string f in pUniqueSqFiles)
            {
                if (!CancellationPending)
                {
                    if (pMkPsf2Struct.TryCombinations)
                    {
                        foreach (string hdFile in pUniqueHdFiles)
                        {
                            try
                            {
                                this.makePsf2File(pMkPsf2Struct, f, hdFile, Path.ChangeExtension(hdFile, ".bd"),
                                    makePsf2DestinationPath);
                            }
                            catch (Exception ex)
                            {
                                this.progressStruct.Clear();
                                this.progressStruct.FileName = String.Format("S({0})_H({1})", Path.GetFileNameWithoutExtension(f), Path.GetFileNameWithoutExtension(hdFile));
                                this.progressStruct.ErrorMessage = ex.Message;
                                ReportProgress(this.progress, this.progressStruct);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (!pMkPsf2Struct.CreatePsf2lib)
                            {
                                this.makePsf2File(pMkPsf2Struct, f, Path.ChangeExtension(f, ".hd"), Path.ChangeExtension(f, ".bd"),
                                    makePsf2DestinationPath);
                            }
                            else
                            {
                                this.makePsf2File(pMkPsf2Struct, f, pUniqueHdFiles[0], Path.ChangeExtension(pUniqueHdFiles[0], ".bd"),
                                    makePsf2DestinationPath);                            
                            }
                        }
                        catch (Exception ex)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.FileName = f;
                            this.progressStruct.ErrorMessage = ex.Message;
                            ReportProgress(this.progress, this.progressStruct);
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                } // if (!CancellationPending)

            } // foreach (string f in pUniqueSqFiles)
            
            // cleanup working folder
            try
            {
                Directory.Delete(WORKING_FOLDER, true);
                File.Delete(makePsf2DestinationPath);
            }
            catch (Exception ex3)
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = ex3.Message;
                ReportProgress(100, this.progressStruct);
            }
        }

        private void makePsf2File(MkPsf2Struct pMkPsf2Struct, string sqName, string hdName, string bdName,
            string makePsf2DestinationPath)
        {
            Process makePsf2Process;
            StringBuilder sqArguments = new StringBuilder();
            
            string sqFileName = String.Empty;
            string destinationSqFile = String.Empty;
            string hdFileName = String.Empty;
            string destinationHdFile = String.Empty;
            string bdFileName = String.Empty;
            string destinationBdFile = String.Empty;

            string destinationPsf2Irx = Path.Combine(WORKING_FOLDER, "psf2.irx");
            string destinationSqIrx = Path.Combine(WORKING_FOLDER, "sq.irx");
            string destinationLibSdIrx = Path.Combine(WORKING_FOLDER, "LIBSD.IRX");
            string destinationModHsynIrx = Path.Combine(WORKING_FOLDER, "MODHSYN.IRX");
            string destinationModMidiIrx = Path.Combine(WORKING_FOLDER, "MODMIDI.IRX");

            int totalSequences = 0;
            string outputFile;
            string outputFileExtension;

            // report progress
            this.progress = (++this.fileCount * 100) / maxFiles;
            this.progressStruct.Clear();
            this.progressStruct.FileName = sqName;
            ReportProgress(progress, this.progressStruct);

            try
            {                                
                // copy data files to working directory                    
                string filePrefix;
                string outputFilePrefix;

                if (pMkPsf2Struct.TryCombinations)
                {
                    filePrefix = String.Format("S({0})_H({1})", Path.GetFileNameWithoutExtension(sqName), Path.GetFileNameWithoutExtension(hdName));
                }
                else
                {
                    filePrefix = Path.GetFileNameWithoutExtension(sqName);
                }
                                
                // copy files to working dir if they exist
                if ((!pMkPsf2Struct.CreatePsf2lib) || 
                    (pMkPsf2Struct.CreatePsf2lib && String.IsNullOrEmpty(sqName)))

                {                                       
                    // copy generic modules to working directory
                    File.Copy(Path.Combine(MODULES_FOLDER, "psf2.irx"), destinationPsf2Irx, true);
                    File.Copy(Path.Combine(MODULES_FOLDER, "sq.irx"), destinationSqIrx, true);

                    // copy source modules to working directory
                    File.Copy(Path.Combine(pMkPsf2Struct.modulePath, "LIBSD.IRX"), destinationLibSdIrx, true);
                    File.Copy(Path.Combine(pMkPsf2Struct.modulePath, "MODHSYN.IRX"), destinationModHsynIrx, true);
                    File.Copy(Path.Combine(pMkPsf2Struct.modulePath, "MODMIDI.IRX"), destinationModMidiIrx, true);                
                }
                
                hdFileName = Path.GetFileName(hdName);
                bdFileName = Path.GetFileName(bdName);

                if ((!pMkPsf2Struct.CreatePsf2lib) ||
                    (pMkPsf2Struct.CreatePsf2lib && String.IsNullOrEmpty(sqName)))
                {
                    destinationHdFile = Path.Combine(WORKING_FOLDER, hdFileName);
                    destinationBdFile = Path.Combine(WORKING_FOLDER, bdFileName);
                    File.Copy(hdName, destinationHdFile);                    
                    File.Copy(bdName, destinationBdFile);
                }                                

                if (!String.IsNullOrEmpty(sqName))
                {
                    sqFileName = Path.GetFileName(sqName);
                    destinationSqFile = Path.Combine(WORKING_FOLDER, sqFileName);
                    File.Copy(sqName, destinationSqFile);

                    totalSequences = getTotalSequenceCount(destinationSqFile);

                    for (int i = 0; i < totalSequences; i++)
                    {
                        if (validSequenceArray[i])
                        {
                            sqArguments = new StringBuilder();

                            if (totalSequences > 1)
                            {
                                outputFilePrefix = filePrefix + "_" + i.ToString("X2");
                                sqArguments.Append(String.Format(" -n={0}", i));
                            }
                            else
                            {
                                outputFilePrefix = filePrefix;
                            }

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

                            sw.WriteLine(String.Format("sq.irx {0}", sqArguments.ToString()));
                            sw.Close();
                            sw.Dispose();

                            // set output extension
                            if (pMkPsf2Struct.CreatePsf2lib)
                            {
                                outputFileExtension = ".minipsf2";
                            }
                            else
                            {
                                outputFileExtension = ".psf2";
                            }
                                

                            // run makepsf2                                            
                            outputFile = String.Format("{0}{1}", outputFilePrefix, outputFileExtension);
                            string arguments = String.Format(" {0} {1}", outputFile, WORKING_FOLDER);
                            makePsf2Process = new Process();
                            makePsf2Process.StartInfo = new ProcessStartInfo(makePsf2DestinationPath, arguments);
                            makePsf2Process.StartInfo.UseShellExecute = false;
                            makePsf2Process.StartInfo.CreateNoWindow = true;
                            bool isSuccess = makePsf2Process.Start();
                            makePsf2Process.WaitForExit();

                            if (isSuccess)
                            {
                                // add lib tag
                                if (pMkPsf2Struct.CreatePsf2lib)
                                {
                                    using (FileStream ofs = File.Open(outputFile, FileMode.Open, FileAccess.Write))
                                    { 
                                        ofs.Seek(0, SeekOrigin.End);
                                        using (BinaryWriter bw = new BinaryWriter(ofs))
                                        {
                                            System.Text.Encoding enc = System.Text.Encoding.ASCII;
                                            bw.Write(enc.GetBytes(Xsf.ASCII_TAG)); // [TAG]

                                            bw.Write(enc.GetBytes(String.Format("_lib={0}", pMkPsf2Struct.Psf2LibName)));
                                            bw.Write(new byte[] { 0x0A });
                                        }
                                    }
                                }
                                
                                this.progressStruct.Clear();
                                this.progressStruct.GenericMessage = String.Format("{0} created.{1}", outputFile, Environment.NewLine);
                                ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                                if (!Directory.Exists(Path.Combine(OUTPUT_FOLDER, pMkPsf2Struct.outputFolder)))
                                {
                                    Directory.CreateDirectory(Path.Combine(OUTPUT_FOLDER, pMkPsf2Struct.outputFolder));
                                }

                                File.Move(outputFile, Path.Combine(Path.Combine(OUTPUT_FOLDER, pMkPsf2Struct.outputFolder), outputFile));
                            }

                            File.Delete(iniPath);
                        } // if (validSequenceArray[i])                                
                        else if (totalSequences == 1)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.GenericMessage = String.Format("WARNING: {0}.SQ has only ONE sequence and it is INVALID.  Skipping...", filePrefix) +
                                Environment.NewLine;
                            ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
                        }
                    } // for (int i = 0; i < totalSequences; i++)
                }
                else // make .psf2lib with HD/BD only
                {
                    // run makepsf2                
                    string arguments = String.Format(" {0} {1}", pMkPsf2Struct.Psf2LibName, WORKING_FOLDER);
                    makePsf2Process = new Process();
                    makePsf2Process.StartInfo = new ProcessStartInfo(makePsf2DestinationPath, arguments);
                    makePsf2Process.StartInfo.UseShellExecute = false;
                    makePsf2Process.StartInfo.CreateNoWindow = true;
                    bool isSuccess = makePsf2Process.Start();
                    makePsf2Process.WaitForExit();

                    if (isSuccess)
                    {
                        this.progressStruct.Clear();
                        this.progressStruct.GenericMessage = String.Format("{0} created.{1}", pMkPsf2Struct.Psf2LibName, Environment.NewLine);
                        ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                        if (!Directory.Exists(Path.Combine(OUTPUT_FOLDER, pMkPsf2Struct.outputFolder)))
                        {
                            Directory.CreateDirectory(Path.Combine(OUTPUT_FOLDER, pMkPsf2Struct.outputFolder));
                        }

                        File.Move(pMkPsf2Struct.Psf2LibName, Path.Combine(Path.Combine(OUTPUT_FOLDER, pMkPsf2Struct.outputFolder), pMkPsf2Struct.Psf2LibName));
                    }
                
                    // delete modules from working dir
                    if (File.Exists(destinationPsf2Irx))
                    {
                        File.Delete(destinationPsf2Irx);
                    }
                    if (File.Exists(destinationSqIrx))
                    {
                        File.Delete(destinationSqIrx);
                    }
                    if (File.Exists(destinationLibSdIrx))
                    {
                        File.Delete(destinationLibSdIrx);
                    }
                    if (File.Exists(destinationModHsynIrx))
                    {
                        File.Delete(destinationModHsynIrx);
                    }
                    if (File.Exists(destinationModMidiIrx))
                    {
                        File.Delete(destinationModMidiIrx);
                    }
                }

                // delete files if needed
                if (File.Exists(destinationSqFile))
                {
                    File.Delete(destinationSqFile);
                }
                if (File.Exists(destinationHdFile))
                {
                    File.Delete(destinationHdFile);
                }
                if (File.Exists(destinationBdFile))
                {
                    File.Delete(destinationBdFile);
                }                                                                       
            }
            catch (Exception ex2)
            {
                this.progressStruct.Clear();
                this.progressStruct.FileName = sqName;
                this.progressStruct.ErrorMessage = String.Format("{0}{1}", ex2.Message, Environment.NewLine);
                ReportProgress(progress, this.progressStruct);
            }                   
        }

        private int getTotalSequenceCount(string pFileName)
        {
            int ret = NO_SEQ_NUM_FOUND;
            long seqOffset = PSF2_CSL_SEQ_COUNT_OFFSET;

            using (FileStream fs = File.OpenRead(Path.GetFullPath(pFileName)))
            {
                // byte[] seqNumberBytes = ParseFile.parseSimpleOffset(fs, seqOffset, 1);
                byte[] seqNumberBytes = ParseFile.ParseSimpleOffset(fs, seqOffset, 4);
                ret = System.BitConverter.ToInt32(seqNumberBytes, 0);

                ret += 1;

                this.buildValidSequenceArray(fs, ret);

                fs.Close();
            }

            return ret;
        }

        private void buildValidSequenceArray(FileStream pFileStream, int pSequenceCount)
        {
            int tempValue;
            this.validSequenceArray = new bool[pSequenceCount];

            tempValue = BitConverter.ToInt32(ParseFile.ParseSimpleOffset(pFileStream,
                PSF2_CSL_SEQ_OFFSET_BEGIN, 4), 0);
            
            if (tempValue > 0)
            {
                this.validSequenceArray[0] = true;
            }

            for (int i = 1; i < pSequenceCount; i++)
            {
                tempValue = BitConverter.ToInt32(ParseFile.ParseSimpleOffset(pFileStream,
                    PSF2_CSL_SEQ_OFFSET_BEGIN + (i * 4), 4), 0);
                
                if (tempValue > 0)
                {
                    this.validSequenceArray[i] = true;
                }            
            }           
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            MkPsf2Struct mkPsf2StructStruct = (MkPsf2Struct)e.Argument;

            this.makePsf2s(mkPsf2StructStruct, e);
        }    
    }

}
