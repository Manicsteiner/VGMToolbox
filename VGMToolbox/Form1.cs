using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using VGMToolbox.auditing;
using VGMToolbox.format.auditing;
using VGMToolbox.format.hoot;
using VGMToolbox.tools;
using VGMToolbox.tools.gbs;
using VGMToolbox.tools.hoot;
using VGMToolbox.tools.nds;
using VGMToolbox.tools.nsf;
using VGMToolbox.tools.xsf;
using VGMToolbox.util;


namespace VGMToolbox
{
    public partial class Form1 : Form
    {
        DatafileCreatorWorker datCreator;
        RebuilderWorker rebuilder;
        TreeBuilderWorker treeBuilder;
        DatafileCheckerWorker datafileCheckerWorker;
        HootXmlBuilderWorker hootXmlBuilder;
        XsfCompressedProgramExtractorWorker xsfCompressedProgramExtractor;
        GbsM3uBuilderWorker gbsM3uBuilder;
        NsfeM3uBuilderWorker nsfeM3uBuilder;
        // Rip2sfWorker rip2sfWorker;
        SdatExtractorWorker sdatExtractorWorker;
        

        DateTime elapsedTimeStart;
        DateTime elapsedTimeEnd;
        TimeSpan elapsedTime;

        public Form1()
        {
            InitializeComponent();
            
            // Show Splash
            //Thread th = new Thread(new ThreadStart(DoSplash));
            //Thread th2 = new Thread(new ThreadStart(DoWelcomeMessage));
            //th2.Start();
            //th.Start();            
            //Thread.Sleep(1000);
            //th.Abort();
            //Thread.Sleep(1000);            
        }

        private void btnMdxBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbMdxSourceFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnMdxFindPdx_Click(object sender, EventArgs e)
        {
            if (tbMdxSourceFolder.Text.Length > 0)
            {
                tbOutput.Clear();
                treeViewTools.Nodes.Clear();
                
                TreeNode parentNode = new TreeNode(tbMdxSourceFolder.Text);
                treeViewTools.Nodes.Add(parentNode);
                MdxUtil mdxTools = new MdxUtil();
                mdxTools.getPdxForDir(tbMdxSourceFolder.Text, tbOutput, parentNode, cbMdxCheckPdxExist.Checked);
            }
        }

        private void tbMdxSourceFolder_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tbMdxSourceFolder_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);                                   
            MdxUtil mdxTools = new MdxUtil();
            TreeNode parentNode;
            int i;
            
