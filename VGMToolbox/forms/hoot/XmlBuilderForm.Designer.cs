namespace VGMToolbox.forms.hoot
{
    partial class XmlBuilderForm
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
            this.gbHootXML_Source = new System.Windows.Forms.GroupBox();
            this.gbHootXML_Options = new System.Windows.Forms.GroupBox();
            this.cbParseFile = new System.Windows.Forms.CheckBox();
            this.cbHootXML_SplitOutput = new System.Windows.Forms.CheckBox();
            this.cbHootXML_CombineOutput = new System.Windows.Forms.CheckBox();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.gbHootXML_Source.SuspendLayout();
            this.gbHootXML_Options.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 373);
            this.pnlLabels.Size = new System.Drawing.Size(779, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(779, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 296);
            this.tbOutput.Size = new System.Drawing.Size(779, 77);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 276);
            this.pnlButtons.Size = new System.Drawing.Size(779, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(719, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(659, 0);
            // 
            // gbHootXML_Source
            // 
            this.gbHootXML_Source.Controls.Add(this.gbHootXML_Options);
            this.gbHootXML_Source.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbHootXML_Source.Location = new System.Drawing.Point(0, 23);
            this.gbHootXML_Source.Name = "gbHootXML_Source";
            this.gbHootXML_Source.Size = new System.Drawing.Size(779, 253);
            this.gbHootXML_Source.TabIndex = 5;
            this.gbHootXML_Source.TabStop = false;
            this.gbHootXML_Source.Text = "Source";
            this.gbHootXML_Source.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbHootXML_Path_DragDrop);
            this.gbHootXML_Source.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // gbHootXML_Options
            // 
            this.gbHootXML_Options.Controls.Add(this.cbParseFile);
            this.gbHootXML_Options.Controls.Add(this.cbHootXML_SplitOutput);
            this.gbHootXML_Options.Controls.Add(this.cbHootXML_CombineOutput);
            this.gbHootXML_Options.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbHootXML_Options.Location = new System.Drawing.Point(3, 170);
            this.gbHootXML_Options.Name = "gbHootXML_Options";
            this.gbHootXML_Options.Size = new System.Drawing.Size(773, 80);
            this.gbHootXML_Options.TabIndex = 6;
            this.gbHootXML_Options.TabStop = false;
            this.gbHootXML_Options.Text = "Options";
            // 
            // cbParseFile
            // 
            this.cbParseFile.AutoSize = true;
            this.cbParseFile.Location = new System.Drawing.Point(9, 19);
            this.cbParseFile.Name = "cbParseFile";
            this.cbParseFile.Size = new System.Drawing.Size(314, 17);
            this.cbParseFile.TabIndex = 2;
            this.cbParseFile.Text = "Parse file name according to Knurek style naming convention";
            this.cbParseFile.UseVisualStyleBackColor = true;
            // 
            // cbHootXML_SplitOutput
            // 
            this.cbHootXML_SplitOutput.AutoSize = true;
            this.cbHootXML_SplitOutput.Location = new System.Drawing.Point(9, 58);
            this.cbHootXML_SplitOutput.Name = "cbHootXML_SplitOutput";
            this.cbHootXML_SplitOutput.Size = new System.Drawing.Size(142, 17);
            this.cbHootXML_SplitOutput.TabIndex = 1;
            this.cbHootXML_SplitOutput.Text = "Output one file per game";
            this.cbHootXML_SplitOutput.UseVisualStyleBackColor = true;
            // 
            // cbHootXML_CombineOutput
            // 
            this.cbHootXML_CombineOutput.AutoSize = true;
            this.cbHootXML_CombineOutput.Location = new System.Drawing.Point(9, 39);
            this.cbHootXML_CombineOutput.Name = "cbHootXML_CombineOutput";
            this.cbHootXML_CombineOutput.Size = new System.Drawing.Size(183, 17);
            this.cbHootXML_CombineOutput.TabIndex = 0;
            this.cbHootXML_CombineOutput.Text = "Combine output to single XML file";
            this.cbHootXML_CombineOutput.UseVisualStyleBackColor = true;
            // 
            // Hoot_XmlBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 414);
            this.Controls.Add(this.gbHootXML_Source);
            this.Name = "XmlBuilderForm";
            this.Text = "XmlBuilder";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.gbHootXML_Source, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.gbHootXML_Source.ResumeLayout(false);
            this.gbHootXML_Options.ResumeLayout(false);
            this.gbHootXML_Options.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbHootXML_Source;
        private System.Windows.Forms.GroupBox gbHootXML_Options;
        private System.Windows.Forms.CheckBox cbHootXML_SplitOutput;
        private System.Windows.Forms.CheckBox cbHootXML_CombineOutput;
        private System.Windows.Forms.CheckBox cbParseFile;
    }
}