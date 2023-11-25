using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;

using VGMToolbox.format.sdat;

namespace _2sftimer
{
    class Program

    {
        public const string SSEQ2MID_TXT = "sseq2mid_output.txt";
        public const string PSFPOINT_BATCH_TXT = "psfpoint_batch.bat";

        public const string SSEQ2MID_TXT_MARKER = ".sseq:";
        public const string SSEQ2MID_TXT_END_OF_TRACK = "End of Track";

        public const string EMPTY_FILE_DIRECTORY = "Empty_Files";

        static void Main(string[] args)
        {
            string pathTo2sf;
            string pathToSdat;
            string filePrefix;
            string smapPath = String.Empty;
            bool processSuccess = false;
            string emptyFolderFileName;

            if (args.Length != 3)
            {
                usage();
                return;
            }
            else
            {
                pathTo2sf = args[0];
                pathToSdat = args[1];
                filePrefix = args[2];
            }

            if (!Directory.Exists(pathTo2sf))
            {
                Console.WriteLine(String.Format("Cannot find directory <{0}>", pathTo2sf));
                return;
            }

            if (!File.Exists(pathToSdat))
            {
                Console.WriteLine(String.Format("Cannot find file <{0}>", pathToSdat));
                return;
            }

            if (!String.IsNullOrEmpty(smapPath) && !File.Exists(smapPath))
            {
                Console.WriteLine(String.Format("Cannot find file <{0}>", smapPath));
                return;
            }

            string sseq2MidPath = Path.Combine(Path.Combine(".", "helper"), "sseq2mid.exe");
            string psfpointPath = Path.Combine(Path.Combine(".", "helper"), "psfpoint.exe");

            // delete old .bat file
            string psfpointBatchFilePath = Path.Combine(Path.Combine(pathTo2sf, "text"), PSFPOINT_BATCH_TXT);

            Console.WriteLine();
            
            if (File.Exists(psfpointBatchFilePath))
            {
                Console.WriteLine("Deleting Old Batch File");
                File.Delete(psfpointBatchFilePath);
            }
                        
            // Extract SDAT
            Console.WriteLine("Extracting SDAT");

            string extractedSdatPath = Path.Combine(Path.GetDirectoryName(pathToSdat), Path.GetFileNameWithoutExtension(pathToSdat));
            if (Directory.Exists(extractedSdatPath))
            {
                extractedSdatPath += String.Format("_temp_{0}", new Random().Next().ToString()); 
            }

            string extractedSseqPath = Path.Combine(extractedSdatPath, "Seq");
            
            FileStream fs = File.Open(pathToSdat, FileMode.Open, FileAccess.Read);
            Sdat sdat = new Sdat();
            sdat.Initialize(fs, pathToSdat);
            sdat.ExtractSseqs(fs, extractedSdatPath);            
            fs.Close();
            fs.Dispose();

            // Make SMAP
            Console.WriteLine("Building Internal SMAP");
            Smap smap = new Smap(sdat);

            // Loop through SMAP and build timing script
            Console.WriteLine("Building Timing Script");
            string emptyFileDir = Path.Combine(pathTo2sf, EMPTY_FILE_DIRECTORY);

            int totalSequences = smap.SseqSection.Length;
            int i = 1;

            // Initialize Bass            
            if (smap.SseqSection.Length > 0)
            {
                Console.WriteLine("Going to sleep for 5 seconds to avoid the freeware billboard...");
                Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero, null);
                Thread.Sleep(5000);
                Console.WriteLine("Ok, back to work...");
            }

            foreach (Smap.SmapSeqStruct s in smap.SseqSection)
            {
                Console.Write("\r" + String.Format("Processing [{0}/{1}]",
                    i.ToString().PadLeft(4, '0'), totalSequences.ToString().PadLeft(4, '0')));
                
                string rippedFileName = String.Format("{0}-{1}.mini2sf", filePrefix, s.number.ToString("X4"));
                string rippedFilePath = Path.Combine(pathTo2sf, rippedFileName);

                // check if file is empty or not
                if (s.fileID == Smap.EMPTY_FILE_ID)
                {
                    // move to empty dir
                    if (!Directory.Exists(emptyFileDir))
                    {
                        Directory.CreateDirectory(emptyFileDir);
                    }

                    if (File.Exists(rippedFilePath))
                    {
                        emptyFolderFileName = Path.Combine(emptyFileDir, rippedFileName);
                        File.Copy(rippedFilePath, emptyFolderFileName, true);
                        File.Delete(rippedFilePath);
                    }
                }
                else
                {
                    string sseqFilePath = Path.Combine(extractedSseqPath, s.name);
                    
                    // convert sseq file to midi
                    processSuccess = convertSseqFile(sseq2MidPath, pathTo2sf,
                        sseqFilePath);

                    // time file
                    if (processSuccess)
                    {
                        processSuccess = buildFileTimingBatch(pathTo2sf,
                            rippedFilePath, sseqFilePath);
                    }
                }
                i++;
            }

