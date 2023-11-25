using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class UnpackPsf2Form : AVgmtForm
    {        
        public UnpackPsf2Form(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_UnpkPsf2FE_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_UnpkPsf2FE_IntroText"];
            this.btnDoTask.Hide();

            InitializeComponent();

            this.grpSource.AllowDrop = true;
            this.grpSource.Text = ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
        }

        private void tbPsf2Source_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            UnpkPsf2Worker.UnpkPsf2Struct unpkStruct = new UnpkPsf2Worker.UnpkPsf2Struct();
            unpkStruct.SourcePaths = s;

            base.backgroundWorker_Execute(unpkStruct);
        }
        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new UnpkPsf2Worker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_UnpkPsf2FE_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_UnpkPsf2FE_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_UnpkPsf2FE_MessageBegin"];
        }
    }
}
