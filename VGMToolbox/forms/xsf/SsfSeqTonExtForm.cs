using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class SsfSeqTonExtForm : AVgmtForm
    {
        public SsfSeqTonExtForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_SeqextTonextFE_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_SeqextTonextFE_IntroText"];

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            InitializeComponent();

            this.grpSource.Text =
                ConfigurationManager.AppSettings["Form_SeqextTonextFE_GroupSource"];
            this.lblDragNDrop.Text =
                ConfigurationManager.AppSettings["Form_SeqextTonextFE_LblDragNDrop"];
            this.cbExtractToSubfolder.Text =
                ConfigurationManager.AppSettings["Form_SeqextTonextFE_CheckBoxExtractToSubfolder"];
            this.lblAuthor.Text =
                ConfigurationManager.AppSettings["Form_SeqextTonextFE_LblAuthor"];
        }

        private void tbSsfSqTonExtSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            SsfSeqTonExtractorWorker.SsfSeqTonExtractorStruct stexStruct = new SsfSeqTonExtractorWorker.SsfSeqTonExtractorStruct();
            stexStruct.SourcePaths = s;
            stexStruct.extractToSubFolder = cbExtractToSubfolder.Checked;

            base.backgroundWorker_Execute(stexStruct);
        }
        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new SsfSeqTonExtractorWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_SeqextTonextFE_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_SeqextTonextFE_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_SeqextTonextFE_MessageBegin"];
        }
    }
}
