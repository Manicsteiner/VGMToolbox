namespace VGMToolbox.forms.audit
{
    partial class DatafileCheckerForm
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
            this.gbSourceDestination = new System.Windows.Forms.GroupBox();
            this.lblReportDestination = new System.Windows.Forms.Label();
            this.lblSourceDataFile = new System.Windows.Forms.Label();
            this.tbDatafileChecker_BrowseDestination = new System.Windows.Forms.Button();
            this.tbDatafileChecker_BrowseSource = new System.Windows.Forms.Button();
            this.tbDatafileChecker_OutputPath = new System.Windows.Forms.TextBox();
            this.tbDatafileChecker_SourceFile = new System.Windows.Forms.TextBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.gbSourceDestination.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 487);
            this.pnlLabels.Size = new System.Drawing.Size(868, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(868, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 410);
            this.tbOutput.Size = new System.Drawing.Size(868, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 390);
            this.pnlButtons.Size = new System.Drawing.Size(868, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(808, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(748, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDatafileChecker_Check_Click);
            // 
            // gbSourceDestination
            // 
            this.gbSourceDestination.Controls.Add(this.lblReportDestination);
            this.gbSourceDestination.Controls.Add(this.lblSourceDataFile);
            this.gbSourceDestination.Controls.Add(this.tbDatafileChecker_BrowseDestination);
            this.gbSourceDestination.Controls.Add(this.tbDatafileChecker_BrowseSource);
            this.gbSourceDestination.Controls.Add(this.tbDatafileChecker_OutputPath);
            this.gbSourceDestination.Controls.Add(this.tbDatafileChecker_SourceFile);
            this.gbSourceDestination.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSourceDestination.Location = new System.Drawing.Point(0, 23);
            this.gbSourceDestination.Name = "gbSourceDestination";
            this.gbSourceDestination.Size = new System.Drawing.Size(868, 97);
            this.gbSourceDestination.TabIndex = 5;
            this.gbSourceDestination.TabStop = false;
            this.gbSourceDestination.Text = "Source and Destination";
            // 
            // lblReportDestination
            // 
            this.lblReportDestination.AutoSize = true;
            this.lblReportDestination.Location = new System.Drawing.Point(3, 55);
            this.lblReportDestination.Name = "lblReportDestination";
            this.lblReportDestination.Size = new System.Drawing.Size(138, 13);
            this.lblReportDestination.TabIndex = 5;
            this.lblReportDestination.Text = "Report(s) Destination Folder";
            // 
            // lblSourceDataFile
            // 
            this.lblSourceDataFile.AutoSize = true;
            this.lblSourceDataFile.Location = new System.Drawing.Point(3, 18);
            this.lblSourceDataFile.Name = "lblSourceDataFile";
            this.lblSourceDataFile.Size = new System.Drawing.Size(80, 13);
            this.lblSourceDataFile.TabIndex = 4;
            this.lblSourceDataFile.Text = "Source Datafile";
            // 
            // tbDatafileChecker_BrowseDestination
            // 
            this.tbDatafileChecker_BrowseDestination.Location = new System.Drawing.Point(271, 71);
            this.tbDatafileChecker_BrowseDestination.Name = "tbDatafileChecker_BrowseDestination";
            this.tbDatafileChecker_BrowseDestination.Size = new System.Drawing.Size(28, 20);
            this.tbDatafileChecker_BrowseDestination.TabIndex = 3;
            this.tbDatafileChecker_BrowseDestination.Text = "...";
            this.tbDatafileChecker_BrowseDestination.UseVisualStyleBackColor = true;
            this.tbDatafileChecker_BrowseDestination.Click += new System.EventHandler(this.tbDatafileChecker_BrowseDestination_Click);
            // 
            // tbDatafileChecker_BrowseSource
            // 
            this.tbDatafileChecker_BrowseSource.Location = new System.Drawing.Point(270, 32);
            this.tbDatafileChecker_BrowseSource.Name = "tbDatafileChecker_BrowseSource";
            this.tbDatafileChecker_BrowseSource.Size = new System.Drawing.Size(28, 20);
            this.tbDatafileChecker_BrowseSource.TabIndex = 1;
            this.tbDatafileChecker_BrowseSource.Text = "...";
            this.tbDatafileChecker_BrowseSource.UseVisualStyleBackColor = true;
            this.tbDatafileChecker_BrowseSource.Click += new System.EventHandler(this.tbDatafileChecker_BrowseSource_Click);
            // 
            // tbDatafileChecker_OutputPath
            // 
            this.tbDatafileChecker_OutputPath.Location = new System.Drawing.Point(6, 71);
            this.tbDatafileChecker_OutputPath.Name = "tbDatafileChecker_OutputPath";
            this.tbDatafileChecker_OutputPath.Size = new System.Drawing.Size(259, 20);
            this.tbDatafileChecker_OutputPath.TabIndex = 2;
            // 
            // tbDatafileChecker_SourceFile
            // 
            this.tbDatafileChecker_SourceFile.Location = new System.Drawing.Point(6, 32);
            this.tbDatafileChecker_SourceFile.Name = "tbDatafileChecker_SourceFile";
            this.tbDatafileChecker_SourceFile.Size = new System.Drawing.Size(259, 20);
            this.tbDatafileChecker_SourceFile.TabIndex = 0;
            // 
            // Auditing_DatafileCheckerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 528);
            this.Controls.Add(this.gbSourceDestination);
            this.Name = "DatafileCheckerForm";
            this.Text = "DatafileCheckerForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.gbSourceDestination, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.gbSourceDestination.ResumeLayout(false);
            this.gbSourceDestination.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSourceDestination;
        private System.Windows.Forms.Label lblReportDestination;
        private System.Windows.Forms.Label lblSourceDataFile;
        private System.Windows.Forms.Button tbDatafileChecker_BrowseDestination;
        private System.Windows.Forms.Button tbDatafileChecker_BrowseSource;
        private System.Windows.Forms.TextBox tbDatafileChecker_OutputPath;
        private System.Windows.Forms.TextBox tbDatafileChecker_SourceFile;
    }
}