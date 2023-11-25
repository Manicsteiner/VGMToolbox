using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class MobiclipNdsStream : MobiclipStream
    {
        public static readonly byte[] StreamTypeBytes = new byte[] { 0x44, 0x53 };

        public MobiclipNdsStream(string path)
        {
            this.FilePath = path;
            this.FileExtensionAudio = ".audio.bin";
            this.FileExtensionVideo = ".video.bin";
        }

        protected override long GetBlockSize(FileStream inStream, long currentOffset)
        {
            long blockSize = (long)(BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(inStream, currentOffset + 2, 2), 0) * 4);
            blockSize += 4;
            
            return blockSize;
        }

        protected override ChunkStruct[] GetAudioChunk(FileStream inStream, long currentOffset, long blockSize, long videoChunkSize)
        {
            // @TODO: Fix this for multi-audio streams
            ChunkStruct[] audioChunkStructs = new ChunkStruct[1];

            long chunkSize = (long)BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(inStream, currentOffset, 2), 0);
            chunkSize *= 4;

            if (chunkSize > 0)
            {
                chunkSize += 8;
            }

            byte[] audioChunk = ParseFile.ParseSimpleOffset(inStream, currentOffset + 4, (int)chunkSize);

            audioChunkStructs[0] = new ChunkStruct();
            audioChunkStructs[0].Chunk = audioChunk;
            audioChunkStructs[0].ChunkId = (uint)BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(inStream, currentOffset, 2), 0);

            return audioChunkStructs;        
        }

        protected override ChunkStruct GetVideoChunk(FileStream inStream, long currentOffset)
        {
            ChunkStruct videoChunkStruct = new ChunkStruct();
            
            long blockSize = (long)(BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(inStream, currentOffset + 2, 2), 0) * 4);
            blockSize += 4;
            
            long audioChunkSize = (long)(BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(inStream, currentOffset, 2), 0) * 4);

            if (audioChunkSize > 0)
            {
                audioChunkSize += 8;
            }

            byte[] videoChunk = ParseFile.ParseSimpleOffset(inStream, currentOffset + 4 + audioChunkSize, (int)(blockSize - audioChunkSize));

            videoChunkStruct.Chunk = videoChunk;
            videoChunkStruct.ChunkId = (uint)BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(inStream, currentOffset, 2), 0);

            return videoChunkStruct;
        }
    
    }
}
