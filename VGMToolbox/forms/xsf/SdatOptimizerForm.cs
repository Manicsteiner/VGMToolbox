using System;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class SdatOptimizerForm : AVgmtForm
    {
        public SdatOptimizerForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_SdatOptimizer_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_SdatOptimizer_IntroText"] + Environment.NewLine;

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            InitializeComponent();

            this.grpSource.Text =
                ConfigurationManager.AppSettings["Form_SdatOptimizer_GroupSource"];
            this.lblDragNDrop.Text =
                ConfigurationManager.AppSettings["Form_SdatOptimizer_LblDragNDrop"];
            this.grpOptions.Text =
                ConfigurationManager.AppSettings["Form_SdatOptimizer_GroupOptions"];
            this.lblStartingSequence.Text =
                ConfigurationManager.AppSettings["Form_SdatOptimizer_LblStartingSequence"];
            this.lblEndingSequence.Text =
                ConfigurationManager.AppSettings["Form_SdatOptimizer_LblEndingSequence"];
            this.cbIncludeAllSseq.Text =
                ConfigurationManager.AppSettings["Form_SdatOptimizer_CheckBoxIncludeAllSequences"];
        }

        private void tbSourceSdat_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            SdatOptimizerWorker.SdatOptimizerStruct soptStruct = new SdatOptimizerWorker.SdatOptimizerStruct();
            soptStruct.SourcePaths = s;

            if (!cbIncludeAllSseq.Checked)
            {
                soptStruct.startSequence = tbStartSequence.Text;
                soptStruct.endSequence = tbEndSequence.Text;
            }

            base.backgroundWorker_Execute(soptStruct);
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void cbIncludeAllSseq_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIncludeAllSseq.Checked)
            {
                tbStartSequence.ReadOnly = true;
                tbEndSequence.ReadOnly = true;
            }
            else
            {
                tbStartSequence.ReadOnly = false;
                tbEndSequence.ReadOnly = false;            
            }
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new SdatOptimizerWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_SdatOptimizer_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_SdatOptimizer_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_SdatOptimizer_MessageBegin"];
        }
    }
}
