using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;

namespace VGMToolbox.forms.xsf
{
    public partial class PsfTimerForm : AVgmtForm
    {        
        public PsfTimerForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_SeqExtractor_Title"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_SeqExtractor_IntroText"];
            this.btnDoTask.Hide();
            
            InitializeComponent();

            this.grpSource.Text =
                ConfigurationManager.AppSettings["Form_SeqExtractor_GroupSource"];
            this.grpOptions.Text =
                ConfigurationManager.AppSettings["Form_SeqExtractor_GroupOptions"];
            this.cbForce2Loops.Text =
                ConfigurationManager.AppSettings["Form_SeqExtractor_CheckBoxForce2Loops"];
            this.rbForceSepType.Text =
                ConfigurationManager.AppSettings["Form_SeqExtractor_RadioForceSepType"];
            this.rbForceSeqType.Text =
                ConfigurationManager.AppSettings["Form_SeqExtractor_RadioForceSeqType"];
            this.cbLoopEntireTrack.Text =
                ConfigurationManager.AppSettings["Form_SeqExtractor_CheckBoxLoopEntireTrack"];

            this.grpSource.AllowDrop = true;
            this.rbForceSeqType.Checked = true;
            this.tbSepIndexOffset.Text = "0x80101014";
            this.initializeSepIndexParameterLengthComboBox();
            this.doSepSeqCheckChanged();
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void tbSource_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (this.ValidateInputs())
            {
                PsfTimerWorker.PsxSeqExtractStruct psxStruct = new PsfTimerWorker.PsxSeqExtractStruct();
                psxStruct.SourcePaths = s;
                psxStruct.force2Loops = cbForce2Loops.Checked;
                psxStruct.forceSepType = rbForceSepType.Checked;
                psxStruct.forceSeqType = rbForceSeqType.Checked;
                psxStruct.loopEntireTrack = cbLoopEntireTrack.Checked;

                if (psxStruct.forceSepType)
                {
                    psxStruct.SepSeqOffset = this.tbSepIndexOffset.Text;
                    psxStruct.SepSeqIndexLength = (string)this.sepIndexParameterLengthComboBox.SelectedItem;
                }

                base.backgroundWorker_Execute(psxStruct);
            }
        }
        
        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new PsfTimerWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_SeqExtractor_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_SeqExtractor_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_SeqExtractor_MessageBegin"];
        }

        private void rbForceSepType_CheckedChanged(object sender, EventArgs e)
        {
            this.doSepSeqCheckChanged();
        }

        private void rbForceSeqType_CheckedChanged(object sender, EventArgs e)
        {
            this.doSepSeqCheckChanged();
        }

        private void doSepSeqCheckChanged()
        {
            if (rbForceSepType.Checked)
            {
                this.lblSepIndexOffset.Enabled = true;
                this.tbSepIndexOffset.Enabled = true;
                this.tbSepIndexOffset.ReadOnly = false;

                this.lblSepIndexParameterLength.Enabled = true;
                this.sepIndexParameterLengthComboBox.Enabled = true;
            }
            else
            {
                this.lblSepIndexOffset.Enabled = false;
                this.tbSepIndexOffset.Enabled = false;
                this.tbSepIndexOffset.ReadOnly = true;

                this.lblSepIndexParameterLength.Enabled = false;
                this.sepIndexParameterLengthComboBox.Enabled = false;
            }
        }

        private void initializeSepIndexParameterLengthComboBox()
        {
            sepIndexParameterLengthComboBox.Items.Add("1");
            sepIndexParameterLengthComboBox.Items.Add("2");
            sepIndexParameterLengthComboBox.Items.Add("4");

            sepIndexParameterLengthComboBox.SelectedIndex = 2;
        }
        
        private void sepIndexParameterLengthComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void sepIndexParameterLengthComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void sepIndexParameterLengthComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private bool ValidateInputs()
        {
            bool ret = true;
            
            if (this.rbForceSepType.Checked)
            {
                if (!AVgmtForm.checkTextBox(this.tbSepIndexOffset.Text, "SEQ Offset"))
                {
                    ret = false;
                }
            }

            return ret;
        }
    }
}
