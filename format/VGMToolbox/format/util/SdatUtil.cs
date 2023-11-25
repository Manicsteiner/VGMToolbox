using System;
using System.Collections;
using System.IO;

using VGMToolbox.format.sdat;
using VGMToolbox.util;

namespace VGMToolbox.format.util
{
    public class SdatUtil
    {
        public static string ExtractSdat(string pSdatPath)
        {
            string fullPath = Path.GetFullPath(pSdatPath);
            string outputPath = null;
            string waveArcOutputPath;

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(String.Format("Cannot find file <{0}>", fullPath));
            }
            else
            {
                Swar swar = new Swar();
                string swavOutputPath;

                using (FileStream fs = File.OpenRead(fullPath))
                {
                    Type dataType = FormatUtil.getObjectType(fs);
                    
                    if (dataType != null && dataType.Name.Equals("Sdat"))
                    {
                        string filePrefix = Path.GetFileNameWithoutExtension(fullPath);
                        outputPath = Path.Combine(Path.GetDirectoryName(fullPath), filePrefix);
                        
                        Sdat sdat = new Sdat();
                        sdat.Initialize(fs, fullPath);

                        sdat.ExtractBanks(fs, outputPath);
                        sdat.ExtractSseqs(fs, outputPath);
                        sdat.ExtractSeqArc(fs, outputPath);
                        sdat.ExtractStrms(fs, outputPath);
                        waveArcOutputPath = sdat.ExtractWaveArcs(fs, outputPath);
                        
                        // extract SWAVs
                        if (!String.IsNullOrEmpty(waveArcOutputPath))
                        {
                            foreach (string f in Directory.GetFiles(waveArcOutputPath, "*" + Swar.FILE_EXTENSION))
                            {
                                using (FileStream swarFs = File.Open(f, FileMode.Open, FileAccess.Read))
                                {
                                    dataType = FormatUtil.getObjectType(swarFs);

                                    if (dataType != null && dataType.Name.Equals("Swar"))
                                    {
                                        swavOutputPath = Path.Combine(waveArcOutputPath, Path.GetFileNameWithoutExtension(f));
                                        swar.Initialize(swarFs, f);

                                        ExtractAndWriteSwavFromSwar(swarFs, swar, swavOutputPath);
                                    }
                                }
                            }

                        }

                        sdat.BuildSmap(outputPath, filePrefix);
                    }
                }                                
            }
            return outputPath;
        }

