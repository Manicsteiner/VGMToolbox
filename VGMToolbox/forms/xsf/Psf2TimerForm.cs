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
    public partial class Psf2TimerForm : AVgmtForm
    {        
        public Psf2TimerForm(TreeNode pTreeNode)
            : base(pTreeNode) 
        {
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_Psf2Timer_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_Psf2Timer_IntroText"];

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();
            
            InitializeComponent();

            this.gbSource.AllowDrop = true;
            this.gbSource.Text =
                ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void tbSourcePaths_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            Psf2TimerWorker.Psf2TimerStruct timeStruct = new Psf2TimerWorker.Psf2TimerStruct();
            timeStruct.SourcePaths = s;

            base.backgroundWorker_Execute(timeStruct);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new Psf2TimerWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_Psf2Timer_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_Psf2Timer_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_Psf2Timer_MessageBegin"];
        }
    }
}
