using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VGMToolbox.util;

namespace VGMToolbox.format.iso
{
    public class Panasonic3do
    {
        public static readonly byte[] STANDARD_IDENTIFIER =
            new byte[] { 0x01, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x01, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x43, 0x44, 0x2D, 0x52, 0x4F, 0x4D, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public static readonly byte[] STANDARD_IDENTIFIER2 =
            new byte[] { 0x01, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x01, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,                          
                         0x63, 0x64, 0x2D, 0x72, 0x6F, 0x6D, 0x00, 0x00,                          
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public static readonly byte[] STANDARD_IDENTIFIER3 =
            new byte[] { 0x01, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x01, 0x09, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x63, 0x64, 0x2D, 0x72, 0x6F, 0x6D, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public static uint SECTOR_SIZE = 0x800;
        public static string FORMAT_DESCRIPTION_STRING = "Opera FS";        
    }

    public class Panasonic3doVolume : IVolume
        {
            public string VolumeIdentifier { set; get; }
            public string FormatDescription { set; get; }
            public VolumeDataType VolumeType { set; get; }
            public long VolumeBaseOffset { set; get; }
            public bool IsRawDump { set; get; }
            public ArrayList DirectoryStructureArray { set; get; }
            public IDirectoryStructure[] Directories
            {
                set { Directories = value; }
                get
                {
                    DirectoryStructureArray.Sort();
                    return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(Panasonic3doDirectoryStructure));
                }
            }

            public uint RootDirectoryCount { set; get; }
            public uint[] RootDirectoryLbas { set; get; }
            
            public DateTime ImageCreationTime { set; get; }

            public void Initialize(FileStream isoStream, long offset, bool isRawDump)
            {                
                byte[] rootSector;

                this.VolumeBaseOffset = offset;
                this.FormatDescription = Panasonic3do.FORMAT_DESCRIPTION_STRING;
                this.VolumeType = VolumeDataType.Data;
                this.IsRawDump = isRawDump;
                this.DirectoryStructureArray = new ArrayList();

                rootSector = CdRom.GetSectorByLba(isoStream, this.VolumeBaseOffset, 0, this.IsRawDump, (int)Panasonic3do.SECTOR_SIZE);
                rootSector = CdRom.GetDataChunkFromSector(rootSector, this.IsRawDump);

                this.RootDirectoryCount = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(rootSector, 0x60, 4)) + 1;
                this.RootDirectoryLbas = new uint[this.RootDirectoryCount];

                for (uint i = 0; i < this.RootDirectoryCount; i++)
                { 
                    this.RootDirectoryLbas[i] = 
                        ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(rootSector, (int)(0x64 + (i * 4)), 4));
                }

                this.LoadDirectories(isoStream);       
            }

            public void ExtractAll(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
            {
                foreach (Panasonic3doDirectoryStructure ds in this.DirectoryStructureArray)
                {
                    ds.Extract(ref streamCache, destinationFolder, extractAsRaw);
                }
            }

            public void LoadDirectories(FileStream isoStream)
            {
                Panasonic3doDirectoryStructure rootDirectory;
                
                //for (uint i = 0; i < this.RootDirectoryCount; i++)
                //{
                    rootDirectory = new Panasonic3doDirectoryStructure(isoStream,
                        isoStream.Name, new DateTime(), this.VolumeBaseOffset,
                        this.RootDirectoryLbas[0], Panasonic3do.SECTOR_SIZE,
                        String.Empty, String.Empty, this.IsRawDump, (int)Panasonic3do.SECTOR_SIZE);

                    this.DirectoryStructureArray.Add(rootDirectory);
                //}                                
            }
        }

    public class Panasonic3doFileStructure : IFileStructure
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

        public int CompareTo(object obj)
        {
            if (obj is Panasonic3doFileStructure)
            {
                Panasonic3doFileStructure o = (Panasonic3doFileStructure)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not an Panasonic3doFileStructure");
        }

        public Panasonic3doFileStructure(string parentDirectoryName, string sourceFilePath, string fileName, long volumeBaseOffset, uint lba, long size, bool isRaw, int nonRawSectorSize, DateTime fileTime)
        { 
            this.ParentDirectoryName = parentDirectoryName;
            this.SourceFilePath = sourceFilePath;
            this.FileName = fileName;
            this.VolumeBaseOffset = volumeBaseOffset;
            this.Lba = lba;
            this.IsRaw = isRaw;
            this.NonRawSectorSize = nonRawSectorSize;
            this.Size = size;
            this.FileMode = CdSectorType.Unknown;
            this.FileDateTime = fileTime;
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string destinationFile = Path.Combine(Path.Combine(destinationFolder, this.ParentDirectoryName), this.FileName);

            if (!streamCache.ContainsKey(this.SourceFilePath))
            {
                streamCache[this.SourceFilePath] = File.OpenRead(this.SourceFilePath);
            }

            CdRom.ExtractCdData(streamCache[this.SourceFilePath], destinationFile, this.VolumeBaseOffset, this.Lba, this.Size, this.IsRaw, this.NonRawSectorSize, this.FileMode, extractAsRaw);
        }
    }

