using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class MobiclipAudioStreamFeatures
    {
        public ushort StreamType { set; get; }
        public uint Frequency { set; get; }
        public uint Channels { set; get; }
    }
    
    public abstract class MobiclipStream
    {
        public enum MovieType
        { 
            NintendoDs,
            Wii,
            Unknown
        }

        public struct ChunkStruct
        {
            public byte[] Chunk { set; get; }
            public uint ChunkId { set; get; }
        }

        public static readonly byte[] MagicBytes = new byte[] { 0x4D, 0x4F };
        public static readonly byte[] DataStartBytes = new byte[] { 0x48, 0x45, 0x00, 0x00 };
        
        public long HeaderSize { set; get; }

        public uint VideoStreamCount { set; get; }
        public uint AudioStreamCount { set; get; }
        public MobiclipAudioStreamFeatures[] AudioStreamFeatures { set; get; }

        public string FilePath { get; set; }
        public string FileExtensionAudio { get; set; }
        public string FileExtensionVideo { get; set; }

        protected abstract long GetBlockSize(FileStream inStream, long currentOffset);

        protected abstract ChunkStruct GetVideoChunk(FileStream inStream, long currentOffset);

        protected abstract ChunkStruct[] GetAudioChunk(FileStream inStream, long currentOffset, long blockSize, long videoChunkSize);

        protected virtual void ReadHeader(FileStream inStream, long offset) { }

        protected virtual void DoFinalTasks(Dictionary<uint, FileStream> streamWriters,
            MpegStream.DemuxOptionsStruct demuxOptions)
        {
            foreach (uint key in streamWriters.Keys)
            {
                if (streamWriters[key] != null &&
                    streamWriters[key].CanRead)
                {
                    streamWriters[key].Close();
                    streamWriters[key].Dispose();
                }
            }
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

        public virtual void DemultiplexStreams(MpegStream.DemuxOptionsStruct demuxOptions)
        {
            long currentOffset = 0;
            long fileSize;

            long blockSize;
            long videoChunkSize;

            Dictionary<uint, FileStream> streamOutputWriters = new Dictionary<uint, FileStream>();

            ChunkStruct currentChunk = new ChunkStruct();
            ChunkStruct[] audioChunks;

            try
            {
                using (FileStream fs = File.OpenRead(this.FilePath))
                {
                    this.ReadHeader(fs, currentOffset);
                    
                    fileSize = fs.Length;
                    currentOffset = ParseFile.GetNextOffset(fs, currentOffset, DataStartBytes) + 4;

                    while (currentOffset < fileSize)
                    {

                        if (currentOffset == 0x4C7DC)
                        {
                            int ccc = 1;
                        }
                        
                        blockSize = this.GetBlockSize(fs, currentOffset);

                        if (blockSize > 0)
                        {
                            // write video block to stream                        
                            currentChunk = this.GetVideoChunk(fs, currentOffset);

                            if (demuxOptions.ExtractVideo && (currentChunk.Chunk != null))
                            {
                                this.writeChunkToStream(currentChunk.Chunk, currentChunk.ChunkId, streamOutputWriters, this.FileExtensionVideo);
                            }
                            // NDS -- currentBlockId = (uint)(BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(fs, currentOffset, 2), 0) & 0x00FF);


                            // write audio block to stream
                            if (demuxOptions.ExtractAudio && (this.AudioStreamCount > 0))
                            {
                                videoChunkSize = currentChunk.Chunk == null ? 0 : currentChunk.Chunk.Length;
                                audioChunks = this.GetAudioChunk(fs, currentOffset, blockSize, videoChunkSize);

                                for (uint i = 0; i < this.AudioStreamCount; i++)
                                {
                                    if (audioChunks[i].Chunk != null)
                                    {
                                        this.writeChunkToStream(audioChunks[i].Chunk, audioChunks[i].ChunkId, streamOutputWriters, this.FileExtensionAudio);
                                    }
                                }
                            }

                            // move offset
                            currentOffset += blockSize;
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
        

        public static MovieType GetMobiclipStreamType(string path)
        {
            MovieType streamType = MovieType.Unknown;

            using (FileStream fs = File.OpenRead(path))
            {
                byte[] typeBytes = ParseFile.ParseSimpleOffset(fs, 2, 2);

                if (ParseFile.CompareSegment(typeBytes, 0, MobiclipNdsStream.StreamTypeBytes))
                {
                    streamType = MovieType.NintendoDs;
                }
                else if (ParseFile.CompareSegment(typeBytes, 0, MobiclipWiiStream.StreamTypeBytes))
                {
                    streamType = MovieType.Wii;
                }
            }

            return streamType;
        }        
    }
}