            // run timing script
            Console.WriteLine(Environment.NewLine + "Executing Timing Script");
            executeBatchScript(psfpointPath, pathTo2sf);

            // Cleanup
            Console.WriteLine("Cleaning Up");
            doCleanup(extractedSdatPath, pathTo2sf, psfpointPath);

        }

        private static void usage()
        {
            Console.WriteLine("Usage: 2sftimer.exe <mini2sf directory> <sdat path> <filename prefix>");
        }

        private static bool convertSseqFile(string pSseq2MidToolPath, string pMini2sfPath, 
            string pSseqFilePath)
        {
            Process ndsProcess;
            bool isSuccess;

            // convert existing sseq to mid            
            string sseqPath = Path.GetDirectoryName(pSseqFilePath);

            string sseq2MidDestinationPath = Path.Combine(sseqPath, Path.GetFileName(pSseq2MidToolPath));

            try
            {
                File.Copy(pSseq2MidToolPath, sseq2MidDestinationPath, true);

                string arguments = String.Format(" -2 -l {0}", Path.GetFileName(pSseqFilePath));

                ndsProcess = new Process();

                ndsProcess.StartInfo = new ProcessStartInfo(sseq2MidDestinationPath, arguments);
                ndsProcess.StartInfo.WorkingDirectory = sseqPath;
                ndsProcess.StartInfo.UseShellExecute = false;
                ndsProcess.StartInfo.CreateNoWindow = true;
                ndsProcess.StartInfo.RedirectStandardOutput = true;
                isSuccess = ndsProcess.Start();
                string sseqOutputFile = ndsProcess.StandardOutput.ReadToEnd();
                ndsProcess.WaitForExit();

                // output redirected standard output
                string textOutputPath = Path.Combine(pMini2sfPath, "text");
                string sseq2MidOutputPath = Path.Combine(textOutputPath, SSEQ2MID_TXT);

                if (!Directory.Exists(textOutputPath)) { Directory.CreateDirectory(textOutputPath); }

                TextWriter tw = File.CreateText(sseq2MidOutputPath);
                tw.Write(sseqOutputFile);
                tw.Close();
                tw.Dispose();
            }
            catch (Exception _e)
            {
                isSuccess = false;
                Console.WriteLine(_e.Message);
            }

            return isSuccess;
        }

