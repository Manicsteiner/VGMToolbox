using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class XsfTagEditorForm : AVgmtForm
    {
        IXsfTagFormat vgmData;
        bool isBatchMode;
        
        public XsfTagEditorForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            this.isBatchMode = false;
            
            InitializeComponent();

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_DoTaskButton"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_Info"];

            this.grpSourceFiles.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_GrpSourceFiles"];
            this.grpSetTags.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_GrpSetTags"];
            this.lblGame.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblGame"];
            this.lblArtist.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblArtist"];
            this.lblCopyright.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblCopyright"];
            this.lblGenre.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblGenre"];
            this.lblYear.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblYear"];
            this.lblXsfBy.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblXsfBy"];
            this.lblSystem.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblSystem"];
            this.grpTrackTags.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_GrpTrackTags"];
            this.lblTrackTitle.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblTrackTitle"];
            this.lblLength.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblLength"];
            this.lblFade.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblFade"];
            this.lblVolume.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_LblVolume"];
            this.grpComments.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_GrpComments"];
            this.cbDeleteEmpty.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_CbDeleteEmpty"];
            this.cbAddToBatchFile.Text = ConfigurationManager.AppSettings["Form_XsfTagEditor_CheckboxCreateBatch"];

            // this.cbRemoveBrackets.Text
            this.cbRemoveBrackets.Enabled = false;
        }

        private void btnBrowseDirectory_Click(object sender, EventArgs e)
        {
            this.tbSourceDirectory.Text = base.browseForFolder(sender, e);
        }

        private void tbSourceDirectory_TextChanged(object sender, EventArgs e)
        {
            this.lbFiles.Items.Clear();

            if (Directory.Exists(this.tbSourceDirectory.Text))
            {
                foreach (string f in Directory.GetFiles(this.tbSourceDirectory.Text))
                {
                    this.addFileToListBox(f);                   
                }
            }
            else
            {
                this.lbFiles.Items.Clear();
            }
        }

        private void lbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFiles.SelectedIndices.Count > 1)
            {
                this.isBatchMode = true;
                this.disableTrackItems();
            }
            else
            {
                this.isBatchMode = false;
                this.enableTrackItems();
                this.loadSelectedTrack();
            }
        }

        private void loadSelectedTrack()
        {
            string selectedFilePath;
            ListBoxFileInfoObject listBoxFile;

            if (lbFiles.SelectedIndices.Count == 1)
            {
                listBoxFile = (ListBoxFileInfoObject)(this.lbFiles.Items[this.lbFiles.SelectedIndex]);
                selectedFilePath = listBoxFile.FilePath;

                using (FileStream fs =
                    File.Open(selectedFilePath, FileMode.Open, FileAccess.Read))
                {
                    Type formatType = FormatUtil.getObjectType(fs);
                    if (formatType != null)
                    {
                        this.vgmData = (IXsfTagFormat)Activator.CreateInstance(formatType);
                        this.vgmData.Initialize(fs, selectedFilePath);

                        this.tbGame.Text = this.vgmData.GetGameTag();
                        this.tbArtist.Text = this.vgmData.GetArtistTag();
                        this.tbCopyright.Text = this.vgmData.GetCopyrightTag();
                        this.tbGenre.Text = this.vgmData.GetGenreTag();
                        this.tbYear.Text = this.vgmData.GetYearTag();
                        this.tbXsfBy.Text = this.vgmData.GetXsfByTag();
                        this.tbTagger.Text = this.vgmData.GetTaggerTag();                         

                        if (this.cbGenerateTitleFromFilename.Checked)
                        {
                            this.tbTitle.Text = XsfUtil.GetTitleForFileName(selectedFilePath, this.cbRemoveBrackets.Checked);
                        }
                        else
                        {
                            this.tbTitle.Text = this.vgmData.GetTitleTag();
                        }
                        
                        this.tbLength.Text = this.vgmData.GetLengthTag();
                        this.tbFade.Text = this.vgmData.GetFadeTag();
                        this.tbVolume.Text = this.vgmData.GetVolumeTag();

                        this.tbComments.Text = this.vgmData.GetCommentTag();
                    }
                }

                this.setTitleTagEnable();
            }
        }

        private void disableTrackItems()
        {
            this.lblTrackTitle.Enabled = false;            
            this.tbTitle.Clear();
            this.tbTitle.Enabled = false;
            this.tbTitle.ReadOnly = true;

            this.lblLength.Enabled = false;
            this.tbLength.Clear();
            this.tbLength.Enabled = false;
            this.tbLength.ReadOnly = true;

            this.lblFade.Enabled = false;
            this.tbFade.Clear();
            this.tbFade.Enabled = false;
            this.tbFade.ReadOnly = true;

            this.lblVolume.Enabled = false;
            this.tbVolume.Clear();
            this.tbVolume.Enabled = false;
            this.tbVolume.ReadOnly = true;
        }
        private void enableTrackItems()
        {
            this.lblTrackTitle.Enabled = true;
            this.tbTitle.Enabled = true;
            this.tbTitle.ReadOnly = false;

            this.lblLength.Enabled = true;
            this.tbLength.Enabled = true;
            this.tbLength.ReadOnly = false;

            this.lblFade.Enabled = true;
            this.tbFade.Enabled = true;
            this.tbFade.ReadOnly = false;

            this.lblVolume.Enabled = true;
            this.tbVolume.Enabled = true;
            this.tbVolume.ReadOnly = false;
        }

        private void btnDoTask_Click(object sender, EventArgs e)
        {
            ListBoxFileInfoObject listBoxFile;
            int j = 0;
            XsfTagUpdaterWorker.XsfTagUpdaterStruct xtUpdateStruct = new XsfTagUpdaterWorker.XsfTagUpdaterStruct();
                        
            xtUpdateStruct.SourcePaths = new string[this.lbFiles.SelectedIndices.Count];            
            foreach (int i in this.lbFiles.SelectedIndices)
            {
                listBoxFile = (ListBoxFileInfoObject)this.lbFiles.Items[i];
                xtUpdateStruct.SourcePaths[j++] = listBoxFile.FilePath;
            }

            xtUpdateStruct.RemoveEmptyTags = cbDeleteEmpty.Checked;
            xtUpdateStruct.IsBatchMode = this.isBatchMode;
            xtUpdateStruct.GenerateTitleFromFilename = this.cbGenerateTitleFromFilename.Checked;
            xtUpdateStruct.RemoveBracketInfoFromTitle = this.cbRemoveBrackets.Checked;
            xtUpdateStruct.AddToBatchFile = this.cbAddToBatchFile.Checked;

            xtUpdateStruct.TitleTag = this.tbTitle.Text;
            xtUpdateStruct.ArtistTag = this.tbArtist.Text;
            xtUpdateStruct.GameTag = this.tbGame.Text;
            xtUpdateStruct.YearTag = this.tbYear.Text;
            xtUpdateStruct.GenreTag = this.tbGenre.Text;
            xtUpdateStruct.CommentTag = this.tbComments.Text;
            xtUpdateStruct.CopyrightTag = this.tbCopyright.Text;
            xtUpdateStruct.XsfByTag = this.tbXsfBy.Text;
            xtUpdateStruct.TaggerTag = this.tbTagger.Text;
            xtUpdateStruct.VolumeTag = this.tbVolume.Text;
            xtUpdateStruct.LengthTag = this.tbLength.Text;
            xtUpdateStruct.FadeTag = this.tbFade.Text;
            xtUpdateStruct.SystemTag = this.tbSystem.Text;

            base.backgroundWorker_Execute(xtUpdateStruct);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new XsfTagUpdaterWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_XsfTagEditor_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_XsfTagEditor_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_XsfTagEditor_MessageBegin"];
        }

        private void cbGenerateTitleFromFilename_CheckedChanged(object sender, EventArgs e)
        {
            this.setTitleTagEnable();

            if (this.cbGenerateTitleFromFilename.Checked)
            {
                this.cbRemoveBrackets.Enabled = true;
            }
            else
            {
                this.cbRemoveBrackets.Enabled = false;
                this.cbRemoveBrackets.Checked = false;
            }
        }
        private void setTitleTagEnable()
        {
            if (this.cbGenerateTitleFromFilename.Checked)
            {
                this.tbTitle.Enabled = false;
            }
            else
            {
                this.tbTitle.Enabled = true;
            }        
        }
        
        private void refreshFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tbSourceDirectory_TextChanged(sender, e);
        }
        private void lbFiles_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // show menu
                contextMenuRefresh.Show(lbFiles, p);
            }
        }

        private void lbFiles_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void addFileToListBox(string f)
        {
            Type formatType = null;
            ListBoxFileInfoObject listFileObject;

            using (FileStream fs = File.OpenRead(f))
            {
                formatType = FormatUtil.getObjectType(fs);

                if ((formatType != null) &&
                    typeof(IXsfTagFormat).IsAssignableFrom(formatType))
                {
                    listFileObject = new ListBoxFileInfoObject(f);
                    this.lbFiles.Items.Add(listFileObject);
                }
            }
        }

        private void lbFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (s.Length > 0)
            {
                tbSourceDirectory.Clear();

                foreach (string filePath in s)
                {
                    this.addFileToListBox(filePath);
                }
            }
        }

        private void clearFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.lbFiles.Items.Clear();
        }
    }
}
