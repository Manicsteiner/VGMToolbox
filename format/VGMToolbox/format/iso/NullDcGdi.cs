using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VGMToolbox.util;

namespace VGMToolbox.format.iso
{
    public class NullDcGdi
    {
        public enum TrackType
        {
            Audio,
            Data
        };
        
        public struct TrackEntry
        {
            public uint TrackNumber { set; get; }
            public uint StartSector { set; get; }
            public TrackType EntryType { set; get; }
            public uint SectorSize { set; get; }
            public string FilePath { set; get; }
            public long Offset { set; get; }
            public FileStream TrackStream { set; get; }
        }
        
        public static string FORMAT_DESCRIPTION_STRING = "Dreamcast GDI";
        public const string GDI_CTRL_AUDIO = "0";
        public const string GDI_CTRL_DATA = "4";

        public int TrackCount { set; get; }
        public TrackEntry[] TrackEntries { set; get; }

        public ArrayList VolumeList { set; get; }
        public IVolume[] Volumes
        {
            set
            {
                this.Volumes = value;
            }

            get
            {
                return (IVolume[])this.VolumeList.ToArray(typeof(IVolume));
            }
        }
        
        public NullDcGdi(string gdiPath)
        {
            CdAudio audioVolume;
            NullDcGdiVolume gdiVolume;
            
            long currentOffset;
            long fileSize;

            byte[] sectorBytes;
            byte[] sectorDataBytes;
            byte[] volumeIdBytes;

            try
            {
                // parse GDI
                this.ParseGdiFile(gdiPath);

                // create volumes
                this.VolumeList = new ArrayList();
                
                if (this.TrackEntries != null)
                {
                    for (int i = 0; i < this.TrackEntries.Length; i++)
                    {
                        if (this.TrackEntries[i].EntryType == TrackType.Audio)
                        {
                            audioVolume = new CdAudio();
                            audioVolume.Initialize(null, 0, true);
                            audioVolume.VolumeIdentifier = String.Format("Track {0}", this.TrackEntries[i].TrackNumber.ToString("D2"));
                            this.VolumeList.Add(audioVolume);
                        }
                        else
                        {
                            currentOffset = 0;
                            fileSize = this.TrackEntries[i].TrackStream.Length;

                            while (currentOffset < fileSize)
                            {
                                // get sector
                                sectorBytes = ParseFile.ParseSimpleOffset(this.TrackEntries[i].TrackStream, currentOffset, (int)this.TrackEntries[i].SectorSize);

                                // get data bytes from sector
                                sectorDataBytes = CdRom.GetDataChunkFromSector(sectorBytes, (this.TrackEntries[i].SectorSize == CdRom.RAW_SECTOR_SIZE));

                                // get header bytes
                                volumeIdBytes = ParseFile.ParseSimpleOffset(sectorDataBytes, 0, 0x100);

                                if (ParseFile.CompareSegmentUsingSourceOffset(volumeIdBytes, 0, Iso9660.VOLUME_DESCRIPTOR_IDENTIFIER.Length, Iso9660.VOLUME_DESCRIPTOR_IDENTIFIER))
                                {
                                    gdiVolume = new NullDcGdiVolume(this);
                                    gdiVolume.Initialize(this.TrackEntries[i].TrackStream, currentOffset, (this.TrackEntries[i].SectorSize == CdRom.RAW_SECTOR_SIZE));
                                    this.VolumeList.Add((IVolume)gdiVolume);
                                }

                                currentOffset += this.TrackEntries[i].SectorSize;
                            }                                                                              
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException(String.Format("Error loading GDI: {0}.", ex.Message));
            }
            finally
            { 
                // close all streams
                if (this.TrackEntries != null)
                {
                    for (int i = 0; i < this.TrackEntries.Length; i++)
                    {
                        if (this.TrackEntries[i].TrackStream.CanRead)
                        {
                            this.TrackEntries[i].TrackStream.Close();
                            this.TrackEntries[i].TrackStream.Dispose();
                        }
                    }
                }
            }
        }

        public byte[] GetSectorByLba(long lba)
        {
            long sectorOffset;
            byte[] sectorBytes = null;

            for (int i = (this.TrackCount - 1); i >= 0; i--)
            {
                if (lba >= this.TrackEntries[i].StartSector)
                {
                    sectorOffset = ((lba - this.TrackEntries[i].StartSector) * this.TrackEntries[i].SectorSize) + this.TrackEntries[i].Offset;
                    sectorBytes = ParseFile.ParseSimpleOffset(this.TrackEntries[i].TrackStream, sectorOffset, (int)this.TrackEntries[i].SectorSize);
                    break;
                }
            }            
            
            return sectorBytes;
        }

        public void ExtractGdRomData(Stream cdStream,
            string destinationPath, long volumeBaseOffset,
            long lba, long length, bool isRaw, long nonRawSectorSize,
            CdSectorType fileMode, bool extractAsRaw)
        {
            long offset;
            long adjustedLba;
            int maxWriteSize;
            long bytesWritten = 0;

            CdSectorType mode;
            long lbaCounter = 0;

            byte[] sectorHeader;
            byte[] sector;

            // create directory
            string destinationFolder = Path.GetDirectoryName(destinationPath);

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            for (int i = (this.TrackCount - 1); i >= 0; i--)
            {
                if (lba >= this.TrackEntries[i].StartSector)
                {
                    if (isRaw)
                    {
                        mode = fileMode;

                        using (FileStream outStream = File.OpenWrite(destinationPath))
                        {
                            adjustedLba = lba - this.TrackEntries[i].StartSector;
                            
                            while (bytesWritten < length)
                            {

                                offset = volumeBaseOffset + ((adjustedLba + lbaCounter) * this.TrackEntries[i].SectorSize);
                                sector = ParseFile.ParseSimpleOffset(cdStream, offset, (int)CdRom.RAW_SECTOR_SIZE);
                                sectorHeader = ParseFile.ParseSimpleOffset(sector, 0, CdRom.MAX_HEADER_SIZE);

                                if (mode == CdSectorType.Unknown)
                                {
                                    mode = CdRom.GetSectorType(sectorHeader);
                                }

                                maxWriteSize = CdRom.ModeDataSize[mode] < (length - bytesWritten) ? CdRom.ModeDataSize[mode] : (int)(length - bytesWritten);

                                if (extractAsRaw)
                                {
                                    outStream.Write(sector, 0, sector.Length);
                                }
                                else
                                {
                                    outStream.Write(sector, CdRom.ModeHeaderSize[mode], maxWriteSize);
                                }

                                bytesWritten += maxWriteSize;
                                lbaCounter++;
                            }
                        }
                    }
                    else
                    {
                        offset = ((lba - this.TrackEntries[i].StartSector) * this.TrackEntries[i].SectorSize) + this.TrackEntries[i].Offset;
                        ParseFile.ExtractChunkToFile(cdStream, offset, length, destinationPath);
                    }        

                    break;
                }
            }            
        }

        public string GetFilePathForLba(uint sectorLba)
        {
            string fileName = null;

            for (int i = (this.TrackCount - 1); i >= 0; i--)
            {
                if (sectorLba >= this.TrackEntries[i].StartSector)
                {
                    fileName = this.TrackEntries[i].FilePath;
                    break;
                }
            }

            return fileName;
        }
        
        private void ParseGdiFile(string gdiPath)
        {
            string trackCount;
            string trackEntryLine;
            string[] trackEntryLineArray;
            string[] splitDelimeters = new string[] { " " };

            int openQuoteIndex;
            int closeQuoteIndex;

            using (FileStream gdiStream = File.OpenRead(gdiPath))
            {
                using (StreamReader gdiReader = new StreamReader(gdiStream))
                { 
                    // get track count
                    trackCount = gdiReader.ReadLine().Trim();
                    this.TrackCount = int.Parse(trackCount);

                    // populate track entries
                    this.TrackEntries = new TrackEntry[this.TrackCount];

                    for (int i = 0; i < this.TrackCount; i++)
                    {
                        trackEntryLine = gdiReader.ReadLine().Trim();
                        
                        // check for quotes for filename handling
                        openQuoteIndex = trackEntryLine.IndexOf('\"', 0);
                        closeQuoteIndex = trackEntryLine.IndexOf('\"', openQuoteIndex + 1);

                        trackEntryLineArray = trackEntryLine.Split(splitDelimeters, StringSplitOptions.RemoveEmptyEntries);

                        this.TrackEntries[i] = new TrackEntry();
                        this.TrackEntries[i].TrackNumber = uint.Parse(trackEntryLineArray[0].Trim());
                        this.TrackEntries[i].StartSector = uint.Parse(trackEntryLineArray[1].Trim());
                        
                        switch (trackEntryLineArray[2].Trim())
                        {
                            case NullDcGdi.GDI_CTRL_AUDIO:
                                this.TrackEntries[i].EntryType = TrackType.Audio;
                                break;
                            case NullDcGdi.GDI_CTRL_DATA:
                                this.TrackEntries[i].EntryType = TrackType.Data;
                                break;
                        }

                        this.TrackEntries[i].SectorSize = uint.Parse(trackEntryLineArray[3].Trim());

                        if ((openQuoteIndex > 0) && (closeQuoteIndex > 0))
                        {
                            this.TrackEntries[i].FilePath =
                                Path.Combine(Path.GetDirectoryName(gdiPath), trackEntryLine.Substring(openQuoteIndex + 1, closeQuoteIndex - openQuoteIndex - 1).Trim());

                            this.TrackEntries[i].Offset = long.Parse(trackEntryLineArray[trackEntryLineArray.GetUpperBound(0)].Trim());                        
                        }
                        else
                        {

                            this.TrackEntries[i].FilePath = Path.Combine(Path.GetDirectoryName(gdiPath), trackEntryLineArray[4].Trim());
                            this.TrackEntries[i].Offset = long.Parse(trackEntryLineArray[5].Trim());
                        }

                        // verify file exists
                        if (!File.Exists(this.TrackEntries[i].FilePath))
                        {
                            throw new FileNotFoundException(String.Format("Cannot find file referenced by GDI: {0}", this.TrackEntries[i].FilePath));
                        }
                        else
                        {
                            this.TrackEntries[i].TrackStream = File.OpenRead(this.TrackEntries[i].FilePath);
                        }
                    }
                }
            }
        }
    }

    public class NullDcGdiDirectoryRecord
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

        public NullDcGdiDirectoryRecord(byte[] directoryBytes, bool volumeContainsXaData)
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

    public class NullDcGdiFileStructure : IFileStructure
    {
        public struct InitializeStruct
        {
            public NullDcGdi Gdi { set; get; }
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

        public NullDcGdi Gdi { set; get; }
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
            if (obj is NullDcGdiFileStructure)
            {
                NullDcGdiFileStructure o = (NullDcGdiFileStructure)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not an NullDcGdiFileStructure");
        }

        public NullDcGdiFileStructure(InitializeStruct initStruct)
        {
            this.Gdi = initStruct.Gdi;
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

            this.Gdi.ExtractGdRomData(streamCache[this.SourceFilePath], destinationFile,
                this.VolumeBaseOffset, this.Lba, this.Size,
                this.IsRaw, this.NonRawSectorSize, this.FileMode, extractAsRaw);
            
        }
    }

    public class NullDcGdiDirectoryStructure : IDirectoryStructure
    {
        public struct InitializeStruct
        {
            public NullDcGdi Gdi { set; get; }
            public string SourceFilePath { set; get; }
            public long BaseOffset { set; get; }
            public NullDcGdiDirectoryRecord DirectoryRecord { set; get; }
            public uint LogicalBlockSize { set; get; }
            public bool IsRaw { set; get; }
            public int NonRawSectorSize { set; get; }
            public bool VolumeContainsXaData { set; get; }
            public string ParentDirectory { set; get; }           
        }

        public NullDcGdiDirectoryRecord ParentDirectoryRecord { set; get; }
        public string ParentDirectoryName { set; get; }

        public ArrayList SubDirectoryArray { set; get; }
        public IDirectoryStructure[] SubDirectories
        {
            set { this.SubDirectories = value; }
            get
            {
                SubDirectoryArray.Sort();
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(NullDcGdiDirectoryStructure));
            }
        }

        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { this.Files = value; }
            get
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(NullDcGdiFileStructure));
            }
        }

