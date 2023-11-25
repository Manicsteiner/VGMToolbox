namespace VGMToolbox.forms
{
    partial class Vgm_VgmTagEditorForm
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
            this.grpTrackTags = new System.Windows.Forms.GroupBox();
            this.cbTitleJp = new System.Windows.Forms.CheckBox();
            this.cbTitleEn = new System.Windows.Forms.CheckBox();
            this.lblTrackTitleJp = new System.Windows.Forms.Label();
            this.tbTitleJp = new System.Windows.Forms.TextBox();
            this.lblTrackTitleEn = new System.Windows.Forms.Label();
            this.tbTitleEn = new System.Windows.Forms.TextBox();
            this.grpSetTags = new System.Windows.Forms.GroupBox();
            this.cbRipper = new System.Windows.Forms.CheckBox();
            this.cbReleaseDate = new System.Windows.Forms.CheckBox();
            this.cbArtistJp = new System.Windows.Forms.CheckBox();
            this.cbArtistEn = new System.Windows.Forms.CheckBox();
            this.checkboxSystemJp = new System.Windows.Forms.CheckBox();
            this.checkboxSystemEn = new System.Windows.Forms.CheckBox();
            this.cbGameJp = new System.Windows.Forms.CheckBox();
            this.cbGameEnglish = new System.Windows.Forms.CheckBox();
            this.cbSystemJp = new System.Windows.Forms.ComboBox();
            this.cbSystemEn = new System.Windows.Forms.ComboBox();
            this.lblGameDate = new System.Windows.Forms.Label();
            this.tbGameDate = new System.Windows.Forms.TextBox();
            this.tbRipper = new System.Windows.Forms.TextBox();
            this.lblRipper = new System.Windows.Forms.Label();
            this.lblArtistJp = new System.Windows.Forms.Label();
            this.tbArtistJp = new System.Windows.Forms.TextBox();
            this.lblSystemJp = new System.Windows.Forms.Label();
            this.lblGameJp = new System.Windows.Forms.Label();
            this.tbGameJp = new System.Windows.Forms.TextBox();
            this.lblSystemEn = new System.Windows.Forms.Label();
            this.lblArtistEn = new System.Windows.Forms.Label();
            this.tbArtistEn = new System.Windows.Forms.TextBox();
            this.lblGameEn = new System.Windows.Forms.Label();
            this.tbGameEn = new System.Windows.Forms.TextBox();
            this.grpSourceFiles = new System.Windows.Forms.GroupBox();
            this.tbSourceDirectory = new System.Windows.Forms.TextBox();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.btnBrowseDirectory = new System.Windows.Forms.Button();
            this.grpComments = new System.Windows.Forms.GroupBox();
            this.cbComments = new System.Windows.Forms.CheckBox();
            this.tbComments = new System.Windows.Forms.TextBox();
            this.contextMenuRefresh = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.clearFileListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpTrackTags.SuspendLayout();
            this.grpSetTags.SuspendLayout();
            this.grpSourceFiles.SuspendLayout();
            this.grpComments.SuspendLayout();
            this.contextMenuRefresh.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 537);
            this.pnlLabels.Size = new System.Drawing.Size(1003, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(1003, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 460);
            this.tbOutput.Size = new System.Drawing.Size(1003, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 440);
            this.pnlButtons.Size = new System.Drawing.Size(1003, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(943, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(883, 0);
            this.btnDoTask.TabIndex = 15;
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpTrackTags
            // 
            this.grpTrackTags.Controls.Add(this.cbTitleJp);
            this.grpTrackTags.Controls.Add(this.cbTitleEn);
            this.grpTrackTags.Controls.Add(this.lblTrackTitleJp);
            this.grpTrackTags.Controls.Add(this.tbTitleJp);
            this.grpTrackTags.Controls.Add(this.lblTrackTitleEn);
            this.grpTrackTags.Controls.Add(this.tbTitleEn);
            this.grpTrackTags.Location = new System.Drawing.Point(238, 237);
            this.grpTrackTags.Name = "grpTrackTags";
            this.grpTrackTags.Size = new System.Drawing.Size(310, 68);
            this.grpTrackTags.TabIndex = 26;
            this.grpTrackTags.TabStop = false;
            this.grpTrackTags.Text = "Track Tags";
            // 
            // cbTitleJp
            // 
            this.cbTitleJp.AutoSize = true;
            this.cbTitleJp.Location = new System.Drawing.Point(6, 45);
            this.cbTitleJp.Name = "cbTitleJp";
            this.cbTitleJp.Size = new System.Drawing.Size(15, 14);
            this.cbTitleJp.TabIndex = 16;
            this.cbTitleJp.UseVisualStyleBackColor = true;
            this.cbTitleJp.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // cbTitleEn
            // 
            this.cbTitleEn.AutoSize = true;
            this.cbTitleEn.Location = new System.Drawing.Point(6, 19);
            this.cbTitleEn.Name = "cbTitleEn";
            this.cbTitleEn.Size = new System.Drawing.Size(15, 14);
            this.cbTitleEn.TabIndex = 15;
            this.cbTitleEn.UseVisualStyleBackColor = true;
            this.cbTitleEn.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // lblTrackTitleJp
            // 
            this.lblTrackTitleJp.AutoSize = true;
            this.lblTrackTitleJp.Location = new System.Drawing.Point(25, 45);
            this.lblTrackTitleJp.Name = "lblTrackTitleJp";
            this.lblTrackTitleJp.Size = new System.Drawing.Size(47, 13);
            this.lblTrackTitleJp.TabIndex = 14;
            this.lblTrackTitleJp.Text = "Title (Jp)";
            // 
            // tbTitleJp
            // 
            this.tbTitleJp.Location = new System.Drawing.Point(83, 42);
            this.tbTitleJp.Name = "tbTitleJp";
            this.tbTitleJp.Size = new System.Drawing.Size(214, 20);
            this.tbTitleJp.TabIndex = 13;
            // 
            // lblTrackTitleEn
            // 
            this.lblTrackTitleEn.AutoSize = true;
            this.lblTrackTitleEn.Location = new System.Drawing.Point(25, 19);
            this.lblTrackTitleEn.Name = "lblTrackTitleEn";
            this.lblTrackTitleEn.Size = new System.Drawing.Size(49, 13);
            this.lblTrackTitleEn.TabIndex = 12;
            this.lblTrackTitleEn.Text = "Title (En)";
            // 
            // tbTitleEn
            // 
            this.tbTitleEn.Location = new System.Drawing.Point(83, 16);
            this.tbTitleEn.Name = "tbTitleEn";
            this.tbTitleEn.Size = new System.Drawing.Size(214, 20);
            this.tbTitleEn.TabIndex = 12;
            // 
            // grpSetTags
            // 
            this.grpSetTags.Controls.Add(this.cbRipper);
            this.grpSetTags.Controls.Add(this.cbReleaseDate);
            this.grpSetTags.Controls.Add(this.cbArtistJp);
            this.grpSetTags.Controls.Add(this.cbArtistEn);
            this.grpSetTags.Controls.Add(this.checkboxSystemJp);
            this.grpSetTags.Controls.Add(this.checkboxSystemEn);
            this.grpSetTags.Controls.Add(this.cbGameJp);
            this.grpSetTags.Controls.Add(this.cbGameEnglish);
            this.grpSetTags.Controls.Add(this.cbSystemJp);
            this.grpSetTags.Controls.Add(this.cbSystemEn);
            this.grpSetTags.Controls.Add(this.lblGameDate);
            this.grpSetTags.Controls.Add(this.tbGameDate);
            this.grpSetTags.Controls.Add(this.tbRipper);
            this.grpSetTags.Controls.Add(this.lblRipper);
            this.grpSetTags.Controls.Add(this.lblArtistJp);
            this.grpSetTags.Controls.Add(this.tbArtistJp);
            this.grpSetTags.Controls.Add(this.lblSystemJp);
            this.grpSetTags.Controls.Add(this.lblGameJp);
            this.grpSetTags.Controls.Add(this.tbGameJp);
            this.grpSetTags.Controls.Add(this.lblSystemEn);
            this.grpSetTags.Controls.Add(this.lblArtistEn);
            this.grpSetTags.Controls.Add(this.tbArtistEn);
            this.grpSetTags.Controls.Add(this.lblGameEn);
            this.grpSetTags.Controls.Add(this.tbGameEn);
            this.grpSetTags.Location = new System.Drawing.Point(238, 29);
            this.grpSetTags.Name = "grpSetTags";
            this.grpSetTags.Size = new System.Drawing.Size(310, 202);
            this.grpSetTags.TabIndex = 25;
            this.grpSetTags.TabStop = false;
            this.grpSetTags.Text = "Game Tags";
            // 
            // cbRipper
            // 
            this.cbRipper.AutoSize = true;
            this.cbRipper.Location = new System.Drawing.Point(155, 181);
            this.cbRipper.Name = "cbRipper";
            this.cbRipper.Size = new System.Drawing.Size(15, 14);
            this.cbRipper.TabIndex = 31;
            this.cbRipper.UseVisualStyleBackColor = true;
            this.cbRipper.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // cbReleaseDate
            // 
            this.cbReleaseDate.AutoSize = true;
            this.cbReleaseDate.Location = new System.Drawing.Point(6, 181);
            this.cbReleaseDate.Name = "cbReleaseDate";
            this.cbReleaseDate.Size = new System.Drawing.Size(15, 14);
            this.cbReleaseDate.TabIndex = 30;
            this.cbReleaseDate.UseVisualStyleBackColor = true;
            this.cbReleaseDate.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // cbArtistJp
            // 
            this.cbArtistJp.AutoSize = true;
            this.cbArtistJp.Location = new System.Drawing.Point(6, 155);
            this.cbArtistJp.Name = "cbArtistJp";
            this.cbArtistJp.Size = new System.Drawing.Size(15, 14);
            this.cbArtistJp.TabIndex = 29;
            this.cbArtistJp.UseVisualStyleBackColor = true;
            this.cbArtistJp.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // cbArtistEn
            // 
            this.cbArtistEn.AutoSize = true;
            this.cbArtistEn.Location = new System.Drawing.Point(6, 129);
            this.cbArtistEn.Name = "cbArtistEn";
            this.cbArtistEn.Size = new System.Drawing.Size(15, 14);
            this.cbArtistEn.TabIndex = 28;
            this.cbArtistEn.UseVisualStyleBackColor = true;
            this.cbArtistEn.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // checkboxSystemJp
            // 
            this.checkboxSystemJp.AutoSize = true;
            this.checkboxSystemJp.Location = new System.Drawing.Point(6, 103);
            this.checkboxSystemJp.Name = "checkboxSystemJp";
            this.checkboxSystemJp.Size = new System.Drawing.Size(15, 14);
            this.checkboxSystemJp.TabIndex = 27;
            this.checkboxSystemJp.UseVisualStyleBackColor = true;
            this.checkboxSystemJp.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // checkboxSystemEn
            // 
            this.checkboxSystemEn.AutoSize = true;
            this.checkboxSystemEn.Location = new System.Drawing.Point(6, 77);
            this.checkboxSystemEn.Name = "checkboxSystemEn";
            this.checkboxSystemEn.Size = new System.Drawing.Size(15, 14);
            this.checkboxSystemEn.TabIndex = 26;
            this.checkboxSystemEn.UseVisualStyleBackColor = true;
            this.checkboxSystemEn.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // cbGameJp
            // 
            this.cbGameJp.AutoSize = true;
            this.cbGameJp.Location = new System.Drawing.Point(6, 48);
            this.cbGameJp.Name = "cbGameJp";
            this.cbGameJp.Size = new System.Drawing.Size(15, 14);
            this.cbGameJp.TabIndex = 25;
            this.cbGameJp.UseVisualStyleBackColor = true;
            this.cbGameJp.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // cbGameEnglish
            // 
            this.cbGameEnglish.AutoSize = true;
            this.cbGameEnglish.Location = new System.Drawing.Point(6, 22);
            this.cbGameEnglish.Name = "cbGameEnglish";
            this.cbGameEnglish.Size = new System.Drawing.Size(15, 14);
            this.cbGameEnglish.TabIndex = 24;
            this.cbGameEnglish.UseVisualStyleBackColor = true;
            this.cbGameEnglish.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // cbSystemJp
            // 
            this.cbSystemJp.FormattingEnabled = true;
            this.cbSystemJp.Location = new System.Drawing.Point(94, 100);
            this.cbSystemJp.Name = "cbSystemJp";
            this.cbSystemJp.Size = new System.Drawing.Size(210, 21);
            this.cbSystemJp.TabIndex = 6;
            // 
            // cbSystemEn
            // 
            this.cbSystemEn.FormattingEnabled = true;
            this.cbSystemEn.Location = new System.Drawing.Point(94, 74);
            this.cbSystemEn.Name = "cbSystemEn";
            this.cbSystemEn.Size = new System.Drawing.Size(210, 21);
            this.cbSystemEn.TabIndex = 5;
            // 
            // lblGameDate
            // 
            this.lblGameDate.AutoSize = true;
            this.lblGameDate.Location = new System.Drawing.Point(25, 181);
            this.lblGameDate.Name = "lblGameDate";
            this.lblGameDate.Size = new System.Drawing.Size(51, 13);
            this.lblGameDate.TabIndex = 23;
            this.lblGameDate.Text = "Rls. Date";
            // 
            // tbGameDate
            // 
            this.tbGameDate.Location = new System.Drawing.Point(82, 178);
            this.tbGameDate.Name = "tbGameDate";
            this.tbGameDate.Size = new System.Drawing.Size(67, 20);
            this.tbGameDate.TabIndex = 9;
            // 
            // tbRipper
            // 
            this.tbRipper.Location = new System.Drawing.Point(215, 178);
            this.tbRipper.Name = "tbRipper";
            this.tbRipper.Size = new System.Drawing.Size(89, 20);
            this.tbRipper.TabIndex = 10;
            // 
            // lblRipper
            // 
            this.lblRipper.AutoSize = true;
            this.lblRipper.Location = new System.Drawing.Point(171, 181);
            this.lblRipper.Name = "lblRipper";
            this.lblRipper.Size = new System.Drawing.Size(38, 13);
            this.lblRipper.TabIndex = 20;
            this.lblRipper.Text = "Ripper";
            // 
            // lblArtistJp
            // 
            this.lblArtistJp.AutoSize = true;
            this.lblArtistJp.Location = new System.Drawing.Point(25, 155);
            this.lblArtistJp.Name = "lblArtistJp";
            this.lblArtistJp.Size = new System.Drawing.Size(50, 13);
            this.lblArtistJp.TabIndex = 18;
            this.lblArtistJp.Text = "Artist (Jp)";
            // 
            // tbArtistJp
            // 
            this.tbArtistJp.Location = new System.Drawing.Point(82, 152);
            this.tbArtistJp.Name = "tbArtistJp";
            this.tbArtistJp.Size = new System.Drawing.Size(222, 20);
            this.tbArtistJp.TabIndex = 8;
            // 
            // lblSystemJp
            // 
            this.lblSystemJp.AutoSize = true;
            this.lblSystemJp.Location = new System.Drawing.Point(25, 103);
            this.lblSystemJp.Name = "lblSystemJp";
            this.lblSystemJp.Size = new System.Drawing.Size(61, 13);
            this.lblSystemJp.TabIndex = 16;
            this.lblSystemJp.Text = "System (Jp)";
            // 
            // lblGameJp
            // 
            this.lblGameJp.AutoSize = true;
            this.lblGameJp.Location = new System.Drawing.Point(25, 48);
            this.lblGameJp.Name = "lblGameJp";
            this.lblGameJp.Size = new System.Drawing.Size(55, 13);
            this.lblGameJp.TabIndex = 14;
            this.lblGameJp.Text = "Game (Jp)";
            // 
            // tbGameJp
            // 
            this.tbGameJp.Location = new System.Drawing.Point(82, 45);
            this.tbGameJp.Name = "tbGameJp";
            this.tbGameJp.Size = new System.Drawing.Size(222, 20);
            this.tbGameJp.TabIndex = 4;
            // 
            // lblSystemEn
            // 
            this.lblSystemEn.AutoSize = true;
            this.lblSystemEn.Location = new System.Drawing.Point(25, 77);
            this.lblSystemEn.Name = "lblSystemEn";
            this.lblSystemEn.Size = new System.Drawing.Size(63, 13);
            this.lblSystemEn.TabIndex = 12;
            this.lblSystemEn.Text = "System (En)";
            // 
            // lblArtistEn
            // 
            this.lblArtistEn.AutoSize = true;
            this.lblArtistEn.Location = new System.Drawing.Point(25, 129);
            this.lblArtistEn.Name = "lblArtistEn";
            this.lblArtistEn.Size = new System.Drawing.Size(52, 13);
            this.lblArtistEn.TabIndex = 2;
            this.lblArtistEn.Text = "Artist (En)";
            // 
            // tbArtistEn
            // 
            this.tbArtistEn.Location = new System.Drawing.Point(82, 126);
            this.tbArtistEn.Name = "tbArtistEn";
            this.tbArtistEn.Size = new System.Drawing.Size(222, 20);
            this.tbArtistEn.TabIndex = 7;
            // 
            // lblGameEn
            // 
            this.lblGameEn.AutoSize = true;
            this.lblGameEn.Location = new System.Drawing.Point(25, 22);
            this.lblGameEn.Name = "lblGameEn";
            this.lblGameEn.Size = new System.Drawing.Size(57, 13);
            this.lblGameEn.TabIndex = 0;
            this.lblGameEn.Text = "Game (En)";
            // 
            // tbGameEn
            // 
            this.tbGameEn.Location = new System.Drawing.Point(82, 19);
            this.tbGameEn.Name = "tbGameEn";
            this.tbGameEn.Size = new System.Drawing.Size(222, 20);
            this.tbGameEn.TabIndex = 3;
            // 
            // grpSourceFiles
            // 
            this.grpSourceFiles.Controls.Add(this.tbSourceDirectory);
            this.grpSourceFiles.Controls.Add(this.lbFiles);
            this.grpSourceFiles.Controls.Add(this.btnBrowseDirectory);
            this.grpSourceFiles.Location = new System.Drawing.Point(3, 29);
            this.grpSourceFiles.Name = "grpSourceFiles";
            this.grpSourceFiles.Size = new System.Drawing.Size(229, 352);
            this.grpSourceFiles.TabIndex = 24;
            this.grpSourceFiles.TabStop = false;
            this.grpSourceFiles.Text = "Source Files";
            // 
            // tbSourceDirectory
            // 
            this.tbSourceDirectory.Location = new System.Drawing.Point(6, 16);
            this.tbSourceDirectory.Name = "tbSourceDirectory";
            this.tbSourceDirectory.Size = new System.Drawing.Size(183, 20);
            this.tbSourceDirectory.TabIndex = 1;
            this.tbSourceDirectory.TextChanged += new System.EventHandler(this.tbSourceDirectory_TextChanged);
            // 
            // lbFiles
            // 
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.HorizontalScrollbar = true;
            this.lbFiles.Location = new System.Drawing.Point(6, 43);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFiles.Size = new System.Drawing.Size(215, 303);
            this.lbFiles.TabIndex = 5;
            this.lbFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbFiles_MouseUp);
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            // 
            // btnBrowseDirectory
            // 
            this.btnBrowseDirectory.Location = new System.Drawing.Point(195, 16);
            this.btnBrowseDirectory.Name = "btnBrowseDirectory";
            this.btnBrowseDirectory.Size = new System.Drawing.Size(26, 20);
            this.btnBrowseDirectory.TabIndex = 2;
            this.btnBrowseDirectory.Text = "...";
            this.btnBrowseDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseDirectory.Click += new System.EventHandler(this.btnBrowseDirectory_Click);
            // 
            // grpComments
            // 
            this.grpComments.Controls.Add(this.cbComments);
            this.grpComments.Controls.Add(this.tbComments);
            this.grpComments.Location = new System.Drawing.Point(238, 311);
            this.grpComments.Name = "grpComments";
            this.grpComments.Size = new System.Drawing.Size(310, 70);
            this.grpComments.TabIndex = 28;
            this.grpComments.TabStop = false;
            this.grpComments.Text = "Comments";
            // 
            // cbComments
            // 
            this.cbComments.AutoSize = true;
            this.cbComments.Location = new System.Drawing.Point(6, 30);
            this.cbComments.Name = "cbComments";
            this.cbComments.Size = new System.Drawing.Size(15, 14);
            this.cbComments.TabIndex = 15;
            this.cbComments.UseVisualStyleBackColor = true;
            this.cbComments.CheckedChanged += new System.EventHandler(this.doChecksChange_CheckedChanged);
            // 
            // tbComments
            // 
            this.tbComments.AcceptsReturn = true;
            this.tbComments.Location = new System.Drawing.Point(25, 15);
            this.tbComments.Multiline = true;
            this.tbComments.Name = "tbComments";
            this.tbComments.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbComments.Size = new System.Drawing.Size(272, 49);
            this.tbComments.TabIndex = 14;
            // 
            // contextMenuRefresh
            // 
            this.contextMenuRefresh.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRefresh,
            this.clearFileListToolStripMenuItem});
            this.contextMenuRefresh.Name = "contextMenuRefresh";
            this.contextMenuRefresh.Size = new System.Drawing.Size(151, 48);
            // 
            // tsmRefresh
            // 
            this.tsmRefresh.Name = "tsmRefresh";
            this.tsmRefresh.Size = new System.Drawing.Size(150, 22);
            this.tsmRefresh.Text = "Refresh File List";
            this.tsmRefresh.Click += new System.EventHandler(this.tsmRefresh_Click);
            // 
            // clearFileListToolStripMenuItem
            // 
            this.clearFileListToolStripMenuItem.Name = "clearFileListToolStripMenuItem";
            this.clearFileListToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.clearFileListToolStripMenuItem.Text = "Clear File List";
            this.clearFileListToolStripMenuItem.Click += new System.EventHandler(this.clearFileListToolStripMenuItem_Click);
            // 
            // Vgm_VgmTagEditorForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 578);
            this.Controls.Add(this.grpComments);
            this.Controls.Add(this.grpTrackTags);
            this.Controls.Add(this.grpSetTags);
            this.Controls.Add(this.grpSourceFiles);
            this.Name = "Vgm_VgmTagEditorForm";
            this.Text = "Vgm_VgmTagEditorForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Vgm_VgmTagEditorForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Vgm_VgmTagEditorForm_DragEnter);
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSourceFiles, 0);
            this.Controls.SetChildIndex(this.grpSetTags, 0);
            this.Controls.SetChildIndex(this.grpTrackTags, 0);
            this.Controls.SetChildIndex(this.grpComments, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpTrackTags.ResumeLayout(false);
            this.grpTrackTags.PerformLayout();
            this.grpSetTags.ResumeLayout(false);
            this.grpSetTags.PerformLayout();
            this.grpSourceFiles.ResumeLayout(false);
            this.grpSourceFiles.PerformLayout();
            this.grpComments.ResumeLayout(false);
            this.grpComments.PerformLayout();
            this.contextMenuRefresh.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpTrackTags;
        private System.Windows.Forms.Label lblTrackTitleEn;
        private System.Windows.Forms.TextBox tbTitleEn;
        private System.Windows.Forms.GroupBox grpSetTags;
        private System.Windows.Forms.Label lblSystemEn;
        private System.Windows.Forms.Label lblArtistEn;
        private System.Windows.Forms.TextBox tbArtistEn;
        private System.Windows.Forms.Label lblGameEn;
        private System.Windows.Forms.TextBox tbGameEn;
        private System.Windows.Forms.GroupBox grpSourceFiles;
        private System.Windows.Forms.TextBox tbSourceDirectory;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Button btnBrowseDirectory;
        private System.Windows.Forms.Label lblTrackTitleJp;
        private System.Windows.Forms.TextBox tbTitleJp;
        private System.Windows.Forms.Label lblGameJp;
        private System.Windows.Forms.TextBox tbGameJp;
        private System.Windows.Forms.Label lblSystemJp;
        private System.Windows.Forms.Label lblArtistJp;
        private System.Windows.Forms.TextBox tbArtistJp;
        private System.Windows.Forms.Label lblRipper;
        private System.Windows.Forms.TextBox tbRipper;
        private System.Windows.Forms.Label lblGameDate;
        private System.Windows.Forms.TextBox tbGameDate;
        private System.Windows.Forms.GroupBox grpComments;
        private System.Windows.Forms.TextBox tbComments;
        private System.Windows.Forms.ContextMenuStrip contextMenuRefresh;
        private System.Windows.Forms.ToolStripMenuItem tsmRefresh;
        private System.Windows.Forms.ComboBox cbSystemEn;
        private System.Windows.Forms.ComboBox cbSystemJp;
        private System.Windows.Forms.ToolStripMenuItem clearFileListToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkboxSystemEn;
        private System.Windows.Forms.CheckBox cbGameJp;
        private System.Windows.Forms.CheckBox cbGameEnglish;
        private System.Windows.Forms.CheckBox checkboxSystemJp;
        private System.Windows.Forms.CheckBox cbRipper;
        private System.Windows.Forms.CheckBox cbReleaseDate;
        private System.Windows.Forms.CheckBox cbArtistJp;
        private System.Windows.Forms.CheckBox cbArtistEn;
        private System.Windows.Forms.CheckBox cbTitleJp;
        private System.Windows.Forms.CheckBox cbTitleEn;
        private System.Windows.Forms.CheckBox cbComments;
    }
}