namespace VGMToolbox.forms.examine
{
    partial class VgmTagsUpdateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VgmTagsUpdateForm));
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpComments = new System.Windows.Forms.GroupBox();
            this.tbComments = new System.Windows.Forms.TextBox();
            this.grpTrackTags = new System.Windows.Forms.GroupBox();
            this.lblTrackTitleJp = new System.Windows.Forms.Label();
            this.tbTitleJp = new System.Windows.Forms.TextBox();
            this.lblTrackTitleEn = new System.Windows.Forms.Label();
            this.tbTitleEn = new System.Windows.Forms.TextBox();
            this.grpSetTags = new System.Windows.Forms.GroupBox();
            this.cbSystemJp = new System.Windows.Forms.ComboBox();
            this.cbSystemEn = new System.Windows.Forms.ComboBox();
            this.lblGameDate = new System.Windows.Forms.Label();
            this.tbGameDate = new System.Windows.Forms.TextBox();
            this.tbRipper = new System.Windows.Forms.TextBox();
            this.lblRipper = new System.Windows.Forms.Label();
            this.lblArtistJp = new System.Windows.Forms.Label();
            this.tbArtistJp = new System.Windows.Forms.TextBox();
            this.lblSystemJp = new System.Windows.Forms.Label();
            this.lblGameJp = new System.Windows.Forms.Label();
            this.tbGameJp = new System.Windows.Forms.TextBox();
            this.lblSystemEn = new System.Windows.Forms.Label();
            this.lblArtistEn = new System.Windows.Forms.Label();
            this.tbArtistEn = new System.Windows.Forms.TextBox();
            this.lblGameEn = new System.Windows.Forms.Label();
            this.tbGameEn = new System.Windows.Forms.TextBox();
            this.grpComments.SuspendLayout();
            this.grpTrackTags.SuspendLayout();
            this.grpSetTags.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(121, 346);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 12;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(202, 346);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpComments
            // 
            this.grpComments.Controls.Add(this.tbComments);
            this.grpComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpComments.Location = new System.Drawing.Point(0, 270);
            this.grpComments.Name = "grpComments";
            this.grpComments.Size = new System.Drawing.Size(281, 70);
            this.grpComments.TabIndex = 31;
            this.grpComments.TabStop = false;
            this.grpComments.Text = "Comments";
            // 
            // tbComments
            // 
            this.tbComments.AcceptsReturn = true;
            this.tbComments.Location = new System.Drawing.Point(6, 15);
            this.tbComments.Multiline = true;
            this.tbComments.Name = "tbComments";
            this.tbComments.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbComments.Size = new System.Drawing.Size(272, 49);
            this.tbComments.TabIndex = 11;
            // 
            // grpTrackTags
            // 
            this.grpTrackTags.Controls.Add(this.lblTrackTitleJp);
            this.grpTrackTags.Controls.Add(this.tbTitleJp);
            this.grpTrackTags.Controls.Add(this.lblTrackTitleEn);
            this.grpTrackTags.Controls.Add(this.tbTitleEn);
            this.grpTrackTags.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTrackTags.Location = new System.Drawing.Point(0, 202);
            this.grpTrackTags.Name = "grpTrackTags";
            this.grpTrackTags.Size = new System.Drawing.Size(281, 68);
            this.grpTrackTags.TabIndex = 30;
            this.grpTrackTags.TabStop = false;
            this.grpTrackTags.Text = "Track Tags";
            // 
            // lblTrackTitleJp
            // 
            this.lblTrackTitleJp.AutoSize = true;
            this.lblTrackTitleJp.Location = new System.Drawing.Point(6, 42);
            this.lblTrackTitleJp.Name = "lblTrackTitleJp";
            this.lblTrackTitleJp.Size = new System.Drawing.Size(47, 13);
            this.lblTrackTitleJp.TabIndex = 14;
            this.lblTrackTitleJp.Text = "Title (Jp)";
            // 
            // tbTitleJp
            // 
            this.tbTitleJp.Location = new System.Drawing.Point(64, 39);
            this.tbTitleJp.Name = "tbTitleJp";
            this.tbTitleJp.Size = new System.Drawing.Size(214, 20);
            this.tbTitleJp.TabIndex = 10;
            // 
            // lblTrackTitleEn
            // 
            this.lblTrackTitleEn.AutoSize = true;
            this.lblTrackTitleEn.Location = new System.Drawing.Point(6, 16);
            this.lblTrackTitleEn.Name = "lblTrackTitleEn";
            this.lblTrackTitleEn.Size = new System.Drawing.Size(49, 13);
            this.lblTrackTitleEn.TabIndex = 12;
            this.lblTrackTitleEn.Text = "Title (En)";
            // 
            // tbTitleEn
            // 
            this.tbTitleEn.Location = new System.Drawing.Point(64, 13);
            this.tbTitleEn.Name = "tbTitleEn";
            this.tbTitleEn.Size = new System.Drawing.Size(214, 20);
            this.tbTitleEn.TabIndex = 9;
            // 
            // grpSetTags
            // 
            this.grpSetTags.Controls.Add(this.cbSystemJp);
            this.grpSetTags.Controls.Add(this.cbSystemEn);
            this.grpSetTags.Controls.Add(this.lblGameDate);
            this.grpSetTags.Controls.Add(this.tbGameDate);
            this.grpSetTags.Controls.Add(this.tbRipper);
            this.grpSetTags.Controls.Add(this.lblRipper);
            this.grpSetTags.Controls.Add(this.lblArtistJp);
            this.grpSetTags.Controls.Add(this.tbArtistJp);
            this.grpSetTags.Controls.Add(this.lblSystemJp);
            this.grpSetTags.Controls.Add(this.lblGameJp);
            this.grpSetTags.Controls.Add(this.tbGameJp);
            this.grpSetTags.Controls.Add(this.lblSystemEn);
            this.grpSetTags.Controls.Add(this.lblArtistEn);
            this.grpSetTags.Controls.Add(this.tbArtistEn);
            this.grpSetTags.Controls.Add(this.lblGameEn);
            this.grpSetTags.Controls.Add(this.tbGameEn);
            this.grpSetTags.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSetTags.Location = new System.Drawing.Point(0, 0);
            this.grpSetTags.Name = "grpSetTags";
            this.grpSetTags.Size = new System.Drawing.Size(281, 202);
            this.grpSetTags.TabIndex = 29;
            this.grpSetTags.TabStop = false;
            this.grpSetTags.Text = "Game Tags";
            // 
            // cbSystemJp
            // 
            this.cbSystemJp.FormattingEnabled = true;
            this.cbSystemJp.Location = new System.Drawing.Point(75, 100);
            this.cbSystemJp.Name = "cbSystemJp";
            this.cbSystemJp.Size = new System.Drawing.Size(202, 21);
            this.cbSystemJp.TabIndex = 4;
            // 
            // cbSystemEn
            // 
            this.cbSystemEn.FormattingEnabled = true;
            this.cbSystemEn.Location = new System.Drawing.Point(75, 74);
            this.cbSystemEn.Name = "cbSystemEn";
            this.cbSystemEn.Size = new System.Drawing.Size(202, 21);
            this.cbSystemEn.TabIndex = 3;
            // 
            // lblGameDate
            // 
            this.lblGameDate.AutoSize = true;
            this.lblGameDate.Location = new System.Drawing.Point(6, 181);
            this.lblGameDate.Name = "lblGameDate";
            this.lblGameDate.Size = new System.Drawing.Size(51, 13);
            this.lblGameDate.TabIndex = 23;
            this.lblGameDate.Text = "Rls. Date";
            // 
            // tbGameDate
            // 
            this.tbGameDate.Location = new System.Drawing.Point(63, 178);
            this.tbGameDate.Name = "tbGameDate";
            this.tbGameDate.Size = new System.Drawing.Size(67, 20);
            this.tbGameDate.TabIndex = 7;
            // 
            // tbRipper
            // 
            this.tbRipper.Location = new System.Drawing.Point(180, 178);
            this.tbRipper.Name = "tbRipper";
            this.tbRipper.Size = new System.Drawing.Size(97, 20);
            this.tbRipper.TabIndex = 8;
            // 
            // lblRipper
            // 
            this.lblRipper.AutoSize = true;
            this.lblRipper.Location = new System.Drawing.Point(136, 181);
            this.lblRipper.Name = "lblRipper";
            this.lblRipper.Size = new System.Drawing.Size(38, 13);
            this.lblRipper.TabIndex = 20;
            this.lblRipper.Text = "Ripper";
            // 
            // lblArtistJp
            // 
            this.lblArtistJp.AutoSize = true;
            this.lblArtistJp.Location = new System.Drawing.Point(6, 155);
            this.lblArtistJp.Name = "lblArtistJp";
            this.lblArtistJp.Size = new System.Drawing.Size(50, 13);
            this.lblArtistJp.TabIndex = 18;
            this.lblArtistJp.Text = "Artist (Jp)";
            // 
            // tbArtistJp
            // 
            this.tbArtistJp.Location = new System.Drawing.Point(63, 152);
            this.tbArtistJp.Name = "tbArtistJp";
            this.tbArtistJp.Size = new System.Drawing.Size(214, 20);
            this.tbArtistJp.TabIndex = 6;
            // 
            // lblSystemJp
            // 
            this.lblSystemJp.AutoSize = true;
            this.lblSystemJp.Location = new System.Drawing.Point(6, 103);
            this.lblSystemJp.Name = "lblSystemJp";
            this.lblSystemJp.Size = new System.Drawing.Size(61, 13);
            this.lblSystemJp.TabIndex = 16;
            this.lblSystemJp.Text = "System (Jp)";
            // 
            // lblGameJp
            // 
            this.lblGameJp.AutoSize = true;
            this.lblGameJp.Location = new System.Drawing.Point(6, 48);
            this.lblGameJp.Name = "lblGameJp";
            this.lblGameJp.Size = new System.Drawing.Size(55, 13);
            this.lblGameJp.TabIndex = 14;
            this.lblGameJp.Text = "Game (Jp)";
            // 
            // tbGameJp
            // 
            this.tbGameJp.Location = new System.Drawing.Point(63, 45);
            this.tbGameJp.Name = "tbGameJp";
            this.tbGameJp.Size = new System.Drawing.Size(214, 20);
            this.tbGameJp.TabIndex = 2;
            // 
            // lblSystemEn
            // 
            this.lblSystemEn.AutoSize = true;
            this.lblSystemEn.Location = new System.Drawing.Point(6, 77);
            this.lblSystemEn.Name = "lblSystemEn";
            this.lblSystemEn.Size = new System.Drawing.Size(63, 13);
            this.lblSystemEn.TabIndex = 12;
            this.lblSystemEn.Text = "System (En)";
            // 
            // lblArtistEn
            // 
            this.lblArtistEn.AutoSize = true;
            this.lblArtistEn.Location = new System.Drawing.Point(6, 129);
            this.lblArtistEn.Name = "lblArtistEn";
            this.lblArtistEn.Size = new System.Drawing.Size(52, 13);
            this.lblArtistEn.TabIndex = 2;
            this.lblArtistEn.Text = "Artist (En)";
            // 
            // tbArtistEn
            // 
            this.tbArtistEn.Location = new System.Drawing.Point(63, 126);
            this.tbArtistEn.Name = "tbArtistEn";
            this.tbArtistEn.Size = new System.Drawing.Size(214, 20);
            this.tbArtistEn.TabIndex = 5;
            // 
            // lblGameEn
            // 
            this.lblGameEn.AutoSize = true;
            this.lblGameEn.Location = new System.Drawing.Point(6, 22);
            this.lblGameEn.Name = "lblGameEn";
            this.lblGameEn.Size = new System.Drawing.Size(57, 13);
            this.lblGameEn.TabIndex = 0;
            this.lblGameEn.Text = "Game (En)";
            // 
            // tbGameEn
            // 
            this.tbGameEn.Location = new System.Drawing.Point(63, 19);
            this.tbGameEn.Name = "tbGameEn";
            this.tbGameEn.Size = new System.Drawing.Size(214, 20);
            this.tbGameEn.TabIndex = 1;
            // 
            // VgmTagsUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 370);
            this.Controls.Add(this.grpComments);
            this.Controls.Add(this.grpTrackTags);
            this.Controls.Add(this.grpSetTags);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VgmTagsUpdateForm";
            this.Text = "Update Tags";
            this.grpComments.ResumeLayout(false);
            this.grpComments.PerformLayout();
            this.grpTrackTags.ResumeLayout(false);
            this.grpTrackTags.PerformLayout();
            this.grpSetTags.ResumeLayout(false);
            this.grpSetTags.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpComments;
        private System.Windows.Forms.TextBox tbComments;
        private System.Windows.Forms.GroupBox grpTrackTags;
        private System.Windows.Forms.Label lblTrackTitleJp;
        private System.Windows.Forms.TextBox tbTitleJp;
        private System.Windows.Forms.Label lblTrackTitleEn;
        private System.Windows.Forms.TextBox tbTitleEn;
        private System.Windows.Forms.GroupBox grpSetTags;
        private System.Windows.Forms.ComboBox cbSystemJp;
        private System.Windows.Forms.ComboBox cbSystemEn;
        private System.Windows.Forms.Label lblGameDate;
        private System.Windows.Forms.TextBox tbGameDate;
        private System.Windows.Forms.TextBox tbRipper;
        private System.Windows.Forms.Label lblRipper;
        private System.Windows.Forms.Label lblArtistJp;
        private System.Windows.Forms.TextBox tbArtistJp;
        private System.Windows.Forms.Label lblSystemJp;
        private System.Windows.Forms.Label lblGameJp;
        private System.Windows.Forms.TextBox tbGameJp;
        private System.Windows.Forms.Label lblSystemEn;
        private System.Windows.Forms.Label lblArtistEn;
        private System.Windows.Forms.TextBox tbArtistEn;
        private System.Windows.Forms.Label lblGameEn;
        private System.Windows.Forms.TextBox tbGameEn;
    }
}