using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using VGMToolbox.plugin;
using VGMToolbox.tools;
using VGMToolbox.tools.extract;
using VGMToolbox.util;

namespace VGMToolbox.forms.extraction
{
    public partial class VfsExtractorForm : AVgmtForm
    {
        private static readonly string PLUGIN_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "plugins"), "VFSExtractor");                
        
        public VfsExtractorForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            InitializeComponent();

            this.btnDoTask.Text = "Extract Files";
            this.lblTitle.Text = "Virtual File System (VFS) Extractor";
            this.tbOutput.Text =  "- Extract files from simple VFS archives." + System.Environment.NewLine;
            this.tbOutput.Text += "- Preset .xml files should be placed in <" + PLUGIN_PATH + ">" + System.Environment.NewLine;
            this.tbOutput.Text += "1) Enter settings." + System.Environment.NewLine;
            this.tbOutput.Text += "2) Select data file and header file and click 'Extract Files'" + System.Environment.NewLine;
            this.tbOutput.Text += "          OR" + System.Environment.NewLine;
            this.tbOutput.Text += "Drag and Drop data files onto the application (Header file not supported for Drag and Drop)." + System.Environment.NewLine;

            // header file
            this.doUserHeaderFileCheckbox();

            // file count
            this.rbHeaderSizeValue.Checked = true;

            // offset
            this.rbUseVfsFileOffset.Checked = true;
            this.createFileRecordSizeList();

            this.doFileRecordOffsetRadioButtons();

            // length
            this.rbUseVfsFileLength.Checked = true;
            this.doFileRecordLengthRadioButtons();

            // name
            this.doFileRecordNameCheckbox();
            this.createFileNameRelativeOffsetLocationList();
            
            // presets
            this.loadPresetList();
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new VfsExtractorWorker();
        }
        protected override string getCancelMessage()
        {
            return "Extracting files using VFS...Cancelled.";
        }
        protected override string getCompleteMessage()
        {
            return "Extracting files using VFS...Complete.";
        }
        protected override string getBeginMessage()
        {
            return "Extracting files using VFS...Begin.";
        }

        private void VfsExtractorForm_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void VfsExtractorForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);            
            this.extractFiles(s, false);            
        }

        private void extractFiles(string[] inputPaths, bool includeHeaderPath)
        {
            if (this.validateInputs())
            {
                VfsExtractorWorker.VfsExtractorStruct bgStruct = new VfsExtractorWorker.VfsExtractorStruct();
                VfsExtractionStruct vfsExtractionInformation = new VfsExtractionStruct();

                bgStruct.SourcePaths = inputPaths;

                if (includeHeaderPath)
                {
                    bgStruct.HeaderSourcePath = this.tbHeaderFilePath.Text;
                }

                bgStruct.OutputFolderPath = this.tbOutputFolder.Text;
                bgStruct.OutputLogFiles = this.cbOutputLogFiles.Checked;

                // header size
                vfsExtractionInformation.UseStaticHeaderSize = this.rbHeaderSizeValue.Checked;
                vfsExtractionInformation.StaticHeaderSize = this.tbHeaderSizeValue.Text;
                vfsExtractionInformation.UseHeaderSizeOffset = this.rbHeaderSizeOffset.Checked;                        
                vfsExtractionInformation.HeaderSizeOffsetDescription = this.headerSizeOffsetDescription.GetOffsetValues();
                vfsExtractionInformation.ReadHeaderToEof = this.rbReadHeaderToEof.Checked;

                // file count
                vfsExtractionInformation.UseStaticFileCount = this.rbStaticFileCount.Checked;                
                vfsExtractionInformation.StaticFileCount = this.tbStaticFileCount.Text;
                vfsExtractionInformation.UseFileCountOffset = this.rbFileCountOffset.Checked;
                vfsExtractionInformation.FileCountOffsetDescription = this.fileCountOffsetDescription.GetOffsetValues();

                // file record basic information
                vfsExtractionInformation.FileRecordsStartOffset = this.tbFileRecordsBeginOffset.Text;
                vfsExtractionInformation.FileRecordSize = this.comboFileRecordSize.Text;
                
                // file offset
                vfsExtractionInformation.UseFileOffsetOffset = this.rbUseVfsFileOffset.Checked;
                vfsExtractionInformation.FileOffsetOffsetDescription = this.fileOffsetOffsetDescription.GetOffsetValues();

                vfsExtractionInformation.UsePreviousFilesSizeToDetermineOffset = this.rbUseFileSizeToDetermineOffset.Checked;
                vfsExtractionInformation.BeginCuttingFilesAtOffset = this.tbUseFileLengthBeginOffset.Text;
                vfsExtractionInformation.UseByteAlignmentValue = this.cbDoOffsetByteAlginment.Checked;
                vfsExtractionInformation.ByteAlignmentValue = this.tbByteAlignement.Text;

                // file length/size
                vfsExtractionInformation.UseFileLengthOffset = this.rbUseVfsFileOffset.Checked;
                vfsExtractionInformation.FileLengthOffsetDescription = this.fileLengthOffsetDescription.GetOffsetValues();

                vfsExtractionInformation.UseLocationOfNextFileToDetermineLength = this.rbUseOffsetsToDetermineLength.Checked;
                
                // file name
                vfsExtractionInformation.FileNameIsPresent = this.cbFileNameIsPresent.Checked;

                vfsExtractionInformation.UseStaticFileNameOffsetWithinRecord = this.rbFileRecordFileNameInRecord.Checked;
                vfsExtractionInformation.StaticFileNameOffsetWithinRecord = this.tbFileRecordNameOffset.Text;
                
                vfsExtractionInformation.UseAbsoluteFileNameOffset  = this.rbFileNameAbsoluteOffset.Checked;
                vfsExtractionInformation.AbsoluteFileNameOffsetDescription = this.fileNameAbsoluteOffsetOffsetDescription.GetOffsetValues();

                vfsExtractionInformation.UseRelativeFileNameOffset = this.rbFileNameRelativeOffset.Checked;
                vfsExtractionInformation.RelativeFileNameOffsetDescription = this.fileNameRelativeOffsetOffsetDescription.GetOffsetValues();
        
                switch (this.comboFileRecordNameRelativeLocation.Text)
                {
                    case VfsExtractorWorker.RELATIVE_TO_START_OF_FILE_RECORD:
                        vfsExtractionInformation.FileRecordNameRelativeOffsetLocation = VfsFileRecordRelativeOffsetLocationType.FileRecordStart;
                        break;
                    case VfsExtractorWorker.RELATIVE_TO_END_OF_FILE_RECORD:
                        vfsExtractionInformation.FileRecordNameRelativeOffsetLocation = VfsFileRecordRelativeOffsetLocationType.FileRecordEnd;
                        break;                    
                }                

                // name size
                vfsExtractionInformation.UseStaticFileNameLength = this.rbFileRecordNameSize.Checked;                        
                vfsExtractionInformation.StaticFileNameLength = this.tbFileRecordNameSize.Text;

                vfsExtractionInformation.UseFileNameTerminatorString = this.rbFileRecordNameTerminator.Checked;
                vfsExtractionInformation.FileNameTerminatorString = this.tbFileRecordNameTerminatorBytes.Text;
                              
                bgStruct.VfsExtractionInformation = vfsExtractionInformation;

                base.backgroundWorker_Execute(bgStruct);
            }        
        }

        // Validate Inputs
        private bool validateInputs()
        {
            bool isValid = true;

            isValid &= this.validateFileCountSection();
            
            // add File Record Validation
            
            isValid &= this.validateFileNameSection();

            return isValid;
        }

        //////////////
        // File Count
        //////////////        
        private void doFileCountRadioButtons()
        {
            if (this.rbHeaderSizeOffset.Checked)
            {
                this.headerSizeOffsetDescription.Enabled = true;

                this.tbStaticFileCount.Clear();
                this.tbStaticFileCount.Enabled = false;
                this.tbStaticFileCount.ReadOnly = true;

                this.fileCountOffsetDescription.Enabled = false;

                this.tbHeaderSizeValue.Clear();
                this.tbHeaderSizeValue.Enabled = false;
                this.tbHeaderSizeValue.ReadOnly = true;
            }            
            else if (this.rbStaticFileCount.Checked)
            {
                this.headerSizeOffsetDescription.Enabled = false;
                
                this.tbStaticFileCount.Enabled = true;
                this.tbStaticFileCount.ReadOnly = false;

                this.fileCountOffsetDescription.Enabled = false;

                this.tbHeaderSizeValue.Clear();
                this.tbHeaderSizeValue.Enabled = false;
                this.tbHeaderSizeValue.ReadOnly = true;
            }
            else if (this.rbFileCountOffset.Checked)
            {
                this.headerSizeOffsetDescription.Enabled = false;
                
                this.tbStaticFileCount.Clear();
                this.tbStaticFileCount.Enabled = false;
                this.tbStaticFileCount.ReadOnly = true;

                this.fileCountOffsetDescription.Enabled = true;

                this.tbHeaderSizeValue.Clear();
                this.tbHeaderSizeValue.Enabled = false;
                this.tbHeaderSizeValue.ReadOnly = true;
            }
            else if (this.rbHeaderSizeValue.Checked)
            {
                this.headerSizeOffsetDescription.Enabled = false;
                
                this.tbStaticFileCount.Clear();
                this.tbStaticFileCount.Enabled = false;
                this.tbStaticFileCount.ReadOnly = true;

                this.fileCountOffsetDescription.Enabled = false;

                this.tbHeaderSizeValue.Enabled = true;
                this.tbHeaderSizeValue.ReadOnly = false;            
            }
            else if (this.rbReadHeaderToEof.Checked)
            {
                this.headerSizeOffsetDescription.Enabled = false;

                this.tbStaticFileCount.Clear();
                this.tbStaticFileCount.Enabled = false;
                this.tbStaticFileCount.ReadOnly = true;

                this.fileCountOffsetDescription.Enabled = false;

                this.tbHeaderSizeValue.Clear();
                this.tbHeaderSizeValue.Enabled = false;
                this.tbHeaderSizeValue.ReadOnly = true;
            }
        }
        private void rbUserEnteredFileCount_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileCountRadioButtons();
        }
        private void rbOffsetBasedFileCount_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileCountRadioButtons();
        }
        private void rbFileCountEndOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileCountRadioButtons();
        }
        private void rbReadHeaderToEof_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileCountRadioButtons();
        }

        private bool validateFileCountSection()
        {
            bool isValid = true;

            if (this.rbHeaderSizeOffset.Checked)
            {
                isValid &= this.headerSizeOffsetDescription.IsValid("Header Size Offset");
            }
            else if (this.rbStaticFileCount.Checked)
            {
                isValid &= AVgmtForm.checkTextBox(this.tbStaticFileCount.Text, this.rbStaticFileCount.Text);               
            }
            else if (this.rbFileCountOffset.Checked)
            {
                isValid &= this.fileCountOffsetDescription.IsValid("File Count Offset");
            }
            else if (this.rbHeaderSizeValue.Checked)
            {
                isValid &= AVgmtForm.checkTextBox(this.tbHeaderSizeValue.Text, this.rbHeaderSizeValue.Text);
            }

            return isValid;
        }
        
        private bool validateFileNameSection()
        {
            bool isValid = true;

            if (this.cbFileNameIsPresent.Checked)
            {
                if (!this.rbFileRecordFileNameInRecord.Checked &&
                    !this.rbFileNameAbsoluteOffset.Checked &&
                    !this.rbFileNameRelativeOffset.Checked)
                {
                    MessageBox.Show("Please choose a File Name Location/Offset type.", "Required Field Missing.");
                    isValid = false;
                }
                else if (!this.rbFileRecordNameSize.Checked &&
                         !this.rbFileRecordNameTerminator.Checked)
                {
                    MessageBox.Show("Please choose a File Name Size type.", "Required Field Missing.");
                    isValid = false;
                }
                else
                { 
                    // File Name Location/Offset
                    if (this.rbFileRecordFileNameInRecord.Checked)
                    {
                        isValid &= AVgmtForm.checkTextBox(this.tbFileRecordNameOffset.Text, this.rbFileRecordFileNameInRecord.Text);
                    }
                    else if (this.rbFileNameAbsoluteOffset.Checked)
                    {
                        isValid &= this.fileNameAbsoluteOffsetOffsetDescription.IsValid("File Name Absolute Offset Information");
                    }
                    else if (this.rbFileNameRelativeOffset.Checked)
                    {
                        isValid &= this.fileNameRelativeOffsetOffsetDescription.IsValid("File Name Relative Offset Information");
                        isValid &= AVgmtForm.checkTextBox(this.comboFileRecordNameRelativeLocation.Text, this.lblFileNameRecordRelativeLocation.Text);
                    }

                    // File Name Size
                    if (this.rbFileRecordNameSize.Checked)
                    {
                        isValid &= AVgmtForm.checkTextBox(this.tbFileRecordNameSize.Text, this.rbFileRecordNameSize.Text);
                    }
                    else if (this.rbFileRecordNameTerminator.Checked)
                    {
                        isValid &= AVgmtForm.checkTextBox(this.tbFileRecordNameTerminatorBytes.Text, this.rbFileRecordNameTerminator.Text);
                    }
                }
            }

            return isValid;
        }

        ///////////////
        // File Record
        ///////////////
        private void createFileRecordSizeList()
        {
            this.comboFileRecordSize.Items.Add("0x02");
            this.comboFileRecordSize.Items.Add("0x04");
            this.comboFileRecordSize.Items.Add("0x08");
            this.comboFileRecordSize.Items.Add("0x0C");
            this.comboFileRecordSize.Items.Add("0x10");
            this.comboFileRecordSize.Items.Add("0x14");
            this.comboFileRecordSize.Items.Add("0x18");
            this.comboFileRecordSize.Items.Add("0x1C");
            this.comboFileRecordSize.Items.Add("0x20");
        }

        private void doFileRecordOffsetRadioButtons()
        {
            if (this.rbUseVfsFileOffset.Checked)
            {
                this.fileOffsetOffsetDescription.Enabled = true;
                             
                this.tbUseFileLengthBeginOffset.Clear();
                this.tbUseFileLengthBeginOffset.Enabled = false;
                this.tbUseFileLengthBeginOffset.ReadOnly = true;

                this.cbDoOffsetByteAlginment.Checked = false;
                this.cbDoOffsetByteAlginment.Enabled = false;

                this.tbByteAlignement.Clear();
                this.tbByteAlignement.Enabled = false;
                this.tbByteAlignement.ReadOnly = true;
            }
            else if (this.rbUseFileSizeToDetermineOffset.Checked)
            {
                this.fileOffsetOffsetDescription.Enabled = false;
                
                this.tbUseFileLengthBeginOffset.Enabled = true;
                this.tbUseFileLengthBeginOffset.ReadOnly = false;

                this.cbDoOffsetByteAlginment.Enabled = true;

                this.tbByteAlignement.Enabled = true;
                this.tbByteAlignement.ReadOnly = false;
            }

            this.doCbDoOffsetByteAlignment();
        }
        private void rbUseVfsFileOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileRecordOffsetRadioButtons();
        }
        private void rbUseFileSizeToDetermineOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileRecordOffsetRadioButtons();
        }
        
        private void doFileRecordLengthRadioButtons()
        {
            if (this.rbUseVfsFileLength.Checked)
            {
                this.fileLengthOffsetDescription.Enabled = true;                
            }
            else if (this.rbUseOffsetsToDetermineLength.Checked)
            {
                this.fileLengthOffsetDescription.Enabled = false;
            }
        }

        private void rbUseVfsFileLength_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileRecordLengthRadioButtons();
        }
        private void rbUseOffsetsToDetermineLength_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileRecordLengthRadioButtons();
        }

        private void createFileNameRelativeOffsetLocationList()
        {
            this.comboFileRecordNameRelativeLocation.Items.Add(VfsExtractorWorker.RELATIVE_TO_START_OF_FILE_RECORD);
            this.comboFileRecordNameRelativeLocation.Items.Add(VfsExtractorWorker.RELATIVE_TO_END_OF_FILE_RECORD);
        }

        private void doFileRecordNameCheckbox()
        {
            if (this.cbFileNameIsPresent.Checked)
            {
                // radio buttons for location/offset
                this.rbFileRecordFileNameInRecord.Enabled = true;
                this.rbFileNameAbsoluteOffset.Enabled = true;
                this.rbFileNameRelativeOffset.Enabled = true;
                
                this.rbFileRecordNameSize.Enabled = true;
                this.rbFileRecordNameTerminator.Enabled = true;

                // name size
                if (this.rbFileRecordNameSize.Checked)
                {
                    this.tbFileRecordNameSize.Enabled = true;
                    this.tbFileRecordNameSize.ReadOnly = false;

                    this.tbFileRecordNameTerminatorBytes.Clear();
                    this.tbFileRecordNameTerminatorBytes.Enabled = false;
                    this.tbFileRecordNameTerminatorBytes.ReadOnly = true;
                    this.lblHexOnly.Enabled = false;
                }
                else if (this.rbFileRecordNameTerminator.Checked)
                {
                    this.tbFileRecordNameSize.Clear();
                    this.tbFileRecordNameSize.Enabled = false;
                    this.tbFileRecordNameSize.ReadOnly = true;

                    this.tbFileRecordNameTerminatorBytes.Enabled = true;
                    this.tbFileRecordNameTerminatorBytes.ReadOnly = false;
                    this.lblHexOnly.Enabled = true;
                }
            }
            else
            {
                // radio buttons for location/offset
                this.rbFileRecordFileNameInRecord.Checked = false;
                this.rbFileRecordFileNameInRecord.Enabled = false;
                this.rbFileNameAbsoluteOffset.Checked = false;
                this.rbFileNameAbsoluteOffset.Enabled = false;
                this.rbFileNameRelativeOffset.Checked = false;
                this.rbFileNameRelativeOffset.Enabled = false;


                // name size
                this.rbFileRecordNameSize.Enabled = false;
                this.tbFileRecordNameSize.Clear();
                this.tbFileRecordNameSize.Enabled = false;
                this.tbFileRecordNameSize.ReadOnly = true;

                this.rbFileRecordNameTerminator.Enabled = false;
                this.tbFileRecordNameTerminatorBytes.Clear();
                this.tbFileRecordNameTerminatorBytes.Enabled = false;
                this.tbFileRecordNameTerminatorBytes.ReadOnly = true;
                this.lblHexOnly.Enabled = false;
            }

            this.doRbFileRecordFileNameInRecord();
            this.doRbFileNameAbsoluteOffset();
            this.doRbFileNameRelativeOffset();
        }
        private void cbFileNameIsPresent_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileRecordNameCheckbox();
        }
        private void rbFileRecordNameSize_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileRecordNameCheckbox();
        }
        private void rbFileRecordNameTerminator_CheckedChanged(object sender, EventArgs e)
        {
            this.doFileRecordNameCheckbox();
        }

        private void doRbFileRecordFileNameInRecord()
        {
            if (this.rbFileRecordFileNameInRecord.Checked)
            {
                this.tbFileRecordNameOffset.Enabled = true;
                this.tbFileRecordNameOffset.ReadOnly = false;
            }
            else
            {
                this.tbFileRecordNameOffset.Enabled = false;
                this.tbFileRecordNameOffset.ReadOnly = true;
            }
        }
        private void rbFileRecordFileNameInRecord_CheckedChanged(object sender, EventArgs e)
        {
            this.doRbFileRecordFileNameInRecord();
        }

        private void doRbFileNameAbsoluteOffset()
        {
            if (this.rbFileNameAbsoluteOffset.Checked)
            {
                this.fileNameAbsoluteOffsetOffsetDescription.Enabled = true;
            }
            else
            {
                this.fileNameAbsoluteOffsetOffsetDescription.Enabled = false;
            }
        }
        private void rbFileNameAbsoluteOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doRbFileNameAbsoluteOffset();
        }

        private void doRbFileNameRelativeOffset()
        {
            if (this.rbFileNameRelativeOffset.Checked)
            {
                this.fileNameRelativeOffsetOffsetDescription.Enabled = true;                
                this.comboFileRecordNameRelativeLocation.Enabled = true;
            }
            else
            {
                this.fileNameRelativeOffsetOffsetDescription.Enabled = false;
                this.comboFileRecordNameRelativeLocation.Enabled = false;
            }
        }
        private void rbFileNameRelativeOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.doRbFileNameRelativeOffset();
        }

        private void doUserHeaderFileCheckbox()
        {
            if (cbUseHeaderFile.Checked)
            {
                this.tbHeaderFilePath.Enabled = true;
                this.tbHeaderFilePath.ReadOnly = false;
                this.btnBrowseHeaderFile.Enabled = true;
            }
            else
            {
                this.tbHeaderFilePath.Clear();
                this.tbHeaderFilePath.Enabled = false;
                this.tbHeaderFilePath.ReadOnly = true;
                this.btnBrowseHeaderFile.Enabled = false;
            }        
        }
        private void cbUseHeaderFile_CheckedChanged(object sender, EventArgs e)
        {
            this.doUserHeaderFileCheckbox();
        }

        private void btnBrowseHeaderFile_Click(object sender, EventArgs e)
        {
            this.tbHeaderFilePath.Text = base.browseForFile(sender, e);
        }
        private void tbHeaderFilePath_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void tbHeaderFilePath_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (File.Exists(s[0])))
            {
                this.tbHeaderFilePath.Text = s[0];
            }
        }

        private void btnBrowseDataFile_Click(object sender, EventArgs e)
        {
            this.tbDataFilePath.Text = base.browseForFile(sender, e);
        }
        private void tbDataFilePath_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void tbDataFilePath_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (File.Exists(s[0])))
            {
                this.tbDataFilePath.Text = s[0];
            }
        }

        private void btnDoTask_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(this.tbDataFilePath.Text))
            {
                string[] dataFiles = new string[] { Path.GetFullPath(this.tbDataFilePath.Text) };
                this.extractFiles(dataFiles, true);
            }
        }

        private VfsExtractorSettings getPresetFromFile(string filePath)
        {
            VfsExtractorSettings preset = null;

            preset = new VfsExtractorSettings();
            XmlSerializer serializer = new XmlSerializer(preset.GetType());
            using (FileStream xmlFs = File.OpenRead(filePath))
            {
                using (XmlTextReader textReader = new XmlTextReader(xmlFs))
                {
                    preset = (VfsExtractorSettings)serializer.Deserialize(textReader);
                }
            }

            return preset;
        }

        private void loadPresetList()
        {
            comboPresets.Items.Clear();

            foreach (string f in Directory.GetFiles(PLUGIN_PATH, "*.xml", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    VfsExtractorSettings preset = getPresetFromFile(f);

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

        private void loadVfsSettings(VfsExtractorSettings vfsSettings)
        {            
            #region HEADER_SETTINGS

            switch (vfsSettings.HeaderParameters.HeaderSizeMethod)
            {
                case HeaderSizeMethod.HeaderSizeValue:
                    this.rbHeaderSizeValue.Checked = true;
                    this.tbHeaderSizeValue.Text = vfsSettings.HeaderParameters.HeaderEndsAtOffset;
                    break;
                
                case HeaderSizeMethod.HeaderSizeOffset:
                    this.rbHeaderSizeOffset.Checked = true;
                    this.headerSizeOffsetDescription.OffsetValue = vfsSettings.HeaderParameters.HeaderSizeOffset;
                    this.headerSizeOffsetDescription.OffsetSize = vfsSettings.HeaderParameters.HeaderSizeOffsetSize;
                    
                    if (vfsSettings.HeaderParameters.HeaderSizeOffsetEndianessSpecified)
                    {
                        switch (vfsSettings.HeaderParameters.HeaderSizeOffsetEndianess)
                        {
                            case Endianness.big:
                                this.headerSizeOffsetDescription.OffsetByteOrder = Constants.BigEndianByteOrder;
                                break;
                            case Endianness.little:
                                this.headerSizeOffsetDescription.OffsetByteOrder = Constants.LittleEndianByteOrder;
                                break;
                        }
                    }
                    break;

                case HeaderSizeMethod.FileCountValue:
                    this.rbStaticFileCount.Checked = true;
                    this.tbStaticFileCount.Text = vfsSettings.HeaderParameters.FileCountValue;
                    break;

                case HeaderSizeMethod.FileCountOffset:
                    this.rbFileCountOffset.Checked = true;
                    this.fileCountOffsetDescription.OffsetValue = vfsSettings.HeaderParameters.FileCountOffset;
                    this.fileCountOffsetDescription.OffsetSize = vfsSettings.HeaderParameters.FileCountOffsetSize;
                    
                    if (vfsSettings.HeaderParameters.FileCountOffsetEndianessSpecified)
                    {
                        switch (vfsSettings.HeaderParameters.FileCountOffsetEndianess)
                        {
                            case Endianness.big:
                                this.fileCountOffsetDescription.OffsetByteOrder = Constants.BigEndianByteOrder;
                                break;
                            case Endianness.little:
                                this.fileCountOffsetDescription.OffsetByteOrder = Constants.LittleEndianByteOrder;
                                break;
                        }
                    }

                    break;
                
                case HeaderSizeMethod.ReadHeaderToEof:
                    this.rbReadHeaderToEof.Checked = true;
                    break;
            }

            #endregion

            #region FILE_RECORD_SETTINGS

            this.tbFileRecordsBeginOffset.Text = vfsSettings.FileRecordParameters.FileRecordsStartOffset;
            this.comboFileRecordSize.Text = vfsSettings.FileRecordParameters.FileRecordSize;

            // Offset
            switch (vfsSettings.FileRecordParameters.FileOffsetMethod)
            { 
                case FileOffsetLengthMethod.offset:
                    this.rbUseVfsFileOffset.Checked = true;
                    this.fileOffsetOffsetDescription.OffsetValue = vfsSettings.FileRecordParameters.FileOffsetOffset;
                    this.fileOffsetOffsetDescription.OffsetSize = vfsSettings.FileRecordParameters.FileOffsetOffsetSize;
                    
                    if (vfsSettings.FileRecordParameters.FileOffsetOffsetEndianessSpecified)
                    {
                        switch (vfsSettings.FileRecordParameters.FileOffsetOffsetEndianess)
                        {
                            case Endianness.big:
                                this.fileOffsetOffsetDescription.OffsetByteOrder = Constants.BigEndianByteOrder;
                                break;
                            case Endianness.little:
                                this.fileOffsetOffsetDescription.OffsetByteOrder = Constants.LittleEndianByteOrder;
                                break;
                        }
                    }                    

                    if (!String.IsNullOrEmpty(vfsSettings.FileRecordParameters.FileOffsetCalculation))
                    {
                        this.fileOffsetOffsetDescription.CalculationValue = vfsSettings.FileRecordParameters.FileOffsetCalculation;
                    }
                    else if (!String.IsNullOrEmpty(vfsSettings.FileRecordParameters.FileOffsetOffsetMultiplier))
                    {
                        this.fileOffsetOffsetDescription.CalculationValue = String.Format("($V * {0})", vfsSettings.FileRecordParameters.FileOffsetOffsetMultiplier);
                    }
                    else
                    { 
                        this.fileOffsetOffsetDescription.CalculationValue = String.Empty;
                    }

                    break;
                
                case FileOffsetLengthMethod.length:
                    this.rbUseFileSizeToDetermineOffset.Checked = true;
                    this.tbUseFileLengthBeginOffset.Text = vfsSettings.FileRecordParameters.FileCutStartOffset;
                    break;
            }

            // Length
            switch (vfsSettings.FileRecordParameters.FileLengthMethod)
            {
                case FileOffsetLengthMethod.offset:
                    this.rbUseVfsFileLength.Checked = true;
                    this.fileLengthOffsetDescription.OffsetValue = vfsSettings.FileRecordParameters.FileLengthOffset;
                    this.fileLengthOffsetDescription.OffsetSize = vfsSettings.FileRecordParameters.FileLengthOffsetSize;
                    
                    if (vfsSettings.FileRecordParameters.FileLengthOffsetEndianessSpecified)
                    {
                        switch (vfsSettings.FileRecordParameters.FileLengthOffsetEndianess)
                        {
                            case Endianness.big:
                                this.fileLengthOffsetDescription.OffsetByteOrder = Constants.BigEndianByteOrder;
                                break;
                            case Endianness.little:
                                this.fileLengthOffsetDescription.OffsetByteOrder = Constants.LittleEndianByteOrder;
                                break;
                        }
                    }

                    if (!String.IsNullOrEmpty(vfsSettings.FileRecordParameters.FileLengthCalculation))
                    {
                        this.fileLengthOffsetDescription.CalculationValue = vfsSettings.FileRecordParameters.FileLengthCalculation;
                    }
                    else if (!String.IsNullOrEmpty(vfsSettings.FileRecordParameters.FileLengthMultiplier))
                    {
                        this.fileLengthOffsetDescription.CalculationValue = String.Format("($V * {0})", vfsSettings.FileRecordParameters.FileLengthMultiplier);
                    }
                    else
                    {
                        this.fileLengthOffsetDescription.CalculationValue = String.Empty;
                    }

                    break;

                case FileOffsetLengthMethod.length:
                    this.rbUseOffsetsToDetermineLength.Checked = true;
                    break;
            }

            // NAME
            if (vfsSettings.FileRecordParameters.ExtractFileName)
            {
                this.cbFileNameIsPresent.Checked = true;

                switch (vfsSettings.FileRecordParameters.FileNameLocationType)
                { 
                    case NameLocationType.fileRecord:
                        this.rbFileRecordFileNameInRecord.Checked = true;
                        this.tbFileRecordNameOffset.Text = vfsSettings.FileRecordParameters.FileNameOffset;
                        break;
                    
                    case NameLocationType.absoluteOffset:
                        this.rbFileNameAbsoluteOffset.Checked = true;
                        this.fileNameAbsoluteOffsetOffsetDescription.OffsetValue = vfsSettings.FileRecordParameters.FileNameAbsoluteOffsetOffset;
                        this.fileNameAbsoluteOffsetOffsetDescription.OffsetSize = vfsSettings.FileRecordParameters.FileNameAbsoluteOffsetSize;

                        switch (vfsSettings.FileRecordParameters.FileNameAbsoluteOffsetEndianess)
                        {
                            case Endianness.big:
                                this.fileNameAbsoluteOffsetOffsetDescription.OffsetByteOrder = Constants.BigEndianByteOrder;
                                break;
                            case Endianness.little:
                                this.fileNameAbsoluteOffsetOffsetDescription.OffsetByteOrder = Constants.LittleEndianByteOrder;
                                break;
                        }

                        break;
                    case NameLocationType.relativeOffset:
                        this.rbFileNameRelativeOffset.Checked = true;

                        this.fileNameRelativeOffsetOffsetDescription.OffsetValue = vfsSettings.FileRecordParameters.FileNameRelativeOffsetOffset;
                        this.fileNameRelativeOffsetOffsetDescription.OffsetSize = vfsSettings.FileRecordParameters.FileNameRelativeOffsetSize;

                        switch (vfsSettings.FileRecordParameters.FileNameRelativeOffsetEndianess)
                        {
                            case Endianness.big:
                                this.fileNameRelativeOffsetOffsetDescription.OffsetByteOrder = Constants.BigEndianByteOrder;
                                break;
                            case Endianness.little:
                                this.fileNameRelativeOffsetOffsetDescription.OffsetByteOrder = Constants.LittleEndianByteOrder;
                                break;
                        }

                        switch (vfsSettings.FileRecordParameters.FileNameRelativeOffsetLocation)
                        {
                            case RelativeLocationType.fileRecordStart:
                                this.comboFileRecordNameRelativeLocation.SelectedText = VfsExtractorWorker.RELATIVE_TO_START_OF_FILE_RECORD;
                                break;
                            case RelativeLocationType.fileRecordEnd:
                                this.comboFileRecordNameRelativeLocation.SelectedText = VfsExtractorWorker.RELATIVE_TO_END_OF_FILE_RECORD;
                                break;
                        }

                        break;
                }

                // name length
                if (vfsSettings.FileRecordParameters.FileNameLengthMethod.Equals(NameLengthType.staticSize))
                {
                    this.rbFileRecordNameSize.Checked = true;
                    this.tbFileRecordNameSize.Text = vfsSettings.FileRecordParameters.FileNameSize;
                }
                else if (vfsSettings.FileRecordParameters.FileNameLengthMethod.Equals(NameLengthType.terminator))
                {
                    this.rbFileRecordNameTerminator.Checked = true;
                    this.tbFileRecordNameTerminatorBytes.Text = vfsSettings.FileRecordParameters.FileNameTerminator;
                }
            }

            #endregion
        }
        private void loadSelectedPreset()
        {
            VfsExtractorSettings preset = (VfsExtractorSettings)this.comboPresets.SelectedItem;

            if (preset != null)
            {
                this.resetHeaderSection();
                this.resetFileRecordSection();

                this.loadVfsSettings(preset);

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

        private VfsExtractorSettings buildPresetForCurrentValues()
        {
            VfsExtractorSettings vfsSettings = new VfsExtractorSettings();

            #region HEADER_SETTINGS

            if (this.rbHeaderSizeValue.Checked)
            {
                vfsSettings.HeaderParameters.HeaderSizeMethod = HeaderSizeMethod.HeaderSizeValue;
                vfsSettings.HeaderParameters.HeaderEndsAtOffset = this.tbHeaderSizeValue.Text;
            }
            else if (this.rbHeaderSizeOffset.Checked)
            {
                vfsSettings.HeaderParameters.HeaderSizeMethod = HeaderSizeMethod.HeaderSizeOffset;
                vfsSettings.HeaderParameters.HeaderSizeOffset = this.headerSizeOffsetDescription.OffsetValue;
                vfsSettings.HeaderParameters.HeaderSizeOffsetSize = this.headerSizeOffsetDescription.OffsetSize;

                if (this.headerSizeOffsetDescription.OffsetByteOrder.Equals(Constants.BigEndianByteOrder))
                {
                    vfsSettings.HeaderParameters.HeaderSizeOffsetEndianessSpecified = true;
                    vfsSettings.HeaderParameters.HeaderSizeOffsetEndianess = Endianness.big;
                }
                else if (this.headerSizeOffsetDescription.OffsetByteOrder.Equals(Constants.LittleEndianByteOrder))
                {
                    vfsSettings.HeaderParameters.HeaderSizeOffsetEndianessSpecified = true;
                    vfsSettings.HeaderParameters.HeaderSizeOffsetEndianess = Endianness.little;
                }
            }
            else if (this.rbStaticFileCount.Checked)
            {
                vfsSettings.HeaderParameters.HeaderSizeMethod = HeaderSizeMethod.FileCountValue;
                vfsSettings.HeaderParameters.FileCountValue = this.tbStaticFileCount.Text;
            }
            else if (this.rbFileCountOffset.Checked)
            {
                vfsSettings.HeaderParameters.HeaderSizeMethod = HeaderSizeMethod.FileCountOffset;
                vfsSettings.HeaderParameters.FileCountOffset = this.fileCountOffsetDescription.OffsetValue;
                vfsSettings.HeaderParameters.FileCountOffsetSize = this.fileCountOffsetDescription.OffsetSize;

                if (this.fileCountOffsetDescription.OffsetByteOrder.Equals(Constants.BigEndianByteOrder))
                {
                    vfsSettings.HeaderParameters.FileCountOffsetEndianessSpecified = true;
                    vfsSettings.HeaderParameters.FileCountOffsetEndianess = Endianness.big;
                }
                else if (this.fileCountOffsetDescription.OffsetByteOrder.Equals(Constants.LittleEndianByteOrder))
                {
                    vfsSettings.HeaderParameters.FileCountOffsetEndianessSpecified = true;
                    vfsSettings.HeaderParameters.FileCountOffsetEndianess = Endianness.little;
                }
            }
            else if (this.rbReadHeaderToEof.Checked)
            {
                vfsSettings.HeaderParameters.HeaderSizeMethod = HeaderSizeMethod.ReadHeaderToEof;
            }

            #endregion

            #region FILE_RECORD_SETTINGS

            vfsSettings.FileRecordParameters.FileRecordsStartOffset = this.tbFileRecordsBeginOffset.Text;
            vfsSettings.FileRecordParameters.FileRecordSize = this.comboFileRecordSize.Text;

            // Offset
            if (this.rbUseVfsFileOffset.Checked)
            {
                vfsSettings.FileRecordParameters.FileOffsetMethod = FileOffsetLengthMethod.offset;
                vfsSettings.FileRecordParameters.FileOffsetOffset = this.fileOffsetOffsetDescription.OffsetValue;
                vfsSettings.FileRecordParameters.FileOffsetOffsetSize = this.fileOffsetOffsetDescription.OffsetSize;

                if (this.fileOffsetOffsetDescription.OffsetByteOrder.Equals(Constants.BigEndianByteOrder))
                {
                    vfsSettings.FileRecordParameters.FileOffsetOffsetEndianessSpecified = true;
                    vfsSettings.FileRecordParameters.FileOffsetOffsetEndianess = Endianness.big;
                }
                else if (this.fileOffsetOffsetDescription.OffsetByteOrder.Equals(Constants.LittleEndianByteOrder))
                {
                    vfsSettings.FileRecordParameters.FileOffsetOffsetEndianessSpecified = true;
                    vfsSettings.FileRecordParameters.FileOffsetOffsetEndianess = Endianness.little;
                }

                vfsSettings.FileRecordParameters.FileOffsetCalculation = this.fileOffsetOffsetDescription.CalculationValue;
            }
            else if (this.rbUseFileSizeToDetermineOffset.Checked)
            {
                vfsSettings.FileRecordParameters.FileOffsetMethod = FileOffsetLengthMethod.length;
                vfsSettings.FileRecordParameters.FileCutStartOffset = this.tbUseFileLengthBeginOffset.Text;
            }

            // Length
            if (this.rbUseVfsFileLength.Checked)
            {
                vfsSettings.FileRecordParameters.FileLengthMethod = FileOffsetLengthMethod.offset;
                vfsSettings.FileRecordParameters.FileLengthOffset = this.fileLengthOffsetDescription.OffsetValue;
                vfsSettings.FileRecordParameters.FileLengthOffsetSize = this.fileLengthOffsetDescription.OffsetSize;

                if (this.fileLengthOffsetDescription.OffsetByteOrder.Equals(Constants.BigEndianByteOrder))
                {
                    vfsSettings.FileRecordParameters.FileLengthOffsetEndianessSpecified = true;
                    vfsSettings.FileRecordParameters.FileLengthOffsetEndianess = Endianness.big;
                }
                else if (this.fileLengthOffsetDescription.OffsetByteOrder.Equals(Constants.LittleEndianByteOrder))
                {
                    vfsSettings.FileRecordParameters.FileLengthOffsetEndianessSpecified = true;
                    vfsSettings.FileRecordParameters.FileLengthOffsetEndianess = Endianness.little;
                }

                vfsSettings.FileRecordParameters.FileLengthCalculation = this.fileLengthOffsetDescription.CalculationValue;
            }
            else if (this.rbUseOffsetsToDetermineLength.Checked)
            {
                vfsSettings.FileRecordParameters.FileLengthMethod = FileOffsetLengthMethod.length;            
            }
            
            // NAME
            if (this.cbFileNameIsPresent.Checked)
            {
                vfsSettings.FileRecordParameters.ExtractFileName = true;

                if (this.rbFileRecordFileNameInRecord.Checked)
                {
                    vfsSettings.FileRecordParameters.FileNameLocationType = NameLocationType.fileRecord;
                    vfsSettings.FileRecordParameters.FileNameLocationTypeSpecified = true;
                    vfsSettings.FileRecordParameters.FileNameOffset = this.tbFileRecordNameOffset.Text;
                }
                else if (this.rbFileNameAbsoluteOffset.Checked)
                {
                    vfsSettings.FileRecordParameters.FileNameLocationType = NameLocationType.absoluteOffset;
                    vfsSettings.FileRecordParameters.FileNameLocationTypeSpecified = true;

                    vfsSettings.FileRecordParameters.FileNameAbsoluteOffsetOffset = this.fileNameAbsoluteOffsetOffsetDescription.OffsetValue;
                    vfsSettings.FileRecordParameters.FileNameAbsoluteOffsetSize = this.fileNameAbsoluteOffsetOffsetDescription.OffsetSize;

                    if (this.fileNameAbsoluteOffsetOffsetDescription.OffsetByteOrder.Equals(Constants.LittleEndianByteOrder))
                    {
                        vfsSettings.FileRecordParameters.FileNameAbsoluteOffsetEndianess = Endianness.little;
                    }
                    else
                    {
                        vfsSettings.FileRecordParameters.FileNameAbsoluteOffsetEndianess = Endianness.big;
                    }
                }
                else if (this.rbFileNameRelativeOffset.Checked)
                {
                    vfsSettings.FileRecordParameters.FileNameLocationType = NameLocationType.relativeOffset;
                    vfsSettings.FileRecordParameters.FileNameLocationTypeSpecified = true;

                    vfsSettings.FileRecordParameters.FileNameRelativeOffsetOffset = this.fileNameRelativeOffsetOffsetDescription.OffsetValue;
                    vfsSettings.FileRecordParameters.FileNameRelativeOffsetSize = this.fileNameRelativeOffsetOffsetDescription.OffsetSize;

                    if (this.fileNameRelativeOffsetOffsetDescription.OffsetByteOrder.Equals(Constants.LittleEndianByteOrder))
                    {
                        vfsSettings.FileRecordParameters.FileNameRelativeOffsetEndianess = Endianness.little;
                    }
                    else
                    {
                        vfsSettings.FileRecordParameters.FileNameRelativeOffsetEndianess = Endianness.big;
                    }

                    switch (this.comboFileRecordNameRelativeLocation.Text)
                    { 
                        case VfsExtractorWorker.RELATIVE_TO_START_OF_FILE_RECORD:
                            vfsSettings.FileRecordParameters.FileNameRelativeOffsetLocation = RelativeLocationType.fileRecordStart;
                            vfsSettings.FileRecordParameters.FileNameRelativeOffsetLocationSpecified = true;
                            break;
                        case VfsExtractorWorker.RELATIVE_TO_END_OF_FILE_RECORD:
                            vfsSettings.FileRecordParameters.FileNameRelativeOffsetLocation = RelativeLocationType.fileRecordEnd;
                            vfsSettings.FileRecordParameters.FileNameRelativeOffsetLocationSpecified = true;
                            break;
                    }
                }

                if (this.rbFileRecordNameSize.Checked)
                {
                    vfsSettings.FileRecordParameters.FileNameLengthMethodSpecified = true;
                    vfsSettings.FileRecordParameters.FileNameLengthMethod = NameLengthType.staticSize;
                    vfsSettings.FileRecordParameters.FileNameSize = this.tbFileRecordNameSize.Text;
                }
                else if (this.rbFileRecordNameTerminator.Checked)
                {
                    vfsSettings.FileRecordParameters.FileNameLengthMethodSpecified = true;
                    vfsSettings.FileRecordParameters.FileNameLengthMethod = NameLengthType.terminator;
                    vfsSettings.FileRecordParameters.FileNameTerminator = this.tbFileRecordNameTerminatorBytes.Text;
                }
            }
            
            #endregion

            return vfsSettings;
        }
        private void btnSavePreset_Click(object sender, EventArgs e)
        {
            VfsExtractorSettings preset = buildPresetForCurrentValues();

            if (preset != null)
            {
                SavePresetForm saveForm = new SavePresetForm(preset);
                saveForm.Show();
            }
        }

        private void resetHeaderSection()
        {
            this.cbUseHeaderFile.Checked = false;
            this.doUserHeaderFileCheckbox();

            this.rbHeaderSizeValue.Checked = true;
            this.doFileCountRadioButtons();
        }
        private void resetFileRecordSection()
        {
            this.tbFileRecordsBeginOffset.Clear();
            this.comboFileRecordSize.ResetText();

            this.rbUseVfsFileOffset.Checked = true;
            this.doFileRecordOffsetRadioButtons();

            this.rbUseVfsFileLength.Checked = true;
            this.doFileRecordLengthRadioButtons();

            this.cbFileNameIsPresent.Checked = false;
            this.doFileRecordNameCheckbox();
        }

        private void btnBrowseOutputFolder_Click(object sender, EventArgs e)
        {
            this.tbOutputFolder.Text = base.browseForFolder(sender, e);
        }

        private void tbOutputFolder_DragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }
        private void tbOutputFolder_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if ((s.Length == 1) && (Directory.Exists(s[0])))
            {
                this.tbOutputFolder.Text = s[0];
            }
        }

        // combo box key capture
        private void comboFileRecordNameRelativeLocation_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void comboFileRecordNameRelativeLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void doCbDoOffsetByteAlignment()
        {
            if (this.cbDoOffsetByteAlginment.Checked)
            {
                this.tbByteAlignement.Enabled = true;
                this.tbByteAlignement.ReadOnly = false;
            }
            else
            {
                this.tbByteAlignement.Clear();
                this.tbByteAlignement.Enabled = false;
                this.tbByteAlignement.ReadOnly = true;
            }
        }
        private void cbDoOffsetByteAlginment_CheckedChanged(object sender, EventArgs e)
        {
            this.doCbDoOffsetByteAlignment();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.resetForm();
        }
        private void resetForm()
        {
            this.tbDataFilePath.Clear();
            this.cbUseHeaderFile.Checked = false;
            this.tbHeaderFilePath.Clear();
            this.tbOutputFolder.Clear();
            this.cbOutputLogFiles.Checked = false;

            this.rbHeaderSizeValue.Checked = false;
            this.tbHeaderSizeValue.Clear();
            this.rbHeaderSizeOffset.Checked = false;
            this.headerSizeOffsetDescription.Reset();
            this.rbStaticFileCount.Checked = false;
            this.tbStaticFileCount.Clear();
            this.rbFileCountOffset.Checked = false;
            this.fileCountOffsetDescription.Reset();

            this.tbFileRecordsBeginOffset.Clear();
            this.comboFileRecordSize.SelectedText = String.Empty;

            this.rbUseVfsFileOffset.Checked = false;
            this.fileOffsetOffsetDescription.Reset();
            this.rbUseFileSizeToDetermineOffset.Checked = false;
            this.tbUseFileLengthBeginOffset.Clear();
            this.cbDoOffsetByteAlginment.Checked = false;
            this.tbByteAlignement.Clear();

            this.rbUseVfsFileLength.Checked = false;
            this.fileLengthOffsetDescription.Reset();
            this.rbUseOffsetsToDetermineLength.Checked = false;

            this.cbFileNameIsPresent.Checked = false;
            this.rbFileRecordFileNameInRecord.Checked = false;
            this.tbFileRecordNameOffset.Clear();
            this.rbFileNameAbsoluteOffset.Checked = false;
            this.fileNameAbsoluteOffsetOffsetDescription.Reset();
            this.rbFileNameRelativeOffset.Checked = false;
            this.fileNameRelativeOffsetOffsetDescription.Reset();
            this.comboFileRecordNameRelativeLocation.SelectedText = String.Empty;

            this.rbFileRecordNameSize.Checked = false;
            this.tbFileRecordNameSize.Clear();
            this.rbFileRecordNameTerminator.Checked = false;
            this.tbFileRecordNameTerminatorBytes.Clear();

            this.resetHeaderSection();
            this.resetFileRecordSection();
            
            // header file
            this.doUserHeaderFileCheckbox();

            // file count
            this.rbHeaderSizeValue.Checked = true;

            // offset
            this.rbUseVfsFileOffset.Checked = true;
            this.createFileRecordSizeList();

            this.doFileRecordOffsetRadioButtons();

            // length
            this.rbUseVfsFileLength.Checked = true;
            this.doFileRecordLengthRadioButtons();

            // name
            this.doFileRecordNameCheckbox();
            this.createFileNameRelativeOffsetLocationList();

            // presets
            this.loadPresetList();
        }        
    }
}