            try
            {
                doCleanup();               

                for (i = 0; i < s.Length; i++)
                {
                    if (File.Exists(s[i]))
                    {
                        parentNode = new TreeNode(Path.GetFileName(s[i]));
                        treeViewTools.Nodes.Add(parentNode);
                        mdxTools.getPdxForFile(s[i], tbOutput, parentNode, cbMdxCheckPdxExist.Checked);
                    }
                    else if (Directory.Exists(s[i]))
                    {
                        parentNode = new TreeNode(s[i]);
                        treeViewTools.Nodes.Add(parentNode);
                        mdxTools.getPdxForDir(s[i], tbOutput, parentNode, cbMdxCheckPdxExist.Checked);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }                

        private void DoSplash()
        {
            Splash sp = new Splash();
            sp.ShowDialog();           
        }

        # region DATAFILE CREATOR

        /*
        private void btnDatCreator_BuildDat_Click(object sender, EventArgs e)
        {
            doCleanup();
            
            if (checkDatafileCreatorInputs())
            {
                toolStripStatusLabel1.Text = "Building Datafile...";
                toolStripProgressBar.Maximum = Directory.GetFiles(tbDatCreator_SourceFolder.Text, "*.*", SearchOption.AllDirectories).Length;

                string outputMessage = "";
                Datafile datafile = new Datafile();
                DatCreator datCreator = new DatCreator();
                datafile.header = datCreator.buildHeader(tbDatCreator_Author.Text, tbDatCreator_Category.Text,
                    tbDatCreator_Comment.Text, tbDatCreator_Date.Text, tbDatCreator_Description.Text,
                    tbDatCreator_Email.Text, tbDatCreator_Homepage.Text, tbDatCreator_Name.Text,
                    tbDatCreator_Url.Text, tbDatCreator_Version.Text);

                // datafile.game = datCreator.buildGames(tbDatCreator_SourceFolder.Text, ref outputMessage, cbDir2Dat_UseLibHash.Checked);
                datafile.game = datCreator.buildGames(tbDatCreator_SourceFolder.Text, ref outputMessage, false, toolStripProgressBar, cbDatCreator_UseLessRam.Checked);

                XmlSerializer serializer = new XmlSerializer(typeof(Datafile));
                TextWriter textWriter = new StreamWriter(tbDatCreator_OutputDat.Text);
                serializer.Serialize(textWriter, datafile);
                textWriter.Close();
                textWriter.Dispose();

                tbOutput.Text += outputMessage;
                toolStripStatusLabel1.Text = "Building Datafile...Complete";

                //Cleanup
                datafile = null;
                datCreator = null;
                serializer = null;
                textWriter = null;
            }
        }
        */

        private void btnDatCreator_BuildDat_Click(object sender, EventArgs e)
        {
            doCleanup();

            if (checkDatafileCreatorInputs())
            {
                toolStripStatusLabel1.Text = "Building Datafile...";
                
                DatafileCreatorWorker.GetGameParamsStruct vGetGameParamsStruct = new DatafileCreatorWorker.GetGameParamsStruct();
                vGetGameParamsStruct.pDir = tbDatCreator_SourceFolder.Text;
                vGetGameParamsStruct.pOutputMessage = "";
                vGetGameParamsStruct.totalFiles = Directory.GetFiles(tbDatCreator_SourceFolder.Text, "*.*", SearchOption.AllDirectories).Length;

                datCreator = new DatafileCreatorWorker();
                datCreator.ProgressChanged += backgroundWorker_ReportProgress;
                datCreator.RunWorkerCompleted += datafileCreatorWorker_WorkComplete;
                datCreator.RunWorkerAsync(vGetGameParamsStruct);               
            }
        }
        
        private void btnDatCreator_BrowseSource_Click(object sender, EventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbDatCreator_SourceFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnDatCreator_BrowseDestination_Click(object sender, EventArgs e)
        {
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "XML File (*.xml)|*.xml|Datafile (*.dat)| *.dat";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbDatCreator_OutputDat.Text = saveFileDialog1.FileName;
            }
        }

        private void btnDatCreator_Cancel_Click(object sender, EventArgs e)
        {
            if (datCreator!= null && datCreator.IsBusy)
            {
                tbOutput.Text += "CANCEL PENDING...";
                datCreator.CancelAsync();
            }
        }

        private bool checkDatafileCreatorInputs()
        {
            bool ret = true;

            if (!(checkTextBox(tbDatCreator_Name.Text, "Datafile Name") &&
                checkTextBox(tbDatCreator_Description.Text, "Description") &&
                checkTextBox(tbDatCreator_SourceFolder.Text, "Source Directory") &&
                checkTextBox(tbDatCreator_OutputDat.Text, "Destination Datafile") &&
                checkFolderExists(tbDatCreator_SourceFolder.Text, "Source Directory")))
            {
                ret = false;
            }

            return ret;
        }

        # endregion
        
        #region REBUILDER

        private void btnRebuilder_BrowseSourceDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbRebuilder_SourceDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnRebuilder_BrowseDestinationDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbRebuilder_DestinationDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnRebuilder_BrowseDatafile_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbRebuilder_Datafile.Text = openFileDialog1.FileName;
            }
        }

