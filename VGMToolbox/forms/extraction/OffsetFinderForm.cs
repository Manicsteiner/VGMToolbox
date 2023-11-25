using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using VGMToolbox.dbutil;
using VGMToolbox.plugin;
using VGMToolbox.tools;
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class OffsetFinderForm : AVgmtForm
    {
        private static readonly string DB_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "db"), "collection.s3db");
        private static readonly string PLUGIN_PATH = 
            Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "plugins"), "AdvancedCutter");        
        
        public OffsetFinderForm(TreeNode pTreeNode)
            : base(pTreeNode)
        {
            // set title
            this.lblTitle.Text = ConfigurationManager.AppSettings["Form_SimpleCutter_Title"];

            // hide the DoTask button since this is a drag and drop form
            this.btnDoTask.Hide();

            this.tbOutput.Text = String.Format(ConfigurationManager.AppSettings["Form_SimpleCutter_IntroText"], PLUGIN_PATH);
           
            InitializeComponent();

            this.toolTip1.SetToolTip(this.btnRefresh, "Refresh Presets");

            this.grpFiles.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_GroupFiles"];
            this.lblDragNDrop.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_LblDragNDrop"];
            this.grpCriteria.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_GroupCriteria"];
            this.cbDoCut.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_CheckBoxDoCut"];
            this.lblStringAtOffset.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_LblStringAtOffset"];
            this.lblOutputExtension.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_LblOutputExtension"];
            this.gbCutSizeOptions.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_GroupCutSizeOptions"];
            this.rbStaticCutSize.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_RadioStaticCutSize"];
            this.rbOffsetBasedCutSize.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_RadioOffsetBasedCutSize"];
            this.lblHasSize.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_LblHasSize"];
            this.lblStoredIn.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_LblStoredIn"];
            this.lblInBytes.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_LblInBytes"];
            this.lblFromStart.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_LblFromStart"];
            this.lblInBytes2.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_LblInBytes2"];
            this.lblByteOrder.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_LblByteOrder"];
            this.rbUseTerminator.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_RadioUseTerminator"];
            this.cbTreatTerminatorAsHex.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_CheckBoxTreatTerminatorAsHex"];
            this.cbIncludeTerminatorInLength.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_CheckBoxIncludeTerminatorInLength"];
            this.cbAddExtraBytes.Text =
                ConfigurationManager.AppSettings["Form_SimpleCutter_CheckBoxExtracCutSizeBytes"];

            // this.lblStartingOffset.Text
            // this.lblMinCutSize.Text
            // this.lblMinCutSizeBytes.Text

            this.createEndianList();
            this.createOffsetSizeList();
            this.doOffsetModuloSearchStringCheckbox();
            this.resetCutSection();
            
            //this.loadPresetsComboBox();
            this.loadOffsetPlugins();
        }

        private void tbSourcePaths_DragDrop(object sender, DragEventArgs e)
        {
            if (validateInputs())
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                OffsetFinderWorker.OffsetFinderStruct ofStruct = new OffsetFinderWorker.OffsetFinderStruct();
                ofStruct.SourcePaths = s;
                ofStruct.searchString = tbSearchString.Text;
                ofStruct.startingOffset = 
                    String.IsNullOrEmpty(tbStartingOffset.Text) ? "0" : tbStartingOffset.Text;
                ofStruct.treatSearchStringAsHex = cbSearchAsHex.Checked;

                if (cbModOffsetSearchString.Checked)
                {
                    ofStruct.DoSearchStringModulo = true;
                    ofStruct.SearchStringModuloDivisor = this.tbOffsetModuloSearchStringDivisor.Text;
                    ofStruct.SearchStringModuloResult = this.tbOffsetModuloSearchStringResult.Text;
                }

                if (cbDoCut.Checked)
                {
                    ofStruct.cutFile = true;

                    ofStruct.searchStringOffset = this.tbSearchStringOffset.Text;
                    ofStruct.OutputFolder = this.tbOutputFolder.Text;
                    ofStruct.outputFileExtension = this.tbOutputExtension.Text;
                    ofStruct.MinimumSize = this.tbMinSizeForCut.Text;

                    if (this.rbOffsetBasedCutSize.Checked)
                    {
                        ofStruct.cutSize = this.tbCutSizeOffset.Text;
                        ofStruct.cutSizeOffsetSize = this.cbOffsetSize.Text;
                        ofStruct.isCutSizeAnOffset = true;
                        ofStruct.isLittleEndian = (cbByteOrder.Text.Equals(OffsetFinderWorker.LITTLE_ENDIAN));
                        ofStruct.UseLengthMultiplier = cbUseLengthMultiplier.Checked;
                        ofStruct.LengthMultiplier = this.tbLengthMultiplier.Text;

                    }
                    else if (this.rbUseTerminator.Checked)
                    {
                        ofStruct.useTerminatorForCutsize = true;
                        ofStruct.terminatorString = this.tbTerminatorString.Text;
                        ofStruct.treatTerminatorStringAsHex = this.cbTreatTerminatorAsHex.Checked;
                        ofStruct.includeTerminatorLength = this.cbIncludeTerminatorInLength.Checked;
                        ofStruct.CutToEofIfTerminatorNotFound = this.cbCutToEOFWhenTerminatorNotFound.Checked;

                        if (cbModOffsetTerminator.Checked)
                        {
                            ofStruct.DoTerminatorModulo = true;
                            ofStruct.TerminatorStringModuloDivisor = this.tbOffsetModuloTerminatorDivisor.Text;
                            ofStruct.TerminatorStringModuloResult = this.tbOffsetModuloTerminatorResult.Text;
                        }
                    }
                    else if (this.rbStaticCutSize.Checked)
                    {
                        ofStruct.cutSize = this.tbStaticCutsize.Text;
                    }
                    else
                    {
                        MessageBox.Show("Please select a radio button indicating the Cut Size Options to use.");
                        return; // hokey, but oh well
                    }

                    if (cbAddExtraBytes.Checked)
                    {
                        ofStruct.extraCutSizeBytes = tbExtraCutSizeBytes.Text;
                    }
                }

                ofStruct.OutputLogFile = this.cbOutputLogFile.Checked;

                base.backgroundWorker_Execute(ofStruct);
            }
        }
        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }        
        private void doRadioCheckedChanged(object sender, EventArgs e)
        {
            if (rbStaticCutSize.Checked)
            {
                tbStaticCutsize.ReadOnly = false;
                tbCutSizeOffset.ReadOnly = true;
                cbOffsetSize.Enabled = false;
                cbByteOrder.Enabled = false;
                this.cbUseLengthMultiplier.Checked = false;
                this.cbUseLengthMultiplier.Enabled = false;                
                tbTerminatorString.ReadOnly = true;
                cbTreatTerminatorAsHex.Enabled = false;
                cbIncludeTerminatorInLength.Enabled = false;
                this.cbModOffsetTerminator.Enabled = false;
                this.tbOffsetModuloTerminatorDivisor.ReadOnly = true;
                this.tbOffsetModuloTerminatorDivisor.Enabled = false;                
                this.tbOffsetModuloTerminatorResult.ReadOnly = true;
                this.tbOffsetModuloTerminatorResult.Enabled = false;
                this.cbCutToEOFWhenTerminatorNotFound.Checked = false;
                this.cbCutToEOFWhenTerminatorNotFound.Enabled = false;
            }
            else if (rbOffsetBasedCutSize.Checked)
            {
                tbStaticCutsize.ReadOnly = true;
                tbCutSizeOffset.ReadOnly = false;
                cbOffsetSize.Enabled = true;
                cbByteOrder.Enabled = true;
                this.cbUseLengthMultiplier.Enabled = true;
                tbTerminatorString.ReadOnly = true;
                cbTreatTerminatorAsHex.Enabled = false;
                cbIncludeTerminatorInLength.Enabled = false;
                this.cbModOffsetTerminator.Enabled = false;
                this.tbOffsetModuloTerminatorDivisor.ReadOnly = true;
                this.tbOffsetModuloTerminatorDivisor.Enabled = false;
                this.tbOffsetModuloTerminatorResult.ReadOnly = true;
                this.tbOffsetModuloTerminatorResult.Enabled = false;
                this.cbCutToEOFWhenTerminatorNotFound.Checked = false;
                this.cbCutToEOFWhenTerminatorNotFound.Enabled = false;
            }
            else
            {
                tbStaticCutsize.ReadOnly = true;
                tbCutSizeOffset.ReadOnly = true;
                cbOffsetSize.Enabled = false;
                cbByteOrder.Enabled = false;
                this.cbUseLengthMultiplier.Enabled = false;
                tbTerminatorString.ReadOnly = false;
                cbTreatTerminatorAsHex.Enabled = true;
                cbIncludeTerminatorInLength.Enabled = true;
                this.cbModOffsetTerminator.Enabled = true;
                this.tbOffsetModuloTerminatorDivisor.ReadOnly = false;
                this.tbOffsetModuloTerminatorDivisor.Enabled = true;
                this.tbOffsetModuloTerminatorResult.ReadOnly = false;
                this.tbOffsetModuloTerminatorResult.Enabled = true;
                this.cbCutToEOFWhenTerminatorNotFound.Enabled = true;
            }

            this.doOffsetModuloTerminatorCheckbox();
            this.doCbUseLengthMultiplier();
        }

        private void createEndianList()
        {
            cbByteOrder.Items.Add(OffsetFinderWorker.BIG_ENDIAN);
            cbByteOrder.Items.Add(OffsetFinderWorker.LITTLE_ENDIAN);
        }
        private void createOffsetSizeList()
        {
            cbOffsetSize.Items.Add("1");
            cbOffsetSize.Items.Add("2");
            cbOffsetSize.Items.Add("4");            
        }

        private void cbByteOrder_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void cbByteOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void cbOffsetSize_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void cbOffsetSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private bool validateInputs()
        {
            bool ret = AVgmtForm.checkTextBox(this.tbSearchString.Text, "Search String");

            if (cbDoCut.Checked)
            {
                if (!String.IsNullOrEmpty(this.tbOutputFolder.Text))
                {
                    ret = ret && AVgmtForm.checkFolderExists(this.tbOutputFolder.Text, this.lblOutputFolder.Text);
                }
                ret = ret && AVgmtForm.checkTextBox(this.tbOutputExtension.Text, "Output Extension");

                if (this.rbStaticCutSize.Checked)
                {
                    ret = ret && AVgmtForm.checkTextBox(this.tbStaticCutsize.Text, "Static Cut Size");
                }
                else if (rbOffsetBasedCutSize.Checked)
                {
                    ret = ret && AVgmtForm.checkTextBox(this.tbCutSizeOffset.Text, "Cut Size Offset");
                    ret = ret && AVgmtForm.checkTextBox((string)this.cbOffsetSize.Text, "Offset Size");
                    ret = ret && AVgmtForm.checkTextBox((string)this.cbByteOrder.Text, "Byte Order");
                }
                else
                {
                    ret = ret && AVgmtForm.checkTextBox(this.tbTerminatorString.Text, "Terminator String");
                }
            }

            return ret;
        }
        private void cbDoCut_CheckedChanged(object sender, EventArgs e)
        {
            this.resetCutSection();            
        }
        private void resetCutSection()
        {
            this.cbUseLengthMultiplier.Checked = false;
            
            if (cbDoCut.Checked)
            {
                this.cbOutputLogFile.Enabled = true;
                this.cbOutputLogFile.Checked = false;
                this.cbOutputLogFile.Show();

                tbSearchStringOffset.ReadOnly = false;
                tbOutputExtension.ReadOnly = false;

                this.lblMinCutSize.Show();
                this.tbMinSizeForCut.ReadOnly = false;
                this.tbMinSizeForCut.Enabled = true;
                this.tbMinSizeForCut.Show();                
                this.lblMinCutSizeBytes.Show();

                rbStaticCutSize.Enabled = true;
                rbOffsetBasedCutSize.Enabled = true;

                rbStaticCutSize.Checked = false;
                rbOffsetBasedCutSize.Checked = false;

                tbStaticCutsize.ReadOnly = false;
                tbCutSizeOffset.ReadOnly = false;
                cbOffsetSize.Enabled = true;
                cbByteOrder.Enabled = true;

                tbSearchStringOffset.Show();
                tbOutputExtension.Show();
                rbStaticCutSize.Show();
                rbOffsetBasedCutSize.Show();
                rbStaticCutSize.Show();
                rbOffsetBasedCutSize.Show();
                tbStaticCutsize.Show();
                tbCutSizeOffset.Show();
                cbOffsetSize.Show();
                cbByteOrder.Show();

                cbUseLengthMultiplier.Show();
                tbLengthMultiplier.Show();

                gbCutSizeOptions.Show();
                lblStringAtOffset.Show();
                lblHasSize.Show();
                lblFromStart.Show();
                lblInBytes2.Show();
                lblInBytes.Show();
                lblStoredIn.Show();
                lblByteOrder.Show();
                lblOutputExtension.Show();
                
                this.rbUseTerminator.Show();
                this.tbTerminatorString.Show();
                this.cbTreatTerminatorAsHex.Show();
                this.cbIncludeTerminatorInLength.Show();

                this.cbAddExtraBytes.Show();
                this.tbExtraCutSizeBytes.Show();

                this.cbModOffsetTerminator.Checked = false;
                this.cbModOffsetTerminator.Show();
                this.tbOffsetModuloTerminatorDivisor.Show();
                this.tbOffsetModuloTerminatorResult.Show();
                this.lblOffsetModuloEquals.Show();

                this.lblOutputFolder.Show();
                this.tbOutputFolder.Show();
                this.btnBrowseOutputFolder.Show();
            }
            else
            {
                this.cbOutputLogFile.Enabled = false;
                this.cbOutputLogFile.Checked = false;
                this.cbOutputLogFile.Hide();
                
                tbSearchStringOffset.ReadOnly = true;
                tbOutputExtension.ReadOnly = true;

                this.lblMinCutSize.Hide();
                this.tbMinSizeForCut.ReadOnly = true;
                this.tbMinSizeForCut.Enabled = false;
                this.tbMinSizeForCut.Hide();
                this.lblMinCutSizeBytes.Hide();

                rbStaticCutSize.Checked = false;
                rbOffsetBasedCutSize.Checked = false;

                rbStaticCutSize.Enabled = false;
                rbOffsetBasedCutSize.Enabled = false;

                tbStaticCutsize.ReadOnly = true;
                tbCutSizeOffset.ReadOnly = true;
                cbOffsetSize.Enabled = false;
                cbByteOrder.Enabled = false;

                cbUseLengthMultiplier.Show();
                tbLengthMultiplier.Show();

                tbSearchStringOffset.Hide();
                tbOutputExtension.Hide();
                rbStaticCutSize.Hide();
                rbOffsetBasedCutSize.Hide();
                rbStaticCutSize.Hide();
                rbOffsetBasedCutSize.Hide();
                tbStaticCutsize.Hide();
                tbCutSizeOffset.Hide();
                cbOffsetSize.Hide();
                cbByteOrder.Hide();

                gbCutSizeOptions.Hide();
                lblStringAtOffset.Hide();
                lblHasSize.Hide();
                lblFromStart.Hide();
                lblInBytes2.Hide();
                lblInBytes.Hide();
                lblStoredIn.Hide();
                lblByteOrder.Hide();
                lblOutputExtension.Hide();

                this.rbUseTerminator.Hide();
                this.tbTerminatorString.Hide();
                this.cbTreatTerminatorAsHex.Hide();
                this.cbIncludeTerminatorInLength.Hide();

                this.cbAddExtraBytes.Hide();
                this.tbExtraCutSizeBytes.Hide();

                this.cbModOffsetTerminator.Checked = false;
                this.cbModOffsetTerminator.Hide();
                this.tbOffsetModuloTerminatorDivisor.Hide();
                this.tbOffsetModuloTerminatorResult.Hide();
                this.lblOffsetModuloEquals.Hide();

                this.lblOutputFolder.Hide();
                this.tbOutputFolder.Hide();
                this.btnBrowseOutputFolder.Hide();
            }

            this.doCbUseLengthMultiplier();
            this.doOffsetModuloTerminatorCheckbox();
        }
        private void resetCriteriaSection()
        {
            this.tbSearchString.Clear();
            this.cbSearchAsHex.Checked = false;
            this.tbStartingOffset.Text = "0";

            this.cbModOffsetSearchString.Checked = false;
            this.doOffsetModuloSearchStringCheckbox();
        }
        private void cbAddExtraBytes_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAddExtraBytes.Checked)
            {
                tbExtraCutSizeBytes.ReadOnly = false;
            }
            else
            {
                tbExtraCutSizeBytes.ReadOnly = true;
            }
        }
        
        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new OffsetFinderWorker();
        }
        protected override string getCancelMessage()
        {
            return ConfigurationManager.AppSettings["Form_SimpleCutter_MessageCancel"];
        }
        protected override string getCompleteMessage()
        {
            return ConfigurationManager.AppSettings["Form_SimpleCutter_MessageComplete"];
        }
        protected override string getBeginMessage()
        {
            return ConfigurationManager.AppSettings["Form_SimpleCutter_MessageBegin"];
        }

        private void loadPresetsComboBox()
        {
            this.comboPresets.Items.Add(String.Empty);

            this.comboPresets.DataSource = SqlLiteUtil.GetSimpleDataTable(DB_PATH, "OffsetFinder", "OffsetFinderFormatName");
            this.comboPresets.DisplayMember = "OffsetFinderFormatName";
            this.comboPresets.ValueMember = "OffsetFinderId";
        }                
        
        private void loadSelectedItem()
        {
            OffsetFinderTemplate preset = (OffsetFinderTemplate)this.comboPresets.SelectedItem;

            if (preset != null)
            {
                this.resetCriteriaSection();
                this.resetCutSection();
                this.loadOffsetFinderPreset(preset);

                if (!String.IsNullOrEmpty(preset.NotesOrWarnings))
                {
                    MessageBox.Show(preset.NotesOrWarnings, "Notes/Warnings");
                }
            }
        }
        private void comboPresets_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void comboPresets_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnLoadPreset_Click(object sender, EventArgs e)
        {
            loadSelectedItem();
        }

        private void loadOffsetPlugins()
        {
            comboPresets.Items.Clear();
            
            foreach (string f in Directory.GetFiles(PLUGIN_PATH, "*.xml", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    OffsetFinderTemplate preset = getPresetFromFile(f);

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
        private OffsetFinderTemplate getPresetFromFile(string filePath)
        {
            OffsetFinderTemplate preset = null;
            
            preset = new OffsetFinderTemplate();
            XmlSerializer serializer = new XmlSerializer(preset.GetType());
            using (FileStream xmlFs = File.OpenRead(filePath))
            {
                using (XmlTextReader textReader = new XmlTextReader(xmlFs))
                {
                    preset = (OffsetFinderTemplate)serializer.Deserialize(textReader);
                }
            }

            return preset;
        }
        private void loadOffsetFinderPreset(OffsetFinderTemplate presets)
        {           
            this.cbDoCut.Checked = true;

            // Criteria Section
            this.tbSearchString.Text = presets.SearchParameters.SearchString;
            this.cbSearchAsHex.Checked = presets.SearchParameters.TreatSearchStringAsHex;
            this.tbStartingOffset.Text = presets.SearchParameters.StartingOffset;

            if (presets.SearchParameters.UseModOffsetForSearchStringSpecified &&
                presets.SearchParameters.UseModOffsetForSearchString)
            {
                this.cbModOffsetSearchString.Checked = true;
                this.tbOffsetModuloSearchStringDivisor.Text = presets.SearchParameters.ModOffsetForSearchStringDivisor;
                this.tbOffsetModuloSearchStringResult.Text = presets.SearchParameters.ModOffsetForSearchStringResult;            
            }

            // Cut Options
            this.tbSearchStringOffset.Text = presets.SearchParameters.SearchStringOffset;
            this.tbOutputExtension.Text = presets.SearchParameters.OutputFileExtension;
            this.tbMinSizeForCut.Text = presets.SearchParameters.MinimumSizeForCutting;

            // Cut Size Options
            switch (presets.SearchParameters.CutParameters.CutStyle)
            {
                case CutStyle.@static:
                    this.rbStaticCutSize.Checked = true;
                    this.tbStaticCutsize.Text = presets.SearchParameters.CutParameters.StaticCutSize;
                    break;
                case CutStyle.offset:
                    this.rbOffsetBasedCutSize.Checked = true;
                    this.tbCutSizeOffset.Text = presets.SearchParameters.CutParameters.CutSizeAtOffset;
                    this.cbOffsetSize.SelectedItem = presets.SearchParameters.CutParameters.CutSizeOffsetSize;
                    switch (presets.SearchParameters.CutParameters.CutSizeOffsetEndianess)
                    {
                        case Endianness.big:
                            this.cbByteOrder.SelectedItem = "Big Endian";
                            break;
                        case Endianness.little:
                            this.cbByteOrder.SelectedItem = "Little Endian";
                            break;
                    }

                    if (!String.IsNullOrEmpty(presets.SearchParameters.CutParameters.CutSizeMultiplier))
                    {
                        this.cbUseLengthMultiplier.Checked = true;
                        this.tbLengthMultiplier.Text = presets.SearchParameters.CutParameters.CutSizeMultiplier;
                    }

                    break;
                case CutStyle.terminator:
                    this.rbUseTerminator.Checked = true;
                    this.tbTerminatorString.Text = presets.SearchParameters.CutParameters.TerminatorString;
                    this.cbTreatTerminatorAsHex.Checked = presets.SearchParameters.CutParameters.TreatTerminatorStringAsHex;
                    this.cbIncludeTerminatorInLength.Checked = presets.SearchParameters.CutParameters.IncludeTerminatorInSize;
                    break;
            }

            if (presets.SearchParameters.CutParameters.UseModOffsetForTerminatorStringSpecified &&
                presets.SearchParameters.CutParameters.UseModOffsetForTerminatorString)
            {
                this.cbModOffsetTerminator.Checked = true;
                this.tbOffsetModuloTerminatorDivisor.Text = presets.SearchParameters.CutParameters.ModOffsetForTerminatorStringDivisor;
                this.tbOffsetModuloTerminatorResult.Text = presets.SearchParameters.CutParameters.ModOffsetForTerminatorStringResult;
            }
            
            this.cbAddExtraBytes.Checked = presets.SearchParameters.AddExtraBytes;
            this.tbExtraCutSizeBytes.Text = presets.SearchParameters.AddExtraByteSize;        
        }

        private OffsetFinderTemplate getPresetForCurrentValues()
        { 
            OffsetFinderTemplate preset = new OffsetFinderTemplate();

            // Criteria Section
            preset.SearchParameters.SearchString = this.tbSearchString.Text;
            preset.SearchParameters.TreatSearchStringAsHex = this.cbSearchAsHex.Checked;
            preset.SearchParameters.StartingOffset = this.tbStartingOffset.Text;

            if (this.cbModOffsetSearchString.Checked)
            {
                preset.SearchParameters.UseModOffsetForSearchStringSpecified = true;
                preset.SearchParameters.UseModOffsetForSearchString = true;

                preset.SearchParameters.ModOffsetForSearchStringDivisor = this.tbOffsetModuloSearchStringDivisor.Text;
                preset.SearchParameters.ModOffsetForSearchStringResult = this.tbOffsetModuloSearchStringResult.Text;
            }
            else
            {
                preset.SearchParameters.UseModOffsetForSearchStringSpecified = true;
                preset.SearchParameters.UseModOffsetForSearchString = false;            
            }
            
            // Cut Options Section
            preset.SearchParameters.SearchStringOffset = this.tbSearchStringOffset.Text;
            preset.SearchParameters.OutputFileExtension = this.tbOutputExtension.Text;
            preset.SearchParameters.MinimumSizeForCutting = this.tbMinSizeForCut.Text;

            // Cut Size Section
            if (this.rbStaticCutSize.Checked)
            {
                preset.SearchParameters.CutParameters.CutStyle = CutStyle.@static;
                preset.SearchParameters.CutParameters.StaticCutSize = this.tbStaticCutsize.Text;

            }
            else if (this.rbOffsetBasedCutSize.Checked)
            {
                preset.SearchParameters.CutParameters.CutStyle = CutStyle.offset;
                preset.SearchParameters.CutParameters.CutSizeAtOffset = this.tbCutSizeOffset.Text;
                preset.SearchParameters.CutParameters.CutSizeOffsetSize = this.cbOffsetSize.SelectedItem.ToString();

                if (this.cbByteOrder.SelectedItem.Equals("Little Endian"))
                {
                    preset.SearchParameters.CutParameters.CutSizeOffsetEndianessSpecified = true;
                    preset.SearchParameters.CutParameters.CutSizeOffsetEndianess = Endianness.little;
                }
                else if (this.cbByteOrder.SelectedItem.Equals("Big Endian"))
                {
                    preset.SearchParameters.CutParameters.CutSizeOffsetEndianessSpecified = true;
                    preset.SearchParameters.CutParameters.CutSizeOffsetEndianess = Endianness.big;
                }

                if (!String.IsNullOrEmpty(this.tbLengthMultiplier.Text))
                {
                    preset.SearchParameters.CutParameters.CutSizeMultiplier = this.tbLengthMultiplier.Text;
                }
            }
            else if (this.rbUseTerminator.Checked)
            {
                preset.SearchParameters.CutParameters.CutStyle = CutStyle.terminator;
                preset.SearchParameters.CutParameters.TerminatorString = this.tbTerminatorString.Text;
                preset.SearchParameters.CutParameters.TreatTerminatorStringAsHexSpecified = this.cbTreatTerminatorAsHex.Checked;
                preset.SearchParameters.CutParameters.TreatTerminatorStringAsHex = this.cbTreatTerminatorAsHex.Checked;
                preset.SearchParameters.CutParameters.IncludeTerminatorInSizeSpecified = true;
                preset.SearchParameters.CutParameters.IncludeTerminatorInSize = this.cbIncludeTerminatorInLength.Checked;

                if (this.cbModOffsetTerminator.Checked)
                {
                    preset.SearchParameters.CutParameters.UseModOffsetForTerminatorStringSpecified = true;
                    preset.SearchParameters.CutParameters.UseModOffsetForTerminatorString = true;

                    preset.SearchParameters.CutParameters.ModOffsetForTerminatorStringDivisor = this.tbOffsetModuloTerminatorDivisor.Text;
                    preset.SearchParameters.CutParameters.ModOffsetForTerminatorStringResult = this.tbOffsetModuloTerminatorResult.Text;

                }
            }

            preset.SearchParameters.AddExtraBytes = this.cbAddExtraBytes.Checked;
            preset.SearchParameters.AddExtraByteSize = this.tbExtraCutSizeBytes.Text;

            return preset;
        }
        private void btnSavePreset_Click(object sender, EventArgs e)
        {
            OffsetFinderTemplate preset = getPresetForCurrentValues();

            if (preset != null)
            {
                SavePresetForm saveForm = new SavePresetForm(preset);
                saveForm.Show();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.loadOffsetPlugins();
        }

        private void cbModOffsetTerminator_CheckedChanged(object sender, EventArgs e)
        {
            doOffsetModuloTerminatorCheckbox();
        }

        private void doOffsetModuloTerminatorCheckbox()
        {
            if (!cbModOffsetTerminator.Checked)
            {
                this.tbOffsetModuloTerminatorDivisor.ReadOnly = true;
                this.tbOffsetModuloTerminatorDivisor.Enabled = false;
                this.tbOffsetModuloTerminatorDivisor.Clear();

                this.tbOffsetModuloTerminatorResult.ReadOnly = true;
                this.tbOffsetModuloTerminatorResult.Enabled = false;
                this.tbOffsetModuloTerminatorResult.Clear();
            }
            else
            {
                this.tbOffsetModuloTerminatorDivisor.ReadOnly = false;
                this.tbOffsetModuloTerminatorDivisor.Enabled = true;
                this.tbOffsetModuloTerminatorDivisor.Clear();

                this.tbOffsetModuloTerminatorResult.ReadOnly = false;
                this.tbOffsetModuloTerminatorResult.Enabled = true;
                this.tbOffsetModuloTerminatorResult.Clear();
            }
        }

        private void doOffsetModuloSearchStringCheckbox()
        {
            if (!cbModOffsetSearchString.Checked)
            {
                this.tbOffsetModuloSearchStringDivisor.ReadOnly = true;
                this.tbOffsetModuloSearchStringDivisor.Enabled = false;
                this.tbOffsetModuloSearchStringDivisor.Clear();

                this.tbOffsetModuloSearchStringResult.ReadOnly = true;
                this.tbOffsetModuloSearchStringResult.Enabled = false;
                this.tbOffsetModuloSearchStringResult.Clear();
            }
            else
            {
                this.tbOffsetModuloSearchStringDivisor.ReadOnly = false;
                this.tbOffsetModuloSearchStringDivisor.Enabled = true;
                this.tbOffsetModuloSearchStringDivisor.Clear();

                this.tbOffsetModuloSearchStringResult.ReadOnly = false;
                this.tbOffsetModuloSearchStringResult.Enabled = true;
                this.tbOffsetModuloSearchStringResult.Clear();
            }
        }

        private void cbModOffsetSearchString_CheckedChanged(object sender, EventArgs e)
        {
            this.doOffsetModuloSearchStringCheckbox();
        }

        private void btnBrowseOutputFolder_Click(object sender, EventArgs e)
        {
            this.tbOutputFolder.Text = base.browseForFolder(sender, e);
        }

        private void doCbUseLengthMultiplier()
        {
            if (this.cbUseLengthMultiplier.Checked)
            {
                this.tbLengthMultiplier.Enabled = true;
                this.tbLengthMultiplier.ReadOnly = false;
            }
            else
            {
                this.tbLengthMultiplier.Clear();
                this.tbLengthMultiplier.Enabled = false;
                this.tbLengthMultiplier.ReadOnly = true;            
            }

        }
        private void cbUseLengthMultiplier_CheckedChanged(object sender, EventArgs e)
        {
            this.doCbUseLengthMultiplier();
        }
    }
}
