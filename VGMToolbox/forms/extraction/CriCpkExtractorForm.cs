using System;
using System.Configuration;
using System.Windows.Forms;
using VGMToolbox.plugin;
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class CriCpkExtractorForm : AVgmtForm
    {
        public CriCpkExtractorForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = "CRI CPK Archive Extractor";
            this.tbOutput.Text = "Extract Files from CRI CPK Archives" + Environment.NewLine;
            this.tbOutput.Text += " Currently supports encrypted and unencrypted CPK files using TOC and ITOC structures." + Environment.NewLine;

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
            return new ExtractCriCpkWorker();
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

            ExtractCriCpkWorker.ExtractCriCpkStruct bwStruct = new ExtractCriCpkWorker.ExtractCriCpkStruct();
            bwStruct.SourcePaths = s;

            base.backgroundWorker_Execute(bwStruct);
        }
    }
}
