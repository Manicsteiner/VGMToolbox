namespace VGMToolbox.forms.other
{
    partial class ExternalListFileRenamerForm
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
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.tbListFilesCodePage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboListFileNameTextFormat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gbListFileFileNames = new System.Windows.Forms.GroupBox();
            this.tbListFileTerminator = new System.Windows.Forms.TextBox();
            this.tbListFileStaticSize = new System.Windows.Forms.TextBox();
            this.rbListFileTerminator = new System.Windows.Forms.RadioButton();
            this.rbListFileStaticSize = new System.Windows.Forms.RadioButton();
            this.tbListFileOffsetToFileList = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowseListFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbListFile = new System.Windows.Forms.TextBox();
            this.gbFilesToRename = new System.Windows.Forms.GroupBox();
            this.cbKeepExtension = new System.Windows.Forms.CheckBox();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSourceFileMask = new System.Windows.Forms.TextBox();
            this.tbSourceFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.gbListFileFileNames.SuspendLayout();
            this.gbFilesToRename.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 443);
            this.pnlLabels.Size = new System.Drawing.Size(858, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(858, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 366);
            this.tbOutput.Size = new System.Drawing.Size(858, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 346);
            this.pnlButtons.Size = new System.Drawing.Size(858, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(798, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(738, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.tbListFilesCodePage);
            this.gbOptions.Controls.Add(this.label6);
            this.gbOptions.Controls.Add(this.comboListFileNameTextFormat);
            this.gbOptions.Controls.Add(this.label5);
            this.gbOptions.Controls.Add(this.gbListFileFileNames);
            this.gbOptions.Controls.Add(this.tbListFileOffsetToFileList);
            this.gbOptions.Controls.Add(this.label2);
            this.gbOptions.Controls.Add(this.btnBrowseListFile);
            this.gbOptions.Controls.Add(this.label1);
            this.gbOptions.Controls.Add(this.tbListFile);
            this.gbOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbOptions.Location = new System.Drawing.Point(0, 86);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(858, 120);
            this.gbOptions.TabIndex = 5;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "List File Details";
            // 
            // tbListFilesCodePage
            // 
            this.tbListFilesCodePage.Location = new System.Drawing.Point(489, 39);
            this.tbListFilesCodePage.Name = "tbListFilesCodePage";
            this.tbListFilesCodePage.Size = new System.Drawing.Size(50, 20);
            this.tbListFilesCodePage.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(423, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Code Page";
            // 
            // comboListFileNameTextFormat
            // 
            this.comboListFileNameTextFormat.FormattingEnabled = true;
            this.comboListFileNameTextFormat.Location = new System.Drawing.Point(344, 39);
            this.comboListFileNameTextFormat.Name = "comboListFileNameTextFormat";
            this.comboListFileNameTextFormat.Size = new System.Drawing.Size(73, 21);
            this.comboListFileNameTextFormat.TabIndex = 7;
            this.comboListFileNameTextFormat.SelectedIndexChanged += new System.EventHandler(this.comboListFileNameTextFormat_SelectedIndexChanged);
            this.comboListFileNameTextFormat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboListFileNameTextFormat_KeyDown);
            this.comboListFileNameTextFormat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboListFileNameTextFormat_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(225, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "File Name Text Format";
            // 
            // gbListFileFileNames
            // 
            this.gbListFileFileNames.Controls.Add(this.tbListFileTerminator);
            this.gbListFileFileNames.Controls.Add(this.tbListFileStaticSize);
            this.gbListFileFileNames.Controls.Add(this.rbListFileTerminator);
            this.gbListFileFileNames.Controls.Add(this.rbListFileStaticSize);
            this.gbListFileFileNames.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbListFileFileNames.Location = new System.Drawing.Point(3, 63);
            this.gbListFileFileNames.Name = "gbListFileFileNames";
            this.gbListFileFileNames.Size = new System.Drawing.Size(852, 54);
            this.gbListFileFileNames.TabIndex = 5;
            this.gbListFileFileNames.TabStop = false;
            this.gbListFileFileNames.Text = "File Name Size";
            // 
            // tbListFileTerminator
            // 
            this.tbListFileTerminator.Location = new System.Drawing.Point(195, 18);
            this.tbListFileTerminator.Name = "tbListFileTerminator";
            this.tbListFileTerminator.Size = new System.Drawing.Size(46, 20);
            this.tbListFileTerminator.TabIndex = 3;
            // 
            // tbListFileStaticSize
            // 
            this.tbListFileStaticSize.Location = new System.Drawing.Point(410, 18);
            this.tbListFileStaticSize.Name = "tbListFileStaticSize";
            this.tbListFileStaticSize.Size = new System.Drawing.Size(49, 20);
            this.tbListFileStaticSize.TabIndex = 2;
            // 
            // rbListFileTerminator
            // 
            this.rbListFileTerminator.AutoSize = true;
            this.rbListFileTerminator.Location = new System.Drawing.Point(6, 18);
            this.rbListFileTerminator.Name = "rbListFileTerminator";
            this.rbListFileTerminator.Size = new System.Drawing.Size(183, 30);
            this.rbListFileTerminator.TabIndex = 1;
            this.rbListFileTerminator.TabStop = true;
            this.rbListFileTerminator.Text = "File Name Has Terminator \r\n(hex value, no \'0x\' prefix needed.)";
            this.rbListFileTerminator.UseVisualStyleBackColor = true;
            this.rbListFileTerminator.CheckedChanged += new System.EventHandler(this.rbListFileTerminator_CheckedChanged);
            // 
            // rbListFileStaticSize
            // 
            this.rbListFileStaticSize.AutoSize = true;
            this.rbListFileStaticSize.Location = new System.Drawing.Point(257, 18);
            this.rbListFileStaticSize.Name = "rbListFileStaticSize";
            this.rbListFileStaticSize.Size = new System.Drawing.Size(147, 17);
            this.rbListFileStaticSize.TabIndex = 0;
            this.rbListFileStaticSize.TabStop = true;
            this.rbListFileStaticSize.Text = "File Name Has Static Size";
            this.rbListFileStaticSize.UseVisualStyleBackColor = true;
            this.rbListFileStaticSize.CheckedChanged += new System.EventHandler(this.rbListFileStaticSize_CheckedChanged);
            // 
            // tbListFileOffsetToFileList
            // 
            this.tbListFileOffsetToFileList.Location = new System.Drawing.Point(146, 40);
            this.tbListFileOffsetToFileList.Name = "tbListFileOffsetToFileList";
            this.tbListFileOffsetToFileList.Size = new System.Drawing.Size(73, 20);
            this.tbListFileOffsetToFileList.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Offset to File Name List";
            // 
            // btnBrowseListFile
            // 
            this.btnBrowseListFile.Location = new System.Drawing.Point(401, 14);
            this.btnBrowseListFile.Name = "btnBrowseListFile";
            this.btnBrowseListFile.Size = new System.Drawing.Size(24, 20);
            this.btnBrowseListFile.TabIndex = 2;
            this.btnBrowseListFile.Text = "...";
            this.btnBrowseListFile.UseVisualStyleBackColor = true;
            this.btnBrowseListFile.Click += new System.EventHandler(this.btnBrowseListFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "List File Location";
            // 
            // tbListFile
            // 
            this.tbListFile.AllowDrop = true;
            this.tbListFile.Location = new System.Drawing.Point(116, 14);
            this.tbListFile.Name = "tbListFile";
            this.tbListFile.Size = new System.Drawing.Size(279, 20);
            this.tbListFile.TabIndex = 0;
            this.tbListFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbListFile_DragDrop);
            this.tbListFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbListFile_DragEnter);
            // 
            // gbFilesToRename
            // 
            this.gbFilesToRename.Controls.Add(this.cbKeepExtension);
            this.gbFilesToRename.Controls.Add(this.btnBrowseSource);
            this.gbFilesToRename.Controls.Add(this.label4);
            this.gbFilesToRename.Controls.Add(this.tbSourceFileMask);
            this.gbFilesToRename.Controls.Add(this.tbSourceFolder);
            this.gbFilesToRename.Controls.Add(this.label3);
            this.gbFilesToRename.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFilesToRename.Location = new System.Drawing.Point(0, 23);
            this.gbFilesToRename.Name = "gbFilesToRename";
            this.gbFilesToRename.Size = new System.Drawing.Size(858, 63);
            this.gbFilesToRename.TabIndex = 6;
            this.gbFilesToRename.TabStop = false;
            this.gbFilesToRename.Text = "Files to be Renamed";
            // 
            // cbKeepExtension
            // 
            this.cbKeepExtension.AutoSize = true;
            this.cbKeepExtension.Location = new System.Drawing.Point(260, 41);
            this.cbKeepExtension.Name = "cbKeepExtension";
            this.cbKeepExtension.Size = new System.Drawing.Size(157, 17);
            this.cbKeepExtension.TabIndex = 5;
            this.cbKeepExtension.Text = "Keep Original File Extension";
            this.cbKeepExtension.UseVisualStyleBackColor = true;
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(401, 13);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(24, 20);
            this.btnBrowseSource.TabIndex = 4;
            this.btnBrowseSource.Text = "...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "File Mask";
            // 
            // tbSourceFileMask
            // 
            this.tbSourceFileMask.Location = new System.Drawing.Point(116, 39);
            this.tbSourceFileMask.Name = "tbSourceFileMask";
            this.tbSourceFileMask.Size = new System.Drawing.Size(128, 20);
            this.tbSourceFileMask.TabIndex = 2;
            // 
            // tbSourceFolder
            // 
            this.tbSourceFolder.AllowDrop = true;
            this.tbSourceFolder.Location = new System.Drawing.Point(116, 13);
            this.tbSourceFolder.Name = "tbSourceFolder";
            this.tbSourceFolder.Size = new System.Drawing.Size(279, 20);
            this.tbSourceFolder.TabIndex = 1;
            this.tbSourceFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSourceFolder_DragDrop);
            this.tbSourceFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbSourceFolder_DragEnter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Source Folder";
            // 
            // ExternalListFileRenamerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 484);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.gbFilesToRename);
            this.Name = "ExternalListFileRenamerForm";
            this.Text = "ExternalListFileRenamerForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.gbFilesToRename, 0);
            this.Controls.SetChildIndex(this.gbOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.gbListFileFileNames.ResumeLayout(false);
            this.gbListFileFileNames.PerformLayout();
            this.gbFilesToRename.ResumeLayout(false);
            this.gbFilesToRename.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.Button btnBrowseListFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbListFile;
        private System.Windows.Forms.GroupBox gbListFileFileNames;
        private System.Windows.Forms.TextBox tbListFileOffsetToFileList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbListFileTerminator;
        private System.Windows.Forms.TextBox tbListFileStaticSize;
        private System.Windows.Forms.RadioButton rbListFileTerminator;
        private System.Windows.Forms.RadioButton rbListFileStaticSize;
        private System.Windows.Forms.GroupBox gbFilesToRename;
        private System.Windows.Forms.TextBox tbSourceFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSourceFileMask;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.CheckBox cbKeepExtension;
        private System.Windows.Forms.ComboBox comboListFileNameTextFormat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbListFilesCodePage;
        private System.Windows.Forms.Label label6;
    }
}