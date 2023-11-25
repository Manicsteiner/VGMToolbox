using System;
using System.Collections.Generic;
using System.Text;

namespace VGMToolbox.format
{
    public class Atrac3Plus
    {
        public const string FileExtension = ".aa3";
        public const string FileExtensionPsp = ".oma";

        public static readonly byte[] Aa3HeaderChunk =
            new byte[] {0x65, 0x61, 0x33, 0x03, 0x00, 0x00, 0x00, 0x00, 0x07, 0x76, 0x47, 0x45, 0x4F, 0x42, 0x00, 0x00,
                        0x01, 0xC6, 0x00, 0x00, 0x02, 0x62, 0x69, 0x6E, 0x61, 0x72, 0x79, 0x00, 0x00, 0x00, 0x00, 0x4F, 
                        0x00, 0x4D, 0x00, 0x47, 0x00, 0x5F, 0x00, 0x4C, 0x00, 0x53, 0x00, 0x49, 0x00, 0x00, 0x00, 0x01,
                        0x00, 0x40, 0x00, 0xDC, 0x00, 0x70, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4B, 0x45,
                        0x59, 0x52, 0x49, 0x4E, 0x47};

        public static readonly byte[] Ea3HeaderChunk =
            new byte[] { 0x45, 0x41, 0x33, 0x01, 0x00, 0x60, 0xFF, 0xFF };

        //public static readonly byte[] RiffHeaderChunkForPam

        public const long Aa3HeaderSize = 0x460;
        public const long Aa3FormatStringLocation = 0x420;
        public const long Aa3HeaderLocation = 0x00;
        public const long Ea3HeaderLocation = 0x400;

        public static byte[] GetFormatBytes(int frequency, int channels, int bitrate)
        {
            byte[] formatString = new byte[] { 0x01, 0x00, 0x00, 0x00 };

            // Thanks to Alpha23 for his QuickBMS scripts with this info.
            switch (frequency)
            { 
                case 44100:
                    switch (channels)
                    { 
                        case 1:
                        case 2:
                            if (channels == 1)
                            {
                                formatString[2] = 0x24;
                            }
                            else 
                            {
                                formatString[2] = 0x28;
                            }
                            
                            switch (bitrate)
                            {
                                case 32:
                                    formatString[3] = 0x17;
                                    break;
                                case 48:
                                    formatString[3] = 0x22;
                                    break;
                                case 64:
                                    formatString[3] = 0x2E;
                                    break;
                                case 96:
                                    formatString[3] = 0x45;
                                    break;
                                case 128:
                                    formatString[3] = 0x5C;
                                    break;
                                case 192:
                                    formatString[3] = 0x8B;
                                    break;
                                default:
                                    throw new FormatException(String.Format("Unsupported bitrate ({0}), for input frequency ({1}) and channels ({2}), cannot get AA3 format string.", bitrate.ToString(), frequency.ToString(), channels.ToString()));
                            }
                            break;                                                       
                        case 6:
                            switch (bitrate)
                            {
                                case 320:
                                    formatString[2] = 0x34;
                                    formatString[3] = 0xE8;
                                    break;
                                case 512:
                                    formatString[2] = 0x35;
                                    formatString[3] = 0x73;
                                    break;
                                default:
                                    throw new FormatException(String.Format("Unsupported bitrate ({0}), for input frequency ({1}) and channels ({2}), cannot get AA3 format string.", bitrate.ToString(), frequency.ToString(), channels.ToString()));
                            }
                            break;
                        default:
                            throw new FormatException(String.Format("Unsupported channels ({0}), for input frequency ({1}), cannot get AA3 format string.", channels.ToString(), frequency.ToString()));
                    }
                    break;
                case 48000:
                    switch (channels)
                    { 
                        case 1:
                        case 2:
                            if (channels == 1)
                            {
                                formatString[2] = 0x44;
                            }
                            else
                            {
                                formatString[2] = 0x48;
                            }

                            if (bitrate == 128)
                            {
                                formatString[3] = 0x55;
                            }
                            else
                            {
                                throw new FormatException(String.Format("Unsupported bitrate ({0}), for input frequency ({1}) and channels ({2}), cannot get AA3 format string.", bitrate.ToString(), frequency.ToString(), channels.ToString()));
                            }
                            break;
                        case 6:
                            switch (bitrate)
                            { 
                                case 256:
                                    formatString[2] = 0x54;
                                    formatString[3] = 0xAA;
                                    break;
                                case 320:
                                    formatString[2] = 0x54;
                                    formatString[3] = 0xD5;
                                    break;
                                case 384:
                                    formatString[2] = 0x54;
                                    formatString[3] = 0xFF;
                                    break;
                                case 512:
                                    formatString[2] = 0x55;
                                    formatString[3] = 0x55;
                                    break;
                                default:
                                    throw new FormatException(String.Format("Unsupported bitrate ({0}), for input frequency ({1}) and channels ({2}), cannot get AA3 format string.", bitrate.ToString(), frequency.ToString(), channels.ToString()));                                    
                            }
                            break;
                        default:
                            throw new FormatException(String.Format("Unsupported channels ({0}), for input frequency ({1}), cannot get AA3 format string.", channels.ToString(), frequency.ToString()));
                    }
                    break;
                default:
                    throw new FormatException(String.Format("Unsupported frequency: {0}, cannot get AA3 format string.", frequency.ToString()));
            }

            return formatString;
        }

        public static byte[] GetFormatBytes(uint headerBlockValue)
        {
            uint formatValue = 0x01000000;
            formatValue = formatValue | (0xFFFF & headerBlockValue); // Thanks to FastElbJa for this info.
                        
            byte[] formatBytes = BitConverter.GetBytes(formatValue);
            Array.Reverse(formatBytes);

            return formatBytes;
        }

        public static byte[] GetAa3Header(int frequency, int channels, int bitrate)
        {
            byte[] headerBytes = GetFormatBytes(frequency, channels, bitrate);

            return GetAa3Header(headerBytes);
        }

        public static byte[] GetAa3Header(uint headerBlockValue)
        {
            byte[] headerBytes = GetFormatBytes(headerBlockValue);

            return GetAa3Header(headerBytes);
        }

        public static byte[] GetAa3Header(byte[] formatString)
        {
            byte[] headerBytes = new byte[Aa3HeaderSize];

            // copy AA3 header
            Array.Copy(Aa3HeaderChunk, 0, headerBytes, Aa3HeaderLocation, Aa3HeaderChunk.Length);

            // copy EA3 header
            Array.Copy(Ea3HeaderChunk, 0, headerBytes, Ea3HeaderLocation, Ea3HeaderChunk.Length);

            // insert format string
            Array.Copy(formatString, 0, headerBytes, Aa3FormatStringLocation, formatString.Length);
            return headerBytes;
        }

        //public static byte[] GetRiffHeaderForPam(uint headerBlockValue)
        //{ 
        
        
        //}

        //public static byte[] GetRiffHeaderFromMsfHeader(byte[] msfHeader)
        //{ 
        
        //}
    }
}
