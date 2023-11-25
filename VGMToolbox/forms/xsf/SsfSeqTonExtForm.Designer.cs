namespace VGMToolbox.forms.xsf
{
    partial class SsfSeqTonExtForm
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
            this.lblDragNDrop = new System.Windows.Forms.Label();
            this.tbSsfSqTonExtSource = new System.Windows.Forms.TextBox();
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbExtractToSubfolder = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 512);
            this.pnlLabels.Size = new System.Drawing.Size(846, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(846, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 435);
            this.tbOutput.Size = new System.Drawing.Size(846, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 415);
            this.pnlButtons.Size = new System.Drawing.Size(846, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(786, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(726, 0);
            // 
            // lblDragNDrop
            // 
            this.lblDragNDrop.AutoSize = true;
            this.lblDragNDrop.Location = new System.Drawing.Point(6, 42);
            this.lblDragNDrop.Name = "lblDragNDrop";
            this.lblDragNDrop.Size = new System.Drawing.Size(129, 13);
            this.lblDragNDrop.TabIndex = 6;
            this.lblDragNDrop.Text = "Drag file(s) to check here.";
            // 
            // tbSsfSqTonExtSource
            // 
            this.tbSsfSqTonExtSource.AllowDrop = true;
            this.tbSsfSqTonExtSource.Location = new System.Drawing.Point(6, 19);
            this.tbSsfSqTonExtSource.Name = "tbSsfSqTonExtSource";
            this.tbSsfSqTonExtSource.Size = new System.Drawing.Size(282, 20);
            this.tbSsfSqTonExtSource.TabIndex = 5;
            this.tbSsfSqTonExtSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSsfSqTonExtSource_DragDrop);
            this.tbSsfSqTonExtSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.tbSsfSqTonExtSource);
            this.grpSource.Controls.Add(this.lblDragNDrop);
            this.grpSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSource.Location = new System.Drawing.Point(0, 23);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(846, 63);
            this.grpSource.TabIndex = 7;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source Files";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(3, 129);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(417, 13);
            this.lblAuthor.TabIndex = 8;
            this.lblAuthor.Text = "seqext.py and tonext.py are written by kingshriek (snesmusic.org/hoot/kingshriek/" +
                "ssf/).";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbExtractToSubfolder);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(846, 40);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // cbExtractToSubfolder
            // 
            this.cbExtractToSubfolder.AutoSize = true;
            this.cbExtractToSubfolder.Location = new System.Drawing.Point(6, 19);
            this.cbExtractToSubfolder.Name = "cbExtractToSubfolder";
            this.cbExtractToSubfolder.Size = new System.Drawing.Size(172, 17);
            this.cbExtractToSubfolder.TabIndex = 0;
            this.cbExtractToSubfolder.Text = "Extract each file to a subfolder.";
            this.cbExtractToSubfolder.UseVisualStyleBackColor = true;
            // 
            // Xsf_SsfSeqTonExtForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 553);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.grpSource);
            this.Name = "SsfSeqTonExtForm";
            this.Text = "SsfSeqTonExtForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSource, 0);
            this.Controls.SetChildIndex(this.lblAuthor, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDragNDrop;
        private System.Windows.Forms.TextBox tbSsfSqTonExtSource;
        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbExtractToSubfolder;
    }
}