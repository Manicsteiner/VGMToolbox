using System.Windows.Forms;

using VGMToolbox.plugin;
using VGMToolbox.util;


namespace VGMToolbox.controls
{
    public partial class OffsetDescriptionControl : UserControl
    {
        public OffsetDescriptionControl()
        {
            InitializeComponent();

            this.InitializeSizeComboBox();
            this.InitializeByteOrderComboBox();
        }

        //---------------
        // properties
        //---------------
        public string OffsetValue
        {
            get { return this.tbOffset.Text; }
            set { this.tbOffset.Text = value; }
        }
        
        public string OffsetSize
        {
            get { return this.comboSize.Text; }
            set { this.comboSize.Text = value; }
        }

        public string OffsetByteOrder
        {
            get { return this.comboByteOrder.Text; }
            set { this.comboByteOrder.Text = value; }
        }

        public new bool Enabled
        {
            set 
            {
                this.tbOffset.Enabled = value;
                this.comboSize.Enabled = value;
                this.comboByteOrder.Enabled = value;

                this.lblSizeLabel.Enabled = value;
                this.lblByteOrder.Enabled = value;
            }
        }

        //---------
        // methods
        //---------
        public OffsetDescription GetOffsetValues()
        {
            OffsetDescription allValues = new OffsetDescription();

            allValues.OffsetValue = this.tbOffset.Text;
            allValues.OffsetSize = this.comboSize.Text;
            allValues.OffsetByteOrder = this.comboByteOrder.Text;

            return allValues;
        }
        
        public void SetOffsetValues(string offset, string size, string byteOrder)
        {
            this.tbOffset.Text = offset;
            this.comboSize.Text = size;
            this.comboByteOrder.Text = byteOrder;
        }

        public bool IsValid(string offsetLabel)
        {
            bool isValid = true;

            isValid &= AVgmtForm.checkTextBox(this.tbOffset.Text, offsetLabel + " Offset");
            isValid &= AVgmtForm.checkTextBox(this.comboSize.Text, offsetLabel + " Size");
            isValid &= AVgmtForm.checkTextBox(this.comboByteOrder.Text, offsetLabel + " Byte Order");

            return isValid;
        }

        //----------------
        // initialization
        //----------------
        public void InitializeSizeComboBox()
        {
            this.comboSize.Items.Clear();
            this.comboSize.Items.Add("1");
            this.comboSize.Items.Add("2");
            this.comboSize.Items.Add("4");

            this.comboSize.SelectedItem = "4";
        }

        public void InitializeByteOrderComboBox()
        {
            this.comboByteOrder.Items.Clear();
            this.comboByteOrder.Items.Add(Constants.BigEndianByteOrder);
            this.comboByteOrder.Items.Add(Constants.LittleEndianByteOrder);

            this.comboByteOrder.SelectedItem = Constants.LittleEndianByteOrder;
        }

        //-----------------------
        // combo box key presses
        //-----------------------
        private void comboSize_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void comboSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboByteOrder_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void comboByteOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //---------
        // reset
        //---------
        public void Reset()
        {
            this.tbOffset.Clear();
            this.InitializeSizeComboBox();
            this.InitializeByteOrderComboBox();
        }
    }
}
