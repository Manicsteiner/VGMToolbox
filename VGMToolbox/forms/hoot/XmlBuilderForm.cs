using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.hoot;

namespace VGMToolbox.forms.hoot
{
    public partial class XmlBuilderForm : AVgmtForm
    {        
        public XmlBuilderForm(TreeNode pTreeNode) : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = 
                ConfigurationManager.AppSettings["Form_HootXmlBuilder_Title"];
            this.tbOutput.Text =
                ConfigurationManager.AppSettings["Form_HootXmlBuilder_Intro"];
            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            InitializeComponent();

            this.gbHootXML_Source.AllowDrop = true;

            this.gbHootXML_Source.Text =
                ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
            this.gbHootXML_Options.Text =
                ConfigurationManager.AppSettings["Form_HootXmlBuilder_GroupOptions"];
            this.cbHootXML_CombineOutput.Text =
                ConfigurationManager.AppSettings["Form_HootXmlBuilder_CheckBoxCombineOutput"];
            this.cbHootXML_SplitOutput.Text =
                ConfigurationManager.AppSettings["Form_HootXmlBuilder_CheckBoxSplitOutput"];
        }

        private void tbHootXML_Path_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            HootXmlBuilderWorker.HootXmlBuilderStruct xbStruct = new HootXmlBuilderWorker.HootXmlBuilderStruct();
            xbStruct.SourcePaths = s;
            xbStruct.combineOutput = cbHootXML_CombineOutput.Checked;
            xbStruct.splitOutput = cbHootXML_SplitOutput.Checked;
            xbStruct.parseFileName = cbParseFile.Checked;

            base.backgroundWorker_Execute(xbStruct);
        }
        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new HootXmlBuilderWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_HootXmlBuilder_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_HootXmlBuilder_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_HootXmlBuilder_MessageBegin"];
        }
    }
}
