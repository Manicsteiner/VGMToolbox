using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.forms;
using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;
using VGMToolbox.util;

namespace VGMToolbox.forms.xsf
{
    public partial class SsfMakeAdvancedForm : VgmtForm
    {        
        public const float TOTAL_68000_MEMORY = 0x80000;
        public const float RECT_WIDTH = 140;
        public const float RECT_HEIGHT = 180;

        public static readonly long DATA_START_OFFSET = 0xB000;
        public static readonly float DATA_START_HEIGHT = (DATA_START_OFFSET / TOTAL_68000_MEMORY) * RECT_HEIGHT;

        private Graphics graphicsRenderer;
        private Pen blackPen;

        public SsfMakeAdvancedForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            this.lblTitle.Text = "ssfmake Advanced Front End (note: Python must be installed and in your PATH.)";
            this.btnDoTask.Text = "Make SSFs";
            
            InitializeComponent();
                        
            // setup colors
            this.tbDriverFile.BackColor = Color.Aqua;
            this.tbSequenceFile.BackColor = Color.BlueViolet;
            this.tbDspProgram.BackColor = Color.DarkKhaki;
            this.lbToneData.BackColor = Color.Plum;


            // init memory
            this.InitializeMemoryPictureBox();
            this.RenderMemoryGrid();
        }

        protected void InitializeMemoryPictureBox()
        {
            this.pb68000Memory.Image = new Bitmap((int)RECT_WIDTH, (int)RECT_HEIGHT);
            this.graphicsRenderer = Graphics.FromImage(this.pb68000Memory.Image);
            this.blackPen = new Pen(Color.Black, 1);
        }

        protected void RenderMemoryGrid()
        {
            long? driverFileSize;
            long? seqFileSize;
            long? dspFileSize;
            uint? dspRamNeeded;
            long? tonFileSize;

            long currentOffset = DATA_START_OFFSET;
            float currentHeight = DATA_START_HEIGHT;
            
            float seqHeight;
            float dspHeight;
            float dspRamHeight;
            float tonHeight;

            Brush rectangleColor;

            // draw white background
            this.graphicsRenderer.FillRectangle(Brushes.White, 0, 0, RECT_WIDTH, RECT_HEIGHT);

            #region Render Driver
            if (!String.IsNullOrEmpty(this.tbDriverFile.Text))
            {
                driverFileSize = FileUtil.GetFileSize(this.tbDriverFile.Text);

                if (driverFileSize != null)
                {
                    if (driverFileSize > TOTAL_68000_MEMORY)
                    {
                        rectangleColor = Brushes.Red;
                    }
                    else
                    {
                        rectangleColor = Brushes.Aqua;
                    }

                    this.graphicsRenderer.FillRectangle(rectangleColor, 0, 0, RECT_WIDTH, this.GetRectangleHeight((float)driverFileSize));
                }
            }
            #endregion

            #region Render SEQ
            if (!String.IsNullOrEmpty(this.tbSequenceFile.Text))
            {
                seqFileSize = FileUtil.GetFileSize(this.tbSequenceFile.Text);

                if (seqFileSize != null)
                {
                    if ((currentOffset + seqFileSize) > TOTAL_68000_MEMORY)
                    {
                        rectangleColor = Brushes.Red;
                    }
                    else
                    {
                        rectangleColor = Brushes.BlueViolet;
                    }
                    
                    seqHeight = this.GetRectangleHeight((float)seqFileSize);
                    this.graphicsRenderer.FillRectangle(rectangleColor, 0, currentHeight, RECT_WIDTH, seqHeight);

                    // update current offset/hieght
                    currentOffset += (long)seqFileSize;
                    currentHeight += seqHeight;
                }
            }
            #endregion

            #region Render DSP
            if (!String.IsNullOrEmpty(this.tbDspProgram.Text))
            {
                dspFileSize = FileUtil.GetFileSize(this.tbDspProgram.Text);
                
                // shoudl do some error handling around this
                dspRamNeeded = SegaSaturnSequence.GetDspRamNeeded(this.tbDspProgram.Text);

                if (dspFileSize != null)
                {
                    currentOffset = MathUtil.RoundUpToByteAlignment((long)currentOffset, 0x2000);
                    currentHeight = GetRectangleHeight(currentOffset);

                    if ((currentOffset + dspFileSize) > TOTAL_68000_MEMORY)
                    {
                        rectangleColor = Brushes.Red;
                    }
                    else
                    {
                        rectangleColor = Brushes.DarkKhaki;
                    }

                    dspHeight = this.GetRectangleHeight((float)dspFileSize);
                    this.graphicsRenderer.FillRectangle(rectangleColor, 0, currentHeight, RECT_WIDTH, dspHeight);

                    // update current offset/height
                    currentOffset += (long)dspFileSize;
                    currentHeight += dspHeight;

                    // handle DSP RAM
                    dspRamHeight = this.GetRectangleHeight((float)dspRamNeeded);

                    if ((currentOffset + dspRamHeight) > TOTAL_68000_MEMORY)
                    {
                        rectangleColor = Brushes.Red;
                    }
                    else
                    {
                        rectangleColor = Brushes.Lime;
                    }

                    this.graphicsRenderer.FillRectangle(rectangleColor, 0, currentHeight, RECT_WIDTH, dspRamHeight);
                    this.graphicsRenderer.DrawString("DSP RAM", new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, 5, currentHeight + 1);

                    currentOffset += (long)dspRamNeeded;
                    currentHeight += dspRamHeight;
                }
            }
            #endregion

            #region Render TON

            if (this.lbToneData.Items.Count > 0)
            {
                foreach (ListBoxFileInfoObject listFileObject in this.lbToneData.Items)
                {
                    tonFileSize = FileUtil.GetFileSize(listFileObject.FilePath);

                    if (tonFileSize != null)
                    {
                        if ((currentOffset + tonFileSize) > TOTAL_68000_MEMORY)
                        {
                            rectangleColor = Brushes.Red;
                        }
                        else
                        {
                            rectangleColor = Brushes.Plum;
                        }
                                                
                        tonHeight = this.GetRectangleHeight((float)tonFileSize);
                        this.graphicsRenderer.FillRectangle(rectangleColor, 0, currentHeight, RECT_WIDTH, tonHeight);

                        // update current offset/hieght
                        currentOffset += (long)tonFileSize;
                        currentHeight += tonHeight;
                    }
                }
            }

            #endregion

            //render outline
            this.graphicsRenderer.DrawRectangle(this.blackPen, 0, 0, RECT_WIDTH - this.blackPen.Width, RECT_HEIGHT - this.blackPen.Width);

            // repaint
            this.pb68000Memory.Refresh();
        }

