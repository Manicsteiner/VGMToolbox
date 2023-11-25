using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class Mkpsf2FrontEndForm : AVgmtForm
    {        
        public Mkpsf2FrontEndForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_MkPsf2FE_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_MkPsf2FE_DoTaskButton"];

            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_MkPsf2FE_IntroText1"] + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_MkPsf2FE_IntroText2"] + Environment.NewLine + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_MkPsf2FE_IntroText3"];

            InitializeComponent();

            this.grpDirectory.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_GroupDirectory"];
            this.lblSourceDirectory.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_LblSourceDirectory"];
            this.lblModulesDirectory.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_LblModulesDirectory"];
            this.lblOutputFolder.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_LblOutputFolder"];
            this.grpOptions.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_GroupOptions"];
            this.lblReverb.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_LblReverb"];
            this.lblDepth.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_LblDepth"];
            this.lblVolume.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_LblVolume"];
            this.lblTempo.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_LblTempo"];
            this.lblTickInterval.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_LblTickInterval"];
            this.lblAuthor.Text =
                ConfigurationManager.AppSettings["Form_MkPsf2FE_LblAuthor"];
            
            this.lblPsf2LibName.Text = "PSF2Lib Name";
            this.cbMakePsf2Lib.Text = "Create .psf2lib for HD/BD data";

            this.cbTryCombinations.Text = "Try all combinations of SQ and HD/BD (good for finding matching pairs).";

            this.setGuiForPsf2Lib();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if ((cbMakePsf2Lib.Checked) && (!tbPsf2LibName.Text.EndsWith(MkPsf2Worker.PSF2LIB_FILE_EXTENSION)))
            {
                tbPsf2LibName.Text += MkPsf2Worker.PSF2LIB_FILE_EXTENSION;
            }
            
            MkPsf2Worker.MkPsf2Struct mkStruct = new MkPsf2Worker.MkPsf2Struct();
            mkStruct.sourcePath = tbSourceDirectory.Text;
            mkStruct.modulePath = tbModulesDirectory.Text;
            mkStruct.outputFolder = tbOutputFolderName.Text;

            mkStruct.depth = tbDepth.Text;
            mkStruct.reverb = tbReverb.Text;
            mkStruct.tempo = tbTempo.Text;
            mkStruct.tickInterval = tbTickInterval.Text;
            mkStruct.volume = tbVolume.Text;

            mkStruct.TryCombinations = cbTryCombinations.Checked;
            mkStruct.CreatePsf2lib = cbMakePsf2Lib.Checked;
            mkStruct.Psf2LibName = tbPsf2LibName.Text;

            base.backgroundWorker_Execute(mkStruct);
        }

        private void btnSourceDirectoryBrowse_Click(object sender, EventArgs e)
        {
            tbSourceDirectory.Text = base.browseForFolder(sender, e);
        }
        private void btnModulesDirectoryBrowse_Click(object sender, EventArgs e)
        {
            tbModulesDirectory.Text = base.browseForFolder(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new MkPsf2Worker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_MkPsf2FE_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_MkPsf2FE_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_MkPsf2FE_MessageBegin"];
        }

        private void tbSourceDirectory_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void tbSourceDirectory_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (Directory.Exists(s[0])))
            {
                this.tbSourceDirectory.Text = s[0];
            }
        }

        private void tbModulesDirectory_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void tbModulesDirectory_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (Directory.Exists(s[0])))
            {
                this.tbModulesDirectory.Text = s[0];
            }
        }

        private void cbMakePsf2Lib_CheckedChanged(object sender, EventArgs e)
        {
            this.setGuiForPsf2Lib();
        }

        private void setGuiForPsf2Lib()
        {
            if (cbMakePsf2Lib.Checked)
            {
                this.tbPsf2LibName.Enabled = true;
                this.tbPsf2LibName.ReadOnly = false;
                
                this.cbTryCombinations.Checked = false;
                this.cbTryCombinations.Enabled = false;
            }
            else
            {
                this.tbPsf2LibName.Enabled = false;
                this.tbPsf2LibName.ReadOnly = true;
                this.tbPsf2LibName.Clear();

                this.cbTryCombinations.Enabled = true;
            }
        }

        private void cbTryCombinations_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbTryCombinations.Checked)
            {
                this.cbMakePsf2Lib.Enabled = false;
                this.cbMakePsf2Lib.Checked = false;
            }
            else
            {
                this.cbMakePsf2Lib.Enabled = true;
            }

            this.setGuiForPsf2Lib();
        }
    }
}
