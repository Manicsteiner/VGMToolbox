using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VGMToolbox.util;

namespace VGMToolbox.format.iso
{
    public class XDvdFs
    {
        public static readonly byte[] STANDARD_IDENTIFIER = 
            new byte[] { 0x4D, 0x49, 0x43, 0x52, 0x4F, 0x53, 0x4F, 0x46, 
                         0x54, 0x2A, 0x58, 0x42, 0x4F, 0x58, 0x2A, 0x4D, 
                         0x45, 0x44, 0x49, 0x41 };

        public static long SECTOR_SIZE = 0x800;
        public static long BASE_OFFSET_CORRECTION = 0x10000;
        public static long DWORD_SIZE = 4;

        public static string FORMAT_DESCRIPTION_STRING = "XDVDFS";
        public static string XGD3_WARNING_FILE_NAME = "README.TXT";

        public static IVolume[] GetVolumes(string isoPath, bool isRawDump)
        {
            ArrayList volumeList = new ArrayList();
            XDvdFsVolume volume;
            long volumeOffset;

            using (FileStream fs = File.OpenRead(isoPath))
            {
                volumeOffset = ParseFile.GetNextOffset(fs, 0, XDvdFs.STANDARD_IDENTIFIER, true, 0x800, 0);
                volume = new XDvdFsVolume();
                volume.Initialize(fs, volumeOffset, isRawDump);
                volumeList.Add(volume);
            }

            return (IVolume[])volumeList.ToArray(typeof(XDvdFsVolume));
        }
    }

