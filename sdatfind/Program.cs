using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.format.util;
using VGMToolbox.util;

namespace sdatfind
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                usage();
                return;
            }
            
            string filePath = Path.GetFullPath(args[0]);

            // get file path and check it exists
            Console.WriteLine("Checking if file exists.");

            filePath = Path.GetFullPath(args[0]);

            if (!File.Exists(filePath))
            {
                Console.WriteLine(String.Format("File <{0}> not found.", filePath));
                return;
            }

            // open file and extract the sdat
            Console.WriteLine("Extracting SDATs.");

            try
            {
                string[] outputPaths = SdatUtil.ExtractSdatsFromFile(filePath, "_sdatfind");
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Error processing <{0}>.  Error received: ", filePath) + ex.Message);
            }                                
        }

        private static void usage()
        {
            Console.WriteLine("sdatfind.exe fileName");
        }
    }
}
