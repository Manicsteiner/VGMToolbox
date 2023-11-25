using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.GZip;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Vgm : IFormat, IGd3TagFormat
    {
        public Vgm() { }
        
        // Constants
        private static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x56, 0x67, 0x6d, 0x20 }; // "Vgm "
        private static readonly byte[] ASCII_SIGNATURE_GD3 = new byte[] { 0x47, 0x64, 0x33, 0x20 }; // "Gd3 "


        private const string TYPE_VGM = "VGM";
        private const string TYPE_VGM7Z = "VGM7Z";
        private const string TYPE_VGZ = "VGZ";

        private static readonly byte[] VERSION_0100 = new byte[] { 0x00, 0x01, 0x00, 0x00 };
        private static readonly byte[] VERSION_0101 = new byte[] { 0x01, 0x01, 0x00, 0x00 };
        private static readonly byte[] VERSION_0110 = new byte[] { 0x10, 0x01, 0x00, 0x00 };
        private static readonly byte[] VERSION_0150 = new byte[] { 0x50, 0x01, 0x00, 0x00 };
        private static readonly byte[] VERSION_0151 = new byte[] { 0x51, 0x01, 0x00, 0x00 };
        private static readonly byte[] VERSION_0160 = new byte[] { 0x60, 0x01, 0x00, 0x00 };
        private static readonly byte[] VERSION_0161 = new byte[] { 0x61, 0x01, 0x00, 0x00 };
        private static readonly byte[] VERSION_0170 = new byte[] { 0x70, 0x01, 0x00, 0x00 };

        private const int INT_VERSION_UNKNOWN = -1;
        private const int INT_VERSION_0100 = 100;
        private const int INT_VERSION_0101 = 101;
        private const int INT_VERSION_0110 = 110;
        private const int INT_VERSION_0150 = 150;
        private const int INT_VERSION_0151 = 151;
        private const int INT_VERSION_0160 = 160;
        private const int INT_VERSION_0161 = 161;
        private const int INT_VERSION_0170 = 170;

        private static readonly byte[] VERSION_GD3_0100 = new byte[] { 0x00, 0x01, 0x00, 0x00 };

        // Version 1.0 or greater
        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x04;

        private const int EOF_OFFSET_OFFSET = 0x04;
        private const int EOF_OFFSET_LENGTH = 0x04;

        private const int VERSION_OFFSET = 0x08;
        private const int VERSION_LENGTH = 0x04;

        private const int SN76489_CLOCK_OFFSET = 0x0C;
        private const int SN76489_CLOCK_LENGTH = 0x04;

        private const int YM2413_CLOCK_OFFSET = 0x10;
        private const int YM2413_CLOCK_LENGTH = 0x04;

        private const int GD3_OFFSET_OFFSET = 0x14;
        private const int GD3_OFFSET_LENGTH = 0x04;

        private const int TOTAL_NUM_SAMPLES_OFFSET = 0x18;
        private const int TOTAL_NUM_SAMPLES_LENGTH = 0x04;

        private const int LOOP_OFFSET_OFFSET = 0x1C;
        private const int LOOP_OFFSET_LENGTH = 0x04;

        private const int LOOP_NUM_SAMPLES_OFFSET = 0x20;
        private const int LOOP_NUM_SAMPLES_LENGTH = 0x04;

        private const int VGM_DATA_OFFSET_PRE150 = 0x40;

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private byte[] signatureTag;
        private byte[] eofOffset;
        private byte[] version;
        private byte[] sn76489Clock;
        private byte[] ym2413Clock;
        private byte[] gd3Offset;
        private byte[] totalNumOfSamples;
        private byte[] loopOffset;
        private byte[] loopNumOfSamples;
        private byte[] vgmData;

        public byte[] SignatureTag { get { return this.signatureTag; } }
        public byte[] EofOffset { get { return this.eofOffset; } }
        public byte[] Version { get { return this.version; } }
        public byte[] Sn76489Clock { get { return this.sn76489Clock; } }
        public byte[] Ym2413Clock { get { return this.ym2413Clock; } }
        public byte[] Gd3Offset { get { return this.gd3Offset; } }
        public byte[] TotalNumOfSamples { get { return this.totalNumOfSamples; } }
        public byte[] LoopOffset { get { return this.loopOffset; } }
        public byte[] LoopNumOfSamples { get { return this.loopNumOfSamples; } }
        public byte[] VgmData { get { return this.vgmData; } }

        // All
        public uint FullHeaderSize { set; get; }
        public byte[] FullHeader { set; get; }

        // Version 1.01 or greater
        private const int RATE_OFFSET = 0x24;
        private const int RATE_LENGTH = 0x04;

        private byte[] rate;

        public byte[] Rate { get { return this.rate; } }

        // Version 1.10 or greater
        private const int SN76489_FEEDBACK_OFFSET = 0x28;
        private const int SN76489_FEEDBACK_LENGTH = 0x02;

        private const int SN76489_SRW_OFFSET = 0x2A;
        private const int SN76489_SRW_LENGTH = 0x01;

        private const int RESERVED1_OFFSET = 0x2B;
        private const int RESERVED1_LENGTH = 0x01;

        private const int YM2612_CLOCK_OFFSET = 0x2C;
        private const int YM2612_CLOCK_LENGTH = 0x04;

        private const int YM2151_CLOCK_OFFSET = 0x30;
        private const int YM2151_CLOCK_LENGTH = 0x04;

        private byte[] sn76489Feedback;
        private byte[] sn76489Srw;
        private byte[] ym2612Clock;
        private byte[] ym2151Clock;

        public byte[] Sn76489Feedback { get { return this.sn76489Feedback; } }
        public byte[] Sn76489Srw { get { return this.sn76489Srw; } }
        public byte[] Ym2612Clock { get { return this.ym2612Clock; } }
        public byte[] Ym2151Clock { get { return this.ym2151Clock; } }

        // Version 1.50 or greater        
        private const int VGM_DATA_OFFSET_OFFSET = 0x34;
        private const int VGM_DATA_OFFSET_LENGTH = 0x04;

        private const int RESERVED2_OFFSET = 0x38;
        private const int RESERVED2_LENGTH = 0x08;

        private byte[] vgmDataOffset;
        
        public byte[] VgmDataOffset { get { return this.vgmDataOffset; } }

        // Version 1.51 or greater
        private const int SN76489_FLAGS_OFFSET = 0x2B;
        private const int SN76489_FLAGS_LENGTH = 0x01;

        private const int SPCM_CLOCK_OFFSET = 0x38;
        private const int SPCM_CLOCK_LENGTH = 0x04;

        private const int SPCM_INTERFACE_OFFSET = 0x3C;
        private const int SPCM_INTERFACE_LENGTH = 0x04;

        private const int RF5C68_CLOCK_OFFSET = 0x40;
        private const int RF5C68_CLOCK_LENGTH = 0x04;

        private const int YM2203_CLOCK_OFFSET = 0x44;
        private const int YM2203_CLOCK_LENGTH = 0x04;

        private const int YM2608_CLOCK_OFFSET = 0x48;
        private const int YM2608_CLOCK_LENGTH = 0x04;

        private const int YM2610_CLOCK_OFFSET = 0x4C;
        private const int YM2610_CLOCK_LENGTH = 0x04;
        
        private const int YM3812_CLOCK_OFFSET = 0x50;
        private const int YM3812_CLOCK_LENGTH = 0x04;

        private const int YM3526_CLOCK_OFFSET = 0x54;
        private const int YM3526_CLOCK_LENGTH = 0x04;

        private const int Y8950_CLOCK_OFFSET = 0x58;
        private const int Y8950_CLOCK_LENGTH = 0x04;

        private const int YMF262_CLOCK_OFFSET = 0x5C;
        private const int YMF262_CLOCK_LENGTH = 0x04;

        private const int YMF278B_CLOCK_OFFSET = 0x60;
        private const int YMF278B_CLOCK_LENGTH = 0x04;

        private const int YMF271_CLOCK_OFFSET = 0x64;
        private const int YMF271_CLOCK_LENGTH = 0x04;

        private const int YMZ280B_CLOCK_OFFSET = 0x68;
        private const int YMZ280B_CLOCK_LENGTH = 0x04;

        private const int RF5C164_CLOCK_OFFSET = 0x6C;
        private const int RF5C164_CLOCK_LENGTH = 0x04;

        private const int PWM_CLOCK_OFFSET = 0x70;
        private const int PWM_CLOCK_LENGTH = 0x04;

        private const int AY8910_CLOCK_OFFSET = 0x74;
        private const int AY8910_CLOCK_LENGTH = 0x04;

        private const int AY8910_CHIP_TYPE_OFFSET = 0x78;
        private const int AY8910_CHIP_TYPE_LENGTH = 0x01;

        private const int AY8910_FLAGS_OFFSET = 0x79;
        private const int AY8910_FLAGS_LENGTH = 0x01;

        private const int AY8910_YM2203_FLAGS_OFFSET = 0x7A;
        private const int AY8910_YM2203_FLAGS_LENGTH = 0x01;

        private const int AY8910_YM2610_FLAGS_OFFSET = 0x7B;
        private const int AY8910_YM2610_FLAGS_LENGTH = 0x01;

        private const int LOOP_MODIFIER_OFFSET = 0x7F;
        private const int LOOP_MODIFIER_LENGTH = 0x01;

        private byte[] sn76489Flags;
        private byte[] spcmClock;
        private byte[] spcmInterface;

        public byte[] Sn76489Flags { get { return this.sn76489Flags; } }
        public byte[] SpcmClock { get { return this.spcmClock; } }
        public byte[] SpcmInterface { get { return this.SpcmInterface; } }
        
        public byte[] Rf5c68Clock { set; get; }
        public byte[] Ym2203Clock { set; get; }
        public byte[] Ym2608Clock { set; get; }
        public byte[] Ym2610Clock { set; get; }

        public byte[] Ym3812Clock { set; get; }
        public byte[] Ym3526Clock { set; get; }
        public byte[] Y8950Clock { set; get; }
        public byte[] Ymf262Clock { set; get; }

        public byte[] Ymf278BClock { set; get; }
        public byte[] Ymf271Clock { set; get; }
        public byte[] Ymz280BClock { set; get; }

        public byte[] Rf5C164Clock { set; get; }
        public byte[] PwmClock { set; get; }
        public byte[] Ay8910Clock { set; get; }
        public byte[] Ay8910ChipType { set; get; }
        public byte[] Ay8910Flags { set; get; }
        public byte[] Ay8910Ym2203Flags { set; get; }
        public byte[] Ay8910Ym2610Flags { set; get; }

        // v1.60 vals not added

        public byte[] LoopModifier { set; get; }


        // GD3 TAG
        private const int GD3_SIGNATURE_OFFSET = 0x00;
        private const int GD3_SIGNATURE_LENGTH = 0x04;
        
        private const int GD3_VERSION_OFFSET = 0x04;
        private const int GD3_VERSION_LENGTH = 0x04;
        
        private const int GD3_DATA_LENGTH_OFFSET = 0x08;
        private const int GD3_DATA_LENGTH_LENGTH = 0x04;

        private byte[] gd3Signature;
        private byte[] gd3Version;
        private byte[] gd3DataLength;

        private const int GD3_TRACK_NAME_E_IDX = 0;
        private const int GD3_TRACK_NAME_J_IDX = 1;
        private const int GD3_GAME_NAME_E_IDX = 2;
        private const int GD3_GAME_NAME_J_IDX = 3;
        private const int GD3_SYSTEM_NAME_E_IDX = 4;
        private const int GD3_SYSTEM_NAME_J_IDX = 5;
        private const int GD3_AUTHOR_NAME_E_IDX = 6;
        private const int GD3_AUTHOR_NAME_J_IDX = 7;
        private const int GD3_GAME_DATE_IDX = 8;
        private const int GD3_RIPPER_IDX = 9;
        private const int GD3_NOTES = 10;

        // Others
        Dictionary<string, string> tagHash = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private int vgmDataAbsoluteOffset;
        private int gd3AbsoluteOffset;
        private int eofAbsoluteOffset;

        // Methods        

        #region BYTE FUNCTIONS
        protected byte[] getEofOffset(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, EOF_OFFSET_OFFSET, EOF_OFFSET_LENGTH);
        }

        protected byte[] getGd3Offset(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, GD3_OFFSET_OFFSET, GD3_OFFSET_LENGTH);
        }

        protected byte[] getLoopNumOfSamples(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, LOOP_NUM_SAMPLES_OFFSET, LOOP_NUM_SAMPLES_LENGTH);
        }

        protected byte[] getLoopOffset(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, LOOP_OFFSET_OFFSET, LOOP_OFFSET_LENGTH);
        }

        /*
        private void getPackedType(FileStream pFileStream)
        {
            this.fileType = checkPackedType(pFileStream, FileUtil.GZIP_SIGNATURE, "VGZ");
            if (this.fileType == null)
            {
                this.fileType = checkPackedType(pFileStream, ASCII_SIGNATURE, "VGM");
            }
            if (this.fileType == null)
            {
                this.fileType = checkPackedType(pFileStream, FileUtil.SEVEN_ZIP_SIGNATURE, "VGM7Z");
            }
        }
        */
        
        protected byte[] getRate(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, RATE_OFFSET, RATE_LENGTH);
        }

        protected byte[] getRf5c68Clock(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, RF5C68_CLOCK_OFFSET, RF5C68_CLOCK_LENGTH);
        }

        protected byte[] getSignatureTag(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SIG_OFFSET, SIG_LENGTH);
        }

        protected byte[] getSn76489Clock(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SN76489_CLOCK_OFFSET, SN76489_CLOCK_LENGTH);
        }

        protected byte[] getSn76489Feedback(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SN76489_FEEDBACK_OFFSET, SN76489_FEEDBACK_LENGTH);
        }

        protected byte[] getSn76489Srw(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SN76489_SRW_OFFSET, SN76489_SRW_LENGTH);
        }

        protected byte[] getSn76489Flags(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SN76489_FLAGS_OFFSET, SN76489_FLAGS_LENGTH);
        }

        protected byte[] getSpcmClock(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SPCM_CLOCK_OFFSET, SPCM_CLOCK_LENGTH);
        }

        protected byte[] getSpcmInterface(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, SPCM_INTERFACE_OFFSET, SPCM_INTERFACE_LENGTH);
        }

        protected byte[] getTotalNumOfSamples(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, TOTAL_NUM_SAMPLES_OFFSET, TOTAL_NUM_SAMPLES_LENGTH);
        }

        protected byte[] getVersion(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, VERSION_OFFSET, VERSION_LENGTH);
        }

        protected byte[] getVgmData(byte[] pBytes)
        {
            // NEED TO FIX
            return ParseFile.ParseSimpleOffset(pBytes, (VGM_DATA_OFFSET_OFFSET + BitConverter.ToInt32(this.getVgmDataOffset(pBytes), 0)), 4);
        }

        protected byte[] getVgmDataOffset(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, VGM_DATA_OFFSET_OFFSET, VGM_DATA_OFFSET_LENGTH);
        }

        protected byte[] getYm2151Clock(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, YM2151_CLOCK_OFFSET, YM2151_CLOCK_LENGTH);
        }

        protected byte[] getYm2413Clock(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, YM2413_CLOCK_OFFSET, YM2413_CLOCK_LENGTH);
        }

        protected byte[] getYm2612Clock(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, YM2612_CLOCK_OFFSET, YM2612_CLOCK_LENGTH);
        }

        protected byte[] getYm3812Clock(byte[] pBytes)
        {
            return ParseFile.ParseSimpleOffset(pBytes, YM3812_CLOCK_OFFSET, YM3812_CLOCK_LENGTH);
        }

        /*
        public static bool isVgm(FileStream pFileStream)
        {
            string str = checkPackedType(pFileStream, FileUtil.GZIP_SIGNATURE, "VGZ");
            if (str == null)
            {
                str = checkPackedType(pFileStream, ASCII_SIGNATURE, "VGM");
            }
            if (str == null)
            {
                str = checkPackedType(pFileStream, FileUtil.SEVEN_ZIP_SIGNATURE, "VGM7Z");
            }
            if (str != null)
            {
                string str2 = str;
                if (str2 != null)
                {
                    byte[] buffer;
                    if (!(str2 == "VGM"))
                    {
                        if (str2 == "VGZ")
                        {
                            buffer = FileUtil.decompressGzip(FileUtil.getChunk(pFileStream, 0L, (int)pFileStream.Length));
                        }
                        else if (str2 == "VGM7Z")
                        {
                        }
                    }
                    else
                    {
                        buffer = FileUtil.getChunk(pFileStream, 0L, (int)pFileStream.Length);
                    }
                }
            }
            return (str != null);
        }
        */
