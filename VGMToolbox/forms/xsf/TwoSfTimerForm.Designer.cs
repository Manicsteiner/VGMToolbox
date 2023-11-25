namespace VGMToolbox.forms.xsf
{
    partial class TwoSfTimerForm
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
            this.grpSourcePaths = new System.Windows.Forms.GroupBox();
            this.btnSdatBrowse = new System.Windows.Forms.Button();
            this.btn2sfBrowse = new System.Windows.Forms.Button();
            this.lblPathToSdat = new System.Windows.Forms.Label();
            this.lblPathTo2sfFiles = new System.Windows.Forms.Label();
            this.tbSdatPath = new System.Windows.Forms.TextBox();
            this.tbPathTo2sfs = new System.Windows.Forms.TextBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbOneLoop = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSourcePaths.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 418);
            this.pnlLabels.Size = new System.Drawing.Size(1179, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(1179, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 341);
            this.tbOutput.Size = new System.Drawing.Size(1179, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 321);
            this.pnlButtons.Size = new System.Drawing.Size(1179, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1119, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(1059, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpSourcePaths
            // 
            this.grpSourcePaths.Controls.Add(this.btnSdatBrowse);
            this.grpSourcePaths.Controls.Add(this.btn2sfBrowse);
            this.grpSourcePaths.Controls.Add(this.lblPathToSdat);
            this.grpSourcePaths.Controls.Add(this.lblPathTo2sfFiles);
            this.grpSourcePaths.Controls.Add(this.tbSdatPath);
            this.grpSourcePaths.Controls.Add(this.tbPathTo2sfs);
            this.grpSourcePaths.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSourcePaths.Location = new System.Drawing.Point(0, 23);
            this.grpSourcePaths.Name = "grpSourcePaths";
            this.grpSourcePaths.Size = new System.Drawing.Size(1179, 74);
            this.grpSourcePaths.TabIndex = 5;
            this.grpSourcePaths.TabStop = false;
            this.grpSourcePaths.Text = "Source Paths";
            // 
            // btnSdatBrowse
            // 
            this.btnSdatBrowse.Location = new System.Drawing.Point(381, 48);
            this.btnSdatBrowse.Name = "btnSdatBrowse";
            this.btnSdatBrowse.Size = new System.Drawing.Size(29, 20);
            this.btnSdatBrowse.TabIndex = 5;
            this.btnSdatBrowse.Text = "...";
            this.btnSdatBrowse.UseVisualStyleBackColor = true;
            this.btnSdatBrowse.Click += new System.EventHandler(this.btnSdatBrowse_Click);
            // 
            // btn2sfBrowse
            // 
            this.btn2sfBrowse.Location = new System.Drawing.Point(381, 22);
            this.btn2sfBrowse.Name = "btn2sfBrowse";
            this.btn2sfBrowse.Size = new System.Drawing.Size(29, 20);
            this.btn2sfBrowse.TabIndex = 4;
            this.btn2sfBrowse.Text = "...";
            this.btn2sfBrowse.UseVisualStyleBackColor = true;
            this.btn2sfBrowse.Click += new System.EventHandler(this.btn2sfBrowse_Click);
            // 
            // lblPathToSdat
            // 
            this.lblPathToSdat.AutoSize = true;
            this.lblPathToSdat.Location = new System.Drawing.Point(6, 51);
            this.lblPathToSdat.Name = "lblPathToSdat";
            this.lblPathToSdat.Size = new System.Drawing.Size(73, 13);
            this.lblPathToSdat.TabIndex = 3;
            this.lblPathToSdat.Text = "Path to SDAT";
            // 
            // lblPathTo2sfFiles
            // 
            this.lblPathTo2sfFiles.AutoSize = true;
            this.lblPathTo2sfFiles.Location = new System.Drawing.Point(6, 25);
            this.lblPathTo2sfFiles.Name = "lblPathTo2sfFiles";
            this.lblPathTo2sfFiles.Size = new System.Drawing.Size(84, 13);
            this.lblPathTo2sfFiles.TabIndex = 2;
            this.lblPathTo2sfFiles.Text = "Path to 2SF files";
            // 
            // tbSdatPath
            // 
            this.tbSdatPath.Location = new System.Drawing.Point(127, 48);
            this.tbSdatPath.Name = "tbSdatPath";
            this.tbSdatPath.Size = new System.Drawing.Size(248, 20);
            this.tbSdatPath.TabIndex = 1;
            // 
            // tbPathTo2sfs
            // 
            this.tbPathTo2sfs.Location = new System.Drawing.Point(127, 22);
            this.tbPathTo2sfs.Name = "tbPathTo2sfs";
            this.tbPathTo2sfs.Size = new System.Drawing.Size(248, 20);
            this.tbPathTo2sfs.TabIndex = 0;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbOneLoop);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.Location = new System.Drawing.Point(0, 97);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(1179, 47);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbOneLoop
            // 
            this.cbOneLoop.AutoSize = true;
            this.cbOneLoop.Location = new System.Drawing.Point(9, 19);
            this.cbOneLoop.Name = "cbOneLoop";
            this.cbOneLoop.Size = new System.Drawing.Size(237, 17);
            this.cbOneLoop.TabIndex = 0;
            this.cbOneLoop.Text = "Single Loop (leave unchecked for two loops)";
            this.cbOneLoop.UseVisualStyleBackColor = true;
            // 
            // Xsf_2sfTimerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 459);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpSourcePaths);
            this.Name = "TwoSfTimerForm";
            this.Text = "TwoSfTimerForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSourcePaths, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSourcePaths.ResumeLayout(false);
            this.grpSourcePaths.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSourcePaths;
        private System.Windows.Forms.TextBox tbSdatPath;
        private System.Windows.Forms.TextBox tbPathTo2sfs;
        private System.Windows.Forms.Label lblPathTo2sfFiles;
        private System.Windows.Forms.Label lblPathToSdat;
        private System.Windows.Forms.Button btnSdatBrowse;
        private System.Windows.Forms.Button btn2sfBrowse;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox cbOneLoop;
    }
}