        private void btnRebuilder_Rebuild_Click(object sender, EventArgs e)
        {
            doCleanup();
            
            if (checkRebuilderInputs())
            {                                
                toolStripStatusLabel1.Text = "Rebuilding...";

                datafile dataFile = new datafile();
                XmlSerializer serializer = new XmlSerializer(typeof(datafile));
                TextReader textReader = new StreamReader(tbRebuilder_Datafile.Text);
                dataFile = (datafile)serializer.Deserialize(textReader);
                textReader.Close();

                RebuilderWorker.RebuildSetsStruct vRebuildSetsStruct = new RebuilderWorker.RebuildSetsStruct();
                vRebuildSetsStruct.pSourceDir = tbRebuilder_SourceDir.Text;
                vRebuildSetsStruct.pDestinationDir = tbRebuilder_DestinationDir.Text;
                vRebuildSetsStruct.pDatFile = dataFile;
                vRebuildSetsStruct.pRemoveSource = cbRebuilder_RemoveSource.Checked;
                vRebuildSetsStruct.pOverwriteExisting = cbRebuilder_Overwrite.Checked;
                vRebuildSetsStruct.ScanOnly = cbRebuilder_ScanOnly.Checked;
                vRebuildSetsStruct.pCompressOutput = cbRebuilder_CompressOutput.Checked;

                try
                {
                    vRebuildSetsStruct.totalFiles = Directory.GetFiles(tbRebuilder_SourceDir.Text, "*.*", SearchOption.AllDirectories).Length;

                    rebuilder = new RebuilderWorker();
                    rebuilder.ProgressChanged += backgroundWorker_ReportProgress;
                    rebuilder.RunWorkerCompleted += rebuilderWorker_WorkComplete;
                    rebuilder.RunWorkerAsync(vRebuildSetsStruct);
                }
                catch (Exception exception2)
                {
                    tbOutput.Text += exception2.Message;
                }
            }
        }

        private bool checkRebuilderInputs()
        {
            bool ret = true;

            if (checkTextBox(tbRebuilder_SourceDir.Text, "Source Directory") &&
                checkTextBox(tbRebuilder_DestinationDir.Text, "Destination Directory") &&
                checkTextBox(tbRebuilder_Datafile.Text, "Datafile Path") &&
                checkFolderExists(tbRebuilder_SourceDir.Text, "Source Directory") &&
                checkFolderExists(tbRebuilder_DestinationDir.Text, "Destination Directory") &&
                checkFileExists(tbRebuilder_Datafile.Text, "Datafile Path"))
            {

                if (tbRebuilder_SourceDir.Text.Trim().Equals(tbRebuilder_DestinationDir.Text.Trim()))
                {
                    MessageBox.Show("Source directory cannot be the same as the Destination directory");
                    ret = false;
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }

        private void cbRebuilder_CompressOutput_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRebuilder_CompressOutput.Checked)
            {
                cbRebuilder_Overwrite.Checked = false;
                cbRebuilder_ScanOnly.Checked = false;
            }
        }

