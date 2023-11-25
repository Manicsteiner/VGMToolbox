using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.util;

namespace format.VGMToolbox.format
{
    class Hes : IHootFormat
    {
        public static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x48, 0x45, 0x53, 0x4D }; // HESM
        private const string FORMAT_ABBREVIATION = "HES";

        private const string HOOT_DRIVER_ALIAS = "TurboGrafx-16";
        private const string HOOT_DRIVER_TYPE = "hes";
        private const string HOOT_DRIVER = "pcengine";
        private const string HOOT_CHIP = "Hudson WSG";

        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x04;

        private const int VERSION_OFFSET = 0x04;
        private const int VERSION_LENGTH = 0x01;

        private const int STARTING_SONG_OFFSET = 0x05;
        private const int STARTING_SONG_LENGTH = 0x01;

        private const int REQUEST_ADDRESS_OFFSET = 0x06;
        private const int REQUEST_ADDRESS_LENGTH = 0x02;

        private const int INITIAL_MPR0_OFFSET = 0x08;
        private const int INITIAL_MPR0_LENGTH = 0x01;

        private const int INITIAL_MPR1_OFFSET = 0x09;
        private const int INITIAL_MPR1_LENGTH = 0x01;

        private const int INITIAL_MPR2_OFFSET = 0x0A;
        private const int INITIAL_MPR2_LENGTH = 0x01;

        private const int INITIAL_MPR3_OFFSET = 0x0B;
        private const int INITIAL_MPR3_LENGTH = 0x01;

        private const int INITIAL_MPR4_OFFSET = 0x0C;
        private const int INITIAL_MPR4_LENGTH = 0x01;

        private const int INITIAL_MPR5_OFFSET = 0x0D;
        private const int INITIAL_MPR5_LENGTH = 0x01;

        private const int INITIAL_MPR6_OFFSET = 0x0E;
        private const int INITIAL_MPR6_LENGTH = 0x01;

        private const int INITIAL_MPR7_OFFSET = 0x0F;
        private const int INITIAL_MPR7_LENGTH = 0x01;

        private const int DATA_SIG_OFFSET = 0x10;
        private const int DATA_SIG_LENGTH = 0x04;

        private const int DATA_SIZE_OFFSET = 0x14;
        private const int DATA_SIZE_LENGTH = 0x04;

        private const int LOAD_ADDRESS_OFFSET = 0x18;
        private const int LOAD_ADDRESS_LENGTH = 0x04;

        private const int RESERVE_OFFSET = 0x1C;
        private const int RESERVE_LENGTH = 0x04;

        private const int DATA_OFFSET = 0x20;

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private byte[] asciiSignature;
        private byte[] versionNumber;
        private byte[] startingSong;
        private byte[] requestAddress;
        private byte[] initialMpr0;
        private byte[] initialMpr1;
        private byte[] initialMpr2;
        private byte[] initialMpr3;
        private byte[] initialMpr4;
        private byte[] initialMpr5;
        private byte[] initialMpr6;
        private byte[] initialMpr7;

        private byte[] dataSig;
        private byte[] dataSize;
        private byte[] loadAddress;
        private byte[] reserve;
        private byte[] data;

        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        public byte[] AsciiSignature { get { return this.asciiSignature; } }
        public byte[] VersionNumber { get { return this.versionNumber; } }
        public byte[] StartingSong { get { return this.startingSong; } }
        public byte[] RequestAddress { get { return this.requestAddress; } }
        public byte[] InitialMpr0 { get { return this.initialMpr0; } }
        public byte[] InitialMpr1 { get { return this.initialMpr1; } }
        public byte[] InitialMpr2 { get { return this.initialMpr2; } }
        public byte[] InitialMpr3 { get { return this.initialMpr3; } }
        public byte[] InitialMpr4 { get { return this.initialMpr4; } }
        public byte[] InitialMpr5 { get { return this.initialMpr5; } }
        public byte[] InitialMpr6 { get { return this.initialMpr6; } }
        public byte[] InitialMpr7 { get { return this.initialMpr7; } }

        public byte[] DataSig { get { return this.dataSig; } }
        public byte[] DataSize { get { return this.dataSize; } }
        public byte[] LoadAddress { get { return this.loadAddress; } }
        public byte[] Reserve { get { return this.reserve; } }
        public byte[] Data { get { return this.data; } }

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath = pFilePath;
            
