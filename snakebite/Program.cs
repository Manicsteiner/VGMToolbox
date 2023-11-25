using System;
using System.IO;

using VGMToolbox.util;

namespace snakebite
{
    class Program
    {
        static void Main(string[] args)
        {
            string inFilename;
            string outFilename;
            string startOffset;
            string endOffset;

            string fullInputPath;
            string fullOutputPath;

            long longStartOffset;
            long longEndOffset;

            if (args.Length < 3)
            {
                Console.WriteLine("Usage: snakebite.exe <infile> <outfile> <start offset> <end offset>");
                Console.WriteLine("   or: snakebite.exe <infile> <outfile> <start offset>");
                Console.WriteLine();
                Console.WriteLine("The 3 parameter option will read from <start offset> to EOF.");
            }
            else
            {

                inFilename = args[0];
                outFilename = args[1];
                startOffset = args[2];


                fullInputPath = Path.GetFullPath(inFilename);
                fullOutputPath = Path.GetFullPath(outFilename);

                if (File.Exists(fullInputPath))
                {

                    using (FileStream fs = File.OpenRead(fullInputPath))
                    {

                        if (startOffset.StartsWith("0x"))
                        {
                            startOffset = startOffset.Substring(2);
                            longStartOffset = long.Parse(startOffset, System.Globalization.NumberStyles.HexNumber, null);
                        }
                        else
                        {
                            longStartOffset = long.Parse(startOffset, System.Globalization.NumberStyles.Integer, null);
                        }

                        if (args.Length > 3)
                        {
                            endOffset = args[3];

                            if (endOffset.StartsWith("0x"))
                            {
                                endOffset = endOffset.Substring(2);
                                longEndOffset = long.Parse(endOffset, System.Globalization.NumberStyles.HexNumber, null);
                            }
                            else
                            {
                                longEndOffset = long.Parse(endOffset, System.Globalization.NumberStyles.Integer, null);
                            }
                        }
                        else
                        {
                            longEndOffset = fs.Length;
                        }

                        long size = ((longEndOffset - longStartOffset) + 1);

                        ParseFile.ExtractChunkToFile(fs, longStartOffset, size, fullOutputPath);
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
