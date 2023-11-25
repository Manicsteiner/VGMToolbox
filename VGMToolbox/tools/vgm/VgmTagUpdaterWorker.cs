using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.vgm
{
    class VgmTagUpdaterWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct VgmTagUpdaterStruct : IVgmtWorkerStruct
        {
            public bool IsBatchMode;

            public string TitleTagEn;
            public string TitleTagJp;
            public string GameTagEn;
            public string GameTagJp;
            public string SystemTagEn;
            public string SystemTagJp;
            public string ArtistTagEn;
            public string ArtistTagJp;
            public string DateTag;
            public string RipperTag;
            public string CommentTag;

            public bool DoTitleTagEn;
            public bool DoTitleTagJp;
            public bool DoGameTagEn;
            public bool DoGameTagJp;
            public bool DoSystemTagEn;
            public bool DoSystemTagJp;
            public bool DoArtistTagEn;
            public bool DoArtistTagJp;
            public bool DoDateTag;
            public bool DoRipperTag;
            public bool DoCommentTag;

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public VgmTagUpdaterWorker() : base() { }

        protected override void DoTaskForFile(string pPath,
            IVgmtWorkerStruct pVgmTagUpdaterStruct, DoWorkEventArgs e)
        {
            VgmTagUpdaterStruct vgmTagUpdaterStruct = (VgmTagUpdaterStruct)pVgmTagUpdaterStruct;

            Type formatType = null;
            IGd3TagFormat vgmData = null;

            using (FileStream fs = File.OpenRead(pPath))
            {
                formatType = FormatUtil.getObjectType(fs);
                if (formatType != null)
                {
                    vgmData = (IGd3TagFormat)Activator.CreateInstance(formatType);
                    vgmData.Initialize(fs, pPath);
                }
            }

            if (vgmData != null)
            {
                if (vgmTagUpdaterStruct.DoGameTagEn)
                {
                    vgmData.SetGameTagEn(vgmTagUpdaterStruct.GameTagEn);
                }

                if (vgmTagUpdaterStruct.DoGameTagJp)
                {
                    vgmData.SetGameTagJp(vgmTagUpdaterStruct.GameTagJp);
                }

                if (vgmTagUpdaterStruct.DoSystemTagEn)
                {
                    vgmData.SetSystemTagEn(vgmTagUpdaterStruct.SystemTagEn);
                }

                if (vgmTagUpdaterStruct.DoSystemTagJp)
                {
                    vgmData.SetSystemTagJp(vgmTagUpdaterStruct.SystemTagJp);
                }

                if (vgmTagUpdaterStruct.DoArtistTagEn)
                {
                    vgmData.SetArtistTagEn(vgmTagUpdaterStruct.ArtistTagEn);
                }

                if (vgmTagUpdaterStruct.DoArtistTagJp)
                {
                    vgmData.SetArtistTagJp(vgmTagUpdaterStruct.ArtistTagJp);
                }

                if (vgmTagUpdaterStruct.DoDateTag)
                {
                    vgmData.SetDateTag(vgmTagUpdaterStruct.DateTag);
                }

                if (vgmTagUpdaterStruct.DoRipperTag)
                {
                    vgmData.SetRipperTag(vgmTagUpdaterStruct.RipperTag);
                }

                if (vgmTagUpdaterStruct.DoCommentTag)
                {
                    vgmData.SetCommentTag(vgmTagUpdaterStruct.CommentTag);
                }

                if (vgmTagUpdaterStruct.DoTitleTagEn)
                {
                    vgmData.SetTitleTagEn(vgmTagUpdaterStruct.TitleTagEn);
                }

                if (vgmTagUpdaterStruct.DoTitleTagJp)
                {
                    vgmData.SetTitleTagJp(vgmTagUpdaterStruct.TitleTagJp);
                }

                vgmData.UpdateTags();
            }
        }
    }
}
