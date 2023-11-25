using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    public class Xsf2sfTagMigratorWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        protected int fileCount;
        protected int maxFiles;
        protected int progress;
        protected VGMToolbox.util.ProgressStruct progressStruct;        
        
        public struct Xsf2sfTagMigratorStruct
        {
            public string SourceDirectory;
            public string DestinationDirectory;

            public bool CopyEmptyTags;

            public bool UpdateFileName;
            public bool UpdateTitleTag;
            public bool UpdateArtistTag;
            public bool UpdateGameTag;
            public bool UpdateYearTag;
            public bool UpdateGenreTag;
            public bool UpdateCommentTag;
            public bool UpdateCopyrightTag;
            public bool UpdateXsfByTag;
            public bool UpdateVolumeTag;
            public bool UpdateLengthTag;
            public bool UpdateFadeTag;
        }

        public Xsf2sfTagMigratorWorker()
        {
            this.progressStruct = new VGMToolbox.util.ProgressStruct();
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;

            this.progress = 0;
        }

        private void migrateTags(Xsf2sfTagMigratorStruct pXsf2sfTagMigratorStruct)
        {
            Dictionary<int, string> sourceFiles = new Dictionary<int,string>();
            int songNumber;
            Xsf source2sf;
            Xsf destination2sf;
            string renameFilePath;

            try
            {
                // build source list
                foreach (string f in Directory.GetFiles(pXsf2sfTagMigratorStruct.SourceDirectory, "*.*"))
                {                    
                    songNumber = XsfUtil.GetSongNumberForYoshiIslandMini2sf(f);

                    if (songNumber != XsfUtil.InvalidData)
                    {
                        if (!sourceFiles.ContainsKey(songNumber))
                        {
                            sourceFiles.Add(songNumber, f);
                        }
                        else
                        {
                            this.progress = 0;
                            this.progressStruct.Clear();
                            this.progressStruct.GenericMessage = String.Format(
                                "Warning duplicate track ID for <{0}>.  Does the V1 set use 2 .2sflibs?  Skipping...{1}",
                                f, 
                                Environment.NewLine);
                            this.ReportProgress(this.progress, this.progressStruct);                        
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                this.progress = 0;
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = String.Format("Error building source directory list: {0}{1}", 
                    _ex.Message, Environment.NewLine);
                this.ReportProgress(this.progress, this.progressStruct);
            }

            if (sourceFiles.Count > 0)
            {
                this.maxFiles = FileUtil.GetFileCount(new string[] { pXsf2sfTagMigratorStruct.DestinationDirectory }, false);
                
                // loop through destination
                foreach (string f in Directory.GetFiles(pXsf2sfTagMigratorStruct.DestinationDirectory, "*.*"))
                {
                    this.progress = (++fileCount * 100) / maxFiles;
                    
                    try
                    {
                        songNumber = XsfUtil.GetSongNumberForMini2sf(f);

                        // check for source
                        if ((songNumber != XsfUtil.InvalidData) &&
                            (sourceFiles.ContainsKey(songNumber)))
                        {
                            // copy the tags
                            XsfTagCopyStruct xtcStruct = new XsfTagCopyStruct();
                            xtcStruct.CopyEmptyTags = pXsf2sfTagMigratorStruct.CopyEmptyTags;
                            xtcStruct.UpdateArtistTag = pXsf2sfTagMigratorStruct.UpdateArtistTag;
                            xtcStruct.UpdateCommentTag = pXsf2sfTagMigratorStruct.UpdateCommentTag;
                            xtcStruct.UpdateCopyrightTag = pXsf2sfTagMigratorStruct.UpdateCopyrightTag;
                            xtcStruct.UpdateFadeTag = pXsf2sfTagMigratorStruct.UpdateFadeTag;
                            xtcStruct.UpdateGameTag = pXsf2sfTagMigratorStruct.UpdateGameTag;
                            xtcStruct.UpdateGenreTag = pXsf2sfTagMigratorStruct.UpdateGenreTag;
                            xtcStruct.UpdateLengthTag = pXsf2sfTagMigratorStruct.UpdateLengthTag;
                            xtcStruct.UpdateSystemTag = false;
                            xtcStruct.UpdateTitleTag = pXsf2sfTagMigratorStruct.UpdateTitleTag;
                            xtcStruct.UpdateVolumeTag = pXsf2sfTagMigratorStruct.UpdateVolumeTag;
                            xtcStruct.UpdateXsfByTag = pXsf2sfTagMigratorStruct.UpdateXsfByTag;
                            xtcStruct.UpdateYearTag = pXsf2sfTagMigratorStruct.UpdateYearTag;

                            using (FileStream sourceFs = File.Open(sourceFiles[songNumber], FileMode.Open, FileAccess.Read))
                            {
                                source2sf = new Xsf();
                                source2sf.Initialize(sourceFs, sourceFiles[songNumber]);
                            }

                            using (FileStream destinationFs = File.Open(f, FileMode.Open, FileAccess.Read))
                            {
                                destination2sf = new Xsf();
                                destination2sf.Initialize(destinationFs, f);
                            }

                            XsfUtil.CopyTags(source2sf, destination2sf, xtcStruct);

                            // rename the file
                            if (pXsf2sfTagMigratorStruct.UpdateFileName)
                            {
                                renameFilePath = Path.Combine(Path.GetDirectoryName(f),
                                    Path.GetFileNameWithoutExtension(sourceFiles[songNumber]) + Path.GetExtension(f));
                                File.Move(f, renameFilePath);
                            }
                        }

                        this.progressStruct.Clear();
                        this.progressStruct.FileName = f;
                        this.ReportProgress(this.progress, this.progressStruct);                                
                    }
                    catch (Exception _ex2)
                    {
                        this.progressStruct.Clear();
                        this.progressStruct.FileName = f;
                        this.progressStruct.ErrorMessage = String.Format("Error updating <{0}>: {1}{2}", f, _ex2.Message, Environment.NewLine);
                        this.ReportProgress(this.progress, this.progressStruct);                                                    
                    }
                } //foreach
            }
            else
            {
                this.progress = 0;
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = String.Format("ERROR, no 2SFs found in source directory.{0}", Environment.NewLine);
                this.ReportProgress(this.progress, this.progressStruct);            
            }
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Xsf2sfTagMigratorStruct tagMigratorStruct = (Xsf2sfTagMigratorStruct)e.Argument;
            this.migrateTags(tagMigratorStruct);
        }
    }
}
