using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VGMToolbox.util;

namespace VGMToolbox.format.iso
{
    public class GreenBookCdi
    {
        public static readonly byte[] VOLUME_DESCRIPTOR_IDENTIFIER = new byte[] { 0x01, 0x43, 0x44, 0x2D, 0x49, 0x20 };
        public static string FORMAT_DESCRIPTION_STRING = "CD-i";
    }

    public class GreenBookCdiVolume : IVolume
    {
        public long VolumeBaseOffset { set; get; }
        public string FormatDescription { set; get; }
        public VolumeDataType VolumeType { set; get; }
        public ArrayList DirectoryStructureArray { set; get; }
        public bool IsRawDump { set; get; }
        public int SectorSize { set; get; }
        public IDirectoryStructure[] Directories
        {
            set { Directories = value; }
            get
            {
                DirectoryStructureArray.Sort();
                return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(GreenBookCdiDirectoryStructure));
            }
        }

        #region Standard Attributes

        public byte VolumeDescriptorType { set; get; }
        public byte[] StandardIdentifier { set; get; }
        public byte VolumeDescriptorVersion { set; get; }

        public byte UnusedField1 { set; get; }

        public string SystemIdentifier { set; get; }
        public string VolumeIdentifier { set; get; }

        public byte[] UnusedField2 { set; get; }

        public uint VolumeSpaceSize { set; get; }

        public byte[] UnusedField3 { set; get; }

        public ushort VolumeSetSize { set; get; }
        public ushort VolumeSequenceNumber { set; get; }
        public ushort LogicalBlockSize { set; get; }

        public uint PathTableSize { set; get; }
        public uint LocationOfOccurrenceOfTypeLPathTable { set; get; }
        public uint LocationOfOptionalOccurrenceOfTypeLPathTable { set; get; }
        public uint LocationOfOccurrenceOfTypeMPathTable { set; get; }
        public uint LocationOfOptionalOccurrenceOfTypeMPathTable { set; get; }

        public byte[] DirectoryRecordForRootDirectoryBytes { set; get; }
        public GreenBookCdiDirectoryRecord DirectoryRecordForRootDirectory { set; get; }

        public string VolumeSetIdentifier { set; get; }
        public string PublisherIdentifier { set; get; }
        public string DataPreparerIdentifier { set; get; }
        public string ApplicationIdentifier { set; get; }
        public string CopyrightFileIdentifier { set; get; }
        public string AbstractFileIdentifier { set; get; }
        public string BibliographicFileIdentifier { set; get; }

        public DateTime VolumeCreationDateAndTime { set; get; }
        public DateTime VolumeModificationDateAndTime { set; get; }
        public DateTime VolumeExpirationDateAndTime { set; get; }
        public DateTime VolumeEffectiveDateAndTime { set; get; }

        public byte FileStructureVersion { set; get; }

        public byte Reserved1 { set; get; }

        public byte[] ApplicationUse { set; get; }

        public byte[] Reserved2 { set; get; }

        #endregion
        
