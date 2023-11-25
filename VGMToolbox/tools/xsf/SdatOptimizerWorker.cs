using System;
using System.ComponentModel;
using System.IO;

using VGMToolbox.format.sdat;
using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.xsf
{
    class SdatOptimizerWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct SdatOptimizerStruct : IVgmtWorkerStruct
        {
            public string startSequence;
            public string endSequence;

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public SdatOptimizerWorker() : base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pSdatOptimizerStruct, 
            DoWorkEventArgs e)
        {
            SdatOptimizerStruct sdatOptimizerStruct = (SdatOptimizerStruct)pSdatOptimizerStruct;
            
            string sdatDirectory;
            string sdatOptimizingFileName;
            string sdatOptimizingPath;

            string sdatCompletedFileName;
            string sdatCompletedPath;

            Sdat sdat = null;
            int startSequence = Sdat.NO_SEQUENCE_RESTRICTION;
            int endSequence = Sdat.NO_SEQUENCE_RESTRICTION;

            sdatDirectory = Path.GetDirectoryName(pPath);
            sdatOptimizingFileName = String.Format("{0}_OPTIMIZING.sdat", 
                Path.GetFileNameWithoutExtension(pPath));
            sdatOptimizingPath = Path.Combine(sdatDirectory, sdatOptimizingFileName);

            sdatCompletedFileName = String.Format("{0}_OPTIMIZED.sdat",
                Path.GetFileNameWithoutExtension(pPath));
            sdatCompletedPath = Path.Combine(sdatDirectory, sdatCompletedFileName);

            File.Copy(pPath, sdatOptimizingPath, true);

            using (FileStream fs = File.Open(sdatOptimizingPath, FileMode.Open, FileAccess.ReadWrite))
            {
                Type dataType = FormatUtil.getObjectType(fs);
                
                if (dataType != null && dataType.Name.Equals("Sdat"))
                {
                    sdat = new Sdat();
                    sdat.Initialize(fs, sdatOptimizingPath);                        
                }
            }

            if (sdat != null)
            {
                if (!String.IsNullOrEmpty(sdatOptimizerStruct.startSequence))
                {
                    startSequence = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(sdatOptimizerStruct.startSequence.Trim());
                }

                if (!String.IsNullOrEmpty(sdatOptimizerStruct.endSequence))
                {
                    endSequence = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(sdatOptimizerStruct.endSequence);
                }

                sdat.OptimizeForZlib(startSequence, endSequence);                
            }

            File.Copy(sdatOptimizingPath, sdatCompletedPath, true);
            File.Delete(sdatOptimizingPath);
        }            
    }        
}
