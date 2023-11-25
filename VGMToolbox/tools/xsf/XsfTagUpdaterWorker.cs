using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.xsf
{
    public class XsfTagUpdaterWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        private delegate void XsfTagSetter(string pValue, bool flag);
        
        public struct XsfTagUpdaterStruct : IVgmtWorkerStruct
        {
            public bool RemoveEmptyTags;
            public bool IsBatchMode;
            public bool GenerateTitleFromFilename;
            public bool RemoveBracketInfoFromTitle { set; get; }
            public bool AddToBatchFile;

            public string TitleTag;
            public string ArtistTag;
            public string GameTag;
            public string YearTag;
            public string GenreTag;
            public string CommentTag;
            public string CopyrightTag;
            public string XsfByTag;
            public string TaggerTag;
            public string VolumeTag;
            public string LengthTag;
            public string FadeTag;
            public string SystemTag;

            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public XsfTagUpdaterWorker() : base() { }

        protected override void DoTaskForFile(string pPath,
            IVgmtWorkerStruct pXsfTagUpdaterStruct, DoWorkEventArgs e)
        {
            XsfTagUpdaterStruct xsfTagUpdaterStruct = (XsfTagUpdaterStruct)pXsfTagUpdaterStruct;

            Type formatType = null;
            IXsfTagFormat vgmData = null;

            using (FileStream fs = File.OpenRead(pPath))
            {
                formatType = FormatUtil.getObjectType(fs);
                if (formatType != null)
                {
                    vgmData = (IXsfTagFormat)Activator.CreateInstance(formatType);
                    vgmData.Initialize(fs, pPath);
                }                
            }

            if (vgmData != null)
            {
                XsfTagSetter xts;

                xts = new XsfTagSetter(vgmData.SetArtistTag);
                updateXsfTag(xts, xsfTagUpdaterStruct.ArtistTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                xts = new XsfTagSetter(vgmData.SetGameTag);
                updateXsfTag(xts, xsfTagUpdaterStruct.GameTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                xts = new XsfTagSetter(vgmData.SetYearTag);
                updateXsfTag(xts, xsfTagUpdaterStruct.YearTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                xts = new XsfTagSetter(vgmData.SetGenreTag);
                updateXsfTag(xts, xsfTagUpdaterStruct.GenreTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                xts = new XsfTagSetter(vgmData.SetCommentTag);
                updateXsfTag(xts, xsfTagUpdaterStruct.CommentTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                xts = new XsfTagSetter(vgmData.SetCopyrightTag);
                updateXsfTag(xts, xsfTagUpdaterStruct.CopyrightTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                xts = new XsfTagSetter(vgmData.SetXsfByTag);
                updateXsfTag(xts, xsfTagUpdaterStruct.XsfByTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                xts = new XsfTagSetter(vgmData.SetTaggerTag);
                updateXsfTag(xts, xsfTagUpdaterStruct.TaggerTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                xts = new XsfTagSetter(vgmData.SetSystemTag);
                updateXsfTag(xts, xsfTagUpdaterStruct.SystemTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);

                if (xsfTagUpdaterStruct.GenerateTitleFromFilename)
                {
                    xsfTagUpdaterStruct.TitleTag = XsfUtil.GetTitleForFileName(pPath, xsfTagUpdaterStruct.RemoveBracketInfoFromTitle);
                    xts = new XsfTagSetter(vgmData.SetTitleTag);
                    updateXsfTag(xts, xsfTagUpdaterStruct.TitleTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                }

                if (!xsfTagUpdaterStruct.IsBatchMode)
                {
                    xts = new XsfTagSetter(vgmData.SetVolumeTag);
                    updateXsfTag(xts, xsfTagUpdaterStruct.VolumeTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                    xts = new XsfTagSetter(vgmData.SetLengthTag);
                    updateXsfTag(xts, xsfTagUpdaterStruct.LengthTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                    xts = new XsfTagSetter(vgmData.SetFadeTag);
                    updateXsfTag(xts, xsfTagUpdaterStruct.FadeTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                    xts = new XsfTagSetter(vgmData.SetTitleTag);
                    updateXsfTag(xts, xsfTagUpdaterStruct.TitleTag, xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                }
                else if (xsfTagUpdaterStruct.AddToBatchFile)
                {
                    // reset tags with existing values to add to batch script
                    xts = new XsfTagSetter(vgmData.SetVolumeTag);
                    updateXsfTag(xts, vgmData.GetVolumeTag(), xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                    xts = new XsfTagSetter(vgmData.SetLengthTag);
                    updateXsfTag(xts, vgmData.GetLengthTag(), xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                    xts = new XsfTagSetter(vgmData.SetFadeTag);
                    updateXsfTag(xts, vgmData.GetFadeTag(), xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);
                    xts = new XsfTagSetter(vgmData.SetTitleTag);
                    updateXsfTag(xts, vgmData.GetTitleTag(), xsfTagUpdaterStruct.RemoveEmptyTags, xsfTagUpdaterStruct.AddToBatchFile);                
                }

                vgmData.UpdateTags();
            }
        }

        private void updateXsfTag(XsfTagSetter pXsfTagSetter, string pValue, 
            bool pRemoveEmptyTags, bool addActionToBatchFile)
        {
            if (pRemoveEmptyTags)
            {
                pXsfTagSetter(pValue, addActionToBatchFile);
            }
            else if ((!String.IsNullOrEmpty(pValue)) && (!String.IsNullOrEmpty(pValue.Trim())))
            {
                pXsfTagSetter(pValue, addActionToBatchFile);
            }
        }
    }
}
