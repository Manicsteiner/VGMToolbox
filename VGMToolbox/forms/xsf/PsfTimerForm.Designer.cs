namespace VGMToolbox.forms.xsf
{
    partial class PsfTimerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.sepIndexParameterLengthComboBox = new System.Windows.Forms.ComboBox();
            this.lblSepIndexParameterLength = new System.Windows.Forms.Label();
            this.lblSepIndexOffset = new System.Windows.Forms.Label();
            this.tbSepIndexOffset = new System.Windows.Forms.TextBox();
            this.cbLoopEntireTrack = new System.Windows.Forms.CheckBox();
            this.rbForceSeqType = new System.Windows.Forms.RadioButton();
            this.rbForceSepType = new System.Windows.Forms.RadioButton();
            this.cbForce2Loops = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 452);
            this.pnlLabels.Size = new System.Drawing.Size(850, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(850, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 375);
            this.tbOutput.Size = new System.Drawing.Size(850, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 355);
            this.pnlButtons.Size = new System.Drawing.Size(850, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(790, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(730, 0);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.grpOptions);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(850, 332);
            this.grpSource.TabIndex = 5;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source Files";
            this.grpSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSource_DragDrop);
            this.grpSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.sepIndexParameterLengthComboBox);
            this.grpOptions.Controls.Add(this.lblSepIndexParameterLength);
            this.grpOptions.Controls.Add(this.lblSepIndexOffset);
            this.grpOptions.Controls.Add(this.tbSepIndexOffset);
            this.grpOptions.Controls.Add(this.cbLoopEntireTrack);
            this.grpOptions.Controls.Add(this.rbForceSeqType);
            this.grpOptions.Controls.Add(this.rbForceSepType);
            this.grpOptions.Controls.Add(this.cbForce2Loops);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 166);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(844, 163);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // sepIndexParameterLengthComboBox
            // 
            this.sepIndexParameterLengthComboBox.FormattingEnabled = true;
            this.sepIndexParameterLengthComboBox.Location = new System.Drawing.Point(339, 132);
            this.sepIndexParameterLengthComboBox.Name = "sepIndexParameterLengthComboBox";
            this.sepIndexParameterLengthComboBox.Size = new System.Drawing.Size(41, 21);
            this.sepIndexParameterLengthComboBox.TabIndex = 8;
            this.sepIndexParameterLengthComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sepIndexParameterLengthComboBox_KeyPress);
            this.sepIndexParameterLengthComboBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sepIndexParameterLengthComboBox_KeyUp);
            this.sepIndexParameterLengthComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sepIndexParameterLengthComboBox_KeyDown);
            // 
            // lblSepIndexParameterLength
            // 
            this.lblSepIndexParameterLength.AutoSize = true;
            this.lblSepIndexParameterLength.Location = new System.Drawing.Point(219, 135);
            this.lblSepIndexParameterLength.Name = "lblSepIndexParameterLength";
            this.lblSepIndexParameterLength.Size = new System.Drawing.Size(114, 13);
            this.lblSepIndexParameterLength.TabIndex = 7;
            this.lblSepIndexParameterLength.Text = "Index Paramter Length";
            // 
            // lblSepIndexOffset
            // 
            this.lblSepIndexOffset.Location = new System.Drawing.Point(94, 103);
            this.lblSepIndexOffset.Name = "lblSepIndexOffset";
            this.lblSepIndexOffset.Size = new System.Drawing.Size(239, 26);
            this.lblSepIndexOffset.TabIndex = 6;
            this.lblSepIndexOffset.Text = "Offset of SEQ Index for .minipsfs, use PSX offset. See stub source for value PARA" +
                "M_SEQNUM.";
            // 
            // tbSepIndexOffset
            // 
            this.tbSepIndexOffset.Location = new System.Drawing.Point(339, 100);
            this.tbSepIndexOffset.Name = "tbSepIndexOffset";
            this.tbSepIndexOffset.Size = new System.Drawing.Size(80, 20);
            this.tbSepIndexOffset.TabIndex = 5;
            // 
            // cbLoopEntireTrack
            // 
            this.cbLoopEntireTrack.AutoSize = true;
            this.cbLoopEntireTrack.Location = new System.Drawing.Point(6, 19);
            this.cbLoopEntireTrack.Name = "cbLoopEntireTrack";
            this.cbLoopEntireTrack.Size = new System.Drawing.Size(561, 17);
            this.cbLoopEntireTrack.TabIndex = 4;
            this.cbLoopEntireTrack.Text = "Loop entire track (this is useful for tracks that loop the entire track, but do n" +
                "ot actually contain the loop command).";
            this.cbLoopEntireTrack.UseVisualStyleBackColor = true;
            // 
            // rbForceSeqType
            // 
            this.rbForceSeqType.AutoSize = true;
            this.rbForceSeqType.Location = new System.Drawing.Point(6, 78);
            this.rbForceSeqType.Name = "rbForceSeqType";
            this.rbForceSeqType.Size = new System.Drawing.Size(74, 17);
            this.rbForceSeqType.TabIndex = 3;
            this.rbForceSeqType.TabStop = true;
            this.rbForceSeqType.Text = "SEQ Type";
            this.rbForceSeqType.UseVisualStyleBackColor = true;
            this.rbForceSeqType.CheckedChanged += new System.EventHandler(this.rbForceSeqType_CheckedChanged);
            // 
            // rbForceSepType
            // 
            this.rbForceSepType.AutoSize = true;
            this.rbForceSepType.Location = new System.Drawing.Point(6, 101);
            this.rbForceSepType.Name = "rbForceSepType";
            this.rbForceSepType.Size = new System.Drawing.Size(73, 17);
            this.rbForceSepType.TabIndex = 2;
            this.rbForceSepType.TabStop = true;
            this.rbForceSepType.Text = "SEP Type";
            this.rbForceSepType.UseVisualStyleBackColor = true;
            this.rbForceSepType.CheckedChanged += new System.EventHandler(this.rbForceSepType_CheckedChanged);
            // 
            // cbForce2Loops
            // 
            this.cbForce2Loops.AutoSize = true;
            this.cbForce2Loops.Location = new System.Drawing.Point(6, 55);
            this.cbForce2Loops.Name = "cbForce2Loops";
            this.cbForce2Loops.Size = new System.Drawing.Size(413, 17);
            this.cbForce2Loops.TabIndex = 0;
            this.cbForce2Loops.Text = "Force 2 loops for all loops found (Not recommended, use only for overtimed tracks" +
                ")";
            this.cbForce2Loops.UseVisualStyleBackColor = true;
            // 
            // PsfTimerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 493);
            this.Controls.Add(this.grpSource);
            this.Name = "PsfTimerForm";
            this.Text = "PsfTimerForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSource, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSource.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox cbForce2Loops;
        private System.Windows.Forms.RadioButton rbForceSeqType;
        private System.Windows.Forms.RadioButton rbForceSepType;
        private System.Windows.Forms.CheckBox cbLoopEntireTrack;
        private System.Windows.Forms.Label lblSepIndexOffset;
        private System.Windows.Forms.TextBox tbSepIndexOffset;
        private System.Windows.Forms.Label lblSepIndexParameterLength;
        private System.Windows.Forms.ComboBox sepIndexParameterLengthComboBox;
    }
}