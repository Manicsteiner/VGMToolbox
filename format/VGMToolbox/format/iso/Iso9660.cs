using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using VGMToolbox.util;

namespace VGMToolbox.format.iso
{
    public class Iso9660
    {
        public const long EMPTY_HEADER_SIZE = 0x8000;
        public const long EMPTY_HEADER_SIZE_RAW = 0x9300;        
        public static readonly byte[] STANDARD_IDENTIFIER = new byte[] { 0x43, 0x44, 0x30, 0x30, 0x31 };
        public static readonly byte[] EMPTY_DATETIME = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 
                                                                    0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x00 };
        public static readonly byte[] EMPTY_DATETIME_HACK = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 
                                                                    0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 };

        public static readonly byte[] EMPTY_FILE_DATETIME = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public static readonly byte[] VOLUME_DESCRIPTOR_IDENTIFIER = new byte[] { 0x01, 0x43, 0x44, 0x30, 0x30, 0x31 };
        public static readonly byte[] VOLUME_DESCRIPTOR_IDENTIFIER_SUPPLMENTARY = new byte[] { 0x02, 0x43, 0x44, 0x30, 0x30, 0x31 };        
        public static string FORMAT_DESCRIPTION_STRING = "ISO 9660";

        public static readonly byte[] CDXA_IDENTIFIER = new byte[] { 0x43, 0x44, 0x2D, 0x58, 0x41, 0x30, 0x30, 0x31 }; // CD-XA001
        public const long CDXA_IDENTIFIER_OFFSET = 0x400;

        public static DateTime GetIsoDateTime(byte[] isoDateArray)
        {
            DateTime dateValue = new DateTime();
            string dateString;

            if (ParseFile.CompareSegment(isoDateArray, 0, EMPTY_DATETIME))
            {
                dateValue = DateTime.MinValue;
            }
            // Easy CD Creator v4.2 (310), and maybe others, 
            //  doesn't set the Grenwich Mean Time offset to zero as it should
            else if (ParseFile.CompareSegmentUsingSourceOffset(isoDateArray, 0, EMPTY_DATETIME_HACK.Length, EMPTY_DATETIME_HACK))
            {
                dateValue = DateTime.MinValue;
            }
            else
            {
                dateString = ByteConversion.GetAsciiText(isoDateArray);
                dateValue = new DateTime(Int32.Parse(dateString.Substring(0, 4).Replace("0000", "2000")),
                                         Int16.Parse(dateString.Substring(4, 2)),
                                         Int16.Parse(dateString.Substring(6, 2)),
                                         Int16.Parse(dateString.Substring(8, 2)),
                                         Int16.Parse(dateString.Substring(10, 2)),
                                         Int16.Parse(dateString.Substring(12, 2)),
                                         Int16.Parse(dateString.Substring(14, 2)));
            }

            return dateValue;
        }

        public static IVolume[] GetVolumes(string isoPath, bool isRawDump)
        {
            ArrayList volumeList = new ArrayList();
            Iso9660Volume volume;
            long volumeOffset;

            using (FileStream fs = File.OpenRead(isoPath))
            {
                volumeOffset = ParseFile.GetNextOffset(fs, 0, Iso9660.VOLUME_DESCRIPTOR_IDENTIFIER);

                while (volumeOffset > -1)
                {
                    volume = new Iso9660Volume();
                    volume.Initialize(fs, volumeOffset, isRawDump);
                    volumeList.Add(volume);

                    volumeOffset = ParseFile.GetNextOffset(fs, volume.VolumeBaseOffset + ((long)volume.VolumeSpaceSize * (long)volume.LogicalBlockSize), Iso9660.VOLUME_DESCRIPTOR_IDENTIFIER);
                }
            }

            return (IVolume[])volumeList.ToArray(typeof(Iso9660Volume));
        }
    }

    public class Iso9660Volume : IVolume
    {
        public long VolumeBaseOffset { set; get; }
        public string FormatDescription { set; get; }
        public VolumeDataType VolumeType { set; get; }

        public bool IsRawDump { set; get; }
        public bool ContainsCdxaData { set; get; }
        public int SectorSize { set; get; }
        
        public ArrayList DirectoryStructureArray { set; get; }
        public IDirectoryStructure[] Directories 
        {
            set { Directories = value; }
            get 
            {
                DirectoryStructureArray.Sort();
                return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(Iso9660DirectoryStructure)); 
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
        public Iso9660DirectoryRecord DirectoryRecordForRootDirectory { set; get; }

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

        public virtual void Initialize(FileStream isoStream, long offset, bool isRawDump)
        {
            byte[] sectorBytes;
            byte[] sectorDataBytes;
            
            this.FormatDescription = Iso9660.FORMAT_DESCRIPTION_STRING;
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

            // check for CDXA marker
            this.ContainsCdxaData = ParseFile.CompareSegmentUsingSourceOffset(sectorDataBytes, (int)Iso9660.CDXA_IDENTIFIER_OFFSET, Iso9660.CDXA_IDENTIFIER.Length, Iso9660.CDXA_IDENTIFIER);

            this.VolumeDescriptorType = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x00, 1)[0];
            this.StandardIdentifier = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x01, 5);
            this.VolumeDescriptorVersion = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x06, 1)[0];

            this.UnusedField1 = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x07, 1)[0];

            this.SystemIdentifier = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x08, 0x20)).Trim();
            this.VolumeIdentifier = ByteConversion.GetAsciiText(FileUtil.ReplaceNullByteWithSpace(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x28, 0x20))).Trim();

            this.UnusedField2 = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x48, 0x08);

            this.VolumeSpaceSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x50, 0x04), 0);

            this.UnusedField3 = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x58, 0x20);

            this.VolumeSetSize = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x78, 0x02), 0);
            this.VolumeSequenceNumber = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x7C, 0x02), 0);
            this.LogicalBlockSize = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x80, 0x02), 0);

            this.PathTableSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x84, 0x04), 0);
            this.LocationOfOccurrenceOfTypeLPathTable = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x8C, 0x04), 0);
            this.LocationOfOptionalOccurrenceOfTypeLPathTable = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x90, 0x04), 0);
            this.LocationOfOccurrenceOfTypeMPathTable = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x94, 0x04));
            this.LocationOfOptionalOccurrenceOfTypeMPathTable = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(sectorDataBytes, 0x98, 0x04));

            this.DirectoryRecordForRootDirectoryBytes = ParseFile.ParseSimpleOffset(sectorDataBytes, 0x9C, 0x22);
            this.DirectoryRecordForRootDirectory = new Iso9660DirectoryRecord(this.DirectoryRecordForRootDirectoryBytes, this.ContainsCdxaData);

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
            foreach (Iso9660DirectoryStructure ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, destintionFolder, extractAsRaw);
            }
        }

        public virtual void LoadDirectories(FileStream isoStream)
        {
            Iso9660DirectoryStructure.InitializeStruct dirInitStruct = new Iso9660DirectoryStructure.InitializeStruct();
            
            // change name of top level folder
            this.DirectoryRecordForRootDirectory.FileIdentifierString = String.Empty;

            // populate this volume's directory structure
            dirInitStruct.IsoStream = isoStream;
            dirInitStruct.SourceFilePath = isoStream.Name;
            dirInitStruct.BaseOffset = this.VolumeBaseOffset;
            dirInitStruct.DirectoryRecord = this.DirectoryRecordForRootDirectory;
            dirInitStruct.LogicalBlockSize = this.LogicalBlockSize;
            dirInitStruct.IsRaw = this.IsRawDump;
            dirInitStruct.NonRawSectorSize = this.SectorSize;
            dirInitStruct.VolumeContainsXaData = this.ContainsCdxaData;
            dirInitStruct.ParentDirectory = null;

            Iso9660DirectoryStructure rootDirectory = new Iso9660DirectoryStructure(dirInitStruct);
            this.DirectoryStructureArray.Add(rootDirectory);
        }
    }

    public class Iso9660DirectoryRecord
    {
        public static readonly byte[] XA_SIGNATURE = new byte[] { 0x58, 0x41 };
        public const ushort XA_ATTR_MODE2FORM1 = (1 << 11);
        public const ushort XA_ATTR_MODE2FORM2 = (1 << 12);
        public const ushort XA_ATTR_INTERLEAVED = (1 << 13);
        public const ushort XA_ATTR_CDDA = (1 << 14);
        public const ushort XA_ATTR_DIRECTORY = (1 << 15);

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

        public Iso9660DirectoryRecord(byte[] directoryBytes, bool volumeContainsXaData)
        {
            byte[] xaAttributes;
            ushort xaItemDetails;

            this.LengthOfDirectoryRecord = directoryBytes[0];

            if (this.LengthOfDirectoryRecord > 0)
            {
                this.ExtendedAttributeRecordLength = directoryBytes[1];

                this.LocationOfExtent = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(directoryBytes, 2, 4), 0);
                this.DataLength = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(directoryBytes, 0x0A, 4), 0);

                if (ParseFile.CompareSegmentUsingSourceOffset(directoryBytes, 0x12, Iso9660.EMPTY_FILE_DATETIME.Length, Iso9660.EMPTY_FILE_DATETIME))
                {
                    this.RecordingDateAndTime = DateTime.MinValue;
                }
                else
                {
                    this.RecordingDateAndTime = new DateTime(directoryBytes[0x12] + 1900,
                                                             directoryBytes[0x13],
                                                             directoryBytes[0x14],
                                                             directoryBytes[0x15],
                                                             directoryBytes[0x16],
                                                             directoryBytes[0x17]);
                }
                this.FileFlags = directoryBytes[0x19];

                this.FlagExistance = (this.FileFlags & 0x1) == 0x1 ? true : false;
                this.FlagDirectory = (this.FileFlags & 0x2) == 0x2 ? true : false;
                this.FlagAssociatedFile = (this.FileFlags & 0x4) == 0x4 ? true : false;
                this.FlagRecord = (this.FileFlags & 0x08) == 0x08 ? true : false;
                this.FlagProtection = (this.FileFlags & 0x10) == 0x10 ? true : false;
                this.FlagMultiExtent = (this.FileFlags & 0x80) == 0x80 ? true : false;

                this.FileUnitSize = directoryBytes[0x1A];
                this.InterleaveGapSize = directoryBytes[0x1B];
                this.VolumeSequenceNumber = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(directoryBytes, 0x1C, 2), 0);
                this.LengthOfFileIdentifier = directoryBytes[0x20];

                this.FileIdentifier = ParseFile.ParseSimpleOffset(directoryBytes, 0x21, this.LengthOfFileIdentifier);

                // parse identifier
                if ((this.LengthOfFileIdentifier == 1) && (this.FileIdentifier[0] == 0))
                {
                    this.FileIdentifierString = ".";
                }
                else if ((this.LengthOfFileIdentifier == 1) && (this.FileIdentifier[0] == 1))
                {
                    this.FileIdentifierString = "..";
                }
                else if (this.LengthOfFileIdentifier > 0)
                {
                    this.FileIdentifierString = 
                        ByteConversion.GetEncodedText(this.FileIdentifier, 
                            ByteConversion.GetPredictedCodePageForTags(this.FileIdentifier));
                }

                if (this.LengthOfFileIdentifier % 2 == 0)
                {
                    this.PaddingField = ParseFile.ParseSimpleOffset(directoryBytes, 0x21 + this.LengthOfFileIdentifier, 1);
                }
                else
                {
                    this.PaddingField = new byte[0];
                }

                this.ItemMode = CdSectorType.Unknown;

                // CD-XA
                if (volumeContainsXaData)
                {
                    if (directoryBytes.Length >=
                        (0x21 + this.LengthOfFileIdentifier + this.PaddingField.Length + 0xE))
                    {
                        xaAttributes = ParseFile.ParseSimpleOffset(directoryBytes, 0x21 + this.LengthOfFileIdentifier + this.PaddingField.Length, 0xE); //verify cut size

                        // verify this is an XA entry
                        if (ParseFile.CompareSegmentUsingSourceOffset(xaAttributes, 6, XA_SIGNATURE.Length, XA_SIGNATURE))
                        {
                            xaItemDetails = ByteConversion.GetUInt16BigEndian(ParseFile.ParseSimpleOffset(xaAttributes, 4, 2));

                            if ((xaItemDetails & XA_ATTR_INTERLEAVED) == XA_ATTR_INTERLEAVED)
                            {
                                this.ItemMode = CdSectorType.XaInterleaved;
                                this.DataLength = (uint)(this.DataLength / (uint)CdRom.NON_RAW_SECTOR_SIZE) * (uint)CdRom.RAW_SECTOR_SIZE;
                            }
                            else if ((xaItemDetails & XA_ATTR_MODE2FORM1) == XA_ATTR_MODE2FORM1)
                            {
                                this.ItemMode = CdSectorType.Mode2Form1;
                            }
                            else if ((xaItemDetails & XA_ATTR_MODE2FORM2) == XA_ATTR_MODE2FORM2)
                            {
                                this.ItemMode = CdSectorType.Mode2Form2;
                                this.DataLength = (uint)(this.DataLength / (uint)CdRom.NON_RAW_SECTOR_SIZE) * (uint)CdRom.RAW_SECTOR_SIZE;
                            }
                            else if ((xaItemDetails & XA_ATTR_CDDA) == XA_ATTR_CDDA)
                            {
                                this.ItemMode = CdSectorType.Audio;
                                this.DataLength = (uint)(this.DataLength / (uint)CdRom.NON_RAW_SECTOR_SIZE) * (uint)CdRom.RAW_SECTOR_SIZE;
                            }                           
                            else
                            {
                                this.ItemMode = CdSectorType.Unknown;
                            }
                        }


                    }
                }
                
                /*           
                public byte[] SystemUse { set; get; }        
                */
            }
        }
    }

    public class Iso9660FileStructure : IFileStructure
    {
        public struct InitializeStruct
        {
            public string ParentDirectoryName { set; get; }
            public string SourceFilePath { set; get; }
            public string FileName { set; get; }
            public long VolumeBaseOffset { set; get; }
            public uint Lba { set; get; }
            public long Size { set; get; }
            public bool IsRaw { set; get; }
            public int NonRawSectorSize { set; get; }
            public CdSectorType FileMode { set; get; }
            public DateTime FileTime { set; get; }
        }
        
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
            if (obj is Iso9660FileStructure)
            {
                Iso9660FileStructure o = (Iso9660FileStructure)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not an Iso9660FileStructure");
        }    

        public Iso9660FileStructure(InitializeStruct initStruct)
        {
            this.ParentDirectoryName = initStruct.ParentDirectoryName;
            this.SourceFilePath = initStruct.SourceFilePath;
            this.FileName = initStruct.FileName;
            this.VolumeBaseOffset = initStruct.VolumeBaseOffset;
            this.Lba = initStruct.Lba;
            this.IsRaw = initStruct.IsRaw;
            this.NonRawSectorSize = initStruct.NonRawSectorSize;
            this.Size = initStruct.Size;
            this.FileMode = initStruct.FileMode;
            this.FileDateTime = initStruct.FileTime;
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

    public class Iso9660DirectoryStructure : IDirectoryStructure
    {
        public struct InitializeStruct
        {
            public FileStream IsoStream { set; get; }
            public string SourceFilePath { set; get; }
            public long BaseOffset { set; get; }
            public Iso9660DirectoryRecord DirectoryRecord { set; get; }
            public uint LogicalBlockSize { set; get; }
            public bool IsRaw { set; get; }
            public int NonRawSectorSize { set; get; }
            public bool VolumeContainsXaData { set; get; }
            public string ParentDirectory { set; get; }
        }
        
        public Iso9660DirectoryRecord ParentDirectoryRecord { set; get; }
        public string ParentDirectoryName { set; get; }
        
        public ArrayList SubDirectoryArray { set; get; }
        public IDirectoryStructure[] SubDirectories {
            set { this.SubDirectories = value; }
            get 
            {
                SubDirectoryArray.Sort();
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(Iso9660DirectoryStructure)); 
            }
        }
        
        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { this.Files = value; }
            get 
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(Iso9660FileStructure)); 
            }
        }

        public string SourceFilePath { set; get; }
        public string DirectoryName { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is Iso9660DirectoryStructure)
            {
                Iso9660DirectoryStructure o = (Iso9660DirectoryStructure)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not an Iso9660DirectoryStructure");
        }

        public Iso9660DirectoryStructure(InitializeStruct initStruct)
        {
            InitializeStruct dirInitStruct = new InitializeStruct();
            string nextDirectory;
            this.SourceFilePath = initStruct.SourceFilePath;
            this.SubDirectoryArray = new ArrayList();
            this.FileArray = new ArrayList();

            if (String.IsNullOrEmpty(initStruct.ParentDirectory))
            {
                this.ParentDirectoryName = String.Empty;
                this.DirectoryName = initStruct.DirectoryRecord.FileIdentifierString;
                nextDirectory = this.DirectoryName;
            }
            else
            {
                this.ParentDirectoryName = initStruct.ParentDirectory;
                this.DirectoryName = initStruct.DirectoryRecord.FileIdentifierString;
                nextDirectory = this.ParentDirectoryName + Path.DirectorySeparatorChar + this.DirectoryName;
            }

            dirInitStruct = initStruct;
            dirInitStruct.ParentDirectory = nextDirectory;

            this.parseDirectoryRecord(dirInitStruct);            
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string fullDirectoryPath = Path.Combine(destinationFolder, Path.Combine(this.ParentDirectoryName, this.DirectoryName));
            
            // create directory
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            foreach (Iso9660FileStructure f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (Iso9660DirectoryStructure d in this.SubDirectoryArray)
            {
                d.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }
        }

        private void parseDirectoryRecord(InitializeStruct dirInitStruct)
        {
            byte recordSize;
            int currentOffset;
            uint bytesRead = 0;
            uint currentLba = dirInitStruct.DirectoryRecord.LocationOfExtent;
            byte[] directoryRecordBytes;
            Iso9660DirectoryRecord tempDirectoryRecord;
            Iso9660DirectoryStructure tempDirectory;
            Iso9660DirectoryStructure.InitializeStruct newDirInitStruct;

            Iso9660FileStructure tempFile;
            Iso9660FileStructure.InitializeStruct fileInitStruct = new Iso9660FileStructure.InitializeStruct();

            byte[] directorySector = CdRom.GetSectorByLba(dirInitStruct.IsoStream, dirInitStruct.BaseOffset, currentLba, dirInitStruct.IsRaw, dirInitStruct.NonRawSectorSize);
            directorySector = CdRom.GetDataChunkFromSector(directorySector, dirInitStruct.IsRaw);
            
            currentOffset = 0;

            while (bytesRead < dirInitStruct.DirectoryRecord.DataLength)
            {
                recordSize = ParseFile.ParseSimpleOffset(directorySector, currentOffset, 1)[0];

                if (recordSize > 0)
                {
                    try
                    {
                        directoryRecordBytes = ParseFile.ParseSimpleOffset(directorySector, currentOffset, recordSize);
                        tempDirectoryRecord = new Iso9660DirectoryRecord(directoryRecordBytes, dirInitStruct.VolumeContainsXaData);

                        if (!tempDirectoryRecord.FileIdentifierString.Equals(".") &&
                            !tempDirectoryRecord.FileIdentifierString.Equals("..")) // skip "this" directory
                        {
                            if (tempDirectoryRecord.FlagDirectory)
                            {
                                newDirInitStruct = dirInitStruct;
                                newDirInitStruct.DirectoryRecord = tempDirectoryRecord;

                                tempDirectory = new Iso9660DirectoryStructure(newDirInitStruct);
                                this.SubDirectoryArray.Add(tempDirectory);
                            }
                            else
                            {
                                fileInitStruct.ParentDirectoryName = dirInitStruct.ParentDirectory;
                                fileInitStruct.SourceFilePath = this.SourceFilePath;
                                fileInitStruct.FileName = tempDirectoryRecord.FileIdentifierString.Replace(";1", String.Empty);
                                fileInitStruct.VolumeBaseOffset = dirInitStruct.BaseOffset;
                                fileInitStruct.Lba = tempDirectoryRecord.LocationOfExtent;
                                fileInitStruct.Size = tempDirectoryRecord.DataLength;
                                fileInitStruct.IsRaw = dirInitStruct.IsRaw;
                                fileInitStruct.NonRawSectorSize = dirInitStruct.NonRawSectorSize;
                                fileInitStruct.FileMode = tempDirectoryRecord.ItemMode;
                                fileInitStruct.FileTime = tempDirectoryRecord.RecordingDateAndTime;

                                tempFile = new Iso9660FileStructure(fileInitStruct);
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
                    catch (Exception ex)
                    {
                        throw new Exception(String.Format("Error parsing directory structure at offset: {0}, {1}", currentOffset, ex.Message));
                    }
                }
                else if ((dirInitStruct.DirectoryRecord.DataLength - bytesRead) > (directorySector.Length - currentOffset))
                {
                    // move to start of next sector
                    directorySector = CdRom.GetSectorByLba(dirInitStruct.IsoStream, dirInitStruct.BaseOffset, ++currentLba, dirInitStruct.IsRaw, dirInitStruct.NonRawSectorSize);
                    directorySector = CdRom.GetDataChunkFromSector(directorySector, dirInitStruct.IsRaw);
                    bytesRead += (uint)(dirInitStruct.LogicalBlockSize - currentOffset);
                    currentOffset = 0;                
                }
                else
                {
                    break;
                }
            }
        }        
    }

    public class Iso9660PathTable
    {
        public bool IsLittleEndian { set; get; }
        public uint PathTableSize { set; get; }
        public Iso9660PathTableRecord[] PathTableRecords { set; get; }
        ArrayList PathTableRecordsList { set; get; }

        public Iso9660PathTable(uint lba, uint pathTableSize, bool isLittleEndian)
        { 
            //uint pathTableSectorCount = pathTableSectorCount
        }

    }

    public class Iso9660PathTableRecord
    {
        public byte LengthOfDirectoryIdentifier { set; get; }
        public byte ExtendedAttributeRecordLength { set; get; }
        public uint LocationOfExtent { set; get; }
        public ushort DirectoryNumberOfParentDirectory { set; get; }
        public byte[] DirectoryIdentifierBytes { set; get; }
        public string DirectoryIdentifier { set; get; }
        public byte PaddingByte { set; get; }

        public Iso9660PathTableRecord(byte[] pathBytes, bool isLittleEndian)
        {
            this.LengthOfDirectoryIdentifier = ParseFile.ParseSimpleOffset(pathBytes, 0, 1)[0];
            this.ExtendedAttributeRecordLength = ParseFile.ParseSimpleOffset(pathBytes, 1, 1)[0];
            
            if (isLittleEndian)
            {
                this.LocationOfExtent = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pathBytes, 2, 4), 0);
            }
            else
            {
                this.LocationOfExtent = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(pathBytes, 2, 4));
            }
            if (isLittleEndian)
            {
                this.DirectoryNumberOfParentDirectory = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(pathBytes, 6, 2), 0);
            }
            else
            {
                this.DirectoryNumberOfParentDirectory = ByteConversion.GetUInt16BigEndian(ParseFile.ParseSimpleOffset(pathBytes, 6, 2));            
            }

            this.DirectoryIdentifierBytes = ParseFile.ParseSimpleOffset(pathBytes, 8, this.LengthOfDirectoryIdentifier);
            this.DirectoryIdentifier = 
                ByteConversion.GetEncodedText(this.DirectoryIdentifierBytes, 
                    ByteConversion.GetPredictedCodePageForTags(this.DirectoryIdentifierBytes));
        
            if (this.LengthOfDirectoryIdentifier % 2 == 1)
            {
                this.PaddingByte = ParseFile.ParseSimpleOffset(pathBytes, this.LengthOfDirectoryIdentifier + 8, 1)[0];
            }
        }
    }
}
