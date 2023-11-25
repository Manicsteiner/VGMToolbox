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
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class SnakebiteGuiForm : AVgmtForm
    {
        bool doDrag;
        
        public SnakebiteGuiForm(TreeNode pTreeNode) : base(pTreeNode)
        {
            InitializeComponent();

            this.grpFiles.AllowDrop = true;
            this.doDrag = false;

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_IntroText"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_BtnDoTask"];

            this.grpFiles.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_GrpFiles"];
            this.lblSourceFiles.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_LblSourceFiles"];
            this.lblDragNDrop.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_LblDragNDrop"];
            this.groupOutputMode.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_GroupOutputMode"];
            this.rbNameOutput.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_RbNameOutput"];
            this.rbAutoName.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_RbAutoName"];
            this.lblOutputFile.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_LblOutputFile"];
            this.lblFileExtension.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_LblFileExtension"];            
            this.grpOptions.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_GrpOptions"];
            this.lblStartAddress.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_LblStartAddress"];
            this.rbEndAddress.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_RbEndAddress"];
            this.rbLength.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_RbLength"];
            this.rbEndOfFile.Text = ConfigurationManager.AppSettings["Form_SnakebiteGUI_RbEndOfFile"];

            this.rbEndAddress.Checked = true;
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            this.tbSourceFiles.Text = base.browseForFile(sender, e);
        }
        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            this.tbOutputFile.Text = base.browseForFileToSave(sender, e);
        }

        private void setRadioButtons()
        {
            if (rbEndAddress.Checked)
            {
                tbEndAddress.Enabled = true;
                tbEndAddress.ReadOnly = false;

                tbLength.Enabled = false;
                tbLength.ReadOnly = true;
            }
            else if (rbLength.Checked)
            {
                tbEndAddress.Enabled = false;
                tbEndAddress.ReadOnly = true;

                tbLength.Enabled = true;
                tbLength.ReadOnly = false;
            }
            else if (rbEndOfFile.Checked)
            {
                tbEndAddress.Enabled = false;
                tbEndAddress.ReadOnly = true;

                tbLength.Enabled = false;
                tbLength.ReadOnly = true;
            }
        }
        private void rbEndAddress_CheckedChanged(object sender, EventArgs e)
        {
            this.setRadioButtons();
        }
        private void rbLength_CheckedChanged(object sender, EventArgs e)
        {
            this.setRadioButtons();
        }
        private void rbEndOfFile_CheckedChanged(object sender, EventArgs e)
        {
            this.setRadioButtons();
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        } 

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new SimpleCutterSnakebiteWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_SnakebiteGUI_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_SnakebiteGUI_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_SnakebiteGUI_MessageBegin"];
        }

        private void tbSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            bool cutFiles = false;
            string warningMessage;
            
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (((s.Length > 1) && (!this.rbAutoName.Checked)) ||
                ((s.Length == 1) && (Directory.Exists(s[0]))))
            {
                warningMessage =
                    ConfigurationManager.AppSettings["Form_SnakebiteGUI_ErrorSingleFile"];
                MessageBox.Show(warningMessage, 
                    ConfigurationManager.AppSettings["Form_Global_ErrorWindowTitle"]);
            }
            else
            {
                cutFiles = true;
            }

            if (cutFiles)
            {
                this.doDrag = true;
                this.cutTheFile(s);
            }
        }

        private void cutTheFile(string[] pPaths)
        {
            if (this.validateInputs(!this.doDrag))
            {
                this.doDrag = false;
                
                SimpleCutterSnakebiteWorker.SimpleCutterSnakebiteStruct snbStruct =
                    new SimpleCutterSnakebiteWorker.SimpleCutterSnakebiteStruct();

                snbStruct.EndAddress = this.tbEndAddress.Text;
                snbStruct.Length = this.tbLength.Text;
                snbStruct.OutputFile = this.tbOutputFile.Text;
                snbStruct.NewFileExtension = this.tbFileExtension.Text;
                snbStruct.SourcePaths = pPaths;
                snbStruct.StartOffset = this.tbStartAddress.Text;
                snbStruct.UseEndAddress = this.rbEndAddress.Checked;
                snbStruct.UseFileEnd = this.rbEndOfFile.Checked;
                snbStruct.UseLength = this.rbLength.Checked;

                base.backgroundWorker_Execute(snbStruct);
            }
        }

        private void btnDoTask_Click(object sender, EventArgs e)
        {
            string[] s = new string[] { this.tbSourceFiles.Text };
            this.doDrag = false;
            this.cutTheFile(s);
        }

        private bool validateInputs()
        { 
            return validateInputs(true);
        }
        private bool validateInputs(bool pCheckInputFile)
        {
            bool ret = true;

            if (pCheckInputFile)
            {
                ret &= AVgmtForm.checkFileExists(this.tbSourceFiles.Text, this.lblSourceFiles.Text);
            }
            if (this.rbNameOutput.Checked)
            {
                ret &= AVgmtForm.checkTextBox(this.tbOutputFile.Text, this.rbNameOutput.Text);
            }
            if (this.rbAutoName.Checked)
            {
                ret &= AVgmtForm.checkTextBox(this.tbFileExtension.Text, this.rbAutoName.Text);
            }

            ret &= AVgmtForm.checkTextBox(this.tbStartAddress.Text, this.lblStartAddress.Text);

            if (rbEndAddress.Checked)
            {
                ret &= AVgmtForm.checkTextBox(this.tbEndAddress.Text, this.rbEndAddress.Text);
            }
            if (rbLength.Checked)
            {
                ret &= AVgmtForm.checkTextBox(this.tbLength.Text, this.rbLength.Text);
            }
            
            if (pCheckInputFile && (this.tbSourceFiles.Text.Equals(this.tbOutputFile.Text)))
            {
                MessageBox.Show(ConfigurationManager.AppSettings["Form_SnakebiteGUI_ErrorInputOutputSame"],
                    ConfigurationManager.AppSettings["Form_Global_ErrorWindowTitle"]);
                ret = false;
            }

            return ret;
        }

        private void rbFileNameButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbNameOutput.Checked)
            {
                this.tbOutputFile.Enabled = true;
                this.tbOutputFile.ReadOnly = false;
                this.btnBrowseOutput.Enabled = true;

                this.tbFileExtension.Enabled = false;
                this.tbFileExtension.ReadOnly = true;
                this.tbFileExtension.Clear();
            }
            else
            {
                this.tbOutputFile.Enabled = false;
                this.tbOutputFile.ReadOnly = true;
                this.tbOutputFile.Clear();
                this.btnBrowseOutput.Enabled = false;

                this.tbFileExtension.Enabled = true;
                this.tbFileExtension.ReadOnly = false;                            
            }
        }
    }
}
