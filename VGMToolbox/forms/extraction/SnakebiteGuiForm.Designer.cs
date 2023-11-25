namespace VGMToolbox.forms.extraction
{
    partial class SnakebiteGuiForm
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
            this.grpFiles = new System.Windows.Forms.GroupBox();
            this.lblDragNDrop = new System.Windows.Forms.Label();
            this.groupOutputMode = new System.Windows.Forms.GroupBox();
            this.rbNameOutput = new System.Windows.Forms.RadioButton();
            this.tbOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.rbAutoName = new System.Windows.Forms.RadioButton();
            this.lblOutputFile = new System.Windows.Forms.Label();
            this.lblFileExtension = new System.Windows.Forms.Label();
            this.tbFileExtension = new System.Windows.Forms.TextBox();
            this.lblSourceFiles = new System.Windows.Forms.Label();
            this.tbSourceFiles = new System.Windows.Forms.TextBox();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.rbEndOfFile = new System.Windows.Forms.RadioButton();
            this.tbLength = new System.Windows.Forms.TextBox();
            this.tbEndAddress = new System.Windows.Forms.TextBox();
            this.rbLength = new System.Windows.Forms.RadioButton();
            this.rbEndAddress = new System.Windows.Forms.RadioButton();
            this.tbStartAddress = new System.Windows.Forms.TextBox();
            this.lblStartAddress = new System.Windows.Forms.Label();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpFiles.SuspendLayout();
            this.groupOutputMode.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 506);
            this.pnlLabels.Size = new System.Drawing.Size(830, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(830, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 429);
            this.tbOutput.Size = new System.Drawing.Size(830, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 409);
            this.pnlButtons.Size = new System.Drawing.Size(830, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(770, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(710, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpFiles
            // 
            this.grpFiles.Controls.Add(this.lblDragNDrop);
            this.grpFiles.Controls.Add(this.groupOutputMode);
            this.grpFiles.Controls.Add(this.lblSourceFiles);
            this.grpFiles.Controls.Add(this.tbSourceFiles);
            this.grpFiles.Controls.Add(this.btnBrowseSource);
            this.grpFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFiles.Location = new System.Drawing.Point(0, 23);
            this.grpFiles.Name = "grpFiles";
            this.grpFiles.Size = new System.Drawing.Size(830, 126);
            this.grpFiles.TabIndex = 5;
            this.grpFiles.TabStop = false;
            this.grpFiles.Text = "Files";
            // 
            // lblDragNDrop
            // 
            this.lblDragNDrop.AutoSize = true;
            this.lblDragNDrop.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDragNDrop.Location = new System.Drawing.Point(79, 41);
            this.lblDragNDrop.Name = "lblDragNDrop";
            this.lblDragNDrop.Size = new System.Drawing.Size(127, 13);
            this.lblDragNDrop.TabIndex = 12;
            this.lblDragNDrop.Text = "Drag and Drop or Browse";
            // 
            // groupOutputMode
            // 
            this.groupOutputMode.Controls.Add(this.rbNameOutput);
            this.groupOutputMode.Controls.Add(this.tbOutputFile);
            this.groupOutputMode.Controls.Add(this.btnBrowseOutput);
            this.groupOutputMode.Controls.Add(this.rbAutoName);
            this.groupOutputMode.Controls.Add(this.lblOutputFile);
            this.groupOutputMode.Controls.Add(this.lblFileExtension);
            this.groupOutputMode.Controls.Add(this.tbFileExtension);
            this.groupOutputMode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupOutputMode.Location = new System.Drawing.Point(3, 60);
            this.groupOutputMode.Name = "groupOutputMode";
            this.groupOutputMode.Size = new System.Drawing.Size(824, 63);
            this.groupOutputMode.TabIndex = 11;
            this.groupOutputMode.TabStop = false;
            this.groupOutputMode.Text = "Output Mode";
            // 
            // rbNameOutput
            // 
            this.rbNameOutput.AutoSize = true;
            this.rbNameOutput.Location = new System.Drawing.Point(6, 19);
            this.rbNameOutput.Name = "rbNameOutput";
            this.rbNameOutput.Size = new System.Drawing.Size(103, 17);
            this.rbNameOutput.TabIndex = 8;
            this.rbNameOutput.TabStop = true;
            this.rbNameOutput.Text = "Single File Mode";
            this.rbNameOutput.UseVisualStyleBackColor = true;
            this.rbNameOutput.CheckedChanged += new System.EventHandler(this.rbFileNameButtons_CheckedChanged);
            // 
            // tbOutputFile
            // 
            this.tbOutputFile.Location = new System.Drawing.Point(224, 16);
            this.tbOutputFile.Name = "tbOutputFile";
            this.tbOutputFile.Size = new System.Drawing.Size(249, 20);
            this.tbOutputFile.TabIndex = 4;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(479, 16);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseOutput.TabIndex = 5;
            this.btnBrowseOutput.Text = "...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // rbAutoName
            // 
            this.rbAutoName.AutoSize = true;
            this.rbAutoName.Location = new System.Drawing.Point(6, 42);
            this.rbAutoName.Name = "rbAutoName";
            this.rbAutoName.Size = new System.Drawing.Size(83, 17);
            this.rbAutoName.TabIndex = 7;
            this.rbAutoName.TabStop = true;
            this.rbAutoName.Text = "Batch Mode";
            this.rbAutoName.UseVisualStyleBackColor = true;
            this.rbAutoName.CheckedChanged += new System.EventHandler(this.rbFileNameButtons_CheckedChanged);
            // 
            // lblOutputFile
            // 
            this.lblOutputFile.AutoSize = true;
            this.lblOutputFile.Location = new System.Drawing.Point(160, 19);
            this.lblOutputFile.Name = "lblOutputFile";
            this.lblOutputFile.Size = new System.Drawing.Size(58, 13);
            this.lblOutputFile.TabIndex = 3;
            this.lblOutputFile.Text = "Output File";
            // 
            // lblFileExtension
            // 
            this.lblFileExtension.AutoSize = true;
            this.lblFileExtension.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFileExtension.Location = new System.Drawing.Point(111, 42);
            this.lblFileExtension.Name = "lblFileExtension";
            this.lblFileExtension.Size = new System.Drawing.Size(107, 13);
            this.lblFileExtension.TabIndex = 10;
            this.lblFileExtension.Text = "Output File Extension";
            // 
            // tbFileExtension
            // 
            this.tbFileExtension.Location = new System.Drawing.Point(224, 39);
            this.tbFileExtension.Name = "tbFileExtension";
            this.tbFileExtension.Size = new System.Drawing.Size(100, 20);
            this.tbFileExtension.TabIndex = 9;
            // 
            // lblSourceFiles
            // 
            this.lblSourceFiles.AutoSize = true;
            this.lblSourceFiles.Location = new System.Drawing.Point(6, 21);
            this.lblSourceFiles.Name = "lblSourceFiles";
            this.lblSourceFiles.Size = new System.Drawing.Size(71, 13);
            this.lblSourceFiles.TabIndex = 0;
            this.lblSourceFiles.Text = "Source File(s)";
            // 
            // tbSourceFiles
            // 
            this.tbSourceFiles.AllowDrop = true;
            this.tbSourceFiles.Location = new System.Drawing.Point(82, 18);
            this.tbSourceFiles.Name = "tbSourceFiles";
            this.tbSourceFiles.Size = new System.Drawing.Size(359, 20);
            this.tbSourceFiles.TabIndex = 1;
            this.tbSourceFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSourceFiles_DragDrop);
            this.tbSourceFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(447, 18);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseSource.TabIndex = 2;
            this.btnBrowseSource.Text = "...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.rbEndOfFile);
            this.grpOptions.Controls.Add(this.tbLength);
            this.grpOptions.Controls.Add(this.tbEndAddress);
            this.grpOptions.Controls.Add(this.rbLength);
            this.grpOptions.Controls.Add(this.rbEndAddress);
            this.grpOptions.Controls.Add(this.tbStartAddress);
            this.grpOptions.Controls.Add(this.lblStartAddress);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.Location = new System.Drawing.Point(0, 149);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(830, 84);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Cut Options";
            // 
            // rbEndOfFile
            // 
            this.rbEndOfFile.AutoSize = true;
            this.rbEndOfFile.Location = new System.Drawing.Point(217, 60);
            this.rbEndOfFile.Name = "rbEndOfFile";
            this.rbEndOfFile.Size = new System.Drawing.Size(105, 17);
            this.rbEndOfFile.TabIndex = 6;
            this.rbEndOfFile.TabStop = true;
            this.rbEndOfFile.Text = "End of File (EOF)";
            this.rbEndOfFile.UseVisualStyleBackColor = true;
            this.rbEndOfFile.CheckedChanged += new System.EventHandler(this.rbEndOfFile_CheckedChanged);
            // 
            // tbLength
            // 
            this.tbLength.Location = new System.Drawing.Point(308, 36);
            this.tbLength.Name = "tbLength";
            this.tbLength.Size = new System.Drawing.Size(128, 20);
            this.tbLength.TabIndex = 5;
            // 
            // tbEndAddress
            // 
            this.tbEndAddress.Location = new System.Drawing.Point(308, 13);
            this.tbEndAddress.Name = "tbEndAddress";
            this.tbEndAddress.Size = new System.Drawing.Size(128, 20);
            this.tbEndAddress.TabIndex = 4;
            // 
            // rbLength
            // 
            this.rbLength.AutoSize = true;
            this.rbLength.Location = new System.Drawing.Point(217, 37);
            this.rbLength.Name = "rbLength";
            this.rbLength.Size = new System.Drawing.Size(58, 17);
            this.rbLength.TabIndex = 3;
            this.rbLength.TabStop = true;
            this.rbLength.Text = "Length";
            this.rbLength.UseVisualStyleBackColor = true;
            this.rbLength.CheckedChanged += new System.EventHandler(this.rbLength_CheckedChanged);
            // 
            // rbEndAddress
            // 
            this.rbEndAddress.AutoSize = true;
            this.rbEndAddress.Location = new System.Drawing.Point(217, 14);
            this.rbEndAddress.Name = "rbEndAddress";
            this.rbEndAddress.Size = new System.Drawing.Size(85, 17);
            this.rbEndAddress.TabIndex = 2;
            this.rbEndAddress.TabStop = true;
            this.rbEndAddress.Text = "End Address";
            this.rbEndAddress.UseVisualStyleBackColor = true;
            this.rbEndAddress.CheckedChanged += new System.EventHandler(this.rbEndAddress_CheckedChanged);
            // 
            // tbStartAddress
            // 
            this.tbStartAddress.Location = new System.Drawing.Point(82, 13);
            this.tbStartAddress.Name = "tbStartAddress";
            this.tbStartAddress.Size = new System.Drawing.Size(129, 20);
            this.tbStartAddress.TabIndex = 1;
            // 
            // lblStartAddress
            // 
            this.lblStartAddress.AutoSize = true;
            this.lblStartAddress.Location = new System.Drawing.Point(6, 16);
            this.lblStartAddress.Name = "lblStartAddress";
            this.lblStartAddress.Size = new System.Drawing.Size(70, 13);
            this.lblStartAddress.TabIndex = 0;
            this.lblStartAddress.Text = "Start Address";
            // 
            // Extract_SnakebiteGuiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 547);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpFiles);
            this.Name = "SnakebiteGuiForm";
            this.Text = "SnakebiteGuiForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpFiles, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpFiles.ResumeLayout(false);
            this.grpFiles.PerformLayout();
            this.groupOutputMode.ResumeLayout(false);
            this.groupOutputMode.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFiles;
        private System.Windows.Forms.TextBox tbSourceFiles;
        private System.Windows.Forms.Label lblSourceFiles;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.TextBox tbOutputFile;
        private System.Windows.Forms.Label lblOutputFile;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.RadioButton rbLength;
        private System.Windows.Forms.RadioButton rbEndAddress;
        private System.Windows.Forms.TextBox tbStartAddress;
        private System.Windows.Forms.Label lblStartAddress;
        private System.Windows.Forms.TextBox tbLength;
        private System.Windows.Forms.TextBox tbEndAddress;
        private System.Windows.Forms.RadioButton rbEndOfFile;
        private System.Windows.Forms.RadioButton rbNameOutput;
        private System.Windows.Forms.RadioButton rbAutoName;
        private System.Windows.Forms.Label lblFileExtension;
        private System.Windows.Forms.TextBox tbFileExtension;
        private System.Windows.Forms.GroupBox groupOutputMode;
        private System.Windows.Forms.Label lblDragNDrop;
    }
}