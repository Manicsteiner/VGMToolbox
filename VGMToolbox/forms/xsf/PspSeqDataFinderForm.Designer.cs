namespace VGMToolbox.forms.xsf
{
    partial class PspSeqDataFinderForm
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
            this.cbReorderMidFiles = new System.Windows.Forms.CheckBox();
            this.cb00ByteAligned = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 311);
            this.pnlLabels.Size = new System.Drawing.Size(801, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(801, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 234);
            this.tbOutput.Size = new System.Drawing.Size(801, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 214);
            this.pnlButtons.Size = new System.Drawing.Size(801, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(741, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(681, 0);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.grpOptions);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(801, 191);
            this.grpSource.TabIndex = 5;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Drop Files Here";
            this.grpSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.grpSource_DragDrop);
            this.grpSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbReorderMidFiles);
            this.grpOptions.Controls.Add(this.cb00ByteAligned);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 121);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(795, 67);
            this.grpOptions.TabIndex = 0;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbReorderMidFiles
            // 
            this.cbReorderMidFiles.AutoSize = true;
            this.cbReorderMidFiles.Location = new System.Drawing.Point(6, 42);
            this.cbReorderMidFiles.Name = "cbReorderMidFiles";
            this.cbReorderMidFiles.Size = new System.Drawing.Size(263, 17);
            this.cbReorderMidFiles.TabIndex = 1;
            this.cbReorderMidFiles.Text = "Name MID to correspond to maximum PHD values";
            this.cbReorderMidFiles.UseVisualStyleBackColor = true;
            // 
            // cb00ByteAligned
            // 
            this.cb00ByteAligned.AutoSize = true;
            this.cb00ByteAligned.Location = new System.Drawing.Point(6, 19);
            this.cb00ByteAligned.Name = "cb00ByteAligned";
            this.cb00ByteAligned.Size = new System.Drawing.Size(382, 17);
            this.cb00ByteAligned.TabIndex = 0;
            this.cb00ByteAligned.Text = "PBD data always begins at 0x00 offset (faster, and can skip flase positives).";
            this.cb00ByteAligned.UseVisualStyleBackColor = true;
            // 
            // PspSeqDataFinderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 352);
            this.Controls.Add(this.grpSource);
            this.Name = "PspSeqDataFinderForm";
            this.Text = "PspSeqDataFinder";
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
        private System.Windows.Forms.CheckBox cb00ByteAligned;
        private System.Windows.Forms.CheckBox cbReorderMidFiles;
    }
}