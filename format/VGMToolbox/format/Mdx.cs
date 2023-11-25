using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Mdx : IFormat, ISingleTagFormat
    {
        private const string FORMAT_ABBREVIATION = "MDX";
        
        public static readonly byte[] TITLE_TERMINATOR = new byte[3] { 0x0D, 0x0A, 0x1A };
        public static readonly byte[] PDX_TERMINATOR = new byte[1] { 0x00 };
        public static readonly string MDX_FILE_EXTENSION = ".MDX";
        public static readonly string PDX_FILE_EXTENSION = ".PDX";
        public static readonly string EXCEPTION_PDX_MISSING = "PDX file for this file was not found.";

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private string title = String.Empty;
        private string pdxFileName = String.Empty;

        private byte[] titleBytes;
        private byte[] pdxBytes;
        private byte[] dataBytes;
        
        private int titleLength;

        private int pdxOffset;
        private int pdxLength;

        private int dataOffset;
        private int dataLength;

        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        public Mdx() { }

        #region METHODS

        public Mdx(byte[] pBytes)
        {
            initialize(pBytes);
        }

        public string Title { get { return title; } }
        public string PdxFileName { get { return pdxFileName; } }


        public void initialize(byte[] pBytes)
        {
            // int _titleLength;
            // int _pdxLength;

            // Title
            this.titleLength = ParseFile.GetSegmentLength(pBytes, 0, TITLE_TERMINATOR);

            if (this.titleLength > 0)
            {
                title = VGMToolbox.util.ByteConversion.GetJapaneseEncodedText(ParseFile.ParseSimpleOffset(pBytes, 0,
                         this.titleLength));
            }

            // PDX
            this.pdxOffset = this.titleLength + TITLE_TERMINATOR.Length;
            this.pdxLength = ParseFile.GetSegmentLength(pBytes,
                            (this.titleLength + TITLE_TERMINATOR.Length), PDX_TERMINATOR);
            if (this.pdxLength > 0)
            {
                pdxFileName = VGMToolbox.util.ByteConversion.GetJapaneseEncodedText(ParseFile.ParseSimpleOffset(pBytes,
                                (this.titleLength + TITLE_TERMINATOR.Length), this.pdxLength));
            }

            this.dataOffset = this.titleLength + TITLE_TERMINATOR.Length +
                this.pdxLength + PDX_TERMINATOR.Length;
            this.dataLength = (int)(pBytes.Length - this.dataOffset);
        }

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath = pFilePath;
            
            // Title
            this.titleLength = ParseFile.GetSegmentLength(pStream, 0, TITLE_TERMINATOR);
            
            if (this.titleLength > 0)
            {
                this.titleBytes = ParseFile.ParseSimpleOffset(pStream, 0, this.titleLength);
                this.title = VGMToolbox.util.ByteConversion.GetJapaneseEncodedText(this.titleBytes);
            }

            // PDX
            this.pdxOffset = this.titleLength + TITLE_TERMINATOR.Length;
            this.pdxLength = ParseFile.GetSegmentLength(pStream, this.pdxOffset, PDX_TERMINATOR);

            if (this.pdxLength > 0)
            {
                this.pdxBytes = ParseFile.ParseSimpleOffset(pStream, this.pdxOffset, this.pdxLength);
                this.pdxFileName = VGMToolbox.util.ByteConversion.GetJapaneseEncodedText(this.pdxBytes);
                
                string pdxExtension = Path.GetExtension(this.pdxFileName);

                if (pdxExtension.Equals(String.Empty))
                {
                    this.pdxFileName += PDX_FILE_EXTENSION;
                }
            }

            this.initializeTagHash();

            this.dataOffset = this.titleLength + TITLE_TERMINATOR.Length +
                this.pdxLength + PDX_TERMINATOR.Length;
            this.dataLength = (int)(pStream.Length - this.dataOffset);
            this.dataBytes = ParseFile.ParseSimpleOffset(pStream, this.dataOffset, this.dataLength);
        }

        private void initializeTagHash()
        {
            tagHash.Add("Title", this.title);

            if (pdxFileName != null && !this.pdxFileName.Equals(String.Empty))
            {
                tagHash.Add("PDX", pdxFileName);
            }
        }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            int read;
            byte[] data;
            FileStream fs;

            pChecksum.Reset();

            // Add data segment
            if (this.dataLength > 0)
            {
                pChecksum.Update(this.dataBytes);
            }

            // Add PDX if found
            if (pdxFileName != null && !this.pdxFileName.Equals(String.Empty))
            {
                string pdxPath = Path.GetDirectoryName(this.filePath) + 
                    Path.DirectorySeparatorChar + this.pdxFileName;

                if (File.Exists(pdxPath))
                {
                    data = new byte[4096];

                    fs = new FileStream(pdxPath, FileMode.Open, FileAccess.Read);
                    fs.Seek(this.dataOffset, SeekOrigin.Begin);

                    while ((read = fs.Read(data, 0, data.Length)) > 0)
                    {
                        pChecksum.Update(data, 0, read);
                    }

                    fs.Close();
                    fs.Dispose();
                }
                else  // Cannot be sure of unique value, raise exception to cause
                {     //   fall back to full file checksum

                    pChecksum.Reset();
                    throw new IOException(EXCEPTION_PDX_MISSING);
                }
            }
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            int read;
            byte[] data;
            FileStream fs;

            pChecksum.Reset();

            // Add data segment
            if (this.dataLength > 0)
            {
                pChecksum.Update(this.dataBytes);
                pMd5CryptoStream.Write(this.dataBytes, 0, this.dataBytes.Length);
                pSha1CryptoStream.Write(this.dataBytes, 0, this.dataBytes.Length);
            }

            // Add PDX if found
            if (pdxFileName != null && !this.pdxFileName.Equals(String.Empty))
            {
                string pdxPath = Path.GetDirectoryName(this.filePath) +
                    Path.DirectorySeparatorChar + this.pdxFileName;

                if (File.Exists(pdxPath))
                {
                    data = new byte[4096];

                    fs = new FileStream(pdxPath, FileMode.Open, FileAccess.Read);
                    fs.Seek(this.dataOffset, SeekOrigin.Begin);

                    while ((read = fs.Read(data, 0, data.Length)) > 0)
                    {
                        pChecksum.Update(data, 0, read);
                        pMd5CryptoStream.Write(data, 0, read);
                        pSha1CryptoStream.Write(data, 0, read);
                    }

                    fs.Close();
                    fs.Dispose();
                }
                else  // Cannot be sure of unique value, raise exception to cause
                {     //   fall back to full file checksum

                    pChecksum.Reset();
                    throw new IOException(EXCEPTION_PDX_MISSING);
                }
            }
        }

        public byte[] GetAsciiSignature()
        {
            return null;
        }

        public string GetFileExtensions()
        {
            return MDX_FILE_EXTENSION;
        }
        
        public string GetFormatAbbreviation()
        {
            return FORMAT_ABBREVIATION;
        }

        public bool IsFileLibrary() { return false; }

        public bool HasMultipleFileExtensions()
        {
            return false;
        }

        public Dictionary<string, string> GetTagHash()
        {
            return this.tagHash;
        }

        public bool UsesLibraries() { return true; }
        public bool IsLibraryPresent() 
        {
            bool ret = false;

            if (!String.IsNullOrEmpty(this.pdxFileName))
            {
                string searchFile =
                    Path.Combine(Path.GetDirectoryName(Path.GetFullPath(this.filePath)), this.pdxFileName);

                if (File.Exists(searchFile))
                {
                    ret = true;
                }
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        #endregion

        #region ISingleTagFormat Functions

        public string GetTagAsText() { return title; }        
        public void UpdateTag(string pNewValue)
        {
            string tempFilePath = Path.GetTempFileName();

            using (BinaryWriter bw =
                new BinaryWriter(File.Open(tempFilePath, FileMode.Open, FileAccess.ReadWrite)))
            {
                byte[] newTagBytes = 
                    System.Text.Encoding.GetEncoding(VGMToolbox.util.ByteConversion.CodePageJapan).GetBytes(pNewValue);

                bw.Write(newTagBytes);
                bw.Write(TITLE_TERMINATOR);
                if (pdxBytes != null)
                {
                    bw.Write(pdxBytes);
                }
                bw.Write(PDX_TERMINATOR);
                bw.Write(dataBytes);                
            }

            File.Delete(this.filePath);
            File.Move(tempFilePath, this.filePath);
        }

        #endregion
    }    
}
