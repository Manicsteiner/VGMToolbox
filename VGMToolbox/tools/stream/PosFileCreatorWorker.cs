using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.stream
{
    public class PosFileCreatorWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        private const long DEFAULT_LOOP_VALUE = -99887766;

        public const string FILEMASK_BASE_NAME = "$B";
        public const string FILEMASK_EXTENSION = "$E";

        public const string POS_FILE_EXTENSION = ".pos";

        public struct PosFileCreatorStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }
            public string OutputFolder { set; get; }
            public string OutputFileMask { set; get; }

            // Loop Start
            public bool DoLoopStartStatic { set; get; }
            public bool DoLoopStartOffset { set; get; }
            public bool DoLoopStartRiffOffset { set; get; }
            public bool DoLoopStartByteStringOffset { set; get; }

            public string LoopStartStaticValue { set; get; }
            public CalculatingOffsetDescription LoopStartCalculatingOffset { set; get; }
            public RiffCalculatingOffsetDescription LoopStartRiffCalculatingOffset { set; get; }
            public ByteSearchCalculatingOffsetDescription LoopStartByteSearchCalculatingOffset { set; get; }

            // Loop End
            public bool LoopEndIsLoopEnd { set; get; }
            public bool LoopEndIsLoopLength { set; get; }
            
            public bool DoLoopEndStatic { set; get; }
            public bool DoLoopEndOffset { set; get; }
            public bool DoLoopEndRiffOffset { set; get; }
            public bool DoLoopEndByteStringOffset { set; get; }

            public string LoopEndStaticValue { set; get; }
            public CalculatingOffsetDescription LoopEndCalculatingOffset { set; get; }
            public RiffCalculatingOffsetDescription LoopEndRiffCalculatingOffset { set; get; }
            public ByteSearchCalculatingOffsetDescription LoopEndByteSearchCalculatingOffset { set; get; }

            // Loop Shift
            public bool DoStaticLoopShift { set; get; }
            public bool DoLoopShiftWavCompare { set; get; }
            public bool PredictLoopShiftForBatch { set; get; }
            public string StaticLoopShiftValue { set; get; }

            // Other
            public bool CreateM3u { set; get; }

        }

        public struct WorkingItemStruct
        {
            public long LoopStart { set; get; }
            public long LoopEnd { set; get; }

            public long LoopShift { set; get; }
            public long WavSamples { set; get; }
            public long SampleDifference { set; get; }

            public void Initialize()
            {
                this.LoopStart = DEFAULT_LOOP_VALUE;
                this.LoopEnd = DEFAULT_LOOP_VALUE;

                this.LoopShift = DEFAULT_LOOP_VALUE;
                this.WavSamples = DEFAULT_LOOP_VALUE;
                this.SampleDifference = DEFAULT_LOOP_VALUE;
            }
        }



        public PosFileCreatorWorker() : base() { }

        private Dictionary<string, WorkingItemStruct> WorkingList = new Dictionary<string, WorkingItemStruct>();

        // DoTaskForFile
        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pPosFileCreatorStruct,
            DoWorkEventArgs e)
        { 
            PosFileCreatorStruct posStruct = (PosFileCreatorStruct) pPosFileCreatorStruct;

            WorkingItemStruct currentItem = new WorkingItemStruct();

            long loopStartValue = DEFAULT_LOOP_VALUE;
            long loopEndValue = DEFAULT_LOOP_VALUE;

            string outputFileMask;
            string[] outputFileList;

            outputFileMask = posStruct.OutputFileMask.Replace(FILEMASK_BASE_NAME, Path.GetFileNameWithoutExtension(pPath));
            outputFileMask = outputFileMask.Replace(FILEMASK_EXTENSION, Path.GetExtension(pPath).Remove(0, 1));

            #region LOOP POINTS
            using (FileStream fs = File.OpenRead(pPath))
            {
                currentItem.Initialize();
                
                try
                {
                    //----------------
                    // Get Loop Start
                    //----------------
                    if (posStruct.DoLoopStartStatic)
                    {
                        loopStartValue = ByteConversion.GetLongValueFromString(posStruct.LoopStartStaticValue);
                    }
                    else if (posStruct.DoLoopStartOffset)
                    {
                        loopStartValue = ParseFile.GetCalculatedVaryingByteValueAtAbsoluteOffset(fs, posStruct.LoopStartCalculatingOffset, true);
                    }
                    else if (posStruct.DoLoopStartRiffOffset)
                    {
                        try
                        {
                            loopStartValue = ParseFile.GetRiffCalculatedVaryingByteValueAtAbsoluteOffset(fs, posStruct.LoopStartRiffCalculatingOffset, true);
                        }
                        catch (IndexOutOfRangeException iorEx)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.GenericMessage = String.Format("Error processing RIFF Loop Start for <{0}>: {1}{2}", Path.GetFileName(pPath), iorEx.Message, Environment.NewLine);
                            ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
                        }
                    }
                    else if (posStruct.DoLoopStartByteStringOffset)
                    {
                        loopStartValue = ParseFile.GetByteSearchCalculatedVaryingByteValueAtAbsoluteOffset(fs, posStruct.LoopStartByteSearchCalculatingOffset, true);
                    }

                    if ((loopStartValue != DEFAULT_LOOP_VALUE) && ((int)loopStartValue >= 0))
                    {
                        //----------------
                        // Get Loop End
                        //----------------
                        if (posStruct.DoLoopEndStatic)
                        {
                            loopEndValue = ByteConversion.GetLongValueFromString(posStruct.LoopEndStaticValue);
                        }
                        else if (posStruct.DoLoopEndOffset)
                        {
                            loopEndValue = ParseFile.GetCalculatedVaryingByteValueAtAbsoluteOffset(fs, posStruct.LoopEndCalculatingOffset, true);
                        }
                        else if (posStruct.DoLoopEndRiffOffset)
                        {
                            try
                            {
                                loopEndValue = ParseFile.GetRiffCalculatedVaryingByteValueAtAbsoluteOffset(fs, posStruct.LoopEndRiffCalculatingOffset, true);
                            }
                            catch (IndexOutOfRangeException iorEx)
                            {
                                this.progressStruct.Clear();
                                this.progressStruct.GenericMessage = String.Format("Error processing RIFF Loop End for <{0}>: {1}{2}", Path.GetFileName(pPath), iorEx.Message, Environment.NewLine);
                                ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
                            }
                        }
                        else if (posStruct.DoLoopEndByteStringOffset)
                        {
                            loopEndValue = ParseFile.GetByteSearchCalculatedVaryingByteValueAtAbsoluteOffset(fs, posStruct.LoopEndByteSearchCalculatingOffset, true);
                        }

                        if ((loopEndValue != DEFAULT_LOOP_VALUE) && (loopEndValue >= 0))
                        {                                                        
                            // Calculate Loop End if Needed
                            if (posStruct.LoopEndIsLoopLength)
                            {
                                loopEndValue += loopStartValue;
                            }

                            if (loopStartValue < loopEndValue)
                            {
                                // update working item
                                currentItem.LoopStart = loopStartValue;
                                currentItem.LoopEnd = loopEndValue;
                            }
                            else
                            {
                                throw new Exception(String.Format("Loop Start Value greater than or equal Loop End Value: {0}", pPath));
                            }
                        }
                        else
                        {
                            throw new IndexOutOfRangeException(String.Format("Loop End Value not Found or Loop End is Less than 0: {0}", pPath));
                        }

                    }
                    else
                    {
                        throw new IndexOutOfRangeException(String.Format("Loop Start Value not Found or Loop Start is Less than 0: {0}", pPath));
                    }
                }
                catch (Exception ex)
                {                                        
                    this.progressStruct.Clear();
                    this.progressStruct.GenericMessage = String.Format("Cannot find loop points for <{0}>: {1}{2}", Path.GetFileName(pPath), ex.Message, Environment.NewLine);
                    ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);  
                }
            }
            #endregion

            #region LOOP SHIFT

            // get list of matching WAV files
            outputFileList = Directory.GetFiles(Path.GetDirectoryName(pPath), outputFileMask, SearchOption.TopDirectoryOnly);
            
            // loop over list and add each item to the dictionary
            foreach (string outputFile in outputFileList)
            {
                // only shift valid values
                if ((loopStartValue != DEFAULT_LOOP_VALUE) && (loopEndValue != DEFAULT_LOOP_VALUE))
                {
                    // Static Loop Shift
                    if (posStruct.DoStaticLoopShift)
                    {
                        // update shift
                        currentItem.LoopShift = ByteConversion.GetLongValueFromString(posStruct.StaticLoopShiftValue);
                    }
                    else if (posStruct.DoLoopShiftWavCompare) // Wav Compare
                    {
                        // get samplecount for this file
                        currentItem.WavSamples = GetSampleCountForRiffHeaderedFile(outputFile);
                        currentItem.SampleDifference = currentItem.WavSamples - currentItem.LoopEnd;

                        // update shift
                        currentItem.LoopShift = currentItem.SampleDifference;
                    }
                }
                
                this.WorkingList.Add(outputFile, currentItem);
            }            
            
            #endregion

        }

        protected override void  DoFinalTask(IVgmtWorkerStruct pTaskStruct)
        {
            PosFileCreatorStruct posStruct = (PosFileCreatorStruct)pTaskStruct;

            long predictedShift = DEFAULT_LOOP_VALUE;
            long predictedShiftCount = 1;

            Dictionary<long, long> shiftCount = new Dictionary<long, long>();
            long loopShift = DEFAULT_LOOP_VALUE;

            ArrayList keyList = new ArrayList();
            
            StringBuilder m3uString = new StringBuilder();
            string m3uPath;
            string posFileName;

            StringBuilder outputMessage = new StringBuilder();


            // predict shift
            foreach (string s in this.WorkingList.Keys)
            {
                // increment count for this shift value
                if (WorkingList[s].LoopShift != DEFAULT_LOOP_VALUE)
                {
                    if (shiftCount.ContainsKey(WorkingList[s].LoopShift))
                    {
                        shiftCount[WorkingList[s].LoopShift]++;
                    }
                    else
                    {
                        shiftCount.Add(WorkingList[s].LoopShift, 1);
                    }
                }

                // add to key list for use later
                keyList.Add(s);
            }

            // determine highest valued shift
            foreach (long l in shiftCount.Keys)
            {
                if (shiftCount[l] > predictedShiftCount)
                {
                    predictedShift = l;
                    predictedShiftCount = shiftCount[l];
                }
            }

            // sort keys
            keyList.Sort();

            // loop over keys
            foreach (string s in keyList)
            {
                // output info
                outputMessage.Length = 0;
                outputMessage.AppendFormat("[{0}]{1}", Path.GetFileName(s), Environment.NewLine);
                
                // create m3u and output pos file
                if ((this.WorkingList[s].LoopStart != DEFAULT_LOOP_VALUE) &&
                    (this.WorkingList[s].LoopEnd != DEFAULT_LOOP_VALUE))
                {
                    // output info
                    outputMessage.AppendFormat("  Loop Start: 0x{0}{1}", this.WorkingList[s].LoopStart.ToString("X8"), Environment.NewLine);
                    outputMessage.AppendFormat("  Loop End: 0x{0}{1}", this.WorkingList[s].LoopEnd.ToString("X8"), Environment.NewLine);
                    
                    posFileName = Path.ChangeExtension(s, POS_FILE_EXTENSION);
                    m3uString.AppendFormat("{0}{1}", Path.GetFileName(posFileName), Environment.NewLine);

                    // set loop shift
                    if (posStruct.PredictLoopShiftForBatch && (predictedShiftCount > (long)(this.WorkingList.Count * .1))) // greater than 10%
                    {
                        loopShift = predictedShift;

                        // output info
                        if (predictedShift == this.WorkingList[s].LoopShift)
                        {
                            outputMessage.AppendFormat("  Loop Shift (predicted): {0}{1}", loopShift.ToString("D"), Environment.NewLine);
                        }
                        else
                        {
                            outputMessage.AppendFormat("  * Loop Shift (predicted): {0} (Calculated: {1}){2}", loopShift.ToString("D"), this.WorkingList[s].LoopShift.ToString("D"), Environment.NewLine);
                        }
                    }
                    else if (this.WorkingList[s].LoopShift != DEFAULT_LOOP_VALUE)
                    {
                        loopShift = this.WorkingList[s].LoopShift;

                        //output info
                        outputMessage.AppendFormat("  Loop Shift: {0}{1}", loopShift.ToString("D"), Environment.NewLine);
                    }

                    // write pos file
                    using (FileStream posStream = File.Open(posFileName, FileMode.Create, FileAccess.Write))
                    {
                        byte[] loopPointBytes = new byte[4];

                        loopPointBytes = BitConverter.GetBytes((uint)(this.WorkingList[s].LoopStart + loopShift));
                        posStream.Write(loopPointBytes, 0, 4);

                        loopPointBytes = BitConverter.GetBytes((uint)(this.WorkingList[s].LoopEnd + loopShift));
                        posStream.Write(loopPointBytes, 0, 4);

                        // output info
                        outputMessage.AppendFormat("  POS File Written: {0}{1}", Path.GetFileName(posFileName), Environment.NewLine);
                    }

                }
                else
                {
                    m3uString.AppendFormat("{0}{1}", Path.GetFileName(s), Environment.NewLine);

                    // output info
                    outputMessage.AppendFormat("  Looping info not found.{0}", Environment.NewLine);
                }

                // Output Message
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = outputMessage.ToString();
                ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);  
            }
            
            // output m3u
            if (posStruct.CreateM3u)
            {
                m3uPath = Path.Combine(Path.GetDirectoryName(posStruct.SourcePaths[0]), "!BGM.m3u");
                
                using (StreamWriter m3uWriter = new StreamWriter(File.Open(m3uPath, FileMode.Create, FileAccess.Write)))
                {
                    m3uWriter.Write(m3uString.ToString());
                }
            }
        }

        public static long GetSampleCountForRiffHeaderedFile(string path)
        {
            long sampleCount = -1;

            ushort channelCount;
            uint dataSize;

            RiffCalculatingOffsetDescription riffValue = new RiffCalculatingOffsetDescription();

            // get values from file
            using (FileStream fs = File.OpenRead(path))
            {
                // over kill, but i don't feel like recoding it
                riffValue.CalculationString = String.Empty;
                riffValue.OffsetByteOrder = Constants.LittleEndianByteOrder;
                riffValue.OffsetSize = "2";
                riffValue.OffsetValue = "10";
                riffValue.RelativeLocationToRiffChunkString = RiffCalculatingOffsetDescription.START_OF_STRING;
                riffValue.RiffChunkString = "fmt ";

                channelCount = (ushort)ParseFile.GetRiffCalculatedVaryingByteValueAtAbsoluteOffset(fs, riffValue, true);

                riffValue.CalculationString = String.Empty;
                riffValue.OffsetByteOrder = Constants.LittleEndianByteOrder;
                riffValue.OffsetSize = "4";
                riffValue.OffsetValue = "4";
                riffValue.RelativeLocationToRiffChunkString = RiffCalculatingOffsetDescription.START_OF_STRING;
                riffValue.RiffChunkString = "data";

                dataSize = (uint)ParseFile.GetRiffCalculatedVaryingByteValueAtAbsoluteOffset(fs, riffValue, true);

                sampleCount = (long)dataSize / 2 / (long)channelCount;

            } // using (FileStream fs = File.OpenRead(path))
                        
            return sampleCount;
        }
    }
}
