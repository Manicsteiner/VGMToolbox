using System.ComponentModel;

using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.extract
{
    class ExtractCdxaWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct ExtractCdxaStruct : IVgmtWorkerStruct
        {
            public bool AddRiffHeader;
            public bool PatchByte0x11;
            public uint SilentFramesCount;

            public string[] SourcePaths { set; get; }
            public bool FilterAgainstBlockId { set; get; }
            public bool DoTwoPass { set; get; }

            public bool UseSilentBlocksForEof { set; get; }
            public bool UseEndOfTrackMarkerForEof { set; get; }
        }

        public ExtractCdxaWorker():
            base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pExtractCdxaStruct, DoWorkEventArgs e)
        {
            ExtractCdxaStruct extractCdxaStruct = (ExtractCdxaStruct) pExtractCdxaStruct;
            
            ExtractXaStruct extStruct = new ExtractXaStruct();
            extStruct.Path = pPath;
            extStruct.AddRiffHeader = extractCdxaStruct.AddRiffHeader;
            extStruct.PatchByte0x11 = extractCdxaStruct.PatchByte0x11;
            extStruct.SilentFramesCount = extractCdxaStruct.SilentFramesCount;
            extStruct.FilterAgainstBlockId = extractCdxaStruct.FilterAgainstBlockId;
            extStruct.DoTwoPass = extractCdxaStruct.DoTwoPass;

            extStruct.UseEndOfTrackMarkerForEof = extractCdxaStruct.UseEndOfTrackMarkerForEof;
            extStruct.UseSilentBlocksForEof = extractCdxaStruct.UseSilentBlocksForEof;

            CdxaUtil.ExtractXaFiles(extStruct);            
        }       
    }
}
