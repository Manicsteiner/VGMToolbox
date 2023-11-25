using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class PsxSepToSeqExtractorWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {

        public struct PsxSepToSeqExtractorStruct : IVgmtWorkerStruct
        {            
            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public PsxSepToSeqExtractorWorker() : base() { }

        protected override void DoTaskForFile(string pPath,
            IVgmtWorkerStruct pPsxSepToSeqExtractorStruct, DoWorkEventArgs e)
        {
            PsxSepToSeqExtractorStruct psxSepToSeqExtractorStruct =
                (PsxSepToSeqExtractorStruct)pPsxSepToSeqExtractorStruct;

            if (PsxSequence.IsSepTypeSequence(pPath))
            {
                uint seqCount = PsxSequence.GetSeqCount(pPath);
                string extractedFileName;

                for (uint i = 0; i < seqCount; i++)
                {
                    extractedFileName = PsxSequence.ExtractSeqFromSep(pPath, i);

                    this.progressStruct.Clear();
                    this.progressStruct.FileName = pPath;
                    this.progressStruct.GenericMessage = 
                        String.Format("<{0}> extracted.{1}", extractedFileName, Environment.NewLine);
                    this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
                }
            }
        }        
    }
}
