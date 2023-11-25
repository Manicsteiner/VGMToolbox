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
    public partial class UnpackNintendoWadForm : AVgmtForm
    {
        public UnpackNintendoWadForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = "Nintendo WAD Unpacker";
            this.tbOutput.Text = "Unpack Nintendo WII WAD files.";

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
            return new UnpackNintendoWadWorker();
        }
        protected override string getCancelMessage()
        {
            return "Unpacking WAD file...Cancelled.";
        }
        protected override string getCompleteMessage()
        {
            return "Unpacking WAD file...Completed.";
        }
        protected override string getBeginMessage()
        {
            return "Unpacking WAD file...Begin.";
        }

        private void UnpackNintendoWadForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            UnpackNintendoWadWorker.WadUnpackerStruct taskStruct = new UnpackNintendoWadWorker.WadUnpackerStruct();
            taskStruct.SourcePaths = s;
            taskStruct.UnpackExtractedU8Files = this.cbUnpackU8Archives.Checked;
            taskStruct.ExtractAllFiles = this.rbExtractAllSections.Checked;


            base.backgroundWorker_Execute(taskStruct);
        }
    }
}
