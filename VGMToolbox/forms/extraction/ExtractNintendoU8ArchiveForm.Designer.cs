namespace VGMToolbox.forms.extraction
{
    partial class ExtractNintendoU8ArchiveForm
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
            this.grpDrop = new System.Windows.Forms.GroupBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 268);
            this.pnlLabels.Size = new System.Drawing.Size(843, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(843, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 191);
            this.tbOutput.Size = new System.Drawing.Size(843, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 171);
            this.pnlButtons.Size = new System.Drawing.Size(843, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(783, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(723, 0);
            // 
            // grpDrop
            // 
            this.grpDrop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDrop.Location = new System.Drawing.Point(0, 23);
            this.grpDrop.Name = "grpDrop";
            this.grpDrop.Size = new System.Drawing.Size(843, 148);
            this.grpDrop.TabIndex = 5;
            this.grpDrop.TabStop = false;
            this.grpDrop.Text = "Drop Files Here";
            // 
            // ExtractNintendoU8ArchiveForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 309);
            this.Controls.Add(this.grpDrop);
            this.Name = "ExtractNintendoU8ArchiveForm";
            this.Text = "ExtractNintendoU8ArchiveForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ExtractNintendoU8ArchiveForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpDrop, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDrop;
    }
}