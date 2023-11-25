namespace VGMToolbox.forms.stream
{
    partial class XmaConvertForm
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
            this.grpPresets = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSavePreset = new System.Windows.Forms.Button();
            this.btnLoadPreset = new System.Windows.Forms.Button();
            this.comboPresets = new System.Windows.Forms.ComboBox();
            this.grpOutputOptions = new System.Windows.Forms.GroupBox();
            this.btnBrowseOutputFolder = new System.Windows.Forms.Button();
            this.lblOutputFolderDefault = new System.Windows.Forms.Label();
            this.tbOutputFolder = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.groupOtherOptions = new System.Windows.Forms.GroupBox();
            this.cbKeepTempFiles = new System.Windows.Forms.CheckBox();
            this.cbShowAllExeOutput = new System.Windows.Forms.CheckBox();
            this.grpToWavOptions = new System.Windows.Forms.GroupBox();
            this.rbDoToWav = new System.Windows.Forms.RadioButton();
            this.rbDoXmaEncode = new System.Windows.Forms.RadioButton();
            this.grpPosFileMaker = new System.Windows.Forms.GroupBox();
            this.grpPosEndOffset = new System.Windows.Forms.GroupBox();
            this.rbLoopEndOffset = new System.Windows.Forms.RadioButton();
            this.loopEndValueOffsetDescription = new VGMToolbox.controls.CalculatingOffsetDescriptionControl();
            this.tbLoopEndStatic = new System.Windows.Forms.TextBox();
            this.rbLoopEndStatic = new System.Windows.Forms.RadioButton();
            this.grpPosStartOffset = new System.Windows.Forms.GroupBox();
            this.rbLoopStartOffset = new System.Windows.Forms.RadioButton();
            this.loopStartValueOffsetDescription = new VGMToolbox.controls.CalculatingOffsetDescriptionControl();
            this.tbLoopStartStatic = new System.Windows.Forms.TextBox();
            this.rbLoopStartStatic = new System.Windows.Forms.RadioButton();
            this.cbMakePosFile = new System.Windows.Forms.CheckBox();
            this.grpRiffHeaderOptions = new System.Windows.Forms.GroupBox();
            this.grpFrequency = new System.Windows.Forms.GroupBox();
            this.frequencyOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.rbFrequencyOffset = new System.Windows.Forms.RadioButton();
            this.rbAddManualFrequency = new System.Windows.Forms.RadioButton();
            this.comboRiffFrequency = new System.Windows.Forms.ComboBox();
            this.rbGetFrequencyFromRiff = new System.Windows.Forms.RadioButton();
            this.grpChannels = new System.Windows.Forms.GroupBox();
            this.numberOfChannelsOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.rbNumberOfChannelsOffset = new System.Windows.Forms.RadioButton();
            this.rbAddManualChannels = new System.Windows.Forms.RadioButton();
            this.rbGetChannelsFromRiff = new System.Windows.Forms.RadioButton();
            this.comboRiffChannels = new System.Windows.Forms.ComboBox();
            this.cbAddRiffHeader = new System.Windows.Forms.CheckBox();
            this.grpXmaParseOptions = new System.Windows.Forms.GroupBox();
            this.grpStartOffset = new System.Windows.Forms.GroupBox();
            this.rbStartOffsetIsAfterRiff = new System.Windows.Forms.RadioButton();
            this.XmaParseStartOffsetOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.rbXmaParseStartOffsetOffset = new System.Windows.Forms.RadioButton();
            this.rbXmaParseStartOffsetStatic = new System.Windows.Forms.RadioButton();
            this.tbXmaParseStartOffset = new System.Windows.Forms.TextBox();
            this.grpBlockSize = new System.Windows.Forms.GroupBox();
            this.XmaParseBlockSizeOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.rbXmaParseBlockSizeOffset = new System.Windows.Forms.RadioButton();
            this.rbXmaParseBlockSizeStatic = new System.Windows.Forms.RadioButton();
            this.tbXmaParseBlockSize = new System.Windows.Forms.TextBox();
            this.grpXmaParseDataSize = new System.Windows.Forms.GroupBox();
            this.rbGetDataSizeFromRiff = new System.Windows.Forms.RadioButton();
            this.XmaParseDataSizeOffsetDescription = new VGMToolbox.controls.OffsetDescriptionControl();
            this.rbXmaParseDataSizeOffset = new System.Windows.Forms.RadioButton();
            this.rbXmaParseDataSizeStatic = new System.Windows.Forms.RadioButton();
            this.tbXmaParseDataSize = new System.Windows.Forms.TextBox();
            this.cbXmaParseIgnoreErrors = new System.Windows.Forms.CheckBox();
            this.cbXmaParseDoRebuild = new System.Windows.Forms.CheckBox();
            this.cbDoXmaParse = new System.Windows.Forms.CheckBox();
            this.comboXmaParseInputType = new System.Windows.Forms.ComboBox();
            this.lblXmaParseInputType = new System.Windows.Forms.Label();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpPresets.SuspendLayout();
            this.grpOutputOptions.SuspendLayout();
            this.pnlOptions.SuspendLayout();
            this.groupOtherOptions.SuspendLayout();
            this.grpToWavOptions.SuspendLayout();
            this.grpPosFileMaker.SuspendLayout();
            this.grpPosEndOffset.SuspendLayout();
            this.grpPosStartOffset.SuspendLayout();
            this.grpRiffHeaderOptions.SuspendLayout();
            this.grpFrequency.SuspendLayout();
            this.grpChannels.SuspendLayout();
            this.grpXmaParseOptions.SuspendLayout();
            this.grpStartOffset.SuspendLayout();
            this.grpBlockSize.SuspendLayout();
            this.grpXmaParseDataSize.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 661);
            this.pnlLabels.Size = new System.Drawing.Size(851, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(851, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 584);
            this.tbOutput.Size = new System.Drawing.Size(851, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 564);
            this.pnlButtons.Size = new System.Drawing.Size(851, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(791, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(731, 0);
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
            this.grpPresets.Size = new System.Drawing.Size(851, 40);
            this.grpPresets.TabIndex = 5;
            this.grpPresets.TabStop = false;
            this.grpPresets.Text = "Presets";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::VGMToolbox.Properties.Resources.Button_Refresh_16x16;
            this.btnRefresh.Location = new System.Drawing.Point(507, 13);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(21, 21);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSavePreset
            // 
            this.btnSavePreset.Location = new System.Drawing.Point(457, 13);
            this.btnSavePreset.Name = "btnSavePreset";
            this.btnSavePreset.Size = new System.Drawing.Size(48, 21);
            this.btnSavePreset.TabIndex = 6;
            this.btnSavePreset.Text = "Save";
            this.btnSavePreset.UseVisualStyleBackColor = true;
            this.btnSavePreset.Click += new System.EventHandler(this.btnSavePreset_Click);
            // 
            // btnLoadPreset
            // 
            this.btnLoadPreset.Location = new System.Drawing.Point(407, 13);
            this.btnLoadPreset.Name = "btnLoadPreset";
            this.btnLoadPreset.Size = new System.Drawing.Size(48, 21);
            this.btnLoadPreset.TabIndex = 5;
            this.btnLoadPreset.Text = "Load";
            this.btnLoadPreset.UseVisualStyleBackColor = true;
            this.btnLoadPreset.Click += new System.EventHandler(this.btnLoadPreset_Click);
            // 
            // comboPresets
            // 
            this.comboPresets.FormattingEnabled = true;
            this.comboPresets.Location = new System.Drawing.Point(6, 13);
            this.comboPresets.Name = "comboPresets";
            this.comboPresets.Size = new System.Drawing.Size(395, 21);
            this.comboPresets.TabIndex = 4;
            // 
            // grpOutputOptions
            // 
            this.grpOutputOptions.Controls.Add(this.btnBrowseOutputFolder);
            this.grpOutputOptions.Controls.Add(this.lblOutputFolderDefault);
            this.grpOutputOptions.Controls.Add(this.tbOutputFolder);
            this.grpOutputOptions.Controls.Add(this.lblOutputFolder);
            this.grpOutputOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOutputOptions.Location = new System.Drawing.Point(0, 63);
            this.grpOutputOptions.Name = "grpOutputOptions";
            this.grpOutputOptions.Size = new System.Drawing.Size(851, 70);
            this.grpOutputOptions.TabIndex = 6;
            this.grpOutputOptions.TabStop = false;
            this.grpOutputOptions.Text = "Output Options";
            // 
            // btnBrowseOutputFolder
            // 
            this.btnBrowseOutputFolder.Location = new System.Drawing.Point(412, 19);
            this.btnBrowseOutputFolder.Name = "btnBrowseOutputFolder";
            this.btnBrowseOutputFolder.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseOutputFolder.TabIndex = 3;
            this.btnBrowseOutputFolder.Text = "...";
            this.btnBrowseOutputFolder.UseVisualStyleBackColor = true;
            this.btnBrowseOutputFolder.Click += new System.EventHandler(this.btnBrowseOutputFolder_Click);
            // 
            // lblOutputFolderDefault
            // 
            this.lblOutputFolderDefault.AutoSize = true;
            this.lblOutputFolderDefault.Location = new System.Drawing.Point(84, 42);
            this.lblOutputFolderDefault.Name = "lblOutputFolderDefault";
            this.lblOutputFolderDefault.Size = new System.Drawing.Size(127, 13);
            this.lblOutputFolderDefault.TabIndex = 2;
            this.lblOutputFolderDefault.Text = "(Leave empty for default.)";
            // 
            // tbOutputFolder
            // 
            this.tbOutputFolder.AllowDrop = true;
            this.tbOutputFolder.Location = new System.Drawing.Point(83, 19);
            this.tbOutputFolder.Name = "tbOutputFolder";
            this.tbOutputFolder.Size = new System.Drawing.Size(323, 20);
            this.tbOutputFolder.TabIndex = 1;
            this.tbOutputFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbOutputFolder_DragDrop);
            this.tbOutputFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbOutputFolder_DragEnter);
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(6, 22);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(71, 13);
            this.lblOutputFolder.TabIndex = 0;
            this.lblOutputFolder.Text = "Output Folder";
            // 
            // pnlOptions
            // 
            this.pnlOptions.AutoScroll = true;
            this.pnlOptions.Controls.Add(this.groupOtherOptions);
            this.pnlOptions.Controls.Add(this.grpToWavOptions);
            this.pnlOptions.Controls.Add(this.grpPosFileMaker);
            this.pnlOptions.Controls.Add(this.grpRiffHeaderOptions);
            this.pnlOptions.Controls.Add(this.grpXmaParseOptions);
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOptions.Location = new System.Drawing.Point(0, 133);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(851, 431);
            this.pnlOptions.TabIndex = 7;
            // 
            // groupOtherOptions
            // 
            this.groupOtherOptions.Controls.Add(this.cbKeepTempFiles);
            this.groupOtherOptions.Controls.Add(this.cbShowAllExeOutput);
            this.groupOtherOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupOtherOptions.Location = new System.Drawing.Point(0, 813);
            this.groupOtherOptions.Name = "groupOtherOptions";
            this.groupOtherOptions.Size = new System.Drawing.Size(834, 64);
            this.groupOtherOptions.TabIndex = 2;
            this.groupOtherOptions.TabStop = false;
            this.groupOtherOptions.Text = "Other Options";
            // 
            // cbKeepTempFiles
            // 
            this.cbKeepTempFiles.AutoSize = true;
            this.cbKeepTempFiles.Location = new System.Drawing.Point(6, 42);
            this.cbKeepTempFiles.Name = "cbKeepTempFiles";
            this.cbKeepTempFiles.Size = new System.Drawing.Size(160, 17);
            this.cbKeepTempFiles.TabIndex = 1;
            this.cbKeepTempFiles.Text = "Keep temp/intermediate files";
            this.cbKeepTempFiles.UseVisualStyleBackColor = true;
            // 
            // cbShowAllExeOutput
            // 
            this.cbShowAllExeOutput.AutoSize = true;
            this.cbShowAllExeOutput.Location = new System.Drawing.Point(6, 19);
            this.cbShowAllExeOutput.Name = "cbShowAllExeOutput";
            this.cbShowAllExeOutput.Size = new System.Drawing.Size(226, 17);
            this.cbShowAllExeOutput.TabIndex = 0;
            this.cbShowAllExeOutput.Text = "Show All Output from External Applications";
            this.cbShowAllExeOutput.UseVisualStyleBackColor = true;
            // 
            // grpToWavOptions
            // 
            this.grpToWavOptions.Controls.Add(this.rbDoToWav);
            this.grpToWavOptions.Controls.Add(this.rbDoXmaEncode);
            this.grpToWavOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpToWavOptions.Location = new System.Drawing.Point(0, 750);
            this.grpToWavOptions.Name = "grpToWavOptions";
            this.grpToWavOptions.Size = new System.Drawing.Size(834, 63);
            this.grpToWavOptions.TabIndex = 3;
            this.grpToWavOptions.TabStop = false;
            this.grpToWavOptions.Text = "4. WAV Conversion Options";
            // 
            // rbDoToWav
            // 
            this.rbDoToWav.AutoSize = true;
            this.rbDoToWav.Location = new System.Drawing.Point(3, 42);
            this.rbDoToWav.Name = "rbDoToWav";
            this.rbDoToWav.Size = new System.Drawing.Size(215, 17);
            this.rbDoToWav.TabIndex = 2;
            this.rbDoToWav.TabStop = true;
            this.rbDoToWav.Text = "Execute ToWav.exe on the Output Files";
            this.rbDoToWav.UseVisualStyleBackColor = true;
            // 
            // rbDoXmaEncode
            // 
            this.rbDoXmaEncode.AutoSize = true;
            this.rbDoXmaEncode.Location = new System.Drawing.Point(3, 19);
            this.rbDoXmaEncode.Name = "rbDoXmaEncode";
            this.rbDoXmaEncode.Size = new System.Drawing.Size(234, 17);
            this.rbDoXmaEncode.TabIndex = 1;
            this.rbDoXmaEncode.TabStop = true;
            this.rbDoXmaEncode.Text = "Execute xmaencode.exe on the Output Files";
            this.rbDoXmaEncode.UseVisualStyleBackColor = true;
            // 
            // grpPosFileMaker
            // 
            this.grpPosFileMaker.Controls.Add(this.grpPosEndOffset);
            this.grpPosFileMaker.Controls.Add(this.grpPosStartOffset);
            this.grpPosFileMaker.Controls.Add(this.cbMakePosFile);
            this.grpPosFileMaker.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPosFileMaker.Location = new System.Drawing.Point(0, 525);
            this.grpPosFileMaker.Name = "grpPosFileMaker";
            this.grpPosFileMaker.Size = new System.Drawing.Size(834, 225);
            this.grpPosFileMaker.TabIndex = 4;
            this.grpPosFileMaker.TabStop = false;
            this.grpPosFileMaker.Text = "3. POS File Maker";
            // 
            // grpPosEndOffset
            // 
            this.grpPosEndOffset.Controls.Add(this.rbLoopEndOffset);
            this.grpPosEndOffset.Controls.Add(this.loopEndValueOffsetDescription);
            this.grpPosEndOffset.Controls.Add(this.tbLoopEndStatic);
            this.grpPosEndOffset.Controls.Add(this.rbLoopEndStatic);
            this.grpPosEndOffset.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPosEndOffset.Location = new System.Drawing.Point(3, 127);
            this.grpPosEndOffset.Name = "grpPosEndOffset";
            this.grpPosEndOffset.Size = new System.Drawing.Size(828, 94);
            this.grpPosEndOffset.TabIndex = 2;
            this.grpPosEndOffset.TabStop = false;
            this.grpPosEndOffset.Text = "Loop Length Value";
            // 
            // rbLoopEndOffset
            // 
            this.rbLoopEndOffset.AutoSize = true;
            this.rbLoopEndOffset.Location = new System.Drawing.Point(6, 44);
            this.rbLoopEndOffset.Name = "rbLoopEndOffset";
            this.rbLoopEndOffset.Size = new System.Drawing.Size(125, 17);
            this.rbLoopEndOffset.TabIndex = 7;
            this.rbLoopEndOffset.TabStop = true;
            this.rbLoopEndOffset.Text = "Loop Length Value is";
            this.rbLoopEndOffset.UseVisualStyleBackColor = true;
            this.rbLoopEndOffset.CheckedChanged += new System.EventHandler(this.rbLoopEndOffset_CheckedChanged);
            // 
            // loopEndValueOffsetDescription
            // 
            this.loopEndValueOffsetDescription.CalculationValue = "";
            this.loopEndValueOffsetDescription.Location = new System.Drawing.Point(129, 40);
            this.loopEndValueOffsetDescription.Name = "loopEndValueOffsetDescription";
            this.loopEndValueOffsetDescription.OffsetByteOrder = "Little Endian";
            this.loopEndValueOffsetDescription.OffsetSize = "4";
            this.loopEndValueOffsetDescription.OffsetValue = "";
            this.loopEndValueOffsetDescription.Size = new System.Drawing.Size(422, 52);
            this.loopEndValueOffsetDescription.TabIndex = 6;
            // 
            // tbLoopEndStatic
            // 
            this.tbLoopEndStatic.Location = new System.Drawing.Point(180, 18);
            this.tbLoopEndStatic.Name = "tbLoopEndStatic";
            this.tbLoopEndStatic.Size = new System.Drawing.Size(70, 20);
            this.tbLoopEndStatic.TabIndex = 5;
            // 
            // rbLoopEndStatic
            // 
            this.rbLoopEndStatic.AutoSize = true;
            this.rbLoopEndStatic.Location = new System.Drawing.Point(6, 19);
            this.rbLoopEndStatic.Name = "rbLoopEndStatic";
            this.rbLoopEndStatic.Size = new System.Drawing.Size(115, 17);
            this.rbLoopEndStatic.TabIndex = 4;
            this.rbLoopEndStatic.TabStop = true;
            this.rbLoopEndStatic.Text = "Loop Length Value";
            this.rbLoopEndStatic.UseVisualStyleBackColor = true;
            this.rbLoopEndStatic.CheckedChanged += new System.EventHandler(this.rbLoopEndStatic_CheckedChanged);
            // 
            // grpPosStartOffset
            // 
            this.grpPosStartOffset.Controls.Add(this.rbLoopStartOffset);
            this.grpPosStartOffset.Controls.Add(this.loopStartValueOffsetDescription);
            this.grpPosStartOffset.Controls.Add(this.tbLoopStartStatic);
            this.grpPosStartOffset.Controls.Add(this.rbLoopStartStatic);
            this.grpPosStartOffset.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPosStartOffset.Location = new System.Drawing.Point(3, 33);
            this.grpPosStartOffset.Name = "grpPosStartOffset";
            this.grpPosStartOffset.Size = new System.Drawing.Size(828, 94);
            this.grpPosStartOffset.TabIndex = 1;
            this.grpPosStartOffset.TabStop = false;
            this.grpPosStartOffset.Text = "Loop Start Value";
            // 
            // rbLoopStartOffset
            // 
            this.rbLoopStartOffset.AutoSize = true;
            this.rbLoopStartOffset.Location = new System.Drawing.Point(6, 44);
            this.rbLoopStartOffset.Name = "rbLoopStartOffset";
            this.rbLoopStartOffset.Size = new System.Drawing.Size(114, 17);
            this.rbLoopStartOffset.TabIndex = 3;
            this.rbLoopStartOffset.TabStop = true;
            this.rbLoopStartOffset.Text = "Loop Start Value is";
            this.rbLoopStartOffset.UseVisualStyleBackColor = true;
            this.rbLoopStartOffset.CheckedChanged += new System.EventHandler(this.rbLoopStartOffset_CheckedChanged);
            // 
            // loopStartValueOffsetDescription
            // 
            this.loopStartValueOffsetDescription.CalculationValue = "";
            this.loopStartValueOffsetDescription.Location = new System.Drawing.Point(118, 40);
            this.loopStartValueOffsetDescription.Name = "loopStartValueOffsetDescription";
            this.loopStartValueOffsetDescription.OffsetByteOrder = "Little Endian";
            this.loopStartValueOffsetDescription.OffsetSize = "4";
            this.loopStartValueOffsetDescription.OffsetValue = "";
            this.loopStartValueOffsetDescription.Size = new System.Drawing.Size(422, 52);
            this.loopStartValueOffsetDescription.TabIndex = 2;
            // 
            // tbLoopStartStatic
            // 
            this.tbLoopStartStatic.Location = new System.Drawing.Point(169, 18);
            this.tbLoopStartStatic.Name = "tbLoopStartStatic";
            this.tbLoopStartStatic.Size = new System.Drawing.Size(70, 20);
            this.tbLoopStartStatic.TabIndex = 1;
            // 
            // rbLoopStartStatic
            // 
            this.rbLoopStartStatic.AutoSize = true;
            this.rbLoopStartStatic.Location = new System.Drawing.Point(6, 19);
            this.rbLoopStartStatic.Name = "rbLoopStartStatic";
            this.rbLoopStartStatic.Size = new System.Drawing.Size(104, 17);
            this.rbLoopStartStatic.TabIndex = 0;
            this.rbLoopStartStatic.TabStop = true;
            this.rbLoopStartStatic.Text = "Loop Start Value";
            this.rbLoopStartStatic.UseVisualStyleBackColor = true;
            this.rbLoopStartStatic.CheckedChanged += new System.EventHandler(this.rbLoopStartStatic_CheckedChanged);
            // 
            // cbMakePosFile
            // 
            this.cbMakePosFile.AutoSize = true;
            this.cbMakePosFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbMakePosFile.Location = new System.Drawing.Point(3, 16);
            this.cbMakePosFile.Name = "cbMakePosFile";
            this.cbMakePosFile.Size = new System.Drawing.Size(828, 17);
            this.cbMakePosFile.TabIndex = 0;
            this.cbMakePosFile.Text = "Make POS File";
            this.cbMakePosFile.UseVisualStyleBackColor = true;
            this.cbMakePosFile.CheckedChanged += new System.EventHandler(this.cbMakePosFile_CheckedChanged);
            // 
            // grpRiffHeaderOptions
            // 
            this.grpRiffHeaderOptions.Controls.Add(this.grpFrequency);
            this.grpRiffHeaderOptions.Controls.Add(this.grpChannels);
            this.grpRiffHeaderOptions.Controls.Add(this.cbAddRiffHeader);
            this.grpRiffHeaderOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpRiffHeaderOptions.Location = new System.Drawing.Point(0, 310);
            this.grpRiffHeaderOptions.Name = "grpRiffHeaderOptions";
            this.grpRiffHeaderOptions.Size = new System.Drawing.Size(834, 215);
            this.grpRiffHeaderOptions.TabIndex = 1;
            this.grpRiffHeaderOptions.TabStop = false;
            this.grpRiffHeaderOptions.Text = "2. RIFF Header Options";
            // 
            // grpFrequency
            // 
            this.grpFrequency.Controls.Add(this.frequencyOffsetDescription);
            this.grpFrequency.Controls.Add(this.rbFrequencyOffset);
            this.grpFrequency.Controls.Add(this.rbAddManualFrequency);
            this.grpFrequency.Controls.Add(this.comboRiffFrequency);
            this.grpFrequency.Controls.Add(this.rbGetFrequencyFromRiff);
            this.grpFrequency.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpFrequency.Location = new System.Drawing.Point(3, 35);
            this.grpFrequency.Name = "grpFrequency";
            this.grpFrequency.Size = new System.Drawing.Size(828, 90);
            this.grpFrequency.TabIndex = 10;
            this.grpFrequency.TabStop = false;
            this.grpFrequency.Text = "Frequency";
            // 
            // frequencyOffsetDescription
            // 
            this.frequencyOffsetDescription.Location = new System.Drawing.Point(156, 40);
            this.frequencyOffsetDescription.Name = "frequencyOffsetDescription";
            this.frequencyOffsetDescription.OffsetByteOrder = "Little Endian";
            this.frequencyOffsetDescription.OffsetSize = "4";
            this.frequencyOffsetDescription.OffsetValue = "";
            this.frequencyOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.frequencyOffsetDescription.TabIndex = 12;
            // 
            // rbFrequencyOffset
            // 
            this.rbFrequencyOffset.AutoSize = true;
            this.rbFrequencyOffset.Location = new System.Drawing.Point(6, 45);
            this.rbFrequencyOffset.Name = "rbFrequencyOffset";
            this.rbFrequencyOffset.Size = new System.Drawing.Size(151, 17);
            this.rbFrequencyOffset.TabIndex = 11;
            this.rbFrequencyOffset.TabStop = true;
            this.rbFrequencyOffset.Text = "Frequency Size is at Offset";
            this.rbFrequencyOffset.UseVisualStyleBackColor = true;
            this.rbFrequencyOffset.CheckedChanged += new System.EventHandler(this.rbFrequencyOffset_CheckedChanged);
            // 
            // rbAddManualFrequency
            // 
            this.rbAddManualFrequency.AutoSize = true;
            this.rbAddManualFrequency.Location = new System.Drawing.Point(6, 19);
            this.rbAddManualFrequency.Name = "rbAddManualFrequency";
            this.rbAddManualFrequency.Size = new System.Drawing.Size(97, 17);
            this.rbAddManualFrequency.TabIndex = 5;
            this.rbAddManualFrequency.TabStop = true;
            this.rbAddManualFrequency.Text = "Use Frequency";
            this.rbAddManualFrequency.UseVisualStyleBackColor = true;
            this.rbAddManualFrequency.CheckedChanged += new System.EventHandler(this.rbAddManualFrequency_CheckedChanged);
            // 
            // comboRiffFrequency
            // 
            this.comboRiffFrequency.FormattingEnabled = true;
            this.comboRiffFrequency.Location = new System.Drawing.Point(156, 18);
            this.comboRiffFrequency.Name = "comboRiffFrequency";
            this.comboRiffFrequency.Size = new System.Drawing.Size(77, 21);
            this.comboRiffFrequency.TabIndex = 1;
            // 
            // rbGetFrequencyFromRiff
            // 
            this.rbGetFrequencyFromRiff.AutoSize = true;
            this.rbGetFrequencyFromRiff.Location = new System.Drawing.Point(6, 68);
            this.rbGetFrequencyFromRiff.Name = "rbGetFrequencyFromRiff";
            this.rbGetFrequencyFromRiff.Size = new System.Drawing.Size(185, 17);
            this.rbGetFrequencyFromRiff.TabIndex = 6;
            this.rbGetFrequencyFromRiff.TabStop = true;
            this.rbGetFrequencyFromRiff.Text = "Get Frequency From RIFF Header";
            this.rbGetFrequencyFromRiff.UseVisualStyleBackColor = true;
            this.rbGetFrequencyFromRiff.CheckedChanged += new System.EventHandler(this.rbGetFrequencyFromRiff_CheckedChanged);
            // 
            // grpChannels
            // 
            this.grpChannels.Controls.Add(this.numberOfChannelsOffsetDescription);
            this.grpChannels.Controls.Add(this.rbNumberOfChannelsOffset);
            this.grpChannels.Controls.Add(this.rbAddManualChannels);
            this.grpChannels.Controls.Add(this.rbGetChannelsFromRiff);
            this.grpChannels.Controls.Add(this.comboRiffChannels);
            this.grpChannels.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpChannels.Location = new System.Drawing.Point(3, 125);
            this.grpChannels.Name = "grpChannels";
            this.grpChannels.Size = new System.Drawing.Size(828, 87);
            this.grpChannels.TabIndex = 9;
            this.grpChannels.TabStop = false;
            this.grpChannels.Text = "Channels";
            // 
            // numberOfChannelsOffsetDescription
            // 
            this.numberOfChannelsOffsetDescription.Location = new System.Drawing.Point(184, 39);
            this.numberOfChannelsOffsetDescription.Name = "numberOfChannelsOffsetDescription";
            this.numberOfChannelsOffsetDescription.OffsetByteOrder = "Little Endian";
            this.numberOfChannelsOffsetDescription.OffsetSize = "4";
            this.numberOfChannelsOffsetDescription.OffsetValue = "";
            this.numberOfChannelsOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.numberOfChannelsOffsetDescription.TabIndex = 14;
            // 
            // rbNumberOfChannelsOffset
            // 
            this.rbNumberOfChannelsOffset.AutoSize = true;
            this.rbNumberOfChannelsOffset.Location = new System.Drawing.Point(6, 44);
            this.rbNumberOfChannelsOffset.Name = "rbNumberOfChannelsOffset";
            this.rbNumberOfChannelsOffset.Size = new System.Drawing.Size(174, 17);
            this.rbNumberOfChannelsOffset.TabIndex = 13;
            this.rbNumberOfChannelsOffset.TabStop = true;
            this.rbNumberOfChannelsOffset.Text = "Number of Channels is at Offset";
            this.rbNumberOfChannelsOffset.UseVisualStyleBackColor = true;
            this.rbNumberOfChannelsOffset.CheckedChanged += new System.EventHandler(this.rbNumberOfChannelsOffset_CheckedChanged);
            // 
            // rbAddManualChannels
            // 
            this.rbAddManualChannels.AutoSize = true;
            this.rbAddManualChannels.Location = new System.Drawing.Point(6, 19);
            this.rbAddManualChannels.Name = "rbAddManualChannels";
            this.rbAddManualChannels.Size = new System.Drawing.Size(91, 17);
            this.rbAddManualChannels.TabIndex = 8;
            this.rbAddManualChannels.TabStop = true;
            this.rbAddManualChannels.Text = "Use Channels";
            this.rbAddManualChannels.UseVisualStyleBackColor = true;
            this.rbAddManualChannels.CheckedChanged += new System.EventHandler(this.rbAddManualChannels_CheckedChanged);
            // 
            // rbGetChannelsFromRiff
            // 
            this.rbGetChannelsFromRiff.AutoSize = true;
            this.rbGetChannelsFromRiff.Location = new System.Drawing.Point(6, 65);
            this.rbGetChannelsFromRiff.Name = "rbGetChannelsFromRiff";
            this.rbGetChannelsFromRiff.Size = new System.Drawing.Size(179, 17);
            this.rbGetChannelsFromRiff.TabIndex = 7;
            this.rbGetChannelsFromRiff.TabStop = true;
            this.rbGetChannelsFromRiff.Text = "Get Channels From RIFF Header";
            this.rbGetChannelsFromRiff.UseVisualStyleBackColor = true;
            this.rbGetChannelsFromRiff.CheckedChanged += new System.EventHandler(this.rbGetChannelsFromRiff_CheckedChanged);
            // 
            // comboRiffChannels
            // 
            this.comboRiffChannels.FormattingEnabled = true;
            this.comboRiffChannels.Location = new System.Drawing.Point(184, 18);
            this.comboRiffChannels.Name = "comboRiffChannels";
            this.comboRiffChannels.Size = new System.Drawing.Size(77, 21);
            this.comboRiffChannels.TabIndex = 3;
            this.comboRiffChannels.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboRiffChannels_KeyPress);
            this.comboRiffChannels.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboRiffChannels_KeyDown);
            // 
            // cbAddRiffHeader
            // 
            this.cbAddRiffHeader.AutoSize = true;
            this.cbAddRiffHeader.Location = new System.Drawing.Point(3, 19);
            this.cbAddRiffHeader.Name = "cbAddRiffHeader";
            this.cbAddRiffHeader.Size = new System.Drawing.Size(109, 17);
            this.cbAddRiffHeader.TabIndex = 4;
            this.cbAddRiffHeader.Text = "Add RIFF Header";
            this.cbAddRiffHeader.UseVisualStyleBackColor = true;
            this.cbAddRiffHeader.CheckedChanged += new System.EventHandler(this.cbAddRiffHeader_CheckedChanged);
            // 
            // grpXmaParseOptions
            // 
            this.grpXmaParseOptions.Controls.Add(this.grpStartOffset);
            this.grpXmaParseOptions.Controls.Add(this.grpBlockSize);
            this.grpXmaParseOptions.Controls.Add(this.grpXmaParseDataSize);
            this.grpXmaParseOptions.Controls.Add(this.cbXmaParseIgnoreErrors);
            this.grpXmaParseOptions.Controls.Add(this.cbXmaParseDoRebuild);
            this.grpXmaParseOptions.Controls.Add(this.cbDoXmaParse);
            this.grpXmaParseOptions.Controls.Add(this.comboXmaParseInputType);
            this.grpXmaParseOptions.Controls.Add(this.lblXmaParseInputType);
            this.grpXmaParseOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpXmaParseOptions.Location = new System.Drawing.Point(0, 0);
            this.grpXmaParseOptions.Name = "grpXmaParseOptions";
            this.grpXmaParseOptions.Size = new System.Drawing.Size(834, 310);
            this.grpXmaParseOptions.TabIndex = 0;
            this.grpXmaParseOptions.TabStop = false;
            this.grpXmaParseOptions.Text = "1. xma_parse Options";
            // 
            // grpStartOffset
            // 
            this.grpStartOffset.Controls.Add(this.rbStartOffsetIsAfterRiff);
            this.grpStartOffset.Controls.Add(this.XmaParseStartOffsetOffsetDescription);
            this.grpStartOffset.Controls.Add(this.rbXmaParseStartOffsetOffset);
            this.grpStartOffset.Controls.Add(this.rbXmaParseStartOffsetStatic);
            this.grpStartOffset.Controls.Add(this.tbXmaParseStartOffset);
            this.grpStartOffset.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpStartOffset.Location = new System.Drawing.Point(3, 61);
            this.grpStartOffset.Name = "grpStartOffset";
            this.grpStartOffset.Size = new System.Drawing.Size(828, 86);
            this.grpStartOffset.TabIndex = 13;
            this.grpStartOffset.TabStop = false;
            this.grpStartOffset.Text = "Start Offset";
            // 
            // rbStartOffsetIsAfterRiff
            // 
            this.rbStartOffsetIsAfterRiff.AutoSize = true;
            this.rbStartOffsetIsAfterRiff.Location = new System.Drawing.Point(6, 65);
            this.rbStartOffsetIsAfterRiff.Name = "rbStartOffsetIsAfterRiff";
            this.rbStartOffsetIsAfterRiff.Size = new System.Drawing.Size(185, 17);
            this.rbStartOffsetIsAfterRiff.TabIndex = 7;
            this.rbStartOffsetIsAfterRiff.TabStop = true;
            this.rbStartOffsetIsAfterRiff.Text = "Set Start Offset after RIFF Header";
            this.rbStartOffsetIsAfterRiff.UseVisualStyleBackColor = true;
            this.rbStartOffsetIsAfterRiff.CheckedChanged += new System.EventHandler(this.rbStartOffsetIsAfterRiff_CheckedChanged);
            // 
            // XmaParseStartOffsetOffsetDescription
            // 
            this.XmaParseStartOffsetOffsetDescription.Location = new System.Drawing.Point(136, 39);
            this.XmaParseStartOffsetOffsetDescription.Name = "XmaParseStartOffsetOffsetDescription";
            this.XmaParseStartOffsetOffsetDescription.OffsetByteOrder = "Little Endian";
            this.XmaParseStartOffsetOffsetDescription.OffsetSize = "4";
            this.XmaParseStartOffsetOffsetDescription.OffsetValue = "";
            this.XmaParseStartOffsetOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.XmaParseStartOffsetOffsetDescription.TabIndex = 6;
            // 
            // rbXmaParseStartOffsetOffset
            // 
            this.rbXmaParseStartOffsetOffset.AutoSize = true;
            this.rbXmaParseStartOffsetOffset.Location = new System.Drawing.Point(6, 42);
            this.rbXmaParseStartOffsetOffset.Name = "rbXmaParseStartOffsetOffset";
            this.rbXmaParseStartOffsetOffset.Size = new System.Drawing.Size(123, 17);
            this.rbXmaParseStartOffsetOffset.TabIndex = 5;
            this.rbXmaParseStartOffsetOffset.TabStop = true;
            this.rbXmaParseStartOffsetOffset.Text = "Start Size is at Offset";
            this.rbXmaParseStartOffsetOffset.UseVisualStyleBackColor = true;
            this.rbXmaParseStartOffsetOffset.CheckedChanged += new System.EventHandler(this.rbXmaParseStartOffsetOffset_CheckedChanged);
            // 
            // rbXmaParseStartOffsetStatic
            // 
            this.rbXmaParseStartOffsetStatic.AutoSize = true;
            this.rbXmaParseStartOffsetStatic.Location = new System.Drawing.Point(6, 19);
            this.rbXmaParseStartOffsetStatic.Name = "rbXmaParseStartOffsetStatic";
            this.rbXmaParseStartOffsetStatic.Size = new System.Drawing.Size(78, 17);
            this.rbXmaParseStartOffsetStatic.TabIndex = 4;
            this.rbXmaParseStartOffsetStatic.TabStop = true;
            this.rbXmaParseStartOffsetStatic.Text = "Start Offset";
            this.rbXmaParseStartOffsetStatic.UseVisualStyleBackColor = true;
            this.rbXmaParseStartOffsetStatic.CheckedChanged += new System.EventHandler(this.rbXmaParseStartOffsetStatic_CheckedChanged);
            // 
            // tbXmaParseStartOffset
            // 
            this.tbXmaParseStartOffset.Location = new System.Drawing.Point(136, 18);
            this.tbXmaParseStartOffset.Name = "tbXmaParseStartOffset";
            this.tbXmaParseStartOffset.Size = new System.Drawing.Size(70, 20);
            this.tbXmaParseStartOffset.TabIndex = 3;
            // 
            // grpBlockSize
            // 
            this.grpBlockSize.Controls.Add(this.XmaParseBlockSizeOffsetDescription);
            this.grpBlockSize.Controls.Add(this.rbXmaParseBlockSizeOffset);
            this.grpBlockSize.Controls.Add(this.rbXmaParseBlockSizeStatic);
            this.grpBlockSize.Controls.Add(this.tbXmaParseBlockSize);
            this.grpBlockSize.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpBlockSize.Location = new System.Drawing.Point(3, 147);
            this.grpBlockSize.Name = "grpBlockSize";
            this.grpBlockSize.Size = new System.Drawing.Size(828, 69);
            this.grpBlockSize.TabIndex = 12;
            this.grpBlockSize.TabStop = false;
            this.grpBlockSize.Text = "Block Size";
            // 
            // XmaParseBlockSizeOffsetDescription
            // 
            this.XmaParseBlockSizeOffsetDescription.Location = new System.Drawing.Point(136, 40);
            this.XmaParseBlockSizeOffsetDescription.Name = "XmaParseBlockSizeOffsetDescription";
            this.XmaParseBlockSizeOffsetDescription.OffsetByteOrder = "Little Endian";
            this.XmaParseBlockSizeOffsetDescription.OffsetSize = "4";
            this.XmaParseBlockSizeOffsetDescription.OffsetValue = "";
            this.XmaParseBlockSizeOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.XmaParseBlockSizeOffsetDescription.TabIndex = 6;
            // 
            // rbXmaParseBlockSizeOffset
            // 
            this.rbXmaParseBlockSizeOffset.AutoSize = true;
            this.rbXmaParseBlockSizeOffset.Location = new System.Drawing.Point(6, 45);
            this.rbXmaParseBlockSizeOffset.Name = "rbXmaParseBlockSizeOffset";
            this.rbXmaParseBlockSizeOffset.Size = new System.Drawing.Size(128, 17);
            this.rbXmaParseBlockSizeOffset.TabIndex = 3;
            this.rbXmaParseBlockSizeOffset.TabStop = true;
            this.rbXmaParseBlockSizeOffset.Text = "Block Size is at Offset";
            this.rbXmaParseBlockSizeOffset.UseVisualStyleBackColor = true;
            this.rbXmaParseBlockSizeOffset.CheckedChanged += new System.EventHandler(this.rbXmaParseBlockSizeOffset_CheckedChanged);
            // 
            // rbXmaParseBlockSizeStatic
            // 
            this.rbXmaParseBlockSizeStatic.AutoSize = true;
            this.rbXmaParseBlockSizeStatic.Location = new System.Drawing.Point(6, 19);
            this.rbXmaParseBlockSizeStatic.Name = "rbXmaParseBlockSizeStatic";
            this.rbXmaParseBlockSizeStatic.Size = new System.Drawing.Size(75, 17);
            this.rbXmaParseBlockSizeStatic.TabIndex = 2;
            this.rbXmaParseBlockSizeStatic.TabStop = true;
            this.rbXmaParseBlockSizeStatic.Text = "Block Size";
            this.rbXmaParseBlockSizeStatic.UseVisualStyleBackColor = true;
            this.rbXmaParseBlockSizeStatic.CheckedChanged += new System.EventHandler(this.rbXmaParseBlockSizeStatic_CheckedChanged);
            // 
            // tbXmaParseBlockSize
            // 
            this.tbXmaParseBlockSize.Location = new System.Drawing.Point(136, 18);
            this.tbXmaParseBlockSize.Name = "tbXmaParseBlockSize";
            this.tbXmaParseBlockSize.Size = new System.Drawing.Size(70, 20);
            this.tbXmaParseBlockSize.TabIndex = 5;
            // 
            // grpXmaParseDataSize
            // 
            this.grpXmaParseDataSize.Controls.Add(this.rbGetDataSizeFromRiff);
            this.grpXmaParseDataSize.Controls.Add(this.XmaParseDataSizeOffsetDescription);
            this.grpXmaParseDataSize.Controls.Add(this.rbXmaParseDataSizeOffset);
            this.grpXmaParseDataSize.Controls.Add(this.rbXmaParseDataSizeStatic);
            this.grpXmaParseDataSize.Controls.Add(this.tbXmaParseDataSize);
            this.grpXmaParseDataSize.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpXmaParseDataSize.Location = new System.Drawing.Point(3, 216);
            this.grpXmaParseDataSize.Name = "grpXmaParseDataSize";
            this.grpXmaParseDataSize.Size = new System.Drawing.Size(828, 91);
            this.grpXmaParseDataSize.TabIndex = 11;
            this.grpXmaParseDataSize.TabStop = false;
            this.grpXmaParseDataSize.Text = "Data Size";
            // 
            // rbGetDataSizeFromRiff
            // 
            this.rbGetDataSizeFromRiff.AutoSize = true;
            this.rbGetDataSizeFromRiff.Location = new System.Drawing.Point(6, 67);
            this.rbGetDataSizeFromRiff.Name = "rbGetDataSizeFromRiff";
            this.rbGetDataSizeFromRiff.Size = new System.Drawing.Size(178, 17);
            this.rbGetDataSizeFromRiff.TabIndex = 11;
            this.rbGetDataSizeFromRiff.TabStop = true;
            this.rbGetDataSizeFromRiff.Text = "Get Data Size from RIFF Header";
            this.rbGetDataSizeFromRiff.UseVisualStyleBackColor = true;
            this.rbGetDataSizeFromRiff.CheckedChanged += new System.EventHandler(this.rbGetDataSizeFromRiff_CheckedChanged);
            // 
            // XmaParseDataSizeOffsetDescription
            // 
            this.XmaParseDataSizeOffsetDescription.Location = new System.Drawing.Point(136, 40);
            this.XmaParseDataSizeOffsetDescription.Name = "XmaParseDataSizeOffsetDescription";
            this.XmaParseDataSizeOffsetDescription.OffsetByteOrder = "Little Endian";
            this.XmaParseDataSizeOffsetDescription.OffsetSize = "4";
            this.XmaParseDataSizeOffsetDescription.OffsetValue = "";
            this.XmaParseDataSizeOffsetDescription.Size = new System.Drawing.Size(372, 27);
            this.XmaParseDataSizeOffsetDescription.TabIndex = 10;
            // 
            // rbXmaParseDataSizeOffset
            // 
            this.rbXmaParseDataSizeOffset.AutoSize = true;
            this.rbXmaParseDataSizeOffset.Location = new System.Drawing.Point(6, 44);
            this.rbXmaParseDataSizeOffset.Name = "rbXmaParseDataSizeOffset";
            this.rbXmaParseDataSizeOffset.Size = new System.Drawing.Size(124, 17);
            this.rbXmaParseDataSizeOffset.TabIndex = 1;
            this.rbXmaParseDataSizeOffset.TabStop = true;
            this.rbXmaParseDataSizeOffset.Text = "Data Size is at Offset";
            this.rbXmaParseDataSizeOffset.UseVisualStyleBackColor = true;
            this.rbXmaParseDataSizeOffset.CheckedChanged += new System.EventHandler(this.rbXmaParseDataSizeOffset_CheckedChanged);
            // 
            // rbXmaParseDataSizeStatic
            // 
            this.rbXmaParseDataSizeStatic.AutoSize = true;
            this.rbXmaParseDataSizeStatic.Location = new System.Drawing.Point(6, 19);
            this.rbXmaParseDataSizeStatic.Name = "rbXmaParseDataSizeStatic";
            this.rbXmaParseDataSizeStatic.Size = new System.Drawing.Size(71, 17);
            this.rbXmaParseDataSizeStatic.TabIndex = 0;
            this.rbXmaParseDataSizeStatic.TabStop = true;
            this.rbXmaParseDataSizeStatic.Text = "Data Size";
            this.rbXmaParseDataSizeStatic.UseVisualStyleBackColor = true;
            this.rbXmaParseDataSizeStatic.CheckedChanged += new System.EventHandler(this.rbXmaParseDataSizeStatic_CheckedChanged);
            // 
            // tbXmaParseDataSize
            // 
            this.tbXmaParseDataSize.Location = new System.Drawing.Point(136, 18);
            this.tbXmaParseDataSize.Name = "tbXmaParseDataSize";
            this.tbXmaParseDataSize.Size = new System.Drawing.Size(70, 20);
            this.tbXmaParseDataSize.TabIndex = 9;
            // 
            // cbXmaParseIgnoreErrors
            // 
            this.cbXmaParseIgnoreErrors.AutoSize = true;
            this.cbXmaParseIgnoreErrors.Location = new System.Drawing.Point(315, 36);
            this.cbXmaParseIgnoreErrors.Name = "cbXmaParseIgnoreErrors";
            this.cbXmaParseIgnoreErrors.Size = new System.Drawing.Size(86, 17);
            this.cbXmaParseIgnoreErrors.TabIndex = 10;
            this.cbXmaParseIgnoreErrors.Text = "Ignore Errors";
            this.cbXmaParseIgnoreErrors.UseVisualStyleBackColor = true;
            // 
            // cbXmaParseDoRebuild
            // 
            this.cbXmaParseDoRebuild.AutoSize = true;
            this.cbXmaParseDoRebuild.Location = new System.Drawing.Point(148, 36);
            this.cbXmaParseDoRebuild.Name = "cbXmaParseDoRebuild";
            this.cbXmaParseDoRebuild.Size = new System.Drawing.Size(161, 17);
            this.cbXmaParseDoRebuild.TabIndex = 7;
            this.cbXmaParseDoRebuild.Text = "Use Rebuild XMA1 Mode (-r)";
            this.cbXmaParseDoRebuild.UseVisualStyleBackColor = true;
            // 
            // cbDoXmaParse
            // 
            this.cbDoXmaParse.AutoSize = true;
            this.cbDoXmaParse.Location = new System.Drawing.Point(3, 19);
            this.cbDoXmaParse.Name = "cbDoXmaParse";
            this.cbDoXmaParse.Size = new System.Drawing.Size(119, 17);
            this.cbDoXmaParse.TabIndex = 6;
            this.cbDoXmaParse.Text = "Use xma_parse.exe";
            this.cbDoXmaParse.UseVisualStyleBackColor = true;
            this.cbDoXmaParse.CheckedChanged += new System.EventHandler(this.cbDoXmaParse_CheckedChanged);
            // 
            // comboXmaParseInputType
            // 
            this.comboXmaParseInputType.FormattingEnabled = true;
            this.comboXmaParseInputType.Location = new System.Drawing.Point(86, 36);
            this.comboXmaParseInputType.Name = "comboXmaParseInputType";
            this.comboXmaParseInputType.Size = new System.Drawing.Size(50, 21);
            this.comboXmaParseInputType.TabIndex = 1;
            this.comboXmaParseInputType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboXmaParseInputType_KeyPress);
            this.comboXmaParseInputType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboXmaParseInputType_KeyDown);
            // 
            // lblXmaParseInputType
            // 
            this.lblXmaParseInputType.AutoSize = true;
            this.lblXmaParseInputType.Location = new System.Drawing.Point(20, 39);
            this.lblXmaParseInputType.Name = "lblXmaParseInputType";
            this.lblXmaParseInputType.Size = new System.Drawing.Size(57, 13);
            this.lblXmaParseInputType.TabIndex = 0;
            this.lblXmaParseInputType.Text = "XMA Type";
            // 
            // XmaConvertForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 702);
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.grpOutputOptions);
            this.Controls.Add(this.grpPresets);
            this.Name = "XmaConvertForm";
            this.Text = "XmaConvertForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.XmaConvertForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.XmaConvertForm_DragEnter);
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpPresets, 0);
            this.Controls.SetChildIndex(this.grpOutputOptions, 0);
            this.Controls.SetChildIndex(this.pnlOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpPresets.ResumeLayout(false);
            this.grpOutputOptions.ResumeLayout(false);
            this.grpOutputOptions.PerformLayout();
            this.pnlOptions.ResumeLayout(false);
            this.groupOtherOptions.ResumeLayout(false);
            this.groupOtherOptions.PerformLayout();
            this.grpToWavOptions.ResumeLayout(false);
            this.grpToWavOptions.PerformLayout();
            this.grpPosFileMaker.ResumeLayout(false);
            this.grpPosFileMaker.PerformLayout();
            this.grpPosEndOffset.ResumeLayout(false);
            this.grpPosEndOffset.PerformLayout();
            this.grpPosStartOffset.ResumeLayout(false);
            this.grpPosStartOffset.PerformLayout();
            this.grpRiffHeaderOptions.ResumeLayout(false);
            this.grpRiffHeaderOptions.PerformLayout();
            this.grpFrequency.ResumeLayout(false);
            this.grpFrequency.PerformLayout();
            this.grpChannels.ResumeLayout(false);
            this.grpChannels.PerformLayout();
            this.grpXmaParseOptions.ResumeLayout(false);
            this.grpXmaParseOptions.PerformLayout();
            this.grpStartOffset.ResumeLayout(false);
            this.grpStartOffset.PerformLayout();
            this.grpBlockSize.ResumeLayout(false);
            this.grpBlockSize.PerformLayout();
            this.grpXmaParseDataSize.ResumeLayout(false);
            this.grpXmaParseDataSize.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPresets;
        private System.Windows.Forms.GroupBox grpOutputOptions;
        private System.Windows.Forms.Panel pnlOptions;
        private System.Windows.Forms.GroupBox grpXmaParseOptions;
        private System.Windows.Forms.Label lblXmaParseInputType;
        private System.Windows.Forms.ComboBox comboXmaParseInputType;
        private System.Windows.Forms.TextBox tbXmaParseBlockSize;
        private System.Windows.Forms.TextBox tbXmaParseStartOffset;
        private System.Windows.Forms.GroupBox grpRiffHeaderOptions;
        private System.Windows.Forms.ComboBox comboRiffFrequency;
        private System.Windows.Forms.ComboBox comboRiffChannels;
        private System.Windows.Forms.GroupBox groupOtherOptions;
        private System.Windows.Forms.CheckBox cbShowAllExeOutput;
        private System.Windows.Forms.Label lblOutputFolderDefault;
        private System.Windows.Forms.TextBox tbOutputFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.Button btnBrowseOutputFolder;
        private System.Windows.Forms.CheckBox cbDoXmaParse;
        private System.Windows.Forms.CheckBox cbAddRiffHeader;
        private System.Windows.Forms.CheckBox cbXmaParseDoRebuild;
        private System.Windows.Forms.TextBox tbXmaParseDataSize;
        private System.Windows.Forms.CheckBox cbXmaParseIgnoreErrors;
        private System.Windows.Forms.CheckBox cbKeepTempFiles;
        private System.Windows.Forms.GroupBox grpToWavOptions;
        private System.Windows.Forms.GroupBox grpXmaParseDataSize;
        private System.Windows.Forms.RadioButton rbXmaParseDataSizeOffset;
        private System.Windows.Forms.RadioButton rbXmaParseDataSizeStatic;
        private System.Windows.Forms.GroupBox grpBlockSize;
        private System.Windows.Forms.RadioButton rbXmaParseBlockSizeOffset;
        private System.Windows.Forms.RadioButton rbXmaParseBlockSizeStatic;
        private System.Windows.Forms.GroupBox grpStartOffset;
        private System.Windows.Forms.RadioButton rbXmaParseStartOffsetOffset;
        private System.Windows.Forms.RadioButton rbXmaParseStartOffsetStatic;
        private VGMToolbox.controls.OffsetDescriptionControl XmaParseStartOffsetOffsetDescription;
        private VGMToolbox.controls.OffsetDescriptionControl XmaParseBlockSizeOffsetDescription;
        private VGMToolbox.controls.OffsetDescriptionControl XmaParseDataSizeOffsetDescription;
        private System.Windows.Forms.RadioButton rbDoToWav;
        private System.Windows.Forms.RadioButton rbDoXmaEncode;
        private System.Windows.Forms.RadioButton rbStartOffsetIsAfterRiff;
        private System.Windows.Forms.RadioButton rbGetDataSizeFromRiff;
        private System.Windows.Forms.RadioButton rbGetFrequencyFromRiff;
        private System.Windows.Forms.RadioButton rbAddManualFrequency;
        private System.Windows.Forms.RadioButton rbGetChannelsFromRiff;
        private System.Windows.Forms.RadioButton rbAddManualChannels;
        private System.Windows.Forms.GroupBox grpFrequency;
        private System.Windows.Forms.GroupBox grpChannels;
        private System.Windows.Forms.GroupBox grpPosFileMaker;
        private System.Windows.Forms.CheckBox cbMakePosFile;
        private System.Windows.Forms.GroupBox grpPosEndOffset;
        private System.Windows.Forms.GroupBox grpPosStartOffset;
        private System.Windows.Forms.TextBox tbLoopStartStatic;
        private System.Windows.Forms.RadioButton rbLoopStartStatic;
        private System.Windows.Forms.RadioButton rbLoopStartOffset;
        private VGMToolbox.controls.CalculatingOffsetDescriptionControl loopStartValueOffsetDescription;
        private System.Windows.Forms.RadioButton rbLoopEndOffset;
        private VGMToolbox.controls.CalculatingOffsetDescriptionControl loopEndValueOffsetDescription;
        private System.Windows.Forms.TextBox tbLoopEndStatic;
        private System.Windows.Forms.RadioButton rbLoopEndStatic;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSavePreset;
        private System.Windows.Forms.Button btnLoadPreset;
        private System.Windows.Forms.ComboBox comboPresets;
        private VGMToolbox.controls.OffsetDescriptionControl frequencyOffsetDescription;
        private System.Windows.Forms.RadioButton rbFrequencyOffset;
        private VGMToolbox.controls.OffsetDescriptionControl numberOfChannelsOffsetDescription;
        private System.Windows.Forms.RadioButton rbNumberOfChannelsOffset;
    }
}