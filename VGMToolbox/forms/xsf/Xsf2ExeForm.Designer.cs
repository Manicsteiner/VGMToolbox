namespace VGMToolbox.forms.xsf
{
    partial class Xsf2ExeForm
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
            this.grpXsfPsf2Exe_Source = new System.Windows.Forms.GroupBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbExtractReservedSection = new System.Windows.Forms.CheckBox();
            this.cbXsfPsf2Exe_StripGsfHeader = new System.Windows.Forms.CheckBox();
            this.cbXsfPsf2Exe_IncludeOrigExt = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpXsfPsf2Exe_Source.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 373);
            this.pnlLabels.Size = new System.Drawing.Size(779, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(779, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 296);
            this.tbOutput.Size = new System.Drawing.Size(779, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 276);
            this.pnlButtons.Size = new System.Drawing.Size(779, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(719, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(659, 0);
            // 
            // grpXsfPsf2Exe_Source
            // 
            this.grpXsfPsf2Exe_Source.Controls.Add(this.grpOptions);
            this.grpXsfPsf2Exe_Source.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpXsfPsf2Exe_Source.Location = new System.Drawing.Point(0, 23);
            this.grpXsfPsf2Exe_Source.Name = "grpXsfPsf2Exe_Source";
            this.grpXsfPsf2Exe_Source.Size = new System.Drawing.Size(779, 253);
            this.grpXsfPsf2Exe_Source.TabIndex = 5;
            this.grpXsfPsf2Exe_Source.TabStop = false;
            this.grpXsfPsf2Exe_Source.Text = "Source";
            this.grpXsfPsf2Exe_Source.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbXsfPsf2Exe_Source_DragDrop);
            this.grpXsfPsf2Exe_Source.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbExtractReservedSection);
            this.grpOptions.Controls.Add(this.cbXsfPsf2Exe_StripGsfHeader);
            this.grpOptions.Controls.Add(this.cbXsfPsf2Exe_IncludeOrigExt);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 163);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(773, 87);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbExtractReservedSection
            // 
            this.cbExtractReservedSection.AutoSize = true;
            this.cbExtractReservedSection.Location = new System.Drawing.Point(6, 18);
            this.cbExtractReservedSection.Name = "cbExtractReservedSection";
            this.cbExtractReservedSection.Size = new System.Drawing.Size(188, 17);
            this.cbExtractReservedSection.TabIndex = 2;
            this.cbExtractReservedSection.Text = "Also Extract the Reserved Section";
            this.cbExtractReservedSection.UseVisualStyleBackColor = true;
            // 
            // cbXsfPsf2Exe_StripGsfHeader
            // 
            this.cbXsfPsf2Exe_StripGsfHeader.AutoSize = true;
            this.cbXsfPsf2Exe_StripGsfHeader.Location = new System.Drawing.Point(6, 64);
            this.cbXsfPsf2Exe_StripGsfHeader.Name = "cbXsfPsf2Exe_StripGsfHeader";
            this.cbXsfPsf2Exe_StripGsfHeader.Size = new System.Drawing.Size(209, 17);
            this.cbXsfPsf2Exe_StripGsfHeader.TabIndex = 1;
            this.cbXsfPsf2Exe_StripGsfHeader.Text = "Strip GSF Header after Decompression";
            this.cbXsfPsf2Exe_StripGsfHeader.UseVisualStyleBackColor = true;
            // 
            // cbXsfPsf2Exe_IncludeOrigExt
            // 
            this.cbXsfPsf2Exe_IncludeOrigExt.AutoSize = true;
            this.cbXsfPsf2Exe_IncludeOrigExt.Location = new System.Drawing.Point(6, 41);
            this.cbXsfPsf2Exe_IncludeOrigExt.Name = "cbXsfPsf2Exe_IncludeOrigExt";
            this.cbXsfPsf2Exe_IncludeOrigExt.Size = new System.Drawing.Size(234, 17);
            this.cbXsfPsf2Exe_IncludeOrigExt.TabIndex = 0;
            this.cbXsfPsf2Exe_IncludeOrigExt.Text = "Include original file extension in output name";
            this.cbXsfPsf2Exe_IncludeOrigExt.UseVisualStyleBackColor = true;
            // 
            // Xsf_Xsf2ExeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 414);
            this.Controls.Add(this.grpXsfPsf2Exe_Source);
            this.Name = "Xsf2ExeForm";
            this.Text = "Xsf2ExeForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpXsfPsf2Exe_Source, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpXsfPsf2Exe_Source.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpXsfPsf2Exe_Source;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox cbXsfPsf2Exe_StripGsfHeader;
        private System.Windows.Forms.CheckBox cbXsfPsf2Exe_IncludeOrigExt;
        private System.Windows.Forms.CheckBox cbExtractReservedSection;
    }
}