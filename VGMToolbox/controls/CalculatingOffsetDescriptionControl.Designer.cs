namespace VGMToolbox.controls
{
    partial class CalculatingOffsetDescriptionControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbCalculation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblAtOffset = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboByteOrder
            // 
            this.comboByteOrder.Location = new System.Drawing.Point(333, 3);
            // 
            // comboSize
            // 
            this.comboSize.Location = new System.Drawing.Point(199, 3);
            // 
            // tbOffset
            // 
            this.tbOffset.Location = new System.Drawing.Point(51, 3);
            // 
            // lblByteOrder
            // 
            this.lblByteOrder.Location = new System.Drawing.Point(252, 6);
            // 
            // lblSizeLabel
            // 
            this.lblSizeLabel.Location = new System.Drawing.Point(127, 6);
            // 
            // tbCalculation
            // 
            this.tbCalculation.Location = new System.Drawing.Point(314, 30);
            this.tbCalculation.Name = "tbCalculation";
            this.tbCalculation.Size = new System.Drawing.Size(108, 20);
            this.tbCalculation.TabIndex = 22;
            this.toolTip1.SetToolTip(this.tbCalculation, "use $V to represent the value at the offset");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "and do calculation (use $V to represent the value at the offset)";
            // 
            // lblAtOffset
            // 
            this.lblAtOffset.AutoSize = true;
            this.lblAtOffset.Location = new System.Drawing.Point(-2, 6);
            this.lblAtOffset.Name = "lblAtOffset";
            this.lblAtOffset.Size = new System.Drawing.Size(47, 13);
            this.lblAtOffset.TabIndex = 24;
            this.lblAtOffset.Text = "at Offset";
            // 
            // CalculatingOffsetDescriptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAtOffset);
            this.Controls.Add(this.tbCalculation);
            this.Name = "CalculatingOffsetDescriptionControl";
            this.Size = new System.Drawing.Size(422, 52);
            this.Controls.SetChildIndex(this.tbOffset, 0);
            this.Controls.SetChildIndex(this.tbCalculation, 0);
            this.Controls.SetChildIndex(this.comboSize, 0);
            this.Controls.SetChildIndex(this.lblAtOffset, 0);
            this.Controls.SetChildIndex(this.lblByteOrder, 0);
            this.Controls.SetChildIndex(this.lblSizeLabel, 0);
            this.Controls.SetChildIndex(this.comboByteOrder, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCalculation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblAtOffset;
    }
}
