using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using VGMToolbox.tools;

namespace VGMToolbox.forms.extraction
{
    public partial class SavePresetForm : Form
    {
        ISerializablePreset template;

        public SavePresetForm(ISerializablePreset preset)
        {
            InitializeComponent();

            this.template = preset;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            this.template.Header.FormatName = this.tbFormatName.Text;
            this.template.Header.Author = this.tbAuthor.Text;
            this.template.NotesOrWarnings = this.tbNotesWarnings.Text;

            if (String.IsNullOrEmpty(this.template.Header.FormatName))
            {
                MessageBox.Show("Please enter a Format Name.");
            }
            else if (String.IsNullOrEmpty(this.tbDestination.Text))
            {
                MessageBox.Show("Please enter an output file path.");
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(this.template.GetType());

                TextWriter textWriter = new StreamWriter(this.tbDestination.Text);
                serializer.Serialize(textWriter, this.template);
                textWriter.Close();
                textWriter.Dispose();

                this.Close();
                this.Dispose();
            }
        }

        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.tbDestination.Text = saveFileDialog.FileName;
            }
        }
    }
}
