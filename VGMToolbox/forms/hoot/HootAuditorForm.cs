using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.hoot;


namespace VGMToolbox.forms.hoot
{
    public partial class HootAuditorForm : TreeViewVgmtForm
    {
        static readonly string AUDIT_OUTPUT_FILE = 
            Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "hoot"), "audit.txt");

        
        public HootAuditorForm(TreeNode pTreeNode)
            : base(pTreeNode) 
        {
            base.outputToText = true;
            base.outputTextFile = AUDIT_OUTPUT_FILE;

            NodePrintMessageStruct n = new NodePrintMessageStruct();
            n.NodeColor = HootAuditorWorker.HOOT_MISSING_FILE_COLOR;
            n.Message = ConfigurationManager.AppSettings["Form_HootCollectionAuditor_MessageMissing"];
            base.outputColorToMessageRules = new NodePrintMessageStruct[] { n };

            InitializeComponent();

            this.grpOptions.Text = ConfigurationManager.AppSettings["Form_HootCollectionAuditor_GrpOptions"];
            this.lblFolder.Text = ConfigurationManager.AppSettings["Form_HootCollectionAuditor_LblFolder"];
            this.lblArchiveFolders.Text = ConfigurationManager.AppSettings["Form_HootCollectionAuditor_LblArchiveFolders"];
            this.cbIncludeSubDirs.Text = ConfigurationManager.AppSettings["Form_HootCollectionAuditor_CheckboxIncludeSubDirs"];

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_HootCollectionAuditor_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_HootCollectionAuditor_BtnDoTask"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_HootCollectionAuditor_Intro"];
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new HootAuditorWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_HootCollectionAuditor_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_HootCollectionAuditor_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_HootCollectionAuditor_MessageBegin"];
        }

        private void btnDoTask_Click(object sender, EventArgs e)
        {
            string[] sourcePaths = new string[] { tbXmlPath.Text };
            HootAuditorWorker.HootAuditorStruct hootStruct = new HootAuditorWorker.HootAuditorStruct();
            hootStruct.SourcePaths = sourcePaths;
            hootStruct.SetArchivePaths = 
                tbArchiveFolders.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            hootStruct.IncludeSubDirectories = cbIncludeSubDirs.Checked;

            base.backgroundWorker_Execute(hootStruct);
        }

        private void btnBrowseXmlFolder_Click(object sender, EventArgs e)
        {
            tbXmlPath.Text = base.browseForFolder(sender, e);
        }

        private void btnBrowseArchiveFolders_Click(object sender, EventArgs e)
        {
            string newFolder = base.browseForFolder(sender, e);
            tbArchiveFolders.Text += newFolder + Environment.NewLine;
        }
    }
}
