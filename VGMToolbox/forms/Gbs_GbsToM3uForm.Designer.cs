namespace VGMToolbox.forms
{
    partial class Gbs_GbsToM3uForm
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
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbKnurekFormat = new System.Windows.Forms.CheckBox();
            this.cbGBS_OneM3uPerTrack = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 525);
            this.pnlLabels.Size = new System.Drawing.Size(851, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(851, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 448);
            this.tbOutput.Size = new System.Drawing.Size(851, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 428);
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
            // grpSource
            // 
            this.grpSource.Controls.Add(this.grpOptions);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(851, 405);
            this.grpSource.TabIndex = 5;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source";
            this.grpSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbGBS_gbsm3uSource_DragDrop);
            this.grpSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbKnurekFormat);
            this.grpOptions.Controls.Add(this.cbGBS_OneM3uPerTrack);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 334);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(845, 68);
            this.grpOptions.TabIndex = 8;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbKnurekFormat
            // 
            this.cbKnurekFormat.AutoSize = true;
            this.cbKnurekFormat.Location = new System.Drawing.Point(6, 42);
            this.cbKnurekFormat.Name = "cbKnurekFormat";
            this.cbKnurekFormat.Size = new System.Drawing.Size(280, 17);
            this.cbKnurekFormat.TabIndex = 1;
            this.cbKnurekFormat.Text = "Custom Knurek Parsing (you probably don\'t need this).";
            this.cbKnurekFormat.UseVisualStyleBackColor = true;
            // 
            // cbGBS_OneM3uPerTrack
            // 
            this.cbGBS_OneM3uPerTrack.AutoSize = true;
            this.cbGBS_OneM3uPerTrack.Location = new System.Drawing.Point(6, 19);
            this.cbGBS_OneM3uPerTrack.Name = "cbGBS_OneM3uPerTrack";
            this.cbGBS_OneM3uPerTrack.Size = new System.Drawing.Size(177, 17);
            this.cbGBS_OneM3uPerTrack.TabIndex = 0;
            this.cbGBS_OneM3uPerTrack.Text = "Output additional .m3u per track";
            this.cbGBS_OneM3uPerTrack.UseVisualStyleBackColor = true;
            // 
            // Gbs_GbsToM3uForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 566);
            this.Controls.Add(this.grpSource);
            this.Name = "Gbs_GbsToM3uForm";
            this.Text = "Gbs_GbsToM3uForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSource, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSource.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox cbGBS_OneM3uPerTrack;
        private System.Windows.Forms.CheckBox cbKnurekFormat;
    }
}