        private void cbRebuilder_Overwrite_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRebuilder_Overwrite.Checked)
            {
                cbRebuilder_CompressOutput.Checked = false;
                cbRebuilder_ScanOnly.Checked = false;
            }
        }

        private void cbRebuilder_RemoveSource_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRebuilder_RemoveSource.Checked)
            {
                cbRebuilder_ScanOnly.Checked = false;
            }
        }

        private void cbRebuilder_ScanOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRebuilder_ScanOnly.Checked)
            {
                cbRebuilder_RemoveSource.Checked = false;
                cbRebuilder_Overwrite.Checked = false;
                cbRebuilder_CompressOutput.Checked = false;
            }
        }

        private void btnRebuilder_Cancel_Click(object sender, EventArgs e)
        {
            if (rebuilder != null && rebuilder.IsBusy)
            {
                tbOutput.Text += "CANCEL PENDING...";
                rebuilder.CancelAsync();
            }
        }

        # endregion

        # region EXAMINE

        private void tbXsfSource_DragDrop(object sender, DragEventArgs e)
        {
            doCleanup();

            toolStripStatusLabel1.Text = "Examination...Begin";

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            int totalFileCount = 0;
            
            foreach (string path in s)
            {
                if (File.Exists(path))
                {
                    totalFileCount++;
                }
                else if (Directory.Exists(path))
                {
                    totalFileCount += Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Length;
                }
            }

            TreeBuilderWorker.TreeBuilderStruct tbStruct = new TreeBuilderWorker.TreeBuilderStruct();
            tbStruct.pPaths = s;
            tbStruct.totalFiles = totalFileCount;
            
            treeBuilder = new TreeBuilderWorker();
            treeBuilder.ProgressChanged += backgroundWorker_ReportProgress;
            treeBuilder.RunWorkerCompleted += TreeBuilderWorker_WorkComplete;
            treeBuilder.RunWorkerAsync(tbStruct);
        }

        private void TreeBuilderWorker_WorkComplete(object sender,
                             RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                doCancelCleanup(false);
                toolStripStatusLabel1.Text = "Examination...Cancelled";
                tbOutput.Text += "Operation cancelled.";
            }
            else
            {
                lblProgressLabel.Text = String.Empty;
                toolStripStatusLabel1.Text = "Examination...Complete";
            }
        }

        private void tbXsfSource_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void btnXsfGetInfo_Click(object sender, EventArgs e)
        {
            doCleanup();

            toolStripStatusLabel1.Text = "Examination...Begin";

            string[] s = new string[1];
            s[0] = tbXsfSource.Text;

            int totalFileCount = 0;

            foreach (string path in s)
            {
                if (File.Exists(path))
                {
                    totalFileCount++;
                }
                else if (Directory.Exists(path))
                {
                    totalFileCount += Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Length;
                }
            }

            TreeBuilderWorker.TreeBuilderStruct tbStruct = new TreeBuilderWorker.TreeBuilderStruct();
            tbStruct.pPaths = s;
            tbStruct.totalFiles = totalFileCount;

            treeBuilder = new TreeBuilderWorker();
            treeBuilder.ProgressChanged += backgroundWorker_ReportProgress;
            treeBuilder.RunWorkerCompleted += TreeBuilderWorker_WorkComplete;
            treeBuilder.RunWorkerAsync(tbStruct);
        }

        private void btnPsfBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbXsfSource.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnXsf_Cancel_Click(object sender, EventArgs e)
        {
            if (treeBuilder != null && treeBuilder.IsBusy)
            {
                tbOutput.Text += "CANCEL PENDING...";
                treeBuilder.CancelAsync();
            }
        }

        # endregion

        #region MISC

        private void doCleanup()
        {
            tbOutput.Clear();
            treeViewTools.Nodes.Clear();
            doCancelCleanup();
            
            this.elapsedTimeStart = DateTime.Now;
            this.showElapsedTime();
        }

        private void doCancelCleanup()
        {
            doCancelCleanup(true);
        }

        private void doCancelCleanup(bool pClearNodes)
        {
            if (pClearNodes)
            {
                treeViewTools.Nodes.Clear();
            }
            toolStripProgressBar.Value = 0;
            lblProgressLabel.Text = String.Empty;
        }

        private void showElapsedTime()
        {
            this.elapsedTimeEnd = DateTime.Now;
            this.elapsedTime = new TimeSpan();
            this.elapsedTime = elapsedTimeEnd - elapsedTimeStart;
            lblTimeElapsed.Text = String.Format("{0:D2}:{1:D2}:{2:D2}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
        }

        private bool checkTextBox(string pText, string pFieldName)
        {
            bool ret = true;
            
            if (pText.Trim().Length == 0)
            {
                MessageBox.Show(String.Format("{0} cannot be empty.", pFieldName));
                ret = false;
            }
            return ret;
        }

        private bool checkFolderExists(string pPath, string pFolderName)
        {
            bool ret = true;

            if (!Directory.Exists(pPath))
            {
                MessageBox.Show(String.Format("{0} cannot be found.", pFolderName));
                ret = false;
            }
            return ret;
        }

        private bool checkFileExists(string pPath, string pFileName)
        {
            bool ret = true;

            if (!File.Exists(pPath))
            {
                MessageBox.Show(String.Format("{0} cannot be found.", pFileName));
                ret = false;
            }
            return ret;
        }

        # endregion
                
        #region BACKGROUND WORKER
        private void backgroundWorker_ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != Constants.IgnoreProgress &&
                e.ProgressPercentage != Constants.ProgressMessageOnly)
            {
                toolStripProgressBar.Value = e.ProgressPercentage;
                this.Text = "VGMToolbox [" + e.ProgressPercentage + "%]";
            }

            if ((e.ProgressPercentage == Constants.ProgressMessageOnly) && e.UserState != null)
            {
                VGMToolbox.util.ProgressStruct vProgressStruct = (VGMToolbox.util.ProgressStruct)e.UserState;
                tbOutput.Text += vProgressStruct.GenericMessage;
            }            
            else if (e.UserState != null)
            {
                VGMToolbox.util.ProgressStruct vProgressStruct = (VGMToolbox.util.ProgressStruct)e.UserState;

                if (vProgressStruct.NewNode != null)
                {
                    treeViewTools.Nodes.Add(vProgressStruct.NewNode);
                }
                
                
                lblProgressLabel.Text = vProgressStruct.FileName ?? String.Empty;
                tbOutput.Text += vProgressStruct.ErrorMessage ?? String.Empty;
            }

            this.showElapsedTime();
        }

        private void rebuilderWorker_WorkComplete(object sender,
                                     RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                doCancelCleanup();
                toolStripStatusLabel1.Text = "Rebuilding...Cancelled";                
                tbOutput.Text += "Operation cancelled.";
            }
            else
            {
                lblProgressLabel.Text = String.Empty;
                toolStripStatusLabel1.Text = "Rebuilding...Complete";
            }
            this.Text = "VGMToolbox";
        }

        private void datafileCreatorWorker_WorkComplete(object sender,
                                    RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                doCancelCleanup();
                toolStripStatusLabel1.Text = "Building Datafile...Canceled";
                tbOutput.Text += "Operation canceled.";                
            }
            else
            {
                lblProgressLabel.Text = String.Empty;
                
                datafile dataFile = new datafile();
                dataFile.header = DatafileCreatorWorker.buildHeader(tbDatCreator_Author.Text, tbDatCreator_Category.Text,
                    tbDatCreator_Comment.Text, tbDatCreator_Date.Text, tbDatCreator_Description.Text,
                    tbDatCreator_Email.Text, tbDatCreator_Homepage.Text, tbDatCreator_Name.Text,
                    tbDatCreator_Url.Text, tbDatCreator_Version.Text);

                dataFile.game = (VGMToolbox.format.auditing.game[])e.Result;

                XmlSerializer serializer = new XmlSerializer(dataFile.GetType());
                
                //StreamWriter sw = new StreamWriter(tbDatCreator_OutputDat.Text);
                //XmlWriter xw = new XmlTextWriter(sw);
                //xw.WriteDocType("datafile", @"-//Logiqx//DTD ROM Management Datafile//EN", null, 
                //@"http://www.logiqx.com/Dats/datafile.dtd");
                //serializer.Serialize(xw, dataFile);
                //xw.Close();
                //sw.Close();
                //sw.Dispose();

                TextWriter textWriter = new StreamWriter(tbDatCreator_OutputDat.Text);
                serializer.Serialize(textWriter, dataFile);
                textWriter.Close();
                textWriter.Dispose();

                toolStripStatusLabel1.Text = "Building Datafile...Complete";
            }
            this.Text = "VGMToolbox";
        }
        
        private void datafileCheckerWorker_WorkComplete(object sender,
                     RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                doCancelCleanup();
                toolStripStatusLabel1.Text = "Checking Datafile...Cancelled";
                tbOutput.Text += "Operation cancelled.";
            }
            else
            {
                lblProgressLabel.Text = String.Empty;
                toolStripStatusLabel1.Text = "Checking Datafile...Complete";
            }
            this.Text = "VGMToolbox";
        }
        
        #endregion                

        #region DATAFILE CHECKER
        private void btnDatafileChecker_Check_Click(object sender, EventArgs e)
        {
            doCleanup();

            toolStripStatusLabel1.Text = "Checking Datafile...Complete";

            DatafileCheckerWorker.DatafileCheckerStruct datafileCheckerStruct = new DatafileCheckerWorker.DatafileCheckerStruct();
            datafileCheckerStruct.datafilePath = tbDatafileChecker_SourceFile.Text;
            datafileCheckerStruct.outputPath = tbDatafileChecker_OutputPath.Text;

            datafileCheckerWorker = new DatafileCheckerWorker();
            datafileCheckerWorker.ProgressChanged += backgroundWorker_ReportProgress;
            datafileCheckerWorker.RunWorkerCompleted += datafileCheckerWorker_WorkComplete;
            datafileCheckerWorker.RunWorkerAsync(datafileCheckerStruct);
        }

        private void tbDatafileChecker_BrowseSource_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbDatafileChecker_SourceFile.Text = openFileDialog1.FileName;
            }
        }

        private void tbDatafileChecker_BrowseDestination_Click(object sender, EventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbDatafileChecker_OutputPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        #endregion

        #region HOOT - CSV
        private void btnHoot_BrowseCsvFile_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbHoot_CsvSourceFile.Text = openFileDialog1.FileName;
            }
        }
        
        private void btnHoot_BrowseDatFile_Click(object sender, EventArgs e)
        {            
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbHoot_DatSourceFile.Text = openFileDialog1.FileName;
            }
            
        }

        private void btnHoot_DestinationFileBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.DefaultExt = "dat";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "Datafile (*.dat)| *.dat";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbHoot_DatDestinationFile.Text = saveFileDialog1.FileName;
            }        
        }

        private void btnHoot_AddInfo_Click(object sender, EventArgs e)
        {
            doCleanup();

            HootCsvTools.AddCsvInformationToDataFile(tbHoot_CsvSourceFile.Text,
                tbHoot_DatSourceFile.Text, tbHoot_DatDestinationFile.Text);

            tbOutput.Text += "Hoot CSV information transfer to XML Datafile Complete.";
        }        
                
        #endregion

        #region NSFE - M3U
        //nsfeM3uBuilder
        private void tbNSF_nsfe2m3uSource_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tbNSF_nsfe2m3uSource_DragDrop(object sender, DragEventArgs e)
        {
            doCleanup();

            toolStripStatusLabel1.Text = "NSFE to .M3U Conversion...Begin";

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            NsfeM3uBuilderWorker.NsfeM3uBuilderStruct nsfeStruct = new NsfeM3uBuilderWorker.NsfeM3uBuilderStruct();
            nsfeStruct.SourcePaths = s;
            nsfeStruct.OnePlaylistPerFile = cbNSFE_OneM3uPerTrack.Checked;

            nsfeM3uBuilder = new NsfeM3uBuilderWorker();
            nsfeM3uBuilder.ProgressChanged += backgroundWorker_ReportProgress;
            nsfeM3uBuilder.RunWorkerCompleted += NsfeM3uBuilderWorker_WorkComplete;
            nsfeM3uBuilder.RunWorkerAsync(nsfeStruct);
        }

        private void NsfeM3uBuilderWorker_WorkComplete(object sender,
                             RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                doCancelCleanup(false);
                toolStripStatusLabel1.Text = "NSFE to .M3U Conversion...Cancelled";
                tbOutput.Text += "Operation cancelled.";
            }
            else
            {
                lblProgressLabel.Text = String.Empty;
                toolStripStatusLabel1.Text = "NSFE to .M3U Conversion...Complete";
            }
        }

        private void btnNsfM3u_Cancel_Click(object sender, EventArgs e)
        {
            if (nsfeM3uBuilder != null && nsfeM3uBuilder.IsBusy)
            {
                tbOutput.Text += "CANCEL PENDING...";
                nsfeM3uBuilder.CancelAsync();
            }
        }

        #endregion
                
        #region GBS - M3U

        private void tbGBS_gbsm3uSource_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tbGBS_gbsm3uSource_DragDrop(object sender, DragEventArgs e)
        {
            doCleanup();

            toolStripStatusLabel1.Text = "GBS .M3U Creation...Begin";

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            GbsM3uBuilderWorker.GbsM3uWorkerStruct gbStruct = new GbsM3uBuilderWorker.GbsM3uWorkerStruct();
            gbStruct.SourcePaths = s;
            gbStruct.onePlaylistPerFile = cbGBS_OneM3uPerTrack.Checked;

            gbsM3uBuilder = new GbsM3uBuilderWorker();
            gbsM3uBuilder.ProgressChanged += backgroundWorker_ReportProgress;
            gbsM3uBuilder.RunWorkerCompleted += GbsM3uBuilderWorker_WorkComplete;
            gbsM3uBuilder.RunWorkerAsync(gbStruct);
        }

        private void GbsM3uBuilderWorker_WorkComplete(object sender,
                             RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                doCancelCleanup(false);
                toolStripStatusLabel1.Text = "GBS .M3U Creation...Cancelled";
                tbOutput.Text += "Operation cancelled.";
            }
            else
            {
                lblProgressLabel.Text = String.Empty;
                toolStripStatusLabel1.Text = "GBS .M3U Creation...Complete";
            }
        }

        private void btnGbsM3u_Cancel_Click(object sender, EventArgs e)
        {
            if (gbsM3uBuilder != null && gbsM3uBuilder.IsBusy)
            {
                tbOutput.Text += "CANCEL PENDING...";
                gbsM3uBuilder.CancelAsync();
            }
        }
        #endregion

        #region HOOT - XML

        private void HootXmlBuilderWorker_WorkComplete(object sender,
                             RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                doCancelCleanup(false);
                toolStripStatusLabel1.Text = "Hoot XML Generation...Cancelled";
                tbOutput.Text += "Operation cancelled.";
            }
            else
            {
                lblProgressLabel.Text = String.Empty;
                toolStripStatusLabel1.Text = "Hoot XML Generation...Complete";
            }
        }

        private void tbHootXML_Path_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tbHootXML_Path_DragDrop(object sender, DragEventArgs e)
        {
            doCleanup();

            toolStripStatusLabel1.Text = "Hoot XML Generation...Begin";

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            HootXmlBuilderWorker.HootXmlBuilderStruct xbStruct = new HootXmlBuilderWorker.HootXmlBuilderStruct();
            xbStruct.SourcePaths = s;
            xbStruct.combineOutput = cbHootXML_CombineOutput.Checked;
            xbStruct.splitOutput = cbHootXML_SplitOutput.Checked;

            hootXmlBuilder = new HootXmlBuilderWorker();
            hootXmlBuilder.ProgressChanged += backgroundWorker_ReportProgress;
            hootXmlBuilder.RunWorkerCompleted += HootXmlBuilderWorker_WorkComplete;
            hootXmlBuilder.RunWorkerAsync(xbStruct);
        }

        private void btnHootXML_Cancel_Click(object sender, EventArgs e)
        {
            if (hootXmlBuilder != null && hootXmlBuilder.IsBusy)
            {
                tbOutput.Text += "CANCEL PENDING...";
                hootXmlBuilder.CancelAsync();
            }
        }

        #endregion

        #region XSF - EXTRACT COMPRESSED SECTION

        // xsfCompressedProgramExtractor
        private void tbXsfPsf2Exe_Source_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tbXsfPsf2Exe_Source_DragDrop(object sender, DragEventArgs e)
        {
            doCleanup();

            toolStripStatusLabel1.Text = "PSF2EXE...Begin";

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            XsfCompressedProgramExtractorWorker.XsfCompressedProgramExtractorStruct xcpeStruct = 
                new XsfCompressedProgramExtractorWorker.XsfCompressedProgramExtractorStruct();
            xcpeStruct.SourcePaths = s;
            xcpeStruct.includeExtension = cbXsfPsf2Exe_IncludeOrigExt.Checked;
            xcpeStruct.stripGsfHeader = cbXsfPsf2Exe_StripGsfHeader.Checked;

            xsfCompressedProgramExtractor = new XsfCompressedProgramExtractorWorker();
            xsfCompressedProgramExtractor.ProgressChanged += backgroundWorker_ReportProgress;
            xsfCompressedProgramExtractor.RunWorkerCompleted += XsfCompressedProgramExtractorWorker_WorkComplete;
            xsfCompressedProgramExtractor.RunWorkerAsync(xcpeStruct);
        }

        private void XsfCompressedProgramExtractorWorker_WorkComplete(object sender,
                             RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                doCancelCleanup(false);
                toolStripStatusLabel1.Text = "PSF2EXE...Cancelled";
                tbOutput.Text += "Operation cancelled.";
            }
            else
            {
                lblProgressLabel.Text = String.Empty;
                toolStripStatusLabel1.Text = "PSF2EXE...Complete";
            }
        }

        private void btnXsfPsf2Exe_Cancel_Click(object sender, EventArgs e)
        {
            if (xsfCompressedProgramExtractor != null && xsfCompressedProgramExtractor.IsBusy)
            {
                tbOutput.Text += "CANCEL PENDING...";
                xsfCompressedProgramExtractor.CancelAsync();
            }
        }

        #endregion

        #region NDS - SDAT EXTRACTOR

        private void tbNDS_SdatExtractor_Source_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tbNDS_SdatExtractor_Source_DragDrop(object sender, DragEventArgs e)
        {
            doCleanup();

            toolStripStatusLabel1.Text = "SDAT Extraction...Begin";

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            SdatExtractorWorker.SdatExtractorStruct sdexStruct = new SdatExtractorWorker.SdatExtractorStruct();
            sdexStruct.SourcePaths = s;

            sdatExtractorWorker = new SdatExtractorWorker();
            sdatExtractorWorker.ProgressChanged += backgroundWorker_ReportProgress;
            sdatExtractorWorker.RunWorkerCompleted += SdatExtractorWorker_WorkComplete;
            sdatExtractorWorker.RunWorkerAsync(sdexStruct);
        }

        private void SdatExtractorWorker_WorkComplete(object sender,
                     RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                doCancelCleanup(false);
                toolStripStatusLabel1.Text = "SDAT Extraction...Cancelled";
                tbOutput.Text += "Operation cancelled.";
            }
            else
            {
                lblProgressLabel.Text = String.Empty;
                toolStripStatusLabel1.Text = "SDAT Extraction...Complete";
            }
        }

        private void btnSdatExtractor_Cancel_Click(object sender, EventArgs e)
        {
            if (sdatExtractorWorker != null && sdatExtractorWorker.IsBusy)
            {
                tbOutput.Text += "CANCEL PENDING...";
                sdatExtractorWorker.CancelAsync();
            }
        }

        #endregion

        private void btnTestGui_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

    }
}
