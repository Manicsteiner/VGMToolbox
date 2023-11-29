using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using VGMToolbox.plugin;
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class CriAcbAwbExtractorForm : AVgmtForm
    {
        public CriAcbAwbExtractorForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = "CRI ACB/AWB Archive Extractor";
            this.tbOutput.Text = "Extract Files from CRI ACB/AWB Archives" + Environment.NewLine;
            this.tbOutput.Text += "*** For ACB/AWB pairs, only the ACB needs to be dropped." + Environment.NewLine;
            this.tbOutput.Text += "*** If you encounter problems with the extract process, please try using VGMStream first." + Environment.NewLine;

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            InitializeComponent();

            this.grpSourceFiles.AllowDrop = true;
            this.grpSourceFiles.Text = "Drop .acb/.awb files here.";
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new ExtractCriAcbAwbWorker();
        }
        protected override string getCancelMessage()
        {
            return "Extracting files...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "Extracting files...Complete";
        }
        protected override string getBeginMessage()
        {
            return "Extracting files...Begin";
        }

        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            
            ExtractCriAcbAwbWorker.ExtractCriAcbAwbStruct bwStruct = new ExtractCriAcbAwbWorker.ExtractCriAcbAwbStruct();
            bwStruct.SourcePaths = s;
            bwStruct.IncludeCueIdInFileName = this.cbIncludeCueIdInFileName.Checked;

            base.backgroundWorker_Execute(bwStruct);
        }
    }
}
