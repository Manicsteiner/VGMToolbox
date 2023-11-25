using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class RacjinDsiStream
    {
        const string DefaultFileExtensionAudio = ".mib";
        const string DefaultFileExtensionVideo = ".m2v";
        const long BlockAlignment = 0x20000;

        public struct StreamChunkStruct
        {
            public uint RelativeOffset { set; get; }
            public long AbsoluteOffset { set; get; }
            public byte[] StreamChunkType { set; get; }
            public uint StreamChunkId { set; get; }
            public uint ChunkSize { set; get; }
            public byte[] ChunkData { set; get; }
        }

        public RacjinDsiStream(string filePath)
        {
            this.FilePath = filePath;
            this.FileExtensionAudio = RacjinDsiStream.DefaultFileExtensionAudio;
            this.FileExtensionVideo = RacjinDsiStream.DefaultFileExtensionVideo;
        }

        public string FilePath { get; set; }
        public string FileExtensionAudio { get; set; }
        public string FileExtensionVideo { get; set; }

        private StreamChunkStruct[] parseStreamChunk(FileStream inStream, long chunkStartOffset)
        {
            ArrayList streamChunks = new ArrayList(); ;
            StreamChunkStruct sc;
            
            uint streamCount = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(inStream, chunkStartOffset, 4), 0);

            for (int i = 0; i < streamCount; i++)
            {
                sc = new StreamChunkStruct();

                sc.RelativeOffset = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(inStream, chunkStartOffset + (i * 0xC) + 4, 4), 0);
                sc.AbsoluteOffset = chunkStartOffset + (long)sc.RelativeOffset;
                sc.StreamChunkType = ParseFile.ParseSimpleOffset(inStream, chunkStartOffset + (i * 0xC) + 8, 4);
                sc.StreamChunkId = BitConverter.ToUInt32(sc.StreamChunkType, 0);
                sc.ChunkSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(inStream, chunkStartOffset + (i * 0xC) + 0xC, 4), 0);
                sc.ChunkData = ParseFile.ParseSimpleOffset(inStream, sc.AbsoluteOffset, (int)sc.ChunkSize);

                streamChunks.Add(sc);
            }

            return (StreamChunkStruct[])streamChunks.ToArray(typeof(StreamChunkStruct));
        }

        // best guesses being used here due to limited samples
        private bool isAudioBlock(byte[] chunkTypeBytes)
        {
            return ((chunkTypeBytes[1] >= 0xD0) && (chunkTypeBytes[1] <= 0xEF));
        }

        // best guesses being used here due to limited samples
        private bool isVideoBlock(byte[] chunkTypeBytes)
        {
            return ((chunkTypeBytes[1] >= 0xC0) && (chunkTypeBytes[1] <= 0xCF));
        }

        private void writeChunkToStream(
            byte[] chunk,
            uint chunkId,
            Dictionary<uint, FileStream> streamWriters,
            string fileExtension)
        {
            string destinationFile;

            if (!streamWriters.ContainsKey(chunkId))
            {
                destinationFile = Path.Combine(Path.GetDirectoryName(this.FilePath),
                    String.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(this.FilePath), chunkId.ToString("X4"), fileExtension));
                streamWriters[chunkId] = File.Open(destinationFile, FileMode.Create, FileAccess.ReadWrite);
            }

            streamWriters[chunkId].Write(chunk, 0, chunk.Length);

        }

        protected virtual void DoFinalTasks(Dictionary<uint, FileStream> streamWriters,
            MpegStream.DemuxOptionsStruct demuxOptions)
        {
            GenhCreationStruct gcStruct;
            string sourceFile;
            string genhFile;

            foreach (uint key in streamWriters.Keys)
            {
                // create GENH since MIB support is spotty       
                if (demuxOptions.AddHeader)
                {
                    if (streamWriters[key].Name.EndsWith(this.FileExtensionAudio))
                    {                        
                        sourceFile = streamWriters[key].Name;

                        streamWriters[key].Close();
                        streamWriters[key].Dispose();

                        gcStruct = new GenhCreationStruct();
                        gcStruct.Format = "0x00";
                        gcStruct.HeaderSkip = "0";
                        gcStruct.Interleave = "0x100";
                        gcStruct.Channels = "2";
                        gcStruct.Frequency = "48000";
                        gcStruct.NoLoops = true;

                        genhFile = GenhUtil.CreateGenhFile(sourceFile, gcStruct);

                        // delete original file
                        if (!String.IsNullOrEmpty(genhFile))
                        {
                            File.Delete(sourceFile);
                        }
                    }
                }
                                
                // close streams if open
                if (streamWriters[key] != null &&
                    streamWriters[key].CanRead)
                {
                    streamWriters[key].Close();
                    streamWriters[key].Dispose();
                }
            }
        }

        public virtual void DemultiplexStreams(MpegStream.DemuxOptionsStruct demuxOptions)
        {
            long currentOffset = 0;
            long fileSize;
            long blockEndOffset;

            StreamChunkStruct[] streamChunks;

            Dictionary<uint, FileStream> streamOutputWriters = new Dictionary<uint, FileStream>();

            try
            {
                using (FileStream fs = File.OpenRead(this.FilePath))
                {
                    fileSize = fs.Length;
                    currentOffset = 0;

                    while (currentOffset < fileSize)
                    {
                        blockEndOffset = 0;
                        streamChunks = parseStreamChunk(fs, currentOffset);

                        if (streamChunks.Length > 0)
                        {
                            foreach (StreamChunkStruct sc in streamChunks)
                            {
                                blockEndOffset = sc.AbsoluteOffset + sc.ChunkSize;

                                if (this.isVideoBlock(sc.StreamChunkType))
                                {
                                    if (demuxOptions.ExtractVideo)
                                    {
                                        this.writeChunkToStream(sc.ChunkData, sc.StreamChunkId, streamOutputWriters, RacjinDsiStream.DefaultFileExtensionVideo);
                                    }
                                }
                                else if (this.isAudioBlock(sc.StreamChunkType))
                                {
                                    if (demuxOptions.ExtractAudio)
                                    {
                                        this.writeChunkToStream(sc.ChunkData, sc.StreamChunkId, streamOutputWriters, RacjinDsiStream.DefaultFileExtensionAudio);
                                    }
                                }
                                else
                                {
                                    throw new Exception(String.Format("Exception processing block at offset 0x{0}: {1}{2}", currentOffset.ToString("X"), "Unknown Stream Type Identifier", Environment.NewLine));
                                }
                            }

                            // move offset
                            currentOffset = blockEndOffset;
                            currentOffset = (currentOffset + RacjinDsiStream.BlockAlignment - 1) / RacjinDsiStream.BlockAlignment * RacjinDsiStream.BlockAlignment;
                        }
                        else
                        {
                            break;
                        }
                    }

                } // using (FileStream fs = File.OpenRead(this.FilePath))
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Exception processing block at offset 0x{0}: {1}{2}", currentOffset.ToString("X"), ex.Message, Environment.NewLine));
            }
            finally
            {
                // clean up
                this.DoFinalTasks(streamOutputWriters, demuxOptions);
            }
        }
    }
}