        public string SourceFilePath { set; get; }
        public string DirectoryName { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is NullDcGdiDirectoryStructure)
            {
                NullDcGdiDirectoryStructure o = (NullDcGdiDirectoryStructure)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not an NullDcGdiDirectoryStructure");
        }

        public NullDcGdiDirectoryStructure(InitializeStruct initStruct)
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

            foreach (NullDcGdiFileStructure f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (NullDcGdiDirectoryStructure d in this.SubDirectoryArray)
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
            NullDcGdiDirectoryRecord tempDirectoryRecord;
            NullDcGdiDirectoryStructure tempDirectory;
            NullDcGdiDirectoryStructure.InitializeStruct newDirInitStruct;

            NullDcGdiFileStructure tempFile;
            NullDcGdiFileStructure.InitializeStruct fileInitStruct = new NullDcGdiFileStructure.InitializeStruct();

            byte[] directorySector = dirInitStruct.Gdi.GetSectorByLba(currentLba);
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
                        tempDirectoryRecord = new NullDcGdiDirectoryRecord(directoryRecordBytes, dirInitStruct.VolumeContainsXaData);

                        if (!tempDirectoryRecord.FileIdentifierString.Equals(".") &&
                            !tempDirectoryRecord.FileIdentifierString.Equals("..")) // skip "this" directory
                        {
                            if (tempDirectoryRecord.FlagDirectory)
                            {
                                newDirInitStruct = dirInitStruct;
                                newDirInitStruct.DirectoryRecord = tempDirectoryRecord;

                                tempDirectory = new NullDcGdiDirectoryStructure(newDirInitStruct);
                                this.SubDirectoryArray.Add(tempDirectory);
                            }
                            else
                            {
                                fileInitStruct.Gdi = dirInitStruct.Gdi;
                                fileInitStruct.ParentDirectoryName = dirInitStruct.ParentDirectory;
                                fileInitStruct.SourceFilePath = dirInitStruct.Gdi.GetFilePathForLba(tempDirectoryRecord.LocationOfExtent);
                                fileInitStruct.FileName = tempDirectoryRecord.FileIdentifierString.Replace(";1", String.Empty);
                                fileInitStruct.VolumeBaseOffset = dirInitStruct.BaseOffset;
                                fileInitStruct.Lba = tempDirectoryRecord.LocationOfExtent;
                                fileInitStruct.Size = tempDirectoryRecord.DataLength;
                                fileInitStruct.IsRaw = dirInitStruct.IsRaw;
                                fileInitStruct.NonRawSectorSize = dirInitStruct.NonRawSectorSize;
                                fileInitStruct.FileMode = tempDirectoryRecord.ItemMode;
                                fileInitStruct.FileTime = tempDirectoryRecord.RecordingDateAndTime;

                                tempFile = new NullDcGdiFileStructure(fileInitStruct);
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
                    directorySector = dirInitStruct.Gdi.GetSectorByLba(++currentLba);
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

    public class NullDcGdiVolume : IVolume
    {
        public NullDcGdi ParentGdi { set; get; }
        
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
                return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(NullDcGdiDirectoryStructure));
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
        public NullDcGdiDirectoryRecord DirectoryRecordForRootDirectory { set; get; }

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

        public NullDcGdiVolume(NullDcGdi gdi)
        {
            this.ParentGdi = gdi;
        }

        public virtual void Initialize(FileStream isoStream, long offset, bool isRawDump)
        {
            byte[] sectorBytes;
            byte[] sectorDataBytes;

            this.FormatDescription = NullDcGdi.FORMAT_DESCRIPTION_STRING;
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
            this.DirectoryRecordForRootDirectory = new NullDcGdiDirectoryRecord(this.DirectoryRecordForRootDirectoryBytes, this.ContainsCdxaData);

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
            foreach (NullDcGdiDirectoryStructure ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, destintionFolder, extractAsRaw);
            }
        }

        public virtual void LoadDirectories(FileStream isoStream)
        {
            NullDcGdiDirectoryStructure.InitializeStruct dirInitStruct = new NullDcGdiDirectoryStructure.InitializeStruct();

            // change name of top level folder
            this.DirectoryRecordForRootDirectory.FileIdentifierString = String.Empty;

            // populate this volume's directory structure
            dirInitStruct.Gdi = this.ParentGdi;
            dirInitStruct.SourceFilePath = isoStream.Name;
            dirInitStruct.BaseOffset = this.VolumeBaseOffset;
            dirInitStruct.DirectoryRecord = this.DirectoryRecordForRootDirectory;
            dirInitStruct.LogicalBlockSize = this.LogicalBlockSize;
            dirInitStruct.IsRaw = this.IsRawDump;
            dirInitStruct.NonRawSectorSize = this.SectorSize;
            dirInitStruct.VolumeContainsXaData = this.ContainsCdxaData;
            dirInitStruct.ParentDirectory = null;

            NullDcGdiDirectoryStructure rootDirectory = new NullDcGdiDirectoryStructure(dirInitStruct);
            this.DirectoryStructureArray.Add(rootDirectory);
        }
    }
}
