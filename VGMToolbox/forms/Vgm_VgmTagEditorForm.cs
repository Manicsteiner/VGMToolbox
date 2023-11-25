using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.dbutil;
using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.tools.vgm;

namespace VGMToolbox.forms
{    
    public partial class Vgm_VgmTagEditorForm : AVgmtForm
    {
        private static readonly string DB_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "db"), "collection.s3db");
        
        IGd3TagFormat vgmData;
        bool isBatchMode;
        
        public Vgm_VgmTagEditorForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            this.isBatchMode = false;
            
            InitializeComponent();

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_DoTaskButton"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_Info"];

            this.grpSourceFiles.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_GrpSourceFiles"];
            this.grpSetTags.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_GrpSetTags"];
            this.lblGameEn.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblGameEn"];
            this.lblGameJp.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblGameJp"];
            this.lblSystemEn.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblSystemEn"];
            this.lblSystemJp.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblSystemJp"];
            this.lblArtistEn.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblArtistEn"];
            this.lblArtistJp.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblArtistJp"];
            this.lblGameDate.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblGameDate"];
            this.lblRipper.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblRipper"];
            this.grpTrackTags.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_GrpTrackTags"];
            this.lblTrackTitleEn.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblTrackTitleEn"];
            this.lblTrackTitleJp.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_LblTrackTitleJp"];
            this.grpComments.Text = ConfigurationManager.AppSettings["Form_VgmTagEditor_GrpComments"];

            this.loadSystems();
            this.setReadonlyFields();
        }


        private void loadSystems()
        {
            System.Text.Encoding unicode = System.Text.Encoding.Unicode;

            DataTable dtEn = SqlLiteUtil.GetSimpleDataTable(DB_PATH, "VgmSystemNames", "SystemName");
            DataRow drEn = dtEn.NewRow();
            dtEn.Rows.InsertAt(drEn, 0);
            this.cbSystemEn.DataSource = dtEn;
            this.cbSystemEn.DisplayMember = "SystemName";
            this.cbSystemEn.ValueMember = "SystemName";


            DataTable dtJp = SqlLiteUtil.GetSimpleDataTable(DB_PATH, "VgmSystemNames", "SystemName");
            DataRow drJp = dtJp.NewRow();
            dtJp.Rows.InsertAt(drJp, 0);
            this.cbSystemJp.DataSource = dtJp;
            this.cbSystemJp.DisplayMember = "SystemName";
            this.cbSystemJp.ValueMember = "SystemName";
            
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
                // this.disableTrackItems();
                this.uncheckTrackLabels();
            }
            else
            {
                this.isBatchMode = false;
                // this.enableTrackItems();
                this.checkAllLabels();
                this.loadSelectedTrack();
            }
        }
        private void disableTrackItems()
        {
            this.lblTrackTitleEn.Enabled = false;
            this.tbTitleEn.Enabled = false;
            this.tbTitleEn.ReadOnly = true;

            this.lblTrackTitleJp.Enabled = false;
            this.tbTitleJp.Enabled = false;
            this.tbTitleJp.ReadOnly = true;
        }
        private void enableTrackItems()
        {
            this.lblTrackTitleEn.Enabled = true;
            this.tbTitleEn.Enabled = true;
            this.tbTitleEn.ReadOnly = false;

            this.lblTrackTitleJp.Enabled = true;
            this.tbTitleJp.Enabled = true;
            this.tbTitleJp.ReadOnly = false;
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
                        this.vgmData = (IGd3TagFormat)Activator.CreateInstance(formatType);
                        this.vgmData.Initialize(fs, selectedFilePath);

                        this.tbGameEn.Text = this.vgmData.GetGameTagEn();
                        this.tbGameJp.Text = this.vgmData.GetGameTagJp();
                        this.cbSystemEn.Text = this.vgmData.GetSystemTagEn();
                        this.cbSystemJp.Text = this.vgmData.GetSystemTagJp();
                        this.tbArtistEn.Text = this.vgmData.GetArtistTagEn();
                        this.tbArtistJp.Text = this.vgmData.GetArtistTagJp();
                        this.tbGameDate.Text = this.vgmData.GetDateTag();
                        this.tbRipper.Text = this.vgmData.GetRipperTag();
                        this.tbTitleEn.Text = this.vgmData.GetTitleTagEn();
                        this.tbTitleJp.Text = this.vgmData.GetTitleTagJp();
                        this.tbComments.Text = this.vgmData.GetCommentTag();
                    }
                }
            }
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new VgmTagUpdaterWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_VgmTagEditor_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_VgmTagEditor_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_VgmTagEditor_MessageBegin"];
        }

        private void btnDoTask_Click(object sender, EventArgs e)
        {
            ListBoxFileInfoObject listBoxFile;
            int j = 0;
            VgmTagUpdaterWorker.VgmTagUpdaterStruct vtUpdateStruct = new VgmTagUpdaterWorker.VgmTagUpdaterStruct();

            vtUpdateStruct.SourcePaths = new string[this.lbFiles.SelectedIndices.Count];
            
            foreach (int i in this.lbFiles.SelectedIndices)
            {
                listBoxFile = (ListBoxFileInfoObject)this.lbFiles.Items[i];
                vtUpdateStruct.SourcePaths[j++] = listBoxFile.FilePath;
            }

            vtUpdateStruct.IsBatchMode = this.isBatchMode;

            vtUpdateStruct.TitleTagEn = this.tbTitleEn.Text;
            vtUpdateStruct.TitleTagJp = this.tbTitleJp.Text;
            vtUpdateStruct.GameTagEn = this.tbGameEn.Text;
            vtUpdateStruct.GameTagJp = this.tbGameJp.Text;
            vtUpdateStruct.SystemTagEn = this.cbSystemEn.Text;
            vtUpdateStruct.SystemTagJp = this.cbSystemJp.Text;
            vtUpdateStruct.ArtistTagEn = this.tbArtistEn.Text;
            vtUpdateStruct.ArtistTagJp = this.tbArtistJp.Text;
            vtUpdateStruct.DateTag = this.tbGameDate.Text;
            vtUpdateStruct.RipperTag = this.tbRipper.Text;
            vtUpdateStruct.CommentTag = this.tbComments.Text;

            vtUpdateStruct.DoTitleTagEn = this.cbTitleEn.Checked;
            vtUpdateStruct.DoTitleTagJp = this.cbTitleJp.Checked;
            vtUpdateStruct.DoGameTagEn = this.cbGameEnglish.Checked;
            vtUpdateStruct.DoGameTagJp = this.cbGameJp.Checked;
            vtUpdateStruct.DoSystemTagEn = this.checkboxSystemEn.Checked;
            vtUpdateStruct.DoSystemTagJp = this.checkboxSystemJp.Checked;
            vtUpdateStruct.DoArtistTagEn = this.cbArtistEn.Checked;
            vtUpdateStruct.DoArtistTagJp = this.cbArtistJp.Checked;
            vtUpdateStruct.DoDateTag = this.cbReleaseDate.Checked;
            vtUpdateStruct.DoRipperTag = this.cbRipper.Checked;
            vtUpdateStruct.DoCommentTag = this.cbComments.Checked;

            base.backgroundWorker_Execute(vtUpdateStruct);
        }

        private void cbSystemEn_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void cbSystemEn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbSystemJp_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void cbSystemJp_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
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
        private void tsmRefresh_Click(object sender, EventArgs e)
        {
            this.tbSourceDirectory_TextChanged(sender, e);
        }
        private void clearFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.lbFiles.Items.Clear();
        }

        private void Vgm_VgmTagEditorForm_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void Vgm_VgmTagEditorForm_DragDrop(object sender, DragEventArgs e)
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

        private void addFileToListBox(string f)
        {
            Type formatType = null;
            ListBoxFileInfoObject listFileObject;
            
            using (FileStream fs = File.OpenRead(f))
            {
                formatType = FormatUtil.getObjectType(fs);
                
                if ((formatType != null) &&
                    typeof(IGd3TagFormat).IsAssignableFrom(formatType))
                {
                    listFileObject = new ListBoxFileInfoObject(f);
                    this.lbFiles.Items.Add(listFileObject);
                }
            }        
        }

        private void checkAllLabels()
        {
            foreach (Control c in this.Controls)
            {
                foreach (Control cc in c.Controls)
                {
                    if (cc.GetType().Equals(typeof(CheckBox)))
                    {
                        CheckBox temp = (CheckBox)cc;
                        temp.Checked = true;
                    }
                }
            }

        }

        private void uncheckTrackLabels()
        {
            this.cbTitleEn.Checked = false;
            this.cbTitleJp.Checked = false;
        }

        private void setReadonlyFields()
        {
            this.tbGameEn.Enabled = this.cbGameEnglish.Checked;
            this.tbGameJp.Enabled = this.cbGameJp.Checked;
            
            this.cbSystemEn.Enabled = this.checkboxSystemEn.Checked;
            this.cbSystemJp.Enabled = this.checkboxSystemJp.Checked;
            
            this.tbArtistEn.Enabled = this.cbArtistEn.Checked;
            this.tbArtistJp.Enabled = this.cbArtistJp.Checked;

            this.tbGameDate.Enabled = this.cbReleaseDate.Checked;
            this.tbRipper.Enabled = this.cbRipper.Checked;

            this.tbTitleEn.Enabled = this.cbTitleEn.Checked;
            this.tbTitleJp.Enabled = this.cbTitleJp.Checked;

            this.tbComments.Enabled = this.cbComments.Checked;
        }

        private void doChecksChange_CheckedChanged(object sender, EventArgs e)
        {
            this.setReadonlyFields();
        }
    }
}
