using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.extract;
using VGMToolbox.util;

namespace VGMToolbox.forms.compression
{
    public partial class GzipCompressionForm : AVgmtForm
    {
        public GzipCompressionForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            InitializeComponent();

            this.grpSourceFiles.AllowDrop = true;
            this.btnDoTask.Hide();

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_GzipCompress_Title"];
            this.tbOutput.Text = String.Format(ConfigurationManager.AppSettings["Form_GzipCompress_IntroText1"],
                            CompressionUtil.GzipDecompressOutputExtension, Environment.NewLine);
            this.tbOutput.Text += String.Format(ConfigurationManager.AppSettings["Form_GzipCompress_IntroText2"],
                CompressionUtil.GzipCompressOutputExtension, Environment.NewLine);

            this.grpSourceFiles.Text = ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
            this.grpOptions.Text = ConfigurationManager.AppSettings["Form_GzipCompress_GrpOptions"];
            this.rbDecompress.Text = ConfigurationManager.AppSettings["Form_GzipCompress_RbDecompress"];
            this.rbCompress.Text = ConfigurationManager.AppSettings["Form_GzipCompress_RbCompress"];
            this.lblOffset.Text = ConfigurationManager.AppSettings["Form_GzipCompress_LblOffset"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new GzipExtractorWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_GzipCompress_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_GzipCompress_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_GzipCompress_MessageBegin"];
        }

        private bool validateInputs()
        {
            bool ret = true;

            // put 0 in Offset if it is empty
            if (String.IsNullOrEmpty(this.tbOffset.Text))
            {
                this.tbOffset.Text = "0";
            }

            try
            {
                long tempval = VGMToolbox.util.ByteConversion.GetLongValueFromString(this.tbOffset.Text);
            }
            catch
            {
                MessageBox.Show(ConfigurationManager.AppSettings["Form_GzipCompress_ErrorIntParse"],
                    ConfigurationManager.AppSettings["Form_Global_ErrorWindowTitle"]);
                ret = false;
            }

            return ret;
        }
        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (validateInputs())
            {

                GzipExtractorWorker.GzipExtractorStruct bwStruct = new GzipExtractorWorker.GzipExtractorStruct();
                bwStruct.SourcePaths = s;
                bwStruct.DoDecompress = this.rbDecompress.Checked;
                bwStruct.StartingOffset = VGMToolbox.util.ByteConversion.GetLongValueFromString(this.tbOffset.Text);

                base.backgroundWorker_Execute(bwStruct);
            }
        }
    }
}
