using System;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class Psf2ToPsf2LibForm : AVgmtForm
    {
        public Psf2ToPsf2LibForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_DoTaskButton"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_IntroText1"] + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_IntroText2"] + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_IntroText3"] + Environment.NewLine;

            InitializeComponent();

            this.grpSource.Text =
                ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_GroupSource"];
            this.grpOptions.Text =
                ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_GroupOptions"];
            this.lblOutputFilePrefix.Text =
                ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_LblOutputFilePrefix"];

        }

        private void btnDoTask_Click(object sender, EventArgs e)
        {
            Psf2toPsf2LibWorker.Psf2ToPsf2LibStruct psf2Struct = new Psf2toPsf2LibWorker.Psf2ToPsf2LibStruct();
            psf2Struct.sourcePath = tbSourceDirectory.Text;
            psf2Struct.libraryName = tbFilePrefix.Text;

            base.backgroundWorker_Execute(psf2Struct);
        }
        private void btnSourceDirBrowse_Click(object sender, EventArgs e)
        {
            tbSourceDirectory.Text = base.browseForFolder(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new Psf2toPsf2LibWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_Psf2ToPsf2Lib_MessageBegin"];
        }
    }
}
