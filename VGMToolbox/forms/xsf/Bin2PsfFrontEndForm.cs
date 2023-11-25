using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class Bin2PsfFrontEndForm : AVgmtForm
    {
        public Bin2PsfFrontEndForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_Bin2PsfFE_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_Bin2PsfFE_DoTaskButton"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_Bin2PsfFE_IntroText"];

            InitializeComponent();

            this.grpSource.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_GroupSource"];
            this.lblDriverPath.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_LblDriverPath"];
            this.lblSourceFiles.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_LblSourceFiles"];
            this.lblOutputFolder.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_LblOutputFolder"];
            this.lblPsfLibName.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_LblPsfLibName"];
            this.cbMinipsf.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_CheckBoxMinipsf"];
            this.grpOptions.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_GroupOptions"];
            this.lblSeqOffset.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_LblSeqOffset"];
            this.lblVhOffset.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_LblVhOffset"];
            this.lblVbOffset.Text =
                ConfigurationManager.AppSettings["Form_Bin2PsfFE_LblVbOffset"];

            this.grpGenericDrivers.Text = ConfigurationManager.AppSettings["Form_Bin2PsfFE_GrpGenericDrivers"];
            this.lblGenericDriver.Text = ConfigurationManager.AppSettings["Form_Bin2PsfFE_LblGenericDriver"];

            this.loadGenericDriversList();

            //this.grpGenericDrivers.Hide();
        }

        private void btnDoTask_Click(object sender, EventArgs e)
        {
            if ((cbMinipsf.Checked) && (!tbPsflibName.Text.EndsWith(Bin2PsfWorker.PSFLIB_FILE_EXTENSION)))
            {
                tbPsflibName.Text += Bin2PsfWorker.PSFLIB_FILE_EXTENSION;
            }

            if (this.validateInputs())
            {
                Bin2PsfWorker.Bin2PsfStruct bpStruct = new Bin2PsfWorker.Bin2PsfStruct();
                bpStruct.sourcePath = tbSourceFilesPath.Text;
                bpStruct.seqOffset = tbSeqOffset.Text;
                bpStruct.vbOffset = tbVbOffset.Text;
                bpStruct.vhOffset = tbVhOffset.Text;
                bpStruct.exePath = tbExePath.Text;
                bpStruct.outputFolder = tbOutputFolderName.Text;
                bpStruct.MakePsfLib = cbMinipsf.Checked;
                bpStruct.TryCombinations = this.cbTryMixing.Checked;
                bpStruct.DriverName = (string)this.genericDriver.SelectedItem;
                bpStruct.psflibName = tbPsflibName.Text;
                bpStruct.SeqSize = this.tbMySeqSize.Text;
                bpStruct.ParamOffset = this.tbParamOffset.Text;

                bpStruct.ForceSepTrackNo = cbForceSepTrackNo.Checked;
                bpStruct.SepTrackNo = (int)sepTrackUpDown.Value;

                base.backgroundWorker_Execute(bpStruct);
            }
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void btnExeBrowse_Click(object sender, EventArgs e)
        {
            tbExePath.Text = base.browseForFile(sender, e);
        }
        private void btnSourceDirectoryBrowse_Click(object sender, EventArgs e)
        {
            tbSourceFilesPath.Text = base.browseForFolder(sender, e);
        }
        private void cbMinipsf_CheckedChanged(object sender, EventArgs e)
        {
            this.doMiniPsfCheckChange();
        }
        private void doMiniPsfCheckChange()
        {
            string selectedItem = (string)this.genericDriver.SelectedItem;

            if (cbMinipsf.Checked)
            {
                this.tbPsflibName.ReadOnly = false;
                this.tbPsflibName.Enabled = true;
                this.cbTryMixing.Checked = false;
                this.cbTryMixing.Enabled = false;                
            }
            else
            {
                tbPsflibName.ReadOnly = true;
                this.cbTryMixing.Enabled = true;
                tbPsflibName.Clear();
            }

            if (String.IsNullOrEmpty((string)this.genericDriver.SelectedItem))
            {
                this.tbMySeqSize.Enabled = true;
                this.tbMySeqSize.ReadOnly = false;
            }
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new Bin2PsfWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_Bin2PsfFE_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_Bin2PsfFE_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_Bin2PsfFE_MessageBegin"];
        }

        private void loadGenericDriversList()
        {
            this.genericDriver.Items.Add(String.Empty);
            this.genericDriver.Items.Add(Bin2PsfWorker.STUB_BUILDER);                        
            this.genericDriver.Items.Add(Bin2PsfWorker.GENERIC_DRIVER_MGRASS);
            this.genericDriver.Items.Add(Bin2PsfWorker.GENERIC_DRIVER_MGRASS_300);
            this.genericDriver.Items.Add(Bin2PsfWorker.GENERIC_DRIVER_MGRASS_422);
            this.genericDriver.Items.Add(Bin2PsfWorker.GENERIC_DRIVER_DAVIRONICA);
        }
        private void loadGenericDriverPreset()
        {
            string selectedItem = (string)this.genericDriver.SelectedItem;

            switch (selectedItem)
            {
                case Bin2PsfWorker.STUB_BUILDER:
                    this.disablePresetFields();
                    this.loadStubBuilderPresets();
                    break;
                case Bin2PsfWorker.GENERIC_DRIVER_MGRASS:
                    this.disablePresetFields();
                    this.loadMarkGrassGenericPresets();
                    break;
                case Bin2PsfWorker.GENERIC_DRIVER_MGRASS_300:
                    this.disablePresetFields();
                    this.loadMarkGrass300GenericPresets();
                    break;
                case Bin2PsfWorker.GENERIC_DRIVER_MGRASS_422:
                    this.disablePresetFields();
                    this.loadMarkGrass422GenericPresets();
                    break;
                case Bin2PsfWorker.GENERIC_DRIVER_DAVIRONICA:
                    this.disablePresetFields();
                    this.loadDavironicaGenericPresets();
                    break;
                default:
                    this.enablePresetFields();
                    break;
            }
        }
        private void loadStubBuilderPresets()
        {
            this.tbExePath.Enabled = true;
            this.tbExePath.ReadOnly = false;
            this.btnExeBrowse.Enabled = true;
            
            this.tbSeqOffset.Text = "0x80120000";
            this.tbMySeqSize.Text = "0x00010000";
            this.tbVhOffset.Text =  "0x80130000";
            this.tbVbOffset.Text =  "0x80140000";
            this.tbParamOffset.Text = "0x80101000";

            this.cbMinipsf.Enabled = true;
        }
        private void loadMarkGrassGenericPresets()
        {            
            this.tbExePath.Text = Bin2PsfWorker.MGRASS_EXE_PATH;
            this.tbPsflibName.Clear();

            this.tbSeqOffset.Text = "0x800A0000";
            this.tbMySeqSize.Text = "0x00040000";
            this.tbVhOffset.Text = "0x800E0000";
            this.tbVbOffset.Text = "0x80160000";
            this.tbParamOffset.Text = "0";

            this.tbExePath.Enabled = false;
            this.tbExePath.ReadOnly = true;
            this.btnExeBrowse.Enabled = false;
        }
        private void loadMarkGrass300GenericPresets()
        {
            this.tbExePath.Text = Bin2PsfWorker.MGRASS300_EXE_PATH;
            this.tbPsflibName.Clear();

            this.tbSeqOffset.Text = "0x800A0000";
            this.tbMySeqSize.Text = "0x00040000";
            this.tbVhOffset.Text = "0x800E0000";
            this.tbVbOffset.Text = "0x80160000";
            this.tbParamOffset.Text = "0";

            this.tbExePath.Enabled = false;
            this.tbExePath.ReadOnly = true;
            this.btnExeBrowse.Enabled = false;
        }

        private void loadMarkGrass422GenericPresets()
        {
            this.tbExePath.Text = Bin2PsfWorker.MGRASS422_EXE_PATH;
            this.tbPsflibName.Clear();

            this.tbSeqOffset.Text = "0x800A0000";
            this.tbMySeqSize.Text = "0x00040000";
            this.tbVhOffset.Text = "0x800E0000";
            this.tbVbOffset.Text = "0x80160000";
            this.tbParamOffset.Text = "0";

            this.tbExePath.Enabled = false;
            this.tbExePath.ReadOnly = true;
            this.btnExeBrowse.Enabled = false;
        }

        private void loadDavironicaGenericPresets()
        {
            this.tbExePath.Text = Bin2PsfWorker.EZPSF_EXE_PATH;
            this.tbPsflibName.Clear();

            this.tbSeqOffset.Text = "0x80100000";
            this.tbMySeqSize.Text = "0x00020000";
            this.tbVhOffset.Text = "0x80120000";
            this.tbVbOffset.Text = "0x80140000";
            this.tbParamOffset.Text = "0";

            this.tbExePath.Enabled = false;
            this.tbExePath.ReadOnly = true;
            this.btnExeBrowse.Enabled = false;
        }

        private void disablePresetFields()
        {
            // this.tbExePath.Enabled = false;
            // this.tbExePath.ReadOnly = true;
            // this.btnExeBrowse.Enabled = false;
            this.tbPsflibName.Enabled = false;
            this.tbPsflibName.ReadOnly = true;

            this.tbParamOffset.Enabled = false;
            this.tbParamOffset.ReadOnly = true;

            this.tbSeqOffset.Enabled = false;
            this.tbSeqOffset.ReadOnly = true;
            this.tbMySeqSize.Enabled = false;
            this.tbMySeqSize.ReadOnly = true;            
            this.tbVhOffset.Enabled = false;
            this.tbVhOffset.ReadOnly = true;
            this.tbVbOffset.Enabled = false;
            this.tbVbOffset.ReadOnly = true;
        }
        private void enablePresetFields()
        {            
            this.tbExePath.Enabled = true;
            this.tbExePath.ReadOnly = false;
            // this.tbExePath.Text = String.Empty;
            this.btnExeBrowse.Enabled = true;
            this.tbPsflibName.Enabled = true;
            this.tbPsflibName.ReadOnly = false;
            this.cbMinipsf.Enabled = true;
            this.cbMinipsf_CheckedChanged(null, null);

            this.tbParamOffset.Enabled = true;
            this.tbParamOffset.ReadOnly = false;
            this.tbParamOffset.Clear();

            this.tbSeqOffset.Enabled = true;
            this.tbSeqOffset.ReadOnly = false;
            this.tbSeqOffset.Clear();            
            this.tbMySeqSize.Enabled = true;
            this.tbMySeqSize.ReadOnly = false;
            this.tbMySeqSize.Clear();                        
            this.tbVhOffset.Enabled = true;
            this.tbVhOffset.ReadOnly = false;
            this.tbVhOffset.Clear();
            this.tbVbOffset.Enabled = true;
            this.tbVbOffset.ReadOnly = false;
            this.tbVbOffset.Clear();

            this.doMiniPsfCheckChange();
        }

        private bool validateInputs()
        {
            bool ret = true;

            if (this.cbMinipsf.Checked && 
                String.IsNullOrEmpty(this.tbMySeqSize.Text))
            {
                MessageBox.Show("Please enter an SEQ size for building .minipsfs.", "Error");
                ret = ret && false;
            }

            ret = ret && AVgmtForm.checkFileExists(this.tbExePath.Text, this.lblDriverPath.Text);
            ret = ret && AVgmtForm.checkFolderExists(this.tbSourceFilesPath.Text, this.lblSourceFiles.Text);
            ret = ret && AVgmtForm.checkTextBox(this.tbOutputFolderName.Text, this.lblOutputFolder.Text);

            return ret;
        }

        private void genericDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadGenericDriverPreset();
        }
        private void genericDriver_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void genericDriver_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnLoadFromStubMaker_Click(object sender, EventArgs e)
        {
            Form2 parentForm = (Form2)this.ParentForm;
            PsfStubMakerForm psfForm = (PsfStubMakerForm)parentForm.GetFormByName("PsfStubMakerForm");

            if (psfForm != null)
            {
                this.tbSeqOffset.Text = psfForm.GetSeqOffset();
                this.tbVhOffset.Text = psfForm.GetVhOffset();
                this.tbVbOffset.Text = psfForm.GetVbOffset();
                this.tbMySeqSize.Text = psfForm.GetSeqSize();
                this.tbParamOffset.Text = psfForm.GetParamOffset();
            }
        }

        private void tbExePath_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void tbExePath_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (File.Exists(s[0])))
            {
                this.tbExePath.Text = s[0];
            }
        }

        private void tbSourceFilesPath_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void tbSourceFilesPath_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (Directory.Exists(s[0])))
            {
                this.tbSourceFilesPath.Text = s[0];
            }
        }

        private void cbForceSepTrackNo_CheckedChanged(object sender, EventArgs e)
        {
            if (cbForceSepTrackNo.Checked)
            {
                sepTrackUpDown.Enabled = true;
            }
            else
            {
                sepTrackUpDown.Enabled = false;
            }
        }
    }
}
