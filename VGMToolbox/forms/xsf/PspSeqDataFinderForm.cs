using System;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class PspSeqDataFinderForm : AVgmtForm
    {
        public PspSeqDataFinderForm(TreeNode pTreeNode) :
            base(pTreeNode)
        {
            InitializeComponent();

            this.btnDoTask.Hide();
            this.grpSource.AllowDrop = true;

            this.lblTitle.Text = "PSP Sequence Data Finder";
            this.tbOutput.Text = "- Extract MID/PHD/PBD data from files." + Environment.NewLine;
            this.tbOutput.Text += "- PHD/PBD should always be correctly paired." + Environment.NewLine;
            this.tbOutput.Text += "- NOTICE: PBD DETECTION CAN BE VERY SLOW, PLEASE BE PATIENT..." + Environment.NewLine;
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new PspSeqDataFinderWorker();
        }
        protected override string getCancelMessage()
        {
            return "Extracting PSP Data...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "Extracting PSP Data...Complete";
        }
        protected override string getBeginMessage()
        {
            return "Extracting PSP Data...Begin";
        }

        private void grpSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            PspSeqDataFinderWorker.PspSeqDataFinderStruct bwStruct = new PspSeqDataFinderWorker.PspSeqDataFinderStruct();
            bwStruct.SourcePaths = s;
            bwStruct.ReorderMidFiles = cbReorderMidFiles.Checked;
            bwStruct.UseZeroOffsetForPbd = cb00ByteAligned.Checked;

            base.backgroundWorker_Execute(bwStruct);
        }
    }
}
