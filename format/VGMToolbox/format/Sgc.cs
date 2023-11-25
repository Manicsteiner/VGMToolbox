using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Sgc : IFormat, IEmbeddedTagsFormat, INezPlugPlaylistFormat
    {
        private static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x53, 0x47, 0x43, 0x1A }; // GBS
        private const string FORMAT_ABBREVIATION = "SGC";

        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x04;

        private const int VERSION_OFFSET = 0x04;
        private const int VERSION_LENGTH = 0x01;

        private const int PAL_NTSC_OFFSET = 0x05;
        private const int PAL_NTSC_LENGTH = 0x01;

        private const int SCANLINES_OFFSET = 0x06;
        private const int SCANLINES_LENGTH = 0x01;

        private const int RESERVED1_OFFSET = 0x07;
        private const int RESERVED1_LENGTH = 0x01;

        private const int START_ADDRESS_OFFSET = 0x08;
        private const int START_ADDRESS_LENGTH = 0x02;

        private const int INIT_ADDRESS_OFFSET = 0x0A;
        private const int INIT_ADDRESS_LENGTH = 0x02;

        private const int PLAY_ADDRESS_OFFSET = 0x0C;
        private const int PLAY_ADDRESS_LENGTH = 0x02;

        private const int STACK_POINTER_OFFSET = 0x0E;
        private const int STACK_POINTER_LENGTH = 0x02;

        private const int RESERVED2_OFFSET = 0x10;
        private const int RESERVED2_LENGTH = 0x02;

        private const int RST_POINTERS_OFFSET = 0x12;
        private const int RST_POINTERS_LENGTH = 0x0E;

        private const int MAPPER_CHIP_OFFSET = 0x20;
        private const int MAPPER_CHIP_LENGTH = 0x04;

        private const int STARTING_SONG_OFFSET = 0x24;
        private const int STARTING_SONG_LENGTH = 0x01;

        private const int TOTAL_SONGS_OFFSET = 0x25;
        private const int TOTAL_SONGS_LENGTH = 0x01;

        private const int STARTING_SFX_OFFSET = 0x26;
        private const int STARTING_SFX_LENGTH = 0x01;

        private const int ENDING_SFX_OFFSET = 0x27;
        private const int ENDING_SFX_LENGTH = 0x01;

        private const int SYSTEM_TYPE_OFFSET = 0x28;
        private const int SYSTEM_TYPE_LENGTH = 0x01;

        private const int RESERVED3_OFFSET = 0x29;
        private const int RESERVED3_LENGTH = 0x17;

        private const int NAME_OFFSET = 0x40;
        private const int NAME_LENGTH = 0x20;

        private const int ARTIST_OFFSET = 0x60;
        private const int ARTIST_LENGTH = 0x20;

        private const int COPYRIGHT_OFFSET = 0x80;
        private const int COPYRIGHT_LENGTH = 0x20;

        private const int DATA_OFFSET = 0xA0;

        public string FilePath { set; get; }

        public byte[] AsciiSignature { set; get; }
        public byte[] VersionNumber { set; get; }
        public byte[] PalNtscFlag { set; get; }
        public byte[] Scanlines { set; get; }
        public byte[] Reserved1 { set; get; }
        public byte[] StartAddress { set; get; }
        public byte[] InitAddress { set; get; }
        public byte[] PlayAddress { set; get; }
        public byte[] StackPointer { set; get; }
        public byte[] Reserved2 { set; get; }
        public byte[] RstPointers { set; get; }
        public byte[] MapperChip { set; get; }
        public byte[] StartingSong { set; get; }
        public byte[] TotalSongs { set; get; }
        public byte[] StartingSfx { set; get; }
        public byte[] EndingSfx { set; get; }
        public byte[] SystemType { set; get; }
        public byte[] Reserved3 { set; get; }
        public byte[] SongName { set; get; }
        public byte[] SongArtist { set; get; }
        public byte[] SongCopyright { set; get; }
        public byte[] Data { set; get; }

        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        #region METHODS

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.FilePath = pFilePath;
            this.AsciiSignature = ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
            this.VersionNumber = ParseFile.ParseSimpleOffset(pStream, VERSION_OFFSET, VERSION_LENGTH);
            this.PalNtscFlag = ParseFile.ParseSimpleOffset(pStream, PAL_NTSC_OFFSET, PAL_NTSC_LENGTH);
            this.Scanlines = ParseFile.ParseSimpleOffset(pStream, SCANLINES_OFFSET, SCANLINES_LENGTH);
            this.Reserved1 = ParseFile.ParseSimpleOffset(pStream, RESERVED1_OFFSET, RESERVED1_LENGTH);
            this.StartAddress = ParseFile.ParseSimpleOffset(pStream, START_ADDRESS_OFFSET, START_ADDRESS_LENGTH);
            this.InitAddress = ParseFile.ParseSimpleOffset(pStream, INIT_ADDRESS_OFFSET, INIT_ADDRESS_LENGTH);
            this.PlayAddress = ParseFile.ParseSimpleOffset(pStream, PLAY_ADDRESS_OFFSET, PLAY_ADDRESS_LENGTH);
            this.StackPointer = ParseFile.ParseSimpleOffset(pStream, STACK_POINTER_OFFSET, STACK_POINTER_LENGTH);
            this.Reserved2 = ParseFile.ParseSimpleOffset(pStream, RESERVED2_OFFSET, RESERVED2_LENGTH);
            this.RstPointers = ParseFile.ParseSimpleOffset(pStream, RST_POINTERS_OFFSET, RST_POINTERS_LENGTH);
            this.MapperChip = ParseFile.ParseSimpleOffset(pStream, MAPPER_CHIP_OFFSET, MAPPER_CHIP_LENGTH);
            this.StartingSong = ParseFile.ParseSimpleOffset(pStream, STARTING_SONG_OFFSET, STARTING_SONG_LENGTH);
            this.TotalSongs = ParseFile.ParseSimpleOffset(pStream, TOTAL_SONGS_OFFSET, TOTAL_SONGS_LENGTH);
            this.StartingSfx = ParseFile.ParseSimpleOffset(pStream, STARTING_SFX_OFFSET, STARTING_SFX_LENGTH);
            this.EndingSfx = ParseFile.ParseSimpleOffset(pStream, ENDING_SFX_OFFSET, ENDING_SFX_LENGTH);
            this.SystemType = ParseFile.ParseSimpleOffset(pStream, SYSTEM_TYPE_OFFSET, SYSTEM_TYPE_LENGTH);            
            this.Reserved3 = ParseFile.ParseSimpleOffset(pStream, RESERVED3_OFFSET, RESERVED3_LENGTH);
            this.SongName = ParseFile.ParseSimpleOffset(pStream, NAME_OFFSET, NAME_LENGTH);
            this.SongArtist = ParseFile.ParseSimpleOffset(pStream, ARTIST_OFFSET, ARTIST_LENGTH);
            this.SongCopyright = ParseFile.ParseSimpleOffset(pStream, COPYRIGHT_OFFSET, COPYRIGHT_LENGTH);
            this.Data = ParseFile.ParseSimpleOffset(pStream, DATA_OFFSET, (int)(pStream.Length - DATA_OFFSET) + 1);
                       
            this.initializeTagHash();
        }

        private void initializeTagHash()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            tagHash.Add("Name", enc.GetString(this.SongName));
            tagHash.Add("Artist", enc.GetString(this.SongArtist));
            tagHash.Add("Copyright", enc.GetString(this.SongCopyright));
            tagHash.Add("System", this.GetSystemTypeText());
            tagHash.Add("Total Songs", this.TotalSongs[0].ToString());
            tagHash.Add("Starting Song", this.StartingSong[0].ToString());
        }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();

            pChecksum.Update(this.VersionNumber);
            pChecksum.Update(this.PalNtscFlag);
            pChecksum.Update(this.Scanlines);
            pChecksum.Update(this.Reserved1);
            pChecksum.Update(this.StartAddress);
            pChecksum.Update(this.InitAddress);
            pChecksum.Update(this.PlayAddress);
            pChecksum.Update(this.StackPointer);
            pChecksum.Update(this.Reserved2);
            pChecksum.Update(this.RstPointers);
            pChecksum.Update(this.MapperChip);
            pChecksum.Update(this.StartingSong);
            pChecksum.Update(this.TotalSongs);
            pChecksum.Update(this.StartingSfx);
            pChecksum.Update(this.EndingSfx);
            pChecksum.Update(this.SystemType);
            pChecksum.Update(this.Reserved3);
            pChecksum.Update(this.Data);
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            pChecksum.Update(this.VersionNumber);
            pChecksum.Update(this.PalNtscFlag);
            pChecksum.Update(this.Scanlines);
            pChecksum.Update(this.Reserved1);
            pChecksum.Update(this.StartAddress);
            pChecksum.Update(this.InitAddress);
            pChecksum.Update(this.PlayAddress);
            pChecksum.Update(this.StackPointer);
            pChecksum.Update(this.Reserved2);
            pChecksum.Update(this.RstPointers);
            pChecksum.Update(this.MapperChip);
            pChecksum.Update(this.StartingSong);
            pChecksum.Update(this.TotalSongs);
            pChecksum.Update(this.StartingSfx);
            pChecksum.Update(this.EndingSfx);
            pChecksum.Update(this.SystemType);
            pChecksum.Update(this.Reserved3);
            pChecksum.Update(this.Data);

            pMd5CryptoStream.Write(this.VersionNumber, 0, this.VersionNumber.Length);
            pMd5CryptoStream.Write(this.PalNtscFlag, 0, this.PalNtscFlag.Length);
            pMd5CryptoStream.Write(this.Scanlines, 0, this.Scanlines.Length);
            pMd5CryptoStream.Write(this.Reserved1, 0, this.Reserved1.Length);
            pMd5CryptoStream.Write(this.StartAddress, 0, this.StartAddress.Length);
            pMd5CryptoStream.Write(this.InitAddress, 0, this.InitAddress.Length);
            pMd5CryptoStream.Write(this.PlayAddress, 0, this.PlayAddress.Length);
            pMd5CryptoStream.Write(this.StackPointer, 0, this.StackPointer.Length);
            pMd5CryptoStream.Write(this.Reserved2, 0, this.Reserved2.Length);
            pMd5CryptoStream.Write(this.RstPointers, 0, this.RstPointers.Length);
            pMd5CryptoStream.Write(this.MapperChip, 0, this.MapperChip.Length);
            pMd5CryptoStream.Write(this.StartingSong, 0, this.StartingSong.Length);
            pMd5CryptoStream.Write(this.TotalSongs, 0, this.TotalSongs.Length);
            pMd5CryptoStream.Write(this.StartingSfx, 0, this.StartingSfx.Length);
            pMd5CryptoStream.Write(this.EndingSfx, 0, this.EndingSfx.Length);
            pMd5CryptoStream.Write(this.SystemType, 0, this.SystemType.Length);
            pMd5CryptoStream.Write(this.Reserved3, 0, this.Reserved3.Length);
            pMd5CryptoStream.Write(this.Data, 0, this.Data.Length);

            pSha1CryptoStream.Write(this.VersionNumber, 0, this.VersionNumber.Length);
            pSha1CryptoStream.Write(this.PalNtscFlag, 0, this.PalNtscFlag.Length);
            pSha1CryptoStream.Write(this.Scanlines, 0, this.Scanlines.Length);
            pSha1CryptoStream.Write(this.Reserved1, 0, this.Reserved1.Length);
            pSha1CryptoStream.Write(this.StartAddress, 0, this.StartAddress.Length);
            pSha1CryptoStream.Write(this.InitAddress, 0, this.InitAddress.Length);
            pSha1CryptoStream.Write(this.PlayAddress, 0, this.PlayAddress.Length);
            pSha1CryptoStream.Write(this.StackPointer, 0, this.StackPointer.Length);
            pSha1CryptoStream.Write(this.Reserved2, 0, this.Reserved2.Length);
            pSha1CryptoStream.Write(this.RstPointers, 0, this.RstPointers.Length);
            pSha1CryptoStream.Write(this.MapperChip, 0, this.MapperChip.Length);
            pSha1CryptoStream.Write(this.StartingSong, 0, this.StartingSong.Length);
            pSha1CryptoStream.Write(this.TotalSongs, 0, this.TotalSongs.Length);
            pSha1CryptoStream.Write(this.StartingSfx, 0, this.StartingSfx.Length);
            pSha1CryptoStream.Write(this.EndingSfx, 0, this.EndingSfx.Length);
            pSha1CryptoStream.Write(this.SystemType, 0, this.SystemType.Length);
            pSha1CryptoStream.Write(this.Reserved3, 0, this.Reserved3.Length);
            pSha1CryptoStream.Write(this.Data, 0, this.Data.Length);
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

        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }

        public int GetStartingSong() { return Convert.ToInt16(this.StartingSong[0]); }
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
                ret = Convert.ToInt16(this.TotalSongs[0]);
            }

            return ret;
        }
        public string GetSongName()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            return enc.GetString(FileUtil.ReplaceNullByteWithSpace(this.SongName)).Trim();
        }
        public string GetSystemTypeText()
        {
            int systemType = this.SystemType[0];
            string ret;

            switch (systemType)
            { 
                case 0:
                    ret = "Sega Master System";
                    break;
                case 1:
                    ret = "Sega Game Gear";
                    break;
                case 2:
                    ret = "Coleco ColecoVision";
                    break;
                default:
                    ret = "Unknown";
                    break;
            }

            return ret;
        }

        public NezPlugM3uEntry[] GetPlaylistEntries()
        {
            NezPlugM3uEntry[] entries = null;

            string m3uFileName = Path.ChangeExtension(this.FilePath, NezPlugUtil.M3U_FILE_EXTENSION);
            entries = NezPlugUtil.GetNezPlugM3uEntriesFromFile(m3uFileName);

            return entries;
        }

        #endregion

        #region EMBEDDED TAG METHODS

        public void UpdateSongName(string pNewValue)
        {
            FileUtil.UpdateTextField(this.FilePath, pNewValue, NAME_OFFSET,
                NAME_LENGTH);
        }
        public void UpdateArtist(string pNewValue)
        {
            FileUtil.UpdateTextField(this.FilePath, pNewValue, ARTIST_OFFSET,
                ARTIST_LENGTH);
        }
        public void UpdateCopyright(string pNewValue)
        {
            FileUtil.UpdateTextField(this.FilePath, pNewValue, COPYRIGHT_OFFSET,
                COPYRIGHT_LENGTH);
        }

        public string GetSongNameAsText()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            return enc.GetString(FileUtil.ReplaceNullByteWithSpace(this.SongName)).Trim();
        }
        public string GetArtistAsText()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            return enc.GetString(FileUtil.ReplaceNullByteWithSpace(this.SongArtist)).Trim();
        }
        public string GetCopyrightAsText()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            return enc.GetString(FileUtil.ReplaceNullByteWithSpace(this.SongCopyright)).Trim();
        }

        #endregion

        #region NEZPLUG M3U METHODS

        public string GetNezPlugPlaylistFormat() { return "SGC"; }

        #endregion
    }
}
