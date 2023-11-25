namespace VGMToolbox.controls
{
    partial class SortableFileListControl
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
            this.lbFileList = new System.Windows.Forms.ListBox();
            this.btnMoveFileUp = new System.Windows.Forms.Button();
            this.btnMoveFileDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbFileList
            // 
            this.lbFileList.FormattingEnabled = true;
            this.lbFileList.Location = new System.Drawing.Point(3, 3);
            this.lbFileList.Name = "lbFileList";
            this.lbFileList.ScrollAlwaysVisible = true;
            this.lbFileList.Size = new System.Drawing.Size(421, 186);
            this.lbFileList.TabIndex = 0;
            // 
            // btnMoveFileUp
            // 
            this.btnMoveFileUp.Location = new System.Drawing.Point(430, 92);
            this.btnMoveFileUp.Name = "btnMoveFileUp";
            this.btnMoveFileUp.Size = new System.Drawing.Size(75, 23);
            this.btnMoveFileUp.TabIndex = 1;
            this.btnMoveFileUp.Text = "Move Up";
            this.btnMoveFileUp.UseVisualStyleBackColor = true;
            this.btnMoveFileUp.Click += new System.EventHandler(this.btnMoveFileUp_Click);
            // 
            // btnMoveFileDown
            // 
            this.btnMoveFileDown.Location = new System.Drawing.Point(430, 121);
            this.btnMoveFileDown.Name = "btnMoveFileDown";
            this.btnMoveFileDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveFileDown.TabIndex = 2;
            this.btnMoveFileDown.Text = "Move Down";
            this.btnMoveFileDown.UseVisualStyleBackColor = true;
            this.btnMoveFileDown.Click += new System.EventHandler(this.btnMoveFileDown_Click);
            // 
            // SortableFileListControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnMoveFileDown);
            this.Controls.Add(this.btnMoveFileUp);
            this.Controls.Add(this.lbFileList);
            this.Name = "SortableFileListControl";
            this.Size = new System.Drawing.Size(508, 193);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.SortableFileListControl_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.SortableFileListControl_DragEnter);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbFileList;
        private System.Windows.Forms.Button btnMoveFileUp;
        private System.Windows.Forms.Button btnMoveFileDown;
    }
}
