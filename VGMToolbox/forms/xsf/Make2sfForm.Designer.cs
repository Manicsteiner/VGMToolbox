namespace VGMToolbox.forms.xsf
{
    partial class Make2sfForm
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
            this.grpSourcePaths = new System.Windows.Forms.GroupBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.lblSdat = new System.Windows.Forms.Label();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.grpSetInformation = new System.Windows.Forms.GroupBox();
            this.tbGameSerial = new System.Windows.Forms.TextBox();
            this.lblGameSerial = new System.Windows.Forms.Label();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblGame = new System.Windows.Forms.Label();
            this.tbGame = new System.Windows.Forms.TextBox();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.tbCopyright = new System.Windows.Forms.TextBox();
            this.lblArtist = new System.Windows.Forms.Label();
            this.tbArtist = new System.Windows.Forms.TextBox();
            this.dataGridSseq = new System.Windows.Forms.DataGridView();
            this.CheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.NumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BankColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VolumeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CprColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PprColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLabels.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.grpSourcePaths.SuspendLayout();
            this.grpSetInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSseq)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLabels
            // 
            this.pnlLabels.Location = new System.Drawing.Point(0, 460);
            this.pnlLabels.Size = new System.Drawing.Size(791, 19);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Size = new System.Drawing.Size(791, 20);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(0, 383);
            this.tbOutput.Size = new System.Drawing.Size(791, 77);
            this.toolTip1.SetToolTip(this.tbOutput, "Double-Click to view in your default text editor.");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 363);
            this.pnlButtons.Size = new System.Drawing.Size(791, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(731, 0);
            // 
            // btnDoTask
            // 
            this.btnDoTask.Location = new System.Drawing.Point(671, 0);
            this.btnDoTask.Click += new System.EventHandler(this.btnDoTask_Click);
            // 
            // grpSourcePaths
            // 
            this.grpSourcePaths.Controls.Add(this.btnBrowseOutput);
            this.grpSourcePaths.Controls.Add(this.tbOutputPath);
            this.grpSourcePaths.Controls.Add(this.lblOutputPath);
            this.grpSourcePaths.Controls.Add(this.lblSdat);
            this.grpSourcePaths.Controls.Add(this.btnBrowseSource);
            this.grpSourcePaths.Controls.Add(this.tbSource);
            this.grpSourcePaths.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSourcePaths.Location = new System.Drawing.Point(0, 23);
            this.grpSourcePaths.Name = "grpSourcePaths";
            this.grpSourcePaths.Size = new System.Drawing.Size(791, 72);
            this.grpSourcePaths.TabIndex = 5;
            this.grpSourcePaths.TabStop = false;
            this.grpSourcePaths.Text = "Source Paths";
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(373, 45);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseOutput.TabIndex = 3;
            this.btnBrowseOutput.Text = "...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Location = new System.Drawing.Point(78, 45);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(289, 20);
            this.tbOutputPath.TabIndex = 2;
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.Location = new System.Drawing.Point(6, 48);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(64, 13);
            this.lblOutputPath.TabIndex = 3;
            this.lblOutputPath.Text = "Output Path";
            // 
            // lblSdat
            // 
            this.lblSdat.AutoSize = true;
            this.lblSdat.Location = new System.Drawing.Point(6, 23);
            this.lblSdat.Name = "lblSdat";
            this.lblSdat.Size = new System.Drawing.Size(36, 13);
            this.lblSdat.TabIndex = 2;
            this.lblSdat.Text = "SDAT";
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(373, 19);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(25, 20);
            this.btnBrowseSource.TabIndex = 1;
            this.btnBrowseSource.Text = "...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // tbSource
            // 
            this.tbSource.Location = new System.Drawing.Point(78, 19);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(289, 20);
            this.tbSource.TabIndex = 0;
            this.tbSource.TextChanged += new System.EventHandler(this.tbSource_TextChanged);
            // 
            // grpSetInformation
            // 
            this.grpSetInformation.Controls.Add(this.tbGameSerial);
            this.grpSetInformation.Controls.Add(this.lblGameSerial);
            this.grpSetInformation.Controls.Add(this.tbYear);
            this.grpSetInformation.Controls.Add(this.lblYear);
            this.grpSetInformation.Controls.Add(this.lblGame);
            this.grpSetInformation.Controls.Add(this.tbGame);
            this.grpSetInformation.Controls.Add(this.lblCopyright);
            this.grpSetInformation.Controls.Add(this.tbCopyright);
            this.grpSetInformation.Controls.Add(this.lblArtist);
            this.grpSetInformation.Controls.Add(this.tbArtist);
            this.grpSetInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSetInformation.Location = new System.Drawing.Point(0, 95);
            this.grpSetInformation.Name = "grpSetInformation";
            this.grpSetInformation.Size = new System.Drawing.Size(791, 114);
            this.grpSetInformation.TabIndex = 6;
            this.grpSetInformation.TabStop = false;
            this.grpSetInformation.Text = "Set Information";
            // 
            // tbGameSerial
            // 
            this.tbGameSerial.Location = new System.Drawing.Point(407, 19);
            this.tbGameSerial.Name = "tbGameSerial";
            this.tbGameSerial.Size = new System.Drawing.Size(100, 20);
            this.tbGameSerial.TabIndex = 7;
            // 
            // lblGameSerial
            // 
            this.lblGameSerial.AutoSize = true;
            this.lblGameSerial.Location = new System.Drawing.Point(331, 22);
            this.lblGameSerial.Name = "lblGameSerial";
            this.lblGameSerial.Size = new System.Drawing.Size(64, 13);
            this.lblGameSerial.TabIndex = 8;
            this.lblGameSerial.Text = "Game Serial";
            // 
            // tbYear
            // 
            this.tbYear.Location = new System.Drawing.Point(407, 71);
            this.tbYear.MaxLength = 4;
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(58, 20);
            this.tbYear.TabIndex = 8;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(331, 74);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(29, 13);
            this.lblYear.TabIndex = 6;
            this.lblYear.Text = "Year";
            // 
            // lblGame
            // 
            this.lblGame.AutoSize = true;
            this.lblGame.Location = new System.Drawing.Point(6, 22);
            this.lblGame.Name = "lblGame";
            this.lblGame.Size = new System.Drawing.Size(35, 13);
            this.lblGame.TabIndex = 5;
            this.lblGame.Text = "Game";
            // 
            // tbGame
            // 
            this.tbGame.Location = new System.Drawing.Point(78, 19);
            this.tbGame.Name = "tbGame";
            this.tbGame.Size = new System.Drawing.Size(239, 20);
            this.tbGame.TabIndex = 4;
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(6, 74);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(51, 13);
            this.lblCopyright.TabIndex = 3;
            this.lblCopyright.Text = "Copyright";
            // 
            // tbCopyright
            // 
            this.tbCopyright.Location = new System.Drawing.Point(78, 71);
            this.tbCopyright.Name = "tbCopyright";
            this.tbCopyright.Size = new System.Drawing.Size(239, 20);
            this.tbCopyright.TabIndex = 6;
            // 
            // lblArtist
            // 
            this.lblArtist.AutoSize = true;
            this.lblArtist.Location = new System.Drawing.Point(6, 48);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(30, 13);
            this.lblArtist.TabIndex = 1;
            this.lblArtist.Text = "Artist";
            // 
            // tbArtist
            // 
            this.tbArtist.Location = new System.Drawing.Point(78, 45);
            this.tbArtist.Name = "tbArtist";
            this.tbArtist.Size = new System.Drawing.Size(239, 20);
            this.tbArtist.TabIndex = 5;
            // 
            // dataGridSseq
            // 
            this.dataGridSseq.AllowUserToAddRows = false;
            this.dataGridSseq.AllowUserToDeleteRows = false;
            this.dataGridSseq.AllowUserToResizeColumns = false;
            this.dataGridSseq.AllowUserToResizeRows = false;
            this.dataGridSseq.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridSseq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSseq.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CheckBoxColumn,
            this.NumberColumn,
            this.FileIdColumn,
            this.SizeColumn,
            this.NameColumn,
            this.BankColumn,
            this.VolumeColumn,
            this.CprColumn,
            this.PprColumn,
            this.PlyColumn});
            this.dataGridSseq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridSseq.Location = new System.Drawing.Point(0, 209);
            this.dataGridSseq.Name = "dataGridSseq";
            this.dataGridSseq.Size = new System.Drawing.Size(791, 154);
            this.dataGridSseq.TabIndex = 9;
            this.dataGridSseq.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridSseq_CellEndEdit);
            // 
            // CheckBoxColumn
            // 
            this.CheckBoxColumn.HeaderText = "Select";
            this.CheckBoxColumn.Name = "CheckBoxColumn";
            this.CheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CheckBoxColumn.Width = 40;
            // 
            // NumberColumn
            // 
            this.NumberColumn.HeaderText = "Number";
            this.NumberColumn.Name = "NumberColumn";
            this.NumberColumn.ReadOnly = true;
            this.NumberColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.NumberColumn.Width = 45;
            // 
            // FileIdColumn
            // 
            this.FileIdColumn.HeaderText = "File ID";
            this.FileIdColumn.Name = "FileIdColumn";
            this.FileIdColumn.ReadOnly = true;
            this.FileIdColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FileIdColumn.Width = 45;
            // 
            // SizeColumn
            // 
            this.SizeColumn.HeaderText = "Size";
            this.SizeColumn.Name = "SizeColumn";
            this.SizeColumn.ReadOnly = true;
            this.SizeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SizeColumn.Width = 50;
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            this.NameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.NameColumn.Width = 150;
            // 
            // BankColumn
            // 
            this.BankColumn.HeaderText = "Bank";
            this.BankColumn.Name = "BankColumn";
            this.BankColumn.ReadOnly = true;
            this.BankColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.BankColumn.Width = 40;
            // 
            // VolumeColumn
            // 
            this.VolumeColumn.HeaderText = "Volume";
            this.VolumeColumn.MaxInputLength = 3;
            this.VolumeColumn.Name = "VolumeColumn";
            this.VolumeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.VolumeColumn.Width = 45;
            // 
            // CprColumn
            // 
            this.CprColumn.HeaderText = "CPR";
            this.CprColumn.Name = "CprColumn";
            this.CprColumn.ReadOnly = true;
            this.CprColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CprColumn.Width = 40;
            // 
            // PprColumn
            // 
            this.PprColumn.HeaderText = "PPR";
            this.PprColumn.Name = "PprColumn";
            this.PprColumn.ReadOnly = true;
            this.PprColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PprColumn.Width = 40;
            // 
            // PlyColumn
            // 
            this.PlyColumn.HeaderText = "PLY";
            this.PlyColumn.Name = "PlyColumn";
            this.PlyColumn.ReadOnly = true;
            this.PlyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PlyColumn.Width = 40;
            // 
            // Make2sfForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 501);
            this.Controls.Add(this.dataGridSseq);
            this.Controls.Add(this.grpSetInformation);
            this.Controls.Add(this.grpSourcePaths);
            this.Name = "Make2sfForm";
            this.Text = "Make2sfForm";
            this.Controls.SetChildIndex(this.pnlLabels, 0);
            this.Controls.SetChildIndex(this.tbOutput, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.grpSourcePaths, 0);
            this.Controls.SetChildIndex(this.grpSetInformation, 0);
            this.Controls.SetChildIndex(this.dataGridSseq, 0);
            this.pnlLabels.ResumeLayout(false);
            this.pnlLabels.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.grpSourcePaths.ResumeLayout(false);
            this.grpSourcePaths.PerformLayout();
            this.grpSetInformation.ResumeLayout(false);
            this.grpSetInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSseq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSourcePaths;
        private System.Windows.Forms.GroupBox grpSetInformation;
        private System.Windows.Forms.DataGridView dataGridSseq;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.Label lblOutputPath;
        private System.Windows.Forms.Label lblSdat;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.TextBox tbArtist;
        private System.Windows.Forms.TextBox tbYear;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblGame;
        private System.Windows.Forms.TextBox tbGame;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.TextBox tbCopyright;
        private System.Windows.Forms.TextBox tbGameSerial;
        private System.Windows.Forms.Label lblGameSerial;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BankColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn VolumeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CprColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PprColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlyColumn;

    }
}