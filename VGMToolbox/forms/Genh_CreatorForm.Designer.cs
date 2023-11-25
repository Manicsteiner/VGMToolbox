namespace VGMToolbox.forms
{
    partial class Genh_CreatorForm
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
            this.grpFormat = new System.Windows.Forms.GroupBox();
            this.comboFormat = new System.Windows.Forms.ComboBox();
            this.cbHeaderOnly = new System.Windows.Forms.CheckBox();
            this.rbCreate = new System.Windows.Forms.RadioButton();
            this.rbEdit = new System.Windows.Forms.RadioButton();
            this.rbExtract = new System.Windows.Forms.RadioButton();
            this.contextMenuRefresh = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshFileListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearFileListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuBytesToSamples = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bytesToSamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.grpCoefOptions = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCoefficientType = new System.Windows.Forms.ComboBox();
            this.lblRightCoef = new System.Windows.Forms.Label();
            this.tbLeftCoef = new System.Windows.Forms.TextBox();
            this.lblLeftCoef = new System.Windows.Forms.Label();
            this.tbRightCoef = new System.Windows.Forms.TextBox();
            this.grpXmaOptions = new System.Windows.Forms.GroupBox();
            this.cbXmaStreamMode = new System.Windows.Forms.ComboBox();
            this.lblXmaStreamMode = new System.Windows.Forms.Label();
            this.grpAtracOptions = new System.Windows.Forms.GroupBox();
            this.lblAtrac3StereoMode = new System.Windows.Forms.Label();
            this.cbAtrac3StereoMode = new System.Windows.Forms.ComboBox();
            this.grpSkipSamples = new System.Windows.Forms.GroupBox();
            this.tbForceSkipSamplesNumber = new System.Windows.Forms.TextBox();
            this.cbForceSkipSamples = new System.Windows.Forms.CheckBox();
            this.grpLoopOptions = new System.Windows.Forms.GroupBox();
            this.cbTotalSamplesBytesToSamples = new System.Windows.Forms.CheckBox();
            this.totalSamplesOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.cbUseTotalSamplesOffset = new System.Windows.Forms.CheckBox();
            this.lblTotalSamples = new System.Windows.Forms.Label();
            this.tbTotalSamples = new System.Windows.Forms.TextBox();
            this.cbLoopEndBytesToSamples = new System.Windows.Forms.CheckBox();
            this.cbLoopStartBytesToSamples = new System.Windows.Forms.CheckBox();
            this.lblLoopStart = new System.Windows.Forms.Label();
            this.cbUseLoopEndOffset = new System.Windows.Forms.CheckBox();
            this.lblLoopEnd = new System.Windows.Forms.Label();
            this.loopEndOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.tbLoopEnd = new System.Windows.Forms.TextBox();
            this.cbUseLoopStartOffset = new System.Windows.Forms.CheckBox();
            this.tbLoopStart = new System.Windows.Forms.TextBox();
            this.loopStartOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.cbManualEntry = new System.Windows.Forms.RadioButton();
            this.cbFindLoop = new System.Windows.Forms.RadioButton();
            this.cbNoLoops = new System.Windows.Forms.RadioButton();
            this.cbLoopFileEnd = new System.Windows.Forms.RadioButton();
            this.grpGeneralOptions = new System.Windows.Forms.GroupBox();
            this.tbRawDataSize = new System.Windows.Forms.TextBox();
            this.lblRawDataSize = new System.Windows.Forms.Label();
            this.cbUseInterleaveOffset = new System.Windows.Forms.CheckBox();
            this.interleaveOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.channelsOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.cbUseChannelsOffset = new System.Windows.Forms.CheckBox();
            this.lblHeaderSkip = new System.Windows.Forms.Label();
            this.cbHeaderSkip = new System.Windows.Forms.ComboBox();
            this.lblInterleave = new System.Windows.Forms.Label();
            this.cbInterleave = new System.Windows.Forms.ComboBox();
            this.cbChannels = new System.Windows.Forms.ComboBox();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.cbUseFrequencyOffset = new System.Windows.Forms.CheckBox();
            this.cbFrequency = new System.Windows.Forms.ComboBox();
            this.frequencyOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.lblChannels = new System.Windows.Forms.Label();
            this.grpFunction = new System.Windows.Forms.GroupBox();
            this.lblFilenameFilter = new System.Windows.Forms.Label();
            this.tbFilenameFilter = new System.Windows.Forms.TextBox();
            this.tbSourceDirectory = new System.Windows.Forms.TextBox();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.btnBrowseDirectory = new System.Windows.Forms.Button();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpFormat.SuspendLayout();
            this.contextMenuRefresh.SuspendLayout();
            this.contextMenuBytesToSamples.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.pnlOptions.SuspendLayout();
            this.grpCoefOptions.SuspendLayout();
            this.grpXmaOptions.SuspendLayout();
            this.grpAtracOptions.SuspendLayout();
            this.grpSkipSamples.SuspendLayout();
            this.grpLoopOptions.SuspendLayout();
            this.grpGeneralOptions.SuspendLayout();
            this.grpFunction.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 498);
            this.pnlLabels.Size = new System.Drawing.Size(948, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(948, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 421);
            this.tbOutput.Size = new System.Drawing.Size(948, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 401);
            this.pnlButtons.Size = new System.Drawing.Size(948, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(888, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(828, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpFormat
            // 
            this.grpFormat.Controls.Add(this.comboFormat);
            this.grpFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFormat.Location = new System.Drawing.Point(0, 0);
            this.grpFormat.Name = "grpFormat";
            this.grpFormat.Size = new System.Drawing.Size(925, 41);
            this.grpFormat.TabIndex = 7;
            this.grpFormat.TabStop = false;
            this.grpFormat.Text = "Format";
            // 
            // comboFormat
            // 
            this.comboFormat.FormattingEnabled = true;
            this.comboFormat.Location = new System.Drawing.Point(6, 14);
            this.comboFormat.Name = "comboFormat";
            this.comboFormat.Size = new System.Drawing.Size(527, 21);
            this.comboFormat.TabIndex = 0;
            this.comboFormat.SelectedValueChanged += new System.EventHandler(this.comboFormat_SelectedValueChanged);
            this.comboFormat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboFormat_KeyDown);
            this.comboFormat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboFormat_KeyPress);
            // 
            // cbHeaderOnly
            // 
            this.cbHeaderOnly.AutoSize = true;
            this.cbHeaderOnly.Location = new System.Drawing.Point(10, 40);
            this.cbHeaderOnly.Name = "cbHeaderOnly";
            this.cbHeaderOnly.Size = new System.Drawing.Size(117, 17);
            this.cbHeaderOnly.TabIndex = 0;
            this.cbHeaderOnly.Text = "Ouput Header Only";
            this.cbHeaderOnly.UseVisualStyleBackColor = true;
            // 
            // rbCreate
            // 
            this.rbCreate.AutoSize = true;
            this.rbCreate.Location = new System.Drawing.Point(3, 17);
            this.rbCreate.Name = "rbCreate";
            this.rbCreate.Size = new System.Drawing.Size(56, 17);
            this.rbCreate.TabIndex = 9;
            this.rbCreate.TabStop = true;
            this.rbCreate.Text = "Create";
            this.rbCreate.UseVisualStyleBackColor = true;
            this.rbCreate.CheckedChanged += new System.EventHandler(this.rbCreate_CheckedChanged);
            // 
            // rbEdit
            // 
            this.rbEdit.AutoSize = true;
            this.rbEdit.Location = new System.Drawing.Point(3, 86);
            this.rbEdit.Name = "rbEdit";
            this.rbEdit.Size = new System.Drawing.Size(43, 17);
            this.rbEdit.TabIndex = 10;
            this.rbEdit.TabStop = true;
            this.rbEdit.Text = "Edit";
            this.rbEdit.UseVisualStyleBackColor = true;
            this.rbEdit.CheckedChanged += new System.EventHandler(this.rbEdit_CheckedChanged);
            // 
            // rbExtract
            // 
            this.rbExtract.AutoSize = true;
            this.rbExtract.Location = new System.Drawing.Point(3, 63);
            this.rbExtract.Name = "rbExtract";
            this.rbExtract.Size = new System.Drawing.Size(58, 17);
            this.rbExtract.TabIndex = 11;
            this.rbExtract.TabStop = true;
            this.rbExtract.Text = "Extract";
            this.rbExtract.UseVisualStyleBackColor = true;
            this.rbExtract.CheckedChanged += new System.EventHandler(this.rbExtract_CheckedChanged);
            // 
            // contextMenuRefresh
            // 
            this.contextMenuRefresh.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshFileListToolStripMenuItem,
            this.clearFileListToolStripMenuItem});
            this.contextMenuRefresh.Name = "contextMenuRefresh";
            this.contextMenuRefresh.Size = new System.Drawing.Size(156, 48);
            // 
            // refreshFileListToolStripMenuItem
            // 
            this.refreshFileListToolStripMenuItem.Name = "refreshFileListToolStripMenuItem";
            this.refreshFileListToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.refreshFileListToolStripMenuItem.Text = "Refresh File List";
            this.refreshFileListToolStripMenuItem.Click += new System.EventHandler(this.refreshFileListToolStripMenuItem_Click);
            // 
            // clearFileListToolStripMenuItem
            // 
            this.clearFileListToolStripMenuItem.Name = "clearFileListToolStripMenuItem";
            this.clearFileListToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.clearFileListToolStripMenuItem.Text = "Clear File List";
            this.clearFileListToolStripMenuItem.Click += new System.EventHandler(this.clearFileListToolStripMenuItem_Click);
            // 
            // contextMenuBytesToSamples
            // 
            this.contextMenuBytesToSamples.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bytesToSamplesToolStripMenuItem});
            this.contextMenuBytesToSamples.Name = "contextMenuBytesToSamples";
            this.contextMenuBytesToSamples.Size = new System.Drawing.Size(164, 26);
            // 
            // bytesToSamplesToolStripMenuItem
            // 
            this.bytesToSamplesToolStripMenuItem.Name = "bytesToSamplesToolStripMenuItem";
            this.bytesToSamplesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.bytesToSamplesToolStripMenuItem.Text = "Bytes to Samples";
            this.bytesToSamplesToolStripMenuItem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bytesToSamplesToolStripMenuItem_MouseUp);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.pnlOptions);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpOptions.Location = new System.Drawing.Point(0, 178);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(948, 223);
            this.grpOptions.TabIndex = 12;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // pnlOptions
            // 
            this.pnlOptions.AutoScroll = true;
            this.pnlOptions.AutoSize = true;
            this.pnlOptions.Controls.Add(this.grpCoefOptions);
            this.pnlOptions.Controls.Add(this.grpXmaOptions);
            this.pnlOptions.Controls.Add(this.grpAtracOptions);
            this.pnlOptions.Controls.Add(this.grpSkipSamples);
            this.pnlOptions.Controls.Add(this.grpLoopOptions);
            this.pnlOptions.Controls.Add(this.grpGeneralOptions);
            this.pnlOptions.Controls.Add(this.grpFormat);
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOptions.Location = new System.Drawing.Point(3, 16);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(942, 204);
            this.pnlOptions.TabIndex = 0;
            // 
            // grpCoefOptions
            // 
            this.grpCoefOptions.Controls.Add(this.label1);
            this.grpCoefOptions.Controls.Add(this.cbCoefficientType);
            this.grpCoefOptions.Controls.Add(this.lblRightCoef);
            this.grpCoefOptions.Controls.Add(this.tbLeftCoef);
            this.grpCoefOptions.Controls.Add(this.lblLeftCoef);
            this.grpCoefOptions.Controls.Add(this.tbRightCoef);
            this.grpCoefOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCoefOptions.Location = new System.Drawing.Point(0, 594);
            this.grpCoefOptions.Name = "grpCoefOptions";
            this.grpCoefOptions.Size = new System.Drawing.Size(925, 67);
            this.grpCoefOptions.TabIndex = 37;
            this.grpCoefOptions.TabStop = false;
            this.grpCoefOptions.Text = "Coefficients";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Coefficient Type";
            // 
            // cbCoefficientType
            // 
            this.cbCoefficientType.FormattingEnabled = true;
            this.cbCoefficientType.Location = new System.Drawing.Point(341, 18);
            this.cbCoefficientType.Name = "cbCoefficientType";
            this.cbCoefficientType.Size = new System.Drawing.Size(121, 21);
            this.cbCoefficientType.TabIndex = 19;
            // 
            // lblRightCoef
            // 
            this.lblRightCoef.AutoSize = true;
            this.lblRightCoef.Location = new System.Drawing.Point(6, 22);
            this.lblRightCoef.Name = "lblRightCoef";
            this.lblRightCoef.Size = new System.Drawing.Size(133, 13);
            this.lblRightCoef.TabIndex = 15;
            this.lblRightCoef.Text = "Coef Offset: Right Channel";
            // 
            // tbLeftCoef
            // 
            this.tbLeftCoef.Location = new System.Drawing.Point(145, 45);
            this.tbLeftCoef.Name = "tbLeftCoef";
            this.tbLeftCoef.Size = new System.Drawing.Size(100, 20);
            this.tbLeftCoef.TabIndex = 16;
            // 
            // lblLeftCoef
            // 
            this.lblLeftCoef.AutoSize = true;
            this.lblLeftCoef.Location = new System.Drawing.Point(6, 48);
            this.lblLeftCoef.Name = "lblLeftCoef";
            this.lblLeftCoef.Size = new System.Drawing.Size(126, 13);
            this.lblLeftCoef.TabIndex = 17;
            this.lblLeftCoef.Text = "Coef Offset: Left Channel";
            // 
            // tbRightCoef
            // 
            this.tbRightCoef.Location = new System.Drawing.Point(145, 19);
            this.tbRightCoef.Name = "tbRightCoef";
            this.tbRightCoef.Size = new System.Drawing.Size(100, 20);
            this.tbRightCoef.TabIndex = 14;
            // 
            // grpXmaOptions
            // 
            this.grpXmaOptions.Controls.Add(this.cbXmaStreamMode);
            this.grpXmaOptions.Controls.Add(this.lblXmaStreamMode);
            this.grpXmaOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpXmaOptions.Location = new System.Drawing.Point(0, 540);
            this.grpXmaOptions.Name = "grpXmaOptions";
            this.grpXmaOptions.Size = new System.Drawing.Size(925, 54);
            this.grpXmaOptions.TabIndex = 39;
            this.grpXmaOptions.TabStop = false;
            this.grpXmaOptions.Text = "XMA Options";
            // 
            // cbXmaStreamMode
            // 
            this.cbXmaStreamMode.FormattingEnabled = true;
            this.cbXmaStreamMode.Location = new System.Drawing.Point(90, 17);
            this.cbXmaStreamMode.Name = "cbXmaStreamMode";
            this.cbXmaStreamMode.Size = new System.Drawing.Size(102, 21);
            this.cbXmaStreamMode.TabIndex = 1;
            // 
            // lblXmaStreamMode
            // 
            this.lblXmaStreamMode.AutoSize = true;
            this.lblXmaStreamMode.Location = new System.Drawing.Point(15, 20);
            this.lblXmaStreamMode.Name = "lblXmaStreamMode";
            this.lblXmaStreamMode.Size = new System.Drawing.Size(70, 13);
            this.lblXmaStreamMode.TabIndex = 0;
            this.lblXmaStreamMode.Text = "Stream Mode";
            // 
            // grpAtracOptions
            // 
            this.grpAtracOptions.Controls.Add(this.lblAtrac3StereoMode);
            this.grpAtracOptions.Controls.Add(this.cbAtrac3StereoMode);
            this.grpAtracOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpAtracOptions.Location = new System.Drawing.Point(0, 492);
            this.grpAtracOptions.Name = "grpAtracOptions";
            this.grpAtracOptions.Size = new System.Drawing.Size(925, 48);
            this.grpAtracOptions.TabIndex = 38;
            this.grpAtracOptions.TabStop = false;
            this.grpAtracOptions.Text = "ATRAC3 Options";
            // 
            // lblAtrac3StereoMode
            // 
            this.lblAtrac3StereoMode.AutoSize = true;
            this.lblAtrac3StereoMode.Location = new System.Drawing.Point(12, 16);
            this.lblAtrac3StereoMode.Name = "lblAtrac3StereoMode";
            this.lblAtrac3StereoMode.Size = new System.Drawing.Size(68, 13);
            this.lblAtrac3StereoMode.TabIndex = 1;
            this.lblAtrac3StereoMode.Text = "Stereo Mode";
            // 
            // cbAtrac3StereoMode
            // 
            this.cbAtrac3StereoMode.FormattingEnabled = true;
            this.cbAtrac3StereoMode.Location = new System.Drawing.Point(90, 13);
            this.cbAtrac3StereoMode.Name = "cbAtrac3StereoMode";
            this.cbAtrac3StereoMode.Size = new System.Drawing.Size(102, 21);
            this.cbAtrac3StereoMode.TabIndex = 0;
            // 
            // grpSkipSamples
            // 
            this.grpSkipSamples.Controls.Add(this.tbForceSkipSamplesNumber);
            this.grpSkipSamples.Controls.Add(this.cbForceSkipSamples);
            this.grpSkipSamples.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSkipSamples.Location = new System.Drawing.Point(0, 447);
            this.grpSkipSamples.Name = "grpSkipSamples";
            this.grpSkipSamples.Size = new System.Drawing.Size(925, 45);
            this.grpSkipSamples.TabIndex = 21;
            this.grpSkipSamples.TabStop = false;
            this.grpSkipSamples.Text = "Skip Samples";
            // 
            // tbForceSkipSamplesNumber
            // 
            this.tbForceSkipSamplesNumber.Location = new System.Drawing.Point(139, 17);
            this.tbForceSkipSamplesNumber.Name = "tbForceSkipSamplesNumber";
            this.tbForceSkipSamplesNumber.Size = new System.Drawing.Size(100, 20);
            this.tbForceSkipSamplesNumber.TabIndex = 1;
            // 
            // cbForceSkipSamples
            // 
            this.cbForceSkipSamples.AutoSize = true;
            this.cbForceSkipSamples.Location = new System.Drawing.Point(14, 19);
            this.cbForceSkipSamples.Name = "cbForceSkipSamples";
            this.cbForceSkipSamples.Size = new System.Drawing.Size(120, 17);
            this.cbForceSkipSamples.TabIndex = 0;
            this.cbForceSkipSamples.Text = "Force Skip Samples";
            this.cbForceSkipSamples.UseVisualStyleBackColor = true;
            this.cbForceSkipSamples.CheckedChanged += new System.EventHandler(this.cbForceSkipSamples_CheckedChanged);
            // 
            // grpLoopOptions
            // 
            this.grpLoopOptions.Controls.Add(this.cbTotalSamplesBytesToSamples);
            this.grpLoopOptions.Controls.Add(this.totalSamplesOffsetDescription);
            this.grpLoopOptions.Controls.Add(this.cbUseTotalSamplesOffset);
            this.grpLoopOptions.Controls.Add(this.lblTotalSamples);
            this.grpLoopOptions.Controls.Add(this.tbTotalSamples);
            this.grpLoopOptions.Controls.Add(this.cbLoopEndBytesToSamples);
            this.grpLoopOptions.Controls.Add(this.cbLoopStartBytesToSamples);
            this.grpLoopOptions.Controls.Add(this.lblLoopStart);
            this.grpLoopOptions.Controls.Add(this.cbUseLoopEndOffset);
            this.grpLoopOptions.Controls.Add(this.lblLoopEnd);
            this.grpLoopOptions.Controls.Add(this.loopEndOffsetDescription);
            this.grpLoopOptions.Controls.Add(this.tbLoopEnd);
            this.grpLoopOptions.Controls.Add(this.cbUseLoopStartOffset);
            this.grpLoopOptions.Controls.Add(this.tbLoopStart);
            this.grpLoopOptions.Controls.Add(this.loopStartOffsetDescription);
            this.grpLoopOptions.Controls.Add(this.cbManualEntry);
            this.grpLoopOptions.Controls.Add(this.cbFindLoop);
            this.grpLoopOptions.Controls.Add(this.cbNoLoops);
            this.grpLoopOptions.Controls.Add(this.cbLoopFileEnd);
            this.grpLoopOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLoopOptions.Location = new System.Drawing.Point(0, 254);
            this.grpLoopOptions.Name = "grpLoopOptions";
            this.grpLoopOptions.Size = new System.Drawing.Size(925, 193);
            this.grpLoopOptions.TabIndex = 36;
            this.grpLoopOptions.TabStop = false;
            this.grpLoopOptions.Text = "Looping";
            // 
            // cbTotalSamplesBytesToSamples
            // 
            this.cbTotalSamplesBytesToSamples.AutoSize = true;
            this.cbTotalSamplesBytesToSamples.Location = new System.Drawing.Point(486, 164);
            this.cbTotalSamplesBytesToSamples.Name = "cbTotalSamplesBytesToSamples";
            this.cbTotalSamplesBytesToSamples.Size = new System.Drawing.Size(107, 17);
            this.cbTotalSamplesBytesToSamples.TabIndex = 41;
            this.cbTotalSamplesBytesToSamples.Text = "Bytes to Samples";
            this.cbTotalSamplesBytesToSamples.UseVisualStyleBackColor = true;
            this.cbTotalSamplesBytesToSamples.CheckedChanged += new System.EventHandler(this.cbTotalSamplesBytesToSamples_CheckedChanged);
            // 
            // totalSamplesOffsetDescription
            // 
            this.totalSamplesOffsetDescription.Location = new System.Drawing.Point(108, 159);
            this.totalSamplesOffsetDescription.Name = "totalSamplesOffsetDescription";
            this.totalSamplesOffsetDescription.OffsetByteOrder = "Little Endian";
            this.totalSamplesOffsetDescription.OffsetSize = "4";
            this.totalSamplesOffsetDescription.OffsetValue = "";
            this.totalSamplesOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.totalSamplesOffsetDescription.TabIndex = 40;
            // 
            // cbUseTotalSamplesOffset
            // 
            this.cbUseTotalSamplesOffset.AutoSize = true;
            this.cbUseTotalSamplesOffset.Location = new System.Drawing.Point(26, 164);
            this.cbUseTotalSamplesOffset.Name = "cbUseTotalSamplesOffset";
            this.cbUseTotalSamplesOffset.Size = new System.Drawing.Size(76, 17);
            this.cbUseTotalSamplesOffset.TabIndex = 39;
            this.cbUseTotalSamplesOffset.Text = "Use Offset";
            this.cbUseTotalSamplesOffset.UseVisualStyleBackColor = true;
            this.cbUseTotalSamplesOffset.CheckedChanged += new System.EventHandler(this.cbUseTotalSamplesOffset_CheckedChanged);
            // 
            // lblTotalSamples
            // 
            this.lblTotalSamples.AutoSize = true;
            this.lblTotalSamples.Location = new System.Drawing.Point(12, 141);
            this.lblTotalSamples.Name = "lblTotalSamples";
            this.lblTotalSamples.Size = new System.Drawing.Size(74, 13);
            this.lblTotalSamples.TabIndex = 38;
            this.lblTotalSamples.Text = "Total Samples";
            this.lblTotalSamples.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTotalSamples_MouseUp);
            // 
            // tbTotalSamples
            // 
            this.tbTotalSamples.Location = new System.Drawing.Point(122, 138);
            this.tbTotalSamples.Name = "tbTotalSamples";
            this.tbTotalSamples.Size = new System.Drawing.Size(100, 20);
            this.tbTotalSamples.TabIndex = 37;
            // 
            // cbLoopEndBytesToSamples
            // 
            this.cbLoopEndBytesToSamples.AutoSize = true;
            this.cbLoopEndBytesToSamples.Location = new System.Drawing.Point(486, 88);
            this.cbLoopEndBytesToSamples.Name = "cbLoopEndBytesToSamples";
            this.cbLoopEndBytesToSamples.Size = new System.Drawing.Size(107, 17);
            this.cbLoopEndBytesToSamples.TabIndex = 36;
            this.cbLoopEndBytesToSamples.Text = "Bytes to Samples";
            this.cbLoopEndBytesToSamples.UseVisualStyleBackColor = true;
            this.cbLoopEndBytesToSamples.CheckedChanged += new System.EventHandler(this.cbLoopEndBytesToSamples_CheckedChanged);
            // 
            // cbLoopStartBytesToSamples
            // 
            this.cbLoopStartBytesToSamples.AutoSize = true;
            this.cbLoopStartBytesToSamples.Location = new System.Drawing.Point(486, 40);
            this.cbLoopStartBytesToSamples.Name = "cbLoopStartBytesToSamples";
            this.cbLoopStartBytesToSamples.Size = new System.Drawing.Size(107, 17);
            this.cbLoopStartBytesToSamples.TabIndex = 35;
            this.cbLoopStartBytesToSamples.Text = "Bytes to Samples";
            this.cbLoopStartBytesToSamples.UseVisualStyleBackColor = true;
            this.cbLoopStartBytesToSamples.CheckedChanged += new System.EventHandler(this.cbLoopStartBytesToSamples_CheckedChanged);
            // 
            // lblLoopStart
            // 
            this.lblLoopStart.AutoSize = true;
            this.lblLoopStart.Location = new System.Drawing.Point(12, 16);
            this.lblLoopStart.Name = "lblLoopStart";
            this.lblLoopStart.Size = new System.Drawing.Size(105, 13);
            this.lblLoopStart.TabIndex = 9;
            this.lblLoopStart.Text = "Loop Start (Samples)";
            this.lblLoopStart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblLoopStart_MouseUp);
            // 
            // cbUseLoopEndOffset
            // 
            this.cbUseLoopEndOffset.AutoSize = true;
            this.cbUseLoopEndOffset.Location = new System.Drawing.Point(26, 88);
            this.cbUseLoopEndOffset.Name = "cbUseLoopEndOffset";
            this.cbUseLoopEndOffset.Size = new System.Drawing.Size(76, 17);
            this.cbUseLoopEndOffset.TabIndex = 34;
            this.cbUseLoopEndOffset.Text = "Use Offset";
            this.cbUseLoopEndOffset.UseVisualStyleBackColor = true;
            this.cbUseLoopEndOffset.CheckedChanged += new System.EventHandler(this.cbUseLoopEndOffset_CheckedChanged);
            // 
            // lblLoopEnd
            // 
            this.lblLoopEnd.AutoSize = true;
            this.lblLoopEnd.Location = new System.Drawing.Point(12, 65);
            this.lblLoopEnd.Name = "lblLoopEnd";
            this.lblLoopEnd.Size = new System.Drawing.Size(102, 13);
            this.lblLoopEnd.TabIndex = 11;
            this.lblLoopEnd.Text = "Loop End (Samples)";
            this.lblLoopEnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblLoopEnd_MouseUp);
            // 
            // loopEndOffsetDescription
            // 
            this.loopEndOffsetDescription.Location = new System.Drawing.Point(108, 83);
            this.loopEndOffsetDescription.Name = "loopEndOffsetDescription";
            this.loopEndOffsetDescription.OffsetByteOrder = "Little Endian";
            this.loopEndOffsetDescription.OffsetSize = "4";
            this.loopEndOffsetDescription.OffsetValue = "";
            this.loopEndOffsetDescription.Size = new System.Drawing.Size(372, 24);
            this.loopEndOffsetDescription.TabIndex = 33;
            // 
            // tbLoopEnd
            // 
            this.tbLoopEnd.Location = new System.Drawing.Point(123, 62);
            this.tbLoopEnd.Name = "tbLoopEnd";
            this.tbLoopEnd.Size = new System.Drawing.Size(100, 20);
            this.tbLoopEnd.TabIndex = 10;
            // 
            // cbUseLoopStartOffset
            // 
            this.cbUseLoopStartOffset.AutoSize = true;
            this.cbUseLoopStartOffset.Location = new System.Drawing.Point(26, 40);
            this.cbUseLoopStartOffset.Name = "cbUseLoopStartOffset";
            this.cbUseLoopStartOffset.Size = new System.Drawing.Size(76, 17);
            this.cbUseLoopStartOffset.TabIndex = 32;
            this.cbUseLoopStartOffset.Text = "Use Offset";
            this.cbUseLoopStartOffset.UseVisualStyleBackColor = true;
            this.cbUseLoopStartOffset.CheckedChanged += new System.EventHandler(this.cbUseLoopStartOffset_CheckedChanged);
            // 
            // tbLoopStart
            // 
            this.tbLoopStart.Location = new System.Drawing.Point(122, 13);
            this.tbLoopStart.Name = "tbLoopStart";
            this.tbLoopStart.Size = new System.Drawing.Size(100, 20);
            this.tbLoopStart.TabIndex = 8;
            // 
            // loopStartOffsetDescription
            // 
            this.loopStartOffsetDescription.Location = new System.Drawing.Point(108, 35);
            this.loopStartOffsetDescription.Name = "loopStartOffsetDescription";
            this.loopStartOffsetDescription.OffsetByteOrder = "Little Endian";
            this.loopStartOffsetDescription.OffsetSize = "4";
            this.loopStartOffsetDescription.OffsetValue = "";
            this.loopStartOffsetDescription.Size = new System.Drawing.Size(372, 24);
            this.loopStartOffsetDescription.TabIndex = 31;
            // 
            // cbManualEntry
            // 
            this.cbManualEntry.AutoSize = true;
            this.cbManualEntry.Checked = true;
            this.cbManualEntry.Location = new System.Drawing.Point(14, 108);
            this.cbManualEntry.Name = "cbManualEntry";
            this.cbManualEntry.Size = new System.Drawing.Size(87, 17);
            this.cbManualEntry.TabIndex = 28;
            this.cbManualEntry.TabStop = true;
            this.cbManualEntry.Text = "Manual Entry";
            this.cbManualEntry.UseVisualStyleBackColor = true;
            this.cbManualEntry.CheckedChanged += new System.EventHandler(this.cbManualEntry_CheckedChanged);
            // 
            // cbFindLoop
            // 
            this.cbFindLoop.AutoSize = true;
            this.cbFindLoop.Location = new System.Drawing.Point(278, 108);
            this.cbFindLoop.Name = "cbFindLoop";
            this.cbFindLoop.Size = new System.Drawing.Size(72, 17);
            this.cbFindLoop.TabIndex = 27;
            this.cbFindLoop.Text = "Find Loop";
            this.cbFindLoop.UseVisualStyleBackColor = true;
            this.cbFindLoop.CheckedChanged += new System.EventHandler(this.cbFindLoop_CheckedChanged);
            // 
            // cbNoLoops
            // 
            this.cbNoLoops.AutoSize = true;
            this.cbNoLoops.Location = new System.Drawing.Point(107, 108);
            this.cbNoLoops.Name = "cbNoLoops";
            this.cbNoLoops.Size = new System.Drawing.Size(71, 17);
            this.cbNoLoops.TabIndex = 25;
            this.cbNoLoops.Text = "No Loops";
            this.cbNoLoops.UseVisualStyleBackColor = true;
            this.cbNoLoops.CheckedChanged += new System.EventHandler(this.cbNoLoops_CheckedChanged);
            // 
            // cbLoopFileEnd
            // 
            this.cbLoopFileEnd.AutoSize = true;
            this.cbLoopFileEnd.Location = new System.Drawing.Point(187, 108);
            this.cbLoopFileEnd.Name = "cbLoopFileEnd";
            this.cbLoopFileEnd.Size = new System.Drawing.Size(85, 17);
            this.cbLoopFileEnd.TabIndex = 26;
            this.cbLoopFileEnd.Text = "Use File End";
            this.cbLoopFileEnd.UseVisualStyleBackColor = true;
            this.cbLoopFileEnd.CheckedChanged += new System.EventHandler(this.cbLoopFileEnd_CheckedChanged);
            // 
            // grpGeneralOptions
            // 
            this.grpGeneralOptions.Controls.Add(this.tbRawDataSize);
            this.grpGeneralOptions.Controls.Add(this.lblRawDataSize);
            this.grpGeneralOptions.Controls.Add(this.cbUseInterleaveOffset);
            this.grpGeneralOptions.Controls.Add(this.interleaveOffsetDescription);
            this.grpGeneralOptions.Controls.Add(this.channelsOffsetDescription);
            this.grpGeneralOptions.Controls.Add(this.cbUseChannelsOffset);
            this.grpGeneralOptions.Controls.Add(this.lblHeaderSkip);
            this.grpGeneralOptions.Controls.Add(this.cbHeaderSkip);
            this.grpGeneralOptions.Controls.Add(this.lblInterleave);
            this.grpGeneralOptions.Controls.Add(this.cbInterleave);
            this.grpGeneralOptions.Controls.Add(this.cbChannels);
            this.grpGeneralOptions.Controls.Add(this.lblFrequency);
            this.grpGeneralOptions.Controls.Add(this.cbUseFrequencyOffset);
            this.grpGeneralOptions.Controls.Add(this.cbFrequency);
            this.grpGeneralOptions.Controls.Add(this.frequencyOffsetDescription);
            this.grpGeneralOptions.Controls.Add(this.lblChannels);
            this.grpGeneralOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpGeneralOptions.Location = new System.Drawing.Point(0, 41);
            this.grpGeneralOptions.Name = "grpGeneralOptions";
            this.grpGeneralOptions.Size = new System.Drawing.Size(925, 213);
            this.grpGeneralOptions.TabIndex = 35;
            this.grpGeneralOptions.TabStop = false;
            this.grpGeneralOptions.Text = "General";
            // 
            // tbRawDataSize
            // 
            this.tbRawDataSize.Location = new System.Drawing.Point(382, 13);
            this.tbRawDataSize.Name = "tbRawDataSize";
            this.tbRawDataSize.Size = new System.Drawing.Size(94, 20);
            this.tbRawDataSize.TabIndex = 36;
            // 
            // lblRawDataSize
            // 
            this.lblRawDataSize.AutoSize = true;
            this.lblRawDataSize.Location = new System.Drawing.Point(251, 16);
            this.lblRawDataSize.Name = "lblRawDataSize";
            this.lblRawDataSize.Size = new System.Drawing.Size(124, 13);
            this.lblRawDataSize.TabIndex = 35;
            this.lblRawDataSize.Text = "Raw Data Size (optional)";
            // 
            // cbUseInterleaveOffset
            // 
            this.cbUseInterleaveOffset.AutoSize = true;
            this.cbUseInterleaveOffset.Location = new System.Drawing.Point(22, 75);
            this.cbUseInterleaveOffset.Name = "cbUseInterleaveOffset";
            this.cbUseInterleaveOffset.Size = new System.Drawing.Size(76, 17);
            this.cbUseInterleaveOffset.TabIndex = 34;
            this.cbUseInterleaveOffset.Text = "Use Offset";
            this.cbUseInterleaveOffset.UseVisualStyleBackColor = true;
            this.cbUseInterleaveOffset.CheckedChanged += new System.EventHandler(this.cbUseInterleaveOffset_CheckedChanged);
            // 
            // interleaveOffsetDescription
            // 
            this.interleaveOffsetDescription.Location = new System.Drawing.Point(104, 70);
            this.interleaveOffsetDescription.Name = "interleaveOffsetDescription";
            this.interleaveOffsetDescription.OffsetByteOrder = "Little Endian";
            this.interleaveOffsetDescription.OffsetSize = "4";
            this.interleaveOffsetDescription.OffsetValue = "";
            this.interleaveOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.interleaveOffsetDescription.TabIndex = 33;
            // 
            // channelsOffsetDescription
            // 
            this.channelsOffsetDescription.Location = new System.Drawing.Point(104, 125);
            this.channelsOffsetDescription.Name = "channelsOffsetDescription";
            this.channelsOffsetDescription.OffsetByteOrder = "Little Endian";
            this.channelsOffsetDescription.OffsetSize = "4";
            this.channelsOffsetDescription.OffsetValue = "";
            this.channelsOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.channelsOffsetDescription.TabIndex = 32;
            // 
            // cbUseChannelsOffset
            // 
            this.cbUseChannelsOffset.AutoSize = true;
            this.cbUseChannelsOffset.Location = new System.Drawing.Point(22, 130);
            this.cbUseChannelsOffset.Name = "cbUseChannelsOffset";
            this.cbUseChannelsOffset.Size = new System.Drawing.Size(76, 17);
            this.cbUseChannelsOffset.TabIndex = 31;
            this.cbUseChannelsOffset.Text = "Use Offset";
            this.cbUseChannelsOffset.UseVisualStyleBackColor = true;
            this.cbUseChannelsOffset.CheckedChanged += new System.EventHandler(this.cbUseChannelOffset_CheckedChanged);
            // 
            // lblHeaderSkip
            // 
            this.lblHeaderSkip.AutoSize = true;
            this.lblHeaderSkip.Location = new System.Drawing.Point(9, 16);
            this.lblHeaderSkip.Name = "lblHeaderSkip";
            this.lblHeaderSkip.Size = new System.Drawing.Size(66, 13);
            this.lblHeaderSkip.TabIndex = 1;
            this.lblHeaderSkip.Text = "Header Skip";
            // 
            // cbHeaderSkip
            // 
            this.cbHeaderSkip.FormattingEnabled = true;
            this.cbHeaderSkip.Location = new System.Drawing.Point(92, 13);
            this.cbHeaderSkip.Name = "cbHeaderSkip";
            this.cbHeaderSkip.Size = new System.Drawing.Size(100, 21);
            this.cbHeaderSkip.TabIndex = 21;
            this.cbHeaderSkip.Text = "0";
            this.cbHeaderSkip.SelectedValueChanged += new System.EventHandler(this.cbHeaderSkip_SelectedValueChanged);
            // 
            // lblInterleave
            // 
            this.lblInterleave.AutoSize = true;
            this.lblInterleave.Location = new System.Drawing.Point(9, 46);
            this.lblInterleave.Name = "lblInterleave";
            this.lblInterleave.Size = new System.Drawing.Size(115, 13);
            this.lblInterleave.TabIndex = 3;
            this.lblInterleave.Text = "Interleave / Block Size";
            // 
            // cbInterleave
            // 
            this.cbInterleave.FormattingEnabled = true;
            this.cbInterleave.Location = new System.Drawing.Point(153, 43);
            this.cbInterleave.Name = "cbInterleave";
            this.cbInterleave.Size = new System.Drawing.Size(100, 21);
            this.cbInterleave.TabIndex = 22;
            this.cbInterleave.Text = "0";
            this.cbInterleave.SelectedValueChanged += new System.EventHandler(this.cbInterleave_SelectedValueChanged);
            // 
            // cbChannels
            // 
            this.cbChannels.FormattingEnabled = true;
            this.cbChannels.Location = new System.Drawing.Point(90, 103);
            this.cbChannels.Name = "cbChannels";
            this.cbChannels.Size = new System.Drawing.Size(100, 21);
            this.cbChannels.TabIndex = 23;
            this.cbChannels.SelectedValueChanged += new System.EventHandler(this.cbChannels_SelectedValueChanged);
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(8, 161);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(57, 13);
            this.lblFrequency.TabIndex = 7;
            this.lblFrequency.Text = "Frequency";
            // 
            // cbUseFrequencyOffset
            // 
            this.cbUseFrequencyOffset.AutoSize = true;
            this.cbUseFrequencyOffset.Location = new System.Drawing.Point(22, 185);
            this.cbUseFrequencyOffset.Name = "cbUseFrequencyOffset";
            this.cbUseFrequencyOffset.Size = new System.Drawing.Size(76, 17);
            this.cbUseFrequencyOffset.TabIndex = 30;
            this.cbUseFrequencyOffset.Text = "Use Offset";
            this.cbUseFrequencyOffset.UseVisualStyleBackColor = true;
            this.cbUseFrequencyOffset.CheckedChanged += new System.EventHandler(this.cbUseFrequencyOffset_CheckedChanged);
            // 
            // cbFrequency
            // 
            this.cbFrequency.FormattingEnabled = true;
            this.cbFrequency.Location = new System.Drawing.Point(90, 158);
            this.cbFrequency.Name = "cbFrequency";
            this.cbFrequency.Size = new System.Drawing.Size(100, 21);
            this.cbFrequency.TabIndex = 24;
            this.cbFrequency.SelectedValueChanged += new System.EventHandler(this.cbFrequency_SelectedValueChanged);
            // 
            // frequencyOffsetDescription
            // 
            this.frequencyOffsetDescription.Location = new System.Drawing.Point(104, 180);
            this.frequencyOffsetDescription.Name = "frequencyOffsetDescription";
            this.frequencyOffsetDescription.OffsetByteOrder = "Little Endian";
            this.frequencyOffsetDescription.OffsetSize = "4";
            this.frequencyOffsetDescription.OffsetValue = "";
            this.frequencyOffsetDescription.Size = new System.Drawing.Size(372, 24);
            this.frequencyOffsetDescription.TabIndex = 29;
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.Location = new System.Drawing.Point(9, 107);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new System.Drawing.Size(51, 13);
            this.lblChannels.TabIndex = 5;
            this.lblChannels.Text = "Channels";
            // 
            // grpFunction
            // 
            this.grpFunction.Controls.Add(this.lblFilenameFilter);
            this.grpFunction.Controls.Add(this.tbFilenameFilter);
            this.grpFunction.Controls.Add(this.tbSourceDirectory);
            this.grpFunction.Controls.Add(this.lbFiles);
            this.grpFunction.Controls.Add(this.btnBrowseDirectory);
            this.grpFunction.Controls.Add(this.rbCreate);
            this.grpFunction.Controls.Add(this.rbEdit);
            this.grpFunction.Controls.Add(this.rbExtract);
            this.grpFunction.Controls.Add(this.cbHeaderOnly);
            this.grpFunction.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFunction.Location = new System.Drawing.Point(0, 23);
            this.grpFunction.Name = "grpFunction";
            this.grpFunction.Size = new System.Drawing.Size(948, 155);
            this.grpFunction.TabIndex = 13;
            this.grpFunction.TabStop = false;
            this.grpFunction.Text = "Functions";
            // 
            // lblFilenameFilter
            // 
            this.lblFilenameFilter.AutoSize = true;
            this.lblFilenameFilter.Location = new System.Drawing.Point(322, 44);
            this.lblFilenameFilter.Name = "lblFilenameFilter";
            this.lblFilenameFilter.Size = new System.Drawing.Size(76, 13);
            this.lblFilenameFilter.TabIndex = 16;
            this.lblFilenameFilter.Text = "FileName Filter";
            this.lblFilenameFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFilenameFilter
            // 
            this.tbFilenameFilter.Location = new System.Drawing.Point(142, 41);
            this.tbFilenameFilter.Name = "tbFilenameFilter";
            this.tbFilenameFilter.Size = new System.Drawing.Size(174, 20);
            this.tbFilenameFilter.TabIndex = 15;
            this.tbFilenameFilter.TextChanged += new System.EventHandler(this.tbFilenameFilter_TextChanged);
            // 
            // tbSourceDirectory
            // 
            this.tbSourceDirectory.Location = new System.Drawing.Point(142, 14);
            this.tbSourceDirectory.Name = "tbSourceDirectory";
            this.tbSourceDirectory.Size = new System.Drawing.Size(350, 20);
            this.tbSourceDirectory.TabIndex = 13;
            this.tbSourceDirectory.Click += new System.EventHandler(this.tbSourceDirectory_Click);
            this.tbSourceDirectory.TextChanged += new System.EventHandler(this.tbSourceDirectory_TextChanged);
            // 
            // lbFiles
            // 
            this.lbFiles.AllowDrop = true;
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.HorizontalScrollbar = true;
            this.lbFiles.Location = new System.Drawing.Point(142, 67);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFiles.Size = new System.Drawing.Size(388, 82);
            this.lbFiles.TabIndex = 12;
            this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
            this.lbFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragDrop);
            this.lbFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragEnter);
            this.lbFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbFiles_MouseUp);
            // 
            // btnBrowseDirectory
            // 
            this.btnBrowseDirectory.Location = new System.Drawing.Point(498, 14);
            this.btnBrowseDirectory.Name = "btnBrowseDirectory";
            this.btnBrowseDirectory.Size = new System.Drawing.Size(32, 20);
            this.btnBrowseDirectory.TabIndex = 14;
            this.btnBrowseDirectory.Text = "...";
            this.btnBrowseDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseDirectory.Click += new System.EventHandler(this.btnBrowseDirectory_Click);
            // 
            // Genh_CreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 539);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpFunction);
            this.Name = "Genh_CreatorForm";
            this.Text = "Genh_CreatorForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpFunction, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpFormat.ResumeLayout(false);
            this.contextMenuRefresh.ResumeLayout(false);
            this.contextMenuBytesToSamples.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.pnlOptions.ResumeLayout(false);
            this.grpCoefOptions.ResumeLayout(false);
            this.grpCoefOptions.PerformLayout();
            this.grpXmaOptions.ResumeLayout(false);
            this.grpXmaOptions.PerformLayout();
            this.grpAtracOptions.ResumeLayout(false);
            this.grpAtracOptions.PerformLayout();
            this.grpSkipSamples.ResumeLayout(false);
            this.grpSkipSamples.PerformLayout();
            this.grpLoopOptions.ResumeLayout(false);
            this.grpLoopOptions.PerformLayout();
            this.grpGeneralOptions.ResumeLayout(false);
            this.grpGeneralOptions.PerformLayout();
            this.grpFunction.ResumeLayout(false);
            this.grpFunction.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFormat;
        private System.Windows.Forms.ComboBox comboFormat;
        private System.Windows.Forms.CheckBox cbHeaderOnly;
        private System.Windows.Forms.RadioButton rbCreate;
        private System.Windows.Forms.RadioButton rbEdit;
        private System.Windows.Forms.RadioButton rbExtract;
        private System.Windows.Forms.ContextMenuStrip contextMenuRefresh;
        private System.Windows.Forms.ToolStripMenuItem refreshFileListToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuBytesToSamples;
        private System.Windows.Forms.ToolStripMenuItem bytesToSamplesToolStripMenuItem;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.Panel pnlOptions;
        private System.Windows.Forms.RadioButton cbFindLoop;
        private System.Windows.Forms.Label lblHeaderSkip;
        private System.Windows.Forms.RadioButton cbLoopFileEnd;
        private System.Windows.Forms.Label lblInterleave;
        private System.Windows.Forms.RadioButton cbNoLoops;
        private System.Windows.Forms.Label lblChannels;
        private System.Windows.Forms.ComboBox cbFrequency;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.RadioButton cbManualEntry;
        private System.Windows.Forms.TextBox tbLoopStart;
        private System.Windows.Forms.ComboBox cbChannels;
        private System.Windows.Forms.Label lblLoopStart;
        private System.Windows.Forms.ComboBox cbInterleave;
        private System.Windows.Forms.TextBox tbLoopEnd;
        private System.Windows.Forms.ComboBox cbHeaderSkip;
        private System.Windows.Forms.Label lblLoopEnd;
        private System.Windows.Forms.TextBox tbRightCoef;
        private System.Windows.Forms.Label lblLeftCoef;
        private System.Windows.Forms.Label lblRightCoef;
        private System.Windows.Forms.TextBox tbLeftCoef;
        private VGMToolbox.controls.OffsetDescriptionControl frequencyOffsetDescription;
        private System.Windows.Forms.GroupBox grpFunction;
        private System.Windows.Forms.CheckBox cbUseFrequencyOffset;
        private System.Windows.Forms.CheckBox cbUseLoopStartOffset;
        private VGMToolbox.controls.OffsetDescriptionControl loopStartOffsetDescription;
        private System.Windows.Forms.CheckBox cbUseLoopEndOffset;
        private VGMToolbox.controls.OffsetDescriptionControl loopEndOffsetDescription;
        private System.Windows.Forms.GroupBox grpGeneralOptions;
        private System.Windows.Forms.GroupBox grpLoopOptions;
        private System.Windows.Forms.GroupBox grpCoefOptions;
        private System.Windows.Forms.Label lblFilenameFilter;
        private System.Windows.Forms.TextBox tbFilenameFilter;
        private System.Windows.Forms.TextBox tbSourceDirectory;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Button btnBrowseDirectory;
        private System.Windows.Forms.CheckBox cbLoopEndBytesToSamples;
        private System.Windows.Forms.CheckBox cbLoopStartBytesToSamples;
        private System.Windows.Forms.ToolStripMenuItem clearFileListToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCoefficientType;
        private controls.OffsetDescriptionControl channelsOffsetDescription;
        private System.Windows.Forms.CheckBox cbUseChannelsOffset;
        private System.Windows.Forms.CheckBox cbUseInterleaveOffset;
        private controls.OffsetDescriptionControl interleaveOffsetDescription;
        private System.Windows.Forms.CheckBox cbTotalSamplesBytesToSamples;
        private controls.OffsetDescriptionControl totalSamplesOffsetDescription;
        private System.Windows.Forms.CheckBox cbUseTotalSamplesOffset;
        private System.Windows.Forms.Label lblTotalSamples;
        private System.Windows.Forms.TextBox tbTotalSamples;
        private System.Windows.Forms.GroupBox grpXmaOptions;
        private System.Windows.Forms.GroupBox grpAtracOptions;
        private System.Windows.Forms.GroupBox grpSkipSamples;
        private System.Windows.Forms.Label lblAtrac3StereoMode;
        private System.Windows.Forms.ComboBox cbAtrac3StereoMode;
        private System.Windows.Forms.TextBox tbForceSkipSamplesNumber;
        private System.Windows.Forms.CheckBox cbForceSkipSamples;
        private System.Windows.Forms.ComboBox cbXmaStreamMode;
        private System.Windows.Forms.Label lblXmaStreamMode;
        private System.Windows.Forms.TextBox tbRawDataSize;
        private System.Windows.Forms.Label lblRawDataSize;
    }
}