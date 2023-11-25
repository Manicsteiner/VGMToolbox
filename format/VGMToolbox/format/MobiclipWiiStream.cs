using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class MobiclipWiiStream : MobiclipStream
    {
        public static readonly byte[] StreamTypeBytes = new byte[] { 0x43, 0x35 }; //C5
        
        public const ushort AudioChunkSignature = 0x414D; // AM
        public const ushort AudioChunkSignaturePcm = 0x4150;   // AP
        public const ushort AudioChunkSignatureA3 = 0x4133;   // A3

        public const string FileExtensionAudioA3 = ".a3.raw";
        public const uint VideoChunkId = 0xFFFF;

        public MobiclipWiiStream(string path)
        {
            this.FilePath = path;
            this.FileExtensionAudio = ".raw";
            this.FileExtensionVideo = ".bin";
        }

        protected override long GetBlockSize(FileStream inStream, long currentOffset)
        {
            long blockSize = (long)BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(inStream, currentOffset, 4), 0);

            if (blockSize > 0)
            {
                blockSize += 4;

                if ((blockSize % 4) != 0)
                {
                    blockSize -= (blockSize % 4);
                }
            }
            return blockSize;
        }

        protected override void ReadHeader(FileStream inStream, long offset) 
        { 
            long currentOffset = offset;
            
            ushort chunkTypeId;
            ushort subChunkCount;

            // read header size
            this.HeaderSize = ParseFile.ReadUintLE(inStream, currentOffset + 4) + 8;
            currentOffset += 8;

            while (currentOffset < this.HeaderSize)
            { 
                // read chunk type id
                chunkTypeId = ParseFile.ReadUshortBE(inStream, currentOffset);

                // read subchunk count
                subChunkCount = ParseFile.ReadUshortLE(inStream, currentOffset + 2);

                switch (chunkTypeId)
                { 
                    // get audio details
                    case MobiclipWiiStream.AudioChunkSignature:
                        this.AudioStreamCount = ParseFile.ReadUintLE(inStream, currentOffset + 4);
                        this.AudioStreamFeatures = new MobiclipAudioStreamFeatures[this.AudioStreamCount];

                        for (uint i = 0; i < this.AudioStreamCount; i++)
                        {
                            this.AudioStreamFeatures[i] = new MobiclipAudioStreamFeatures();
                            this.AudioStreamFeatures[i].StreamType = ParseFile.ReadUshortBE(inStream, currentOffset + 8 + (i * 0xA));
                            this.AudioStreamFeatures[i].Frequency = ParseFile.ReadUintLE(inStream, currentOffset + 8 + (i * 0xA) + 2);
                            this.AudioStreamFeatures[i].Channels = ParseFile.ReadUintLE(inStream, currentOffset + 8 + (i * 0xA) + 6);
                        }

                        break;                
                    
                    case MobiclipWiiStream.AudioChunkSignaturePcm:
                    case MobiclipWiiStream.AudioChunkSignatureA3:
                        this.AudioStreamCount = 1;
                        this.AudioStreamFeatures = new MobiclipAudioStreamFeatures[this.AudioStreamCount];
                        this.AudioStreamFeatures[0] = new MobiclipAudioStreamFeatures();
                        this.AudioStreamFeatures[0].StreamType = ParseFile.ReadUshortBE(inStream, currentOffset);
                        this.AudioStreamFeatures[0].Frequency = ParseFile.ReadUintLE(inStream, currentOffset + 4);
                        this.AudioStreamFeatures[0].Channels = ParseFile.ReadUintLE(inStream, currentOffset + 8);                                                                        
                        break;
                                        
                    default:                        
                        break;
                }

                currentOffset += (subChunkCount * 4) + 4;            
            }
        }

        protected override ChunkStruct GetVideoChunk(FileStream inStream, long currentOffset)
        {
            ChunkStruct videoChunkStruct = new ChunkStruct();

            long chunkSize = (long)ParseFile.ReadUintLE(inStream, currentOffset + 4);
            byte[] videoChunk = ParseFile.ParseSimpleOffset(inStream, currentOffset + 8, (int)chunkSize);

            videoChunkStruct.Chunk = videoChunk;
            videoChunkStruct.ChunkId = MobiclipWiiStream.VideoChunkId;
            
            return videoChunkStruct;        
        }

        protected override ChunkStruct[] GetAudioChunk(FileStream inStream, long currentOffset, long blockSize, long videoChunkSize)
        {
            ChunkStruct[] audioChunkStructs = new ChunkStruct[this.AudioStreamCount];

            long absoluteOffset = currentOffset + 8 + videoChunkSize + 4;
            uint baseChunkId;// = ParseFile.ReadUintLE(inStream, absoluteOffset - 4);
            baseChunkId = 0;
            uint chunkSize = 0;

            for (uint i = 0; i < this.AudioStreamCount; i++)
            {
                audioChunkStructs[i] = new ChunkStruct();
                
                // Untested for file with more than 2 streams
                if (this.AudioStreamCount > 1)
                {
                    if (i == 0)
                    {
                        // read chunk size before first audio stream, should be the same for each.
                        chunkSize = ParseFile.ReadUintLE(inStream, absoluteOffset);
                        absoluteOffset += 4;
                    }                        
                }
                else
                {
                    chunkSize = (uint)((currentOffset + blockSize) - absoluteOffset);
                }

                if (chunkSize > 0)
                {                    
                    audioChunkStructs[i].ChunkId = baseChunkId + i;
                    audioChunkStructs[i].Chunk = ParseFile.ParseSimpleOffset(inStream, absoluteOffset, (int)chunkSize);
                }

                absoluteOffset += chunkSize;
            }
            
            //int chunkSize = (int)(blockSize - videoChunkSize - 8 - 4);

            //if (chunkSize > 0)
            //{                
            //    audioChunk = ParseFile.ParseSimpleOffset(inStream, absoluteOffset, chunkSize);

            //    audioChunkStruct.Chunk = audioChunk;
            //    audioChunkStructs[0].ChunkId = ParseFile.ReadUintLE(inStream, absoluteOffset - 4);
            //    audioChunkStruct.ChunkId = 0;
            //}

            return audioChunkStructs;
        }

        protected override void DoFinalTasks(Dictionary<uint, FileStream> streamWriters,
            MpegStream.DemuxOptionsStruct demuxOptions)
        {
            GenhCreationStruct gcStruct;
            ushort frequency;
            string sourceFile;
            string genhFile = null;

            string rawFile;

            foreach (uint key in streamWriters.Keys)
            {
                if (demuxOptions.AddHeader &&
                   (key != MobiclipWiiStream.VideoChunkId) && 
                   (this.AudioStreamFeatures[key].StreamType != null) &&
                   (this.AudioStreamFeatures[key].StreamType == AudioChunkSignaturePcm))
                {
                    if (streamWriters[key].Name.EndsWith(this.FileExtensionAudio))
                    {
                        sourceFile = streamWriters[key].Name;

                        streamWriters[key].Close();
                        streamWriters[key].Dispose();

                        gcStruct = new GenhCreationStruct();

                        switch (this.AudioStreamFeatures[key].StreamType)
                        {
                            case AudioChunkSignaturePcm:
                                gcStruct.Format = "0x04";

                                gcStruct.HeaderSkip = "0";
                                gcStruct.Interleave = "0x2";
                                gcStruct.Channels = this.AudioStreamFeatures[key].Channels.ToString();
                                gcStruct.Frequency = this.AudioStreamFeatures[key].Frequency.ToString();
                                gcStruct.NoLoops = true;

                                genhFile = GenhUtil.CreateGenhFile(sourceFile, gcStruct);

                                break;
                            
                            default:
                                break;
                        }
                        
                        // delete original file
                        if (!String.IsNullOrEmpty(genhFile))
                        {
                            File.Delete(sourceFile);
                        }
                    }
                }
                else if (key != MobiclipWiiStream.VideoChunkId)
                { 
                    // update raw file extension
                    if (this.AudioStreamFeatures[key].StreamType == AudioChunkSignatureA3)
                    {
                        rawFile = streamWriters[key].Name;
                        streamWriters[key].Close();
                        streamWriters[key].Dispose();

                        File.Copy(rawFile, Path.ChangeExtension(rawFile, FileExtensionAudioA3));
                        File.Delete(rawFile);
                    }
                }
            }

            base.DoFinalTasks(streamWriters, demuxOptions);
        }
    }
}
