using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.util;

namespace VGMToolbox.format.iso
{
    // Thanks to WiiBrew's File System Wiki.
    public class NintendoWiiOpticalDisc
    {
        public struct WiiBlockStructure
        {
            public long BlockNumber { set; get; }
            public long BlockOffset { set; get; }                        
        }

        public struct PartitionEntry
        {
            public long PartitionOffset { set; get; }
            public byte[] PartitionType { set; get; }

            public byte[] TitleId { set; get; }
            public byte[] EncryptedTitleKey { set; get; }
            public byte[] DecryptedTitleKey { set; get; }
            public byte CommonKeyIndex { set; get; }

            public long RelativeDataOffset { set; get; }
            public long DataSize { set; get; }
        }

        public struct Partition
        {
            public uint PartitionCount { set; get; }
            public long PartitionTableOffset { set; get; }
            public PartitionEntry[] PartitionEntries { set; get; }
        }

        public static readonly byte[] STANDARD_IDENTIFIER = new byte[] { 0x5D, 0x1C, 0x9E, 0xA3 };
        public const uint IDENTIFIER_OFFSET = 0x18;
        public static string FORMAT_DESCRIPTION_STRING = "Wii Encrypted";
        public static string FORMAT_DESCRIPTION_STRING_DECRYPTED = "Wii Decrypted";

        private static readonly string PROGRAMS_FOLDER =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"));
        public static readonly string WII_EXTERNAL_FOLDER = Path.Combine(PROGRAMS_FOLDER, "wii");
        public static readonly string COMMON_KEY_PATH = Path.Combine(WII_EXTERNAL_FOLDER, "ckey.bin");
        public static readonly string KOREAN_KEY_PATH = Path.Combine(WII_EXTERNAL_FOLDER, "kkey.bin");

        public string WiiDiscId { set; get; }
        public string GameCode { set; get; }
        public string RegionCode { set; get; }
        public byte[] MakerCode { set; get; }

        public long DiscBaseOffset { set; get; }
        public bool IsRawDump { set; get; }

        public byte[] CommonKey { set; get; }
        public byte[] KoreanCommonKey { set; get; }

        public Partition[] Partitions { set; get; }
        public ArrayList VolumeArrayList { set; get; }
        public NintendoWiiOpticalDiscVolume[] Volumes 
        { 
            get 
            {
                return (NintendoWiiOpticalDiscVolume[])this.VolumeArrayList.ToArray(typeof(NintendoWiiOpticalDiscVolume));
            }
        
        }

        public NintendoWiiEncryptedDiscReader DiscReader { set; get; }

        public static byte[] GetKeyFromFile(string path)
        {
            byte[] keyArray = null;

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(String.Format("Key not found: {0}.{1}", path, Environment.NewLine));
            }
            else
            {
                keyArray = new byte[0x10];
                
                using (FileStream fs = File.OpenRead(path))
                {
                    fs.Read(keyArray, 0, 0x10);
                }
            }

            return keyArray;
        }

        public static WiiBlockStructure GetWiiBlockStructureForOffset(long offset)
        {
            WiiBlockStructure bs = new WiiBlockStructure();
            
            bs.BlockNumber = offset / 0x7C00;
            bs.BlockOffset = offset % 0x7C00;

            return bs;
        }

