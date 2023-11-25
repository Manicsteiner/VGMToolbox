using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class ExtractNintendoU8ArchiveForm : AVgmtForm
    {
        public ExtractNintendoU8ArchiveForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = "Nintendo U8 Unpacker";
            this.tbOutput.Text = "Unpack Nintendo WII U8 files.";

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            InitializeComponent();
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new ExtractNintendoU8ArchiveWorker();
        }
        protected override string getCancelMessage()
        {
            return "Extracting U8 file contents...Cancelled.";
        }
        protected override string getCompleteMessage()
        {
            return "Extracting U8 file contents...Completed.";
        }
        protected override string getBeginMessage()
        {
            return "Extracting U8 file contents...Begin.";
        }

        private void ExtractNintendoU8ArchiveForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            ExtractNintendoU8ArchiveWorker.U8ArchiveUnpackerStruct taskStruct = new ExtractNintendoU8ArchiveWorker.U8ArchiveUnpackerStruct();
            taskStruct.SourcePaths = s;

            base.backgroundWorker_Execute(taskStruct);
        }
    }
}
