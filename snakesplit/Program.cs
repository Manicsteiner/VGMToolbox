using System;
using System.IO;
using VGMToolbox.util;

namespace snakesplit
{
    class Program
    {
        static void Main(string[] args)
        {
            string inFilename;
            long startOffset;
            long blockSize;

            string fullInputPath;
            string fullOutputPath;
            string outputDirectory;
            string filePrefix;

            long currentOffset;
            long fileSize;
            uint fileCount = 0;
            
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: snakesplit.exe <infile> <start offset> <blocksize>");
            }
            else
            {

                inFilename = args[0];
                startOffset = ByteConversion.GetLongValueFromString(args[1]);
                blockSize = ByteConversion.GetLongValueFromString(args[2]);
                
                fullInputPath = Path.GetFullPath(inFilename);
                filePrefix = Path.GetFileNameWithoutExtension(fullInputPath);
                outputDirectory = Path.Combine(Path.GetDirectoryName(fullInputPath), "snakesplit");

                if (File.Exists(fullInputPath))
                {
                    using (FileStream fs = File.OpenRead(fullInputPath))
                    {
                        fileSize = fs.Length;
                        currentOffset = startOffset;

                        while (currentOffset < fileSize)
                        {
                            fullOutputPath = Path.Combine(outputDirectory, String.Format("{0}_{1}.bin", filePrefix, fileCount.ToString("X8")));
                            fileCount++;

                            ParseFile.ExtractChunkToFile(fs, currentOffset, blockSize, fullOutputPath);

                            currentOffset += blockSize;
                        }

                    }
                }
                else
                {
                    Console.WriteLine("File Not Found: " + fullInputPath);
                }
            }
        }
    }
}
