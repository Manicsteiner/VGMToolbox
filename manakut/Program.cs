using System;
using System.IO;
using VGMToolbox.util;

namespace manakut
{
    class Program
    {
        static void Main(string[] args)
        {
            const string LBA_SWITCH = "/LBA";
            const string MULTIPLIER_SWITCH = "/M";

            string inFilename;
            string outFilename;
            string startOffset;
            string cutSize;

            string fullInputPath;
            string fullOutputPath;

            long longStartOffset;
            long longCutSize;

            bool doLba = false;
            bool doMultiplier = false;
            string multiplierChunk;
            long multiplierValue = 1;
            char[] multiplierSplitParam = new char[1] { '=' };

            if (args.Length < 3)
            {
                Console.WriteLine("Usage: manakut.exe <infile> <outfile> <start offset> <cut size> [/lba|/m=<multiplier>]");
                Console.WriteLine("   or: manakut.exe <infile> <outfile> <start offset> [/lba|/m=<multiplier>]");
                Console.WriteLine();
                Console.WriteLine("The 4 parameter option will read from <start offset> to EOF.");
                Console.WriteLine("Use the /lba switch to multiply start offset by 0x800.");
                Console.WriteLine("Use the /m switch to multiply start offset by the value after the equals sign.");
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
                        longStartOffset = VGMToolbox.util.ByteConversion.GetLongValueFromString(startOffset);

                        // check for LBA or MULTIPLIER switch
                        if ((args.Length > 4) && (args[4].ToUpper().Equals(LBA_SWITCH)))
                        {
                            doLba = true;
                        }
                        else if ((args.Length > 4) && (args[4].ToUpper().Substring(0, 2).Equals(MULTIPLIER_SWITCH)))
                        {
                            doMultiplier = true;
                            multiplierChunk = args[4].Split(multiplierSplitParam)[1];
                            multiplierValue = VGMToolbox.util.ByteConversion.GetLongValueFromString(multiplierChunk);
                        }
                        else if ((args.Length > 3) && (args[3].ToUpper().Equals(LBA_SWITCH)))
                        {
                            doLba = true;
                        }
                        else if ((args.Length > 3) &&
                                 (args[3].Length >= MULTIPLIER_SWITCH.Length) &&
                                 (args[3].ToUpper().Substring(0, 2).Equals(MULTIPLIER_SWITCH)))
                        {
                            doMultiplier = true;
                            multiplierChunk = args[3].Split(multiplierSplitParam)[1];
                            multiplierValue = VGMToolbox.util.ByteConversion.GetLongValueFromString(multiplierChunk);
                        }

                        // GET CUTSIZE
                        if ((args.Length > 3) &&                            
                            (!args[3].ToUpper().Equals(LBA_SWITCH)) &&
                            ((args[3].Length < MULTIPLIER_SWITCH.Length) || (!args[3].ToUpper().Substring(0, 2).Equals(MULTIPLIER_SWITCH))))
                        {
                            cutSize = args[3];
                            longCutSize = VGMToolbox.util.ByteConversion.GetLongValueFromString(cutSize);
                        }
                        else
                        {
                            longCutSize = fs.Length - longStartOffset;
                        }

                        // set LBA/MULTIPLIER values
                        if (doLba)
                        {
                            longStartOffset *= 0x800;
                        }
                        else if (doMultiplier)
                        {
                            longStartOffset *= multiplierValue;
                        }

                        ParseFile.ExtractChunkToFile(fs, longStartOffset, longCutSize, fullOutputPath);
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
