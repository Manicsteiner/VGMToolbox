using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.format.iso;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class MicrosoftExFatFileSystem
    {
        public static readonly byte[] STANDARD_IDENTIFIER = new byte[] { 0x45, 0x58, 0x46, 0x41, 0x54, 0x20, 0x20, 0x20 };
        public const uint IDENTIFIER_OFFSET = 0x03;
        public const string FORMAT_DESCRIPTION_STRING = "Microsoft exFAT File System";

        public const uint EXFAT_VERSION_0100 = 0x100;

        public const uint FAT_FIRST_USABLE_CLUSTER = 2;
        public const uint FAT_MEDIA_TYPE = 0xFFFFFFF8;
        public const uint FAT_CELL1_PLACEHOLDER = 0xFFFFFFFF;
        
        public const uint FAT_CELL_EOF = 0xFFFFFFFF;
        public const uint FAT_CELL_NO_CHAIN = 0;
        public const uint FAT_CELL_MAX_VALUE = 0xFFFFFFF7;

        public const byte ROOT_DIR_ENTRY_SIZE = 0X20;

        public const byte ROOT_DIR_ENTRY_EMPTY = 0;
        public const byte ROOT_DIR_ENTRY_TYPE_ALLOCATION_BITMAP = 0x81;
        public const byte ROOT_DIR_ENTRY_TYPE_UPCASE = 0x82;
        public const byte ROOT_DIR_ENTRY_TYPE_VOLUME_LABEL = 0x83;
        public const byte ROOT_DIR_ENTRY_TYPE_FILE_DIRECTORY = 0x85;        

        public const byte ROOT_DIR_ENTRY_TYPE_VOLUME_GUID = 0xA0;
        public const byte ROOT_DIR_ENTRY_TYPE_TEXFAT_PADDING = 0xA1;

        public const byte ROOT_DIR_ENTRY_TYPE_STREAM_EXTENSION_DIRECTORY = 0xC0;
        public const byte ROOT_DIR_ENTRY_TYPE_FILENAME_EXTENSION_DIRECTORY = 0xC1;

        public const byte ROOT_DIR_ENTRY_TYPE_WINDOWS_CE_ACL = 0xE2;




        public const uint CLUSTER_CORRECTION_OFFSET = 2;  // first sector in actual cluster heap is number 2, 
                                                          //   cluster 0 and 1 are reserved (see page 37 in 
                                                          //   Robert Shullich's "Reverse Engineering...[exFAT]"

        public static ulong GetOffsetForClusterId(ulong clusterHeapOffset, uint clusterSize, uint clusterId)
        {
            ulong offset;

            offset = clusterHeapOffset +
                ((clusterId - MicrosoftExFatFileSystem.CLUSTER_CORRECTION_OFFSET) * clusterSize);


            return offset;
        }
    }
    
    public class MicrosoftExFatVolume : IVolume
    {
        public string SourceFileName { set; get; }

        public uint JumpBoot { set; get; }
        public byte[] MagicBytes { set; get; }
        public byte[] ZeroChunk { set; get; }

        public UInt64 PartitionOffset { set; get; }
        public UInt64 VolumeLength { set; get; }
        public uint FatOffset { set; get; }
        public uint FatLength { set; get; }
        public uint ClusterHeapOffset { set; get; }
        public uint ClusterCount { set; get; }
        public uint RootDirectoryFirstCluster { set; get; }

        public uint VolumeSerialNumber { set; get; }
        public ushort FileSystemRevision { set; get; }
        public ushort VolumeFlags { set; get; }

        public byte BytesPerSector { set; get; }
        public byte SectorsPerCluster { set; get; }

        public byte NumberOfFats { set; get; }
        
        // calculated helpers
        public uint SectorSizeInBytes { set; get; }
        public uint ClusterSizeInBytes { set; get; }
        
        public ulong VolumeLengthInBytes { set; get; }
        public ulong FatAbsoluteOffset { set; get; }
        public ulong FatLengthInBytes { set; get; }        
        public ulong ClusterHeapAbsoluteOffset { set; get; }
        public ulong RootDirectoryAbsoluteOffset { set; get; }

        // FAT
        public uint[] FileAllocationTable { set; get; }
        public uint MediaDescriptor { set; get; }
        public uint FatCell1Constant { set; get; }

        #region IVolume

        public ArrayList DirectoryStructureArray { set; get; }
        public IDirectoryStructure[] Directories
        {
            set { Directories = value; }
            get
            {
                DirectoryStructureArray.Sort();
                return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(MicrosoftExFatDirectoryStructure));
            }
        }

        public long VolumeBaseOffset { set; get; }
        public string FormatDescription { set; get; }
        public VolumeDataType VolumeType { set; get; }
        public string VolumeIdentifier { set; get; }
        public bool IsRawDump { set; get; }
      
        public void Initialize(FileStream isoStream, long offset, bool isRawDump)
        {
            this.VolumeBaseOffset = offset;
            this.IsRawDump = isRawDump;
            this.VolumeType = VolumeDataType.Data;
            this.DirectoryStructureArray = new ArrayList();
            this.FormatDescription = MicrosoftExFatFileSystem.FORMAT_DESCRIPTION_STRING;

            this.JumpBoot = ParseFile.ReadUint24LE(isoStream, this.VolumeBaseOffset + 0);
            this.MagicBytes = ParseFile.ParseSimpleOffset(isoStream, this.VolumeBaseOffset + 3, 8);
            this.ZeroChunk = ParseFile.ParseSimpleOffset(isoStream, this.VolumeBaseOffset + 11, 53);

            this.PartitionOffset = ParseFile.ReadUlongLE(isoStream, this.VolumeBaseOffset + 64);
            this.VolumeLength = ParseFile.ReadUlongLE(isoStream, this.VolumeBaseOffset + 72);
            this.FatOffset = ParseFile.ReadUintLE(isoStream, this.VolumeBaseOffset + 80);
            this.FatLength = ParseFile.ReadUintLE(isoStream, this.VolumeBaseOffset + 84);
            this.ClusterHeapOffset = ParseFile.ReadUintLE(isoStream, this.VolumeBaseOffset + 88);
            this.ClusterCount = ParseFile.ReadUintLE(isoStream, this.VolumeBaseOffset + 92);
            this.RootDirectoryFirstCluster = ParseFile.ReadUintLE(isoStream, this.VolumeBaseOffset + 96);

            this.VolumeSerialNumber = ParseFile.ReadUintLE(isoStream, this.VolumeBaseOffset + 100);
            this.VolumeIdentifier = this.VolumeSerialNumber.ToString("X2");

            this.FileSystemRevision = ParseFile.ReadUshortLE(isoStream, this.VolumeBaseOffset + 104);
            this.VolumeFlags = ParseFile.ReadUshortLE(isoStream, this.VolumeBaseOffset + 106);

            this.BytesPerSector = ParseFile.ReadByte(isoStream, this.VolumeBaseOffset + 108);
            this.SectorsPerCluster = ParseFile.ReadByte(isoStream, this.VolumeBaseOffset + 109);

            this.NumberOfFats = ParseFile.ReadByte(isoStream, this.VolumeBaseOffset + 110);

            // caclulate helper values
            this.SectorSizeInBytes = (uint)(1 << this.BytesPerSector);
            this.ClusterSizeInBytes = (uint)(1 << (this.SectorsPerCluster + this.BytesPerSector));
            
            this.VolumeLengthInBytes = this.VolumeLength * this.SectorSizeInBytes;
            this.FatAbsoluteOffset = (ulong)this.VolumeBaseOffset + (this.FatOffset * this.SectorSizeInBytes);
            this.FatLengthInBytes = this.FatLength * this.SectorSizeInBytes;
            this.ClusterHeapAbsoluteOffset = (ulong)this.VolumeBaseOffset + (this.ClusterHeapOffset * this.SectorSizeInBytes);
            this.RootDirectoryAbsoluteOffset =
                MicrosoftExFatFileSystem.GetOffsetForClusterId(this.ClusterHeapAbsoluteOffset, this.ClusterSizeInBytes, this.RootDirectoryFirstCluster); // this.ClusterHeapAbsoluteOffset + ((this.RootDirectoryFirstCluster - MicrosoftExFatFileSystem.CLUSTER_CORRECTION_OFFSET) * this.ClusterSizeInBytes);

            if (this.FileSystemRevision == MicrosoftExFatFileSystem.EXFAT_VERSION_0100)
            {
                // initialize FAT
                this.InitializeAndValidateFat(isoStream);

                // process root directory entry
                this.GetVolumeLabel(isoStream);

                // load directories
                this.LoadDirectories(isoStream);
            }
            else
            {
                MessageBox.Show(String.Format("Unsupported exFAT version: {0}", this.FileSystemRevision.ToString("X8")));
            }

        }

        public void ExtractAll(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            foreach (MicrosoftExFatDirectoryStructure ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }
        }

        #endregion

        public ulong GetMaxDataSizeForFirstCluster(uint firstCluster)
        {
            ulong maxDataSize = 0;
            uint currentCluster = firstCluster;

            do
            {
                maxDataSize += this.ClusterSizeInBytes;
            }
            while ((this.FileAllocationTable[currentCluster] != MicrosoftExFatFileSystem.FAT_CELL_NO_CHAIN) &&
                  (this.FileAllocationTable[currentCluster] != MicrosoftExFatFileSystem.FAT_CELL_EOF) &&
                  (this.FileAllocationTable[currentCluster] <= MicrosoftExFatFileSystem.FAT_CELL_MAX_VALUE));


            return maxDataSize;
        }

        private void InitializeAndValidateFat(FileStream isoStream)
        {
            this.FileAllocationTable = new uint[this.ClusterCount + 2]; // first 2 entries of FAT contain info

            for (ulong i = 0; i < (this.ClusterCount + 2); i++ )
            {
                this.FileAllocationTable[i] = ParseFile.ReadUintLE(isoStream, (long)(this.FatAbsoluteOffset + (i * 4)));

                if (i == 1) // first two cells are populated
                { 
                    if (!((this.FileAllocationTable[0] == MicrosoftExFatFileSystem.FAT_MEDIA_TYPE) &&
                          (this.FileAllocationTable[1] == MicrosoftExFatFileSystem.FAT_CELL1_PLACEHOLDER)))
                    {
                        MessageBox.Show("WARNING: File Allocation Table (FAT) cell check failed.");                       
                    }
                }
            } // for (ulong i = 0; i < (this.ClusterCount + 2); i++ )               
        }

        private void GetVolumeLabel(FileStream isoStream)
        {
            byte[] primaryEntry = new byte[MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE];

            byte entryType = 0xFF;
            long currentOffset = (long)this.RootDirectoryAbsoluteOffset;

            bool volumeLabelProcessed = false;

            do
            {
                // read primary entry
                primaryEntry = ParseFile.ParseSimpleOffset(isoStream, currentOffset, MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE);

                // move pointer
                currentOffset += MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE;
                
                // process primary entry and subentries if needed
                entryType = primaryEntry[0];

                switch (entryType)
                { 
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_VOLUME_LABEL:
                        this.ProcessRootDirectoryVolumeLabelEntry(primaryEntry);
                        volumeLabelProcessed = true;
                        break;
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_FILE_DIRECTORY:
                        // this.ProcessRootDirectoryFileDirectoryEntry(isoStream, ref currentOffset, primaryEntry);
                     
                        // skip the extra info, primaryEntry[1] is the FileDirectoryEntry.SecondaryCount value
                        currentOffset += (uint)(MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE * primaryEntry[1]);                        
                        break;
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_ALLOCATION_BITMAP:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_UPCASE:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_VOLUME_GUID:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_TEXFAT_PADDING:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_WINDOWS_CE_ACL:
                        break;
                    default:
                        break;                                
                } // switch (entryType)                
            }
            while ((!volumeLabelProcessed) && (entryType != MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_EMPTY));        
        }

        private void LoadDirectories(FileStream isoStream)
        {
            MicrosoftExFatDirectoryStructure rootDirectory;
            byte[] primaryEntry = new byte[MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE];

            byte entryType = 0xFF;
            long currentOffset = (long)this.RootDirectoryAbsoluteOffset;
            ulong maxDirectoryLength;

            bool rootDirectoryProcessed = false;

            // @TODO: Updated to use max data size and to jump clusters if needed.
            do
            {
                // read primary entry
                primaryEntry = ParseFile.ParseSimpleOffset(isoStream, currentOffset, MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE);

                // process primary entry and subentries if needed
                entryType = primaryEntry[0];

                switch (entryType)
                {
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_FILE_DIRECTORY:
                        // calculate max size for root directory entries
                        maxDirectoryLength = this.GetMaxDataSizeForFirstCluster(this.RootDirectoryFirstCluster);

                        // build root directory
                        rootDirectory = new MicrosoftExFatDirectoryStructure(isoStream, this.VolumeBaseOffset, (long)this.RootDirectoryAbsoluteOffset, (long)maxDirectoryLength,
                            String.Empty, String.Empty, this.ClusterHeapAbsoluteOffset, this.ClusterSizeInBytes);

                            // add root directory
                        this.DirectoryStructureArray.Add(rootDirectory);

                        // skip the root directory entries, primaryEntry[1] is the FileDirectoryEntry.SecondaryCount value
                        currentOffset += MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE + 
                            (uint)(MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE * primaryEntry[1]);
                        
                        // set flag to indicate directory was processed
                        rootDirectoryProcessed = true;
                        break;
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_VOLUME_LABEL:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_ALLOCATION_BITMAP:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_UPCASE:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_VOLUME_GUID:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_TEXFAT_PADDING:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_WINDOWS_CE_ACL:
                        currentOffset += MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE;    
                        break;
                    default:
                        currentOffset += MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE;
                        break;
                } // switch (entryType)                
            }
            while ((!rootDirectoryProcessed) && (entryType != MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_EMPTY)); // @todo: add max size?
        }

        private void ProcessRootDirectoryVolumeLabelEntry(byte[] volumeLabelEntry)
        {
            byte characterCount = volumeLabelEntry[1];    // chars, not bytes
            int nameLength = characterCount *= 2;         // multiple by 2 to get bytes since these are Unicode Chars

            byte[] labelBytes = ParseFile.SimpleArrayCopy(volumeLabelEntry, 2, nameLength);
            this.VolumeIdentifier = Encoding.Unicode.GetString(labelBytes);        
        }          
    }




    public class MicrosoftExFatRootDirFileDirectoryEntry
    {
        public byte EntryType { set; get; }
        
        public byte SecondaryCount { set; get; }
        public ushort SetChecksum { set; get; }
        public ushort FileAttributes { set; get; }
        public ushort Reserved1 { set; get; }
        
        public uint Create { set; get; }
        public uint LastModified { set; get; }
        public uint LastAccessed { set; get; }

        public byte Create10ms { set; get; }
        public byte LastModified10ms { set; get; }
        
        public byte CreateTZOffset { set; get; }
        public byte LastModifiedTZOffset { set; get; }
        public byte LastAccessTZOffset { set; get; }

        public byte[] Reserved2 { set; get; }

        public bool IsInUse { set; get; }
        public bool IsDirectory { set; get; }

        public MicrosoftExFatRootDirFileDirectoryEntry(byte[] directoryEntry)
        {
            this.EntryType = directoryEntry[0];

            this.SecondaryCount = directoryEntry[1];
            this.SetChecksum = ParseFile.ReadUshortLE(directoryEntry, 2);
            this.FileAttributes = ParseFile.ReadUshortLE(directoryEntry, 4);
            this.Reserved1 = ParseFile.ReadUshortLE(directoryEntry, 6);

            this.Create = ParseFile.ReadUintLE(directoryEntry, 8);
            this.LastModified = ParseFile.ReadUintLE(directoryEntry, 0x0C);
            this.LastAccessed = ParseFile.ReadUintLE(directoryEntry, 0x10);

            this.Create10ms = directoryEntry[0x14];
            this.LastModified10ms = directoryEntry[0x15];

            this.CreateTZOffset = directoryEntry[0x16];
            this.LastModifiedTZOffset = directoryEntry[0x17];
            this.LastAccessTZOffset = directoryEntry[0x18];

            this.Reserved2 = ParseFile.SimpleArrayCopy(directoryEntry, 0x19, 7);

            this.IsInUse = ((this.EntryType & 0x80) == 0x80);
            this.IsDirectory = ((this.FileAttributes & 0x10) == 0x10);        
        }
    
    }

    public class MicrosoftExFatRootDirStreamExtensionEntry
    {
        public byte EntryType { set; get; }
        public byte GeneralSecondaryFlags { set; get; }
        public byte Reserved1 { set; get; }

        public byte NameLength { set; get; }
        public ushort NameHash { set; get; }
        public ushort Reserved2 { set; get; }

        public ulong ValidDataLength { set; get; }
        public uint Reserved3 { set; get; }

        public uint FirstCluster { set; get; }
        public ulong DataLength { set; get; }

        public MicrosoftExFatRootDirStreamExtensionEntry(byte[] streamExtensionEntry)
        {
            this.EntryType = streamExtensionEntry[0];
            this.GeneralSecondaryFlags = streamExtensionEntry[1];
            this.Reserved1 = streamExtensionEntry[2];

            this.NameLength = streamExtensionEntry[3];
            this.NameHash = ParseFile.ReadUshortLE(streamExtensionEntry, 4);
            this.Reserved2 = ParseFile.ReadUshortLE(streamExtensionEntry, 6);

            this.ValidDataLength = ParseFile.ReadUlongLE(streamExtensionEntry, 8);
            this.Reserved3 = ParseFile.ReadUintLE(streamExtensionEntry, 0x10);

            this.FirstCluster = ParseFile.ReadUintLE(streamExtensionEntry, 0x14);
            this.DataLength = ParseFile.ReadUlongLE(streamExtensionEntry, 0x18);                        
        }


    
    }

    public class MicrosoftExFatRootDirFileNameExtensionEntry
    {
        public byte EntryType { set; get; }
        public byte GeneralSecondaryFlags { set; get; }
        public string FileNameChunk { set; get; }

        public MicrosoftExFatRootDirFileNameExtensionEntry(byte[] fileNameExtensionEntry)
        {
            byte[] fileNameChunkBytes;
            
            this.EntryType = fileNameExtensionEntry[0];
            this.GeneralSecondaryFlags = fileNameExtensionEntry[1];

            fileNameChunkBytes = ParseFile.SimpleArrayCopy(fileNameExtensionEntry, 2, 30);
            this.FileNameChunk = Encoding.Unicode.GetString(fileNameChunkBytes);        
        }

    }

    public class MicrosoftExFatFileStructure : IFileStructure
    {
        public string ParentDirectoryName { set; get; }
        public string SourceFilePath { set; get; }
        public string FileName { set; get; }
        public long Offset { set; get; }

        public long VolumeBaseOffset { set; get; }
        public long Lba { set; get; }
        public long Size { set; get; }
        public bool IsRaw { set; get; }
        public CdSectorType FileMode { set; get; }
        public int NonRawSectorSize { set; get; }


        public DateTime FileDateTime { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is MicrosoftExFatFileStructure)
            {
                MicrosoftExFatFileStructure o = (MicrosoftExFatFileStructure)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not an MicrosoftExFatFileStructure");
        }

        public MicrosoftExFatFileStructure(string parentDirectoryName, string sourceFilePath, string fileName, 
            long offset, long volumeBaseOffset, long lba, long size, DateTime fileTime)
        {
            this.ParentDirectoryName = parentDirectoryName;
            this.SourceFilePath = sourceFilePath;
            this.FileName = fileName;
            this.Offset = offset;
            this.Size = size;
            this.FileDateTime = fileTime;

            this.VolumeBaseOffset = volumeBaseOffset;
            this.Lba = lba;
            this.IsRaw = true;
            this.NonRawSectorSize = -1;
            this.FileMode = CdSectorType.Unknown;
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string destinationFile = Path.Combine(Path.Combine(destinationFolder, this.ParentDirectoryName), this.FileName);

            if (!streamCache.ContainsKey(this.SourceFilePath))
            {
                streamCache[this.SourceFilePath] = File.OpenRead(this.SourceFilePath);
            }

            ParseFile.ExtractChunkToFile(streamCache[this.SourceFilePath], this.Offset, this.Size, destinationFile);
        }
    }

   
    
    public class MicrosoftExFatDirectoryStructure : IDirectoryStructure
    {
        public long DirectoryRecordOffset { set; get; }
        public string ParentDirectoryName { set; get; }

        public ArrayList SubDirectoryArray { set; get; }
        public IDirectoryStructure[] SubDirectories
        {
            set { this.SubDirectories = value; }
            get
            {
                SubDirectoryArray.Sort();
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(MicrosoftExFatDirectoryStructure));
            }
        }

        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { this.Files = value; }
            get
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(MicrosoftExFatFileStructure));
            }
        }

        public string SourceFilePath { set; get; }
        public string DirectoryName { set; get; }

        public MicrosoftExFatDirectoryStructure(
            FileStream isoStream, 
            long volumeBaseOffset,
            long directoryOffset,
            long maxDirectoryLength,
            string directoryName, 
            string parentDirectory,
            ulong clusterHeapOffset,
            uint clusterSize)
        {
            string nextDirectory;
            this.SourceFilePath = isoStream.Name;
            this.SubDirectoryArray = new ArrayList();
            this.FileArray = new ArrayList();
            this.DirectoryRecordOffset = directoryOffset;

            if (String.IsNullOrEmpty(parentDirectory))
            {
                this.ParentDirectoryName = String.Empty;
                this.DirectoryName = directoryName;
                nextDirectory = this.DirectoryName;
            }
            else
            {
                this.ParentDirectoryName = parentDirectory;
                this.DirectoryName = directoryName;
                nextDirectory = this.ParentDirectoryName + Path.DirectorySeparatorChar + this.DirectoryName;
            }

            this.processDirectory(isoStream, directoryOffset, maxDirectoryLength, nextDirectory, volumeBaseOffset, clusterHeapOffset, clusterSize);                                
        
        }

        private void processDirectory(
           FileStream isoStream,
           long directoryOffset,
           long maxDirectoryLength,
           string parentDirectory,
           long volumeBaseOffset, 
           ulong clusterHeapOffset,
           uint clusterSize)
        {
            byte[] primaryEntry;
            byte[] secondaryRecord;
            byte entryType = 0xFF;

            MicrosoftExFatDirectoryStructure directory;
            MicrosoftExFatFileStructure newFile;
            MicrosoftExFatDirectoryStructure newDirectory;
            ulong objectOffset;

            MicrosoftExFatRootDirFileDirectoryEntry fileEntry; // = new MicrosoftExFatRootDirFileDirectoryEntry(fileDirectoryEntry);

            MicrosoftExFatRootDirStreamExtensionEntry streamExtensionEntry;
            MicrosoftExFatRootDirFileNameExtensionEntry fileNameExtensionEntry;

            string fileName = String.Empty;
            DateTime fileDate = new DateTime();  // @todo update to actually parse date

            long currentOffset = directoryOffset;
            long maxOffset = directoryOffset + maxDirectoryLength;

            // @TODO: need to handle when in non-contiguous clusters
            while ((currentOffset < maxOffset) && (entryType != MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_EMPTY))
            {
                // read primary entry
                primaryEntry = ParseFile.ParseSimpleOffset(isoStream, currentOffset, MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE);

                // process primary entry and subentries if needed
                entryType = primaryEntry[0];

                switch (entryType)
                {
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_FILE_DIRECTORY:
                        fileEntry = new MicrosoftExFatRootDirFileDirectoryEntry(primaryEntry);
                        currentOffset += MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE;
                        
                        // check if in use and process, and skip if not
                        if (fileEntry.IsInUse)
                        {
                            // process Stream Extension Entry
                            secondaryRecord = ParseFile.ParseSimpleOffset(isoStream, currentOffset, MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE);
                            streamExtensionEntry = new MicrosoftExFatRootDirStreamExtensionEntry(secondaryRecord);
                            objectOffset = MicrosoftExFatFileSystem.GetOffsetForClusterId(clusterHeapOffset, clusterSize, streamExtensionEntry.FirstCluster);
                            
                            currentOffset += MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE;

                            // process File Name Extention Entry
                            fileName = String.Empty;
                            
                            for (int i = 0; i < (fileEntry.SecondaryCount - 1); i++)
                            {
                                secondaryRecord = ParseFile.ParseSimpleOffset(isoStream, (currentOffset), MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE);
                                fileNameExtensionEntry = new MicrosoftExFatRootDirFileNameExtensionEntry(secondaryRecord);

                                // build name from chunks
                                fileName += fileNameExtensionEntry.FileNameChunk;

                                // move pointer
                                currentOffset += MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE;
                            }

                            // trim any excess from name
                            fileName = fileName.Substring(0, streamExtensionEntry.NameLength);
                            
                            // create dir or file and add to array list
                            if (fileEntry.IsDirectory)
                            {
                                // get new directory
                                newDirectory = new MicrosoftExFatDirectoryStructure(isoStream, volumeBaseOffset,
                                    (long)objectOffset, (long)streamExtensionEntry.DataLength, 
                                    fileName, parentDirectory, clusterHeapOffset, clusterSize);

                                // add directory to subdirectory array
                                this.SubDirectoryArray.Add(newDirectory);
                                
                            }
                            else // file
                            {
                                newFile = new MicrosoftExFatFileStructure(parentDirectory, isoStream.Name, fileName, (long)objectOffset,
                                    volumeBaseOffset, streamExtensionEntry.FirstCluster, (long)streamExtensionEntry.DataLength, fileDate);

                                this.FileArray.Add(newFile);
                            }
                        }
                        else
                        {
                            currentOffset = (uint)(MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE * fileEntry.SecondaryCount);
                        }

                        break;
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_VOLUME_LABEL:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_ALLOCATION_BITMAP:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_UPCASE:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_VOLUME_GUID:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_TEXFAT_PADDING:
                    case MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_TYPE_WINDOWS_CE_ACL:
                        currentOffset += MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE;
                        break;
                    default:
                        currentOffset += MicrosoftExFatFileSystem.ROOT_DIR_ENTRY_SIZE;
                        break;
                } // switch (entryType)                

                       
            }











        
        
        
        
        }



        public int CompareTo(object obj)
        {
            if (obj is MicrosoftExFatDirectoryStructure)
            {
                MicrosoftExFatDirectoryStructure o = (MicrosoftExFatDirectoryStructure)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not an MicrosoftExFatDirectoryStructure");
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string fullDirectoryPath = Path.Combine(destinationFolder, Path.Combine(this.ParentDirectoryName, this.DirectoryName));

            // create directory
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            foreach (MicrosoftExFatFileStructure f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (MicrosoftExFatDirectoryStructure d in this.SubDirectoryArray)
            {
                d.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }
        }
    }



}
