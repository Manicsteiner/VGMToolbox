namespace VGMToolbox.forms.stream
{
    partial class XmashMashForm
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
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.chkIgnoreXmashFailure = new System.Windows.Forms.CheckBox();
            this.chkInterleaveMultiChannelOutput = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 405);
            this.pnlLabels.Size = new System.Drawing.Size(843, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(843, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 328);
            this.tbOutput.Size = new System.Drawing.Size(843, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 308);
            this.pnlButtons.Size = new System.Drawing.Size(843, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(783, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(723, 0);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.chkIgnoreXmashFailure);
            this.grpOptions.Controls.Add(this.chkInterleaveMultiChannelOutput);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(0, 242);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(843, 66);
            this.grpOptions.TabIndex = 5;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // chkIgnoreXmashFailure
            // 
            this.chkIgnoreXmashFailure.AutoSize = true;
            this.chkIgnoreXmashFailure.Location = new System.Drawing.Point(6, 42);
            this.chkIgnoreXmashFailure.Name = "chkIgnoreXmashFailure";
            this.chkIgnoreXmashFailure.Size = new System.Drawing.Size(303, 17);
            this.chkIgnoreXmashFailure.TabIndex = 1;
            this.chkIgnoreXmashFailure.Text = "Ignore XMAsh failure (Use for files already in XMA1 format).";
            this.chkIgnoreXmashFailure.UseVisualStyleBackColor = true;
            // 
            // chkInterleaveMultiChannelOutput
            // 
            this.chkInterleaveMultiChannelOutput.AutoSize = true;
            this.chkInterleaveMultiChannelOutput.Checked = true;
            this.chkInterleaveMultiChannelOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInterleaveMultiChannelOutput.Location = new System.Drawing.Point(6, 19);
            this.chkInterleaveMultiChannelOutput.Name = "chkInterleaveMultiChannelOutput";
            this.chkInterleaveMultiChannelOutput.Size = new System.Drawing.Size(268, 17);
            this.chkInterleaveMultiChannelOutput.TabIndex = 0;
            this.chkInterleaveMultiChannelOutput.Text = "Interleave multi-channel output into single WAV file.";
            this.chkInterleaveMultiChannelOutput.UseVisualStyleBackColor = true;
            // 
            // XmashMashForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 446);
            this.Controls.Add(this.grpOptions);
            this.Name = "XmashMashForm";
            this.Text = "XmashMashForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.XmashMashForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.XmashMashForm_DragEnter);
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox chkInterleaveMultiChannelOutput;
        private System.Windows.Forms.CheckBox chkIgnoreXmashFailure;
    }
}