#endregion

        #region STREAM FUNCTIONS
        protected byte[] getEofOffset(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, EOF_OFFSET_OFFSET, EOF_OFFSET_LENGTH);
        }

        protected byte[] getGd3Offset(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, GD3_OFFSET_OFFSET, GD3_OFFSET_LENGTH);
        }

        protected byte[] getLoopNumOfSamples(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, LOOP_NUM_SAMPLES_OFFSET, LOOP_NUM_SAMPLES_LENGTH);
        }

        protected byte[] getLoopOffset(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, LOOP_OFFSET_OFFSET, LOOP_OFFSET_LENGTH);
        }

        protected byte[] getRate(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, RATE_OFFSET, RATE_LENGTH);
        }

        protected byte[] getRf5c68Clock(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, RF5C68_CLOCK_OFFSET, RF5C68_CLOCK_LENGTH);
        }

        protected byte[] getSignatureTag(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
        }

        protected byte[] getSn76489Clock(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SN76489_CLOCK_OFFSET, SN76489_CLOCK_LENGTH);
        }

        protected byte[] getSn76489Feedback(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SN76489_FEEDBACK_OFFSET, SN76489_FEEDBACK_LENGTH);
        }

        protected byte[] getSn76489Srw(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SN76489_SRW_OFFSET, SN76489_SRW_LENGTH);
        }

        protected byte[] getSn76489Flags(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SN76489_FLAGS_OFFSET, SN76489_FLAGS_LENGTH);
        }

        protected byte[] getSpcmClock(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SPCM_CLOCK_OFFSET, SPCM_CLOCK_LENGTH);
        }

        protected byte[] getSpcmInterface(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SPCM_INTERFACE_OFFSET, SPCM_INTERFACE_LENGTH);
        }

        protected byte[] getTotalNumOfSamples(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, TOTAL_NUM_SAMPLES_OFFSET, TOTAL_NUM_SAMPLES_LENGTH);
        }

        protected byte[] getVersion(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, VERSION_OFFSET, VERSION_LENGTH);
        }

        protected byte[] getVgmData(Stream pStream)
        {
            // NEED TO FIX
            return new byte[1];
        }

        protected byte[] getVgmDataOffset(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, VGM_DATA_OFFSET_OFFSET, VGM_DATA_OFFSET_LENGTH);
        }

        protected byte[] getYm2151Clock(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, YM2151_CLOCK_OFFSET, YM2151_CLOCK_LENGTH);
        }

        protected byte[] getYm2413Clock(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, YM2413_CLOCK_OFFSET, YM2413_CLOCK_LENGTH);
        }

        protected byte[] getYm2612Clock(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, YM2612_CLOCK_OFFSET, YM2612_CLOCK_LENGTH);
        }

        protected byte[] getYm3812Clock(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, YM3812_CLOCK_OFFSET, YM3812_CLOCK_LENGTH);
        }

        #endregion

        #region METHODS
        
        private int getIntVersion()
        {
            int ret = Convert.ToInt32(this.Version[0].ToString("X2")) +
                  (Convert.ToInt32(this.Version[1].ToString("X2")) * 100) +
                  (Convert.ToInt32(this.Version[2].ToString("X2")) * 10000) +
                  (Convert.ToInt32(this.Version[3].ToString("X2")) * 1000000);

            return ret;
        }

        private uint getHeaderSize()
        {
            uint headerSize;
            uint vgmDataOffset;
            int intVersion = this.getIntVersion();

            vgmDataOffset = BitConverter.ToUInt32(this.vgmDataOffset, 0);

            switch (intVersion)
            { 
                case INT_VERSION_0100:
                case INT_VERSION_0101:
                case INT_VERSION_0110:
                    headerSize = 0x40;
                    break;
                /*
                 * Removing, too many oddities and offsets to adjust.  For example,
                 *  v1.51 files with v1.61 flags.  Also, current VGMT code does 
                 *  not adjust the Loop Offset if the header size is trimmed to the 
                 *  size identified in the specs.  That should be fixed if this
                 *  code is uncommented.  The rebuildFile function should track the
                 *  difference and adjust the Loop Start Offset accordingly.
                 *                                
                case INT_VERSION_0150:
                case INT_VERSION_0151:
                case INT_VERSION_0160:
                    headerSize = Math.Min(0x80,
                        ((uint)VGM_DATA_OFFSET_OFFSET + vgmDataOffset));
                    break;
                case INT_VERSION_0161:
                    headerSize = Math.Min(0xC0,
                        ((uint)VGM_DATA_OFFSET_OFFSET + vgmDataOffset));
                    break;
                */
                default:
                    if (vgmDataOffset > 0)
                    {
                        headerSize = (uint)VGM_DATA_OFFSET_OFFSET + vgmDataOffset;
                    }
                    else
                    {
                        throw new FormatException("Unsupported VGM format version.");
                    }
                    break;
            }

            return headerSize;
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
            return "VGM";
        }

        public bool IsFileLibrary() { return false; }

        public bool HasMultipleFileExtensions()
        {
            return true;
        }

        public Dictionary<string, string> GetTagHash()
        {
            return this.tagHash;
        }

        public void Initialize(Stream pStream, string pFilePath)
        {            
            if (FormatUtil.IsGzipFile(pStream))
            {
                GZipInputStream gZipInputStream = new GZipInputStream(pStream);
                string tempGzipFile = Path.GetTempFileName();
                FileStream gZipFileStream = new FileStream(tempGzipFile, FileMode.Open, FileAccess.ReadWrite);

                int size = 4096;
                byte[] writeData = new byte[size];
                while (true)
                {
                    size = gZipInputStream.Read(writeData, 0, size);
                    if (size > 0)
                    {
                        gZipFileStream.Write(writeData, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }

                gZipFileStream.Seek(0, SeekOrigin.Begin);
                this.Initialize(gZipFileStream, tempGzipFile);

                gZipFileStream.Close();
                gZipFileStream.Dispose();
                File.Delete(tempGzipFile);
            }
            else
            {
                int intVersion = INT_VERSION_UNKNOWN;

                // Version 1.0 or greater (all versions)
                this.signatureTag = this.getSignatureTag(pStream);
                this.eofOffset = this.getEofOffset(pStream);
                this.version = this.getVersion(pStream);
                this.sn76489Clock = this.getSn76489Clock(pStream);
                this.ym2413Clock = this.getYm2413Clock(pStream);
                this.gd3Offset = this.getGd3Offset(pStream);
                this.totalNumOfSamples = this.getTotalNumOfSamples(pStream);
                this.loopOffset = this.getLoopOffset(pStream);
                this.loopNumOfSamples = this.getLoopNumOfSamples(pStream);
                // vgmData; get only during checksum to save memory

                // Get version to determine if other tags could be present
                intVersion = this.getIntVersion();

                if (intVersion >= INT_VERSION_0101)
                {
                    // Version 1.01 or greater
                    this.rate = this.getRate(pStream);

                    if (intVersion >= INT_VERSION_0110)
                    {
                        // Version 1.10 or greater
                        this.sn76489Feedback = this.getSn76489Feedback(pStream);
                        this.sn76489Srw = this.getSn76489Srw(pStream);
                        this.ym2612Clock = this.getYm2612Clock(pStream);
                        this.ym2151Clock = this.getYm2151Clock(pStream);

                        if (intVersion >= INT_VERSION_0150)
                        {
                            // Version 1.50 or greater
                            this.vgmDataOffset = this.getVgmDataOffset(pStream);

                            if (intVersion >= INT_VERSION_0151)
                            { 
                                // Version 1.51 or greater
                                this.sn76489Flags = this.getSn76489Flags(pStream);

                                this.spcmClock = this.getSpcmClock(pStream);
                                this.spcmInterface = this.getSpcmInterface(pStream);
                                this.Rf5c68Clock = this.getRf5c68Clock(pStream);
                                // will refactor old stuff later
                                this.Ym2203Clock = ParseFile.ParseSimpleOffset(pStream, YM2203_CLOCK_OFFSET, YM2203_CLOCK_LENGTH);
                                this.Ym2608Clock = ParseFile.ParseSimpleOffset(pStream, YM2608_CLOCK_OFFSET, YM2608_CLOCK_LENGTH);
                                this.Ym2610Clock = ParseFile.ParseSimpleOffset(pStream, YM2610_CLOCK_OFFSET, YM2610_CLOCK_LENGTH);
                                
                                this.Ym3812Clock = this.getYm3812Clock(pStream);
                                this.Ym3526Clock = ParseFile.ParseSimpleOffset(pStream, YM3526_CLOCK_OFFSET, YM3526_CLOCK_LENGTH);
                                this.Y8950Clock = ParseFile.ParseSimpleOffset(pStream, Y8950_CLOCK_OFFSET, Y8950_CLOCK_LENGTH);
                                this.Ymf262Clock = ParseFile.ParseSimpleOffset(pStream, YMF262_CLOCK_OFFSET, YMF262_CLOCK_LENGTH);

                                this.Ymf278BClock = ParseFile.ParseSimpleOffset(pStream, YMF278B_CLOCK_OFFSET, YMF278B_CLOCK_LENGTH);
                                this.Ymf271Clock = ParseFile.ParseSimpleOffset(pStream, YMF271_CLOCK_OFFSET, YMF271_CLOCK_LENGTH);
                                this.Ymz280BClock = ParseFile.ParseSimpleOffset(pStream, YMZ280B_CLOCK_OFFSET, YMZ280B_CLOCK_LENGTH);
                                this.Rf5C164Clock = ParseFile.ParseSimpleOffset(pStream, RF5C164_CLOCK_OFFSET, RF5C164_CLOCK_LENGTH);
        
                                this.PwmClock = ParseFile.ParseSimpleOffset(pStream, PWM_CLOCK_OFFSET, PWM_CLOCK_LENGTH);
                                this.Ay8910Clock = ParseFile.ParseSimpleOffset(pStream, AY8910_CLOCK_OFFSET, AY8910_CLOCK_LENGTH);
                                this.Ay8910ChipType = ParseFile.ParseSimpleOffset(pStream, AY8910_CHIP_TYPE_OFFSET, AY8910_CHIP_TYPE_LENGTH);
                                this.Ay8910Flags = ParseFile.ParseSimpleOffset(pStream, AY8910_FLAGS_OFFSET, AY8910_FLAGS_LENGTH);
                                this.Ay8910Ym2203Flags = ParseFile.ParseSimpleOffset(pStream, AY8910_YM2203_FLAGS_OFFSET, AY8910_YM2203_FLAGS_LENGTH);
                                this.Ay8910Ym2610Flags = ParseFile.ParseSimpleOffset(pStream, AY8910_YM2610_FLAGS_OFFSET, AY8910_YM2610_FLAGS_LENGTH);

                                this.LoopModifier = ParseFile.ParseSimpleOffset(pStream, LOOP_MODIFIER_OFFSET, LOOP_MODIFIER_LENGTH);

                            }
                        }
                    }
                }

                if (intVersion < INT_VERSION_0150)
                {
                    this.vgmDataOffset = BitConverter.GetBytes(VGM_DATA_OFFSET_PRE150);
                }
               
                // get full header
                this.FullHeaderSize = this.getHeaderSize();
                this.FullHeader = ParseFile.ParseSimpleOffset(pStream, 0, (int)this.FullHeaderSize);

                this.getAbsoluteOffsets(intVersion);
                this.initializeTagHash(pStream);

            } // if (FormatUtil.IsGzipFile(pFileStream))

            this.filePath = pFilePath; 
        }

        private void getAbsoluteOffsets(int pIntVersion)
        {
            if (pIntVersion >= INT_VERSION_0150)
            {
                this.vgmDataAbsoluteOffset = BitConverter.ToInt32(this.vgmDataOffset, 0) + VGM_DATA_OFFSET_OFFSET;
            }
            else 
            {
                this.vgmDataAbsoluteOffset = VGM_DATA_OFFSET_PRE150;
            }

            if (BitConverter.ToInt32(this.gd3Offset, 0) != 0)
            {
                this.gd3AbsoluteOffset = BitConverter.ToInt32(this.gd3Offset, 0) + GD3_OFFSET_OFFSET;
            }
            else
            {
                this.gd3AbsoluteOffset = 0;
            }
            
            this.eofAbsoluteOffset = BitConverter.ToInt32(this.eofOffset, 0) + EOF_OFFSET_OFFSET;
        }
        
        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            int intVersion = INT_VERSION_UNKNOWN;
            
            pChecksum.Reset();            

            // Version 1.0 or greater (all versions)
            pChecksum.Update(this.version);
            pChecksum.Update(this.sn76489Clock);
            pChecksum.Update(this.ym2413Clock);
            pChecksum.Update(this.totalNumOfSamples);
            pChecksum.Update(this.loopOffset);
            pChecksum.Update(this.loopNumOfSamples);            

            // Get version to determine if other tags are present
            intVersion = this.getIntVersion();

            if (intVersion >= INT_VERSION_0101)
            {
                // Version 1.01 or greater
                pChecksum.Update(this.rate);

                if (intVersion >= INT_VERSION_0110)
                {
                    // Version 1.10 or greater
                    pChecksum.Update(this.sn76489Feedback);
                    pChecksum.Update(this.sn76489Srw);
                    pChecksum.Update(this.ym2612Clock);
                    pChecksum.Update(this.ym2151Clock);
                }
            }

            // Add data
            FileStream fs = new FileStream(this.filePath, FileMode.Open, FileAccess.Read);
            this.addDataSection(fs, ref pChecksum);
            fs.Close();
            fs.Dispose();
        }

        public void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            int intVersion = INT_VERSION_UNKNOWN;

            pChecksum.Reset();

            // Version 1.0 or greater (all versions)
            pChecksum.Update(this.version);
            pChecksum.Update(this.sn76489Clock);
            pChecksum.Update(this.ym2413Clock);
            pChecksum.Update(this.totalNumOfSamples);
            pChecksum.Update(this.loopOffset);
            pChecksum.Update(this.loopNumOfSamples);

            pMd5CryptoStream.Write(this.version, 0, this.version.Length);
            pMd5CryptoStream.Write(this.sn76489Clock, 0, this.sn76489Clock.Length);
            pMd5CryptoStream.Write(this.ym2413Clock, 0, this.ym2413Clock.Length);
            pMd5CryptoStream.Write(this.totalNumOfSamples, 0, this.totalNumOfSamples.Length);
            pMd5CryptoStream.Write(this.loopOffset, 0, this.loopOffset.Length);
            pMd5CryptoStream.Write(this.loopNumOfSamples, 0, this.loopNumOfSamples.Length);

            pSha1CryptoStream.Write(this.version, 0, this.version.Length);
            pSha1CryptoStream.Write(this.sn76489Clock, 0, this.sn76489Clock.Length);
            pSha1CryptoStream.Write(this.ym2413Clock, 0, this.ym2413Clock.Length);
            pSha1CryptoStream.Write(this.totalNumOfSamples, 0, this.totalNumOfSamples.Length);
            pSha1CryptoStream.Write(this.loopOffset, 0, this.loopOffset.Length);
            pSha1CryptoStream.Write(this.loopNumOfSamples, 0, this.loopNumOfSamples.Length);

            // Get version to determine if other tags are present
            intVersion = this.getIntVersion();

            if (intVersion >= INT_VERSION_0101)
            {
                // Version 1.01 or greater
                pChecksum.Update(this.rate);
                pMd5CryptoStream.Write(this.rate, 0, this.rate.Length);
                pSha1CryptoStream.Write(this.rate, 0, this.rate.Length);

                if (intVersion >= INT_VERSION_0110)
                {
                    // Version 1.10 or greater
                    pChecksum.Update(this.sn76489Feedback);
                    pChecksum.Update(this.sn76489Srw);
                    pChecksum.Update(this.ym2612Clock);
                    pChecksum.Update(this.ym2151Clock);

                    pMd5CryptoStream.Write(this.sn76489Feedback, 0, this.sn76489Feedback.Length);
                    pMd5CryptoStream.Write(this.sn76489Srw, 0, this.sn76489Srw.Length);
                    pMd5CryptoStream.Write(this.ym2612Clock, 0, this.ym2612Clock.Length);
                    pMd5CryptoStream.Write(this.ym2151Clock, 0, this.ym2151Clock.Length);

                    pSha1CryptoStream.Write(this.sn76489Feedback, 0, this.sn76489Feedback.Length);
                    pSha1CryptoStream.Write(this.sn76489Srw, 0, this.sn76489Srw.Length);
                    pSha1CryptoStream.Write(this.ym2612Clock, 0, this.ym2612Clock.Length);
                    pSha1CryptoStream.Write(this.ym2151Clock, 0, this.ym2151Clock.Length);
                }
            }

            // Add data
            FileStream fs = new FileStream(this.filePath, FileMode.Open, FileAccess.Read);
            this.addDataSection(fs, ref pChecksum, ref pMd5CryptoStream, ref pSha1CryptoStream);
            fs.Close();
            fs.Dispose();
        }

        private void addDataSection(Stream pFileStream, ref Crc32 pChecksum)
        {
            long currentPosition = pFileStream.Position;
            int bytesToRead = 0;

            // Determine count of bytes to read for data section
            if (this.gd3AbsoluteOffset > this.vgmDataAbsoluteOffset)
            {
                bytesToRead = this.gd3AbsoluteOffset - this.vgmDataAbsoluteOffset;
            }
            else
            {
                bytesToRead = this.eofAbsoluteOffset - this.vgmDataAbsoluteOffset;
            }

            if (FormatUtil.IsGzipFile(pFileStream))
            {
                GZipInputStream gZipInputStream = new GZipInputStream(pFileStream);
                string tempGzipFile = Path.GetTempFileName();
                FileStream gZipFileStream = new FileStream(tempGzipFile, FileMode.Open, FileAccess.ReadWrite);

                int size = 4096;
                byte[] writeData = new byte[size];
                while (true)
                {
                    size = gZipInputStream.Read(writeData, 0, size);
                    if (size > 0)
                    {
                        gZipFileStream.Write(writeData, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }

                ChecksumUtil.AddChunkToChecksum(gZipFileStream, this.vgmDataAbsoluteOffset, bytesToRead,
                    ref pChecksum);

                gZipFileStream.Close();
                gZipFileStream.Dispose();
                File.Delete(tempGzipFile);
            }
            else
            {
                ChecksumUtil.AddChunkToChecksum(pFileStream, this.vgmDataAbsoluteOffset, bytesToRead,
                    ref pChecksum);
            }

            pFileStream.Position = currentPosition;
        }

        private void addDataSection(Stream pFileStream, ref Crc32 pChecksum, 
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            long currentPosition = pFileStream.Position;
            int bytesToRead = 0;

            // Determine count of bytes to read for data section
            if (this.gd3AbsoluteOffset > this.vgmDataAbsoluteOffset)
            {
                bytesToRead = this.gd3AbsoluteOffset - this.vgmDataAbsoluteOffset;
            }
            else
            {
                bytesToRead = this.eofAbsoluteOffset - this.vgmDataAbsoluteOffset;
            }

            if (FormatUtil.IsGzipFile(pFileStream))
            {
                GZipInputStream gZipInputStream = new GZipInputStream(pFileStream);
                string tempGzipFile = Path.GetTempFileName();
                FileStream gZipFileStream = new FileStream(tempGzipFile, FileMode.Open, FileAccess.ReadWrite);

                int size = 4096;
                byte[] writeData = new byte[size];
                while (true)
                {
                    size = gZipInputStream.Read(writeData, 0, size);
                    if (size > 0)
                    {
                        gZipFileStream.Write(writeData, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }

                ChecksumUtil.AddChunkToChecksum(gZipFileStream, this.vgmDataAbsoluteOffset, bytesToRead,
                    ref pChecksum, ref pMd5CryptoStream, ref pSha1CryptoStream);

                gZipFileStream.Close();
                gZipFileStream.Dispose();
                File.Delete(tempGzipFile);
            }
            else
            {
                ChecksumUtil.AddChunkToChecksum(pFileStream, this.vgmDataAbsoluteOffset, bytesToRead,
                    ref pChecksum, ref pMd5CryptoStream, ref pSha1CryptoStream);
            }

            pFileStream.Position = currentPosition;
        }

        private void initializeTagHash(Stream pStream)
        {
            if (this.gd3AbsoluteOffset != 0)
            {
                this.gd3Signature = ParseFile.ParseSimpleOffset(pStream,
                    this.gd3AbsoluteOffset + GD3_SIGNATURE_OFFSET, GD3_SIGNATURE_LENGTH);
                this.gd3Version = ParseFile.ParseSimpleOffset(pStream,
                    this.gd3AbsoluteOffset + GD3_VERSION_OFFSET, GD3_VERSION_LENGTH);
                this.gd3DataLength = ParseFile.ParseSimpleOffset(pStream,
                    this.gd3AbsoluteOffset + GD3_DATA_LENGTH_OFFSET, GD3_DATA_LENGTH_LENGTH);

                byte[] tagBytes = ParseFile.ParseSimpleOffset(pStream,
                    this.gd3AbsoluteOffset + GD3_SIGNATURE_LENGTH + GD3_VERSION_LENGTH + GD3_DATA_LENGTH_LENGTH,
                    BitConverter.ToInt32(this.gd3DataLength, 0));

                System.Text.Encoding enc = System.Text.Encoding.Unicode;
                string tagsString = enc.GetString(tagBytes);

                char[] tagSeparator = new char[] { (char)(0x00), (char)(0x00) };
                string[] splitTags = tagsString.Trim().Split(tagSeparator);

                string versionTag = getIntVersion().ToString();
                this.tagHash.Add("VGM Format Version", versionTag);                
                
                this.tagHash.Add("Track Name (E)", splitTags[GD3_TRACK_NAME_E_IDX]);
                this.tagHash.Add("Track Name (J)", splitTags[GD3_TRACK_NAME_J_IDX]);
                this.tagHash.Add("Game Name (E)", splitTags[GD3_GAME_NAME_E_IDX]);
                this.tagHash.Add("Game Name (J)", splitTags[GD3_GAME_NAME_J_IDX]);
                this.tagHash.Add("System Name (E)", splitTags[GD3_SYSTEM_NAME_E_IDX]);
                this.tagHash.Add("System Name (J)", splitTags[GD3_SYSTEM_NAME_J_IDX]);
                this.tagHash.Add("Author Name (E)", splitTags[GD3_AUTHOR_NAME_E_IDX]);
                this.tagHash.Add("Author Name (J)", splitTags[GD3_AUTHOR_NAME_J_IDX]);
                this.tagHash.Add("Game Date", splitTags[GD3_GAME_DATE_IDX]);
                this.tagHash.Add("Set Ripper", splitTags[GD3_RIPPER_IDX]);
                this.tagHash.Add("Notes", splitTags[GD3_NOTES]);

                // Need to add code for newlines in Notes field.
            }
        }

        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }

        #endregion

        #region IGd3TagFormat

        private string GetSimpleTag(string pTagKey)
        {
            string ret = String.Empty;

            if (this.tagHash.ContainsKey(pTagKey))
            {
                ret = tagHash[pTagKey];
            }
            return ret;
        }
        public string GetTitleTagEn() { return GetSimpleTag("Track Name (E)"); }
        public string GetTitleTagJp() { return GetSimpleTag("Track Name (J)"); }
        public string GetGameTagEn() { return GetSimpleTag("Game Name (E)"); }
        public string GetGameTagJp() { return GetSimpleTag("Game Name (J)"); }
        public string GetSystemTagEn() { return GetSimpleTag("System Name (E)"); }
        public string GetSystemTagJp() { return GetSimpleTag("System Name (J)"); }
        public string GetArtistTagEn() { return GetSimpleTag("Author Name (E)"); }
        public string GetArtistTagJp() { return GetSimpleTag("Author Name (J)"); }
        public string GetDateTag() { return GetSimpleTag("Game Date"); }
        public string GetRipperTag() { return GetSimpleTag("Set Ripper"); }
        public string GetCommentTag() { return GetSimpleTag("Notes"); }

        private void SetSimpleTag(string pKey, string pNewValue)
        {
            //if (!String.IsNullOrEmpty(pNewValue) && !String.IsNullOrEmpty(pNewValue.Trim()))
            //{
                this.tagHash[pKey] = pNewValue.Trim();
            //}            
        }
        public void SetTitleTagEn(string pNewValue) { SetSimpleTag("Track Name (E)", pNewValue); }
        public void SetTitleTagJp(string pNewValue) { SetSimpleTag("Track Name (J)", pNewValue); }
        public void SetGameTagEn(string pNewValue) { SetSimpleTag("Game Name (E)", pNewValue); }
        public void SetGameTagJp(string pNewValue) { SetSimpleTag("Game Name (J)", pNewValue); }
        public void SetSystemTagEn(string pNewValue) { SetSimpleTag("System Name (E)", pNewValue); }
        public void SetSystemTagJp(string pNewValue) { SetSimpleTag("System Name (J)", pNewValue); }
        public void SetArtistTagEn(string pNewValue) { SetSimpleTag("Author Name (E)", pNewValue); }
        public void SetArtistTagJp(string pNewValue) { SetSimpleTag("Author Name (J)", pNewValue); }
        public void SetDateTag(string pNewValue) { SetSimpleTag("Game Date", pNewValue); }
        public void SetRipperTag(string pNewValue) { SetSimpleTag("Set Ripper", pNewValue); }
        public void SetCommentTag(string pNewValue) { SetSimpleTag("Notes", pNewValue); }

        public void UpdateTags() 
        {
            string retaggingFilePath;

            try
            {
                retaggingFilePath = Path.Combine(Path.GetDirectoryName(this.filePath),
                    String.Format("{0}_RETAG_{1}.vgm", Path.GetFileNameWithoutExtension(this.filePath), new Random().Next().ToString()));

                // rebuild file
                //this.outputToVersion150File(retaggingFilePath);
                this.rebuildFile(retaggingFilePath);

                // recompress if original was gzipped
                if (FormatUtil.IsGzipFile(this.filePath))
                {
                    CompressionUtil.GzipEntireFile(retaggingFilePath);
                }

                // delete original
                if (File.Exists(this.filePath))
                {
                    File.Delete(this.filePath);
                }
                
                // rename rebuilt file to original
                File.Move(retaggingFilePath, this.filePath);

            }
            catch (Exception _ex)
            {
                throw new Exception(String.Format("Error updating tags for <{0}>", this.filePath), _ex);
            }
        }

        private void rebuildFile(string pOutputPath)
        {
            UInt32 headerLength;
            UInt32 dataLength;
            long gd3Offset;
            UInt32 gd3Length = 0;
            UInt32 eofOffset = 0;


            // Write Incomplete Header
            headerLength = this.writeRawHeaderSection(pOutputPath);

            // Write Data
            dataLength = this.writeDataSection(pOutputPath, headerLength);

            // Write GD3
            gd3Offset = (long)(headerLength + dataLength);
            gd3Length = this.writeGd3Section(pOutputPath, gd3Offset);

            // Update Header
            eofOffset = (headerLength + dataLength + gd3Length) - Vgm.EOF_OFFSET_OFFSET;
            gd3Offset -= Vgm.GD3_OFFSET_OFFSET;
            this.updateHeader(pOutputPath, eofOffset, (UInt32)gd3Offset, headerLength);        
        }

        private void outputToVersion150File(string pOutputPath)
        {
            UInt32 headerLength;
            UInt32 dataLength;
            long gd3Offset;
            UInt32 gd3Length = 0;
            UInt32 eofOffset = 0;
            

            // Write Incomplete Header
            headerLength = this.writeHeaderSection(pOutputPath);

            // Write Data
            dataLength = this.writeDataSection(pOutputPath, headerLength);

            // Write GD3
            gd3Offset = (long)(headerLength + dataLength);
            gd3Length = this.writeGd3Section(pOutputPath, gd3Offset);
            
            // Update Header
            eofOffset = (headerLength + dataLength + gd3Length) - Vgm.EOF_OFFSET_OFFSET;
            gd3Offset -= Vgm.GD3_OFFSET_OFFSET;
            this.updateHeader(pOutputPath, eofOffset, (UInt32)gd3Offset, headerLength);
        }

        private UInt32 writeRawHeaderSection(string pOutputPath)
        {
            UInt32 headerSize = this.getHeaderSize();
                
            using (FileStream outFs = File.Open(pOutputPath, FileMode.Create, FileAccess.Write))
            {
                outFs.Write(this.FullHeader, 0, this.FullHeader.Length);
            }

            return headerSize;
        }

        private UInt32 writeHeaderSection(string pOutputPath)
        {
            byte[] empty32BitVal = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            UInt32 headerSize = this.getHeaderSize();
            int numberOfZeroesToWrite;
            long finalPosition;
            uint relativeDataStart;

            using (FileStream outFs = File.Open(pOutputPath, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(outFs))
                {
                    // write header, adding empty sections for unknown values
                    bw.Write(Vgm.ASCII_SIGNATURE);
                    bw.Write(empty32BitVal);      // EOF Offset
                    bw.Write(this.version);
                    bw.Write(this.sn76489Clock);

                    bw.Write(this.ym2413Clock);
                    bw.Write(empty32BitVal);      // GD3 Offset
                    bw.Write(this.totalNumOfSamples);
                    bw.Write(this.loopOffset);    // not sure if this needs to be adjusted

                    bw.Write(this.loopNumOfSamples);

                    /////////
                    // v1.01
                    /////////                                        
                    if (this.getIntVersion() < INT_VERSION_0101)
                    {
                        // fill in the rest of the header with zeroes
                        numberOfZeroesToWrite = (int)((long)headerSize - outFs.Position);
                    }
                    else
                    {
                        bw.Write(this.Rate);

                        //////////
                        // v1.10
                        //////////                                        
                        if (this.getIntVersion() < INT_VERSION_0110)
                        {
                            numberOfZeroesToWrite = (int)((long)headerSize - outFs.Position);

                        }
                        else
                        {
                            bw.Write(this.sn76489Feedback);
                            bw.Write(this.sn76489Srw);

                            if (this.getIntVersion() < INT_VERSION_0151)
                            {
                                bw.Write(new byte[] { 0x00 });  // Reserved - 0x2B
                            }
                            else
                            {
                                bw.Write(this.sn76489Flags);      // added in v1.51
                            }
                            
                            bw.Write(this.ym2612Clock);
                            bw.Write(this.ym2151Clock);
                        }

                        /////////
                        // v1.50
                        /////////
                        if (this.getIntVersion() < INT_VERSION_0150)
                        {
                            numberOfZeroesToWrite = (int)((long)headerSize - outFs.Position);
                        }
                        else
                        {
                            relativeDataStart = (uint)((long)headerSize - outFs.Position);
                            bw.Write(relativeDataStart); // Data Start Address
                            
                            /////////
                            // v1.51
                            /////////
                            if (this.getIntVersion() >= INT_VERSION_0151)
                            {
                                bw.Write(this.spcmClock);
                                bw.Write(this.spcmInterface);
                                
                                bw.Write(this.Rf5c68Clock);  // offset 0x40
                                bw.Write(this.Ym2203Clock);
                                bw.Write(this.Ym2608Clock);
                                bw.Write(this.Ym2610Clock);

                                bw.Write(this.Ym3812Clock); // offset 0x50
                                bw.Write(this.Ym3526Clock);
                                bw.Write(this.Y8950Clock);
                                bw.Write(this.Ymf262Clock);

                                bw.Write(this.Ymf278BClock); // offset 0x60
                                bw.Write(this.Ymf271Clock);
                                bw.Write(this.Ymz280BClock);
                                bw.Write(this.Rf5C164Clock);
                                        
                                bw.Write(PwmClock); // offset 0x70
                                bw.Write(Ay8910Clock);
                                bw.Write(Ay8910ChipType);
                                bw.Write(Ay8910Flags);
                                bw.Write(Ay8910Ym2203Flags);
                                bw.Write(Ay8910Ym2610Flags);

                                // reserved at 0x7C
                                bw.Write(new byte[] { 0x00, 0x00, 0x00 });

                                bw.Write(this.LoopModifier); // offset 0x7F
                            }

                            // fill in remainder of header
                            numberOfZeroesToWrite = (int)((long)headerSize - outFs.Position);
                        }
                    }

                    finalPosition = outFs.Position;
                }
            }

            FileUtil.ZeroOutFileChunk(pOutputPath, finalPosition, numberOfZeroesToWrite);

            return headerSize;
        }
        
        private UInt32 writeDataSection(string pOutputPath, long pDestinationOffset)
        {
            int bytesToRead = 0;
            string decompressedFilePath;
            string tempGzipFile = null;

            int size = 4096;
            byte[] writeData = new byte[size];

            // Determine count of bytes to read for data section
            if (this.gd3AbsoluteOffset > this.vgmDataAbsoluteOffset)
            {
                bytesToRead = this.gd3AbsoluteOffset - this.vgmDataAbsoluteOffset;
            }
            else
            {
                bytesToRead = this.eofAbsoluteOffset - this.vgmDataAbsoluteOffset;
            }

            using (FileStream fs = File.OpenRead(this.filePath)) // open source file
            {
                if (FormatUtil.IsGzipFile(fs))
                {
                    using (GZipInputStream gZipInputStream = new GZipInputStream(fs)) // open for reading
                    {
                        tempGzipFile = Path.GetTempFileName();              // get temp file

                        // open temp file for output
                        using (FileStream gZipFileStream = new FileStream(tempGzipFile, FileMode.Open, FileAccess.ReadWrite))
                        {
                            // decompress file
                            while (true)
                            {
                                size = gZipInputStream.Read(writeData, 0, size);
                                if (size > 0)
                                {
                                    gZipFileStream.Write(writeData, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        decompressedFilePath = tempGzipFile;
                    }                    
                }
                else
                {
                    decompressedFilePath = this.filePath;
                }
            }

            // write data section
            FileUtil.ReplaceFileChunk(decompressedFilePath, this.vgmDataAbsoluteOffset,
                bytesToRead, pOutputPath, pDestinationOffset);

            // delete temp file
            if (!String.IsNullOrEmpty(tempGzipFile))
            {
                File.Delete(tempGzipFile);
            }

            return (UInt32)bytesToRead;
        }

        private UInt32 writeGd3Section(string pOutputPath, long pDestinationOffset)
        {
            UInt32 totalLength = 0;
            
            using (MemoryStream ms = new MemoryStream())
            {
                totalLength += this.addGd3TagToMemoryStream("Track Name (E)", ms);
                totalLength += this.addGd3TagToMemoryStream("Track Name (J)", ms);
                totalLength += this.addGd3TagToMemoryStream("Game Name (E)", ms);
                totalLength += this.addGd3TagToMemoryStream("Game Name (J)", ms);
                totalLength += this.addGd3TagToMemoryStream("System Name (E)", ms);
                totalLength += this.addGd3TagToMemoryStream("System Name (J)", ms);
                totalLength += this.addGd3TagToMemoryStream("Author Name (E)", ms);
                totalLength += this.addGd3TagToMemoryStream("Author Name (J)", ms);
                totalLength += this.addGd3TagToMemoryStream("Game Date", ms);
                totalLength += this.addGd3TagToMemoryStream("Set Ripper", ms);
                totalLength += this.addGd3TagToMemoryStream("Notes", ms);

                using (FileStream outFs = File.Open(pOutputPath, FileMode.Open, FileAccess.Write))
                {                
                    using (BinaryWriter bw = new BinaryWriter(outFs))
                    {
                        bw.BaseStream.Position = pDestinationOffset;
                        bw.Write(Vgm.ASCII_SIGNATURE_GD3);
                        totalLength += (uint)Vgm.ASCII_SIGNATURE_GD3.Length;
                        
                        bw.Write(Vgm.VERSION_GD3_0100);
                        totalLength += (uint)Vgm.VERSION_GD3_0100.Length;
                        
                        bw.Write(BitConverter.GetBytes((UInt32)ms.Length));
                        totalLength += sizeof(UInt32);
                        
                        bw.Write(ms.ToArray());
                    }
                }
            }
            return totalLength;
        }

        private void updateHeader(string pOutputPath, UInt32 pEofOffset, UInt32 pGd3Offset, uint headerSize)
        { 
            using (FileStream fs = File.Open(pOutputPath, FileMode.Open, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.BaseStream.Position = Vgm.EOF_OFFSET_OFFSET;
                    bw.Write(BitConverter.GetBytes(pEofOffset));

                    bw.BaseStream.Position = Vgm.GD3_OFFSET_OFFSET;
                    bw.Write(BitConverter.GetBytes(pGd3Offset));

                    if (this.getIntVersion() >= INT_VERSION_0150)
                    {
                        bw.BaseStream.Position = Vgm.VGM_DATA_OFFSET_OFFSET;
                        bw.Write((uint)(headerSize - Vgm.VGM_DATA_OFFSET_OFFSET));
                    }
                }
            }
        }

        private UInt32 addGd3TagToMemoryStream(string pKeyName, MemoryStream pMs)
        {
            System.Text.Encoding enc = System.Text.Encoding.Unicode;
            byte[] nullTerminator = new byte[] { 0x00, 0x00 };
            byte[] newLine = new byte[] { 0x00, 0x0A };
            byte[] tagValue;
            UInt32 totalBytesWritten = 0;

            // add tag
            if (tagHash.ContainsKey(pKeyName))
            {
                tagValue = enc.GetBytes(tagHash[pKeyName].Trim());
                pMs.Write(tagValue, 0, tagValue.Length);
                totalBytesWritten += (UInt32)tagValue.Length;
            }

            // add terminator
            pMs.Write(nullTerminator, 0, nullTerminator.Length);
            totalBytesWritten += (UInt32)nullTerminator.Length;

            return totalBytesWritten;
        }

        #endregion

    }
}