        public void Initialize(FileStream isoStream, long offset, bool isRawDump)
        {
            byte[] volumeIdentifierBytes;

            this.DiscBaseOffset = offset;
            this.DiscReader = new NintendoWiiEncryptedDiscReader();

            this.WiiDiscId = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(isoStream, this.DiscBaseOffset, 1));
            this.GameCode = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(isoStream, this.DiscBaseOffset + 1, 2));
            this.RegionCode = ByteConversion.GetAsciiText(ParseFile.ParseSimpleOffset(isoStream, this.DiscBaseOffset + 3, 1));
            this.MakerCode = ParseFile.ParseSimpleOffset(isoStream, this.DiscBaseOffset + 4, 2);

            this.IsRawDump = isRawDump;

            // get identifier
            volumeIdentifierBytes = ParseFile.ParseSimpleOffset(isoStream, this.DiscBaseOffset + 0x20, 64);
            volumeIdentifierBytes = FileUtil.ReplaceNullByteWithSpace(volumeIdentifierBytes);

            // initialize partition info
            this.InitializePartitions(isoStream);

            // initialize volumes
            this.VolumeArrayList = new ArrayList();
            this.LoadVolumes(isoStream);

        }

        public void InitializePartitions(FileStream isoStream)
        {
            //byte[] encryptedPartitionCluster;
            //byte[] decryptedClusterDataSection;
            //byte[] clusterIV;
            //byte[] encryptedClusterDataSection;

            this.CommonKey = null;
            this.KoreanCommonKey = null;
            this.Partitions = new Partition[4];

            for (int i = 0; i < 4; i++)
            {
                this.Partitions[i] = new Partition();
                this.Partitions[i].PartitionCount = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(isoStream, this.DiscBaseOffset + 0x40000 + (i * 8), 4));
                this.Partitions[i].PartitionEntries = new PartitionEntry[this.Partitions[i].PartitionCount];

                this.Partitions[i].PartitionTableOffset = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(isoStream, this.DiscBaseOffset + 0x40004 + (i * 8), 4));
                this.Partitions[i].PartitionTableOffset <<= 2;

                if (this.Partitions[i].PartitionTableOffset > 0)
                {
                    // set absolute offset of partition
                    this.Partitions[i].PartitionTableOffset += this.DiscBaseOffset;

                    for (int j = 0; j < this.Partitions[i].PartitionCount; j++)
                    {
                        this.Partitions[i].PartitionEntries[j] = new PartitionEntry();

                        // get offset to this partition
                        this.Partitions[i].PartitionEntries[j].PartitionOffset =
                            ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(isoStream, this.Partitions[i].PartitionTableOffset + (j * 8), 4));
                        this.Partitions[i].PartitionEntries[j].PartitionOffset <<= 2;
                        this.Partitions[i].PartitionEntries[j].PartitionOffset += this.DiscBaseOffset;

                        // get partition type
                        this.Partitions[i].PartitionEntries[j].PartitionType =
                            ParseFile.ParseSimpleOffset(isoStream, this.Partitions[i].PartitionTableOffset + 4 + (i * 8), 4);

                        // get relative offset partition's data section
                        this.Partitions[i].PartitionEntries[j].RelativeDataOffset = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(isoStream, this.Partitions[i].PartitionEntries[j].PartitionOffset + 0x02B8, 4));
                        this.Partitions[i].PartitionEntries[j].RelativeDataOffset <<= 2;
                        this.Partitions[i].PartitionEntries[j].RelativeDataOffset += this.DiscBaseOffset;

                        // get the size of partition's data section
                        this.Partitions[i].PartitionEntries[j].DataSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(isoStream, this.Partitions[i].PartitionEntries[j].PartitionOffset + 0x02BC, 4));
                        this.Partitions[i].PartitionEntries[j].DataSize <<= 2;

                        //---------------------------
                        // parse this entry's ticket
                        //---------------------------
                        this.Partitions[i].PartitionEntries[j].TitleId = new byte[0x10];
                        Array.Copy(
                            ParseFile.ParseSimpleOffset(isoStream, this.Partitions[i].PartitionEntries[j].PartitionOffset + 0x01DC, 8),
                            0,
                            this.Partitions[i].PartitionEntries[j].TitleId,
                            0,
                            8);

                        this.Partitions[i].PartitionEntries[j].EncryptedTitleKey = ParseFile.ParseSimpleOffset(isoStream, this.Partitions[i].PartitionEntries[j].PartitionOffset + 0x01BF, 0x10);
                        this.Partitions[i].PartitionEntries[j].CommonKeyIndex = ParseFile.ParseSimpleOffset(isoStream, this.Partitions[i].PartitionEntries[j].PartitionOffset + 0x01F1, 1)[0];

                        //---------------------
                        // decrypt the TitleId
                        //---------------------
                        switch (this.Partitions[i].PartitionEntries[j].CommonKeyIndex)
                        {
                            case 0:
                                if (this.CommonKey == null)
                                {
                                    this.CommonKey = NintendoWiiOpticalDisc.GetKeyFromFile(NintendoWiiOpticalDisc.COMMON_KEY_PATH);
                                }

                                this.Partitions[i].PartitionEntries[j].DecryptedTitleKey =
                                    AESEngine.Decrypt(this.Partitions[i].PartitionEntries[j].EncryptedTitleKey,
                                        this.CommonKey, this.Partitions[i].PartitionEntries[j].TitleId,
                                        CipherMode.CBC, PaddingMode.Zeros);

                                break;
                            case 1:
                                if (this.KoreanCommonKey == null)
                                {
                                    this.KoreanCommonKey = NintendoWiiOpticalDisc.GetKeyFromFile(NintendoWiiOpticalDisc.KOREAN_KEY_PATH);
                                }

                                this.Partitions[i].PartitionEntries[j].DecryptedTitleKey =
                                    AESEngine.Decrypt(this.Partitions[i].PartitionEntries[j].EncryptedTitleKey,
                                        this.KoreanCommonKey, this.Partitions[i].PartitionEntries[j].TitleId,
                                        CipherMode.CBC, PaddingMode.Zeros);

                                break;
                        } // switch (this.Partitions[i].PartitionEntries[j].CommonKeyIndex)


                        //string outFile = Path.Combine(Path.GetDirectoryName(isoStream.Name), String.Format("{0}-{1}.bin", i.ToString(), j.ToString()));                                                
                        //long currentOffset = 0;

                        //using (FileStream outStream = File.OpenWrite(outFile))
                        //{

                        //    while (currentOffset < this.Partitions[i].PartitionEntries[j].DataSize)
                        //    {
                        //        encryptedPartitionCluster = ParseFile.ParseSimpleOffset(isoStream,
                        //            this.Partitions[i].PartitionEntries[j].PartitionOffset + this.Partitions[i].PartitionEntries[j].RelativeDataOffset + currentOffset,
                        //            0x8000);

                        //        clusterIV = ParseFile.ParseSimpleOffset(encryptedPartitionCluster, 0x03D0, 0x10);
                        //        encryptedClusterDataSection = ParseFile.ParseSimpleOffset(encryptedPartitionCluster, 0x400, 0x7C00);
                        //        decryptedClusterDataSection = AESEngine.Decrypt(encryptedClusterDataSection,
                        //                        this.Partitions[i].PartitionEntries[j].DecryptedTitleKey, clusterIV,
                        //                        CipherMode.CBC, PaddingMode.Zeros);

                        //        outStream.Write(decryptedClusterDataSection, 0, decryptedClusterDataSection.Length);

                        //        currentOffset += 0x8000;
                        //    }
                        //}

                    } // for (int j = 0; j < this.Partitions[i].PartitionCount; j++)
                } // if (this.Partitions[i].PartitionTableOffset > 0)
            }
        }

        public void LoadVolumes(FileStream isoStream)
        {
            NintendoWiiOpticalDiscVolume newVolume;
            
            for (int i = 0; i < 4; i++)
            {
                if (this.Partitions[i].PartitionTableOffset > 0)
                {
                    for (int j = 0; j < this.Partitions[i].PartitionCount; j++)
                    {
                        newVolume = new NintendoWiiOpticalDiscVolume(
                            this.Partitions[i].PartitionEntries[j].PartitionOffset,
                            this.Partitions[i].PartitionEntries[j].RelativeDataOffset,
                            this.Partitions[i].PartitionEntries[j].DecryptedTitleKey,
                            this.DiscReader);

                        newVolume.Initialize(isoStream, this.Partitions[i].PartitionEntries[j].PartitionOffset, this.IsRawDump);
                        this.VolumeArrayList.Add(newVolume);
                    }
                }
            }        
        }
    }

    public class NintendoWiiOpticalDiscVolume : IVolume
    {
        public string VolumeIdentifier { set; get; }
        public string FormatDescription { set; get; }
        public VolumeDataType VolumeType { set; get; }

        public long VolumeBaseOffset { set; get; }
        public long DataOffset { set; get; }
        public byte[] PartitionKey {set;get;}

        public bool IsRawDump { set; get; }
        public NintendoWiiEncryptedDiscReader DiscReader { set; get; }

        public ArrayList DirectoryStructureArray { set; get; }
        public IDirectoryStructure[] Directories
        {
            set { Directories = value; }
            get
            {
                DirectoryStructureArray.Sort();
                return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(NintendoWiiOpticalDiscDirectoryStructure));
            }
        }

        public long RootDirectoryOffset { set; get; }
        public DateTime ImageCreationTime { set; get; }

        public long NameTableOffset { set; get; }

        public NintendoWiiOpticalDiscVolume(long volumeBaseOffset, long dataOffset, 
            byte[] partitionKey, NintendoWiiEncryptedDiscReader discReader)
        { 
            this.VolumeBaseOffset = volumeBaseOffset;
            this.DataOffset = dataOffset;
            this.PartitionKey = partitionKey;
            this.DiscReader = discReader;
            this.DiscReader.CurrentDecryptedBlockNumber = -1;
            this.DiscReader.CurrentDecryptedBlock = null;
        }

        public int CompareTo(object obj)
        {
            if (obj is NintendoWiiOpticalDiscVolume)
            {
                NintendoWiiOpticalDiscVolume o = (NintendoWiiOpticalDiscVolume)obj;

                return this.VolumeIdentifier.CompareTo(o.VolumeIdentifier);
            }

            throw new ArgumentException("object is not an NintendoWiiOpticalDiscVolume");
        }

        public void Initialize(FileStream isoStream, long offset, bool isRawDump)
        {
            byte[] volumeIdentifierBytes;
            byte[] imageDateBytes;
            string imageDateString;

            this.IsRawDump = isRawDump;
            this.DirectoryStructureArray = new ArrayList();

            this.FormatDescription = NintendoWiiOpticalDisc.FORMAT_DESCRIPTION_STRING;
            this.VolumeType = VolumeDataType.Data;

            // get identifier
            volumeIdentifierBytes = DiscReader.GetBytes(isoStream, this.VolumeBaseOffset,
                this.DataOffset, 0x20, 64, this.PartitionKey); 
            volumeIdentifierBytes = FileUtil.ReplaceNullByteWithSpace(volumeIdentifierBytes);
            this.VolumeIdentifier = ByteConversion.GetEncodedText(volumeIdentifierBytes, ByteConversion.GetPredictedCodePageForTags(volumeIdentifierBytes)).Trim(); ;

            // get date 
            imageDateBytes = DiscReader.GetBytes(isoStream, this.VolumeBaseOffset,
                this.DataOffset, 0x2440, 0xA, this.PartitionKey); 
            imageDateString = ByteConversion.GetAsciiText(imageDateBytes);

            try
            {
                this.ImageCreationTime = new DateTime(int.Parse(imageDateString.Substring(0, 4)),
                                                      int.Parse(imageDateString.Substring(5, 2)),
                                                      int.Parse(imageDateString.Substring(8, 2)));
            }
            catch (Exception)
            {
                this.ImageCreationTime = new DateTime();
            }

            // get offset of file table
            this.RootDirectoryOffset = (long)ByteConversion.GetUInt32BigEndian(DiscReader.GetBytes(isoStream, this.VolumeBaseOffset,
                this.DataOffset, 0x424, 4, this.PartitionKey));
            this.RootDirectoryOffset <<= 2;

            this.LoadDirectories(isoStream);
        }

        public void ExtractAll(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            foreach (NintendoWiiOpticalDiscDirectoryStructure ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }
        }

        public void LoadDirectories(FileStream isoStream)
        {
            byte[] rootDirectoryBytes;
            NintendoWiiOpticalDiscDirectoryRecord rootDirectoryRecord;
            NintendoWiiOpticalDiscDirectoryStructure rootDirectory;

            // Get name table offset
            rootDirectoryBytes = DiscReader.GetBytes(isoStream, this.VolumeBaseOffset,
                this.DataOffset, this.RootDirectoryOffset, 0xC, this.PartitionKey);
            rootDirectoryRecord = new NintendoWiiOpticalDiscDirectoryRecord(rootDirectoryBytes);
            this.NameTableOffset = this.RootDirectoryOffset + ((long)rootDirectoryRecord.FileSize * 0xC);

            rootDirectory = new NintendoWiiOpticalDiscDirectoryStructure(isoStream,
                isoStream.Name, rootDirectoryRecord, this.ImageCreationTime,
                this.VolumeBaseOffset, this.DataOffset, this.RootDirectoryOffset,
                this.RootDirectoryOffset, this.NameTableOffset,
                String.Empty, String.Empty, this.DiscReader, this.PartitionKey);

            this.DirectoryStructureArray.Add(rootDirectory);
        }
    }

    public class NintendoWiiOpticalDiscFileStructure : IFileStructure
    {
        public string ParentDirectoryName { set; get; }
        public string SourceFilePath { set; get; }
        public string FileName { set; get; }

        public long VolumeBaseOffset { set; get; }
        public long DataSectionOffset { set; get; }
        public long Lba { set; get; }
        public long Size { set; get; }
        public bool IsRaw { set; get; }
        public CdSectorType FileMode { set; get; }
        public int NonRawSectorSize { set; get; }

        public NintendoWiiEncryptedDiscReader DiscReader {set;get;}
        public byte[] PartitionKey { set; get; }

        public DateTime FileDateTime { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is NintendoWiiOpticalDiscFileStructure)
            {
                NintendoWiiOpticalDiscFileStructure o = (NintendoWiiOpticalDiscFileStructure)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not an NintendoWiiOpticalDiscFileStructure");
        }

        public NintendoWiiOpticalDiscFileStructure(string parentDirectoryName, 
            string sourceFilePath, string fileName, long volumeBaseOffset, 
            long dataSectionOffset, long lba, long size, DateTime fileTime,
            NintendoWiiEncryptedDiscReader discReader, byte[] partitionKey)
        {
            this.ParentDirectoryName = parentDirectoryName;
            this.SourceFilePath = sourceFilePath;
            this.FileName = fileName;
            this.VolumeBaseOffset = volumeBaseOffset;
            this.DataSectionOffset = dataSectionOffset;
            this.Lba = lba;
            this.IsRaw = false;
            this.NonRawSectorSize = -1;
            this.Size = size;
            this.FileMode = CdSectorType.Unknown;
            this.FileDateTime = fileTime;
            this.DiscReader = discReader;
            this.PartitionKey = partitionKey;
        }

        public virtual void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string destinationFile = Path.Combine(Path.Combine(destinationFolder, this.ParentDirectoryName), this.FileName);

            if (!streamCache.ContainsKey(this.SourceFilePath))
            {
                streamCache[this.SourceFilePath] = File.OpenRead(this.SourceFilePath);
            }

            this.DiscReader.ExtractFile(streamCache[this.SourceFilePath], destinationFile, this.VolumeBaseOffset,
                this.DataSectionOffset, this.Lba, this.Size, this.PartitionKey);
        }
    }

    public class NintendoWiiOpticalDiscDirectoryStructure : IDirectoryStructure
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
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(NintendoWiiOpticalDiscDirectoryStructure));
            }
        }

        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { this.Files = value; }
            get
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(NintendoWiiOpticalDiscFileStructure));
            }
        }

        public string SourceFilePath { set; get; }
        public string DirectoryName { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is NintendoWiiOpticalDiscDirectoryStructure)
            {
                NintendoWiiOpticalDiscDirectoryStructure o = (NintendoWiiOpticalDiscDirectoryStructure)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not an NintendoWiiOpticalDiscDirectoryStructure");
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string fullDirectoryPath = Path.Combine(destinationFolder, Path.Combine(this.ParentDirectoryName, this.DirectoryName));

            // create directory
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            foreach (NintendoWiiOpticalDiscFileStructure f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (NintendoWiiOpticalDiscDirectoryStructure d in this.SubDirectoryArray)
            {
                d.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }
        }

        public NintendoWiiOpticalDiscDirectoryStructure(
            FileStream isoStream,
            string sourceFilePath,
            NintendoWiiOpticalDiscDirectoryRecord directoryRecord,
            DateTime creationDateTime,
            long baseOffset,
            long dataSectionOffset,
            long rootDirectoryOffset,
            long directoryOffset,
            long nameTableOffset,
            string directoryName,
            string parentDirectory,
            NintendoWiiEncryptedDiscReader discReader,
            byte[] partitionKey)
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

            this.parseDirectoryRecord(isoStream, directoryRecord, creationDateTime, baseOffset, dataSectionOffset, rootDirectoryOffset, directoryOffset, nameTableOffset, nextDirectory, discReader, partitionKey);
        }


        private void parseDirectoryRecord(
            FileStream isoStream,
            NintendoWiiOpticalDiscDirectoryRecord directoryRecord,
            DateTime creationDateTime,
            long baseOffset,
            long dataSectionOffset,
            long rootDirectoryOffset,
            long directoryOffset,
            long nameTableOffset,
            string parentDirectory,
            NintendoWiiEncryptedDiscReader discReader,
            byte[] partitionKey)
        {
            long directoryRecordEndOffset;
            long newDirectoryEndOffset;
            long currentOffset = directoryOffset;

            int itemNameSize;
            byte[] itemNameBytes;
            string itemName;

            byte[] newDirectoryRecordBytes;
            NintendoWiiOpticalDiscDirectoryRecord newDirectoryRecord;
            NintendoWiiOpticalDiscDirectoryStructure newDirectory;
            NintendoWiiOpticalDiscFileStructure newFile;

            directoryRecordEndOffset = rootDirectoryOffset + (directoryRecord.FileSize * 0xC);
            currentOffset += 0xC;

            while (currentOffset < directoryRecordEndOffset)
            {
                newDirectoryRecordBytes = discReader.GetBytes(isoStream, baseOffset, dataSectionOffset,
                    currentOffset, 0xC, partitionKey);
                newDirectoryRecord = new NintendoWiiOpticalDiscDirectoryRecord(newDirectoryRecordBytes);

                itemNameBytes = discReader.GetBytes(isoStream, baseOffset, dataSectionOffset,
                    nameTableOffset + newDirectoryRecord.NameOffset, 512, partitionKey);
                itemNameSize = ParseFile.GetSegmentLength(itemNameBytes, 0, Constants.NullByteArray);
                itemNameBytes = discReader.GetBytes(isoStream, baseOffset, dataSectionOffset,
                    nameTableOffset + newDirectoryRecord.NameOffset, itemNameSize, partitionKey);                
                itemName = ByteConversion.GetEncodedText(itemNameBytes, ByteConversion.GetPredictedCodePageForTags(itemNameBytes));

                if (!newDirectoryRecord.IsDirectory)
                {
                    newFile = new NintendoWiiOpticalDiscFileStructure(parentDirectory,
                        this.SourceFilePath, itemName,
                        baseOffset, dataSectionOffset, newDirectoryRecord.FileOffset,
                        newDirectoryRecord.FileSize, creationDateTime, discReader,
                        partitionKey);

                    this.FileArray.Add(newFile);
                    currentOffset += 0xC;
                }
                else
                {
                    newDirectory =
                        new NintendoWiiOpticalDiscDirectoryStructure(isoStream,
                            isoStream.Name, newDirectoryRecord,
                            creationDateTime, baseOffset, dataSectionOffset, rootDirectoryOffset,
                            currentOffset, nameTableOffset,
                            itemName, parentDirectory, discReader, partitionKey);

                    this.SubDirectoryArray.Add(newDirectory);

                    newDirectoryEndOffset = rootDirectoryOffset + (newDirectoryRecord.FileSize * 0xC);
                    currentOffset = newDirectoryEndOffset;
                }
            }
        }
    }

    public class NintendoWiiOpticalDiscDirectoryRecord
    {
        public uint NameOffset { set; get; }
        public long FileOffset { set; get; }
        public uint FileSize { set; get; }
        public bool IsDirectory { set; get; }

        public NintendoWiiOpticalDiscDirectoryRecord(byte[] directoryBytes)
        {
            this.NameOffset = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, 0, 4));

            this.FileOffset = (long)ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, 4, 4));
            this.FileOffset <<= 2;

            this.FileSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(directoryBytes, 8, 4));
            this.IsDirectory = ((this.NameOffset & 0xFF000000) != 0);

            this.NameOffset &= 0xFFFFFF;
        }
    }

    public class NintendoWiiEncryptedDiscReader
    {
        public byte[] CurrentDecryptedBlock { set; get; }
        public long CurrentDecryptedBlockNumber { set; get; }
        public Rijndael Algorithm { set; get; }

        public NintendoWiiEncryptedDiscReader()
        {
            this.CurrentDecryptedBlock = null;
            this.CurrentDecryptedBlockNumber = -1;
            this.Algorithm = Rijndael.Create();
        }

        public byte[] GetBytes(FileStream isoStream, long volumeOffset,
                long dataOffset, long blockOffset, long size, byte[] partitionKey)
        {
            byte[] value = new byte[size];
            byte[] encryptedPartitionCluster;
            byte[] encryptedClusterDataSection;
            byte[] decryptedClusterDataSection;
            byte[] clusterIV;

            long bufferLocation = 0;
            long maxCopySize;
            long copySize;

            while (size > 0)
            {
                // get block offset info
                NintendoWiiOpticalDisc.WiiBlockStructure blockStructure =
                    NintendoWiiOpticalDisc.GetWiiBlockStructureForOffset(blockOffset);

                // read current block
                //encryptedPartitionCluster = ParseFile.ParseSimpleOffset(isoStream,
                //    volumeOffset + dataOffset + (blockStructure.BlockNumber * 0x8000),
                //    0x8000);

                if (this.CurrentDecryptedBlockNumber != blockStructure.BlockNumber)
                {
                    encryptedPartitionCluster = ParseFile.ParseSimpleOffset(isoStream,
                        volumeOffset + dataOffset + (blockStructure.BlockNumber * 0x8000),
                        0x8000);

                    clusterIV = ParseFile.ParseSimpleOffset(encryptedPartitionCluster, 0x03D0, 0x10);
                    encryptedClusterDataSection = ParseFile.ParseSimpleOffset(encryptedPartitionCluster, 0x400, 0x7C00);
                    decryptedClusterDataSection = AESEngine.Decrypt(this.Algorithm, encryptedClusterDataSection,
                                    partitionKey, clusterIV, CipherMode.CBC, PaddingMode.Zeros);

                    this.CurrentDecryptedBlock = decryptedClusterDataSection;
                    this.CurrentDecryptedBlockNumber = blockStructure.BlockNumber;
                }

                // copy the decrypted data
                maxCopySize = 0x7C00 - blockStructure.BlockOffset;
                copySize = (size > maxCopySize) ? maxCopySize : size;
                Array.Copy(this.CurrentDecryptedBlock, blockStructure.BlockOffset,
                    value, bufferLocation, copySize);

                // update counters
                size -= copySize;
                bufferLocation += copySize;
                blockOffset += copySize;
            }

            return value;
        }

        public void ExtractFile(FileStream isoStream, string destinationPath, long volumeOffset,
            long dataOffset, long blockOffset, long size, byte[] partitionKey)
        {
            byte[] value = new byte[0x7C00];
            byte[] encryptedPartitionCluster;
            byte[] encryptedClusterDataSection;
            byte[] decryptedClusterDataSection;
            byte[] clusterIV;

            long maxCopySize;
            long copySize;

            // create destination
            string destintionDirectory = Path.GetDirectoryName(destinationPath);

            if (!Directory.Exists(destintionDirectory))
            {
                Directory.CreateDirectory(destintionDirectory);
            }

            using (FileStream outStream = File.Open(destinationPath, FileMode.Create, FileAccess.Write))
            {
                while (size > 0)
                {
                    try
                    {
                        // get block offset info
                        NintendoWiiOpticalDisc.WiiBlockStructure blockStructure =
                            NintendoWiiOpticalDisc.GetWiiBlockStructureForOffset(blockOffset);

                        // read current block
                        //encryptedPartitionCluster = ParseFile.ParseSimpleOffset(isoStream,
                        //    volumeOffset + dataOffset + (blockStructure.BlockNumber * 0x8000),
                        //    0x8000);

                        if (this.CurrentDecryptedBlockNumber != blockStructure.BlockNumber)
                        {
                            encryptedPartitionCluster = ParseFile.ParseSimpleOffset(isoStream,
                                volumeOffset + dataOffset + (blockStructure.BlockNumber * 0x8000),
                                0x8000);

                            if (encryptedPartitionCluster.Length == 0)
                            {
                                throw new Exception(String.Format("Encrypted cluster 0x{0) has size zero.  This image is probably corrupt.", blockStructure.BlockNumber.ToString("X8")));
                            }
                            else
                            {
                                clusterIV = ParseFile.ParseSimpleOffset(encryptedPartitionCluster, 0x03D0, 0x10);
                                encryptedClusterDataSection = ParseFile.ParseSimpleOffset(encryptedPartitionCluster, 0x400, 0x7C00);
                                decryptedClusterDataSection = AESEngine.Decrypt(this.Algorithm, encryptedClusterDataSection,
                                                partitionKey, clusterIV, CipherMode.CBC, PaddingMode.Zeros);

                                this.CurrentDecryptedBlock = decryptedClusterDataSection;
                                this.CurrentDecryptedBlockNumber = blockStructure.BlockNumber;
                            }
                        }

                        // copy the encrypted data
                        maxCopySize = 0x7C00 - blockStructure.BlockOffset;
                        copySize = (size > maxCopySize) ? maxCopySize : size;
                        outStream.Write(this.CurrentDecryptedBlock, (int)blockStructure.BlockOffset, (int)copySize);

                        // update counters
                        size -= copySize;
                        blockOffset += copySize;
                    }
                    catch (Exception ie)
                    {
                        throw ie;
                    }
                }
            }
        }
    }

    // Taken from code found somewhere on the net, can't remember where
    public class AESEngine
    {
        // Decrypt a byte array into a byte array using a key and an IV 
        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV, CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] decryptedData = null;
            
            // Create a MemoryStream that is going to accept the
            // decrypted bytes 
            using (MemoryStream ms = new MemoryStream())
            {

                // Create a symmetric algorithm. 
                // We are going to use Rijndael because it is strong and
                // available on all platforms. 
                // You can use other algorithms, to do so substitute the next
                // line with something like 
                //     TripleDES alg = TripleDES.Create(); 

                Rijndael alg = Rijndael.Create();

                // Now set the key and the IV. 
                // We need the IV (Initialization Vector) because the algorithm
                // is operating in its default 
                // mode called CBC (Cipher Block Chaining). The IV is XORed with
                // the first block (8 byte) 
                // of the data after it is decrypted, and then each decrypted
                // block is XORed with the previous 
                // cipher block. This is done to make encryption more secure. 
                // There is also a mode called ECB which does not need an IV,
                // but it is much less secure. 

                alg.KeySize = 128;
                alg.Mode = cipherMode;
                alg.Padding = paddingMode;
                alg.Key = Key;
                alg.IV = IV;

                // Create a CryptoStream through which we are going to be
                // pumping our data. 
                // CryptoStreamMode.Write means that we are going to be
                // writing data to the stream 
                // and the output will be written in the MemoryStream
                // we have provided. 

                CryptoStream cs = new CryptoStream(ms,
                    alg.CreateDecryptor(), CryptoStreamMode.Write);

                // Write the data and make it do the decryption 
                cs.Write(cipherData, 0, cipherData.Length);

                // Close the crypto stream (or do FlushFinalBlock). 
                // This will tell it that we have done our decryption
                // and there is no more data coming in, 
                // and it is now a good time to remove the padding
                // and finalize the decryption process. 

                cs.Close();

                // Now get the decrypted data from the MemoryStream. 
                // Some people make a mistake of using GetBuffer() here,
                // which is not the right way. 

                decryptedData = ms.ToArray();
            }

            return decryptedData;
        }

        public static byte[] Decrypt(Rijndael algorithm, byte[] cipherData, byte[] Key, byte[] IV, CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] decryptedData = null;

            // Create a MemoryStream that is going to accept the
            // decrypted bytes 
            using (MemoryStream ms = new MemoryStream())
            {

                // Create a symmetric algorithm. 
                // We are going to use Rijndael because it is strong and
                // available on all platforms. 
                // You can use other algorithms, to do so substitute the next
                // line with something like 
                //     TripleDES alg = TripleDES.Create(); 

                Rijndael alg = algorithm;

                // Now set the key and the IV. 
                // We need the IV (Initialization Vector) because the algorithm
                // is operating in its default 
                // mode called CBC (Cipher Block Chaining). The IV is XORed with
                // the first block (8 byte) 
                // of the data after it is decrypted, and then each decrypted
                // block is XORed with the previous 
                // cipher block. This is done to make encryption more secure. 
                // There is also a mode called ECB which does not need an IV,
                // but it is much less secure. 

                alg.KeySize = 128;
                alg.Mode = cipherMode;
                alg.Padding = paddingMode;
                alg.Key = Key;
                alg.IV = IV;

                // Create a CryptoStream through which we are going to be
                // pumping our data. 
                // CryptoStreamMode.Write means that we are going to be
                // writing data to the stream 
                // and the output will be written in the MemoryStream
                // we have provided. 

                CryptoStream cs = new CryptoStream(ms,
                    alg.CreateDecryptor(), CryptoStreamMode.Write);

                // Write the data and make it do the decryption 
                cs.Write(cipherData, 0, cipherData.Length);

                // Close the crypto stream (or do FlushFinalBlock). 
                // This will tell it that we have done our decryption
                // and there is no more data coming in, 
                // and it is now a good time to remove the padding
                // and finalize the decryption process. 

                cs.Close();

                // Now get the decrypted data from the MemoryStream. 
                // Some people make a mistake of using GetBuffer() here,
                // which is not the right way. 

                decryptedData = ms.ToArray();
            }

            return decryptedData;
        }

        public static byte[] Decrypt(Rijndael algorithm, byte[] cipherData, byte[] Key, ref byte[] IV, CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] decryptedData = null;

            // Create a MemoryStream that is going to accept the
            // decrypted bytes 
            using (MemoryStream ms = new MemoryStream())
            {

                // Create a symmetric algorithm. 
                // We are going to use Rijndael because it is strong and
                // available on all platforms. 
                // You can use other algorithms, to do so substitute the next
                // line with something like 
                //     TripleDES alg = TripleDES.Create(); 

                Rijndael alg = algorithm;

                // Now set the key and the IV. 
                // We need the IV (Initialization Vector) because the algorithm
                // is operating in its default 
                // mode called CBC (Cipher Block Chaining). The IV is XORed with
                // the first block (8 byte) 
                // of the data after it is decrypted, and then each decrypted
                // block is XORed with the previous 
                // cipher block. This is done to make encryption more secure. 
                // There is also a mode called ECB which does not need an IV,
                // but it is much less secure. 

                alg.KeySize = 128;
                alg.Mode = cipherMode;
                alg.Padding = paddingMode;
                alg.Key = Key;
                alg.IV = IV;

                // Create a CryptoStream through which we are going to be
                // pumping our data. 
                // CryptoStreamMode.Write means that we are going to be
                // writing data to the stream 
                // and the output will be written in the MemoryStream
                // we have provided. 

                CryptoStream cs = new CryptoStream(ms,
                    alg.CreateDecryptor(), CryptoStreamMode.Write);

                // Write the data and make it do the decryption 
                cs.Write(cipherData, 0, cipherData.Length);

                // Close the crypto stream (or do FlushFinalBlock). 
                // This will tell it that we have done our decryption
                // and there is no more data coming in, 
                // and it is now a good time to remove the padding
                // and finalize the decryption process. 

                cs.Close();

                // Now get the decrypted data from the MemoryStream. 
                // Some people make a mistake of using GetBuffer() here,
                // which is not the right way. 

                decryptedData = ms.ToArray();
            }

            for (int i = 0; i < 0x10; i++)
            {
                IV[i] = cipherData[cipherData.Length - 0x10 + i];
            }


            return decryptedData;
        }
    }
}
