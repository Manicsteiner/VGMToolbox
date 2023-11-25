using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using VGMToolbox.util;

namespace VGMToolbox.format.util
{
    public struct NezPlugM3uEntry
    {
        public string filename;
        public string format;
        public int songNumber;
        public string title;
        public string time;
        public string loop;
        public string fade;
        public string loopCount;
    }

    public struct M3uBuilderStruct
    {
        public string Path {set; get;}
        public bool OnePlaylistPerFile { set; get; }
        public bool UseKnurekFormatParsing { set; get; }
    }

    public class NezPlugUtil
    {
        public const string FORMAT_NSF = "NSF";
        public const string FORMAT_GBS = "GBS";
        
        public const string COMMENT_MARKER = "#";
        public const string M3U_FILE_EXTENSION = ".m3u";
        public const int EMPTY_COUNT = -1;

        // convert to use struct
        public static string BuildPlaylistEntry(string pFileType, string pFileName, string pSongNumber,
            string pTitle, string pTime, string pLoop, string pFade, string pLoopCount)
        {
            StringBuilder sb = new StringBuilder();
            string playlistEntry = String.Empty;

            sb.Append(pFileName + "::");
            sb.Append(pFileType + ",");
            sb.Append(pSongNumber + ",");
            sb.Append("" + pTitle.Replace(",", "\\,") + ",");
            sb.Append(pTime + ",");
            sb.Append(pLoop + ",");
            sb.Append(pFade + ",");
            sb.Append(pLoopCount);

            return sb.ToString();
        }    

        public static NezPlugM3uEntry GetNezPlugM3uEntryFromString(string pString)
        {
            NezPlugM3uEntry m3uEntry = new NezPlugM3uEntry();

            string escapedCommaString = "\\,";
            string replaceCommaString = "|*|";
            string plainCommaString = ",";

            string[] stringDelimtier = new string[] { "::" };
            string[] splitLine = pString.Split(stringDelimtier, StringSplitOptions.None);

            if (splitLine.Length > 1)
            {
                char[] charDelimtier = new char[] { ',' };

                splitLine[1] = splitLine[1].Replace(escapedCommaString, replaceCommaString);
                string[] splitData = splitLine[1].Split(charDelimtier, StringSplitOptions.None);

                if (splitLine.Length > 0)
                {
                    m3uEntry.filename = splitLine[0].Replace(replaceCommaString, plainCommaString);
                }

                if (splitData.Length > 0)
                {
                    m3uEntry.format = splitData[0].Replace(replaceCommaString, plainCommaString);
                }

                if (splitData.Length > 1)
                {
                    m3uEntry.songNumber = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(splitData[1].Replace(replaceCommaString, plainCommaString).Replace("$", "0x"));
                }
                else
                {
                    m3uEntry.songNumber = EMPTY_COUNT;
                }

                if (splitData.Length > 2)
                {
                    m3uEntry.title = splitData[2].Replace(replaceCommaString, plainCommaString);
                }

                if (splitData.Length > 3)
                {
                    m3uEntry.time = splitData[3].Replace(replaceCommaString, plainCommaString);
                }

                if (splitData.Length > 4)
                {
                    m3uEntry.loop = splitData[4].Replace(replaceCommaString, plainCommaString);
                }

                if (splitData.Length > 5)
                {
                    m3uEntry.fade = splitData[5].Replace(replaceCommaString, plainCommaString);
                }

                if (splitData.Length > 6)
                {
                    m3uEntry.loopCount = splitData[6].Replace(replaceCommaString, plainCommaString);
                }
            }

            return m3uEntry;
        }

