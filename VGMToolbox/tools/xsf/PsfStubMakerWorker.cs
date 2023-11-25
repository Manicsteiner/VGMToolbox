using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{        
    public struct PsfStubMakerStruct : IVgmtWorkerStruct
    {
        public string[] SourcePaths
        {
            get;
            set;
        }
        public string DriverText
        {
            set;
            get;
        }
        public bool IncludeReverb
        {
            set;
            get;
        }

        public string PsfDrvLoad { set; get; }
        public string PsfDrvSize { set; get; }
        public string PsfDrvParam { set; get; }
        public string PsfDrvParamSize { set; get; }

        public string MySeq{ set; get; }
        public string MySeqSize { set; get; }
        public string MyVh { set; get; }
        public string MyVhSize { set; get; }
        public string MyVb { set; get; }
        public string MyVbSize { set; get; }

        public bool UseSeqFunctions { set; get; }
        public bool OverrideDriverLoadAddress { set; get; }
        public bool RelaxLoadAddressRestriction { set; get; }
    }
    
    class PsfStubMakerWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public const string PsfDrvLoadDefault = "0x80100000";
        public const string PsfDrvSizeDefault = "0x00001000";
        public const string PsfDrvParamDefault = "0x80101000";
        public const string PsfDrvParamSizeDefault = "0x00000100";

        public const string MySeqDefault = "0x80120000";
        public const string MySeqSizeDefault = "0x00010000";
        public const string MyVhDefault = "0x80130000";
        public const string MyVhSizeDefault = "0x00010000";
        public const string MyVbDefault = "0x80140000";
        public const string MyVbSizeDefault = "0x00070000";
        
        public static readonly string PsfToolFolderPath =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "external"), "psf"));

        public static readonly string SigFindFilePath = Path.Combine(PsfToolFolderPath, "sigfind.exe");
        public static readonly string SigFind2FilePath = Path.Combine(PsfToolFolderPath, "sigfind2.exe");
        public static readonly string SigDatFilePath = Path.Combine(PsfToolFolderPath, "sig.dat");

        public static readonly string PsfOCycleSourceCodeFilePath = Path.Combine(PsfToolFolderPath, "psfdrv.c");
        public static readonly string PsfOCycleMakeFilePath = Path.Combine(PsfToolFolderPath, "mk.bat");

        public static readonly string WorkingFolderPath =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "working_psf"));         

        private const int COMMENT_REVERB_OPEN = 295;
        private const int COMMENT_REVERB_CLOSE = 318;

        private const int DoSeqDefineLineNo = 11;

        private const int PsfDrvLoadLineNo = 18;
        private const int PsfDrvSizeLineNo = 19;
        private const int PsfDrvParamLineNo = 20;
        private const int PsfDrvParamSizeLineNo = 21;

        private const int MySeqLineNo = 26;
        private const int MySeqSizeLineNo = 27;
        private const int MyVhLineNo = 28;
        private const int MyVhSizeLineNo = 29;
        private const int MyVbLineNo = 30;
        private const int MyVbSizeLineNo = 31;

        public PsfStubMakerWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pPsfStubMakerStruct, DoWorkEventArgs e)
        {
            if (XsfUtil.IsPsxExe(pPath))
            {
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("[{0}]", pPath) + Environment.NewLine;
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);

                PsfStubMakerStruct stubMakerParameters = (PsfStubMakerStruct)pPsfStubMakerStruct;

                string sigFindDestination;
                string sigFind2Destination;
                string sigDatDestination;
                string driverDestination;
                string psfOCycleSourceDestination;
                string psfOCycleMakeFileDestination;

                string arguments;
                string standardOutput;
                string standardError;
                bool isProcessSuccessful;

                PsfPsyQAddresses sigFindAddresses = null;

                string psfdrvObjFileName;
                string psfdrvBinFileName;
                string psfdrvDestinationFileName;

                //////////////////////////
                // prepare working folder
                //////////////////////////
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = "    - Preparing working folder." + Environment.NewLine;
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);

                if (!Directory.Exists(PsfStubMakerWorker.WorkingFolderPath))
                {
                    Directory.CreateDirectory(PsfStubMakerWorker.WorkingFolderPath);
                }

                sigFindDestination = Path.Combine(PsfStubMakerWorker.WorkingFolderPath, Path.GetFileName(PsfStubMakerWorker.SigFindFilePath));
                sigFind2Destination = Path.Combine(PsfStubMakerWorker.WorkingFolderPath, Path.GetFileName(PsfStubMakerWorker.SigFind2FilePath));
                sigDatDestination = Path.Combine(PsfStubMakerWorker.WorkingFolderPath, Path.GetFileName(PsfStubMakerWorker.SigDatFilePath));
                driverDestination = Path.Combine(PsfStubMakerWorker.WorkingFolderPath, Path.GetFileName(pPath).Replace(" ", String.Empty));
                psfOCycleSourceDestination = Path.Combine(PsfStubMakerWorker.WorkingFolderPath, Path.GetFileName(PsfStubMakerWorker.PsfOCycleSourceCodeFilePath));
                psfOCycleMakeFileDestination = Path.Combine(PsfStubMakerWorker.WorkingFolderPath, Path.GetFileName(PsfStubMakerWorker.PsfOCycleMakeFilePath));

                File.Copy(PsfStubMakerWorker.SigFindFilePath, sigFindDestination, true);
                File.Copy(PsfStubMakerWorker.SigFind2FilePath, sigFind2Destination, true);
                File.Copy(PsfStubMakerWorker.SigDatFilePath, sigDatDestination, true);
                File.Copy(pPath, driverDestination, true);
                File.Copy(PsfStubMakerWorker.PsfOCycleSourceCodeFilePath, psfOCycleSourceDestination, true);
                File.Copy(PsfStubMakerWorker.PsfOCycleMakeFilePath, psfOCycleMakeFileDestination, true);

                ////////////////////
                // call sigfind.exe
                ////////////////////            
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = "    - Execute sigfind/sigfind2." + Environment.NewLine;
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);

                arguments = String.Format("{0}", Path.GetFileName(driverDestination));
                isProcessSuccessful = FileUtil.ExecuteExternalProgram(sigFindDestination, arguments,
                    PsfStubMakerWorker.WorkingFolderPath, out standardOutput, out standardError);

                /////////////////////
                // get psyQAddresses
                /////////////////////
                if ((isProcessSuccessful) && (String.IsNullOrEmpty(standardError)) &&
                    (!standardOutput.Contains("ERROR")))
                {
                    sigFindAddresses = XsfUtil.GetSigFindItems(
                        new MemoryStream(System.Text.Encoding.ASCII.GetBytes(standardOutput)),
                        stubMakerParameters.RelaxLoadAddressRestriction);
                }

                /////////////////
                // call sigfind2
                /////////////////
                arguments = String.Format("{0}", Path.GetFileName(driverDestination));
                isProcessSuccessful = FileUtil.ExecuteExternalProgram(sigFind2Destination, arguments,
                    PsfStubMakerWorker.WorkingFolderPath, out standardOutput, out standardError);

                /////////////////////
                // get psyQAddresses
                /////////////////////
                if ((isProcessSuccessful) && (String.IsNullOrEmpty(standardError)) &&
                    (!standardOutput.Contains("ERROR")))
                {
                    sigFindAddresses = XsfUtil.GetSigFindItems(
                        new MemoryStream(System.Text.Encoding.ASCII.GetBytes(standardOutput)), 
                        stubMakerParameters.RelaxLoadAddressRestriction, 
                        sigFindAddresses);
                }

                /////////////////////////
                // get additional values
                /////////////////////////
                sigFindAddresses = this.getAdditionalDriverInfo(sigFindAddresses, stubMakerParameters, driverDestination);

                //////////////////////////////
                // rewrite driver source code
                //////////////////////////////
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = "    - Rewriting driver source and batch file." + Environment.NewLine;
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);

                this.rewritePsfOCycleDriverSource(psfOCycleSourceDestination, sigFindAddresses, stubMakerParameters);

                /////////////////////
                // rewrite make file
                /////////////////////
                this.rewritePsfOCycleMakeFile(psfOCycleMakeFileDestination, sigFindAddresses, stubMakerParameters);

                //////////////////////////////////
                // delete old files if they exist
                //////////////////////////////////
                psfdrvObjFileName = Path.Combine(PsfStubMakerWorker.WorkingFolderPath, "psfdrv.obj");
                psfdrvBinFileName = Path.Combine(PsfStubMakerWorker.WorkingFolderPath, "psfdrv.bin");

                if (File.Exists(psfdrvObjFileName)) { File.Delete(psfdrvObjFileName); }
                if (File.Exists(psfdrvBinFileName)) { File.Delete(psfdrvBinFileName); }

                ////////////////
                // compile stub
                ////////////////
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = "    - Compiling stub." + Environment.NewLine;
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);

                arguments = String.Empty;
                isProcessSuccessful = FileUtil.ExecuteExternalProgram(psfOCycleMakeFileDestination, arguments,
                    PsfStubMakerWorker.WorkingFolderPath, out standardOutput, out standardError);

                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("        STDOUT: {0}{1}", standardOutput, Environment.NewLine);
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("        STDERR: {0}{1}", standardError, Environment.NewLine);
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);

                ////////////////////////////////////////
                // copy driver and source to source dir
                ////////////////////////////////////////
                psfdrvDestinationFileName = Path.Combine(Path.GetDirectoryName(pPath), Path.GetFileName(pPath) + ".stub.bin");

                if (!File.Exists(psfdrvBinFileName))
                {
                    this.progressStruct.Clear();
                    this.progressStruct.ErrorMessage = "[ERROR from VGMToolbox] Compiled output file not found.  Compilation has failed.  Stub file source code will be copied to your source directory for examination." + Environment.NewLine;
                    ReportProgress(this.progress, progressStruct);
                }
                else
                {
                    File.Copy(psfdrvBinFileName, psfdrvDestinationFileName, true);
                }                
                                
                File.Copy(psfOCycleSourceDestination, Path.Combine(Path.GetDirectoryName(pPath), Path.GetFileName(pPath) + ".stub.c"), true);

                //////////////////////
                // remove working dir
                //////////////////////
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = "    - Delete working folder." + Environment.NewLine;
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);

                if (Directory.Exists(PsfStubMakerWorker.WorkingFolderPath)) { Directory.Delete(PsfStubMakerWorker.WorkingFolderPath, true); }

                // output warnings
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = getWarnings(sigFindAddresses);
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);
            }
            else
            {
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("[{0}] Skipped.  Does not have a valid PS-X EXE signature.", pPath) + Environment.NewLine;
                ReportProgress(Constants.ProgressMessageOnly, progressStruct);
            }
        }

        private PsfPsyQAddresses getAdditionalDriverInfo(PsfPsyQAddresses existingValues, PsfStubMakerStruct stubMakerParameters, 
            string driverPath)
        {
            string checksum;
            byte[] jumpAddress;
            PsfPsyQAddresses ret = existingValues;
            
            using (FileStream checksumStream = File.OpenRead(driverPath))
            {
                checksum = ChecksumUtil.GetCrc32OfFullFile(checksumStream);
                jumpAddress = ParseFile.ParseSimpleOffset(checksumStream, 0x10, 0x04);
            }
           
            ret.DriverTextString = stubMakerParameters.DriverText;
            ret.ExeFileNameCrc = String.Format("  (int)\"{0}\", 0x{1},", Path.GetFileName(driverPath), checksum);
            ret.JumpPatchAddress = String.Format("0x{0}", BitConverter.ToUInt32(jumpAddress, 0).ToString("X8"));

            return ret;
        }

        private void rewritePsfOCycleDriverSource(string driverSourceCodePath, PsfPsyQAddresses addresses, PsfStubMakerStruct stubMakerParameters)
        {
            int lineNumber;
            string inputLine;
            string tempFilePath = Path.GetTempFileName();

            string lineItem;
            string lineFormat;
            string lineValue;
            PropertyInfo psyQValue;

            Dictionary<int, string> psyQSourceCodeLineNumber = XsfUtil.getPsyQSourceCodeLineNumberList();
            Dictionary<string, string> psyQSourceCode = XsfUtil.getPsyQSourceCodeList();

            // open reader
            using (StreamReader reader =
                new StreamReader(File.Open(driverSourceCodePath, FileMode.Open, FileAccess.Read)))
            {
                // open writer for temp file
                using (StreamWriter writer =
                    new StreamWriter(File.Open(tempFilePath, FileMode.Create, FileAccess.Write)))
                {
                    lineNumber = 1;

                    while ((inputLine = reader.ReadLine()) != null)
                    {
                        if (psyQSourceCodeLineNumber.ContainsKey(lineNumber))
                        {
                            if (stubMakerParameters.OverrideDriverLoadAddress &&
                                lineNumber == PsfDrvLoadLineNo)
                            {
                                writer.WriteLine(String.Format("#define PSFDRV_LOAD       ({0})", stubMakerParameters.PsfDrvLoad));                            
                            }
                            else
                            {
                                lineItem = psyQSourceCodeLineNumber[lineNumber];
                                lineFormat = psyQSourceCode[lineItem];
                                psyQValue = addresses.GetType().GetProperty(lineItem);
                                lineValue = (string)psyQValue.GetValue(addresses, null);

                                if (!String.IsNullOrEmpty(lineValue))
                                {
                                    writer.WriteLine(String.Format(lineFormat, lineValue));
                                }
                                else
                                {
                                    // comment out this line
                                    writer.WriteLine(String.Format("//{0}", inputLine));
                                }
                            }
                        }
                        else
                        {
                            switch (lineNumber)
                            {
                                case DoSeqDefineLineNo:
                                    if (!stubMakerParameters.UseSeqFunctions)
                                    {
                                        writer.WriteLine("#undef DO_SEQ");
                                    }
                                    else
                                    {
                                        writer.WriteLine(inputLine);
                                    }
                                    break;
                                case COMMENT_REVERB_OPEN:
                                    if (!stubMakerParameters.IncludeReverb)
                                    {
                                        writer.WriteLine("/*");
                                    }
                                    else
                                    {
                                        writer.WriteLine(inputLine);
                                    }
                                    break;                                
                                case COMMENT_REVERB_CLOSE:
                                    if (!stubMakerParameters.IncludeReverb)
                                    {
                                        writer.WriteLine("*/");
                                    }
                                    else
                                    {
                                        writer.WriteLine(inputLine);
                                    }
                                    break;
                                case PsfDrvSizeLineNo:
                                    this.rewriteOptionalLine(writer, stubMakerParameters.OverrideDriverLoadAddress,
                                        "#define PSFDRV_SIZE       ({0})", stubMakerParameters.PsfDrvSize, inputLine);
                                    break;
                                case PsfDrvParamLineNo:
                                    this.rewriteOptionalLine(writer, stubMakerParameters.OverrideDriverLoadAddress,
                                        "#define PSFDRV_PARAM      ({0})", stubMakerParameters.PsfDrvParam, inputLine);
                                    break;
                                case PsfDrvParamSizeLineNo:
                                    this.rewriteOptionalLine(writer, stubMakerParameters.OverrideDriverLoadAddress,
                                        "#define PSFDRV_PARAM_SIZE ({0})", stubMakerParameters.PsfDrvParamSize, inputLine);
                                    break;
                                case MySeqLineNo:
                                    this.rewriteOptionalLine(writer, stubMakerParameters.OverrideDriverLoadAddress,
                                        "#define MY_SEQ      ({0})", stubMakerParameters.MySeq, inputLine);                                    
                                    break;
                                case MySeqSizeLineNo:
                                    this.rewriteOptionalLine(writer, stubMakerParameters.OverrideDriverLoadAddress,
                                        "#define MY_SEQ_SIZE ({0})", stubMakerParameters.MySeqSize, inputLine);                                    
                                    break;
                                case MyVhLineNo:
                                    this.rewriteOptionalLine(writer, stubMakerParameters.OverrideDriverLoadAddress,
                                        "#define MY_VH ({0})", stubMakerParameters.MyVh, inputLine);
                                    break;
                                case MyVhSizeLineNo:
                                    this.rewriteOptionalLine(writer, stubMakerParameters.OverrideDriverLoadAddress,
                                        "#define MY_VH_SIZE ({0})", stubMakerParameters.MyVhSize, inputLine);
                                    break;
                                case MyVbLineNo:
                                    this.rewriteOptionalLine(writer, stubMakerParameters.OverrideDriverLoadAddress,
                                        "#define MY_VB ({0})", stubMakerParameters.MyVb, inputLine);
                                    break;
                                case MyVbSizeLineNo:
                                    this.rewriteOptionalLine(writer, stubMakerParameters.OverrideDriverLoadAddress,
                                        "#define MY_VB_SIZE ({0})", stubMakerParameters.MyVbSize, inputLine);
                                    break;
                                default:
                                    writer.WriteLine(inputLine);
                                    break;
                            }
                        }

                        lineNumber++;
                    }
                } // using (StreamWriter writer...
            } // using (StreamReader reader...

            // overwrite original
            File.Copy(tempFilePath, driverSourceCodePath, true);
        }

        private void rewritePsfOCycleMakeFile(string makeFilePath, PsfPsyQAddresses addresses, PsfStubMakerStruct stubMakerParameters)
        {

            int lineNumber;
            string inputLine;
            string tempFilePath = Path.GetTempFileName();

            // open reader
            using (StreamReader reader =
                new StreamReader(File.Open(makeFilePath, FileMode.Open, FileAccess.Read)))
            {
                // open writer for temp file
                using (StreamWriter writer =
                    new StreamWriter(File.Open(tempFilePath, FileMode.Create, FileAccess.Write)))
                {
                    lineNumber = 1;

                    while ((inputLine = reader.ReadLine()) != null)
                    {
                        switch (lineNumber)
                        { 
                            case 17:
                                if (stubMakerParameters.OverrideDriverLoadAddress)
                                {
                                    writer.WriteLine(String.Format("psylink /o {0} /p /z psfdrv.obj,psfdrv.bin", stubMakerParameters.PsfDrvLoad));
                                }
                                else
                                {
                                    writer.WriteLine(String.Format("psylink /o {0} /p /z psfdrv.obj,psfdrv.bin", addresses.PsfDrvLoadAddress));
                                }
                                break;
                            default:
                                writer.WriteLine(inputLine);
                                break;
                        }                        

                        lineNumber++;
                    }
                } // using (StreamWriter writer...
            } // using (StreamReader reader...

            // overwrite original
            File.Copy(tempFilePath, makeFilePath, true);
        }

        private string getWarnings(PsfPsyQAddresses addresses)
        {
            PropertyInfo psyQValue;
            string lineValue;
            StringBuilder warnings = new StringBuilder();
            
            foreach (string label in XsfUtil.GetPsyQFunctionList())            
            {
                psyQValue = addresses.GetType().GetProperty(label);
                lineValue = (string)psyQValue.GetValue(addresses, null);

                if (String.IsNullOrEmpty(lineValue))
                {
                    warnings.AppendFormat("    Warning: {0} function not found.{1}", label, Environment.NewLine);
                }
            }

            return warnings.ToString();
        }

        private void rewriteOptionalLine(StreamWriter writer, bool rewriteTheLine, 
            string formatString, string value, string originalLine)
        {
            if (rewriteTheLine)
            {
                writer.WriteLine(String.Format(formatString, value));
            }
            else
            {
                writer.WriteLine(originalLine);
            }
        }
    }
}
