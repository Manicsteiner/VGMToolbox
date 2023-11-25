namespace VGMToolbox.forms.extraction
{
    partial class SavePresetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SavePresetForm));
            this.tbFormatName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.tbAuthor = new System.Windows.Forms.TextBox();
            this.tbDestination = new System.Windows.Forms.TextBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.btnBrowseDestination = new System.Windows.Forms.Button();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.lblNotesWarnings = new System.Windows.Forms.Label();
            this.tbNotesWarnings = new System.Windows.Forms.TextBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.grpDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbFormatName
            // 
            this.tbFormatName.Location = new System.Drawing.Point(101, 12);
            this.tbFormatName.Name = "tbFormatName";
            this.tbFormatName.Size = new System.Drawing.Size(136, 20);
            this.tbFormatName.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(25, 16);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(70, 13);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Format Name";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(57, 42);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(38, 13);
            this.lblAuthor.TabIndex = 2;
            this.lblAuthor.Text = "Author";
            // 
            // tbAuthor
            // 
            this.tbAuthor.Location = new System.Drawing.Point(101, 39);
            this.tbAuthor.Name = "tbAuthor";
            this.tbAuthor.Size = new System.Drawing.Size(136, 20);
            this.tbAuthor.TabIndex = 3;
            // 
            // tbDestination
            // 
            this.tbDestination.Location = new System.Drawing.Point(101, 99);
            this.tbDestination.Name = "tbDestination";
            this.tbDestination.Size = new System.Drawing.Size(279, 20);
            this.tbDestination.TabIndex = 4;
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(19, 102);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(76, 13);
            this.lblDestination.TabIndex = 5;
            this.lblDestination.Text = "DestinationFile";
            // 
            // btnBrowseDestination
            // 
            this.btnBrowseDestination.Location = new System.Drawing.Point(386, 99);
            this.btnBrowseDestination.Name = "btnBrowseDestination";
            this.btnBrowseDestination.Size = new System.Drawing.Size(24, 20);
            this.btnBrowseDestination.TabIndex = 6;
            this.btnBrowseDestination.Text = "...";
            this.btnBrowseDestination.UseVisualStyleBackColor = true;
            this.btnBrowseDestination.Click += new System.EventHandler(this.btnBrowseDestination_Click);
            // 
            // grpDetails
            // 
            this.grpDetails.Controls.Add(this.lblNotesWarnings);
            this.grpDetails.Controls.Add(this.tbNotesWarnings);
            this.grpDetails.Controls.Add(this.tbAuthor);
            this.grpDetails.Controls.Add(this.tbFormatName);
            this.grpDetails.Controls.Add(this.lblDescription);
            this.grpDetails.Controls.Add(this.lblAuthor);
            this.grpDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDetails.Location = new System.Drawing.Point(0, 0);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(422, 93);
            this.grpDetails.TabIndex = 7;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Details";
            // 
            // lblNotesWarnings
            // 
            this.lblNotesWarnings.AutoSize = true;
            this.lblNotesWarnings.Location = new System.Drawing.Point(0, 68);
            this.lblNotesWarnings.Name = "lblNotesWarnings";
            this.lblNotesWarnings.Size = new System.Drawing.Size(95, 13);
            this.lblNotesWarnings.TabIndex = 8;
            this.lblNotesWarnings.Text = "Notes or Warnings";
            // 
            // tbNotesWarnings
            // 
            this.tbNotesWarnings.Location = new System.Drawing.Point(101, 65);
            this.tbNotesWarnings.Name = "tbNotesWarnings";
            this.tbNotesWarnings.Size = new System.Drawing.Size(309, 20);
            this.tbNotesWarnings.TabIndex = 7;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(338, 125);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(72, 20);
            this.btnSaveSettings.TabIndex = 8;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // SavePresetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 151);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.grpDetails);
            this.Controls.Add(this.tbDestination);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.btnBrowseDestination);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SavePresetForm";
            this.Text = "Save Preset";
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFormatName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.TextBox tbAuthor;
        private System.Windows.Forms.TextBox tbDestination;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Button btnBrowseDestination;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblNotesWarnings;
        private System.Windows.Forms.TextBox tbNotesWarnings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}