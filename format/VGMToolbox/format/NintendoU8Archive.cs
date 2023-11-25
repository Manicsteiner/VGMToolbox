using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.format.iso;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class NintendoU8Archive : IVolume
    {
        public static readonly byte[] STANDARD_IDENTIFIER = new byte[] { 0x55, 0xAA, 0x38, 0x2D };
        public const uint IDENTIFIER_OFFSET = 0x00;
        public const string FORMAT_DESCRIPTION_STRING = "Nintendo U8";
        public const string EXTRACTION_FOLDER = "VGMT_U8_EXTRACT";

        public const byte NODE_TYPE_FILE = 0;
        public const byte NODE_TYPE_DIRECTORY = 1;

        public struct u8Node
        {
            public byte NodeType { set; get; }
            public uint NameOffset { set; get; }
            public uint DataOffset { set; get; }
            public uint DataSize { set; get; }

            public bool IsDirectory()
            {
                return (NodeType == NintendoU8Archive.NODE_TYPE_DIRECTORY);
            }
        }
        
        public string SourceFileName { set; get; }

        public uint RootNodeOffset { set; get; }
        public uint HeaderOffset { set; get; }
        public uint DataOffset { set; get; }
        public uint NameTableOffset { set; get; }

        public ArrayList NodeArray { set; get; }
        public u8Node[] NodeList 
        {
            set { NodeList = value; }
            get 
            {
                return (u8Node[])NodeArray.ToArray(typeof(u8Node));
            }
        }

        #region IVolume 
        public ArrayList DirectoryStructureArray { set; get; }
        public IDirectoryStructure[] Directories
        {
            set { Directories = value; }
            get
            {
                DirectoryStructureArray.Sort();
                return (IDirectoryStructure[])DirectoryStructureArray.ToArray(typeof(NintendoU8Directory));
            }
        }

        public long VolumeBaseOffset { set; get; }
        public string FormatDescription { set; get; }
        public VolumeDataType VolumeType { set; get; }
        public string VolumeIdentifier { set; get; }
        public bool IsRawDump { set; get; }

        #endregion

        public NintendoU8Archive(string sourceFile)
        { 
            // check magic bytes
            if (NintendoU8Archive.IsU8File(sourceFile))
            {
                // read header
                using (FileStream fs = File.OpenRead(sourceFile))
                {
                    // set source file
                    this.SourceFileName = sourceFile; 

                    // initialize
                    this.Initialize(fs, NintendoU8Archive.IDENTIFIER_OFFSET, false);

                } // using (FileStream fs = File.OpenRead(sourceFile))
            }
            else
            {
                throw new FormatException("Nintendo U8 magic bytes not found at offset 0x00.");
            }
        }

        public void Initialize(FileStream fs, long offset, bool isRawDump)
        {
            this.FormatDescription = NintendoU8Archive.FORMAT_DESCRIPTION_STRING;

            this.VolumeBaseOffset = offset;
            this.IsRawDump = isRawDump;
            this.VolumeType = VolumeDataType.Data;
            this.DirectoryStructureArray = new ArrayList();

            // parse header
            this.RootNodeOffset = ParseFile.ReadUintBE(fs, 4);
            this.HeaderOffset = ParseFile.ReadUintBE(fs, 8);
            this.DataOffset = ParseFile.ReadUintBE(fs, 0xC);

            // parse nodes
            this.NodeArray = new ArrayList();
            this.ParseNodes(fs);
            this.NameTableOffset = this.RootNodeOffset + ((uint)this.NodeList.Length * 0xC);

            // build directory structure
            this.BuildDirectoryTree(fs);
        }

        private NintendoU8Directory getDirectoryNode(FileStream fs, int nodeIndex, 
            string parentDirectory, out int lastIndexProcessed)
        {
            string newParentDirectoryName;
            uint maxNodeIndex;
            NintendoU8Directory newSubDirectory;
            
            lastIndexProcessed = nodeIndex;

            // get node
            u8Node node = this.NodeList[nodeIndex];
            
            // get directory name
            string directoryName = ParseFile.ReadAsciiString(fs, this.NameTableOffset + node.NameOffset);            

            // create directory item    
            NintendoU8Directory newDirectory = new NintendoU8Directory(this.SourceFileName, directoryName, parentDirectory);

            // parse other items
            maxNodeIndex = node.DataSize;


            for (int i = (nodeIndex + 1); i < maxNodeIndex; i = (lastIndexProcessed + 1))
            {
                node = this.NodeList[i];
                newParentDirectoryName = Path.Combine(parentDirectory, directoryName);

                if (node.IsDirectory())
                {
                    newSubDirectory = this.getDirectoryNode(fs, i, newParentDirectoryName, out lastIndexProcessed);

                    if (!newSubDirectory.DirectoryName.Equals("."))
                    {
                        newDirectory.SubDirectoryArray.Add(newSubDirectory);
                    }
                    else // in current folder "."
                    {
                        // move folders to this folder
                        foreach (NintendoU8Directory d in newSubDirectory.SubDirectories)
                        {
                            newDirectory.SubDirectoryArray.Add(d);
                        }

                        // move files to this folder
                        foreach (NintendoU8File f in newSubDirectory.Files)
                        {
                            newDirectory.FileArray.Add(f);
                        }
                    }
                }
                else
                {
                    newDirectory.FileArray.Add(this.getFileNode(fs, i, newParentDirectoryName));
                    lastIndexProcessed = i;
                }                
            }



            return newDirectory;
        }

        private NintendoU8File getFileNode(FileStream fs, int nodeIndex, string parentDirectory)
        {
            // get node
            u8Node node = this.NodeList[nodeIndex];

            // get directory name
            string fileName = ParseFile.ReadAsciiString(fs, this.NameTableOffset + node.NameOffset);

            // create file item    
            NintendoU8File newFile = new NintendoU8File(parentDirectory, this.SourceFileName, 
                fileName, node.DataOffset, this.VolumeBaseOffset, node.DataOffset, node.DataSize);

            return newFile;
        }

        public void BuildDirectoryTree(FileStream fs)
        {
            int dummy;
            this.DirectoryStructureArray.Add(this.getDirectoryNode(fs, 0, String.Empty, out dummy));
        }

        /// <summary>
        /// Parse directory/file nodes.
        /// </summary>
        /// <param name="fs">Filestream of U8 file.</param>
        public void ParseNodes(FileStream fs)
        { 
            u8Node node = new u8Node();
            uint maxNodeId;

            // read first node
            node.NodeType = ParseFile.ReadByte(fs, this.RootNodeOffset);
            node.NameOffset = ParseFile.ReadUint24BE(fs, this.RootNodeOffset + 1);
            node.DataOffset = ParseFile.ReadUintBE(fs, this.RootNodeOffset + 4);
            node.DataSize = ParseFile.ReadUintBE(fs, this.RootNodeOffset + 8);
            this.NodeArray.Add(node);

            maxNodeId = node.DataSize;

            // parse each node
            for (int i = 1; i < maxNodeId; i++)
            {
                node = new u8Node();
                node.NodeType = ParseFile.ReadByte(fs, this.RootNodeOffset + (i * 0xC));
                node.NameOffset = ParseFile.ReadUint24BE(fs, this.RootNodeOffset + (i * 0xC) + 1);
                node.DataOffset = ParseFile.ReadUintBE(fs, this.RootNodeOffset + (i * 0xC) + 4);
                node.DataSize = ParseFile.ReadUintBE(fs, this.RootNodeOffset + (i * 0xC) + 8);                                                
                this.NodeArray.Add(node);
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
            foreach (NintendoU8Directory ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, destintionFolder, extractAsRaw);
            }
        }

        public void ExtractAll()
        {
            string outputFolder = this.GetUnpackFolder();
            Dictionary<string, FileStream> streamCache = new Dictionary<string, FileStream>();

            foreach (NintendoU8Directory ds in this.DirectoryStructureArray)
            {
                ds.Extract(ref streamCache, outputFolder, false);
            }
        }

        /// <summary>
        /// Builds the output folder path for unpacking.
        /// </summary>
        /// <returns>Full path of folder for unpacking.</returns>
        private string GetUnpackFolder()
        {
            string unpackFolder;

            unpackFolder = Path.Combine(
                Path.GetDirectoryName(this.SourceFileName),
                String.Format("{0}_{1}", NintendoU8Archive.EXTRACTION_FOLDER, Path.GetFileNameWithoutExtension(this.SourceFileName)));
            
            return unpackFolder;
        }

        /// <summary>
        /// Checks for U8 file Magic Bytes.
        /// </summary>
        /// <param name="sourceFile">Full path to file to check.</param>
        /// <returns>Boolean value indicating if input file has U8 magic bytes.</returns>
        public static bool IsU8File(string sourceFile)
        {
            bool isU8 = false;
            byte[] magicBytes = new byte[NintendoU8Archive.STANDARD_IDENTIFIER.Length];

            using (FileStream fs = File.OpenRead(sourceFile))
            {
                magicBytes = ParseFile.ParseSimpleOffset(fs, NintendoU8Archive.IDENTIFIER_OFFSET, NintendoU8Archive.STANDARD_IDENTIFIER.Length);

                if (ParseFile.CompareSegment(magicBytes, 0, NintendoU8Archive.STANDARD_IDENTIFIER))
                {
                    isU8 = true;
                }
            }

            return isU8;
        }
    }



    public class NintendoU8Directory : IDirectoryStructure
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
                return (IDirectoryStructure[])SubDirectoryArray.ToArray(typeof(NintendoU8Directory));
            }
        }

        public ArrayList FileArray { set; get; }
        public IFileStructure[] Files
        {
            set { this.Files = value; }
            get
            {
                FileArray.Sort();
                return (IFileStructure[])FileArray.ToArray(typeof(NintendoU8File));
            }
        }

        public NintendoU8Directory(string sourceFilePath, string directoryName,
            string parentDirectoryName)
        {
            this.SourceFilePath = sourceFilePath;
            this.SubDirectoryArray = new ArrayList();
            this.FileArray = new ArrayList();

            this.ParentDirectoryName = parentDirectoryName;
            this.DirectoryName = directoryName;
        }
        
        public int CompareTo(object obj)
        {
            if (obj is NintendoU8Directory)
            {
                NintendoU8Directory o = (NintendoU8Directory)obj;

                return this.DirectoryName.CompareTo(o.DirectoryName);
            }

            throw new ArgumentException("object is not a NintendoU8Directory");
        }

        public void Extract(ref Dictionary<string, FileStream> streamCache, string destinationFolder, bool extractAsRaw)
        {
            string fullDirectoryPath = Path.Combine(destinationFolder, Path.Combine(this.ParentDirectoryName, this.DirectoryName));

            // create directory
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            foreach (NintendoU8File f in this.FileArray)
            {
                f.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }

            foreach (NintendoU8Directory d in this.SubDirectoryArray)
            {
                d.Extract(ref streamCache, destinationFolder, extractAsRaw);
            }
        }
    }

    public class NintendoU8File : IFileStructure
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
            if (obj is NintendoU8File)
            {
                NintendoU8File o = (NintendoU8File)obj;

                return this.FileName.CompareTo(o.FileName);
            }

            throw new ArgumentException("object is not a NintendoU8File");
        }

        public NintendoU8File(string parentDirectoryName, string sourceFilePath, 
            string fileName, long offset, long volumeBaseOffset, long lba, long size)
        {
            this.ParentDirectoryName = parentDirectoryName;
            this.SourceFilePath = sourceFilePath;
            this.FileName = fileName;
            this.Offset = offset;
            this.Size = size;
            this.FileDateTime = new DateTime();

            this.VolumeBaseOffset = volumeBaseOffset;
            this.Lba =lba;
            this.IsRaw = false;
            this.NonRawSectorSize = 0;
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
}
