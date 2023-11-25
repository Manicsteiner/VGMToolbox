using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;

namespace VGMToolbox.forms.other
{
    public partial class InterleaverForm : VgmtForm
    {
        public InterleaverForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            InitializeComponent();

            this.tbOutput.Text = "Interleave files according to the options.";
            
            this.btnDoTask.Text = "Interleave";
            this.rbUseFillBytes.Checked = true;
        }

        private void doFillBytesRadios()
        {
            this.tbFillBytes.Enabled = this.rbUseFillBytes.Checked;
            this.tbFillBytes.Text = this.rbUseFillBytes.Checked ? this.tbFillBytes.Text : String.Empty;
        }

        private void rbNoFillBytes_CheckedChanged(object sender, EventArgs e)
        {
            this.doFillBytesRadios();
        }
        private void rbUseFillBytes_CheckedChanged(object sender, EventArgs e)
        {
            this.doFillBytesRadios();
        }
    }
}
