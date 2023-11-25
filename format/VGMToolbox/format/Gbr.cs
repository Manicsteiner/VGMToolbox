using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Gbr : IFormat
    {
        private static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x47, 0x42, 0x52, 0x46 }; // GBRF
        private const string FORMAT_ABBREVIATION = "GBR";

        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x04;

        private const int NUMBER_OF_BANKS_OFFSET = 0x04;
        private const int NUMBER_OF_BANKS_LENGTH = 0x01;

        private const int INITIAL_0000_3FFF_BANK_OFFSET = 0x05;
        private const int INITIAL_0000_3FFF_BANK_LENGTH = 0x01;

        private const int INITIAL_4000_7FFF_BANK_OFFSET = 0x06;
        private const int INITIAL_4000_7FFF_BANK_LENGTH = 0x01;

        private const int TIMERFLAG_OFFSET = 0x07;
        private const int TIMERFLAG_LENGTH = 0x01;

        private const int SONG_INIT_OFFSET = 0x08;
        private const int SONG_INIT_LENGTH = 0x02;

        private const int VSYNC_OFFSET = 0x0A;
        private const int VSYNC_LENGTH = 0x02;

        private const int TIMER_OFFSET = 0x0C;
        private const int TIMER_LENGTH = 0x02;

        private const int FF06_WRITE_OFFSET = 0x0E;
        private const int FF06_WRITE_LENGTH = 0x01;

        private const int FF07_WRITE_OFFSET = 0x0F;
        private const int FF07_WRITE_LENGTH = 0x01;

        private const int UNUSED_OFFSET = 0x10;
        private const int UNUSED_LENGTH = 0x10;

        private const int DATA_OFFSET = 0x20;

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private byte[] asciiSignature;
        private byte[] numberOfBanks;
        private byte[] initial0000_3FFFBankOffset;
        private byte[] initial4000_7FFFBankOffset;
        private byte[] timerFlag;
        private byte[] songInit;
        private byte[] vsync;
        private byte[] timer;
        private byte[] ff06Write;
        private byte[] ff07Write;
        private byte[] unused;
        private byte[] data;

        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        public byte[] AsciiSignature { get { return this.asciiSignature; } }
        public byte[] NumberOfBanks { get { return this.numberOfBanks; } }
        public byte[] Initial0000_3FFFBankOffset { get { return this.initial0000_3FFFBankOffset; } }
        public byte[] Initial4000_7FFFBankOffset { get { return this.initial4000_7FFFBankOffset; } }
        public byte[] TimerFlag { get { return this.timerFlag; } }
        public byte[] SongInit { get { return this.songInit; } }
        public byte[] Vsync { get { return this.vsync; } }
        public byte[] Timer { get { return this.timer; } }
        public byte[] Ff06Write { get { return this.ff06Write; } }
        public byte[] Ff07Write { get { return this.ff07Write; } }
        public byte[] Unused { get { return this.unused; } }
        public byte[] Data { get { return this.data; } }
                
        #region METHODS

        public byte[] getAsciiSignature(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
        }

        public byte[] getNumberOfBanks(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, NUMBER_OF_BANKS_OFFSET, NUMBER_OF_BANKS_LENGTH);
        }

        public byte[] getInitial0000_3FFFBankOffset(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, INITIAL_0000_3FFF_BANK_OFFSET, INITIAL_0000_3FFF_BANK_LENGTH);
        }

        public byte[] getInitial4000_7FFFBankOffset(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, INITIAL_4000_7FFF_BANK_OFFSET, INITIAL_4000_7FFF_BANK_LENGTH);
        }

        public byte[] getTimerFlag(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, TIMERFLAG_OFFSET, TIMERFLAG_LENGTH);
        }

        public byte[] getSongInit(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SONG_INIT_OFFSET, SONG_INIT_LENGTH);
        }

        public byte[] getVsync(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, VSYNC_OFFSET, VSYNC_LENGTH);
        }

        public byte[] getTimer(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, TIMER_OFFSET, TIMER_LENGTH);
        }

        public byte[] getFf06Write(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, FF06_WRITE_OFFSET, FF06_WRITE_LENGTH);
        }

        public byte[] getFf07Write(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, FF07_WRITE_OFFSET, FF07_WRITE_LENGTH);
        }

        public byte[] getUnused(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, UNUSED_OFFSET, UNUSED_LENGTH);
        }

        public byte[] getData(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, DATA_OFFSET, (int)(pStream.Length - DATA_OFFSET) + 1);
        }


        public void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath = pFilePath;
            this.asciiSignature = this.getAsciiSignature(pStream);
            this.numberOfBanks = this.getNumberOfBanks(pStream);
            this.initial0000_3FFFBankOffset = this.getInitial0000_3FFFBankOffset(pStream);
            this.initial4000_7FFFBankOffset = this.getInitial4000_7FFFBankOffset(pStream);
            this.timerFlag = this.getTimerFlag(pStream);
            this.songInit = this.getSongInit(pStream);
            this.vsync = this.getVsync(pStream);
            this.timer = this.getTimer(pStream);
            this.ff06Write = this.getFf06Write(pStream);
            this.ff07Write = this.getFf07Write(pStream);
            this.unused = this.getUnused(pStream);
            this.data = this.getData(pStream);

            this.initializeTagHash();
        }

        private void initializeTagHash()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            tagHash.Add("Number of Banks", this.numberOfBanks[0].ToString());
        }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();

            pChecksum.Update(numberOfBanks);            
            pChecksum.Update(initial0000_3FFFBankOffset);
            pChecksum.Update(initial4000_7FFFBankOffset);
            pChecksum.Update(timerFlag);
            pChecksum.Update(vsync);
            pChecksum.Update(timer);
            pChecksum.Update(ff06Write);
            pChecksum.Update(ff07Write);
            pChecksum.Update(data);
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            pChecksum.Update(numberOfBanks);
            pChecksum.Update(initial0000_3FFFBankOffset);
            pChecksum.Update(initial4000_7FFFBankOffset);
            pChecksum.Update(timerFlag);
            pChecksum.Update(vsync);
            pChecksum.Update(timer);
            pChecksum.Update(ff06Write);
            pChecksum.Update(ff07Write);
            pChecksum.Update(data);

            pMd5CryptoStream.Write(numberOfBanks, 0, numberOfBanks.Length);
            pMd5CryptoStream.Write(initial0000_3FFFBankOffset, 0, initial0000_3FFFBankOffset.Length);
            pMd5CryptoStream.Write(initial4000_7FFFBankOffset, 0, initial4000_7FFFBankOffset.Length);
            pMd5CryptoStream.Write(timerFlag, 0, timerFlag.Length);
            pMd5CryptoStream.Write(vsync, 0, vsync.Length);
            pMd5CryptoStream.Write(timer, 0, timer.Length);
            pMd5CryptoStream.Write(ff06Write, 0, ff06Write.Length);
            pMd5CryptoStream.Write(ff07Write, 0, ff07Write.Length);
            pMd5CryptoStream.Write(data, 0, data.Length);

            pSha1CryptoStream.Write(numberOfBanks, 0, numberOfBanks.Length);
            pSha1CryptoStream.Write(initial0000_3FFFBankOffset, 0, initial0000_3FFFBankOffset.Length);
            pSha1CryptoStream.Write(initial4000_7FFFBankOffset, 0, initial4000_7FFFBankOffset.Length);
            pSha1CryptoStream.Write(timerFlag, 0, timerFlag.Length);
            pSha1CryptoStream.Write(vsync, 0, vsync.Length);
            pSha1CryptoStream.Write(timer, 0, timer.Length);
            pSha1CryptoStream.Write(ff06Write, 0, ff06Write.Length);
            pSha1CryptoStream.Write(ff07Write, 0, ff07Write.Length);
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

        #endregion
    }
}