        protected float GetRectangleHeight(float value)
        {
            return (value / TOTAL_68000_MEMORY) * RECT_HEIGHT;
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new SsfMakeWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_SsfMakeFE_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_SsfMakeFE_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_SsfMakeFE_MessageBegin"];
        }

        protected string getDroppedFile(DragEventArgs e)
        {
            string droppedFile = null;
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (File.Exists(s[0])))
            {
                droppedFile = s[0];
            }

            return droppedFile;
        }
        private void tbDriverFile_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void tbDriverFile_DragDrop(object sender, DragEventArgs e)
        {
            this.tbDriverFile.Text = this.getDroppedFile(e);            
        }
        private void tbDriverFile_TextChanged(object sender, EventArgs e)
        {
            this.RenderMemoryGrid();
        }

        private void tbSequenceFile_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void tbSequenceFile_DragDrop(object sender, DragEventArgs e)
        {
            this.tbSequenceFile.Text = this.getDroppedFile(e);

            // get track count
            SegaSaturnSequence s = new SegaSaturnSequence(this.tbSequenceFile.Text);
            this.tbSequenceCount.Text = String.Format("0x{0}", s.SequenceCount.ToString("X2"));

            // (de)activate ssflib checkbox
            if (s.SequenceCount <= 1)
            {
                this.cbMakeSsflib.Checked = false;
                this.cbMakeSsflib.Enabled = false;
            }
            else
            {
                this.cbMakeSsflib.Checked = true;
                this.cbMakeSsflib.Enabled = true;            
            }
        }
        private void tbSequenceFile_TextChanged(object sender, EventArgs e)
        {
            this.RenderMemoryGrid();
        }

        private void tbDspProgram_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void tbDspProgram_DragDrop(object sender, DragEventArgs e)
        {
            this.tbDspProgram.Text = this.getDroppedFile(e);
        }
        private void tbDspProgram_TextChanged(object sender, EventArgs e)
        {
            this.RenderMemoryGrid();
        }

        private void addToneData(string f)
        {
            ListBoxFileInfoObject listFileObject;
            listFileObject = new ListBoxFileInfoObject(f);            
            this.lbToneData.Items.Add(listFileObject);

            this.RenderMemoryGrid();
        }        
        private void lbToneData_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void lbToneData_DragDrop(object sender, DragEventArgs e)
        {
            string filename = this.getDroppedFile(e);
            this.addToneData(filename);
        }
        private void lbToneData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.lbToneData.Items.Remove(this.lbToneData.SelectedItem);

                this.RenderMemoryGrid();
            }
        }


        
    }
}
