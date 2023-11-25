using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using VGMToolbox.forms.extraction;
using VGMToolbox.plugin;
using VGMToolbox.tools.stream;
using VGMToolbox.util;

namespace VGMToolbox.forms.stream
{
    public partial class XmaConvertForm : AVgmtForm
    {
        private static readonly string PLUGIN_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "plugins"), "XmaConverter");                
        
        public XmaConvertForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = "XMA to WAV Converter";
            
            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            InitializeComponent();

            this.tbOutput.Text = String.Format("- Convert XMA to WAV using external tools{0}", Environment.NewLine);
            this.tbOutput.Text += String.Format("- Preset .xml files should be placed in: <{0}>{1}", PLUGIN_PATH, Environment.NewLine);
            this.tbOutput.Text += String.Format("* Note: This tool requires 'ToWav.exe' by Xplorer, 'xma_test.exe' (xma_parse) by hcs, and (optionally) xmaencode.exe.{0}", Environment.NewLine);
            this.tbOutput.Text += String.Format("  Please download both files and place in the following directory: <{0}>{1}", Path.GetDirectoryName(XmaConverterWorker.TOWAV_FULL_PATH), Environment.NewLine);

            this.initializeXmaParseInputSection();
            this.initializeRiffSection();
            this.initializePosMakerSection();
            this.initializeToWavSection();

            this.loadPresetList();
        }

        //----------------
        // initialization
        //----------------
        private void initializeXmaParseInputSection()
        {
            this.cbDoXmaParse.Checked = true;
            
            // XMA Type
            this.comboXmaParseInputType.Items.Clear();
            this.comboXmaParseInputType.Items.Add("1");
            this.comboXmaParseInputType.Items.Add("2");
            
            this.comboXmaParseInputType.SelectedItem = "2";

            // Defaults
            this.tbXmaParseStartOffset.Text = "0x00";
            this.tbXmaParseBlockSize.Text = "0x8000";
            this.cbXmaParseIgnoreErrors.Checked = true;

            this.tbXmaParseStartOffset.Clear();
            this.XmaParseStartOffsetOffsetDescription.Reset();

            this.tbXmaParseBlockSize.Clear();
            this.XmaParseBlockSizeOffsetDescription.Reset();

            this.tbXmaParseDataSize.Clear();
            this.XmaParseDataSizeOffsetDescription.Reset();

            // radio buttons
            this.rbXmaParseStartOffsetStatic.Checked = true;
            this.rbXmaParseBlockSizeStatic.Checked = true;
            this.rbXmaParseDataSizeStatic.Checked = true;

            this.doXmaParseStartOffsetRadios();
            this.doXmaParseBlockSizeRadios();
            this.doXmaParseDataSizeRadios();
        }

        private void initializeRiffSection()
        {
            this.cbAddRiffHeader.Checked = true;
            this.comboRiffFrequency.Items.Clear();

            // RIFF Frequency
            this.comboRiffFrequency.Items.Add("22050");
            this.comboRiffFrequency.Items.Add("24000");
            this.comboRiffFrequency.Items.Add("32000");
            this.comboRiffFrequency.Items.Add("44100");
            this.comboRiffFrequency.Items.Add("48000");

            this.comboRiffFrequency.SelectedItem = "48000";
            this.rbAddManualFrequency.Checked = true;
            this.rbAddManualChannels.Checked = true;

            // RIFF Channels
            this.comboRiffChannels.Items.Add(XmaConverterWorker.RIFF_CHANNELS_1);
            this.comboRiffChannels.Items.Add(XmaConverterWorker.RIFF_CHANNELS_2);

            this.comboRiffChannels.SelectedItem = XmaConverterWorker.RIFF_CHANNELS_2;
        }

        private void initializePosMakerSection()
        {
            this.cbMakePosFile.Checked = false;
            this.rbLoopStartOffset.Checked = true;
            this.rbLoopEndOffset.Checked = true;
        }

        private void initializeToWavSection()
        {
            this.rbDoToWav.Checked = true;
        }
                  
        //-----------------------------
        // drag and drop functionality
        //-----------------------------
        private void tbOutputFolder_DragEnter(object sender, DragEventArgs e)
        {
            this.doDragEnter(sender, e);
        }

        private void tbOutputFolder_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (Directory.Exists(s[0])))
            {
                this.tbOutputFolder.Text = s[0];
            }
        }        

        private void XmaConvertForm_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        private void XmaConvertForm_DragDrop(object sender, DragEventArgs e)
        {            
            // validate inputs and check for prerequisites
            if (this.validateAll())
            {                
                XmaConverterWorker.XmaConverterStruct taskStruct = new XmaConverterWorker.XmaConverterStruct();
                
                // paths
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                taskStruct.SourcePaths = s;

                // OUTPUT
                if (!String.IsNullOrEmpty(this.tbOutputFolder.Text))
                {
                    taskStruct.OutputFolder = this.tbOutputFolder.Text.Trim();
                }

                // XMA PARSE
                taskStruct.DoXmaParse = this.cbDoXmaParse.Checked;
                taskStruct.XmaParseDoRebuildMode = this.cbXmaParseDoRebuild.Checked;
                taskStruct.XmaParseIgnoreErrors = this.cbXmaParseIgnoreErrors.Checked;
                taskStruct.XmaParseXmaType = this.comboXmaParseInputType.Text;

                taskStruct.XmaParseStartOffsetIsStatic = this.rbXmaParseStartOffsetStatic.Checked;
                taskStruct.XmaParseStartOffset = this.tbXmaParseStartOffset.Text;
                taskStruct.XmaParseStartOffsetOffsetInfo = this.XmaParseStartOffsetOffsetDescription.GetOffsetValues();
                taskStruct.StartOffsetAfterRiffHeader = this.rbStartOffsetIsAfterRiff.Checked;

                taskStruct.XmaParseBlockSizeIsStatic = this.rbXmaParseBlockSizeStatic.Checked;
                taskStruct.XmaParseBlockSize = this.tbXmaParseBlockSize.Text;
                taskStruct.XmaParseBlockSizeOffsetInfo = this.XmaParseBlockSizeOffsetDescription.GetOffsetValues();

                taskStruct.XmaParseDataSizeIsStatic = this.rbXmaParseDataSizeStatic.Checked;
                taskStruct.XmaParseDataSize = this.tbXmaParseDataSize.Text;
                taskStruct.XmaParseDataSizeOffsetInfo = this.XmaParseDataSizeOffsetDescription.GetOffsetValues();
                taskStruct.GetDataSizeFromRiffHeader = this.rbGetDataSizeFromRiff.Checked;

                // RIFF
                taskStruct.DoRiffHeader = this.cbAddRiffHeader.Checked;
                
                taskStruct.RiffFrequency = this.comboRiffFrequency.Text;
                taskStruct.GetFrequencyFromRiffHeader = this.rbGetFrequencyFromRiff.Checked;
                taskStruct.GetFrequencyFromOffset = this.rbFrequencyOffset.Checked;
                taskStruct.RiffHeaderFrequencyOffsetInfo = this.frequencyOffsetDescription.GetOffsetValues();

                taskStruct.GetChannelsFromRiffHeader = this.rbGetChannelsFromRiff.Checked;
                taskStruct.GetChannelsFromOffset = this.rbNumberOfChannelsOffset.Checked;
                taskStruct.RiffHeaderChannelOffsetInfo = this.numberOfChannelsOffsetDescription.GetOffsetValues();

                switch (this.comboRiffChannels.Text)
                {
                    case XmaConverterWorker.RIFF_CHANNELS_1:
                        taskStruct.RiffChannelCount = "1";
                        break;
                    case XmaConverterWorker.RIFF_CHANNELS_2:
                        taskStruct.RiffChannelCount = "2";
                        break;
                    default:
                        taskStruct.RiffChannelCount = "2";
                        break;
                }

                // POS MAKER
                taskStruct.DoPosMaker = this.cbMakePosFile.Checked;
            
                taskStruct.PosLoopStartIsStatic = this.rbLoopStartStatic.Checked;
                taskStruct.PosLoopStartStaticValue = this.tbLoopStartStatic.Text;
                taskStruct.PosLoopStartOffsetInfo = this.loopStartValueOffsetDescription.GetOffsetValues();

                taskStruct.PosLoopEndIsStatic = this.rbLoopEndStatic.Checked;
                taskStruct.PosLoopEndStaticValue = this.tbLoopEndStatic.Text;
                taskStruct.PosLoopEndOffsetInfo = this.loopEndValueOffsetDescription.GetOffsetValues();

                // TOWAV
                taskStruct.DoToWav = this.rbDoToWav.Checked;

                // XMAENCODE
                taskStruct.DoXmaEncode = this.rbDoXmaEncode.Checked;

                // OTHER                
                taskStruct.ShowExeOutput = this.cbShowAllExeOutput.Checked;
                taskStruct.KeepIntermediateFiles = this.cbKeepTempFiles.Checked;

                base.backgroundWorker_Execute(taskStruct);
            }
        }

        //------------
        // validation
        //------------
        private bool validateAll()
        {
            bool isValid = true;

            isValid &= this.checkPrerequisites();
            isValid &= this.validateOutputOptions();
            isValid &= this.validateXmaParseOptions();
            isValid &= this.validateRiffOptions();
            isValid &= this.validatePosMakerOptions();

            return isValid;
        }

        private bool checkPrerequisites()
        {
            bool isValid = true;
            string errorMessage;

            if (rbDoToWav.Checked && !File.Exists(XmaConverterWorker.TOWAV_FULL_PATH))
            {
                errorMessage = 
                    String.Format(
                        "Cannot find \"{0}\".  \"{0}\" is not included and should be placed in the following directory: <{1}>",
                        Path.GetFileName(XmaConverterWorker.TOWAV_FULL_PATH),
                        Path.GetDirectoryName(XmaConverterWorker.TOWAV_FULL_PATH));

                MessageBox.Show(errorMessage, "Missing File");

                isValid = false;
            }

            if (cbDoXmaParse.Checked && !File.Exists(XmaConverterWorker.XMAPARSE_FULL_PATH))
            {
                errorMessage =
                    String.Format(
                        "Cannot find \"{0}\".  \"{0}\" is not included and should be placed in the following directory: <{1}>",
                        Path.GetFileName(XmaConverterWorker.XMAPARSE_FULL_PATH),
                        Path.GetDirectoryName(XmaConverterWorker.XMAPARSE_FULL_PATH));

                MessageBox.Show(errorMessage, "Missing File");

                isValid = false;
            }

            if (rbDoXmaEncode.Checked && !File.Exists(XmaConverterWorker.XMAENCODE_FULL_PATH))
            {
                errorMessage =
                    String.Format(
                        "Cannot find \"{0}\".  \"{0}\" is not included and should be placed in the following directory: <{1}>",
                        Path.GetFileName(XmaConverterWorker.XMAENCODE_FULL_PATH),
                        Path.GetDirectoryName(XmaConverterWorker.XMAENCODE_FULL_PATH));

                MessageBox.Show(errorMessage, "Missing File");

                isValid = false;
            }
            
            return isValid;
        }

        private bool validateOutputOptions()
        {
            bool isValid = true;

            if (!String.IsNullOrEmpty(this.tbOutputFolder.Text))
            {
                isValid &= AVgmtForm.checkFolderExists(this.tbOutputFolder.Text, this.lblOutputFolder.Text);
            }

            return isValid;
        }

        private bool validateXmaParseOptions()
        {
            bool isValid = true;

            isValid &= AVgmtForm.checkTextBox(this.comboXmaParseInputType.Text, this.lblXmaParseInputType.Text);

            if (this.rbXmaParseStartOffsetStatic.Checked)
            {
                isValid &= AVgmtForm.checkIfTextIsParsableAsLong(this.tbXmaParseStartOffset.Text, this.rbXmaParseStartOffsetStatic.Text);
            }
            
            if (this.rbXmaParseBlockSizeStatic.Checked)
            {
                isValid &= AVgmtForm.checkIfTextIsParsableAsLong(this.tbXmaParseBlockSize.Text, this.rbXmaParseBlockSizeStatic.Text);
            }

            if (this.rbXmaParseDataSizeStatic.Checked)
            {
                isValid &= AVgmtForm.checkIfTextIsParsableAsLong(this.tbXmaParseDataSize.Text, this.rbXmaParseDataSizeStatic.Text);
            }

            if (this.rbXmaParseStartOffsetOffset.Checked)
            {
                isValid &= AVgmtForm.checkTextBox(this.XmaParseStartOffsetOffsetDescription.OffsetValue, this.rbXmaParseStartOffsetOffset.Text);
                isValid &= AVgmtForm.checkIfTextIsParsableAsLong(this.XmaParseStartOffsetOffsetDescription.OffsetValue, this.rbXmaParseStartOffsetOffset.Text);
            }

            if (this.rbXmaParseBlockSizeOffset.Checked)
            {
                isValid &= AVgmtForm.checkTextBox(this.XmaParseBlockSizeOffsetDescription.OffsetValue, this.rbXmaParseBlockSizeOffset.Text);
                isValid &= AVgmtForm.checkIfTextIsParsableAsLong(this.XmaParseBlockSizeOffsetDescription.OffsetValue, this.rbXmaParseBlockSizeOffset.Text);
            }

            if (this.rbXmaParseDataSizeOffset.Checked)
            {
                isValid &= AVgmtForm.checkTextBox(this.XmaParseDataSizeOffsetDescription.OffsetValue, this.rbXmaParseDataSizeOffset.Text);
                isValid &= AVgmtForm.checkIfTextIsParsableAsLong(this.XmaParseDataSizeOffsetDescription.OffsetValue, this.rbXmaParseDataSizeOffset.Text);
            }

            return isValid;
        }

        private bool validateRiffOptions()
        {
            bool isValid = true;

            if (this.cbAddRiffHeader.Checked)
            {
                // Frequency
                if (this.rbAddManualFrequency.Checked)
                {
                    isValid &= AVgmtForm.checkTextBox(this.comboRiffFrequency.Text, this.rbAddManualFrequency.Text);

                    if (isValid)
                    {
                        isValid &= AVgmtForm.checkIfTextIsParsableAsLong(this.comboRiffFrequency.Text, this.rbAddManualFrequency.Text);
                    }
                }
                else if (this.rbFrequencyOffset.Checked)
                {
                    isValid &= this.frequencyOffsetDescription.Validate();
                }

                // Channels
                if (this.rbNumberOfChannelsOffset.Checked)
                {
                    isValid &= this.numberOfChannelsOffsetDescription.Validate();
                }
            }

            return isValid;        
        }

        private bool validatePosMakerOptions()
        {
            bool isValid = true;

            if (this.cbMakePosFile.Checked)
            {
                if (this.rbLoopStartStatic.Checked)
                {
                    isValid &= AVgmtForm.checkTextBox(this.tbLoopStartStatic.Text, this.rbLoopStartStatic.Text);
                }
                if (this.rbLoopEndStatic.Checked)
                {
                    isValid &= AVgmtForm.checkTextBox(this.tbLoopEndStatic.Text, this.rbLoopEndStatic.Text);
                }

                if (this.rbLoopStartOffset.Checked)
                {
                    isValid &= this.loopStartValueOffsetDescription.Validate();
                }
                if (this.rbLoopEndOffset.Checked)
                {
                    isValid &= this.loopEndValueOffsetDescription.Validate();
                }                                
            }

            return isValid; 
        }

        //----------------------------
        // background worker defaults
        //----------------------------
        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new XmaConverterWorker();
        }
        protected override string getCancelMessage()
        {
            return "Converting XMA...Cancelled";
        }
        protected override string getCompleteMessage()
        {
            return "Converting XMA...Complete";
        }
        protected override string getBeginMessage()
        {
            return "Converting XMA...Begin";
        }

        //--------------------
        // combo box handlers
        //--------------------
        private void comboXmaParseInputType_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void comboXmaParseInputType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboRiffChannels_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void comboRiffChannels_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //---------------
        // output folder
        //---------------
        private void btnBrowseOutputFolder_Click(object sender, EventArgs e)
        {
            this.tbOutputFolder.Text = base.browseForFolder(sender, e);
        }

        //--------------------------
        // checkboxes/radio buttons
        //--------------------------
        private void cbDoXmaParse_CheckedChanged(object sender, EventArgs e)
        {
            this.comboXmaParseInputType.Enabled = this.cbDoXmaParse.Checked;
            this.cbXmaParseDoRebuild.Enabled = this.cbDoXmaParse.Checked;
            this.cbXmaParseIgnoreErrors.Enabled = this.cbDoXmaParse.Checked;

            this.rbXmaParseStartOffsetStatic.Enabled = this.cbDoXmaParse.Checked;
            this.rbXmaParseStartOffsetOffset.Enabled = this.cbDoXmaParse.Checked;

            this.rbXmaParseBlockSizeStatic.Enabled = this.cbDoXmaParse.Checked;
            this.rbXmaParseBlockSizeOffset.Enabled = this.cbDoXmaParse.Checked;

            this.rbXmaParseDataSizeStatic.Enabled = this.cbDoXmaParse.Checked;
            this.rbXmaParseDataSizeOffset.Enabled = this.cbDoXmaParse.Checked;

            this.doXmaParseStartOffsetRadios();
            this.doXmaParseBlockSizeRadios();
            this.doXmaParseDataSizeRadios();
        }
        private void cbAddRiffHeader_CheckedChanged(object sender, EventArgs e)
        {
            this.comboRiffFrequency.Enabled = this.cbAddRiffHeader.Checked;
            this.comboRiffChannels.Enabled = this.cbAddRiffHeader.Checked;

            this.rbGetFrequencyFromRiff.Enabled = this.cbAddRiffHeader.Checked;
            this.rbAddManualFrequency.Enabled = this.cbAddRiffHeader.Checked;

            this.rbGetChannelsFromRiff.Enabled = this.cbAddRiffHeader.Checked;
            this.rbAddManualChannels.Enabled = this.cbAddRiffHeader.Checked;
        }

        // xma_parse - start offset
        private void doXmaParseStartOffsetRadios()
        {
            this.tbXmaParseStartOffset.Enabled = this.rbXmaParseStartOffsetStatic.Checked && this.rbXmaParseStartOffsetStatic.Enabled;
            this.XmaParseStartOffsetOffsetDescription.Enabled = this.rbXmaParseStartOffsetOffset.Checked && this.rbXmaParseStartOffsetOffset.Enabled;
            this.rbStartOffsetIsAfterRiff.Enabled = this.cbDoXmaParse.Checked;
        }
        private void rbXmaParseStartOffsetOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doXmaParseStartOffsetRadios();
        }
        private void rbXmaParseStartOffsetStatic_CheckedChanged(object sender, EventArgs e)
        {
            this.doXmaParseStartOffsetRadios();
        }
        private void rbStartOffsetIsAfterRiff_CheckedChanged(object sender, EventArgs e)
        {
            this.doXmaParseStartOffsetRadios();
        }        

        // xma parse - block size
        private void doXmaParseBlockSizeRadios()
        {
            this.tbXmaParseBlockSize.Enabled = this.rbXmaParseBlockSizeStatic.Checked && this.rbXmaParseBlockSizeStatic.Enabled;
            this.XmaParseBlockSizeOffsetDescription.Enabled = this.rbXmaParseBlockSizeOffset.Checked && this.rbXmaParseBlockSizeOffset.Enabled;
        }
        private void rbXmaParseBlockSizeOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doXmaParseBlockSizeRadios();
        }
        private void rbXmaParseBlockSizeStatic_CheckedChanged(object sender, EventArgs e)
        {
            this.doXmaParseBlockSizeRadios();
        }

        // xma parse - data size
        private void doXmaParseDataSizeRadios()
        {
            this.tbXmaParseDataSize.Enabled = this.rbXmaParseDataSizeStatic.Checked && this.rbXmaParseDataSizeStatic.Enabled;
            this.XmaParseDataSizeOffsetDescription.Enabled = this.rbXmaParseDataSizeOffset.Checked && this.rbXmaParseDataSizeOffset.Enabled;
            this.rbGetDataSizeFromRiff.Enabled = this.cbDoXmaParse.Checked;
        }
        private void rbXmaParseDataSizeOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doXmaParseDataSizeRadios();
        }
        private void rbXmaParseDataSizeStatic_CheckedChanged(object sender, EventArgs e)
        {
            this.doXmaParseDataSizeRadios();
        }
        private void rbGetDataSizeFromRiff_CheckedChanged(object sender, EventArgs e)
        {
            this.doXmaParseDataSizeRadios();
        }     

        private void doFrequencyRadios()
        {
            this.comboRiffFrequency.Enabled = this.rbAddManualFrequency.Checked;
            this.frequencyOffsetDescription.Enabled = this.rbFrequencyOffset.Checked;
        }
        private void rbAddManualFrequency_CheckedChanged(object sender, EventArgs e)
        {
            this.doFrequencyRadios();
        }
        private void rbGetFrequencyFromRiff_CheckedChanged(object sender, EventArgs e)
        {
            this.doFrequencyRadios();
        }
        private void rbFrequencyOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doChannelRadios();
        }
        
        private void doChannelRadios()
        {
            this.comboRiffChannels.Enabled = rbAddManualChannels.Checked;
            this.numberOfChannelsOffsetDescription.Enabled = rbNumberOfChannelsOffset.Checked;
        }
        private void rbAddManualChannels_CheckedChanged(object sender, EventArgs e)
        {
            this.doChannelRadios();
        }
        private void rbGetChannelsFromRiff_CheckedChanged(object sender, EventArgs e)
        {
            this.doChannelRadios();
        }
        private void rbNumberOfChannelsOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doChannelRadios();
        }

        // pos maker
        private void cbMakePosFile_CheckedChanged(object sender, EventArgs e)
        {
            this.doPosLoopStartRadios();
            this.doPosLoopEndRadios();
        }

        // pos maker - loop start
        private void doPosLoopStartRadios()
        {
            this.rbLoopStartStatic.Enabled = this.cbMakePosFile.Checked;
            this.tbLoopStartStatic.Enabled = this.rbLoopStartStatic.Checked;
            this.rbLoopStartOffset.Enabled = this.cbMakePosFile.Checked;
            this.loopStartValueOffsetDescription.Enabled = this.rbLoopStartOffset.Enabled && this.rbLoopStartOffset.Checked;
        }
        private void rbLoopStartStatic_CheckedChanged(object sender, EventArgs e)
        {
            this.doPosLoopStartRadios();
        }
        private void rbLoopStartOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doPosLoopStartRadios();
        }

        // pos maker - loop end
        private void doPosLoopEndRadios()
        {
            this.rbLoopEndStatic.Enabled = this.cbMakePosFile.Checked;
            this.tbLoopEndStatic.Enabled = this.rbLoopEndStatic.Checked;
            this.rbLoopEndOffset.Enabled = this.cbMakePosFile.Checked;
            this.loopEndValueOffsetDescription.Enabled = this.rbLoopEndOffset.Enabled && this.rbLoopEndOffset.Checked;
        }
        private void rbLoopEndStatic_CheckedChanged(object sender, EventArgs e)
        {
            this.doPosLoopEndRadios();
        }
        private void rbLoopEndOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doPosLoopEndRadios();
        }

        //--------------------
        // Preset Processing
        //--------------------
        private XmaConverterSettings getPresetFromFile(string filePath)
        {
            XmaConverterSettings preset = null;

            preset = new XmaConverterSettings();
            XmlSerializer serializer = new XmlSerializer(preset.GetType());
            using (FileStream xmlFs = File.OpenRead(filePath))
            {
                using (XmlTextReader textReader = new XmlTextReader(xmlFs))
                {
                    preset = (XmaConverterSettings)serializer.Deserialize(textReader);
                }
            }

            return preset;
        }

        private string getEndiannessStringForXmlValue(Endianness xmlValue)
        {
            string ret = Constants.LittleEndianByteOrder;

            switch(xmlValue)
            {
                case Endianness.big:
                    ret = Constants.BigEndianByteOrder;
                    break;
                case Endianness.little:
                    ret = Constants.LittleEndianByteOrder;
                    break;
            }

            return ret;
        }
        private Endianness getEndiannessForStringValue(string textValue)
        {
            Endianness ret = Endianness.little;

            switch (textValue)
            {
                case Constants.BigEndianByteOrder:
                    ret = Endianness.big;
                    break;
                case Constants.LittleEndianByteOrder:
                    ret = Endianness.little;
                    break;
            }

            return ret;
        }

        private void loadPresetList()
        {
            this.comboPresets.Items.Clear();
            this.comboPresets.Text = String.Empty;

            foreach (string f in Directory.GetFiles(PLUGIN_PATH, "*.xml", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    XmaConverterSettings preset = getPresetFromFile(f);

                    if ((preset != null) && (!String.IsNullOrEmpty(preset.Header.FormatName)))
                    {
                        comboPresets.Items.Add(preset);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(String.Format("Error loading preset file <{0}>", Path.GetFileName(f)), "Error");
                }
            }

            comboPresets.Sorted = true;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.loadPresetList();
        }

        private void loadXmaConverterSettings(XmaConverterSettings xmaSettings)
        {
            #region XMA PARSE

            //general
            this.cbDoXmaParse.Checked = xmaSettings.UseXmaParse;

            if (xmaSettings.XmaParseParameters.XmaTypeSpecified)
            {
                switch (xmaSettings.XmaParseParameters.XmaType)
                { 
                    case XmaType.Item1:
                        this.comboXmaParseInputType.Text = "1";
                        break;
                    case XmaType.Item2:
                        this.comboXmaParseInputType.Text = "2";
                        break;
                    default:
                        this.comboXmaParseInputType.Text = String.Empty;
                        break;
                }
            }

            this.cbXmaParseDoRebuild.Checked = xmaSettings.XmaParseParameters.RebuildXmaSpecified && xmaSettings.XmaParseParameters.RebuildXma;
            this.cbXmaParseIgnoreErrors.Checked = xmaSettings.XmaParseParameters.IgnoreXmaParseErrorsSpecified && xmaSettings.XmaParseParameters.IgnoreXmaParseErrors;

            // start offset
            this.rbXmaParseStartOffsetStatic.Checked = xmaSettings.XmaParseParameters.UseStaticStartOffset;
            this.rbXmaParseStartOffsetOffset.Checked = xmaSettings.XmaParseParameters.UseDynamicStartOffset;
            this.rbStartOffsetIsAfterRiff.Checked = xmaSettings.XmaParseParameters.SetStartOffsetAfterRiffHeader;

            this.tbXmaParseStartOffset.Text = xmaSettings.XmaParseParameters.StartOffsetStatic;

            if (xmaSettings.XmaParseParameters.UseDynamicStartOffset)
            {
                this.XmaParseStartOffsetOffsetDescription.OffsetValue = xmaSettings.XmaParseParameters.StartOffsetOffset;
                this.XmaParseStartOffsetOffsetDescription.OffsetSize = xmaSettings.XmaParseParameters.StartOffsetOffsetSize;
                this.XmaParseStartOffsetOffsetDescription.OffsetByteOrder = getEndiannessStringForXmlValue(xmaSettings.XmaParseParameters.StartOffsetOffsetEndianess);
            }

            // block size
            this.rbXmaParseBlockSizeStatic.Checked = xmaSettings.XmaParseParameters.UseStaticBlockSize;
            this.rbXmaParseBlockSizeOffset.Checked = xmaSettings.XmaParseParameters.UseDynamicBlockSize;

            this.tbXmaParseBlockSize.Text = xmaSettings.XmaParseParameters.BlockSizeStatic;

            if (xmaSettings.XmaParseParameters.UseDynamicBlockSize)
            {
                this.XmaParseBlockSizeOffsetDescription.OffsetValue = xmaSettings.XmaParseParameters.BlockSizeOffset;
                this.XmaParseBlockSizeOffsetDescription.OffsetSize = xmaSettings.XmaParseParameters.BlockSizeOffsetSize;
                this.XmaParseBlockSizeOffsetDescription.OffsetByteOrder = getEndiannessStringForXmlValue(xmaSettings.XmaParseParameters.BlockSizeOffsetEndianess);
            }

            // data size                        
            this.rbXmaParseDataSizeStatic.Checked = xmaSettings.XmaParseParameters.UseStaticDataSize;
            this.rbXmaParseDataSizeOffset.Checked = xmaSettings.XmaParseParameters.UseDynamicDataSize;
            this.rbGetDataSizeFromRiff.Checked = xmaSettings.XmaParseParameters.GetDataSizeFromRiffHeader;

            this.tbXmaParseDataSize.Text = xmaSettings.XmaParseParameters.DataSizeStatic;

            if (xmaSettings.XmaParseParameters.UseDynamicDataSize)
            {
                this.XmaParseDataSizeOffsetDescription.OffsetValue = xmaSettings.XmaParseParameters.DataSizeOffset;
                this.XmaParseDataSizeOffsetDescription.OffsetSize = xmaSettings.XmaParseParameters.DataSizeOffsetSize;
                this.XmaParseDataSizeOffsetDescription.OffsetByteOrder = getEndiannessStringForXmlValue(xmaSettings.XmaParseParameters.DataSizeOffsetEndianess);
            }

            #endregion

            #region RIFF HEADER

            this.cbAddRiffHeader.Checked = xmaSettings.AddRiffHeader;

            if (xmaSettings.AddRiffHeader)
            {
                // Frequency
                if (xmaSettings.RiffParameters.UseStaticFrequency)
                {
                    this.rbAddManualFrequency.Checked = true;
                    this.comboRiffFrequency.Text = xmaSettings.RiffParameters.FrequencyStatic;
                }
                else if (xmaSettings.RiffParameters.GetFrequencyFromOffset)
                {
                    this.rbFrequencyOffset.Checked = true;
                    
                    this.frequencyOffsetDescription.OffsetValue = xmaSettings.RiffParameters.FrequencyOffset;
                    this.frequencyOffsetDescription.OffsetSize = xmaSettings.RiffParameters.FrequencyOffsetSize;
                    this.frequencyOffsetDescription.OffsetByteOrder = getEndiannessStringForXmlValue(xmaSettings.RiffParameters.FrequencyOffsetEndianess);
                }

                this.rbGetFrequencyFromRiff.Checked = xmaSettings.RiffParameters.GetFrequencyFromRiffHeader;

                // Channels
                if (xmaSettings.RiffParameters.UseStaticChannels)
                {
                    this.rbAddManualChannels.Checked = true;

                    switch (xmaSettings.RiffParameters.ChannelStatic)
                    {
                        case "mono":
                            this.comboRiffChannels.Text = XmaConverterWorker.RIFF_CHANNELS_1;
                            break;
                        case "stereo":
                            this.comboRiffChannels.Text = XmaConverterWorker.RIFF_CHANNELS_2;
                            break;
                        default:
                            this.comboRiffChannels.Text = XmaConverterWorker.RIFF_CHANNELS_2;
                            break;
                    }
                }
                else if (xmaSettings.RiffParameters.GetChannelsFromOffset)
                {
                    this.rbNumberOfChannelsOffset.Checked = true;

                    this.numberOfChannelsOffsetDescription.OffsetValue = xmaSettings.RiffParameters.ChannelOffset;
                    this.numberOfChannelsOffsetDescription.OffsetSize = xmaSettings.RiffParameters.ChannelOffsetSize;
                    this.numberOfChannelsOffsetDescription.OffsetByteOrder = getEndiannessStringForXmlValue(xmaSettings.RiffParameters.ChannelOffsetEndianess);
                }

                this.rbGetChannelsFromRiff.Checked = xmaSettings.RiffParameters.GetChannelsFromRiffHeader;
            }
            
            #endregion

            #region POS MAKER

            this.cbMakePosFile.Checked = xmaSettings.CreatePosFile;

            // loop start
            this.rbLoopStartStatic.Checked = xmaSettings.PosFileParameters.UseStaticStartOffset;
            this.rbLoopStartOffset.Checked = xmaSettings.PosFileParameters.UseDynamicStartOffset;

            this.tbLoopStartStatic.Text = xmaSettings.PosFileParameters.StartOffsetStatic;

            if (xmaSettings.PosFileParameters.UseDynamicStartOffset)
            {
                this.loopStartValueOffsetDescription.OffsetValue = xmaSettings.PosFileParameters.StartOffsetOffset;
                this.loopStartValueOffsetDescription.OffsetSize = xmaSettings.PosFileParameters.StartOffsetOffsetSize;
                this.loopStartValueOffsetDescription.OffsetByteOrder = getEndiannessStringForXmlValue(xmaSettings.PosFileParameters.StartOffsetOffsetEndianess);
                this.loopStartValueOffsetDescription.CalculationValue = xmaSettings.PosFileParameters.StartOffsetCalculation;
            }

            // loop end
            this.rbLoopEndStatic.Checked = xmaSettings.PosFileParameters.UseStaticEndOffset;
            this.rbLoopEndOffset.Checked = xmaSettings.PosFileParameters.UseDynamicEndOffset;

            this.tbLoopEndStatic.Text = xmaSettings.PosFileParameters.EndOffsetStatic;

            if (xmaSettings.PosFileParameters.UseDynamicEndOffset)
            {
                this.loopEndValueOffsetDescription.OffsetValue = xmaSettings.PosFileParameters.EndOffsetOffset;
                this.loopEndValueOffsetDescription.OffsetSize = xmaSettings.PosFileParameters.EndOffsetOffsetSize;
                this.loopEndValueOffsetDescription.OffsetByteOrder = getEndiannessStringForXmlValue(xmaSettings.PosFileParameters.EndOffsetOffsetEndianess);
                this.loopEndValueOffsetDescription.CalculationValue = xmaSettings.PosFileParameters.EndOffsetCalculation;
            }

            #endregion

            #region WAV CONVERSION

            this.rbDoToWav.Checked = xmaSettings.WavConversionParameters.UseToWav;
            this.rbDoXmaEncode.Checked = xmaSettings.WavConversionParameters.UseXmaEncode;
            
            #endregion
        }        
        private void loadSelectedPreset()
        {
            XmaConverterSettings preset = (XmaConverterSettings)this.comboPresets.SelectedItem;

            if (preset != null)
            {
                this.initializeXmaParseInputSection();        
                this.initializeRiffSection();
                this.initializePosMakerSection();
                this.initializeToWavSection();

                this.loadXmaConverterSettings(preset);

                if (!String.IsNullOrEmpty(preset.NotesOrWarnings))
                {
                    MessageBox.Show(preset.NotesOrWarnings, "Notes/Warnings");
                }
            }
        }
        private void btnLoadPreset_Click(object sender, EventArgs e)
        {
            this.loadSelectedPreset();
        }

        private XmaConverterSettings buildPresetForCurrentValues()
        {
            XmaConverterSettings xmaSettings = new XmaConverterSettings();

            #region XMA PARSE

            //general
            xmaSettings.UseXmaParse = this.cbDoXmaParse.Checked;

            if (!String.IsNullOrEmpty(this.comboXmaParseInputType.Text))
            {
                xmaSettings.XmaParseParameters.XmaTypeSpecified = true;

                switch (this.comboXmaParseInputType.Text)
                {
                    case "1":
                        xmaSettings.XmaParseParameters.XmaType = XmaType.Item1;
                        break;
                    case "2":
                        xmaSettings.XmaParseParameters.XmaType = XmaType.Item2;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                xmaSettings.XmaParseParameters.XmaTypeSpecified = false;
            }

            xmaSettings.XmaParseParameters.RebuildXmaSpecified = true;
            xmaSettings.XmaParseParameters.RebuildXma = this.cbXmaParseDoRebuild.Checked;
            xmaSettings.XmaParseParameters.IgnoreXmaParseErrorsSpecified = true;
            xmaSettings.XmaParseParameters.IgnoreXmaParseErrors = this.cbXmaParseIgnoreErrors.Checked;

            // start offset
            xmaSettings.XmaParseParameters.UseStaticStartOffset = this.rbXmaParseStartOffsetStatic.Checked;
            xmaSettings.XmaParseParameters.UseDynamicStartOffset = this.rbXmaParseStartOffsetOffset.Checked;
            xmaSettings.XmaParseParameters.SetStartOffsetAfterRiffHeader = this.rbStartOffsetIsAfterRiff.Checked;

            xmaSettings.XmaParseParameters.StartOffsetStatic = this.tbXmaParseStartOffset.Text;

            if (this.rbXmaParseStartOffsetOffset.Checked)
            {
                xmaSettings.XmaParseParameters.StartOffsetOffset = this.XmaParseStartOffsetOffsetDescription.OffsetValue;
                xmaSettings.XmaParseParameters.StartOffsetOffsetSize = this.XmaParseStartOffsetOffsetDescription.OffsetSize;
                xmaSettings.XmaParseParameters.StartOffsetOffsetEndianessSpecified = true;
                xmaSettings.XmaParseParameters.StartOffsetOffsetEndianess = getEndiannessForStringValue(this.XmaParseStartOffsetOffsetDescription.OffsetByteOrder);
            }

            // block size
            xmaSettings.XmaParseParameters.UseStaticBlockSize = this.rbXmaParseBlockSizeStatic.Checked;
            xmaSettings.XmaParseParameters.UseDynamicBlockSize = this.rbXmaParseBlockSizeOffset.Checked;

            xmaSettings.XmaParseParameters.BlockSizeStatic = this.tbXmaParseBlockSize.Text;

            if (this.rbXmaParseBlockSizeOffset.Checked)
            {
                xmaSettings.XmaParseParameters.BlockSizeOffset = this.XmaParseBlockSizeOffsetDescription.OffsetValue;
                xmaSettings.XmaParseParameters.BlockSizeOffsetSize = this.XmaParseBlockSizeOffsetDescription.OffsetSize;
                xmaSettings.XmaParseParameters.BlockSizeOffsetEndianessSpecified = true;
                xmaSettings.XmaParseParameters.BlockSizeOffsetEndianess = getEndiannessForStringValue(this.XmaParseBlockSizeOffsetDescription.OffsetByteOrder);
            }

            // data size                        
            xmaSettings.XmaParseParameters.UseStaticDataSize = this.rbXmaParseDataSizeStatic.Checked;
            xmaSettings.XmaParseParameters.UseDynamicDataSize = this.rbXmaParseDataSizeOffset.Checked;
            xmaSettings.XmaParseParameters.GetDataSizeFromRiffHeader = this.rbGetDataSizeFromRiff.Checked;

            xmaSettings.XmaParseParameters.DataSizeStatic = this.tbXmaParseDataSize.Text;

            if (this.rbXmaParseDataSizeOffset.Checked)
            {
                xmaSettings.XmaParseParameters.DataSizeOffset = this.XmaParseDataSizeOffsetDescription.OffsetValue;
                xmaSettings.XmaParseParameters.DataSizeOffsetSize = this.XmaParseDataSizeOffsetDescription.OffsetSize;
                xmaSettings.XmaParseParameters.DataSizeOffsetEndianessSpecified = true;
                xmaSettings.XmaParseParameters.DataSizeOffsetEndianess = getEndiannessForStringValue(this.XmaParseDataSizeOffsetDescription.OffsetByteOrder);
            }

            #endregion

            #region RIFF HEADER

            xmaSettings.AddRiffHeader = this.cbAddRiffHeader.Checked;

            // Frequency
            if (this.rbAddManualFrequency.Checked)
            {
                xmaSettings.RiffParameters.UseStaticFrequency = true;
                xmaSettings.RiffParameters.FrequencyStatic = this.comboRiffFrequency.Text;
            }
            else if (this.rbFrequencyOffset.Checked)
            {
                xmaSettings.RiffParameters.GetFrequencyFromOffset = true;
                xmaSettings.RiffParameters.FrequencyOffset = this.frequencyOffsetDescription.OffsetValue;
                xmaSettings.RiffParameters.FrequencyOffsetSize = this.frequencyOffsetDescription.OffsetSize;
                xmaSettings.RiffParameters.FrequencyOffsetEndianessSpecified = true;
                xmaSettings.RiffParameters.FrequencyOffsetEndianess = getEndiannessForStringValue(this.frequencyOffsetDescription.OffsetByteOrder);
            }

            xmaSettings.RiffParameters.GetFrequencyFromRiffHeader = this.rbGetFrequencyFromRiff.Checked;

            // Channels
            if (this.rbAddManualChannels.Checked)
            {
                xmaSettings.RiffParameters.UseStaticChannels = true;

                switch (this.comboRiffChannels.Text)
                {
                    case XmaConverterWorker.RIFF_CHANNELS_1:
                        xmaSettings.RiffParameters.ChannelStatic = "mono";
                        break;
                    case XmaConverterWorker.RIFF_CHANNELS_2:
                        xmaSettings.RiffParameters.ChannelStatic = "stereo";
                        break;
                    default:
                        xmaSettings.RiffParameters.ChannelStatic = "ERROR";
                        break;
                }
            }
            else if (this.rbNumberOfChannelsOffset.Checked)
            {                
                xmaSettings.RiffParameters.GetChannelsFromOffset = true;
                xmaSettings.RiffParameters.ChannelOffset = this.numberOfChannelsOffsetDescription.OffsetValue;
                xmaSettings.RiffParameters.ChannelOffsetSize = this.numberOfChannelsOffsetDescription.OffsetSize;
                xmaSettings.RiffParameters.ChannelOffsetEndianessSpecified = true;
                xmaSettings.RiffParameters.ChannelOffsetEndianess = getEndiannessForStringValue(this.numberOfChannelsOffsetDescription.OffsetByteOrder);
            }

            xmaSettings.RiffParameters.GetChannelsFromRiffHeader = this.rbGetChannelsFromRiff.Checked;

            #endregion

            #region POS MAKER

            xmaSettings.CreatePosFile = this.cbMakePosFile.Checked;

            // loop start
            xmaSettings.PosFileParameters.UseStaticStartOffset = this.rbLoopStartStatic.Checked;
            xmaSettings.PosFileParameters.UseDynamicStartOffset = this.rbLoopStartOffset.Checked;

            xmaSettings.PosFileParameters.StartOffsetStatic = this.tbLoopStartStatic.Text;

            if (this.rbLoopStartOffset.Checked)
            {
                xmaSettings.PosFileParameters.StartOffsetOffset = this.loopStartValueOffsetDescription.OffsetValue;
                xmaSettings.PosFileParameters.StartOffsetOffsetSize = this.loopStartValueOffsetDescription.OffsetSize;
                xmaSettings.PosFileParameters.StartOffsetOffsetEndianessSpecified = true;
                xmaSettings.PosFileParameters.StartOffsetOffsetEndianess = getEndiannessForStringValue(this.loopStartValueOffsetDescription.OffsetByteOrder);
                xmaSettings.PosFileParameters.StartOffsetCalculation = this.loopStartValueOffsetDescription.CalculationValue;
            }

            // loop end
            xmaSettings.PosFileParameters.UseStaticEndOffset = this.rbLoopEndStatic.Checked;
            xmaSettings.PosFileParameters.UseDynamicEndOffset = this.rbLoopEndOffset.Checked;

            xmaSettings.PosFileParameters.EndOffsetStatic = this.tbLoopEndStatic.Text;

            if (this.rbLoopEndOffset.Checked)
            {
                xmaSettings.PosFileParameters.EndOffsetOffset = this.loopEndValueOffsetDescription.OffsetValue;
                xmaSettings.PosFileParameters.EndOffsetOffsetSize = this.loopEndValueOffsetDescription.OffsetSize;
                xmaSettings.PosFileParameters.EndOffsetOffsetEndianessSpecified = true;
                xmaSettings.PosFileParameters.EndOffsetOffsetEndianess = getEndiannessForStringValue(this.loopEndValueOffsetDescription.OffsetByteOrder);
                xmaSettings.PosFileParameters.EndOffsetCalculation = this.loopEndValueOffsetDescription.CalculationValue;
            }

            #endregion

            #region WAV CONVERSION

            xmaSettings.WavConversionParameters.UseToWav = this.rbDoToWav.Checked;
            xmaSettings.WavConversionParameters.UseXmaEncode = this.rbDoXmaEncode.Checked;

            #endregion

            return xmaSettings;
        }
        private void btnSavePreset_Click(object sender, EventArgs e)
        {
            XmaConverterSettings preset = buildPresetForCurrentValues();

            if (preset != null)
            {
                SavePresetForm saveForm = new SavePresetForm(preset);
                saveForm.Show();
            }
        }
    }
}
