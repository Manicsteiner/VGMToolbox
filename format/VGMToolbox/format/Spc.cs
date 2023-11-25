using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Spc : IFormat, IHootFormat
    {
        private static readonly byte[] ASCII_SIGNATURE = 
            new byte[] { 0x53, 0x4E, 0x45, 0x53, 0x2D, 0x53, 0x50, 0x43, 0x37,
                         0x30, 0x30, 0x20, 0x53, 0x6F, 0x75, 0x6E, 0x64, 0x20,
                         0x46, 0x69, 0x6C, 0x65, 0x20, 0x44, 0x61, 0x74, 0x61,
                         0x20, 0x76, 0x30, 0x2E, 0x33, 0x30 }; // SNES-SPC700 Sound File Data v0.30
        private const string FORMAT_ABBREVIATION = "SPC";

        private static readonly byte[] EXTENDED_ID666_SIGNATURE =
            new byte[] { 0x78, 0x69, 0x64, 0x36 }; // xid6
        private static readonly byte[] ID666_IN_HEADER_VAL = new byte[] { 0x1A };
        private static readonly byte[] ID666_NOT_IN_HEADER_VAL = new byte[] { 0x1B };

        private static readonly byte[] EXID666_SUBCHUNK_TYPE_LENGTH = new byte[] { 0x00 };
        private static readonly byte[] EXID666_SUBCHUNK_TYPE_STRING = new byte[] { 0x01 };
        private static readonly byte[] EXID666_SUBCHUNK_TYPE_INTEGER = new byte[] { 0x04 };

        private const string EXID666_SUBCHUNK_ID_DUMPDATE = "05";
        private const string EXID666_SUBCHUNK_ID_OST_TRACK = "12";

        private const string HOOT_DRIVER_ALIAS = "SNES";
        private const string HOOT_DRIVER_TYPE = "spc";
        private const string HOOT_DRIVER = "snes";
        private const string HOOT_CHIP = "SPC700";

        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x21;

        private const int DUMMY_26_OFFSET = 0x21;
        private const int DUMMY_26_LENGTH = 0x02;

        private const int HEADER_HAS_ID666_OFFSET = 0x23;
        private const int HEADER_HAS_ID666_LENGTH = 0x01;

        private const int VERSION_MINOR_OFFSET = 0x24;
        private const int VERSION_MINOR_LENGTH = 0x01;

        private const int REGISTER_PC_OFFSET = 0x25;
        private const int REGISTER_PC_LENGTH = 0x02;

        private const int REGISTER_A_OFFSET = 0x27;
        private const int REGISTER_A_LENGTH = 0x01;

        private const int REGISTER_X_OFFSET = 0x28;
        private const int REGISTER_X_LENGTH = 0x01;

        private const int REGISTER_Y_OFFSET = 0x29;
        private const int REGISTER_Y_LENGTH = 0x01;

        private const int REGISTER_PSW_OFFSET = 0x2A;
        private const int REGISTER_PSW_LENGTH = 0x01;

        private const int REGISTER_SP_OFFSET = 0x2B;
        private const int REGISTER_SP_LENGTH = 0x01;

        private const int RESERVED_OFFSET = 0x2C;
        private const int RESERVED_LENGTH = 0x02;

        private const int ID666_OFFSET = 0x2E;
        private const int ID666_LENGTH = 0xD2;

        private const int RAM_64K_OFFSET = 0x100;
        private const int RAM_64K_LENGTH = 0x10000;

        private const int DSP_REGISTERS_OFFSET = 0x10100;
        private const int DSP_REGISTERS_LENGTH = 0x80;

        private const int UNUSED_OFFSET = 0x10180;
        private const int UNUSED_LENGTH = 0x40;

        private const int EXTRA_RAM_OFFSET = 0x101C0;
        private const int EXTRA_RAM_LENGTH = 0x40;

        private const int EXTENDED_INFO_OFFSET = 0x10200;

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private byte[] asciiSignature;
        private byte[] dummy26;
        private byte[] headerHasId666;
        private byte[] versionMinor;
        private byte[] registerPc;
        private byte[] registerA;
        private byte[] registerX;
        private byte[] registerY;
        private byte[] registerPSW;
        private byte[] registerSP;
        private byte[] reserved;
        private byte[] id666;
        private byte[] ram64k;
        private byte[] dspRegisters;
        private byte[] unused;
        private byte[] extraRam;
        private byte[] extendedInfo;

        Dictionary<string, string> tagHash = new Dictionary<string, string>();
        Dictionary<string, string> exId666Hash = new Dictionary<string, string>();

        public byte[] AsciiSignature { get { return this.asciiSignature; } }
        public byte[] Dummy26 { get { return this.dummy26; } }
        public byte[] HeaderHasId666 { get { return this.headerHasId666; } }
        public byte[] VersionMinor { get { return this.versionMinor; } }
        public byte[] RegisterPc { get { return this.registerPc; } }
        public byte[] RegisterA { get { return this.registerA; } }
        public byte[] RegisterX { get { return this.registerX; } }
        public byte[] RegisterY { get { return this.registerY; } }
        public byte[] RegisterPSW { get { return this.registerPSW; } }
        public byte[] RegisterSP { get { return this.registerSP; } }
        public byte[] Reserved { get { return this.reserved; } }
        public byte[] Id666 { get { return this.id666; } }
        public byte[] Ram64k { get { return this.ram64k; } }
        public byte[] DspRegisters { get { return this.dspRegisters; } }
        public byte[] Unused { get { return this.unused; } }
        public byte[] ExtraRam { get { return this.extraRam; } }
        public byte[] ExtendedInfo { get { return this.extendedInfo; } }

        // ID666
        private const int ID_SONG_TITLE_OFFSET = 0x00;
        private const int ID_SONG_TITLE_LENGTH = 0x20;

        private const int ID_GAME_TITLE_OFFSET = 0x20;
        private const int ID_GAME_TITLE_LENGTH = 0x20;

        private const int ID_NAME_OF_DUMPER_OFFSET = 0x40;
        private const int ID_NAME_OF_DUMPER_LENGTH = 0x10;

        private const int ID_COMMENTS_OFFSET = 0x50;
        private const int ID_COMMENTS_LENGTH = 0x20;

        private const int ID_DUMP_DATE_OFFSET = 0x70;
        private const int ID_DUMP_DATE_LENGTH = 0x0B;

        private const int ID_NUM_SECS_TO_PLAY_OFFSET = 0x7B;
        private const int ID_NUM_SECS_TO_PLAY_LENGTH = 0x03;

        private const int ID_LENGTH_OF_FADE_OFFSET = 0x7E;
        private const int ID_LENGTH_OF_FADE_LENGTH = 0x05;

        private const int ID_SONG_ARTIST_OFFSET = 0x83;
        private const int ID_SONG_ARTIST_LENGTH = 0x20;

        private const int ID_DEFAULT_CHAN_DISAB_OFFSET = 0xA3;
        private const int ID_DEFAULT_CHAN_DISAB_LENGTH = 0x01;

        private const int ID_EMULATOR_USED_OFFSET = 0xA4;
        private const int ID_EMULATOR_USED_LENGTH = 0x01;

        private const int ID_RESERVED_OFFSET = 0xA5;
        private const int ID_RESERVED_LENGTH = 0x2D;                
       
        private byte[] id_songTitle;
        private byte[] id_gameTitle;
        private byte[] id_nameOfDumper;
        private byte[] id_comments;
        private byte[] id_dumpDate;
        private byte[] id_numSecondsToPlay;
        private byte[] id_lengthOfFade;
        private byte[] id_songArtist;
        private byte[] id_defaultChanDisab;
        private byte[] id_emulatorUsed;
        private byte[] id_reserved;

        public byte[] IdSongTitle { get { return this.id_songTitle; } }
        public byte[] IdGameTitle { get { return this.id_gameTitle; } }
        public byte[] IdNameOfDumper { get { return this.id_nameOfDumper; } }
        public byte[] IdComments { get { return this.id_comments; } }        
        public byte[] IdDumpDate { get { return this.id_songTitle; } }
        public byte[] IdNumSecondsToPlay { get { return this.id_songTitle; } }
        public byte[] IdLengthOfFade { get { return this.id_songTitle; } }
        public byte[] IdSongArtist { get { return this.id_songTitle; } }
        public byte[] IdDefaultChanDisab { get { return this.id_songTitle; } }
        public byte[] IdEmulatorUsed { get { return this.id_songTitle; } }
        public byte[] IdReserved { get { return this.id_songTitle; } }

        // EXID666 - CHUNK HEADER
        private const int EX_ID666_HEADER_CHUNK_SIZE_OFFSET = 0x04;
        private const int EX_ID666_HEADER_CHUNK_SIZE_LENGTH = 0x04;

        private const int EX_ID666_CHUNK_DATA_OFFSET = 0x08;

        private byte[] exidHeaderChunkSize;
        private byte[] exidFullChunk;

        public byte[] ExidHeaderChunkSize { get { return this.exidHeaderChunkSize; } }
        
        // EXID666 - SUBCHUNK
        private const int EX_ID666_SUBCHUNK_ID_OFFSET = 0x00;
        private const int EX_ID666_SUBCHUNK_ID_LENGTH = 0x01;

        private const int EX_ID666_SUBCHUNK_TYPE_OFFSET = 0x01;
        private const int EX_ID666_SUBCHUNK_TYPE_LENGTH = 0x01;

        private const int EX_ID666_SUBCHUNK_LENGTH_OFFSET = 0x02;
        private const int EX_ID666_SUBCHUNK_LENGTH_LENGTH = 0x02;

        // private byte[] exidSubChunkId;
        // private byte[] exidSubChunkType;
        // private byte[] exidSubChunkLength;

        #region METHODS

        public byte[] getAsciiSignature(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
        }
        public byte[] getDummy26(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, DUMMY_26_OFFSET, DUMMY_26_LENGTH);
        }
        public byte[] getHeaderHasId666(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, HEADER_HAS_ID666_OFFSET, HEADER_HAS_ID666_LENGTH);
        }
        public byte[] getVersionMinor(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, VERSION_MINOR_OFFSET, VERSION_MINOR_LENGTH);
        }
        public byte[] getRegisterPc(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, REGISTER_PC_OFFSET, REGISTER_PC_LENGTH);
        }
        public byte[] getRegisterA(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, REGISTER_A_OFFSET, REGISTER_A_LENGTH);
        }
        public byte[] getRegisterX(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, REGISTER_X_OFFSET, REGISTER_X_LENGTH);
        }
        public byte[] getRegisterY(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, REGISTER_Y_OFFSET, REGISTER_Y_LENGTH);
        }
        public byte[] getRegisterPSW(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, REGISTER_PSW_OFFSET, REGISTER_PSW_LENGTH);
        }
        public byte[] getRegisterSP(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, REGISTER_SP_OFFSET, REGISTER_SP_LENGTH);
        }
        public byte[] getReserved(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, RESERVED_OFFSET, RESERVED_LENGTH);
        }
        public byte[] getId666(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, ID666_OFFSET, ID666_LENGTH);
        }
        public byte[] getRam64k(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, RAM_64K_OFFSET, RAM_64K_LENGTH);
        }
        public byte[] getDspRegisters(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, DSP_REGISTERS_OFFSET, DSP_REGISTERS_LENGTH);
        }
        public byte[] getUnused(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, UNUSED_OFFSET, UNUSED_LENGTH);
        }
        public byte[] getExtraRam(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, EXTRA_RAM_OFFSET, EXTRA_RAM_LENGTH);
        }
        public byte[] getExtendedInfo(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, EXTENDED_INFO_OFFSET, 
                (int)(pStream.Length - EXTENDED_INFO_OFFSET));
        }

        /* ID666 */
        public byte[] getIdSongTitle(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_SONG_TITLE_OFFSET, ID_SONG_TITLE_LENGTH);
        }
        public byte[] getIdGameTitle(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_GAME_TITLE_OFFSET, ID_GAME_TITLE_LENGTH);
        }
        public byte[] getIdNameOfDumper(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_NAME_OF_DUMPER_OFFSET, ID_NAME_OF_DUMPER_LENGTH);
        }
        public byte[] getIdComments(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_COMMENTS_OFFSET, ID_COMMENTS_LENGTH);
        }
        public byte[] getIdDumpDate(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_DUMP_DATE_OFFSET, ID_DUMP_DATE_LENGTH);
        }
        public byte[] getIdNumSecondsToPlay(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_NUM_SECS_TO_PLAY_OFFSET, ID_NUM_SECS_TO_PLAY_LENGTH);
        }
        public byte[] getIdLengthOfFade(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_LENGTH_OF_FADE_OFFSET, ID_LENGTH_OF_FADE_LENGTH);
        }
        public byte[] getIdSongArtist(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_SONG_ARTIST_OFFSET, ID_SONG_ARTIST_LENGTH);
        }
        public byte[] getIdDefaultChanDisab(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_DEFAULT_CHAN_DISAB_OFFSET, ID_DEFAULT_CHAN_DISAB_LENGTH);
        }
        public byte[] getIdEmulatorUsed(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_EMULATOR_USED_OFFSET, ID_EMULATOR_USED_LENGTH);
        }
        public byte[] getIdReserved(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, ID_RESERVED_OFFSET, ID_RESERVED_LENGTH);
        }
        
        /* EXID666 CHUNK */
        public byte[] getExidHeaderChunkSize(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, EX_ID666_HEADER_CHUNK_SIZE_OFFSET, EX_ID666_HEADER_CHUNK_SIZE_LENGTH);
        }

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.filePath = pFilePath;
            this.asciiSignature = this.getAsciiSignature(pStream);
            this.dummy26 = this.getDummy26(pStream);
            this.headerHasId666 = this.getHeaderHasId666(pStream);
            this.versionMinor = this.getVersionMinor(pStream);
            this.registerPc = this.getRegisterPc(pStream);
            this.registerA = this.getRegisterA(pStream);
            this.registerX = this.getRegisterX(pStream);
            this.registerY = this.getRegisterY(pStream);
            this.registerPSW = this.getRegisterPSW(pStream);
            this.registerSP = this.getRegisterSP(pStream);
            this.reserved = this.getReserved(pStream);

            // ID666
            if (ParseFile.CompareSegment(this.headerHasId666, 0, ID666_IN_HEADER_VAL))
            {
                this.id666 = this.getId666(pStream);
                
                this.id_songTitle = this.getIdSongTitle(this.id666);
                this.id_gameTitle = this.getIdGameTitle(this.id666);
                this.id_nameOfDumper = this.getIdNameOfDumper(this.id666);
                this.id_comments = this.getIdComments(this.id666);
                this.id_dumpDate = this.getIdDumpDate(this.id666);
                this.id_numSecondsToPlay = this.getIdNumSecondsToPlay(this.id666);
                this.id_lengthOfFade = this.getIdLengthOfFade(this.id666);
                this.id_songArtist = this.getIdSongArtist(this.id666);
                this.id_defaultChanDisab = this.getIdDefaultChanDisab(this.id666);
                this.id_emulatorUsed = this.getIdEmulatorUsed(this.id666);
                this.id_reserved = this.getIdReserved(this.id666);
            }
            
            this.ram64k = this.getRam64k(pStream);
            this.dspRegisters = this.getDspRegisters(pStream);
            this.unused = this.getUnused(pStream);
            this.extraRam = this.getExtraRam(pStream);
            this.extendedInfo = this.getExtendedInfo(pStream);

            this.initializeTagHash();
            this.initializeExId666Hash();

            // Extended ID666
            if (this.extendedInfo.Length > 0 &&
                ParseFile.CompareSegment(this.extendedInfo, 0, EXTENDED_ID666_SIGNATURE))
            {
                exidHeaderChunkSize = this.getExidHeaderChunkSize(this.extendedInfo);
                if (BitConverter.ToInt32(exidHeaderChunkSize, 0) > 0)
                {
                    exidFullChunk = ParseFile.ParseSimpleOffset(this.extendedInfo, EX_ID666_CHUNK_DATA_OFFSET, BitConverter.ToInt32(exidHeaderChunkSize, 0));
                }
                this.parseExtendedChunk(exidFullChunk);
            }
        }

        private void initializeTagHash()
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            
            tagHash.Add("Game Name", enc.GetString(this.id_gameTitle));
            tagHash.Add("Song Name", enc.GetString(this.id_songTitle));
            tagHash.Add("Artist", enc.GetString(this.id_songArtist));
            tagHash.Add("Dumper", enc.GetString(this.id_nameOfDumper));
            tagHash.Add("Comments", enc.GetString(this.id_comments));
        }

        private void initializeExId666Hash()
        {
            exId666Hash.Add("01", "Song Name");
            exId666Hash.Add("02", "Game Name");
            exId666Hash.Add("03", "Artist");
            exId666Hash.Add("04", "Dumper");
            exId666Hash.Add("05", "Dump Date");
            exId666Hash.Add("06", "Emulator Used");
            exId666Hash.Add("07", "Comments");
            exId666Hash.Add("10", "Official Soundtrack Title");
            exId666Hash.Add("11", "OST Disc");
            exId666Hash.Add("12", "OST Track"); // Need to Parse
            exId666Hash.Add("13", "Publisher");
            exId666Hash.Add("14", "Copyright Year");           
        }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();

            pChecksum.Update(dummy26);
            pChecksum.Update(headerHasId666);
            pChecksum.Update(versionMinor);
            pChecksum.Update(registerPc);
            pChecksum.Update(registerA);
            pChecksum.Update(registerX);
            pChecksum.Update(registerY);
            pChecksum.Update(registerPSW);
            pChecksum.Update(registerSP);
            pChecksum.Update(ram64k);
            pChecksum.Update(dspRegisters);
            pChecksum.Update(extraRam);
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            pChecksum.Update(dummy26);
            pChecksum.Update(headerHasId666);
            pChecksum.Update(versionMinor);
            pChecksum.Update(registerPc);
            pChecksum.Update(registerA);
            pChecksum.Update(registerX);
            pChecksum.Update(registerY);
            pChecksum.Update(registerPSW);
            pChecksum.Update(registerSP);
            pChecksum.Update(ram64k);
            pChecksum.Update(dspRegisters);
            pChecksum.Update(extraRam);

            pMd5CryptoStream.Write(dummy26, 0, dummy26.Length);
            pMd5CryptoStream.Write(headerHasId666, 0, headerHasId666.Length);
            pMd5CryptoStream.Write(versionMinor, 0, versionMinor.Length);
            pMd5CryptoStream.Write(registerPc, 0, registerPc.Length);
            pMd5CryptoStream.Write(registerA, 0, registerA.Length);
            pMd5CryptoStream.Write(registerX, 0, registerX.Length);
            pMd5CryptoStream.Write(registerY, 0, registerY.Length);
            pMd5CryptoStream.Write(registerPSW, 0, registerPSW.Length);
            pMd5CryptoStream.Write(registerSP, 0, registerSP.Length);
            pMd5CryptoStream.Write(ram64k, 0, ram64k.Length);
            pMd5CryptoStream.Write(dspRegisters, 0, dspRegisters.Length);
            pMd5CryptoStream.Write(extraRam, 0, extraRam.Length);

            pSha1CryptoStream.Write(dummy26, 0, dummy26.Length);
            pSha1CryptoStream.Write(headerHasId666, 0, headerHasId666.Length);
            pSha1CryptoStream.Write(versionMinor, 0, versionMinor.Length);
            pSha1CryptoStream.Write(registerPc, 0, registerPc.Length);
            pSha1CryptoStream.Write(registerA, 0, registerA.Length);
            pSha1CryptoStream.Write(registerX, 0, registerX.Length);
            pSha1CryptoStream.Write(registerY, 0, registerY.Length);
            pSha1CryptoStream.Write(registerPSW, 0, registerPSW.Length);
            pSha1CryptoStream.Write(registerSP, 0, registerSP.Length);
            pSha1CryptoStream.Write(ram64k, 0, ram64k.Length);
            pSha1CryptoStream.Write(dspRegisters, 0, dspRegisters.Length);
            pSha1CryptoStream.Write(extraRam, 0, extraRam.Length);
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

        private void parseExtendedChunk(byte[] pBytes)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            // EXID666 - SUBCHUNK
            byte[] exidSubChunkId;
            byte[] exidSubChunkType;
            byte[] exidSubChunkLength;

            string exidSubChunkIntId;
            int exidSubChunkIntLength;            
            // int exidSubChunkRemainder;

            string ostTrackNo;

            int offset = 0;
            while (offset < pBytes.Length)
            {
                exidSubChunkId = ParseFile.ParseSimpleOffset(pBytes, offset + EX_ID666_SUBCHUNK_ID_OFFSET, EX_ID666_SUBCHUNK_ID_LENGTH);
                exidSubChunkIntId = BitConverter.ToString(exidSubChunkId, 0);

                exidSubChunkType = ParseFile.ParseSimpleOffset(pBytes, offset + EX_ID666_SUBCHUNK_TYPE_OFFSET, EX_ID666_SUBCHUNK_TYPE_LENGTH);
                exidSubChunkLength = ParseFile.ParseSimpleOffset(pBytes, offset + EX_ID666_SUBCHUNK_LENGTH_OFFSET, EX_ID666_SUBCHUNK_LENGTH_LENGTH);                
                exidSubChunkIntLength = BitConverter.ToInt16(exidSubChunkLength, 0);
                
                // LENGTH
                if (!exId666Hash.ContainsKey(exidSubChunkIntId))
                {
                    offset += EX_ID666_SUBCHUNK_ID_LENGTH + EX_ID666_SUBCHUNK_TYPE_LENGTH +
                        EX_ID666_SUBCHUNK_LENGTH_LENGTH + (int)exidSubChunkIntLength;                
                }
                else if (ParseFile.CompareSegment(exidSubChunkType, 0, EXID666_SUBCHUNK_TYPE_LENGTH))
                {
                    if (tagHash.ContainsKey(exId666Hash[exidSubChunkIntId]))
                    {
                        tagHash.Remove(exId666Hash[exidSubChunkIntId]);
                    }

                    if (exidSubChunkIntId.Equals(EXID666_SUBCHUNK_ID_OST_TRACK))
                    {
                        Array.Reverse(exidSubChunkLength);
                        ostTrackNo = ParseFile.ByteArrayToString(exidSubChunkLength);
                        if (ostTrackNo.Substring(2).Equals("00"))
                        {
                            ostTrackNo = ostTrackNo.Substring(0, 2);
                        }

                        tagHash.Add(exId666Hash[exidSubChunkIntId], ostTrackNo);                        
                    }
                    else
                    {
                        tagHash.Add(exId666Hash[exidSubChunkIntId], System.BitConverter.ToInt16(exidSubChunkLength, 0).ToString());
                    }

                    offset += EX_ID666_SUBCHUNK_ID_LENGTH + EX_ID666_SUBCHUNK_TYPE_LENGTH +
                        EX_ID666_SUBCHUNK_LENGTH_LENGTH;                
                }
                
                // STRING
                else if (ParseFile.CompareSegment(exidSubChunkType, 0, EXID666_SUBCHUNK_TYPE_STRING))
                {                    
                    int stringStartOffset = offset + EX_ID666_SUBCHUNK_LENGTH_OFFSET + EX_ID666_SUBCHUNK_LENGTH_LENGTH;

                    // setup for 32-bit align
                    //exidSubChunkRemainder = (EXTENDED_INFO_OFFSET + EX_ID666_CHUNK_DATA_OFFSET + stringStartOffset + exidSubChunkIntLength) % 4;
                    
                    //if (exidSubChunkRemainder != 0)
                    //{
                    //    exidSubChunkIntLength += exidSubChunkRemainder;
                    //}
                                        
                    byte[] subChunkData = ParseFile.ParseSimpleOffset(pBytes, stringStartOffset, (int) exidSubChunkIntLength);

                    if (tagHash.ContainsKey(exId666Hash[exidSubChunkIntId]))
                    {
                        tagHash.Remove(exId666Hash[exidSubChunkIntId]);
                    }
                    tagHash.Add(exId666Hash[exidSubChunkIntId], enc.GetString(subChunkData));

                    offset += EX_ID666_SUBCHUNK_ID_LENGTH + EX_ID666_SUBCHUNK_TYPE_LENGTH +
                        EX_ID666_SUBCHUNK_LENGTH_LENGTH + (int) exidSubChunkIntLength;                
                }

                // INTEGER
                else if (ParseFile.CompareSegment(exidSubChunkType, 0, EXID666_SUBCHUNK_TYPE_INTEGER))
                {
                    if (exidSubChunkIntId.Equals(EXID666_SUBCHUNK_ID_DUMPDATE))
                    {
                        int intStartOffset = offset + EX_ID666_SUBCHUNK_LENGTH_OFFSET + EX_ID666_SUBCHUNK_LENGTH_LENGTH;
                        byte[] subChunkData = ParseFile.ParseSimpleOffset(pBytes, intStartOffset, (int)exidSubChunkIntLength);
                        
                        ostTrackNo = ParseFile.ByteArrayToString(exidSubChunkLength);
                        tagHash.Add(exId666Hash[exidSubChunkIntId], ParseFile.ByteArrayToString(subChunkData));
                    }
                    
                    
                    offset += EX_ID666_SUBCHUNK_ID_LENGTH + EX_ID666_SUBCHUNK_TYPE_LENGTH +
                        EX_ID666_SUBCHUNK_LENGTH_LENGTH + (int)exidSubChunkIntLength;
                }
            } // while (offset < pBytes.GetLength())

        }

        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }

        #endregion

        #region HOOT

        public int GetStartingSong() { return -1; }
        public int GetTotalSongs() { return 1; }
        public string GetSongName() 
        { 
            return ByteConversion.GetEncodedText(
                    FileUtil.ReplaceNullByteWithSpace(this.IdSongTitle), 
                    ByteConversion.GetPredictedCodePageForTags(this.IdSongTitle)).Trim();
        }

        public bool UsesPlaylist() { return false; }
        public NezPlugM3uEntry[] GetPlaylistEntries() { return null; }

        public bool HasMultipleFilesPerSet() { return true; }
        public string GetSetName()
        {
            return ByteConversion.GetEncodedText(
                FileUtil.ReplaceNullByteWithSpace(this.IdGameTitle),
                ByteConversion.GetPredictedCodePageForTags(this.IdGameTitle)).Trim();
        }

        public string GetHootDriverAlias() { return HOOT_DRIVER_ALIAS; }
        public string GetHootDriverType() { return HOOT_DRIVER_TYPE; }
        public string GetHootDriver() { return HOOT_DRIVER; }
        public string GetHootChips() { return HOOT_CHIP; }

        #endregion
    }
}
