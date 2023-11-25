using System;
using System.Collections;
using System.ComponentModel;
using System.IO;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    class PspSeqDataFinderWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public static readonly byte[] PPHD_SIGNATURE = new byte[] { 0x50, 0x50, 0x48, 0x44,
                                                                    0x38, 0x00, 0x00, 0x00, 
                                                                    0x00, 0x00, 0x01, 0x00};

        #region STRUCTURES
        public struct PspSeqDataFinderStruct : IVgmtWorkerStruct
        {
            public bool ReorderMidFiles;
            public bool UseZeroOffsetForPbd;

            public string[] SourcePaths { set; get; }
        }

        public struct PphdStruct
        {
            public long progSectionOffset;
            public long toneSectionOffset;
            public long vagSectionOffset;

            public long maxVagInfoNumber;
            public long vagInfoSize;
            public long[] vagOffsets;
            public long[] vagLengths;
            public bool IsSmallSamplePresent { set; get; }

            public string FileName;
            public long startingOffset;
            public long length;

            public long expectedPbdLength;
            public long pbdStartingOffset;
            public long pbdLength;
        }

        #endregion

        public PspSeqDataFinderWorker() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pPspSeqDataFinderStruct, DoWorkEventArgs e)
        {
            PspSeqDataFinderStruct pspStruct = (PspSeqDataFinderStruct)pPspSeqDataFinderStruct;

            long offset;

            int phdNumber = 0;
            PphdStruct phdObject;
            ArrayList phdArrayList = new ArrayList();

            string midName;
            int midNumber = 0;
            Psf.ProbableItemStruct midEntry;
            ArrayList midFiles = new ArrayList();
            bool midNamingMessageDisplayed = false;
            Midi midiFile = new Midi();

            Psf.ProbableItemStruct potentialPbd;
            Psf.ProbableItemStruct[] potentialPbdList;
            byte[] pbdRow = new byte[Psf.SONY_ADPCM_ROW_SIZE];
            ArrayList emptyRowList = new ArrayList();

            // display file name
            this.progressStruct.Clear();
            this.progressStruct.GenericMessage = String.Format("[{0}]{1}", pPath, Environment.NewLine);
            this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

            // open file
            using (FileStream fs = File.OpenRead(pPath))
            {
                string destinationFolder = Path.Combine(Path.GetDirectoryName(pPath), Path.GetFileNameWithoutExtension(pPath));

                // get PHD files
                #region PHD EXTRACT
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("  Extracting PHD{0}", Environment.NewLine);
                this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                offset = 0;

                while ((offset = ParseFile.GetNextOffset(fs, offset, PPHD_SIGNATURE)) > -1)
                {
                    try
                    {
                        // get PPHD data
                        phdObject = GetPphdData(fs, offset);
                        phdObject.FileName = String.Format("{0}_{1}.PHD", Path.GetFileNameWithoutExtension(pPath), phdNumber++.ToString("X4"));

                        // extract the file
                        ParseFile.ExtractChunkToFile(fs, offset, (int)phdObject.length,
                            Path.Combine(destinationFolder, phdObject.FileName), true, true);

                        // add to array
                        phdArrayList.Add(phdObject);
                    }
                    catch (Exception pphdEx)
                    {
                        this.progressStruct.Clear();
                        this.progressStruct.ErrorMessage = String.Format("      Error extracting PPHD at offset 0x{0}: {1}{2}", offset.ToString("X8"), pphdEx.Message, Environment.NewLine);
                        ReportProgress(progress, this.progressStruct);
                    }

                    // increment offset
                    offset += 1;
                }

                #endregion

                // get MID Files
                #region MID EXTRACT
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("  Extracting MID{0}", Environment.NewLine);
                this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                midEntry = new Psf.ProbableItemStruct();
                offset = 0;

                // build file list
                while ((offset = ParseFile.GetNextOffset(fs, offset, Midi.ASCII_SIGNATURE_MTHD)) > -1)
                {
                    midiFile.Initialize(fs, pPath, offset);
                    midEntry.offset = midiFile.FileStartOffset;
                    midEntry.length = (uint)midiFile.TotalFileLength;
                    midFiles.Add(midEntry);
                                      
                    offset += 1;
                }

                foreach (Psf.ProbableItemStruct mid in midFiles)
                {
                    if (pspStruct.ReorderMidFiles)
                    {
                        if (phdArrayList.Count < midFiles.Count)
                        {
                            if (!midNamingMessageDisplayed)
                            {
                                this.progressStruct.Clear();
                                this.progressStruct.ErrorMessage = String.Format(
                                    "Warning, cannot reorder MID files, there are less PHD files than MID files.{0}", Environment.NewLine);
                                this.ReportProgress(this.progress, this.progressStruct);
                                midNamingMessageDisplayed = true;
                            }

                            midName = String.Format("{0}_{1}.MID", Path.GetFileNameWithoutExtension(pPath), midNumber++.ToString("X4"));
                        }
                        else
                        {
                            phdObject = (PphdStruct)phdArrayList[phdArrayList.Count - midFiles.Count + midNumber++];
                            midName = Path.ChangeExtension(phdObject.FileName, ".MID");
                        }
                    }
                    else
                    {
                        midName = String.Format("{0}_{1}.MID", Path.GetFileNameWithoutExtension(pPath), midNumber++.ToString("X4"));
                    }

                    ParseFile.ExtractChunkToFile(fs, mid.offset, (int)mid.length,
                        Path.Combine(destinationFolder, midName), true, true);


                }
                #endregion

                // get PBD files
                #region PBD EXTRACT
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("  Extracting PBD...WARNING, THIS CAN TAKE A LONG TIME...{0}", Environment.NewLine);
                this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                offset = 0;

                // build list of potential adpcm start indexes
                potentialPbd = new Psf.ProbableItemStruct();

                // build list of potential adpcm start indexes
                while ((offset = ParseFile.GetNextOffset(fs, offset, Psf.VB_START_BYTES, pspStruct.UseZeroOffsetForPbd, 0x10L, 0x00L, true)) > -1)
                {
                    try
                    {
                        pbdRow = ParseFile.ParseSimpleOffset(fs, offset, pbdRow.Length);

                        if (Psf.IsPotentialAdpcm(fs, offset + 0x10, Psf.MIN_ADPCM_ROW_COUNT, false))
                        {
                            potentialPbd.offset = offset;
                            emptyRowList.Add(potentialPbd);
                        }

                    }
                    catch (Exception pbdEx)
                    {
                        this.progressStruct.Clear();
                        this.progressStruct.ErrorMessage = String.Format(" ERROR finding BD for <{0}> at Offset 0x{1}: {2}{3}", pPath, offset.ToString("X8"), pbdEx.Message, Environment.NewLine);
                        this.ReportProgress(this.progress, this.progressStruct);
                    }

                    offset += 1;
                }

                potentialPbdList = (Psf.ProbableItemStruct[])emptyRowList.ToArray(typeof(Psf.ProbableItemStruct));

                // set probable lengths
                for (int i = 0; i < potentialPbdList.Length; i++)
                {
                    if (i > 0)
                    {
                        potentialPbdList[i - 1].length = (uint)(potentialPbdList[i].offset - potentialPbdList[i - 1].offset);
                    }
                }

                // compare PHD sample sizes to potential adpcm sizes/indexes
                phdObject.startingOffset = 0;
                phdObject.length = 0;
                phdObject.pbdStartingOffset = 0;
                phdObject.pbdLength = 0;

                string pbdName;
                string newFileName;
                string[] dupeFileNames;

                for (int i = 0; i < phdArrayList.Count; i++)
                {
                    phdObject = (PphdStruct)phdArrayList[i];
                    if (phdObject.vagLengths.Length < 1)
                    {
                        this.progressStruct.Clear();
                        this.progressStruct.ErrorMessage = String.Format(" ERROR building PBD for <{0}>: {1} refers to a single VAG, cannot determine proper PBD.  Skipping...{2}", pPath, phdObject.FileName, Environment.NewLine);
                        this.ReportProgress(this.progress, this.progressStruct);
                    }
                    else
                    {
                        for (int j = 0; j < potentialPbdList.Length; j++)
                        {
                            // we have a potential match or are at the last item.
                            if ((phdObject.vagLengths[0] <= potentialPbdList[j].length) ||
                                (potentialPbdList[j].length == 0))
                            {
                                try
                                {
                                    phdObject = PopulatePbdOffsetLength(fs, potentialPbdList, j, phdObject);

                                    if (phdObject.pbdLength > 0)
                                    {
                                        // check for other BD files that matched and rename accordingly
                                        dupeFileNames = Directory.GetFiles(destinationFolder, Path.GetFileNameWithoutExtension(phdObject.FileName) + "*.PBD");

                                        if (dupeFileNames.Length >= 1)
                                        {
                                            pbdName = String.Format("{0}_{1}.PBD", Path.GetFileNameWithoutExtension(phdObject.FileName), (dupeFileNames.Length).ToString("X4"));

                                            if (dupeFileNames.Length == 1)
                                            {
                                                // rename existing
                                                newFileName = String.Format("{0}_{1}.PBD", Path.GetFileNameWithoutExtension(phdObject.FileName), (dupeFileNames.Length - 1).ToString("X4"));
                                                File.Move(dupeFileNames[0], Path.Combine(Path.GetDirectoryName(dupeFileNames[0]), newFileName));
                                            }
                                        }
                                        else
                                        {
                                            pbdName = Path.ChangeExtension(phdObject.FileName, ".PBD");
                                        }


                                        ParseFile.ExtractChunkToFile(fs, phdObject.pbdStartingOffset, phdObject.pbdLength,
                                            Path.Combine(destinationFolder, pbdName), true, true);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    this.progressStruct.Clear();
                                    this.progressStruct.ErrorMessage = String.Format("     ERROR building BD for <{0}>: {1}{2}", pPath, ex.Message, Environment.NewLine);
                                    this.ReportProgress(this.progress, this.progressStruct);
                                }
                            }
                        }
                    }
                }

                #endregion
            }
        }

        public static PphdStruct GetPphdData(FileStream fs, long offset)
        {
            PphdStruct phdObject = new PphdStruct();

            // save off info
            phdObject.startingOffset = offset;

            phdObject.progSectionOffset = offset + BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, offset + 0x10, 4), 0);
            phdObject.toneSectionOffset = offset + BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, offset + 0x14, 4), 0);
            phdObject.vagSectionOffset = offset + BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, offset + 0x18, 4), 0);

            phdObject.maxVagInfoNumber = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, phdObject.vagSectionOffset + 0x14, 4), 0);
            phdObject.vagInfoSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, phdObject.vagSectionOffset + 8, 4), 0);

            phdObject.vagOffsets = new long[phdObject.maxVagInfoNumber + 1];
            phdObject.vagLengths = new long[phdObject.maxVagInfoNumber + 1];
            phdObject.expectedPbdLength = 0;
            phdObject.IsSmallSamplePresent = false;

            for (int i = 0; i <= phdObject.maxVagInfoNumber; i++)
            {
                phdObject.vagOffsets[i] = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, phdObject.vagSectionOffset + 0x20 + (i * phdObject.vagInfoSize) + 0, 4), 0);
                phdObject.vagLengths[i] = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, phdObject.vagSectionOffset + 0x20 + (i * phdObject.vagInfoSize) + 8, 4), 0);
                phdObject.expectedPbdLength += phdObject.vagLengths[i];

                if (phdObject.vagLengths[i] < Psf.MIN_ADPCM_ROW_SIZE)
                {
                    phdObject.IsSmallSamplePresent = true;
                }
            }

            phdObject.length = phdObject.vagSectionOffset - offset + 0x20 + ((phdObject.maxVagInfoNumber + 1) * phdObject.vagInfoSize);

            return phdObject;
        }

        private PphdStruct PopulatePbdOffsetLength(Stream searchStream,
            Psf.ProbableItemStruct[] potentialPbdList, int potentialPbdStartIndex,
            PphdStruct phdObject)
        {
            PphdStruct ret = phdObject;
            long totalLength = 0;
            byte[] lastLine = new byte[Psf.SONY_ADPCM_ROW_SIZE];
            string errorMessage;

            for (int i = 0; i < phdObject.vagLengths.Length; i++)
            {
                if ((potentialPbdStartIndex + i) >= potentialPbdList.Length)
                {
                    errorMessage = String.Format("  Warning, a potential PBD match for {0} found at 0x{1}, " +
                        "but index would exceed array bounds.  It is suggested that you check manually if a match " +
                        "is not found at completion.{2}", phdObject.FileName,
                        potentialPbdList[potentialPbdStartIndex].offset.ToString("X8"),
                        Environment.NewLine);
                    throw new IndexOutOfRangeException(errorMessage);
                }
                else
                {
                    totalLength += phdObject.vagLengths[i];

                    if (i == (phdObject.vagLengths.Length - 1))
                    {
                        // check last value
                        searchStream.Position =
                            potentialPbdList[potentialPbdStartIndex + i].offset + phdObject.vagLengths[i] +
                            (phdObject.expectedPbdLength - totalLength) - lastLine.Length;
                        searchStream.Read(lastLine, 0, lastLine.Length);

                        if (lastLine[1] == 3 ||
                            ParseFile.CompareSegment(lastLine, 0, Psf.VB_END_BYTES_1) ||
                            ParseFile.CompareSegment(lastLine, 0, Psf.VB_END_BYTES_2))
                        {
                            ret.pbdStartingOffset = potentialPbdList[potentialPbdStartIndex].offset;
                            ret.pbdLength = phdObject.expectedPbdLength;
                        }
                        else // reset in case a match has already been found for this HD
                        {
                            ret.pbdStartingOffset = 0;
                            ret.pbdLength = 0;
                        }
                    }
                    else if (phdObject.vagLengths[i] != potentialPbdList[potentialPbdStartIndex + i].length)
                    {
                        // if we have a small sample, and a minimum number of matches, check the expected length
                        if (phdObject.IsSmallSamplePresent)
                        {
                            double matchPercentage = (double)i / (double)phdObject.vagLengths.Length;

                            if (matchPercentage >= Psf.MIN_SAMPLE_MATCH_PERCENTAGE)
                            {
                                // check last row for expected length
                                searchStream.Position = potentialPbdList[potentialPbdStartIndex].offset + phdObject.expectedPbdLength - lastLine.Length;
                                searchStream.Read(lastLine, 0, lastLine.Length);

                                if (lastLine[1] == 3 ||
                                    ParseFile.CompareSegment(lastLine, 0, Psf2.VB_END_BYTES_1) ||
                                    ParseFile.CompareSegment(lastLine, 0, Psf2.VB_END_BYTES_2))
                                {
                                    ret.pbdStartingOffset = potentialPbdList[potentialPbdStartIndex].offset;
                                    ret.pbdLength = phdObject.expectedPbdLength;

                                    this.progressStruct.Clear();
                                    this.progressStruct.GenericMessage = String.Format("     PHD <{0}> contains a sample smaller than 0x{1}, partial matching will be used.  Be sure to thoroughly listen to assembled files.{2}", ret.FileName, Psf2.MIN_ADPCM_ROW_SIZE.ToString("X8"), Environment.NewLine);
                                    this.ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                                }
                                else // reset in case a match has already been found for this HD
                                {
                                    ret.pbdStartingOffset = 0;
                                    ret.pbdLength = 0;
                                }
                            }
                            else // reset in case a match has already been found for this HD
                            {
                                ret.pbdStartingOffset = 0;
                                ret.pbdLength = 0;
                            }
                        }
                        else
                        {
                            ret.pbdStartingOffset = -1;
                            ret.pbdLength = -1;
                        }

                        break;
                    }
                }
            }

            return ret;
        }
    }
}