            this.asciiSignature = ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
            this.versionNumber = ParseFile.ParseSimpleOffset(pStream, VERSION_OFFSET, VERSION_LENGTH);
            this.startingSong = ParseFile.ParseSimpleOffset(pStream, STARTING_SONG_OFFSET, STARTING_SONG_LENGTH);
            this.requestAddress = ParseFile.ParseSimpleOffset(pStream, REQUEST_ADDRESS_OFFSET, REQUEST_ADDRESS_LENGTH);
            this.initialMpr0 = ParseFile.ParseSimpleOffset(pStream, INITIAL_MPR0_OFFSET, INITIAL_MPR0_LENGTH);
            this.initialMpr1 = ParseFile.ParseSimpleOffset(pStream, INITIAL_MPR1_OFFSET, INITIAL_MPR1_LENGTH);
            this.initialMpr2 = ParseFile.ParseSimpleOffset(pStream, INITIAL_MPR2_OFFSET, INITIAL_MPR2_LENGTH);
            this.initialMpr3 = ParseFile.ParseSimpleOffset(pStream, INITIAL_MPR3_OFFSET, INITIAL_MPR3_LENGTH);
            this.initialMpr4 = ParseFile.ParseSimpleOffset(pStream, INITIAL_MPR4_OFFSET, INITIAL_MPR4_LENGTH);
            this.initialMpr5 = ParseFile.ParseSimpleOffset(pStream, INITIAL_MPR5_OFFSET, INITIAL_MPR5_LENGTH);
            this.initialMpr6 = ParseFile.ParseSimpleOffset(pStream, INITIAL_MPR6_OFFSET, INITIAL_MPR6_LENGTH);
            this.initialMpr7 = ParseFile.ParseSimpleOffset(pStream, INITIAL_MPR7_OFFSET, INITIAL_MPR7_LENGTH);

            this.dataSig = ParseFile.ParseSimpleOffset(pStream, DATA_SIG_OFFSET, DATA_SIG_LENGTH);
            this.dataSize = ParseFile.ParseSimpleOffset(pStream, DATA_SIZE_OFFSET, DATA_SIZE_LENGTH);                                                           
            this.loadAddress = ParseFile.ParseSimpleOffset(pStream, LOAD_ADDRESS_OFFSET, LOAD_ADDRESS_LENGTH);
            this.reserve = ParseFile.ParseSimpleOffset(pStream, RESERVE_OFFSET, RESERVE_LENGTH);
            
            int dataLength = BitConverter.ToInt32(this.dataSize, 0);
            this.data = ParseFile.ParseSimpleOffset(pStream, DATA_OFFSET, dataLength);            

