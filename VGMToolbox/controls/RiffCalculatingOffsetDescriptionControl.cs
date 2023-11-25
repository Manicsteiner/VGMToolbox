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
    public partial class RiffCalculatingOffsetDescriptionControl : OffsetDescriptionControl
    {
        public RiffCalculatingOffsetDescriptionControl()
        {
            InitializeComponent();

            initializeRiffRelativeOffsetComboBox();
            initializeRiffChunkComboBox();
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
                
                this.label2.Enabled = value;
                this.label3.Enabled = value;

                this.cbRiffRelativeLocation.Enabled = value;
                this.cbRiffChunkList.Enabled = value;
            }
        }

        public new RiffCalculatingOffsetDescription GetOffsetValues()
        {
            RiffCalculatingOffsetDescription allValues = new RiffCalculatingOffsetDescription();

            allValues.OffsetValue = this.tbOffset.Text;
            allValues.OffsetSize = this.comboSize.Text;
            allValues.OffsetByteOrder = this.comboByteOrder.Text;
            allValues.CalculationString = this.tbCalculation.Text;
            allValues.RelativeLocationToRiffChunkString = this.cbRiffRelativeLocation.Text;
            allValues.RiffChunkString = this.cbRiffChunkList.Text;

            return allValues;
        }

        public new void Reset()
        {
            base.Reset();
            this.tbCalculation.Clear();

            this.initializeRiffRelativeOffsetComboBox();
            this.initializeRiffChunkComboBox();
        }

        //----------------
        // initialization
        //----------------
        private void initializeRiffRelativeOffsetComboBox()
        {
            this.cbRiffRelativeLocation.Items.Clear();
            this.cbRiffRelativeLocation.Items.Add(RiffCalculatingOffsetDescription.START_OF_STRING);
            this.cbRiffRelativeLocation.Items.Add(RiffCalculatingOffsetDescription.END_OF_STRING);

            this.cbRiffRelativeLocation.SelectedItem = RiffCalculatingOffsetDescription.START_OF_STRING;
        }

        private void initializeRiffChunkComboBox()
        {
            this.cbRiffChunkList.Items.Clear();

            this.cbRiffChunkList.Items.Add("fact");
            this.cbRiffChunkList.Items.Add("fmt");
            this.cbRiffChunkList.Items.Add("smpl");
        }
    }
}
