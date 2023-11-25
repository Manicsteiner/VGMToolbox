namespace VGMToolbox.forms.extraction
{
    partial class VfsExtractorForm
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
            this.grpFileCount = new System.Windows.Forms.GroupBox();
            this.rbReadHeaderToEof = new System.Windows.Forms.RadioButton();
            this.fileCountOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.headerSizeOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.rbHeaderSizeOffset = new System.Windows.Forms.RadioButton();
            this.tbHeaderSizeValue = new System.Windows.Forms.TextBox();
            this.rbHeaderSizeValue = new System.Windows.Forms.RadioButton();
            this.tbStaticFileCount = new System.Windows.Forms.TextBox();
            this.rbFileCountOffset = new System.Windows.Forms.RadioButton();
            this.rbStaticFileCount = new System.Windows.Forms.RadioButton();
            this.grpPresets = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSavePreset = new System.Windows.Forms.Button();
            this.btnLoadPreset = new System.Windows.Forms.Button();
            this.comboPresets = new System.Windows.Forms.ComboBox();
            this.grpFileRecordInfo = new System.Windows.Forms.GroupBox();
            this.grpFileRecordNameSize = new System.Windows.Forms.GroupBox();
            this.lblHexOnly = new System.Windows.Forms.Label();
            this.rbFileRecordNameSize = new System.Windows.Forms.RadioButton();
            this.tbFileRecordNameTerminatorBytes = new System.Windows.Forms.TextBox();
            this.rbFileRecordNameTerminator = new System.Windows.Forms.RadioButton();
            this.tbFileRecordNameSize = new System.Windows.Forms.TextBox();
            this.grpFileRecordName = new System.Windows.Forms.GroupBox();
            this.fileNameRelativeOffsetOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.fileNameAbsoluteOffsetOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.lblFileNameRecordRelativeLocation = new System.Windows.Forms.Label();
            this.comboFileRecordNameRelativeLocation = new System.Windows.Forms.ComboBox();
            this.rbFileNameRelativeOffset = new System.Windows.Forms.RadioButton();
            this.rbFileNameAbsoluteOffset = new System.Windows.Forms.RadioButton();
            this.rbFileRecordFileNameInRecord = new System.Windows.Forms.RadioButton();
            this.tbFileRecordNameOffset = new System.Windows.Forms.TextBox();
            this.cbFileNameIsPresent = new System.Windows.Forms.CheckBox();
            this.grpIndividualFileLength = new System.Windows.Forms.GroupBox();
            this.fileLengthOffsetDescription = new VGMToolbox.controls.CalculatingOffsetDescriptionControl();
            this.rbUseOffsetsToDetermineLength = new System.Windows.Forms.RadioButton();
            this.rbUseVfsFileLength = new System.Windows.Forms.RadioButton();
            this.grpIndividualFileOffset = new System.Windows.Forms.GroupBox();
            this.fileOffsetOffsetDescription = new VGMToolbox.controls.CalculatingOffsetDescriptionControl();
            this.tbByteAlignement = new System.Windows.Forms.TextBox();
            this.cbDoOffsetByteAlginment = new System.Windows.Forms.CheckBox();
            this.lblUseFileLengthToDetermineOffset = new System.Windows.Forms.Label();
            this.tbUseFileLengthBeginOffset = new System.Windows.Forms.TextBox();
            this.rbUseFileSizeToDetermineOffset = new System.Windows.Forms.RadioButton();
            this.rbUseVfsFileOffset = new System.Windows.Forms.RadioButton();
            this.pnlFileRecordsHeader = new System.Windows.Forms.Panel();
            this.lblFileRecordSizeNameWarning = new System.Windows.Forms.Label();
            this.lblFileRecordsStartOffset = new System.Windows.Forms.Label();
            this.comboFileRecordSize = new System.Windows.Forms.ComboBox();
            this.tbFileRecordsBeginOffset = new System.Windows.Forms.TextBox();
            this.lblFileRecordSize = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpSourceFiles = new System.Windows.Forms.GroupBox();
            this.cbOutputLogFiles = new System.Windows.Forms.CheckBox();
            this.btnBrowseOutputFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.tbOutputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseDataFile = new System.Windows.Forms.Button();
            this.lblDataFilePath = new System.Windows.Forms.Label();
            this.tbDataFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowseHeaderFile = new System.Windows.Forms.Button();
            this.tbHeaderFilePath = new System.Windows.Forms.TextBox();
            this.cbUseHeaderFile = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpFileCount.SuspendLayout();
            this.grpPresets.SuspendLayout();
            this.grpFileRecordInfo.SuspendLayout();
            this.grpFileRecordNameSize.SuspendLayout();
            this.grpFileRecordName.SuspendLayout();
            this.grpIndividualFileLength.SuspendLayout();
            this.grpIndividualFileOffset.SuspendLayout();
            this.pnlFileRecordsHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpSourceFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 386);
            this.pnlLabels.Size = new System.Drawing.Size(801, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(801, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 309);
            this.tbOutput.Size = new System.Drawing.Size(801, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnReset);
            this.pnlButtons.Location = new System.Drawing.Point(0, 289);
            this.pnlButtons.Size = new System.Drawing.Size(801, 20);
            this.pnlButtons.Controls.SetChildIndex(this.btnReset, 0);
            this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlButtons.Controls.SetChildIndex(this.btnDoTask, 0);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(741, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(681, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpFileCount
            // 
            this.grpFileCount.Controls.Add(this.rbReadHeaderToEof);
            this.grpFileCount.Controls.Add(this.fileCountOffsetDescription);
            this.grpFileCount.Controls.Add(this.headerSizeOffsetDescription);
            this.grpFileCount.Controls.Add(this.rbHeaderSizeOffset);
            this.grpFileCount.Controls.Add(this.tbHeaderSizeValue);
            this.grpFileCount.Controls.Add(this.rbHeaderSizeValue);
            this.grpFileCount.Controls.Add(this.tbStaticFileCount);
            this.grpFileCount.Controls.Add(this.rbFileCountOffset);
            this.grpFileCount.Controls.Add(this.rbStaticFileCount);
            this.grpFileCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFileCount.Location = new System.Drawing.Point(0, 0);
            this.grpFileCount.Name = "grpFileCount";
            this.grpFileCount.Size = new System.Drawing.Size(784, 131);
            this.grpFileCount.TabIndex = 7;
            this.grpFileCount.TabStop = false;
            this.grpFileCount.Text = "Header Size or File Count";
            // 
            // rbReadHeaderToEof
            // 
            this.rbReadHeaderToEof.AutoSize = true;
            this.rbReadHeaderToEof.Location = new System.Drawing.Point(6, 112);
            this.rbReadHeaderToEof.Name = "rbReadHeaderToEof";
            this.rbReadHeaderToEof.Size = new System.Drawing.Size(125, 17);
            this.rbReadHeaderToEof.TabIndex = 18;
            this.rbReadHeaderToEof.TabStop = true;
            this.rbReadHeaderToEof.Text = "Read Header to EOF";
            this.rbReadHeaderToEof.UseVisualStyleBackColor = true;
            this.rbReadHeaderToEof.CheckedChanged += new System.EventHandler(this.rbReadHeaderToEof_CheckedChanged);
            // 
            // fileCountOffsetDescription
            // 
            this.fileCountOffsetDescription.Location = new System.Drawing.Point(148, 86);
            this.fileCountOffsetDescription.Name = "fileCountOffsetDescription";
            this.fileCountOffsetDescription.OffsetByteOrder = "Little Endian";
            this.fileCountOffsetDescription.OffsetSize = "4";
            this.fileCountOffsetDescription.OffsetValue = "";
            this.fileCountOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.fileCountOffsetDescription.TabIndex = 17;
            // 
            // headerSizeOffsetDescription
            // 
            this.headerSizeOffsetDescription.Location = new System.Drawing.Point(148, 38);
            this.headerSizeOffsetDescription.Name = "headerSizeOffsetDescription";
            this.headerSizeOffsetDescription.OffsetByteOrder = "Little Endian";
            this.headerSizeOffsetDescription.OffsetSize = "4";
            this.headerSizeOffsetDescription.OffsetValue = "";
            this.headerSizeOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.headerSizeOffsetDescription.TabIndex = 16;
            // 
            // rbHeaderSizeOffset
            // 
            this.rbHeaderSizeOffset.AutoSize = true;
            this.rbHeaderSizeOffset.Location = new System.Drawing.Point(6, 42);
            this.rbHeaderSizeOffset.Name = "rbHeaderSizeOffset";
            this.rbHeaderSizeOffset.Size = new System.Drawing.Size(136, 17);
            this.rbHeaderSizeOffset.TabIndex = 10;
            this.rbHeaderSizeOffset.TabStop = true;
            this.rbHeaderSizeOffset.Text = "Header Size is at Offset";
            this.rbHeaderSizeOffset.UseVisualStyleBackColor = true;
            // 
            // tbHeaderSizeValue
            // 
            this.tbHeaderSizeValue.Location = new System.Drawing.Point(148, 18);
            this.tbHeaderSizeValue.Name = "tbHeaderSizeValue";
            this.tbHeaderSizeValue.Size = new System.Drawing.Size(70, 20);
            this.tbHeaderSizeValue.TabIndex = 9;
            // 
            // rbHeaderSizeValue
            // 
            this.rbHeaderSizeValue.AutoSize = true;
            this.rbHeaderSizeValue.Location = new System.Drawing.Point(6, 19);
            this.rbHeaderSizeValue.Name = "rbHeaderSizeValue";
            this.rbHeaderSizeValue.Size = new System.Drawing.Size(130, 17);
            this.rbHeaderSizeValue.TabIndex = 8;
            this.rbHeaderSizeValue.TabStop = true;
            this.rbHeaderSizeValue.Text = "Header Ends at Offset";
            this.rbHeaderSizeValue.UseVisualStyleBackColor = true;
            this.rbHeaderSizeValue.CheckedChanged += new System.EventHandler(this.rbFileCountEndOffset_CheckedChanged);
            // 
            // tbStaticFileCount
            // 
            this.tbStaticFileCount.Location = new System.Drawing.Point(148, 65);
            this.tbStaticFileCount.MaxLength = 10;
            this.tbStaticFileCount.Name = "tbStaticFileCount";
            this.tbStaticFileCount.Size = new System.Drawing.Size(70, 20);
            this.tbStaticFileCount.TabIndex = 3;
            // 
            // rbFileCountOffset
            // 
            this.rbFileCountOffset.AutoSize = true;
            this.rbFileCountOffset.Location = new System.Drawing.Point(6, 89);
            this.rbFileCountOffset.Name = "rbFileCountOffset";
            this.rbFileCountOffset.Size = new System.Drawing.Size(125, 17);
            this.rbFileCountOffset.TabIndex = 1;
            this.rbFileCountOffset.TabStop = true;
            this.rbFileCountOffset.Text = "File Count is at Offset";
            this.rbFileCountOffset.UseVisualStyleBackColor = true;
            this.rbFileCountOffset.CheckedChanged += new System.EventHandler(this.rbOffsetBasedFileCount_CheckedChanged);
            // 
            // rbStaticFileCount
            // 
            this.rbStaticFileCount.AutoSize = true;
            this.rbStaticFileCount.Location = new System.Drawing.Point(6, 66);
            this.rbStaticFileCount.Name = "rbStaticFileCount";
            this.rbStaticFileCount.Size = new System.Drawing.Size(107, 17);
            this.rbStaticFileCount.TabIndex = 0;
            this.rbStaticFileCount.TabStop = true;
            this.rbStaticFileCount.Text = "File Count Equals";
            this.rbStaticFileCount.UseVisualStyleBackColor = true;
            this.rbStaticFileCount.CheckedChanged += new System.EventHandler(this.rbUserEnteredFileCount_CheckedChanged);
            // 
            // grpPresets
            // 
            this.grpPresets.Controls.Add(this.btnRefresh);
            this.grpPresets.Controls.Add(this.btnSavePreset);
            this.grpPresets.Controls.Add(this.btnLoadPreset);
            this.grpPresets.Controls.Add(this.comboPresets);
            this.grpPresets.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPresets.Location = new System.Drawing.Point(0, 23);
            this.grpPresets.Name = "grpPresets";
            this.grpPresets.Size = new System.Drawing.Size(801, 43);
            this.grpPresets.TabIndex = 8;
            this.grpPresets.TabStop = false;
            this.grpPresets.Text = "Presets";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::VGMToolbox.Properties.Resources.Button_Refresh_16x16;
            this.btnRefresh.Location = new System.Drawing.Point(507, 16);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(21, 21);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSavePreset
            // 
            this.btnSavePreset.Location = new System.Drawing.Point(457, 16);
            this.btnSavePreset.Name = "btnSavePreset";
            this.btnSavePreset.Size = new System.Drawing.Size(48, 21);
            this.btnSavePreset.TabIndex = 2;
            this.btnSavePreset.Text = "Save";
            this.btnSavePreset.UseVisualStyleBackColor = true;
            this.btnSavePreset.Click += new System.EventHandler(this.btnSavePreset_Click);
            // 
            // btnLoadPreset
            // 
            this.btnLoadPreset.Location = new System.Drawing.Point(407, 16);
            this.btnLoadPreset.Name = "btnLoadPreset";
            this.btnLoadPreset.Size = new System.Drawing.Size(48, 21);
            this.btnLoadPreset.TabIndex = 1;
            this.btnLoadPreset.Text = "Load";
            this.btnLoadPreset.UseVisualStyleBackColor = true;
            this.btnLoadPreset.Click += new System.EventHandler(this.btnLoadPreset_Click);
            // 
            // comboPresets
            // 
            this.comboPresets.FormattingEnabled = true;
            this.comboPresets.Location = new System.Drawing.Point(6, 16);
            this.comboPresets.Name = "comboPresets";
            this.comboPresets.Size = new System.Drawing.Size(395, 21);
            this.comboPresets.TabIndex = 0;
            // 
            // grpFileRecordInfo
            // 
            this.grpFileRecordInfo.AutoSize = true;
            this.grpFileRecordInfo.Controls.Add(this.grpFileRecordNameSize);
            this.grpFileRecordInfo.Controls.Add(this.grpFileRecordName);
            this.grpFileRecordInfo.Controls.Add(this.grpIndividualFileLength);
            this.grpFileRecordInfo.Controls.Add(this.grpIndividualFileOffset);
            this.grpFileRecordInfo.Controls.Add(this.pnlFileRecordsHeader);
            this.grpFileRecordInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFileRecordInfo.Location = new System.Drawing.Point(0, 131);
            this.grpFileRecordInfo.Name = "grpFileRecordInfo";
            this.grpFileRecordInfo.Size = new System.Drawing.Size(784, 481);
            this.grpFileRecordInfo.TabIndex = 9;
            this.grpFileRecordInfo.TabStop = false;
            this.grpFileRecordInfo.Text = "File Record Information";
            // 
            // grpFileRecordNameSize
            // 
            this.grpFileRecordNameSize.Controls.Add(this.lblHexOnly);
            this.grpFileRecordNameSize.Controls.Add(this.rbFileRecordNameSize);
            this.grpFileRecordNameSize.Controls.Add(this.tbFileRecordNameTerminatorBytes);
            this.grpFileRecordNameSize.Controls.Add(this.rbFileRecordNameTerminator);
            this.grpFileRecordNameSize.Controls.Add(this.tbFileRecordNameSize);
            this.grpFileRecordNameSize.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFileRecordNameSize.Location = new System.Drawing.Point(3, 408);
            this.grpFileRecordNameSize.Name = "grpFileRecordNameSize";
            this.grpFileRecordNameSize.Size = new System.Drawing.Size(778, 70);
            this.grpFileRecordNameSize.TabIndex = 8;
            this.grpFileRecordNameSize.TabStop = false;
            this.grpFileRecordNameSize.Text = "Individual File Name Size";
            // 
            // lblHexOnly
            // 
            this.lblHexOnly.AutoSize = true;
            this.lblHexOnly.Location = new System.Drawing.Point(219, 47);
            this.lblHexOnly.Name = "lblHexOnly";
            this.lblHexOnly.Size = new System.Drawing.Size(159, 13);
            this.lblHexOnly.TabIndex = 12;
            this.lblHexOnly.Text = "(hex value, \'0x\' prefix unneeded)";
            // 
            // rbFileRecordNameSize
            // 
            this.rbFileRecordNameSize.AutoSize = true;
            this.rbFileRecordNameSize.Location = new System.Drawing.Point(3, 19);
            this.rbFileRecordNameSize.Name = "rbFileRecordNameSize";
            this.rbFileRecordNameSize.Size = new System.Drawing.Size(97, 17);
            this.rbFileRecordNameSize.TabIndex = 10;
            this.rbFileRecordNameSize.TabStop = true;
            this.rbFileRecordNameSize.Text = "Has Static Size";
            this.rbFileRecordNameSize.UseVisualStyleBackColor = true;
            this.rbFileRecordNameSize.CheckedChanged += new System.EventHandler(this.rbFileRecordNameSize_CheckedChanged);
            // 
            // tbFileRecordNameTerminatorBytes
            // 
            this.tbFileRecordNameTerminatorBytes.Location = new System.Drawing.Point(140, 44);
            this.tbFileRecordNameTerminatorBytes.Name = "tbFileRecordNameTerminatorBytes";
            this.tbFileRecordNameTerminatorBytes.Size = new System.Drawing.Size(73, 20);
            this.tbFileRecordNameTerminatorBytes.TabIndex = 11;
            // 
            // rbFileRecordNameTerminator
            // 
            this.rbFileRecordNameTerminator.AutoSize = true;
            this.rbFileRecordNameTerminator.Location = new System.Drawing.Point(3, 45);
            this.rbFileRecordNameTerminator.Name = "rbFileRecordNameTerminator";
            this.rbFileRecordNameTerminator.Size = new System.Drawing.Size(126, 17);
            this.rbFileRecordNameTerminator.TabIndex = 10;
            this.rbFileRecordNameTerminator.TabStop = true;
            this.rbFileRecordNameTerminator.Text = "Has Terminator Bytes";
            this.rbFileRecordNameTerminator.UseVisualStyleBackColor = true;
            this.rbFileRecordNameTerminator.CheckedChanged += new System.EventHandler(this.rbFileRecordNameTerminator_CheckedChanged);
            // 
            // tbFileRecordNameSize
            // 
            this.tbFileRecordNameSize.Location = new System.Drawing.Point(140, 18);
            this.tbFileRecordNameSize.Name = "tbFileRecordNameSize";
            this.tbFileRecordNameSize.Size = new System.Drawing.Size(73, 20);
            this.tbFileRecordNameSize.TabIndex = 2;
            // 
            // grpFileRecordName
            // 
            this.grpFileRecordName.Controls.Add(this.fileNameRelativeOffsetOffsetDescription);
            this.grpFileRecordName.Controls.Add(this.fileNameAbsoluteOffsetOffsetDescription);
            this.grpFileRecordName.Controls.Add(this.lblFileNameRecordRelativeLocation);
            this.grpFileRecordName.Controls.Add(this.comboFileRecordNameRelativeLocation);
            this.grpFileRecordName.Controls.Add(this.rbFileNameRelativeOffset);
            this.grpFileRecordName.Controls.Add(this.rbFileNameAbsoluteOffset);
            this.grpFileRecordName.Controls.Add(this.rbFileRecordFileNameInRecord);
            this.grpFileRecordName.Controls.Add(this.tbFileRecordNameOffset);
            this.grpFileRecordName.Controls.Add(this.cbFileNameIsPresent);
            this.grpFileRecordName.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFileRecordName.Location = new System.Drawing.Point(3, 265);
            this.grpFileRecordName.Name = "grpFileRecordName";
            this.grpFileRecordName.Size = new System.Drawing.Size(778, 143);
            this.grpFileRecordName.TabIndex = 7;
            this.grpFileRecordName.TabStop = false;
            this.grpFileRecordName.Text = "Individual File Name Location/Offset";
            // 
            // fileNameRelativeOffsetOffsetDescription
            // 
            this.fileNameRelativeOffsetOffsetDescription.Location = new System.Drawing.Point(209, 85);
            this.fileNameRelativeOffsetOffsetDescription.Name = "fileNameRelativeOffsetOffsetDescription";
            this.fileNameRelativeOffsetOffsetDescription.OffsetByteOrder = "Little Endian";
            this.fileNameRelativeOffsetOffsetDescription.OffsetSize = "4";
            this.fileNameRelativeOffsetOffsetDescription.OffsetValue = "";
            this.fileNameRelativeOffsetOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.fileNameRelativeOffsetOffsetDescription.TabIndex = 20;
            // 
            // fileNameAbsoluteOffsetOffsetDescription
            // 
            this.fileNameAbsoluteOffsetOffsetDescription.Location = new System.Drawing.Point(209, 61);
            this.fileNameAbsoluteOffsetOffsetDescription.Name = "fileNameAbsoluteOffsetOffsetDescription";
            this.fileNameAbsoluteOffsetOffsetDescription.OffsetByteOrder = "Little Endian";
            this.fileNameAbsoluteOffsetOffsetDescription.OffsetSize = "4";
            this.fileNameAbsoluteOffsetOffsetDescription.OffsetValue = "";
            this.fileNameAbsoluteOffsetOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.fileNameAbsoluteOffsetOffsetDescription.TabIndex = 19;
            // 
            // lblFileNameRecordRelativeLocation
            // 
            this.lblFileNameRecordRelativeLocation.AutoSize = true;
            this.lblFileNameRecordRelativeLocation.Location = new System.Drawing.Point(90, 115);
            this.lblFileNameRecordRelativeLocation.Name = "lblFileNameRecordRelativeLocation";
            this.lblFileNameRecordRelativeLocation.Size = new System.Drawing.Size(113, 13);
            this.lblFileNameRecordRelativeLocation.TabIndex = 18;
            this.lblFileNameRecordRelativeLocation.Text = "Offset is Relative From";
            // 
            // comboFileRecordNameRelativeLocation
            // 
            this.comboFileRecordNameRelativeLocation.FormattingEnabled = true;
            this.comboFileRecordNameRelativeLocation.Location = new System.Drawing.Point(209, 112);
            this.comboFileRecordNameRelativeLocation.Name = "comboFileRecordNameRelativeLocation";
            this.comboFileRecordNameRelativeLocation.Size = new System.Drawing.Size(198, 21);
            this.comboFileRecordNameRelativeLocation.TabIndex = 17;
            this.comboFileRecordNameRelativeLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboFileRecordNameRelativeLocation_KeyPress);
            this.comboFileRecordNameRelativeLocation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboFileRecordNameRelativeLocation_KeyDown);
            // 
            // rbFileNameRelativeOffset
            // 
            this.rbFileNameRelativeOffset.AutoSize = true;
            this.rbFileNameRelativeOffset.Location = new System.Drawing.Point(3, 87);
            this.rbFileNameRelativeOffset.Name = "rbFileNameRelativeOffset";
            this.rbFileNameRelativeOffset.Size = new System.Drawing.Size(198, 17);
            this.rbFileNameRelativeOffset.TabIndex = 11;
            this.rbFileNameRelativeOffset.TabStop = true;
            this.rbFileNameRelativeOffset.Text = "File Name Relative Offset is at Offset";
            this.rbFileNameRelativeOffset.UseVisualStyleBackColor = true;
            this.rbFileNameRelativeOffset.CheckedChanged += new System.EventHandler(this.rbFileNameRelativeOffset_CheckedChanged);
            // 
            // rbFileNameAbsoluteOffset
            // 
            this.rbFileNameAbsoluteOffset.AutoSize = true;
            this.rbFileNameAbsoluteOffset.Location = new System.Drawing.Point(3, 64);
            this.rbFileNameAbsoluteOffset.Name = "rbFileNameAbsoluteOffset";
            this.rbFileNameAbsoluteOffset.Size = new System.Drawing.Size(200, 17);
            this.rbFileNameAbsoluteOffset.TabIndex = 3;
            this.rbFileNameAbsoluteOffset.TabStop = true;
            this.rbFileNameAbsoluteOffset.Text = "File Name Absolute Offset is at Offset";
            this.rbFileNameAbsoluteOffset.UseVisualStyleBackColor = true;
            this.rbFileNameAbsoluteOffset.CheckedChanged += new System.EventHandler(this.rbFileNameAbsoluteOffset_CheckedChanged);
            // 
            // rbFileRecordFileNameInRecord
            // 
            this.rbFileRecordFileNameInRecord.AutoSize = true;
            this.rbFileRecordFileNameInRecord.Location = new System.Drawing.Point(3, 42);
            this.rbFileRecordFileNameInRecord.Name = "rbFileRecordFileNameInRecord";
            this.rbFileRecordFileNameInRecord.Size = new System.Drawing.Size(303, 17);
            this.rbFileRecordFileNameInRecord.TabIndex = 2;
            this.rbFileRecordFileNameInRecord.TabStop = true;
            this.rbFileRecordFileNameInRecord.Text = "File Name is Included in the Individual File Record at Offset";
            this.rbFileRecordFileNameInRecord.UseVisualStyleBackColor = true;
            this.rbFileRecordFileNameInRecord.CheckedChanged += new System.EventHandler(this.rbFileRecordFileNameInRecord_CheckedChanged);
            // 
            // tbFileRecordNameOffset
            // 
            this.tbFileRecordNameOffset.Location = new System.Drawing.Point(312, 41);
            this.tbFileRecordNameOffset.Name = "tbFileRecordNameOffset";
            this.tbFileRecordNameOffset.Size = new System.Drawing.Size(73, 20);
            this.tbFileRecordNameOffset.TabIndex = 1;
            // 
            // cbFileNameIsPresent
            // 
            this.cbFileNameIsPresent.AutoSize = true;
            this.cbFileNameIsPresent.Location = new System.Drawing.Point(3, 19);
            this.cbFileNameIsPresent.Name = "cbFileNameIsPresent";
            this.cbFileNameIsPresent.Size = new System.Drawing.Size(179, 17);
            this.cbFileNameIsPresent.TabIndex = 0;
            this.cbFileNameIsPresent.Text = "File Name is Included in the VFS";
            this.cbFileNameIsPresent.UseVisualStyleBackColor = true;
            this.cbFileNameIsPresent.CheckedChanged += new System.EventHandler(this.cbFileNameIsPresent_CheckedChanged);
            // 
            // grpIndividualFileLength
            // 
            this.grpIndividualFileLength.Controls.Add(this.fileLengthOffsetDescription);
            this.grpIndividualFileLength.Controls.Add(this.rbUseOffsetsToDetermineLength);
            this.grpIndividualFileLength.Controls.Add(this.rbUseVfsFileLength);
            this.grpIndividualFileLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpIndividualFileLength.Location = new System.Drawing.Point(3, 176);
            this.grpIndividualFileLength.Name = "grpIndividualFileLength";
            this.grpIndividualFileLength.Size = new System.Drawing.Size(778, 89);
            this.grpIndividualFileLength.TabIndex = 6;
            this.grpIndividualFileLength.TabStop = false;
            this.grpIndividualFileLength.Text = "Individual File Length";
            // 
            // fileLengthOffsetDescription
            // 
            this.fileLengthOffsetDescription.CalculationValue = "";
            this.fileLengthOffsetDescription.Location = new System.Drawing.Point(90, 15);
            this.fileLengthOffsetDescription.Name = "fileLengthOffsetDescription";
            this.fileLengthOffsetDescription.OffsetByteOrder = "Little Endian";
            this.fileLengthOffsetDescription.OffsetSize = "4";
            this.fileLengthOffsetDescription.OffsetValue = "";
            this.fileLengthOffsetDescription.Size = new System.Drawing.Size(422, 53);
            this.fileLengthOffsetDescription.TabIndex = 10;
            // 
            // rbUseOffsetsToDetermineLength
            // 
            this.rbUseOffsetsToDetermineLength.AutoSize = true;
            this.rbUseOffsetsToDetermineLength.Location = new System.Drawing.Point(6, 65);
            this.rbUseOffsetsToDetermineLength.Name = "rbUseOffsetsToDetermineLength";
            this.rbUseOffsetsToDetermineLength.Size = new System.Drawing.Size(220, 17);
            this.rbUseOffsetsToDetermineLength.TabIndex = 9;
            this.rbUseOffsetsToDetermineLength.TabStop = true;
            this.rbUseOffsetsToDetermineLength.Text = "Use File Offsets to determine File Lengths";
            this.rbUseOffsetsToDetermineLength.UseVisualStyleBackColor = true;
            this.rbUseOffsetsToDetermineLength.CheckedChanged += new System.EventHandler(this.rbUseOffsetsToDetermineLength_CheckedChanged);
            // 
            // rbUseVfsFileLength
            // 
            this.rbUseVfsFileLength.AutoSize = true;
            this.rbUseVfsFileLength.Location = new System.Drawing.Point(6, 19);
            this.rbUseVfsFileLength.Name = "rbUseVfsFileLength";
            this.rbUseVfsFileLength.Size = new System.Drawing.Size(87, 17);
            this.rbUseVfsFileLength.TabIndex = 0;
            this.rbUseVfsFileLength.TabStop = true;
            this.rbUseVfsFileLength.Text = "File Length is";
            this.rbUseVfsFileLength.UseVisualStyleBackColor = true;
            this.rbUseVfsFileLength.CheckedChanged += new System.EventHandler(this.rbUseVfsFileLength_CheckedChanged);
            // 
            // grpIndividualFileOffset
            // 
            this.grpIndividualFileOffset.Controls.Add(this.fileOffsetOffsetDescription);
            this.grpIndividualFileOffset.Controls.Add(this.tbByteAlignement);
            this.grpIndividualFileOffset.Controls.Add(this.cbDoOffsetByteAlginment);
            this.grpIndividualFileOffset.Controls.Add(this.lblUseFileLengthToDetermineOffset);
            this.grpIndividualFileOffset.Controls.Add(this.tbUseFileLengthBeginOffset);
            this.grpIndividualFileOffset.Controls.Add(this.rbUseFileSizeToDetermineOffset);
            this.grpIndividualFileOffset.Controls.Add(this.rbUseVfsFileOffset);
            this.grpIndividualFileOffset.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpIndividualFileOffset.Location = new System.Drawing.Point(3, 61);
            this.grpIndividualFileOffset.Name = "grpIndividualFileOffset";
            this.grpIndividualFileOffset.Size = new System.Drawing.Size(778, 115);
            this.grpIndividualFileOffset.TabIndex = 5;
            this.grpIndividualFileOffset.TabStop = false;
            this.grpIndividualFileOffset.Text = "Individual File Offset";
            // 
            // fileOffsetOffsetDescription
            // 
            this.fileOffsetOffsetDescription.CalculationValue = "";
            this.fileOffsetOffsetDescription.Location = new System.Drawing.Point(84, 15);
            this.fileOffsetOffsetDescription.Name = "fileOffsetOffsetDescription";
            this.fileOffsetOffsetDescription.OffsetByteOrder = "Little Endian";
            this.fileOffsetOffsetDescription.OffsetSize = "4";
            this.fileOffsetOffsetDescription.OffsetValue = "";
            this.fileOffsetOffsetDescription.Size = new System.Drawing.Size(422, 53);
            this.fileOffsetOffsetDescription.TabIndex = 15;
            // 
            // tbByteAlignement
            // 
            this.tbByteAlignement.Location = new System.Drawing.Point(140, 90);
            this.tbByteAlignement.Name = "tbByteAlignement";
            this.tbByteAlignement.Size = new System.Drawing.Size(73, 20);
            this.tbByteAlignement.TabIndex = 14;
            // 
            // cbDoOffsetByteAlginment
            // 
            this.cbDoOffsetByteAlginment.AutoSize = true;
            this.cbDoOffsetByteAlginment.Location = new System.Drawing.Point(21, 92);
            this.cbDoOffsetByteAlginment.Name = "cbDoOffsetByteAlginment";
            this.cbDoOffsetByteAlginment.Size = new System.Drawing.Size(117, 17);
            this.cbDoOffsetByteAlginment.TabIndex = 13;
            this.cbDoOffsetByteAlginment.Text = "with Byte alignment";
            this.cbDoOffsetByteAlginment.UseVisualStyleBackColor = true;
            this.cbDoOffsetByteAlginment.CheckedChanged += new System.EventHandler(this.cbDoOffsetByteAlginment_CheckedChanged);
            // 
            // lblUseFileLengthToDetermineOffset
            // 
            this.lblUseFileLengthToDetermineOffset.AutoSize = true;
            this.lblUseFileLengthToDetermineOffset.Location = new System.Drawing.Point(219, 71);
            this.lblUseFileLengthToDetermineOffset.Name = "lblUseFileLengthToDetermineOffset";
            this.lblUseFileLengthToDetermineOffset.Size = new System.Drawing.Size(247, 13);
            this.lblUseFileLengthToDetermineOffset.TabIndex = 10;
            this.lblUseFileLengthToDetermineOffset.Text = "and use File Lengths to determine following offsets.";
            // 
            // tbUseFileLengthBeginOffset
            // 
            this.tbUseFileLengthBeginOffset.Location = new System.Drawing.Point(140, 68);
            this.tbUseFileLengthBeginOffset.Name = "tbUseFileLengthBeginOffset";
            this.tbUseFileLengthBeginOffset.Size = new System.Drawing.Size(73, 20);
            this.tbUseFileLengthBeginOffset.TabIndex = 9;
            // 
            // rbUseFileSizeToDetermineOffset
            // 
            this.rbUseFileSizeToDetermineOffset.AutoSize = true;
            this.rbUseFileSizeToDetermineOffset.Location = new System.Drawing.Point(6, 69);
            this.rbUseFileSizeToDetermineOffset.Name = "rbUseFileSizeToDetermineOffset";
            this.rbUseFileSizeToDetermineOffset.Size = new System.Drawing.Size(128, 17);
            this.rbUseFileSizeToDetermineOffset.TabIndex = 8;
            this.rbUseFileSizeToDetermineOffset.TabStop = true;
            this.rbUseFileSizeToDetermineOffset.Text = "Begin cutting at offset";
            this.rbUseFileSizeToDetermineOffset.UseVisualStyleBackColor = true;
            this.rbUseFileSizeToDetermineOffset.CheckedChanged += new System.EventHandler(this.rbUseFileSizeToDetermineOffset_CheckedChanged);
            // 
            // rbUseVfsFileOffset
            // 
            this.rbUseVfsFileOffset.AutoSize = true;
            this.rbUseVfsFileOffset.Location = new System.Drawing.Point(6, 19);
            this.rbUseVfsFileOffset.Name = "rbUseVfsFileOffset";
            this.rbUseVfsFileOffset.Size = new System.Drawing.Size(82, 17);
            this.rbUseVfsFileOffset.TabIndex = 0;
            this.rbUseVfsFileOffset.TabStop = true;
            this.rbUseVfsFileOffset.Text = "File Offset is";
            this.rbUseVfsFileOffset.UseVisualStyleBackColor = true;
            this.rbUseVfsFileOffset.CheckedChanged += new System.EventHandler(this.rbUseVfsFileOffset_CheckedChanged);
            // 
            // pnlFileRecordsHeader
            // 
            this.pnlFileRecordsHeader.Controls.Add(this.lblFileRecordSizeNameWarning);
            this.pnlFileRecordsHeader.Controls.Add(this.lblFileRecordsStartOffset);
            this.pnlFileRecordsHeader.Controls.Add(this.comboFileRecordSize);
            this.pnlFileRecordsHeader.Controls.Add(this.tbFileRecordsBeginOffset);
            this.pnlFileRecordsHeader.Controls.Add(this.lblFileRecordSize);
            this.pnlFileRecordsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFileRecordsHeader.Location = new System.Drawing.Point(3, 16);
            this.pnlFileRecordsHeader.Name = "pnlFileRecordsHeader";
            this.pnlFileRecordsHeader.Size = new System.Drawing.Size(778, 45);
            this.pnlFileRecordsHeader.TabIndex = 4;
            // 
            // lblFileRecordSizeNameWarning
            // 
            this.lblFileRecordSizeNameWarning.AutoSize = true;
            this.lblFileRecordSizeNameWarning.Location = new System.Drawing.Point(222, 26);
            this.lblFileRecordSizeNameWarning.Name = "lblFileRecordSizeNameWarning";
            this.lblFileRecordSizeNameWarning.Size = new System.Drawing.Size(283, 13);
            this.lblFileRecordSizeNameWarning.TabIndex = 4;
            this.lblFileRecordSizeNameWarning.Text = "(if File Name uses a terminator, do not include it in this size)";
            // 
            // lblFileRecordsStartOffset
            // 
            this.lblFileRecordsStartOffset.AutoSize = true;
            this.lblFileRecordsStartOffset.Location = new System.Drawing.Point(3, 5);
            this.lblFileRecordsStartOffset.Name = "lblFileRecordsStartOffset";
            this.lblFileRecordsStartOffset.Size = new System.Drawing.Size(131, 13);
            this.lblFileRecordsStartOffset.TabIndex = 2;
            this.lblFileRecordsStartOffset.Text = "File records begin at offset";
            // 
            // comboFileRecordSize
            // 
            this.comboFileRecordSize.FormattingEnabled = true;
            this.comboFileRecordSize.Location = new System.Drawing.Point(351, 2);
            this.comboFileRecordSize.Name = "comboFileRecordSize";
            this.comboFileRecordSize.Size = new System.Drawing.Size(70, 21);
            this.comboFileRecordSize.TabIndex = 1;
            // 
            // tbFileRecordsBeginOffset
            // 
            this.tbFileRecordsBeginOffset.Location = new System.Drawing.Point(140, 2);
            this.tbFileRecordsBeginOffset.Name = "tbFileRecordsBeginOffset";
            this.tbFileRecordsBeginOffset.Size = new System.Drawing.Size(73, 20);
            this.tbFileRecordsBeginOffset.TabIndex = 3;
            // 
            // lblFileRecordSize
            // 
            this.lblFileRecordSize.AutoSize = true;
            this.lblFileRecordSize.Location = new System.Drawing.Point(219, 5);
            this.lblFileRecordSize.Name = "lblFileRecordSize";
            this.lblFileRecordSize.Size = new System.Drawing.Size(126, 13);
            this.lblFileRecordSize.TabIndex = 0;
            this.lblFileRecordSize.Text = "and each record has size";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.grpFileRecordInfo);
            this.panel1.Controls.Add(this.grpFileCount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 195);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(801, 94);
            this.panel1.TabIndex = 10;
            // 
            // grpSourceFiles
            // 
            this.grpSourceFiles.Controls.Add(this.cbOutputLogFiles);
            this.grpSourceFiles.Controls.Add(this.btnBrowseOutputFolder);
            this.grpSourceFiles.Controls.Add(this.label3);
            this.grpSourceFiles.Controls.Add(this.lblOutputFolder);
            this.grpSourceFiles.Controls.Add(this.tbOutputFolder);
            this.grpSourceFiles.Controls.Add(this.btnBrowseDataFile);
            this.grpSourceFiles.Controls.Add(this.lblDataFilePath);
            this.grpSourceFiles.Controls.Add(this.tbDataFilePath);
            this.grpSourceFiles.Controls.Add(this.btnBrowseHeaderFile);
            this.grpSourceFiles.Controls.Add(this.tbHeaderFilePath);
            this.grpSourceFiles.Controls.Add(this.cbUseHeaderFile);
            this.grpSourceFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSourceFiles.Location = new System.Drawing.Point(0, 66);
            this.grpSourceFiles.Name = "grpSourceFiles";
            this.grpSourceFiles.Size = new System.Drawing.Size(801, 129);
            this.grpSourceFiles.TabIndex = 10;
            this.grpSourceFiles.TabStop = false;
            this.grpSourceFiles.Text = "Source Files/Destination Folder";
            // 
            // cbOutputLogFiles
            // 
            this.cbOutputLogFiles.AutoSize = true;
            this.cbOutputLogFiles.Location = new System.Drawing.Point(4, 104);
            this.cbOutputLogFiles.Name = "cbOutputLogFiles";
            this.cbOutputLogFiles.Size = new System.Drawing.Size(200, 17);
            this.cbOutputLogFiles.TabIndex = 10;
            this.cbOutputLogFiles.Text = "Output Extraction Log and Batch File";
            this.cbOutputLogFiles.UseVisualStyleBackColor = true;
            // 
            // btnBrowseOutputFolder
            // 
            this.btnBrowseOutputFolder.Location = new System.Drawing.Point(407, 63);
            this.btnBrowseOutputFolder.Name = "btnBrowseOutputFolder";
            this.btnBrowseOutputFolder.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseOutputFolder.TabIndex = 9;
            this.btnBrowseOutputFolder.Text = "...";
            this.btnBrowseOutputFolder.UseVisualStyleBackColor = true;
            this.btnBrowseOutputFolder.Click += new System.EventHandler(this.btnBrowseOutputFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(89, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "(Leave Empty for Default)";
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(13, 66);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(71, 13);
            this.lblOutputFolder.TabIndex = 7;
            this.lblOutputFolder.Text = "Output Folder";
            // 
            // tbOutputFolder
            // 
            this.tbOutputFolder.AllowDrop = true;
            this.tbOutputFolder.Location = new System.Drawing.Point(92, 63);
            this.tbOutputFolder.Name = "tbOutputFolder";
            this.tbOutputFolder.Size = new System.Drawing.Size(309, 20);
            this.tbOutputFolder.TabIndex = 6;
            this.tbOutputFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbOutputFolder_DragDrop);
            this.tbOutputFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbOutputFolder_DragEnter);
            // 
            // btnBrowseDataFile
            // 
            this.btnBrowseDataFile.Location = new System.Drawing.Point(407, 11);
            this.btnBrowseDataFile.Name = "btnBrowseDataFile";
            this.btnBrowseDataFile.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseDataFile.TabIndex = 5;
            this.btnBrowseDataFile.Text = "...";
            this.btnBrowseDataFile.UseVisualStyleBackColor = true;
            this.btnBrowseDataFile.Click += new System.EventHandler(this.btnBrowseDataFile_Click);
            // 
            // lblDataFilePath
            // 
            this.lblDataFilePath.AutoSize = true;
            this.lblDataFilePath.Location = new System.Drawing.Point(35, 14);
            this.lblDataFilePath.Name = "lblDataFilePath";
            this.lblDataFilePath.Size = new System.Drawing.Size(49, 13);
            this.lblDataFilePath.TabIndex = 4;
            this.lblDataFilePath.Text = "Data File";
            // 
            // tbDataFilePath
            // 
            this.tbDataFilePath.AllowDrop = true;
            this.tbDataFilePath.Location = new System.Drawing.Point(92, 11);
            this.tbDataFilePath.Name = "tbDataFilePath";
            this.tbDataFilePath.Size = new System.Drawing.Size(309, 20);
            this.tbDataFilePath.TabIndex = 3;
            this.tbDataFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbDataFilePath_DragDrop);
            this.tbDataFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbDataFilePath_DragEnter);
            // 
            // btnBrowseHeaderFile
            // 
            this.btnBrowseHeaderFile.Location = new System.Drawing.Point(407, 37);
            this.btnBrowseHeaderFile.Name = "btnBrowseHeaderFile";
            this.btnBrowseHeaderFile.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseHeaderFile.TabIndex = 2;
            this.btnBrowseHeaderFile.Text = "...";
            this.btnBrowseHeaderFile.UseVisualStyleBackColor = true;
            this.btnBrowseHeaderFile.Click += new System.EventHandler(this.btnBrowseHeaderFile_Click);
            // 
            // tbHeaderFilePath
            // 
            this.tbHeaderFilePath.AllowDrop = true;
            this.tbHeaderFilePath.Location = new System.Drawing.Point(92, 37);
            this.tbHeaderFilePath.Name = "tbHeaderFilePath";
            this.tbHeaderFilePath.Size = new System.Drawing.Size(309, 20);
            this.tbHeaderFilePath.TabIndex = 1;
            this.tbHeaderFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbHeaderFilePath_DragDrop);
            this.tbHeaderFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbHeaderFilePath_DragEnter);
            // 
            // cbUseHeaderFile
            // 
            this.cbUseHeaderFile.AutoSize = true;
            this.cbUseHeaderFile.Location = new System.Drawing.Point(4, 39);
            this.cbUseHeaderFile.Name = "cbUseHeaderFile";
            this.cbUseHeaderFile.Size = new System.Drawing.Size(80, 17);
            this.cbUseHeaderFile.TabIndex = 0;
            this.cbUseHeaderFile.Text = "Header File";
            this.cbUseHeaderFile.UseVisualStyleBackColor = true;
            this.cbUseHeaderFile.CheckedChanged += new System.EventHandler(this.cbUseHeaderFile_CheckedChanged);
            // 
            // btnReset
            // 
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnReset.Location = new System.Drawing.Point(0, 0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(60, 20);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // VfsExtractorForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 427);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpSourceFiles);
            this.Controls.Add(this.grpPresets);
            this.Name = "VfsExtractorForm";
            this.Text = "VfsExtractorForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.VfsExtractorForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.VfsExtractorForm_DragEnter);
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpPresets, 0);
            this.Controls.SetChildIndex(this.grpSourceFiles, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpFileCount.ResumeLayout(false);
            this.grpFileCount.PerformLayout();
            this.grpPresets.ResumeLayout(false);
            this.grpFileRecordInfo.ResumeLayout(false);
            this.grpFileRecordNameSize.ResumeLayout(false);
            this.grpFileRecordNameSize.PerformLayout();
            this.grpFileRecordName.ResumeLayout(false);
            this.grpFileRecordName.PerformLayout();
            this.grpIndividualFileLength.ResumeLayout(false);
            this.grpIndividualFileLength.PerformLayout();
            this.grpIndividualFileOffset.ResumeLayout(false);
            this.grpIndividualFileOffset.PerformLayout();
            this.pnlFileRecordsHeader.ResumeLayout(false);
            this.pnlFileRecordsHeader.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpSourceFiles.ResumeLayout(false);
            this.grpSourceFiles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFileCount;
        private System.Windows.Forms.RadioButton rbFileCountOffset;
        private System.Windows.Forms.RadioButton rbStaticFileCount;
        private System.Windows.Forms.GroupBox grpPresets;
        private System.Windows.Forms.TextBox tbStaticFileCount;
        private System.Windows.Forms.RadioButton rbHeaderSizeValue;
        private System.Windows.Forms.TextBox tbHeaderSizeValue;
        private System.Windows.Forms.GroupBox grpFileRecordInfo;
        private System.Windows.Forms.Label lblFileRecordSize;
        private System.Windows.Forms.ComboBox comboFileRecordSize;
        private System.Windows.Forms.Label lblFileRecordsStartOffset;
        private System.Windows.Forms.TextBox tbFileRecordsBeginOffset;
        private System.Windows.Forms.Panel pnlFileRecordsHeader;
        private System.Windows.Forms.GroupBox grpIndividualFileOffset;
        private System.Windows.Forms.RadioButton rbUseVfsFileOffset;
        private System.Windows.Forms.RadioButton rbUseFileSizeToDetermineOffset;
        private System.Windows.Forms.TextBox tbUseFileLengthBeginOffset;
        private System.Windows.Forms.Label lblUseFileLengthToDetermineOffset;
        private System.Windows.Forms.GroupBox grpIndividualFileLength;
        private System.Windows.Forms.RadioButton rbUseVfsFileLength;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbUseOffsetsToDetermineLength;
        private System.Windows.Forms.GroupBox grpFileRecordName;
        private System.Windows.Forms.CheckBox cbFileNameIsPresent;
        private System.Windows.Forms.TextBox tbFileRecordNameOffset;
        private System.Windows.Forms.TextBox tbFileRecordNameSize;
        private System.Windows.Forms.RadioButton rbHeaderSizeOffset;
        private System.Windows.Forms.GroupBox grpSourceFiles;
        private System.Windows.Forms.TextBox tbHeaderFilePath;
        private System.Windows.Forms.CheckBox cbUseHeaderFile;
        private System.Windows.Forms.Button btnBrowseHeaderFile;
        private System.Windows.Forms.TextBox tbDataFilePath;
        private System.Windows.Forms.Label lblDataFilePath;
        private System.Windows.Forms.Button btnBrowseDataFile;
        private System.Windows.Forms.ComboBox comboPresets;
        private System.Windows.Forms.Button btnLoadPreset;
        private System.Windows.Forms.Button btnSavePreset;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.RadioButton rbFileRecordNameSize;
        private System.Windows.Forms.TextBox tbFileRecordNameTerminatorBytes;
        private System.Windows.Forms.RadioButton rbFileRecordNameTerminator;
        private System.Windows.Forms.Label lblHexOnly;
        private System.Windows.Forms.Label lblFileRecordSizeNameWarning;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.TextBox tbOutputFolder;
        private System.Windows.Forms.Button btnBrowseOutputFolder;
        private System.Windows.Forms.GroupBox grpFileRecordNameSize;
        private System.Windows.Forms.RadioButton rbFileRecordFileNameInRecord;
        private System.Windows.Forms.RadioButton rbFileNameAbsoluteOffset;
        private System.Windows.Forms.RadioButton rbFileNameRelativeOffset;
        private System.Windows.Forms.Label lblFileNameRecordRelativeLocation;
        private System.Windows.Forms.ComboBox comboFileRecordNameRelativeLocation;
        private System.Windows.Forms.CheckBox cbDoOffsetByteAlginment;
        private System.Windows.Forms.TextBox tbByteAlignement;
        private VGMToolbox.controls.OffsetDescriptionControl headerSizeOffsetDescription;
        private VGMToolbox.controls.OffsetDescriptionControl fileCountOffsetDescription;
        private VGMToolbox.controls.CalculatingOffsetDescriptionControl fileOffsetOffsetDescription;
        private VGMToolbox.controls.CalculatingOffsetDescriptionControl fileLengthOffsetDescription;
        private VGMToolbox.controls.OffsetDescriptionControl fileNameRelativeOffsetOffsetDescription;
        private VGMToolbox.controls.OffsetDescriptionControl fileNameAbsoluteOffsetOffsetDescription;
        private System.Windows.Forms.CheckBox cbOutputLogFiles;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.RadioButton rbReadHeaderToEof;
    }
}