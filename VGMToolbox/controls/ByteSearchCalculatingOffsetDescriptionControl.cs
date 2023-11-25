using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.util;

namespace VGMToolbox.controls
{
    public partial class ByteSearchCalculatingOffsetDescriptionControl : OffsetDescriptionControl
    {
        public ByteSearchCalculatingOffsetDescriptionControl()
        {
            InitializeComponent();

            this.Reset();
        }

        public string CalculationValue
        {
            get { return this.tbCalculation.Text; }
            set { this.tbCalculation.Text = value; }
        }

        public new bool Enabled
        {
            set
            {
                base.Enabled = value;
                this.tbCalculation.Enabled = value;

                this.label1.Enabled = value;
                this.lblAtOffset.Enabled = value;

                this.label2.Enabled = value;
                this.lblByteString.Enabled = value;

                this.cbStringRelativeLocation.Enabled = value;
                this.tbByteString.Enabled = value;
                this.cbTreatAsHex.Enabled = value;
            }
        }

        public new ByteSearchCalculatingOffsetDescription GetOffsetValues()
        {
            ByteSearchCalculatingOffsetDescription allValues = new ByteSearchCalculatingOffsetDescription();

            allValues.OffsetValue = this.tbOffset.Text;
            allValues.OffsetSize = this.comboSize.Text;
            allValues.OffsetByteOrder = this.comboByteOrder.Text;
            allValues.CalculationString = this.tbCalculation.Text;
            allValues.RelativeLocationToByteString = this.cbStringRelativeLocation.Text;
            allValues.ByteString = this.tbByteString.Text;
            allValues.TreatByteStringAsHex = this.cbTreatAsHex.Checked;

            return allValues;
        }

        public new void Reset()
        {
            base.Reset();
            this.tbCalculation.Clear();

            this.initializeStringRelativeOffsetComboBox();
            this.tbByteString.Clear();
            this.cbTreatAsHex.Checked = false;
        }

        //----------------
        // initialization
        //----------------
        private void initializeStringRelativeOffsetComboBox()
        {
            this.cbStringRelativeLocation.Items.Clear();
            this.cbStringRelativeLocation.Items.Add(ByteSearchCalculatingOffsetDescription.START_OF_STRING);
            this.cbStringRelativeLocation.Items.Add(ByteSearchCalculatingOffsetDescription.END_OF_STRING);

            this.cbStringRelativeLocation.SelectedItem = ByteSearchCalculatingOffsetDescription.START_OF_STRING;
        }
    }
}
