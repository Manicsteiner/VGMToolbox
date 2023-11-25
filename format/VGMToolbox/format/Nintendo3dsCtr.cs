using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.format.iso;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class SimpleOffset
    {
        public uint Offset { set; get; }
        public uint Size { set; get; }

        public SimpleOffset(uint offset, uint size)
        {
            this.Offset = offset;
            this.Size = size;
        }
    }
    
    public class Nintendo3dsCtr
    {
        public static readonly byte[] STANDARD_IDENTIFIER = new byte[] { 0x4E, 0x43, 0x53, 0x44 }; // "NCSD"
        public const uint IDENTIFIER_OFFSET = 0x100;
        public static string FORMAT_DESCRIPTION_STRING = "3DS CTR Encrypted"; // currently unsupported
        public static string FORMAT_DESCRIPTION_STRING_DECRYPTED = "3DS CTR Decrypted";
        public const long MEDIA_UNIT_SIZE = 0x200;

        public const string CTR_SYSTEM_PARTITION = "CTR-P-CTAP";
        public enum FileSystemType {ExeFS, RomFS};

        public long DiscBaseOffset { set; get; }
        public string SourceFileName { set; get; }
        public bool IsRawDump { set; get; }
        
        public ArrayList VolumeArrayList { set; get; }
        public Nintendo3dsNcchContainer[] Volumes
        {
            get
            {
                return (Nintendo3dsNcchContainer[])this.VolumeArrayList.ToArray(typeof(Nintendo3dsNcchContainer));
            }

        }

        // NCSD Header

        public byte[] NcsdHash { set; get; }
        public uint MagicBytes { set; get; }
        public uint ImageSize { set; get; }
        public ulong MediaId { set; get; }
        public ulong PartitionsFsType { set; get; }
        public ulong PartitionsEncryptionType { set; get; }
        public SimpleOffset[] NcchOffsetInfo { set; get; }

        public byte[] ExHeaderHash { set; get; }
        public uint AdditionalHeaderSize { set; get; }
        public uint SectorZeroOffset { set; get; }
        public ulong PartitionFlags { set; get; }
        public ulong[] PartitionIds { set; get; }
        public byte[] Reserved01 { set; get; }
        public byte[] Reserved02 { set; get; }
        public byte Unknown01 { set; get; }
        public byte Unknown02 { set; get; }

        // Card Info Header

        public int CardInfoWritableAddress { set; get; }
        public uint CardInfoBitmask { set; get; }
        public byte[] CardInfoReserved01 { set; get; }
        public ulong CardInfoMediaId { set; get; }
        public ulong CardInfoReserved02 { set; get; }
        public byte[] CardInfoInitialData { set; get; }
        public byte[] CardInfoReserved03 { set; get; }
        public byte[] CardInfoNcchHeaderCopy { set; get; }

        public Nintendo3dsCtr(string sourceFile)
        { 
            // check magic bytes
            if (Nintendo3dsCtr.Is3dsCtrFile(sourceFile))
            {
                // read header
                using (FileStream fs = File.OpenRead(sourceFile))
                {
                    // set source file
                    this.SourceFileName = sourceFile; 

                    // initialize
                    this.Initialize(fs, 0, false);

                } // using (FileStream fs = File.OpenRead(sourceFile))
            }
            else
            {
                throw new FormatException("'NCSD' magic bytes not found at offset 0x100.");
            }
        }

        public void Initialize(FileStream fs, long offset, bool isRawDump)
        {
            // @TODO: Is there a way to determine if this file is exncrypted or not?
            //this.FormatDescription = Nintendo3dsCtr.FORMAT_DESCRIPTION_STRING_DECRYPTED;        
            this.DiscBaseOffset = offset;
            this.IsRawDump = isRawDump;

            // parse NCSD header
            this.ParseNcsdHeader(fs, offset);

            // initialize volumes
            this.VolumeArrayList = new ArrayList();
            this.LoadVolumes(fs);
        }


        public void LoadVolumes(FileStream isoStream)
        {
            Nintendo3dsNcchContainer newVolume;
            long streamLength = isoStream.Length;
            long ncchAbsoluteOffset;

            for (int i = 0; i < NcchOffsetInfo.Length; i++)
            {
                if (this.NcchOffsetInfo[i].Offset > 0)  
                {
                    ncchAbsoluteOffset = this.DiscBaseOffset + (long)((long)this.NcchOffsetInfo[i].Offset * Nintendo3dsCtr.MEDIA_UNIT_SIZE);

                    // skip trimmed update partition
                    if (ncchAbsoluteOffset + 1 < streamLength)
                    {
                        newVolume = new Nintendo3dsNcchContainer();
                        newVolume.Initialize(isoStream, ncchAbsoluteOffset, this.IsRawDump);
                        this.VolumeArrayList.Add(newVolume);
                    }
                }
            }
        }

        private void ParseNcsdHeader(FileStream fs, long offset)
        {
            // parse NCSD header
            this.NcsdHash = ParseFile.ParseSimpleOffset(fs, offset, 0x100);
            this.MagicBytes = ParseFile.ReadUintBE(fs, offset + 0x100);
            this.ImageSize = ParseFile.ReadUintLE(fs, offset + 0x104);
            this.MediaId = ParseFile.ReadUlongLE(fs, offset + 0x108);
            this.PartitionsFsType = ParseFile.ReadUlongLE(fs, offset + 0x110);
            this.PartitionsEncryptionType = ParseFile.ReadUlongLE(fs, offset + 0x118);

            this.NcchOffsetInfo = new SimpleOffset[8];
            for (int i = 0; i < 8; i++)
            {
                this.NcchOffsetInfo[i] = new SimpleOffset(
                    ParseFile.ReadUintLE(fs, offset + 0x120 + (8 * i)),
                    ParseFile.ReadUintLE(fs, offset + 0x120 + ((8 * i) + 4)));
            }

            // parse Exheader
            this.ExHeaderHash = ParseFile.ParseSimpleOffset(fs, offset + 0x160, 0x20);
            this.AdditionalHeaderSize = ParseFile.ReadUintLE(fs, offset + 0x180);
            this.SectorZeroOffset = ParseFile.ReadUintLE(fs, offset + 0x184);
            this.PartitionFlags = ParseFile.ReadUlongLE(fs, offset + 0x188);

            this.PartitionIds = new ulong[8];
            for (int i = 0; i < 8; i++)
            {
                this.PartitionIds[i] = ParseFile.ReadUlongLE(fs, offset + 0x190 + (8 * i));
            }


            this.Reserved01 = ParseFile.ParseSimpleOffset(fs, offset + 0x1D0, 0x20);
            this.Reserved02 = ParseFile.ParseSimpleOffset(fs, offset + 0x1F0, 0xE);
            this.Unknown01 = ParseFile.ReadByte(fs, 0x1FE);
            this.Unknown02 = ParseFile.ReadByte(fs, 0x1FF);            
          
            // parse Card Info
            this.CardInfoWritableAddress = ParseFile.ReadInt32LE(fs, offset + 0x200);
            this.CardInfoBitmask = ParseFile.ReadUintLE(fs, offset + 0x204);
            this.CardInfoReserved01 = ParseFile.ParseSimpleOffset(fs, offset + 0x208, 0xDF8);
            this.CardInfoMediaId = ParseFile.ReadUlongLE(fs, offset + 0x1000);
            this.CardInfoReserved02 = ParseFile.ReadUlongLE(fs, offset + 0x1008);
            this.CardInfoInitialData = ParseFile.ParseSimpleOffset(fs, offset + 0x1010, 0x30);
            this.CardInfoReserved03 = ParseFile.ParseSimpleOffset(fs, offset + 0x1040, 0xC0);
            this.CardInfoNcchHeaderCopy = ParseFile.ParseSimpleOffset(fs, offset + 0x1011, 0x100);
        }

        /// <summary>
        /// Checks for 3DS CTR file Magic Bytes.
        /// </summary>
        /// <param name="sourceFile">Full path to file to check.</param>
        /// <returns>Boolean value indicating if input file has 3DS CTR magic bytes.</returns>
        public static bool Is3dsCtrFile(string sourceFile)
        {
            bool isCtr = false;
            byte[] magicBytes = new byte[Nintendo3dsCtr.STANDARD_IDENTIFIER.Length];

            using (FileStream fs = File.OpenRead(sourceFile))
            {
                magicBytes = ParseFile.ParseSimpleOffset(fs, Nintendo3dsCtr.IDENTIFIER_OFFSET, Nintendo3dsCtr.STANDARD_IDENTIFIER.Length);

                if (ParseFile.CompareSegment(magicBytes, 0, Nintendo3dsCtr.STANDARD_IDENTIFIER))
                {
                    isCtr = true;
                }
            }

            return isCtr;
        }
    }

    //--------------------------------------------
    // NCCH
    //--------------------------------------------
    public class Nintendo3dsNcchContainer : IVolume 
    {
        public byte[] NcsdHash { set; get; }
        public uint MagicBytes { set; get; }
        public uint ContentSize { set; get; }
        public ulong PartitionId { set; get; }

        public ushort MakerCode { set; get; }
        public ushort Version { set; get; }
        public uint Reserved01 { set; get; }
        public ulong ProgramId { set; get; }
        public byte[] Reserved02 { set; get; }
        public byte[] LogoRegionHash { set; get; }
        public string ProductCode { set; get; }
        
        public byte[] ExtendedHeaderHash { set; get; }
        public uint ExtendedHeaderSize { set; get; }

        public uint Reserved03 { set; get; }
        public ulong Flags { set; get; }

        public uint PlainRegionOffset { set; get; }
        public uint PlainRegionSize { set; get; }
        
        public uint LogoRegionOffset { set; get; }
        public uint LogoRegionSize { set; get; }
        
        public uint ExeFsOffset { set; get; }
        public uint ExeFsSize { set; get; }
        public uint ExeFsHashSize { set; get; }
        public uint Reserved04 { set; get; }

        public uint RomFsOffset { set; get; }
        public uint RomFsSize { set; get; }
        public uint RomFsHashSize { set; get; }
        public uint Reserved05 { set; get; }

        public byte[] ExeSuperblockHash { set; get; }
        public byte[] RomFsSuperblockHash { set; get; }

        //public Nintendo3dsCtrExeFileSystem ExeFs { set; get; }
        //public Nintendo3dsCtrRomFileSystem RomFs { set; get; }

        #region IVolume
        public ArrayList DirectoryStructureArray { set; get; }
        public IDirectoryStructure[] Directories
        {
            set { Directories = value; }
            get
            {
                DirectoryStructureArray.Sort();
                return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(Nintendo3dsCtrDirectory));
            }
        }

        public long VolumeBaseOffset { set; get; }
        public string FormatDescription { set; get; }
        public VolumeDataType VolumeType { set; get; }
        public string VolumeIdentifier { set; get; }
        public bool IsRawDump { set; get; }

        #endregion

        private void ParseNcchHeader(FileStream fs, long offset)
        {
            this.NcsdHash = ParseFile.ParseSimpleOffset(fs, offset, 0x100);
            this.MagicBytes = ParseFile.ReadUintBE(fs, offset + 0x100);
            this.ContentSize = ParseFile.ReadUintLE(fs, offset + 0x104);
            this.PartitionId = ParseFile.ReadUlongLE(fs, offset + 0x108);

            this.MakerCode = ParseFile.ReadUshortLE(fs, offset + 0x110);
            this.Version = ParseFile.ReadUshortLE(fs, offset + 0x112);
            this.Reserved01 = ParseFile.ReadUintLE(fs, offset + 0x114);
            this.ProgramId = ParseFile.ReadUlongLE(fs, offset + 0x118);
            this.Reserved02 = ParseFile.ParseSimpleOffset(fs, offset + 0x120, 0x10);
            this.LogoRegionHash = ParseFile.ParseSimpleOffset(fs, offset + 0x130, 0x20);
            this.ProductCode = ParseFile.ReadAsciiString(fs, offset + 0x150);

            this.ExtendedHeaderHash = ParseFile.ParseSimpleOffset(fs, offset + 0x160, 0x10);
            this.ExtendedHeaderSize = ParseFile.ReadUintLE(fs, offset + 0x180);

            this.Reserved03 = ParseFile.ReadUintLE(fs, offset + 0x184);
            this.Flags = ParseFile.ReadUlongLE(fs, offset + 0x188);

            this.PlainRegionOffset = ParseFile.ReadUintLE(fs, offset + 0x190);
            this.PlainRegionSize = ParseFile.ReadUintLE(fs, offset + 0x194);

            this.LogoRegionOffset = ParseFile.ReadUintLE(fs, offset + 0x198);
            this.LogoRegionSize = ParseFile.ReadUintLE(fs, offset + 0x19C);

            this.ExeFsOffset = ParseFile.ReadUintLE(fs, offset + 0x1A0);
            this.ExeFsSize = ParseFile.ReadUintLE(fs, offset + 0x1A4);
            this.ExeFsHashSize = ParseFile.ReadUintLE(fs, offset + 0x1A8);
            this.Reserved04 = ParseFile.ReadUintLE(fs, offset + 0x1AC);

            this.RomFsOffset = ParseFile.ReadUintLE(fs, offset + 0x1B0);
            this.RomFsSize = ParseFile.ReadUintLE(fs, offset + 0x1B4);
            this.RomFsHashSize = ParseFile.ReadUintLE(fs, offset + 0x1B8);
            this.Reserved05 = ParseFile.ReadUintLE(fs, offset + 0x1BC);

            this.ExeSuperblockHash = ParseFile.ParseSimpleOffset(fs, offset + 0x1C0, 0x20);
            this.RomFsSuperblockHash = ParseFile.ParseSimpleOffset(fs, offset + 0x1E0, 0x20);        
        }

        public void Initialize(FileStream fs, long offset, bool isRawDump)
        {
            long absoluteOffset;

            this.FormatDescription = Nintendo3dsCtr.FORMAT_DESCRIPTION_STRING_DECRYPTED;

            this.VolumeBaseOffset = offset;
            this.IsRawDump = isRawDump;
            this.VolumeType = VolumeDataType.Data;
            this.DirectoryStructureArray = new ArrayList();

            // parse NCCH header
            this.ParseNcchHeader(fs, offset);

            // set volume name
            this.VolumeIdentifier = this.ProductCode;

            // parse File Systems
            Nintendo3dsCtrDirectory dummyRoot = new Nintendo3dsCtrDirectory();
            dummyRoot.SubDirectoryArray = new ArrayList();
            dummyRoot.FileArray = new ArrayList();

            if (this.ExeFsOffset > 0)
            {
                absoluteOffset = offset + ((long)this.ExeFsOffset * Nintendo3dsCtr.MEDIA_UNIT_SIZE);
                Nintendo3dsCtrDirectory ExeFs = new Nintendo3dsCtrDirectory(fs, fs.Name, this.VolumeIdentifier, 
                    absoluteOffset, Nintendo3dsCtr.FileSystemType.ExeFS, null);
                dummyRoot.SubDirectoryArray.Add((IDirectoryStructure)ExeFs);
            }

            if (this.RomFsOffset > 0)
            {
                absoluteOffset = offset + ((long)this.RomFsOffset * Nintendo3dsCtr.MEDIA_UNIT_SIZE);

                try
                {
                    Nintendo3dsCtrDirectory RomFs = new Nintendo3dsCtrDirectory(fs, fs.Name, this.VolumeIdentifier,
                        absoluteOffset, Nintendo3dsCtr.FileSystemType.RomFS, null);                   
                    dummyRoot.SubDirectoryArray.Add((IDirectoryStructure)RomFs);
                }
                catch (Exception ex)
                {
                    if (ex is FormatException)
                    {
                        MessageBox.Show(String.Format("IVFC magic bytes not found at expected RomFS offset for Volume '{0}, are you sure it is decrypted?'  Note: System Volume, {1}, has not been tested due to unavailability of decrypted samples.",
                                                      this.VolumeIdentifier, Nintendo3dsCtr.CTR_SYSTEM_PARTITION), "Error Processing 3DS CTR");
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

            if (dummyRoot.SubDirectoryArray.Count > 0)
            {
                this.DirectoryStructureArray.Add((IDirectoryStructure)dummyRoot);
            }
        }

        /// <summary>
        /// Extract all files in archive.
        /// </summary>
        /// <param name="streamCache"></param>
        /// <param name="destintionFolder"></param>
        /// <param name="extractAsRaw"></param>
        public void ExtractAll(ref Dictionary<string, FileStream> streamCache, string destintionFolder, bool extractAsRaw)
        {
            foreach (Nintendo3dsCtrDirectory ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, destintionFolder, extractAsRaw);
            }
        }
    
    
    }

    //--------------------------------------------
    // Nintendo3dsCtrFile (Combine ExeFS and RomFS)
    //--------------------------------------------
    public class Nintendo3dsCtrFile : IFileStructure
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
        public byte[] Sha256Hash { set; get; }
        
        public Nintendo3dsCtr.FileSystemType FileSystem { set; get; }
        public int SiblingOffset { set; get; }

        public IvfcLevelInfo Level3HashInfo { set; get; }

        public int CompareTo(object obj)
        {
            if (obj is Nintendo3dsCtrFile)
            {
                Nintendo3dsCtrFile o = (Nintendo3dsCtrFile)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not a Nintendo3dsCtrFile");
        }

        public Nintendo3dsCtrFile(FileStream isoStream, string parentDirectoryName, string sourceFilePath,
            string fileName, long offset, long size, Nintendo3dsCtr.FileSystemType fileSystem,
            IvfcLevelInfo level3HashInfo, long fileEntryOffset = 0, long fileBlockOffset = -1, long romFsDataOffset = -1)
        {                        
            this.ParentDirectoryName = parentDirectoryName;
            this.SourceFilePath = sourceFilePath;
            this.FileDateTime = new DateTime();            
            
            this.IsRaw = false;
            this.NonRawSectorSize = 0;
            this.FileMode = CdSectorType.Unknown;
            
            this.Level3HashInfo = level3HashInfo;

            this.FileSystem = fileSystem;

            if (this.FileSystem == Nintendo3dsCtr.FileSystemType.RomFS)
            {
                this.BuildRomFsFile(isoStream, offset, fileEntryOffset, fileBlockOffset, romFsDataOffset);
            }
            else
            {
                this.FileName = fileName;
                this.Offset = offset;
                this.Size = size;
            }

            this.Lba = this.Offset;
        }

        private void BuildRomFsFile(FileStream isoStream, long ivfcOffset, long fileEntryOffset, long fileBlockOffset, long romFsDataOffset)
        {
            RomFsFileEntry file = new RomFsFileEntry();
            // Nintendo3dsCtrFile tempFile;
            byte[] nameBytes;

            // load dir
            file.ParentDirOffset = ParseFile.ReadUintLE(isoStream, ivfcOffset + fileBlockOffset + fileEntryOffset);
            file.SiblingOffset = ParseFile.ReadInt32LE(isoStream, ivfcOffset + fileBlockOffset + fileEntryOffset + 4);
            file.DataOffset = ParseFile.ReadUlongLE(isoStream, ivfcOffset + fileBlockOffset + fileEntryOffset + 8);
            file.DataSize = ParseFile.ReadUlongLE(isoStream, ivfcOffset + fileBlockOffset + fileEntryOffset + 0x10);
            file.WeirdOffset = ParseFile.ReadUintLE(isoStream, ivfcOffset + fileBlockOffset + fileEntryOffset + 0x18);
            file.NameSize = ParseFile.ReadUintLE(isoStream, ivfcOffset + fileBlockOffset + fileEntryOffset + 0x1C);

            // build directory name
            if (file.NameSize > 0)
            {
                nameBytes = ParseFile.ParseSimpleOffset(isoStream, ivfcOffset + fileBlockOffset + fileEntryOffset + 0x20, (int)file.NameSize);
                file.Name = ByteConversion.GetUtf16LeText(nameBytes);
            }
            else // this is root
            {
                file.Name = "NO_NAME_FOUND"; // @TODO Make this a constant
            }

            this.FileName = file.Name;
            this.Offset = ivfcOffset + romFsDataOffset + (long)file.DataOffset;
            this.Size = (long)file.DataSize;

            // get sibling
            this.SiblingOffset = file.SiblingOffset;            
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string destinationFile = Path.Combine(Path.Combine(destinationFolder, this.ParentDirectoryName), this.FileName);
            
            if (!streamCache.ContainsKey(this.SourceFilePath))
            {
                streamCache[this.SourceFilePath] = File.OpenRead(this.SourceFilePath);
            }

            //-----------------------------------------------------------
            // ExeFS
            //-----------------------------------------------------------
            if (this.FileSystem == Nintendo3dsCtr.FileSystemType.ExeFS)
            {
                // write file while calculating hash
                HashAlgorithm cryptoHash = SHA256.Create();
                byte[] outputHash = ParseFile.ExtractChunkToFile64ReturningHash(streamCache[this.SourceFilePath], (ulong)this.Offset, (ulong)this.Size,
                    destinationFile, cryptoHash, false, false);

                if (!ParseFile.CompareSegment(outputHash, 0, this.Sha256Hash))
                {
                    // @TODO: only show error once per file
                    //if (!sha1ErrorDisplayed)
                    {
                        MessageBox.Show(String.Format("Warning: '{0},' failed SHA256 verification in ExeFS at offset 0x{1} during extraction.{2}",
                            Path.GetFileName(destinationFile), this.Offset, Environment.NewLine), "Warning - SHA256 Failure");
                    }
                }
            }
            //---------------
            // RomFS
            //---------------
            else
            {
                // @TODO: Modify to verify on extraction
                ParseFile.ExtractChunkToFile64(streamCache[this.SourceFilePath], (ulong)this.Offset,
                    (ulong)this.Size, destinationFile, false, false);
            }
        }
    }

    //---------------------------------------------------
    // Nintendo3dsCtrDirectory (Combine ExeFS and RomFS)
    //---------------------------------------------------
    #region RomFS Helpers
    public class IvfcLevelInfo
    {
        public ulong HashOffset { set; get; }
        public ulong HashBlockSize { set; get; }
        public ulong DataOffset { set; get; }
        public ulong DataSize { set; get; }
    }

    public class RomFsDirEntry
    {
        public uint ParentOffset { set; get; }
        public int SiblingOffset { set; get; }
        public int ChildOffset { set; get; }
        public int FileOffset { set; get; }
        public int WeirdOffset { set; get; }
        public uint NameSize { set; get; }
        public string Name { set; get; }

        public RomFsDirEntry() { }
    }

    public class RomFsFileEntry
    {
        public uint ParentDirOffset { set; get; }
        public int SiblingOffset { set; get; }
        public ulong DataOffset { set; get; }
        public ulong DataSize { set; get; }
        public uint WeirdOffset { set; get; }
        public uint NameSize { set; get; }
        public string Name { set; get; }

        public RomFsFileEntry() { }
    }



    #endregion
    
    public class Nintendo3dsCtrDirectory : IDirectoryStructure
    {
        public string SourceFilePath { set; get; }
        public string DirectoryName { set; get; }
        public string ParentDirectoryName { set; get; }

        public ArrayList SubDirectoryArray { set; get; }
        public IDirectoryStructure[] SubDirectories
        {
            set { this.SubDirectories = value; }
            get
            {
                SubDirectoryArray.Sort();
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(Nintendo3dsCtrDirectory));
            }
        }

        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { this.Files = value; }
            get
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(Nintendo3dsCtrFile));
            }
        }

        public Nintendo3dsCtr.FileSystemType FileSystem { set; get; }

        #region RomFS Members

        public Nintendo3dsCtrDirectory SiblingDirectory { set; get; }

        // IVFC
        public uint MagicBytes { set; get; }
        public uint VersionNumber { set; get; }
        public uint MasterHashSize { set; get; }

        public ulong Level1Offset { set; get; }
        public ulong Level1HashDataSize { set; get; }
        public uint Level1BlockSize { set; get; }
        public uint Reserved01 { set; get; }

        public ulong Level2Offset { set; get; }
        public ulong Level2HashDataSize { set; get; }
        public uint Level2BlockSize { set; get; }
        public uint Reserved02 { set; get; }

        public ulong Level3Offset { set; get; }
        public ulong Level3HashDataSize { set; get; }
        public uint Level3BlockSize { set; get; }
        public uint Reserved03 { set; get; }

        public uint Reserved04 { set; get; }
        public uint OptionalInfoSize { set; get; }

        // IVFC Header
        public IvfcLevelInfo[] IvfcLevels { set; get; }
        public ulong BodyOffset { set; get; }
        public ulong BodySize { set; get; }

        // RomFS
        public uint RomFsHeaderSize { set; get; }
        public SimpleOffset[] RomFsSections { set; get; }
        public uint RomFsDataOffset { set; get; }

        public long DirectoryBlockOffset { set; get; }
        public long FileBlockOffset { set; get; }

        #endregion

        public Nintendo3dsCtrDirectory() { }

        public Nintendo3dsCtrDirectory(FileStream isoStream, string sourceFilePath, string parentDirectoryName,
            long offset, Nintendo3dsCtr.FileSystemType fileSystem, IvfcLevelInfo[] ivfcLevels, 
            long directoryEntryOffset = 0, long directoryBlockOffset = -1, long fileBlockOffset = -1, 
            uint romFsDataOffset = 0)
        {
            string nextDirectoryName;

            this.SourceFilePath = sourceFilePath;
            this.SubDirectoryArray = new ArrayList();
            this.FileArray = new ArrayList();
            this.ParentDirectoryName = parentDirectoryName;            

            this.FileSystem = fileSystem;
            this.SiblingDirectory = null;

            this.IvfcLevels = ivfcLevels;

            if (this.FileSystem == Nintendo3dsCtr.FileSystemType.ExeFS)
            {
                this.DirectoryName = "ExeFS"; // @TODO: Make a constant
                nextDirectoryName = this.ParentDirectoryName + Path.DirectorySeparatorChar + this.DirectoryName;
                this.InitializeExeFileSystem(isoStream, offset, nextDirectoryName);
            }
            else if (this.FileSystem == Nintendo3dsCtr.FileSystemType.RomFS)
            {
                this.DirectoryBlockOffset = directoryBlockOffset;
                this.FileBlockOffset = fileBlockOffset;

                if (directoryEntryOffset == 0)
                {
                    this.InitializeRomFileSystem(isoStream, offset);
                }
                else
                {
                    this.RomFsDataOffset = romFsDataOffset;
                }

                this.BuildDirectory(isoStream, offset, directoryEntryOffset, this.DirectoryBlockOffset, this.FileBlockOffset);               
            }        
        }

        #region RomFS Functions 

        private void ParseIvfcHeader(FileStream fs, long offset)
        {
            this.MagicBytes = ParseFile.ReadUintBE(fs, offset);

            // verify magic bytes
            if (this.MagicBytes != 0x49564643)
            {
                throw new FormatException(String.Format("IVFC bytes not found."));
            }

            this.VersionNumber = ParseFile.ReadUintLE(fs, offset + 0x04);
            this.MasterHashSize = ParseFile.ReadUintLE(fs, offset + 0x08);

            this.Level1Offset = ParseFile.ReadUlongLE(fs, offset + 0x0C);
            this.Level1HashDataSize = ParseFile.ReadUlongLE(fs, offset + 0x14);
            this.Level1BlockSize = ParseFile.ReadUintLE(fs, offset + 0x1C);
            this.Reserved01 = ParseFile.ReadUintLE(fs, offset + 0x20);

            this.Level2Offset = ParseFile.ReadUlongLE(fs, offset + 0x24);
            this.Level2HashDataSize = ParseFile.ReadUlongLE(fs, offset + 0x2C);
            this.Level2BlockSize = ParseFile.ReadUintLE(fs, offset + 0x34);
            this.Reserved02 = ParseFile.ReadUintLE(fs, offset + 0x38);

            this.Level3Offset = ParseFile.ReadUlongLE(fs, offset + 0x3C);
            this.Level3HashDataSize = ParseFile.ReadUlongLE(fs, offset + 0x44);
            this.Level3BlockSize = ParseFile.ReadUintLE(fs, offset + 0x4C);
            this.Reserved03 = ParseFile.ReadUintLE(fs, offset + 0x50);

            this.Reserved04 = ParseFile.ReadUintLE(fs, offset + 0x54);
            this.OptionalInfoSize = ParseFile.ReadUintLE(fs, offset + 0x58);
        }

        private void BuildIvfcLevels() // thanks to neimod's ctrtool
        {
            this.IvfcLevels = new IvfcLevelInfo[3];
            for (int i = 0; i < 3; i++) { this.IvfcLevels[i] = new IvfcLevelInfo(); }

            this.IvfcLevels[0].HashOffset = 0x60;
            this.IvfcLevels[0].HashBlockSize = (ulong)1 << (int)this.Level1BlockSize;
            this.IvfcLevels[1].HashBlockSize = (ulong)1 << (int)this.Level2BlockSize;
            this.IvfcLevels[2].HashBlockSize = (ulong)1 << (int)this.Level3BlockSize;

            this.BodyOffset = MathUtil.RoundUpToByteAlignment((this.IvfcLevels[0].HashOffset + (ulong)this.MasterHashSize),
                                                               this.IvfcLevels[2].HashBlockSize);
            this.BodySize = this.Level3HashDataSize;

            this.IvfcLevels[2].DataOffset = this.BodyOffset;
            this.IvfcLevels[2].DataSize = MathUtil.RoundUpToByteAlignment(this.BodySize, this.IvfcLevels[2].HashBlockSize);

            this.IvfcLevels[1].HashOffset = MathUtil.RoundUpToByteAlignment((this.BodyOffset + this.BodySize),
                                                                             this.IvfcLevels[2].HashBlockSize);
            this.IvfcLevels[2].HashOffset = this.IvfcLevels[1].HashOffset + this.Level2Offset - this.Level1Offset;

            this.IvfcLevels[1].DataOffset = this.IvfcLevels[2].HashOffset;
            this.IvfcLevels[1].DataSize = MathUtil.RoundUpToByteAlignment(this.Level2HashDataSize, this.IvfcLevels[1].HashBlockSize);

            this.IvfcLevels[0].DataOffset = this.IvfcLevels[1].HashOffset;
            this.IvfcLevels[0].DataSize = MathUtil.RoundUpToByteAlignment(this.Level1HashDataSize, this.IvfcLevels[0].HashBlockSize);
        }

        private void ValidateIvfcContainer(FileStream fs, long offset) // thanks to neimod's ctrtool
        {
            StringBuilder hashFailures = new StringBuilder();
            
            ulong blockCount;
            byte[] blockToHash;
            byte[] calculatedHash;
            byte[] testHash;
            HashAlgorithm cryptoHash = SHA256.Create();

            uint badHashCount = 0, goodHashCount = 0;

            // only check level 1 and 2 hash on load, level 3 takes too long and can be performed on extraction
            for (ulong i = 0; i < 3; i++)
            {
                // verify hash block to data size
                blockCount = this.IvfcLevels[i].DataSize / this.IvfcLevels[i].HashBlockSize;

                if ((blockCount * this.IvfcLevels[i].HashBlockSize) != this.IvfcLevels[i].DataSize)
                {
                    throw new Exception("Error, IVFC block mismatch.");
                }

                // calculate hash and compare
                for (ulong j = 0; j < blockCount; j++)
                {
                    testHash = ParseFile.ParseSimpleOffset(fs, offset + (long)this.IvfcLevels[i].HashOffset + (long)(0x20 * j), 0x20);

                    blockToHash = ParseFile.ParseSimpleOffset(fs, offset + (long)this.IvfcLevels[i].DataOffset + ((long)this.IvfcLevels[i].HashBlockSize * (long)j), (int)this.IvfcLevels[i].HashBlockSize);
                    calculatedHash = cryptoHash.ComputeHash(blockToHash);

                    if (!ParseFile.CompareSegment(calculatedHash, 0, testHash))
                    {
                        badHashCount++;
                    }
                    else 
                    {
                        goodHashCount++;
                    }
                }

                if (badHashCount > 0)
                {
                    hashFailures.AppendFormat("IVFC hash failure(s) in Level {0}.  Good Blocks: {1}  Bad Blocks: {2}{3}", i + 1, goodHashCount.ToString(), badHashCount.ToString(), Environment.NewLine);
                }
                else
                {
                    hashFailures.AppendFormat("No IVFC hash failures found in Level {0} check.{1}", i + 1, Environment.NewLine);
                }
            } // for (ulong i = 0;...

            // display warning about hash failures
            if (hashFailures.Length > 0)
            {
                //hashFailures.Insert(0, String.Format("Hash Failures when Validating .3DS file.  This file is corrupted.{0}{0}", Environment.NewLine));
                //MessageBox.Show(hashFailures.ToString(), "Warning: Hash Failures in .3DS file.");
                MessageBox.Show(hashFailures.ToString(), "Hash Validation Results");
            }
        }

        private void ParseRomFsHeader(FileStream fs, long offset)
        {
            this.RomFsHeaderSize = ParseFile.ReadUintLE(fs, offset + 0x1000);

            this.RomFsSections = new SimpleOffset[4];
            for (int i = 0; i < 4; i++)
            {
                this.RomFsSections[i] = new SimpleOffset((ParseFile.ReadUintLE(fs, offset + 0x1004 + (8 * i))),
                                                          ParseFile.ReadUintLE(fs, offset + 0x1004 + ((8 * i) + 4)));
            }

            this.DirectoryBlockOffset = (long)(this.RomFsSections[1].Offset + 0x1000);
            this.FileBlockOffset = (long)(this.RomFsSections[3].Offset + 0x1000);
            this.RomFsDataOffset = ParseFile.ReadUintLE(fs, offset + 0x1024) + 0x1000;
        }

        private void BuildDirectory(FileStream isoStream, long ivfcOffset, long directoryEntryOffset, long directoryBlockOffset, 
            long fileBlockOffset)
        {
            RomFsDirEntry dir = new RomFsDirEntry();
            RomFsFileEntry file = new RomFsFileEntry();
            Nintendo3dsCtrDirectory tempDir;
            Nintendo3dsCtrFile tempFile;

            byte[] nameBytes;
            string nextDirectory;

            // load dir
            dir.ParentOffset = ParseFile.ReadUintLE(isoStream, ivfcOffset + directoryBlockOffset + directoryEntryOffset);
            dir.SiblingOffset = ParseFile.ReadInt32LE(isoStream, ivfcOffset + directoryBlockOffset + directoryEntryOffset + 4);
            dir.ChildOffset = ParseFile.ReadInt32LE(isoStream, ivfcOffset + directoryBlockOffset + directoryEntryOffset + 8);
            dir.FileOffset = ParseFile.ReadInt32LE(isoStream, ivfcOffset + directoryBlockOffset + directoryEntryOffset + 0xC);
            dir.WeirdOffset = ParseFile.ReadInt32LE(isoStream, ivfcOffset + directoryBlockOffset + directoryEntryOffset + 0x10);
            dir.NameSize = ParseFile.ReadUintLE(isoStream, ivfcOffset + directoryBlockOffset + directoryEntryOffset + 0x14);

            // build directory name
            if (dir.NameSize > 0)
            {
                nameBytes = ParseFile.ParseSimpleOffset(isoStream, ivfcOffset + directoryBlockOffset + directoryEntryOffset + 0x18, (int)dir.NameSize);
                dir.Name = ByteConversion.GetUtf16LeText(nameBytes);
            }
            else // this is root
            {
                dir.Name = "RomFS"; // @TODO Make this a constant
            }

            this.DirectoryName = dir.Name;
            nextDirectory = this.ParentDirectoryName + Path.DirectorySeparatorChar + this.DirectoryName;

            // add files
            if (dir.FileOffset != -1)
            {                
                tempFile = new Nintendo3dsCtrFile(isoStream, nextDirectory, isoStream.Name,
                    null, ivfcOffset, -1, Nintendo3dsCtr.FileSystemType.RomFS, this.IvfcLevels[2], 
                    dir.FileOffset, fileBlockOffset, this.RomFsDataOffset);
                this.FileArray.Add(tempFile);

                while (tempFile.SiblingOffset != - 1)
                {
                    tempFile = new Nintendo3dsCtrFile(isoStream, nextDirectory, isoStream.Name,
                        null, ivfcOffset, -1, Nintendo3dsCtr.FileSystemType.RomFS, this.IvfcLevels[2], 
                        tempFile.SiblingOffset, fileBlockOffset, this.RomFsDataOffset);

                    this.FileArray.Add(tempFile);
                }

            }

            // get sibling dir
            if (dir.SiblingOffset != -1)
            {
                tempDir = new Nintendo3dsCtrDirectory(isoStream, isoStream.Name, this.ParentDirectoryName,
                    ivfcOffset, Nintendo3dsCtr.FileSystemType.RomFS, this.IvfcLevels, dir.SiblingOffset, directoryBlockOffset, 
                    fileBlockOffset, this.RomFsDataOffset);
                this.SiblingDirectory = tempDir;
            }                                

            // add subdirs
            if (dir.ChildOffset != -1)
            {
                tempDir = new Nintendo3dsCtrDirectory(isoStream, isoStream.Name, nextDirectory,
                    ivfcOffset, Nintendo3dsCtr.FileSystemType.RomFS, this.IvfcLevels, dir.ChildOffset, directoryBlockOffset, 
                    fileBlockOffset, this.RomFsDataOffset);
                this.SubDirectoryArray.Add(tempDir);

                while (tempDir.SiblingDirectory != null)
                {
                    this.SubDirectoryArray.Add(tempDir.SiblingDirectory);
                    tempDir = tempDir.SiblingDirectory;
                }
            }                                
        }

        public void InitializeRomFileSystem(FileStream isoStream, long offset)
        {
            this.ParseIvfcHeader(isoStream, offset);

            if (this.IvfcLevels == null)
            {
                this.BuildIvfcLevels();
            }

            DialogResult m = MessageBox.Show("Verify partition against internal hashes/checksums?", "Verify Integrity?", MessageBoxButtons.YesNo);

            if (m == DialogResult.Yes)
            {
                this.ValidateIvfcContainer(isoStream, offset);
            }

            this.ParseRomFsHeader(isoStream, offset);       
        }
        
        
        #endregion

        #region ExeFS Functions

        public void InitializeExeFileSystem(FileStream isoStream, long offset, string nextDirectoryName)
        {
            string fileName;
            long fileOffset;
            uint fileLength;
            byte[] hash;
            long hashOffset;

            Nintendo3dsCtrFile file;
            
            // parse file entries
            for (int i = 0; i < 10; i++)
            {
                // check if row has data
                if (ParseFile.ReadByte(isoStream, offset + (0x10 * i)) != 0)
                {
                    // read VFS items
                    fileName = ParseFile.ReadAsciiString(isoStream, offset + (0x10 * i));

                    fileOffset = ParseFile.ReadUintLE(isoStream, offset + (0x10 * i) + 8);
                    fileOffset += offset + 0x200;

                    fileLength = ParseFile.ReadUintLE(isoStream, offset + (0x10 * i) + 0xC);

                    // read SHA256 hash
                    hashOffset = offset + 0x200 - (0x20 * (i + 1));
                    hash = ParseFile.ParseSimpleOffset(isoStream, hashOffset, 0x20);

                    // build file object
                    file = new Nintendo3dsCtrFile(isoStream, nextDirectoryName, this.SourceFilePath,
                        fileName, fileOffset, fileLength, this.FileSystem, null);
                    file.Sha256Hash = hash;
                    this.FileArray.Add(file);
                }
            }        
        }
        
        #endregion

        public int CompareTo(object obj)
        {
            if ((obj is Nintendo3dsCtrDirectory))
            {
                Nintendo3dsCtrDirectory o = (Nintendo3dsCtrDirectory)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not an Nintendo3dsCtrDirectory");
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string fullDirectoryPath = Path.Combine(destinationFolder, Path.Combine(this.ParentDirectoryName, this.DirectoryName));

            // create directory
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            foreach (Nintendo3dsCtrFile f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (Nintendo3dsCtrDirectory d in this.SubDirectoryArray)
            {
                d.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }            
        }
    }

    public class Nintendo3dsCtrHashFailureException : Exception
    {
        public Nintendo3dsCtrHashFailureException() { }

        public Nintendo3dsCtrHashFailureException(string message)
            : base(message) { }

        public Nintendo3dsCtrHashFailureException(string message, Exception inner)
            : base(message, inner) { }
    }
}
