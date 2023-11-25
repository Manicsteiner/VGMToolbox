namespace VGMToolbox.forms.examine
{
    partial class EmbeddedTagsUpdateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmbeddedTagsUpdateForm));
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbArtist = new System.Windows.Forms.TextBox();
            this.tbCopyright = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblArtist = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.grpTags = new System.Windows.Forms.GroupBox();
            this.grpTags.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(76, 13);
            this.tbName.MaxLength = 31;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(193, 20);
            this.tbName.TabIndex = 0;
            // 
            // tbArtist
            // 
            this.tbArtist.Location = new System.Drawing.Point(76, 39);
            this.tbArtist.MaxLength = 31;
            this.tbArtist.Name = "tbArtist";
            this.tbArtist.Size = new System.Drawing.Size(192, 20);
            this.tbArtist.TabIndex = 1;
            // 
            // tbCopyright
            // 
            this.tbCopyright.Location = new System.Drawing.Point(76, 65);
            this.tbCopyright.MaxLength = 31;
            this.tbCopyright.Name = "tbCopyright";
            this.tbCopyright.Size = new System.Drawing.Size(192, 20);
            this.tbCopyright.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(193, 100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(112, 100);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Name";
            // 
            // lblArtist
            // 
            this.lblArtist.AutoSize = true;
            this.lblArtist.Location = new System.Drawing.Point(9, 42);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(30, 13);
            this.lblArtist.TabIndex = 6;
            this.lblArtist.Text = "Artist";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(9, 68);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(51, 13);
            this.lblCopyright.TabIndex = 7;
            this.lblCopyright.Text = "Copyright";
            // 
            // grpTags
            // 
            this.grpTags.Controls.Add(this.lblName);
            this.grpTags.Controls.Add(this.lblCopyright);
            this.grpTags.Controls.Add(this.tbName);
            this.grpTags.Controls.Add(this.lblArtist);
            this.grpTags.Controls.Add(this.tbArtist);
            this.grpTags.Controls.Add(this.tbCopyright);
            this.grpTags.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTags.Location = new System.Drawing.Point(0, 0);
            this.grpTags.Name = "grpTags";
            this.grpTags.Size = new System.Drawing.Size(283, 94);
            this.grpTags.TabIndex = 8;
            this.grpTags.TabStop = false;
            this.grpTags.Text = "Tags";
            // 
            // EmbeddedTagsUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 132);
            this.Controls.Add(this.grpTags);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EmbeddedTagsUpdateForm";
            this.Text = "Update Tags";
            this.grpTags.ResumeLayout(false);
            this.grpTags.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbArtist;
        private System.Windows.Forms.TextBox tbCopyright;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.GroupBox grpTags;
    }
}