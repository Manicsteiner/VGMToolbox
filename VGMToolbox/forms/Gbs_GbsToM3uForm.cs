using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.gbs;

namespace VGMToolbox.forms
{
    public partial class Gbs_GbsToM3uForm : AVgmtForm
    {        
        public Gbs_GbsToM3uForm(TreeNode pTreeNode): base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = 
                ConfigurationManager.AppSettings["Form_GbsM3u_Title"];
            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();
            
            InitializeComponent();

            this.grpSource.AllowDrop = true;

            this.grpSource.Text =
                ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
            this.grpOptions.Text =
                ConfigurationManager.AppSettings["Form_GbsM3u_GroupOptions"];
            this.cbGBS_OneM3uPerTrack.Text =
                ConfigurationManager.AppSettings["Form_GbsM3u_CheckBoxOneM3uPerTrack"];
        }

        private void tbGBS_gbsm3uSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            GbsM3uBuilderWorker.GbsM3uWorkerStruct gbStruct = new GbsM3uBuilderWorker.GbsM3uWorkerStruct();
            gbStruct.SourcePaths = s;
            gbStruct.UseKnurekFormatParsing = this.cbKnurekFormat.Checked;
            gbStruct.onePlaylistPerFile = cbGBS_OneM3uPerTrack.Checked;

            base.backgroundWorker_Execute(gbStruct);
        }        
        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new GbsM3uBuilderWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_GbsM3u_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_GbsM3u_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_GbsM3u_MessageBegin"];
        }
    }
}
