namespace VGMToolbox.forms.extraction
{
    partial class ByteRemoverForm
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
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.tbEndAddress = new System.Windows.Forms.TextBox();
            this.tbLength = new System.Windows.Forms.TextBox();
            this.rbCutToEof = new System.Windows.Forms.RadioButton();
            this.rbLength = new System.Windows.Forms.RadioButton();
            this.rbEndAddress = new System.Windows.Forms.RadioButton();
            this.tbStartAddress = new System.Windows.Forms.TextBox();
            this.lblStartAddress = new System.Windows.Forms.Label();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 635);
            this.pnlLabels.Size = new System.Drawing.Size(790, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(790, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 558);
            this.tbOutput.Size = new System.Drawing.Size(790, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 538);
            this.pnlButtons.Size = new System.Drawing.Size(790, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(730, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(670, 0);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.tbEndAddress);
            this.grpOptions.Controls.Add(this.tbLength);
            this.grpOptions.Controls.Add(this.rbCutToEof);
            this.grpOptions.Controls.Add(this.rbLength);
            this.grpOptions.Controls.Add(this.rbEndAddress);
            this.grpOptions.Controls.Add(this.tbStartAddress);
            this.grpOptions.Controls.Add(this.lblStartAddress);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(0, 454);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(790, 84);
            this.grpOptions.TabIndex = 5;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Cut Options";
            // 
            // tbEndAddress
            // 
            this.tbEndAddress.Location = new System.Drawing.Point(274, 13);
            this.tbEndAddress.Name = "tbEndAddress";
            this.tbEndAddress.Size = new System.Drawing.Size(75, 20);
            this.tbEndAddress.TabIndex = 6;
            // 
            // tbLength
            // 
            this.tbLength.Location = new System.Drawing.Point(274, 36);
            this.tbLength.Name = "tbLength";
            this.tbLength.Size = new System.Drawing.Size(75, 20);
            this.tbLength.TabIndex = 5;
            // 
            // rbCutToEof
            // 
            this.rbCutToEof.AutoSize = true;
            this.rbCutToEof.Location = new System.Drawing.Point(163, 60);
            this.rbCutToEof.Name = "rbCutToEof";
            this.rbCutToEof.Size = new System.Drawing.Size(105, 17);
            this.rbCutToEof.TabIndex = 4;
            this.rbCutToEof.TabStop = true;
            this.rbCutToEof.Text = "End of File (EOF)";
            this.rbCutToEof.UseVisualStyleBackColor = true;
            // 
            // rbLength
            // 
            this.rbLength.AutoSize = true;
            this.rbLength.Location = new System.Drawing.Point(163, 37);
            this.rbLength.Name = "rbLength";
            this.rbLength.Size = new System.Drawing.Size(58, 17);
            this.rbLength.TabIndex = 3;
            this.rbLength.TabStop = true;
            this.rbLength.Text = "Length";
            this.rbLength.UseVisualStyleBackColor = true;
            this.rbLength.CheckedChanged += new System.EventHandler(this.rbLength_CheckedChanged);
            // 
            // rbEndAddress
            // 
            this.rbEndAddress.AutoSize = true;
            this.rbEndAddress.Location = new System.Drawing.Point(163, 14);
            this.rbEndAddress.Name = "rbEndAddress";
            this.rbEndAddress.Size = new System.Drawing.Size(85, 17);
            this.rbEndAddress.TabIndex = 2;
            this.rbEndAddress.TabStop = true;
            this.rbEndAddress.Text = "End Address";
            this.rbEndAddress.UseVisualStyleBackColor = true;
            this.rbEndAddress.CheckedChanged += new System.EventHandler(this.rbEndAddress_CheckedChanged);
            // 
            // tbStartAddress
            // 
            this.tbStartAddress.Location = new System.Drawing.Point(82, 13);
            this.tbStartAddress.Name = "tbStartAddress";
            this.tbStartAddress.Size = new System.Drawing.Size(75, 20);
            this.tbStartAddress.TabIndex = 1;
            // 
            // lblStartAddress
            // 
            this.lblStartAddress.AutoSize = true;
            this.lblStartAddress.Location = new System.Drawing.Point(6, 16);
            this.lblStartAddress.Name = "lblStartAddress";
            this.lblStartAddress.Size = new System.Drawing.Size(70, 13);
            this.lblStartAddress.TabIndex = 0;
            this.lblStartAddress.Text = "Start Address";
            // 
            // ByteRemoverForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 676);
            this.Controls.Add(this.grpOptions);
            this.Name = "ByteRemoverForm";
            this.Text = "ByteRemoverForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ByteRemoverForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ByteRemoverForm_DragEnter);
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpOptions, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.Label lblStartAddress;
        private System.Windows.Forms.TextBox tbStartAddress;
        private System.Windows.Forms.RadioButton rbCutToEof;
        private System.Windows.Forms.RadioButton rbLength;
        private System.Windows.Forms.RadioButton rbEndAddress;
        private System.Windows.Forms.TextBox tbLength;
        private System.Windows.Forms.TextBox tbEndAddress;
    }
}