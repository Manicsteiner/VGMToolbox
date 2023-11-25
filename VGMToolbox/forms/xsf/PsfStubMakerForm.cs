using System;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;
using VGMToolbox.util;

namespace VGMToolbox.forms.xsf
{
    public partial class PsfStubMakerForm : AVgmtForm
    {
        public PsfStubMakerForm(TreeNode pTreeNode): 
            base(pTreeNode)
        {
            InitializeComponent();

            this.grpSourceFiles.AllowDrop = true;

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_PsfStubCreator_Title"];
            this.grpSourceFiles.Text = ConfigurationManager.AppSettings["Form_Global_DropSourceFiles"];
            this.grpOptions.Text = ConfigurationManager.AppSettings["Form_PsfStubCreator_GroupOptions"];
            this.lblDriverText.Text = ConfigurationManager.AppSettings["Form_PsfStubCreator_LblDriverText"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_PsfStubCreator_IntroText"];
            
            this.btnDoTask.Hide();            
            this.loadDefaults();

            this.cbOverrideDriverOffset.Checked = false;
            this.overrideDriverOffset();
            this.btnHighestAddress.Enabled = false;
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new PsfStubMakerWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_PsfStubCreator_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_PsfStubCreator_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_PsfStubCreator_MessageBegin"];
        }

        private void grpSourceFiles_DragDrop(object sender, DragEventArgs e)
        {
            // check for PsyQ in PATH
            if (!XsfUtil.IsPsyQPathVariablePresent())
            {
                MessageBox.Show(
                    ConfigurationManager.AppSettings["Form_PsfStubCreator_ErrPsyQPath"],
                    ConfigurationManager.AppSettings["Form_Global_ErrorWindowTitle"]);                           
            }
            else if (!XsfUtil.IsPsyQSdkPresent())
            {
                MessageBox.Show(
                    ConfigurationManager.AppSettings["Form_PsfStubCreator_IntroText"],
                    ConfigurationManager.AppSettings["Form_Global_ErrorWindowTitle"]);
            }
            else if (this.validateInputs())
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                PsfStubMakerStruct bwStruct = new PsfStubMakerStruct();
                bwStruct.SourcePaths = s;
                bwStruct.UseSeqFunctions = this.cbSeqFunctions.Checked;
                bwStruct.DriverText = this.tbDriverText.Text;
                bwStruct.IncludeReverb = this.cbIncludeReverb.Checked;

                bwStruct.PsfDrvLoad = this.tbPsfDrvLoad.Text;
                bwStruct.PsfDrvSize = this.tbPsfDrvSize.Text;
                bwStruct.PsfDrvParam = this.tbPsfDrvParam.Text;
                bwStruct.PsfDrvParamSize = this.tbPadDrvParamSize.Text;

                bwStruct.MySeq = this.tbMySeq.Text;
                bwStruct.MySeqSize = this.tbMySeqSize.Text;
                bwStruct.MyVh = this.tbMyVh.Text;
                bwStruct.MyVhSize = this.tbMyVhSize.Text;
                bwStruct.MyVb = this.tbMyVb.Text;
                bwStruct.MyVbSize = this.tbMyVbSize.Text;

                bwStruct.OverrideDriverLoadAddress = cbOverrideDriverOffset.Checked;
                bwStruct.RelaxLoadAddressRestriction = cbRelaxPsfLoadAddress.Checked;

                base.backgroundWorker_Execute(bwStruct);
            }
        }

        private void loadDefaults()
        {
            this.tbPsfDrvLoad.Text = PsfStubMakerWorker.PsfDrvLoadDefault;
            this.tbPsfDrvSize.Text = PsfStubMakerWorker.PsfDrvSizeDefault;
            this.tbPsfDrvParam.Text = PsfStubMakerWorker.PsfDrvParamDefault;
            this.tbPadDrvParamSize.Text = PsfStubMakerWorker.PsfDrvParamSizeDefault;

            this.tbMySeq.Text = PsfStubMakerWorker.MySeqDefault;
            this.tbMySeqSize.Text = PsfStubMakerWorker.MySeqSizeDefault;
            this.tbMyVh.Text = PsfStubMakerWorker.MyVhDefault;
            this.tbMyVhSize.Text = PsfStubMakerWorker.MyVhSizeDefault;
            this.tbMyVb.Text = PsfStubMakerWorker.MyVbDefault;
            this.tbMyVbSize.Text = PsfStubMakerWorker.MyVbSizeDefault;        
        }

        private bool validateInputs()
        {
            bool ret = true;

            if (cbOverrideDriverOffset.Checked)
            {
                ret = ret && AVgmtForm.checkTextBox(this.tbPsfDrvLoad.Text, this.lblPsfDrvLoad.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbPsfDrvSize.Text, this.lblPsfDrvSize.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbPsfDrvParam.Text, this.lblPsfDrvParam.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbPadDrvParamSize.Text, this.lblPadDrvParamSize.Text);

                ret = ret && AVgmtForm.checkTextBox(this.tbMySeq.Text, this.lblMySeq.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbMySeqSize.Text, this.lblMySeqSize.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbMyVh.Text, this.lblMyVh.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbMyVhSize.Text, this.lblMyVhSize.Text);

                ret = ret && AVgmtForm.checkTextBox(this.tbMyVb.Text, this.lblMyVb.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbMyVbSize.Text, this.lblMyVbSize.Text);
            }
            
            return ret;
        }

        private void btnLoadDefaults_Click(object sender, System.EventArgs e)
        {
            cbOverrideDriverOffset.Checked = true;
            this.overrideDriverOffset();
            this.loadDefaults();
        }

