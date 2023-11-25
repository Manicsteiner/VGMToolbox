namespace VGMToolbox.forms.hoot
{
    partial class HootAuditorForm
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
            this.cbIncludeSubDirs = new System.Windows.Forms.CheckBox();
            this.btnBrowseArchiveFolders = new System.Windows.Forms.Button();
            this.tbArchiveFolders = new System.Windows.Forms.TextBox();
            this.lblArchiveFolders = new System.Windows.Forms.Label();
            this.btnBrowseXmlFolder = new System.Windows.Forms.Button();
            this.lblFolder = new System.Windows.Forms.Label();
            this.tbXmlPath = new System.Windows.Forms.TextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewTools
            // 
            this.treeViewTools.LineColor = System.Drawing.Color.Black;
            this.treeViewTools.Size = new System.Drawing.Size(779, 223);
            // 
            // splitContainer1
            // 
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grpOptions);
            this.splitContainer1.Size = new System.Drawing.Size(779, 403);
            this.splitContainer1.SplitterDistance = 223;
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 523);
            this.pnlLabels.Size = new System.Drawing.Size(779, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(779, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 446);
            this.tbOutput.Size = new System.Drawing.Size(779, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 426);
            this.pnlButtons.Size = new System.Drawing.Size(779, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(719, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(659, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbIncludeSubDirs);
            this.grpOptions.Controls.Add(this.btnBrowseArchiveFolders);
            this.grpOptions.Controls.Add(this.tbArchiveFolders);
            this.grpOptions.Controls.Add(this.lblArchiveFolders);
            this.grpOptions.Controls.Add(this.btnBrowseXmlFolder);
            this.grpOptions.Controls.Add(this.lblFolder);
            this.grpOptions.Controls.Add(this.tbXmlPath);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpOptions.Location = new System.Drawing.Point(0, 0);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(779, 176);
            this.grpOptions.TabIndex = 0;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbIncludeSubDirs
            // 
            this.cbIncludeSubDirs.AutoSize = true;
            this.cbIncludeSubDirs.Checked = true;
            this.cbIncludeSubDirs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeSubDirs.Location = new System.Drawing.Point(92, 119);
            this.cbIncludeSubDirs.Name = "cbIncludeSubDirs";
            this.cbIncludeSubDirs.Size = new System.Drawing.Size(131, 17);
            this.cbIncludeSubDirs.TabIndex = 6;
            this.cbIncludeSubDirs.Text = "Include Subdirectories";
            this.cbIncludeSubDirs.UseVisualStyleBackColor = true;
            // 
            // btnBrowseArchiveFolders
            // 
            this.btnBrowseArchiveFolders.Location = new System.Drawing.Point(419, 48);
            this.btnBrowseArchiveFolders.Name = "btnBrowseArchiveFolders";
            this.btnBrowseArchiveFolders.Size = new System.Drawing.Size(24, 65);
            this.btnBrowseArchiveFolders.TabIndex = 5;
            this.btnBrowseArchiveFolders.Text = "...";
            this.btnBrowseArchiveFolders.UseVisualStyleBackColor = true;
            this.btnBrowseArchiveFolders.Click += new System.EventHandler(this.btnBrowseArchiveFolders_Click);
            // 
            // tbArchiveFolders
            // 
            this.tbArchiveFolders.Location = new System.Drawing.Point(92, 48);
            this.tbArchiveFolders.Multiline = true;
            this.tbArchiveFolders.Name = "tbArchiveFolders";
            this.tbArchiveFolders.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbArchiveFolders.Size = new System.Drawing.Size(321, 65);
            this.tbArchiveFolders.TabIndex = 4;
            // 
            // lblArchiveFolders
            // 
            this.lblArchiveFolders.AutoSize = true;
            this.lblArchiveFolders.Location = new System.Drawing.Point(6, 51);
            this.lblArchiveFolders.Name = "lblArchiveFolders";
            this.lblArchiveFolders.Size = new System.Drawing.Size(80, 26);
            this.lblArchiveFolders.TabIndex = 3;
            this.lblArchiveFolders.Text = "Archive Folders\r\n(one per line)";
            // 
            // btnBrowseXmlFolder
            // 
            this.btnBrowseXmlFolder.Location = new System.Drawing.Point(419, 19);
            this.btnBrowseXmlFolder.Name = "btnBrowseXmlFolder";
            this.btnBrowseXmlFolder.Size = new System.Drawing.Size(24, 20);
            this.btnBrowseXmlFolder.TabIndex = 2;
            this.btnBrowseXmlFolder.Text = "...";
            this.btnBrowseXmlFolder.UseVisualStyleBackColor = true;
            this.btnBrowseXmlFolder.Click += new System.EventHandler(this.btnBrowseXmlFolder_Click);
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(6, 22);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(61, 13);
            this.lblFolder.TabIndex = 1;
            this.lblFolder.Text = "XML Folder";
            // 
            // tbXmlPath
            // 
            this.tbXmlPath.Location = new System.Drawing.Point(92, 19);
            this.tbXmlPath.Name = "tbXmlPath";
            this.tbXmlPath.Size = new System.Drawing.Size(321, 20);
            this.tbXmlPath.TabIndex = 0;
            // 
            // Hoot_AuditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 564);
            this.Name = "HootAuditorForm";
            this.Text = "HootAuditorForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.TextBox tbXmlPath;
        private System.Windows.Forms.Button btnBrowseXmlFolder;
        private System.Windows.Forms.TextBox tbArchiveFolders;
        private System.Windows.Forms.Label lblArchiveFolders;
        private System.Windows.Forms.Button btnBrowseArchiveFolders;
        private System.Windows.Forms.CheckBox cbIncludeSubDirs;
    }
}