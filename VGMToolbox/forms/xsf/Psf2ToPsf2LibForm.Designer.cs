namespace VGMToolbox.forms.xsf
{
    partial class Psf2ToPsf2LibForm
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.btnSourceDirBrowse = new System.Windows.Forms.Button();
            this.tbSourceDirectory = new System.Windows.Forms.TextBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.tbFilePrefix = new System.Windows.Forms.TextBox();
            this.lblOutputFilePrefix = new System.Windows.Forms.Label();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 415);
            this.pnlLabels.Size = new System.Drawing.Size(846, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(846, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 338);
            this.tbOutput.Size = new System.Drawing.Size(846, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.checkBox1);
            this.pnlButtons.Location = new System.Drawing.Point(0, 318);
            this.pnlButtons.Size = new System.Drawing.Size(846, 20);
            this.pnlButtons.Controls.SetChildIndex(this.checkBox1, 0);
            this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlButtons.Controls.SetChildIndex(this.btnDoTask, 0);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(786, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(726, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(710, 6);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.btnSourceDirBrowse);
            this.grpSource.Controls.Add(this.tbSourceDirectory);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(846, 51);
            this.grpSource.TabIndex = 5;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source Directory";
            // 
            // btnSourceDirBrowse
            // 
            this.btnSourceDirBrowse.Location = new System.Drawing.Point(245, 19);
            this.btnSourceDirBrowse.Name = "btnSourceDirBrowse";
            this.btnSourceDirBrowse.Size = new System.Drawing.Size(28, 20);
            this.btnSourceDirBrowse.TabIndex = 1;
            this.btnSourceDirBrowse.Text = "...";
            this.btnSourceDirBrowse.UseVisualStyleBackColor = true;
            this.btnSourceDirBrowse.Click += new System.EventHandler(this.btnSourceDirBrowse_Click);
            // 
            // tbSourceDirectory
            // 
            this.tbSourceDirectory.Location = new System.Drawing.Point(6, 19);
            this.tbSourceDirectory.Name = "tbSourceDirectory";
            this.tbSourceDirectory.Size = new System.Drawing.Size(233, 20);
            this.tbSourceDirectory.TabIndex = 0;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.tbFilePrefix);
            this.grpOptions.Controls.Add(this.lblOutputFilePrefix);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.Location = new System.Drawing.Point(0, 74);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(846, 54);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // tbFilePrefix
            // 
            this.tbFilePrefix.Location = new System.Drawing.Point(96, 22);
            this.tbFilePrefix.Name = "tbFilePrefix";
            this.tbFilePrefix.Size = new System.Drawing.Size(143, 20);
            this.tbFilePrefix.TabIndex = 1;
            // 
            // lblOutputFilePrefix
            // 
            this.lblOutputFilePrefix.AutoSize = true;
            this.lblOutputFilePrefix.Location = new System.Drawing.Point(6, 25);
            this.lblOutputFilePrefix.Name = "lblOutputFilePrefix";
            this.lblOutputFilePrefix.Size = new System.Drawing.Size(84, 13);
            this.lblOutputFilePrefix.TabIndex = 0;
            this.lblOutputFilePrefix.Text = "Ouput File Prefix";
            // 
            // Xsf_Psf2ToPsf2LibForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 456);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpSource);
            this.Name = "Psf2ToPsf2LibForm";
            this.Text = "Psf2ToPsf2Lib";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSource, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.TextBox tbSourceDirectory;
        private System.Windows.Forms.Button btnSourceDirBrowse;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.TextBox tbFilePrefix;
        private System.Windows.Forms.Label lblOutputFilePrefix;
    }
}