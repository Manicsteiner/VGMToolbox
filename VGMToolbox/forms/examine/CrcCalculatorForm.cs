using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.examine;

namespace VGMToolbox.forms.examine
{
    public partial class CrcCalculatorForm : AVgmtForm
    {
        public CrcCalculatorForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            InitializeComponent();

            this.grpSourceFiles.AllowDrop = true;
            
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_ChecksumCalculator_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_ChecksumCalculator_IntroText"];

            this.grpSourceFiles.Text = ConfigurationManager.AppSettings["Form_ChecksumCalculator_GroupSourceFiles"];
            this.grpOptions.Text = ConfigurationManager.AppSettings["Form_ChecksumCalculator_GroupOptions"];
            this.cbDoVgmtChecksums.Text = ConfigurationManager.AppSettings["Form_ChecksumCalculator_CheckBoxDoVgmtChecksums"];
            this.checkForDuplicatesFlag.Text = ConfigurationManager.AppSettings["Form_ChecksumCalculator_CheckBoxCheckForDuplicates"];

            this.cbMoveDuplicates.Text = ConfigurationManager.AppSettings["Form_ChecksumCalculator_CheckBoxMoveDuplicates"];
            this.cbMoveDuplicates.Enabled = false;
            this.cbMoveVgmtDuplicates.Text = ConfigurationManager.AppSettings["Form_ChecksumCalculator_CheckBoxMoveVgmtDuplicates"];
            this.cbMoveVgmtDuplicates.Enabled = false;

            this.btnDoTask.Hide();
        }

        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            ExamineChecksumGeneratorWorker.ExamineChecksumGeneratorStruct crcStruct =
                new ExamineChecksumGeneratorWorker.ExamineChecksumGeneratorStruct();
            crcStruct.SourcePaths = s;
            crcStruct.DoVgmtChecksums = cbDoVgmtChecksums.Checked;
            crcStruct.CheckForDuplicates = checkForDuplicatesFlag.Checked;
            crcStruct.MoveStandardDuplicatesToSubfolder = cbMoveDuplicates.Checked;
            crcStruct.MoveVgmtDuplicatesToSubfolder = cbMoveVgmtDuplicates.Checked;

            base.backgroundWorker_Execute(crcStruct);
        }
        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new ExamineChecksumGeneratorWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_ChecksumCalculator_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_ChecksumCalculator_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_ChecksumCalculator_MessageBegin"];
        }

        private void checkForDuplicatesFlag_CheckedChanged(object sender, EventArgs e)
        {
            cbMoveDuplicates.Enabled = checkForDuplicatesFlag.Checked;
            cbMoveVgmtDuplicates.Enabled = cbDoVgmtChecksums.Checked && checkForDuplicatesFlag.Checked;
   
            if (!checkForDuplicatesFlag.Checked)
            {
                cbMoveDuplicates.Checked = false;
                cbMoveVgmtDuplicates.Checked = false;
            }
        }

        private void cbDoVgmtChecksums_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbDoVgmtChecksums.Checked)
            {
                cbMoveVgmtDuplicates.Checked = false;
            }

            checkForDuplicatesFlag_CheckedChanged(sender, e);
        }
    }
}
