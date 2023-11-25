using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.hoot
{
    class HootXmlBuilderWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public string SUPPORTED_MULTI_FILE_FORMATS = "SPC";

        private ArrayList hootGamesArrayList = new ArrayList();

        public struct HootXmlBuilderStruct : IVgmtWorkerStruct
        {
            public bool combineOutput;
            public bool splitOutput;
            public bool parseFileName;

            public string[] SourcePaths { set; get; }

            public bool DoMultiFileSet { set; get; }
            public string OverrideSetName { set; get; }
        }

        public struct HootXmlFilenameStruct
        {
            public string GameName;
            public string JpGameName;
            public string Company;
        }

        public HootXmlBuilderWorker() : 
            base()
        {
            hootGamesArrayList = new ArrayList();
        }

        protected override void DoTaskForDirectory(string pPath, IVgmtWorkerStruct pHootXmlBuilderStruct,
            DoWorkEventArgs e)
        {
            HootXmlBuilderStruct hootXmlBuilderStruct = (HootXmlBuilderStruct)pHootXmlBuilderStruct;

            // pass control to base class if we are not doing a multifile set
            if (!hootXmlBuilderStruct.DoMultiFileSet)
            {
                base.DoTaskForDirectory(pPath, pHootXmlBuilderStruct, e);
            }
            else
            { 
                //-------------------------
                // handle a multi-file set
                //-------------------------
                string setName = getSetNameForMultiFileSet(pPath);

                // set set name and process files as usual
                if (!String.IsNullOrEmpty(setName))
                {
                    hootXmlBuilderStruct.OverrideSetName = setName;

                    foreach (string file in Directory.GetFiles(pPath))
                    {
                        if (!CancellationPending)
                        {
                            if (this.isValidHootFormat(file))
                            {
                                try
                                {
                                    this.DoTaskForFile(file, hootXmlBuilderStruct, e);
                                }
                                catch (Exception ex)
                                {
                                    this.progressStruct.Clear();
                                    this.progressStruct.ErrorMessage =
                                        String.Format(CultureInfo.CurrentCulture, "Error processing <{0}>.  Error received: ", pPath) + ex.Message + Environment.NewLine;
                                    ReportProgress(progress, this.progressStruct);
                                }
                                finally
                                {
                                    this.DoFinally();

                                    fileCount += 1;

                                    // Report Progress
                                    if ((fileCount == maxFiles) ||
                                        (((fileCount * 100) / maxFiles) > this.progressCounter))
                                    {
                                        this.progressCounter += this.progressCounterIncrementer;

                                        // output info
                                        if (this.outputBuffer.Length > 0)
                                        {
                                            this.progressStruct.Clear();
                                            progressStruct.GenericMessage = this.outputBuffer.ToString();
                                            ReportProgress(this.Progress, progressStruct);

                                            // clear out old info
                                            this.outputBuffer.Length = 0;
                                        }

                                        // output progress
                                        this.progress = (fileCount * 100) / maxFiles;
                                        this.progressStruct.Clear();
                                        this.progressStruct.FileName = pPath;
                                        ReportProgress(this.progress, progressStruct);
                                    }
                                }

                                break;
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }

                }
                else
                {
                    this.progressStruct.Clear();
                    this.progressStruct.ErrorMessage =
                    String.Format(CultureInfo.CurrentCulture, "Error processing <{0}>: Cannot find set name for .zip file xml creation.  Please verfiy your .zip file contains only the following format(s): {1}{2}", pPath, SUPPORTED_MULTI_FILE_FORMATS, Environment.NewLine);
                    ReportProgress(progress, this.progressStruct);
                }
            }
        }
        
        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pHootXmlBuilderStruct, DoWorkEventArgs e)
        {          
            VGMToolbox.format.hoot.game hootGame = null;
            HootXmlBuilderStruct hootXmlBuilderStruct = (HootXmlBuilderStruct)pHootXmlBuilderStruct;

            if (CompressionUtil.Is7zSupportedArchive(pPath))
            {
                // get tempfolder
                string tempFolder = Path.Combine(Path.GetTempPath(), "vgmt" + new Random().Next(100000).ToString());

                if (Directory.Exists(tempFolder))
                {
                    Directory.Delete(tempFolder, true);
                }

                // unzip file to temp folder
                CompressionUtil.ExtractAllFilesFromArchive(pPath, tempFolder);
                
                // process directory
                hootXmlBuilderStruct.DoMultiFileSet = true;
                this.DoTaskForDirectory(tempFolder, hootXmlBuilderStruct, e);

                // delete temp folder
                if (Directory.Exists(tempFolder))
                {
                    Directory.Delete(tempFolder, true);
                }
            }
            else
            {
                try
                {
                    using (FileStream fs = File.OpenRead(pPath))
                    {
                        Type dataType = FormatUtil.getHootObjectType(fs);

                        if (dataType != null)
                        {
                            IHootFormat vgmData = (IHootFormat)Activator.CreateInstance(dataType);
                            vgmData.Initialize(fs, pPath);

                            if (!String.IsNullOrEmpty(vgmData.GetHootDriver()))
                            {
                                hootGame = new VGMToolbox.format.hoot.game();

                                string setName = "GAME NAME";
                                string jpName = "(JP PLACE HOLDER)";
                                string companyName = "COMPANY";

                                if (hootXmlBuilderStruct.parseFileName)
                                {
                                    HootXmlFilenameStruct hootXmlFilenameStruct;
                                    hootXmlFilenameStruct = getFilenameStruct(pPath);

                                    setName = hootXmlFilenameStruct.GameName;
                                    jpName = hootXmlFilenameStruct.JpGameName;

                                    if (!String.IsNullOrEmpty(hootXmlFilenameStruct.Company))
                                    {
                                        companyName = hootXmlFilenameStruct.Company;
                                    }
                                    else
                                    {
                                        companyName = "Unknown";
                                    }
                                }
                                else if (!String.IsNullOrEmpty(hootXmlBuilderStruct.OverrideSetName))
                                {
                                    setName = hootXmlBuilderStruct.OverrideSetName;
                                }
                                else if (!String.IsNullOrEmpty(vgmData.GetSongName()))
                                {
                                    setName = vgmData.GetSongName();
                                }

                                hootGame.name = String.Format("[{0}] {1} {2} ({3})",
                                    vgmData.GetHootDriverAlias(), setName, jpName, vgmData.GetHootChips()).Replace("  ", " ");

                                hootGame.driver = new VGMToolbox.format.hoot.driver();
                                hootGame.driver.type = vgmData.GetHootDriverType();
                                hootGame.driver.Value = vgmData.GetHootDriver();

                                hootGame.driveralias = new VGMToolbox.format.hoot.driveralias();
                                hootGame.driveralias.type = vgmData.GetHootDriverAlias();
                                hootGame.driveralias.Value = companyName;

                                hootGame.romlist = new VGMToolbox.format.hoot.romlist();
                                hootGame.romlist.archive = "INSERT ARCHIVE NAME HERE";
                                hootGame.romlist.rom = new VGMToolbox.format.hoot.rom[1];
                                hootGame.romlist.rom[0] = new VGMToolbox.format.hoot.rom();
                                hootGame.romlist.rom[0].type = "code";
                                hootGame.romlist.rom[0].Value = Path.GetFileName(pPath);

                                int totalSongs;

                                // get song count
                                if (hootXmlBuilderStruct.DoMultiFileSet)
                                {
                                    totalSongs = Directory.GetFiles(Path.GetDirectoryName(pPath)).Length;
                                }
                                else
                                {
                                    totalSongs = vgmData.GetTotalSongs();
                                }


                                hootGame.titlelist = new VGMToolbox.format.hoot.title[totalSongs];
                                int j = 0;
                                VGMToolbox.format.hoot.title hootTitle;

                                if (vgmData.UsesPlaylist() ||
                                    File.Exists(Path.ChangeExtension(vgmData.FilePath, NezPlugUtil.M3U_FILE_EXTENSION)))
                                {
                                    NezPlugM3uEntry[] entries = vgmData.GetPlaylistEntries();

                                    foreach (NezPlugM3uEntry en in entries)
                                    {
                                        if (en.songNumber != NezPlugUtil.EMPTY_COUNT)
                                        {
                                            hootTitle = new VGMToolbox.format.hoot.title();
                                            hootTitle.code = "0x" + en.songNumber.ToString("X2");
                                            hootTitle.Value = en.title;

                                            hootGame.titlelist[j] = hootTitle;
                                            j++;
                                        }
                                    }
                                }
                                else if (hootXmlBuilderStruct.DoMultiFileSet)
                                {
                                    // set value for this file
                                    hootTitle = new VGMToolbox.format.hoot.title();
                                    hootTitle.code = "0x00";
                                    hootTitle.Value = String.IsNullOrEmpty(vgmData.GetSongName()) ? ("BGM #" + j.ToString("X2")) : vgmData.GetSongName();
                                    hootGame.titlelist[j++] = hootTitle;
                                    
                                    // loop through remaining files
                                    foreach (string hootFile in Directory.GetFiles(Path.GetDirectoryName(pPath)))
                                    {
                                        if (hootFile != pPath) // this file has already been added
                                        {
                                            using (FileStream hfs = File.OpenRead(hootFile))
                                            {
                                                Type titleType = FormatUtil.getHootObjectType(hfs);

                                                if (titleType != null)
                                                {
                                                    IHootFormat titleData = (IHootFormat)Activator.CreateInstance(titleType);
                                                    titleData.Initialize(hfs, hootFile);

                                                    hootTitle = new VGMToolbox.format.hoot.title();
                                                    hootTitle.code = "0x" + j.ToString("X2"); ;
                                                    hootTitle.Value = String.IsNullOrEmpty(titleData.GetSongName()) ? ("BGM #" + j.ToString("X2")) : titleData.GetSongName();
                                                    hootGame.titlelist[j++] = hootTitle;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < totalSongs; i++)
                                    {
                                        hootTitle = new VGMToolbox.format.hoot.title();
                                        hootTitle.code = "0x" + i.ToString("X2");
                                        hootTitle.Value = "BGM #" + i.ToString("X2");

                                        hootGame.titlelist[j] = hootTitle;
                                        j++;
                                    }
                                }

                                hootGamesArrayList.Add(hootGame);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.progressStruct.Clear();
                    this.progressStruct.ErrorMessage = String.Format("Error processing <{0}>.  Error received: ", pPath) + ex.Message;
                    ReportProgress(this.Progress, this.progressStruct);
                }
            }
        }

        private string getSetNameForMultiFileSet(string pPath)
        {
            string[] files = Directory.GetFiles(pPath);
            string setName = String.Empty;

            // Loop through files until and grab set name from first file possible
            foreach (string f in files)
            {                    
                using (FileStream fs = File.OpenRead(f))
                {
                    Type dataType = FormatUtil.getHootObjectType(fs);

                    if (dataType != null)
                    {
                        IHootFormat vgmData = (IHootFormat)Activator.CreateInstance(dataType);
                        vgmData.Initialize(fs, f);

                        if (vgmData.HasMultipleFilesPerSet())
                        {
                            setName = vgmData.GetSetName();
                            break;
                        }
                    }
                }
            }
        
            return setName;
        }

        private HootXmlFilenameStruct getFilenameStruct(string pPath)
        {
            HootXmlFilenameStruct ret = new HootXmlFilenameStruct();
            string filename = Path.GetFileNameWithoutExtension(pPath);

            // Get Game Name
            int firstParenthesesLocation = filename.IndexOf('(');
            int firstBracketLocation = filename.IndexOf('[');
            int lastBracketLocation = filename.IndexOf(']');
            int gameNameEnd;

            if (firstBracketLocation != -1)
            {
                gameNameEnd = firstParenthesesLocation < firstBracketLocation ? firstParenthesesLocation : firstBracketLocation;
            }
            else
            {
                if (firstParenthesesLocation != -1)
                {
                    gameNameEnd = firstParenthesesLocation;
                }
                else
                {
                    gameNameEnd = filename.Length;
                }
            }

            ret.GameName = filename.Substring(0, gameNameEnd).Trim();

            // JP Game Name
            if (firstBracketLocation != -1 &&
                lastBracketLocation  != -1)
            {
                ret.JpGameName = String.Format("({0})", filename.Substring(firstBracketLocation + 1, (lastBracketLocation - firstBracketLocation) - 1).Trim());
            }
            else
            {
                ret.JpGameName = String.Empty;
            }

            // Company //

            // Last Section
            int lastOpenParenLocation = filename.LastIndexOf('(');
            int lastCloseParenLocation = filename.LastIndexOf(')');
            string lastSection; 

            if (lastOpenParenLocation != -1 &&
                lastCloseParenLocation != -1)
            {
                lastSection = filename.Substring(lastOpenParenLocation + 1, (lastCloseParenLocation - lastOpenParenLocation) - 1).Trim();
            }
            else
            {
                lastSection = String.Empty;
            }

            // Second to Last Section
            int secondLastOpenParenLocation = filename.LastIndexOf('(', lastOpenParenLocation - 1);
            int secondLastCloseParenLocation = filename.LastIndexOf(')', lastCloseParenLocation - 1);
            string secondLastSection;
            double dummyDouble;

            if (secondLastOpenParenLocation != -1 &&
                secondLastCloseParenLocation != -1)
            {
                secondLastSection = filename.Substring(secondLastOpenParenLocation + 1, (secondLastCloseParenLocation - secondLastOpenParenLocation) - 1).Trim();
            }
            else
            {
                secondLastSection = String.Empty;
            }

            if (!String.IsNullOrEmpty(secondLastSection) &&
                !secondLastSection.Trim().Equals("-") &&
                !Double.TryParse(secondLastSection, out dummyDouble))
            { 
                ret.Company = secondLastSection;
            }
            else
            {
                ret.Company = lastSection;
            }

            return ret;
        }

        private bool isValidHootFormat(string pPath)
        {
            bool ret = false;

            using (FileStream fs = File.OpenRead(pPath))
            {
                Type dataType = FormatUtil.getHootObjectType(fs);

                if (dataType != null)
                {
                    ret = true;
                }
            }

            return ret;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {            
            base.OnDoWork(e);
            
            HootXmlBuilderStruct hootXmlBuilderStruct = (HootXmlBuilderStruct)e.Argument;
            VGMToolbox.format.hoot.game[] hootGames = 
                (VGMToolbox.format.hoot.game[])this.hootGamesArrayList.ToArray(typeof(VGMToolbox.format.hoot.game));
            
            if (hootGames.Length > 0)
            {
                string outputPath;

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                settings.Indent = true;
                settings.IndentChars = "\t";
                settings.NewLineChars = Environment.NewLine;
                settings.ConformanceLevel = ConformanceLevel.Document;

                // Use to suppress namespace attributes
                XmlSerializerNamespaces namespaceSerializer = new XmlSerializerNamespaces();
                namespaceSerializer.Add("", "");

                if (hootXmlBuilderStruct.combineOutput)
                {
                    outputPath = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + "hoot" +
                        Path.DirectorySeparatorChar + "[combined] hootgame.xml";
                    
                    XmlSerializer serializer = new XmlSerializer(hootGames.GetType());

                    using (XmlWriter xmlWriter = XmlTextWriter.Create(outputPath, settings))
                    {
                        serializer.Serialize(xmlWriter, hootGames, namespaceSerializer);
                    }                    
                }

                if (hootXmlBuilderStruct.splitOutput)
                {
                    foreach (VGMToolbox.format.hoot.game g in hootGames)
                    {
                        outputPath = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + "hoot" +
                            Path.DirectorySeparatorChar + g.romlist.rom[0].Value + ".xml";

                        XmlSerializer serializer = new XmlSerializer(g.GetType());

                        using (XmlWriter xmlWriter = XmlTextWriter.Create(outputPath, settings))
                        {
                            serializer.Serialize(xmlWriter, g, namespaceSerializer);
                        }
                    }                
                }
            }
        }        
    }
}
