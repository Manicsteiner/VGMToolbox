using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class TwoSfTimerForm : AVgmtForm
    {        
        public TwoSfTimerForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = 
                ConfigurationManager.AppSettings["Form_2sfTimer_Title"];
            this.btnDoTask.Text = 
                ConfigurationManager.AppSettings["Form_2sfTimer_DoTaskButton"];
            this.tbOutput.Text = 
                ConfigurationManager.AppSettings["Form_2sfTimer_IntroText"];

            InitializeComponent();

            this.grpSourcePaths.Text =
                ConfigurationManager.AppSettings["Form_2sfTimer_GroupSourcePaths"];
            this.lblPathTo2sfFiles.Text =
                ConfigurationManager.AppSettings["Form_2sfTimer_LblPathTo2sfFiles"];
            this.lblPathToSdat.Text =
                ConfigurationManager.AppSettings["Form_2sfTimer_LblPathToSdat"];
            this.grpOptions.Text =
                ConfigurationManager.AppSettings["Form_2sfTimer_GroupOptions"];
            this.cbOneLoop.Text =
                ConfigurationManager.AppSettings["Form_2sfTimer_CheckboxSingleLoop"];
        }

        private void btnDoTask_Click(object sender, EventArgs e)
        {
            Time2sfStruct t2Struct = new Time2sfStruct();
            t2Struct.Mini2sfDirectory = tbPathTo2sfs.Text;
            t2Struct.SdatPath = tbSdatPath.Text;
            t2Struct.DoSingleLoop = cbOneLoop.Checked;

            base.backgroundWorker_Execute(t2Struct);
        } 
        
        private void btn2sfBrowse_Click(object sender, EventArgs e)
        {
            tbPathTo2sfs.Text = base.browseForFolder(sender, e);
        }
        private void btnSdatBrowse_Click(object sender, EventArgs e)
        {
            tbSdatPath.Text = base.browseForFile(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new Time2sfWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_2sfTimer_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_2sfTimer_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_2sfTimer_MessageBegin"];
        }
    }
}
