namespace VGMToolbox.forms.stream
{
    partial class MpegDemuxForm
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
            this.grpSourceFiles = new System.Windows.Forms.GroupBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.rbExtractVideoOnly = new System.Windows.Forms.RadioButton();
            this.rbExtractAudioOnly = new System.Windows.Forms.RadioButton();
            this.rbExtractAudioAndVideo = new System.Windows.Forms.RadioButton();
            this.cbAddHeader = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboFormat = new System.Windows.Forms.ComboBox();
            this.cbSplitAudioTracks = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSourceFiles.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 495);
            this.pnlLabels.Size = new System.Drawing.Size(1003, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(1003, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 418);
            this.tbOutput.Size = new System.Drawing.Size(1003, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 398);
            this.pnlButtons.Size = new System.Drawing.Size(1003, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(943, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(883, 0);
            // 
            // grpSourceFiles
            // 
            this.grpSourceFiles.Controls.Add(this.grpOptions);
            this.grpSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSourceFiles.Location = new System.Drawing.Point(0, 23);
            this.grpSourceFiles.Name = "grpSourceFiles";
            this.grpSourceFiles.Size = new System.Drawing.Size(1003, 375);
            this.grpSourceFiles.TabIndex = 5;
            this.grpSourceFiles.TabStop = false;
            this.grpSourceFiles.Text = "Drop Files Here";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbSplitAudioTracks);
            this.grpOptions.Controls.Add(this.rbExtractVideoOnly);
            this.grpOptions.Controls.Add(this.rbExtractAudioOnly);
            this.grpOptions.Controls.Add(this.rbExtractAudioAndVideo);
            this.grpOptions.Controls.Add(this.cbAddHeader);
            this.grpOptions.Controls.Add(this.label1);
            this.grpOptions.Controls.Add(this.comboFormat);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 287);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(997, 85);
            this.grpOptions.TabIndex = 0;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // rbExtractVideoOnly
            // 
            this.rbExtractVideoOnly.AutoSize = true;
            this.rbExtractVideoOnly.Location = new System.Drawing.Point(253, 62);
            this.rbExtractVideoOnly.Name = "rbExtractVideoOnly";
            this.rbExtractVideoOnly.Size = new System.Drawing.Size(112, 17);
            this.rbExtractVideoOnly.TabIndex = 5;
            this.rbExtractVideoOnly.Text = "Extract Video Only";
            this.rbExtractVideoOnly.UseVisualStyleBackColor = true;
            // 
            // rbExtractAudioOnly
            // 
            this.rbExtractAudioOnly.AutoSize = true;
            this.rbExtractAudioOnly.Location = new System.Drawing.Point(253, 38);
            this.rbExtractAudioOnly.Name = "rbExtractAudioOnly";
            this.rbExtractAudioOnly.Size = new System.Drawing.Size(112, 17);
            this.rbExtractAudioOnly.TabIndex = 4;
            this.rbExtractAudioOnly.Text = "Extract Audio Only";
            this.rbExtractAudioOnly.UseVisualStyleBackColor = true;
            // 
            // rbExtractAudioAndVideo
            // 
            this.rbExtractAudioAndVideo.AutoSize = true;
            this.rbExtractAudioAndVideo.Checked = true;
            this.rbExtractAudioAndVideo.Location = new System.Drawing.Point(253, 14);
            this.rbExtractAudioAndVideo.Name = "rbExtractAudioAndVideo";
            this.rbExtractAudioAndVideo.Size = new System.Drawing.Size(139, 17);
            this.rbExtractAudioAndVideo.TabIndex = 3;
            this.rbExtractAudioAndVideo.TabStop = true;
            this.rbExtractAudioAndVideo.Text = "Extract Audio and Video";
            this.rbExtractAudioAndVideo.UseVisualStyleBackColor = true;
            // 
            // cbAddHeader
            // 
            this.cbAddHeader.AutoSize = true;
            this.cbAddHeader.Location = new System.Drawing.Point(9, 40);
            this.cbAddHeader.Name = "cbAddHeader";
            this.cbAddHeader.Size = new System.Drawing.Size(130, 17);
            this.cbAddHeader.TabIndex = 2;
            this.cbAddHeader.Text = "Add Header to Output";
            this.cbAddHeader.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Format";
            // 
            // comboFormat
            // 
            this.comboFormat.FormattingEnabled = true;
            this.comboFormat.Location = new System.Drawing.Point(51, 13);
            this.comboFormat.Name = "comboFormat";
            this.comboFormat.Size = new System.Drawing.Size(196, 21);
            this.comboFormat.TabIndex = 0;
            this.comboFormat.SelectedIndexChanged += new System.EventHandler(this.comboFormat_SelectedIndexChanged);
            // 
            // cbSplitAudioTracks
            // 
            this.cbSplitAudioTracks.AutoSize = true;
            this.cbSplitAudioTracks.Location = new System.Drawing.Point(9, 62);
            this.cbSplitAudioTracks.Name = "cbSplitAudioTracks";
            this.cbSplitAudioTracks.Size = new System.Drawing.Size(112, 17);
            this.cbSplitAudioTracks.TabIndex = 6;
            this.cbSplitAudioTracks.Text = "Split Audio Tracks";
            this.cbSplitAudioTracks.UseVisualStyleBackColor = true;
            // 
            // MpegDemuxForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 536);
            this.Controls.Add(this.grpSourceFiles);
            this.Name = "MpegDemuxForm";
            this.Text = "MpegDemuxForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MpegDemuxForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MpegDemuxForm_DragEnter);
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSourceFiles, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSourceFiles.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSourceFiles;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboFormat;
        private System.Windows.Forms.CheckBox cbAddHeader;
        private System.Windows.Forms.RadioButton rbExtractVideoOnly;
        private System.Windows.Forms.RadioButton rbExtractAudioOnly;
        private System.Windows.Forms.RadioButton rbExtractAudioAndVideo;
        private System.Windows.Forms.CheckBox cbSplitAudioTracks;
    }
}