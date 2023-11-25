using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    public enum PsxSequenceType
    { 
        None,
        SeqType,
        SepType        
    }
    
    public class PsxSequence
    {
        public static readonly byte[] ASCII_SIGNATURE_SEQ = new byte[] {0x70, 0x51, 0x45, 0x53,
                                                                        0x00, 0x00, 0x00, 0x01}; //pQES SEQ
        public static readonly byte[] ASCII_SIGNATURE_SEP = new byte[] {0x70, 0x51, 0x45, 0x53,
                                                                        0x00, 0x00, 0x00, 0x00}; //pQES SEP
        public static readonly byte[] END_SEQUENCE = new byte[] { 0xFF, 0x2F, 0x00 };
        public static readonly byte[] END_SEQUENCE_TYPE2 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public static readonly byte[] END_SEQUENCE_TYPE3 = new byte[] { 0xFF, 0x00, 0x00 };
                
        public const string FILE_EXTENSION_SEQ = ".seq";
        public const string FILE_EXTENSION_SEP = ".sep";

        public const int FADE_SECONDS_LOOP = 10;
        public const int FADE_SECONDS_NO_LOOP = 0;
        
        public struct PsxSqTimingStruct
        {
            public double TimeInSeconds;
            public int FadeInSeconds;
            public string Warnings;

            public double LoopStartInSeconds;
            public double LoopEndInSeconds;
        }

        struct SqHeaderStruct
        {
            public byte[] MagicBytes;
            public UInt32 Version;
            public UInt16 Resolution;
            public UInt32 InitialTempo;
            public UInt16 unknown;
        }

        public struct PsxSqInitStruct
        {
            public bool force2Loops;
            public bool forceOppositeFormatType;
            public bool forceSepType;
            public bool forceSeqType;
        }

        private SqHeaderStruct sqHeader;
        private PsxSqTimingStruct timingInfo;
        private bool force2Loops;
        private bool forceOppositeFormatType;
        private bool forceSepType;
        private bool forceSeqType;
        private Dictionary<int, int> dataBytesPerCommand;

        public PsxSqTimingStruct TimingInfo { get { return timingInfo; } }

        public PsxSequence(Stream pStream, PsxSqInitStruct pPsxSqInitStruct)
        {
            this.sqHeader = parseSqHeader(pStream);

            this.force2Loops = pPsxSqInitStruct.force2Loops;
            this.forceOppositeFormatType = pPsxSqInitStruct.forceOppositeFormatType;
            this.forceSepType = pPsxSqInitStruct.forceSepType;
            this.forceSeqType = pPsxSqInitStruct.forceSeqType;

            this.dataBytesPerCommand = new Dictionary<int, int>();
            this.dataBytesPerCommand.Add(0x80, 2);
            this.dataBytesPerCommand.Add(0x90, 2);
            this.dataBytesPerCommand.Add(0xA0, 2);
            this.dataBytesPerCommand.Add(0xB0, 2);
            this.dataBytesPerCommand.Add(0xC0, 1);
            this.dataBytesPerCommand.Add(0xD0, 1);
            this.dataBytesPerCommand.Add(0xE0, 2);
            this.dataBytesPerCommand.Add(0xF0, 0);

            timingInfo = getTimingInfo(pStream);
        }

        private SqHeaderStruct parseSqHeader(Stream pStream)
        {
            SqHeaderStruct ret;
            
            byte[] temp; // used to reverse from big endian to little endian

            // magic bytes
            ret = new SqHeaderStruct();
            ret.MagicBytes = ParseFile.ParseSimpleOffset(pStream, 0, 4); // pQES

            // version
            temp = ParseFile.ParseSimpleOffset(pStream, 4, 4);
            Array.Reverse(temp);
            ret.Version = BitConverter.ToUInt32(temp, 0);

            // resolution
            temp = ParseFile.ParseSimpleOffset(pStream, 8, 2);
            Array.Reverse(temp);
            ret.Resolution = BitConverter.ToUInt16(temp, 0);

            // tempo
            temp = new byte[4];
            Array.Copy(ParseFile.ParseSimpleOffset(pStream, 10, 3), 0, temp, 1, 3);
            Array.Reverse(temp);
            ret.InitialTempo = BitConverter.ToUInt32(temp, 0);

            // unknown
            temp = ParseFile.ParseSimpleOffset(pStream, 13, 2);
            Array.Reverse(temp);
            ret.unknown = BitConverter.ToUInt16(temp, 0);

            return ret;
        }

        private PsxSqTimingStruct getTimingInfo(Stream pStream)
        {
            PsxSqTimingStruct ret = new PsxSqTimingStruct();
            ret.Warnings = String.Empty;
            ret.LoopStartInSeconds = -1;
            ret.LoopEndInSeconds = -1;

            long incomingStreamPosition = pStream.Position;

            int currentByte;
            long currentOffset = 0;
            long iterationOffset = 0;
            long commandOffset = 0;
            int dataByte1;
            int dataByte2;

            int deltaTimeByteCount;

            int runningCommand = 0;
            int dataByteCount;
            uint tempo = this.sqHeader.InitialTempo;

            int metaCommandByte;
            int metaCommandLengthByte;

            bool running = false;

            bool loopFound = false;
            bool loopEndFound = false;
            bool emptyTimeNext = false;
            double loopTime;


            int loopTimeMultiplier = 1;
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

            #region Start Offset

            //if (!this.forceSepType && !this.forceSeqType)
            //{

                if ((this.sqHeader.Version == 0) || (this.sqHeader.Version > 1))
                {
                    if (!this.forceOppositeFormatType)
                    {
                        pStream.Position = 19;
                    }
                    else
                    {
                        pStream.Position = 15;
                    }
                }
                else
                {
                    if (!this.forceOppositeFormatType)
                    {
                        pStream.Position = 15;
                    }
                    else
                    {
                        pStream.Position = 19;
                    }
                }
            //}
            //else
            //{
            //    if (this.forceSepType)
            //    {
            //        pStream.Position = 19;
            //    }
            //    else if (this.forceSeqType)
            //    {
            //        pStream.Position = 15;
            //    }
            //}
                        
            if (this.forceOppositeFormatType)
            {
                ret.Warnings += "Failed as internal version type, trying to force opposite version.  Please verify after completion." + Environment.NewLine;
            }

            #endregion

            while (pStream.Position < pStream.Length)
            {
                iterationOffset = pStream.Position;

                if (!emptyTimeNext)
                {
                    currentByte = pStream.ReadByte();
                    currentOffset = pStream.Position - 1;

                    // build 7-bit num from bytes (variable length string)                
                    if ((currentByte & 0x80) != 0)
                    {
                        deltaTimeByteCount = 1;
                        currentTicks = (ulong)(currentByte & 0x7F);

                        do
                        {
                            currentByte = pStream.ReadByte();
                            currentOffset = pStream.Position - 1;

                            deltaTimeByteCount++;
                            //if (deltaTimeByteCount > 4)
                            //{
                            //    int x4 = 1;
                            //}

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

                    currentTime = currentTicks != 0 ? (double)((currentTicks * (ulong)tempo) / (ulong)this.sqHeader.Resolution) : 0;

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
                        if (pStream.Position != pStream.Length) // time at the end of the file is ignored
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
                else
                {
                    emptyTimeNext = false; // reset
                }
                
                // get command
                currentByte = pStream.ReadByte();
                currentOffset = pStream.Position - 1;

                //if (currentOffset > 0x1580) // code to quickly get to a position for debugging
                //{
                //    int x = 1;
                //}

                //if ((currentTime != 0) && (currentTime != 38461) && (currentTime != 192307))
                //{
                //    int x3 = 1;
                //}


                if ((currentByte & 0x80) == 0) // data byte, we should be running
                {
                    if (runningCommand == 0)
                    {
                        throw new PsxSeqFormatException(String.Format("Empty running command at 0x{0}", currentOffset.ToString("X2")));
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

                if (dataByteCount == 0)
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
                    // check for tempo switch here
                    if (metaCommandByte == 0x51)
                    {
                        // tempo switch
                        tempoValBytes = new byte[4];
                        Array.Copy(ParseFile.ParseSimpleOffset(pStream, pStream.Position, 3), 0, tempoValBytes, 1, 3);

                        Array.Reverse(tempoValBytes); // flip order to LE for use with BitConverter
                        tempo = BitConverter.ToUInt32(tempoValBytes, 0);

                        pStream.Position += 3;
                    }
                    else
                    {
                        // get length bytes                   
                        metaCommandLengthByte = pStream.ReadByte();

                        // skip data bytes, they have already been grabbed above if needed
                        if (metaCommandLengthByte > 0)
                        {
                            pStream.Position += (long)metaCommandLengthByte;
                        }
                    }
                    currentOffset = pStream.Position;

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


                        // check for loop start
                        if ((((currentByte & 0xF0) == 0xB0) || ((runningCommand & 0xF0) == 0xB0)) &&
                             dataByte1 == 0x63 && dataByte2 == 0x14)
                        {                            
                            loopTimeStack.Push(0);
                            loopTickStack.Push(0);
                            loopsOpened++;

                            ret.LoopStartInSeconds = ((totalTime) * Math.Pow(10, -6));
                        }

                        // check for loop end
                        if ((((currentByte & 0xF0) == 0xB0) || ((runningCommand & 0xF0) == 0xB0)) &&
                             dataByte1 == 0x63 && dataByte2 == 0x1E)
                        {
                            loopEndFound = true;
                            loopsClosed++;
                        }

                        if ((((currentByte & 0xF0) == 0xB0) || ((runningCommand & 0xF0) == 0xB0)) &&
                            dataByte1 == 0x06)
                        {
                            loopTimeMultiplier = dataByte2;
                        }

                        // check for loop count
                        if (loopEndFound)
                        {
                            if (this.force2Loops)
                            {
                                loopTimeMultiplier = 2;
                                loopFound = true;
                            }
                            else if (loopTimeMultiplier == 127)
                            {
                                loopTimeMultiplier = 2;
                                loopFound = true;
                            }
                            else if (loopTimeMultiplier == 0)
                            {
                                loopTimeMultiplier = 1;
                            }

                            // add loop time
                            if (loopTimeStack.Count > 0)
                            {
                                loopTime = loopTimeStack.Pop();
                                
                                // set loop end
                                ret.LoopEndInSeconds = ((totalTime + loopTime) * Math.Pow(10, -6));
                                
                                // multiply by loop multiplier.
                                loopTime = (loopTime * loopTimeMultiplier);
                                totalTime += loopTime;

                                loopTicks = loopTickStack.Pop();
                                loopTicks = (loopTicks * (ulong)loopTimeMultiplier);
                                totalTicks += loopTicks;

                                timeSinceLastLoopEnd = 0;

                                
                            }

                            loopEndFound = false;
                            // emptyTimeNext = true;
                        }
                    }

                    currentOffset = pStream.Position - 1;
                }

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
                ret.Warnings += "Unmatched Loop End tag(s) found." + Environment.NewLine;
                totalTime -= timeSinceLastLoopEnd;
            }

            ret.TimeInSeconds = ((totalTime) * Math.Pow(10, -6));

            if (loopFound)
            {
                ret.FadeInSeconds = FADE_SECONDS_LOOP; // looping
            }
            else
            {
                ret.FadeInSeconds = FADE_SECONDS_NO_LOOP;  // non-looping
            }

            // return stream to incoming position
            pStream.Position = incomingStreamPosition;
            return ret;        
        }

        public static uint GetSeqCount(string fileName)
        {
            uint seqCount = 0;
            long offset = 0;

            if (PsxSequence.IsSepTypeSequence(fileName))
            {
                using (FileStream fs = File.OpenRead(fileName))
                { 
                    while ((offset = ParseFile.GetNextOffset(fs, offset, PsxSequence.END_SEQUENCE)) > -1)
                    {
                        seqCount++;
                        offset++;
                    }
                }
            }
            else if (PsxSequence.IsSeqTypeSequence(fileName))
            {
                seqCount = 1;
            }
            
            return seqCount;
        }

        public static bool IsSepTypeSequence(string fileName)
        {
            bool ret = false;
            int numBytesRead;
            byte[] checkBytes = new byte[PsxSequence.ASCII_SIGNATURE_SEP.Length];

            using (FileStream fs = File.OpenRead(fileName))
            {
                numBytesRead = fs.Read(checkBytes, 0, PsxSequence.ASCII_SIGNATURE_SEP.Length);

                if (numBytesRead == PsxSequence.ASCII_SIGNATURE_SEP.Length)
                {
                    ret = ParseFile.CompareSegment(checkBytes, 0, PsxSequence.ASCII_SIGNATURE_SEP);
                }
            }

            return ret;
        }

        public static bool IsSeqTypeSequence(string fileName)
        {
            bool ret = false;
            int numBytesRead;
            byte[] checkBytes = new byte[PsxSequence.ASCII_SIGNATURE_SEQ.Length];

            using (FileStream fs = File.OpenRead(fileName))
            {
                numBytesRead = fs.Read(checkBytes, 0, PsxSequence.ASCII_SIGNATURE_SEQ.Length);

                if (numBytesRead == PsxSequence.ASCII_SIGNATURE_SEQ.Length)
                {
                    ret = ParseFile.CompareSegment(checkBytes, 0, PsxSequence.ASCII_SIGNATURE_SEQ);
                }
            }

            return ret;
        }

        public static string ExtractSeqFromSep(string fileName, uint sepId)
        {
            string outputSeqPath = null;

            // check if this is an SEP type and the Id is within range
            if (IsSepTypeSequence(fileName) &&
                (sepId < GetSeqCount(fileName)))
            {
                byte[] resolution;
                byte[] tempo;
                byte[] rhythm;
                byte[] midiSectionLength;
                uint midiSectionLengthValue;
                uint midiSectionOffset;

                // setup values for SEQ 0
                uint currentSeqNumber = 0;
                uint currentSeqOffset = 0x08;
                uint currentSeqEof;                
                                
                // open file strean
                using (FileStream fs = File.OpenRead(fileName))
                {                    
                    midiSectionLength = ParseFile.ParseSimpleOffset(fs, (currentSeqOffset + 7), 4);                    
                    Array.Reverse(midiSectionLength);
                    midiSectionLengthValue = BitConverter.ToUInt32(midiSectionLength, 0);

                    midiSectionOffset = currentSeqOffset + 0x0B;
                    currentSeqEof = midiSectionOffset + midiSectionLengthValue;

                    // loop for values greater than 0
                    while (currentSeqNumber < sepId)
                    {
                        currentSeqNumber++;
                        currentSeqOffset = currentSeqEof + 2; // Size of SEQ ID is 2
                        
                        midiSectionLength = ParseFile.ParseSimpleOffset(fs, (currentSeqOffset + 7), 4);
                        Array.Reverse(midiSectionLength);
                        midiSectionLengthValue = BitConverter.ToUInt32(midiSectionLength, 0);

                        midiSectionOffset = currentSeqOffset + 0x0B;
                        currentSeqEof = midiSectionOffset + midiSectionLengthValue;                        
                    }

                    // get chunks and write to new file
                    resolution = ParseFile.ParseSimpleOffset(fs, currentSeqOffset, 2);
                    tempo = ParseFile.ParseSimpleOffset(fs, (currentSeqOffset + 2), 3);
                    rhythm = ParseFile.ParseSimpleOffset(fs, (currentSeqOffset + 5), 2);
                    midiSectionLength = ParseFile.ParseSimpleOffset(fs, (currentSeqOffset + 7), 4);
                }

                // write file
                outputSeqPath = Path.Combine(
                    Path.GetDirectoryName(fileName),
                    String.Format(
                        "{0}_{1}{2}",
                        Path.GetFileNameWithoutExtension(fileName),
                        currentSeqNumber.ToString("X2"),
                        PsxSequence.FILE_EXTENSION_SEQ));

                // write header
                using (FileStream seqStream = File.Open(outputSeqPath, FileMode.Create, FileAccess.Write))
                {
                    seqStream.Write(ASCII_SIGNATURE_SEQ, 0, ASCII_SIGNATURE_SEQ.Length);
                    seqStream.Write(resolution, 0, resolution.Length);
                    seqStream.Write(tempo, 0, tempo.Length);
                    seqStream.Write(rhythm, 0, rhythm.Length);
                }

                // write MIDI data
                FileUtil.ReplaceFileChunk(fileName, midiSectionOffset, 
                    (currentSeqEof - midiSectionOffset), outputSeqPath, 0x0F);
            }

            return outputSeqPath;
        }
    }

    public class PsxSeqFormatException : Exception
    {
        public PsxSeqFormatException(string pMessage)
            : base(pMessage) { }
    }
}
