using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    class Psid : IFormat, IEmbeddedTagsFormat
    {
        # region CONSTANTS

        public static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x50, 0x53, 0x49, 0x44 }; // PSID
        private const string FORMAT_ABBREVIATION = "PSID";

        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x04;

        private const int VERSION_OFFSET = 0x04;
        private const int VERSION_LENGTH = 0x02;

        private const int DATA_OFFSET_OFFSET = 0x06;
        private const int DATA_OFFSET_LENGTH = 0x02;

        private const int LOAD_ADDRESS_OFFSET = 0x08;
        private const int LOAD_ADDRESS_LENGTH = 0x02;

        private const int INIT_ADDRESS_OFFSET = 0x0A;
        private const int INIT_ADDRESS_LENGTH = 0x02;

        private const int PLAY_ADDRESS_OFFSET = 0x0C;
        private const int PLAY_ADDRESS_LENGTH = 0x02;

        private const int TOTAL_SONGS_OFFSET = 0x0E;
        private const int TOTAL_SONGS_LENGTH = 0x02;

        private const int STARTING_SONG_OFFSET = 0x10;
        private const int STARTING_SONG_LENGTH = 0x02;

        private const int SPEED_OFFSET = 0x12;
        private const int SPEED_LENGTH = 0x04;

        private const int NAME_OFFSET = 0x16;
        private const int NAME_LENGTH = 0x20;

        private const int ARTIST_OFFSET = 0x36;
        private const int ARTIST_LENGTH = 0x20;

        private const int COPYRIGHT_OFFSET = 0x56;
        private const int COPYRIGHT_LENGTH = 0x20;

        private const int V2_FLAGS_OFFSET = 0x76;
        private const int V2_FLAGS_LENGTH = 0x02;

        private const int V2_START_PAGE_OFFSET = 0x78;
        private const int V2_START_PAGE_LENGTH = 0x01;

        private const int V2_PAGE_LENGTH_OFFSET = 0x79;
        private const int V2_PAGE_LENGTH_LENGTH = 0x01;

        private const int V2_RESERVED_OFFSET = 0x7A;
        private const int V2_RESERVED_LENGTH = 0x02;

        # endregion

        #region FIELDS

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private byte[] asciiSignature;
        private byte[] versionNumber;
        private byte[] dataOffset;
        private byte[] loadAddress;
        private byte[] initAddress;
        private byte[] playAddress;
        private byte[] totalSongs;
        private byte[] startingSong;
        private byte[] speed;
        private byte[] songName;
        private byte[] songArtist;
        private byte[] songCopyright;
        private byte[] v2Flags;
        private byte[] v2StartPage;
        private byte[] v2PageLength;
        private byte[] v2Reserved;
        private byte[] data;

        private byte[] versionNumberLE;
        private int intVersionNumber;
        private byte[] totalSongsLE;
        private byte[] startingSongLE;

        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        public byte[] AsciiSignature { get { return this.asciiSignature; } }
        public byte[] VersionNumber { get { return this.versionNumber; } }
        public byte[] DataOffset { get { return this.dataOffset; } }
        public byte[] LoadAddress { get { return this.loadAddress; } }
        public byte[] InitAddress { get { return this.initAddress; } }
        public byte[] PlayAddress { get { return this.playAddress; } }
        public byte[] TotalSongs { get { return this.totalSongs; } }
        public byte[] StartingSong { get { return this.startingSong; } }
        public byte[] Speed { get { return this.speed; } }
        public byte[] SongName { get { return this.songName; } }
        public byte[] SongArtist { get { return this.songArtist; } }
        public byte[] SongCopyright { get { return this.songCopyright; } }
        public byte[] V2Flags { get { return this.v2Flags; } }
        public byte[] V2StartPage { get { return this.v2StartPage; } }
        public byte[] V2PageLength { get { return this.v2PageLength; } }
        public byte[] V2Reserved { get { return this.v2Reserved; } }
        public byte[] Data { get { return this.data; } }

        #endregion

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath =  pFilePath;
            this.asciiSignature = ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
            this.versionNumber = ParseFile.ParseSimpleOffset(pStream, VERSION_OFFSET, VERSION_LENGTH);            
            this.dataOffset = ParseFile.ParseSimpleOffset(pStream, DATA_OFFSET_OFFSET, DATA_OFFSET_LENGTH);
            this.loadAddress = ParseFile.ParseSimpleOffset(pStream, LOAD_ADDRESS_OFFSET, LOAD_ADDRESS_LENGTH);
            this.initAddress = ParseFile.ParseSimpleOffset(pStream, INIT_ADDRESS_OFFSET, INIT_ADDRESS_LENGTH);
            this.playAddress = ParseFile.ParseSimpleOffset(pStream, PLAY_ADDRESS_OFFSET, PLAY_ADDRESS_LENGTH);
            this.totalSongs = ParseFile.ParseSimpleOffset(pStream, TOTAL_SONGS_OFFSET, TOTAL_SONGS_LENGTH);
            this.startingSong = ParseFile.ParseSimpleOffset(pStream, STARTING_SONG_OFFSET, STARTING_SONG_LENGTH);
            this.speed = ParseFile.ParseSimpleOffset(pStream, SPEED_OFFSET, SPEED_LENGTH);
            this.songName = ParseFile.ParseSimpleOffset(pStream, NAME_OFFSET, NAME_LENGTH);
            this.songArtist = ParseFile.ParseSimpleOffset(pStream, ARTIST_OFFSET, ARTIST_LENGTH);
            this.songCopyright = ParseFile.ParseSimpleOffset(pStream, COPYRIGHT_OFFSET, COPYRIGHT_LENGTH);

            this.totalSongsLE = new byte[TOTAL_SONGS_LENGTH];
            Array.Copy(this.totalSongs, this.totalSongsLE, TOTAL_SONGS_LENGTH);
            Array.Reverse(this.totalSongsLE);

            this.startingSongLE = new byte[STARTING_SONG_LENGTH];
            Array.Copy(this.startingSong, this.startingSongLE, STARTING_SONG_LENGTH);
            Array.Reverse(this.startingSongLE);

            // version 2 stuff
            this.versionNumberLE = new byte[VERSION_LENGTH]; 
            Array.Copy(this.versionNumber, this.versionNumberLE, VERSION_LENGTH);
            Array.Reverse(this.versionNumberLE);
            this.intVersionNumber = BitConverter.ToInt16(this.versionNumberLE, 0);

            if (this.intVersionNumber == 2)
            {
                this.v2Flags = ParseFile.ParseSimpleOffset(pStream, V2_FLAGS_OFFSET, V2_FLAGS_LENGTH);
                this.v2StartPage = ParseFile.ParseSimpleOffset(pStream, V2_START_PAGE_OFFSET, V2_START_PAGE_LENGTH);
                this.v2PageLength = ParseFile.ParseSimpleOffset(pStream, V2_PAGE_LENGTH_OFFSET, V2_PAGE_LENGTH_LENGTH);
                this.v2Reserved = ParseFile.ParseSimpleOffset(pStream, V2_RESERVED_OFFSET, V2_RESERVED_LENGTH);
            }

            // data
            byte[] dataOffsetLE = this.dataOffset;
            Array.Reverse(dataOffsetLE);
            int intDataOffset = BitConverter.ToInt16(dataOffsetLE, 0);

            this.data = ParseFile.ParseSimpleOffset(pStream, intDataOffset, (int) (pStream.Length - intDataOffset));

            this.initializeTagHash();
        }

        private void initializeTagHash()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            // Build Tag Hash
            tagHash.Add("Name", enc.GetString(this.songName));
            tagHash.Add("Artist", enc.GetString(this.songArtist));
            tagHash.Add("Copyright", enc.GetString(this.songCopyright));
            tagHash.Add("PSID Version", BitConverter.ToInt16(this.versionNumberLE, 0).ToString());
            tagHash.Add("Total Songs", BitConverter.ToInt16(this.totalSongsLE, 0).ToString());
            tagHash.Add("Starting Song", BitConverter.ToInt16(this.startingSongLE, 0).ToString());
        }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();

            /*
            private byte[] v2Reserved;
            */

            pChecksum.Update(versionNumber);
            pChecksum.Update(loadAddress);
            pChecksum.Update(initAddress);
            pChecksum.Update(playAddress);            
            pChecksum.Update(totalSongs);
            pChecksum.Update(startingSong);
            pChecksum.Update(speed);

            if (this.intVersionNumber == 2)
            {
                pChecksum.Update(v2Flags);
                pChecksum.Update(v2StartPage);
                pChecksum.Update(v2PageLength);
            }

            pChecksum.Update(data);
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            /*
            private byte[] v2Reserved;
            */

            pChecksum.Update(versionNumber);
            pChecksum.Update(loadAddress);
            pChecksum.Update(initAddress);
            pChecksum.Update(playAddress);
            pChecksum.Update(totalSongs);
            pChecksum.Update(startingSong);
            pChecksum.Update(speed);

            pMd5CryptoStream.Write(versionNumber, 0, versionNumber.Length);
            pMd5CryptoStream.Write(loadAddress, 0, loadAddress.Length);
            pMd5CryptoStream.Write(initAddress, 0, initAddress.Length);
            pMd5CryptoStream.Write(playAddress, 0, playAddress.Length);
            pMd5CryptoStream.Write(totalSongs, 0, totalSongs.Length);
            pMd5CryptoStream.Write(startingSong, 0, startingSong.Length);
            pMd5CryptoStream.Write(speed, 0, speed.Length);

            pSha1CryptoStream.Write(versionNumber, 0, versionNumber.Length);
            pSha1CryptoStream.Write(loadAddress, 0, loadAddress.Length);
            pSha1CryptoStream.Write(initAddress, 0, initAddress.Length);
            pSha1CryptoStream.Write(playAddress, 0, playAddress.Length);
            pSha1CryptoStream.Write(totalSongs, 0, totalSongs.Length);
            pSha1CryptoStream.Write(startingSong, 0, startingSong.Length);
            pSha1CryptoStream.Write(speed, 0, speed.Length);

            if (this.intVersionNumber == 2)
            {
                pChecksum.Update(v2Flags);
                pChecksum.Update(v2StartPage);
                pChecksum.Update(v2PageLength);

                pMd5CryptoStream.Write(v2Flags, 0, v2Flags.Length);
                pMd5CryptoStream.Write(v2StartPage, 0, v2StartPage.Length);
                pMd5CryptoStream.Write(v2PageLength, 0, v2PageLength.Length);

                pSha1CryptoStream.Write(v2Flags, 0, v2Flags.Length);
                pSha1CryptoStream.Write(v2StartPage, 0, v2StartPage.Length);
                pSha1CryptoStream.Write(v2PageLength, 0, v2PageLength.Length);
            }

            pChecksum.Update(data);
            pMd5CryptoStream.Write(data, 0, data.Length);
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

        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }

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
    }
}
