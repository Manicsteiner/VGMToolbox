namespace VGMToolbox.controls
{
    partial class ByteSearchCalculatingOffsetDescriptionControl
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
            this.cbStringRelativeLocation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAtOffset = new System.Windows.Forms.Label();
            this.lblByteString = new System.Windows.Forms.Label();
            this.tbByteString = new System.Windows.Forms.TextBox();
            this.cbTreatAsHex = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCalculation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboByteOrder
            // 
            this.comboByteOrder.Location = new System.Drawing.Point(209, 48);
            // 
            // comboSize
            // 
            this.comboSize.Location = new System.Drawing.Point(75, 48);
            // 
            // tbOffset
            // 
            this.tbOffset.Location = new System.Drawing.Point(42, 4);
            // 
            // lblByteOrder
            // 
            this.lblByteOrder.Location = new System.Drawing.Point(128, 51);
            // 
            // lblSizeLabel
            // 
            this.lblSizeLabel.Location = new System.Drawing.Point(3, 51);
            // 
            // cbStringRelativeLocation
            // 
            this.cbStringRelativeLocation.FormattingEnabled = true;
            this.cbStringRelativeLocation.Location = new System.Drawing.Point(169, 3);
            this.cbStringRelativeLocation.Name = "cbStringRelativeLocation";
            this.cbStringRelativeLocation.Size = new System.Drawing.Size(64, 21);
            this.cbStringRelativeLocation.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "from the";
            // 
            // lblAtOffset
            // 
            this.lblAtOffset.AutoSize = true;
            this.lblAtOffset.Location = new System.Drawing.Point(3, 7);
            this.lblAtOffset.Name = "lblAtOffset";
            this.lblAtOffset.Size = new System.Drawing.Size(33, 13);
            this.lblAtOffset.TabIndex = 29;
            this.lblAtOffset.Text = "offset";
            // 
            // lblByteString
            // 
            this.lblByteString.AutoSize = true;
            this.lblByteString.Location = new System.Drawing.Point(239, 7);
            this.lblByteString.Name = "lblByteString";
            this.lblByteString.Size = new System.Drawing.Size(50, 13);
            this.lblByteString.TabIndex = 30;
            this.lblByteString.Text = "the string";
            // 
            // tbByteString
            // 
            this.tbByteString.Location = new System.Drawing.Point(295, 3);
            this.tbByteString.Name = "tbByteString";
            this.tbByteString.Size = new System.Drawing.Size(106, 20);
            this.tbByteString.TabIndex = 31;
            // 
            // cbTreatAsHex
            // 
            this.cbTreatAsHex.AutoSize = true;
            this.cbTreatAsHex.Location = new System.Drawing.Point(251, 29);
            this.cbTreatAsHex.Name = "cbTreatAsHex";
            this.cbTreatAsHex.Size = new System.Drawing.Size(87, 17);
            this.cbTreatAsHex.TabIndex = 32;
            this.cbTreatAsHex.Text = "Treat as Hex";
            this.cbTreatAsHex.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "and do calculation (use $V to represent the value at the offset)";
            // 
            // tbCalculation
            // 
            this.tbCalculation.Location = new System.Drawing.Point(314, 69);
            this.tbCalculation.Name = "tbCalculation";
            this.tbCalculation.Size = new System.Drawing.Size(87, 20);
            this.tbCalculation.TabIndex = 33;
            // 
            // ByteSearchCalculatingOffsetDescriptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbCalculation);
            this.Controls.Add(this.cbTreatAsHex);
            this.Controls.Add(this.tbByteString);
            this.Controls.Add(this.lblByteString);
            this.Controls.Add(this.lblAtOffset);
            this.Controls.Add(this.cbStringRelativeLocation);
            this.Controls.Add(this.label2);
            this.Name = "ByteSearchCalculatingOffsetDescriptionControl";
            this.Size = new System.Drawing.Size(404, 92);
            this.Controls.SetChildIndex(this.comboByteOrder, 0);
            this.Controls.SetChildIndex(this.lblByteOrder, 0);
            this.Controls.SetChildIndex(this.comboSize, 0);
            this.Controls.SetChildIndex(this.lblSizeLabel, 0);
            this.Controls.SetChildIndex(this.tbOffset, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cbStringRelativeLocation, 0);
            this.Controls.SetChildIndex(this.lblAtOffset, 0);
            this.Controls.SetChildIndex(this.lblByteString, 0);
            this.Controls.SetChildIndex(this.tbByteString, 0);
            this.Controls.SetChildIndex(this.cbTreatAsHex, 0);
            this.Controls.SetChildIndex(this.tbCalculation, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbStringRelativeLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAtOffset;
        private System.Windows.Forms.Label lblByteString;
        private System.Windows.Forms.TextBox tbByteString;
        private System.Windows.Forms.CheckBox cbTreatAsHex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCalculation;

    }
}
