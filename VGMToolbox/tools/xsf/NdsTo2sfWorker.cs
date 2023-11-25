using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.format.sdat;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class NdsTo2sfWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public const string TESTPACK_PATH = "external\\2sf\\testpack.nds";
        public const string TESTPACK_CRC32 = "FB16DF0E";
        public readonly string TESTPACK_FULL_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), TESTPACK_PATH);

        public struct NdsTo2sfStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { get; set; }
            public bool UseSmapNames { set; get; }
        }

        public NdsTo2sfWorker()
        {
            this.progressStruct = new VGMToolbox.util.ProgressStruct();

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pMk2sfStruct, DoWorkEventArgs e)
        {
            this.progressStruct.Clear();
            this.progressStruct.GenericMessage = String.Format("Processing: [{0}]{1}", pPath, Environment.NewLine);
            this.ReportProgress(this.progress, this.progressStruct);

            NdsTo2sfStruct workingStruct = (NdsTo2sfStruct)pMk2sfStruct;
            string ndsTo2sfOutputPath = Path.Combine(Path.GetDirectoryName(pPath), String.Format("{0}{1}", Path.GetFileNameWithoutExtension(pPath), XsfUtil.NDSTO2SF_FOLDER_SUFFIX));

            // Build Nds object
            Nds ndsFile = new Nds();
            using (FileStream ndsStream = File.OpenRead(pPath))
            {
                ndsFile.Initialize(ndsStream, pPath);
            }
 
            //------------------------
            // extract 2SFs and STRMs
            //------------------------
            bool filesWereRipped = XsfUtil.NdsTo2sf(pPath, TESTPACK_FULL_PATH, workingStruct.UseSmapNames);

            // output info
            this.progressStruct.Clear();

            if (!filesWereRipped)
            {
                this.progressStruct.GenericMessage = String.Format("  No extractable SSEQ/STRM/SWAR data found.{0}", Environment.NewLine);
            }
            else
            {
                this.progressStruct.GenericMessage = String.Format("  SSEQ/STRM/SWAR Data extracted.{0}", Environment.NewLine);
            }

            this.ReportProgress(this.progress, this.progressStruct);

            //---------------
            // extract SWAVs
            //---------------
            this.progressStruct.Clear();
            this.progressStruct.GenericMessage = String.Format("  Searching for SWAVs{0}", Environment.NewLine);
            this.ReportProgress(this.progress, this.progressStruct);

            string swavMessages = String.Empty;
            
            FindOffsetStruct findOffsetStruct = new FindOffsetStruct();
            findOffsetStruct.CutFile = true;
            findOffsetStruct.SearchString = "53574156FFFE";
            findOffsetStruct.TreatSearchStringAsHex = true;
            findOffsetStruct.SearchStringOffset = "0";
            findOffsetStruct.OutputFileExtension = ".swav";
            findOffsetStruct.IsCutSizeAnOffset = true;
            findOffsetStruct.CutSize = "8";
            findOffsetStruct.CutSizeOffsetSize = "4";
            findOffsetStruct.IsLittleEndian = true;
            findOffsetStruct.OutputFolder = Path.Combine(ndsTo2sfOutputPath, "SWAV");

            ParseFile.FindOffsetAndCutFile(pPath, findOffsetStruct, out swavMessages, false, false);

            //--------------
            // extract ADXs
            //--------------
            this.progressStruct.Clear();
            this.progressStruct.GenericMessage = String.Format("  Searching for ADXs{0}", Environment.NewLine);
            this.ReportProgress(this.progress, this.progressStruct);
            
            string adxMessages = String.Empty;
            CriUtil.ExtractAdxStreams(pPath, ndsFile.GameSerial, Path.Combine(ndsTo2sfOutputPath, "ADX"), out adxMessages);
        }
    }
}
