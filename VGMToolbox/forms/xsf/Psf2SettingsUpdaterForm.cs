using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class Psf2SettingsUpdaterForm : AVgmtForm
    {
        public Psf2SettingsUpdaterForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            InitializeComponent();

            this.grpSourceFiles.AllowDrop = true;
            this.btnDoTask.Hide();

            this.lblTitle.Text = "PSF2 Settings Updater";
            this.tbOutput.Text = "Update the psf2.ini settings for PSF2s in batch mode.";
        }

        private void grpSourceFiles_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new Psf2SettingsUpdaterWorker();
        }
        protected override string getCancelMessage()
        {
            return "Updating PSF2 Settings...Cancelled.";
        }
        protected override string getCompleteMessage()
        {
            return "Updating PSF2 Settings...Complete.";
        }
        protected override string getBeginMessage()
        {
            return "Updating PSF2 Settings...Begin.";
        }

        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            Psf2SettingsUpdaterWorker.Psf2SettingsUpdaterStruct bwStruct = new Psf2SettingsUpdaterWorker.Psf2SettingsUpdaterStruct();
            Psf2.Psf2IniSqIrxStruct iniValues = new Psf2.Psf2IniSqIrxStruct();

            iniValues.SqFileName = this.tbSqFile.Text;
            iniValues.HdFileName = this.tbHdFile.Text;
            iniValues.BdFileName = this.tbBdFile.Text;

            iniValues.SequenceNumber = this.tbSequenceNumber.Text;
            iniValues.TimerTickInterval = this.tbTickInterval.Text;
            iniValues.Reverb = this.tbReverb.Text;
            iniValues.Depth = this.tbDepth.Text;
            iniValues.Tempo = this.tbTempo.Text;
            iniValues.Volume = this.tbVolume.Text;

            bwStruct.IniSettings = iniValues;
            bwStruct.SourcePaths = s;

            base.backgroundWorker_Execute(bwStruct);
        }
    }
}
