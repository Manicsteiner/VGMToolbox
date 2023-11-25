namespace VGMToolbox.forms.xsf
{
    partial class Psf2DataFinderForm
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
            this.cb00ByteAligned = new System.Windows.Forms.CheckBox();
            this.cbReorderSqFiles = new System.Windows.Forms.CheckBox();
            this.cbUseMinimum = new System.Windows.Forms.CheckBox();
            this.tbMinimumSize = new System.Windows.Forms.TextBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 479);
            this.pnlLabels.Size = new System.Drawing.Size(757, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(757, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 402);
            this.tbOutput.Size = new System.Drawing.Size(757, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 382);
            this.pnlButtons.Size = new System.Drawing.Size(757, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(697, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(637, 0);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.grpOptions);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(757, 359);
            this.grpSource.TabIndex = 5;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Drop Files Here";
            this.grpSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.grpSource_DragDrop);
            this.grpSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cb00ByteAligned);
            this.grpOptions.Controls.Add(this.cbReorderSqFiles);
            this.grpOptions.Controls.Add(this.cbUseMinimum);
            this.grpOptions.Controls.Add(this.tbMinimumSize);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 267);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(751, 89);
            this.grpOptions.TabIndex = 0;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cb00ByteAligned
            // 
            this.cb00ByteAligned.AutoSize = true;
            this.cb00ByteAligned.Location = new System.Drawing.Point(6, 38);
            this.cb00ByteAligned.Name = "cb00ByteAligned";
            this.cb00ByteAligned.Size = new System.Drawing.Size(375, 17);
            this.cb00ByteAligned.TabIndex = 4;
            this.cb00ByteAligned.Text = "BD data always begins at 0x00 offset (faster, and can skip false positives).";
            this.cb00ByteAligned.UseVisualStyleBackColor = true;
            // 
            // cbReorderSqFiles
            // 
            this.cbReorderSqFiles.AutoSize = true;
            this.cbReorderSqFiles.Location = new System.Drawing.Point(6, 61);
            this.cbReorderSqFiles.Name = "cbReorderSqFiles";
            this.cbReorderSqFiles.Size = new System.Drawing.Size(275, 17);
            this.cbReorderSqFiles.TabIndex = 3;
            this.cbReorderSqFiles.Text = "Name SQ files to correspond to maximum HD values.";
            this.cbReorderSqFiles.UseVisualStyleBackColor = true;
            // 
            // cbUseMinimum
            // 
            this.cbUseMinimum.AutoSize = true;
            this.cbUseMinimum.Location = new System.Drawing.Point(6, 15);
            this.cbUseMinimum.Name = "cbUseMinimum";
            this.cbUseMinimum.Size = new System.Drawing.Size(108, 17);
            this.cbUseMinimum.TabIndex = 2;
            this.cbUseMinimum.Text = "Minimum SQ Size";
            this.cbUseMinimum.UseVisualStyleBackColor = true;
            // 
            // tbMinimumSize
            // 
            this.tbMinimumSize.Location = new System.Drawing.Point(120, 13);
            this.tbMinimumSize.Name = "tbMinimumSize";
            this.tbMinimumSize.Size = new System.Drawing.Size(100, 20);
            this.tbMinimumSize.TabIndex = 0;
            // 
            // Psf2DataFinderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 520);
            this.Controls.Add(this.grpSource);
            this.Name = "Psf2DataFinderForm";
            this.Text = "Psf2DataFinderForm";
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
        private System.Windows.Forms.TextBox tbMinimumSize;
        private System.Windows.Forms.CheckBox cbUseMinimum;
        private System.Windows.Forms.CheckBox cbReorderSqFiles;
        private System.Windows.Forms.CheckBox cb00ByteAligned;
    }
}