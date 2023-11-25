using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class ExtractCdxaForm : AVgmtForm
    {        
        public ExtractCdxaForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_CdxaExtractor_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_CdxaExtractor_IntroText"];
            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();
            
            InitializeComponent();

            this.grpSource.AllowDrop = true;

            this.grpSource.Text =
                ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
            this.grpOptions.Text =
                ConfigurationManager.AppSettings["Form_CdxaExtractor_GroupOptions"];
            this.lblSilentBlocks.Text = ConfigurationManager.AppSettings["Form_CdxaExtractor_LblSilentBlocks"];
            
            this.cbAddRiffHeader.Text =
                ConfigurationManager.AppSettings["Form_CdxaExtractor_CheckBoxAddRiffHeader"];
            this.cbPatchByte0x11.Text =
                ConfigurationManager.AppSettings["Form_CdxaExtractor_CheckBoxPatchByte0x11"];

            this.silentFrameCounter.Value = Cdxa.NUM_SILENT_FRAMES_FOR_SILENT_BLOCK;
            this.rbUseTrackEof.Checked = true;
        }

        private void tbSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            ExtractCdxaWorker.ExtractCdxaStruct extStruct = new ExtractCdxaWorker.ExtractCdxaStruct();
            extStruct.SourcePaths = s;
            extStruct.AddRiffHeader = cbAddRiffHeader.Checked;
            extStruct.PatchByte0x11 = cbPatchByte0x11.Checked;
            extStruct.SilentFramesCount = (uint)this.silentFrameCounter.Value;
            extStruct.FilterAgainstBlockId = this.cbFilterById.Checked;
            extStruct.DoTwoPass = this.cbDoTwoPass.Checked;

            extStruct.UseEndOfTrackMarkerForEof = this.rbUseTrackEof.Checked;
            extStruct.UseSilentBlocksForEof = this.rbUseSilentBlockEof.Checked;

            base.backgroundWorker_Execute(extStruct);
        }
        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new ExtractCdxaWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_CdxaExtractor_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_CdxaExtractor_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_CdxaExtractor_MessageBegin"];
        }

        private void toggleEofOptions()
        {
            this.silentFrameCounter.Enabled = this.rbUseSilentBlockEof.Checked;
            this.lblSilentBlocks.Enabled = this.rbUseSilentBlockEof.Checked;
            this.cbDoTwoPass.Enabled = this.rbUseSilentBlockEof.Checked;

            if (this.rbUseTrackEof.Checked)
            {
                this.cbDoTwoPass.Checked = false;
            }
        }

        private void rbUseTrackEof_CheckedChanged(object sender, EventArgs e)
        {
            this.toggleEofOptions();
        }

        private void rbUseSilentBlockEof_CheckedChanged(object sender, EventArgs e)
        {
            this.toggleEofOptions();
        }
    }
}
