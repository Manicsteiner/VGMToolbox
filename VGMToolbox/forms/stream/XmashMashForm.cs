using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.tools.stream;

using VGMToolbox.plugin;

namespace VGMToolbox.forms.stream
{
    public partial class XmashMashForm : AVgmtForm
    {
        public XmashMashForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            InitializeComponent();

            // set title
            this.lblTitle.Text = "XMAshMash";

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            this.tbOutput.Text = String.Format("- Convert XMA to WAV using XMAsh and other tools.{0}", Environment.NewLine);
            this.tbOutput.Text += String.Format("* Note: This tool requires 'xmash.exe' (XMAsh v0.6 or greater) by hcs, 'ToWav.exe' by Xplorer, and SoX by cbagwell and robs.{0}", Environment.NewLine);
            this.tbOutput.Text += String.Format("  Please download XMAsh and ToWAV files and place 'xmash.exe' and 'ToWav.exe' in the following directory: <{0}>{1}", Path.GetDirectoryName(XmashMashWorker.XMASH_FULL_PATH), Environment.NewLine);
            this.tbOutput.Text += String.Format("  Please download all SoX files and place 'sox.exe' and all required .dls in the following directory: <{0}>{1}", XmashMashWorker.SOX_FOLDER, Environment.NewLine);
        }


        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new XmashMashWorker();
        }
        protected override string getCancelMessage()
        {
            return "XMAsh'ing...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "XMAsh'ing...Complete";
        }
        protected override string getBeginMessage()
        {
            return "XMAsh'ing...Begin";
        }


        private void XmashMashForm_DragEnter(object sender, DragEventArgs e)
        {
            this.doDragEnter(sender, e);
        }
        
        private void XmashMashForm_DragDrop(object sender, DragEventArgs e)
        {
            XmashMashWorker.XmaMashMashStruct taskStruct = new XmashMashWorker.XmaMashMashStruct();

            // paths
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            taskStruct.SourcePaths = s;

            // options
            taskStruct.IgnoreXmashFailure = this.chkIgnoreXmashFailure.Checked;

            taskStruct.ReinterleaveMultichannel = this.chkInterleaveMultiChannelOutput.Checked;

            base.backgroundWorker_Execute(taskStruct);  
        }

        
    }


}
