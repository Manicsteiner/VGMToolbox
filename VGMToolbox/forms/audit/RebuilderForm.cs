using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using VGMToolbox.auditing;
using VGMToolbox.format.auditing;
using VGMToolbox.plugin;

namespace VGMToolbox.forms.audit
{
    public partial class RebuilderForm : AVgmtForm
    {        
        public RebuilderForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = 
                ConfigurationManager.AppSettings["Form_AuditRebuilder_Title"];
            this.btnDoTask.Text = 
                ConfigurationManager.AppSettings["Form_AuditRebuilder_DoTaskButton"];
            this.tbOutput.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_IntroText"];
            InitializeComponent();

            grpRebuilder_Directories.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_GroupDirectories"];
            lblRebuilder_SourceDir.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_LblSourceDir"];
            lblRebuilder_DestinationDir.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_LblDestinationDir"];
            grpRebuilder_Datafile.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_GroupDatafile"];
            grpRebuilder_Options.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_GroupOptions"];
            cbRebuilder_RemoveSource.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_CheckBoxRemoveSource"];
            cbRebuilder_Overwrite.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_CheckBoxOverwrite"];
            cbRebuilder_ScanOnly.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_CheckBoxScanOnly"];
            cbRebuilder_CompressOutput.Text =
                ConfigurationManager.AppSettings["Form_AuditRebuilder_CheckBoxCompressOutput"];
        }

        private void btnRebuilder_Rebuild_Click(object sender, EventArgs e)
        {
            if (checkRebuilderInputs())
            {
                datafile dataFile = new datafile();
                XmlSerializer serializer = new XmlSerializer(typeof(datafile));
                TextReader textReader = new StreamReader(tbRebuilder_Datafile.Text);
                dataFile = (datafile)serializer.Deserialize(textReader);
                textReader.Close();

                RebuilderWorker.RebuildSetsStruct vRebuildSetsStruct = new RebuilderWorker.RebuildSetsStruct();
                vRebuildSetsStruct.pSourceDir = tbRebuilder_SourceDir.Text;
                vRebuildSetsStruct.pDestinationDir = tbRebuilder_DestinationDir.Text;
                vRebuildSetsStruct.pDatFile = dataFile;
                vRebuildSetsStruct.pRemoveSource = cbRebuilder_RemoveSource.Checked;
                vRebuildSetsStruct.pOverwriteExisting = cbRebuilder_Overwrite.Checked;
                vRebuildSetsStruct.ScanOnly = cbRebuilder_ScanOnly.Checked;
                vRebuildSetsStruct.pCompressOutput = cbRebuilder_CompressOutput.Checked;

                try
                {
                    vRebuildSetsStruct.totalFiles = Directory.GetFiles(tbRebuilder_SourceDir.Text, "*.*", SearchOption.AllDirectories).Length;
                    
                    base.backgroundWorker_Execute(vRebuildSetsStruct);
                }
                catch (Exception exception2)
                {
                    tbOutput.Text += exception2.Message;
                }
            }
        }

        private void btnRebuilder_BrowseSourceDir_Click(object sender, EventArgs e)
        {
            tbRebuilder_SourceDir.Text = base.browseForFolder(sender, e);
        }
        private void btnRebuilder_BrowseDestinationDir_Click(object sender, EventArgs e)
        {
            tbRebuilder_DestinationDir.Text = base.browseForFolder(sender, e);
        }
        private void btnRebuilder_BrowseDatafile_Click(object sender, EventArgs e)
        {
            tbRebuilder_Datafile.Text = base.browseForFile(sender, e);
        }

        private bool checkRebuilderInputs()
        {
            bool ret = true;

            if (checkTextBox(tbRebuilder_SourceDir.Text, lblRebuilder_SourceDir.Text) &&
                checkTextBox(tbRebuilder_DestinationDir.Text, lblRebuilder_DestinationDir.Text) &&
                checkTextBox(tbRebuilder_Datafile.Text, grpRebuilder_Datafile.Text) &&
                checkFolderExists(tbRebuilder_SourceDir.Text, lblRebuilder_SourceDir.Text) &&
                checkFolderExists(tbRebuilder_DestinationDir.Text, lblRebuilder_DestinationDir.Text) &&
                checkFileExists(tbRebuilder_Datafile.Text, grpRebuilder_Datafile.Text))
            {

                if (tbRebuilder_SourceDir.Text.Trim().Equals(tbRebuilder_DestinationDir.Text.Trim()))
                {
                    MessageBox.Show(ConfigurationManager.AppSettings["Form_AuditRebuilder_ErrorSourceDestSame"]);
                    ret = false;
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }
        private void cbRebuilder_CompressOutput_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRebuilder_CompressOutput.Checked)
            {
                cbRebuilder_Overwrite.Checked = false;
                cbRebuilder_ScanOnly.Checked = false;
            }
        }
        private void cbRebuilder_Overwrite_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRebuilder_Overwrite.Checked)
            {
                cbRebuilder_CompressOutput.Checked = false;
                cbRebuilder_ScanOnly.Checked = false;
            }
        }
        private void cbRebuilder_RemoveSource_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRebuilder_RemoveSource.Checked)
            {
                cbRebuilder_ScanOnly.Checked = false;
            }
        }
        private void cbRebuilder_ScanOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRebuilder_ScanOnly.Checked)
            {
                cbRebuilder_RemoveSource.Checked = false;
                cbRebuilder_Overwrite.Checked = false;
                cbRebuilder_CompressOutput.Checked = false;
            }
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new RebuilderWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_AuditRebuilder_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_AuditRebuilder_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_AuditRebuilder_MessageBegin"];
        }
    }
}
