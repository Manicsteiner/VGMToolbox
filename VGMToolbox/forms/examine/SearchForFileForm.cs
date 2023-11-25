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
    public partial class SearchForFileForm : AVgmtForm
    {
        public SearchForFileForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            InitializeComponent();

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_SearchForFile_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_SearchForFile_IntroText"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_SearchForFile_DoTaskButton"];

            this.grpSource.Text = ConfigurationManager.AppSettings["Form_SearchForFile_GroupSource"];
            this.lblDragNDrop.Text = ConfigurationManager.AppSettings["Form_SearchForFile_LblDragNDrop"];
            this.grpOptions.Text = ConfigurationManager.AppSettings["Form_SearchForFile_GroupOptions"];
            this.lblSearchString.Text = ConfigurationManager.AppSettings["Form_SearchForFile_LblSearchString"];
            this.cbCaseSensitive.Text = ConfigurationManager.AppSettings["Form_SearchForFile_CheckBoxCaseSensitive"];
            this.cbExtract.Text = ConfigurationManager.AppSettings["Form_SearchForFile_CheckBoxExtract"];
            this.lblOutputFolder.Text = ConfigurationManager.AppSettings["Form_SearchForFile_LblOutputFolder"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new ExamineSearchForFileWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_SearchForFile_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_SearchForFile_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_SearchForFile_MessageBegin"];
        }

        private void tbSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            string[] searchStrings = tbSearchString.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);


            ExamineSearchForFileWorker.ExamineSearchForFileStruct exStruct =
                new ExamineSearchForFileWorker.ExamineSearchForFileStruct();            
            exStruct.SourcePaths = s;
            exStruct.ExtractFile = cbExtract.Checked;
            exStruct.SearchStrings = searchStrings;
            exStruct.CaseSensitive = cbCaseSensitive.Checked;
            exStruct.OutputFolder = tbOutputFolder.Text;

            base.backgroundWorker_Execute(exStruct);
        }
        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            tbSource.Text = base.browseForFolder(sender, e);
        }
        private void btnDoTask_Click(object sender, EventArgs e)
        {
            if (AVgmtForm.checkFolderExists(tbSource.Text, this.grpSource.Text))
            {
                string[] s = new string[1];
                s[0] = tbSource.Text;
                string[] searchStrings = tbSearchString.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                ExamineSearchForFileWorker.ExamineSearchForFileStruct exStruct =
                    new ExamineSearchForFileWorker.ExamineSearchForFileStruct();
                exStruct.SourcePaths = s;
                exStruct.ExtractFile = cbExtract.Checked;
                exStruct.SearchStrings = searchStrings;
                exStruct.CaseSensitive = cbCaseSensitive.Checked;
                exStruct.OutputFolder = tbOutputFolder.Text;

                base.backgroundWorker_Execute(exStruct);
            }
        }
        private void cbExtract_CheckedChanged(object sender, EventArgs e)
        {
            if (cbExtract.Checked)
            {
                tbOutputFolder.Enabled = true;
                btnOutputDir.Enabled = true;
                lblOutputFolder.Enabled = true;
            }
            else
            {
                tbOutputFolder.Enabled = false;
                btnOutputDir.Enabled = false;
                lblOutputFolder.Enabled = false;
            }
        }
    }
}
