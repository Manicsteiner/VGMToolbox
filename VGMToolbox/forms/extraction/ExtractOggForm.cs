using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class ExtractOggForm : AVgmtForm
    {
        public ExtractOggForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = "Xiph.Org OGG Extractor";
            this.tbOutput.Text = "Extract Xiph.Org OGG Data" + Environment.NewLine;            

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            InitializeComponent();

            //this.grpSourceFiles.AllowDrop = true;
            this.grpSourceFiles.Text = ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new ExtractOggWorker();
        }
        protected override string getCancelMessage()
        {
            return "Extracting OGGs...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "Extracting OGGs...Complete";
        }
        protected override string getBeginMessage()
        {
            return "Extracting OGGs...Begin";
        }       

        private void ExtractOggForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            ExtractOggWorker.ExtractOggStruct bwStruct = new ExtractOggWorker.ExtractOggStruct();
            bwStruct.SourcePaths = s;
            bwStruct.StopParsingOnFormatError = cbStopParsingOnError.Checked;

            base.backgroundWorker_Execute(bwStruct);
        }

        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            ExtractOggWorker.ExtractOggStruct bwStruct = new ExtractOggWorker.ExtractOggStruct();
            bwStruct.SourcePaths = s;
            bwStruct.StopParsingOnFormatError = cbStopParsingOnError.Checked;

            base.backgroundWorker_Execute(bwStruct);
        }
    }
}
