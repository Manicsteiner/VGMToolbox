using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.format;

namespace VGMToolbox.format.util
{
    public class NsfUtil
    {
        public struct NsfM3uBuilderStruct
        {
            public string Path;
            public bool OnePlaylistPerFile;
        }

        public static void BuildM3uForFile(NsfM3uBuilderStruct pNsfM3uBuilderStruct)
        {
            using (FileStream fs = File.OpenRead(pNsfM3uBuilderStruct.Path))
            {
                Type dataType = FormatUtil.getObjectType(fs);
                System.Text.Encoding enc = System.Text.Encoding.ASCII;

                if (dataType != null && dataType.Name.Equals("Nsfe"))
                {
                    string filename = Path.GetFileName(pNsfM3uBuilderStruct.Path);
                    string trackItem = String.Empty;

                    Nsfe nsfeData = new Nsfe();
                    fs.Seek(0, SeekOrigin.Begin);
                    nsfeData.Initialize(fs, pNsfM3uBuilderStruct.Path);

                    string outputFile = Path.GetDirectoryName(pNsfM3uBuilderStruct.Path) + Path.DirectorySeparatorChar +
                        Path.GetFileNameWithoutExtension(pNsfM3uBuilderStruct.Path) + ".m3u";

                    using (StreamWriter sw = File.CreateText(outputFile))
                    {
                        string[] playlist = nsfeData.Playlist.Split(',');

                        sw.WriteLine("#######################################################");
                        sw.WriteLine("#");
                        sw.WriteLine("# Game: " + nsfeData.SongName);
                        sw.WriteLine("# Artist: " + nsfeData.SongArtist);
                        sw.WriteLine("# Copyright: " + nsfeData.SongCopyright);
                        sw.WriteLine("# Ripper: " + nsfeData.NsfRipper);
                        sw.WriteLine("#");
                        sw.WriteLine("#######################################################");
                        sw.WriteLine();

                        // Build by playlist if it exists            
                        if (!String.IsNullOrEmpty(nsfeData.Playlist))
                        {
                            int fileIndex = 1;
                            int index;
                            foreach (string s in nsfeData.Playlist.Split(','))
                            {
                                index = int.Parse(s.Trim());
                                trackItem = NsfUtil.BuildTrackItem(index, nsfeData, pNsfM3uBuilderStruct.Path);
                                sw.WriteLine(trackItem);

                                if (pNsfM3uBuilderStruct.OnePlaylistPerFile)
                                {
                                    NsfUtil.BuildSingleFileM3u(pNsfM3uBuilderStruct.Path, nsfeData, trackItem, fileIndex, index);
                                }

                                fileIndex++;
                            }
                        }
                        // Use default order if playlist does not exist
                        else
                        {
                            // !!! CHANGE TO START FROM nsfeData.StartingSong???????
                            for (int i = 0; i < nsfeData.TotalSongs[0]; i++)
                            {
                                trackItem = NsfUtil.BuildTrackItem(i, nsfeData, pNsfM3uBuilderStruct.Path);
                                sw.WriteLine(trackItem);

                                if (pNsfM3uBuilderStruct.OnePlaylistPerFile)
                                {
                                    NsfUtil.BuildSingleFileM3u(pNsfM3uBuilderStruct.Path, nsfeData, trackItem, i, i);
                                }
                            }
                        }
                    }
                    
                    NsfUtil.NsfeToNsf(nsfeData, pNsfM3uBuilderStruct.Path);
                }
            }
        }
        
        public static string BuildTrackItem(int pIndex, Nsfe pNsfeData, string pPath)
        {
            int tempTime;
            int timeMinutes = 0;
            int timeSeconds = 0;
            string timeTotal = String.Empty;
            int fadeMinutes = 0;
            int fadeSeconds = 0;
            string fadeTotal = String.Empty;
            string title;
            string entry;

            title = NsfUtil.ParseTitle(pNsfeData, pIndex);

            if (pNsfeData.Times != null)
            {
                tempTime = pNsfeData.Times[pIndex];

                if (tempTime >= 0)
                {
                    timeMinutes = tempTime / 60000;
                    timeSeconds = ((tempTime - (timeMinutes * 60000)) % 60000) / 1000;
                    timeTotal = timeMinutes + ":" + timeSeconds.ToString("d2");
                }
            }

            if (pNsfeData.Fades != null)
            {
                tempTime = pNsfeData.Fades[pIndex];

                if (tempTime >= 0)
                {
                    fadeMinutes = tempTime / 60000;
                    fadeSeconds = ((tempTime - (fadeMinutes * 60000)) % 60000) / 1000;
                    fadeTotal = fadeMinutes + ":" + fadeSeconds.ToString("d2");
                }
            }

            entry = NezPlugUtil.BuildPlaylistEntry(NezPlugUtil.FORMAT_NSF,
                Path.GetFileNameWithoutExtension(pPath) + ".nsf",
                (pIndex + 1).ToString(),
                title.Trim(),
                timeTotal,
                String.Empty,
                fadeTotal,
                String.Empty);
            return entry;
        }
        