            this.initializeTagHash();
        }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();

            pChecksum.Update(versionNumber);
            pChecksum.Update(startingSong);
            pChecksum.Update(requestAddress);

            pChecksum.Update(initialMpr0);
            pChecksum.Update(initialMpr1);
            pChecksum.Update(initialMpr2);
            pChecksum.Update(initialMpr3);
            pChecksum.Update(initialMpr4);
            pChecksum.Update(initialMpr5);
            pChecksum.Update(initialMpr6);
            pChecksum.Update(initialMpr7);

            pChecksum.Update(loadAddress);
            pChecksum.Update(reserve);
            pChecksum.Update(data);
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            pChecksum.Update(versionNumber);
            pChecksum.Update(startingSong);
            pChecksum.Update(requestAddress);
            pChecksum.Update(initialMpr0);
            pChecksum.Update(initialMpr1);
            pChecksum.Update(initialMpr2);
            pChecksum.Update(initialMpr3);
            pChecksum.Update(initialMpr4);
            pChecksum.Update(initialMpr5);
            pChecksum.Update(initialMpr6);
            pChecksum.Update(initialMpr7);
            pChecksum.Update(loadAddress);
            pChecksum.Update(reserve);
            pChecksum.Update(data);

            pMd5CryptoStream.Write(versionNumber, 0, versionNumber.Length);
            pMd5CryptoStream.Write(startingSong, 0, startingSong.Length);
            pMd5CryptoStream.Write(requestAddress, 0, requestAddress.Length);
            pMd5CryptoStream.Write(initialMpr0, 0, initialMpr0.Length);
            pMd5CryptoStream.Write(initialMpr1, 0, initialMpr1.Length);
            pMd5CryptoStream.Write(initialMpr2, 0, initialMpr2.Length);
            pMd5CryptoStream.Write(initialMpr3, 0, initialMpr3.Length);
            pMd5CryptoStream.Write(initialMpr4, 0, initialMpr4.Length);
            pMd5CryptoStream.Write(initialMpr5, 0, initialMpr5.Length);
            pMd5CryptoStream.Write(initialMpr6, 0, initialMpr6.Length);
            pMd5CryptoStream.Write(initialMpr7, 0, initialMpr7.Length);
            pMd5CryptoStream.Write(loadAddress, 0, loadAddress.Length);
            pMd5CryptoStream.Write(reserve, 0, reserve.Length);
            pMd5CryptoStream.Write(data, 0, data.Length);

            pSha1CryptoStream.Write(versionNumber, 0, versionNumber.Length);
            pSha1CryptoStream.Write(startingSong, 0, startingSong.Length);
            pSha1CryptoStream.Write(requestAddress, 0, requestAddress.Length);
            pSha1CryptoStream.Write(initialMpr0, 0, initialMpr0.Length);
            pSha1CryptoStream.Write(initialMpr1, 0, initialMpr1.Length);
            pSha1CryptoStream.Write(initialMpr2, 0, initialMpr2.Length);
            pSha1CryptoStream.Write(initialMpr3, 0, initialMpr3.Length);
            pSha1CryptoStream.Write(initialMpr4, 0, initialMpr4.Length);
            pSha1CryptoStream.Write(initialMpr5, 0, initialMpr5.Length);
            pSha1CryptoStream.Write(initialMpr6, 0, initialMpr6.Length);
            pSha1CryptoStream.Write(initialMpr7, 0, initialMpr7.Length);
            pSha1CryptoStream.Write(loadAddress, 0, loadAddress.Length);
            pSha1CryptoStream.Write(reserve, 0, reserve.Length);
            pSha1CryptoStream.Write(data, 0, data.Length);
        }

        private void initializeTagHash()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;            
            tagHash.Add("Starting Song", this.startingSong[0].ToString());
            tagHash.Add("Request Address", String.Format("0x{0}", BitConverter.ToInt16(this.requestAddress, 0).ToString("X2")));
            tagHash.Add("Initial MPR0", String.Format("0x{0}", this.initialMpr0[0].ToString("X2")));
            tagHash.Add("Initial MPR1", String.Format("0x{0}", this.initialMpr1[0].ToString("X2")));
            tagHash.Add("Initial MPR2", String.Format("0x{0}", this.initialMpr2[0].ToString("X2")));
            tagHash.Add("Initial MPR3", String.Format("0x{0}", this.initialMpr3[0].ToString("X2")));
            tagHash.Add("Initial MPR4", String.Format("0x{0}", this.initialMpr4[0].ToString("X2")));
            tagHash.Add("Initial MPR5", String.Format("0x{0}", this.initialMpr5[0].ToString("X2")));
            tagHash.Add("Initial MPR6", String.Format("0x{0}", this.initialMpr6[0].ToString("X2")));
            tagHash.Add("Initial MPR7", String.Format("0x{0}", this.initialMpr7[0].ToString("X2")));            
            tagHash.Add("Data Size", String.Format("0x{0}", BitConverter.ToInt32(this.dataSize, 0).ToString("X4")));
            tagHash.Add("Load Address", String.Format("0x{0}", BitConverter.ToInt32(this.loadAddress, 0).ToString("X4")));
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

        #region HOOT

        public int GetStartingSong() 
        {
            return this.startingSong[0];
        }
        public int GetTotalSongs()
        {
            NezPlugM3uEntry[] entries = this.GetPlaylistEntries();
            return entries.Length;
        }
        public string GetSongName()
        {
            return null;
        }

        public string GetHootDriverAlias() { return HOOT_DRIVER_ALIAS; }
        public string GetHootDriverType() { return HOOT_DRIVER_TYPE; }
        public string GetHootDriver() { return HOOT_DRIVER; }
        public string GetHootChips() { return HOOT_CHIP; }

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
