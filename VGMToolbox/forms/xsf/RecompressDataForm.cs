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
using VGMToolbox.tools.xsf;
using VGMToolbox.plugin;

namespace VGMToolbox.forms.xsf
{
    public partial class RecompressDataForm : AVgmtForm
    {
        private static readonly string DB_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "db"), "collection.s3db");        
        
        public RecompressDataForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            InitializeComponent();

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_RecompressXsf_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_RecompressXsf_DoTaskButton"];
            this.loadCompressionComboBox();

            this.grpSource.Text = ConfigurationManager.AppSettings["Form_RecompressXsf_GroupSource"];
            this.lblSourceDragNDrop.Text = ConfigurationManager.AppSettings["Form_RecompressXsf_LabelSourceDragNDrop"];
            this.grpOptions.Text = ConfigurationManager.AppSettings["Form_RecompressXsf_GroupOptions"];
            this.cb7zipTopLevelFolders.Text = ConfigurationManager.AppSettings["Form_RecompressXsf_CheckBox7zipTopLevelFolders"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new XsfRecompressDataWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_RecompressXsf_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_RecompressXsf_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_RecompressXsf_MessageBegin"];
        }

        private void loadCompressionComboBox()
        {
            this.cbCompressionLevel.Items.Add(String.Empty);
            this.cbCompressionLevel.DataSource = SqlLiteUtil.GetSimpleDataTable(DB_PATH, "ZlibCompressionLevels", "CompressionLevelId");
            this.cbCompressionLevel.DisplayMember = "CompressionLevelDescription";
            this.cbCompressionLevel.ValueMember = "CompressionLevelId";        
        }

        private void tbSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            this.recompressData(s);            
        }
        private void cbCompressionLevel_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void cbCompressionLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            this.tbSource.Text = base.browseForFolder(sender, e);
        }        
        private void btnDoTask_Click(object sender, EventArgs e)
        {
            if (AVgmtForm.checkFolderExists(this.tbSource.Text, this.grpSource.Text))
            {
                string[] paths = new string[] { this.tbSource.Text };
                recompressData(paths);
            }
        }

        private void recompressData(string[] pSourcePaths)
        {
            XsfRecompressDataWorker.XsfRecompressDataStruct xrdStruct = new XsfRecompressDataWorker.XsfRecompressDataStruct();
            xrdStruct.SourcePaths = pSourcePaths;
            xrdStruct.RecompressFolders = cb7zipTopLevelFolders.Checked;
            xrdStruct.CompressionLevel = Convert.ToInt32(this.cbCompressionLevel.SelectedValue);

            base.backgroundWorker_Execute(xrdStruct);
        }
    }
}
