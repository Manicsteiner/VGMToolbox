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
    public partial class CalculatingOffsetDescriptionControl : OffsetDescriptionControl
    {
        public CalculatingOffsetDescriptionControl()
        {
            InitializeComponent();
        }

        //------------
        // properties
        //------------
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
            }
        }

        public new CalculatingOffsetDescription GetOffsetValues()
        {
            CalculatingOffsetDescription allValues = new CalculatingOffsetDescription();

            allValues.OffsetValue = this.tbOffset.Text;
            allValues.OffsetSize = this.comboSize.Text;
            allValues.OffsetByteOrder = this.comboByteOrder.Text;
            allValues.CalculationString = this.tbCalculation.Text;

            return allValues;
        }

        public new void Reset()
        {
            base.Reset();
            this.tbCalculation.Clear();
        }
    }
}
