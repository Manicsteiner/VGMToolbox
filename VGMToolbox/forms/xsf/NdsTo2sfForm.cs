using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;
using VGMToolbox.util;

namespace VGMToolbox.forms.xsf
{
    public partial class NdsTo2sfForm : AVgmtForm
    {
        public NdsTo2sfForm(TreeNode pTreeNode) :
            base(pTreeNode)
        {
            string testpackPath = Path.Combine(
                Path.GetDirectoryName(Application.ExecutablePath),
                NdsTo2sfWorker.TESTPACK_PATH);
            
            InitializeComponent();

            this.grpSourceFiles.AllowDrop = true;
            this.btnDoTask.Hide();

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_NdsTo2sf_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_NdsTo2sf_IntroText"] + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_Make2sf_IntroText1"] + Environment.NewLine;            
            this.tbOutput.Text +=
                String.Format(ConfigurationManager.AppSettings["Form_Make2sf_IntroText2"], Path.GetDirectoryName(testpackPath)) + Environment.NewLine;
            this.grpSourceFiles.Text = ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new NdsTo2sfWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_NdsTo2sf_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_NdsTo2sf_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_NdsTo2sf_MessageBegin"];
        }

        private static bool CheckForTestPackNds()
        {
            bool ret = true;
            string testpackPath =
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), NdsTo2sfWorker.TESTPACK_PATH);

            if (!File.Exists(testpackPath))
            {
                ret = false;
                MessageBox.Show(String.Format(ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageTestpackMissing"],
                    Path.GetFileName(testpackPath), Path.GetDirectoryName(testpackPath)),
                    String.Format(ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageTestpackMissingHeader"], Path.GetFileName(testpackPath)));
            }
            else
            {
                using (FileStream fs = File.OpenRead(testpackPath))
                {
                    if (!ChecksumUtil.GetCrc32OfFullFile(fs).Equals(Mk2sfWorker.TESTPACK_CRC32))
                    {
                        ret = false;
                        MessageBox.Show(String.Format(ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageTestpackCrc32"],
                            Path.GetFileName(testpackPath), Path.GetDirectoryName(testpackPath), NdsTo2sfWorker.TESTPACK_CRC32),
                            String.Format(ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageTestpackCrc32Header"], Path.GetFileName(testpackPath)));
                    }
                }
            }

            return ret;
        }

        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (CheckForTestPackNds())
            {
                NdsTo2sfWorker.NdsTo2sfStruct bwStruct = new NdsTo2sfWorker.NdsTo2sfStruct();
                bwStruct.SourcePaths = s;
                bwStruct.UseSmapNames = this.cbUseSmapNames.Checked;

                base.backgroundWorker_Execute(bwStruct);
            }
            
        }
    }
}
