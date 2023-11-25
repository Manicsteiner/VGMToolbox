namespace VGMToolbox.forms.xsf
{
    partial class SdatOptimizerForm
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
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.tbSourceSdat = new System.Windows.Forms.TextBox();
            this.lblDragNDrop = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbIncludeAllSseq = new System.Windows.Forms.CheckBox();
            this.tbEndSequence = new System.Windows.Forms.TextBox();
            this.lblEndingSequence = new System.Windows.Forms.Label();
            this.tbStartSequence = new System.Windows.Forms.TextBox();
            this.lblStartingSequence = new System.Windows.Forms.Label();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 415);
            this.pnlLabels.Size = new System.Drawing.Size(716, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(716, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 338);
            this.tbOutput.Size = new System.Drawing.Size(716, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 318);
            this.pnlButtons.Size = new System.Drawing.Size(716, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(656, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(596, 0);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.tbSourceSdat);
            this.grpSource.Controls.Add(this.lblDragNDrop);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(716, 59);
            this.grpSource.TabIndex = 7;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source SDAT";
            // 
            // tbSourceSdat
            // 
            this.tbSourceSdat.AllowDrop = true;
            this.tbSourceSdat.Location = new System.Drawing.Point(6, 19);
            this.tbSourceSdat.Name = "tbSourceSdat";
            this.tbSourceSdat.Size = new System.Drawing.Size(282, 20);
            this.tbSourceSdat.TabIndex = 2;
            this.tbSourceSdat.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSourceSdat_DragDrop);
            this.tbSourceSdat.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // lblDragNDrop
            // 
            this.lblDragNDrop.AutoSize = true;
            this.lblDragNDrop.Location = new System.Drawing.Point(6, 42);
            this.lblDragNDrop.Name = "lblDragNDrop";
            this.lblDragNDrop.Size = new System.Drawing.Size(153, 13);
            this.lblDragNDrop.TabIndex = 1;
            this.lblDragNDrop.Text = "Drag SDAT(s) to optimize here.";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbIncludeAllSseq);
            this.grpOptions.Controls.Add(this.tbEndSequence);
            this.grpOptions.Controls.Add(this.lblEndingSequence);
            this.grpOptions.Controls.Add(this.tbStartSequence);
            this.grpOptions.Controls.Add(this.lblStartingSequence);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOptions.Location = new System.Drawing.Point(0, 82);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(716, 63);
            this.grpOptions.TabIndex = 8;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbIncludeAllSseq
            // 
            this.cbIncludeAllSseq.AutoSize = true;
            this.cbIncludeAllSseq.Checked = true;
            this.cbIncludeAllSseq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeAllSseq.Location = new System.Drawing.Point(165, 15);
            this.cbIncludeAllSseq.Name = "cbIncludeAllSseq";
            this.cbIncludeAllSseq.Size = new System.Drawing.Size(132, 17);
            this.cbIncludeAllSseq.TabIndex = 4;
            this.cbIncludeAllSseq.Text = "Include All Sequences";
            this.cbIncludeAllSseq.UseVisualStyleBackColor = true;
            this.cbIncludeAllSseq.CheckedChanged += new System.EventHandler(this.cbIncludeAllSseq_CheckedChanged);
            // 
            // tbEndSequence
            // 
            this.tbEndSequence.Location = new System.Drawing.Point(107, 37);
            this.tbEndSequence.Name = "tbEndSequence";
            this.tbEndSequence.ReadOnly = true;
            this.tbEndSequence.Size = new System.Drawing.Size(52, 20);
            this.tbEndSequence.TabIndex = 3;
            // 
            // lblEndingSequence
            // 
            this.lblEndingSequence.AutoSize = true;
            this.lblEndingSequence.Location = new System.Drawing.Point(6, 40);
            this.lblEndingSequence.Name = "lblEndingSequence";
            this.lblEndingSequence.Size = new System.Drawing.Size(92, 13);
            this.lblEndingSequence.TabIndex = 2;
            this.lblEndingSequence.Text = "Ending Sequence";
            // 
            // tbStartSequence
            // 
            this.tbStartSequence.Location = new System.Drawing.Point(107, 13);
            this.tbStartSequence.Name = "tbStartSequence";
            this.tbStartSequence.ReadOnly = true;
            this.tbStartSequence.Size = new System.Drawing.Size(52, 20);
            this.tbStartSequence.TabIndex = 1;
            // 
            // lblStartingSequence
            // 
            this.lblStartingSequence.AutoSize = true;
            this.lblStartingSequence.Location = new System.Drawing.Point(6, 16);
            this.lblStartingSequence.Name = "lblStartingSequence";
            this.lblStartingSequence.Size = new System.Drawing.Size(95, 13);
            this.lblStartingSequence.TabIndex = 0;
            this.lblStartingSequence.Text = "Starting Sequence";
            // 
            // Xsf_2sfSdatOptimizerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 456);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpSource);
            this.Name = "SdatOptimizerForm";
            this.Text = "SdatOptimizer";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSource, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.Label lblDragNDrop;
        private System.Windows.Forms.TextBox tbSourceSdat;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.Label lblStartingSequence;
        private System.Windows.Forms.TextBox tbEndSequence;
        private System.Windows.Forms.Label lblEndingSequence;
        private System.Windows.Forms.TextBox tbStartSequence;
        private System.Windows.Forms.CheckBox cbIncludeAllSseq;
    }
}