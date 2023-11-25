using System;
using System.IO;

using VGMToolbox.format.util;

namespace sdatext
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath;

            if (args.Length < 1)
            { 
                usage();
                return;
            }

            // get file path and check it exists
            Console.WriteLine("Checking if file exists.");
            
            filePath = Path.GetFullPath(args[0]);

            if (!File.Exists(filePath))
            {
                Console.WriteLine(String.Format("File <{0}> not found.", filePath));
                return;
            }

            // open file and extract the sdat
            Console.WriteLine("Extracting SDAT.");
            
            try
            {
                string outputDir = SdatUtil.ExtractSdat(filePath);
                Console.WriteLine(String.Format("Done!  SDAT extracted to: {0}", outputDir));
            }
            catch (Exception _e)
            {
                Console.WriteLine(_e.Message);
            }            
        }

        private static void usage()
        {
            Console.WriteLine("sdatext sdatfile");
        }
    }
}
