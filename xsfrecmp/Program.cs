using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VGMToolbox.format.util;


namespace xsfrecmp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename;
            string compressionLevel;
            int compressionLevelValue;
            string outputPath = null;

            XsfRecompressStruct xsfStruct;

            if ((args.Length < 1) || (args.Length > 2))
            {
                usage();
            }
            else
            {
                filename = Path.GetFullPath(args[0]);

                if (args.Length == 2)
                {
                    compressionLevel = args[1];
                }
                else
                {
                    compressionLevel = "0";
                }

                try
                {
                    if (!int.TryParse(compressionLevel, out compressionLevelValue) ||
                        (compressionLevelValue > 9) ||
                        (compressionLevelValue < 0))
                    {
                        Console.WriteLine("  Error parsing compression level.  Input must be an integer between 0 and 9.");
                    }
                    else if (!File.Exists(filename))
                    {
                        Console.WriteLine(String.Format("  Error: Input file cannot be found <{0}>", filename));
                    }
                    else if (XsfUtil.GetXsfFormatString(filename) == null)
                    {
                        Console.WriteLine("  Error: Input file does not seem to be an xSF file.");
                    }
                    else
                    {

                        xsfStruct = new XsfRecompressStruct();
                        xsfStruct.CompressionLevel = compressionLevelValue;
                        outputPath = XsfUtil.ReCompressDataSection(filename, xsfStruct);

                        if (String.IsNullOrEmpty(outputPath))
                        {
                            Console.WriteLine("  Complete: No data section to compress.");
                        }
                        else
                        {
                            Console.WriteLine(String.Format("  Complete: Recompressed and output to <{0}>", outputPath));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("  Error: {0}", ex.Message));
                }
            }
        }

        static void usage()
        {
            Console.WriteLine("xsfrecmp.exe input_file [compression level]");
            Console.WriteLine("  input_file: file to recompress");
            Console.WriteLine("  compression level: level of zlib compression to use (0-9).  Store (0) will be used if parameter not included.");

        }
    }
}
