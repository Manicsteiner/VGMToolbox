using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.forms.examine;
using VGMToolbox.plugin;
using VGMToolbox.util;
using VGMToolbox.tools;

namespace VGMToolbox.forms
{
    public abstract partial class TreeViewVgmtForm : AVgmtForm
    {
        protected TreeNode selectedNode;
        protected TreeNode oldNode;
        
        protected bool outputToText;
        protected string outputTextFile;
        protected NodePrintMessageStruct[] outputColorToMessageRules;

        protected struct NodePrintMessageStruct
        { 
            public Color NodeColor;
            public string Message;
        }

        public TreeViewVgmtForm() : base() 
        {
            InitializeComponent();
        }
        
        public TreeViewVgmtForm(TreeNode pTreeNode) : base(pTreeNode)
        {
            InitializeComponent();
            this.outputToText = false;
        }

        protected override void backgroundWorker_ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != Constants.IgnoreProgress &&
                e.ProgressPercentage != Constants.ProgressMessageOnly)
            {
                toolStripProgressBar.Value = e.ProgressPercentage;
            }

            if ((e.ProgressPercentage == Constants.ProgressMessageOnly) && e.UserState != null)
            {
                VGMToolbox.util.ProgressStruct vProgressStruct = (VGMToolbox.util.ProgressStruct)e.UserState;
                tbOutput.Text += vProgressStruct.GenericMessage;
            }
            else if (e.UserState != null)
            {
                VGMToolbox.util.ProgressStruct vProgressStruct = (VGMToolbox.util.ProgressStruct)e.UserState;

                if (vProgressStruct.NewNode != null)
                {
                    treeViewTools.Nodes.Add(vProgressStruct.NewNode);

                    if (this.outputToText)
                    {
                        using (StreamWriter sw =
                            new StreamWriter(File.Open(this.outputTextFile, FileMode.Append, FileAccess.Write), System.Text.Encoding.Unicode))
                        {
                            this.writeNodesToFile(vProgressStruct.NewNode, sw, 0);
                        }
                    }
                }

                lblProgressLabel.Text = vProgressStruct.FileName ?? String.Empty;

                if (!String.IsNullOrEmpty(vProgressStruct.ErrorMessage))
                {
                    tbOutput.Text += vProgressStruct.ErrorMessage;
                    errorFound = true;
                } 
            }

            this.showElapsedTime();
        }

        protected override void initializeProcessing()
        {
            base.initializeProcessing();
            treeViewTools.Nodes.Clear();
        }

        protected virtual void treeViewTools_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                TreeNode node = treeViewTools.GetNodeAt(p);
                this.selectedNode = node;
                this.oldNode = treeViewTools.SelectedNode;
                treeViewTools.SelectedNode = node;

                if (node != null && node.Tag is VGMToolbox.util.NodeTagStruct)
                {
                    VGMToolbox.util.NodeTagStruct nts = (VGMToolbox.util.NodeTagStruct)node.Tag;

                    if (typeof(IEmbeddedTagsFormat).IsAssignableFrom(Type.GetType(nts.ObjectType)) ||
                        typeof(ISingleTagFormat).IsAssignableFrom(Type.GetType(nts.ObjectType)) ||
                        typeof(IXsfTagFormat).IsAssignableFrom(Type.GetType(nts.ObjectType)) ||
                        typeof(IGd3TagFormat).IsAssignableFrom(Type.GetType(nts.ObjectType)))
                    {
                        contextMenuStrip1.Show(treeViewTools, p);
                    }
                }
            }
        }

        private void filePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedNode != null && this.selectedNode.Tag is VGMToolbox.util.NodeTagStruct)
            {
                VGMToolbox.util.NodeTagStruct nts = (VGMToolbox.util.NodeTagStruct) this.selectedNode.Tag;

                if (typeof(IEmbeddedTagsFormat).IsAssignableFrom(Type.GetType(nts.ObjectType)))
                {
                    EmbeddedTagsUpdateForm etuForm = new EmbeddedTagsUpdateForm(nts);
                    etuForm.Show();
                }
                else if (typeof(ISingleTagFormat).IsAssignableFrom(Type.GetType(nts.ObjectType)))
                {
                    SingleTagUpdateForm stuForm = new SingleTagUpdateForm(nts);
                    stuForm.Show();
                }
                else if (typeof(IXsfTagFormat).IsAssignableFrom(Type.GetType(nts.ObjectType)))
                {
                    XsfTagsUpdateForm xtuForm = new XsfTagsUpdateForm(nts);
                    xtuForm.Show();
                }
                else if (typeof(IGd3TagFormat).IsAssignableFrom(Type.GetType(nts.ObjectType)))
                {
                    VgmTagsUpdateForm gd3uForm = new VgmTagsUpdateForm(nts);
                    gd3uForm.Show();
                }
                                
                this.selectedNode = this.oldNode;
                this.oldNode = null;
            }
        }

        private void writeNodesToFile(TreeNode pTreeNode, StreamWriter pStreamWriter, int pDepth)
        {
            int padAmount = pDepth * 4;
            StringBuilder sbPadding = new StringBuilder();
            StringBuilder sbMessageSuffix = new StringBuilder();

            for (int i = 0; i <padAmount; i++)
            {
                sbPadding.Append(" ");
            }
            
            foreach (NodePrintMessageStruct n in this.outputColorToMessageRules)
            {
                if (pTreeNode.ForeColor.Equals(n.NodeColor))
                {
                    sbMessageSuffix.Append(n.Message);
                }
            }

            pStreamWriter.WriteLine(String.Format("{0}{1} {2}", sbPadding.ToString(), pTreeNode.Text.Trim(), sbMessageSuffix.ToString()));

            foreach (TreeNode t in pTreeNode.Nodes)
            {
                this.writeNodesToFile(t, pStreamWriter, pDepth + 1);
            }
        }
    }
}
