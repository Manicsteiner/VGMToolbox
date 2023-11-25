namespace VGMToolbox.forms.examine
{
    partial class TagViewerForm
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
            this.cbCheckForLibs = new System.Windows.Forms.CheckBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewTools
            // 
            this.treeViewTools.LineColor = System.Drawing.Color.Black;
            this.treeViewTools.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbXsfSource_DragDrop);
            this.treeViewTools.DragEnter += new System.Windows.Forms.DragEventHandler(this.doDragEnter);
            // 
            // splitContainer1
            // 
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cbCheckForLibs);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // cbCheckForLibs
            // 
            this.cbCheckForLibs.AutoSize = true;
            this.cbCheckForLibs.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbCheckForLibs.Location = new System.Drawing.Point(0, 0);
            this.cbCheckForLibs.Name = "cbCheckForLibs";
            this.cbCheckForLibs.Padding = new System.Windows.Forms.Padding(5);
            this.cbCheckForLibs.Size = new System.Drawing.Size(716, 27);
            this.cbCheckForLibs.TabIndex = 1;
            this.cbCheckForLibs.Text = "Check for Missing Library Files ";
            this.cbCheckForLibs.UseVisualStyleBackColor = true;
            // 
            // Examine_TagViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 527);
            this.Name = "TagViewerForm";
            this.Text = "TagViewerForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbCheckForLibs;



    }
}