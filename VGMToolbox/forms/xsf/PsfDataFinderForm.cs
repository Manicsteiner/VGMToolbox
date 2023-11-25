using System;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class PsfDataFinderForm : AVgmtForm
    {
        public PsfDataFinderForm(TreeNode pTreeNode) :
            base(pTreeNode)
        {
            InitializeComponent();

            this.btnDoTask.Hide();
            this.grpSource.AllowDrop = true;

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_PsfDataFinder_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_PsfDataFinder_IntroText"] + Environment.NewLine;

            this.grpOptions.Text = ConfigurationManager.AppSettings["Form_PsfDataFinder_GrpOptions"];
            this.cbUseMinimum.Text = ConfigurationManager.AppSettings["Form_PsfDataFinder_CbMinSqSize"];
            this.cbRenameSeq.Text = ConfigurationManager.AppSettings["Form_PsfDataFinder_CbReorderSq"];
            this.cbVbStartAt0x00.Text = ConfigurationManager.AppSettings["Form_PsfDataFinder_CbByteAlign"];
            this.cbRelaxVbEof.Text = ConfigurationManager.AppSettings["Form_PsfDataFinder_CbRelaxVbEof"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new PsfDataFinderWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_PsfDataFinder_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_PsfDataFinder_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_PsfDataFinder_MessageBegin"];
        }

        private void grpSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            
            PsfDataFinderWorker.PsfDataFinderStruct bwStruct = new PsfDataFinderWorker.PsfDataFinderStruct();
            bwStruct.SourcePaths = s;
            bwStruct.UseSeqMinimumSize = cbUseMinimum.Checked;
            bwStruct.ReorderSeqFiles = cbRenameSeq.Checked;
            bwStruct.UseZeroOffsetForVb = cbVbStartAt0x00.Checked;
            bwStruct.RelaxVbEofRestrictions = cbRelaxVbEof.Checked;

            if (cbUseMinimum.Checked)
            {
                bwStruct.MinimumSize = int.Parse(this.tbMinimumSize.Text);
            }

            base.backgroundWorker_Execute(bwStruct);
        }
    }
}
