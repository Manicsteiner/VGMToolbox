using System;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.util;

namespace VGMToolbox.forms.examine
{
    public partial class SingleTagUpdateForm : Form
    {
        VGMToolbox.util.NodeTagStruct nodeTagInfo;
        ISingleTagFormat vgmData;
        
        public SingleTagUpdateForm(VGMToolbox.util.NodeTagStruct pNts)
        {
            nodeTagInfo = pNts;
            
            InitializeComponent();

            loadCurrentTagInformation();
        }

        private void loadCurrentTagInformation()
        {
            using (FileStream fs =
                File.Open(this.nodeTagInfo.FilePath, FileMode.Open, FileAccess.Read))
            {
                this.vgmData =
                    (ISingleTagFormat)Activator.CreateInstance(Type.GetType(this.nodeTagInfo.ObjectType));
                this.vgmData.Initialize(fs, this.nodeTagInfo.FilePath);

                this.tbTag.Text = this.vgmData.GetTagAsText();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.vgmData.UpdateTag(this.tbTag.Text);

            MessageBox.Show(String.Format("Tags for \"{0}\" have been updated.  Changes will not be displayed in the tree until you add the files again.", Path.GetFileName(this.vgmData.FilePath)));
            this.Close();
            this.Dispose();
        }
    }
}