        public static string[] ExtractSdatsFromFile(string pPath, string pDirectorySuffix)
        {
            ArrayList extractedSdatPaths = new ArrayList();

            string fullPath = Path.GetFullPath(pPath);
            string outputPath;
            string filePrefix = Path.GetFileNameWithoutExtension(fullPath);
            if (!String.IsNullOrEmpty(pDirectorySuffix))
            {
                filePrefix += pDirectorySuffix;
            }

            int sdatIndex = 0;
            long sdatOffset;
            long previousOffset;

            byte[] sdatSizeBytes;
            int sdatSize;

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(String.Format("Cannot find file <{0}>", fullPath));
            }
            else
            {
                using (FileStream fs = File.Open(fullPath, FileMode.Open, FileAccess.Read))
                {
                    previousOffset = 0;

                    while ((sdatOffset = ParseFile.GetNextOffset(fs, previousOffset, Sdat.ASCII_SIGNATURE)) != -1)
                    {
                        sdatSizeBytes = ParseFile.ParseSimpleOffset(fs, sdatOffset + 8, 4);
                        sdatSize = BitConverter.ToInt32(sdatSizeBytes, 0);

                        outputPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(pPath), Path.Combine(filePrefix,
                            String.Format("sound_data_{0}.sdat", sdatIndex++.ToString("X2")))));

                        ParseFile.ExtractChunkToFile(fs, sdatOffset, sdatSize, outputPath);

                        extractedSdatPaths.Add(outputPath);

                        // only increment by one since there can be fake or bad SDATs which will move us past proper ones.
                        previousOffset = sdatOffset + 1;
                    }
                }
            }

            // return paths of extracted SDATs
            if (extractedSdatPaths.Count > 0)
            {
                return (string[])extractedSdatPaths.ToArray(typeof(string));
            }
            else
            {
                return null;
            }
        }

        public static void ExtractAndWriteSwavFromSwar(Stream pStream, Swar pSwar, string pOutputPath)
        {
            ExtractAndWriteSwavFromSwar(pStream, pSwar, pOutputPath, false);
        }
        
        public static void ExtractAndWriteSwavFromSwar(Stream pStream, Swar pSwar, string pOutputPath, bool includeOutputFolderInFileName)
        {
            string outputFileName;
            string outputFolderName = Path.GetFileName(pOutputPath);

            if (pSwar.SampleOffsets.Length > 0)
            {
                if (!Directory.Exists(pOutputPath))
                {
                    Directory.CreateDirectory(pOutputPath);
                }
            }

            for (int i = 0; i < pSwar.SampleOffsets.Length; i++)
            {
                if (pSwar.SampleOffsets[i] > 0)
                {
                    Swav.SwavInfo swavInfo = Swav.GetSwavInfo(pStream, pSwar.SampleOffsets[i]);
                    UInt32 fileSize = (swavInfo.LoopOffset + swavInfo.NonLoopLength) * 4;

                    if (includeOutputFolderInFileName)
                    {
                        outputFileName = Path.Combine(pOutputPath, String.Format("{0}_{1}", outputFolderName, i.ToString("X2")) + Swav.FILE_EXTENSION);
                    }
                    else
                    {
                        outputFileName = Path.Combine(pOutputPath, i.ToString("X2") + Swav.FILE_EXTENSION);
                    }

                    using (BinaryWriter bw = new BinaryWriter(File.Open(outputFileName, FileMode.Create, FileAccess.Write)))
                    {
                        bw.Write(Swav.ASCII_SIGNATURE);
                        bw.Write(BitConverter.GetBytes(fileSize + 0x10 + 0x08 + Swav.SWAV_INFO_SIZE));
                        bw.Write(BitConverter.GetBytes((UInt16)0x10));
                        bw.Write(BitConverter.GetBytes((UInt16)0x01));

                        bw.Write(Swav.DATA_SIGNATURE);
                        bw.Write(BitConverter.GetBytes(fileSize + 0x08 + Swav.SWAV_INFO_SIZE));
                        bw.Write(ParseFile.ParseSimpleOffset(pStream, pSwar.SampleOffsets[i], (int)Swav.SWAV_INFO_SIZE));
                        bw.Write(ParseFile.ParseSimpleOffset(pStream, pSwar.SampleOffsets[i] + Swav.SWAV_INFO_SIZE, (int)fileSize));
                    }

                }
            }
        }

        public static Smap GetSmapFromSdat(string pSdatPath)
        {
            Smap smap = new Smap();
            string fullPath = Path.GetFullPath(pSdatPath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(String.Format("Cannot find file <{0}>", fullPath));
            }
            else
            {
                using (FileStream fs = File.OpenRead(fullPath))
                {
                    Type dataType = FormatUtil.getObjectType(fs);
                    
                    if (dataType != null && dataType.Name.Equals("Sdat"))
                    {
                        Sdat sdat = new Sdat();
                        sdat.Initialize(fs, fullPath);
                        smap = new Smap(sdat);
                    }
                }
            }
            
            return smap;
        }

        public static ArrayList GetDuplicateSseqsList(string pSdatPath)
        {
            string fullPath = Path.GetFullPath(pSdatPath);
            ArrayList ret = null;

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(String.Format("Cannot find file <{0}>", fullPath));
            }
            else
            {
                if (Sdat.IsSdat(fullPath))
                {
                    using (FileStream fs = File.OpenRead(fullPath))
                    {
                        Sdat sdat = new Sdat();
                        sdat.Initialize(fs, fullPath);
                        ret = sdat.GetDuplicatesList();
                    }
                }
            }

            return ret;        
        }
    }
}
