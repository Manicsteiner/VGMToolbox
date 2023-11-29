using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VGMToolbox.forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            this.linkLabelHomePage.Links.Add(0, this.linkLabelHomePage.Text.Length, "https://sourceforge.net/projects/vgmtoolbox/");
            this.linkLabelSupport.Links.Add(0, this.linkLabelSupport.Text.Length, "https://hcs64.com/mboard/forum.php?showthread=22580");
            this.linkLabelGithub.Links.Add(0, this.linkLabelGithub.Text.Length, "https://github.com/Manicsteiner/VGMToolbox");
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
