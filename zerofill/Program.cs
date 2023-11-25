using System;
using System.IO;

using VGMToolbox.util;

namespace zerofill
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
                Console.WriteLine("Usage: zerofill.exe <infile> <outfile> <start offset> <end offset>");
                Console.WriteLine("   or: zerofill.exe <infile> <outfile> <start offset>");
                Console.WriteLine();
                Console.WriteLine("The 3 parameter option will fill from <start offset> to EOF.");
                Console.WriteLine(String.Format("The only current limitation is that fill size cannot exceed [{0}].", int.MaxValue.ToString()));
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
                        using (FileStream fs = File.OpenRead(fullInputPath))
                        {
                            longEndOffset = fs.Length;
                        }
                    }

                    long size = ((longEndOffset - longStartOffset) + 1);

                    if (size > (long)int.MaxValue)
                    {
                        Console.WriteLine(String.Format("Sorry that fill size is too large: {0}", size.ToString()));
                    }
                    else
                    {
                        try
                        {
                            File.Copy(fullInputPath, fullOutputPath, false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(String.Format("Cannot create destination file <{0}>: {1}", fullOutputPath, ex.Message));
                        }

                        FileUtil.ZeroOutFileChunk(fullOutputPath, longStartOffset, (int)size);
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
