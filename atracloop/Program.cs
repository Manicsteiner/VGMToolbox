using System;
using System.IO;

using VGMToolbox.util;

namespace atracloop
{
    class Program
    {
        private const string POS_FILE_EXTENSION = ".pos";
        private const string WAV_FILE_EXTENSION = ".wav";
        private const long DEFAULT_LOOP_VALUE = -99887766;
        private const string PLAYLIST_FILE_NAME = "!playlist.m3u";

        static void usage()
        {
            Console.WriteLine("atracloop <source file> <output .pos file> <.m3u filename>");
            Console.WriteLine("   .pos file will only be created for looping files.");
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                usage();
                return;
            }
            
            // file names
            string sourceFileName = Path.GetFullPath(args[0]);
            string posFileName = Path.GetFullPath(args[1]);
            string wavFileName = Path.ChangeExtension(posFileName, WAV_FILE_EXTENSION);
            string m3uFileName = Path.GetFullPath(args[2]);

            // loop values
            long loopStartValue = DEFAULT_LOOP_VALUE;
            long loopEndValue = DEFAULT_LOOP_VALUE;
            long loopShiftValue = DEFAULT_LOOP_VALUE;

            using (FileStream fs = File.Open(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Console.WriteLine(String.Format("Processing {0}", Path.GetFileName(sourceFileName)));
                
                // loop start
                loopStartValue = GetRiffValue(fs, "smpl", "0x34");
                
                if (loopStartValue != DEFAULT_LOOP_VALUE)
                {
                    Console.WriteLine(String.Format("     Loop Start (Initial): 0x{0}", loopStartValue.ToString("X8")));

                    // loop end
                    loopEndValue = GetRiffValue(fs, "smpl", "0x38");
                    Console.WriteLine(String.Format("     Loop End (Initial): 0x{0}", loopEndValue.ToString("X8")));

                    if ((loopStartValue != DEFAULT_LOOP_VALUE) && (loopStartValue != DEFAULT_LOOP_VALUE))
                    {
                        // loop shift
                        loopShiftValue = GetRiffValue(fs, "fact", "0xC");
                        Console.WriteLine(String.Format("     Loop Shift: 0x{0}", loopShiftValue.ToString("X8")));

                        // apply shift
                        if (loopShiftValue > 0)
                        {
                            loopStartValue -= loopShiftValue;
                            loopEndValue -= loopShiftValue;

                            Console.WriteLine(String.Format("     Loop Start (Shifted): 0x{0}", loopStartValue.ToString("X8")));
                            Console.WriteLine(String.Format("     Loop End (Shifted): 0x{0}", loopEndValue.ToString("X8")));
                        }

                        Console.WriteLine(String.Format("     Loop Position: [{0}, {1}]", 
                            loopStartValue.ToString(),
                            loopEndValue.ToString()));

                        // write .pos file
                        WritePosFile(posFileName, loopStartValue, loopEndValue);
                        Console.WriteLine(String.Format("     Writing .POS File: {0}", Path.GetFileName(posFileName)));

                        // update .m3u to add .pos file
                        UpdateM3uFile(m3uFileName, Path.GetFileName(posFileName));
                        Console.WriteLine(String.Format("     Updating M3U: {0} (Add {1})", 
                            Path.GetFileName(m3uFileName), 
                            Path.GetFileName(posFileName)));

                    }  // if ((loopStartValue != DEFAULT_LOOP_VALUE) && (loopStartValue != DEFAULT_LOOP_VALUE))

                } // if (loopStartValue != DEFAULT_LOOP_VALUE)
                else
                {
                    Console.WriteLine("     No looping information found.");

                    // update m3u to add .wav since .pos won't exist
                    UpdateM3uFile(m3uFileName, Path.GetFileName(wavFileName));
                    Console.WriteLine(String.Format("     Updating M3U: {0} (Add {1})",
                        Path.GetFileName(m3uFileName),
                        Path.GetFileName(wavFileName)));
                }

            } // using (FileStream fs = File.Open(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))

            Console.WriteLine(); // adding a blank line for cleanliness
        }

        static void WritePosFile(string posFileName, long loopStart, long loopEnd)
        {
            using (FileStream posStream = File.Open(posFileName, FileMode.Create, FileAccess.Write))
            {
                byte[] loopPointBytes = new byte[4];

                loopPointBytes = BitConverter.GetBytes((uint)loopStart);
                posStream.Write(loopPointBytes, 0, 4);

                loopPointBytes = BitConverter.GetBytes((uint)loopEnd);
                posStream.Write(loopPointBytes, 0, 4);
            }

        }

        static void UpdateM3uFile(string m3uFileName, string newEntry)
        {
            using (StreamWriter m3uWriter = new StreamWriter(File.Open(m3uFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)))
            {
                m3uWriter.BaseStream.Position = m3uWriter.BaseStream.Length;
                m3uWriter.WriteLine(newEntry);
            }
        }

        static long GetRiffValue(FileStream fs, string chunk, string offset)
        {
            long ret = DEFAULT_LOOP_VALUE;

            // riff values
            RiffCalculatingOffsetDescription riffCalculatingOffset = new RiffCalculatingOffsetDescription();
            riffCalculatingOffset.RelativeLocationToRiffChunkString = RiffCalculatingOffsetDescription.START_OF_STRING;
            riffCalculatingOffset.OffsetSize = "4";
            riffCalculatingOffset.OffsetByteOrder = Constants.LittleEndianByteOrder;

            try
            {
                riffCalculatingOffset.RiffChunkString = chunk;
                riffCalculatingOffset.OffsetValue = offset;

                ret = ParseFile.GetRiffCalculatedVaryingByteValueAtAbsoluteOffset(fs, riffCalculatingOffset, true);
            }
            catch (IndexOutOfRangeException iorEx)
            {
                // Console.WriteLine(String.Format("Error processing RIFF item for <{0}>: {1}", Path.GetFileName(fs.Name), iorEx.Message));
            }

            return ret;
        }
    }
}
