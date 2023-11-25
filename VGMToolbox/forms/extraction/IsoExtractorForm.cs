using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using VGMToolbox.format.iso;
using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.tools.extract;

namespace VGMToolbox.forms.extraction
{
    public partial class IsoExtractorForm : AVgmtForm
    {
        private static readonly string ICON_PATH =
            Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources");
        
        protected TreeNode selectedNode;
        protected TreeNode oldNode;

        protected ListViewItem selectedListViewItem;
        protected ListViewItem oldListViewItem;

        protected string sourceFile;
        protected string destinationFolder;
        protected bool isTreeSelected;

        private Font listViewBoldFont;

        private ListViewColumnSorter lvwColumnSorter;

        public IsoExtractorForm(TreeNode pTreeNode)
            : base(pTreeNode) 
        {
            InitializeComponent();

            this.lvwColumnSorter = new ListViewColumnSorter();
            this.fileListView.ListViewItemSorter = this.lvwColumnSorter;
            this.listViewBoldFont = new Font(this.fileListView.Font, FontStyle.Bold);

            this.lblTitle.Text = "ISO and Archive Browser/Extractor";
            
            this.tbOutput.Text = "Browse and Extract contents of disc images and archives." + Environment.NewLine;
            this.tbOutput.Text += "- Currently supported image types: .3DS, .CUE/.BIN, .GDI/ISO/BIN, .IMG, .ISO, .MDF, .WUD" + Environment.NewLine;
            this.tbOutput.Text += "- Currently supported file systems: 3DS NCSD/CTR, GameCube, Green Book (CD-i), ISO 9660 (PC/PSX/PS2), NullDC GDI (Dreamcast), Opera FS (3DO), Wii Optical Disc, Wii U Optical Disc, XDVDFS (XBOX/XBOX360)" + Environment.NewLine;
            //this.tbOutput.Text += "- Currently supported archive types: CRI CPK, Microsoft STFS (XBLA), Nintendo U8 Archives (Wii)" + Environment.NewLine;
            this.tbOutput.Text += "- Currently supported archive types: CRI CPK, Nintendo U8 Archives (Wii)" + Environment.NewLine;
            this.tbOutput.Text += String.Format("- WII images require 'ckey.bin' and/or 'kkey.bin' (for Korean discs) in the <{0}> directory.", NintendoWiiOpticalDisc.WII_EXTERNAL_FOLDER) + Environment.NewLine;
            this.tbOutput.Text += String.Format("- WII U images require 'ckey.bin' in the <{0}> directory, and 'disckey.bin' or '<iso name>.key' in the same directory as the source .wud image file.", NintendoWiiUOpticalDisc.WIIU_EXTERNAL_FOLDER) + Environment.NewLine;
            this.tbOutput.Text += "- Not yet supported/tested: .CCD, .MDS style cue sheets; Redbook Audio Extraction" + Environment.NewLine;
            
            this.btnDoTask.Hide();

            this.SetupImageList();
        }

        private void SetupImageList()
        {
            ImageList images = new ImageList();

            // CD
            images.Images.Add("emptyCd", (Image)new Bitmap(Path.Combine(ICON_PATH, "emptycd.ico")));
            images.Images.Add("audioCd", (Image)new Bitmap(Path.Combine(ICON_PATH, "audioCd.ico")));

            // folder
            images.Images.Add("normalFolder", (Image)new Bitmap(Path.Combine(ICON_PATH, "normalFolder.ico")));
            images.Images.Add("openFolder", (Image)new Bitmap(Path.Combine(ICON_PATH, "openFolder.ico")));

            // folder
            images.Images.Add("genericFile", (Image)new Bitmap(Path.Combine(ICON_PATH, "genericFile.ico")));

            IsoFolderTreeView.ImageList = images;
            fileListView.SmallImageList = images;
            fileListView.LargeImageList = images;
        }

        protected override void doDragEnter(object sender, DragEventArgs e)
        {
            base.doDragEnter(sender, e);
        }

        protected override IVgmtBackgroundWorker getBackgroundWorker()
        {
            return new IsoExtractorWorker();
        }
        protected override string getCancelMessage()
        {
            return "Extract ISO...Cancelled.";
        }
        protected override string getCompleteMessage()
        {
            return "Extract ISO...Complete.";
        }
        protected override string getBeginMessage()
        {
            return "Extract ISO...Begin.";
        }

        private void IsoExtractorForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            IsoExtractorWorker.IsoExtractorStruct taskStruct = 
                new IsoExtractorWorker.IsoExtractorStruct();
            taskStruct.SourcePaths = s;
                        
