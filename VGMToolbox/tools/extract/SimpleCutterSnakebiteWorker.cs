using System;
using System.ComponentModel;
using System.IO;

using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.extract
{
    class SimpleCutterSnakebiteWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct SimpleCutterSnakebiteStruct : IVgmtWorkerStruct
        {
            public string OutputFile;
            public string NewFileExtension;


            public bool UseEndAddress;
            public bool UseLength;
            public bool UseFileEnd;

            public string StartOffset;
            public string EndAddress;
            public string Length;

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public SimpleCutterSnakebiteWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pSimpleCutterSnakebiteStruct, DoWorkEventArgs e)
        {
            SimpleCutterSnakebiteStruct simpleCutterSnakebiteStruct = (SimpleCutterSnakebiteStruct)pSimpleCutterSnakebiteStruct;

            using (FileStream fs = File.OpenRead(pPath))
            {
                long startOffset = VGMToolbox.util.ByteConversion.GetLongValueFromString(simpleCutterSnakebiteStruct.StartOffset);
                long cutLength = -1;

                if (simpleCutterSnakebiteStruct.UseEndAddress)
                {
                    cutLength = (VGMToolbox.util.ByteConversion.GetLongValueFromString(simpleCutterSnakebiteStruct.EndAddress) - startOffset) + 1;
                }
                else if (simpleCutterSnakebiteStruct.UseLength)
                {
                    cutLength = VGMToolbox.util.ByteConversion.GetLongValueFromString(simpleCutterSnakebiteStruct.Length);
                }
                else if (simpleCutterSnakebiteStruct.UseFileEnd)
                {
                    cutLength = (fs.Length - startOffset) + 1;
                }

                if (!String.IsNullOrEmpty(simpleCutterSnakebiteStruct.NewFileExtension))
                {
                    if (!simpleCutterSnakebiteStruct.NewFileExtension.StartsWith("."))
                    {
                        simpleCutterSnakebiteStruct.NewFileExtension = "." + simpleCutterSnakebiteStruct.NewFileExtension;
                    }
                    simpleCutterSnakebiteStruct.OutputFile = Path.ChangeExtension(pPath, simpleCutterSnakebiteStruct.NewFileExtension);
                }
                
                if (!Path.IsPathRooted(simpleCutterSnakebiteStruct.OutputFile))
                { 
                    simpleCutterSnakebiteStruct.OutputFile = Path.Combine(Path.GetDirectoryName(pPath), simpleCutterSnakebiteStruct.OutputFile);

                    if (simpleCutterSnakebiteStruct.OutputFile.Equals(pPath))
                    {
                        simpleCutterSnakebiteStruct.OutputFile = Path.Combine(Path.GetDirectoryName(simpleCutterSnakebiteStruct.OutputFile),
                            Path.GetFileNameWithoutExtension(simpleCutterSnakebiteStruct.OutputFile) + "_cut" + 
                            Path.GetExtension(simpleCutterSnakebiteStruct.OutputFile));
                    }
                }
                
                ParseFile.ExtractChunkToFile(fs, startOffset, cutLength, simpleCutterSnakebiteStruct.OutputFile, true, true);

                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("File <{0}>, extracted.{1}", simpleCutterSnakebiteStruct.OutputFile, Environment.NewLine);
                ReportProgress(this.progress, this.progressStruct);
            }
        }
    }
}