        private void cbOverrideDriverOffset_CheckedChanged(object sender, System.EventArgs e)
        {
            this.overrideDriverOffset();
        }

        public void overrideDriverOffset()
        {
            if (!cbOverrideDriverOffset.Checked)
            {
                this.tbPsfDrvLoad.ReadOnly = true;
                this.tbPsfDrvSize.ReadOnly = true;
                this.tbPsfDrvParam.ReadOnly = true;
                this.tbPadDrvParamSize.ReadOnly = true;
                this.tbMySeq.ReadOnly = true;
                this.tbMySeqSize.ReadOnly = true;
                this.tbMyVh.ReadOnly = true;
                this.tbMyVhSize.ReadOnly = true;
                this.tbMyVb.ReadOnly = true;
                this.tbMyVbSize.ReadOnly = true;

                this.tbPsfDrvLoad.Clear();
                this.tbPsfDrvSize.Clear();
                this.tbPsfDrvParam.Clear();
                this.tbPadDrvParamSize.Clear();
                this.tbMySeq.Clear();
                this.tbMySeqSize.Clear();
                this.tbMyVh.Clear();
                this.tbMyVhSize.Clear();
                this.tbMyVb.Clear();
                this.tbMyVbSize.Clear();

                this.btnHighestAddress.Enabled = false;
            }
            else
            {
                this.tbPsfDrvLoad.ReadOnly = false;
                this.tbPsfDrvSize.ReadOnly = false;
                this.tbPsfDrvParam.ReadOnly = false;
                this.tbPadDrvParamSize.ReadOnly = false;
                this.tbMySeq.ReadOnly = false;
                this.tbMySeqSize.ReadOnly = false;
                this.tbMyVh.ReadOnly = false;
                this.tbMyVhSize.ReadOnly = false;
                this.tbMyVb.ReadOnly = false;
                this.tbMyVbSize.ReadOnly = false;                
                
                /*
                this.tbPsfDrvLoad.Text = PsfStubMakerWorker.PsfDrvLoadDefault;
                this.tbPsfDrvSize.Text = PsfStubMakerWorker.PsfDrvSizeDefault;
                this.tbPsfDrvParam.Text = PsfStubMakerWorker.PsfDrvParamDefault;
                this.tbPadDrvParamSize.Text = PsfStubMakerWorker.PsfDrvParamSizeDefault;
                this.tbMySeq.Text = PsfStubMakerWorker.MySeqDefault;
                this.tbMySeqSize.Text = PsfStubMakerWorker.MySeqSizeDefault;
                this.tbMyVh.Text = PsfStubMakerWorker.MyVhDefault;
                this.tbMyVhSize.Text = PsfStubMakerWorker.MyVhSizeDefault;
                this.tbMyVb.Text = PsfStubMakerWorker.MyVbDefault;
                this.tbMyVbSize.Text = PsfStubMakerWorker.MyVbSizeDefault;
                 */

                this.btnHighestAddress.Enabled = true;
            }        
        }

        public string GetSeqOffset()
        {
            return this.tbMySeq.Text;
        }
        public string GetSeqSize()
        {
            return this.tbMySeqSize.Text;
        }
        public string GetVhOffset()
        {
            return this.tbMyVh.Text;
        }
        public string GetVbOffset()
        {
            return this.tbMyVb.Text;
        }

        public string GetParamOffset()
        {
            return this.tbPsfDrvParam.Text;
        }

        private void cbRelaxPsfLoadAddress_CheckedChanged(object sender, System.EventArgs e)
        {
            if (cbRelaxPsfLoadAddress.Checked)
            {
                this.cbOverrideDriverOffset.Checked = true;
            }
            else
            {
                this.cbOverrideDriverOffset.Checked = false;
            }
        }

        private bool validateMoveHighestInputs()
        {
            bool ret = true;

            if (cbOverrideDriverOffset.Checked)
            {
                ret = ret && AVgmtForm.checkTextBox(this.tbPsfDrvSize.Text, this.lblPsfDrvSize.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbPadDrvParamSize.Text, this.lblPadDrvParamSize.Text);

                ret = ret && AVgmtForm.checkTextBox(this.tbMySeqSize.Text, this.lblMySeqSize.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbMyVhSize.Text, this.lblMyVhSize.Text);
                ret = ret && AVgmtForm.checkTextBox(this.tbMyVbSize.Text, this.lblMyVbSize.Text);
            }

            return ret;
        }

        private void btnHighestAddress_Click(object sender, System.EventArgs e)
        {
            if (this.validateMoveHighestInputs())
            {
                uint vbLocation, vhLocation, seqLocation, paramLocation, psdrvLocation;

                try
                {
                    // calculate locations
                    vbLocation = Psf.MAX_TEXT_SECTION_OFFSET -
                        (uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(this.tbMyVbSize.Text);
                    vhLocation = vbLocation -
                        (uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(this.tbMyVhSize.Text);
                    seqLocation = vhLocation -
                        (uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(this.tbMySeqSize.Text);
                    paramLocation = seqLocation -
                        (uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(this.tbPadDrvParamSize.Text);
                    psdrvLocation = paramLocation -
                        (uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(this.tbPsfDrvSize.Text);

                    // update form
                    this.tbPsfDrvLoad.Text = "0x" + psdrvLocation.ToString("X8");
                    this.tbPsfDrvParam.Text = "0x" + paramLocation.ToString("X8");

                    this.tbMySeq.Text = "0x" + seqLocation.ToString("X8");
                    this.tbMyVh.Text = "0x" + vhLocation.ToString("X8");
                    this.tbMyVb.Text = "0x" + vbLocation.ToString("X8");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
