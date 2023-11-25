namespace VGMToolbox.forms.extraction
{
    partial class UnpackNintendoWadForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbUnpackU8Archives = new System.Windows.Forms.CheckBox();
            this.rbExtractAllSections = new System.Windows.Forms.RadioButton();
            this.rbExtractDataOnly = new System.Windows.Forms.RadioButton();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(843, 148);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Drop Files Here";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbUnpackU8Archives);
            this.groupBox2.Controls.Add(this.rbExtractAllSections);
            this.groupBox2.Controls.Add(this.rbExtractDataOnly);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(837, 74);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // cbUnpackU8Archives
            // 
            this.cbUnpackU8Archives.AutoSize = true;
            this.cbUnpackU8Archives.Checked = true;
            this.cbUnpackU8Archives.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUnpackU8Archives.Location = new System.Drawing.Point(302, 19);
            this.cbUnpackU8Archives.Name = "cbUnpackU8Archives";
            this.cbUnpackU8Archives.Size = new System.Drawing.Size(184, 17);
            this.cbUnpackU8Archives.TabIndex = 2;
            this.cbUnpackU8Archives.Text = "Unpack all extracted U8 archives";
            this.cbUnpackU8Archives.UseVisualStyleBackColor = true;
            // 
            // rbExtractAllSections
            // 
            this.rbExtractAllSections.AutoSize = true;
            this.rbExtractAllSections.Location = new System.Drawing.Point(6, 42);
            this.rbExtractAllSections.Name = "rbExtractAllSections";
            this.rbExtractAllSections.Size = new System.Drawing.Size(267, 17);
            this.rbExtractAllSections.TabIndex = 1;
            this.rbExtractAllSections.TabStop = true;
            this.rbExtractAllSections.Text = "Extract All Chunks (content, cert, ticket, title, footer)";
            this.rbExtractAllSections.UseVisualStyleBackColor = true;
            // 
            // rbExtractDataOnly
            // 
            this.rbExtractDataOnly.AutoSize = true;
            this.rbExtractDataOnly.Checked = true;
            this.rbExtractDataOnly.Location = new System.Drawing.Point(6, 19);
            this.rbExtractDataOnly.Name = "rbExtractDataOnly";
            this.rbExtractDataOnly.Size = new System.Drawing.Size(122, 17);
            this.rbExtractDataOnly.TabIndex = 0;
            this.rbExtractDataOnly.TabStop = true;
            this.rbExtractDataOnly.Text = "Extract Content Only";
            this.rbExtractDataOnly.UseVisualStyleBackColor = true;
            // 
            // UnpackNintendoWadForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 309);
            this.Controls.Add(this.groupBox1);
            this.Name = "UnpackNintendoWadForm";
            this.Text = "UnpackNintendoWadForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.UnpackNintendoWadForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbExtractDataOnly;
        private System.Windows.Forms.RadioButton rbExtractAllSections;
        private System.Windows.Forms.CheckBox cbUnpackU8Archives;
    }
}