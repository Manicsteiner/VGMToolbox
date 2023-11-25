using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class NintendoThpMovieStream
    {
        public enum ThpVersion
        {
            Version10,
            Version11,
        }

        public static readonly byte[] MAGIC_BYTES = new byte[] { 0x54, 0x48, 0x50, 0x00 }; // THP\0
        public static readonly byte[] VERSON_11 = new byte[] { 0x00, 0x01, 0x10, 0x00 };
        public static readonly byte[] VERSON_10 = new byte[] { 0x00, 0x01, 0x00, 0x00 };

        public const byte VIDEO_COMPONENT = 0;
        public const byte AUDIO_COMPONENT = 1;

        public ThpVersion Version { set; get; }
        public bool ContainsAudio { set; get; }
        public uint MaxBufferSize { set; get; }
        public uint MaxAudioSamples { set; get; }

        public float Fps { set; get; }
        public uint NumberOfFrames { set; get; }
        public uint FirstFrameSize { set; get; }

        public uint DataSize { set; get; }
        public uint ComponentDataOffset { set; get; }
        public byte[] ComponentTypes { set; get; }

        public uint FirstFrameOffset { set; get; }
        public uint LastFrameOffset { set; get; }

        public uint ComponentCount { set; get; }

        // audio
        public uint NumberOfChannels { set; get; }
        public uint Frequency { set; get; }
        public uint NumberOfSamples { set; get; }
        public uint NumberOfAudioBlocksPerFrame { set; get; }

        // video
        public uint Width { set; get; }
        public uint Height { set; get; }
        public uint Unknown { set; get; }

        public string FilePath { set; get; }
        public string FileExtensionAudio { set; get; }
        public string FileExtensionVideo { set; get; }

        public NintendoThpMovieStream(string path)
        {            
            this.FilePath = path;
            this.FileExtensionVideo = ".video.thp";
            this.FileExtensionAudio = ".audio.thp";
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

        private void ReadHeader(FileStream thpStream, long currentOffset)
        {
            byte[] magicBytes = ParseFile.ParseSimpleOffset(thpStream, currentOffset, MAGIC_BYTES.Length);
            byte[] tempChunk;

            if (ParseFile.CompareSegment(magicBytes, 0, MAGIC_BYTES))
            {
                tempChunk = ParseFile.ParseSimpleOffset(thpStream, currentOffset + 4, 4);
                
                if (ParseFile.CompareSegment(tempChunk, 0, VERSON_10))
                {
                    this.Version = ThpVersion.Version10;
                }
                else
                {
                    this.Version = ThpVersion.Version11;
                }

                this.MaxBufferSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, currentOffset + 8, 4));
                this.MaxAudioSamples = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, currentOffset + 0xC, 4));
                this.ContainsAudio = (this.MaxAudioSamples > 0);

                // public float Fps { set; get; }
                this.NumberOfFrames = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, currentOffset + 0x14, 4));
                this.FirstFrameSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, currentOffset + 0x18, 4));

                this.DataSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, currentOffset + 0x1C, 4));
                this.ComponentDataOffset = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, currentOffset + 0x20, 4));
                this.ComponentTypes = ParseFile.ParseSimpleOffset(thpStream, currentOffset + 0x24, 16);

                this.FirstFrameOffset = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, currentOffset + 0x28, 4));
                this.LastFrameOffset = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, currentOffset + 0x2C, 4));
            }
            else
            {
                throw new FormatException("Magic bytes 'THP\\0' not found.");
            }
        }

        private void ParseComponents(FileStream thpStream)
        {
            this.ComponentCount = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, this.ComponentDataOffset, 4));
            
            byte componentType;
            long componentDetailsOffset = this.ComponentDataOffset + 4 + 0x10;

            // load components
            for (int i = 0; i < this.ComponentCount; i++)
            {
                componentType = ParseFile.ParseSimpleOffset(thpStream, this.ComponentDataOffset + 4 + (i * 1), 1)[0];
                
                if (componentType == VIDEO_COMPONENT)
                {                    
                    this.Width = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, componentDetailsOffset, 4));
                    this.Height = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, componentDetailsOffset + 4, 4));

                    if (this.Version == ThpVersion.Version11)
                    {
                        this.Unknown = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, componentDetailsOffset + 8, 4));
                        componentDetailsOffset += 0xC;
                    }
                    else
                    {
                        this.Unknown = 0;
                        componentDetailsOffset += 0x8;
                    }
                }
                else if (componentType == AUDIO_COMPONENT)
                {                    
                    this.NumberOfChannels  = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, componentDetailsOffset, 4));
                    this.Frequency = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, componentDetailsOffset + 4, 4));
                    this.NumberOfSamples = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, componentDetailsOffset + 8, 4));

                    if (this.Version == ThpVersion.Version11)
                    {
                        this.NumberOfAudioBlocksPerFrame = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(thpStream, componentDetailsOffset + 0xC, 4));
                        componentDetailsOffset += 0x10;
                    }
                    else
                    {
                        this.NumberOfAudioBlocksPerFrame = 1; // 1 audio chunk per frame
                        componentDetailsOffset += 0xC;
                    }                   
                }
            }
        }

        private byte[] RemoveAudioInfoFromThpHeader(byte[] dirtyThpHeader)
        {
            byte[] dataBytes;
            byte[] fourEmptyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            
            // clear max audio samples
            Array.Copy(fourEmptyBytes, 0, dirtyThpHeader, 0xC, 4);

            // reset components
            Array.Copy(fourEmptyBytes, 0, dirtyThpHeader, this.ComponentDataOffset, 4);
            dirtyThpHeader[this.ComponentDataOffset + 3] = 0x01;
            dirtyThpHeader[this.ComponentDataOffset + 4] = 0x00; // set to video
            dirtyThpHeader[this.ComponentDataOffset + 5] = 0xFF;

            // add video details in case it was the second component
            dataBytes = ByteConversion.GetBytesBigEndian(this.Width);
            Array.Copy(dataBytes, 0, dirtyThpHeader, 0x44, 4);

            dataBytes = ByteConversion.GetBytesBigEndian(this.Height);
            Array.Copy(dataBytes, 0, dirtyThpHeader, 0x48, 4);

            if (this.Version == ThpVersion.Version11)
            {
                dataBytes = ByteConversion.GetBytesBigEndian(this.Unknown);
                Array.Copy(dataBytes, 0, dirtyThpHeader, 0x4C, 4);            
            }

            // remove audio component details
            if (this.ContainsAudio)
            {
                if (this.Version == ThpVersion.Version10)
                {
                    for (int i = 0x4C; i < this.FirstFrameOffset; i++)
                    {
                        dirtyThpHeader[i] = 0;
                    }
                }
                else // version 1.1
                {
                    for (int i = 0x50; i < this.FirstFrameOffset; i++)
                    {
                        dirtyThpHeader[i] = 0;
                    }                
                }
            }

            return dirtyThpHeader;
        }

        private byte[] RemoveVideoInfoFromThpHeader(byte[] dirtyThpHeader)
        {
            byte[] dataBytes;
            byte[] fourEmptyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };

            // reset components
            Array.Copy(fourEmptyBytes, 0, dirtyThpHeader, this.ComponentDataOffset, 4);
            dirtyThpHeader[this.ComponentDataOffset + 3] = 0x01;
            dirtyThpHeader[this.ComponentDataOffset + 4] = 0x01; // set to audio
            dirtyThpHeader[this.ComponentDataOffset + 5] = 0xFF;

            // add audio details in case it was the second component
            dataBytes = ByteConversion.GetBytesBigEndian(this.NumberOfChannels);
            Array.Copy(dataBytes, 0, dirtyThpHeader, 0x44, 4);

            dataBytes = ByteConversion.GetBytesBigEndian(this.Frequency);
            Array.Copy(dataBytes, 0, dirtyThpHeader, 0x48, 4);

            dataBytes = ByteConversion.GetBytesBigEndian(this.NumberOfSamples);
            Array.Copy(dataBytes, 0, dirtyThpHeader, 0x4C, 4);

            if (this.Version == ThpVersion.Version11)
            {
                dataBytes = ByteConversion.GetBytesBigEndian(this.NumberOfAudioBlocksPerFrame);
                Array.Copy(dataBytes, 0, dirtyThpHeader, 0x50, 4);

                for (int i = 0x54; i < this.FirstFrameOffset; i++)
                {
                    dirtyThpHeader[i] = 0;
                }
            }
            else
            {
                for (int i = 0x50; i < this.FirstFrameOffset; i++)
                {
                    dirtyThpHeader[i] = 0;
                }            
            }
                       
            return dirtyThpHeader;
        }

        public void DemultiplexStreams(MpegStream.DemuxOptionsStruct demuxOptions)
        {
            long currentOffset = 0;
            uint frameCount = 1;
            uint nextFrameSize;
            byte[] currentFrame;

            byte[] videoChunkSizeBytes = null;
            uint videoChunkSize;
            byte[] audioChunkSizeBytes = null;
            uint audioChunkSize;
            long dataStart;
            byte[] videoChunk;
            byte[] audioChunk;

            // bool isAudioHeaderWritten = false;
            Dictionary<string, bool> isAudioHeaderWritten = new Dictionary<string, bool>();
            bool isVideoHeaderWritten = false;
            byte[] thpHeader;
            byte[] firstFrameSize;
            long headerLocation;

            uint previousFrameSizeVideo = 0;
            uint previousFrameSizeAudio = 0;
            uint nextFrameSizeVideo;
            uint nextFrameSizeAudio;

            byte[] previousFrameSizeBytes;
            byte[] nextFrameSizeBytes;

            uint totalDataSize = 0;
            byte[] totalDataSizeBytes;
            
            byte[] fourEmptyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };

            long videoFrameOffset = 0;
            long audioFrameOffset = 0;
            byte[] lastOffsetBytes;
            
            string audioStreamHashKey;
            byte[] bigEndianOne = new byte[] { 0x00, 0x00, 0x00, 0x01 };

            Dictionary<string, FileStream> streamWriters = new Dictionary<string, FileStream>();

            try
            {
                using (FileStream fs = File.OpenRead(this.FilePath))
                {
                    headerLocation = ParseFile.GetNextOffset(fs, currentOffset, MAGIC_BYTES);
                    currentOffset = headerLocation;

                    if (currentOffset > -1)
                    {
                        // read header
                        this.ReadHeader(fs, currentOffset);
                        nextFrameSize = this.FirstFrameSize;

                        // get component info
                        this.ParseComponents(fs);

                        // process frames
                        currentOffset = this.FirstFrameOffset;

                        while (currentOffset <= this.LastFrameOffset)
                        {
                            // read frame
                            currentFrame = ParseFile.ParseSimpleOffset(fs, (long)currentOffset, (int)nextFrameSize);

                            // get size of next frame
                            nextFrameSize = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(currentFrame, 0, 4));
                            
                            // get size of next audio/video frame (for writing output frame headers)
                            if (frameCount < this.NumberOfFrames)
                            {
                                nextFrameSizeVideo = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(fs, currentOffset + currentFrame.Length + 8, 4));
                                nextFrameSizeAudio = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(fs, currentOffset + currentFrame.Length + 0xC, 4));
                            }
                            else
                            {
                                nextFrameSizeVideo = 0;
                                nextFrameSizeAudio = 0;
                            }

                            videoChunkSizeBytes = ParseFile.ParseSimpleOffset(currentFrame, 8, 4);
                            videoChunkSize = ByteConversion.GetUInt32BigEndian(videoChunkSizeBytes);

                            if (this.ContainsAudio)
                            {
                                audioChunkSizeBytes = ParseFile.ParseSimpleOffset(currentFrame, 0xC, 4);
                                audioChunkSize = ByteConversion.GetUInt32BigEndian(audioChunkSizeBytes);
                                dataStart = 0x10;
                            }
                            else
                            {
                                audioChunkSize = 0;
                                dataStart = 0xC;
                            }

                            #region WRITE VIDEO
                            ///////////////
                            // write video
                            ///////////////
                            if (demuxOptions.ExtractVideo)
                            {
                                if (streamWriters.ContainsKey("video"))
                                {
                                    videoFrameOffset = streamWriters["video"].Length;
                                }

                                // attach THP header
                                if (!isVideoHeaderWritten)
                                {
                                    // get original header
                                    thpHeader = ParseFile.ParseSimpleOffset(fs, headerLocation, (int)(fs.Length - this.DataSize));

                                    // clean out audio info
                                    thpHeader = this.RemoveAudioInfoFromThpHeader(thpHeader);
                                    
                                    // update first frame size in header
                                    firstFrameSize = ByteConversion.GetBytesBigEndian((uint)(videoChunkSize + 0xC));
                                    Array.Copy(firstFrameSize, 0, thpHeader, 0x18, 4);

                                    // write updated header
                                    this.writeChunkToStream(thpHeader, "video", streamWriters, this.FileExtensionVideo);
                                    isVideoHeaderWritten = true;

                                }
                                
                                // add frame header
                                
                                // write next frame size
                                nextFrameSizeBytes = ByteConversion.GetBytesBigEndian((uint)(nextFrameSizeVideo + 0xC));
                                this.writeChunkToStream(nextFrameSizeBytes, "video", streamWriters, this.FileExtensionVideo);

                                // write previous frame size
                                previousFrameSizeBytes = ByteConversion.GetBytesBigEndian((uint)(previousFrameSizeVideo + 0xC));
                                this.writeChunkToStream(nextFrameSizeBytes, "video", streamWriters, this.FileExtensionVideo);

                                // write video size
                                this.writeChunkToStream(videoChunkSizeBytes, "video", streamWriters, this.FileExtensionVideo);

                                // write data
                                videoChunk = ParseFile.ParseSimpleOffset(currentFrame, (int)dataStart, (int)videoChunkSize);
                                this.writeChunkToStream(videoChunk, "video", streamWriters, this.FileExtensionVideo);

                                // save previous bytes for next frame
                                previousFrameSizeVideo = videoChunkSize;
                            }
                            #endregion

                            #region WRITE AUDIO
                            ///////////////
                            // write audio
                            ///////////////
                            if (demuxOptions.ExtractAudio && this.ContainsAudio)
                            {
                                //if (this.NumberOfAudioBlocksPerFrame > 1)
                                //{
                                //    int x = 1;
                                //}

                                // add blocks
                                for (int i = 0; i < this.NumberOfAudioBlocksPerFrame; i++)
                                {
                                    audioStreamHashKey = String.Format("track_{0}", i.ToString("D2"));
                             
                                    // write file header
                                    if (streamWriters.ContainsKey(audioStreamHashKey))
                                    {
                                        audioFrameOffset = streamWriters[audioStreamHashKey].Position;
                                    }

                                    // attach THP header
                                    if (!isAudioHeaderWritten.ContainsKey(audioStreamHashKey) || 
                                        isAudioHeaderWritten[audioStreamHashKey] == false)
                                    {
                                        // get original header
                                        thpHeader = ParseFile.ParseSimpleOffset(fs, headerLocation, (int)(fs.Length - this.DataSize));

                                        // clean out video info
                                        thpHeader = this.RemoveVideoInfoFromThpHeader(thpHeader);

                                        // update first frame size in header
                                        firstFrameSize = ByteConversion.GetBytesBigEndian((uint)(audioChunkSize + 0x10));
                                        Array.Copy(firstFrameSize, 0, thpHeader, 0x18, 4);

                                        // set NumberOfAudioBlocksPerFrame to 1
                                        Array.Copy(bigEndianOne, 0, thpHeader, 0x50, 4);

                                        // write updated header
                                        this.writeChunkToStream(thpHeader, audioStreamHashKey, streamWriters, this.FileExtensionAudio);
                                        isAudioHeaderWritten.Add(audioStreamHashKey, true);
                                    }
                                    
                                    //////////////////////
                                    // write frame header
                                    //////////////////////

                                    // write next frame size
                                    nextFrameSizeBytes = ByteConversion.GetBytesBigEndian((uint)(nextFrameSizeAudio + 0x10));
                                    this.writeChunkToStream(nextFrameSizeBytes, audioStreamHashKey, streamWriters, this.FileExtensionAudio);
                                    
                                    // write previous frame size
                                    previousFrameSizeBytes = ByteConversion.GetBytesBigEndian((uint)(previousFrameSizeAudio + 0x10));
                                    this.writeChunkToStream(nextFrameSizeBytes, audioStreamHashKey, streamWriters, this.FileExtensionAudio);
                                    
                                    // write video size (zero)
                                    this.writeChunkToStream(fourEmptyBytes, audioStreamHashKey, streamWriters, this.FileExtensionAudio);
                                    
                                    // write audio size for this frame
                                    this.writeChunkToStream(audioChunkSizeBytes, audioStreamHashKey, streamWriters, this.FileExtensionAudio);
                                    
                                    // write chunk
                                    audioChunk = ParseFile.ParseSimpleOffset(currentFrame, (int)(dataStart + videoChunkSize + (i * audioChunkSize)), (int)audioChunkSize);
                                    this.writeChunkToStream(audioChunk, audioStreamHashKey, streamWriters, this.FileExtensionAudio);

                                    // set previous frame size for next frame
                                    previousFrameSizeAudio = audioChunkSize;
                                }
                            }
                            #endregion

                            // increment offset and frame counter
                            currentOffset += currentFrame.Length;
                            frameCount++;
                        }                        
                    
                        // fix headers as needed

                        // data size
                        foreach (string key in streamWriters.Keys)
                        {
                            totalDataSize = (uint)(streamWriters[key].Length - this.FirstFrameOffset);
                            totalDataSizeBytes = ByteConversion.GetBytesBigEndian(totalDataSize);

                            streamWriters[key].Position = 0x1C;
                            streamWriters[key].Write(totalDataSizeBytes, 0, 4);
                        }

                        // frame offsets
                        for (int i = 0; i < this.NumberOfAudioBlocksPerFrame; i++)
                        {
                            audioStreamHashKey = String.Format("audio_{0}", i.ToString("DD"));
                            
                            if (streamWriters.ContainsKey(audioStreamHashKey))
                            {
                                lastOffsetBytes = ByteConversion.GetBytesBigEndian((uint)audioFrameOffset);

                                streamWriters[audioStreamHashKey].Position = 0x2C;
                                streamWriters[audioStreamHashKey].Write(lastOffsetBytes, 0, 4);
                            }
                        }

                        if (streamWriters.ContainsKey("video"))
                        {
                            lastOffsetBytes = ByteConversion.GetBytesBigEndian((uint)videoFrameOffset);

                            streamWriters["video"].Position = 0x2C;
                            streamWriters["video"].Write(lastOffsetBytes, 0, 4);                        
                        }

                    }
                    else
                    {
                        throw new FormatException("Cannot find THP header.");
                    }

                } // using (FileStream fs = File.OpenRead(this.FilePath))
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                foreach (string key in streamWriters.Keys)
                {
                    // close writers
                    if (streamWriters[key] != null &&
                        streamWriters[key].CanWrite)
                    {
                        streamWriters[key].Close();
                        streamWriters[key].Dispose();
                    }
                }
            }
        }
    }
}
