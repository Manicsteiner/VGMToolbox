using System;
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
    public class XmaConverterWorker: AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        #region CONSTANTS

        // xma parse
        public const string XMAPARSE_PATH = "external\\xma\\xma_test.exe";
        public const string XMAPARSE_CRC32 = "7437EE24"; // v0.8
        public static readonly string XMAPARSE_FULL_PATH =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            XMAPARSE_PATH);

        // to wav
        public const string TOWAV_PATH = "external\\xma\\ToWav.exe";
        public static readonly string TOWAV_FULL_PATH =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            TOWAV_PATH);

        // xmaencode
        public const string XMAENCODE_PATH = "external\\xma\\xmaencode.exe";
        public static readonly string XMAENCODE_FULL_PATH =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            XMAENCODE_PATH);

        // paths
        private const string WORKING_FOLDER_NAME = "_vgmt_xma_convert";

        private const string XMAPARSE_OUTPUT_EXTENSION = ".xmpo";
        private const string RIFF_COPY_OUTPUT_EXTENSION = ".xmariff";
        private const string TOWAV_OUTPUT_EXTENSION = ".wav";
        private const string XMAENCODE_OUTPUT_EXTENSION = ".wav";

        // combo items
        public const string RIFF_CHANNELS_1 = "Mono (1)";
        public const string RIFF_CHANNELS_2 = "Stereo (2+)";

        // RIFF header
        public static readonly byte[] RIFF_HEADER_XMA = new byte[] { 
            0x52, 0x49, 0x46, 0x46, 0xB8, 0x59, 0xA7, 0x00, 0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20,
            0x20, 0x00, 0x00, 0x00, 0x65, 0x01, 0x10, 0x00, 0xD6, 0x10, 0x00, 0x00, 0x01, 0x00, 0x00, 0x03, 
            0xE3, 0x9A, 0x00, 0x00, 0x80, 0xBB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x02, 0x02, 0x00, 0x64, 0x61, 0x74, 0x61, 0x00, 0x58, 0xA7, 0x00};

        #endregion

        public struct XmaConverterStruct : IVgmtWorkerStruct
        {            
            public string[] SourcePaths { set; get; }
            public string OutputFolder { set; get; }

            public int NumberOfStreams { set; get; }

            public bool DoXmaParse { set; get; }
            public bool DoRiffHeader { set; get; }
            public bool DoToWav { set; get; }
            public bool DoPosMaker { set; get; }
            public bool DoXmaEncode { set; get; }

            public string XmaParseXmaType { set; get; }
            public bool XmaParseDoRebuildMode { set; get; }
            public bool XmaParseIgnoreErrors { set; get; }

            public bool XmaParseStartOffsetIsStatic { set; get; }
            public string XmaParseStartOffset { set; get; }
            public OffsetDescription XmaParseStartOffsetOffsetInfo { set; get; }
            public bool StartOffsetAfterRiffHeader { set; get; }

            public bool XmaParseBlockSizeIsStatic { set; get; }
            public string XmaParseBlockSize { set; get; }
            public OffsetDescription XmaParseBlockSizeOffsetInfo { set; get; }

            public bool XmaParseDataSizeIsStatic { set; get; }
            public string XmaParseDataSize { set; get; }
            public OffsetDescription XmaParseDataSizeOffsetInfo { set; get; }
            public bool GetDataSizeFromRiffHeader { set; get; }

            public string RiffFrequency { set; get; }
            public bool GetFrequencyFromOffset { set; get; }
            public OffsetDescription RiffHeaderFrequencyOffsetInfo { set; get; }
            public bool GetFrequencyFromRiffHeader { set; get; }

            public string RiffChannelCount { set; get; }
            public bool GetChannelsFromOffset { set; get; }
            public OffsetDescription RiffHeaderChannelOffsetInfo { set; get; }            
            public bool GetChannelsFromRiffHeader { set; get; }
            
            public bool PosLoopStartIsStatic { set; get; }
            public string PosLoopStartStaticValue { set; get; }
            public CalculatingOffsetDescription PosLoopStartOffsetInfo { set; get; }

            public bool PosLoopEndIsStatic { set; get; }
            public string PosLoopEndStaticValue { set; get; }
            public CalculatingOffsetDescription PosLoopEndOffsetInfo { set; get; }

            public bool ShowExeOutput { set; get; }
            public bool KeepIntermediateFiles { set; get; }
        }

        public XmaConverterWorker() : 
            base() 
        {
            this.progressCounterIncrementer = 10;
        }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pXmaConverterStruct,
            DoWorkEventArgs e)
        {
            XmaConverterStruct taskStruct = (XmaConverterStruct)pXmaConverterStruct;
            string workingFolder;
            string workingFile;
            string workingSourceFile;

            string consoleOutput = String.Empty;
            string consoleError = String.Empty;

            uint riffFrequency;
            uint riffChannelCount;
            uint riffFileSize;

            string riffHeaderedFile;
            byte[] riffHeaderBytes;

            uint loopStart;
            uint loopLength;
            uint loopEnd;
            byte[] posBytes;

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

            taskStruct.NumberOfStreams = 1;

            for (int i = 0; i < taskStruct.NumberOfStreams; i++)
            {
                // set working file
                workingFile = workingSourceFile;

                #region XMAPARSE
                //---------------------------
                // xma_parse.exe
                //---------------------------            
                if (taskStruct.DoXmaParse)
                {
                    this.ShowOutput(pPath, "---- calling xma_parse.exe", false);

                    // call xma_parse and set output as working_file for next step
                    workingFile = this.callXmaParse(workingFolder, workingFile, taskStruct, out consoleOutput, out consoleError);

                    // show output
                    if (taskStruct.ShowExeOutput && !String.IsNullOrEmpty(consoleOutput))
                    {
                        this.ShowOutput(pPath, consoleOutput, false);
                    }

                    // clear errors if ignore is selected, but only if a file was created
                    if (taskStruct.XmaParseIgnoreErrors && (File.Exists(workingFile)))
                    {
                        consoleError = String.Empty;
                    }
                } // 
                #endregion

                #region RIFF HEADER
                if ((taskStruct.DoRiffHeader) && (String.IsNullOrEmpty(consoleError)))
                {
                    //-----------------
                    // get RIFF header
                    //-----------------
                    this.ShowOutput(pPath, "---- adding RIFF header.", false);

                    // frequency
                    if (taskStruct.GetFrequencyFromRiffHeader)
                    {
                        riffFrequency = (uint)RiffUtil.GetFrequencyFromRiffHeader(workingSourceFile);
                    }
                    else if (taskStruct.GetFrequencyFromOffset)
                    {
                        using (FileStream fs = File.OpenRead(workingSourceFile))
                        {
                            riffFrequency = (uint)ParseFile.GetVaryingByteValueAtAbsoluteOffset(fs, taskStruct.RiffHeaderFrequencyOffsetInfo, true);
                        }
                    }
                    else
                    {
                        riffFrequency = (uint)ByteConversion.GetLongValueFromString(taskStruct.RiffFrequency);
                    }

                    // channels
                    if (taskStruct.GetChannelsFromRiffHeader)
                    {
                        riffChannelCount = RiffUtil.GetChannelsFromRiffHeader(workingSourceFile);
                    }
                    else if (taskStruct.GetChannelsFromOffset)
                    {
                        using (FileStream fs = File.OpenRead(workingSourceFile))
                        {
                            riffChannelCount = (uint)ParseFile.GetVaryingByteValueAtAbsoluteOffset(fs, taskStruct.RiffHeaderChannelOffsetInfo, true);

                            if (riffChannelCount > 2)
                            {
                                riffChannelCount = 2;
                            }
                        }
                    }
                    else
                    {
                        riffChannelCount = (uint)ByteConversion.GetLongValueFromString(taskStruct.RiffChannelCount);
                    }

                    riffFileSize = (uint)new FileInfo(workingFile).Length;

                    riffHeaderBytes = this.getRiffHeader(riffFrequency, riffChannelCount, riffFileSize);

                    //-------------------------
                    // add RIFF header to file
                    //-------------------------
                    riffHeaderedFile = Path.ChangeExtension(workingFile, RIFF_COPY_OUTPUT_EXTENSION);
                    FileUtil.AddHeaderToFile(riffHeaderBytes, workingFile, riffHeaderedFile);

                    // set working file for next
                    workingFile = riffHeaderedFile;
                }
                else if (!String.IsNullOrEmpty(consoleError))
                {
                    // dispay xma_parse.exe error
                    this.ShowOutput(pPath, consoleError, true);
                }
                #endregion

                #region POS CREATOR
                //-----------
                // POS Maker
                //-----------
                if ((taskStruct.DoPosMaker) && (String.IsNullOrEmpty(consoleError)))
                {
                    this.ShowOutput(pPath, "---- creating POS file", false);

                    // loop start
                    if (taskStruct.PosLoopStartIsStatic)
                    {
                        loopStart = (uint)ByteConversion.GetLongValueFromString(taskStruct.PosLoopStartStaticValue);
                    }
                    else
                    {
                        using (FileStream fs = File.OpenRead(workingSourceFile))
                        {
                            loopStart = (uint)ParseFile.GetVaryingByteValueAtAbsoluteOffset(fs, taskStruct.PosLoopStartOffsetInfo, true);

                            if (!String.IsNullOrEmpty(taskStruct.PosLoopStartOffsetInfo.CalculationString))
                            {
                                string calculationString =
                                    taskStruct.PosLoopStartOffsetInfo.CalculationString.Replace(CalculatingOffsetDescription.OFFSET_VARIABLE_STRING, loopStart.ToString());

                                loopStart = (uint)ByteConversion.GetLongValueFromString(MathUtil.Evaluate(calculationString));
                            }
                        }
                    }

                    // loop length
                    if (taskStruct.PosLoopEndIsStatic)
                    {
                        loopLength = (uint)ByteConversion.GetLongValueFromString(taskStruct.PosLoopEndStaticValue);
                    }
                    else
                    {
                        using (FileStream fs = File.OpenRead(workingSourceFile))
                        {
                            loopLength = (uint)ParseFile.GetVaryingByteValueAtAbsoluteOffset(fs, taskStruct.PosLoopEndOffsetInfo, true);

                            if (!String.IsNullOrEmpty(taskStruct.PosLoopEndOffsetInfo.CalculationString))
                            {
                                string calculationString =
                                    taskStruct.PosLoopEndOffsetInfo.CalculationString.Replace(CalculatingOffsetDescription.OFFSET_VARIABLE_STRING, loopLength.ToString());

                                loopLength = (uint)ByteConversion.GetLongValueFromString(MathUtil.Evaluate(calculationString));
                            }
                        }
                    }

                    if (loopLength > 0)
                    {
                        loopEnd = loopStart + loopLength;

                        // build .pos array and write to file
                        posBytes = new byte[8];
                        Array.Copy(BitConverter.GetBytes(loopStart), 0, posBytes, 0, 4);
                        Array.Copy(BitConverter.GetBytes(loopEnd), 0, posBytes, 4, 4);

                        using (FileStream fs = File.Open(Path.ChangeExtension(workingSourceFile, ".pos"), FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(posBytes, 0, posBytes.Length);
                        }
                    }
                }
                #endregion

                #region TOWAV
                //-----------
                // ToWav.exe
                //-----------
                if ((taskStruct.DoToWav) && (String.IsNullOrEmpty(consoleError)))
                {
                    this.ShowOutput(pPath, "---- calling ToWav.exe", false);

                    // call ToWav.exe and set working file for next step (if ever needed)
                    workingFile = this.callToWav(workingFolder, workingFile, taskStruct, out consoleOutput, out consoleError);

                    // show output
                    if (taskStruct.ShowExeOutput && !String.IsNullOrEmpty(consoleOutput))
                    {
                        this.ShowOutput(pPath, consoleOutput, false);
                    }

                    // dispay ToWav.exe error
                    if (!String.IsNullOrEmpty(consoleError))
                    {
                        this.ShowOutput(pPath, consoleError, true);
                    }
                }
                #endregion

                #region XMAENCODE
                //---------------
                // XmaEncode.exe
                //---------------
                else if ((taskStruct.DoXmaEncode) && (String.IsNullOrEmpty(consoleError)))
                {
                    this.ShowOutput(pPath, "---- calling xmaencode.exe", false);

                    // call xmaencode.exe and set working file for next step (if ever needed)
                    workingFile = this.callXmaEncode(workingFolder, workingFile, taskStruct, out consoleOutput, out consoleError);

                    // show output
                    if (taskStruct.ShowExeOutput && !String.IsNullOrEmpty(consoleOutput))
                    {
                        this.ShowOutput(pPath, consoleOutput, false);
                    }

                    // dispay xmaencode.exe error
                    if (!String.IsNullOrEmpty(consoleError))
                    {
                        this.ShowOutput(pPath, consoleError, true);
                    }
                }
                #endregion
            } // for (int i = 0; i < taskStruct.NumberOfStreams; i++)
                

            //----------------------
            // clean working folder
            //----------------------
            this.cleanWorkingFolder(pPath, workingSourceFile, taskStruct);
           
        }

        //--------------------------
        // Working Folder functions
        //--------------------------
        private string getWorkingFolderPath(string processingFilePath, XmaConverterStruct taskStruct)
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
        
        private string createWorkingFolder(string processingFilePath, XmaConverterStruct taskStruct)
        {
            string workingFolder = getWorkingFolderPath(processingFilePath, taskStruct);
            
            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder);
            }

            return workingFolder;
        }

        private void cleanWorkingFolder(string processingFilePath, string workingFile, XmaConverterStruct taskStruct)
        {
            string workingFolder = getWorkingFolderPath(processingFilePath, taskStruct);

            // delete working copy of source file
            if (File.Exists(workingFile))
            {
                // extensions are corrected in each function
                if ((taskStruct.DoXmaEncode && !Path.GetExtension(workingFile).ToLower().Equals(XMAENCODE_OUTPUT_EXTENSION)) ||
                    (taskStruct.DoToWav && !Path.GetExtension(workingFile).ToLower().Equals(TOWAV_OUTPUT_EXTENSION)))
                {
                    File.Delete(workingFile);
                }
            }

            //---------------------------
            // delete intermediate files
            //---------------------------
            if (!taskStruct.KeepIntermediateFiles)
            {
                // delete xma_parse output file(s)
                foreach (string xmpo in Directory.GetFiles(workingFolder, String.Format("*{0}", XMAPARSE_OUTPUT_EXTENSION)))
                {
                    File.Delete(xmpo);
                }

                // delete riff output file(s)
                foreach (string riff in Directory.GetFiles(workingFolder, String.Format("*{0}", RIFF_COPY_OUTPUT_EXTENSION)))
                {
                    File.Delete(riff);
                }
            }
            
            // delete xma_test.exe
            string xmaParseWorkingPath = Path.Combine(workingFolder, Path.GetFileName(XMAPARSE_FULL_PATH));

            if (File.Exists(xmaParseWorkingPath))
            {
                File.Delete(xmaParseWorkingPath);
            }

            // delete towav.exe
            string toWavWorkingPath = Path.Combine(workingFolder, Path.GetFileName(TOWAV_FULL_PATH));

            if (File.Exists(toWavWorkingPath))
            {
                File.Delete(toWavWorkingPath);
            }

            // delete xmaencode.exe
            string xmaencodeWorkingPath = Path.Combine(workingFolder, Path.GetFileName(XMAENCODE_FULL_PATH));

            if (File.Exists(xmaencodeWorkingPath))
            {
                File.Delete(xmaencodeWorkingPath);
            }
        }        

        //---------------
        // External Apps
        //---------------
        private string callXmaParse(
            string workingFolder, 
            string workingFile, 
            XmaConverterStruct taskStruct,
            out string consoleOutput, 
            out string consoleError)
        {
            string xmaParseWorkingPath;
            string xmaParseOutputFilePath;
            Process xmaParseProcess;
            StringBuilder parameters = new StringBuilder();

            // copy to working folder
            xmaParseWorkingPath = Path.Combine(workingFolder, Path.GetFileName(XMAPARSE_FULL_PATH));
            File.Copy(XMAPARSE_FULL_PATH, xmaParseWorkingPath, true);

            // build parameters
            parameters.AppendFormat(" \"{0}\"", Path.GetFileName(workingFile)); // Filename
            parameters.AppendFormat(" -{0}", taskStruct.XmaParseXmaType); // Input File Type

            using (FileStream workingFileStream = File.OpenRead(workingFile))
            {
                // offset
                if (taskStruct.StartOffsetAfterRiffHeader)
                {
                    uint riffHeaderSize = RiffUtil.GetRiffHeaderSize(workingFile);
                    parameters.AppendFormat(" -o {0}", riffHeaderSize.ToString("X"));
                }
                else if ((taskStruct.XmaParseStartOffsetIsStatic) && (!String.IsNullOrEmpty(taskStruct.XmaParseStartOffset)))
                {
                    // allow decimal or hex input, convert to hex for xma_parse.exe
                    parameters.AppendFormat(" -o {0}", ByteConversion.GetLongValueFromString(taskStruct.XmaParseStartOffset).ToString("X"));
                }
                else if (!String.IsNullOrEmpty(taskStruct.XmaParseStartOffsetOffsetInfo.OffsetValue))
                {
                    long offsetValue = ParseFile.GetVaryingByteValueAtAbsoluteOffset(workingFileStream, taskStruct.XmaParseStartOffsetOffsetInfo, true);
                    parameters.AppendFormat(" -o {0}", offsetValue.ToString("X"));
                }

                // block size
                if ((taskStruct.XmaParseBlockSizeIsStatic) && (!String.IsNullOrEmpty(taskStruct.XmaParseBlockSize)))
                {
                    // allow decimal or hex input, convert to hex for xma_parse.exe
                    parameters.AppendFormat(" -b {0}", ByteConversion.GetLongValueFromString(taskStruct.XmaParseBlockSize).ToString("X"));
                }
                else if (!String.IsNullOrEmpty(taskStruct.XmaParseBlockSizeOffsetInfo.OffsetValue))
                {
                    long blockSizeValue = ParseFile.GetVaryingByteValueAtAbsoluteOffset(workingFileStream, taskStruct.XmaParseBlockSizeOffsetInfo, true);
                    parameters.AppendFormat(" -b {0}", blockSizeValue.ToString("X"));
                }

                // data size
                if (taskStruct.GetDataSizeFromRiffHeader)
                {
                    uint riffDataSize = RiffUtil.GetDataSizeFromRiffHeader(workingFile);
                    parameters.AppendFormat(" -d {0}", riffDataSize.ToString("X"));
                }
                else if ((taskStruct.XmaParseDataSizeIsStatic) && (!String.IsNullOrEmpty(taskStruct.XmaParseDataSize)))
                {
                    // allow decimal or hex input, convert to hex for xma_parse.exe
                    parameters.AppendFormat(" -d {0}", ByteConversion.GetLongValueFromString(taskStruct.XmaParseDataSize).ToString("X"));
                }
                else if (!String.IsNullOrEmpty(taskStruct.XmaParseDataSizeOffsetInfo.OffsetValue))
                {
                    long dataSizeValue = ParseFile.GetVaryingByteValueAtAbsoluteOffset(workingFileStream, taskStruct.XmaParseDataSizeOffsetInfo, true);
                    parameters.AppendFormat(" -d {0}", dataSizeValue.ToString("X"));
                }
            }
            
            // output name
            xmaParseOutputFilePath = String.Format("{0}{1}", Path.GetFileNameWithoutExtension(workingFile), XMAPARSE_OUTPUT_EXTENSION);

            if (taskStruct.XmaParseDoRebuildMode)
            {
                parameters.AppendFormat(" -r \"{0}\"", xmaParseOutputFilePath);
            }
            else
            {
                parameters.AppendFormat(" -x \"{0}\"", xmaParseOutputFilePath);
            }

            // call function
            xmaParseProcess = new Process();
            xmaParseProcess.StartInfo = new ProcessStartInfo(xmaParseWorkingPath);
            xmaParseProcess.StartInfo.WorkingDirectory = workingFolder;
            xmaParseProcess.StartInfo.Arguments = parameters.ToString();
            xmaParseProcess.StartInfo.UseShellExecute = false;
            xmaParseProcess.StartInfo.CreateNoWindow = true;

            xmaParseProcess.StartInfo.RedirectStandardError = true;
            xmaParseProcess.StartInfo.RedirectStandardOutput = true;

            bool isSuccess = xmaParseProcess.Start();
            consoleOutput = xmaParseProcess.StandardOutput.ReadToEnd();
            consoleError = xmaParseProcess.StandardError.ReadToEnd();

            xmaParseProcess.WaitForExit();
            xmaParseProcess.Close();
            xmaParseProcess.Dispose();

            xmaParseOutputFilePath = xmaParseWorkingPath = Path.Combine(workingFolder, xmaParseOutputFilePath);

            return xmaParseOutputFilePath;
        }

        private string callToWav(
            string workingFolder,
            string riffHeaderedFile,
            XmaConverterStruct taskStruct,
            out string consoleOutput,
            out string consoleError)
        {
            string toWavWorkingPath;
            string toWavOutputFilePath;
            Process toWavProcess;
            StringBuilder parameters = new StringBuilder();

            // update extension in case other steps were skipped
            if (Path.GetExtension(riffHeaderedFile).ToLower().Equals(TOWAV_OUTPUT_EXTENSION))
            {
                File.Move(riffHeaderedFile, Path.ChangeExtension(riffHeaderedFile, RIFF_COPY_OUTPUT_EXTENSION));
                riffHeaderedFile = Path.ChangeExtension(riffHeaderedFile, RIFF_COPY_OUTPUT_EXTENSION);
            }

            // copy to working folder
            toWavWorkingPath = Path.Combine(workingFolder, Path.GetFileName(TOWAV_FULL_PATH));
            File.Copy(TOWAV_FULL_PATH, toWavWorkingPath, true);

            // build parameters
            parameters.AppendFormat(" \"{0}\"", Path.GetFileName(riffHeaderedFile)); // Filename

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
            consoleOutput = toWavProcess.StandardOutput.ReadToEnd();
            consoleError = toWavProcess.StandardError.ReadToEnd();

            toWavProcess.WaitForExit();
            toWavProcess.Close();
            toWavProcess.Dispose();

            // build output path
            toWavOutputFilePath = Path.ChangeExtension(riffHeaderedFile, TOWAV_OUTPUT_EXTENSION);

            return toWavOutputFilePath;
        }

        private string callXmaEncode(
            string workingFolder,
            string riffHeaderedFile,
            XmaConverterStruct taskStruct,
            out string consoleOutput,
            out string consoleError)
        {
            string exeWorkingPath;
            string exeOutputFilePath;
            string riffFileName;
            Process exeProcess;
            StringBuilder parameters = new StringBuilder();

            // update extension in case other steps were skipped
            if (Path.GetExtension(riffHeaderedFile).ToLower().Equals(XMAENCODE_OUTPUT_EXTENSION))
            {
                File.Move(riffHeaderedFile, Path.ChangeExtension(riffHeaderedFile, RIFF_COPY_OUTPUT_EXTENSION));
                riffHeaderedFile = Path.ChangeExtension(riffHeaderedFile, RIFF_COPY_OUTPUT_EXTENSION);
            }

            // copy to working folder
            exeWorkingPath = Path.Combine(workingFolder, Path.GetFileName(XMAENCODE_FULL_PATH));
            File.Copy(XMAENCODE_FULL_PATH, exeWorkingPath, true);

            // build parameters
            riffFileName = Path.GetFileName(riffHeaderedFile);
            parameters.AppendFormat(" /X \"{0}\" \"{1}\" /V", Path.ChangeExtension(riffFileName, XMAENCODE_OUTPUT_EXTENSION), riffFileName); // Filename

            // call function
            exeProcess = new Process();
            exeProcess.StartInfo = new ProcessStartInfo(exeWorkingPath);
            exeProcess.StartInfo.WorkingDirectory = workingFolder;
            exeProcess.StartInfo.Arguments = parameters.ToString();
            exeProcess.StartInfo.UseShellExecute = false;
            exeProcess.StartInfo.CreateNoWindow = true;

            exeProcess.StartInfo.RedirectStandardError = true;
            exeProcess.StartInfo.RedirectStandardOutput = true;

            bool isSuccess = exeProcess.Start();
            consoleOutput = exeProcess.StandardOutput.ReadToEnd();
            consoleError = exeProcess.StandardError.ReadToEnd();

            exeProcess.WaitForExit();
            exeProcess.Close();
            exeProcess.Dispose();

            // build output path
            exeOutputFilePath = Path.ChangeExtension(riffHeaderedFile, XMAENCODE_OUTPUT_EXTENSION);

            return exeOutputFilePath;
        }

        //----------------
        // RIFF functions
        //----------------
        private byte[] getRiffHeader(uint frequency, uint channelCount, uint fileSize)
        {
            byte[] riffHeader = new byte[RIFF_HEADER_XMA.Length];
            Array.Copy(RIFF_HEADER_XMA, riffHeader, RIFF_HEADER_XMA.Length);

            // file size
            Array.Copy(BitConverter.GetBytes(fileSize + 0x34), 0, riffHeader, 4, 4);

            // frequency
            Array.Copy(BitConverter.GetBytes(frequency), 0, riffHeader, 0x24, 4);

            // channels
            riffHeader[0x31] = (byte)channelCount;

            // file size
            Array.Copy(BitConverter.GetBytes(fileSize), 0, riffHeader, 0x38, 4);

            return riffHeader;
        }
    }
}