        public void Initialize(FileStream isoStream, long offset, bool isRawDump)
        {
            byte[] sectorBytes;
            byte[] sectorDataBytes;

            this.FormatDescription = GreenBookCdi.FORMAT_DESCRIPTION_STRING;
            this.VolumeType = VolumeDataType.Data;
            this.IsRawDump = isRawDump;
            this.DirectoryStructureArray = new ArrayList();

            this.VolumeBaseOffset =
                this.IsRawDump ? (offset - Iso9660.EMPTY_HEADER_SIZE_RAW) : (offset - Iso9660.EMPTY_HEADER_SIZE);
            this.SectorSize =
                this.IsRawDump ? (int)CdRom.RAW_SECTOR_SIZE : (int)CdRom.NON_RAW_SECTOR_SIZE;

            // parse inital level sector
            sectorBytes = ParseFile.ParseSimpleOffset(isoStream, offset, this.SectorSize);
            sectorDataBytes = CdRom.GetDataChunkFromSector(sectorBytes, this.IsRawDump);


            this.VolumeDescriptorType = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x00, 1)[0];
            this.StandardIdentifier = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x01, 5);
            this.VolumeDescriptorVersion = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x06, 1)[0];

            this.UnusedField1 = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x07, 1)[0];

            this.SystemIdentifier = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x08, 0x20)).Trim();
            this.VolumeIdentifier = ByteConversion.GetAsciiText(FileUtil.ReplaceNullByteWithSpace(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x28, 0x20))).Trim();

            this.UnusedField2 = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x48, 0x08);

            this.VolumeSpaceSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x54, 0x04));

            this.UnusedField3 = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x58, 0x20);

            this.VolumeSetSize = ByteConversion.GetUInt16BigEndian(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x7A, 0x02));
            this.VolumeSequenceNumber = ByteConversion.GetUInt16BigEndian(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x7E, 0x02));
            this.LogicalBlockSize = ByteConversion.GetUInt16BigEndian(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x82, 0x02));

            this.PathTableSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x88, 0x04));
            this.LocationOfOccurrenceOfTypeLPathTable = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x8C, 0x04), 0);
            this.LocationOfOptionalOccurrenceOfTypeLPathTable = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x90, 0x04), 0);
            this.LocationOfOccurrenceOfTypeMPathTable = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x94, 0x04));
            this.LocationOfOptionalOccurrenceOfTypeMPathTable = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x98, 0x04));

            this.DirectoryRecordForRootDirectoryBytes = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x9C, 0x22);
            this.DirectoryRecordForRootDirectory = new GreenBookCdiDirectoryRecord(this.DirectoryRecordForRootDirectoryBytes);

            this.VolumeSetIdentifier = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(sectorDataBytes, 0xBE, 0x80)).Trim();
            this.PublisherIdentifier = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x13E, 0x80)).Trim();
            this.DataPreparerIdentifier = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x1BE, 0x80)).Trim();
            this.ApplicationIdentifier = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x23E, 0x80)).Trim();
            this.CopyrightFileIdentifier = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x2BE, 0x25)).Trim();
            this.AbstractFileIdentifier = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x2E3, 0x25)).Trim();
            this.BibliographicFileIdentifier = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x308, 0x25)).Trim();

            this.VolumeCreationDateAndTime = Iso9660.GetIsoDateTime(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x32D, 0x11));
            this.VolumeModificationDateAndTime = Iso9660.GetIsoDateTime(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x33E, 0x11));
            this.VolumeExpirationDateAndTime = Iso9660.GetIsoDateTime(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x34F, 0x11));
            this.VolumeEffectiveDateAndTime = Iso9660.GetIsoDateTime(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x360, 0x11));

            this.FileStructureVersion = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x371, 1)[0];

            this.Reserved1 = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x372, 1)[0];

            this.ApplicationUse = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x373, 0x200);

            this.Reserved2 = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x573, 0x28D);

            this.LoadDirectories(isoStream);
        }

        public void ExtractAll(ref Dictionary<string, FileStream> streamCache, string destintionFolder, bool extractAsRaw)
        {
            foreach (GreenBookCdiDirectoryStructure ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, destintionFolder, extractAsRaw);
            }
        }

        public virtual void LoadDirectories(FileStream isoStream)
        {
            // change name of top level folder
            this.DirectoryRecordForRootDirectory.FileIdentifierString = String.Empty;

            // get first path record
            byte[] rootDirPathBytes = CdRom.GetSectorByLba(isoStream, this.VolumeBaseOffset, this.LocationOfOccurrenceOfTypeMPathTable, this.IsRawDump, this.LogicalBlockSize);
            rootDirPathBytes = CdRom.GetDataChunkFromSector(rootDirPathBytes, this.IsRawDump);

            // grab the directory record
            uint rootDirectoryOffset = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(rootDirPathBytes, 2, 4));
            byte[] rootDirDirectorySector = CdRom.GetSectorByLba(isoStream, this.VolumeBaseOffset, rootDirectoryOffset, this.IsRawDump, this.LogicalBlockSize);
            rootDirDirectorySector = CdRom.GetDataChunkFromSector(rootDirDirectorySector, this.IsRawDump);
            
            byte rootDirectoryRecordSize = rootDirDirectorySector[0];
            byte[] rootDirectoryRecord = ParseFile.ParseSimpleOffset(rootDirDirectorySector, 0, rootDirectoryRecordSize);
            this.DirectoryRecordForRootDirectory = new GreenBookCdiDirectoryRecord(rootDirectoryRecord);

            // populate this volume's directory structure
            GreenBookCdiDirectoryStructure rootDirectory =
                new GreenBookCdiDirectoryStructure(isoStream, isoStream.Name,
                    this.VolumeBaseOffset, this.DirectoryRecordForRootDirectory,
                    this.LogicalBlockSize, this.IsRawDump, this.SectorSize, null);
            this.DirectoryStructureArray.Add(rootDirectory);
        }
    }

    public class GreenBookCdiDirectoryRecord
    {
        public const ushort CDI_ATTR_MODE2FORM1 = (1 << 11);
        public const ushort CDI_ATTR_MODE2FORM2 = (1 << 12);
        public const ushort CDI_ATTR_CDDA = (1 << 14);
        public const ushort CDI_ATTR_DIRECTORY = (1 << 15);
        
        public byte LengthOfDirectoryRecord { set; get; }
        public byte ExtendedAttributeRecordLength { set; get; }
        public uint LocationOfExtent { set; get; }
        public uint DataLength { set; get; }
        public DateTime RecordingDateAndTime { set; get; }
        public byte FileFlags { set; get; }
        public byte FileUnitSize { set; get; }
        public byte InterleaveGapSize { set; get; }
        public ushort VolumeSequenceNumber { set; get; }
        public byte LengthOfFileIdentifier { set; get; }

        public byte[] FileIdentifier { set; get; }
        public string FileIdentifierString { set; get; }

        public byte[] PaddingField { set; get; }
        public byte[] SystemUse { set; get; }

        public bool FlagExistance { set; get; }
        public bool FlagDirectory { set; get; }
        public bool FlagAssociatedFile { set; get; }
        public bool FlagRecord { set; get; }
        public bool FlagProtection { set; get; }
        public bool FlagMultiExtent { set; get; }

        public CdSectorType ItemMode { set; get; }

        public GreenBookCdiDirectoryRecord(byte[] directoryBytes)
        {
            this.LengthOfDirectoryRecord = directoryBytes[0];

            if (this.LengthOfDirectoryRecord > 0)
            {
                this.ExtendedAttributeRecordLength = directoryBytes[1];
                this.LocationOfExtent = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, 6, 4));
                this.DataLength = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, 0x0E, 4));

                this.RecordingDateAndTime = new DateTime(directoryBytes[0x12] + 1900,
                                                         directoryBytes[0x13],
                                                         directoryBytes[0x14],
                                                         directoryBytes[0x15],
                                                         directoryBytes[0x16],
                                                         directoryBytes[0x17]);

                this.FileFlags = directoryBytes[this.LengthOfDirectoryRecord - 6];
                ushort cdiItemDetails = ByteConversion.GetUInt16BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, this.LengthOfDirectoryRecord - 6, 2));

                if ((cdiItemDetails & CDI_ATTR_DIRECTORY) == CDI_ATTR_DIRECTORY)
                {
                    this.FlagDirectory = true;
                }
                //else if ((cdiItemDetails & CDI_ATTR_MODE2FORM1) == CDI_ATTR_MODE2FORM1)
                //{
                //    this.ItemMode = CdSectorType.Mode2Form1;
                //}
                //else if ((cdiItemDetails & CDI_ATTR_MODE2FORM2) == CDI_ATTR_MODE2FORM2)
                //{
                //    this.ItemMode = CdSectorType.Mode2Form2;
                //    this.DataLength = (uint)(this.DataLength / (uint)CdRom.NON_RAW_SECTOR_SIZE) * (uint)CdRom.RAW_SECTOR_SIZE;
                //}
                //else if ((cdiItemDetails & CDI_ATTR_CDDA) == CDI_ATTR_CDDA)
                //{
                //    this.ItemMode = CdSectorType.Audio;
                //    this.DataLength = (uint)(this.DataLength / (uint)CdRom.NON_RAW_SECTOR_SIZE) * (uint)CdRom.RAW_SECTOR_SIZE;
                //}
                //else
                //{
                //    this.ItemMode = CdSectorType.Unknown;
                //}


                /*
                this.FlagExistance = (this.FileFlags & 0x1) == 0x1 ? true : false;
                this.FlagDirectory = (this.FileFlags & 0x2) == 0x2 ? true : false;
                this.FlagAssociatedFile = (this.FileFlags & 0x4) == 0x4 ? true : false;
                this.FlagRecord = (this.FileFlags & 0x08) == 0x08 ? true : false;
                this.FlagProtection = (this.FileFlags & 0x10) == 0x10 ? true : false;
                this.FlagMultiExtent = (this.FileFlags & 0x80) == 0x80 ? false : true;
                */
                
                this.FileUnitSize = directoryBytes[0x1A];
                this.InterleaveGapSize = directoryBytes[0x1B];
                this.VolumeSequenceNumber = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(directoryBytes, 0x1E, 2), 0);
                this.LengthOfFileIdentifier = directoryBytes[0x20];

                this.FileIdentifier = ParseFile.ParseSimpleOffset(directoryBytes, 0x21, this.LengthOfFileIdentifier);

                // parse identifier
                if (this.LengthOfFileIdentifier > 1)
                {
                    this.FileIdentifierString =
                        ByteConversion.GetEncodedText(this.FileIdentifier,
                            ByteConversion.GetPredictedCodePageForTags(this.FileIdentifier));
                }
                else if (this.FileIdentifier[0] == 0)
                {
                    this.FileIdentifierString = ".";
                }
                else if (this.FileIdentifier[0] == 1)
                {
                    this.FileIdentifierString = "..";
                }

                /*
            
                public byte[] PaddingField { set; get; }
                public byte[] SystemUse { set; get; }        
                */
            }
        }
    }

    public class GreenBookCdiFileStructure : IFileStructure
    {
        public string ParentDirectoryName { set; get; }
        public string SourceFilePath { set; get; }
        public string FileName { set; get; }
        public long VolumeBaseOffset { set; get; }
        public long Lba { set; get; }
        public long Size { set; get; }
        public bool IsRaw { set; get; }
        public int NonRawSectorSize { set; get; }
        public CdSectorType FileMode { set; get; }
        public DateTime FileDateTime { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is GreenBookCdiFileStructure)
            {
                GreenBookCdiFileStructure o = (GreenBookCdiFileStructure)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not an GreenBookCdiFileStructure");
        }

        public GreenBookCdiFileStructure(string parentDirectoryName, string sourceFilePath, string fileName, long volumeBaseOffset, uint lba, long size, bool isRaw, int nonRawSectorSize, DateTime fileTime)
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

            CdRom.ExtractCdData(streamCache[this.SourceFilePath], destinationFile, 
                this.VolumeBaseOffset, this.Lba, this.Size, 
                this.IsRaw, this.NonRawSectorSize, this.FileMode, extractAsRaw);
        }
    }

    public class GreenBookCdiDirectoryStructure : IDirectoryStructure
    {
        public GreenBookCdiDirectoryRecord ParentDirectoryRecord { set; get; }
        public string ParentDirectoryName { set; get; }

        public ArrayList SubDirectoryArray { set; get; }
        public IDirectoryStructure[] SubDirectories
        {
            set { this.SubDirectories = value; }
            get
            {
                SubDirectoryArray.Sort();
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(GreenBookCdiDirectoryStructure));
            }
        }

        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { this.Files = value; }
            get
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(GreenBookCdiFileStructure));
            }
        }

        public string SourceFilePath { set; get; }
        public string DirectoryName { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is GreenBookCdiDirectoryStructure)
            {
                GreenBookCdiDirectoryStructure o = (GreenBookCdiDirectoryStructure)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not an GreenBookCdiDirectoryStructure");
        }

        public GreenBookCdiDirectoryStructure(FileStream isoStream, string sourceFilePath,
            long baseOffset, GreenBookCdiDirectoryRecord directoryRecord,
            uint logicalBlockSize, bool isRaw, int nonRawSectorSize,
            string parentDirectory)
        {
            string nextDirectory;
            this.SourceFilePath = sourceFilePath;
            this.SubDirectoryArray = new ArrayList();
            this.FileArray = new ArrayList();

            if (String.IsNullOrEmpty(parentDirectory))
            {
                this.ParentDirectoryName = String.Empty;
                this.DirectoryName = directoryRecord.FileIdentifierString;
                nextDirectory = this.DirectoryName;
            }
            else
            {
                this.ParentDirectoryName = parentDirectory;
                this.DirectoryName = directoryRecord.FileIdentifierString;
                nextDirectory = this.ParentDirectoryName + Path.DirectorySeparatorChar + this.DirectoryName;
            }

            this.parseDirectoryRecord(isoStream, baseOffset, directoryRecord, logicalBlockSize, isRaw, nonRawSectorSize, nextDirectory);
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string fullDirectoryPath = Path.Combine(destinationFolder, Path.Combine(this.ParentDirectoryName, this.DirectoryName));

            // create directory
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            foreach (GreenBookCdiFileStructure f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (GreenBookCdiDirectoryStructure d in this.SubDirectoryArray)
            {
                d.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }
        }

        private void parseDirectoryRecord(FileStream isoStream, long baseOffset,
            GreenBookCdiDirectoryRecord directoryRecord, uint logicalBlockSize,
            bool isRaw, int nonRawSectorSize, string parentDirectory)
        {
            byte recordSize;
            int currentOffset;
            uint bytesRead = 0;
            uint currentLba = directoryRecord.LocationOfExtent;
            byte[] directoryRecordBytes;
            GreenBookCdiDirectoryRecord tempDirectoryRecord;
            GreenBookCdiDirectoryStructure tempDirectory;
            GreenBookCdiFileStructure tempFile;

            byte[] directorySector = CdRom.GetSectorByLba(isoStream, baseOffset, currentLba, isRaw, nonRawSectorSize);
            directorySector = CdRom.GetDataChunkFromSector(directorySector, isRaw);

            currentOffset = 0;

            while (bytesRead < directoryRecord.DataLength)
            {
                recordSize = ParseFile.ParseSimpleOffset(directorySector, currentOffset, 1)[0];

                if (recordSize > 0)
                {
                    directoryRecordBytes = ParseFile.ParseSimpleOffset(directorySector, currentOffset, recordSize);
                    tempDirectoryRecord = new GreenBookCdiDirectoryRecord(directoryRecordBytes);

                    if (!tempDirectoryRecord.FileIdentifierString.Equals(".") &&
                        !tempDirectoryRecord.FileIdentifierString.Equals("..")) // skip "this" directory
                    {
                        //if (tempDirectoryRecord.FlagMultiExtent)
                        //{
                        //    int x = 1;
                        //}

                        if (tempDirectoryRecord.FlagDirectory)
                        {
                            tempDirectory = new GreenBookCdiDirectoryStructure(isoStream, isoStream.Name, baseOffset, tempDirectoryRecord, logicalBlockSize, isRaw, nonRawSectorSize, parentDirectory);
                            this.SubDirectoryArray.Add(tempDirectory);
                        }
                        else
                        {
                            tempFile = new GreenBookCdiFileStructure(parentDirectory,
                                this.SourceFilePath,
                                tempDirectoryRecord.FileIdentifierString.Replace(";1", String.Empty),
                                baseOffset,
                                tempDirectoryRecord.LocationOfExtent,
                                tempDirectoryRecord.DataLength,
                                isRaw,
                                nonRawSectorSize,
                                tempDirectoryRecord.RecordingDateAndTime);
                            this.FileArray.Add(tempFile);
                        }
                    }
                    else if (tempDirectoryRecord.FileIdentifierString.Equals(".."))
                    {
                        this.ParentDirectoryRecord = tempDirectoryRecord;
                    }

                    bytesRead += recordSize;
                    currentOffset += recordSize;
                }
                else if ((directoryRecord.DataLength - bytesRead) > (directorySector.Length - currentOffset))
                {
                    // move to start of next sector
                    directorySector = CdRom.GetSectorByLba(isoStream, baseOffset, ++currentLba, isRaw, nonRawSectorSize);
                    directorySector = CdRom.GetDataChunkFromSector(directorySector, isRaw);
                    bytesRead += (uint)(logicalBlockSize - currentOffset);
                    currentOffset = 0;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
