using System.IO;
using System.Windows.Forms;

using VGMToolbox.dbutil;
using VGMToolbox.plugin;

namespace VGMToolbox.forms.audit
{
    public partial class CollectionBuilderForm : VgmtForm
    {
        private static readonly string DB_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "db"), "collection.s3db");
        
        public CollectionBuilderForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            this.lblTitle.Text = "Collection Builder";
            
            InitializeComponent();

            loadSystemList();
        }

        private void loadSystemList()
        {
            this.comboBox1.DataSource = SqlLiteUtil.GetSimpleDataTable(DB_PATH, "SYSTEMS", "SystemName");
            this.comboBox1.DisplayMember = "SystemName";
            this.comboBox1.ValueMember = "SystemId";
        }
    }
}
