using System;
using System.IO;
using VGMToolbox.util;

namespace zlibext
{
    class Program
    {
        static void Main(string[] args)
        {
            string inFilename;
            string outFilename;
            string startOffset;

            string fullInputPath;
            string fullOutputPath;

            long longStartOffset;            

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: zlibext.exe <infile> <outfile> <start offset>");
                Console.WriteLine("   or: zlibext.exe <infile> <outfile>");
                Console.WriteLine();
                Console.WriteLine("The 2 parameter option will set <start offset> to 0.");
            }
            else
            {
                inFilename = args[0];
                outFilename = args[1];
                
                if (args.Length == 2)
                {
                    startOffset = "0";
                }
                else
                {
                    startOffset = args[2];
                }
                            
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

                        if (longStartOffset > fs.Length)
                        {
                            Console.WriteLine(String.Format("Sorry that start offset is larger than the entire file: {0}", fs.Length.ToString()));
                        }
                        else
                        {
                            try
                            {
                                CompressionUtil.DecompressZlibStreamToFile(fs, fullOutputPath, longStartOffset);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine(String.Format("Cannot decompress <{0}>.", fullInputPath));
                            }
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
