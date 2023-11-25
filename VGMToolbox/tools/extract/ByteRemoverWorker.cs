using System;
using System.ComponentModel;
using System.IO;

using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.extract
{
    class ByteRemoverWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct ByteRemoverStruct : IVgmtWorkerStruct
        {
            public bool UseEndAddress { get; set; }
            public bool UseLength { get; set; }
            public bool UseFileEnd { get; set; }

            public string StartOffset { get; set; }
            public string EndOffset { get; set; }
            public string Length { get; set; }

            public string[] SourcePaths { get; set; }
        }

        public ByteRemoverWorker():
            base()
        {
            this.progressCounterIncrementer = 10;
        }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pByteRemoverStruct,
            DoWorkEventArgs e)
        {
            ByteRemoverStruct taskStruct = (ByteRemoverStruct)pByteRemoverStruct;

            long startOffset;
            long endOffset;
            long removalLength = 0;

            // parse start offset
            startOffset = ByteConversion.GetLongValueFromString(taskStruct.StartOffset);

            if (taskStruct.UseEndAddress)
            { 
                // parse end offset
                endOffset = ByteConversion.GetLongValueFromString(taskStruct.EndOffset);
                removalLength = endOffset - startOffset + 1;
            }
            else if (taskStruct.UseLength)
            {
                removalLength = ByteConversion.GetLongValueFromString(taskStruct.Length);
            }
            else if (taskStruct.UseFileEnd)
            {
                FileInfo fi = new FileInfo(pPath);
                removalLength = fi.Length - startOffset;
            }
            
            FileUtil.RemoveChunkFromFile(pPath, startOffset, removalLength);

            this.outputBuffer.Append(String.Format("{0}{1}", Path.GetFileName(pPath), Environment.NewLine));
        } 
    
    }
}