    public class XDvdFsVolume : IVolume
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
                return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(XDvdFsDirectoryStructure));
            }
        }

        public uint RootDirectorySector { set; get; }
        public uint RootDirectorySize { set; get; }
        public DateTime ImageCreationTime { set; get; }

        public void Initialize(FileStream isoStream, long offset, bool isRawDump)
        {
            XDvdFsDirectoryStructure rootDir;
            long rootDirectoryOffset;
            byte[] DateTimeBytes;

            this.VolumeBaseOffset = offset - XDvdFs.BASE_OFFSET_CORRECTION;
            this.FormatDescription = XDvdFs.FORMAT_DESCRIPTION_STRING;
            this.VolumeType = VolumeDataType.Data;
            this.IsRawDump = isRawDump;
            this.DirectoryStructureArray = new ArrayList();

            this.RootDirectorySector = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(isoStream, offset + 0x14, 0x4), 0);
            this.RootDirectorySize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(isoStream, offset + 0x18, 0x4), 0);
            
            DateTimeBytes = ParseFile.ParseSimpleOffset(isoStream, offset + 0x1C, 0x8);
            this.ImageCreationTime = DateTime.FromFileTime(BitConverter.ToInt64(DateTimeBytes, 0));
            
            // Build Tree from Root Directory
            rootDirectoryOffset = this.VolumeBaseOffset + (this.RootDirectorySector * XDvdFs.SECTOR_SIZE);
            rootDir = new XDvdFsDirectoryStructure(isoStream, isoStream.Name, this.ImageCreationTime, this.VolumeBaseOffset, rootDirectoryOffset, XDvdFs.SECTOR_SIZE, String.Empty, String.Empty, isRawDump, (int)XDvdFs.SECTOR_SIZE);
            this.DirectoryStructureArray.Add(rootDir);
        }

        public void ExtractAll(ref Dictionary<string, FileStream> streamCache, string destintionFolder, bool extractAsRaw)
        {
            foreach (XDvdFsDirectoryStructure ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, destintionFolder, extractAsRaw);
            }
        }
    }

    public class XDvdFsFileStructure : IFileStructure
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
            if (obj is XDvdFsFileStructure)
            {
                XDvdFsFileStructure o = (XDvdFsFileStructure)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not an XDvdFsFileStructure");
        }

        public XDvdFsFileStructure(string parentDirectoryName, string sourceFilePath, string fileName, long offset, long volumeBaseOffset, long lba, long size, DateTime fileTime, bool isRaw, int nonRawSectorSize)
        {
            this.ParentDirectoryName = parentDirectoryName;
            this.SourceFilePath = sourceFilePath;
            this.FileName = fileName;
            this.Offset = offset;
            this.Size = size;
            this.FileDateTime = fileTime;

            this.VolumeBaseOffset = volumeBaseOffset;
            this.Lba =lba;
            this.IsRaw = isRaw;
            this.NonRawSectorSize = nonRawSectorSize;
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

    public class XDvdFsDirectoryRecord
    {
        public long OffsetToLeftSubTree { set; get; }
        public long OffsetToRightSubTree { set; get; }
        
        public uint StartingSector { set; get; }
        public uint EntrySize { set; get; }

        public byte EntryFlags { set; get; }
        
        public byte EntryNameSize { set; get; }
        public byte[] EntryNameBytes {set;get;}
        public string EntryName { set; get; }

        public XDvdFsDirectoryRecord(byte[] directoryBytes)
        {
            this.OffsetToLeftSubTree = (long)(BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(directoryBytes, 0x00, 2), 0) * XDvdFs.DWORD_SIZE);
            this.OffsetToRightSubTree = (long)(BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(directoryBytes, 0x02, 2), 0) * XDvdFs.DWORD_SIZE);

            this.StartingSector = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(directoryBytes, 0x04, 4), 0);
            this.EntrySize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(directoryBytes, 0x08, 4), 0);

            this.EntryFlags = ParseFile.ParseSimpleOffset(directoryBytes, 0x0C, 1)[0];

            this.EntryNameSize = ParseFile.ParseSimpleOffset(directoryBytes, 0x0D, 1)[0];
            this.EntryNameBytes = ParseFile.ParseSimpleOffset(directoryBytes, 0x0E, this.EntryNameSize);
            this.EntryName = ByteConversion.GetAsciiText(this.EntryNameBytes);            
        }
    }

    public class XDvdFsDirectoryStructure : IDirectoryStructure
    {
        public XDvdFsDirectoryRecord ParentDirectoryRecord { set; get; }
        public long DirectoryRecordOffset { set; get; }
        public string ParentDirectoryName { set; get; }

        public ArrayList SubDirectoryArray { set; get; }
        public IDirectoryStructure[] SubDirectories
        {
            set { this.SubDirectories = value; }
            get
            {
                SubDirectoryArray.Sort();
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(XDvdFsDirectoryStructure));
            }
        }

        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { this.Files = value; }
            get
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(XDvdFsFileStructure));
            }
        }

        public string SourceFilePath { set; get; }
        public string DirectoryName { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is XDvdFsDirectoryStructure)
            {
                XDvdFsDirectoryStructure o = (XDvdFsDirectoryStructure)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not an XDvdFsDirectoryStructure");
        }

        public XDvdFsDirectoryStructure(FileStream isoStream, 
            string sourceFilePath, DateTime creationDateTime, 
            long baseOffset, long directoryOffset, long logicalBlockSize, 
            string directoryName, string parentDirectory, bool isRaw, int nonRawSectorSize)
        {
            string nextDirectory;
            this.SourceFilePath = sourceFilePath;
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

            this.parseDirectoryRecord(isoStream, creationDateTime, baseOffset, directoryOffset, logicalBlockSize, nextDirectory, isRaw, nonRawSectorSize);
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string fullDirectoryPath = Path.Combine(destinationFolder, Path.Combine(this.ParentDirectoryName, this.DirectoryName));

            // create directory
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            foreach (XDvdFsFileStructure f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (XDvdFsDirectoryStructure d in this.SubDirectoryArray)
            {
                d.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }
        }

        private void parseDirectoryRecord(
            FileStream isoStream, 
            DateTime creationDateTime,
            long baseOffset, 
            long recordOffset, 
            long logicalBlockSize, 
            string parentDirectory,
            bool isRaw,
            int nonRawSectorSize)
        {
            byte recordSize;
            byte[] directoryRecordBytes;
            XDvdFsDirectoryRecord tempDirectoryRecord;
            XDvdFsDirectoryStructure tempDirectory;
            XDvdFsFileStructure tempFile;

            long directoryOffset;

            try
            {

                // get the first record
                recordSize = (byte)(0x0E + ParseFile.ParseSimpleOffset(isoStream, ((long)recordOffset + 0x0D), 1)[0]);
                directoryRecordBytes = ParseFile.ParseSimpleOffset(isoStream, recordOffset, recordSize);
                tempDirectoryRecord = new XDvdFsDirectoryRecord(directoryRecordBytes);

                if (tempDirectoryRecord.EntryFlags == 0x10 ||
                    tempDirectoryRecord.EntryFlags == 0x90)
                {
                    if (tempDirectoryRecord.EntrySize > 0)
                    {
                        directoryOffset = baseOffset + (long)((long)tempDirectoryRecord.StartingSector * (long)XDvdFs.SECTOR_SIZE);
                        tempDirectory = new XDvdFsDirectoryStructure(isoStream, isoStream.Name, creationDateTime, baseOffset, directoryOffset, logicalBlockSize, tempDirectoryRecord.EntryName, parentDirectory, isRaw, nonRawSectorSize);
                        this.SubDirectoryArray.Add(tempDirectory);
                    }
                }
                else
                {
                    tempFile = new XDvdFsFileStructure(parentDirectory,
                        this.SourceFilePath,
                        tempDirectoryRecord.EntryName,
                        baseOffset + (long)((long)tempDirectoryRecord.StartingSector * (long)logicalBlockSize),
                        baseOffset,
                        tempDirectoryRecord.StartingSector,
                        tempDirectoryRecord.EntrySize,
                        creationDateTime, 
                        isRaw,
                        nonRawSectorSize);
                    this.FileArray.Add(tempFile);
                }

                // Process Left SubTree
                if (tempDirectoryRecord.OffsetToLeftSubTree != 0)
                {
                    this.parseDirectoryRecord(isoStream, creationDateTime,
                        baseOffset, this.DirectoryRecordOffset + tempDirectoryRecord.OffsetToLeftSubTree,
                        logicalBlockSize, parentDirectory, isRaw, nonRawSectorSize);
                }

                // Process Right SubTree
                if (tempDirectoryRecord.OffsetToRightSubTree != 0)
                {
                    this.parseDirectoryRecord(isoStream, creationDateTime,
                        baseOffset, this.DirectoryRecordOffset + tempDirectoryRecord.OffsetToRightSubTree,
                        logicalBlockSize, parentDirectory, isRaw, nonRawSectorSize);
                }
            }
            catch (Exception ex)
            {
                throw new FormatException(String.Format("Error processing XDVDFS directory record at 0x{0}", recordOffset.ToString("X8")), ex);
            }
        }
    }
}
