using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using VGMToolbox.auditing;
using VGMToolbox.format.auditing;
using VGMToolbox.plugin;

namespace VGMToolbox.forms.audit
{
    public partial class DatafileEditorForm : TreeViewVgmtForm
    {
        private datafile datafileObject;

        public DatafileEditorForm(TreeNode pTreeNode) : 
            base(pTreeNode)
        {
            InitializeComponent();

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_DatafileEditor_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_DatafileEditor_DoTaskButton"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_DatafileEditor_IntroText"];
        }

        /// <summary>
        /// Browse to the source file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseSourceButton_Click(object sender, EventArgs e)
        {
            this.datafileSourcePath.Text = base.browseForFile(sender, e);

            if ((!String.IsNullOrEmpty(this.datafileSourcePath.Text)) &&
                (TreeViewVgmtForm.checkFileExists(this.datafileSourcePath.Text, this.grpSource.Text)))
            {
                try
                {
                    this.datafileObject = this.getDataFileObject();
                    this.loadDataFileIntoTree(this.datafileObject);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        String.Format(ConfigurationManager.AppSettings["Form_DatafileEditor_ErrorDatafileParse"], ex.Message, Environment.NewLine), "Error");
                }
            }
        }

        private datafile getDataFileObject()
        {
            datafile datafileObject = new datafile();
            XmlSerializer serializer = new XmlSerializer(typeof(datafile));            

            using (FileStream xmlFs = File.OpenRead(this.datafileSourcePath.Text))
            {
                using (XmlTextReader textReader = new XmlTextReader(xmlFs))
                {
                    datafileObject = (datafile)serializer.Deserialize(textReader);
                }
            }

            return datafileObject;
        }

        private void loadDataFileIntoTree(datafile datafileObject)
        {
            TreeNode workingGameNode;
            TreeNode workingRomNode;

            TreeNode workingGameNameNode;
            TreeNode workingRomSizeNode;
            TreeNode workingRomCrcNode;
            TreeNode workingRomMd5Node;
            TreeNode workingRomSha1Node;

            // clear tree
            this.treeViewTools.Nodes.Clear();

            foreach (game g in datafileObject.game)
            {
                workingGameNode = new TreeNode(g.description);
                workingGameNode.Tag = g.DeepCopy();

                workingGameNameNode = new TreeNode(String.Format("Folder: {0}", g.name));
                workingGameNode.Nodes.Add(workingGameNameNode);

                if (g.rom != null)
                {
                    foreach (rom r in g.rom)
                    {
                        workingRomNode = new TreeNode(r.name);
                        workingRomNode.Tag = r.DeepCopy();

                        workingRomSizeNode = new TreeNode(String.Format("Size: {0}", r.size));
                        workingRomCrcNode = new TreeNode(String.Format("CRC32: {0}", r.crc));
                        workingRomMd5Node = new TreeNode(String.Format("MD5: {0}", r.md5));
                        workingRomSha1Node = new TreeNode(String.Format("SHA1: {0}", r.sha1));

                        workingRomNode.Nodes.Add(workingRomSizeNode);
                        workingRomNode.Nodes.Add(workingRomCrcNode);
                        workingRomNode.Nodes.Add(workingRomMd5Node);
                        workingRomNode.Nodes.Add(workingRomSha1Node);

                        workingGameNode.Nodes.Add(workingRomNode);
                    }
                }

                if (g.disk != null)
                {
                    foreach (disk d in g.disk)
                    {
                        workingRomNode = new TreeNode(d.name);
                        workingRomNode.Tag = d.DeepCopy();

                        workingGameNode.Nodes.Add(workingRomNode);
                    }
                }

                this.treeViewTools.Nodes.Add(workingGameNode);
            }
        }

        protected override void treeViewTools_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.selectedNode = treeViewTools.GetNodeAt(e.X, e.Y);
                this.oldNode = treeViewTools.SelectedNode;
                treeViewTools.SelectedNode = this.selectedNode;
                treeViewTools.LabelEdit = true;

                if (this.selectedNode != null &&
                    ((this.selectedNode.Tag is game) || (this.selectedNode.Tag is rom) ||
                     (this.selectedNode.Tag is disk)))
                {
                    this.treeNodeContextMenuStrip.Show(treeViewTools, e.X, e.Y);
                }
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedNode != null)
            {
                treeViewTools.SelectedNode = selectedNode;
                treeViewTools.LabelEdit = true;
                if (!selectedNode.IsEditing)
                {
                    selectedNode.BeginEdit();
                }
            }
            else
            {
                MessageBox.Show(ConfigurationManager.AppSettings["Form_DatafileEditor_ErrorDatafileSelectedNode"], 
                    ConfigurationManager.AppSettings["Form_DatafileEditor_ErrorDatafileSelectedNodeTitle"]);
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedNode != null)
            {
                treeViewTools.SelectedNode = selectedNode;

                game sourceGame = null;
                rom sourceRom = null;

                if (selectedNode.Tag is rom)
                {
                    sourceRom = (rom)selectedNode.Tag;
                    sourceGame = (game)selectedNode.Parent.Tag;
                }
                else
                {
                    sourceGame = (game)selectedNode.Tag;
                }

                this.datafileObject = AuditingUtil.DeleteItemFromDatafile(this.datafileObject, sourceGame, sourceRom);
                this.loadDataFileIntoTree(this.datafileObject);
            }
            else
            {
                MessageBox.Show(ConfigurationManager.AppSettings["Form_DatafileEditor_ErrorDatafileSelectedNode"],
                    ConfigurationManager.AppSettings["Form_DatafileEditor_ErrorDatafileSelectedNodeTitle"]);
            }
        }

        private void btnDoTask_Click(object sender, EventArgs e)
        {
            string saveFilePath = base.browseForFileToSave(sender, e);

            if (!String.IsNullOrEmpty(saveFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(this.datafileObject.GetType());

                using (TextWriter textWriter = new StreamWriter(saveFilePath))
                {
                    serializer.Serialize(textWriter, this.datafileObject);

                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            throw new NotImplementedException();
        }
        protected override string getCancelMessage()
        {
            throw new NotImplementedException();
        }
        protected override string getCompleteMessage()
        {
            throw new NotImplementedException();
        }
        protected override string getBeginMessage()
        {
            throw new NotImplementedException();
        }


    }
}
