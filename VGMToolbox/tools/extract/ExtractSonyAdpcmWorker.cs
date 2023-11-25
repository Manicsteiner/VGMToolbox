using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.extract
{
    class ExtractSonyAdpcmWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct ExtractSonyAdpcmStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }
            public bool OutputBatchFiles { set; get; }
            public long StartOffset { set; get; }
        }

        public ExtractSonyAdpcmWorker() :
            base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pExtractSonyAdpcmStruct, DoWorkEventArgs e)
        {
            ExtractSonyAdpcmStruct extractSonyAdpcmStruct = (ExtractSonyAdpcmStruct)pExtractSonyAdpcmStruct;

            long offset = extractSonyAdpcmStruct.StartOffset;
            ArrayList adpcmList = new ArrayList();
            Psf.ProbableItemStruct adpcmDataItem = new Psf.ProbableItemStruct();

            int fileCount = 0;
            string outputPath = Path.Combine(Path.GetDirectoryName(pPath), "_sony_adpcm_ext");
            string outputFileName;
            string outputFilePath;

            bool previousRowIsZeroes = false;

            FileInfo fi = new FileInfo(pPath);            

            using (FileStream fs = File.Open(pPath, FileMode.Open, FileAccess.Read))
            {
                while (offset < fi.Length)
                { 
                    if (!CancellationPending)
                    {
                        if (ExtractSonyAdpcmWorker.IsPotentialAdpcm(fs, offset, Psf.MIN_ADPCM_ROW_SIZE, false, ref previousRowIsZeroes))
                        {
                            // create probable adpcm item
                            adpcmDataItem.Init();
                            
                            // set starting offset
                            adpcmDataItem.offset = offset;
                        
                            // move to next row
                            offset += Psf.SONY_ADPCM_ROW_SIZE;

                            // loop until end
                            while (Psf.IsSonyAdpcmRow(fs, offset))
                            {
                                offset += Psf.SONY_ADPCM_ROW_SIZE;
                            }

                            adpcmDataItem.length = (uint)(offset - adpcmDataItem.offset);
                            adpcmList.Add(adpcmDataItem);
                        }

                        offset += 1;
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }


                // extract files
                foreach (Psf.ProbableItemStruct p in adpcmList)
                {
                    outputFileName = String.Format("{0}_{1}.bin", Path.GetFileNameWithoutExtension(pPath), fileCount.ToString("X8"));
                    outputFilePath = Path.Combine(outputPath, outputFileName);

                    ParseFile.ExtractChunkToFile(fs, p.offset, (int)p.length, outputFilePath, 
                        extractSonyAdpcmStruct.OutputBatchFiles, extractSonyAdpcmStruct.OutputBatchFiles);

                    fileCount++;
                }
            }
        }

        public static bool IsPotentialAdpcm(Stream searchStream, long offset, 
            int rowsToCheck, bool doAdditionalChecks, ref bool previousRowIsZeroes)
        {
            bool ret = true;
            byte[] checkBytes = new byte[0x10 * rowsToCheck];
            int bytesRead;

            searchStream.Position = offset;
            bytesRead = searchStream.Read(checkBytes, 0, checkBytes.Length);

            // check for rows meeting criteria
            for (int i = 0; i < rowsToCheck; i++)
            {
                if (ParseFile.CompareSegmentUsingSourceOffset(checkBytes, (i * 0x10), 
                        Psf.SONY_ADPCM_ROW_SIZE, Psf.VB_START_BYTES))
                {
                    if (previousRowIsZeroes)
                    {
                        ret = false;
                        break;
                    }
                    else
                    {
                        previousRowIsZeroes = true;
                    }
                }
                else
                {
                    previousRowIsZeroes = false;

                    if ((bytesRead < ((i * 0x10) + 0x10)) ||
                             (!IsSonyAdpcmRow(checkBytes, doAdditionalChecks, i)))
                    {
                        ret = false;
                        break;
                    }
                }
            }

            return ret;
        }

        public static bool IsSonyAdpcmRow(byte[] potentialAdpcm, bool doAdditionalChecks, int rowNumber)
        {
            bool ret = true;

            if ((potentialAdpcm[((rowNumber * 0x10) + 1)] > 7) ||
                (potentialAdpcm[(rowNumber * 0x10)] > 0x4C))
            {
                ret = false;
            }
            else if (doAdditionalChecks &&
                     ((potentialAdpcm[0] == 0) && (potentialAdpcm[1] != 2) && (Psf.GetCountOfZeroBytes(potentialAdpcm) > 14)))
            {
                ret = false;
            }
            return ret;
        }
    }
}
