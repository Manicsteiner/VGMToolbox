using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format.util
{
    public class RiffUtil
    {
        public struct InterleaveRiffFilesOptionsStruct
        {
            public byte FillerByte { set; get; }
        }
        
        public static uint GetRiffHeaderSize(string riffHeaderedFile)
        {
            uint headerSize = 0;

            using (FileStream fs = File.OpenRead(riffHeaderedFile))
            {
                long riffHeaderLocation = ParseFile.GetNextOffset(fs, 0, Constants.RiffHeaderBytes);

                if (riffHeaderLocation > -1)
                {
                    long waveChunkLocation = ParseFile.GetNextOffset(fs,
                        riffHeaderLocation + Constants.RiffHeaderBytes.Length, Constants.RiffWaveBytes);

                    if (waveChunkLocation > -1)
                    {
                        long dataChunkLocation = ParseFile.GetNextOffset(fs,
                            waveChunkLocation + Constants.RiffWaveBytes.Length, Constants.RiffDataBytes);

                        if (dataChunkLocation > -1)
                        {
                            headerSize = (uint)(dataChunkLocation + Constants.RiffDataBytes.Length + 4);
                        }
                        else
                        {
                            throw new FormatException("RIFF header data chunk not found.");
                        }
                    }
                    else
                    {
                        throw new FormatException("RIFF header WAVE chunk not found.");
                    }
                }
                else
                {
                    throw new FormatException("RIFF header not found.");
                }
            }

            return headerSize;
        }

        public static uint GetDataSizeFromRiffHeader(string riffHeaderedFile)
        {
            uint dataSize = 0;

            using (FileStream fs = File.OpenRead(riffHeaderedFile))
            {
                long riffHeaderLocation = ParseFile.GetNextOffset(fs, 0, Constants.RiffHeaderBytes);

                if (riffHeaderLocation > -1)
                {
                    long waveChunkLocation = ParseFile.GetNextOffset(fs,
                        riffHeaderLocation + Constants.RiffHeaderBytes.Length, Constants.RiffWaveBytes);

                    if (waveChunkLocation > -1)
                    {
                        long dataChunkLocation = ParseFile.GetNextOffset(fs,
                            waveChunkLocation + Constants.RiffWaveBytes.Length, Constants.RiffDataBytes);

                        if (dataChunkLocation > -1)
                        {
                            dataSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, dataChunkLocation + 4, 4), 0);
                        }
                        else
                        {
                            throw new FormatException("RIFF header data chunk not found.");
                        }
                    }
                    else
                    {
                        throw new FormatException("RIFF header WAVE chunk not found.");
                    }
                }
                else
                {
                    throw new FormatException("RIFF header not found.");
                }
            }

            return dataSize;
        }

        public static uint GetFrequencyFromRiffHeader(string riffHeaderedFile)
        {
            uint frequency = 0;

            using (FileStream fs = File.OpenRead(riffHeaderedFile))
            {
                long riffHeaderLocation = ParseFile.GetNextOffset(fs, 0, Constants.RiffHeaderBytes);

                if (riffHeaderLocation > -1)
                {
                    long waveChunkLocation = ParseFile.GetNextOffset(fs,
                        riffHeaderLocation + Constants.RiffHeaderBytes.Length, Constants.RiffWaveBytes);

                    if (waveChunkLocation > -1)
                    {
                        long fmtChunkLocation = ParseFile.GetNextOffset(fs,
                            waveChunkLocation + Constants.RiffWaveBytes.Length, Constants.RiffFmtBytes);

                        if (fmtChunkLocation > -1)
                        {
                            frequency = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(fs, fmtChunkLocation + 0xC, 4), 0);
                        }
                        else
                        {
                            throw new FormatException("RIFF header data chunk not found.");
                        }
                    }
                    else
                    {
                        throw new FormatException("RIFF header WAVE chunk not found.");
                    }
                }
                else
                {
                    throw new FormatException("RIFF header not found.");
                }
            }

            return frequency;
        }

        public static ushort GetChannelsFromRiffHeader(string riffHeaderedFile)
        {
            ushort channels = 0;

            using (FileStream fs = File.OpenRead(riffHeaderedFile))
            {
                long riffHeaderLocation = ParseFile.GetNextOffset(fs, 0, Constants.RiffHeaderBytes);

                if (riffHeaderLocation > -1)
                {
                    long waveChunkLocation = ParseFile.GetNextOffset(fs,
                        riffHeaderLocation + Constants.RiffHeaderBytes.Length, Constants.RiffWaveBytes);

                    if (waveChunkLocation > -1)
                    {
                        long fmtChunkLocation = ParseFile.GetNextOffset(fs,
                            waveChunkLocation + Constants.RiffWaveBytes.Length, Constants.RiffFmtBytes);

                        if (fmtChunkLocation > -1)
                        {
                            channels = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(fs, fmtChunkLocation + 0xA, 2), 0);
                        }
                        else
                        {
                            throw new FormatException("RIFF header data chunk not found.");
                        }
                    }
                    else
                    {
                        throw new FormatException("RIFF header WAVE chunk not found.");
                    }
                }
                else
                {
                    throw new FormatException("RIFF header not found.");
                }
            }

            return channels;
        }


        public static uint GetRiffHeaderSize(Stream inputStream)
        {
            uint headerSize = 0;

            long riffHeaderLocation = ParseFile.GetNextOffset(inputStream, 0, Constants.RiffHeaderBytes);

            if (riffHeaderLocation > -1)
            {
                long waveChunkLocation = ParseFile.GetNextOffset(inputStream,
                    riffHeaderLocation + Constants.RiffHeaderBytes.Length, Constants.RiffWaveBytes);

                if (waveChunkLocation > -1)
                {
                    long dataChunkLocation = ParseFile.GetNextOffset(inputStream,
                        waveChunkLocation + Constants.RiffWaveBytes.Length, Constants.RiffDataBytes);

                    if (dataChunkLocation > -1)
                    {
                        headerSize = (uint)(dataChunkLocation + Constants.RiffDataBytes.Length + 4);
                    }
                    else
                    {
                        throw new FormatException("RIFF header data chunk not found.");
                    }
                }
                else
                {
                    throw new FormatException("RIFF header WAVE chunk not found.");
                }
            }
            else
            {
                throw new FormatException("RIFF header not found.");
            }

            return headerSize;
        }

        public static uint GetDataSizeFromRiffHeader(Stream inputStream)
        {
            uint dataSize = 0;

            long riffHeaderLocation = ParseFile.GetNextOffset(inputStream, 0, Constants.RiffHeaderBytes);

            if (riffHeaderLocation > -1)
            {
                long waveChunkLocation = ParseFile.GetNextOffset(inputStream,
                    riffHeaderLocation + Constants.RiffHeaderBytes.Length, Constants.RiffWaveBytes);

                if (waveChunkLocation > -1)
                {
                    long dataChunkLocation = ParseFile.GetNextOffset(inputStream,
                        waveChunkLocation + Constants.RiffWaveBytes.Length, Constants.RiffDataBytes);

                    if (dataChunkLocation > -1)
                    {
                        dataSize = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(inputStream, dataChunkLocation + 4, 4), 0);
                    }
                    else
                    {
                        throw new FormatException("RIFF header data chunk not found.");
                    }
                }
                else
                {
                    throw new FormatException("RIFF header WAVE chunk not found.");
                }
            }
            else
            {
                throw new FormatException("RIFF header not found.");
            }

            return dataSize;
        }

        public static long GetDataStartOffsetFromRiffHeader(Stream inputStream)
        {
            long dataOffset = 0;

            long riffHeaderLocation = ParseFile.GetNextOffset(inputStream, 0, Constants.RiffHeaderBytes);

            if (riffHeaderLocation > -1)
            {
                long waveChunkLocation = ParseFile.GetNextOffset(inputStream,
                    riffHeaderLocation + Constants.RiffHeaderBytes.Length, Constants.RiffWaveBytes);

                if (waveChunkLocation > -1)
                {
                    long dataChunkLocation = ParseFile.GetNextOffset(inputStream,
                        waveChunkLocation + Constants.RiffWaveBytes.Length, Constants.RiffDataBytes);

                    if (dataChunkLocation > -1)
                    {
                        dataOffset = dataChunkLocation + 8;
                    }
                    else
                    {
                        throw new FormatException("RIFF header data chunk not found.");
                    }
                }
                else
                {
                    throw new FormatException("RIFF header WAVE chunk not found.");
                }
            }
            else
            {
                throw new FormatException("RIFF header not found.");
            }

            return dataOffset;
        }

        public static uint GetFrequencyFromRiffHeader(Stream inputStream)
        {
            uint frequency = 0;

            long riffHeaderLocation = ParseFile.GetNextOffset(inputStream, 0, Constants.RiffHeaderBytes);

            if (riffHeaderLocation > -1)
            {
                long waveChunkLocation = ParseFile.GetNextOffset(inputStream,
                    riffHeaderLocation + Constants.RiffHeaderBytes.Length, Constants.RiffWaveBytes);

                if (waveChunkLocation > -1)
                {
                    long fmtChunkLocation = ParseFile.GetNextOffset(inputStream,
                        waveChunkLocation + Constants.RiffWaveBytes.Length, Constants.RiffFmtBytes);

                    if (fmtChunkLocation > -1)
                    {
                        frequency = BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(inputStream, fmtChunkLocation + 0xC, 4), 0);
                    }
                    else
                    {
                        throw new FormatException("RIFF header data chunk not found.");
                    }
                }
                else
                {
                    throw new FormatException("RIFF header WAVE chunk not found.");
                }
            }
            else
            {
                throw new FormatException("RIFF header not found.");
            }

            return frequency;
        }

        public static ushort GetChannelsFromRiffHeader(Stream inputStream)
        {
            ushort channels = 0;

            long riffHeaderLocation = ParseFile.GetNextOffset(inputStream, 0, Constants.RiffHeaderBytes);

            if (riffHeaderLocation > -1)
            {
                long waveChunkLocation = ParseFile.GetNextOffset(inputStream,
                    riffHeaderLocation + Constants.RiffHeaderBytes.Length, Constants.RiffWaveBytes);

                if (waveChunkLocation > -1)
                {
                    long fmtChunkLocation = ParseFile.GetNextOffset(inputStream,
                        waveChunkLocation + Constants.RiffWaveBytes.Length, Constants.RiffFmtBytes);

                    if (fmtChunkLocation > -1)
                    {
                        channels = BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(inputStream, fmtChunkLocation + 0xA, 2), 0);
                    }
                    else
                    {
                        throw new FormatException("RIFF header data chunk not found.");
                    }
                }
                else
                {
                    throw new FormatException("RIFF header WAVE chunk not found.");
                }
            }
            else
            {
                throw new FormatException("RIFF header not found.");
            }

            return channels;
        }


        #region INTERLEAVE FILES
        
        // for now assume 16-bit LE PCM, reinterleavein towav output
        public static void InterleaveRiffToWavOutputFiles(string[] inputFiles, string outputFile,
            InterleaveRiffFilesOptionsStruct interleaveOptions)
        {
            uint frequency;
            uint channels;

            // verify that two or more files have been input
            if (inputFiles.Length < 2)
            {
                throw new Exception("More than one file must be input for interleaving.");
            }

            // verify features match
            if (!inputFilesHaveEqualFeatures(inputFiles))
            {
                throw new Exception("Input files must have same channel count and frequency.");
            }

            // interleave data
            interleaveDataChunks(inputFiles, outputFile, interleaveOptions);

            // update RIFF header

        }

        private static bool inputFilesHaveEqualFeatures(string[] inputFiles)
        {
            bool filesAreEqual = true;
            
            uint baseFrequency;
            uint baseChannels;
            
            uint frequency;
            uint channels;


            // get values for first file
            using (FileStream fs = File.OpenRead(inputFiles[0]))
            {
                baseFrequency = GetFrequencyFromRiffHeader(fs);
                baseChannels = GetChannelsFromRiffHeader(fs);
            }

            for (int i = 0; i < inputFiles.Length; i++)
            {
                using (FileStream fs = File.OpenRead(inputFiles[i]))
                {
                    frequency = GetFrequencyFromRiffHeader(fs);
                    channels = GetChannelsFromRiffHeader(fs);
                }

                if (frequency != baseFrequency)
                {
                    filesAreEqual = false;
                    break;
                }
                else if (channels != baseChannels)
                {
                    filesAreEqual = false;
                    break;
                }
            }

            return filesAreEqual;
        }

        private static string interleaveDataChunks(string[] inputFiles, string outputFile,
            InterleaveRiffFilesOptionsStruct interleaveOptions)
        {
            Dictionary<int, FileStream> inputFileHash = new Dictionary<int, FileStream>();
            FileStream outputStream = null;
            string interleavedFilePath = String.Format("{0}.interleaved.data{1}", 
                Path.GetFileNameWithoutExtension(outputFile),
                Path.GetExtension(outputFile));

            uint maxDataSize = 0;
            uint bytesProcessed = 0;

            byte[] bytes = new byte[2];
            int bytesRead;

            uint[] dataSizes = new uint[inputFiles.Length];
            long[] dataStartOffset = new long[inputFiles.Length];
            long[] fileSizes = new long[inputFiles.Length];

            try
            {
                // open file streams and get data location info
                for (int i = 0; i < inputFiles.Length; i++)
                {
                    using (FileStream fs = File.OpenRead(inputFiles[i]))
                    {
                        inputFileHash.Add(i, fs);
                        dataSizes[i] = GetDataSizeFromRiffHeader(inputFileHash[i]);
                        dataStartOffset[i] = GetDataStartOffsetFromRiffHeader(inputFileHash[i]);
                        fileSizes[i] = inputFileHash[i].Length;

                        if (dataSizes[i] > maxDataSize)
                        {
                            maxDataSize = dataSizes[i];
                        }
                    }
                }

                // write output file (16-bit LE PCM assumed)
                using (outputStream = File.OpenWrite(interleavedFilePath))
                {
                    // move to data section
                    for (int i = 0; i < inputFiles.Length; i++)
                    {
                        inputFileHash[i].Position = dataStartOffset[i];
                    }
                    
                    // write to file
                    while (bytesProcessed <= maxDataSize)
                    {
                        for (int i = 0; i < inputFiles.Length; i++)
                        {
                            // read bytes
                            bytesRead = inputFileHash[i].Read(bytes, 0, 2);

                            // insert filler bytes
                            for (int j = bytesRead; j < 2; j++)
                            {
                                bytes[j] = interleaveOptions.FillerByte;
                            }

                            // write bytes to output file
                            outputStream.Write(bytes, 0, 2);
                        }
                        
                        bytesProcessed += 2;
                    }
                }

            }
            finally
            {
                // close all streams
                foreach (int k in inputFileHash.Keys)
                {
                    if ((inputFileHash[k] != null) &&
                        (inputFileHash[k].CanRead))
                    {
                        inputFileHash[k].Close();
                        inputFileHash[k].Dispose();
                    }
                }

                if ((outputStream != null) &&
                    (outputStream.CanWrite))
                {
                    outputStream.Close();
                    outputStream.Dispose();
                }
            }

            return interleavedFilePath;
        }

        #endregion
    }
}
