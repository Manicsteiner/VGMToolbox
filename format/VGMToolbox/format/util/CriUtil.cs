using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VGMToolbox.util;

namespace VGMToolbox.format.util
{
    public class CriUtil
    {
        static readonly byte[] ADX_SIG_BYTES = new byte[] { 0x80, 0x00 };
        static readonly byte[] CRI_COPYRIGHT_BYTES = new byte[] { 0x28, 0x63, 0x29, 0x43, 0x52, 0x49 };

        public static string ExtractAdxStreams(string path, string filePrefix, string outputFolder, out string messages)
        {
            string extractedToFolder = String.Empty;
            StringBuilder messageBuffer = new StringBuilder();            
            messages = String.Empty;

            long offset = 0;

            uint copyrightOffset;
            byte[] copyrightBytes;
            uint totalHeaderSize;

            int encodingType;
            uint blockSize;
            uint bitDepth;
            uint channelCount;
            uint sampleRate;

            uint totalSamples;

            uint fileSize;

            int fileCount = 0;
            string outputFileName;
            string outputFilePath;

            FileInfo fi = new FileInfo(path);

            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                if (!String.IsNullOrEmpty(outputFolder))
                {
                    extractedToFolder = outputFolder;
                }
                else
                {
                    extractedToFolder = Path.Combine(Path.GetDirectoryName(path), String.Format("{0}_ADXs", Path.GetFileNameWithoutExtension(path)));
                }

                while ((offset = ParseFile.GetNextOffset(fs, offset, ADX_SIG_BYTES)) > -1)
                {

                    // get offset to copyright string
                    copyrightOffset = (uint)ByteConversion.GetUInt16BigEndian(ParseFile.ParseSimpleOffset(fs, offset + 2, 2));
                    copyrightBytes = ParseFile.ParseSimpleOffset(fs, offset + copyrightOffset - 2, CRI_COPYRIGHT_BYTES.Length);

                    // check that copyright bytes are present
                    if (ParseFile.CompareSegment(copyrightBytes, 0, CRI_COPYRIGHT_BYTES))
                    {
                        // verify this is standard ADX
                        encodingType = ParseFile.ParseSimpleOffset(fs, offset + 4, 1)[0];

                        if (encodingType != 3)
                        {
                            fileSize = 1;
                        }
                        else
                        {
                            // get other info
                            blockSize = (uint)ParseFile.ParseSimpleOffset(fs, offset + 5, 1)[0];
                            bitDepth = (uint)ParseFile.ParseSimpleOffset(fs, offset + 6, 1)[0];
                            channelCount = (uint)ParseFile.ParseSimpleOffset(fs, offset + 7, 1)[0];
                            sampleRate = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(fs, offset + 8, 4));
                            totalSamples = ByteConversion.GetUInt32BigEndian(ParseFile.ParseSimpleOffset(fs, offset + 0xC, 4));
                            totalHeaderSize = copyrightOffset + 4;

                            // calculate file size
                            fileSize = (((totalSamples + 0x1F) / (bitDepth * 8)) * channelCount * blockSize) + totalHeaderSize + blockSize;

                            // extract file
                            if (!String.IsNullOrEmpty(filePrefix))
                            {
                                outputFileName = String.Format("{0}_{1}.adx", filePrefix, fileCount.ToString("X8"));
                            }
                            else
                            {
                                outputFileName = String.Format("{0}_{1}.adx", Path.GetFileNameWithoutExtension(path), fileCount.ToString("X8"));
                            }
                            
                            outputFilePath = Path.Combine(extractedToFolder, outputFileName);

                            ParseFile.ExtractChunkToFile(fs, offset, (int)fileSize, outputFilePath, true, true);

                            messageBuffer.AppendFormat("{0} - offset: 0x{1} size: 0x{2}{3}", outputFileName, offset.ToString("X8"), fileSize.ToString("X8"), Environment.NewLine);

                            fileCount++;
                        }

                        offset += fileSize;
                    }
                    else
                    {
                        offset += 1;
                    }
                }
            }

            messages = messageBuffer.ToString();

            return extractedToFolder;
        }
    }
}
