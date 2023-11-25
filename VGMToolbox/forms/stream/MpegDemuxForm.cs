using System;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.tools.stream;

namespace VGMToolbox.forms.stream
{
    public partial class MpegDemuxForm : AVgmtForm
    {
        public MpegDemuxForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = "Video Demultiplexer";

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();            
            
            InitializeComponent();

            this.tbOutput.Text = "Demultiplex streams from movies." + Environment.NewLine;
            this.tbOutput.Text += "- Currently supported formats: BIK, DSI (PS2), DVD Video, MO (Wii Only), MPEG1, MPEG2, PAM, PMF, PSS, SFD, THP, USM, XMV" + Environment.NewLine;
            this.tbOutput.Text += "- If the MPEG does not work for your file, be sure to try DVD Video, since it can handle specialized audio types." + Environment.NewLine;
            this.tbOutput.Text += "- Bink Audio files do not always playback correctly with RAD Video Tools (binkplay.exe), but will convert to WAV correctly using RAD Video Tools 'Convert a file' (binkconv.exe)." + Environment.NewLine;
            this.tbOutput.Text += "- ASF/WMV demultiplexing does not yet rebuild to ASF streams for output." + Environment.NewLine;
            this.tbOutput.Text += "- MKVMerge can be used to add raw .264 data to a container file for playback." + Environment.NewLine;
            this.tbOutput.Text += "- The following audio output formats are unknown and untestable: MO (A3 type)." + Environment.NewLine;
            this.tbOutput.Text += "- The following video output formats are unknown and untestable: MO, XMV." + Environment.NewLine;


            this.initializeFormatList();
        }

        private void initializeFormatList()
        {
            this.comboFormat.Items.Clear();
            this.comboFormat.Items.Add("ASF (MS Advanced Systems Format)");
            this.comboFormat.Items.Add("BIK (Bink Video Container)");
            this.comboFormat.Items.Add("DSI (Racjin/Racdym PS2 Video)");
            this.comboFormat.Items.Add("DVD Video (VOB)");
            this.comboFormat.Items.Add("Electronic Arts VP6 (VP6)");
            this.comboFormat.Items.Add("Electronic Arts MPC (MPC)");
            //this.comboFormat.Items.Add("H4M (Hudson GameCube Video)");
            this.comboFormat.Items.Add("MO (Mobiclip)");
            this.comboFormat.Items.Add("MPEG");
            this.comboFormat.Items.Add("MPS (PSP UMD Movie)");
            this.comboFormat.Items.Add("PAM (PlayStation Advanced Movie)");
            this.comboFormat.Items.Add("PMF (PSP Movie Format)");
            this.comboFormat.Items.Add("PSS (PlayStation Stream)");
            this.comboFormat.Items.Add("SFD (CRI Sofdec Video)");
            this.comboFormat.Items.Add("THP");
            this.comboFormat.Items.Add("USM (CRI Movie 2)");
            this.comboFormat.Items.Add("WMV (MS Advanced Systems Format)");
            this.comboFormat.Items.Add("XMV (Xbox Media Video)");

            this.comboFormat.SelectedItem = "ASF (MS Advanced Systems Format)";
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new MpegDemuxWorker();
        }
        protected override string getCancelMessage()
        {
            return "Demultiplexing Streams...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "Demultiplexing Streams...Complete";
        }
        protected override string getBeginMessage()
        {
            return "Demultiplexing Streams...Begin";
        }

        private void MpegDemuxForm_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void MpegDemuxForm_DragDrop(object sender, DragEventArgs e)
        {
            MpegDemuxWorker.MpegDemuxStruct taskStruct = new MpegDemuxWorker.MpegDemuxStruct();

            // paths
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            taskStruct.SourcePaths = s;

            // format
            taskStruct.SourceFormat = this.comboFormat.SelectedItem.ToString();
            
            // options
            taskStruct.AddHeader = this.cbAddHeader.Checked;
            taskStruct.SplitAudioTracks = this.cbSplitAudioTracks.Checked;
            taskStruct.ExtractAudio = (this.rbExtractAudioAndVideo.Checked || 
                                       this.rbExtractAudioOnly.Checked);
            taskStruct.ExtractVideo = (this.rbExtractAudioAndVideo.Checked ||
                                       this.rbExtractVideoOnly.Checked);

            base.backgroundWorker_Execute(taskStruct);
        }

        private void comboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboFormat.SelectedItem.ToString())
            {
                case "BIK (Bink Video Container)":
                    this.cbSplitAudioTracks.Enabled = true;
                    this.cbSplitAudioTracks.Checked = false;
                    this.cbAddHeader.Enabled = true;
                    this.cbAddHeader.Checked = true;
                    break;
                //case "ASF (MS Advanced Systems Format)":
                case "DSI (Racjin/Racdym PS2 Video)":
                case "H4M (Hudson GameCube Video)":
                case "MO (Mobiclip)":
                case "MPS (PSP UMD Movie)":
                case "PAM (PlayStation Advanced Movie)":
                case "PMF (PSP Movie Format)":
                //case "WMV (MS Advanced Systems Format)":
                case "XMV (Xbox Media Video)":
                    this.cbSplitAudioTracks.Enabled = false;
                    this.cbSplitAudioTracks.Checked = false;
                    this.cbAddHeader.Enabled = true;
                    this.cbAddHeader.Checked = true;
                    break;
                default:
                    this.cbSplitAudioTracks.Enabled = false;
                    this.cbSplitAudioTracks.Checked = false;
                    this.cbAddHeader.Checked = false;
                    this.cbAddHeader.Enabled = false;
                    break;
            }
        }
    }
}