            try
            {
                IVolume[] volumes = IsoExtractorWorker.GetVolumes(s[0]);
                
                this.IsoFolderTreeView.Nodes.Clear();
                this.fileListView.Items.Clear();
                
                LoadTreeWithVolumes(volumes);
                this.sourceFile = s[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void LoadTreeWithVolumes(IVolume[] volumes)
        {
            TreeNode volumeNode;
            TreeNode directoryNode;
            string volumeName;
           
            foreach (IVolume v in volumes)
            {
                volumeName = String.IsNullOrEmpty(v.VolumeIdentifier) ? "NO_NAME" : v.VolumeIdentifier.Trim();
                volumeName += String.Format(" ({0})", v.FormatDescription);
                volumeNode = new TreeNode(volumeName);

                if (v.VolumeType == VolumeDataType.Audio)
                {
                    volumeNode.ImageKey = "audioCd";
                    volumeNode.SelectedImageKey = "audioCd";
                }
                else
                {
                    volumeNode.ImageKey = "emptyCd";
                    volumeNode.SelectedImageKey = "emptyCd";
                }

                if (v.Directories != null)
                {
                    foreach (IDirectoryStructure d in v.Directories)
                    {
                        directoryNode = GetDirectoryNode(d);
                        directoryNode.Text = "\\";
                        volumeNode.Nodes.Add(directoryNode);
                    }
                }

                IsoFolderTreeView.Nodes.Add(volumeNode);
            }
        }

        private TreeNode GetDirectoryNode(IDirectoryStructure directory)
        {
            TreeNode directoryNode;
            TreeNode subDirectoryNode;

            directoryNode = new TreeNode(directory.DirectoryName);
            directoryNode.Tag = directory;
            directoryNode.ImageKey = "normalFolder";
            directoryNode.SelectedImageKey = "openFolder";

            foreach (IDirectoryStructure d in directory.SubDirectories)
            {
                subDirectoryNode = GetDirectoryNode(d);
                directoryNode.Nodes.Add(subDirectoryNode);
            }
            
            return directoryNode;
        }

        private TreeNode GetFileNode(IFileStructure file)
        {
            TreeNode fileNode = new TreeNode(file.FileName);
            fileNode.Tag = file;

            return fileNode;
        }

        private void IsoFolderTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ListViewItem directoryItem;
            ListViewItem fileItem;
            ListViewItem.ListViewSubItem fileExtensionItem;
            ListViewItem.ListViewSubItem offsetItem;
            ListViewItem.ListViewSubItem sizeItem;
            ListViewItem.ListViewSubItem dateItem;

            long fileCount = 0;
            string fileExtensionString;

            if (this.IsoFolderTreeView.SelectedNode != null)
            {
                IDirectoryStructure dirStructure = (IDirectoryStructure)this.IsoFolderTreeView.SelectedNode.Tag;

                this.fileListView.Items.Clear();
                this.fileListView.EndUpdate();

                if (dirStructure != null &&
                    dirStructure.Files != null)
                {                   
                    foreach (IDirectoryStructure d in dirStructure.SubDirectories)
                    {
                        directoryItem = new ListViewItem(d.DirectoryName);
                        directoryItem.Tag = d;
                        directoryItem.ImageKey = "normalFolder";

                        this.fileListView.Items.Add(directoryItem);
                    }

                    this.fileListView.BeginUpdate();

                    foreach (IFileStructure f in dirStructure.Files)
                    {
                        fileItem = new ListViewItem(f.FileName);
                        fileExtensionString = Path.GetExtension(f.FileName);
                        fileExtensionString = fileExtensionString.Length > 0 ? fileExtensionString.Substring(1) : fileExtensionString;
                        fileExtensionItem = new ListViewItem.ListViewSubItem(fileItem, fileExtensionString);
                        offsetItem = new ListViewItem.ListViewSubItem(fileItem, String.Format("{0}", f.Lba.ToString()));
                        sizeItem = new ListViewItem.ListViewSubItem(fileItem, f.Size.ToString());
                        dateItem = new ListViewItem.ListViewSubItem(fileItem, f.FileDateTime.ToString());
                        
                        fileItem.Tag = f;
                        fileItem.ImageKey = "genericFile";

                        // make "interesting" files bold
                        if ((f.FileMode == CdSectorType.Mode2Form2) || 
                            (f.FileMode == CdSectorType.Audio) ||
                            (f.FileMode == CdSectorType.XaInterleaved))
                        { 
                            //fileItem.Font = this.listViewBoldFont;
                            fileItem.ForeColor = Color.DarkGreen;
                        }

                        fileItem.SubItems.Add(fileExtensionItem);
                        fileItem.SubItems.Add(offsetItem);
                        fileItem.SubItems.Add(sizeItem);
                        fileItem.SubItems.Add(dateItem);

                        
                        // refresh page for huge sets
                        if (fileCount % 300 == 0)
                        {
                            this.fileListView.EndUpdate();
                            // Application.DoEvents();
                            this.fileListView.BeginUpdate();
                        }
                        
                        
                        this.fileListView.Items.Add(fileItem);

                        fileCount++;                        
                    }

                    this.fileListView.EndUpdate();
                }                
            }
        }

        private void IsoFolderTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            isTreeSelected = true;
            
            if (e.Button == MouseButtons.Right)
            {
                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                TreeNode node = IsoFolderTreeView.GetNodeAt(p);
                this.selectedNode = node;
                this.oldNode = IsoFolderTreeView.SelectedNode;
                IsoFolderTreeView.SelectedNode = node;

                if (node != null && node.Tag != null)
                {
                    if ((node.Tag is IFileStructure) || 
                        (node.Tag is IDirectoryStructure))
                    {
                        contextMenuStrip1.Show(IsoFolderTreeView, p);
                    }
                }
            }
        }

