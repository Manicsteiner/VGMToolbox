using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.format
{
    public struct MidiTrackInfo
    {
        public long StartOffset;
        public byte[] TrackHeader;
        public byte[] TrackLength;

        public string MiscText;
        public string TrackName;
        public string Copyright;
        public string TrackInstrument;
        public string Lyric;
        public string Marker;
        public string CuePoint;
    }
    
    public class Midi : IFormat, IExtractableFormat
    {
        public static readonly byte[] ASCII_SIGNATURE_MTHD = new byte[] { 0x4D, 0x54, 0x68, 0x64 }; // MThd
        public static readonly byte[] ASCII_SIGNATURE_MTRK = new byte[] { 0x4D, 0x54, 0x72, 0x6B }; // MTrk
        private const string FORMAT_ABBREVIATION = "MIDI";
        private const string FILE_EXTENSION = ".mid";

        private const int HEADER_SIG_OFFSET = 0x00;
        private const int HEADER_SIG_LENGTH = 0x04;

        private const int HEADER_SIZE_OFFSET = 0x04;
        private const int HEADER_SIZE_LENGTH = 0x04;

        private const int FILE_FORMAT_OFFSET = 0x08;
        private const int FILE_FORMAT_LENGTH = 0x02;

        private const int NUMBER_OF_TRACKS_OFFSET = 0x0A;
        private const int NUMBER_OF_TRACKS_LENGTH = 0x02;

        private const int DELTA_TICKS_OFFSET = 0x0C;
        private const int DELTA_TICKS_LENGTH = 0x02;

        private const int FIRST_TRACK_HEADER_SIG_OFFSET = 0x0E;
        private const int TRACK_HEADER_SIG_LENGTH = 0x04;

        private const int RELATIVE_TRACK_LENGTH_OFFSET = 0x04;
        private const int TRACK_LENGTH_LENGTH = 0x04;

        private const int FIRST_TRACK_DATA_OFFSET = 0x16;

        Dictionary<string, string> tagHash = new Dictionary<string, string>();
        Dictionary<int, int> dataBytesPerCommand;

        private byte[] asciiSignature;
        private byte[] headerSize;
        private byte[] fileFormat;
        private byte[] numberOfTracks;
        private byte[] deltaTicks;
        private MidiTrackInfo[] midiTracks;

        private string filePath;        
        private long fileStartOffset;
        private long totalFileLength;        
        
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        public long FileStartOffset
        {
            get { return fileStartOffset; }
            set { fileStartOffset = value; }
        }
        public long TotalFileLength
        {
            get { return totalFileLength; }
            set { totalFileLength = value; }
        }

        private string trackName;

        public Midi()
        {
            this.dataBytesPerCommand = new Dictionary<int, int>();
            this.dataBytesPerCommand.Add(0x80, 2);
            this.dataBytesPerCommand.Add(0x90, 2);
            this.dataBytesPerCommand.Add(0xA0, 2);
            this.dataBytesPerCommand.Add(0xB0, 2);
            this.dataBytesPerCommand.Add(0xC0, 1);
            this.dataBytesPerCommand.Add(0xD0, 1);
            this.dataBytesPerCommand.Add(0xE0, 2);
            this.dataBytesPerCommand.Add(0xF0, 0);        
        }

        #region METHODS

        public void Initialize(Stream pStream, string pFilePath)
        {
            this.Initialize(pStream, pFilePath, 0);
        }
        public void Initialize(Stream pStream, string pFilePath, long pFileOffset)
        {
            UInt16 numberOfTracks;
            long totalTrackLength;
            long offset;
                        
            this.filePath = pFilePath;
            this.fileStartOffset = pFileOffset;
            this.asciiSignature = ParseFile.ParseSimpleOffset(pStream, pFileOffset + HEADER_SIG_OFFSET, HEADER_SIG_LENGTH);
            this.headerSize = ParseFile.ParseSimpleOffset(pStream, pFileOffset + HEADER_SIZE_OFFSET, HEADER_SIZE_LENGTH);
            this.fileFormat = ParseFile.ParseSimpleOffset(pStream, pFileOffset + FILE_FORMAT_OFFSET, FILE_FORMAT_LENGTH);
            this.numberOfTracks = ParseFile.ParseSimpleOffset(pStream, pFileOffset + NUMBER_OF_TRACKS_OFFSET, NUMBER_OF_TRACKS_LENGTH);
            this.deltaTicks = ParseFile.ParseSimpleOffset(pStream, pFileOffset + DELTA_TICKS_OFFSET, DELTA_TICKS_LENGTH);

            // get tracks
            this.totalFileLength = Midi.HEADER_SIG_LENGTH + Midi.HEADER_SIG_LENGTH + 
                VGMToolbox.util.ByteConversion.GetUInt32BigEndian(this.headerSize);
            numberOfTracks = VGMToolbox.util.ByteConversion.GetUInt16BigEndian(this.numberOfTracks);
            
            this.midiTracks = new MidiTrackInfo[numberOfTracks];            
            offset = (long)FIRST_TRACK_HEADER_SIG_OFFSET;
            
            for (int i = 0; i < numberOfTracks; i++)
            {
                this.midiTracks[i] = new MidiTrackInfo();
                this.midiTracks[i].StartOffset = offset;
                this.midiTracks[i].TrackHeader = ParseFile.ParseSimpleOffset(pStream, pFileOffset + offset, TRACK_HEADER_SIG_LENGTH);
                this.midiTracks[i].TrackLength = ParseFile.ParseSimpleOffset(pStream, pFileOffset + offset + RELATIVE_TRACK_LENGTH_OFFSET, TRACK_LENGTH_LENGTH);

                totalTrackLength = (long)(TRACK_HEADER_SIG_LENGTH + TRACK_LENGTH_LENGTH +
                   VGMToolbox.util.ByteConversion.GetUInt32BigEndian(this.midiTracks[i].TrackLength));
                offset += totalTrackLength;
                this.totalFileLength += totalTrackLength;

                this.parseTextEvents(pStream, i,
                    (midiTracks[i].StartOffset + TRACK_HEADER_SIG_LENGTH + TRACK_LENGTH_LENGTH),
                    VGMToolbox.util.ByteConversion.GetUInt32BigEndian(this.midiTracks[i].TrackLength));
            }

            tagHash.Clear();
            tagHash.Add("MIDI Type", 
                VGMToolbox.util.ByteConversion.GetUInt16BigEndian(this.fileFormat).ToString());
            tagHash.Add("Total Tracks", numberOfTracks.ToString());
            tagHash.Add("Total File Size", this.totalFileLength.ToString());

            for (int i = 0; i < numberOfTracks; i++)
            {
                tagHash.Add(String.Format("Track {0} - Offset [Length]", i.ToString()),
                    String.Format("0x{0} [0x{1}]", midiTracks[i].StartOffset.ToString("X8"), 
                    VGMToolbox.util.ByteConversion.GetUInt32BigEndian(this.midiTracks[i].TrackLength).ToString("X4")));

                if (!String.IsNullOrEmpty(this.midiTracks[i].TrackName))
                {
                    tagHash.Add(String.Format("Track {0} - Track Name", i.ToString()), this.midiTracks[i].TrackName);
                }
                if (!String.IsNullOrEmpty(this.midiTracks[i].Copyright))
                {
                    tagHash.Add(String.Format("Track {0} - Copyright", i.ToString()), this.midiTracks[i].Copyright);
                }
                if (!String.IsNullOrEmpty(this.midiTracks[i].TrackInstrument))
                {
                    tagHash.Add(String.Format("Track {0} - Track Instrument", i.ToString()), this.midiTracks[i].TrackInstrument);
                }
                if (!String.IsNullOrEmpty(this.midiTracks[i].Marker))
                {
                    tagHash.Add(String.Format("Track {0} - Marker", i.ToString()), this.midiTracks[i].Marker);
                }
                if (!String.IsNullOrEmpty(this.midiTracks[i].Lyric))
                {
                    tagHash.Add(String.Format("Track {0} - Lyric", i.ToString()), this.midiTracks[i].Lyric);
                }
                if (!String.IsNullOrEmpty(this.midiTracks[i].CuePoint))
                {
                    tagHash.Add(String.Format("Track {0} - Cue Point", i.ToString()), this.midiTracks[i].CuePoint);
                }
                if (!String.IsNullOrEmpty(this.midiTracks[i].MiscText))
                {
                    tagHash.Add(String.Format("Track {0} - Misc. Text", i.ToString()), this.midiTracks[i].MiscText);
                }
            }

        }

        public byte[] GetAsciiSignature()
        {
            return ASCII_SIGNATURE_MTHD;
        }
        public string GetFileExtensions()
        {
            return null;
        }
        public string GetFormatAbbreviation()
        {
            return FORMAT_ABBREVIATION;
        }

        public bool IsFileLibrary() { return false; }
        public bool HasMultipleFileExtensions()
        {
            return false;
        }
        public Dictionary<string, string> GetTagHash()
        {
            return this.tagHash;
        }
        public bool UsesLibraries() { return false; }
        public bool IsLibraryPresent() { return true; }

        public void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            throw new NotImplementedException();
        }
        public void GetDatFileChecksums(ref Crc32 pChecksum, ref CryptoStream pMd5CryptoStream,
            ref CryptoStream pSha1CryptoStream)
        {
            throw new NotImplementedException();
        }

        public void ExtractToFile(Stream pStream, string pOutputDirectory)
        { 
            ExtractToFile(pStream, pOutputDirectory, null);
        }
        
        public void ExtractToFile(Stream pStream, string pOutputDirectory, object Options)
        { 
            string outputFileName = Path.GetFileName(this.filePath);
            int fileCount = 0;

            if (!Directory.Exists(pOutputDirectory))
            {
                Directory.CreateDirectory(pOutputDirectory);
            }
            else
            {
                fileCount = Directory.GetFiles(pOutputDirectory, String.Format("{0}*", Path.GetFileNameWithoutExtension(outputFileName)), SearchOption.TopDirectoryOnly).Length;
            }

            outputFileName = String.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(outputFileName), fileCount.ToString("X4"), Midi.FILE_EXTENSION);
            outputFileName = Path.Combine(pOutputDirectory, outputFileName);

            ParseFile.ExtractChunkToFile(pStream, this.fileStartOffset, this.totalFileLength, outputFileName, true, true);
        }

        private void parseTextEvents(Stream pStream, int pTrackIndex, long pOffset, uint pLength)
        {
            bool running = false;
            
            int currentByte;
            int runningCommand = 0;
            int dataByteCount;
            int dataByte1;
            int dataByte2;
            int metaCommandByte;
            int metaCommandLengthByte;
            byte[] metaCommandDataBytes;
            UInt64 currentTicks;

            pStream.Position = this.fileStartOffset + pOffset;

            while (pStream.Position < (this.fileStartOffset + pOffset + pLength))
            {
                currentByte = pStream.ReadByte();

                // build 7-bit num from bytes (variable length string)                
                if ((currentByte & 0x80) != 0)
                {
                    currentTicks = (ulong)(currentByte & 0x7F);

                    do
                    {
                        currentByte = pStream.ReadByte();
                        currentTicks = (currentTicks << 7) + (ulong)(currentByte & 0x7F);
                    } while ((currentByte & 0x80) != 0);
                }
                else // only one byte, no need for conversion
                {
                    currentTicks = (ulong)currentByte;
                }

                // get command
                currentByte = pStream.ReadByte();

                if ((currentByte & 0x80) == 0) // data byte, we should be running
                {
                    running = true;
                }
                else // new command
                {
                    runningCommand = currentByte;
                    running = false;
                }

                dataByteCount = this.dataBytesPerCommand[runningCommand & 0xF0];

                if (dataByteCount == 0)
                {
                    // get meta command bytes
                    if (!running)
                    {
                        metaCommandByte = pStream.ReadByte();
                    }
                    else
                    {
                        metaCommandByte = currentByte;
                    }
                    
                    // get length bytes                   
                    metaCommandLengthByte = pStream.ReadByte();
                    
                    // get meta command data
                    if (metaCommandLengthByte > 0)
                    {
                        metaCommandDataBytes = ParseFile.ParseSimpleOffset(pStream, pStream.Position, metaCommandLengthByte);

                        switch (metaCommandByte)
                        {
                            case 1: // Generic Text Event
                                this.midiTracks[pTrackIndex].MiscText = VGMToolbox.util.ByteConversion.GetAsciiText(metaCommandDataBytes);
                                break;
                            case 2: // Copyright
                                this.midiTracks[pTrackIndex].Copyright = VGMToolbox.util.ByteConversion.GetAsciiText(metaCommandDataBytes);
                                break;
                            case 3: // Track Name
                                this.midiTracks[pTrackIndex].TrackName = VGMToolbox.util.ByteConversion.GetAsciiText(metaCommandDataBytes);
                                break;
                            case 4: // Track Instrument
                                this.midiTracks[pTrackIndex].TrackInstrument = VGMToolbox.util.ByteConversion.GetAsciiText(metaCommandDataBytes);
                                break;
                            case 5: // Lyric
                                this.midiTracks[pTrackIndex].Lyric = VGMToolbox.util.ByteConversion.GetAsciiText(metaCommandDataBytes);
                                break;
                            case 6: // Marker
                                this.midiTracks[pTrackIndex].Marker = VGMToolbox.util.ByteConversion.GetAsciiText(metaCommandDataBytes);
                                break;
                            case 7: // Marker
                                this.midiTracks[pTrackIndex].CuePoint = VGMToolbox.util.ByteConversion.GetAsciiText(metaCommandDataBytes);
                                break;                                                        
                            default:
                                pStream.Position += (long)metaCommandLengthByte;
                                break;
                        } 
                    }
                }
                else
                {
                    if (running) { dataByte1 = currentByte; }
                    else { dataByte1 = pStream.ReadByte(); }

                    if (dataByteCount == 2)
                    {
                        dataByte2 = pStream.ReadByte();                        
                    }
                }

            } // while (pStream.Position < pEndOffset)        
        }

        #endregion

        # region STATIC METHODS

        public static bool IsMidi(string pFilePath)
        {
            bool ret = false;

            if (!String.IsNullOrEmpty(pFilePath))
            {
                string fullPath = Path.GetFullPath(pFilePath);

                if (File.Exists(fullPath))
                {
                    using (FileStream fs = File.OpenRead(fullPath))
                    {
                        Type dataType = FormatUtil.getObjectType(fs);

                        if ((dataType != null) && (dataType.Name.Equals("Midi")))
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

        #endregion
    }
}