    public class Panasonic3doDirectoryStructure : IDirectoryStructure
    {
        // public Panasonic3doDirectoryRecord ParentDirectoryRecord { set; get; }
        public long DirectoryRecordLba { set; get; }
        public string ParentDirectoryName { set; get; }

        public ArrayList SubDirectoryArray { set; get; }
        public IDirectoryStructure[] SubDirectories
        {
            set { this.SubDirectories = value; }
            get
            {
                SubDirectoryArray.Sort();
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(Panasonic3doDirectoryStructure));
            }
        }

        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { this.Files = value; }
            get
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(Panasonic3doFileStructure));
            }
        }

        public string SourceFilePath { set; get; }
        public string DirectoryName { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is Panasonic3doDirectoryStructure)
            {
                Panasonic3doDirectoryStructure o = (Panasonic3doDirectoryStructure)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not an Panasonic3doDirectoryStructure");
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string fullDirectoryPath = Path.Combine(destinationFolder, Path.Combine(this.ParentDirectoryName, this.DirectoryName));

            // create directory
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            foreach (Panasonic3doFileStructure f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (Panasonic3doDirectoryStructure d in this.SubDirectoryArray)
            {
                d.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }
        }

        public Panasonic3doDirectoryStructure(FileStream isoStream, 
            string sourceFilePath, DateTime creationDateTime, 
            long baseOffset, long directoryLba, uint logicalBlockSize,
            string directoryName, string parentDirectory, 
            bool isRaw, int nonRawSectorSize)
        {
            string nextDirectory;
            this.SourceFilePath = sourceFilePath;
            this.SubDirectoryArray = new ArrayList();
            this.FileArray = new ArrayList();
            this.DirectoryRecordLba = directoryLba;

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

            this.parseDirectoryRecord(isoStream, creationDateTime, baseOffset, directoryLba, logicalBlockSize, nextDirectory, isRaw, nonRawSectorSize);
        }

        
        private void parseDirectoryRecord(
            FileStream isoStream,
            DateTime creationDateTime,
            long baseOffset,
            long directoryLba,
            uint logicalBlockSize,
            string parentDirectory,
            bool isRaw,
            int nonRawSectorSize)
        {
            uint recordSize;
            byte[] directorySector;
            byte[] directoryRecordBytes;
            uint directoryRecordLength;
            uint bytesRead;
            uint currentOffset;
            long currentLba = directoryLba;

            Panasonic3doDirectoryRecord tempDirectoryRecord;
            Panasonic3doDirectoryStructure tempDirectory;
            Panasonic3doFileStructure tempFile;

            // get the first sector
            directorySector = CdRom.GetSectorByLba(isoStream, baseOffset, currentLba, isRaw, nonRawSectorSize);
            directorySector = CdRom.GetDataChunkFromSector(directorySector, isRaw);
            directoryRecordLength = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directorySector, 0xC, 4));

            bytesRead = 0x14;
            currentOffset = 0x14;

            while (bytesRead < directoryRecordLength)
            {
                recordSize = 0x48 + (4 * ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directorySector, (int)(currentOffset + 0x40), 4)));
                directoryRecordBytes = ParseFile.ParseSimpleOffset(directorySector, (int)currentOffset, (int)recordSize);
                tempDirectoryRecord = new Panasonic3doDirectoryRecord(directoryRecordBytes);

                if (tempDirectoryRecord.DirectoryItemTypeBytes[3] == 0x07)
                {
                    //for (uint i = 0; i < tempDirectoryRecord.SubDirectoryCount; i++)
                    //{
                        tempDirectory =
                            new Panasonic3doDirectoryStructure(isoStream, isoStream.Name,
                                creationDateTime, baseOffset, tempDirectoryRecord.SubDirectoryLbas[0],
                                logicalBlockSize, tempDirectoryRecord.DirectoryItemName, 
                                parentDirectory, isRaw, nonRawSectorSize);
                        this.SubDirectoryArray.Add(tempDirectory);                    
                    //}
                    

                }
                else
                {                    
                    tempFile = new Panasonic3doFileStructure(parentDirectory, 
                        this.SourceFilePath, tempDirectoryRecord.DirectoryItemName, 
                        baseOffset, tempDirectoryRecord.SubDirectoryLbas[0], 
                        tempDirectoryRecord.DirectoryItemSize, isRaw, nonRawSectorSize,
                        creationDateTime);
                    
                    this.FileArray.Add(tempFile);
                }

                if (tempDirectoryRecord.DirectoryItemTypeBytes[0] == 0xC0)
                {
                    break;
                }
                else if (tempDirectoryRecord.DirectoryItemTypeBytes[0] == 0x40)
                {
                    directorySector = CdRom.GetSectorByLba(isoStream, baseOffset, ++currentLba, isRaw, nonRawSectorSize);
                    directorySector = CdRom.GetDataChunkFromSector(directorySector, isRaw);
                    directoryRecordLength = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directorySector, 0xC, 4));

                    bytesRead = 0x14;
                    currentOffset = 0x14;
                }
                else
                {
                    bytesRead += recordSize;
                    currentOffset += recordSize;
                }
            }
        }
    }

    public class Panasonic3doDirectoryRecord
    {
        // each dir folder (not item) starts with FFFFFFFF, but if it is 00000001 for example, then it will have n + 1 sectors (2 in the example)
        
        public byte[] DirectoryItemTypeBytes { set; get; } // 02 is file, 07 is dir, byte 0 == C0 means last in dir, byte 0 == 40 means dir continues in next sector        
        public byte[] Unknown1 { set; get; }
        public byte[] DirectoryItemTypeTextFlagBytes { set; get; } // 0x20202020 file, *dir directory
        public string DirectoryItemTypeTextFlag { set; get; } // 0x20202020 file, *dir directory
        public uint DirectoryItemLogicalBlockSize { set; get; }
        public uint DirectoryItemSize { set; get; } // filesize or 0x800 * sector count for dir
        public uint DirectoryItemSizeInSectors { set; get; }
        public byte[] Unknown2 { set; get; } // always 0x00000001?
        public byte[] Unknown3 { set; get; } // always 0x00000000?
        public byte[] DirectoryItemNameBytes { set; get; } // constant length of 0x20, null terminated
        public string DirectoryItemName { set; get; }
        public uint SubDirectoryCount { set; get; } // count - 1 on disc, always 0x00000000 for files?
        public uint[] SubDirectoryLbas { set; get; } // 4 bytes each, for total of SubDirectoryCount LBAs
        
        public Panasonic3doDirectoryRecord(byte[] directoryBytes)
        {
            this.DirectoryItemTypeBytes = ParseFile.ParseSimpleOffset(directoryBytes, 0, 4);
            this.Unknown1  = ParseFile.ParseSimpleOffset(directoryBytes, 4, 4);
            this.DirectoryItemTypeTextFlagBytes = ParseFile.ParseSimpleOffset(directoryBytes, 8, 4);
            this.DirectoryItemTypeTextFlag = 
                ByteConversion.GetEncodedText(this.DirectoryItemTypeTextFlagBytes, ByteConversion.GetPredictedCodePageForTags(this.DirectoryItemTypeTextFlagBytes));
            this.DirectoryItemLogicalBlockSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, 0xC, 4));
            this.DirectoryItemSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, 0x10, 4));
            this.DirectoryItemSizeInSectors = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, 0x14, 4));
            this.Unknown2 = ParseFile.ParseSimpleOffset(directoryBytes, 0x18, 4);
            this.Unknown3 = ParseFile.ParseSimpleOffset(directoryBytes, 0x1C, 4);
            this.DirectoryItemNameBytes = ParseFile.ParseSimpleOffset(directoryBytes, 0x20, 0x20);
            this.DirectoryItemName =
                ByteConversion.GetEncodedText(FileUtil.ReplaceNullByteWithSpace(this.DirectoryItemNameBytes), ByteConversion.GetPredictedCodePageForTags(this.DirectoryItemNameBytes)).Trim();
            this.SubDirectoryCount = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, 0x40, 4)) + 1;
            this.SubDirectoryLbas = new uint[this.SubDirectoryCount];

            for (uint i = 0; i < this.SubDirectoryCount; i++)
            {
                this.SubDirectoryLbas[i] =
                    ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, (int)(0x44 + (i * 4)), 4));
            }            
        }
    }
}
