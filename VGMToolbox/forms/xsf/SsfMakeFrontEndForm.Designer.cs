namespace VGMToolbox.forms.xsf
{
    partial class SsfMakeFrontEndForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbTryAllCombinations = new System.Windows.Forms.CheckBox();
            this.cbSeekData = new System.Windows.Forms.CheckBox();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.cbMatchSeqBank = new System.Windows.Forms.CheckBox();
            this.lblEffect = new System.Windows.Forms.Label();
            this.lblMixerNumber = new System.Windows.Forms.Label();
            this.lblMixerBank = new System.Windows.Forms.Label();
            this.lblVolume = new System.Windows.Forms.Label();
            this.lblSequenceTrack = new System.Windows.Forms.Label();
            this.lblSequenceBank = new System.Windows.Forms.Label();
            this.tbEffect = new System.Windows.Forms.TextBox();
            this.tbMixerNumber = new System.Windows.Forms.TextBox();
            this.tbMixerBank = new System.Windows.Forms.TextBox();
            this.tbVolume = new System.Windows.Forms.TextBox();
            this.tbSequenceTrack = new System.Windows.Forms.TextBox();
            this.tbSequenceBank = new System.Windows.Forms.TextBox();
            this.groupFiles = new System.Windows.Forms.GroupBox();
            this.btnBrowseDsp = new System.Windows.Forms.Button();
            this.lblSingleDspFile = new System.Windows.Forms.Label();
            this.tbDspFile = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.tbOutputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.btnBrowseDriver = new System.Windows.Forms.Button();
            this.lblSourcePath = new System.Windows.Forms.Label();
            this.lblDriver = new System.Windows.Forms.Label();
            this.tbSourcePath = new System.Windows.Forms.TextBox();
            this.tbDriver = new System.Windows.Forms.TextBox();
            this.tbCustomMapFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseCustomMap = new System.Windows.Forms.Button();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.groupFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 421);
            this.pnlLabels.Size = new System.Drawing.Size(830, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(830, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 344);
            this.tbOutput.Size = new System.Drawing.Size(830, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 324);
            this.pnlButtons.Size = new System.Drawing.Size(830, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(770, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(710, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.grpSettings);
            this.panel1.Controls.Add(this.groupFiles);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(830, 301);
            this.panel1.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbTryAllCombinations);
            this.groupBox3.Controls.Add(this.cbSeekData);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 251);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(813, 103);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options";
            // 
            // cbTryAllCombinations
            // 
            this.cbTryAllCombinations.AutoSize = true;
            this.cbTryAllCombinations.Location = new System.Drawing.Point(6, 81);
            this.cbTryAllCombinations.Name = "cbTryAllCombinations";
            this.cbTryAllCombinations.Size = new System.Drawing.Size(323, 17);
            this.cbTryAllCombinations.TabIndex = 1;
            this.cbTryAllCombinations.Text = "Try all combinations of SEQ and BIN (good for finding matches)";
            this.cbTryAllCombinations.UseVisualStyleBackColor = true;
            // 
            // cbSeekData
            // 
            this.cbSeekData.AutoSize = true;
            this.cbSeekData.Location = new System.Drawing.Point(6, 19);
            this.cbSeekData.Name = "cbSeekData";
            this.cbSeekData.Size = new System.Drawing.Size(326, 56);
            this.cbSeekData.TabIndex = 0;
            this.cbSeekData.Text = "Ignore Extensions, use seqext.py and tonext.py to find data.\r\nWill grab .EXB file" +
                "s from Source Path if availible.  Make sure the\r\n.EXB filenames match the files " +
                "containing SEQ/TONE\r\n data.";
            this.cbSeekData.UseVisualStyleBackColor = true;
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.cbMatchSeqBank);
            this.grpSettings.Controls.Add(this.lblEffect);
            this.grpSettings.Controls.Add(this.lblMixerNumber);
            this.grpSettings.Controls.Add(this.lblMixerBank);
            this.grpSettings.Controls.Add(this.lblVolume);
            this.grpSettings.Controls.Add(this.lblSequenceTrack);
            this.grpSettings.Controls.Add(this.lblSequenceBank);
            this.grpSettings.Controls.Add(this.tbEffect);
            this.grpSettings.Controls.Add(this.tbMixerNumber);
            this.grpSettings.Controls.Add(this.tbMixerBank);
            this.grpSettings.Controls.Add(this.tbVolume);
            this.grpSettings.Controls.Add(this.tbSequenceTrack);
            this.grpSettings.Controls.Add(this.tbSequenceBank);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSettings.Location = new System.Drawing.Point(0, 153);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(813, 98);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // cbMatchSeqBank
            // 
            this.cbMatchSeqBank.AutoSize = true;
            this.cbMatchSeqBank.Checked = true;
            this.cbMatchSeqBank.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMatchSeqBank.Location = new System.Drawing.Point(305, 21);
            this.cbMatchSeqBank.Name = "cbMatchSeqBank";
            this.cbMatchSeqBank.Size = new System.Drawing.Size(136, 17);
            this.cbMatchSeqBank.TabIndex = 12;
            this.cbMatchSeqBank.Text = "Match Sequence Bank";
            this.cbMatchSeqBank.UseVisualStyleBackColor = true;
            this.cbMatchSeqBank.CheckedChanged += new System.EventHandler(this.cbMatchSeqBank_CheckedChanged);
            // 
            // lblEffect
            // 
            this.lblEffect.AutoSize = true;
            this.lblEffect.Location = new System.Drawing.Point(168, 74);
            this.lblEffect.Name = "lblEffect";
            this.lblEffect.Size = new System.Drawing.Size(35, 13);
            this.lblEffect.TabIndex = 11;
            this.lblEffect.Text = "Effect";
            // 
            // lblMixerNumber
            // 
            this.lblMixerNumber.AutoSize = true;
            this.lblMixerNumber.Location = new System.Drawing.Point(168, 48);
            this.lblMixerNumber.Name = "lblMixerNumber";
            this.lblMixerNumber.Size = new System.Drawing.Size(72, 13);
            this.lblMixerNumber.TabIndex = 10;
            this.lblMixerNumber.Text = "Mixer Number";
            // 
            // lblMixerBank
            // 
            this.lblMixerBank.AutoSize = true;
            this.lblMixerBank.Location = new System.Drawing.Point(168, 22);
            this.lblMixerBank.Name = "lblMixerBank";
            this.lblMixerBank.Size = new System.Drawing.Size(60, 13);
            this.lblMixerBank.TabIndex = 9;
            this.lblMixerBank.Text = "Mixer Bank";
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(6, 74);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(42, 13);
            this.lblVolume.TabIndex = 8;
            this.lblVolume.Text = "Volume";
            // 
            // lblSequenceTrack
            // 
            this.lblSequenceTrack.AutoSize = true;
            this.lblSequenceTrack.Location = new System.Drawing.Point(6, 48);
            this.lblSequenceTrack.Name = "lblSequenceTrack";
            this.lblSequenceTrack.Size = new System.Drawing.Size(87, 13);
            this.lblSequenceTrack.TabIndex = 7;
            this.lblSequenceTrack.Text = "Sequence Track";
            // 
            // lblSequenceBank
            // 
            this.lblSequenceBank.AutoSize = true;
            this.lblSequenceBank.Location = new System.Drawing.Point(6, 22);
            this.lblSequenceBank.Name = "lblSequenceBank";
            this.lblSequenceBank.Size = new System.Drawing.Size(84, 13);
            this.lblSequenceBank.TabIndex = 6;
            this.lblSequenceBank.Text = "Sequence Bank";
            // 
            // tbEffect
            // 
            this.tbEffect.Location = new System.Drawing.Point(248, 71);
            this.tbEffect.Name = "tbEffect";
            this.tbEffect.Size = new System.Drawing.Size(51, 20);
            this.tbEffect.TabIndex = 5;
            this.tbEffect.Text = "0x00";
            // 
            // tbMixerNumber
            // 
            this.tbMixerNumber.Location = new System.Drawing.Point(248, 45);
            this.tbMixerNumber.Name = "tbMixerNumber";
            this.tbMixerNumber.Size = new System.Drawing.Size(51, 20);
            this.tbMixerNumber.TabIndex = 4;
            this.tbMixerNumber.Text = "0x00";
            // 
            // tbMixerBank
            // 
            this.tbMixerBank.Location = new System.Drawing.Point(248, 19);
            this.tbMixerBank.Name = "tbMixerBank";
            this.tbMixerBank.ReadOnly = true;
            this.tbMixerBank.Size = new System.Drawing.Size(51, 20);
            this.tbMixerBank.TabIndex = 3;
            this.tbMixerBank.Text = "0x00";
            // 
            // tbVolume
            // 
            this.tbVolume.Location = new System.Drawing.Point(101, 71);
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Size = new System.Drawing.Size(51, 20);
            this.tbVolume.TabIndex = 2;
            this.tbVolume.Text = "0x7F";
            // 
            // tbSequenceTrack
            // 
            this.tbSequenceTrack.Location = new System.Drawing.Point(101, 45);
            this.tbSequenceTrack.Name = "tbSequenceTrack";
            this.tbSequenceTrack.Size = new System.Drawing.Size(51, 20);
            this.tbSequenceTrack.TabIndex = 1;
            this.tbSequenceTrack.Text = "0x00";
            // 
            // tbSequenceBank
            // 
            this.tbSequenceBank.Location = new System.Drawing.Point(101, 19);
            this.tbSequenceBank.Name = "tbSequenceBank";
            this.tbSequenceBank.Size = new System.Drawing.Size(51, 20);
            this.tbSequenceBank.TabIndex = 0;
            this.tbSequenceBank.Text = "0x00";
            this.tbSequenceBank.TextChanged += new System.EventHandler(this.tbSequenceBank_TextChanged);
            // 
            // groupFiles
            // 
            this.groupFiles.Controls.Add(this.btnBrowseCustomMap);
            this.groupFiles.Controls.Add(this.label1);
            this.groupFiles.Controls.Add(this.tbCustomMapFile);
            this.groupFiles.Controls.Add(this.btnBrowseDsp);
            this.groupFiles.Controls.Add(this.lblSingleDspFile);
            this.groupFiles.Controls.Add(this.tbDspFile);
            this.groupFiles.Controls.Add(this.lblOutputFolder);
            this.groupFiles.Controls.Add(this.tbOutputFolder);
            this.groupFiles.Controls.Add(this.btnBrowseSource);
            this.groupFiles.Controls.Add(this.btnBrowseDriver);
            this.groupFiles.Controls.Add(this.lblSourcePath);
            this.groupFiles.Controls.Add(this.lblDriver);
            this.groupFiles.Controls.Add(this.tbSourcePath);
            this.groupFiles.Controls.Add(this.tbDriver);
            this.groupFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupFiles.Location = new System.Drawing.Point(0, 0);
            this.groupFiles.Name = "groupFiles";
            this.groupFiles.Size = new System.Drawing.Size(813, 153);
            this.groupFiles.TabIndex = 0;
            this.groupFiles.TabStop = false;
            this.groupFiles.Text = "Files";
            // 
            // btnBrowseDsp
            // 
            this.btnBrowseDsp.Location = new System.Drawing.Point(393, 97);
            this.btnBrowseDsp.Name = "btnBrowseDsp";
            this.btnBrowseDsp.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseDsp.TabIndex = 13;
            this.btnBrowseDsp.Text = "...";
            this.btnBrowseDsp.UseVisualStyleBackColor = true;
            this.btnBrowseDsp.Click += new System.EventHandler(this.btnBrowseDsp_Click);
            // 
            // lblSingleDspFile
            // 
            this.lblSingleDspFile.AutoSize = true;
            this.lblSingleDspFile.Location = new System.Drawing.Point(6, 100);
            this.lblSingleDspFile.Name = "lblSingleDspFile";
            this.lblSingleDspFile.Size = new System.Drawing.Size(128, 13);
            this.lblSingleDspFile.TabIndex = 11;
            this.lblSingleDspFile.Text = "Single DSP File (Optional)";
            // 
            // tbDspFile
            // 
            this.tbDspFile.AllowDrop = true;
            this.tbDspFile.Location = new System.Drawing.Point(140, 97);
            this.tbDspFile.Name = "tbDspFile";
            this.tbDspFile.Size = new System.Drawing.Size(247, 20);
            this.tbDspFile.TabIndex = 10;
            this.tbDspFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbDspFile_DragDrop);
            this.tbDspFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbDspFile_DragEnter);
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(6, 74);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(71, 13);
            this.lblOutputFolder.TabIndex = 9;
            this.lblOutputFolder.Text = "Output Folder";
            // 
            // tbOutputFolder
            // 
            this.tbOutputFolder.Location = new System.Drawing.Point(140, 71);
            this.tbOutputFolder.Name = "tbOutputFolder";
            this.tbOutputFolder.Size = new System.Drawing.Size(247, 20);
            this.tbOutputFolder.TabIndex = 8;
            this.tbOutputFolder.Text = "mySsfFolder";
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(393, 45);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseSource.TabIndex = 7;
            this.btnBrowseSource.Text = "...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // btnBrowseDriver
            // 
            this.btnBrowseDriver.Location = new System.Drawing.Point(393, 19);
            this.btnBrowseDriver.Name = "btnBrowseDriver";
            this.btnBrowseDriver.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseDriver.TabIndex = 6;
            this.btnBrowseDriver.Text = "...";
            this.btnBrowseDriver.UseVisualStyleBackColor = true;
            this.btnBrowseDriver.Click += new System.EventHandler(this.btnBrowseDriver_Click);
            // 
            // lblSourcePath
            // 
            this.lblSourcePath.AutoSize = true;
            this.lblSourcePath.Location = new System.Drawing.Point(6, 49);
            this.lblSourcePath.Name = "lblSourcePath";
            this.lblSourcePath.Size = new System.Drawing.Size(66, 13);
            this.lblSourcePath.TabIndex = 5;
            this.lblSourcePath.Text = "Source Path";
            // 
            // lblDriver
            // 
            this.lblDriver.AutoSize = true;
            this.lblDriver.Location = new System.Drawing.Point(6, 23);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(35, 13);
            this.lblDriver.TabIndex = 4;
            this.lblDriver.Text = "Driver";
            // 
            // tbSourcePath
            // 
            this.tbSourcePath.AllowDrop = true;
            this.tbSourcePath.Location = new System.Drawing.Point(140, 45);
            this.tbSourcePath.Name = "tbSourcePath";
            this.tbSourcePath.Size = new System.Drawing.Size(247, 20);
            this.tbSourcePath.TabIndex = 1;
            this.tbSourcePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSourcePath_DragDrop);
            this.tbSourcePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbSourcePath_DragEnter);
            // 
            // tbDriver
            // 
            this.tbDriver.AllowDrop = true;
            this.tbDriver.Location = new System.Drawing.Point(140, 19);
            this.tbDriver.Name = "tbDriver";
            this.tbDriver.Size = new System.Drawing.Size(247, 20);
            this.tbDriver.TabIndex = 0;
            this.tbDriver.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbDriver_DragDrop);
            this.tbDriver.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbDriver_DragEnter);
            // 
            // tbCustomMapFile
            // 
            this.tbCustomMapFile.AllowDrop = true;
            this.tbCustomMapFile.Location = new System.Drawing.Point(140, 123);
            this.tbCustomMapFile.Name = "tbCustomMapFile";
            this.tbCustomMapFile.Size = new System.Drawing.Size(247, 20);
            this.tbCustomMapFile.TabIndex = 14;
            this.tbCustomMapFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbCustomMapFile_DragDrop);
            this.tbCustomMapFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbCustomMapFile_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Custom MAP File";
            // 
            // btnBrowseCustomMap
            // 
            this.btnBrowseCustomMap.Location = new System.Drawing.Point(393, 123);
            this.btnBrowseCustomMap.Name = "btnBrowseCustomMap";
            this.btnBrowseCustomMap.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseCustomMap.TabIndex = 16;
            this.btnBrowseCustomMap.Text = "...";
            this.btnBrowseCustomMap.UseVisualStyleBackColor = true;
            this.btnBrowseCustomMap.Click += new System.EventHandler(this.btnBrowseCustomMap_Click);
            // 
            // SsfMakeFrontEndForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 462);
            this.Controls.Add(this.panel1);
            this.Name = "SsfMakeFrontEndForm";
            this.Text = "SsfMakeFrontEndForm";
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
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.groupFiles.ResumeLayout(false);
            this.groupFiles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupFiles;
        private System.Windows.Forms.TextBox tbSourcePath;
        private System.Windows.Forms.TextBox tbDriver;
        private System.Windows.Forms.Label lblSourcePath;
        private System.Windows.Forms.Label lblDriver;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.TextBox tbSequenceBank;
        private System.Windows.Forms.TextBox tbSequenceTrack;
        private System.Windows.Forms.TextBox tbVolume;
        private System.Windows.Forms.TextBox tbMixerBank;
        private System.Windows.Forms.TextBox tbEffect;
        private System.Windows.Forms.TextBox tbMixerNumber;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Label lblSequenceTrack;
        private System.Windows.Forms.Label lblSequenceBank;
        private System.Windows.Forms.Label lblMixerNumber;
        private System.Windows.Forms.Label lblMixerBank;
        private System.Windows.Forms.Label lblEffect;
        private System.Windows.Forms.CheckBox cbMatchSeqBank;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.Button btnBrowseDriver;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbSeekData;
        private System.Windows.Forms.TextBox tbOutputFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.Label lblSingleDspFile;
        private System.Windows.Forms.TextBox tbDspFile;
        private System.Windows.Forms.Button btnBrowseDsp;
        private System.Windows.Forms.CheckBox cbTryAllCombinations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCustomMapFile;
        private System.Windows.Forms.Button btnBrowseCustomMap;

    }
}