        public static NezPlugM3uEntry[] GetNezPlugM3uEntriesFromFile(string pPath)
        {
            NezPlugM3uEntry[] m3uEntries = null;
            ArrayList m3uList = new ArrayList();

            if (File.Exists(pPath))
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(pPath)))
                {
                    string currentLine;

                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (!currentLine.StartsWith(COMMENT_MARKER) &&
                            (!String.IsNullOrEmpty(currentLine)))
                        {
                            NezPlugM3uEntry m3uEntry = new NezPlugM3uEntry();
                            m3uEntry = GetNezPlugM3uEntryFromString(currentLine);

                            if (!String.IsNullOrEmpty(m3uEntry.filename))
                            {
                                m3uList.Add(m3uEntry);
                            }
                        }
                    }
                    m3uEntries = (NezPlugM3uEntry[])m3uList.ToArray(typeof(NezPlugM3uEntry));
                }
            }
            else
            {
                throw new IOException(String.Format("File not found: <{0}>{1}", pPath, Environment.NewLine));
            }

            return m3uEntries;
        }

        public static void BuildPlaylistForFile(M3uBuilderStruct pM3uBuilderStruct)
        {
            using (FileStream fs = File.OpenRead(pM3uBuilderStruct.Path))
            {
                Type dataType = FormatUtil.getObjectType(fs);
                System.Text.Encoding enc = System.Text.Encoding.ASCII;
               
                if (dataType != null &&
                    (typeof(IEmbeddedTagsFormat).IsAssignableFrom(dataType)))
                {
                    string filename = Path.GetFileName(pM3uBuilderStruct.Path);
                    string trackItem = String.Empty;

                    INezPlugPlaylistFormat vgmData = 
                        (INezPlugPlaylistFormat)Activator.CreateInstance(dataType);
                    fs.Seek(0, SeekOrigin.Begin);
                    vgmData.Initialize(fs, pM3uBuilderStruct.Path);

                    string outputFile = Path.GetDirectoryName(pM3uBuilderStruct.Path) + Path.DirectorySeparatorChar +
                        Path.GetFileNameWithoutExtension(pM3uBuilderStruct.Path) + ".m3u";

                    using (StreamWriter sw = File.CreateText(outputFile))
                    {
                        sw.WriteLine("#######################################################");
                        sw.WriteLine("#");
                        sw.WriteLine("# Game: " + enc.GetString(FileUtil.ReplaceNullByteWithSpace(vgmData.SongName)).Trim());
                        sw.WriteLine("# Artist: " + enc.GetString(FileUtil.ReplaceNullByteWithSpace(vgmData.SongArtist)).Trim());
                        sw.WriteLine("# Copyright: " + enc.GetString(FileUtil.ReplaceNullByteWithSpace(vgmData.SongCopyright)).Trim());
                        sw.WriteLine("#");
                        sw.WriteLine("#######################################################");
                        sw.WriteLine();

                        for (int i = vgmData.StartingSong[0] - 1; i < vgmData.TotalSongs[0]; i++)
                        {
                            if (pM3uBuilderStruct.UseKnurekFormatParsing)
                            {
                                trackItem = BuildKnurekFormatPlaylistTrackItem(i, vgmData, pM3uBuilderStruct.Path);
                            }
                            else
                            {
                                trackItem = BuildPlaylistTrackItem(i, vgmData, pM3uBuilderStruct.Path);
                            }
                            
                            sw.WriteLine(trackItem);

                            if (pM3uBuilderStruct.OnePlaylistPerFile)
                            {
                                BuildSingleFilePlaylist(pM3uBuilderStruct.Path, vgmData, trackItem, i);
                            }
                            else if (pM3uBuilderStruct.UseKnurekFormatParsing)
                            {
                                BuildSingleFileKnurekFormatPlaylist(pM3uBuilderStruct.Path, vgmData, trackItem, i);
                            }
                        }
                    } // using (StreamWriter sw...)
                
                }
            }
        }

        public static string BuildPlaylistTrackItem(int index, INezPlugPlaylistFormat vgmData, string pPath)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            string title = enc.GetString(FileUtil.ReplaceNullByteWithSpace(vgmData.SongArtist)).Trim() + " - " +
                    enc.GetString(FileUtil.ReplaceNullByteWithSpace(vgmData.SongName)).Trim() + " - " +
                    "Track " + index.ToString().PadLeft(2, '0'); ;

            string entry = NezPlugUtil.BuildPlaylistEntry(vgmData.GetNezPlugPlaylistFormat(),
                Path.GetFileName(pPath),
                (index).ToString(),
                title,
                String.Empty,
                String.Empty,
                String.Empty,
                String.Empty);
            return entry;
        }

        public static void BuildSingleFilePlaylist(string pPath, INezPlugPlaylistFormat vgmData, string pTrackData, int pIndex)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            string outputFileName = Path.GetFileNameWithoutExtension(pPath) + " - " + pIndex.ToString().PadLeft(2, '0') +
                " - " + CreatePlaylistTitle(pIndex) + ".m3u";

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                outputFileName = outputFileName.Replace(c, '_');
            }

            string outputPath = Path.GetDirectoryName(pPath);

            StreamWriter singleSW = File.CreateText(outputPath + Path.DirectorySeparatorChar + outputFileName);

            singleSW.WriteLine("#######################################################");
            singleSW.WriteLine("#");
            singleSW.WriteLine("# Game: " + enc.GetString(FileUtil.ReplaceNullByteWithSpace(vgmData.SongName)).Trim());
            singleSW.WriteLine("# Artist: " + enc.GetString(FileUtil.ReplaceNullByteWithSpace(vgmData.SongArtist)).Trim());
            singleSW.WriteLine("# Copyright: " + enc.GetString(FileUtil.ReplaceNullByteWithSpace(vgmData.SongCopyright)).Trim());
            singleSW.WriteLine("#");
            singleSW.WriteLine("#######################################################");
            singleSW.WriteLine();
            singleSW.WriteLine(pTrackData);

            singleSW.Close();
            singleSW.Dispose();
        }

        public static string BuildKnurekFormatPlaylistTrackItem(int index, INezPlugPlaylistFormat vgmData, string pPath)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            short dummy;
            int dateIndex = -1;

            string fileName = Path.GetFileNameWithoutExtension(vgmData.FilePath);
            string[] splitFileName = fileName.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries);
            
            // track title
            string trackTitle = "Track " + (index + 1).ToString().PadLeft(2, '0');
            
            // composer
            string composer = enc.GetString(FileUtil.ReplaceNullByteWithSpace(vgmData.SongArtist)).Trim();
            composer = composer.Replace("<?>", "Unknown");

            // game name
            string gameName = splitFileName[0].Trim();

            // year
            string year = String.Empty;

            // check for system identifiers (GBC), (FDS), etc...
            if ((splitFileName.Length > 1) && Int16.TryParse(splitFileName[1].Substring(0, 2), out dummy))
            {
                dateIndex = 1;
            }
            else if ((splitFileName.Length > 2) && Int16.TryParse(splitFileName[2].Substring(0, 2), out dummy))
            {
                dateIndex = 2;            
            }

            year = splitFileName[dateIndex].Replace(")", String.Empty).Substring(0, 4);

            // developer
            string developer;
            developer = splitFileName[(dateIndex + 1)].Replace(")", String.Empty).Trim();

            // publisher
            string publisher = String.Empty;

            if (splitFileName.GetUpperBound(0) > (dateIndex + 1))
            {
                publisher = splitFileName[splitFileName.GetUpperBound(0)].Replace(")", String.Empty).Trim();
            }

            string title = trackTitle + " - " + composer + " - " + gameName + " - (c)" + year + " " + developer;
            title = String.IsNullOrEmpty(publisher) ? title : (title + ", " + publisher);
            title = title.Trim();

            string entry = NezPlugUtil.BuildPlaylistEntry(vgmData.GetNezPlugPlaylistFormat(),
                Path.GetFileName(pPath),
                (index).ToString(),
                title,
                String.Empty,
                String.Empty,
                String.Empty,
                String.Empty);
            return entry;
        }
        
        public static void BuildSingleFileKnurekFormatPlaylist(string pPath, INezPlugPlaylistFormat vgmData, string pTrackData, int pIndex)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            string outputFileName = (pIndex + 1).ToString().PadLeft(2, '0') + " " + CreatePlaylistTitle(pIndex + 1) + ".m3u";

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                outputFileName = outputFileName.Replace(c, '_');
            }

            string outputPath = Path.GetDirectoryName(pPath);
            outputPath = Path.Combine(outputPath, outputFileName);

            StreamWriter singleSW = File.CreateText(outputPath);
            singleSW.WriteLine(pTrackData);

            singleSW.Close();
            singleSW.Dispose();
        }

        public static string CreatePlaylistTitle(int index)
        {
            return "Track " + index.ToString().PadLeft(2, '0');
        }
    }
}
