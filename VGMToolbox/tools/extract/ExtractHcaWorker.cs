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
    class ExtractHcaWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        static readonly byte[] HCA_SIG_BYTES = new byte[] { 0x48, 0x43, 0x41, 0x00 };
        static readonly byte[] FMT_CHUNK_BYTES = new byte[] { 0x66, 0x6D, 0x74, 0x00 };
        static readonly byte[] DEC_CHUNK_BYTES = new byte[] { 0x64, 0x65, 0x63, 0x00 };
        static readonly byte[] COMP_CHUNK_BYTES = new byte[] { 0x63, 0x6F, 0x6D, 0x70 };
        static readonly byte[] MASK_BYTES = new byte[] { 0x7F, 0x7F, 0x7F, 0x7F };

        const long MAX_HEADER_SIZE = 0x20000; // just a guess, never seen more than 0x1000;

        public struct ExtractHcaStruct : IVgmtWorkerStruct
        {
            public string[] SourcePaths { set; get; }
        }

        public ExtractHcaWorker() :
            base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pExtractAdxStruct, DoWorkEventArgs e)
        {
            ExtractHcaStruct extractAdxStruct = (ExtractHcaStruct)pExtractAdxStruct;

            long offset = 0;

            byte revisionMajor;
            byte revisionMinor;
            ushort dataOffset;

            long fmtChunkOffset;

            uint blockCount;
            ushort blockSize;
            
            long fileSize;

            int fileCount = 0;
            string outputPath = Path.Combine(Path.GetDirectoryName(pPath), "_cri_hca_ext");
            string outputFileName;
            string outputFilePath;

            FileInfo fi = new FileInfo(pPath);

            using (FileStream fs = File.Open(pPath, FileMode.Open, FileAccess.Read))
            {
                outputPath = Path.Combine(Path.GetDirectoryName(pPath), String.Format("{0}_HCAs", Path.GetFileNameWithoutExtension(pPath)));

                while ((offset = ParseFile.GetNextOffsetMasked(fs, offset, HCA_SIG_BYTES, MASK_BYTES)) > -1)
                {
                    if (!this.CancellationPending)
                    {
                        // get version
                        revisionMajor = ParseFile.ReadByte(fs, offset + 4);
                        revisionMinor = ParseFile.ReadByte(fs, offset + 5);

                        // get data offset
                        dataOffset = ParseFile.ReadUshortBE(fs, offset + 6);

                        // get 'fmt' chunk offset
                        fmtChunkOffset = ParseFile.GetNextOffsetMasked(fs, offset, FMT_CHUNK_BYTES, MASK_BYTES);

                        if (fmtChunkOffset > -1)
                        {
                            // get block count
                            blockCount = ParseFile.ReadUintBE(fs, fmtChunkOffset + 8);

                            
                            // get block size
                            blockSize = this.getBlockSize(fs, offset);          


                            // calculate file size
                            fileSize = dataOffset + (blockCount * blockSize);

                            // extract file
                            outputFileName = String.Format("{0}_{1}.hca", Path.GetFileNameWithoutExtension(pPath), fileCount.ToString("X8"));
                            outputFilePath = Path.Combine(outputPath, outputFileName);

                            this.progressStruct.Clear();
                            this.progressStruct.GenericMessage = String.Format("{0} - offset: 0x{1} size: 0x{2}{3}", outputFileName, offset.ToString("X8"), fileSize.ToString("X8"), Environment.NewLine);
                            ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);

                            ParseFile.ExtractChunkToFile(fs, offset, fileSize, outputFilePath, true, true);

                            // increment counter
                            fileCount++;

                            // move pointer
                            offset += fileSize;
                        }
                        else
                        { 
                            throw new FormatException(String.Format("'fmt' chunk not found for HCA starting at 0x{0}", offset.ToString("X8")));
                        }                        

                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private ushort getBlockSize(Stream inStream, long hcaOffset)
        {
            ushort blockSize = 0;
            
            long decChunkOffset;
            long compChunkOffset;
            
            //----------------
            // 'dec ' offset 
            //----------------

            // get 'dec' chunk offset, if exists (v1.3, maybe others?)
            decChunkOffset = ParseFile.GetNextOffsetWithLimitMasked(inStream, hcaOffset, 
                hcaOffset + MAX_HEADER_SIZE, DEC_CHUNK_BYTES, MASK_BYTES, true);

            if (decChunkOffset > -1)
            {
                blockSize = ParseFile.ReadUshortBE(inStream, decChunkOffset + 4);
            }
            else
            {
                //----------------
                // 'comp' offset 
                //----------------

                // get 'comp' chunk offset, if exists (v1.3, maybe others?)
                compChunkOffset = ParseFile.GetNextOffsetWithLimitMasked(inStream, hcaOffset,
                    hcaOffset + MAX_HEADER_SIZE, COMP_CHUNK_BYTES, MASK_BYTES, true);

                if (compChunkOffset > -1)
                {
                    blockSize = ParseFile.ReadUshortBE(inStream, compChunkOffset + 4);
                }
                else
                { 
                    throw new FormatException(
                        String.Format("Cannot find 'dec' or 'comp' chunk to determine block size for HCA starting at 0x{0}", hcaOffset.ToString("X8")));
                }
            }

            return blockSize;
        }
    }
}