        private static bool buildFileTimingBatch(string pMini2sfPath,
            string p2sfFilePath, string pSseqFilePath)
        {
            bool isSuccess = false;
            string arguments;

            StreamWriter sw;

            StringBuilder strReturn = new StringBuilder(128);
            int minutes;
            int seconds;

            int midiStream;

            string _2sfFileName = Path.GetFileName(p2sfFilePath);

            string psfpointBatchFilePath = Path.Combine(Path.Combine(pMini2sfPath, "text"), PSFPOINT_BATCH_TXT);

            if (!File.Exists(psfpointBatchFilePath))
            {
                sw = File.CreateText(psfpointBatchFilePath);
            }
            else
            {
                sw = new StreamWriter(File.Open(psfpointBatchFilePath, FileMode.Append, FileAccess.Write));
            }

            try
            {
                string midiFilePath = pSseqFilePath + ".mid";

                if (File.Exists(midiFilePath))
                {
                    // Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero, null);
                    midiStream = BassMidi.BASS_MIDI_StreamCreateFile(midiFilePath, 0L, 0L, BASSFlag.BASS_DEFAULT, 0);

                    if (midiStream != 0)
                    {
                        // play the channel
                        long length = Bass.BASS_ChannelGetLength(midiStream);
                        double tlength = Bass.BASS_ChannelBytes2Seconds(midiStream, length);
                        minutes = (int)(tlength / 60);
                        seconds = (int)(tlength - (minutes * 60));

                        if (seconds > 59)
                        {
                            minutes++;
                            seconds -= 60;
                        }
                    }
                    else
                    {
                        // error
                        Console.Write(Environment.NewLine);
                        Console.WriteLine("Stream error: {0}", Bass.BASS_ErrorGetCode());
                        return false;
                    }


                    // Do Fade
                    if (isLoopingTrack(pMini2sfPath, Path.GetFileName(pSseqFilePath)))
                    {
                        arguments = " -fade=\"10\" " + _2sfFileName;

                        if (minutes == 0 && seconds == 0)
                        {
                            seconds = 1;
                        }
                    }
                    else
                    {
                        arguments = " -fade=\"1\" " + _2sfFileName;
                        seconds++;
                        if (seconds > 60)
                        {
                            minutes++;
                            seconds -= 60;
                        }

                    }

                    // Add fade info to batch file
                    sw.WriteLine("psfpoint.exe" + arguments);

                    // Add length info to batch file
                    arguments = " -length=\"" + minutes + ":" + seconds.ToString().PadLeft(2, '0') + "\" " + _2sfFileName;
                    sw.WriteLine("psfpoint.exe" + arguments);
                }

                isSuccess = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Error timing {0}: {1}", p2sfFilePath, e.Message));
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }

            return isSuccess;
        }

        private static void executeBatchScript(string pPsfPointPath, string pMini2sfPath)
        {
            Process ndsProcess;
            
            // copy psfpoint.exe
            string psfpointDestinationPath = Path.Combine(pMini2sfPath, Path.GetFileName(pPsfPointPath));
            File.Copy(pPsfPointPath, psfpointDestinationPath, true);

            // copy script
            string psfpointBatchFilePath = Path.Combine(Path.Combine(pMini2sfPath, "text"), PSFPOINT_BATCH_TXT);
            string psfpointBatchFileDestinationPath = Path.Combine(pMini2sfPath, PSFPOINT_BATCH_TXT);
            File.Copy(psfpointBatchFilePath, psfpointBatchFileDestinationPath, true);

            // execute script
            ndsProcess = new Process();
            ndsProcess.StartInfo = new ProcessStartInfo(PSFPOINT_BATCH_TXT);
            ndsProcess.StartInfo.WorkingDirectory = pMini2sfPath;
            ndsProcess.StartInfo.CreateNoWindow = true;
            ndsProcess.Start();
            ndsProcess.WaitForExit();

            // delete files
            // File.Delete(psfpointDestinationPath);
            File.Delete(psfpointBatchFileDestinationPath);
        }

        private static bool isLoopingTrack(string pMini2sfPath, string pSequenceName)
        {
            string sseq2MidOutputPath = Path.Combine(Path.Combine(pMini2sfPath, "text"), SSEQ2MID_TXT);
            string oneLineBack = String.Empty;
            string twoLinesBack = String.Empty;

            bool _ret = true;

            // Check Path
            if (File.Exists(sseq2MidOutputPath))
            {
                string inputLine = String.Empty;

                TextReader textReader = new StreamReader(sseq2MidOutputPath);
                while ((inputLine = textReader.ReadLine()) != null)
                {
                    // Check for the incoming sequence name
                    string sseqFileName = Path.GetFileName(pSequenceName);
                    if (inputLine.Trim().Contains(sseqFileName))
                    {
                        // Skip columns headers
                        textReader.ReadLine();

                        // Read until EOF or End of SEQ section (blank line)
                        while (((inputLine = textReader.ReadLine()) != null) &&
                               !inputLine.Trim().Contains(SSEQ2MID_TXT_MARKER))
                        {
                            twoLinesBack = oneLineBack;
                            oneLineBack = inputLine;
                        }

                        if (twoLinesBack.Contains(SSEQ2MID_TXT_END_OF_TRACK))
                        {
                            _ret = false;
                        }
                    }


                }

                textReader.Close();
                textReader.Dispose();
            }

            return _ret;
        }

        private static void doCleanup(string pExtractedSdatPath, string pMini2sfPath, string pPsfPointPath)
        {
            Directory.Delete(pExtractedSdatPath, true);
        }
    }
}
