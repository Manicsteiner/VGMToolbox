using System;
using System.Configuration;
using System.Windows.Forms;
using VGMToolbox.plugin;
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class ExtractHcaForm : AVgmtForm
    {
        public ExtractHcaForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = "CRI HCA Extractor";
            this.tbOutput.Text = "Extract CRI HCA Data" + Environment.NewLine;

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            InitializeComponent();

            this.grpSourceFiles.AllowDrop = true;
            this.grpSourceFiles.Text = ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new ExtractHcaWorker();
        }
        protected override string getCancelMessage()
        {
            return "Extracting CRI HCA...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "Extracting CRI HCA...Complete";
        }
        protected override string getBeginMessage()
        {
            return "Extracting CRI HCA...Begin";
        }

        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            ExtractHcaWorker.ExtractHcaStruct bwStruct = new ExtractHcaWorker.ExtractHcaStruct();
            bwStruct.SourcePaths = s;

            base.backgroundWorker_Execute(bwStruct);
        }
    }
}
