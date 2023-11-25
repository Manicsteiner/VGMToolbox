using System.ComponentModel;

using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.gbs
{
    class GbsM3uBuilderWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct GbsM3uWorkerStruct : IVgmtWorkerStruct
        {
            public bool onePlaylistPerFile;
            public bool UseKnurekFormatParsing { set; get; }

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public GbsM3uBuilderWorker() :
            base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pGbsM3uWorkerStruct, DoWorkEventArgs e)
        {
            GbsM3uWorkerStruct gbsM3uWorkerStruct = (GbsM3uWorkerStruct)pGbsM3uWorkerStruct;
                      
            M3uBuilderStruct gbsM3uBuilderStruct = new M3uBuilderStruct();
            gbsM3uBuilderStruct.OnePlaylistPerFile = gbsM3uWorkerStruct.onePlaylistPerFile;
            gbsM3uBuilderStruct.UseKnurekFormatParsing = gbsM3uWorkerStruct.UseKnurekFormatParsing;
            gbsM3uBuilderStruct.Path = pPath;
            NezPlugUtil.BuildPlaylistForFile(gbsM3uBuilderStruct);
        }                       
    }
}
