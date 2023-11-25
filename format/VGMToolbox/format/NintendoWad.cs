using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using VGMToolbox.format.iso;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class NintendoWad
    {
        public static readonly byte[] STANDARD_IDENTIFIER = new byte[] { 0x20, 0x49, 0x73, 0x00, 0x00 };
        public const uint IDENTIFIER_OFFSET = 0x03;
        public const string FORMAT_DESCRIPTION_STRING = "Nintendo WAD";
        public const string EXTRACTION_FOLDER = "VGMT_WAD_EXTRACT";
        public const string EXTRACTION_FOLDER_NONCONTENT = "_non-content";

        public struct TmdContentStruct
        {
            public uint ContentId { set; get; }
            public ushort ContentIndex { set; get; }
            public ushort ContentType { set; get; }
            public ulong ContentSize { set; get; }
            public byte[] Sha1Hash { set; get; }

            public ulong ContentOffset { set; get; }
        }

        public string SourceFileName { set; get; }

        public uint HeaderSize { set; get; }
        public uint WadType { set; get; }
        public uint CertificateChainSize { set; get; }
        public uint Reserved { set; get; }
        public uint TicketSize { set; get; }
        public uint TitleMetaDataSize { set; get; }
        public uint DataSize { set; get; }
        public uint FooterSize { set; get; }
        
        // offsets
        public uint CertificateChainOffset { set; get; }
        public uint TicketOffset { set; get; }
        public uint TitleMetaDataOffset { set; get; }
        public uint DataOffset { set; get; }
        public uint FooterOffset { set; get; }

        // other
        public byte[] EncryptedTitleKey { set; get; }
        public byte[] DecryptedTitleKey { set; get; }
        public byte[] CommonKey { set; get; }
        public byte[] KoreanCommonKey { set; get; }
        public ulong TicketId { set; get; }
        public ulong TitleId { set; get; }
        public byte[] TitleIdBytes { set; get; }
        public byte[] IV { set; get; }
        public byte CommonKeyIndex { set; get; }

        public ushort NumberOfContents { set; get; }
        TmdContentStruct[] TmdContentEntries { set; get; }


        public NintendoWad(string sourceFile)
        { 
            // check magic bytes
            if (NintendoWad.IsWadFile(sourceFile))
            {
                // read header
                using (FileStream fs = File.OpenRead(sourceFile))
                {
                    // set source file
                    this.SourceFileName = sourceFile; 

                    // parse header
                    this.HeaderSize = ParseFile.ReadUintBE(fs, 0);
                    this.WadType = ParseFile.ReadUintBE(fs, 4);
                    this.CertificateChainSize = ParseFile.ReadUintBE(fs, 8);
                    this.Reserved = ParseFile.ReadUintBE(fs, 0xC);
                    this.TicketSize = ParseFile.ReadUintBE(fs, 0x10);
                    this.TitleMetaDataSize = ParseFile.ReadUintBE(fs, 0x14);
                    this.DataSize = ParseFile.ReadUintBE(fs, 0x18);
                    this.FooterSize = ParseFile.ReadUintBE(fs, 0x1C);
                    
                    // offsets
                    this.CertificateChainOffset = (uint)this.PadValue(this.HeaderSize, "Certificate Chain Offset");
                    this.TicketOffset = (uint)this.PadValue(this.CertificateChainOffset + this.CertificateChainSize, "Ticket Offset");
                    this.TitleMetaDataOffset = (uint)this.PadValue(this.TicketOffset + this.TicketSize, "Title Meta Data Offset");
                    this.DataOffset = (uint)this.PadValue(this.TitleMetaDataOffset + this.TitleMetaDataSize, "Data Offset");
                    this.FooterOffset = (uint)this.PadValue(this.DataOffset + this.DataSize, "Footer Offset");

                    // get important values
                    this.TicketId = ParseFile.ReadUlongBE(fs, this.TicketOffset + 0x1D0);
                    this.TitleId = ParseFile.ReadUlongBE(fs, this.TicketOffset + 0x1DC);
                    this.TitleIdBytes = ParseFile.ParseSimpleOffset(fs, this.TicketOffset + 0x1DC, 8);
                    this.EncryptedTitleKey = ParseFile.ParseSimpleOffset(fs, this.TicketOffset + 0x1BF, 0x10);
                    this.CommonKeyIndex = ParseFile.ReadByte(fs, this.TicketOffset + 0x1F1);

                    // decrypt title key
                    this.DecryptTitleKey();

                    // get TMD content entries
                    this.NumberOfContents = ParseFile.ReadUshortBE(fs, this.TitleMetaDataOffset + 0x1DE);
                    this.ParseTmdContentEntries(fs);

                } // using (FileStream fs = File.OpenRead(sourceFile))
            }
            else
            {
                throw new FormatException("Nintendo WAD magic bytes not found at offset 0x03.");
            }
        }

        /// <summary>
        /// Extracts all data from the WAD object.
        /// </summary>
        /// <returns>Array containing the paths of all extracted files.</returns>
        public string[] ExtractAll()
        {
            ArrayList extractedFiles = new ArrayList();
            string footerFile;

            extractedFiles.Add(this.ExtractCertificateChain());
            extractedFiles.Add(this.ExtractTicket());
            extractedFiles.Add(this.ExtractTitleMetaData());
            
            //------------------
            // optional footer
            //------------------
            footerFile = this.ExtractFooter();

            if (!String.IsNullOrEmpty(footerFile))
            {
                extractedFiles.Add(footerFile);
            }

            //----------
            // content
            //----------
            for (int i = 0; i < this.TmdContentEntries.Length; i++)
            { 
                this.ExtractContentById((uint)i);
            }
                       
            return (string[])extractedFiles.ToArray(typeof(string));
        }

        /// <summary>
        /// Extracts all content from the WAD object.
        /// </summary>
        /// <returns>Array containing the paths of all extracted files.</returns>
        public string[] ExtractContent()
        {
            ArrayList extractedFiles = new ArrayList();

            for (int i = 0; i < this.TmdContentEntries.Length; i++)
            {
                extractedFiles.Add(this.ExtractContentById((uint)i));
            }

            return (string[])extractedFiles.ToArray(typeof(string));
        }

        /// <summary>
        /// Extract the certificate chain.
        /// </summary>
        /// <returns>File name of extracted file.</returns>
        public string ExtractCertificateChain()
        {
            string destinationFile = Path.Combine(
                                        this.GetUnpackFolder(false),
                                        (this.TitleId.ToString("X8") + ".cert"));
            

            using (FileStream fs = File.OpenRead(this.SourceFileName))
            {
                ParseFile.ExtractChunkToFile(fs, this.CertificateChainOffset, this.CertificateChainSize, destinationFile);
            }

            return destinationFile;
        }

        /// <summary>
        /// Extract the ticket.
        /// </summary>
        /// <returns>File name of extracted file.</returns>
        public string ExtractTicket()
        {
            string destinationFile = Path.Combine(
                                        this.GetUnpackFolder(false),
                                        (this.TitleId.ToString("X8") + ".tik"));


            using (FileStream fs = File.OpenRead(this.SourceFileName))
            {
                ParseFile.ExtractChunkToFile(fs, this.TicketOffset, this.TicketSize, destinationFile);
            }

            return destinationFile;
        }

        /// <summary>
        /// Extract the title meta data.
        /// </summary>
        /// <returns>File name of extracted file.</returns>
        public string ExtractTitleMetaData()
        {
            string destinationFile = Path.Combine(
                                        this.GetUnpackFolder(false),
                                        (this.TitleId.ToString("X8") + ".tmd"));


            using (FileStream fs = File.OpenRead(this.SourceFileName))
            {
                ParseFile.ExtractChunkToFile(fs, this.TitleMetaDataOffset, this.TitleMetaDataSize, destinationFile);
            }

            return destinationFile;
        }

        /// <summary>
        /// Extract optional footer.
        /// </summary>
        /// <returns>File name of extracted file or String.Empty if footer is not present.</returns>
        public string ExtractFooter()
        {
            string destinationFile = String.Empty;

            if (this.FooterSize > 0)
            {
                destinationFile = Path.Combine(
                                    this.GetUnpackFolder(false), 
                                    (this.TitleId.ToString("X8") + ".trailer"));

                using (FileStream fs = File.OpenRead(this.SourceFileName))
                {
                    ParseFile.ExtractChunkToFile(fs, this.FooterOffset, this.FooterSize, destinationFile);
                }
            }
            
            return destinationFile;
        }

        /// <summary>
        /// Build the decrypted title key based on title id bytes.
        /// </summary>
        private void DecryptTitleKey()
        {
            this.IV = new byte[0x10];
            Array.Copy(this.TitleIdBytes, 0, this.IV, 0, 8);
            
            switch (this.CommonKeyIndex)
            {
                case 1:
                    if (this.KoreanCommonKey == null)
                    {
                        this.KoreanCommonKey = NintendoWiiOpticalDisc.GetKeyFromFile(NintendoWiiOpticalDisc.KOREAN_KEY_PATH);
                    }

                    this.DecryptedTitleKey =  AESEngine.Decrypt(this.EncryptedTitleKey,
                                                this.KoreanCommonKey, this.IV,
                                                CipherMode.CBC, PaddingMode.Zeros);

                    break;
                
                case 0:
                default:
                    if (this.CommonKey == null)
                    {
                        this.CommonKey = NintendoWiiOpticalDisc.GetKeyFromFile(NintendoWiiOpticalDisc.COMMON_KEY_PATH);
                    }

                    this.DecryptedTitleKey = AESEngine.Decrypt(this.EncryptedTitleKey,
                                                this.CommonKey, this.IV,
                                                CipherMode.CBC, PaddingMode.Zeros);

                    break;            
            } // switch (this.CommonKeyIndex)

        }

        /// <summary>
        /// Parse the Content Entries in the Title Meta Data section.
        /// </summary>
        /// <param name="fs">File Stream of WAD.</param>
        private void ParseTmdContentEntries(FileStream fs)
        {
            ulong nextOffset = this.DataOffset;
            TmdContentStruct contentEntry;
            ArrayList contentEntryList = new ArrayList();

            for (ushort i = 0; i < this.NumberOfContents; i++)
            {
                contentEntry = new TmdContentStruct();
                contentEntry.ContentId = ParseFile.ReadUintBE(fs, this.TitleMetaDataOffset + 0x1E4 + (i * 0x24) + 0);
                contentEntry.ContentIndex = ParseFile.ReadUshortBE(fs, this.TitleMetaDataOffset + 0x1E4 + (i * 0x24) + 4);
                contentEntry.ContentType = ParseFile.ReadUshortBE(fs, this.TitleMetaDataOffset + 0x1E4 + (i * 0x24) + 6);
                contentEntry.ContentSize = ParseFile.ReadUlongBE(fs, this.TitleMetaDataOffset + 0x1E4 + (i * 0x24) + 8);
                contentEntry.Sha1Hash = ParseFile.ParseSimpleOffset(fs, (this.TitleMetaDataOffset + 0x1E4 + (i * 0x24) + 0x10), 20);
                contentEntry.ContentOffset = nextOffset;
                contentEntryList.Add(contentEntry);

                nextOffset = this.PadValue((contentEntry.ContentOffset + contentEntry.ContentSize), 
                    String.Format("Next Offset for ContentId: 0x{0}", contentEntry.ContentId.ToString("X8")));
            }

            this.TmdContentEntries = (TmdContentStruct[])contentEntryList.ToArray(typeof(TmdContentStruct));
        }

        public string ExtractContentById(uint contentId)
        {

            byte[] buffer = new byte[Constants.FileReadChunkSize];
            int bytesRead = 0;
            ulong bytesReadFromFile = 0;

            byte[] IV = new byte[0x10];
            IV[0] = (byte)(TmdContentEntries[contentId].ContentIndex & 0xFF00);
            IV[1] = (byte)(TmdContentEntries[contentId].ContentIndex & 0x00FF);

            string destinationFile = Path.Combine(this.GetUnpackFolder(true), String.Format("{0}.app", contentId.ToString("X8")));

            using (FileStream fs = File.OpenRead(this.SourceFileName))
            {
                if (!Directory.Exists(this.GetUnpackFolder(true)))
                {
                    Directory.CreateDirectory(this.GetUnpackFolder(true));
                }
                
                using (FileStream outFs = File.Open(destinationFile, FileMode.Create, FileAccess.ReadWrite))
                {
                    // setup the decryption algorithm
                    Rijndael alg = Rijndael.Create();
                    alg.KeySize = 128;
                    alg.Mode = CipherMode.CBC;
                    alg.Padding = PaddingMode.Zeros;
                    alg.Key = this.DecryptedTitleKey;
                    alg.IV = IV;

                    // setup our decryption stream
                    CryptoStream cs = new CryptoStream(outFs, alg.CreateDecryptor(), CryptoStreamMode.Write);

                    // read from file
                    fs.Position = (long)TmdContentEntries[contentId].ContentOffset;
                    
                    while (bytesReadFromFile < TmdContentEntries[contentId].ContentSize)
                    {                        
                        bytesRead = fs.Read(buffer, 0, buffer.Length);

                        if (bytesRead > 0)
                        {
                            // Write the data and make it do the decryption 
                            cs.Write(buffer, 0, bytesRead);

                            // increment bytes read from file
                            bytesReadFromFile += (ulong)bytesRead;
                        }
                    }
                    
                    // close crypto stream                                        
                    cs.Close();
                    cs.Dispose();                    
                }

                FileUtil.TrimFileToLength(destinationFile, (long)TmdContentEntries[contentId].ContentSize);

                return destinationFile;
            }
        }

        /// <summary>
        /// Pads input value to next 0x40 byte alignment.
        /// </summary>
        /// <param name="value">Value to pad.</param>
        /// <param name="textDescription">Text description of value used for error output.</param>
        /// <returns>Value padded to next 0x40 byte alignment.</returns>
        private ulong PadValue(ulong value, string textDescription)
        {
            ulong paddedValue;
            Int64 byteAlignedValue;

            byteAlignedValue = MathUtil.RoundUpToByteAlignment((long)value, 0x40);

            if (byteAlignedValue == -1)
            {
                throw new Exception(String.Format("Error padding value for \"{0}.\"", textDescription));
            }
            else
            {
                paddedValue = (ulong)byteAlignedValue;
            }
            
            return paddedValue;
        }

        /// <summary>
        /// Builds the output folder path for unpacking.
        /// </summary>
        /// <returns>Full path of folder for unpacking.</returns>
        private string GetUnpackFolder(bool isContent)
        {
            string unpackFolder;

            unpackFolder = Path.Combine(
                Path.GetDirectoryName(this.SourceFileName),
                String.Format("{0}_{1}", NintendoWad.EXTRACTION_FOLDER, Path.GetFileNameWithoutExtension(this.SourceFileName)));

            if (!isContent)
            {
                unpackFolder = Path.Combine(unpackFolder, NintendoWad.EXTRACTION_FOLDER_NONCONTENT);            
            }
            
            return unpackFolder;
        }

        /// <summary>
        /// Checks for WAD file Magic Bytes.
        /// </summary>
        /// <param name="sourceFile">Full path to file to check.</param>
        /// <returns>Boolean value indicating if input file has WAD magic bytes.</returns>
        public static bool IsWadFile(string sourceFile)
        {
            bool isWad = false;
            byte[] magicBytes = new byte[NintendoWad.STANDARD_IDENTIFIER.Length];

            using (FileStream fs = File.OpenRead(sourceFile))
            {
                magicBytes = ParseFile.ParseSimpleOffset(fs, NintendoWad.IDENTIFIER_OFFSET, NintendoWad.STANDARD_IDENTIFIER.Length);

                if (ParseFile.CompareSegment(magicBytes, 0, NintendoWad.STANDARD_IDENTIFIER))
                {
                    isWad = true;
                }
            }

            return isWad;
        }

    }
}
