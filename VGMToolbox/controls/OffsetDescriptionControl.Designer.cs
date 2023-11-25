namespace VGMToolbox.controls
{
    partial class OffsetDescriptionControl
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
            this.comboByteOrder = new System.Windows.Forms.ComboBox();
            this.lblByteOrder = new System.Windows.Forms.Label();
            this.comboSize = new System.Windows.Forms.ComboBox();
            this.lblSizeLabel = new System.Windows.Forms.Label();
            this.tbOffset = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboByteOrder
            // 
            this.comboByteOrder.FormattingEnabled = true;
            this.comboByteOrder.Location = new System.Drawing.Point(283, 3);
            this.comboByteOrder.Name = "comboByteOrder";
            this.comboByteOrder.Size = new System.Drawing.Size(89, 21);
            this.comboByteOrder.TabIndex = 20;
            this.comboByteOrder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboByteOrder_KeyPress);
            this.comboByteOrder.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboByteOrder_KeyDown);
            // 
            // lblByteOrder
            // 
            this.lblByteOrder.AutoSize = true;
            this.lblByteOrder.Location = new System.Drawing.Point(201, 6);
            this.lblByteOrder.Name = "lblByteOrder";
            this.lblByteOrder.Size = new System.Drawing.Size(75, 13);
            this.lblByteOrder.TabIndex = 19;
            this.lblByteOrder.Text = "and byte order";
            // 
            // comboSize
            // 
            this.comboSize.FormattingEnabled = true;
            this.comboSize.Location = new System.Drawing.Point(147, 3);
            this.comboSize.Name = "comboSize";
            this.comboSize.Size = new System.Drawing.Size(47, 21);
            this.comboSize.TabIndex = 18;
            this.comboSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboSize_KeyPress);
            this.comboSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboSize_KeyDown);
            // 
            // lblSizeLabel
            // 
            this.lblSizeLabel.AutoSize = true;
            this.lblSizeLabel.Location = new System.Drawing.Point(76, 6);
            this.lblSizeLabel.Name = "lblSizeLabel";
            this.lblSizeLabel.Size = new System.Drawing.Size(66, 13);
            this.lblSizeLabel.TabIndex = 17;
            this.lblSizeLabel.Text = "and has size";
            // 
            // tbOffset
            // 
            this.tbOffset.Location = new System.Drawing.Point(0, 3);
            this.tbOffset.Name = "tbOffset";
            this.tbOffset.Size = new System.Drawing.Size(70, 20);
            this.tbOffset.TabIndex = 16;
            // 
            // OffsetDescriptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboByteOrder);
            this.Controls.Add(this.lblByteOrder);
            this.Controls.Add(this.comboSize);
            this.Controls.Add(this.lblSizeLabel);
            this.Controls.Add(this.tbOffset);
            this.Name = "OffsetDescriptionControl";
            this.Size = new System.Drawing.Size(372, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.ComboBox comboByteOrder;
        protected System.Windows.Forms.ComboBox comboSize;
        protected System.Windows.Forms.TextBox tbOffset;
        protected System.Windows.Forms.Label lblByteOrder;
        protected System.Windows.Forms.Label lblSizeLabel;
    }
}
