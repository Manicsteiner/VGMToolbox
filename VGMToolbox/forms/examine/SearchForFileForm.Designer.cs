namespace VGMToolbox.forms.examine
{
    partial class SearchForFileForm
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
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.lblDragNDrop = new System.Windows.Forms.Label();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.btnOutputDir = new System.Windows.Forms.Button();
            this.tbOutputFolder = new System.Windows.Forms.TextBox();
            this.cbCaseSensitive = new System.Windows.Forms.CheckBox();
            this.cbExtract = new System.Windows.Forms.CheckBox();
            this.lblSearchString = new System.Windows.Forms.Label();
            this.tbSearchString = new System.Windows.Forms.TextBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 523);
            this.pnlLabels.Size = new System.Drawing.Size(771, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(771, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 446);
            this.tbOutput.Size = new System.Drawing.Size(771, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 426);
            this.pnlButtons.Size = new System.Drawing.Size(771, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(711, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(651, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.btnBrowseSource);
            this.grpSource.Controls.Add(this.lblDragNDrop);
            this.grpSource.Controls.Add(this.tbSource);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(771, 64);
            this.grpSource.TabIndex = 5;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Directories to Search";
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(270, 19);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(26, 20);
            this.btnBrowseSource.TabIndex = 2;
            this.btnBrowseSource.Text = "...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // lblDragNDrop
            // 
            this.lblDragNDrop.AutoSize = true;
            this.lblDragNDrop.Location = new System.Drawing.Point(6, 42);
            this.lblDragNDrop.Name = "lblDragNDrop";
            this.lblDragNDrop.Size = new System.Drawing.Size(206, 13);
            this.lblDragNDrop.TabIndex = 1;
            this.lblDragNDrop.Text = "Drag and Drop Directories to Search here.";
            // 
            // tbSource
            // 
            this.tbSource.AllowDrop = true;
            this.tbSource.Location = new System.Drawing.Point(6, 19);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(258, 20);
            this.tbSource.TabIndex = 0;
            this.tbSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSource_DragDrop);
            this.tbSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.lblOutputFolder);
            this.grpOptions.Controls.Add(this.btnOutputDir);
            this.grpOptions.Controls.Add(this.tbOutputFolder);
            this.grpOptions.Controls.Add(this.cbCaseSensitive);
            this.grpOptions.Controls.Add(this.cbExtract);
            this.grpOptions.Controls.Add(this.lblSearchString);
            this.grpOptions.Controls.Add(this.tbSearchString);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.Location = new System.Drawing.Point(0, 87);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(771, 274);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Enabled = false;
            this.lblOutputFolder.Location = new System.Drawing.Point(3, 252);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(359, 13);
            this.lblOutputFolder.TabIndex = 6;
            this.lblOutputFolder.Text = "Output Folder (Leave empty to extract to subfolder named after archive file)";
            // 
            // btnOutputDir
            // 
            this.btnOutputDir.Enabled = false;
            this.btnOutputDir.Location = new System.Drawing.Point(270, 229);
            this.btnOutputDir.Name = "btnOutputDir";
            this.btnOutputDir.Size = new System.Drawing.Size(26, 20);
            this.btnOutputDir.TabIndex = 5;
            this.btnOutputDir.Text = "...";
            this.btnOutputDir.UseVisualStyleBackColor = true;
            // 
            // tbOutputFolder
            // 
            this.tbOutputFolder.Enabled = false;
            this.tbOutputFolder.Location = new System.Drawing.Point(6, 229);
            this.tbOutputFolder.Name = "tbOutputFolder";
            this.tbOutputFolder.Size = new System.Drawing.Size(258, 20);
            this.tbOutputFolder.TabIndex = 4;
            // 
            // cbCaseSensitive
            // 
            this.cbCaseSensitive.AutoSize = true;
            this.cbCaseSensitive.Location = new System.Drawing.Point(273, 38);
            this.cbCaseSensitive.Name = "cbCaseSensitive";
            this.cbCaseSensitive.Size = new System.Drawing.Size(96, 17);
            this.cbCaseSensitive.TabIndex = 3;
            this.cbCaseSensitive.Text = "Case Sensitive";
            this.cbCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // cbExtract
            // 
            this.cbExtract.AutoSize = true;
            this.cbExtract.Location = new System.Drawing.Point(6, 206);
            this.cbExtract.Name = "cbExtract";
            this.cbExtract.Size = new System.Drawing.Size(410, 17);
            this.cbExtract.TabIndex = 2;
            this.cbExtract.Text = "Extract File (if inside an archive, files with the same name will overwrite eacho" +
                "ther)";
            this.cbExtract.UseVisualStyleBackColor = true;
            this.cbExtract.CheckedChanged += new System.EventHandler(this.cbExtract_CheckedChanged);
            // 
            // lblSearchString
            // 
            this.lblSearchString.AutoSize = true;
            this.lblSearchString.Location = new System.Drawing.Point(270, 22);
            this.lblSearchString.Name = "lblSearchString";
            this.lblSearchString.Size = new System.Drawing.Size(146, 13);
            this.lblSearchString.TabIndex = 1;
            this.lblSearchString.Text = "Search String(s) (one per line)";
            // 
            // tbSearchString
            // 
            this.tbSearchString.AcceptsReturn = true;
            this.tbSearchString.Location = new System.Drawing.Point(6, 19);
            this.tbSearchString.Multiline = true;
            this.tbSearchString.Name = "tbSearchString";
            this.tbSearchString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbSearchString.Size = new System.Drawing.Size(258, 154);
            this.tbSearchString.TabIndex = 0;
            // 
            // Examine_SearchForFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 564);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpSource);
            this.Name = "SearchForFileForm";
            this.Text = "SearchForFileForm";
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

        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.Label lblDragNDrop;
        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox cbExtract;
        private System.Windows.Forms.Label lblSearchString;
        private System.Windows.Forms.TextBox tbSearchString;
        private System.Windows.Forms.CheckBox cbCaseSensitive;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.Button btnOutputDir;
        private System.Windows.Forms.TextBox tbOutputFolder;
        private System.Windows.Forms.Label lblOutputFolder;
    }
}