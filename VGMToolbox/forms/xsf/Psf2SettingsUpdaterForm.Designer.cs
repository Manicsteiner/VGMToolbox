namespace VGMToolbox.forms.xsf
{
    partial class Psf2SettingsUpdaterForm
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
            this.tbBdFile = new System.Windows.Forms.TextBox();
            this.lblBdFile = new System.Windows.Forms.Label();
            this.lblHdFile = new System.Windows.Forms.Label();
            this.tbHdFile = new System.Windows.Forms.TextBox();
            this.tbSqFile = new System.Windows.Forms.TextBox();
            this.lblSqFile = new System.Windows.Forms.Label();
            this.cbRemoveEmpty = new System.Windows.Forms.CheckBox();
            this.lblVolume = new System.Windows.Forms.Label();
            this.tbVolume = new System.Windows.Forms.TextBox();
            this.tbTempo = new System.Windows.Forms.TextBox();
            this.lblTempo = new System.Windows.Forms.Label();
            this.tbTickInterval = new System.Windows.Forms.TextBox();
            this.lblTickInterval = new System.Windows.Forms.Label();
            this.tbDepth = new System.Windows.Forms.TextBox();
            this.lblDepth = new System.Windows.Forms.Label();
            this.lblReverb = new System.Windows.Forms.Label();
            this.tbReverb = new System.Windows.Forms.TextBox();
            this.lblSequenceNumber = new System.Windows.Forms.Label();
            this.tbSequenceNumber = new System.Windows.Forms.TextBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSourceFiles.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 442);
            this.pnlLabels.Size = new System.Drawing.Size(733, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(733, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 365);
            this.tbOutput.Size = new System.Drawing.Size(733, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 345);
            this.pnlButtons.Size = new System.Drawing.Size(733, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(673, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(613, 0);
            // 
            // grpSourceFiles
            // 
            this.grpSourceFiles.Controls.Add(this.grpOptions);
            this.grpSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSourceFiles.Location = new System.Drawing.Point(0, 23);
            this.grpSourceFiles.Name = "grpSourceFiles";
            this.grpSourceFiles.Size = new System.Drawing.Size(733, 322);
            this.grpSourceFiles.TabIndex = 5;
            this.grpSourceFiles.TabStop = false;
            this.grpSourceFiles.Text = "Source Files";
            this.grpSourceFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.grpSourceFiles_DragDrop);
            this.grpSourceFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.grpSourceFiles_DragEnter);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.tbSequenceNumber);
            this.grpOptions.Controls.Add(this.lblSequenceNumber);
            this.grpOptions.Controls.Add(this.tbBdFile);
            this.grpOptions.Controls.Add(this.lblBdFile);
            this.grpOptions.Controls.Add(this.lblHdFile);
            this.grpOptions.Controls.Add(this.tbHdFile);
            this.grpOptions.Controls.Add(this.tbSqFile);
            this.grpOptions.Controls.Add(this.lblSqFile);
            this.grpOptions.Controls.Add(this.cbRemoveEmpty);
            this.grpOptions.Controls.Add(this.lblVolume);
            this.grpOptions.Controls.Add(this.tbVolume);
            this.grpOptions.Controls.Add(this.tbTempo);
            this.grpOptions.Controls.Add(this.lblTempo);
            this.grpOptions.Controls.Add(this.tbTickInterval);
            this.grpOptions.Controls.Add(this.lblTickInterval);
            this.grpOptions.Controls.Add(this.tbDepth);
            this.grpOptions.Controls.Add(this.lblDepth);
            this.grpOptions.Controls.Add(this.lblReverb);
            this.grpOptions.Controls.Add(this.tbReverb);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 125);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(727, 194);
            this.grpOptions.TabIndex = 11;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options (Leave empty for defaults)";
            // 
            // tbBdFile
            // 
            this.tbBdFile.Location = new System.Drawing.Point(48, 71);
            this.tbBdFile.Name = "tbBdFile";
            this.tbBdFile.Size = new System.Drawing.Size(265, 20);
            this.tbBdFile.TabIndex = 16;
            // 
            // lblBdFile
            // 
            this.lblBdFile.AutoSize = true;
            this.lblBdFile.Location = new System.Drawing.Point(1, 74);
            this.lblBdFile.Name = "lblBdFile";
            this.lblBdFile.Size = new System.Drawing.Size(41, 13);
            this.lblBdFile.TabIndex = 15;
            this.lblBdFile.Text = "BD File";
            // 
            // lblHdFile
            // 
            this.lblHdFile.AutoSize = true;
            this.lblHdFile.Location = new System.Drawing.Point(1, 48);
            this.lblHdFile.Name = "lblHdFile";
            this.lblHdFile.Size = new System.Drawing.Size(42, 13);
            this.lblHdFile.TabIndex = 14;
            this.lblHdFile.Text = "HD File";
            // 
            // tbHdFile
            // 
            this.tbHdFile.Location = new System.Drawing.Point(48, 45);
            this.tbHdFile.Name = "tbHdFile";
            this.tbHdFile.Size = new System.Drawing.Size(265, 20);
            this.tbHdFile.TabIndex = 13;
            // 
            // tbSqFile
            // 
            this.tbSqFile.Location = new System.Drawing.Point(48, 19);
            this.tbSqFile.Name = "tbSqFile";
            this.tbSqFile.Size = new System.Drawing.Size(265, 20);
            this.tbSqFile.TabIndex = 12;
            // 
            // lblSqFile
            // 
            this.lblSqFile.AutoSize = true;
            this.lblSqFile.Location = new System.Drawing.Point(1, 22);
            this.lblSqFile.Name = "lblSqFile";
            this.lblSqFile.Size = new System.Drawing.Size(41, 13);
            this.lblSqFile.TabIndex = 11;
            this.lblSqFile.Text = "SQ File";
            // 
            // cbRemoveEmpty
            // 
            this.cbRemoveEmpty.AutoSize = true;
            this.cbRemoveEmpty.Location = new System.Drawing.Point(4, 173);
            this.cbRemoveEmpty.Name = "cbRemoveEmpty";
            this.cbRemoveEmpty.Size = new System.Drawing.Size(474, 17);
            this.cbRemoveEmpty.TabIndex = 10;
            this.cbRemoveEmpty.Text = "Remove Items left Empty (default or existing values will be used if a required va" +
                "lue is left empty).";
            this.cbRemoveEmpty.UseVisualStyleBackColor = true;
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(229, 118);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(42, 13);
            this.lblVolume.TabIndex = 9;
            this.lblVolume.Text = "Volume";
            // 
            // tbVolume
            // 
            this.tbVolume.Location = new System.Drawing.Point(277, 115);
            this.tbVolume.MaxLength = 3;
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Size = new System.Drawing.Size(36, 20);
            this.tbVolume.TabIndex = 8;
            // 
            // tbTempo
            // 
            this.tbTempo.Location = new System.Drawing.Point(45, 144);
            this.tbTempo.MaxLength = 5;
            this.tbTempo.Name = "tbTempo";
            this.tbTempo.Size = new System.Drawing.Size(50, 20);
            this.tbTempo.TabIndex = 7;
            // 
            // lblTempo
            // 
            this.lblTempo.AutoSize = true;
            this.lblTempo.Location = new System.Drawing.Point(2, 147);
            this.lblTempo.Name = "lblTempo";
            this.lblTempo.Size = new System.Drawing.Size(40, 13);
            this.lblTempo.TabIndex = 6;
            this.lblTempo.Text = "Tempo";
            // 
            // tbTickInterval
            // 
            this.tbTickInterval.Location = new System.Drawing.Point(170, 144);
            this.tbTickInterval.MaxLength = 4;
            this.tbTickInterval.Name = "tbTickInterval";
            this.tbTickInterval.Size = new System.Drawing.Size(47, 20);
            this.tbTickInterval.TabIndex = 5;
            // 
            // lblTickInterval
            // 
            this.lblTickInterval.AutoSize = true;
            this.lblTickInterval.Location = new System.Drawing.Point(98, 147);
            this.lblTickInterval.Name = "lblTickInterval";
            this.lblTickInterval.Size = new System.Drawing.Size(66, 13);
            this.lblTickInterval.TabIndex = 4;
            this.lblTickInterval.Text = "Tick Interval";
            // 
            // tbDepth
            // 
            this.tbDepth.Location = new System.Drawing.Point(277, 144);
            this.tbDepth.MaxLength = 5;
            this.tbDepth.Name = "tbDepth";
            this.tbDepth.Size = new System.Drawing.Size(36, 20);
            this.tbDepth.TabIndex = 3;
            // 
            // lblDepth
            // 
            this.lblDepth.AutoSize = true;
            this.lblDepth.Location = new System.Drawing.Point(229, 147);
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size(36, 13);
            this.lblDepth.TabIndex = 2;
            this.lblDepth.Text = "Depth";
            // 
            // lblReverb
            // 
            this.lblReverb.AutoSize = true;
            this.lblReverb.Location = new System.Drawing.Point(98, 121);
            this.lblReverb.Name = "lblReverb";
            this.lblReverb.Size = new System.Drawing.Size(42, 13);
            this.lblReverb.TabIndex = 1;
            this.lblReverb.Text = "Reverb";
            // 
            // tbReverb
            // 
            this.tbReverb.Location = new System.Drawing.Point(170, 118);
            this.tbReverb.MaxLength = 1;
            this.tbReverb.Name = "tbReverb";
            this.tbReverb.Size = new System.Drawing.Size(47, 20);
            this.tbReverb.TabIndex = 0;
            // 
            // lblSequenceNumber
            // 
            this.lblSequenceNumber.AutoSize = true;
            this.lblSequenceNumber.Location = new System.Drawing.Point(2, 121);
            this.lblSequenceNumber.Name = "lblSequenceNumber";
            this.lblSequenceNumber.Size = new System.Drawing.Size(39, 13);
            this.lblSequenceNumber.TabIndex = 17;
            this.lblSequenceNumber.Text = "SEQ #";
            // 
            // tbSequenceNumber
            // 
            this.tbSequenceNumber.Location = new System.Drawing.Point(45, 118);
            this.tbSequenceNumber.Name = "tbSequenceNumber";
            this.tbSequenceNumber.Size = new System.Drawing.Size(50, 20);
            this.tbSequenceNumber.TabIndex = 18;
            // 
            // Psf2SettingsUpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 483);
            this.Controls.Add(this.grpSourceFiles);
            this.Name = "Psf2SettingsUpdaterForm";
            this.Text = "Psf2SettingsUpdaterForm";
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
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.TextBox tbVolume;
        private System.Windows.Forms.TextBox tbTempo;
        private System.Windows.Forms.Label lblTempo;
        private System.Windows.Forms.TextBox tbTickInterval;
        private System.Windows.Forms.Label lblTickInterval;
        private System.Windows.Forms.TextBox tbDepth;
        private System.Windows.Forms.Label lblDepth;
        private System.Windows.Forms.Label lblReverb;
        private System.Windows.Forms.TextBox tbReverb;
        private System.Windows.Forms.CheckBox cbRemoveEmpty;
        private System.Windows.Forms.TextBox tbHdFile;
        private System.Windows.Forms.TextBox tbSqFile;
        private System.Windows.Forms.Label lblSqFile;
        private System.Windows.Forms.Label lblHdFile;
        private System.Windows.Forms.TextBox tbBdFile;
        private System.Windows.Forms.Label lblBdFile;
        private System.Windows.Forms.TextBox tbSequenceNumber;
        private System.Windows.Forms.Label lblSequenceNumber;
    }
}