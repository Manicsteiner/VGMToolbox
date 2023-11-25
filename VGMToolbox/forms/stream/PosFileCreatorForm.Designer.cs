namespace VGMToolbox.forms.stream
{
    partial class PosFileCreatorForm
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
            this.grpLoopStart = new System.Windows.Forms.GroupBox();
            this.rbLoopStartByteStringOffset = new System.Windows.Forms.RadioButton();
            this.rbLoopStartRiffOffset = new System.Windows.Forms.RadioButton();
            this.rbLoopStartOffset = new System.Windows.Forms.RadioButton();
            this.tbLoopStartStatic = new System.Windows.Forms.TextBox();
            this.rbLoopStartStatic = new System.Windows.Forms.RadioButton();
            this.grpLoopEnd = new System.Windows.Forms.GroupBox();
            this.grpLoopEndLoopLength = new System.Windows.Forms.GroupBox();
            this.rbLoopLength = new System.Windows.Forms.RadioButton();
            this.rbLoopEnd = new System.Windows.Forms.RadioButton();
            this.rbLoopEndRiffOffset = new System.Windows.Forms.RadioButton();
            this.rbLoopEndOffset = new System.Windows.Forms.RadioButton();
            this.tbLoopEndStatic = new System.Windows.Forms.TextBox();
            this.rbLoopEndStatic = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbOptionsCreateM3u = new System.Windows.Forms.CheckBox();
            this.grpLoopShift = new System.Windows.Forms.GroupBox();
            this.cbLoopShiftPredictShift = new System.Windows.Forms.CheckBox();
            this.rbLoopShiftWavCompare = new System.Windows.Forms.RadioButton();
            this.tbLoopShiftStatic = new System.Windows.Forms.TextBox();
            this.rbLoopShiftStatic = new System.Windows.Forms.RadioButton();
            this.grpOutput = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbOutputFileMask = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbLoopEndByteStringOffset = new System.Windows.Forms.RadioButton();
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl = new VGMToolbox.controls.ByteSearchCalculatingOffsetDescriptionControl();
            this.LoopEndCalculatingOffsetDescriptionControl = new VGMToolbox.controls.CalculatingOffsetDescriptionControl();
            this.LoopEndRiffCalculatingOffsetDescriptionControl = new VGMToolbox.controls.RiffCalculatingOffsetDescriptionControl();
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl = new VGMToolbox.controls.ByteSearchCalculatingOffsetDescriptionControl();
            this.LoopStartCalculatingOffsetDescriptionControl = new VGMToolbox.controls.CalculatingOffsetDescriptionControl();
            this.LoopStartRiffCalculatingOffsetDescriptionControl = new VGMToolbox.controls.RiffCalculatingOffsetDescriptionControl();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpLoopStart.SuspendLayout();
            this.grpLoopEnd.SuspendLayout();
            this.grpLoopEndLoopLength.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.grpLoopShift.SuspendLayout();
            this.grpOutput.SuspendLayout();
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
            // 
            // grpPresets
            // 
            this.grpPresets.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPresets.Location = new System.Drawing.Point(0, 0);
            this.grpPresets.Name = "grpPresets";
            this.grpPresets.Size = new System.Drawing.Size(986, 51);
            this.grpPresets.TabIndex = 5;
            this.grpPresets.TabStop = false;
            this.grpPresets.Text = "Presets";
            // 
            // grpLoopStart
            // 
            this.grpLoopStart.Controls.Add(this.rbLoopStartByteStringOffset);
            this.grpLoopStart.Controls.Add(this.LoopStartByteSearchCalculatingOffsetDescriptionControl);
            this.grpLoopStart.Controls.Add(this.rbLoopStartRiffOffset);
            this.grpLoopStart.Controls.Add(this.LoopStartCalculatingOffsetDescriptionControl);
            this.grpLoopStart.Controls.Add(this.rbLoopStartOffset);
            this.grpLoopStart.Controls.Add(this.tbLoopStartStatic);
            this.grpLoopStart.Controls.Add(this.rbLoopStartStatic);
            this.grpLoopStart.Controls.Add(this.LoopStartRiffCalculatingOffsetDescriptionControl);
            this.grpLoopStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLoopStart.Location = new System.Drawing.Point(0, 94);
            this.grpLoopStart.Name = "grpLoopStart";
            this.grpLoopStart.Size = new System.Drawing.Size(986, 269);
            this.grpLoopStart.TabIndex = 7;
            this.grpLoopStart.TabStop = false;
            this.grpLoopStart.Text = "Loop Start";
            // 
            // rbLoopStartByteStringOffset
            // 
            this.rbLoopStartByteStringOffset.AutoSize = true;
            this.rbLoopStartByteStringOffset.Location = new System.Drawing.Point(9, 179);
            this.rbLoopStartByteStringOffset.Name = "rbLoopStartByteStringOffset";
            this.rbLoopStartByteStringOffset.Size = new System.Drawing.Size(96, 17);
            this.rbLoopStartByteStringOffset.TabIndex = 12;
            this.rbLoopStartByteStringOffset.TabStop = true;
            this.rbLoopStartByteStringOffset.Text = "Loop Start is at";
            this.rbLoopStartByteStringOffset.UseVisualStyleBackColor = true;
            this.rbLoopStartByteStringOffset.CheckedChanged += new System.EventHandler(this.rbLoopStartByteStringOffset_CheckedChanged);
            // 
            // rbLoopStartRiffOffset
            // 
            this.rbLoopStartRiffOffset.AutoSize = true;
            this.rbLoopStartRiffOffset.Location = new System.Drawing.Point(6, 96);
            this.rbLoopStartRiffOffset.Name = "rbLoopStartRiffOffset";
            this.rbLoopStartRiffOffset.Size = new System.Drawing.Size(96, 17);
            this.rbLoopStartRiffOffset.TabIndex = 11;
            this.rbLoopStartRiffOffset.TabStop = true;
            this.rbLoopStartRiffOffset.Text = "Loop Start is at";
            this.rbLoopStartRiffOffset.UseVisualStyleBackColor = true;
            this.rbLoopStartRiffOffset.CheckedChanged += new System.EventHandler(this.rbLoopStartRiffOffset_CheckedChanged);
            // 
            // rbLoopStartOffset
            // 
            this.rbLoopStartOffset.AutoSize = true;
            this.rbLoopStartOffset.Location = new System.Drawing.Point(6, 42);
            this.rbLoopStartOffset.Name = "rbLoopStartOffset";
            this.rbLoopStartOffset.Size = new System.Drawing.Size(84, 17);
            this.rbLoopStartOffset.TabIndex = 9;
            this.rbLoopStartOffset.TabStop = true;
            this.rbLoopStartOffset.Text = "Loop Start is";
            this.rbLoopStartOffset.UseVisualStyleBackColor = true;
            this.rbLoopStartOffset.CheckedChanged += new System.EventHandler(this.rbLoopStartOffset_CheckedChanged);
            // 
            // tbLoopStartStatic
            // 
            this.tbLoopStartStatic.Location = new System.Drawing.Point(165, 16);
            this.tbLoopStartStatic.Name = "tbLoopStartStatic";
            this.tbLoopStartStatic.Size = new System.Drawing.Size(100, 20);
            this.tbLoopStartStatic.TabIndex = 8;
            // 
            // rbLoopStartStatic
            // 
            this.rbLoopStartStatic.AutoSize = true;
            this.rbLoopStartStatic.Location = new System.Drawing.Point(6, 19);
            this.rbLoopStartStatic.Name = "rbLoopStartStatic";
            this.rbLoopStartStatic.Size = new System.Drawing.Size(153, 17);
            this.rbLoopStartStatic.TabIndex = 7;
            this.rbLoopStartStatic.TabStop = true;
            this.rbLoopStartStatic.Text = "Loop Start is a Static Value";
            this.rbLoopStartStatic.UseVisualStyleBackColor = true;
            this.rbLoopStartStatic.CheckedChanged += new System.EventHandler(this.rbLoopStartStatic_CheckedChanged);
            // 
            // grpLoopEnd
            // 
            this.grpLoopEnd.Controls.Add(this.rbLoopEndByteStringOffset);
            this.grpLoopEnd.Controls.Add(this.LoopEndByteSearchCalculatingOffsetDescriptionControl);
            this.grpLoopEnd.Controls.Add(this.grpLoopEndLoopLength);
            this.grpLoopEnd.Controls.Add(this.rbLoopEndRiffOffset);
            this.grpLoopEnd.Controls.Add(this.LoopEndCalculatingOffsetDescriptionControl);
            this.grpLoopEnd.Controls.Add(this.rbLoopEndOffset);
            this.grpLoopEnd.Controls.Add(this.tbLoopEndStatic);
            this.grpLoopEnd.Controls.Add(this.rbLoopEndStatic);
            this.grpLoopEnd.Controls.Add(this.LoopEndRiffCalculatingOffsetDescriptionControl);
            this.grpLoopEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLoopEnd.Location = new System.Drawing.Point(0, 363);
            this.grpLoopEnd.Name = "grpLoopEnd";
            this.grpLoopEnd.Size = new System.Drawing.Size(986, 311);
            this.grpLoopEnd.TabIndex = 8;
            this.grpLoopEnd.TabStop = false;
            this.grpLoopEnd.Text = "Loop End/Loop Length";
            // 
            // grpLoopEndLoopLength
            // 
            this.grpLoopEndLoopLength.Controls.Add(this.rbLoopLength);
            this.grpLoopEndLoopLength.Controls.Add(this.rbLoopEnd);
            this.grpLoopEndLoopLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLoopEndLoopLength.Location = new System.Drawing.Point(3, 16);
            this.grpLoopEndLoopLength.Name = "grpLoopEndLoopLength";
            this.grpLoopEndLoopLength.Size = new System.Drawing.Size(980, 37);
            this.grpLoopEndLoopLength.TabIndex = 12;
            this.grpLoopEndLoopLength.TabStop = false;
            this.grpLoopEndLoopLength.Text = "Select Loop End or Loop Length";
            // 
            // rbLoopLength
            // 
            this.rbLoopLength.AutoSize = true;
            this.rbLoopLength.Location = new System.Drawing.Point(83, 19);
            this.rbLoopLength.Name = "rbLoopLength";
            this.rbLoopLength.Size = new System.Drawing.Size(85, 17);
            this.rbLoopLength.TabIndex = 1;
            this.rbLoopLength.TabStop = true;
            this.rbLoopLength.Text = "Loop Length";
            this.rbLoopLength.UseVisualStyleBackColor = true;
            // 
            // rbLoopEnd
            // 
            this.rbLoopEnd.AutoSize = true;
            this.rbLoopEnd.Location = new System.Drawing.Point(6, 19);
            this.rbLoopEnd.Name = "rbLoopEnd";
            this.rbLoopEnd.Size = new System.Drawing.Size(71, 17);
            this.rbLoopEnd.TabIndex = 0;
            this.rbLoopEnd.TabStop = true;
            this.rbLoopEnd.Text = "Loop End";
            this.rbLoopEnd.UseVisualStyleBackColor = true;
            // 
            // rbLoopEndRiffOffset
            // 
            this.rbLoopEndRiffOffset.AutoSize = true;
            this.rbLoopEndRiffOffset.Location = new System.Drawing.Point(6, 136);
            this.rbLoopEndRiffOffset.Name = "rbLoopEndRiffOffset";
            this.rbLoopEndRiffOffset.Size = new System.Drawing.Size(93, 17);
            this.rbLoopEndRiffOffset.TabIndex = 11;
            this.rbLoopEndRiffOffset.TabStop = true;
            this.rbLoopEndRiffOffset.Text = "Loop End is at";
            this.rbLoopEndRiffOffset.UseVisualStyleBackColor = true;
            this.rbLoopEndRiffOffset.CheckedChanged += new System.EventHandler(this.rbLoopEndRiffOffset_CheckedChanged);
            // 
            // rbLoopEndOffset
            // 
            this.rbLoopEndOffset.AutoSize = true;
            this.rbLoopEndOffset.Location = new System.Drawing.Point(6, 82);
            this.rbLoopEndOffset.Name = "rbLoopEndOffset";
            this.rbLoopEndOffset.Size = new System.Drawing.Size(81, 17);
            this.rbLoopEndOffset.TabIndex = 9;
            this.rbLoopEndOffset.TabStop = true;
            this.rbLoopEndOffset.Text = "Loop End is";
            this.rbLoopEndOffset.UseVisualStyleBackColor = true;
            this.rbLoopEndOffset.CheckedChanged += new System.EventHandler(this.rbLoopEndOffset_CheckedChanged);
            // 
            // tbLoopEndStatic
            // 
            this.tbLoopEndStatic.Location = new System.Drawing.Point(165, 56);
            this.tbLoopEndStatic.Name = "tbLoopEndStatic";
            this.tbLoopEndStatic.Size = new System.Drawing.Size(100, 20);
            this.tbLoopEndStatic.TabIndex = 8;
            // 
            // rbLoopEndStatic
            // 
            this.rbLoopEndStatic.AutoSize = true;
            this.rbLoopEndStatic.Location = new System.Drawing.Point(6, 59);
            this.rbLoopEndStatic.Name = "rbLoopEndStatic";
            this.rbLoopEndStatic.Size = new System.Drawing.Size(150, 17);
            this.rbLoopEndStatic.TabIndex = 7;
            this.rbLoopEndStatic.TabStop = true;
            this.rbLoopEndStatic.Text = "Loop End is a Static Value";
            this.rbLoopEndStatic.UseVisualStyleBackColor = true;
            this.rbLoopEndStatic.CheckedChanged += new System.EventHandler(this.rbLoopEndStatic_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.grpOptions);
            this.panel1.Controls.Add(this.grpLoopShift);
            this.panel1.Controls.Add(this.grpLoopEnd);
            this.panel1.Controls.Add(this.grpLoopStart);
            this.panel1.Controls.Add(this.grpOutput);
            this.panel1.Controls.Add(this.grpPresets);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1003, 417);
            this.panel1.TabIndex = 9;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbOptionsCreateM3u);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.Location = new System.Drawing.Point(0, 736);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(986, 93);
            this.grpOptions.TabIndex = 10;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbOptionsCreateM3u
            // 
            this.cbOptionsCreateM3u.AutoSize = true;
            this.cbOptionsCreateM3u.Location = new System.Drawing.Point(6, 19);
            this.cbOptionsCreateM3u.Name = "cbOptionsCreateM3u";
            this.cbOptionsCreateM3u.Size = new System.Drawing.Size(92, 17);
            this.cbOptionsCreateM3u.TabIndex = 0;
            this.cbOptionsCreateM3u.Text = "Create Playlist";
            this.cbOptionsCreateM3u.UseVisualStyleBackColor = true;
            // 
            // grpLoopShift
            // 
            this.grpLoopShift.Controls.Add(this.cbLoopShiftPredictShift);
            this.grpLoopShift.Controls.Add(this.rbLoopShiftWavCompare);
            this.grpLoopShift.Controls.Add(this.tbLoopShiftStatic);
            this.grpLoopShift.Controls.Add(this.rbLoopShiftStatic);
            this.grpLoopShift.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLoopShift.Location = new System.Drawing.Point(0, 674);
            this.grpLoopShift.Name = "grpLoopShift";
            this.grpLoopShift.Size = new System.Drawing.Size(986, 62);
            this.grpLoopShift.TabIndex = 9;
            this.grpLoopShift.TabStop = false;
            this.grpLoopShift.Text = "Loop Shift";
            // 
            // cbLoopShiftPredictShift
            // 
            this.cbLoopShiftPredictShift.AutoSize = true;
            this.cbLoopShiftPredictShift.Location = new System.Drawing.Point(232, 42);
            this.cbLoopShiftPredictShift.Name = "cbLoopShiftPredictShift";
            this.cbLoopShiftPredictShift.Size = new System.Drawing.Size(129, 17);
            this.cbLoopShiftPredictShift.TabIndex = 5;
            this.cbLoopShiftPredictShift.Text = "Predict Shift for Batch";
            this.cbLoopShiftPredictShift.UseVisualStyleBackColor = true;
            // 
            // rbLoopShiftWavCompare
            // 
            this.rbLoopShiftWavCompare.AutoSize = true;
            this.rbLoopShiftWavCompare.Location = new System.Drawing.Point(6, 42);
            this.rbLoopShiftWavCompare.Name = "rbLoopShiftWavCompare";
            this.rbLoopShiftWavCompare.Size = new System.Drawing.Size(220, 17);
            this.rbLoopShiftWavCompare.TabIndex = 2;
            this.rbLoopShiftWavCompare.TabStop = true;
            this.rbLoopShiftWavCompare.Text = "Compare Loop End to total WAV samples";
            this.rbLoopShiftWavCompare.UseVisualStyleBackColor = true;
            this.rbLoopShiftWavCompare.CheckedChanged += new System.EventHandler(this.rbLoopShiftWavCompare_CheckedChanged);
            // 
            // tbLoopShiftStatic
            // 
            this.tbLoopShiftStatic.Location = new System.Drawing.Point(81, 18);
            this.tbLoopShiftStatic.Name = "tbLoopShiftStatic";
            this.tbLoopShiftStatic.Size = new System.Drawing.Size(49, 20);
            this.tbLoopShiftStatic.TabIndex = 1;
            // 
            // rbLoopShiftStatic
            // 
            this.rbLoopShiftStatic.AutoSize = true;
            this.rbLoopShiftStatic.Location = new System.Drawing.Point(6, 19);
            this.rbLoopShiftStatic.Name = "rbLoopShiftStatic";
            this.rbLoopShiftStatic.Size = new System.Drawing.Size(73, 17);
            this.rbLoopShiftStatic.TabIndex = 0;
            this.rbLoopShiftStatic.TabStop = true;
            this.rbLoopShiftStatic.Text = "Loop Shift";
            this.rbLoopShiftStatic.UseVisualStyleBackColor = true;
            this.rbLoopShiftStatic.CheckedChanged += new System.EventHandler(this.rbLoopShiftStatic_CheckedChanged);
            // 
            // grpOutput
            // 
            this.grpOutput.Controls.Add(this.label3);
            this.grpOutput.Controls.Add(this.tbOutputFileMask);
            this.grpOutput.Controls.Add(this.label2);
            this.grpOutput.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOutput.Location = new System.Drawing.Point(0, 51);
            this.grpOutput.Name = "grpOutput";
            this.grpOutput.Size = new System.Drawing.Size(986, 43);
            this.grpOutput.TabIndex = 11;
            this.grpOutput.TabStop = false;
            this.grpOutput.Text = "Output Options";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(275, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "File Mask ($B: Base, $E: Extension, *: Wildcard)";
            // 
            // tbOutputFileMask
            // 
            this.tbOutputFileMask.Location = new System.Drawing.Point(96, 13);
            this.tbOutputFileMask.Name = "tbOutputFileMask";
            this.tbOutputFileMask.Size = new System.Drawing.Size(169, 20);
            this.tbOutputFileMask.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Output File Mask";
            // 
            // rbLoopEndByteStringOffset
            // 
            this.rbLoopEndByteStringOffset.AutoSize = true;
            this.rbLoopEndByteStringOffset.Location = new System.Drawing.Point(6, 219);
            this.rbLoopEndByteStringOffset.Name = "rbLoopEndByteStringOffset";
            this.rbLoopEndByteStringOffset.Size = new System.Drawing.Size(93, 17);
            this.rbLoopEndByteStringOffset.TabIndex = 14;
            this.rbLoopEndByteStringOffset.TabStop = true;
            this.rbLoopEndByteStringOffset.Text = "Loop End is at";
            this.rbLoopEndByteStringOffset.UseVisualStyleBackColor = true;
            this.rbLoopEndByteStringOffset.CheckedChanged += new System.EventHandler(this.rbLoopEndByteStringOffset_CheckedChanged);
            // 
            // LoopEndByteSearchCalculatingOffsetDescriptionControl
            // 
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl.CalculationValue = "";
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl.Location = new System.Drawing.Point(95, 214);
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl.Name = "LoopEndByteSearchCalculatingOffsetDescriptionControl";
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl.OffsetByteOrder = "Little Endian";
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl.OffsetSize = "4";
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl.OffsetValue = "";
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl.Size = new System.Drawing.Size(404, 91);
            this.LoopEndByteSearchCalculatingOffsetDescriptionControl.TabIndex = 15;
            // 
            // LoopEndCalculatingOffsetDescriptionControl
            // 
            this.LoopEndCalculatingOffsetDescriptionControl.CalculationValue = "";
            this.LoopEndCalculatingOffsetDescriptionControl.Location = new System.Drawing.Point(84, 78);
            this.LoopEndCalculatingOffsetDescriptionControl.Name = "LoopEndCalculatingOffsetDescriptionControl";
            this.LoopEndCalculatingOffsetDescriptionControl.OffsetByteOrder = "Little Endian";
            this.LoopEndCalculatingOffsetDescriptionControl.OffsetSize = "4";
            this.LoopEndCalculatingOffsetDescriptionControl.OffsetValue = "";
            this.LoopEndCalculatingOffsetDescriptionControl.Size = new System.Drawing.Size(422, 52);
            this.LoopEndCalculatingOffsetDescriptionControl.TabIndex = 10;
            // 
            // LoopEndRiffCalculatingOffsetDescriptionControl
            // 
            this.LoopEndRiffCalculatingOffsetDescriptionControl.CalculationValue = "";
            this.LoopEndRiffCalculatingOffsetDescriptionControl.Location = new System.Drawing.Point(99, 132);
            this.LoopEndRiffCalculatingOffsetDescriptionControl.Name = "LoopEndRiffCalculatingOffsetDescriptionControl";
            this.LoopEndRiffCalculatingOffsetDescriptionControl.OffsetByteOrder = "Little Endian";
            this.LoopEndRiffCalculatingOffsetDescriptionControl.OffsetSize = "4";
            this.LoopEndRiffCalculatingOffsetDescriptionControl.OffsetValue = "";
            this.LoopEndRiffCalculatingOffsetDescriptionControl.Size = new System.Drawing.Size(400, 81);
            this.LoopEndRiffCalculatingOffsetDescriptionControl.TabIndex = 6;
            // 
            // LoopStartByteSearchCalculatingOffsetDescriptionControl
            // 
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl.CalculationValue = "";
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl.Location = new System.Drawing.Point(98, 174);
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl.Name = "LoopStartByteSearchCalculatingOffsetDescriptionControl";
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl.OffsetByteOrder = "Little Endian";
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl.OffsetSize = "4";
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl.OffsetValue = "";
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl.Size = new System.Drawing.Size(404, 91);
            this.LoopStartByteSearchCalculatingOffsetDescriptionControl.TabIndex = 13;
            // 
            // LoopStartCalculatingOffsetDescriptionControl
            // 
            this.LoopStartCalculatingOffsetDescriptionControl.CalculationValue = "";
            this.LoopStartCalculatingOffsetDescriptionControl.Location = new System.Drawing.Point(86, 38);
            this.LoopStartCalculatingOffsetDescriptionControl.Name = "LoopStartCalculatingOffsetDescriptionControl";
            this.LoopStartCalculatingOffsetDescriptionControl.OffsetByteOrder = "Little Endian";
            this.LoopStartCalculatingOffsetDescriptionControl.OffsetSize = "4";
            this.LoopStartCalculatingOffsetDescriptionControl.OffsetValue = "";
            this.LoopStartCalculatingOffsetDescriptionControl.Size = new System.Drawing.Size(422, 52);
            this.LoopStartCalculatingOffsetDescriptionControl.TabIndex = 10;
            // 
            // LoopStartRiffCalculatingOffsetDescriptionControl
            // 
            this.LoopStartRiffCalculatingOffsetDescriptionControl.CalculationValue = "";
            this.LoopStartRiffCalculatingOffsetDescriptionControl.Location = new System.Drawing.Point(102, 92);
            this.LoopStartRiffCalculatingOffsetDescriptionControl.Name = "LoopStartRiffCalculatingOffsetDescriptionControl";
            this.LoopStartRiffCalculatingOffsetDescriptionControl.OffsetByteOrder = "Little Endian";
            this.LoopStartRiffCalculatingOffsetDescriptionControl.OffsetSize = "4";
            this.LoopStartRiffCalculatingOffsetDescriptionControl.OffsetValue = "";
            this.LoopStartRiffCalculatingOffsetDescriptionControl.Size = new System.Drawing.Size(400, 81);
            this.LoopStartRiffCalculatingOffsetDescriptionControl.TabIndex = 6;
            // 
            // PosFileCreatorForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 578);
            this.Controls.Add(this.panel1);
            this.Name = "PosFileCreatorForm";
            this.Text = "Atrac3PosFileCreatorForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.PosFileCreatorForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.PosFileCreatorForm_DragEnter);
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpLoopStart.ResumeLayout(false);
            this.grpLoopStart.PerformLayout();
            this.grpLoopEnd.ResumeLayout(false);
            this.grpLoopEnd.PerformLayout();
            this.grpLoopEndLoopLength.ResumeLayout(false);
            this.grpLoopEndLoopLength.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpLoopShift.ResumeLayout(false);
            this.grpLoopShift.PerformLayout();
            this.grpOutput.ResumeLayout(false);
            this.grpOutput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPresets;
        private VGMToolbox.controls.RiffCalculatingOffsetDescriptionControl LoopStartRiffCalculatingOffsetDescriptionControl;
        private System.Windows.Forms.GroupBox grpLoopStart;
        private VGMToolbox.controls.CalculatingOffsetDescriptionControl LoopStartCalculatingOffsetDescriptionControl;
        private System.Windows.Forms.RadioButton rbLoopStartOffset;
        private System.Windows.Forms.TextBox tbLoopStartStatic;
        private System.Windows.Forms.RadioButton rbLoopStartStatic;
        private System.Windows.Forms.RadioButton rbLoopStartRiffOffset;
        private System.Windows.Forms.GroupBox grpLoopEnd;
        private System.Windows.Forms.RadioButton rbLoopEndRiffOffset;
        private VGMToolbox.controls.CalculatingOffsetDescriptionControl LoopEndCalculatingOffsetDescriptionControl;
        private System.Windows.Forms.RadioButton rbLoopEndOffset;
        private System.Windows.Forms.TextBox tbLoopEndStatic;
        private System.Windows.Forms.RadioButton rbLoopEndStatic;
        private VGMToolbox.controls.RiffCalculatingOffsetDescriptionControl LoopEndRiffCalculatingOffsetDescriptionControl;
        private System.Windows.Forms.GroupBox grpLoopEndLoopLength;
        private System.Windows.Forms.RadioButton rbLoopLength;
        private System.Windows.Forms.RadioButton rbLoopEnd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.GroupBox grpLoopShift;
        private System.Windows.Forms.TextBox tbLoopShiftStatic;
        private System.Windows.Forms.RadioButton rbLoopShiftStatic;
        private System.Windows.Forms.RadioButton rbLoopShiftWavCompare;
        private System.Windows.Forms.CheckBox cbOptionsCreateM3u;
        private System.Windows.Forms.CheckBox cbLoopShiftPredictShift;
        private System.Windows.Forms.GroupBox grpOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbOutputFileMask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbLoopStartByteStringOffset;
        private VGMToolbox.controls.ByteSearchCalculatingOffsetDescriptionControl LoopStartByteSearchCalculatingOffsetDescriptionControl;
        private System.Windows.Forms.RadioButton rbLoopEndByteStringOffset;
        private VGMToolbox.controls.ByteSearchCalculatingOffsetDescriptionControl LoopEndByteSearchCalculatingOffsetDescriptionControl;
    }
}