using System;
using System.IO;
using System.Windows.Forms;

namespace VGMToolbox.controls
{
    public partial class SortableFileListControl : UserControl
    {
        public SortableFileListControl()
        {
            InitializeComponent();

            this.lbFileList.HorizontalScrollbar = true;
            
        }

        private void SortableFileListControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void SortableFileListControl_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string pathName in fileList)
            {
                if (File.Exists(pathName))
                {
                    this.ProcessFile(pathName);
                }
                else if (Directory.Exists(pathName))
                {
                    this.ProcessDirectory(pathName);
                }
            }
        }

        private void ProcessDirectory(string pPath)
        {
            foreach (string d in Directory.GetDirectories(pPath))
            {
                this.ProcessDirectory(d);
            }

            foreach (string f in Directory.GetFiles(pPath))
            {
                this.ProcessFile(f);
            }
        }

        private void ProcessFile(string pPath)
        {
            this.lbFileList.Items.Add(pPath);
        }

        private void btnMoveFileUp_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.lbFileList.SelectedIndex;

            if (selectedIndex != 0)
            {
                string selectedText = this.lbFileList.Items[selectedIndex].ToString();
                string destinationText = this.lbFileList.Items[selectedIndex - 1].ToString();

                this.lbFileList.Items[selectedIndex] = destinationText;
                this.lbFileList.Items[selectedIndex - 1] = selectedText;

                this.lbFileList.SelectedIndex -= 1;
            }
        }

        private void btnMoveFileDown_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.lbFileList.SelectedIndex;
            
            if (selectedIndex != (this.lbFileList.Items.Count - 1))
            {
                string selectedText = this.lbFileList.Items[selectedIndex].ToString();
                string destinationText = this.lbFileList.Items[selectedIndex + 1].ToString();

                this.lbFileList.Items[selectedIndex] = destinationText;
                this.lbFileList.Items[selectedIndex + 1] = selectedText;

                this.lbFileList.SelectedIndex += 1;
            }
        }

        public void ShowUpDownButtons(bool showButtons)
        {
            this.btnMoveFileUp.Visible = showButtons;
            this.btnMoveFileDown.Visible = showButtons;
        }

        public string[] GetFileList()
        {
            int index = 0;
            string[] ret = new string[this.lbFileList.Items.Count];

            foreach (object item in this.lbFileList.Items)
            {
                ret[index++] = item.ToString();
            }

            return ret;
        }
    }
}
