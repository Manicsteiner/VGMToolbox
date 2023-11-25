using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.genh
{
    public struct GenhCreatorStruct : IVgmtWorkerStruct
    {
        public string Format { set; get; }
        public string HeaderSkip { set; get; }

        public string Interleave { set; get; }
        public bool UseInterleaveOffset { set; get; }
        public OffsetDescription InterleaveOffsetDescription { set; get; }

        public string Channels { set; get; }
        public bool UseChannelsOffset { set; get; }
        public OffsetDescription ChannelsOffsetDescription { set; get; }

        public string Frequency { set; get; }
        public bool UseFrequencyOffset { set; get; }
        public OffsetDescription FrequencyOffsetDescription { set; get; }

        public string LoopStart { set; get; }
        public bool UseLoopStartOffset { set; get; }
        public OffsetDescription LoopStartOffsetDescription { set; get; }
        public bool DoLoopStartBytesToSamples { set; get; }
        
        public string LoopEnd { set; get; }
        public bool UseLoopEndOffset { set; get; }
        public OffsetDescription LoopEndOffsetDescription { set; get; }
        public bool DoLoopEndBytesToSamples { set; get; }

        public bool NoLoops { set; get; }
        public bool UseFileEnd { set; get; }
        public bool FindLoop { set; get; }

        public string TotalSamples { set; get; }
        public bool UseTotalSamplesOffset { set; get; }
        public OffsetDescription TotalSamplesOffsetDescription { set; get; }
        public bool DoTotalSamplesBytesToSamples { set; get; }

        public byte SkipSamplesMode { set; get; }
        public string SkipSamples { set; get; }

        public byte Atrac3StereoMode { set; get; }
        public byte XmaStreamMode { set; get; }
        public string RawDataSize { set; get; }

        public string CoefRightChannel { set; get; }
        public string CoefLeftChannel { set; get; }
        public byte CoefficientType { set; get; }
        public bool OutputHeaderOnly { set; get; }
        public string[] SourcePaths { set; get; }
        public bool DoCreation { set; get; }
        public bool DoEdit { set; get; }
        public bool DoExtract { set; get; }

        public GenhCreationStruct ToGenhCreationStruct()
        {
            GenhCreationStruct genhCreationStruct = new GenhCreationStruct();

            genhCreationStruct.Format = this.Format;
            genhCreationStruct.HeaderSkip = this.HeaderSkip;

            genhCreationStruct.Interleave = this.Interleave;
            genhCreationStruct.UseInterleaveOffset = this.UseInterleaveOffset;
            genhCreationStruct.InterleaveOffsetDescription = this.InterleaveOffsetDescription;

            genhCreationStruct.Channels = this.Channels;
            genhCreationStruct.UseChannelsOffset = this.UseChannelsOffset;
            genhCreationStruct.ChannelsOffsetDescription = this.ChannelsOffsetDescription;

            genhCreationStruct.Frequency = this.Frequency;
            genhCreationStruct.UseFrequencyOffset = this.UseFrequencyOffset;
            genhCreationStruct.FrequencyOffsetDescription = this.FrequencyOffsetDescription;

            genhCreationStruct.LoopStart = this.LoopStart;
            genhCreationStruct.UseLoopStartOffset = this.UseLoopStartOffset;
            genhCreationStruct.LoopStartOffsetDescription = this.LoopStartOffsetDescription;
            genhCreationStruct.DoLoopStartBytesToSamples = this.DoLoopStartBytesToSamples;

            genhCreationStruct.LoopEnd = this.LoopEnd;
            genhCreationStruct.UseLoopEndOffset = this.UseLoopEndOffset;
            genhCreationStruct.LoopEndOffsetDescription = this.LoopEndOffsetDescription;
            genhCreationStruct.DoLoopEndBytesToSamples = this.DoLoopEndBytesToSamples;

            genhCreationStruct.NoLoops = this.NoLoops;
            genhCreationStruct.UseFileEnd = this.UseFileEnd;
            genhCreationStruct.FindLoop = this.FindLoop;

            genhCreationStruct.TotalSamples = this.TotalSamples;
            genhCreationStruct.UseTotalSamplesOffset = this.UseTotalSamplesOffset;
            genhCreationStruct.TotalSamplesOffsetDescription = this.TotalSamplesOffsetDescription;
            genhCreationStruct.DoTotalSamplesBytesToSamples = this.DoTotalSamplesBytesToSamples;

            genhCreationStruct.SkipSamples = this.SkipSamples;
            genhCreationStruct.SkipSamplesMode = this.SkipSamplesMode;

            genhCreationStruct.Atrac3StereoMode = this.Atrac3StereoMode;
            genhCreationStruct.XmaStreamMode = this.XmaStreamMode;
            genhCreationStruct.RawDataSize = this.RawDataSize;

            genhCreationStruct.CoefRightChannel = this.CoefRightChannel;
            genhCreationStruct.CoefLeftChannel = this.CoefLeftChannel;
            genhCreationStruct.CoefficientType = this.CoefficientType;
            genhCreationStruct.OutputHeaderOnly = this.OutputHeaderOnly;
            genhCreationStruct.SourcePaths = this.SourcePaths;

            return genhCreationStruct;
        }
    }
    
    public class GenhCreatorWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        private int fileCount;
        private int maxFiles;
        ProgressStruct progressStruct = new ProgressStruct();
        
        public GenhCreatorWorker()
        {
            this.progressStruct = new VGMToolbox.util.ProgressStruct();
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        private void createGenhs(GenhCreatorStruct pGenhCreatorStruct, DoWorkEventArgs e)
        {
            int progress;
            GenhCreationStruct genhCreationStruct;

            this.maxFiles = pGenhCreatorStruct.SourcePaths.Length;
            
            string outputFilePath = String.Empty;
            string extractedFilePath;
            string outputMessageAction = String.Empty;
            StringBuilder outputMessage = new StringBuilder();

            int progressReportingPercentage = 10;
            int progressReportingPercentageIncrementValue = 10;

            foreach (string file in pGenhCreatorStruct.SourcePaths)
            {
                progress = (++this.fileCount * 100) / this.maxFiles;
                
                // throttle output to prevent locking up the GUI
                if ((progress > progressReportingPercentage) ||
                    (this.fileCount == this.maxFiles))
                {
                    this.progressStruct.Clear();
                    this.progressStruct.FileName = file;
                    ReportProgress(progress, this.progressStruct);
                }

                if (File.Exists(file))
                {
                    if (pGenhCreatorStruct.DoExtract)
                    {
                        outputFilePath = GenhUtil.ExtractGenhFile(file, true, true, true);
                        outputMessageAction = "Extracted";
                    }
                    else
                    {
                        genhCreationStruct = pGenhCreatorStruct.ToGenhCreationStruct();

                        if (pGenhCreatorStruct.DoCreation)
                        {
                            outputFilePath = GenhUtil.CreateGenhFile(file, genhCreationStruct);
                            outputMessageAction = "Created";
                        }
                        else if (pGenhCreatorStruct.DoEdit)
                        {
                            extractedFilePath = GenhUtil.ExtractGenhFile(file, false, false, false);

                            if (!String.IsNullOrEmpty(extractedFilePath))
                            {
                                genhCreationStruct.SourcePaths = new string[1];
                                genhCreationStruct.SourcePaths[0] = extractedFilePath;

                                outputFilePath = GenhUtil.CreateGenhFile(extractedFilePath, genhCreationStruct);

                                File.Delete(extractedFilePath);
                            }
                            
                            outputMessageAction = "Edited";
                        }
                    }

                    if (!String.IsNullOrEmpty(outputFilePath))
                    {
                        outputMessage.AppendFormat("{0} {1}.{2}", outputFilePath, outputMessageAction, Environment.NewLine);                        
                    }

                    // throttle output to prevent locking up the GUI
                    if ((progress > progressReportingPercentage) || 
                        (this.fileCount == this.maxFiles))
                    {
                        progressReportingPercentage += progressReportingPercentageIncrementValue;
                        
                        this.progressStruct.Clear();
                        this.progressStruct.GenericMessage = outputMessage.ToString();
                        ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                        outputMessage.Length = 0;
                    }
                }
            }
        }
        
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            GenhCreatorStruct genhCreatorStruct = (GenhCreatorStruct)e.Argument;
            this.createGenhs(genhCreatorStruct, e);
        }
    }
}
