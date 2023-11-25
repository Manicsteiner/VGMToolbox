using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.extract
{
    class ExtractOggWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct ExtractOggStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }
            public bool StopParsingOnFormatError { set; get; }
        }

        public ExtractOggWorker() :
            base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pTaskStruct, DoWorkEventArgs e)
        {
            ExtractOggStruct extractOggStruct = (ExtractOggStruct)pTaskStruct;

            long offset = 0;

            byte pageType;
            long pageSize;
            uint bitstreamSerialNumber;
            
            byte segmentCount;
            uint sizeOfAllSegments;
            byte i;

            string outputPath;
            string outputFileName;
            byte[] rawPageBytes;
            bool pageWrittenToFile = false;

            Dictionary<uint, FileStream> outputStreams = new Dictionary<uint, FileStream>();

            using (FileStream fs = File.Open(pPath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    outputPath = Path.Combine(Path.GetDirectoryName(pPath), String.Format("VGMT_EXTRACTED_OGGs", Path.GetFileNameWithoutExtension(pPath)));

                    while ((offset = ParseFile.GetNextOffset(fs, offset, XiphOrgOggContainer.MAGIC_BYTES)) > -1)
                    {
                        if (!this.CancellationPending)
                        {
                            pageWrittenToFile = false;
                            
                            //------------------
                            // get page details
                            //------------------
                            pageType = ParseFile.ParseSimpleOffset(fs, offset + 5, 1)[0];
                            bitstreamSerialNumber = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, offset + 0xE, 4), 0);
                            segmentCount = ParseFile.ParseSimpleOffset(fs, offset + 0x1A, 1)[0];

                            sizeOfAllSegments = 0;

                            for (i = 0; i < segmentCount; i++)
                            {
                                sizeOfAllSegments += ParseFile.ParseSimpleOffset(fs, offset + 0x1B + i, 1)[0];
                            }

                            pageSize = 0x1B + segmentCount + sizeOfAllSegments;

                            //-----------------------------
                            // write page
                            //-----------------------------
                            rawPageBytes = ParseFile.ParseSimpleOffset(fs, offset, (int)pageSize);

                                // open stream
                                if ((pageType & XiphOrgOggContainer.PAGE_TYPE_BEGIN_STREAM) ==
                                    XiphOrgOggContainer.PAGE_TYPE_BEGIN_STREAM)
                                {
                                    if (outputStreams.ContainsKey(bitstreamSerialNumber))
                                    {
                                        if (extractOggStruct.StopParsingOnFormatError)
                                        {
                                            throw new FormatException(
                                                String.Format("Beginning of Stream page found multiple times without End of Stream page, for serial number: {1}.{2}",
                                                pPath,
                                                bitstreamSerialNumber.ToString("X8"),
                                                Environment.NewLine));
                                        }
                                        else
                                        {
                                            //this.progressStruct.Clear();
                                            //progressStruct.GenericMessage = String.Format("Warning, for file <{0}>, Beginning of Stream page found multiple times without End of Stream page, for serial number: {1}.{2}",
                                            //                                    pPath,
                                            //                                    bitstreamSerialNumber.ToString("X8"),
                                            //                                    Environment.NewLine);
                                            //ReportProgress(Constants.ProgressMessageOnly, progressStruct);                                                                            
                                        }
                                    }
                                    else
                                    {
                                        // create the output stream
                                        outputFileName = Path.Combine(outputPath, String.Format("{0}_0x{1}_{2}.ogg", Path.GetFileNameWithoutExtension(pPath), offset.ToString("X8"), bitstreamSerialNumber.ToString("X8")));
                                        outputFileName = FileUtil.GetNonDuplicateFileName(outputFileName);

                                        if (!Directory.Exists(outputPath))
                                        {
                                            Directory.CreateDirectory(outputPath);
                                        }

                                        outputStreams[bitstreamSerialNumber] = File.Open(outputFileName, FileMode.CreateNew, FileAccess.Write);
                                        outputStreams[bitstreamSerialNumber].Write(rawPageBytes, 0, rawPageBytes.Length);
                                        pageWrittenToFile = true;
                                    }
                                }

                                // write page otherwise
                                if (outputStreams.ContainsKey(bitstreamSerialNumber))
                                {
                                    if (!pageWrittenToFile)
                                    {
                                        outputStreams[bitstreamSerialNumber].Write(rawPageBytes, 0, rawPageBytes.Length);
                                        pageWrittenToFile = true;
                                    }
                                }
                                else
                                {
                                    if (extractOggStruct.StopParsingOnFormatError)
                                    {
                                        throw new FormatException(
                                            String.Format("Stream data page found without Beginning of Stream page, for serial number: {1}.{2}",
                                            pPath,
                                            bitstreamSerialNumber.ToString("X8"),
                                            Environment.NewLine));
                                    }
                                    else
                                    {
                                        //this.progressStruct.Clear();
                                        //progressStruct.GenericMessage = String.Format("Warning, for file <{0}>, Stream data page found without Beginning of Stream page, for serial number: {1}.{2}",
                                        //                                    pPath,
                                        //                                    bitstreamSerialNumber.ToString("X8"),
                                        //                                    Environment.NewLine);
                                        //ReportProgress(Constants.ProgressMessageOnly, progressStruct);                                    
                                    }
                                }
                                
                            
                                // close stream if needed
                                if ((pageType & XiphOrgOggContainer.PAGE_TYPE_END_STREAM) ==
                                        XiphOrgOggContainer.PAGE_TYPE_END_STREAM)
                                {
                                    if (outputStreams.ContainsKey(bitstreamSerialNumber))
                                    {
                                        if (!pageWrittenToFile)
                                        {
                                            outputStreams[bitstreamSerialNumber].Write(rawPageBytes, 0, rawPageBytes.Length);
                                            pageWrittenToFile = true;
                                        }

                                        outputStreams[bitstreamSerialNumber].Close();
                                        outputStreams[bitstreamSerialNumber].Dispose();

                                        // remove stream from dictionary
                                        outputStreams.Remove(bitstreamSerialNumber);
                                    }
                                    else
                                    {
                                        if (extractOggStruct.StopParsingOnFormatError)
                                        {
                                            throw new FormatException(
                                                String.Format("End of Stream page found without a Beginning of Stream page, for serial number: {1}{2}.",
                                                pPath,
                                                bitstreamSerialNumber.ToString("X8"),
                                                Environment.NewLine));
                                        }
                                        else
                                        {
                                            //this.progressStruct.Clear();
                                            //progressStruct.GenericMessage = String.Format("Warning, for file <{0}>, End of Stream page found without a Beginning of Stream page, for serial number: {1}.{2}",
                                            //                                    pPath,
                                            //                                    bitstreamSerialNumber.ToString("X8"),
                                            //                                    Environment.NewLine);
                                            //ReportProgress(Constants.ProgressMessageOnly, progressStruct);
                                        }
                                    }
                                }
                                //} if (pageType > 0)
                            
                            offset += pageSize;
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    } // while
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    // close any unfinished streams
                    foreach (uint k in outputStreams.Keys)
                    {
                        outputStreams[k].Close();
                        outputStreams[k].Dispose();
                    }
                }
            }
        }
    }
}
