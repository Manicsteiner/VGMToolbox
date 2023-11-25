using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Kss : IFormat, IHootFormat
    {
        private static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x4B, 0x53, 0x43, 0x43 }; // KSCC
        private const string FORMAT_ABBREVIATION = "KSS";

        private const string HOOT_DRIVER_ALIAS_MSX = "MSX";
        private const string HOOT_DRIVER_MSX = "msx";

        private const string HOOT_DRIVER_ALIAS_SMS = "Sega Master Systems";
        private const string HOOT_DRIVER_SMS = "sms";

        private const string HOOT_DRIVER_ALIAS_GG = "Game Gear";
        private const string HOOT_DRIVER_GG = "gg";

        private const string HOOT_DRIVER_TYPE = "kss";        
        private const string HOOT_CHIP = "Z80";

        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x04;

        private const string CHIP_FMPAC = "FM-PAC";
        private const string CHIP_FMUNIT = "FM-UNIT";
        private const string CHIP_SN76489 = "SN76489";
        private const string CHIP_RAM = "RAM";
        private const string CHIP_GG = "GG Stereo";
        private const string CHIP_MSXAUDIO = "MSX-AUDIO";

        private const int Z80_LOAD_OFFSET = 0x04;
        private const int Z80_LOAD_LENGTH = 0x02;

        private const int INIT_DATA_LENGTH_OFFSET = 0x06;
        private const int INIT_DATA_LENGTH_LENGTH = 0x02;

        private const int Z80_INIT_OFFSET = 0x08;
        private const int Z80_INIT_LENGTH = 0x02;

        private const int Z80_PLAY_OFFSET = 0x0A;
        private const int Z80_PLAY_LENGTH = 0x02;

        private const int BANK_START_NO_OFFSET = 0x0C;
        private const int BANK_START_NO_LENGTH = 0x01;

        private const int BANKED_EXTRA_DATA_OFFSET = 0x0D;
        private const int BANKED_EXTRA_DATA_LENGTH = 0x01;

        private const int RESERVED_OFFSET = 0x0E;
        private const int RESERVED_LENGTH = 0x01;

        private const int EXTRA_CHIP_OFFSET = 0x0F;
        private const int EXTRA_CHIP_LENGTH = 0x01;

        private const int DATA_OFFSET = 0x10;

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private byte[] asciiSignature;
        private byte[] z80Load;
        private byte[] initDataLength;
        private byte[] z80Init;
        private byte[] z80Play;
        private byte[] bankStartNo;
        private byte[] bankedExtraData;
        private byte[] reserved;
        private byte[] extraChips;
        private byte[] data;

        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        public byte[] AsciiSignature { get { return this.asciiSignature; } }
        public byte[] Z80Load { get { return this.z80Load; } }
        public byte[] InitDataLength { get { return this.initDataLength; } }
        public byte[] Z80Init { get { return this.z80Init; } }
        public byte[] Z80Play { get { return this.z80Play; } }
        public byte[] BankStartNo { get { return this.bankStartNo; } }
        public byte[] BankedExtraData { get { return this.bankedExtraData; } }
        public byte[] Reserved { get { return this.reserved; } }
        public byte[] ExtraChips { get { return this.extraChips; } }
        public byte[] Data { get { return this.data; } }

        #region METHODS

        public byte[] getAsciiSignature(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
        }

        public byte[] getZ80Load(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, Z80_LOAD_OFFSET, Z80_LOAD_LENGTH);
        }

        public byte[] getInitDataLength(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, INIT_DATA_LENGTH_OFFSET, INIT_DATA_LENGTH_LENGTH);
        }

        public byte[] getZ80Init(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, Z80_INIT_OFFSET, Z80_INIT_LENGTH);
        }

        public byte[] getZ80Play(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, Z80_PLAY_OFFSET, Z80_PLAY_LENGTH);
        }

        public byte[] getBankStartNo(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, BANK_START_NO_OFFSET, BANK_START_NO_LENGTH);
        }

        public byte[] getBankedExtraData(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, BANKED_EXTRA_DATA_OFFSET, BANKED_EXTRA_DATA_LENGTH);
        }

        public byte[] getReserved(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, RESERVED_OFFSET, RESERVED_LENGTH);
        }

        public byte[] getExtraChips(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, EXTRA_CHIP_OFFSET, EXTRA_CHIP_LENGTH);
        }

        public byte[] getData(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, DATA_OFFSET, (int)(pStream.Length - DATA_OFFSET));
        }

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath = pFilePath;
            this.asciiSignature = this.getAsciiSignature(pStream);
            this.z80Load = this.getZ80Load(pStream);
            this.initDataLength = this.getInitDataLength(pStream);
            this.z80Init = this.getZ80Init(pStream);
            this.z80Play = this.getZ80Play(pStream);
            this.bankStartNo = this.getBankStartNo(pStream);
            this.bankedExtraData = this.getBankedExtraData(pStream);
            this.reserved = this.getReserved(pStream);
            this.extraChips = this.getExtraChips(pStream);
            this.data = this.getData(pStream);

            this.initializeTagHash();
        }

        private void initializeTagHash()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            tagHash.Add("Extra Chips Bytes", this.GetExtraChipsString());
        }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();

            pChecksum.Update(z80Load);
            pChecksum.Update(initDataLength);
            pChecksum.Update(z80Init);
            pChecksum.Update(z80Play);
            pChecksum.Update(bankStartNo);
            pChecksum.Update(bankedExtraData);
            pChecksum.Update(extraChips);
            pChecksum.Update(data);
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            pChecksum.Update(z80Load);
            pChecksum.Update(initDataLength);
            pChecksum.Update(z80Init);
            pChecksum.Update(z80Play);
            pChecksum.Update(bankStartNo);
            pChecksum.Update(bankedExtraData);
            pChecksum.Update(extraChips);
            pChecksum.Update(data);

            pMd5CryptoStream.Write(z80Load, 0, z80Load.Length);
            pMd5CryptoStream.Write(initDataLength, 0, initDataLength.Length);
            pMd5CryptoStream.Write(z80Init, 0, z80Init.Length);
            pMd5CryptoStream.Write(z80Play, 0, z80Play.Length);
            pMd5CryptoStream.Write(bankStartNo, 0, bankStartNo.Length);
            pMd5CryptoStream.Write(bankedExtraData, 0, bankedExtraData.Length);
            pMd5CryptoStream.Write(extraChips, 0, extraChips.Length);
            pMd5CryptoStream.Write(data, 0, data.Length);

            pSha1CryptoStream.Write(z80Load, 0, z80Load.Length);
            pSha1CryptoStream.Write(initDataLength, 0, initDataLength.Length);
            pSha1CryptoStream.Write(z80Init, 0, z80Init.Length);
            pSha1CryptoStream.Write(z80Play, 0, z80Play.Length);
            pSha1CryptoStream.Write(bankStartNo, 0, bankStartNo.Length);
            pSha1CryptoStream.Write(bankedExtraData, 0, bankedExtraData.Length);
            pSha1CryptoStream.Write(extraChips, 0, extraChips.Length);
            pSha1CryptoStream.Write(data, 0, data.Length);
        }

        public string GetExtraChipsString()
        {
            StringBuilder sb = new StringBuilder();
            
            bool bit0set = ((this.extraChips[0] & 1 )== 1);
            bool bit1set = ((this.extraChips[0] & 2) == 2);
            bool bit2set = ((this.extraChips[0] & 4) == 4);
            bool bit3set = ((this.extraChips[0] & 8) == 8);

            if (bit0set)
            {
                if (bit1set)
                {
                    sb.AppendFormat("[{0}]", CHIP_FMUNIT);
                }
                else
                {
                    sb.AppendFormat("[{0}]", CHIP_FMPAC);
                }
            }
            else if (bit2set)
            {
                if (bit1set)
                {
                    sb.AppendFormat("[{0}]", CHIP_GG);
                }
                else
                {
                    sb.AppendFormat("[{0}]", CHIP_RAM);
                }            
            }
            else if (bit3set)
            {
                if (bit1set)
                {
                    sb.AppendFormat("[{0}]", CHIP_RAM);
                }
                else
                {
                    sb.AppendFormat("[{0}]", CHIP_MSXAUDIO);
                }            
            }
            else if (bit1set)
            {
                sb.AppendFormat("[{0}]", CHIP_SN76489);
            }

            return sb.ToString();
        }

        public byte[] GetAsciiSignature()
        {
            return ASCII_SIGNATURE;
        }

        public string GetFileExtensions()
        {
            return null;
        }

        public string GetFormatAbbreviation()
        {
            return FORMAT_ABBREVIATION;
        }

        public bool IsFileLibrary() { return false;}

        public bool HasMultipleFileExtensions()
        {
            return false;
        }

        public Dictionary<string, string> GetTagHash()
        {
            return this.tagHash;
        }

        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }
        
        #endregion

        #region HOOT

        public int GetStartingSong() { return 0; }
        public int GetTotalSongs()
        {
            NezPlugM3uEntry[] entries = this.GetPlaylistEntries();
            return entries.Length;
        }
        public string GetSongName() { return null; }

        public string GetHootDriverAlias() 
        {
            /*
            string ret = null;

            bool bit0set = ((this.extraChips[0] & 1) == 1);
            bool bit1set = ((this.extraChips[0] & 2) == 2);
            bool bit2set = ((this.extraChips[0] & 4) == 4);
            bool bit3set = ((this.extraChips[0] & 8) == 8);

            if (bit0set)
            {
                ret = HOOT_DRIVER_ALIAS_MSX;
            }
            else if (bit2set)
            {
                if (bit1set)
                {
                    ret = HOOT_DRIVER_ALIAS_GG;
                }
                else
                {
                    ret = HOOT_DRIVER_ALIAS_MSX;
                }
            }
            else if (bit3set)
            {
                ret = HOOT_DRIVER_ALIAS_MSX;
            }
            else if (bit1set)
            {
                ret = HOOT_DRIVER_ALIAS_SMS;
            }

            return ret;
            */

            return HOOT_DRIVER_ALIAS_MSX;
        }        
        public string GetHootDriverType() { return HOOT_DRIVER_TYPE; }
        public string GetHootDriver() 
        {
            /*
            string ret = null;

            bool bit0set = ((this.extraChips[0] & 1) == 1);
            bool bit1set = ((this.extraChips[0] & 2) == 2);
            bool bit2set = ((this.extraChips[0] & 4) == 4);
            bool bit3set = ((this.extraChips[0] & 8) == 8);

            if (bit0set)
            {
                ret = HOOT_DRIVER_MSX;
            }
            else if (bit2set)
            {
                if (bit1set)
                {
                    ret = HOOT_DRIVER_GG;
                }
                else
                {
                    ret = HOOT_DRIVER_MSX;
                }
            }
            else if (bit3set)
            {
                ret = HOOT_DRIVER_MSX;
            }
            else if (bit1set)
            {
                ret = HOOT_DRIVER_SMS;
            }

            return ret;
            */

            return HOOT_DRIVER_MSX;
        }
        public string GetHootChips() 
        {
            StringBuilder sb = new StringBuilder(HOOT_CHIP);

            bool bit0set = ((this.extraChips[0] & 1) == 1);
            bool bit1set = ((this.extraChips[0] & 2) == 2);
            bool bit2set = ((this.extraChips[0] & 4) == 4);
            bool bit3set = ((this.extraChips[0] & 8) == 8);

            if (bit0set)
            {
                if (bit1set)
                {
                    sb.AppendFormat(", {0}", CHIP_FMUNIT);
                }
                else
                {
                    sb.AppendFormat(", {0}", CHIP_FMPAC);
                }
            }
            else if (bit2set)
            {
                if (bit1set)
                {
                    sb.AppendFormat(", {0}", CHIP_GG);
                }
                else
                {
                    sb.AppendFormat(", {0}", CHIP_RAM);
                }
            }
            else if (bit3set)
            {
                if (bit1set)
                {
                    sb.AppendFormat(", {0}", CHIP_RAM);
                }
                else
                {
                    sb.AppendFormat(", {0}", CHIP_MSXAUDIO);
                }
            }
            else if (bit1set)
            {
                sb.AppendFormat(", {0}", CHIP_SN76489);
            }

            return sb.ToString();                        
        }

        public bool UsesPlaylist()
        {
            return true;
        }
        public NezPlugM3uEntry[] GetPlaylistEntries()
        {
            NezPlugM3uEntry[] entries = null;

            string m3uFileName = Path.ChangeExtension(this.filePath, NezPlugUtil.M3U_FILE_EXTENSION);
            entries = NezPlugUtil.GetNezPlugM3uEntriesFromFile(m3uFileName);

            return entries;
        }

        public bool HasMultipleFilesPerSet() { return false; }
        public string GetSetName() { return null; }

        #endregion
    }
}
