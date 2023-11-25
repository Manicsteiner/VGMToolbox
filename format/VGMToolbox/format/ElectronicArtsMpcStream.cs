using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class ElectronicArtsMpcStream
    {
        public const string DefaultAudioExtension = ".asf";
        public const string DefaultVideoExtension = ".mpc";

        protected static readonly byte[] MPCH_BYTES = new byte[] { 0x4D, 0x50, 0x43, 0x68 };

        protected static readonly byte[] SCHl_BYTES = new byte[] { 0x53, 0x43, 0x48, 0x6C };
        protected static readonly byte[] SCCl_BYTES = new byte[] { 0x53, 0x43, 0x43, 0x6C };
        protected static readonly byte[] SCDl_BYTES = new byte[] { 0x53, 0x43, 0x44, 0x6C };
        protected static readonly byte[] SCLl_BYTES = new byte[] { 0x53, 0x43, 0x4C, 0x6C };
        protected static readonly byte[] SCEl_BYTES = new byte[] { 0x53, 0x43, 0x45, 0x6C };
        protected static readonly byte[] SCxx_BYTES = new byte[] { 0x53, 0x43 };

        //protected static readonly byte[] SHEN_BYTES = new byte[] { 0x53, 0x48, 0x45, 0x4E };
        //protected static readonly byte[] SCEN_BYTES = new byte[] { 0x53, 0x43, 0x45, 0x4E };
        //protected static readonly byte[] SDEN_BYTES = new byte[] { 0x53, 0x44, 0x45, 0x4E };
        //protected static readonly byte[] SEEN_BYTES = new byte[] { 0x53, 0x45, 0x45, 0x4E };
        //protected static readonly byte[] SHxx_BYTES = new byte[] { 0x53, 0x48 };

        public string FilePath { get; set; }
        public string FileExtensionAudio { get; set; }
        public string FileExtensionVideo { get; set; }

        public ElectronicArtsMpcStream(string path)
        {
            this.FilePath = path;
            this.FileExtensionAudio = DefaultAudioExtension;
            this.FileExtensionVideo = DefaultVideoExtension;
        }

        protected bool IsThisAnAudioBlock(byte[] blockToCheck)
        {
            return (ParseFile.CompareSegment(blockToCheck, 0, ElectronicArtsMpcStream.SCxx_BYTES));/* ||
                    ParseFile.CompareSegment(blockToCheck, 0, ElectronicArtsMpcStream.SHEN_BYTES) ||
                    ParseFile.CompareSegment(blockToCheck, 0, ElectronicArtsMpcStream.SCEN_BYTES) ||
                    ParseFile.CompareSegment(blockToCheck, 0, ElectronicArtsMpcStream.SDEN_BYTES) ||
                    ParseFile.CompareSegment(blockToCheck, 0, ElectronicArtsMpcStream.SEEN_BYTES)); */
        }
        protected bool IsThisAVideoBlock(byte[] blockToCheck)
        {
            return ParseFile.CompareSegment(blockToCheck, 0, ElectronicArtsMpcStream.MPCH_BYTES);
        }

        protected void DoFinalTasks(Dictionary<string, FileStream> outputFiles)
        {
            foreach (string streamId in outputFiles.Keys)
            {
                outputFiles[streamId].Close();
                outputFiles[streamId].Dispose();
            }
        }

        private void writeChunkToStream(
            byte[] chunk,
            string chunkId,
            Dictionary<string, FileStream> streamWriters,
            string fileExtension)
        {
            string destinationFile;

            if (!streamWriters.ContainsKey(chunkId))
            {
                destinationFile = Path.Combine(Path.GetDirectoryName(this.FilePath),
                    String.Format("{0}.{1}{2}", Path.GetFileNameWithoutExtension(this.FilePath), chunkId, fileExtension));
                streamWriters[chunkId] = File.Open(destinationFile, FileMode.Create, FileAccess.ReadWrite);
            }

            streamWriters[chunkId].Write(chunk, 0, chunk.Length);
        }

        public virtual void DemultiplexStreams(MpegStream.DemuxOptionsStruct demuxOptions)
        {
            long currentOffset = -1;
            long fileSize;

            byte[] chunkId;
            byte[] fullChunk;
            uint chunkSize = 0;

            long mpchOffset = -1;
            long schlOffset = -1;
            // long shenOffset = -1;

            Dictionary<string, FileStream> streamWriters = new Dictionary<string, FileStream>();

            try
            {
                using (FileStream fs = File.OpenRead(this.FilePath))
                {
                    fileSize = fs.Length;

                    // find header first header (audio or video)                  
                    mpchOffset = ParseFile.GetNextOffset(fs, 0, ElectronicArtsMpcStream.MPCH_BYTES);

                    if (mpchOffset == 0) // video header comes first
                    {
                        currentOffset = mpchOffset;
                    }
                    else // audio header comes first
                    {
                        schlOffset = ParseFile.GetNextOffset(fs, 0, ElectronicArtsMpcStream.SCHl_BYTES);

                        if ((schlOffset > -1) && (schlOffset < mpchOffset))
                        {
                            currentOffset = schlOffset;
                        }
                        //else
                        //{
                        //    shenOffset = ParseFile.GetNextOffset(fs, 0, ElectronicArtsMpcStream.SHEN_BYTES);

                        //    if ((shenOffset > -1) && (shenOffset < mpchOffset))
                        //    {
                        //        currentOffset = shenOffset;
                        //    }
                        //}
                    }

                    // verify headers found                    
                    if (mpchOffset >= 0)
                    {
                        if (currentOffset >= 0)
                        {
                            // process file
                            while (currentOffset < fileSize)
                            {
                                // get chunk
                                chunkId = ParseFile.ParseSimpleOffset(fs, currentOffset, 4);

                                // get chunk size
                                chunkSize = (uint)BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, currentOffset + 4, 4), 0);

                                // audio chunk
                                if (demuxOptions.ExtractAudio && this.IsThisAnAudioBlock(chunkId))
                                {
                                    fullChunk = ParseFile.ParseSimpleOffset(fs, currentOffset, (int)chunkSize);
                                    this.writeChunkToStream(fullChunk, "audio", streamWriters, this.FileExtensionAudio);
                                }

                                // video chunk
                                if (demuxOptions.ExtractVideo && this.IsThisAVideoBlock(chunkId))
                                {
                                    fullChunk = ParseFile.ParseSimpleOffset(fs, currentOffset, (int)chunkSize);
                                    this.writeChunkToStream(fullChunk, "video", streamWriters, this.FileExtensionVideo);
                                }

                                // unknown chunk
                                if (!this.IsThisAnAudioBlock(chunkId) && !this.IsThisAVideoBlock(chunkId))
                                {
                                    fullChunk = ParseFile.ParseSimpleOffset(fs, currentOffset, (int)chunkSize);
                                    this.writeChunkToStream(fullChunk, "unknown", streamWriters, ".bin");
                                }

                                // move to next chunk
                                currentOffset += (long)chunkSize;
                            }
                        }
                        else
                        {
                            throw new Exception(String.Format("Cannot find MVHD or SCHl headers.{0}", Environment.NewLine));
                        }
                    }
                    else
                    {
                        throw new Exception(String.Format("Cannot find MPCh header.{0}", Environment.NewLine));
                    } // if (mpchOffset >= 0)
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.DoFinalTasks(streamWriters);
            }
        }
    }

}
