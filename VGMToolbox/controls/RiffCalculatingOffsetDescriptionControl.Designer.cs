namespace VGMToolbox.controls
{
    partial class RiffCalculatingOffsetDescriptionControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.cbRiffRelativeLocation = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRiffChunkList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboByteOrder
            // 
            this.comboByteOrder.Location = new System.Drawing.Point(202, 32);
            // 
            // comboSize
            // 
            this.comboSize.Location = new System.Drawing.Point(68, 32);
            // 
            // tbOffset
            // 
            this.tbOffset.Location = new System.Drawing.Point(35, 3);
            // 
            // lblByteOrder
            // 
            this.lblByteOrder.Location = new System.Drawing.Point(121, 35);
            // 
            // lblSizeLabel
            // 
            this.lblSizeLabel.Location = new System.Drawing.Point(-4, 35);
            // 
            // tbCalculation
            // 
            this.tbCalculation.Location = new System.Drawing.Point(307, 58);
            this.tbCalculation.Name = "tbCalculation";
            this.tbCalculation.Size = new System.Drawing.Size(87, 20);
            this.tbCalculation.TabIndex = 22;
            this.toolTip1.SetToolTip(this.tbCalculation, "use $V to represent the value at the offset");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-4, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "and do calculation (use $V to represent the value at the offset)";
            // 
            // lblAtOffset
            // 
            this.lblAtOffset.AutoSize = true;
            this.lblAtOffset.Location = new System.Drawing.Point(-4, 6);
            this.lblAtOffset.Name = "lblAtOffset";
            this.lblAtOffset.Size = new System.Drawing.Size(33, 13);
            this.lblAtOffset.TabIndex = 24;
            this.lblAtOffset.Text = "offset";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "from the";
            // 
            // cbRiffRelativeLocation
            // 
            this.cbRiffRelativeLocation.FormattingEnabled = true;
            this.cbRiffRelativeLocation.Location = new System.Drawing.Point(162, 3);
            this.cbRiffRelativeLocation.Name = "cbRiffRelativeLocation";
            this.cbRiffRelativeLocation.Size = new System.Drawing.Size(64, 21);
            this.cbRiffRelativeLocation.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(232, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "of RIFF chunk";
            // 
            // cbRiffChunkList
            // 
            this.cbRiffChunkList.FormattingEnabled = true;
            this.cbRiffChunkList.Location = new System.Drawing.Point(313, 3);
            this.cbRiffChunkList.Name = "cbRiffChunkList";
            this.cbRiffChunkList.Size = new System.Drawing.Size(81, 21);
            this.cbRiffChunkList.TabIndex = 28;
            // 
            // RiffCalculatingOffsetDescriptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbRiffChunkList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbRiffRelativeLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAtOffset);
            this.Controls.Add(this.tbCalculation);
            this.Name = "RiffCalculatingOffsetDescriptionControl";
            this.Size = new System.Drawing.Size(399, 82);
            this.Controls.SetChildIndex(this.tbOffset, 0);
            this.Controls.SetChildIndex(this.tbCalculation, 0);
            this.Controls.SetChildIndex(this.comboSize, 0);
            this.Controls.SetChildIndex(this.lblAtOffset, 0);
            this.Controls.SetChildIndex(this.lblByteOrder, 0);
            this.Controls.SetChildIndex(this.lblSizeLabel, 0);
            this.Controls.SetChildIndex(this.comboByteOrder, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cbRiffRelativeLocation, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cbRiffChunkList, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCalculation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblAtOffset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbRiffRelativeLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRiffChunkList;
    }
}
