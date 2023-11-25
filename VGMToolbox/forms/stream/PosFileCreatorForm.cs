using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.tools.stream;

namespace VGMToolbox.forms.stream
{
    public partial class PosFileCreatorForm : AVgmtForm
    {
        public PosFileCreatorForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            this.lblTitle.Text = ".POS File Maker";
            this.tbOutput.Text = "Create .POS files for looping .WAV files.";

            InitializeComponent();

            this.btnDoTask.Hide();

            this.tbOutputFileMask.Text = "$B.$E.WAV";

            this.rbLoopStartRiffOffset.Checked = true;

            this.rbLoopEnd.Checked = true;
            this.rbLoopEndRiffOffset.Checked = true;

            this.rbLoopShiftWavCompare.Checked = true;
            this.cbLoopShiftPredictShift.Checked = true;

            this.cbOptionsCreateM3u.Checked = true;

            // init functions
            this.doLoopStartRadios();
            this.doLoopEndRadios();
            this.doLoopShiftRadios();
        }





        //----------------------------
        // background worker defaults
        //----------------------------
        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new PosFileCreatorWorker();
        }
        protected override string getCancelMessage()
        {
            return "Creating POS File...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "Creating POS File...Complete";
        }
        protected override string getBeginMessage()
        {
            return "Creating POS File...Begin";
        }

        private void PosFileCreatorForm_DragDrop(object sender, DragEventArgs e)
        {
            PosFileCreatorWorker.PosFileCreatorStruct taskStruct = new PosFileCreatorWorker.PosFileCreatorStruct();

            // paths
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            taskStruct.SourcePaths = s;

            // public string OutputFolder { set; get; }
            taskStruct.OutputFileMask = this.tbOutputFileMask.Text;

            // Loop Start
            taskStruct.DoLoopStartStatic = this.rbLoopStartStatic.Checked;
            taskStruct.DoLoopStartOffset = this.rbLoopStartOffset.Checked;
            taskStruct.DoLoopStartRiffOffset = this.rbLoopStartRiffOffset.Checked;
            taskStruct.DoLoopStartByteStringOffset = this.rbLoopStartByteStringOffset.Checked;

            taskStruct.LoopStartStaticValue = this.tbLoopStartStatic.Text;
            taskStruct.LoopStartCalculatingOffset = this.LoopStartCalculatingOffsetDescriptionControl.GetOffsetValues();
            taskStruct.LoopStartRiffCalculatingOffset = this.LoopStartRiffCalculatingOffsetDescriptionControl.GetOffsetValues();
            taskStruct.LoopStartByteSearchCalculatingOffset = this.LoopStartByteSearchCalculatingOffsetDescriptionControl.GetOffsetValues();

            // Loop End
            taskStruct.LoopEndIsLoopEnd = this.rbLoopEnd.Checked;
            taskStruct.LoopEndIsLoopLength = this.rbLoopLength.Checked;
            
            taskStruct.DoLoopEndStatic = this.rbLoopEndStatic.Checked;
            taskStruct.DoLoopEndOffset = this.rbLoopEndOffset.Checked;
            taskStruct.DoLoopEndRiffOffset = this.rbLoopEndRiffOffset.Checked;
            taskStruct.DoLoopEndByteStringOffset = this.rbLoopEndByteStringOffset.Checked;

            taskStruct.LoopEndStaticValue = this.tbLoopEndStatic.Text;
            taskStruct.LoopEndCalculatingOffset = this.LoopEndCalculatingOffsetDescriptionControl.GetOffsetValues();
            taskStruct.LoopEndRiffCalculatingOffset = this.LoopEndRiffCalculatingOffsetDescriptionControl.GetOffsetValues();
            taskStruct.LoopEndByteSearchCalculatingOffset = this.LoopEndByteSearchCalculatingOffsetDescriptionControl.GetOffsetValues();

            // Loop Shift
            taskStruct.DoStaticLoopShift = this.rbLoopShiftStatic.Checked;
            taskStruct.DoLoopShiftWavCompare = this.rbLoopShiftWavCompare.Checked;
            taskStruct.PredictLoopShiftForBatch = this.cbLoopShiftPredictShift.Checked;
            taskStruct.StaticLoopShiftValue = this.tbLoopShiftStatic.Text;

            // Other Options
            taskStruct.CreateM3u = this.cbOptionsCreateM3u.Checked;

            // Execute Worker
            base.backgroundWorker_Execute(taskStruct);
        }

        private void PosFileCreatorForm_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        // radio buttons
        private void doLoopShiftRadios()
        {
            this.tbLoopShiftStatic.Enabled = this.rbLoopShiftStatic.Checked;
            this.tbLoopShiftStatic.ReadOnly = !this.rbLoopShiftStatic.Checked;

            this.cbLoopShiftPredictShift.Enabled = this.rbLoopShiftWavCompare.Checked;
            
            if (!this.rbLoopShiftWavCompare.Checked)
            {
                this.cbLoopShiftPredictShift.Checked = false;
            }
        }        
        private void rbLoopShiftStatic_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopShiftRadios();
        }
        private void rbLoopShiftWavCompare_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopShiftRadios();
        }

        private void doLoopStartRadios()
        {
            this.tbLoopStartStatic.Enabled = this.rbLoopStartStatic.Checked;
            this.tbLoopStartStatic.ReadOnly = !this.rbLoopStartStatic.Checked;

            this.LoopStartCalculatingOffsetDescriptionControl.Enabled = this.rbLoopStartOffset.Checked;
            this.LoopStartRiffCalculatingOffsetDescriptionControl.Enabled = this.rbLoopStartRiffOffset.Checked;
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl.Enabled = this.rbLoopStartByteStringOffset.Checked;   
        }
        private void rbLoopStartStatic_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopStartRadios();
        }
        private void rbLoopStartOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopStartRadios();
        }
        private void rbLoopStartRiffOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopStartRadios();
        }
        private void rbLoopStartByteStringOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopStartRadios();
        }

        private void doLoopEndRadios()
        {
            this.tbLoopEndStatic.Enabled = this.rbLoopEndStatic.Checked;
            this.tbLoopEndStatic.ReadOnly = !this.rbLoopEndStatic.Checked;

            this.LoopEndCalculatingOffsetDescriptionControl.Enabled = this.rbLoopEndOffset.Checked;
            this.LoopEndRiffCalculatingOffsetDescriptionControl.Enabled = this.rbLoopEndRiffOffset.Checked;
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl.Enabled = this.rbLoopEndByteStringOffset.Checked;   
        }
        private void rbLoopEndStatic_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopEndRadios();
        }
        private void rbLoopEndOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopEndRadios();
        }
        private void rbLoopEndRiffOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopEndRadios();
        }
        private void rbLoopEndByteStringOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doLoopEndRadios();
        }

        



        //------------
        // Validation
        //------------
    }
}
