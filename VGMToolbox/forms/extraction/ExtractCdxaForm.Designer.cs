namespace VGMToolbox.forms.extraction
{
    partial class ExtractCdxaForm
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
            this.rbUseSilentBlockEof = new System.Windows.Forms.RadioButton();
            this.rbUseTrackEof = new System.Windows.Forms.RadioButton();
            this.cbDoTwoPass = new System.Windows.Forms.CheckBox();
            this.cbFilterById = new System.Windows.Forms.CheckBox();
            this.lblSilentBlocks = new System.Windows.Forms.Label();
            this.silentFrameCounter = new System.Windows.Forms.NumericUpDown();
            this.cbPatchByte0x11 = new System.Windows.Forms.CheckBox();
            this.cbAddRiffHeader = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.silentFrameCounter)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 520);
            this.pnlLabels.Size = new System.Drawing.Size(663, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(663, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 443);
            this.tbOutput.Size = new System.Drawing.Size(663, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 423);
            this.pnlButtons.Size = new System.Drawing.Size(663, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(603, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(543, 0);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.grpOptions);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(663, 400);
            this.grpSource.TabIndex = 5;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source Files";
            this.grpSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSource_DragDrop);
            this.grpSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.rbUseSilentBlockEof);
            this.grpOptions.Controls.Add(this.rbUseTrackEof);
            this.grpOptions.Controls.Add(this.cbDoTwoPass);
            this.grpOptions.Controls.Add(this.cbFilterById);
            this.grpOptions.Controls.Add(this.lblSilentBlocks);
            this.grpOptions.Controls.Add(this.silentFrameCounter);
            this.grpOptions.Controls.Add(this.cbPatchByte0x11);
            this.grpOptions.Controls.Add(this.cbAddRiffHeader);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 170);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(657, 227);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // rbUseSilentBlockEof
            // 
            this.rbUseSilentBlockEof.AutoSize = true;
            this.rbUseSilentBlockEof.Location = new System.Drawing.Point(6, 43);
            this.rbUseSilentBlockEof.Name = "rbUseSilentBlockEof";
            this.rbUseSilentBlockEof.Size = new System.Drawing.Size(193, 17);
            this.rbUseSilentBlockEof.TabIndex = 7;
            this.rbUseSilentBlockEof.TabStop = true;
            this.rbUseSilentBlockEof.Text = "Use Silent Blocks to determine EOF";
            this.rbUseSilentBlockEof.UseVisualStyleBackColor = true;
            this.rbUseSilentBlockEof.CheckedChanged += new System.EventHandler(this.rbUseSilentBlockEof_CheckedChanged);
            // 
            // rbUseTrackEof
            // 
            this.rbUseTrackEof.AutoSize = true;
            this.rbUseTrackEof.Location = new System.Drawing.Point(6, 19);
            this.rbUseTrackEof.Name = "rbUseTrackEof";
            this.rbUseTrackEof.Size = new System.Drawing.Size(239, 17);
            this.rbUseTrackEof.TabIndex = 6;
            this.rbUseTrackEof.TabStop = true;
            this.rbUseTrackEof.Text = "Use \"End of Track\" marker to determine EOF";
            this.rbUseTrackEof.UseVisualStyleBackColor = true;
            this.rbUseTrackEof.CheckedChanged += new System.EventHandler(this.rbUseTrackEof_CheckedChanged);
            // 
            // cbDoTwoPass
            // 
            this.cbDoTwoPass.AutoSize = true;
            this.cbDoTwoPass.Location = new System.Drawing.Point(25, 101);
            this.cbDoTwoPass.Name = "cbDoTwoPass";
            this.cbDoTwoPass.Size = new System.Drawing.Size(343, 30);
            this.cbDoTwoPass.TabIndex = 5;
            this.cbDoTwoPass.Text = "Use Two-Pass Method (use this if multiple tracks are extracted to a \r\nsingle file" +
                " even when Silent Frames setting equals 1)";
            this.cbDoTwoPass.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.cbDoTwoPass.UseVisualStyleBackColor = true;
            // 
            // cbFilterById
            // 
            this.cbFilterById.AutoSize = true;
            this.cbFilterById.Checked = true;
            this.cbFilterById.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFilterById.Location = new System.Drawing.Point(6, 157);
            this.cbFilterById.Name = "cbFilterById";
            this.cbFilterById.Size = new System.Drawing.Size(266, 17);
            this.cbFilterById.TabIndex = 4;
            this.cbFilterById.Text = "Filter by Block ID (uncheck if no files are extracted)";
            this.cbFilterById.UseVisualStyleBackColor = true;
            // 
            // lblSilentBlocks
            // 
            this.lblSilentBlocks.Location = new System.Drawing.Point(71, 66);
            this.lblSilentBlocks.Name = "lblSilentBlocks";
            this.lblSilentBlocks.Size = new System.Drawing.Size(353, 46);
            this.lblSilentBlocks.TabIndex = 3;
            this.lblSilentBlocks.Text = "Number of Silent Frames to determine EOF (decrease this value if multiple tracks " +
                "are extracted to a single file).";
            // 
            // silentFrameCounter
            // 
            this.silentFrameCounter.Location = new System.Drawing.Point(25, 66);
            this.silentFrameCounter.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.silentFrameCounter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.silentFrameCounter.Name = "silentFrameCounter";
            this.silentFrameCounter.Size = new System.Drawing.Size(40, 20);
            this.silentFrameCounter.TabIndex = 2;
            this.silentFrameCounter.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbPatchByte0x11
            // 
            this.cbPatchByte0x11.AutoSize = true;
            this.cbPatchByte0x11.Checked = true;
            this.cbPatchByte0x11.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPatchByte0x11.Location = new System.Drawing.Point(6, 203);
            this.cbPatchByte0x11.Name = "cbPatchByte0x11";
            this.cbPatchByte0x11.Size = new System.Drawing.Size(330, 17);
            this.cbPatchByte0x11.TabIndex = 1;
            this.cbPatchByte0x11.Text = "Patch byte at 0x11 to  equal 0x00 (also needed by most players).";
            this.cbPatchByte0x11.UseVisualStyleBackColor = true;
            // 
            // cbAddRiffHeader
            // 
            this.cbAddRiffHeader.AutoSize = true;
            this.cbAddRiffHeader.Checked = true;
            this.cbAddRiffHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAddRiffHeader.Location = new System.Drawing.Point(6, 180);
            this.cbAddRiffHeader.Name = "cbAddRiffHeader";
            this.cbAddRiffHeader.Size = new System.Drawing.Size(261, 17);
            this.cbAddRiffHeader.TabIndex = 0;
            this.cbAddRiffHeader.Text = "Add RIFF header (currently needed by all players).";
            this.cbAddRiffHeader.UseVisualStyleBackColor = true;
            // 
            // ExtractCdxaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 561);
            this.Controls.Add(this.grpSource);
            this.Name = "ExtractCdxaForm";
            this.Text = "ExtractCdxaForm";
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
            ((System.ComponentModel.ISupportInitialize)(this.silentFrameCounter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox cbPatchByte0x11;
        private System.Windows.Forms.CheckBox cbAddRiffHeader;
        private System.Windows.Forms.Label lblSilentBlocks;
        private System.Windows.Forms.NumericUpDown silentFrameCounter;
        private System.Windows.Forms.CheckBox cbFilterById;
        private System.Windows.Forms.CheckBox cbDoTwoPass;
        private System.Windows.Forms.RadioButton rbUseSilentBlockEof;
        private System.Windows.Forms.RadioButton rbUseTrackEof;
    }
}