using System;
using System.Collections.Generic;
using System.IO;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class SegaSaturnSequence
    {
        public static uint SSF_DATA_START_ADDRESS = 0xB000;
        
        public string FilePath { set; get; }

        public uint SequenceCount { set; get; }

        public SegaSaturnSequence(string sequencePath)
        {
            this.FilePath = Path.GetFullPath(sequencePath);

            using (FileStream fs = File.OpenRead(this.FilePath))
            {
                this.SequenceCount = ParseFile.ReadByte(fs, 1);
            }
        }


        public static byte[] GetFileSizeBE24(long fileLength)
        {
            uint sizeMask = 0x00FFFFFF;
            byte[] sizeBytes = BitConverter.GetBytes((uint)fileLength & sizeMask);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(sizeBytes);
            }

            return sizeBytes;
        }

        public static uint? GetDspRamNeeded(string dspFile)
        {
            uint? dspRamRequired = null;
            byte ramValue;

            if (File.Exists(dspFile))
            {
                using (FileStream fs = File.OpenRead(dspFile))
                {
                    ramValue = ParseFile.ReadByte(fs, 0x20);

                    switch (ramValue)
                    {
                        case 0:
                            dspRamRequired = 0x4040;
                            break;
                        case 1:
                            dspRamRequired = 0x8040;
                            break;
                        case 2:
                            dspRamRequired = 0x10040;
                            break;
                        case 3:
                            dspRamRequired = 0x20040;
                            break;
                        default:
                            dspRamRequired = null;
                            break;
                    }
                }
            }

            return dspRamRequired;
        }

        public static byte[] GetMapEntry(uint startAddress, uint dataSize, byte dataFormatBankNumber)
        {
            byte[] mapEntry = new byte[8];
            byte[] sizeBytes = new byte[4];

            //-----------------
            // build entry
            //-----------------
            mapEntry[0] = dataFormatBankNumber; // data format, bank number

            // file start offset
            sizeBytes = GetFileSizeBE24(startAddress);
            Array.Copy(sizeBytes, 1, mapEntry, 1, 3);

            mapEntry[4] = 0x80; // transfer

            // file size
            sizeBytes = GetFileSizeBE24(dataSize);
            Array.Copy(sizeBytes, 1, mapEntry, 5, 3);

            return mapEntry;
        }

        /// <summary>
        /// From ssfmake.py by Kingshriek:
        /// 
        /// # =====================================================================================================
        /// # Area map format
        /// # -----------------------------------------------------------------------------------------------------
        /// # The currently used area map resides at 0x500-0x5FF in the 68000's address space. The area map consists of
        /// # units of 8-byte blocks (so 32 at most). The format of one of these blocks is as follows:
        /// #
        /// #     fb aa aa aa t0 ss ss ss
        /// #
        /// #     f - Data format (4-bits - 0: TONE, 1: SEQUENCE, 2: DSP_PROGRAM, 3: DSP_RAM)
        /// #     b - Bank number (4-bits - any value between 0-15)
        /// #     a - Start address (24-bits, big-endian, if DSP_RAM, must be a multiple of 0x2000)
        /// #     t - Transfer bit (bit 7 - must be set to 1 for the driver to read the data)
        /// #     s - Data size (24-bits, big-endian)
        /// #
        /// # If DSP_PROGRAM banks are included, there must be a DSP_RAM bank present. The DSP_RAM bank must be large
        /// # enough to meet the DSP_PROGRAM requirements. The DSP RAM size is specified by byte 0x20 in the DSP program 
        /// # header (0: 0x4040 bytes, 1: 0x8040 bytes, 2: 0x10040 bytes, 3: 0x20040 bytes). Also, the start address
        /// # for a DSP RAM bank must be a multiple of 0x2000 bytes.
        /// </summary>
        /// <param name="seqFiles"></param>
        /// <param name="dspFile"></param>
        /// <param name="tonFiles"></param>
        /// <returns></returns>
        public static byte[] GetMapFile(string[] seqFiles, string dspFile, string[] tonFiles)
        {
            byte[] mapFile = new byte[0x100];
            byte[] mapEntry;
            ushort numEntriesInMap = 0;

            uint dataAddress = SSF_DATA_START_ADDRESS;
            int seqCounter = 0;
            int tonCounter = 0;
            uint? dspRamNeeded = null;
            
            
            FileInfo fi;
            byte[] sizeBytes = new byte[4];
            
            //-------------------
            // build SEQ entries
            //-------------------
            foreach (string seq in seqFiles)
            {
                if (!String.IsNullOrEmpty(seq))
                {
                    fi = new FileInfo(seq);
                    mapEntry = GetMapEntry(dataAddress, (uint)fi.Length, (byte)(0x10 | seqCounter));
                    Array.Copy(mapEntry, 0, mapFile, (numEntriesInMap * 8), mapEntry.Length);

                    dataAddress += (uint)fi.Length;
                    seqCounter++;
                    numEntriesInMap++;
                }
            }

            //-----------------
            // build DSP entry
            //-----------------
            if (!String.IsNullOrEmpty(dspFile))
            {
                fi = new FileInfo(dspFile);
                mapEntry = GetMapEntry(dataAddress, (uint)fi.Length, 0x20);
                Array.Copy(mapEntry, 0, mapFile, (numEntriesInMap * 8), mapEntry.Length);

                dataAddress += (uint)fi.Length;
                numEntriesInMap++;

                // build DSP RAM entry
                dspRamNeeded = GetDspRamNeeded(dspFile);

                if (dspRamNeeded != null)
                {
                    mapEntry = GetMapEntry(dataAddress, (uint)dspRamNeeded, 0x30);
                    Array.Copy(mapEntry, 0, mapFile, (numEntriesInMap * 8), mapEntry.Length);

                    dataAddress += (uint)fi.Length;
                    numEntriesInMap++;                
                }
            }

            //-------------------
            // build TON entries
            //-------------------
            foreach (string ton in tonFiles)
            {
                if (!String.IsNullOrEmpty(ton))
                {
                    fi = new FileInfo(ton);
                    mapEntry = GetMapEntry(dataAddress, (uint)fi.Length, (byte)(0x00 | seqCounter));
                    Array.Copy(mapEntry, 0, mapFile, (numEntriesInMap * 8), mapEntry.Length);

                    dataAddress += (uint)fi.Length;
                    tonCounter++;
                    numEntriesInMap++;
                }
            }

            //-------------------
            // write terminator
            //-------------------
            mapFile[numEntriesInMap * 8] = 0xFF;

            return mapFile;
        }
    }
}
