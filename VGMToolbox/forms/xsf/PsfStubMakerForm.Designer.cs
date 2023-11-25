namespace VGMToolbox.forms.xsf
{
    partial class PsfStubMakerForm
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
            this.grpSourceFiles = new System.Windows.Forms.GroupBox();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.btnHighestAddress = new System.Windows.Forms.Button();
            this.cbOverrideDriverOffset = new System.Windows.Forms.CheckBox();
            this.btnLoadDefaults = new System.Windows.Forms.Button();
            this.lblMyVbSize = new System.Windows.Forms.Label();
            this.tbMyVbSize = new System.Windows.Forms.TextBox();
            this.tbMyVb = new System.Windows.Forms.TextBox();
            this.lblMyVb = new System.Windows.Forms.Label();
            this.lblMyVhSize = new System.Windows.Forms.Label();
            this.tbMyVhSize = new System.Windows.Forms.TextBox();
            this.tbMyVh = new System.Windows.Forms.TextBox();
            this.lblMyVh = new System.Windows.Forms.Label();
            this.lblMySeq = new System.Windows.Forms.Label();
            this.tbMySeq = new System.Windows.Forms.TextBox();
            this.tbMySeqSize = new System.Windows.Forms.TextBox();
            this.lblMySeqSize = new System.Windows.Forms.Label();
            this.lblPadDrvParamSize = new System.Windows.Forms.Label();
            this.tbPadDrvParamSize = new System.Windows.Forms.TextBox();
            this.lblPsfDrvParam = new System.Windows.Forms.Label();
            this.tbPsfDrvParam = new System.Windows.Forms.TextBox();
            this.lblPsfDrvSize = new System.Windows.Forms.Label();
            this.tbPsfDrvSize = new System.Windows.Forms.TextBox();
            this.tbPsfDrvLoad = new System.Windows.Forms.TextBox();
            this.lblPsfDrvLoad = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbRelaxPsfLoadAddress = new System.Windows.Forms.CheckBox();
            this.cbSepFunctions = new System.Windows.Forms.RadioButton();
            this.cbSeqFunctions = new System.Windows.Forms.RadioButton();
            this.cbIncludeReverb = new System.Windows.Forms.CheckBox();
            this.tbDriverText = new System.Windows.Forms.TextBox();
            this.lblDriverText = new System.Windows.Forms.Label();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSourceFiles.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 507);
            this.pnlLabels.Size = new System.Drawing.Size(787, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(787, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 430);
            this.tbOutput.Size = new System.Drawing.Size(787, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 410);
            this.pnlButtons.Size = new System.Drawing.Size(787, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(727, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(667, 0);
            // 
            // grpSourceFiles
            // 
            this.grpSourceFiles.Controls.Add(this.grpSettings);
            this.grpSourceFiles.Controls.Add(this.grpOptions);
            this.grpSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSourceFiles.Location = new System.Drawing.Point(0, 23);
            this.grpSourceFiles.Name = "grpSourceFiles";
            this.grpSourceFiles.Size = new System.Drawing.Size(787, 387);
            this.grpSourceFiles.TabIndex = 5;
            this.grpSourceFiles.TabStop = false;
            this.grpSourceFiles.Text = "Drop Files Here";
            this.grpSourceFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.grpSourceFiles_DragDrop);
            this.grpSourceFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.btnHighestAddress);
            this.grpSettings.Controls.Add(this.cbOverrideDriverOffset);
            this.grpSettings.Controls.Add(this.btnLoadDefaults);
            this.grpSettings.Controls.Add(this.lblMyVbSize);
            this.grpSettings.Controls.Add(this.tbMyVbSize);
            this.grpSettings.Controls.Add(this.tbMyVb);
            this.grpSettings.Controls.Add(this.lblMyVb);
            this.grpSettings.Controls.Add(this.lblMyVhSize);
            this.grpSettings.Controls.Add(this.tbMyVhSize);
            this.grpSettings.Controls.Add(this.tbMyVh);
            this.grpSettings.Controls.Add(this.lblMyVh);
            this.grpSettings.Controls.Add(this.lblMySeq);
            this.grpSettings.Controls.Add(this.tbMySeq);
            this.grpSettings.Controls.Add(this.tbMySeqSize);
            this.grpSettings.Controls.Add(this.lblMySeqSize);
            this.grpSettings.Controls.Add(this.lblPadDrvParamSize);
            this.grpSettings.Controls.Add(this.tbPadDrvParamSize);
            this.grpSettings.Controls.Add(this.lblPsfDrvParam);
            this.grpSettings.Controls.Add(this.tbPsfDrvParam);
            this.grpSettings.Controls.Add(this.lblPsfDrvSize);
            this.grpSettings.Controls.Add(this.tbPsfDrvSize);
            this.grpSettings.Controls.Add(this.tbPsfDrvLoad);
            this.grpSettings.Controls.Add(this.lblPsfDrvLoad);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpSettings.Location = new System.Drawing.Point(3, 109);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(781, 167);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings/Offsets";
            // 
            // btnHighestAddress
            // 
            this.btnHighestAddress.Location = new System.Drawing.Point(100, 143);
            this.btnHighestAddress.Name = "btnHighestAddress";
            this.btnHighestAddress.Size = new System.Drawing.Size(106, 20);
            this.btnHighestAddress.TabIndex = 22;
            this.btnHighestAddress.Text = "Highest Address";
            this.btnHighestAddress.UseVisualStyleBackColor = true;
            this.btnHighestAddress.Click += new System.EventHandler(this.btnHighestAddress_Click);
            // 
            // cbOverrideDriverOffset
            // 
            this.cbOverrideDriverOffset.AutoSize = true;
            this.cbOverrideDriverOffset.Location = new System.Drawing.Point(9, 19);
            this.cbOverrideDriverOffset.Name = "cbOverrideDriverOffset";
            this.cbOverrideDriverOffset.Size = new System.Drawing.Size(108, 17);
            this.cbOverrideDriverOffset.TabIndex = 21;
            this.cbOverrideDriverOffset.Text = "Override Defaults";
            this.cbOverrideDriverOffset.UseVisualStyleBackColor = true;
            this.cbOverrideDriverOffset.CheckedChanged += new System.EventHandler(this.cbOverrideDriverOffset_CheckedChanged);
            // 
            // btnLoadDefaults
            // 
            this.btnLoadDefaults.Location = new System.Drawing.Point(9, 143);
            this.btnLoadDefaults.Name = "btnLoadDefaults";
            this.btnLoadDefaults.Size = new System.Drawing.Size(85, 20);
            this.btnLoadDefaults.TabIndex = 20;
            this.btnLoadDefaults.Text = "Load Defaults";
            this.btnLoadDefaults.UseVisualStyleBackColor = true;
            this.btnLoadDefaults.Click += new System.EventHandler(this.btnLoadDefaults_Click);
            // 
            // lblMyVbSize
            // 
            this.lblMyVbSize.AutoSize = true;
            this.lblMyVbSize.Location = new System.Drawing.Point(244, 146);
            this.lblMyVbSize.Name = "lblMyVbSize";
            this.lblMyVbSize.Size = new System.Drawing.Size(73, 13);
            this.lblMyVbSize.TabIndex = 19;
            this.lblMyVbSize.Text = "MY_VB_SIZE";
            // 
            // tbMyVbSize
            // 
            this.tbMyVbSize.Location = new System.Drawing.Point(331, 143);
            this.tbMyVbSize.MaxLength = 10;
            this.tbMyVbSize.Name = "tbMyVbSize";
            this.tbMyVbSize.Size = new System.Drawing.Size(70, 20);
            this.tbMyVbSize.TabIndex = 18;
            // 
            // tbMyVb
            // 
            this.tbMyVb.Location = new System.Drawing.Point(331, 117);
            this.tbMyVb.MaxLength = 10;
            this.tbMyVb.Name = "tbMyVb";
            this.tbMyVb.Size = new System.Drawing.Size(70, 20);
            this.tbMyVb.TabIndex = 17;
            // 
            // lblMyVb
            // 
            this.lblMyVb.AutoSize = true;
            this.lblMyVb.Location = new System.Drawing.Point(244, 120);
            this.lblMyVb.Name = "lblMyVb";
            this.lblMyVb.Size = new System.Drawing.Size(43, 13);
            this.lblMyVb.TabIndex = 16;
            this.lblMyVb.Text = "MY_VB";
            // 
            // lblMyVhSize
            // 
            this.lblMyVhSize.AutoSize = true;
            this.lblMyVhSize.Location = new System.Drawing.Point(244, 94);
            this.lblMyVhSize.Name = "lblMyVhSize";
            this.lblMyVhSize.Size = new System.Drawing.Size(74, 13);
            this.lblMyVhSize.TabIndex = 15;
            this.lblMyVhSize.Text = "MY_VH_SIZE";
            // 
            // tbMyVhSize
            // 
            this.tbMyVhSize.Location = new System.Drawing.Point(331, 91);
            this.tbMyVhSize.MaxLength = 10;
            this.tbMyVhSize.Name = "tbMyVhSize";
            this.tbMyVhSize.Size = new System.Drawing.Size(70, 20);
            this.tbMyVhSize.TabIndex = 14;
            // 
            // tbMyVh
            // 
            this.tbMyVh.Location = new System.Drawing.Point(331, 65);
            this.tbMyVh.MaxLength = 10;
            this.tbMyVh.Name = "tbMyVh";
            this.tbMyVh.Size = new System.Drawing.Size(70, 20);
            this.tbMyVh.TabIndex = 13;
            // 
            // lblMyVh
            // 
            this.lblMyVh.AutoSize = true;
            this.lblMyVh.Location = new System.Drawing.Point(244, 68);
            this.lblMyVh.Name = "lblMyVh";
            this.lblMyVh.Size = new System.Drawing.Size(44, 13);
            this.lblMyVh.TabIndex = 12;
            this.lblMyVh.Text = "MY_VH";
            // 
            // lblMySeq
            // 
            this.lblMySeq.AutoSize = true;
            this.lblMySeq.Location = new System.Drawing.Point(244, 16);
            this.lblMySeq.Name = "lblMySeq";
            this.lblMySeq.Size = new System.Drawing.Size(51, 13);
            this.lblMySeq.TabIndex = 11;
            this.lblMySeq.Text = "MY_SEQ";
            // 
            // tbMySeq
            // 
            this.tbMySeq.Location = new System.Drawing.Point(331, 13);
            this.tbMySeq.Name = "tbMySeq";
            this.tbMySeq.Size = new System.Drawing.Size(70, 20);
            this.tbMySeq.TabIndex = 10;
            // 
            // tbMySeqSize
            // 
            this.tbMySeqSize.Location = new System.Drawing.Point(331, 39);
            this.tbMySeqSize.MaxLength = 10;
            this.tbMySeqSize.Name = "tbMySeqSize";
            this.tbMySeqSize.Size = new System.Drawing.Size(70, 20);
            this.tbMySeqSize.TabIndex = 9;
            // 
            // lblMySeqSize
            // 
            this.lblMySeqSize.AutoSize = true;
            this.lblMySeqSize.Location = new System.Drawing.Point(244, 42);
            this.lblMySeqSize.Name = "lblMySeqSize";
            this.lblMySeqSize.Size = new System.Drawing.Size(81, 13);
            this.lblMySeqSize.TabIndex = 8;
            this.lblMySeqSize.Text = "MY_SEQ_SIZE";
            // 
            // lblPadDrvParamSize
            // 
            this.lblPadDrvParamSize.AutoSize = true;
            this.lblPadDrvParamSize.Location = new System.Drawing.Point(6, 120);
            this.lblPadDrvParamSize.Name = "lblPadDrvParamSize";
            this.lblPadDrvParamSize.Size = new System.Drawing.Size(124, 13);
            this.lblPadDrvParamSize.TabIndex = 7;
            this.lblPadDrvParamSize.Text = "PSFDRV_PARAM_SIZE";
            // 
            // tbPadDrvParamSize
            // 
            this.tbPadDrvParamSize.Location = new System.Drawing.Point(136, 117);
            this.tbPadDrvParamSize.MaxLength = 10;
            this.tbPadDrvParamSize.Name = "tbPadDrvParamSize";
            this.tbPadDrvParamSize.Size = new System.Drawing.Size(70, 20);
            this.tbPadDrvParamSize.TabIndex = 6;
            // 
            // lblPsfDrvParam
            // 
            this.lblPsfDrvParam.AutoSize = true;
            this.lblPsfDrvParam.Location = new System.Drawing.Point(6, 94);
            this.lblPsfDrvParam.Name = "lblPsfDrvParam";
            this.lblPsfDrvParam.Size = new System.Drawing.Size(94, 13);
            this.lblPsfDrvParam.TabIndex = 5;
            this.lblPsfDrvParam.Text = "PSFDRV_PARAM";
            // 
            // tbPsfDrvParam
            // 
            this.tbPsfDrvParam.Location = new System.Drawing.Point(136, 91);
            this.tbPsfDrvParam.MaxLength = 10;
            this.tbPsfDrvParam.Name = "tbPsfDrvParam";
            this.tbPsfDrvParam.Size = new System.Drawing.Size(70, 20);
            this.tbPsfDrvParam.TabIndex = 4;
            // 
            // lblPsfDrvSize
            // 
            this.lblPsfDrvSize.AutoSize = true;
            this.lblPsfDrvSize.Location = new System.Drawing.Point(6, 68);
            this.lblPsfDrvSize.Name = "lblPsfDrvSize";
            this.lblPsfDrvSize.Size = new System.Drawing.Size(80, 13);
            this.lblPsfDrvSize.TabIndex = 3;
            this.lblPsfDrvSize.Text = "PSFDRV_SIZE";
            // 
            // tbPsfDrvSize
            // 
            this.tbPsfDrvSize.Location = new System.Drawing.Point(136, 65);
            this.tbPsfDrvSize.MaxLength = 10;
            this.tbPsfDrvSize.Name = "tbPsfDrvSize";
            this.tbPsfDrvSize.Size = new System.Drawing.Size(70, 20);
            this.tbPsfDrvSize.TabIndex = 2;
            // 
            // tbPsfDrvLoad
            // 
            this.tbPsfDrvLoad.Location = new System.Drawing.Point(136, 39);
            this.tbPsfDrvLoad.MaxLength = 10;
            this.tbPsfDrvLoad.Name = "tbPsfDrvLoad";
            this.tbPsfDrvLoad.Size = new System.Drawing.Size(70, 20);
            this.tbPsfDrvLoad.TabIndex = 1;
            // 
            // lblPsfDrvLoad
            // 
            this.lblPsfDrvLoad.AutoSize = true;
            this.lblPsfDrvLoad.Location = new System.Drawing.Point(6, 42);
            this.lblPsfDrvLoad.Name = "lblPsfDrvLoad";
            this.lblPsfDrvLoad.Size = new System.Drawing.Size(85, 13);
            this.lblPsfDrvLoad.TabIndex = 0;
            this.lblPsfDrvLoad.Text = "PSFDRV_LOAD";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbRelaxPsfLoadAddress);
            this.grpOptions.Controls.Add(this.cbSepFunctions);
            this.grpOptions.Controls.Add(this.cbSeqFunctions);
            this.grpOptions.Controls.Add(this.cbIncludeReverb);
            this.grpOptions.Controls.Add(this.tbDriverText);
            this.grpOptions.Controls.Add(this.lblDriverText);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 276);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(781, 108);
            this.grpOptions.TabIndex = 0;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbRelaxPsfLoadAddress
            // 
            this.cbRelaxPsfLoadAddress.AutoSize = true;
            this.cbRelaxPsfLoadAddress.Location = new System.Drawing.Point(9, 87);
            this.cbRelaxPsfLoadAddress.Name = "cbRelaxPsfLoadAddress";
            this.cbRelaxPsfLoadAddress.Size = new System.Drawing.Size(244, 17);
            this.cbRelaxPsfLoadAddress.TabIndex = 5;
            this.cbRelaxPsfLoadAddress.Text = "Allow drivers with load address > 0x80100000.";
            this.cbRelaxPsfLoadAddress.UseVisualStyleBackColor = true;
            this.cbRelaxPsfLoadAddress.CheckedChanged += new System.EventHandler(this.cbRelaxPsfLoadAddress_CheckedChanged);
            // 
            // cbSepFunctions
            // 
            this.cbSepFunctions.AutoSize = true;
            this.cbSepFunctions.Location = new System.Drawing.Point(136, 18);
            this.cbSepFunctions.Name = "cbSepFunctions";
            this.cbSepFunctions.Size = new System.Drawing.Size(95, 17);
            this.cbSepFunctions.TabIndex = 4;
            this.cbSepFunctions.TabStop = true;
            this.cbSepFunctions.Text = "SEP Functions";
            this.cbSepFunctions.UseVisualStyleBackColor = true;
            // 
            // cbSeqFunctions
            // 
            this.cbSeqFunctions.AutoSize = true;
            this.cbSeqFunctions.Checked = true;
            this.cbSeqFunctions.Location = new System.Drawing.Point(9, 18);
            this.cbSeqFunctions.Name = "cbSeqFunctions";
            this.cbSeqFunctions.Size = new System.Drawing.Size(96, 17);
            this.cbSeqFunctions.TabIndex = 3;
            this.cbSeqFunctions.TabStop = true;
            this.cbSeqFunctions.Text = "SEQ Functions";
            this.cbSeqFunctions.UseVisualStyleBackColor = true;
            // 
            // cbIncludeReverb
            // 
            this.cbIncludeReverb.AutoSize = true;
            this.cbIncludeReverb.Location = new System.Drawing.Point(9, 67);
            this.cbIncludeReverb.Name = "cbIncludeReverb";
            this.cbIncludeReverb.Size = new System.Drawing.Size(441, 17);
            this.cbIncludeReverb.TabIndex = 2;
            this.cbIncludeReverb.Text = "Add reverb information to stub, defaults will be used if reverb parameters are no" +
                "t present.";
            this.cbIncludeReverb.UseVisualStyleBackColor = true;
            // 
            // tbDriverText
            // 
            this.tbDriverText.Location = new System.Drawing.Point(101, 41);
            this.tbDriverText.Name = "tbDriverText";
            this.tbDriverText.Size = new System.Drawing.Size(260, 20);
            this.tbDriverText.TabIndex = 1;
            // 
            // lblDriverText
            // 
            this.lblDriverText.AutoSize = true;
            this.lblDriverText.Location = new System.Drawing.Point(6, 44);
            this.lblDriverText.Name = "lblDriverText";
            this.lblDriverText.Size = new System.Drawing.Size(89, 13);
            this.lblDriverText.TabIndex = 0;
            this.lblDriverText.Text = "Driver Text String";
            // 
            // PsfStubMakerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 548);
            this.Controls.Add(this.grpSourceFiles);
            this.Name = "PsfStubMakerForm";
            this.Text = "PsfStubMakerForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSourceFiles, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSourceFiles.ResumeLayout(false);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSourceFiles;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.TextBox tbDriverText;
        private System.Windows.Forms.Label lblDriverText;
        private System.Windows.Forms.CheckBox cbIncludeReverb;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Label lblPsfDrvLoad;
        private System.Windows.Forms.TextBox tbPsfDrvLoad;
        private System.Windows.Forms.TextBox tbPsfDrvSize;
        private System.Windows.Forms.Label lblPsfDrvSize;
        private System.Windows.Forms.Label lblPsfDrvParam;
        private System.Windows.Forms.TextBox tbPsfDrvParam;
        private System.Windows.Forms.Label lblPadDrvParamSize;
        private System.Windows.Forms.TextBox tbPadDrvParamSize;
        private System.Windows.Forms.TextBox tbMySeqSize;
        private System.Windows.Forms.Label lblMySeqSize;
        private System.Windows.Forms.Label lblMySeq;
        private System.Windows.Forms.TextBox tbMySeq;
        private System.Windows.Forms.TextBox tbMyVh;
        private System.Windows.Forms.Label lblMyVh;
        private System.Windows.Forms.TextBox tbMyVhSize;
        private System.Windows.Forms.Label lblMyVhSize;
        private System.Windows.Forms.TextBox tbMyVb;
        private System.Windows.Forms.Label lblMyVb;
        private System.Windows.Forms.Label lblMyVbSize;
        private System.Windows.Forms.TextBox tbMyVbSize;
        private System.Windows.Forms.Button btnLoadDefaults;
        private System.Windows.Forms.CheckBox cbOverrideDriverOffset;
        private System.Windows.Forms.RadioButton cbSepFunctions;
        private System.Windows.Forms.RadioButton cbSeqFunctions;
        private System.Windows.Forms.CheckBox cbRelaxPsfLoadAddress;
        private System.Windows.Forms.Button btnHighestAddress;
    }
}