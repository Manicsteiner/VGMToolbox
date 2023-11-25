using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

using VGMToolbox.format.hoot;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.hoot
{
    class HootAuditorWorker : AVgmtDragAndDropWorker
    {
        private const string FILE_EXTENSION_XML = ".XML";
        private const string ROM_TYPE_SHELL = "SHELL";
        private const string ROM_TYPE_DEVICE = "DEVICE";
        private const string ROM_TYPE_BINARY = "BINARY";
        private const string ROM_TYPE_CONIN = "CONIN";

        public static readonly Color HOOT_MISSING_FILE_COLOR = Color.Red;

        public struct HootAuditorStruct : IVgmtWorkerStruct
        {
            private string[] sourcePaths; // xml paths
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
            
            public string[] SetArchivePaths;
            public bool IncludeSubDirectories;
        }

        struct HootRomCheckStruct
        {
            public HootRomCheckStruct(string pRomName, bool pIsPresent)
            {
                RomName = pRomName;
                IsPresent = pIsPresent;
            }
            
            public string RomName;
            public bool IsPresent;
        }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pHootAuditorStruct, DoWorkEventArgs e)
        {
            HootAuditorStruct hootAuditorStruct = (HootAuditorStruct)pHootAuditorStruct;
            
            if (Path.GetExtension(pPath).ToUpper().Equals(FILE_EXTENSION_XML))
            {
                TreeNode rootNode = new TreeNode(Path.GetFileNameWithoutExtension(pPath));
                Dictionary<string, HootRomCheckStruct[]> archiveContents = new Dictionary<string, HootRomCheckStruct[]>();

                try
                {
                    gamelist hootGames = new gamelist();
                    XmlSerializer serializer = new XmlSerializer(typeof(gamelist));
                    using (FileStream xmlFs = File.OpenRead(pPath))
                    {
                        using (XmlTextReader textReader = new XmlTextReader(xmlFs))
                        {
                            hootGames = (gamelist)serializer.Deserialize(textReader);
                        }
                    }
                        
                    string gameArchiveFileName;
                    TreeNode gameNode;
                    TreeNode romNode;
                    ArrayList archiveContentsStructs;

                    string romNameUpperCase;
                    string romTypeUpperCase;

                    bool romFoundInDictionary;
                    bool romFoundInArchives;
                    HootRomCheckStruct newRom;

                    foreach (game g in hootGames.Items)
                    {                        
                        gameArchiveFileName = g.romlist.archive + ".zip";

                        gameNode = new TreeNode(String.Format("{0} ({1})]", gameArchiveFileName, g.name));                        

                        if (archiveContents.ContainsKey(gameArchiveFileName))
                        {
                            archiveContentsStructs = new ArrayList(archiveContents[gameArchiveFileName]);
                        }
                        else
                        {
                            archiveContentsStructs = new ArrayList();
                        }

                        foreach (rom r in g.romlist.rom)
                        {
                            romNameUpperCase = r.Value.ToUpper();
                            romTypeUpperCase = r.type.ToUpper();

                            if (!romTypeUpperCase.Equals(ROM_TYPE_SHELL) &&
                                !romTypeUpperCase.Equals(ROM_TYPE_DEVICE) &&
                                !romTypeUpperCase.Equals(ROM_TYPE_BINARY) &&
                                !romTypeUpperCase.Equals(ROM_TYPE_CONIN))
                            {                                
                                // Check if Rom is in Dictionary
                                romFoundInDictionary = false;
                                romFoundInArchives = false;
                                
                                foreach (HootRomCheckStruct h in archiveContentsStructs)
                                {
                                    if (h.RomName.ToUpper().Equals(romNameUpperCase))
                                    {
                                        romFoundInDictionary = true;
                                        romFoundInArchives = h.IsPresent;
                                        break;
                                    }
                                }

                                if (!romFoundInDictionary)
                                {
                                    // Add Rom to Dictionary
                                    newRom = new HootRomCheckStruct(r.Value, false);
                                    archiveContentsStructs.Add(newRom);
                                }                                
                            }
                        }

                        // Scan for all roms
                        archiveContents[gameArchiveFileName] = (HootRomCheckStruct[])archiveContentsStructs.ToArray(typeof(HootRomCheckStruct));
                        this.checkIfRomsArePresent(hootAuditorStruct.SetArchivePaths, ref archiveContents, gameArchiveFileName, hootAuditorStruct.IncludeSubDirectories);

                        foreach (HootRomCheckStruct romCheckStruct in archiveContents[gameArchiveFileName])
                        {                            
                            romNode = new TreeNode(romCheckStruct.RomName);
                            
                            gameNode.Nodes.Add(romNode);

                            if (!romCheckStruct.IsPresent)
                            {
                                romNode.ForeColor  = HOOT_MISSING_FILE_COLOR;
                                gameNode.ForeColor = HOOT_MISSING_FILE_COLOR;
                                rootNode.ForeColor = HOOT_MISSING_FILE_COLOR;                            
                            }
                        }

                        rootNode.Nodes.Add(gameNode);
                    }

                    this.progressStruct.Clear();
                    this.progressStruct.FileName = pPath;
                    this.progressStruct.NewNode = rootNode;
                    ReportProgress(this.progress, this.progressStruct);

                }
                catch (Exception ex)
                {
                    this.progressStruct.Clear();
                    this.progressStruct.ErrorMessage = String.Format("Error processing <{0}>.  Error received: ", pPath) + ex.Message;
                    ReportProgress(this.Progress, this.progressStruct);
                }
            }
        }

        private void checkIfRomsArePresent(string[] pArchivePaths, ref Dictionary<string, HootRomCheckStruct[]> pHootSet, 
            string pSetArchiveName, bool pIncludeSubDirectories)
        {
            ArrayList archiveContents;
            string[] setArchiveSplitParameters = {","};
            string[] setArchiveNames = pSetArchiveName.Split(setArchiveSplitParameters, StringSplitOptions.RemoveEmptyEntries);

            foreach (string path in pArchivePaths)
            {
                if (Directory.Exists(path))
                {
                    string[] archiveFiles;
                    string archiveName;

                    foreach (string splitArchiveName in setArchiveNames)
                    {
                        archiveName = splitArchiveName;
                        
                        // add .zip if needed for split archives
                        if (!archiveName.EndsWith(".zip"))
                        {
                            archiveName = archiveName + ".zip";
                        }
                        
                        if (pIncludeSubDirectories)
                        {
                            archiveFiles = Directory.GetFiles(path, archiveName, SearchOption.AllDirectories);
                        }
                        else
                        {
                            archiveFiles = Directory.GetFiles(path, archiveName, SearchOption.TopDirectoryOnly);
                        }

                        if (archiveFiles.Length > 1)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.GenericMessage = String.Format("WARNING: Multiple copies of {0} found, results may be inaccurate", pSetArchiveName);
                            ReportProgress(this.progress, this.progressStruct);
                        }

                        foreach (string file in archiveFiles)
                        {
                            archiveContents = new ArrayList(CompressionUtil.GetUpperCaseFileList(file));

                            for (int i = 0; i < pHootSet[pSetArchiveName].Length; i++)
                            {
                                if (archiveContents.Contains(pHootSet[pSetArchiveName][i].RomName.Replace('/', Path.DirectorySeparatorChar).ToUpper()))
                                {
                                    pHootSet[pSetArchiveName][i].IsPresent = true;
                                }
                            }
                        }
                    }
                }
            }            
        }
    }
}
