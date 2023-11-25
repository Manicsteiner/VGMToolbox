using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    public struct EasyPsfDriverExtractionStruct : IVgmtWorkerStruct
    {
        private string[] sourcePaths;
        public string[] SourcePaths
        {
            get { return sourcePaths; }
            set { sourcePaths = value; }
        }
    }
    
    class EasyPsfDriverExtractionWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        private const uint PC_OFFSET_CORRECTION = 0x800FF000;
        
        public EasyPsfDriverExtractionWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pEasyPsfDriverExtractionStruct,
            DoWorkEventArgs e)
        {
            byte[] initialProgramCounter;
            uint initialProgramCounterValue;
            
            uint pcOffsetSeq;
            uint pcOffsetVb;
            uint pcOffsetVh;            
            uint lengthSeq;
            uint lengthVb;
            uint lengthVh;

            byte[] vhNumberOfPrograms;
            UInt16 vhNumberOfProgramsValue;

            Xsf2ExeStruct decompressionOptions = new Xsf2ExeStruct();
            decompressionOptions.IncludeExtension = false;
            decompressionOptions.StripGsfHeader = false;
            string extractedDataSectionPath = XsfUtil.ExtractCompressedDataSection(pPath, decompressionOptions);

            if (!String.IsNullOrEmpty(extractedDataSectionPath))
            {
                using (FileStream fs = File.OpenRead(extractedDataSectionPath))
                {
                    // get offset of initial program counter
                    initialProgramCounter = ParseFile.ParseSimpleOffset(fs, 0x10, 4);
                    initialProgramCounterValue = BitConverter.ToUInt32(initialProgramCounter, 0);

                    pcOffsetSeq = initialProgramCounterValue - PC_OFFSET_CORRECTION;
                    pcOffsetVb = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, (long)(pcOffsetSeq - 4), 4), 0) - PC_OFFSET_CORRECTION;
                    pcOffsetVh = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, (long)(pcOffsetSeq - 8), 4), 0) - PC_OFFSET_CORRECTION;

                    lengthSeq = pcOffsetVb - pcOffsetSeq;
                    lengthVb = pcOffsetVh - pcOffsetVb;

                    vhNumberOfPrograms = ParseFile.ParseSimpleOffset(fs, (long)(pcOffsetVh + 0x12), 2);
                    vhNumberOfProgramsValue = BitConverter.ToUInt16(vhNumberOfPrograms, 0);
                    lengthVh = (UInt16)((512 * vhNumberOfProgramsValue) + 2592);

                    // extract files
                    ParseFile.ExtractChunkToFile(fs, (long)pcOffsetSeq, (int)lengthSeq, Path.ChangeExtension(pPath, ".seq"));
                    ParseFile.ExtractChunkToFile(fs, (long)pcOffsetVh, (int)lengthVh, Path.ChangeExtension(pPath, ".vh"));
                    ParseFile.ExtractChunkToFile(fs, (long)pcOffsetVb, (int)lengthVb, Path.ChangeExtension(pPath, ".vb"));

                } // using (FileStream fs = File.OpenRead(pPath))

                // delete extracted data
                File.Delete(extractedDataSectionPath);
            } // if (!String.IsNullOrEmpty(extractedDataSectionPath))
        }
    }
}
