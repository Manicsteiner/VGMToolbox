using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public class Ps2SequenceData
    {
        public static readonly byte[] SIGNATURE_BYTES = new byte[] { 0x49, 0x45, 0x43, 0x53,
                                                                     0x75, 0x71, 0x65, 0x53 };
        
        private const int HEADER_CHUNK_OFFSET = 0x10;
        private const int MIDI_CHUNK_OFFSET = 0x30;

        private const int EMPTY_MIDI_OFFSET = -1;

        static readonly byte[] SET_TEMPO_BYTES = new byte[] { 0xFF, 0x51, 0x03 };
        static readonly byte[] END_OF_TRACK_BYTES = new byte[] { 0xFF, 0x2F, 0x00 };

        private VersionChunkStruct versionChunk;
        private SequChunkStruct headerChunk;
        private MidiChunkStruct midiChunk;

        private Dictionary<int, int> dataBytesPerCommand;

        public struct VersionChunkStruct
        {
            public UInt32 magicBytes;
            public UInt32 magicBytesSection;
            public UInt32 chunkSize;
            public UInt32 unknown;
        }

        public struct SequChunkStruct
        {
            public UInt32 magicBytes;
            public UInt32 magicBytesSection;
            public UInt32 chunkSize;
            public UInt32 fileSize;

            public UInt32 songChunkAddrAbsolute;
            public UInt32 midiChunkAddrAbsolute;
            public UInt32 seSequenceChunkAddrAbsolute;
            public UInt32 seSongChunkAddrAbsolute;
        }

        public struct MidiChunkStruct
        {
            public UInt32 magicBytes;
            public UInt32 magicBytesSection;
            public UInt32 chunkSize;
            public UInt32 maxSeqCount;

            public Int32[] subSeqOffsetAddr; // relative to chunk MIDI_CHUNK_OFFSET
        }

        public struct MidiDataChunkStruct
        {
            public UInt32 sequenceOffsetRelativeToChunk;
            public UInt16 resolution;
        }

        public struct Ps2SqTimingStruct
        {
            public double TimeInSeconds;
            public int FadeInSeconds;
            public string Warnings;
        }

        public Ps2SequenceData(Stream pStream)
        { 
            this.versionChunk = parseVersionChunk(pStream);
            this.headerChunk = parseHeaderChunk(pStream);
            this.midiChunk = parseMidiChunk(pStream);

            dataBytesPerCommand = new Dictionary<int, int>();
            dataBytesPerCommand.Add(0x80, 1);
            dataBytesPerCommand.Add(0x90, 2);
            dataBytesPerCommand.Add(0xA0, 2);
            dataBytesPerCommand.Add(0xB0, 2);
            dataBytesPerCommand.Add(0xC0, 1);
            dataBytesPerCommand.Add(0xD0, 1);
            dataBytesPerCommand.Add(0xE0, 2);
            dataBytesPerCommand.Add(0xF0, 0);
        }
        
        public static VersionChunkStruct parseVersionChunk(Stream pStream)
        {
            VersionChunkStruct ret = new VersionChunkStruct();

            ret.magicBytes = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, 0, 4), 0);
            ret.magicBytesSection = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, 4, 4), 0);
            ret.chunkSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, 8, 4), 0);
            ret.unknown = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(pStream, 12, 4), 0);

            return ret;
        }

        public static SequChunkStruct parseHeaderChunk(Stream pStream)
        {
            SequChunkStruct ret = new SequChunkStruct();

            ret.magicBytes = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, HEADER_CHUNK_OFFSET + 0, 4), 0);
            ret.magicBytesSection = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, HEADER_CHUNK_OFFSET + 4, 4), 0);
            ret.chunkSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, HEADER_CHUNK_OFFSET + 8, 4), 0);
            ret.fileSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, HEADER_CHUNK_OFFSET + 12, 4), 0);

            ret.songChunkAddrAbsolute = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, HEADER_CHUNK_OFFSET + 16, 4), 0);
            ret.midiChunkAddrAbsolute = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, HEADER_CHUNK_OFFSET + 20, 4), 0);
            ret.seSequenceChunkAddrAbsolute = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, HEADER_CHUNK_OFFSET + 24, 4), 0);
            ret.seSongChunkAddrAbsolute = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, HEADER_CHUNK_OFFSET + 28, 4), 0);

            return ret;
        }

        public static MidiChunkStruct parseMidiChunk(Stream pStream)
        {
            MidiChunkStruct ret = new MidiChunkStruct();

            ret.magicBytes = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, MIDI_CHUNK_OFFSET + 0, 4), 0);
            ret.magicBytesSection = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, MIDI_CHUNK_OFFSET + 4, 4), 0);
            ret.chunkSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, MIDI_CHUNK_OFFSET + 8, 4), 0);
            ret.maxSeqCount = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, MIDI_CHUNK_OFFSET + 12, 4), 0);

            ret.subSeqOffsetAddr = new int[ret.maxSeqCount + 1];

            for (int i = 0; i < ret.subSeqOffsetAddr.Length; i++)
            {
                ret.subSeqOffsetAddr[i] = BitConverter.ToInt32(ParseFile.ParseSimpleOffset(pStream, MIDI_CHUNK_OFFSET + 16 + (4 * i), 4), 0);
            }

            return ret;
        }


        public int GetMaxSequenceCount()
        {
            return (int)this.midiChunk.maxSeqCount;
        }
        
        public UInt32 getTempoForSequenceNumber(Stream pStream, int pSequenceNumber)
        {
            UInt32 ret = 0;

            if (pSequenceNumber <= this.midiChunk.maxSeqCount &&
                this.midiChunk.subSeqOffsetAddr[pSequenceNumber] != EMPTY_MIDI_OFFSET)
            {
                Int32 midiBlockOffset = MIDI_CHUNK_OFFSET + this.midiChunk.subSeqOffsetAddr[pSequenceNumber];

                long setTempoOffset = ParseFile.GetNextOffset(pStream, (long)midiBlockOffset, SET_TEMPO_BYTES);
                
                byte[] tempoValBytes = new byte[4];
                Array.Copy(ParseFile.ParseSimpleOffset(pStream, setTempoOffset + 3, 3), 0, tempoValBytes, 1, 3);
                
                Array.Reverse(tempoValBytes); // flip order to LE for use with BitConverter
                ret = BitConverter.ToUInt32(tempoValBytes, 0);
            }

            return ret;
        }

        public UInt32 getEndOfTrackForSequenceNumber(Stream pStream, int pSequenceNumber)
        {
            UInt32 ret = 0;

            if (pSequenceNumber <= this.midiChunk.maxSeqCount &&
                this.midiChunk.subSeqOffsetAddr[pSequenceNumber] != EMPTY_MIDI_OFFSET)
            {
                Int32 midiBlockOffset = MIDI_CHUNK_OFFSET + this.midiChunk.subSeqOffsetAddr[pSequenceNumber];

                long endOfTrackOffset = ParseFile.GetNextOffset(pStream, (long)midiBlockOffset, END_OF_TRACK_BYTES);

                ret = (UInt32)endOfTrackOffset;
            }

            return ret;
        }

        public Ps2SqTimingStruct getTimeInSecondsForSequenceNumber(Stream pStream, int pSequenceNumber)
        {
            Ps2SqTimingStruct ret = new Ps2SqTimingStruct();

            if (pSequenceNumber <= this.midiChunk.maxSeqCount &&
                this.midiChunk.subSeqOffsetAddr[pSequenceNumber] != EMPTY_MIDI_OFFSET)
            {
                uint tempo = this.getTempoForSequenceNumber(pStream, pSequenceNumber);
                long eofOffset = this.getEndOfTrackForSequenceNumber(pStream, pSequenceNumber);
                Int32 midiBlockOffset = MIDI_CHUNK_OFFSET + this.midiChunk.subSeqOffsetAddr[pSequenceNumber];

                MidiDataChunkStruct dataChunkHeader = new MidiDataChunkStruct();
                dataChunkHeader.sequenceOffsetRelativeToChunk = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(pStream, (long)midiBlockOffset, 4), 0);
                dataChunkHeader.resolution = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(pStream, (long)(midiBlockOffset + 4), 2), 0);

                if (dataChunkHeader.sequenceOffsetRelativeToChunk == 6) // uncompressed
                {
                    ret = getTimeInSecondsForChunk(pStream, (long)(midiBlockOffset + dataChunkHeader.sequenceOffsetRelativeToChunk), 
                        eofOffset, tempo, dataChunkHeader.resolution);
                }
                else
                {
                    throw new Exception("Compressed Sequences Not Supported.");
                }
            }

            return ret;
        }

        private Ps2SqTimingStruct getTimeInSecondsForChunk(Stream pStream, long pStartOffset, long pEndOffset, uint pTempo, 
            ushort pResolution)
        {
            long incomingStreamPosition = pStream.Position;
            
            int currentByte;
            long currentOffset = 0;
            long iterationOffset = 0;
            long commandOffset = 0;
            int dataByte1;
            int dataByte2;

            int runningCommand = 0;
            int dataByteCount;
            uint tempo = 0;

            int metaCommandByte;
            int metaCommandLengthByte;

            bool running = false;
            bool emptyTimeFollows = false;
            
            bool loopFound = false;
            bool loopEndFound = false;
            double loopTime;

            int loopTimeMultiplier;
            Stack<double> loopTimeStack = new Stack<double>();
            ulong loopTicks;
            Stack<ulong> loopTickStack = new Stack<ulong>();

            int loopsOpened = 0;
            int loopsClosed = 0;

            UInt64 currentTicks;
            UInt64 totalTicks = 0;

            double currentTime;
            double totalTime = 0;
            double timeSinceLastLoopEnd = 0; // used for extra Loop End tag hack

            byte[] tempoValBytes;
            Ps2SqTimingStruct ret = new Ps2SqTimingStruct();
            ret.Warnings = String.Empty;

            pStream.Position = pStartOffset;

            while (pStream.Position < pEndOffset)
            {
                iterationOffset = pStream.Position;
                
                // get time
                if (!emptyTimeFollows)
                {
                    currentByte = pStream.ReadByte();
                    currentOffset = pStream.Position - 1;

                    // build 7-bit num from bytes (variable length string)
                    if ((currentByte & 0x80) != 0)
                    {
                        currentTicks = (ulong)(currentByte & 0x7F);

                        do
                        {
                            currentByte = pStream.ReadByte();
                            currentOffset = pStream.Position - 1;

                            currentTicks = (currentTicks << 7) + (ulong)(currentByte & 0x7F);
                        } while ((currentByte & 0x80) != 0);
                    }
                    else // only one byte, no need for conversion
                    {
                        currentTicks = (ulong)currentByte;
                    }

                    if ((tempo == 0) && (currentTicks != 0))
                    {
                        throw new Exception("Tempo not set and current ticks not equal zero");
                    }
                    
                    currentTime = currentTicks != 0? (double)((currentTicks * (ulong) tempo) / (ulong) pResolution) : 0;

                    //if (currentTime > 10000000) // debug code used to find strange values
                    //{
                    //    int x2 = 1;
                    //}

                    if (loopTimeStack.Count > 0)
                    {
                        loopTime = loopTimeStack.Pop();
                        loopTime += currentTime;
                        loopTimeStack.Push(loopTime);

                        loopTicks = loopTickStack.Pop();
                        loopTicks += currentTicks;
                        loopTickStack.Push(loopTicks);
                    }
                    else
                    {
                        if (pStream.Position != pEndOffset) // time at the end of the file is ignored
                        {
                            totalTime += currentTime;                            
                            totalTicks += (ulong)currentTicks;

                            timeSinceLastLoopEnd += currentTime;
                        }
                        else
                        {
                            goto DONE;
                        }

                    }                                        
                }

                // get command
                currentByte = pStream.ReadByte();
                currentOffset = pStream.Position - 1;

                //if (currentOffset > 0x19B5) // code to quickly get to a position for debugging
                //{
                //    int x = 1;
                //}


                //if (currentByte != 0xFF) // process normal commands
                //{
                    if ((currentByte & 0x80) == 0) // data byte, we should be running
                    {
                        if (runningCommand == 0)
                        {
                            throw new Exception(String.Format("Empty running command at 0x{0}", currentOffset.ToString("X2")));
                        }
                        else
                        {
                            running = true;
                        }
                    }
                    else // new command
                    {
                        runningCommand = currentByte;
                        running = false;
                        
                        commandOffset = currentOffset;
                    }

                    dataByteCount = this.dataBytesPerCommand[runningCommand & 0xF0];

                    if (dataByteCount == 0) // 0xFF
                    {
                        // get meta command bytes
                        if (!running)
                        {
                            metaCommandByte = pStream.ReadByte();
                            currentOffset = pStream.Position - 1;
                        }
                        else
                        {
                            metaCommandByte = currentByte;
                        }

                        // get length bytes                   
                        metaCommandLengthByte = pStream.ReadByte();

                        // check for tempo switch here
                        if (metaCommandByte == 0x51)
                        {
                            // tempo switch
                            tempoValBytes = new byte[4];
                            Array.Copy(ParseFile.ParseSimpleOffset(pStream, pStream.Position, metaCommandLengthByte), 0, tempoValBytes, 1, 3);

                            Array.Reverse(tempoValBytes); // flip order to LE for use with BitConverter
                            tempo = BitConverter.ToUInt32(tempoValBytes, 0);
                        }

                        // skip data bytes, they have already been grabbed above if needed
                        pStream.Position += (long)metaCommandLengthByte;
                        currentOffset = pStream.Position;

                        // time should follow, since these data bytes can be over 0x80 anyhow
                        emptyTimeFollows = false;
                    }
                    else
                    {
                        if (running)
                        {
                            dataByte1 = currentByte;
                        }
                        else
                        {
                            dataByte1 = pStream.ReadByte();
                            currentOffset = pStream.Position - 1;
                        }

                        if (dataByteCount == 2)
                        {
                            dataByte2 = pStream.ReadByte();
                            currentOffset = pStream.Position - 1;

                            // if high bit of last data byte is 1, next delta tick is zero and byte will be skipped
                            if ((dataByte2 & 0x80) != 0)
                            {
                                emptyTimeFollows = true;
                            }
                            else
                            {
                                emptyTimeFollows = false;
                            }

                            // check for loop start
                            if ((((currentByte & 0xF0) == 0xB0) || ((runningCommand & 0xF0) == 0xB0)) &&
                                 dataByte1 == 0x63 &&
                                (dataByte2 == 0x00 || dataByte2 == 0x80))
                            {
                                loopTimeStack.Push(0);
                                loopTickStack.Push(0);
                                loopsOpened++;
                            }

                            // check for loop end
                            if ((((currentByte & 0xF0) == 0xB0) || ((runningCommand & 0xF0) == 0xB0)) &&
                                 dataByte1 == 0x63 &&
                                (dataByte2 == 0x01 || dataByte2 == 0x81))
                            {
                                loopEndFound = true;
                                loopsClosed++;
                            }

                            // check for loop count
                            if (loopEndFound &&
                               (((currentByte & 0xF0) == 0xB0) || ((runningCommand & 0xF0) == 0xB0)) &&
                                dataByte1 == 0x26)
                            {
                                if (loopTimeStack.Count > 0) // check for unmatched close tag
                                {
                                    loopTimeMultiplier = dataByte2;

                                    // filter out high bytes, they are just indicator if next delta tick is zero
                                    loopTimeMultiplier = loopTimeMultiplier & 0x0F;

                                    if (loopTimeMultiplier == 0)
                                    {
                                        loopTimeMultiplier = 2;
                                        loopFound = true;
                                    }

                                    // add loop time
                                    loopTime = loopTimeStack.Pop();
                                    loopTime = (loopTime * loopTimeMultiplier);
                                    totalTime += loopTime;

                                    loopTicks = loopTickStack.Pop();
                                    loopTicks = (loopTicks * (ulong)loopTimeMultiplier);
                                    totalTicks += loopTicks;

                                    timeSinceLastLoopEnd = 0;
                                }
                                else
                                {
                                    ret.Warnings += "Unmatched Loop End tag(s) found." + Environment.NewLine;
                                }

                                loopEndFound = false;
                            }
                        }
                        else
                        {
                            // if high bit of last data byte is 1, next delta tick is zero and byte will be skipped
                            if ((dataByte1 & 0x80) != 0)
                            {
                                emptyTimeFollows = true;
                            }
                            else
                            {
                                emptyTimeFollows = false;
                            }
                        }

                        currentOffset = pStream.Position - 1;
                    }
                //}
                //else // meta event (only tempo is relevant for timing)
                //{
                //    // get meta command bytes
                //    metaCommandByte = pStream.ReadByte();
                //    currentOffset = pStream.Position - 1;

                //    // get length bytes                   
                //    metaCommandLengthByte = pStream.ReadByte();

                //    // check for tempo switch here
                //    if (metaCommandByte == 0x51)
                //    {
                //        // tempo switch
                //        tempoValBytes = new byte[4];
                //        Array.Copy(ParseFile.parseSimpleOffset(pStream, pStream.Position, metaCommandLengthByte), 0, tempoValBytes, 1, 3);

                //        Array.Reverse(tempoValBytes); // flip order to LE for use with BitConverter
                //        tempo = BitConverter.ToUInt32(tempoValBytes, 0);
                //    }

                //    // skip data bytes, they have already been grabbed above if needed
                //    pStream.Position += (long)metaCommandLengthByte;
                //    currentOffset = pStream.Position;

                //    // time should follow, since these data bytes can be over 0x80 anyhow
                //    emptyTimeFollows = false;
                //}
                        
            } // while (pStream.Position < pEndOffset)
            

DONE:       // Marker used for skipping delta ticks at the end of a file.

            // Not sure how to handle, but for now count each unclosed loop twice, 
            //   since it should be the outermost loop.            
            if (loopTimeStack.Count > 0)
            {
                ret.Warnings += "Unmatched Loop Start tag(s) found." + Environment.NewLine;

                while (loopTimeStack.Count > 0)
                {
                    totalTime += loopTimeStack.Pop() * 2d;
                    loopFound = true;
                }
            }

            if (loopsClosed > loopsOpened)
            {
                totalTime -= timeSinceLastLoopEnd;
            }
            
            ret.TimeInSeconds = ((totalTime) * Math.Pow(10, -6));

            if (loopFound)
            {
                ret.FadeInSeconds = 10; // looping
            }
            else
            {
                ret.FadeInSeconds = 1;  // non-looping
            }

            // return stream to incoming position
            pStream.Position = incomingStreamPosition;
            return ret;

        }
    }
}
