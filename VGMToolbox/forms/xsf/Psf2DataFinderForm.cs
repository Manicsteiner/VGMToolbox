using System;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class Psf2DataFinderForm : AVgmtForm
    {
        public Psf2DataFinderForm(TreeNode pTreeNode) :
            base(pTreeNode)
        {
            InitializeComponent();

            this.btnDoTask.Hide();
            this.grpSource.AllowDrop = true;

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_Psf2DataFinder_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_Psf2DataFinder_IntroText"] + Environment.NewLine;

            this.grpOptions.Text = ConfigurationManager.AppSettings["Form_Psf2DataFinder_GrpOptions"];
            this.cbUseMinimum.Text = ConfigurationManager.AppSettings["Form_Psf2DataFinder_CbMinSqSize"];
            this.cb00ByteAligned.Text = ConfigurationManager.AppSettings["Form_Psf2DataFinder_CbByteAlign"];
            this.cbReorderSqFiles.Text = ConfigurationManager.AppSettings["Form_Psf2DataFinder_CbReorderSq"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new Psf2DataFinderWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_Psf2DataFinder_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_Psf2DataFinder_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_Psf2DataFinder_MessageBegin"];
        }

        private void grpSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            Psf2DataFinderWorker.Psf2DataFinderStruct bwStruct = new Psf2DataFinderWorker.Psf2DataFinderStruct();
            bwStruct.SourcePaths = s;
            bwStruct.ReorderSqFiles = cbReorderSqFiles.Checked;
            bwStruct.UseSeqMinimumSize = cbUseMinimum.Checked;
            bwStruct.UseZeroOffsetForBd = cb00ByteAligned.Checked;

            if (cbUseMinimum.Checked)
            {
                bwStruct.MinimumSize = int.Parse(this.tbMinimumSize.Text);
            }

            base.backgroundWorker_Execute(bwStruct);
        }
    }
}
