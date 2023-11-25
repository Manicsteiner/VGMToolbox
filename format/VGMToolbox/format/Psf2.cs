using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Ionic.Zlib;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Psf2 : Psf
    {
        public const string SQ_FILE = "SQ.IRX";
        public const string FILE_EXTENSION_BD = ".bd";
        public const string FILE_EXTENSION_HD = ".hd";
        public const string FILE_EXTENSION_SQ = ".sq";

        public static readonly byte[] HD_SIGNATURE = new byte[] { 0x49, 0x45, 0x43, 0x53,
                                                                  0x64, 0x61, 0x65, 0x48 };

        public struct Psf2IniSqIrxStruct
        {
            public string SqFileName;
            public string HdFileName;
            public string BdFileName;

            public string SequenceNumber;
            public string TimerTickInterval;
            public string Reverb;
            public string Depth;
            public string Tempo;
            public string Volume;
        }

        public struct Psf2DirectoryEntry
        {
            public string Filename;
            public UInt32 Offset;
            public UInt32 UncompressedSize;
            public UInt32 BlockSize;
        }

        public static Psf2IniSqIrxStruct ParseClsIniFile(Stream pStream)
        {
            string currentLine;
            string[] splitLine;
            string[] splitItem;
            char[] splitDelimeters = new char[] {' '};
            char[] splitItemDelimeter = new char[] {'='};

            Psf2IniSqIrxStruct ret = new Psf2IniSqIrxStruct();

            // get original stream position in case needed by caller
            long originalStreamPosition = pStream.Position;
            
            pStream.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(pStream);

            while ((currentLine = sr.ReadLine()) != null)
            { 
                // check for the SQ.IRX line
                if (currentLine.Trim().ToUpper().StartsWith(SQ_FILE))
                {
                    splitLine = currentLine.Trim().Split(splitDelimeters, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string s in splitLine)
                    {
                        if (s.Contains("=")) // we have an option
                        {
                            splitItem = s.Trim().Split(splitItemDelimeter, StringSplitOptions.RemoveEmptyEntries);

                            switch (splitItem[0])
                            { 
                                case "-s":
                                    ret.SqFileName = splitItem[1].ToUpper();
                                    break;
                                case "-h":
                                    ret.HdFileName = splitItem[1].ToUpper();
                                    break;
                                case "-b":
                                    ret.BdFileName = splitItem[1].ToUpper();
                                    break;
                                case "-n":
                                    ret.SequenceNumber = splitItem[1];
                                    break;
                                case "-u":
                                    ret.TimerTickInterval = splitItem[1];
                                    break;
                                case "-r":
                                    ret.Reverb = splitItem[1];
                                    break;
                                case "-d":
                                    ret.Depth = splitItem[1];
                                    break;
                                case "-t":
                                    ret.Tempo = splitItem[1];
                                    break;
                                case "-v":
                                    ret.Volume = splitItem[1];
                                    break;
                            }
                        }
                    }

                    // add defaults if needed
                    if (String.IsNullOrEmpty(ret.BdFileName)) 
                    { 
                        ret.BdFileName = "DEFAULT.BD"; 
                    }
                    if (String.IsNullOrEmpty(ret.HdFileName)) 
                    { 
                        ret.HdFileName = "DEFAULT.HD"; 
                    }
                    if (String.IsNullOrEmpty(ret.SqFileName)) 
                    { 
                        ret.SqFileName = "DEFAULT.SQ"; 
                    }

                    break;
                }
            }

            // return to incoming position
            pStream.Seek(originalStreamPosition, SeekOrigin.Begin);

            return ret;
        }

        public static void WriteClsIniFile(Psf2IniSqIrxStruct pPsf2IniSqIrxStruct, string pOutputPath)
        {
            StringBuilder sqArguments;
            
            using (StreamWriter sw = new StreamWriter(File.Open(pOutputPath, FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine("libsd.irx");
                sw.WriteLine("modhsyn.irx");
                sw.WriteLine("modmidi.irx");

                sqArguments = new StringBuilder();

                // build sq.irx arguments                    
                sqArguments.Append(String.IsNullOrEmpty(pPsf2IniSqIrxStruct.Reverb) ?
                    " -r=5" : String.Format(" -r={0}", pPsf2IniSqIrxStruct.Reverb.Trim()));
                sqArguments.Append(String.IsNullOrEmpty(pPsf2IniSqIrxStruct.Depth) ?
                    " -d=16383" : String.Format(" -d={0}", pPsf2IniSqIrxStruct.Depth.Trim()));

                sqArguments.Append(String.IsNullOrEmpty(pPsf2IniSqIrxStruct.SequenceNumber) ?
                    String.Empty : String.Format(" -n={0}", pPsf2IniSqIrxStruct.SequenceNumber.Trim()));
                sqArguments.Append(String.IsNullOrEmpty(pPsf2IniSqIrxStruct.TimerTickInterval) ?
                    String.Empty : String.Format(" -u={0}", pPsf2IniSqIrxStruct.TimerTickInterval.Trim()));
                sqArguments.Append(String.IsNullOrEmpty(pPsf2IniSqIrxStruct.Tempo) ?
                    String.Empty : String.Format(" -t={0}", pPsf2IniSqIrxStruct.Tempo.Trim()));
                sqArguments.Append(String.IsNullOrEmpty(pPsf2IniSqIrxStruct.Volume) ?
                    String.Empty : String.Format(" -v={0}", pPsf2IniSqIrxStruct.Volume.Trim()));

                sqArguments.Append(String.Format(" -s={0} -h={1} -b={2}",
                    pPsf2IniSqIrxStruct.SqFileName, pPsf2IniSqIrxStruct.HdFileName, 
                    pPsf2IniSqIrxStruct.BdFileName));

                sw.WriteLine(String.Format("sq.irx {0}", sqArguments.ToString()));
                
                sw.Write(Environment.NewLine);
                sw.Close();
            }
        }

        public void Unpack(string pDestinationFolder)
        {
            Psf2DirectoryEntry[] directoryEntries = this.getDirectoryEntries(Xsf.RESERVED_SECTION_OFFSET);
            this.extractDirectory(directoryEntries, pDestinationFolder);
        }

        private Psf2DirectoryEntry[] getDirectoryEntries(UInt32 pOffset)
        {
            UInt32 numberOfDirectoryEntries;
            Psf2DirectoryEntry[] ret;
            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            using (FileStream fs = File.Open(this.FilePath, FileMode.Open, FileAccess.Read))
            {
                numberOfDirectoryEntries =
                    BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, pOffset, 4), 0);
                ret = new Psf2DirectoryEntry[numberOfDirectoryEntries];

                for (UInt32 i = 0; i < numberOfDirectoryEntries; i++)
                {
                    ret[i].Filename =
                        enc.GetString(FileUtil.ReplaceNullByteWithSpace(ParseFile.ParseSimpleOffset(fs, (pOffset + 4) + (48 * i), 36))).Trim();
                    ret[i].Offset =
                        Xsf.RESERVED_SECTION_OFFSET +
                        BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, (pOffset + 4) + (48 * i) + 36, 4), 0);
                    ret[i].UncompressedSize =
                        BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, (pOffset + 4) + (48 * i) + 40, 4), 0);
                    ret[i].BlockSize =
                        BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, (pOffset + 4) + (48 * i) + 44, 4), 0);
                }
            }
            return ret;
        }

        private void extractDirectory(Psf2DirectoryEntry[] pPsf2DirectoryEntries, string pDestinationFolder)
        {
            Psf2DirectoryEntry[] directoryEntries;
            string outputFolder = Path.GetFullPath(pDestinationFolder);            
            string filename;

            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            // create destination directory if needed
            if (!Directory.Exists(pDestinationFolder))
            {
                Directory.CreateDirectory(pDestinationFolder);
            }

            // loop through entries and process
            foreach (Psf2DirectoryEntry p in pPsf2DirectoryEntries)
            {
                filename = Path.Combine(pDestinationFolder, p.Filename);

                if (IsSubdirectory(p))
                {
                    directoryEntries = this.getDirectoryEntries(p.Offset);
                    this.extractDirectory(directoryEntries, filename);
                }
                else
                {
                    this.extractFile(p, filename);
                }
            }
        }
        
        private void extractFile(Psf2DirectoryEntry pPsf2DirectoryEntry, string pDestinationFile)
        {            
            uint blockCount = 
                ((pPsf2DirectoryEntry.UncompressedSize + pPsf2DirectoryEntry.BlockSize) - 1) / pPsf2DirectoryEntry.BlockSize;
            UInt32[] blockTable = new UInt32[blockCount];

            string tempCompressedFile = Path.GetTempFileName();

            byte[] uncompressedBlock;
            byte[] compressedBlock;
            int bytesRead;
            int bytesInflated;
            long totalBytesInflated = 0;

            MemoryStream compressedBytesStream;

            using (FileStream pfs = File.Open(this.FilePath, FileMode.Open, FileAccess.Read))
            {
                pfs.Position = (long) pPsf2DirectoryEntry.Offset;

                for (int i = 0; i < blockCount; i++)
                {
                    blockTable[i] =
                        BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pfs, (long)pPsf2DirectoryEntry.Offset + (i * 4), 4), 0);
                }

                pfs.Position = (long)(pPsf2DirectoryEntry.Offset + (blockCount * 4));

                if (!IsEmptyFile(pPsf2DirectoryEntry))
                {
                    // write compressed bytes to mem stream and decompress to file                                                       
                    foreach (UInt32 b in blockTable)
                    {
                        compressedBlock = new byte[b];
                        bytesRead = pfs.Read(compressedBlock, 0, (int)b);
                        using (compressedBytesStream = new MemoryStream(compressedBlock, 0, bytesRead))
                        {
                            using (FileStream outputFs = File.Open(pDestinationFile, FileMode.Append, FileAccess.Write))
                            {
                                using (ZlibStream zs = new ZlibStream(compressedBytesStream, CompressionMode.Decompress, true))
                                {
                                    if ((int)b > 0x2000)
                                    {
                                        zs.BufferSize = (int)b;
                                    }
                                    else
                                    {
                                        zs.BufferSize = 0x2000;
                                    }
                                    
                                    uncompressedBlock = new byte[pPsf2DirectoryEntry.BlockSize];

                                    using (BinaryWriter bw = new BinaryWriter(outputFs))
                                    {
                                        while ((bytesInflated = zs.Read(uncompressedBlock, 0, uncompressedBlock.Length)) > 0)
                                        {
                                            totalBytesInflated += bytesInflated;
                                            bw.Write(uncompressedBlock, 0, bytesInflated);
                                        }
                                    }
                                }
                            }
                        }
                    }                                                   
                }                
            }
        }
                
        public static bool IsEmptyFile(Psf2DirectoryEntry pPsf2DirectoryEntry)
        {
            return (pPsf2DirectoryEntry.UncompressedSize == 0 &&
                    pPsf2DirectoryEntry.BlockSize == 0 &&
                    pPsf2DirectoryEntry.Offset == 0);
        }

        public static bool IsSubdirectory(Psf2DirectoryEntry pPsf2DirectoryEntry)
        {
            return (pPsf2DirectoryEntry.UncompressedSize == 0 &&
                    pPsf2DirectoryEntry.BlockSize == 0 &&
                    pPsf2DirectoryEntry.Offset != 0);
        }
    }
}
