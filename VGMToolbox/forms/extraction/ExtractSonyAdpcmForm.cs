using System;
using System.Configuration;
using System.Windows.Forms;
using VGMToolbox.plugin;
using VGMToolbox.tools.extract;
using VGMToolbox.util;

namespace VGMToolbox.forms.extraction
{
    public partial class ExtractSonyAdpcmForm : AVgmtForm
    {
        public ExtractSonyAdpcmForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = "Sony ADPCM Extractor";
            this.tbOutput.Text = "Extract Sony ADPCM Data" + Environment.NewLine;
            this.tbOutput.Text += "- Tool still WIP, results are still imperfect.";
            
            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();
            
            InitializeComponent();

            this.grpSourceFiles.AllowDrop = true;
            this.grpSourceFiles.Text =  ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new ExtractSonyAdpcmWorker();
        }
        protected override string getCancelMessage()
        {
            return "Extracting Sony ADPCM...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "Extracting Sony ADPCM...Complete";
        }
        protected override string getBeginMessage()
        {
            return "Extracting Sony ADPCM...Begin";
        }

        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (this.validateEnteredData())
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                ExtractSonyAdpcmWorker.ExtractSonyAdpcmStruct bwStruct = new ExtractSonyAdpcmWorker.ExtractSonyAdpcmStruct();
                bwStruct.SourcePaths = s;
                bwStruct.OutputBatchFiles = this.cbOutputBatchScripts.Checked;

                if (!String.IsNullOrEmpty(this.tbStartOffset.Text))
                {
                    bwStruct.StartOffset = ByteConversion.GetLongValueFromString(this.tbStartOffset.Text);
                }
                else
                {
                    bwStruct.StartOffset = 0;
                }

                base.backgroundWorker_Execute(bwStruct);
            }
        }

        private bool validateEnteredData()
        {
            bool ret = true;

            if (!String.IsNullOrEmpty(this.tbStartOffset.Text))
            {
                ret &= AVgmtForm.checkIfTextIsParsableAsLong(this.tbStartOffset.Text, this.label1.Text);
            }

            return ret;
        }
    }
}
