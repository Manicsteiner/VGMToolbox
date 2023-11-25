using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.dbutil;
using VGMToolbox.format;

namespace VGMToolbox.forms.examine
{
    public partial class VgmTagsUpdateForm : Form
    {
        private static readonly string DB_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "db"), "collection.s3db");

        VGMToolbox.util.NodeTagStruct nodeTagInfo;
        IGd3TagFormat vgmData;

        public VgmTagsUpdateForm(VGMToolbox.util.NodeTagStruct pNts)
        {
            this.nodeTagInfo = pNts;
            
            InitializeComponent();

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
            this.loadCurrentTagInformation();
        }

        private void loadCurrentTagInformation()
        {
            using (FileStream fs =
                File.Open(this.nodeTagInfo.FilePath, FileMode.Open, FileAccess.Read))
            {
                this.vgmData =
                    (IGd3TagFormat)Activator.CreateInstance(Type.GetType(this.nodeTagInfo.ObjectType));

                this.vgmData.Initialize(fs, this.nodeTagInfo.FilePath);

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.vgmData.SetGameTagEn(this.tbGameEn.Text);
                this.vgmData.SetGameTagJp(this.tbGameJp.Text);
                this.vgmData.SetSystemTagEn(this.cbSystemEn.Text);
                this.vgmData.SetSystemTagJp(this.cbSystemJp.Text);
                this.vgmData.SetArtistTagEn(this.tbArtistEn.Text);
                this.vgmData.SetArtistTagJp(this.tbArtistJp.Text);
                this.vgmData.SetDateTag(this.tbGameDate.Text);
                this.vgmData.SetRipperTag(this.tbRipper.Text);
                this.vgmData.SetTitleTagEn(this.tbTitleEn.Text);
                this.vgmData.SetTitleTagJp(this.tbTitleJp.Text);
                this.vgmData.SetCommentTag(this.tbComments.Text);

                this.vgmData.UpdateTags();

                MessageBox.Show(String.Format("Update complete, updates will not be reflected in the tree until the files are added again.",
                    Path.GetFileName(this.vgmData.FilePath)));

                this.Close();
                this.Dispose();
            }
            catch (Exception _ex)
            {
                MessageBox.Show(String.Format("{0}: {1}- {2}", _ex.Message, Environment.NewLine, 
                    _ex.GetBaseException().Message), "Error Updating Tags");
            }
        }
    }
}
