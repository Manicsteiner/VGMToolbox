using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.format.sdat;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.tools.xsf;
using VGMToolbox.util;

namespace VGMToolbox.forms.xsf
{
    public partial class Make2sfForm : AVgmtForm
    {
        Mk2sfWorker.VolumeChangeStruct[] sseqVolumeList;

        public Make2sfForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            string testpackPath =
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), 
                Mk2sfWorker.TESTPACK_PATH);
            
            InitializeComponent();

            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_Make2sf_Title"];
            this.btnDoTask.Text = ConfigurationManager.AppSettings["Form_Make2sf_DoTaskButton"];
            this.tbOutput.Text = ConfigurationManager.AppSettings["Form_Make2sf_IntroText1"] + Environment.NewLine;
            this.tbOutput.Text +=
                String.Format(ConfigurationManager.AppSettings["Form_Make2sf_IntroText2"], Path.GetDirectoryName(testpackPath)) + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_Make2sf_IntroText3"] + Environment.NewLine;
            this.tbOutput.Text += ConfigurationManager.AppSettings["Form_Make2sf_IntroText4"];

            this.grpSourcePaths.Text = ConfigurationManager.AppSettings["Form_Make2sf_GroupSourcePaths"];
            this.lblSdat.Text = ConfigurationManager.AppSettings["Form_Make2sf_LabelSdat"];
            this.lblOutputPath.Text = ConfigurationManager.AppSettings["Form_Make2sf_LabelOutputPath"];
            this.grpSetInformation.Text = ConfigurationManager.AppSettings["Form_Make2sf_GroupSetInformation"];
            this.lblGame.Text = ConfigurationManager.AppSettings["Form_Make2sf_LabelGame"];
            this.lblArtist.Text = ConfigurationManager.AppSettings["Form_Make2sf_LabelArtist"];
            this.lblCopyright.Text = ConfigurationManager.AppSettings["Form_Make2sf_LabelCopyright"];
            this.lblGameSerial.Text = ConfigurationManager.AppSettings["Form_Make2sf_LabelGameSerial"];
            this.lblYear.Text = ConfigurationManager.AppSettings["Form_Make2sf_LabelYear"];

            this.dataGridSseq.Columns[0].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader0"];
            this.dataGridSseq.Columns[1].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader1"];
            this.dataGridSseq.Columns[2].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader2"];
            this.dataGridSseq.Columns[3].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader3"];
            this.dataGridSseq.Columns[4].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader4"];
            this.dataGridSseq.Columns[5].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader5"];
            this.dataGridSseq.Columns[6].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader6"];
            this.dataGridSseq.Columns[7].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader7"];
            this.dataGridSseq.Columns[8].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader8"];
            this.dataGridSseq.Columns[9].HeaderText = ConfigurationManager.AppSettings["Form_Make2sf_ColumnHeader9"];
        }
        
        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            this.tbSource.Text = base.browseForFile(sender, e);
        }
        private void btnDoTask_Click(object sender, EventArgs e)
        {
            ArrayList allowedSequences;
            ArrayList unallowedSequences;

            this.GetAllowedSequences(out allowedSequences, out unallowedSequences);

            if (CheckSdatPathAndOutputDir() && 
                CheckForTestPackNds() &&
                CheckInputs())
            {
                Mk2sfWorker.Mk2sfStruct mk2sfStruct = new Mk2sfWorker.Mk2sfStruct();
                mk2sfStruct.AllowedSequences = allowedSequences;
                mk2sfStruct.UnAllowedSequences = unallowedSequences;
                mk2sfStruct.DestinationFolder = tbOutputPath.Text;
                mk2sfStruct.SourcePath = tbSource.Text;
                mk2sfStruct.GameSerial = tbGameSerial.Text;

                mk2sfStruct.TagArtist = tbArtist.Text;
                mk2sfStruct.TagCopyright = tbCopyright.Text;
                mk2sfStruct.TagYear = tbYear.Text;
                mk2sfStruct.TagGame = tbGame.Text;

                mk2sfStruct.VolumeChangeList = this.sseqVolumeList;

                base.backgroundWorker_Execute(mk2sfStruct);
            }
        }

        private void tbSource_TextChanged(object sender, EventArgs e)
        {
            this.LoadComboBox(this.tbSource.Text);
        }

        private void LoadComboBox(string pSourcePath)
        {
            Smap.SmapSeqStruct s;

            this.dataGridSseq.Rows.Clear();
            
            if (Sdat.IsSdat(pSourcePath))
            {
                Smap smap = SdatUtil.GetSmapFromSdat(pSourcePath);                
                DataGridViewRow row = new DataGridViewRow();
                
                if ((smap.SseqSection != null) && (smap.SseqSection.Length > 0))
                {
                    // get duplicates list
                    ArrayList duplicatesList = SdatUtil.GetDuplicateSseqsList(pSourcePath);
                    
                    // setup volume tracking struct
                    this.sseqVolumeList = new Mk2sfWorker.VolumeChangeStruct[smap.SseqSection.Length];
                    
                    // foreach (Smap.SmapSeqStruct s in smap.SseqSection)
                    for (int i = 0; i < smap.SseqSection.Length; i++)
                    {
                        s = smap.SseqSection[i];
                        
                        row = new DataGridViewRow();
                        row.CreateCells(this.dataGridSseq);

                        row.Cells[1].Value = s.number.ToString("x4");

                        if (!String.IsNullOrEmpty(s.name))
                        {
                            row.Cells[0].Value = true;
                            row.Cells[2].Value = s.fileID.ToString();
                            row.Cells[3].Value = s.size.ToString().PadLeft(6, '0');
                            row.Cells[4].Value = s.name.ToString();
                            row.Cells[5].Value = s.bnk.ToString();
                            row.Cells[6].Value = s.vol.ToString();
                            row.Cells[7].Value = s.cpr.ToString();
                            row.Cells[8].Value = s.ppr.ToString();
                            row.Cells[9].Value = s.ply.ToString();

                            // add volume tracking
                            this.sseqVolumeList[i].oldValue = s.vol;
                            this.sseqVolumeList[i].newValue = s.vol;

                            // check for duplicate
                            if (duplicatesList.Contains(i))
                            {
                                row.Cells[0].Value = false;
                                row.DefaultCellStyle.BackColor = Color.Crimson;
                            }
                        }
                        else
                        {
                            row.Cells[0].Value = false;
                        }

                        this.dataGridSseq.Rows.Add(row);
                    }
                }
            }
        }
        private void GetAllowedSequences(out ArrayList pAllowedSequences, 
            out ArrayList pUnAllowedSequences)
        {
            ArrayList allowedSequences = new ArrayList();
            ArrayList unallowedSequences = new ArrayList();

            foreach (DataGridViewRow r in dataGridSseq.Rows)
            {
                if (((bool)r.Cells[0].Value) &&
                    (r.Cells[4].Value != null) &&
                    (!String.IsNullOrEmpty((string)r.Cells[4].Value)))
                {
                    allowedSequences.Add(int.Parse((string)r.Cells[1].Value, System.Globalization.NumberStyles.HexNumber));
                }
                else if (!((bool)r.Cells[0].Value) &&
                          (r.Cells[4].Value != null) &&
                          (!String.IsNullOrEmpty((string)r.Cells[4].Value)))
                {
                    unallowedSequences.Add(int.Parse((string)r.Cells[1].Value, System.Globalization.NumberStyles.HexNumber));
                }
            }

            pAllowedSequences = allowedSequences;
            pUnAllowedSequences = unallowedSequences;
        }
        private static bool CheckForTestPackNds()
        {
            bool ret = true;
            string testpackPath = 
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Mk2sfWorker.TESTPACK_PATH);

            if (!File.Exists(testpackPath))
            {
                ret = false;
                MessageBox.Show(String.Format(ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageTestpackMissing"],
                    Path.GetFileName(testpackPath), Path.GetDirectoryName(testpackPath)),
                    String.Format(ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageTestpackMissingHeader"], Path.GetFileName(testpackPath)));
            }
            else
            {
                using (FileStream fs = File.OpenRead(testpackPath))
                {
                    if (!ChecksumUtil.GetCrc32OfFullFile(fs).Equals(Mk2sfWorker.TESTPACK_CRC32))
                    {
                        ret = false;
                        MessageBox.Show(String.Format(ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageTestpackCrc32"],
                            Path.GetFileName(testpackPath), Path.GetDirectoryName(testpackPath), Mk2sfWorker.TESTPACK_CRC32),
                            String.Format(ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageTestpackCrc32Header"], Path.GetFileName(testpackPath)));                    
                    }
                }                
            }

            return ret;
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            this.tbOutputPath.Text = base.browseForFolder(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new Mk2sfWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_Make2sf_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_Make2sf_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_Make2sf_MessageBegin"];
        }

        private bool CheckSdatPathAndOutputDir()
        {
            bool ret = true;
            
            string sdatSourceDirectory = Path.GetDirectoryName(Path.GetFullPath(tbSource.Text));

            if (sdatSourceDirectory.Equals(tbOutputPath.Text.Trim()))
            {
                ret = false;
                MessageBox.Show(ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageOutputFolder"],
                    ConfigurationManager.AppSettings["Form_Make2sf_ErrorMessageOutputFolderHeader"]);             
            }

            return ret;
        }
        private bool CheckInputs()
        {
            return AVgmtForm.checkFolderExists(tbOutputPath.Text, this.lblOutputPath.Text);
        }

        private void dataGridSseq_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string errMessage = null;
            
            if ((e.ColumnIndex == 6)) // volume column 
            {
                DataGridViewCell volumeCell = this.dataGridSseq[e.ColumnIndex, e.RowIndex];
                DataGridViewCell fileIdCell = this.dataGridSseq[2, e.RowIndex];

                if (!String.IsNullOrEmpty((string)fileIdCell.Value))
                {
                    int newVolume;

                    if (int.TryParse((string)volumeCell.Value, out newVolume))
                    {
                        if (newVolume < 0 || newVolume > 255)
                        {
                            errMessage = "Please enter a value between 0 and 255";
                            volumeCell.Value = this.sseqVolumeList[e.RowIndex].oldValue.ToString();
                        }
                        else
                        {
                            this.sseqVolumeList[e.RowIndex].newValue = newVolume;
                        }
                    }
                    else
                    {
                        errMessage = String.Format("Cannot convert [{0}] to an integer.  Please use decimal values only.", volumeCell.Value);
                        volumeCell.Value = this.sseqVolumeList[e.RowIndex].oldValue.ToString();
                    }
                }
                else
                {
                    errMessage = "Volume cannot be adjusted for empty sequences.";
                    volumeCell.Value = "";
                }
            }

            
            // display error if needed
            if (!String.IsNullOrEmpty(errMessage))
            {
                MessageBox.Show(errMessage, "Error");
            }
        }
    }
}
