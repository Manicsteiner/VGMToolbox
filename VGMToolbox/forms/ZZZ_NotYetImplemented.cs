using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VGMToolbox.forms
{
    public partial class ZZZ_NotYetImplemented : Form
    {
        public ZZZ_NotYetImplemented()
        {
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 oldGUI = new Form1();
            oldGUI.Show();
        }
    }
}
