using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.format.iso;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class MicrosoftSTFS
    {
        public static readonly byte[] STANDARD_IDENTIFIER = new byte[] { 0x4C, 0x49, 0x56, 0x45 };
        public const uint IDENTIFIER_OFFSET = 0x00;
        public static string FORMAT_DESCRIPTION_STRING = "Microsoft STFS";

        public const int FIRST_BLOCK_OFFSET = 0xC000;
        public const int BLOCK_SIZE = 0x1000;
        public const int HASH_TABLE_INTERVAL1 = 0xAA;
        public const int HASH_TABLE_INTERVAL2 = 0x70E4;

        public const int ROOT_DIRECTORY_PATH_INDICATOR = -1;
    }

    public class MicrosoftSTFSVolume : IVolume
    {
        public struct FileTableEntry
        {
            public string FileName { set; get; }
            public byte Flags { set; get; }
            public int BlocksForFileLE1 { set; get; }
            public int BlocksForFileLE2 { set; get; }
            public int StartingBlockForFileLE { set; get; }
            public short PathIndicator { set; get; }
            public uint FileSize { set; get; }
            public int UpdateDateTime { set; get; }
            public int AccessDateTime { set; get; }

            public bool IsDirectory
            {
                get { return (Flags & 128) == 128; }
            }
            
            public bool StoredInConsecutiveBlocks
            {
                get { return (Flags & 64) == 64; }
            }
        }

        public string SourceFileName { set; get; }

        public long VolumeBaseOffset { set; get; }
        public string FormatDescription { set; get; }
        public VolumeDataType VolumeType { set; get; }
        public string VolumeIdentifier { set; get; }
        public bool IsRawDump { set; get; }

        public uint HeaderSize { set; get; }
        public byte BlockSeparation { set; get; }
        public short FileTableBlockCount { set; get; }
        public int FileTableBlockNumber { set; get; }
        public ArrayList FileTableEntryArray { set; get; }
        public FileTableEntry[] FileTableEntries
        {
            set { FileTableEntries = value; }
            get
            {
                return (FileTableEntry[])FileTableEntryArray.ToArray(typeof(FileTableEntry));
            }           
        }

        public ArrayList DirectoryStructureArray { set; get; }
        public IDirectoryStructure[] Directories
        {
            set { Directories = value; }
            get
            {
                DirectoryStructureArray.Sort();
                return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(MicrosoftSTFSDirectoryStructure));
            }        
        }

        public void Initialize(FileStream isoStream, long offset, bool isRawDump)
        {
            byte[] volumeIdentifierBytes;

            this.SourceFileName = isoStream.Name;
            this.FormatDescription = MicrosoftSTFS.FORMAT_DESCRIPTION_STRING;

            this.VolumeBaseOffset = offset;
            this.IsRawDump = isRawDump;
            this.VolumeType = VolumeDataType.Data;
            this.FileTableEntryArray = new ArrayList();
            this.DirectoryStructureArray = new ArrayList();
           
            // get identifier
            volumeIdentifierBytes = ParseFile.ParseSimpleOffset(isoStream, this.VolumeBaseOffset + 0x1691, 0x80);
            this.VolumeIdentifier = Encoding.BigEndianUnicode.GetString(volumeIdentifierBytes);
            this.VolumeIdentifier = this.VolumeIdentifier.Substring(0, this.VolumeIdentifier.IndexOf('\0'));

            // get header size
            this.HeaderSize = ParseFile.ReadUintBE(isoStream, this.VolumeBaseOffset + 0x340);

            // get file table info
            this.BlockSeparation = ParseFile.ReadByte(isoStream, this.VolumeBaseOffset + 0x37B);
            this.FileTableBlockCount = ParseFile.ReadInt16LE(isoStream, this.VolumeBaseOffset + 0x37C);
            
            // not sure about endianess, always zero in my samples so far
            this.FileTableBlockNumber = ParseFile.ReadInt24LE(isoStream, this.VolumeBaseOffset + 0x37E);
       
            // parse file table
            this.ParseFileTable(isoStream);                            

            // build directory tree
            this.BuildDirectoryTree();
        }
       
        public void ExtractAll(ref Dictionary<string, FileStream> streamCache, string destintionFolder, bool extractAsRaw)
        {
            foreach (MicrosoftSTFSDirectoryStructure ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, destintionFolder, extractAsRaw);
            }
        }

        public void ParseFileTable(FileStream isoStream)
        {
            FileTableEntry tableEntry = new FileTableEntry();
            long fileTableMinOffset;
            long fileTableMaxOffset;

            long fileSize = isoStream.Length;

            // get offset for blocks
            fileTableMinOffset = ComputeBlockNumberAndGetOffset(this.FileTableBlockNumber, this.HeaderSize, this.BlockSeparation);
            fileTableMaxOffset = ComputeBlockNumberAndGetOffset(this.FileTableBlockNumber + this.FileTableBlockCount, this.HeaderSize, this.BlockSeparation);
            
            // check offsets
            if ((fileTableMinOffset >= MicrosoftSTFS.FIRST_BLOCK_OFFSET) &&
                (fileTableMinOffset < fileTableMaxOffset) &&
                (fileTableMaxOffset < fileSize))
            {
                for (long i = fileTableMinOffset; i < fileTableMaxOffset; i += 0x40)
                {
                    tableEntry.FileName = ParseFile.ReadAsciiString(isoStream, i);

                    if (!String.IsNullOrEmpty(tableEntry.FileName))
                    {
                        tableEntry.Flags = ParseFile.ReadByte(isoStream, i + 0x28);
                        tableEntry.BlocksForFileLE1 = ParseFile.ReadInt24LE(isoStream, i + 0x29);
                        tableEntry.BlocksForFileLE2 = ParseFile.ReadInt24LE(isoStream, i + 0x2C);
                        tableEntry.StartingBlockForFileLE = ParseFile.ReadInt24LE(isoStream, i + 0x2F);
                        tableEntry.PathIndicator = ParseFile.ReadInt16BE(isoStream, i + 0x32);
                        tableEntry.FileSize = ParseFile.ReadUintBE(isoStream, i + 0x34);
                        tableEntry.UpdateDateTime = ParseFile.ReadInt32BE(isoStream, i + 0x38);
                        tableEntry.AccessDateTime = ParseFile.ReadInt32BE(isoStream, i + 0x3C);

                        FileTableEntryArray.Add(tableEntry);
                    }
                }

            }
            else
            {
                throw new IndexOutOfRangeException("File Table block IDs do not make sense.");
            }
        }

        public void BuildDirectoryTree()
        {
            Dictionary<int, MicrosoftSTFSDirectoryStructure> directoryList = new Dictionary<int, MicrosoftSTFSDirectoryStructure>();            

            FileTableEntry fileTableEntry = new FileTableEntry();
            MicrosoftSTFSDirectoryStructure parentDirectoryStructure;
            MicrosoftSTFSDirectoryStructure directoryStructure;
            MicrosoftSTFSFileStructure fileStructure;
            
            String parentDirectoryName = String.Empty;

            ArrayList keyListArray = new ArrayList();
            int[] keyList;

            //------------------
            // add root folder
            //------------------
            directoryStructure = new MicrosoftSTFSDirectoryStructure(this.SourceFileName, String.Empty, String.Empty, MicrosoftSTFS.ROOT_DIRECTORY_PATH_INDICATOR);
            directoryList.Add(MicrosoftSTFS.ROOT_DIRECTORY_PATH_INDICATOR, directoryStructure);

            //----------------------
            // build directory list
            //----------------------
            for (int i = 0; i < FileTableEntries.Length; i++)
            {
                fileTableEntry = FileTableEntries[i];

                // build parent directory name
                parentDirectoryName = Path.Combine(directoryList[fileTableEntry.PathIndicator].ParentDirectoryName,
                    directoryList[fileTableEntry.PathIndicator].DirectoryName);
                
                if (fileTableEntry.IsDirectory)
                {
                    // create directory                    
                    directoryStructure =
                        new MicrosoftSTFSDirectoryStructure(this.SourceFileName,
                            fileTableEntry.FileName, parentDirectoryName, fileTableEntry.PathIndicator);

                    // add to list
                    directoryList.Add(i, directoryStructure);
                }
                else
                { 
                    // create file structure
                    fileStructure = new MicrosoftSTFSFileStructure(
                        parentDirectoryName,this.SourceFileName, 
                        fileTableEntry.FileName, this.VolumeBaseOffset,
                        fileTableEntry.StartingBlockForFileLE,
                        fileTableEntry.FileSize, 
                        ByteConversion.GetDateTimeFromFAT32Date(fileTableEntry.UpdateDateTime),
                        fileTableEntry.BlocksForFileLE1,
                        this.HeaderSize, this.BlockSeparation);

                    // add to directory
                    directoryList[fileTableEntry.PathIndicator].FileArray.Add(fileStructure);
                }
            } // for (int i = 0; i < FileTableEntries.Length; i++)

            //-----------------------
            // assemble directories
            //-----------------------

            // build reversed keyList
            foreach (int key in directoryList.Keys)
            {
                keyListArray.Add(key);
            }

            keyListArray.Sort();
            keyList = (int[])keyListArray.ToArray(typeof(int));            

            // loop through dirs backwards
            for (int i = keyList.GetUpperBound(0); i > 0; i--)
            {
                // add to sub directory of parent
                directoryStructure = directoryList[keyList[i]];
                parentDirectoryStructure = directoryList[directoryStructure.ParentDirectoryId];
                parentDirectoryStructure.SubDirectoryArray.Add(directoryStructure);

                // remove from directoryList
                directoryList.Remove(keyList[i]);
            }
            
            // Add root element to Volumes directory list
            this.DirectoryStructureArray.Add(directoryList[MicrosoftSTFS.ROOT_DIRECTORY_PATH_INDICATOR]);
        }

        // from Free60 wiki
        public static long ComputeBlockNumberAndGetOffset(int xBlock, uint headerSize, byte blockSeparation)
        {            
            int adjustedBlockNumber = ComputeDataBlockNumber(xBlock, headerSize, blockSeparation);
            long offset = BlockToOffset(adjustedBlockNumber, headerSize);

            return offset;
        }

        // from Free60 wiki
        public static int ComputeDataBlockNumber(int xBlock, uint headerSize, byte blockSeparation)
        {
            int xBlockShift;

            if (((headerSize + 0xFFF) & 0xF000) == 0xB000)
            {
                xBlockShift = 1;
            }
            else
            {
                if ((blockSeparation & 1) == 1)
                {
                    xBlockShift = 0;
                }
                else
                {
                    xBlockShift = 1;
                }
            }

            int xBase = ((xBlock + 0xAA) / 0xAA);

            //if (this.Header.Magic == XContent_Header.Header_Magic.CON)
            //    xBase = (xBase << xBlockShift);
            int xReturn = (xBase + xBlock);

            if (xBlock > 0xAA)
            {
                xBase = ((xBlock + 0x70E4) / 0x70E4);
                //if (this.Header.Magic == XContent_Header.Header_Magic.CON)
                //    xBase = (xBase << xBlockShift);
                xReturn += xBase;

                if (xBlock > 0x70E4)
                {
                    xBase = ((xBlock + 0x4AF768) / 0x4AF768);
                    //if (this.Header.Magic == xBlockShift)
                    //    xBase = (xBase << 1);

                    xReturn = (xReturn + xBase);
                }
            }

            return xReturn;
        }

        // from Free60 wiki
        public static long BlockToOffset(int xBlock, uint headerSize)
        {
            long xReturn = 0;

            if (xBlock > 0xFFFFFF)
            {
                xReturn = -1;
            }
            else
            {
                xReturn = (((headerSize + 0xFFF) & 0xF000) + (xBlock << 12));
            }

            return xReturn;
        }
    }

    public class MicrosoftSTFSDirectoryStructure : IDirectoryStructure
    {
        public string SourceFilePath { set; get; }
        public string DirectoryName { set; get; }
        public string ParentDirectoryName { set; get; }

        public ArrayList SubDirectoryArray { set; get; }
        public IDirectoryStructure[] SubDirectories
        {
            set { SubDirectories = value; }
            get
            {
                SubDirectoryArray.Sort();
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(MicrosoftSTFSDirectoryStructure));
            }
        }
        
        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { Files = value; }
            get 
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(MicrosoftSTFSFileStructure));
            }
        }

        public int ParentDirectoryId { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is MicrosoftSTFSDirectoryStructure)
            {
                MicrosoftSTFSDirectoryStructure o = (MicrosoftSTFSDirectoryStructure)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not a MicrosoftSTFSDirectoryStructure");
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, 
            string destinationFolder, bool extractAsRaw)
        {
            string fullDirectoryPath = Path.Combine(destinationFolder, Path.Combine(this.ParentDirectoryName, this.DirectoryName));

            // create directory
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            foreach (MicrosoftSTFSFileStructure f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (MicrosoftSTFSDirectoryStructure d in this.SubDirectoryArray)
            {
                d.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }        
        }

        public MicrosoftSTFSDirectoryStructure(string sourceFilePath, string directoryName,
            string parentDirectoryName, int parentDirectoryId)
        {
            this.SourceFilePath = sourceFilePath;
            this.SubDirectoryArray = new ArrayList();
            this.FileArray = new ArrayList();

            this.ParentDirectoryName = parentDirectoryName;
            this.DirectoryName = directoryName;
            this.ParentDirectoryId = parentDirectoryId;
        }

    }

    public class MicrosoftSTFSFileStructure : IFileStructure
    {
        public string ParentDirectoryName { set; get; }
        public string SourceFilePath { set; get; }
        public string FileName { set; get; }

        public long VolumeBaseOffset { set; get; }
        public long Lba { set; get; }
        public long Size { set; get; }
        public bool IsRaw { set; get; }
        public CdSectorType FileMode { set; get; }
        public int NonRawSectorSize { set; get; }

        public DateTime FileDateTime { set; get; }

        //public bool StoredInConsecutiveBlocks { set; get; }
        public int BlocksForFile { set; get; }
        
        public uint FileHeaderSize { set; get; }
        public byte BlockSeparation { set; get; }


        public int CompareTo(object obj)
        {
            if (obj is MicrosoftSTFSFileStructure)
            {
                MicrosoftSTFSFileStructure o = (MicrosoftSTFSFileStructure)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not an MicrosoftSTFSFileStructure");
        }

        public MicrosoftSTFSFileStructure(string parentDirectoryName, 
            string sourceFilePath, string fileName, long volumeBaseOffset, long lba,
            long size, DateTime fileTime, int blocksForFile, 
            uint headerSize, byte blockSeparation)
        {
            this.ParentDirectoryName = parentDirectoryName;
            this.SourceFilePath = sourceFilePath;
            this.FileName = fileName;
            this.VolumeBaseOffset = volumeBaseOffset;
            this.Lba = lba;
            this.IsRaw = false;
            this.NonRawSectorSize = -1;
            this.Size = size;
            this.FileMode = CdSectorType.Unknown;
            this.FileDateTime = fileTime;
            
            //this.StoredInConsecutiveBlocks = storedInConsecutiveBlocks;
            this.BlocksForFile = blocksForFile;
            this.FileHeaderSize = headerSize;
            this.BlockSeparation = blockSeparation;
        }

        public virtual void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            long bytesWritten = 0;
            long offset;
            long chunkSize;
            long currentBlock = this.Lba;

            byte[] buffer = new byte[MicrosoftSTFS.BLOCK_SIZE];
            int bytesRead;

            string destinationFile = Path.Combine(Path.Combine(destinationFolder, this.ParentDirectoryName), this.FileName);
            string fullDestinationPath = Path.GetDirectoryName(destinationFile);

            if (!streamCache.ContainsKey(this.SourceFilePath))
            {
                streamCache[this.SourceFilePath] = File.OpenRead(this.SourceFilePath);
            }

            // create destination folder, if needed
            if (!Directory.Exists(fullDestinationPath))
            {
                Directory.CreateDirectory(fullDestinationPath);
            }

            using (FileStream outStream = File.Open(destinationFile, System.IO.FileMode.Create, FileAccess.Write))
            {
                // extract to disk
                for (int i = 0; i < this.BlocksForFile; i++)
                {
                    // set current block for reading source data
                    currentBlock = this.Lba + i;
                    
                    // set read size
                    if (i == (this.BlocksForFile - 1))
                    {
                        // last chunk may not use entire block
                        chunkSize = (this.Size - bytesWritten);
                    }
                    else
                    {
                        chunkSize = MicrosoftSTFS.BLOCK_SIZE;
                    }

                    // read from stream
                    offset = MicrosoftSTFSVolume.ComputeBlockNumberAndGetOffset((int)currentBlock, this.FileHeaderSize, this.BlockSeparation);
                    streamCache[this.SourceFilePath].Position = offset;
                    bytesRead = streamCache[this.SourceFilePath].Read(buffer, 0, (int)chunkSize);

                    // write to file
                    outStream.Write(buffer, 0, bytesRead);

                    // increment bytes written
                    bytesWritten += bytesRead;
                }               
            }
        }
    }
}
