using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.format.sdat;
using VGMToolbox.format.util;
using VGMToolbox.util;

namespace sdatopt
{
    class Program
    {
        private static readonly string APPLICATION_PATH = Assembly.GetExecutingAssembly().Location;
        
        static void Main(string[] args)
        {
            if ((args.Length > 3) || (args.Length < 2))
            {
                Console.WriteLine("usage: sdatopt.exe fileName start_sequence end_sequence");
                Console.WriteLine("       sdatopt.exe fileName ALL");
                Console.WriteLine("       sdatopt.exe fileName PREP");
                Console.WriteLine("       sdatopt.exe fileName MAP smap_file");
                Console.WriteLine("       sdatopt.exe fileName MAP");
                Console.WriteLine();
                Console.WriteLine("fileName: .sdat or .2sflib containing SDAT to optimize");
                Console.WriteLine("start_sequence: starting sequence number to keep");
                Console.WriteLine("end_sequence: endinging sequence number to keep");
                Console.WriteLine("ALL: use this if you wish to keep all sequences");
                Console.WriteLine("PREP: use this to output an SMAP to use for sequence selection.  Delete the entire line of sequences you do not want to include.");
                Console.WriteLine("MAP smap_file: uses smap_file from PREP to select sequences to keep.");
                Console.WriteLine("MAP: looks for an smap file based on the sdat name select sequences to keep.  Must be in proper format.");
            }
            else
            {
                Sdat sdat = null;
                bool is2sfSource = false; ;

                string sdatDirectory;
                string sdatOptimizingFileName;
                string sdatOptimizingPath;

                string sdatCompletedFileName;
                string sdatCompletedPath;

                int startSequence = Sdat.NO_SEQUENCE_RESTRICTION;
                int endSequence = Sdat.NO_SEQUENCE_RESTRICTION;

                string[] extractedSdats = null;
                string decompressedDataPath = null;
                string extractedToFolder = null;

                ArrayList cleanupList = new ArrayList();

                string filename = Path.GetFullPath(args[0]);
                string smapFileName = null; 

                if (!File.Exists(filename))
                {
                    Console.WriteLine("Cannot find SDAT: {0}", filename);
                    return;
                }

                if (args[1].Trim().ToUpper().Equals("MAP"))
                {
                    if (args.Length < 3)
                    {
                        smapFileName = Path.ChangeExtension(filename, Smap.FILE_EXTENSION);
                    }
                    else
                    {
                        smapFileName = Path.GetFullPath(args[2]);
                    }
                    
                    if (!File.Exists(smapFileName))
                    {
                        Console.WriteLine("Cannot find SMAP: {0}", smapFileName);
                        return;
                    }
                }

                sdatDirectory = Path.GetDirectoryName(filename);
                sdatOptimizingFileName = String.Format("{0}_OPTIMIZING{1}",
                    Path.GetFileNameWithoutExtension(filename), Path.GetExtension(filename));
                sdatOptimizingPath = Path.Combine(sdatDirectory, sdatOptimizingFileName);

                sdatCompletedFileName = String.Format("{0}_OPTIMIZED{1}",
                    Path.GetFileNameWithoutExtension(filename), Path.GetExtension(filename));
                sdatCompletedPath = Path.Combine(sdatDirectory, sdatCompletedFileName);

                try
                {
                    File.Copy(filename, sdatOptimizingPath, true);

                    using (FileStream fs = File.Open(sdatOptimizingPath, FileMode.Open, FileAccess.Read))
                    {
                        Type dataType = FormatUtil.getObjectType(fs);

                        if (dataType != null)
                        {
                            if (dataType.Name.Equals("Sdat"))
                            {
                                Console.WriteLine("Input file is an SDAT.");
                                Console.WriteLine("Building Internal SDAT.");
                                sdat = new Sdat();
                                sdat.Initialize(fs, sdatOptimizingPath);
                            }
                            else if (dataType.Name.Equals("Xsf")) // is an Xsf, confirm it is a 2sf
                            {
                                Xsf libFile = new Xsf();
                                libFile.Initialize(fs, sdatOptimizingPath);

                                if (libFile.GetFormat().Equals(Xsf.FormatName2sf))
                                {
                                    Console.WriteLine("Input file is a 2SF.");

                                    is2sfSource = true;

                                    // close stream, we're gonna need this file
                                    fs.Close();
                                    fs.Dispose();

                                    // unpack compressed section
                                    Console.WriteLine("Decompressing Compressed Data section of 2SF.");

                                    Xsf2ExeStruct xsf2ExeStruct = new Xsf2ExeStruct();
                                    xsf2ExeStruct.IncludeExtension = true;
                                    xsf2ExeStruct.StripGsfHeader = false;
                                    decompressedDataPath = XsfUtil.ExtractCompressedDataSection(sdatOptimizingPath, xsf2ExeStruct);

                                    // extract SDAT
                                    Console.WriteLine("Extracting SDAT from Decompressed Compressed Data Section.");

                                    VGMToolbox.util.FindOffsetStruct findOffsetStruct = new VGMToolbox.util.FindOffsetStruct();
                                    findOffsetStruct.SearchString = Sdat.ASCII_SIGNATURE_STRING;
                                    findOffsetStruct.TreatSearchStringAsHex = true;
                                    findOffsetStruct.CutFile = true;
                                    findOffsetStruct.SearchStringOffset = "0";
                                    findOffsetStruct.CutSize = "8";
                                    findOffsetStruct.CutSizeOffsetSize = "4";
                                    findOffsetStruct.IsCutSizeAnOffset = true;
                                    findOffsetStruct.OutputFileExtension = ".sdat";
                                    findOffsetStruct.IsLittleEndian = true;

                                    findOffsetStruct.UseTerminatorForCutSize = false;
                                    findOffsetStruct.TerminatorString = null;
                                    findOffsetStruct.TreatTerminatorStringAsHex = false;
                                    findOffsetStruct.IncludeTerminatorLength = false;
                                    findOffsetStruct.ExtraCutSizeBytes = null;

                                    string output;
                                    extractedToFolder = ParseFile.FindOffsetAndCutFile(decompressedDataPath, findOffsetStruct, out output, false, false);

                                    // create SDAT object                                                                        
                                    Console.WriteLine("Building Internal SDAT.");
                                    extractedSdats = Directory.GetFiles(extractedToFolder, "*.sdat");

                                    if (extractedSdats.Length > 1)
                                    {
                                        Console.WriteLine("Sorry, this 2SF file contains more than 1 SDAT.  sdatopt cannot currently handle this.");
                                        return;
                                    }
                                    else if (extractedSdats.Length == 0)
                                    {
                                        Console.WriteLine("ERROR: Did not find an SDAT in the Decompressed Data Section.");
                                        return;
                                    }
                                    else
                                    {
                                        
                                        
                                        using (FileStream sdatFs = File.Open(extractedSdats[0], FileMode.Open, FileAccess.Read))
                                        {
                                            sdat = new Sdat();
                                            sdat.Initialize(sdatFs, extractedSdats[0]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("ERROR: Cannot determine format of the input file.");
                                return;
                            }
                        }
                    }

                    if (sdat != null)
                    {
                        if (args[1].Trim().ToUpper().Equals("PREP"))
                        {
                            sdat.BuildSmapPrep(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
                        }
                        else if (args[1].Trim().ToUpper().Equals("MAP"))
                        {
                            ArrayList allowedSequences = buildSequenceList(smapFileName);

                            Console.WriteLine("Optimizing SDAT.");
                            sdat.OptimizeForZlib(allowedSequences);

                        }
                        else
                        {
                            if (!args[1].Trim().ToUpper().Equals("ALL"))
                            {
                                if (!String.IsNullOrEmpty(args[1]))
                                {
                                    startSequence = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(args[1]);
                                }

                                if (!String.IsNullOrEmpty(args[2]))
                                {
                                    endSequence = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(args[2]);
                                }
                            }

                            Console.WriteLine("Optimizing SDAT.");
                            sdat.OptimizeForZlib(startSequence, endSequence);
                        }
                    }

                    if (is2sfSource)
                    {
                        if (!args[1].Trim().ToUpper().Equals("PREP"))
                        {
                            // replace SDAT section
                            Console.WriteLine("Inserting SDAT back into Decompressed Compressed Data Section.");
                            long sdatOffset;
                            using (FileStream dcFs = File.Open(decompressedDataPath, FileMode.Open, FileAccess.ReadWrite))
                            {
                                sdatOffset = ParseFile.GetNextOffset(dcFs, 0, Sdat.ASCII_SIGNATURE);
                            }

                            FileInfo fi = new FileInfo(extractedSdats[0]);
                            FileUtil.ReplaceFileChunk(extractedSdats[0], 0, fi.Length, decompressedDataPath, sdatOffset);

                            // rebuild 2sf
                            Console.WriteLine("Rebuilding 2sf File.");

                            string bin2PsfStdOut = String.Empty;
                            string bin2PsfStdErr = String.Empty;
                            
                            XsfUtil.Bin2Psf(Path.GetExtension(filename).Substring(1), (int)Xsf.Version2sf,
                                decompressedDataPath, ref bin2PsfStdOut, ref bin2PsfStdErr);
                            
                            Console.WriteLine("Cleaning up intermediate files.");
                            File.Copy(Path.ChangeExtension(decompressedDataPath, Path.GetExtension(filename)), sdatOptimizingPath, true);
                            File.Delete(Path.ChangeExtension(decompressedDataPath, Path.GetExtension(filename)));
                        }
                        
                        File.Delete(decompressedDataPath);
                        Directory.Delete(extractedToFolder, true);
                    }

                    if (!args[1].Trim().ToUpper().Equals("PREP"))
                    {
                        Console.WriteLine("Copying to OPTIMIZED file.");
                        File.Copy(sdatOptimizingPath, sdatCompletedPath, true);
                    }

                    Console.WriteLine("Deleting OPTIMIZING file.");
                    File.Delete(sdatOptimizingPath);

                    Console.WriteLine("Optimization Complete.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("ERROR: {0}", ex.Message));
                }
            }
        }

        static ArrayList buildSequenceList(string pSmapPath)
        {
            ArrayList sequences = new ArrayList();
            string seqIndex;

            using (StreamReader sr = File.OpenText(pSmapPath))
            {
                string lineIn = String.Empty;

                // skip first two lines
                lineIn = sr.ReadLine();
                lineIn = sr.ReadLine();

                while ((lineIn = sr.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(lineIn.Trim()))
                    {
                        seqIndex = lineIn.Split(' ')[0].Trim();
                        sequences.Add(int.Parse(seqIndex));
                    }                    
                }
            }

            return sequences;
        }
    }
}
