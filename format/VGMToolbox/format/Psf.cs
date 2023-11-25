using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Psf: Xsf
    {
        public struct ProbableItemStruct
        {
            public long offset;
            public uint length;

            public void Init()
            {
                offset = -1;
                length = 0;
            }
        }

        public const uint MIN_TEXT_SECTION_OFFSET = 0x80010000;
        public const uint MAX_TEXT_SECTION_OFFSET = 0x801F0000;
        public const uint PC_OFFSET_CORRECTION = 0x800;
        public const uint TEXT_SIZE_OFFSET = 0x1C;

        public const int MINIPSF_INITIAL_PC_OFFSET = 0x10;
        public const int MINIPSF_TEXT_SECTION_OFFSET = 0x18;
        public const int MINIPSF_TEXT_SECTION_SIZE_OFFSET = 0x1C;

        public static readonly byte[] VB_START_BYTES = new byte[] { 0x00, 0x00, 0x00, 0x00,
                                                                    0x00, 0x00, 0x00, 0x00,
                                                                    0x00, 0x00, 0x00, 0x00,
                                                                    0x00, 0x00, 0x00, 0x00};

        public static readonly byte[] VB_END_BYTES_1 = new byte[] { 0x00, 0x07, 0x07, 0x07,
                                                                  0x07, 0x07, 0x07, 0x07, 
                                                                  0x07, 0x07, 0x07, 0x07, 
                                                                  0x07, 0x07, 0x07, 0x07};

        public static readonly byte[] VB_END_BYTES_2 = new byte[] { 0x00, 0x07, 0x77, 0x77,
                                                                  0x77, 0x77, 0x77, 0x77, 
                                                                  0x77, 0x77, 0x77, 0x77,
                                                                  0x77, 0x77, 0x77, 0x77};

        public static int SONY_ADPCM_ROW_SIZE = 0x10;
        public static int MIN_ADPCM_ROW_COUNT = 10;
        public static int MIN_ADPCM_ROW_SIZE = MIN_ADPCM_ROW_COUNT * SONY_ADPCM_ROW_SIZE;
        public static double MIN_SAMPLE_MATCH_PERCENTAGE = 0.05f; // 5%

        public static ArrayList GetSeqFileList(Stream fs, bool UseSeqMinimumSize, int MinimumSeqSize)
        {
            ArrayList ret = new ArrayList();
            
            ProbableItemStruct seqEntry = new ProbableItemStruct();
            long offset = 0;
            long seqEof = 0;
            uint seqLength;


            // build file list
            while ((offset = ParseFile.GetNextOffset(fs, offset, PsxSequence.ASCII_SIGNATURE_SEQ)) > -1)
            {
                seqEof = ParseFile.GetNextOffset(fs, offset, PsxSequence.END_SEQUENCE);

                if (seqEof > 0)
                {
                    seqLength = (uint)(seqEof - offset + PsxSequence.END_SEQUENCE.Length);

                    if ((!UseSeqMinimumSize) || 
                        ((UseSeqMinimumSize) && (seqLength >= MinimumSeqSize)))
                    {
                        seqEntry.offset = offset;
                        seqEntry.length = (uint)(seqEof - offset + PsxSequence.END_SEQUENCE.Length);
                        ret.Add(seqEntry);
                    }
                }
                offset += 1;
            }

            return ret;
        }

        public static ArrayList GetSepFileList(Stream fs)
        {
            ArrayList ret = new ArrayList();
            ProbableItemStruct sepEntry = new ProbableItemStruct();
            
            long offset = 0;
            long sepStartingOffset = 0;
            int sepSeqCount;
            long seqEof = 0;

            // build file list
            while ((offset = ParseFile.GetNextOffset(fs, offset, PsxSequence.ASCII_SIGNATURE_SEP)) > -1)
            {
                sepStartingOffset = offset;
                sepSeqCount = 1;
                byte[] nextSepCheckBytes;
                seqEof = offset;

                while ((seqEof = ParseFile.GetNextOffset(fs, seqEof, PsxSequence.END_SEQUENCE)) > -1)
                {
                    nextSepCheckBytes = ParseFile.ParseSimpleOffset(fs, (long)(seqEof + PsxSequence.END_SEQUENCE.Length), 2);
                    Array.Reverse(nextSepCheckBytes);

                    if (nextSepCheckBytes.Length > 0)
                    {
                        if (BitConverter.ToUInt16(nextSepCheckBytes, 0) == (UInt16)sepSeqCount)
                        {
                            sepSeqCount++;
                        }
                        else
                        {
                            sepEntry.offset = sepStartingOffset;
                            sepEntry.length = (uint)(seqEof - sepStartingOffset + PsxSequence.END_SEQUENCE.Length);
                            ret.Add(sepEntry);
                            break;
                        }
                    }
                    else
                    {
                        sepEntry.offset = sepStartingOffset;
                        sepEntry.length = (uint)(seqEof - sepStartingOffset + PsxSequence.END_SEQUENCE.Length);
                        ret.Add(sepEntry);
                        break;
                    }

                    seqEof += 1;
                }

                offset += 1;
            }

            return ret;
        }

        // function to determine if data is sony adpcm
        public static bool IsPotentialAdpcm(Stream searchStream, long offset, int rowsToCheck, bool doAdditionalChecks)
        {
            bool ret = true;
            byte[] checkBytes = new byte[0x10];
            int bytesRead;

            searchStream.Position = offset;

            // check for rows meeting criteria
            for (int i = 0; i < rowsToCheck; i++)
            {
                bytesRead = searchStream.Read(checkBytes, 0, checkBytes.Length);

                if ((bytesRead != checkBytes.Length) || 
                    (!IsSonyAdpcmRow(checkBytes, doAdditionalChecks)))
                {
                    ret = false;
                    break;
                }

            }

            return ret;
        }

        public static bool IsSonyAdpcmRow(byte[] potentialAdpcm, bool doAdditionalChecks)
        {
            bool ret = true;

            if ((potentialAdpcm.Length != SONY_ADPCM_ROW_SIZE) ||
                (potentialAdpcm[1] > 7) || 
                (potentialAdpcm[0] > 0x4C) ||
                (ParseFile.CompareSegment(potentialAdpcm, 0, VB_START_BYTES))
               )
            {
                ret = false;
            }
            else if (doAdditionalChecks &&
                     ((potentialAdpcm[0] == 0) && (potentialAdpcm[1] != 2) && (GetCountOfZeroBytes(potentialAdpcm) > 14)))
            {
                ret = false;
            }
            return ret;
        }

        public static bool IsSonyAdpcmRow(Stream inputStream, long offset)
        {
            bool ret = true;
            byte[] potentialAdpcm = new byte[SONY_ADPCM_ROW_SIZE];
            int bytesRead;

            inputStream.Position = offset;
            bytesRead = inputStream.Read(potentialAdpcm, 0, SONY_ADPCM_ROW_SIZE);

            if ((bytesRead != SONY_ADPCM_ROW_SIZE) ||
                (potentialAdpcm[1] > 7) ||
                (potentialAdpcm[0] > 0x4C) ||
                ((potentialAdpcm[0] == 0) && (potentialAdpcm[1] != 2) && (GetCountOfZeroBytes(potentialAdpcm) > 14)) ||
                (ParseFile.CompareSegment(potentialAdpcm, 0, VB_START_BYTES))
               )
            {
                ret = false;
            }

            return ret;
        }

        public static bool IsSonyAdpcmRow(Stream inputStream, long offset, ref Dictionary<long, bool> rowCheckHash)
        {
            bool ret = true;

            if (rowCheckHash.ContainsKey(offset))
            {
                ret = rowCheckHash[offset];
            }
            else
            {
                byte[] potentialAdpcm = new byte[SONY_ADPCM_ROW_SIZE];
                int bytesRead;

                inputStream.Position = offset;
                bytesRead = inputStream.Read(potentialAdpcm, 0, SONY_ADPCM_ROW_SIZE);

                if ((bytesRead != SONY_ADPCM_ROW_SIZE) ||
                    (potentialAdpcm[1] > 7) ||
                    (potentialAdpcm[0] > 0x4C) ||
                    ((potentialAdpcm[0] == 0) && (potentialAdpcm[1] != 2) && (GetCountOfZeroBytes(potentialAdpcm) > 14)) ||
                    (ParseFile.CompareSegment(potentialAdpcm, 0, VB_START_BYTES))
                   )
                {
                    ret = false;
                }

                // update hash table
                rowCheckHash[offset] = ret;
            }
            
            return ret;
        }

        public static int GetCountOfZeroBytes(byte[] values)
        {
            int zeroCount = 0;

            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == 0)
                    {
                        zeroCount++;
                    }
                }
            }
            return zeroCount;
        }

        public static ProbableItemStruct GetPotentialAdpcmItem(
            Stream searchStream,
            long offsetToCheck,
            ProbableItemStruct previousItem,
            ProbableItemStruct[][] existingItemLocations,
            int maximumSampleSize)
        {
            ProbableItemStruct probableAdpcmItem = new ProbableItemStruct();
            byte[] adpcmRow = new byte[0x10];
            // bool errorExists = false;

            probableAdpcmItem.Init();
            adpcmRow = ParseFile.ParseSimpleOffset(searchStream, offsetToCheck, adpcmRow.Length);

            // check for potential sony adpcm signature
            if (IsPotentialAdpcm(searchStream, offsetToCheck + 0x10, MIN_ADPCM_ROW_SIZE, false))
            {
                /*
                // check if we have passed a different file type and reset previousVbOffset if we did
                foreach (ProbableItemStruct[] items in existingItemLocations)
                {
                    if (SteppedOverAnotherFile(previousItem.offset, offsetToCheck, items))
                    {
                        errorExists = true;
                        break;
                    }
                }
                
                // check if we have exceeded the max sample size and set error flag
                //  so the chunk size check doesn't apply
                if (!errorExists &&
                    (previousItem.offset != -1) &&
                    ((offsetToCheck - previousItem.offset) > maximumSampleSize))
                {
                    errorExists = true;
                }

                // try to preserve proper VB chunk size
                if (errorExists || 
                    (previousItem.offset == -1) ||
                    ((offsetToCheck - previousItem.offset) % 0x10 == 0))
                {
                    probableAdpcmItem.offset = offsetToCheck;
                }
                */
                probableAdpcmItem.offset = offsetToCheck;
            }

            return probableAdpcmItem;
        }

        // check if the offsets have "stepped" over another file
        private static bool SteppedOverAnotherFile(long previousOffset, long currentOffset, 
            ProbableItemStruct[] items)
        {
            bool ret = false;

            if (previousOffset != -1)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if ((previousOffset < items[i].offset) &&
                        (currentOffset > items[i].offset))
                    {
                        ret = true;
                        break;
                    }
                }

            }

            return ret;
        }
    }
}
