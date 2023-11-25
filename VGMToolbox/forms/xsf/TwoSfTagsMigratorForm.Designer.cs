namespace VGMToolbox.forms.xsf
{
    partial class TwoSfTagsMigratorForm
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
            this.btnBrowseV2Source = new System.Windows.Forms.Button();
            this.btnBrowseV1Source = new System.Windows.Forms.Button();
            this.tbV2Source = new System.Windows.Forms.TextBox();
            this.lblV2Folder = new System.Windows.Forms.Label();
            this.lblV1Folder = new System.Windows.Forms.Label();
            this.tbV1Source = new System.Windows.Forms.TextBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbEmptyTags = new System.Windows.Forms.CheckBox();
            this.cbFileName = new System.Windows.Forms.CheckBox();
            this.btnDefault = new System.Windows.Forms.Button();
            this.btnCheckNone = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.cbFade = new System.Windows.Forms.CheckBox();
            this.cbLength = new System.Windows.Forms.CheckBox();
            this.cb2sfBy = new System.Windows.Forms.CheckBox();
            this.cbCopyright = new System.Windows.Forms.CheckBox();
            this.cbVolume = new System.Windows.Forms.CheckBox();
            this.cbComment = new System.Windows.Forms.CheckBox();
            this.cbGenre = new System.Windows.Forms.CheckBox();
            this.cbArtist = new System.Windows.Forms.CheckBox();
            this.cbGame = new System.Windows.Forms.CheckBox();
            this.cbYear = new System.Windows.Forms.CheckBox();
            this.cbTitle = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 506);
            this.pnlLabels.Size = new System.Drawing.Size(903, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(903, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 429);
            this.tbOutput.Size = new System.Drawing.Size(903, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 409);
            this.pnlButtons.Size = new System.Drawing.Size(903, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(843, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(783, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.btnBrowseV2Source);
            this.grpSource.Controls.Add(this.btnBrowseV1Source);
            this.grpSource.Controls.Add(this.tbV2Source);
            this.grpSource.Controls.Add(this.lblV2Folder);
            this.grpSource.Controls.Add(this.lblV1Folder);
            this.grpSource.Controls.Add(this.tbV1Source);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(903, 72);
            this.grpSource.TabIndex = 5;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Folders";
            // 
            // btnBrowseV2Source
            // 
            this.btnBrowseV2Source.Location = new System.Drawing.Point(324, 45);
            this.btnBrowseV2Source.Name = "btnBrowseV2Source";
            this.btnBrowseV2Source.Size = new System.Drawing.Size(32, 20);
            this.btnBrowseV2Source.TabIndex = 5;
            this.btnBrowseV2Source.Text = "...";
            this.btnBrowseV2Source.UseVisualStyleBackColor = true;
            this.btnBrowseV2Source.Click += new System.EventHandler(this.btnBrowseV2Source_Click);
            // 
            // btnBrowseV1Source
            // 
            this.btnBrowseV1Source.Location = new System.Drawing.Point(324, 19);
            this.btnBrowseV1Source.Name = "btnBrowseV1Source";
            this.btnBrowseV1Source.Size = new System.Drawing.Size(32, 20);
            this.btnBrowseV1Source.TabIndex = 4;
            this.btnBrowseV1Source.Text = "...";
            this.btnBrowseV1Source.UseVisualStyleBackColor = true;
            this.btnBrowseV1Source.Click += new System.EventHandler(this.btnBrowseV1Source_Click);
            // 
            // tbV2Source
            // 
            this.tbV2Source.Location = new System.Drawing.Point(67, 45);
            this.tbV2Source.Name = "tbV2Source";
            this.tbV2Source.Size = new System.Drawing.Size(251, 20);
            this.tbV2Source.TabIndex = 3;
            // 
            // lblV2Folder
            // 
            this.lblV2Folder.AutoSize = true;
            this.lblV2Folder.Location = new System.Drawing.Point(4, 48);
            this.lblV2Folder.Name = "lblV2Folder";
            this.lblV2Folder.Size = new System.Drawing.Size(52, 13);
            this.lblV2Folder.TabIndex = 2;
            this.lblV2Folder.Text = "V2 Folder";
            // 
            // lblV1Folder
            // 
            this.lblV1Folder.AutoSize = true;
            this.lblV1Folder.Location = new System.Drawing.Point(4, 22);
            this.lblV1Folder.Name = "lblV1Folder";
            this.lblV1Folder.Size = new System.Drawing.Size(52, 13);
            this.lblV1Folder.TabIndex = 1;
            this.lblV1Folder.Text = "V1 Folder";
            // 
            // tbV1Source
            // 
            this.tbV1Source.Location = new System.Drawing.Point(67, 19);
            this.tbV1Source.Name = "tbV1Source";
            this.tbV1Source.Size = new System.Drawing.Size(251, 20);
            this.tbV1Source.TabIndex = 0;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbEmptyTags);
            this.grpOptions.Controls.Add(this.cbFileName);
            this.grpOptions.Controls.Add(this.btnDefault);
            this.grpOptions.Controls.Add(this.btnCheckNone);
            this.grpOptions.Controls.Add(this.btnCheckAll);
            this.grpOptions.Controls.Add(this.cbFade);
            this.grpOptions.Controls.Add(this.cbLength);
            this.grpOptions.Controls.Add(this.cb2sfBy);
            this.grpOptions.Controls.Add(this.cbCopyright);
            this.grpOptions.Controls.Add(this.cbVolume);
            this.grpOptions.Controls.Add(this.cbComment);
            this.grpOptions.Controls.Add(this.cbGenre);
            this.grpOptions.Controls.Add(this.cbArtist);
            this.grpOptions.Controls.Add(this.cbGame);
            this.grpOptions.Controls.Add(this.cbYear);
            this.grpOptions.Controls.Add(this.cbTitle);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.Location = new System.Drawing.Point(0, 95);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(903, 89);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbEmptyTags
            // 
            this.cbEmptyTags.AutoSize = true;
            this.cbEmptyTags.Location = new System.Drawing.Point(266, 42);
            this.cbEmptyTags.Name = "cbEmptyTags";
            this.cbEmptyTags.Size = new System.Drawing.Size(109, 17);
            this.cbEmptyTags.TabIndex = 15;
            this.cbEmptyTags.Text = "Copy Empty Tags";
            this.cbEmptyTags.UseVisualStyleBackColor = true;
            // 
            // cbFileName
            // 
            this.cbFileName.AutoSize = true;
            this.cbFileName.Location = new System.Drawing.Point(266, 19);
            this.cbFileName.Name = "cbFileName";
            this.cbFileName.Size = new System.Drawing.Size(73, 17);
            this.cbFileName.TabIndex = 14;
            this.cbFileName.Text = "File Name";
            this.cbFileName.UseVisualStyleBackColor = true;
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(381, 16);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 20);
            this.btnDefault.TabIndex = 13;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // btnCheckNone
            // 
            this.btnCheckNone.Location = new System.Drawing.Point(381, 62);
            this.btnCheckNone.Name = "btnCheckNone";
            this.btnCheckNone.Size = new System.Drawing.Size(75, 20);
            this.btnCheckNone.TabIndex = 12;
            this.btnCheckNone.Text = "Check None";
            this.btnCheckNone.UseVisualStyleBackColor = true;
            this.btnCheckNone.Click += new System.EventHandler(this.btnCheckNone_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(381, 39);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(75, 20);
            this.btnCheckAll.TabIndex = 11;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // cbFade
            // 
            this.cbFade.AutoSize = true;
            this.cbFade.Location = new System.Drawing.Point(199, 65);
            this.cbFade.Name = "cbFade";
            this.cbFade.Size = new System.Drawing.Size(50, 17);
            this.cbFade.TabIndex = 10;
            this.cbFade.Text = "Fade";
            this.cbFade.UseVisualStyleBackColor = true;
            // 
            // cbLength
            // 
            this.cbLength.AutoSize = true;
            this.cbLength.Location = new System.Drawing.Point(199, 42);
            this.cbLength.Name = "cbLength";
            this.cbLength.Size = new System.Drawing.Size(59, 17);
            this.cbLength.TabIndex = 9;
            this.cbLength.Text = "Length";
            this.cbLength.UseVisualStyleBackColor = true;
            // 
            // cb2sfBy
            // 
            this.cb2sfBy.AutoSize = true;
            this.cb2sfBy.Location = new System.Drawing.Point(143, 42);
            this.cb2sfBy.Name = "cb2sfBy";
            this.cb2sfBy.Size = new System.Drawing.Size(52, 17);
            this.cb2sfBy.TabIndex = 8;
            this.cb2sfBy.Text = "2sfBy";
            this.cb2sfBy.UseVisualStyleBackColor = true;
            // 
            // cbCopyright
            // 
            this.cbCopyright.AutoSize = true;
            this.cbCopyright.Checked = true;
            this.cbCopyright.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCopyright.Location = new System.Drawing.Point(67, 42);
            this.cbCopyright.Name = "cbCopyright";
            this.cbCopyright.Size = new System.Drawing.Size(70, 17);
            this.cbCopyright.TabIndex = 7;
            this.cbCopyright.Text = "Copyright";
            this.cbCopyright.UseVisualStyleBackColor = true;
            // 
            // cbVolume
            // 
            this.cbVolume.AutoSize = true;
            this.cbVolume.Location = new System.Drawing.Point(199, 19);
            this.cbVolume.Name = "cbVolume";
            this.cbVolume.Size = new System.Drawing.Size(61, 17);
            this.cbVolume.TabIndex = 6;
            this.cbVolume.Text = "Volume";
            this.cbVolume.UseVisualStyleBackColor = true;
            // 
            // cbComment
            // 
            this.cbComment.AutoSize = true;
            this.cbComment.Location = new System.Drawing.Point(67, 65);
            this.cbComment.Name = "cbComment";
            this.cbComment.Size = new System.Drawing.Size(70, 17);
            this.cbComment.TabIndex = 5;
            this.cbComment.Text = "Comment";
            this.cbComment.UseVisualStyleBackColor = true;
            // 
            // cbGenre
            // 
            this.cbGenre.AutoSize = true;
            this.cbGenre.Checked = true;
            this.cbGenre.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGenre.Location = new System.Drawing.Point(67, 19);
            this.cbGenre.Name = "cbGenre";
            this.cbGenre.Size = new System.Drawing.Size(55, 17);
            this.cbGenre.TabIndex = 4;
            this.cbGenre.Text = "Genre";
            this.cbGenre.UseVisualStyleBackColor = true;
            // 
            // cbArtist
            // 
            this.cbArtist.AutoSize = true;
            this.cbArtist.Checked = true;
            this.cbArtist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbArtist.Location = new System.Drawing.Point(7, 65);
            this.cbArtist.Name = "cbArtist";
            this.cbArtist.Size = new System.Drawing.Size(49, 17);
            this.cbArtist.TabIndex = 3;
            this.cbArtist.Text = "Artist";
            this.cbArtist.UseVisualStyleBackColor = true;
            // 
            // cbGame
            // 
            this.cbGame.AutoSize = true;
            this.cbGame.Checked = true;
            this.cbGame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGame.Location = new System.Drawing.Point(7, 19);
            this.cbGame.Name = "cbGame";
            this.cbGame.Size = new System.Drawing.Size(54, 17);
            this.cbGame.TabIndex = 2;
            this.cbGame.Text = "Game";
            this.cbGame.UseVisualStyleBackColor = true;
            // 
            // cbYear
            // 
            this.cbYear.AutoSize = true;
            this.cbYear.Checked = true;
            this.cbYear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbYear.Location = new System.Drawing.Point(7, 42);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(48, 17);
            this.cbYear.TabIndex = 1;
            this.cbYear.Text = "Year";
            this.cbYear.UseVisualStyleBackColor = true;
            // 
            // cbTitle
            // 
            this.cbTitle.AutoSize = true;
            this.cbTitle.Checked = true;
            this.cbTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTitle.Location = new System.Drawing.Point(143, 19);
            this.cbTitle.Name = "cbTitle";
            this.cbTitle.Size = new System.Drawing.Size(46, 17);
            this.cbTitle.TabIndex = 0;
            this.cbTitle.Text = "Title";
            this.cbTitle.UseVisualStyleBackColor = true;
            // 
            // Xsf_2sfTagsMigratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 547);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpSource);
            this.Name = "TwoSfTagsMigratorForm";
            this.Text = "TwoSfTagsMigratorForm";
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
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox cbTitle;
        private System.Windows.Forms.CheckBox cbGame;
        private System.Windows.Forms.CheckBox cbYear;
        private System.Windows.Forms.CheckBox cbArtist;
        private System.Windows.Forms.CheckBox cbGenre;
        private System.Windows.Forms.CheckBox cbComment;
        private System.Windows.Forms.CheckBox cbVolume;
        private System.Windows.Forms.CheckBox cbCopyright;
        private System.Windows.Forms.CheckBox cb2sfBy;
        private System.Windows.Forms.CheckBox cbFade;
        private System.Windows.Forms.CheckBox cbLength;
        private System.Windows.Forms.Button btnCheckNone;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.TextBox tbV2Source;
        private System.Windows.Forms.Label lblV2Folder;
        private System.Windows.Forms.Label lblV1Folder;
        private System.Windows.Forms.TextBox tbV1Source;
        private System.Windows.Forms.Button btnBrowseV2Source;
        private System.Windows.Forms.Button btnBrowseV1Source;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.CheckBox cbFileName;
        private System.Windows.Forms.CheckBox cbEmptyTags;
    }
}