        private void fileListView_MouseUp(object sender, MouseEventArgs e)
        {
            bool showList = true;
            isTreeSelected = false;
            
            if (e.Button == MouseButtons.Right)
            {
                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                if (this.fileListView.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem lvi in this.fileListView.SelectedItems)
                    {
                        if (lvi.Tag == null)
                        {
                            showList = false;
                            break;
                        }
                    }
                }
                else
                {
                    showList = false;
                }

                if (showList)
                {
                    contextMenuStrip1.Show(this.fileListView, p);
               }
            }
        }

        private void extractToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string destinationFolder = base.browseForFolder(sender, e);

            if (!String.IsNullOrEmpty(destinationFolder))
            {
                this.extractFiles(destinationFolder, false);
            }
        }

        private void extractToSubfolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string destinationFolder =
                Path.Combine(Path.GetDirectoryName(this.sourceFile), String.Format("{0}_isodump", Path.GetFileNameWithoutExtension(this.sourceFile)));

            this.extractFiles(destinationFolder, false);
        }

        private void extractFiles(string destinationFolder, bool extractAsRaw)
        {
            ArrayList files = new ArrayList();
            ArrayList directories = new ArrayList();

            if (this.isTreeSelected &&
                this.IsoFolderTreeView.SelectedNode.Tag != null &&
                this.IsoFolderTreeView.SelectedNode.Tag is IDirectoryStructure)
            {
                directories.Add(this.IsoFolderTreeView.SelectedNode.Tag);
            }
            else if (!this.isTreeSelected)
            {
                foreach (ListViewItem lvi in this.fileListView.SelectedItems)
                {
                    if (lvi.Tag != null)
                    {
                        if (lvi.Tag is IFileStructure)
                        {
                            files.Add(lvi.Tag);
                        }
                        else if (lvi.Tag is IDirectoryStructure)
                        {
                            directories.Add(lvi.Tag);
                        }
                    }

                }
            }

            IsoExtractorWorker.IsoExtractorStruct taskStruct = 
                new IsoExtractorWorker.IsoExtractorStruct();
            taskStruct.SourcePaths = new string[] { this.sourceFile };           
            taskStruct.DestinationFolder = destinationFolder;
            taskStruct.ExtractAsRaw = extractAsRaw;
            taskStruct.Files = (IFileStructure[])files.ToArray(typeof(IFileStructure));
            taskStruct.Directories = (IDirectoryStructure[])directories.ToArray(typeof(IDirectoryStructure));

            base.backgroundWorker_Execute(taskStruct);
        }

        private void fileListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.fileListView.Sort();
        }

        private void openSelectedFolder()
        {
            if (this.fileListView.SelectedItems.Count == 1)
            {
                if (this.fileListView.SelectedItems[0].Tag is IDirectoryStructure)
                {
                    IDirectoryStructure clickedDirectory = (IDirectoryStructure)this.fileListView.SelectedItems[0].Tag;

                    // find the item in the tree and select it
                    foreach (TreeNode t in this.IsoFolderTreeView.SelectedNode.Nodes)
                    {
                        if (((IDirectoryStructure)t.Tag).Equals(clickedDirectory))
                        {
                            this.IsoFolderTreeView.SelectedNode = t;
                            t.Expand();
                        }
                    }
                }

            }
        
        }

        private void fileListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.openSelectedFolder();
        }

        private void fileListView_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                this.openSelectedFolder();
            }
        }

        private void extractRAWToSubfolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string destinationFolder =
                Path.Combine(Path.GetDirectoryName(this.sourceFile), String.Format("{0}_isodump", Path.GetFileNameWithoutExtension(this.sourceFile)));

            this.extractFiles(destinationFolder, true);
        }

        private void extractRAWToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string destinationFolder = base.browseForFolder(sender, e);

            if (!String.IsNullOrEmpty(destinationFolder))
            {
                this.extractFiles(destinationFolder, true);
            }
        }

        private void fileListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            int x = 1;
        }
    }
}
