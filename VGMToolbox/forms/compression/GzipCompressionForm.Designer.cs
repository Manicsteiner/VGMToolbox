namespace VGMToolbox.forms.compression
{
    partial class GzipCompressionForm
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
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.lblOffset = new System.Windows.Forms.Label();
            this.rbDecompress = new System.Windows.Forms.RadioButton();
            this.tbOffset = new System.Windows.Forms.TextBox();
            this.rbCompress = new System.Windows.Forms.RadioButton();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSourceFiles.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 506);
            this.pnlLabels.Size = new System.Drawing.Size(903, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(903, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 429);
            this.tbOutput.Size = new System.Drawing.Size(903, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 409);
            this.pnlButtons.Size = new System.Drawing.Size(903, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(843, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(783, 0);
            // 
            // grpSourceFiles
            // 
            this.grpSourceFiles.Controls.Add(this.grpOptions);
            this.grpSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSourceFiles.Location = new System.Drawing.Point(0, 23);
            this.grpSourceFiles.Name = "grpSourceFiles";
            this.grpSourceFiles.Size = new System.Drawing.Size(903, 386);
            this.grpSourceFiles.TabIndex = 5;
            this.grpSourceFiles.TabStop = false;
            this.grpSourceFiles.Text = "Drop Files Here";
            this.grpSourceFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.grpSourceFiles_DragDrop);
            this.grpSourceFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.lblOffset);
            this.grpOptions.Controls.Add(this.rbDecompress);
            this.grpOptions.Controls.Add(this.tbOffset);
            this.grpOptions.Controls.Add(this.rbCompress);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(3, 316);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(897, 67);
            this.grpOptions.TabIndex = 0;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(121, 21);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(74, 13);
            this.lblOffset.TabIndex = 7;
            this.lblOffset.Text = "Starting Offset";
            // 
            // rbDecompress
            // 
            this.rbDecompress.AutoSize = true;
            this.rbDecompress.Checked = true;
            this.rbDecompress.Location = new System.Drawing.Point(6, 19);
            this.rbDecompress.Name = "rbDecompress";
            this.rbDecompress.Size = new System.Drawing.Size(84, 17);
            this.rbDecompress.TabIndex = 4;
            this.rbDecompress.TabStop = true;
            this.rbDecompress.Text = "Decompress";
            this.rbDecompress.UseVisualStyleBackColor = true;
            // 
            // tbOffset
            // 
            this.tbOffset.Location = new System.Drawing.Point(201, 18);
            this.tbOffset.Name = "tbOffset";
            this.tbOffset.Size = new System.Drawing.Size(78, 20);
            this.tbOffset.TabIndex = 6;
            this.tbOffset.Text = "0";
            // 
            // rbCompress
            // 
            this.rbCompress.AutoSize = true;
            this.rbCompress.Location = new System.Drawing.Point(6, 42);
            this.rbCompress.Name = "rbCompress";
            this.rbCompress.Size = new System.Drawing.Size(71, 17);
            this.rbCompress.TabIndex = 5;
            this.rbCompress.Text = "Compress";
            this.rbCompress.UseVisualStyleBackColor = true;
            // 
            // Compression_GzipCompressionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 547);
            this.Controls.Add(this.grpSourceFiles);
            this.Name = "GzipCompressionForm";
            this.Text = "GzipCompressionForm";
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
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSourceFiles;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.RadioButton rbDecompress;
        private System.Windows.Forms.TextBox tbOffset;
        private System.Windows.Forms.RadioButton rbCompress;
    }
}