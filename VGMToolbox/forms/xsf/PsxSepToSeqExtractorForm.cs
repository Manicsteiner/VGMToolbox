using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class PsxSepToSeqExtractorForm : AVgmtForm
    {
        public PsxSepToSeqExtractorForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            InitializeComponent();

            this.grpSource.AllowDrop = true;
            
            // this.grpSource.Text
            this.lblTitle.Text = "Extract SEQ from SEP files";
            this.tbOutput.Text = "Split PSX SEP format files into SEQ files.";
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new PsxSepToSeqExtractorWorker();
        }
        protected override string getCancelMessage()
        {
            return "Extract SEQ from SEP...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "Extract SEQ from SEP...Complete";
        }
        protected override string getBeginMessage()
        {
            return "Extract SEQ from SEP...Begin";
        }

        private void grpSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            PsxSepToSeqExtractorWorker.PsxSepToSeqExtractorStruct bwStruct = new PsxSepToSeqExtractorWorker.PsxSepToSeqExtractorStruct();
            bwStruct.SourcePaths = s;
 
            base.backgroundWorker_Execute(bwStruct);
        }
    }
}
