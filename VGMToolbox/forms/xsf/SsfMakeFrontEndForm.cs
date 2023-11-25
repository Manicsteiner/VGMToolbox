using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class SsfMakeFrontEndForm : AVgmtForm
    {        
        public SsfMakeFrontEndForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_SsfMakeFE_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_SsfMakeFE_DoTaskButton"];
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_SsfMakeFE_IntroText1"] + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_SsfMakeFE_IntroText2"] + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_SsfMakeFE_IntroText3"] + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_SsfMakeFE_IntroText4"] + Environment.NewLine;

            InitializeComponent();

            this.groupFiles.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_GroupFiles"];
            this.lblDriver.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblDriver"];
            this.lblSourcePath.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblSourcePath"];
            this.lblOutputFolder.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblOutputFolder"];
            this.lblSingleDspFile.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblSingleDspFile"];
            this.grpSettings.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_GroupSettings"];
            this.lblSequenceBank.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblSequenceBank"];
            this.lblSequenceTrack.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblSequenceTrack"];
            this.lblVolume.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblVolume"];
            this.lblMixerBank.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblMixerBank"];
            this.lblMixerNumber.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblMixerNumber"];
            this.lblEffect.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_LblEffect"];
            this.cbMatchSeqBank.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_CheckBoxMatchSeqBank"];
            this.cbSeekData.Text =
                ConfigurationManager.AppSettings["Form_SsfMakeFE_CheckBoxSeekData"];

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            SsfMakeWorker.SsfMakeStruct smStruct = new SsfMakeWorker.SsfMakeStruct();
            smStruct.sequenceBank = tbSequenceBank.Text;
            smStruct.sequenceTrack = tbSequenceTrack.Text;
            smStruct.volume = tbVolume.Text;
            smStruct.mixerBank = tbMixerBank.Text;
            smStruct.mixerNumber = tbMixerNumber.Text;
            smStruct.effectNumber = tbEffect.Text;
            smStruct.useDsp = "0"; // logic within worker will set this
            smStruct.driver = tbDriver.Text;
            smStruct.customMapFile = tbCustomMapFile.Text.Trim();
            smStruct.dspFile = tbDspFile.Text;

            smStruct.sourcePath = tbSourcePath.Text;
            smStruct.outputFolder = tbOutputFolder.Text;
            smStruct.findData = cbSeekData.Checked;
            smStruct.TryAllCombinations = cbTryAllCombinations.Checked;

            base.backgroundWorker_Execute(smStruct);
        }

        private void tbSequenceBank_TextChanged(object sender, EventArgs e)
        {
            if (cbMatchSeqBank.Checked)
            {
                tbMixerBank.Text = tbSequenceBank.Text;
            }
        }
        private void cbMatchSeqBank_CheckedChanged(object sender, EventArgs e)
        {            
            if (cbMatchSeqBank.Checked)
            {
                tbMixerBank.Text = tbSequenceBank.Text;
            }

            tbMixerBank.ReadOnly = cbMatchSeqBank.Checked;
        }
        private void btnBrowseDriver_Click(object sender, EventArgs e)
        {
            tbDriver.Text = base.browseForFile(sender, e);
        }
        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            tbSourcePath.Text = base.browseForFolder(sender, e);
        }
        private void btnBrowseDsp_Click(object sender, EventArgs e)
        {
            tbDspFile.Text = base.browseForFile(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new SsfMakeWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_SsfMakeFE_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_SsfMakeFE_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_SsfMakeFE_MessageBegin"];
        }

        private void tbDriver_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void tbDriver_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (File.Exists(s[0])))
            {
                this.tbDriver.Text = s[0];
            }
        }

        private void tbSourcePath_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void tbSourcePath_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (Directory.Exists(s[0])))
            {
                this.tbSourcePath.Text = s[0];
            }
        }

        private void tbDspFile_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void tbDspFile_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (File.Exists(s[0])))
            {
                this.tbDspFile.Text = s[0];
            }
        }

        private void btnBrowseCustomMap_Click(object sender, EventArgs e)
        {
            this.tbCustomMapFile.Text = base.browseForFile(sender, e);
        }

        private void tbCustomMapFile_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void tbCustomMapFile_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (File.Exists(s[0])))
            {
                this.tbCustomMapFile.Text = s[0];
            }

        }
    }
}
