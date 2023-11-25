namespace VGMToolbox.forms.audit
{
    partial class RebuilderForm
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
            this.grpRebuilder_Directories = new System.Windows.Forms.GroupBox();
            this.lblRebuilder_DestinationDir = new System.Windows.Forms.Label();
            this.btnRebuilder_BrowseDestinationDir = new System.Windows.Forms.Button();
            this.btnRebuilder_BrowseSourceDir = new System.Windows.Forms.Button();
            this.tbRebuilder_DestinationDir = new System.Windows.Forms.TextBox();
            this.lblRebuilder_SourceDir = new System.Windows.Forms.Label();
            this.tbRebuilder_SourceDir = new System.Windows.Forms.TextBox();
            this.grpRebuilder_Datafile = new System.Windows.Forms.GroupBox();
            this.btnRebuilder_BrowseDatafile = new System.Windows.Forms.Button();
            this.tbRebuilder_Datafile = new System.Windows.Forms.TextBox();
            this.grpRebuilder_Options = new System.Windows.Forms.GroupBox();
            this.cbRebuilder_ScanOnly = new System.Windows.Forms.CheckBox();
            this.cbRebuilder_CompressOutput = new System.Windows.Forms.CheckBox();
            this.cbRebuilder_Overwrite = new System.Windows.Forms.CheckBox();
            this.cbRebuilder_RemoveSource = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpRebuilder_Directories.SuspendLayout();
            this.grpRebuilder_Datafile.SuspendLayout();
            this.grpRebuilder_Options.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 486);
            this.pnlLabels.Size = new System.Drawing.Size(868, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(868, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 409);
            this.tbOutput.Size = new System.Drawing.Size(868, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 389);
            this.pnlButtons.Size = new System.Drawing.Size(868, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(808, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(748, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnRebuilder_Rebuild_Click);
            // 
            // grpRebuilder_Directories
            // 
            this.grpRebuilder_Directories.Controls.Add(this.lblRebuilder_DestinationDir);
            this.grpRebuilder_Directories.Controls.Add(this.btnRebuilder_BrowseDestinationDir);
            this.grpRebuilder_Directories.Controls.Add(this.btnRebuilder_BrowseSourceDir);
            this.grpRebuilder_Directories.Controls.Add(this.tbRebuilder_DestinationDir);
            this.grpRebuilder_Directories.Controls.Add(this.lblRebuilder_SourceDir);
            this.grpRebuilder_Directories.Controls.Add(this.tbRebuilder_SourceDir);
            this.grpRebuilder_Directories.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpRebuilder_Directories.Location = new System.Drawing.Point(0, 23);
            this.grpRebuilder_Directories.Name = "grpRebuilder_Directories";
            this.grpRebuilder_Directories.Size = new System.Drawing.Size(868, 95);
            this.grpRebuilder_Directories.TabIndex = 5;
            this.grpRebuilder_Directories.TabStop = false;
            this.grpRebuilder_Directories.Text = "Directories";
            // 
            // lblRebuilder_DestinationDir
            // 
            this.lblRebuilder_DestinationDir.AutoSize = true;
            this.lblRebuilder_DestinationDir.Location = new System.Drawing.Point(3, 54);
            this.lblRebuilder_DestinationDir.Name = "lblRebuilder_DestinationDir";
            this.lblRebuilder_DestinationDir.Size = new System.Drawing.Size(60, 13);
            this.lblRebuilder_DestinationDir.TabIndex = 5;
            this.lblRebuilder_DestinationDir.Text = "Destination";
            // 
            // btnRebuilder_BrowseDestinationDir
            // 
            this.btnRebuilder_BrowseDestinationDir.Location = new System.Drawing.Point(271, 70);
            this.btnRebuilder_BrowseDestinationDir.Name = "btnRebuilder_BrowseDestinationDir";
            this.btnRebuilder_BrowseDestinationDir.Size = new System.Drawing.Size(28, 20);
            this.btnRebuilder_BrowseDestinationDir.TabIndex = 4;
            this.btnRebuilder_BrowseDestinationDir.Text = "...";
            this.btnRebuilder_BrowseDestinationDir.UseVisualStyleBackColor = true;
            this.btnRebuilder_BrowseDestinationDir.Click += new System.EventHandler(this.btnRebuilder_BrowseDestinationDir_Click);
            // 
            // btnRebuilder_BrowseSourceDir
            // 
            this.btnRebuilder_BrowseSourceDir.Location = new System.Drawing.Point(270, 32);
            this.btnRebuilder_BrowseSourceDir.Name = "btnRebuilder_BrowseSourceDir";
            this.btnRebuilder_BrowseSourceDir.Size = new System.Drawing.Size(28, 20);
            this.btnRebuilder_BrowseSourceDir.TabIndex = 3;
            this.btnRebuilder_BrowseSourceDir.Text = "...";
            this.btnRebuilder_BrowseSourceDir.UseVisualStyleBackColor = true;
            this.btnRebuilder_BrowseSourceDir.Click += new System.EventHandler(this.btnRebuilder_BrowseSourceDir_Click);
            // 
            // tbRebuilder_DestinationDir
            // 
            this.tbRebuilder_DestinationDir.Location = new System.Drawing.Point(6, 70);
            this.tbRebuilder_DestinationDir.Name = "tbRebuilder_DestinationDir";
            this.tbRebuilder_DestinationDir.Size = new System.Drawing.Size(259, 20);
            this.tbRebuilder_DestinationDir.TabIndex = 2;
            // 
            // lblRebuilder_SourceDir
            // 
            this.lblRebuilder_SourceDir.AutoSize = true;
            this.lblRebuilder_SourceDir.Location = new System.Drawing.Point(3, 18);
            this.lblRebuilder_SourceDir.Name = "lblRebuilder_SourceDir";
            this.lblRebuilder_SourceDir.Size = new System.Drawing.Size(41, 13);
            this.lblRebuilder_SourceDir.TabIndex = 1;
            this.lblRebuilder_SourceDir.Text = "Source";
            // 
            // tbRebuilder_SourceDir
            // 
            this.tbRebuilder_SourceDir.Location = new System.Drawing.Point(6, 32);
            this.tbRebuilder_SourceDir.Name = "tbRebuilder_SourceDir";
            this.tbRebuilder_SourceDir.Size = new System.Drawing.Size(259, 20);
            this.tbRebuilder_SourceDir.TabIndex = 0;
            // 
            // grpRebuilder_Datafile
            // 
            this.grpRebuilder_Datafile.Controls.Add(this.btnRebuilder_BrowseDatafile);
            this.grpRebuilder_Datafile.Controls.Add(this.tbRebuilder_Datafile);
            this.grpRebuilder_Datafile.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpRebuilder_Datafile.Location = new System.Drawing.Point(0, 118);
            this.grpRebuilder_Datafile.Name = "grpRebuilder_Datafile";
            this.grpRebuilder_Datafile.Size = new System.Drawing.Size(868, 45);
            this.grpRebuilder_Datafile.TabIndex = 6;
            this.grpRebuilder_Datafile.TabStop = false;
            this.grpRebuilder_Datafile.Text = "Datafile";
            // 
            // btnRebuilder_BrowseDatafile
            // 
            this.btnRebuilder_BrowseDatafile.Location = new System.Drawing.Point(270, 19);
            this.btnRebuilder_BrowseDatafile.Name = "btnRebuilder_BrowseDatafile";
            this.btnRebuilder_BrowseDatafile.Size = new System.Drawing.Size(28, 20);
            this.btnRebuilder_BrowseDatafile.TabIndex = 1;
            this.btnRebuilder_BrowseDatafile.Text = "...";
            this.btnRebuilder_BrowseDatafile.UseVisualStyleBackColor = true;
            this.btnRebuilder_BrowseDatafile.Click += new System.EventHandler(this.btnRebuilder_BrowseDatafile_Click);
            // 
            // tbRebuilder_Datafile
            // 
            this.tbRebuilder_Datafile.Location = new System.Drawing.Point(6, 19);
            this.tbRebuilder_Datafile.Name = "tbRebuilder_Datafile";
            this.tbRebuilder_Datafile.Size = new System.Drawing.Size(259, 20);
            this.tbRebuilder_Datafile.TabIndex = 0;
            // 
            // grpRebuilder_Options
            // 
            this.grpRebuilder_Options.Controls.Add(this.cbRebuilder_ScanOnly);
            this.grpRebuilder_Options.Controls.Add(this.cbRebuilder_CompressOutput);
            this.grpRebuilder_Options.Controls.Add(this.cbRebuilder_Overwrite);
            this.grpRebuilder_Options.Controls.Add(this.cbRebuilder_RemoveSource);
            this.grpRebuilder_Options.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpRebuilder_Options.Location = new System.Drawing.Point(0, 163);
            this.grpRebuilder_Options.Name = "grpRebuilder_Options";
            this.grpRebuilder_Options.Size = new System.Drawing.Size(868, 79);
            this.grpRebuilder_Options.TabIndex = 5;
            this.grpRebuilder_Options.TabStop = false;
            this.grpRebuilder_Options.Text = "Options";
            // 
            // cbRebuilder_ScanOnly
            // 
            this.cbRebuilder_ScanOnly.AutoSize = true;
            this.cbRebuilder_ScanOnly.Location = new System.Drawing.Point(6, 56);
            this.cbRebuilder_ScanOnly.Name = "cbRebuilder_ScanOnly";
            this.cbRebuilder_ScanOnly.Size = new System.Drawing.Size(210, 17);
            this.cbRebuilder_ScanOnly.TabIndex = 4;
            this.cbRebuilder_ScanOnly.Text = "Scan Only (Do not move or delete files)";
            this.cbRebuilder_ScanOnly.UseVisualStyleBackColor = true;
            this.cbRebuilder_ScanOnly.CheckedChanged += new System.EventHandler(this.cbRebuilder_ScanOnly_CheckedChanged);
            // 
            // cbRebuilder_CompressOutput
            // 
            this.cbRebuilder_CompressOutput.AutoSize = true;
            this.cbRebuilder_CompressOutput.Location = new System.Drawing.Point(191, 17);
            this.cbRebuilder_CompressOutput.Name = "cbRebuilder_CompressOutput";
            this.cbRebuilder_CompressOutput.Size = new System.Drawing.Size(107, 17);
            this.cbRebuilder_CompressOutput.TabIndex = 3;
            this.cbRebuilder_CompressOutput.Text = "Compress Output";
            this.cbRebuilder_CompressOutput.UseVisualStyleBackColor = true;
            this.cbRebuilder_CompressOutput.CheckedChanged += new System.EventHandler(this.cbRebuilder_CompressOutput_CheckedChanged);
            // 
            // cbRebuilder_Overwrite
            // 
            this.cbRebuilder_Overwrite.AutoSize = true;
            this.cbRebuilder_Overwrite.Location = new System.Drawing.Point(6, 37);
            this.cbRebuilder_Overwrite.Name = "cbRebuilder_Overwrite";
            this.cbRebuilder_Overwrite.Size = new System.Drawing.Size(180, 17);
            this.cbRebuilder_Overwrite.TabIndex = 1;
            this.cbRebuilder_Overwrite.Text = "Update Sets (Overwrite Exisiting)";
            this.cbRebuilder_Overwrite.UseVisualStyleBackColor = true;
            this.cbRebuilder_Overwrite.CheckedChanged += new System.EventHandler(this.cbRebuilder_Overwrite_CheckedChanged);
            // 
            // cbRebuilder_RemoveSource
            // 
            this.cbRebuilder_RemoveSource.AutoSize = true;
            this.cbRebuilder_RemoveSource.Location = new System.Drawing.Point(6, 19);
            this.cbRebuilder_RemoveSource.Name = "cbRebuilder_RemoveSource";
            this.cbRebuilder_RemoveSource.Size = new System.Drawing.Size(139, 17);
            this.cbRebuilder_RemoveSource.TabIndex = 0;
            this.cbRebuilder_RemoveSource.Text = "Remove Rebuilt Source";
            this.cbRebuilder_RemoveSource.UseVisualStyleBackColor = true;
            this.cbRebuilder_RemoveSource.CheckedChanged += new System.EventHandler(this.cbRebuilder_RemoveSource_CheckedChanged);
            // 
            // Auditing_RebuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 527);
            this.Controls.Add(this.grpRebuilder_Options);
            this.Controls.Add(this.grpRebuilder_Datafile);
            this.Controls.Add(this.grpRebuilder_Directories);
            this.Name = "RebuilderForm";
            this.Text = "RebuilderForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpRebuilder_Directories, 0);
            this.Controls.SetChildIndex(this.grpRebuilder_Datafile, 0);
            this.Controls.SetChildIndex(this.grpRebuilder_Options, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpRebuilder_Directories.ResumeLayout(false);
            this.grpRebuilder_Directories.PerformLayout();
            this.grpRebuilder_Datafile.ResumeLayout(false);
            this.grpRebuilder_Datafile.PerformLayout();
            this.grpRebuilder_Options.ResumeLayout(false);
            this.grpRebuilder_Options.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpRebuilder_Directories;
        private System.Windows.Forms.Label lblRebuilder_DestinationDir;
        private System.Windows.Forms.Button btnRebuilder_BrowseDestinationDir;
        private System.Windows.Forms.Button btnRebuilder_BrowseSourceDir;
        private System.Windows.Forms.TextBox tbRebuilder_DestinationDir;
        private System.Windows.Forms.Label lblRebuilder_SourceDir;
        private System.Windows.Forms.TextBox tbRebuilder_SourceDir;
        private System.Windows.Forms.GroupBox grpRebuilder_Datafile;
        private System.Windows.Forms.Button btnRebuilder_BrowseDatafile;
        private System.Windows.Forms.TextBox tbRebuilder_Datafile;
        private System.Windows.Forms.GroupBox grpRebuilder_Options;
        private System.Windows.Forms.CheckBox cbRebuilder_ScanOnly;
        private System.Windows.Forms.CheckBox cbRebuilder_CompressOutput;
        private System.Windows.Forms.CheckBox cbRebuilder_Overwrite;
        private System.Windows.Forms.CheckBox cbRebuilder_RemoveSource;
    }
}