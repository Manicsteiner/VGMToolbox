using System.Collections.Generic;
using System.IO;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Kssx 
    {
        private static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x4B, 0x53, 0x53, 0x58 }; // KSSX
        private const string FORMAT_ABBREVIATION = "KSSX";

        private const int SIG_OFFSET = 0x00;
        private const int SIG_LENGTH = 0x04;

        private const int LOAD_ADDRESS_OFFSET = 0x04;
        private const int LOAD_ADDRESS_LENGTH = 0x02;

        private const int LOAD_SIZE_OFFSET = 0x06;
        private const int LOAD_SIZE_LENGTH = 0x02;

        private const int INIT_ADDRESS_OFFSET = 0x08;
        private const int INIT_ADDRESS_LENGTH = 0x02;

        private const int PLAY_ADDRESS_OFFSET = 0x0A;
        private const int PLAY_ADDRESS_LENGTH = 0x02;

        private const int BANK_START_NO_OFFSET = 0x0C;
        private const int BANK_START_NO_LENGTH = 0x01;

        private const int BANKED_MODE_OFFSET = 0x0D;
        private const int BANKED_MODE_LENGTH = 0x01;

        private const int EXTRA_HEADER_SIZE_OFFSET = 0x0E;
        private const int EXTRA_HEADER_SIZE_LENGTH = 0x01;

        private const int DEVICE_FLAG_OFFSET = 0x0F;
        private const int DEVICE_FLAG_LENGTH = 0x01;

        private byte[] asciiSignature;
        private byte[] loadAddress;
        private byte[] loadSize;
        private byte[] initAddress;
        private byte[] playAddress;
        private byte[] bankStartNo;
        private byte[] bankedMode;
        private byte[] extraHeaderSize;
        private byte[] deviceFlag;
        private byte[] data;

        Dictionary<string, string> tagHash = new Dictionary<string, string>();

        public byte[] AsciiSignature { get { return this.asciiSignature; } }
        public byte[] LoadAddress { get { return this.loadAddress; } }
        public byte[] LoadSize { get { return this.loadSize; } }
        public byte[] InitAddress { get { return this.initAddress; } }
        public byte[] PlayAddress { get { return this.playAddress; } }
        public byte[] BankStartNo { get { return this.bankStartNo; } }
        public byte[] BankedMode { get { return this.bankedMode; } }
        public byte[] ExtraHeaderSize { get { return this.extraHeaderSize; } }
        public byte[] DeviceFlag { get { return this.deviceFlag; } }
        public byte[] Data { get { return this.data; } }

        #region METHODS

        public byte[] getAsciiSignature(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, SIG_OFFSET, SIG_LENGTH);
        }

        public byte[] getLoadAddress(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, LOAD_ADDRESS_OFFSET, LOAD_ADDRESS_LENGTH);
        }

        public byte[] getLoadSize(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, LOAD_SIZE_OFFSET, LOAD_SIZE_LENGTH);
        }

        public byte[] getInitAddress(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, INIT_ADDRESS_OFFSET, INIT_ADDRESS_LENGTH);
        }

        public byte[] getPlayAddress(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, PLAY_ADDRESS_OFFSET, PLAY_ADDRESS_LENGTH);
        }

        public byte[] getBankStartNo(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, BANK_START_NO_OFFSET, BANK_START_NO_LENGTH);
        }

        public byte[] getBankedMode(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, BANKED_MODE_OFFSET, BANKED_MODE_LENGTH);
        }

        public byte[] getExtraHeaderSize(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, EXTRA_HEADER_SIZE_OFFSET, EXTRA_HEADER_SIZE_LENGTH);
        }

        public byte[] getDeviceFlag(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, DEVICE_FLAG_OFFSET, DEVICE_FLAG_LENGTH);
        }






        #endregion
    }
}
