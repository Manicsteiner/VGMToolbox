namespace VGMToolbox.forms.audit
{
    partial class DatafileCreatorForm
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
            this.gbHeader = new System.Windows.Forms.GroupBox();
            this.lblHeaderUrl = new System.Windows.Forms.Label();
            this.tbDatCreator_Url = new System.Windows.Forms.TextBox();
            this.lblHeaderHomepage = new System.Windows.Forms.Label();
            this.tbDatCreator_Homepage = new System.Windows.Forms.TextBox();
            this.lblHeaderEmail = new System.Windows.Forms.Label();
            this.tbDatCreator_Email = new System.Windows.Forms.TextBox();
            this.lblHeaderDate = new System.Windows.Forms.Label();
            this.tbDatCreator_Date = new System.Windows.Forms.TextBox();
            this.lblHeaderCategory = new System.Windows.Forms.Label();
            this.tbDatCreator_Category = new System.Windows.Forms.TextBox();
            this.lblHeaderComment = new System.Windows.Forms.Label();
            this.tbDatCreator_Comment = new System.Windows.Forms.TextBox();
            this.lblHeaderAuthor = new System.Windows.Forms.Label();
            this.tbDatCreator_Author = new System.Windows.Forms.TextBox();
            this.lblHeaderVersion = new System.Windows.Forms.Label();
            this.tbDatCreator_Version = new System.Windows.Forms.TextBox();
            this.lblHeaderDescription = new System.Windows.Forms.Label();
            this.tbDatCreator_Description = new System.Windows.Forms.TextBox();
            this.lblHeaderName = new System.Windows.Forms.Label();
            this.tbDatCreator_Name = new System.Windows.Forms.TextBox();
            this.gbSourceDestPaths = new System.Windows.Forms.GroupBox();
            this.btnDatCreator_BrowseDestination = new System.Windows.Forms.Button();
            this.btnDatCreator_BrowseSource = new System.Windows.Forms.Button();
            this.lblDatCreator_DestinationFolder = new System.Windows.Forms.Label();
            this.tbDatCreator_OutputDat = new System.Windows.Forms.TextBox();
            this.lblDatCreator_SourceFolder = new System.Windows.Forms.Label();
            this.tbDatCreator_SourceFolder = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbUseNormalChecksum = new System.Windows.Forms.CheckBox();
            this.cbMd5Sha1 = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.gbHeader.SuspendLayout();
            this.gbSourceDestPaths.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 501);
            this.pnlLabels.Size = new System.Drawing.Size(645, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(645, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 424);
            this.tbOutput.Size = new System.Drawing.Size(645, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 404);
            this.pnlButtons.Size = new System.Drawing.Size(645, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(585, 0);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(525, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDatCreator_BuildDat_Click);
            // 
            // gbHeader
            // 
            this.gbHeader.Controls.Add(this.lblHeaderUrl);
            this.gbHeader.Controls.Add(this.tbDatCreator_Url);
            this.gbHeader.Controls.Add(this.lblHeaderHomepage);
            this.gbHeader.Controls.Add(this.tbDatCreator_Homepage);
            this.gbHeader.Controls.Add(this.lblHeaderEmail);
            this.gbHeader.Controls.Add(this.tbDatCreator_Email);
            this.gbHeader.Controls.Add(this.lblHeaderDate);
            this.gbHeader.Controls.Add(this.tbDatCreator_Date);
            this.gbHeader.Controls.Add(this.lblHeaderCategory);
            this.gbHeader.Controls.Add(this.tbDatCreator_Category);
            this.gbHeader.Controls.Add(this.lblHeaderComment);
            this.gbHeader.Controls.Add(this.tbDatCreator_Comment);
            this.gbHeader.Controls.Add(this.lblHeaderAuthor);
            this.gbHeader.Controls.Add(this.tbDatCreator_Author);
            this.gbHeader.Controls.Add(this.lblHeaderVersion);
            this.gbHeader.Controls.Add(this.tbDatCreator_Version);
            this.gbHeader.Controls.Add(this.lblHeaderDescription);
            this.gbHeader.Controls.Add(this.tbDatCreator_Description);
            this.gbHeader.Controls.Add(this.lblHeaderName);
            this.gbHeader.Controls.Add(this.tbDatCreator_Name);
            this.gbHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbHeader.Location = new System.Drawing.Point(0, 23);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.Size = new System.Drawing.Size(645, 140);
            this.gbHeader.TabIndex = 5;
            this.gbHeader.TabStop = false;
            this.gbHeader.Text = "Header Information";
            // 
            // lblHeaderUrl
            // 
            this.lblHeaderUrl.AutoSize = true;
            this.lblHeaderUrl.Location = new System.Drawing.Point(233, 114);
            this.lblHeaderUrl.Name = "lblHeaderUrl";
            this.lblHeaderUrl.Size = new System.Drawing.Size(29, 13);
            this.lblHeaderUrl.TabIndex = 39;
            this.lblHeaderUrl.Text = "URL";
            // 
            // tbDatCreator_Url
            // 
            this.tbDatCreator_Url.Location = new System.Drawing.Point(295, 110);
            this.tbDatCreator_Url.Name = "tbDatCreator_Url";
            this.tbDatCreator_Url.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Url.TabIndex = 38;
            // 
            // lblHeaderHomepage
            // 
            this.lblHeaderHomepage.AutoSize = true;
            this.lblHeaderHomepage.Location = new System.Drawing.Point(233, 90);
            this.lblHeaderHomepage.Name = "lblHeaderHomepage";
            this.lblHeaderHomepage.Size = new System.Drawing.Size(59, 13);
            this.lblHeaderHomepage.TabIndex = 37;
            this.lblHeaderHomepage.Text = "Homepage";
            // 
            // tbDatCreator_Homepage
            // 
            this.tbDatCreator_Homepage.Location = new System.Drawing.Point(295, 86);
            this.tbDatCreator_Homepage.Name = "tbDatCreator_Homepage";
            this.tbDatCreator_Homepage.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Homepage.TabIndex = 36;
            // 
            // lblHeaderEmail
            // 
            this.lblHeaderEmail.AutoSize = true;
            this.lblHeaderEmail.Location = new System.Drawing.Point(233, 66);
            this.lblHeaderEmail.Name = "lblHeaderEmail";
            this.lblHeaderEmail.Size = new System.Drawing.Size(33, 13);
            this.lblHeaderEmail.TabIndex = 35;
            this.lblHeaderEmail.Text = "EMail";
            // 
            // tbDatCreator_Email
            // 
            this.tbDatCreator_Email.Location = new System.Drawing.Point(295, 62);
            this.tbDatCreator_Email.Name = "tbDatCreator_Email";
            this.tbDatCreator_Email.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Email.TabIndex = 34;
            // 
            // lblHeaderDate
            // 
            this.lblHeaderDate.AutoSize = true;
            this.lblHeaderDate.Location = new System.Drawing.Point(233, 42);
            this.lblHeaderDate.Name = "lblHeaderDate";
            this.lblHeaderDate.Size = new System.Drawing.Size(30, 13);
            this.lblHeaderDate.TabIndex = 33;
            this.lblHeaderDate.Text = "Date";
            // 
            // tbDatCreator_Date
            // 
            this.tbDatCreator_Date.Location = new System.Drawing.Point(295, 38);
            this.tbDatCreator_Date.Name = "tbDatCreator_Date";
            this.tbDatCreator_Date.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Date.TabIndex = 32;
            // 
            // lblHeaderCategory
            // 
            this.lblHeaderCategory.AutoSize = true;
            this.lblHeaderCategory.Location = new System.Drawing.Point(233, 19);
            this.lblHeaderCategory.Name = "lblHeaderCategory";
            this.lblHeaderCategory.Size = new System.Drawing.Size(49, 13);
            this.lblHeaderCategory.TabIndex = 31;
            this.lblHeaderCategory.Text = "Category";
            // 
            // tbDatCreator_Category
            // 
            this.tbDatCreator_Category.Location = new System.Drawing.Point(295, 15);
            this.tbDatCreator_Category.Name = "tbDatCreator_Category";
            this.tbDatCreator_Category.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Category.TabIndex = 30;
            // 
            // lblHeaderComment
            // 
            this.lblHeaderComment.AutoSize = true;
            this.lblHeaderComment.Location = new System.Drawing.Point(4, 114);
            this.lblHeaderComment.Name = "lblHeaderComment";
            this.lblHeaderComment.Size = new System.Drawing.Size(51, 13);
            this.lblHeaderComment.TabIndex = 29;
            this.lblHeaderComment.Text = "Comment";
            // 
            // tbDatCreator_Comment
            // 
            this.tbDatCreator_Comment.Location = new System.Drawing.Point(66, 111);
            this.tbDatCreator_Comment.Name = "tbDatCreator_Comment";
            this.tbDatCreator_Comment.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Comment.TabIndex = 28;
            // 
            // lblHeaderAuthor
            // 
            this.lblHeaderAuthor.AutoSize = true;
            this.lblHeaderAuthor.Location = new System.Drawing.Point(4, 90);
            this.lblHeaderAuthor.Name = "lblHeaderAuthor";
            this.lblHeaderAuthor.Size = new System.Drawing.Size(38, 13);
            this.lblHeaderAuthor.TabIndex = 27;
            this.lblHeaderAuthor.Text = "Author";
            // 
            // tbDatCreator_Author
            // 
            this.tbDatCreator_Author.Location = new System.Drawing.Point(66, 86);
            this.tbDatCreator_Author.Name = "tbDatCreator_Author";
            this.tbDatCreator_Author.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Author.TabIndex = 26;
            // 
            // lblHeaderVersion
            // 
            this.lblHeaderVersion.AutoSize = true;
            this.lblHeaderVersion.Location = new System.Drawing.Point(4, 66);
            this.lblHeaderVersion.Name = "lblHeaderVersion";
            this.lblHeaderVersion.Size = new System.Drawing.Size(42, 13);
            this.lblHeaderVersion.TabIndex = 25;
            this.lblHeaderVersion.Text = "Version";
            // 
            // tbDatCreator_Version
            // 
            this.tbDatCreator_Version.Location = new System.Drawing.Point(66, 62);
            this.tbDatCreator_Version.Name = "tbDatCreator_Version";
            this.tbDatCreator_Version.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Version.TabIndex = 24;
            // 
            // lblHeaderDescription
            // 
            this.lblHeaderDescription.AutoSize = true;
            this.lblHeaderDescription.Location = new System.Drawing.Point(4, 42);
            this.lblHeaderDescription.Name = "lblHeaderDescription";
            this.lblHeaderDescription.Size = new System.Drawing.Size(60, 13);
            this.lblHeaderDescription.TabIndex = 23;
            this.lblHeaderDescription.Text = "Description";
            // 
            // tbDatCreator_Description
            // 
            this.tbDatCreator_Description.Location = new System.Drawing.Point(66, 38);
            this.tbDatCreator_Description.Name = "tbDatCreator_Description";
            this.tbDatCreator_Description.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Description.TabIndex = 22;
            // 
            // lblHeaderName
            // 
            this.lblHeaderName.AutoSize = true;
            this.lblHeaderName.Location = new System.Drawing.Point(4, 19);
            this.lblHeaderName.Name = "lblHeaderName";
            this.lblHeaderName.Size = new System.Drawing.Size(35, 13);
            this.lblHeaderName.TabIndex = 21;
            this.lblHeaderName.Text = "Name";
            // 
            // tbDatCreator_Name
            // 
            this.tbDatCreator_Name.Location = new System.Drawing.Point(66, 15);
            this.tbDatCreator_Name.Name = "tbDatCreator_Name";
            this.tbDatCreator_Name.Size = new System.Drawing.Size(163, 20);
            this.tbDatCreator_Name.TabIndex = 20;
            // 
            // gbSourceDestPaths
            // 
            this.gbSourceDestPaths.Controls.Add(this.btnDatCreator_BrowseDestination);
            this.gbSourceDestPaths.Controls.Add(this.btnDatCreator_BrowseSource);
            this.gbSourceDestPaths.Controls.Add(this.lblDatCreator_DestinationFolder);
            this.gbSourceDestPaths.Controls.Add(this.tbDatCreator_OutputDat);
            this.gbSourceDestPaths.Controls.Add(this.lblDatCreator_SourceFolder);
            this.gbSourceDestPaths.Controls.Add(this.tbDatCreator_SourceFolder);
            this.gbSourceDestPaths.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSourceDestPaths.Location = new System.Drawing.Point(0, 163);
            this.gbSourceDestPaths.Name = "gbSourceDestPaths";
            this.gbSourceDestPaths.Size = new System.Drawing.Size(645, 94);
            this.gbSourceDestPaths.TabIndex = 6;
            this.gbSourceDestPaths.TabStop = false;
            this.gbSourceDestPaths.Text = "Source/Destination";
            // 
            // btnDatCreator_BrowseDestination
            // 
            this.btnDatCreator_BrowseDestination.Location = new System.Drawing.Point(268, 70);
            this.btnDatCreator_BrowseDestination.Name = "btnDatCreator_BrowseDestination";
            this.btnDatCreator_BrowseDestination.Size = new System.Drawing.Size(28, 20);
            this.btnDatCreator_BrowseDestination.TabIndex = 14;
            this.btnDatCreator_BrowseDestination.Text = "...";
            this.btnDatCreator_BrowseDestination.UseVisualStyleBackColor = true;
            this.btnDatCreator_BrowseDestination.Click += new System.EventHandler(this.btnDatCreator_BrowseDestination_Click);
            // 
            // btnDatCreator_BrowseSource
            // 
            this.btnDatCreator_BrowseSource.Location = new System.Drawing.Point(268, 30);
            this.btnDatCreator_BrowseSource.Name = "btnDatCreator_BrowseSource";
            this.btnDatCreator_BrowseSource.Size = new System.Drawing.Size(28, 20);
            this.btnDatCreator_BrowseSource.TabIndex = 13;
            this.btnDatCreator_BrowseSource.Text = "...";
            this.btnDatCreator_BrowseSource.UseVisualStyleBackColor = true;
            this.btnDatCreator_BrowseSource.Click += new System.EventHandler(this.btnDatCreator_BrowseSource_Click);
            // 
            // lblDatCreator_DestinationFolder
            // 
            this.lblDatCreator_DestinationFolder.AutoSize = true;
            this.lblDatCreator_DestinationFolder.Location = new System.Drawing.Point(3, 53);
            this.lblDatCreator_DestinationFolder.Name = "lblDatCreator_DestinationFolder";
            this.lblDatCreator_DestinationFolder.Size = new System.Drawing.Size(43, 13);
            this.lblDatCreator_DestinationFolder.TabIndex = 12;
            this.lblDatCreator_DestinationFolder.Text = "Datafile";
            // 
            // tbDatCreator_OutputDat
            // 
            this.tbDatCreator_OutputDat.Location = new System.Drawing.Point(3, 70);
            this.tbDatCreator_OutputDat.Name = "tbDatCreator_OutputDat";
            this.tbDatCreator_OutputDat.Size = new System.Drawing.Size(259, 20);
            this.tbDatCreator_OutputDat.TabIndex = 11;
            // 
            // lblDatCreator_SourceFolder
            // 
            this.lblDatCreator_SourceFolder.AutoSize = true;
            this.lblDatCreator_SourceFolder.Location = new System.Drawing.Point(3, 13);
            this.lblDatCreator_SourceFolder.Name = "lblDatCreator_SourceFolder";
            this.lblDatCreator_SourceFolder.Size = new System.Drawing.Size(73, 13);
            this.lblDatCreator_SourceFolder.TabIndex = 10;
            this.lblDatCreator_SourceFolder.Text = "Source Folder";
            // 
            // tbDatCreator_SourceFolder
            // 
            this.tbDatCreator_SourceFolder.Location = new System.Drawing.Point(3, 30);
            this.tbDatCreator_SourceFolder.Name = "tbDatCreator_SourceFolder";
            this.tbDatCreator_SourceFolder.Size = new System.Drawing.Size(259, 20);
            this.tbDatCreator_SourceFolder.TabIndex = 9;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbMd5Sha1);
            this.grpOptions.Controls.Add(this.cbUseNormalChecksum);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.Location = new System.Drawing.Point(0, 257);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(645, 62);
            this.grpOptions.TabIndex = 7;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbUseNormalChecksum
            // 
            this.cbUseNormalChecksum.AutoSize = true;
            this.cbUseNormalChecksum.Location = new System.Drawing.Point(3, 42);
            this.cbUseNormalChecksum.Name = "cbUseNormalChecksum";
            this.cbUseNormalChecksum.Size = new System.Drawing.Size(343, 17);
            this.cbUseNormalChecksum.TabIndex = 0;
            this.cbUseNormalChecksum.Text = "Use normal checksums and sizes (No VGMToolbox methods used).";
            this.cbUseNormalChecksum.UseVisualStyleBackColor = true;
            // 
            // cbMd5Sha1
            // 
            this.cbMd5Sha1.AutoSize = true;
            this.cbMd5Sha1.Location = new System.Drawing.Point(3, 19);
            this.cbMd5Sha1.Name = "cbMd5Sha1";
            this.cbMd5Sha1.Size = new System.Drawing.Size(120, 17);
            this.cbMd5Sha1.TabIndex = 1;
            this.cbMd5Sha1.Text = "Include MD5/SHA1";
            this.cbMd5Sha1.UseVisualStyleBackColor = true;
            // 
            // DatafileCreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 542);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.gbSourceDestPaths);
            this.Controls.Add(this.gbHeader);
            this.Name = "DatafileCreatorForm";
            this.Text = "DatafileCreatorForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.gbHeader, 0);
            this.Controls.SetChildIndex(this.gbSourceDestPaths, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.gbHeader.ResumeLayout(false);
            this.gbHeader.PerformLayout();
            this.gbSourceDestPaths.ResumeLayout(false);
            this.gbSourceDestPaths.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbHeader;
        private System.Windows.Forms.GroupBox gbSourceDestPaths;
        private System.Windows.Forms.Button btnDatCreator_BrowseDestination;
        private System.Windows.Forms.Button btnDatCreator_BrowseSource;
        private System.Windows.Forms.Label lblDatCreator_DestinationFolder;
        private System.Windows.Forms.TextBox tbDatCreator_OutputDat;
        private System.Windows.Forms.Label lblDatCreator_SourceFolder;
        private System.Windows.Forms.TextBox tbDatCreator_SourceFolder;
        private System.Windows.Forms.Label lblHeaderUrl;
        private System.Windows.Forms.TextBox tbDatCreator_Url;
        private System.Windows.Forms.Label lblHeaderHomepage;
        private System.Windows.Forms.TextBox tbDatCreator_Homepage;
        private System.Windows.Forms.Label lblHeaderEmail;
        private System.Windows.Forms.TextBox tbDatCreator_Email;
        private System.Windows.Forms.Label lblHeaderDate;
        private System.Windows.Forms.TextBox tbDatCreator_Date;
        private System.Windows.Forms.Label lblHeaderCategory;
        private System.Windows.Forms.TextBox tbDatCreator_Category;
        private System.Windows.Forms.Label lblHeaderComment;
        private System.Windows.Forms.TextBox tbDatCreator_Comment;
        private System.Windows.Forms.Label lblHeaderAuthor;
        private System.Windows.Forms.TextBox tbDatCreator_Author;
        private System.Windows.Forms.Label lblHeaderVersion;
        private System.Windows.Forms.TextBox tbDatCreator_Version;
        private System.Windows.Forms.Label lblHeaderDescription;
        private System.Windows.Forms.TextBox tbDatCreator_Description;
        private System.Windows.Forms.Label lblHeaderName;
        private System.Windows.Forms.TextBox tbDatCreator_Name;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox cbUseNormalChecksum;
        private System.Windows.Forms.CheckBox cbMd5Sha1;
    }
}