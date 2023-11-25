namespace VGMToolbox.forms.xsf
{
    partial class SsfMakeAdvancedForm
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
            this.tbDriverFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDspProgram = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSequenceFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbToneData = new System.Windows.Forms.ListBox();
            this.grpFiles = new System.Windows.Forms.GroupBox();
            this.tbSequenceCount = new System.Windows.Forms.TextBox();
            this.pb68000Memory = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.tbVolume = new System.Windows.Forms.TextBox();
            this.tbEffectNumber = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbMixerBank = new System.Windows.Forms.TextBox();
            this.tbMixerNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbSeqTrack = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbSeqBank = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbMakeSsflib = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb68000Memory)).BeginInit();
            this.grpSettings.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 452);
            this.pnlLabels.Size = new System.Drawing.Size(755, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(755, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 375);
            this.tbOutput.Size = new System.Drawing.Size(755, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 355);
            this.pnlButtons.Size = new System.Drawing.Size(755, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(695, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(635, 0);
            // 
            // tbDriverFile
            // 
            this.tbDriverFile.AllowDrop = true;
            this.tbDriverFile.Location = new System.Drawing.Point(84, 13);
            this.tbDriverFile.Name = "tbDriverFile";
            this.tbDriverFile.Size = new System.Drawing.Size(229, 20);
            this.tbDriverFile.TabIndex = 5;
            this.tbDriverFile.TextChanged += new System.EventHandler(this.tbDriverFile_TextChanged);
            this.tbDriverFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbDriverFile_DragDrop);
            this.tbDriverFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbDriverFile_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Driver";
            // 
            // tbDspProgram
            // 
            this.tbDspProgram.AllowDrop = true;
            this.tbDspProgram.Location = new System.Drawing.Point(84, 39);
            this.tbDspProgram.Name = "tbDspProgram";
            this.tbDspProgram.Size = new System.Drawing.Size(229, 20);
            this.tbDspProgram.TabIndex = 7;
            this.tbDspProgram.TextChanged += new System.EventHandler(this.tbDspProgram_TextChanged);
            this.tbDspProgram.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbDspProgram_DragDrop);
            this.tbDspProgram.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbDspProgram_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "DSP Program";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Sequence File";
            // 
            // tbSequenceFile
            // 
            this.tbSequenceFile.AllowDrop = true;
            this.tbSequenceFile.Location = new System.Drawing.Point(84, 65);
            this.tbSequenceFile.Name = "tbSequenceFile";
            this.tbSequenceFile.Size = new System.Drawing.Size(229, 20);
            this.tbSequenceFile.TabIndex = 10;
            this.tbSequenceFile.TextChanged += new System.EventHandler(this.tbSequenceFile_TextChanged);
            this.tbSequenceFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSequenceFile_DragDrop);
            this.tbSequenceFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbSequenceFile_DragEnter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Tone Data";
            // 
            // lbToneData
            // 
            this.lbToneData.AllowDrop = true;
            this.lbToneData.FormattingEnabled = true;
            this.lbToneData.Location = new System.Drawing.Point(84, 98);
            this.lbToneData.Name = "lbToneData";
            this.lbToneData.Size = new System.Drawing.Size(229, 95);
            this.lbToneData.TabIndex = 12;
            this.lbToneData.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbToneData_DragDrop);
            this.lbToneData.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbToneData_DragEnter);
            this.lbToneData.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lbToneData_KeyUp);
            // 
            // grpFiles
            // 
            this.grpFiles.Controls.Add(this.tbSequenceCount);
            this.grpFiles.Controls.Add(this.pb68000Memory);
            this.grpFiles.Controls.Add(this.label1);
            this.grpFiles.Controls.Add(this.label4);
            this.grpFiles.Controls.Add(this.tbDriverFile);
            this.grpFiles.Controls.Add(this.tbDspProgram);
            this.grpFiles.Controls.Add(this.lbToneData);
            this.grpFiles.Controls.Add(this.label3);
            this.grpFiles.Controls.Add(this.label2);
            this.grpFiles.Controls.Add(this.tbSequenceFile);
            this.grpFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFiles.Location = new System.Drawing.Point(0, 23);
            this.grpFiles.Name = "grpFiles";
            this.grpFiles.Size = new System.Drawing.Size(755, 202);
            this.grpFiles.TabIndex = 13;
            this.grpFiles.TabStop = false;
            this.grpFiles.Text = "Files (drop onto boxes)";
            // 
            // tbSequenceCount
            // 
            this.tbSequenceCount.Enabled = false;
            this.tbSequenceCount.Location = new System.Drawing.Point(319, 65);
            this.tbSequenceCount.Name = "tbSequenceCount";
            this.tbSequenceCount.ReadOnly = true;
            this.tbSequenceCount.Size = new System.Drawing.Size(30, 20);
            this.tbSequenceCount.TabIndex = 14;
            // 
            // pb68000Memory
            // 
            this.pb68000Memory.Location = new System.Drawing.Point(403, 13);
            this.pb68000Memory.Name = "pb68000Memory";
            this.pb68000Memory.Size = new System.Drawing.Size(140, 180);
            this.pb68000Memory.TabIndex = 13;
            this.pb68000Memory.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.tbVolume);
            this.grpSettings.Controls.Add(this.tbEffectNumber);
            this.grpSettings.Controls.Add(this.label10);
            this.grpSettings.Controls.Add(this.label9);
            this.grpSettings.Controls.Add(this.tbMixerBank);
            this.grpSettings.Controls.Add(this.tbMixerNumber);
            this.grpSettings.Controls.Add(this.label8);
            this.grpSettings.Controls.Add(this.label7);
            this.grpSettings.Controls.Add(this.tbSeqTrack);
            this.grpSettings.Controls.Add(this.label6);
            this.grpSettings.Controls.Add(this.tbSeqBank);
            this.grpSettings.Controls.Add(this.label5);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSettings.Location = new System.Drawing.Point(0, 225);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(755, 69);
            this.grpSettings.TabIndex = 14;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // tbVolume
            // 
            this.tbVolume.Location = new System.Drawing.Point(317, 13);
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Size = new System.Drawing.Size(30, 20);
            this.tbVolume.TabIndex = 11;
            this.tbVolume.Text = "0x7F";
            // 
            // tbEffectNumber
            // 
            this.tbEffectNumber.Location = new System.Drawing.Point(317, 35);
            this.tbEffectNumber.Name = "tbEffectNumber";
            this.tbEffectNumber.Size = new System.Drawing.Size(30, 20);
            this.tbEffectNumber.TabIndex = 10;
            this.tbEffectNumber.Text = "0x00";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(236, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Effect Number";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(236, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Volume";
            // 
            // tbMixerBank
            // 
            this.tbMixerBank.Location = new System.Drawing.Point(186, 13);
            this.tbMixerBank.Name = "tbMixerBank";
            this.tbMixerBank.Size = new System.Drawing.Size(30, 20);
            this.tbMixerBank.TabIndex = 7;
            this.tbMixerBank.Text = "0x00";
            // 
            // tbMixerNumber
            // 
            this.tbMixerNumber.Location = new System.Drawing.Point(186, 35);
            this.tbMixerNumber.Name = "tbMixerNumber";
            this.tbMixerNumber.Size = new System.Drawing.Size(30, 20);
            this.tbMixerNumber.TabIndex = 6;
            this.tbMixerNumber.Text = "0x00";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(108, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Mixer Number";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(108, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Mixer Bank";
            // 
            // tbSeqTrack
            // 
            this.tbSeqTrack.Location = new System.Drawing.Point(72, 35);
            this.tbSeqTrack.Name = "tbSeqTrack";
            this.tbSeqTrack.Size = new System.Drawing.Size(30, 20);
            this.tbSeqTrack.TabIndex = 3;
            this.tbSeqTrack.Text = "0x00";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "SEQ Track";
            // 
            // tbSeqBank
            // 
            this.tbSeqBank.Location = new System.Drawing.Point(72, 13);
            this.tbSeqBank.Name = "tbSeqBank";
            this.tbSeqBank.Size = new System.Drawing.Size(30, 20);
            this.tbSeqBank.TabIndex = 1;
            this.tbSeqBank.Text = "0x00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "SEQ Bank";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbMakeSsflib);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.Location = new System.Drawing.Point(0, 294);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(755, 55);
            this.grpOptions.TabIndex = 15;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbMakeSsflib
            // 
            this.cbMakeSsflib.AutoSize = true;
            this.cbMakeSsflib.Checked = true;
            this.cbMakeSsflib.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMakeSsflib.Location = new System.Drawing.Point(9, 19);
            this.cbMakeSsflib.Name = "cbMakeSsflib";
            this.cbMakeSsflib.Size = new System.Drawing.Size(182, 17);
            this.cbMakeSsflib.TabIndex = 0;
            this.cbMakeSsflib.Text = "Create .minissfs/.ssflib, if possible";
            this.cbMakeSsflib.UseVisualStyleBackColor = true;
            // 
            // SsfMakeAdvancedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(755, 493);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.grpFiles);
            this.Name = "SsfMakeAdvancedForm";
            this.Text = "SsfMakeAdvancedForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpFiles, 0);
            this.Controls.SetChildIndex(this.grpSettings, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpFiles.ResumeLayout(false);
            this.grpFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb68000Memory)).EndInit();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDriverFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDspProgram;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSequenceFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lbToneData;
        private System.Windows.Forms.GroupBox grpFiles;
        private System.Windows.Forms.PictureBox pb68000Memory;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.TextBox tbSeqTrack;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbSeqBank;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbMixerBank;
        private System.Windows.Forms.TextBox tbMixerNumber;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbVolume;
        private System.Windows.Forms.TextBox tbEffectNumber;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox cbMakeSsflib;
        private System.Windows.Forms.TextBox tbSequenceCount;
    }
}