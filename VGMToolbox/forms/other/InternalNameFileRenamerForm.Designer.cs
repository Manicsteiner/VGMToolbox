namespace VGMToolbox.forms.other
{
    partial class InternalNameFileRenamerForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.cbMaintainExtension = new System.Windows.Forms.CheckBox();
            this.tbNameOffset = new System.Windows.Forms.TextBox();
            this.tbNameLength = new System.Windows.Forms.TextBox();
            this.tbTerminatorBytes = new System.Windows.Forms.TextBox();
            this.rbTerminatorBytes = new System.Windows.Forms.RadioButton();
            this.rbNameLength = new System.Windows.Forms.RadioButton();
            this.lblNameOffset = new System.Windows.Forms.Label();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 398);
            this.pnlLabels.Size = new System.Drawing.Size(801, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(801, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 321);
            this.tbOutput.Size = new System.Drawing.Size(801, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 301);
            this.pnlButtons.Size = new System.Drawing.Size(801, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(741, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(681, 0);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.label2);
            this.grpOptions.Controls.Add(this.cbMaintainExtension);
            this.grpOptions.Controls.Add(this.tbNameOffset);
            this.grpOptions.Controls.Add(this.tbNameLength);
            this.grpOptions.Controls.Add(this.tbTerminatorBytes);
            this.grpOptions.Controls.Add(this.rbTerminatorBytes);
            this.grpOptions.Controls.Add(this.rbNameLength);
            this.grpOptions.Controls.Add(this.lblNameOffset);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpOptions.Location = new System.Drawing.Point(0, 215);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(801, 86);
            this.grpOptions.TabIndex = 5;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Enter as Hex String (without 0x prefix)";
            // 
            // cbMaintainExtension
            // 
            this.cbMaintainExtension.AutoSize = true;
            this.cbMaintainExtension.Checked = true;
            this.cbMaintainExtension.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMaintainExtension.Location = new System.Drawing.Point(243, 19);
            this.cbMaintainExtension.Name = "cbMaintainExtension";
            this.cbMaintainExtension.Size = new System.Drawing.Size(157, 17);
            this.cbMaintainExtension.TabIndex = 6;
            this.cbMaintainExtension.Text = "Keep Original File Extension";
            this.cbMaintainExtension.UseVisualStyleBackColor = true;
            // 
            // tbNameOffset
            // 
            this.tbNameOffset.Location = new System.Drawing.Point(116, 17);
            this.tbNameOffset.Name = "tbNameOffset";
            this.tbNameOffset.Size = new System.Drawing.Size(100, 20);
            this.tbNameOffset.TabIndex = 5;
            // 
            // tbNameLength
            // 
            this.tbNameLength.Location = new System.Drawing.Point(116, 40);
            this.tbNameLength.Name = "tbNameLength";
            this.tbNameLength.Size = new System.Drawing.Size(100, 20);
            this.tbNameLength.TabIndex = 4;
            // 
            // tbTerminatorBytes
            // 
            this.tbTerminatorBytes.Location = new System.Drawing.Point(116, 63);
            this.tbTerminatorBytes.Name = "tbTerminatorBytes";
            this.tbTerminatorBytes.Size = new System.Drawing.Size(100, 20);
            this.tbTerminatorBytes.TabIndex = 3;
            // 
            // rbTerminatorBytes
            // 
            this.rbTerminatorBytes.AutoSize = true;
            this.rbTerminatorBytes.Location = new System.Drawing.Point(6, 64);
            this.rbTerminatorBytes.Name = "rbTerminatorBytes";
            this.rbTerminatorBytes.Size = new System.Drawing.Size(104, 17);
            this.rbTerminatorBytes.TabIndex = 2;
            this.rbTerminatorBytes.TabStop = true;
            this.rbTerminatorBytes.Text = "Terminator Bytes";
            this.rbTerminatorBytes.UseVisualStyleBackColor = true;
            this.rbTerminatorBytes.CheckedChanged += new System.EventHandler(this.rbTerminatorBytes_CheckedChanged);
            // 
            // rbNameLength
            // 
            this.rbNameLength.AutoSize = true;
            this.rbNameLength.Location = new System.Drawing.Point(6, 41);
            this.rbNameLength.Name = "rbNameLength";
            this.rbNameLength.Size = new System.Drawing.Size(89, 17);
            this.rbNameLength.TabIndex = 1;
            this.rbNameLength.TabStop = true;
            this.rbNameLength.Text = "Name Length";
            this.rbNameLength.UseVisualStyleBackColor = true;
            this.rbNameLength.CheckedChanged += new System.EventHandler(this.rbNameLength_CheckedChanged);
            // 
            // lblNameOffset
            // 
            this.lblNameOffset.AutoSize = true;
            this.lblNameOffset.Location = new System.Drawing.Point(3, 20);
            this.lblNameOffset.Name = "lblNameOffset";
            this.lblNameOffset.Size = new System.Drawing.Size(66, 13);
            this.lblNameOffset.TabIndex = 0;
            this.lblNameOffset.Text = "Name Offset";
            // 
            // InternalNameFileRenamerForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 439);
            this.Controls.Add(this.grpOptions);
            this.Name = "InternalNameFileRenamerForm";
            this.Text = "InternalNameFileRenamerForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.InternalNameFileRenamerForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
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
        private System.Windows.Forms.Label lblNameOffset;
        private System.Windows.Forms.TextBox tbNameOffset;
        private System.Windows.Forms.TextBox tbNameLength;
        private System.Windows.Forms.TextBox tbTerminatorBytes;
        private System.Windows.Forms.RadioButton rbTerminatorBytes;
        private System.Windows.Forms.RadioButton rbNameLength;
        private System.Windows.Forms.CheckBox cbMaintainExtension;
        private System.Windows.Forms.Label label2;
    }
}