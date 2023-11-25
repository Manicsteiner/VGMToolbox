namespace VGMToolbox.forms.xsf
{
    partial class XsfTagEditorForm
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
            this.components = new System.ComponentModel.Container();
            this.grpSourceFiles = new System.Windows.Forms.GroupBox();
            this.tbSourceDirectory = new System.Windows.Forms.TextBox();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.btnBrowseDirectory = new System.Windows.Forms.Button();
            this.grpSetTags = new System.Windows.Forms.GroupBox();
            this.tbSystem = new System.Windows.Forms.TextBox();
            this.lblSystem = new System.Windows.Forms.Label();
            this.lblXsfBy = new System.Windows.Forms.Label();
            this.tbXsfBy = new System.Windows.Forms.TextBox();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblGenre = new System.Windows.Forms.Label();
            this.tbGenre = new System.Windows.Forms.TextBox();
            this.tbCopyright = new System.Windows.Forms.TextBox();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblArtist = new System.Windows.Forms.Label();
            this.tbArtist = new System.Windows.Forms.TextBox();
            this.lblGame = new System.Windows.Forms.Label();
            this.tbGame = new System.Windows.Forms.TextBox();
            this.grpTrackTags = new System.Windows.Forms.GroupBox();
            this.tbVolume = new System.Windows.Forms.TextBox();
            this.lblVolume = new System.Windows.Forms.Label();
            this.tbFade = new System.Windows.Forms.TextBox();
            this.lblFade = new System.Windows.Forms.Label();
            this.lblTrackTitle = new System.Windows.Forms.Label();
            this.tbLength = new System.Windows.Forms.TextBox();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.lblLength = new System.Windows.Forms.Label();
            this.cbGenerateTitleFromFilename = new System.Windows.Forms.CheckBox();
            this.grpComments = new System.Windows.Forms.GroupBox();
            this.tbComments = new System.Windows.Forms.TextBox();
            this.cbDeleteEmpty = new System.Windows.Forms.CheckBox();
            this.contextMenuRefresh = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshFileListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearFileListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbAddToBatchFile = new System.Windows.Forms.CheckBox();
            this.cbRemoveBrackets = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTagger = new System.Windows.Forms.TextBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSourceFiles.SuspendLayout();
            this.grpSetTags.SuspendLayout();
            this.grpTrackTags.SuspendLayout();
            this.grpComments.SuspendLayout();
            this.contextMenuRefresh.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 404);
            this.pnlLabels.Size = new System.Drawing.Size(867, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(867, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 327);
            this.tbOutput.Size = new System.Drawing.Size(867, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 307);
            this.pnlButtons.Size = new System.Drawing.Size(867, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(807, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(747, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpSourceFiles
            // 
            this.grpSourceFiles.Controls.Add(this.tbSourceDirectory);
            this.grpSourceFiles.Controls.Add(this.lbFiles);
            this.grpSourceFiles.Controls.Add(this.btnBrowseDirectory);
            this.grpSourceFiles.Location = new System.Drawing.Point(3, 29);
            this.grpSourceFiles.Name = "grpSourceFiles";
            this.grpSourceFiles.Size = new System.Drawing.Size(229, 303);
            this.grpSourceFiles.TabIndex = 7;
            this.grpSourceFiles.TabStop = false;
            this.grpSourceFiles.Text = "Source Files";
            // 
            // tbSourceDirectory
            // 
            this.tbSourceDirectory.Location = new System.Drawing.Point(6, 16);
            this.tbSourceDirectory.Name = "tbSourceDirectory";
            this.tbSourceDirectory.Size = new System.Drawing.Size(183, 20);
            this.tbSourceDirectory.TabIndex = 6;
            this.tbSourceDirectory.TextChanged += new System.EventHandler(this.tbSourceDirectory_TextChanged);
            // 
            // lbFiles
            // 
            this.lbFiles.AllowDrop = true;
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.HorizontalScrollbar = true;
            this.lbFiles.Location = new System.Drawing.Point(6, 43);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFiles.Size = new System.Drawing.Size(215, 251);
            this.lbFiles.TabIndex = 5;
            this.lbFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbFiles_MouseUp);
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            this.lbFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragDrop);
            this.lbFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragEnter);
            // 
            // btnBrowseDirectory
            // 
            this.btnBrowseDirectory.Location = new System.Drawing.Point(195, 16);
            this.btnBrowseDirectory.Name = "btnBrowseDirectory";
            this.btnBrowseDirectory.Size = new System.Drawing.Size(26, 20);
            this.btnBrowseDirectory.TabIndex = 9;
            this.btnBrowseDirectory.Text = "...";
            this.btnBrowseDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseDirectory.Click += new System.EventHandler(this.btnBrowseDirectory_Click);
            // 
            // grpSetTags
            // 
            this.grpSetTags.Controls.Add(this.tbTagger);
            this.grpSetTags.Controls.Add(this.label1);
            this.grpSetTags.Controls.Add(this.tbSystem);
            this.grpSetTags.Controls.Add(this.lblSystem);
            this.grpSetTags.Controls.Add(this.lblXsfBy);
            this.grpSetTags.Controls.Add(this.tbXsfBy);
            this.grpSetTags.Controls.Add(this.tbYear);
            this.grpSetTags.Controls.Add(this.lblYear);
            this.grpSetTags.Controls.Add(this.lblGenre);
            this.grpSetTags.Controls.Add(this.tbGenre);
            this.grpSetTags.Controls.Add(this.tbCopyright);
            this.grpSetTags.Controls.Add(this.lblCopyright);
            this.grpSetTags.Controls.Add(this.lblArtist);
            this.grpSetTags.Controls.Add(this.tbArtist);
            this.grpSetTags.Controls.Add(this.lblGame);
            this.grpSetTags.Controls.Add(this.tbGame);
            this.grpSetTags.Location = new System.Drawing.Point(238, 29);
            this.grpSetTags.Name = "grpSetTags";
            this.grpSetTags.Size = new System.Drawing.Size(286, 173);
            this.grpSetTags.TabIndex = 8;
            this.grpSetTags.TabStop = false;
            this.grpSetTags.Text = "GameTags";
            // 
            // tbSystem
            // 
            this.tbSystem.Location = new System.Drawing.Point(63, 150);
            this.tbSystem.Name = "tbSystem";
            this.tbSystem.Size = new System.Drawing.Size(214, 20);
            this.tbSystem.TabIndex = 13;
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.Location = new System.Drawing.Point(9, 153);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(41, 13);
            this.lblSystem.TabIndex = 12;
            this.lblSystem.Text = "System";
            // 
            // lblXsfBy
            // 
            this.lblXsfBy.AutoSize = true;
            this.lblXsfBy.Location = new System.Drawing.Point(9, 127);
            this.lblXsfBy.Name = "lblXsfBy";
            this.lblXsfBy.Size = new System.Drawing.Size(40, 13);
            this.lblXsfBy.TabIndex = 10;
            this.lblXsfBy.Text = "xSF By";
            // 
            // tbXsfBy
            // 
            this.tbXsfBy.Location = new System.Drawing.Point(63, 124);
            this.tbXsfBy.Name = "tbXsfBy";
            this.tbXsfBy.Size = new System.Drawing.Size(80, 20);
            this.tbXsfBy.TabIndex = 11;
            // 
            // tbYear
            // 
            this.tbYear.Location = new System.Drawing.Point(225, 98);
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(52, 20);
            this.tbYear.TabIndex = 9;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(190, 101);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(29, 13);
            this.lblYear.TabIndex = 8;
            this.lblYear.Text = "Year";
            // 
            // lblGenre
            // 
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(6, 101);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(36, 13);
            this.lblGenre.TabIndex = 6;
            this.lblGenre.Text = "Genre";
            // 
            // tbGenre
            // 
            this.tbGenre.Location = new System.Drawing.Point(63, 98);
            this.tbGenre.Name = "tbGenre";
            this.tbGenre.Size = new System.Drawing.Size(118, 20);
            this.tbGenre.TabIndex = 7;
            // 
            // tbCopyright
            // 
            this.tbCopyright.Location = new System.Drawing.Point(63, 72);
            this.tbCopyright.Name = "tbCopyright";
            this.tbCopyright.Size = new System.Drawing.Size(214, 20);
            this.tbCopyright.TabIndex = 5;
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(6, 75);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(51, 13);
            this.lblCopyright.TabIndex = 4;
            this.lblCopyright.Text = "Copyright";
            // 
            // lblArtist
            // 
            this.lblArtist.AutoSize = true;
            this.lblArtist.Location = new System.Drawing.Point(6, 48);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(30, 13);
            this.lblArtist.TabIndex = 2;
            this.lblArtist.Text = "Artist";
            // 
            // tbArtist
            // 
            this.tbArtist.Location = new System.Drawing.Point(63, 46);
            this.tbArtist.Name = "tbArtist";
            this.tbArtist.Size = new System.Drawing.Size(214, 20);
            this.tbArtist.TabIndex = 3;
            // 
            // lblGame
            // 
            this.lblGame.AutoSize = true;
            this.lblGame.Location = new System.Drawing.Point(6, 22);
            this.lblGame.Name = "lblGame";
            this.lblGame.Size = new System.Drawing.Size(35, 13);
            this.lblGame.TabIndex = 0;
            this.lblGame.Text = "Game";
            // 
            // tbGame
            // 
            this.tbGame.Location = new System.Drawing.Point(63, 19);
            this.tbGame.Name = "tbGame";
            this.tbGame.Size = new System.Drawing.Size(214, 20);
            this.tbGame.TabIndex = 1;
            // 
            // grpTrackTags
            // 
            this.grpTrackTags.Controls.Add(this.tbVolume);
            this.grpTrackTags.Controls.Add(this.lblVolume);
            this.grpTrackTags.Controls.Add(this.tbFade);
            this.grpTrackTags.Controls.Add(this.lblFade);
            this.grpTrackTags.Controls.Add(this.lblTrackTitle);
            this.grpTrackTags.Controls.Add(this.tbLength);
            this.grpTrackTags.Controls.Add(this.tbTitle);
            this.grpTrackTags.Controls.Add(this.lblLength);
            this.grpTrackTags.Location = new System.Drawing.Point(238, 208);
            this.grpTrackTags.Name = "grpTrackTags";
            this.grpTrackTags.Size = new System.Drawing.Size(287, 64);
            this.grpTrackTags.TabIndex = 18;
            this.grpTrackTags.TabStop = false;
            this.grpTrackTags.Text = "Track Tags";
            // 
            // tbVolume
            // 
            this.tbVolume.Location = new System.Drawing.Point(231, 39);
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Size = new System.Drawing.Size(47, 20);
            this.tbVolume.TabIndex = 18;
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(186, 42);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(42, 13);
            this.lblVolume.TabIndex = 17;
            this.lblVolume.Text = "Volume";
            // 
            // tbFade
            // 
            this.tbFade.Location = new System.Drawing.Point(144, 39);
            this.tbFade.Name = "tbFade";
            this.tbFade.Size = new System.Drawing.Size(38, 20);
            this.tbFade.TabIndex = 16;
            // 
            // lblFade
            // 
            this.lblFade.AutoSize = true;
            this.lblFade.Location = new System.Drawing.Point(111, 42);
            this.lblFade.Name = "lblFade";
            this.lblFade.Size = new System.Drawing.Size(31, 13);
            this.lblFade.TabIndex = 17;
            this.lblFade.Text = "Fade";
            // 
            // lblTrackTitle
            // 
            this.lblTrackTitle.AutoSize = true;
            this.lblTrackTitle.Location = new System.Drawing.Point(6, 16);
            this.lblTrackTitle.Name = "lblTrackTitle";
            this.lblTrackTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTrackTitle.TabIndex = 12;
            this.lblTrackTitle.Text = "Title";
            // 
            // tbLength
            // 
            this.tbLength.Location = new System.Drawing.Point(64, 39);
            this.tbLength.Name = "tbLength";
            this.tbLength.Size = new System.Drawing.Size(43, 20);
            this.tbLength.TabIndex = 15;
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(64, 13);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(214, 20);
            this.tbTitle.TabIndex = 13;
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(6, 42);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(40, 13);
            this.lblLength.TabIndex = 14;
            this.lblLength.Text = "Length";
            // 
            // cbGenerateTitleFromFilename
            // 
            this.cbGenerateTitleFromFilename.AutoSize = true;
            this.cbGenerateTitleFromFilename.Location = new System.Drawing.Point(9, 338);
            this.cbGenerateTitleFromFilename.Name = "cbGenerateTitleFromFilename";
            this.cbGenerateTitleFromFilename.Size = new System.Drawing.Size(163, 17);
            this.cbGenerateTitleFromFilename.TabIndex = 19;
            this.cbGenerateTitleFromFilename.Text = "Generate Title from FileName";
            this.cbGenerateTitleFromFilename.UseVisualStyleBackColor = true;
            this.cbGenerateTitleFromFilename.CheckedChanged += new System.EventHandler(this.cbGenerateTitleFromFilename_CheckedChanged);
            // 
            // grpComments
            // 
            this.grpComments.Controls.Add(this.tbComments);
            this.grpComments.Location = new System.Drawing.Point(238, 278);
            this.grpComments.Name = "grpComments";
            this.grpComments.Size = new System.Drawing.Size(287, 54);
            this.grpComments.TabIndex = 22;
            this.grpComments.TabStop = false;
            this.grpComments.Text = "Comments";
            // 
            // tbComments
            // 
            this.tbComments.AcceptsReturn = true;
            this.tbComments.Location = new System.Drawing.Point(6, 15);
            this.tbComments.Multiline = true;
            this.tbComments.Name = "tbComments";
            this.tbComments.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbComments.Size = new System.Drawing.Size(269, 35);
            this.tbComments.TabIndex = 19;
            // 
            // cbDeleteEmpty
            // 
            this.cbDeleteEmpty.AutoSize = true;
            this.cbDeleteEmpty.Location = new System.Drawing.Point(238, 338);
            this.cbDeleteEmpty.Name = "cbDeleteEmpty";
            this.cbDeleteEmpty.Size = new System.Drawing.Size(265, 17);
            this.cbDeleteEmpty.TabIndex = 23;
            this.cbDeleteEmpty.Text = "Remove fields you\'ve left empty from the files\' tags.";
            this.cbDeleteEmpty.UseVisualStyleBackColor = true;
            // 
            // contextMenuRefresh
            // 
            this.contextMenuRefresh.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshFileListToolStripMenuItem,
            this.clearFileListToolStripMenuItem});
            this.contextMenuRefresh.Name = "contextMenuStrip1";
            this.contextMenuRefresh.Size = new System.Drawing.Size(151, 48);
            // 
            // refreshFileListToolStripMenuItem
            // 
            this.refreshFileListToolStripMenuItem.Name = "refreshFileListToolStripMenuItem";
            this.refreshFileListToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.refreshFileListToolStripMenuItem.Text = "Refresh File List";
            this.refreshFileListToolStripMenuItem.Click += new System.EventHandler(this.refreshFileListToolStripMenuItem_Click);
            // 
            // clearFileListToolStripMenuItem
            // 
            this.clearFileListToolStripMenuItem.Name = "clearFileListToolStripMenuItem";
            this.clearFileListToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.clearFileListToolStripMenuItem.Text = "Clear File List";
            this.clearFileListToolStripMenuItem.Click += new System.EventHandler(this.clearFileListToolStripMenuItem_Click);
            // 
            // cbAddToBatchFile
            // 
            this.cbAddToBatchFile.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbAddToBatchFile.Location = new System.Drawing.Point(238, 361);
            this.cbAddToBatchFile.Name = "cbAddToBatchFile";
            this.cbAddToBatchFile.Size = new System.Drawing.Size(278, 36);
            this.cbAddToBatchFile.TabIndex = 24;
            this.cbAddToBatchFile.Text = "Build psfpoint.exe batch file for all values in changed files (S98v3 not supporte" +
                "d).";
            this.cbAddToBatchFile.UseVisualStyleBackColor = true;
            // 
            // cbRemoveBrackets
            // 
            this.cbRemoveBrackets.AutoSize = true;
            this.cbRemoveBrackets.Location = new System.Drawing.Point(9, 361);
            this.cbRemoveBrackets.Name = "cbRemoveBrackets";
            this.cbRemoveBrackets.Size = new System.Drawing.Size(223, 17);
            this.cbRemoveBrackets.TabIndex = 25;
            this.cbRemoveBrackets.Text = "Remove Information in Brackets from Title";
            this.cbRemoveBrackets.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Tagger";
            // 
            // tbTagger
            // 
            this.tbTagger.Location = new System.Drawing.Point(196, 124);
            this.tbTagger.Name = "tbTagger";
            this.tbTagger.Size = new System.Drawing.Size(82, 20);
            this.tbTagger.TabIndex = 15;
            // 
            // XsfTagEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 445);
            this.Controls.Add(this.cbRemoveBrackets);
            this.Controls.Add(this.cbAddToBatchFile);
            this.Controls.Add(this.cbGenerateTitleFromFilename);
            this.Controls.Add(this.cbDeleteEmpty);
            this.Controls.Add(this.grpComments);
            this.Controls.Add(this.grpTrackTags);
            this.Controls.Add(this.grpSetTags);
            this.Controls.Add(this.grpSourceFiles);
            this.Name = "XsfTagEditorForm";
            this.Text = "XsfTagEditorForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSourceFiles, 0);
            this.Controls.SetChildIndex(this.grpSetTags, 0);
            this.Controls.SetChildIndex(this.grpTrackTags, 0);
            this.Controls.SetChildIndex(this.grpComments, 0);
            this.Controls.SetChildIndex(this.cbDeleteEmpty, 0);
            this.Controls.SetChildIndex(this.cbGenerateTitleFromFilename, 0);
            this.Controls.SetChildIndex(this.cbAddToBatchFile, 0);
            this.Controls.SetChildIndex(this.cbRemoveBrackets, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSourceFiles.ResumeLayout(false);
            this.grpSourceFiles.PerformLayout();
            this.grpSetTags.ResumeLayout(false);
            this.grpSetTags.PerformLayout();
            this.grpTrackTags.ResumeLayout(false);
            this.grpTrackTags.PerformLayout();
            this.grpComments.ResumeLayout(false);
            this.grpComments.PerformLayout();
            this.contextMenuRefresh.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSourceFiles;
        private System.Windows.Forms.TextBox tbSourceDirectory;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Button btnBrowseDirectory;
        private System.Windows.Forms.GroupBox grpSetTags;
        private System.Windows.Forms.Label lblXsfBy;
        private System.Windows.Forms.TextBox tbXsfBy;
        private System.Windows.Forms.TextBox tbYear;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.TextBox tbGenre;
        private System.Windows.Forms.TextBox tbCopyright;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.TextBox tbArtist;
        private System.Windows.Forms.Label lblGame;
        private System.Windows.Forms.TextBox tbGame;
        private System.Windows.Forms.GroupBox grpTrackTags;
        private System.Windows.Forms.TextBox tbVolume;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.TextBox tbFade;
        private System.Windows.Forms.Label lblFade;
        private System.Windows.Forms.Label lblTrackTitle;
        private System.Windows.Forms.TextBox tbLength;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.GroupBox grpComments;
        private System.Windows.Forms.TextBox tbComments;
        private System.Windows.Forms.CheckBox cbDeleteEmpty;
        private System.Windows.Forms.TextBox tbSystem;
        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.CheckBox cbGenerateTitleFromFilename;
        private System.Windows.Forms.ContextMenuStrip contextMenuRefresh;
        private System.Windows.Forms.ToolStripMenuItem refreshFileListToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbAddToBatchFile;
        private System.Windows.Forms.CheckBox cbRemoveBrackets;
        private System.Windows.Forms.ToolStripMenuItem clearFileListToolStripMenuItem;
        private System.Windows.Forms.TextBox tbTagger;
        private System.Windows.Forms.Label label1;
    }
}