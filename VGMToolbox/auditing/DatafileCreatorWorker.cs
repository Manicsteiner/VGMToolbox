using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format;
using VGMToolbox.format.auditing;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.auditing
{
    class DatafileCreatorWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        private string dir;
        private ArrayList romList;
        private int fileCount = 0;
        VGMToolbox.util.ProgressStruct progressStruct;

        public struct GetGameParamsStruct
        { 
            public string pDir; 
            public string pOutputMessage; 
            public int totalFiles;

            public bool UseNormalChecksums;
            public bool AddMd5Sha1;
        }
        
        public DatafileCreatorWorker()
        {
            fileCount = 0;
            progressStruct = new VGMToolbox.util.ProgressStruct();

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        public static header buildHeader(string pAuthor, string pCategory, string pComment, string pDate, string pDescription,
            string pEmail, string pHomepage, string pName, string pUrl, string pVersion)
        {
            header datHeader = new header();

            datHeader.author = pAuthor;
            datHeader.category = pCategory;
            datHeader.comment = pComment;
            datHeader.date = pDate;
            datHeader.description = pDescription;
            datHeader.email = pEmail;
            datHeader.homepage = pHomepage;
            datHeader.name = pName;
            datHeader.url = pUrl;
            datHeader.version = pVersion;

            return datHeader;
        }

        public game[] buildGames(GetGameParamsStruct pGetGameParamsStruct, DoWorkEventArgs e)
        {
            return this.buildGames(pGetGameParamsStruct, 0, e);
        }

        private game[] buildGames(GetGameParamsStruct pGetGameParamsStruct, uint pDepth, DoWorkEventArgs e)
        {
            game[] gameArray = null;
            ArrayList gameList = new ArrayList();

            pDepth++;

            game set;
            int progress;
            rom romfile;

            BuildRomStruct romParameters;
            string buildRomMessages;

            try
            {                
                // Directories
                foreach (string d in Directory.GetDirectories(pGetGameParamsStruct.pDir))
                {
                    if (Directory.GetFiles(d, "*.*", SearchOption.AllDirectories).Length > 0)
                    {
                        if (pDepth == 1)
                        {
                            this.romList = new ArrayList();
                            this.dir = d;
                        }

                        set = new game();

                        foreach (string f in Directory.GetFiles(d))
                        {
                            if (!CancellationPending)
                            {
                                progress = (++fileCount * 100) / pGetGameParamsStruct.totalFiles;
                                this.progressStruct.Clear();
                                this.progressStruct.FileName = f;
                                ReportProgress(progress, this.progressStruct);

                                try
                                {                                    
                                    romParameters = new BuildRomStruct();
                                    romParameters.AddMd5 = false;
                                    romParameters.AddSha1 = false;
                                    romParameters.FilePath = f;
                                    romParameters.TopLevelSetFolder = this.dir;
                                    romParameters.UseNormalChecksums = pGetGameParamsStruct.UseNormalChecksums;

                                    romfile = AuditingUtil.BuildRom(romParameters, out buildRomMessages);

                                    if (String.IsNullOrEmpty(buildRomMessages))
                                    {
                                        if (romfile.name != null)
                                        {
                                            romList.Add(romfile);
                                        }
                                    }
                                    else
                                    {
                                        this.progressStruct.Clear();
                                        this.progressStruct.FileName = f;
                                        this.progressStruct.ErrorMessage = buildRomMessages;
                                        ReportProgress(Constants.IgnoreProgress, this.progressStruct);                                    
                                    }
                                }
                                catch (Exception _ex)
                                {
                                    this.progressStruct.Clear();
                                    this.progressStruct.FileName = f;
                                    this.progressStruct.ErrorMessage = "Error processing <" + f + "> (" + _ex.Message + ")" + "...Skipped" + Environment.NewLine;
                                    ReportProgress(Constants.IgnoreProgress, this.progressStruct);
                                }
                            }
                            else
                            {
                                e.Cancel = true;
                                break;
                            }
                        }

                        if (!CancellationPending)
                        {
                            GetGameParamsStruct subdirGetGameParamsStruct = pGetGameParamsStruct;
                            subdirGetGameParamsStruct.pDir = d;
                            this.buildGames(subdirGetGameParamsStruct, pDepth, e);

                            if (pDepth == 1 && romList.Count > 0)
                            {
                                set.rom = (rom[])this.romList.ToArray(typeof(rom));
                                set.name = d.Substring(d.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                                set.description = set.name;
                                gameList.Add(set);
                            }
                        }
                        else 
                        { 
                            break; 
                        }
                    } // if ((Directory.GetFiles(d, "*.*", SearchOption.AllDirectories).Length - 1) > 0)              
                } // foreach (string d in Directory.GetDirectories(pDir))

                if (gameList.Count > 0)
                {
                    gameArray = (game[])gameList.ToArray(typeof(game));
                }
            }
            catch (Exception e1)
            {
                this.progressStruct.Clear();
                this.progressStruct.FileName = null;
                this.progressStruct.ErrorMessage = e1.Message;
                ReportProgress(Constants.IgnoreProgress, this.progressStruct);
            }
            return gameArray;            
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            GetGameParamsStruct getGameParamsStruct = (GetGameParamsStruct)e.Argument;
            e.Result = (game[])this.buildGames(getGameParamsStruct, e);
        }
    }
}
