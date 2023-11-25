using System;
using System.ComponentModel;
using System.IO;

using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.extract
{
    class OffsetFinderWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public const string LITTLE_ENDIAN = "Little Endian";
        public const string BIG_ENDIAN = "Big Endian";
        
        public struct OffsetFinderStruct : IVgmtWorkerStruct
        {
            public string searchString;
            public string startingOffset;
            public string MinimumSize { set; get; }
            public bool treatSearchStringAsHex;

            public bool cutFile;
            public string searchStringOffset;            
            public string cutSize;
            public string cutSizeOffsetSize;
            public bool isCutSizeAnOffset;
            public string outputFileExtension;
            public bool isLittleEndian;
            public bool UseLengthMultiplier { set; get; }
            public string LengthMultiplier { set; get; }

            public bool useTerminatorForCutsize;
            public string terminatorString;
            public bool treatTerminatorStringAsHex;
            public bool includeTerminatorLength;
            public bool CutToEofIfTerminatorNotFound { set; get; }

            public string extraCutSizeBytes;

            public bool OutputLogFile { set; get; }

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }

            public bool DoSearchStringModulo
            {
                set;
                get;
            }

            public bool DoTerminatorModulo
            {
                set;
                get;
            }

            public string SearchStringModuloDivisor
            {
                set;
                get;
            }

            public string SearchStringModuloResult
            {
                set;
                get;
            }

            public string TerminatorStringModuloDivisor
            {
                set;
                get;
            }

            public string TerminatorStringModuloResult
            {
                set;
                get;
            }

            public string OutputFolder { get; set; }

            public FindOffsetStruct ToFindOffsetStruct()
            {
                VGMToolbox.util.FindOffsetStruct findOffsetStruct = new VGMToolbox.util.FindOffsetStruct();
                findOffsetStruct.SearchString = this.searchString;
                findOffsetStruct.TreatSearchStringAsHex = this.treatSearchStringAsHex;
                findOffsetStruct.MinimumSize = this.MinimumSize;

                findOffsetStruct.CutFile = this.cutFile;
                findOffsetStruct.SearchStringOffset = this.searchStringOffset;
                findOffsetStruct.StartingOffset = this.startingOffset;
                findOffsetStruct.CutSize = this.cutSize;
                findOffsetStruct.CutSizeOffsetSize = this.cutSizeOffsetSize;
                findOffsetStruct.IsCutSizeAnOffset = this.isCutSizeAnOffset;
                findOffsetStruct.OutputFileExtension = this.outputFileExtension;
                findOffsetStruct.IsLittleEndian = this.isLittleEndian;
                
                findOffsetStruct.UseLengthMultiplier = this.UseLengthMultiplier;
                findOffsetStruct.LengthMultiplier = this.LengthMultiplier;
                
                findOffsetStruct.UseTerminatorForCutSize = this.useTerminatorForCutsize;
                findOffsetStruct.TerminatorString = this.terminatorString;
                findOffsetStruct.TreatTerminatorStringAsHex = this.treatTerminatorStringAsHex;
                findOffsetStruct.IncludeTerminatorLength = this.includeTerminatorLength;
                findOffsetStruct.ExtraCutSizeBytes = this.extraCutSizeBytes;
                findOffsetStruct.CutToEofIfTerminatorNotFound = this.CutToEofIfTerminatorNotFound;

                findOffsetStruct.DoTerminatorModulo = this.DoTerminatorModulo;
                findOffsetStruct.TerminatorStringModuloDivisor = this.TerminatorStringModuloDivisor;
                findOffsetStruct.TerminatorStringModuloResult = this.TerminatorStringModuloResult;
                findOffsetStruct.DoSearchStringModulo = this.DoSearchStringModulo;
                findOffsetStruct.SearchStringModuloDivisor = this.SearchStringModuloDivisor;
                findOffsetStruct.SearchStringModuloResult = this.SearchStringModuloResult;
                findOffsetStruct.OutputFolder = this.OutputFolder;

                return findOffsetStruct;
            }
        }

        public OffsetFinderWorker() : 
            base() 
        {
            this.progressCounterIncrementer = 10;
        }
        
        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pOffsetFinderStruct,
            DoWorkEventArgs e)
        {
            OffsetFinderStruct offsetFinderStruct = (OffsetFinderStruct) pOffsetFinderStruct;

            VGMToolbox.util.FindOffsetStruct findOffsetStruct = offsetFinderStruct.ToFindOffsetStruct();

            string output;
            ParseFile.FindOffsetAndCutFile(pPath, findOffsetStruct, out output, offsetFinderStruct.OutputLogFile, offsetFinderStruct.OutputLogFile);

            this.outputBuffer.Append(output);

            //this.progressStruct.Clear();
            //this.progressStruct.GenericMessage = output;
            //ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);            
        }    

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            OffsetFinderStruct offsetFinderStruct = (OffsetFinderStruct)e.Argument;

            if (!String.IsNullOrEmpty(offsetFinderStruct.outputFileExtension) &&
                !offsetFinderStruct.outputFileExtension.StartsWith("."))
            {
                offsetFinderStruct.outputFileExtension = "." + offsetFinderStruct.outputFileExtension;
            }

            base.OnDoWork(e);
        }    
    }
}
