using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools
{
    class TreeBuilderWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        public static readonly string EXAMINE_OUTPUT_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "examine"), "examine.txt");
        
        private int fileCount = 0;
        private int maxFiles = 0;
        private VGMToolbox.util.ProgressStruct progressStruct;

        public struct TreeBuilderStruct
        {
            public string[] pPaths;
            public int totalFiles;
            public bool checkForLibs;
        }

        public TreeBuilderWorker()
        {
            fileCount = 0;
            maxFiles = 0;
            progressStruct = new VGMToolbox.util.ProgressStruct();

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        private void buildTree(TreeBuilderStruct pTreeBuilderStruct, DoWorkEventArgs e)
        {
            TreeNode t;
            
            StreamWriter sw = null;
            sw = File.CreateText(EXAMINE_OUTPUT_PATH);

            foreach (string path in pTreeBuilderStruct.pPaths)
            {
                t = null;

                if (File.Exists(path))
                {
                    if (!CancellationPending)
                    {
                        t = this.getFileNode(path, sw, pTreeBuilderStruct.checkForLibs, e);
                    }
                    else 
                    {
                        e.Cancel = true;

                        if (sw != null)
                        {
                            sw.Close();
                            sw.Dispose();
                        }
                        return;
                    }
                }
                else if (Directory.Exists(path))
                {
                    t = new TreeNode(Path.GetFileName(path));
                    this.getDirectoryNode(path, t, sw, pTreeBuilderStruct.checkForLibs, e);

                    if (CancellationPending)
                    {
                        e.Cancel = true;

                        if (sw != null)
                        {
                            sw.Close();
                            sw.Dispose();
                        }
                        
                        return;                        
                    }
                }
                
                // Add node, but ignore progress
                this.progressStruct.Clear();
                this.progressStruct.NewNode = (TreeNode)t.Clone();
                ReportProgress(Constants.IgnoreProgress, this.progressStruct);                
            }

            sw.Close();
            sw.Dispose();
        }

        private void getDirectoryNode(string pDirectory, TreeNode pTreeNode, StreamWriter pOutputFileStream, 
            bool pCheckForLibs, DoWorkEventArgs e)
        {
            TreeNode t;
            
            foreach (string d in Directory.GetDirectories(pDirectory))
            {
                if (!CancellationPending)
                {
                    t = new TreeNode(Path.GetFileName(d));
                    this.getDirectoryNode(d, t, pOutputFileStream, pCheckForLibs, e);
                    pTreeNode.Nodes.Add(t);

                    if (t.ForeColor == Color.Red)
                    {
                        pTreeNode.ForeColor = Color.Red;
                    }
                }
                else
                {
                    e.Cancel = true;
                    break;
                }
            }                        
            
            foreach (string f in Directory.GetFiles(pDirectory))
            {
                if (!CancellationPending)
                {
                    t = this.getFileNode(f, pOutputFileStream, pCheckForLibs, e);
                    pTreeNode.Nodes.Add(t);

                    if (t.ForeColor == Color.Red)
                    {
                        pTreeNode.ForeColor = Color.Red;
                    }
                }
                else
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private TreeNode getFileNode(string pFileName, StreamWriter pOutputFileStream, 
            bool pCheckForLibs, DoWorkEventArgs e)
        {
            int progress = (++fileCount * 100) / maxFiles;
            this.progressStruct.Clear();
            this.progressStruct.FileName = pFileName;
            ReportProgress(progress, this.progressStruct);

            TreeNode ret = new TreeNode(Path.GetFileName(pFileName));            
            VGMToolbox.util.NodeTagStruct nodeTag = new VGMToolbox.util.NodeTagStruct();
            TreeNode tagNode;
            string tagHashValue = String.Empty;

            using (FileStream fs = File.OpenRead(pFileName))
            {                
                try
                {
                    Type dataType = FormatUtil.getObjectType(fs);

                    if (dataType != null)
                    {
                        pOutputFileStream.WriteLine(pFileName);

                        IFormat vgmData = (IFormat)Activator.CreateInstance(dataType);
                        vgmData.Initialize(fs, pFileName);
                        Dictionary<string, string> tagHash = vgmData.GetTagHash();
                        
                        // Add Path for possible future use.
                        tagHash.Add("Path", vgmData.FilePath);

                        // check for libs
                        if (pCheckForLibs && vgmData.UsesLibraries())
                        {
                            if (!vgmData.IsLibraryPresent())
                            {
                                ret.ForeColor = Color.Red;
                                ret.Text += " (Missing Library)";
                            }
                        }

                        char[] trimNull = new char[] { '\0' };
                        
                        foreach (string s in tagHash.Keys)
                        {
                            tagHashValue = tagHash[s];

                            if (!String.IsNullOrEmpty(tagHashValue))
                            {
                                tagHashValue = tagHashValue.TrimEnd(trimNull);
                            }
                            else
                            {
                                tagHashValue = String.Empty;
                            }

                            tagNode = new TreeNode(s + ": " + tagHashValue);
                            ret.Nodes.Add(tagNode);

                            pOutputFileStream.WriteLine(s + ": " + tagHashValue);
                        }

                        pOutputFileStream.WriteLine(Environment.NewLine);

                        // add classname to nodeTag
                        nodeTag.ObjectType = dataType.AssemblyQualifiedName;
                    }
                }
                catch (Exception ex)
                {
                    this.progressStruct.Clear();
                    this.progressStruct.ErrorMessage = String.Format("Error processing <{0}>.  Error received: ", pFileName) + ex.Message;
                    ReportProgress(progress, this.progressStruct);
                }
            } // using (FileStream fs = File.OpenRead(pFileName))

            nodeTag.FilePath = pFileName;
            ret.Tag = nodeTag;

            return ret;
        }
     
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            TreeBuilderStruct treeBuilderStruct = (TreeBuilderStruct)e.Argument;
            maxFiles = treeBuilderStruct.totalFiles;
            this.buildTree(treeBuilderStruct, e);
        }    
    }
}