        public static void BuildSingleFileM3u(string pPath, Nsfe pNsfeData, string pTrackData, int pIndex, int pTrackIndex)
        {
            string outputFileName = Path.GetFileNameWithoutExtension(pPath) + " - " + pIndex.ToString().PadLeft(2, '0') +
                " - " + NsfUtil.ParseTitle(pNsfeData, pTrackIndex) + ".m3u";

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                outputFileName = outputFileName.Replace(c, '_');
            }

            string outputPath = Path.GetDirectoryName(pPath);

            StreamWriter singleSW = File.CreateText(outputPath + Path.DirectorySeparatorChar + outputFileName);

            singleSW.WriteLine("#######################################################");
            singleSW.WriteLine("#");
            singleSW.WriteLine("# Game: " + pNsfeData.SongName);
            singleSW.WriteLine("# Artist: " + pNsfeData.SongArtist);
            singleSW.WriteLine("# Copyright: " + pNsfeData.SongCopyright);
            singleSW.WriteLine("# Ripper: " + pNsfeData.NsfRipper);
            singleSW.WriteLine("# Song: " + NsfUtil.ParseTitle(pNsfeData, pIndex));
            singleSW.WriteLine("#");
            singleSW.WriteLine("#######################################################");
            singleSW.WriteLine();
            singleSW.WriteLine(pTrackData);

            singleSW.Close();
            singleSW.Dispose();
        }
        
        public static string ParseTitle(Nsfe pNsfeData, int pIndex)
        {
            string title;

            if ((pNsfeData.TrackLabels.Length > pIndex) && (!String.IsNullOrEmpty(pNsfeData.TrackLabels[pIndex])))
            {
                title = pNsfeData.TrackLabels[pIndex];
            }
            else
            {
                title = "Track " + pIndex;
            }
            return title;
        }

        public static void NsfeToNsf(Nsfe pNsfeData, string pPath)
        {
            int tempLength;
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            string outputPath = Path.GetDirectoryName(pPath) + Path.DirectorySeparatorChar +
                Path.GetFileNameWithoutExtension(pPath) + ".nsf";

            byte[] songName = new byte[32];
            tempLength = pNsfeData.SongNameBytes.Length > 32 ? 32 : pNsfeData.SongNameBytes.Length;
            Array.ConstrainedCopy(pNsfeData.SongNameBytes, 0, songName, 0, tempLength);

            byte[] songArtist = new byte[32];
            tempLength = pNsfeData.SongArtistBytes.Length > 32 ? 32 : pNsfeData.SongArtistBytes.Length;
            Array.ConstrainedCopy(pNsfeData.SongArtistBytes, 0, songArtist, 0, tempLength);

            byte[] songCopyright = new byte[32];
            tempLength = pNsfeData.SongCopyrightBytes.Length > 32 ? 32 : pNsfeData.SongCopyrightBytes.Length;
            Array.ConstrainedCopy(pNsfeData.SongCopyrightBytes, 0, songCopyright, 0, tempLength);

            byte[] bankswitchInit = new byte[8];
            if (pNsfeData.BankSwitchInit != null && pNsfeData.BankSwitchInit.Length > 0)
            {
                pNsfeData.BankSwitchInit.CopyTo(bankswitchInit, 0);
            }

            byte[] expansionBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            byte[] startingSong = new byte[] { 0x01 };

            BinaryWriter bw = new BinaryWriter(File.Create(outputPath));
            bw.Write(Nsf.ASCII_SIGNATURE);
            bw.Write(Nsf.CURRENT_VERSION_NUMBER);
            bw.Write(pNsfeData.TotalSongs);
            bw.Write(startingSong);
            bw.Write(pNsfeData.LoadAddress);
            bw.Write(pNsfeData.InitAddress);
            bw.Write(pNsfeData.PlayAddress);
            bw.Write(songName);
            bw.Write(songArtist);
            bw.Write(songCopyright);

            if (((pNsfeData.PalNtscBits[0] & Nsf.MASK_NTSC) == Nsf.MASK_NTSC) ||
               ((pNsfeData.PalNtscBits[0] & Nsf.MASK_PAL_NTSC) == Nsf.MASK_PAL_NTSC))
            {
                bw.Write(new byte[] { 0x1a, 0x41 });
            }
            else
            {
                bw.Write(new byte[] { 0x00, 0x00 });
            }

            bw.Write(bankswitchInit);

            if (((pNsfeData.PalNtscBits[0] & Nsf.MASK_PAL) == Nsf.MASK_PAL) ||
               ((pNsfeData.PalNtscBits[0] & Nsf.MASK_PAL_NTSC) == Nsf.MASK_PAL_NTSC))
            {
                bw.Write(new byte[] { 0x20, 0x4e });
            }
            else
            {
                bw.Write(new byte[] { 0x00, 0x00 });
            }

            bw.Write(pNsfeData.PalNtscBits);
            bw.Write(pNsfeData.ExtraChipsBits);
            bw.Write(expansionBytes);
            bw.Write(pNsfeData.Data);

            bw.Close();
        }
    }
}
