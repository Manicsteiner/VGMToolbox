using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Nsf : IFormat, IHootFormat, IEmbeddedTagsFormat, INezPlugPlaylistFormat
    {
        public static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x4E, 0x45, 0x53, 0x4D, 0x1A };
        public static readonly byte[] CURRENT_VERSION_NUMBER = new byte[] { 0x01};
        private const string FORMAT_ABBREVIATION = "NSF";
        private const string HOOT_CHIP = "2A03";

        public static readonly byte MASK_NTSC = 0;
        public static readonly byte MASK_PAL = 1;        
        public static readonly byte MASK_PAL_NTSC = 2;
        
        private static readonly byte MASK_VRC6  = 1;
        private static readonly byte MASK_VRC7  = 2;
        private static readonly byte MASK_FDS   = 4;
        private static readonly byte MASK_MMC5  = 8;
        private static readonly byte MASK_N106  = 16;
        private static readonly byte MASK_FME07 = 32;

        private const string CHIP_VRC6  = "VRC6";
        private const string CHIP_VRC7  = "VRC7";
        private const string CHIP_FDS   = "FDS Sound";
        private const string CHIP_MMC5  = "MMC5";
        private const string CHIP_N106  = "NAMCO106";
        private const string CHIP_FME07 = "Sunsoft FME-07";

        private const string HOOT_DRIVER_ALIAS = "NES";
        private const string HOOT_DRIVER_TYPE = "nsf";
        private const string HOOT_DRIVER = "nes";

        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x05;

        private const int VERSION_OFFSET = 0x05;
        private const int VERSION_LENGTH = 0x01;

        private const int TOTAL_SONGS_OFFSET = 0x06;
        private const int TOTAL_SONGS_LENGTH = 0x01;

        private const int STARTING_SONG_OFFSET = 0x07;
        private const int STARTING_SONG_LENGTH = 0x01;

        private const int LOAD_ADDRESS_OFFSET = 0x08;
        private const int LOAD_ADDRESS_LENGTH = 0x02;

        private const int INIT_ADDRESS_OFFSET = 0x0A;
        private const int INIT_ADDRESS_LENGTH = 0x02;

        private const int PLAY_ADDRESS_OFFSET = 0x0C;
        private const int PLAY_ADDRESS_LENGTH = 0x02;

        private const int NAME_OFFSET = 0x0E;
        private const int NAME_LENGTH = 0x20;

        private const int ARTIST_OFFSET = 0x2E;
        private const int ARTIST_LENGTH = 0x20;

        private const int COPYRIGHT_OFFSET = 0x4E;
        private const int COPYRIGHT_LENGTH = 0x20;

        private const int SPEED_NTSC_OFFSET = 0x6E;
        private const int SPEED_NTSC_LENGTH = 0x02;

        private const int BANKSWITCH_INIT_OFFSET = 0x70;
        private const int BANKSWITCH_INIT_LENGTH = 0x08;

        private const int SPEED_PAL_OFFSET = 0x78;
        private const int SPEED_PAL_LENGTH = 0x02;

        private const int PAL_NTSC_BITS_OFFSET = 0x7A;
        private const int PAL_NTSC_BITS_LENGTH = 0x01;

        private const int EXTRA_SOUND_BITS_OFFSET = 0x7B;
        private const int EXTRA_SOUND_BITS_LENGTH = 0x01;

        private const int FUTURE_USE_OFFSET = 0x7C;
        private const int FUTURE_USE_LENGTH = 0x04;

        private const int DATA_OFFSET = 0x80;

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private byte[] asciiSignature;
        private byte[] versionNumber;
        private byte[] totalSongs;
        private byte[] startingSong;
        private byte[] loadAddress;
        private byte[] initAddress;
        private byte[] playAddress;
        private byte[] songName;
        private byte[] songArtist;
        private byte[] songCopyright;
        private byte[] ntscSpeed;
        private byte[] bankSwitchInit;
        private byte[] palSpeed;
        private byte[] palNtscBits;
        private byte[] extraChipsBits;
        private byte[] futureExpansion;
        private byte[] data;

        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        public byte[] AsciiSignature { get { return this.asciiSignature; } }
        public byte[] VersionNumber { get { return this.versionNumber; } }
        public byte[] TotalSongs { get { return this.totalSongs; } }
        public byte[] StartingSong { get { return this.startingSong; } }
        public byte[] LoadAddress { get { return this.loadAddress; } }
        public byte[] InitAddress { get { return this.initAddress; } }
        public byte[] PlayAddress { get { return this.playAddress; } }
        public byte[] SongName { get { return this.songName; } }
        public byte[] SongArtist { get { return this.songArtist; } }
        public byte[] SongCopyright { get { return this.songCopyright; } }
        public byte[] NtscSpeed { get { return this.ntscSpeed; } }
        public byte[] BankSwitchInit { get { return this.bankSwitchInit; } }
        public byte[] PalSpeed { get { return this.palSpeed; } }
        public byte[] PalNtscBits { get { return this.palNtscBits; } }
        public byte[] ExtraChipsBits { get { return this.extraChipsBits; } }
        public byte[] FutureExpansion { get { return this.futureExpansion; } }
        public byte[] Data { get { return this.data; } }


        #region STREAM BASED METHODS
        public byte[] getAsciiSignature(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
        }

        public byte[] getVersionNumber(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, VERSION_OFFSET, VERSION_LENGTH);
        }

        public byte[] getTotalSongs(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, TOTAL_SONGS_OFFSET, TOTAL_SONGS_LENGTH);
        }

        public byte[] getStartingSong(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, STARTING_SONG_OFFSET, STARTING_SONG_LENGTH);
        }

        public byte[] getLoadAddress(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, LOAD_ADDRESS_OFFSET, LOAD_ADDRESS_LENGTH);
        }

        public byte[] getInitAddress(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, INIT_ADDRESS_OFFSET, INIT_ADDRESS_LENGTH);
        }

        public byte[] getPlayAddress(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, PLAY_ADDRESS_OFFSET, PLAY_ADDRESS_LENGTH);
        }

        public byte[] getSongName(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, NAME_OFFSET, NAME_LENGTH);
        }

        public byte[] getSongArtist(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, ARTIST_OFFSET, ARTIST_LENGTH);
        }

        public byte[] getSongCopyright(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, COPYRIGHT_OFFSET, COPYRIGHT_LENGTH);
        }

        public byte[] getNtscSpeed(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SPEED_NTSC_OFFSET, SPEED_NTSC_LENGTH);
        }

        public byte[] getBankSwitchInit(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, BANKSWITCH_INIT_OFFSET, BANKSWITCH_INIT_LENGTH);
        }

        public byte[] getPalSpeed(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SPEED_PAL_OFFSET, SPEED_PAL_LENGTH);
        }

        public byte[] getPalNtscBits(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, PAL_NTSC_BITS_OFFSET, PAL_NTSC_BITS_LENGTH);
        }

        public byte[] getExtraChipsBits(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, EXTRA_SOUND_BITS_OFFSET, EXTRA_SOUND_BITS_LENGTH);
        }

        public byte[] getFutureExpansion(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, FUTURE_USE_OFFSET, FUTURE_USE_LENGTH);
        }

        public byte[] getData(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, DATA_OFFSET, (int) (pStream.Length - DATA_OFFSET));
        }

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath = pFilePath;
            this.asciiSignature = this.getAsciiSignature(pStream);
            this.versionNumber = this.getVersionNumber(pStream);
            this.totalSongs = this.getTotalSongs(pStream);
            this.startingSong = this.getStartingSong(pStream);
            this.loadAddress = this.getLoadAddress(pStream);
            this.initAddress = this.getInitAddress(pStream);
            this.playAddress = this.getPlayAddress(pStream);
            this.songName = this.getSongName(pStream);
            this.songArtist = this.getSongArtist(pStream);
            this.songCopyright = this.getSongCopyright(pStream);
            this.ntscSpeed = this.getNtscSpeed(pStream);
            this.bankSwitchInit = this.getBankSwitchInit(pStream);
            this.palSpeed = this.getPalSpeed(pStream);
            this.palNtscBits = this.getPalNtscBits(pStream);
            this.extraChipsBits = this.getExtraChipsBits(pStream);
            this.futureExpansion = this.getFutureExpansion(pStream);
            this.data = this.getData(pStream);

            this.initializeTagHash();
        }
        #endregion

        #region BYTE[] BASED METHODS
        public byte[] getAsciiSignature(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SIG_OFFSET, SIG_LENGTH);
        }

        public byte[] getVersionNumber(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, VERSION_OFFSET, VERSION_LENGTH);
        }

        public byte[] getTotalSongs(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, TOTAL_SONGS_OFFSET, TOTAL_SONGS_LENGTH);
        }

        public byte[] getStartingSong(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, STARTING_SONG_OFFSET, STARTING_SONG_LENGTH);
        }

        public byte[] getLoadAddress(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, LOAD_ADDRESS_OFFSET, LOAD_ADDRESS_LENGTH);
        }

        public byte[] getInitAddress(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, INIT_ADDRESS_OFFSET, INIT_ADDRESS_LENGTH);
        }

        public byte[] getPlayAddress(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, PLAY_ADDRESS_OFFSET, PLAY_ADDRESS_LENGTH);
        }

        public byte[] getSongName(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, NAME_OFFSET, NAME_LENGTH);
        }

        public byte[] getSongArtist(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ARTIST_OFFSET, ARTIST_LENGTH);
        }

        public byte[] getSongCopyright(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, COPYRIGHT_OFFSET, COPYRIGHT_LENGTH);
        }

        public byte[] getNtscSpeed(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SPEED_NTSC_OFFSET, SPEED_NTSC_LENGTH);
        }

        public byte[] getBankSwitchInit(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, BANKSWITCH_INIT_OFFSET, BANKSWITCH_INIT_LENGTH);
        }

        public byte[] getPalSpeed(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SPEED_PAL_OFFSET, SPEED_PAL_LENGTH);
        }

        public byte[] getPalNtscBits(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, PAL_NTSC_BITS_OFFSET, PAL_NTSC_BITS_LENGTH);
        }

        public byte[] getExtraChipsBits(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, EXTRA_SOUND_BITS_OFFSET, EXTRA_SOUND_BITS_LENGTH);
        }

        public byte[] getFutureExpansion(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, FUTURE_USE_OFFSET, FUTURE_USE_LENGTH);
        }

        public byte[] getData(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, DATA_OFFSET, (int)(pBytes.Length - DATA_OFFSET) + 1);
        }

        public void initialize(byte[] pBytes)
        {
            this.asciiSignature = this.getAsciiSignature(pBytes);
            this.versionNumber = this.getVersionNumber(pBytes);
            this.totalSongs = this.getTotalSongs(pBytes);
            this.startingSong = this.getStartingSong(pBytes);
            this.loadAddress = this.getLoadAddress(pBytes);
            this.initAddress = this.getInitAddress(pBytes);
            this.playAddress = this.getPlayAddress(pBytes);
            this.songName = this.getSongName(pBytes);
            this.songArtist = this.getSongArtist(pBytes);
            this.songCopyright = this.getSongCopyright(pBytes);
            this.ntscSpeed = this.getNtscSpeed(pBytes);
            this.bankSwitchInit = this.getBankSwitchInit(pBytes);
            this.palSpeed = this.getPalSpeed(pBytes);
            this.palNtscBits = this.getPalNtscBits(pBytes);
            this.extraChipsBits = this.getExtraChipsBits(pBytes);
            this.futureExpansion = this.getFutureExpansion(pBytes);
            this.data = this.getData(pBytes);

            this.initializeTagHash();
        }                
        #endregion

        #region METHODS

        private void initializeTagHash()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            
            tagHash.Add("Name", enc.GetString(this.songName));
            tagHash.Add("Artist", enc.GetString(this.songArtist));
            tagHash.Add("Copyright", enc.GetString(this.songCopyright));
            tagHash.Add("Total Songs", this.totalSongs[0].ToString());
            tagHash.Add("Starting Song", this.startingSong[0].ToString());
            tagHash.Add("Extra Chips", getExtraChipsTag());            
        }
        
        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();    
        
            pChecksum.Update(versionNumber);
            pChecksum.Update(totalSongs);
            pChecksum.Update(startingSong);
            pChecksum.Update(loadAddress);
            pChecksum.Update(initAddress);
            pChecksum.Update(playAddress);
            pChecksum.Update(ntscSpeed);
            pChecksum.Update(bankSwitchInit);
            pChecksum.Update(palSpeed);
            pChecksum.Update(palNtscBits);
            pChecksum.Update(extraChipsBits);
            pChecksum.Update(data);
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            pChecksum.Update(versionNumber);
            pChecksum.Update(totalSongs);
            pChecksum.Update(startingSong);
            pChecksum.Update(loadAddress);
            pChecksum.Update(initAddress);
            pChecksum.Update(playAddress);
            pChecksum.Update(ntscSpeed);
            pChecksum.Update(bankSwitchInit);
            pChecksum.Update(palSpeed);
            pChecksum.Update(palNtscBits);
            pChecksum.Update(extraChipsBits);
            pChecksum.Update(data);

            pMd5CryptoStream.Write(versionNumber, 0, versionNumber.Length);
            pMd5CryptoStream.Write(totalSongs, 0, totalSongs.Length);
            pMd5CryptoStream.Write(startingSong, 0, startingSong.Length);
            pMd5CryptoStream.Write(loadAddress, 0, loadAddress.Length);
            pMd5CryptoStream.Write(initAddress, 0, initAddress.Length);
            pMd5CryptoStream.Write(playAddress, 0, playAddress.Length);
            pMd5CryptoStream.Write(ntscSpeed, 0, ntscSpeed.Length);
            pMd5CryptoStream.Write(bankSwitchInit, 0, bankSwitchInit.Length);
            pMd5CryptoStream.Write(palSpeed, 0, palSpeed.Length);
            pMd5CryptoStream.Write(palNtscBits, 0, palNtscBits.Length);
            pMd5CryptoStream.Write(extraChipsBits, 0, extraChipsBits.Length);
            pMd5CryptoStream.Write(data, 0, data.Length);

            pSha1CryptoStream.Write(versionNumber, 0, versionNumber.Length);
            pSha1CryptoStream.Write(totalSongs, 0, totalSongs.Length);
            pSha1CryptoStream.Write(startingSong, 0, startingSong.Length);
            pSha1CryptoStream.Write(loadAddress, 0, loadAddress.Length);
            pSha1CryptoStream.Write(initAddress, 0, initAddress.Length);
            pSha1CryptoStream.Write(playAddress, 0, playAddress.Length);
            pSha1CryptoStream.Write(ntscSpeed, 0, ntscSpeed.Length);
            pSha1CryptoStream.Write(bankSwitchInit, 0, bankSwitchInit.Length);
            pSha1CryptoStream.Write(palSpeed, 0, palSpeed.Length);
            pSha1CryptoStream.Write(palNtscBits, 0, palNtscBits.Length);
            pSha1CryptoStream.Write(extraChipsBits, 0, extraChipsBits.Length);
            pSha1CryptoStream.Write(data, 0, data.Length);
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

        public bool IsFileLibrary() { return false; }

        public bool HasMultipleFileExtensions()
        {
            return false;
        }

        public Dictionary<string, string> GetTagHash()
        {
            return this.tagHash;
        }

        private string getExtraChipsTag()
        { 
            string _ret = String.Empty;
            
            if ((this.extraChipsBits[0] & MASK_VRC6) == MASK_VRC6) 
            {
                _ret += String.Format("[{0}]", CHIP_VRC6);
            }
            if ((this.extraChipsBits[0] & MASK_VRC7) == MASK_VRC7)
            {
                _ret += String.Format("[{0}]", CHIP_VRC7);
            }
            if ((this.extraChipsBits[0] & MASK_FDS) == MASK_FDS)
            {
                _ret += String.Format("[{0}]", CHIP_FDS);
            }
            if ((this.extraChipsBits[0] & MASK_MMC5) == MASK_MMC5)
            {
                _ret += String.Format("[{0}]", CHIP_MMC5);
            }
            if ((this.extraChipsBits[0] & MASK_N106) == MASK_N106)
            {
                _ret += String.Format("[{0}]", CHIP_N106);
            }
            if ((this.extraChipsBits[0] & MASK_FME07) == MASK_FME07)
            {
                _ret += String.Format("[{0}]", CHIP_FME07);
            }
            
            return _ret;
        }

        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }

        public int GetStartingSong() { return Convert.ToInt16(this.startingSong[0]); }
        public int GetTotalSongs() 
        {
            int ret;
            NezPlugM3uEntry[] entries = null;

            try
            {
                entries = this.GetPlaylistEntries();
            }
            catch (IOException) 
            { 
                // gulp! 
            }

            if (entries != null)
            {
                ret = entries.Length;
            }
            else
            {
                ret = Convert.ToInt16(this.totalSongs[0]);
            }
            
            return ret; 
        }
        public string GetSongName()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            return enc.GetString(FileUtil.ReplaceNullByteWithSpace(this.songName)).Trim();
        }

        #endregion

        #region HOOT

        public string GetHootDriverAlias() { return HOOT_DRIVER_ALIAS; }
        public string GetHootDriverType() { return HOOT_DRIVER_TYPE; }
        public string GetHootDriver() { return HOOT_DRIVER; }
        public string GetHootChips() 
        {
            StringBuilder _ret = new StringBuilder();
            _ret.Append(HOOT_CHIP);

            if ((this.extraChipsBits[0] & MASK_VRC6) == MASK_VRC6)
            {
                _ret.AppendFormat(", {0}", CHIP_VRC6);
            }
            if ((this.extraChipsBits[0] & MASK_VRC7) == MASK_VRC7)
            {
                _ret.AppendFormat(", {0}", CHIP_VRC7);
            }
            if ((this.extraChipsBits[0] & MASK_FDS) == MASK_FDS)
            {
                _ret.AppendFormat(", {0}", CHIP_FDS);
            }
            if ((this.extraChipsBits[0] & MASK_MMC5) == MASK_MMC5)
            {
                _ret.AppendFormat(", {0}", CHIP_MMC5);
            }
            if ((this.extraChipsBits[0] & MASK_N106) == MASK_N106)
            {
                _ret.AppendFormat(", {0}", CHIP_N106);
            }
            if ((this.extraChipsBits[0] & MASK_FME07) == MASK_FME07)
            {
                _ret.AppendFormat(", {0}", CHIP_FME07);
            }
                                                
            return _ret.ToString();         
        }

        public bool UsesPlaylist()
        {
            return false;
        }
        public NezPlugM3uEntry[] GetPlaylistEntries()
        {
            NezPlugM3uEntry[] entries = null;

            string m3uFileName = Path.ChangeExtension(this.filePath, NezPlugUtil.M3U_FILE_EXTENSION);
            entries = NezPlugUtil.GetNezPlugM3uEntriesFromFile(m3uFileName);

            return entries;
        }

        public bool HasMultipleFilesPerSet() { return false; }
        public string GetSetName() { return ByteConversion.GetAsciiText(this.songName); }

        #endregion

        #region EMBEDDED TAG METHODS

        public void UpdateSongName(string pNewValue)
        {
            FileUtil.UpdateTextField(this.filePath, pNewValue, NAME_OFFSET,
                NAME_LENGTH);
        }        
        public void UpdateArtist(string pNewValue)
        {
            FileUtil.UpdateTextField(this.filePath, pNewValue, ARTIST_OFFSET,
                ARTIST_LENGTH);
        }
        public void UpdateCopyright(string pNewValue)
        {
            FileUtil.UpdateTextField(this.filePath, pNewValue, COPYRIGHT_OFFSET,
                COPYRIGHT_LENGTH);
        }

        public string GetSongNameAsText()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            return enc.GetString(this.songName);
        }
        public string GetArtistAsText()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            return enc.GetString(this.songArtist);        
        }
        public string GetCopyrightAsText()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            return enc.GetString(this.songCopyright);
        }

        #endregion

        #region NEZPLUG M3U METHODS

        public string GetNezPlugPlaylistFormat() { return "NSF"; }

        #endregion
    }
}
