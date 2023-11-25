using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using VGMToolbox.format;
using VGMToolbox.format.auditing;
using VGMToolbox.format.util;
using VGMToolbox.util;

namespace VGMToolbox.auditing
{
    public struct BuildRomStruct
    {
        public string TopLevelSetFolder;
        public string FilePath;
        public bool AddMd5;
        public bool AddSha1;
        public bool UseNormalChecksums;
    }
    
    class AuditingUtil
    {
        public const string ROM_SPACER = "  ";         // Simple spacer for formatting output lists
        public const string EMPTY_ROM_SIZE = "-1";     // VGMT checksums have -1 for size

        private Hashtable haveList = new Hashtable();   // Stores list of present files
        private Hashtable missList = new Hashtable();   // Stores list of missing files
        private datafile datafile;                      // Datafile used for building lists
        private ArrayList checksumCache = new ArrayList();
        private Hashtable checksumHash = new Hashtable();
        private ArrayList unknownFiles = new ArrayList();

        public bool HasVgmtSizeFlag { set; get; } // check for size = -1, set flag that this is a VGMT checksum DAT
        public Hashtable ChecksumHash { get { return checksumHash; } }

        #region CONSTRUCTORS
        
        /// <summary>
        /// Simple constructor taking input pDatafile
        /// </summary>
        /// <param name="pDatafile">Datafile to use for building lists.</param>
        public AuditingUtil(datafile pDatafile)
        {
            datafile = pDatafile;
            checksumHash = this.BuildChecksumHash();
            HasVgmtSizeFlag = false;            
            // this.addCachedChecksumsToHash();
        }
        
        # endregion

        #region STRUCTS

        /// <summary>
        /// Simple struct used in rebuilding.
        /// </summary>
        public struct ChecksumStruct
        {
            public string game;
            public string rom;
            public bool isFilePresent;
            public bool hasMultipleExtensions;
        }

        #endregion

        #region DATFILE TEXT OUTPUT FUNCTIONS

        /// <summary>
        /// Function to build a text list of the files in the datafile.
        /// </summary>
        /// <returns>String list of all games and roms in the datafile.</returns>
        public string BuildDatafileList()
        {
            StringBuilder ret = new StringBuilder();

            foreach (game g in datafile.game)
            {
                ret.Append(buildGameList(g));
            }

            return ret.ToString();
        }

        private string BuildDatafileList(datafile pDatafile)
        {
            StringBuilder ret = new StringBuilder();

            foreach (game g in pDatafile.game)
            {
                ret.Append(buildGameList(g));
            }

            return ret.ToString();
        }

        /// <summary>
        /// Function to build a text list of the files in the incoming game.
        /// </summary>
        /// <param name="pGame">Game to parse and build text file of.</param>
        /// <returns>String list of the game name and all roms in the game.</returns>
        private string buildGameList(game pGame)
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(System.Environment.NewLine + pGame.name + System.Environment.NewLine);

            foreach (rom r in pGame.rom)
            {
                ret.Append(buildRomOutput(r));
            }

            return ret.ToString();
        }

        /// <summary>
        /// Function to build a text string of the incoming rom.
        /// </summary>
        /// <param name="pRom">Rom to create string of.</param>
        /// <returns>String of the incoming rom.</returns>
        private string buildRomOutput(rom pRom)
        {
            return ROM_SPACER + pRom.name + System.Environment.NewLine;
        }

        # endregion

        #region HAVE/MISS LIST METHODS

        /// <summary>
        /// Function to parse the datafile object and create a hashtable using the
        /// checksums as a key and the game/rom paths as the item.
        /// </summary>
        /// <returns>Hashtable with checksum keys for each game/rom pair.</returns>
        public Hashtable BuildChecksumHash()
        {
            Hashtable checksumHash = new Hashtable();
            ChecksumStruct checksumItem;            
            ArrayList list;
            string checksumKey = String.Empty;

            foreach (game set in datafile.game)
            {
                if (set.rom != null && set.rom.Length > 0)
                {
                    foreach (rom file in set.rom)
                    {
                        // @TODO Add MD5/SHA1 to make checksum hash correct String(CRC32 + MD5 + SHA1)
                        checksumKey = file.crc;

                        if (this.HasVgmtSizeFlag && file.size.Equals(EMPTY_ROM_SIZE))
                        {
                            this.HasVgmtSizeFlag = true;
                        }

                        if (!checksumHash.ContainsKey(checksumKey))
                        {
                            list = new ArrayList();
                            checksumItem = new ChecksumStruct();
                            checksumItem.game = set.name;
                            checksumItem.rom = file.name;
                            checksumItem.isFilePresent = false;
                            checksumItem.hasMultipleExtensions = false;
                            list.Add(checksumItem);
                        }
                        else
                        {
                            list = (ArrayList)checksumHash[checksumKey];
                            checksumItem = new ChecksumStruct();
                            checksumItem.game = set.name;
                            checksumItem.rom = file.name;
                            checksumItem.isFilePresent = false;
                            checksumItem.hasMultipleExtensions = false;
                            
                            list.Add(checksumItem);
                            checksumHash.Remove(checksumKey);
                        }
                        
                        checksumHash.Add(checksumKey, list);
                    }
                }
            }
            return checksumHash;
        }
        
        /// <summary>
        /// Function to write out the have and miss lists to a file.
        /// </summary>
        /// <param name="pPath">Directory to write the Have and Miss lists to.</param>
        public void WriteHaveMissLists(string pPath)
        {                        
            string havePath = pPath + "_HAVE.TXT";
            string missPath = pPath + "_MISS.TXT";
            string unknownPath = pPath + "_UNKNOWNS.TXT";
            string fixDatPath = pPath + "Fix_Datafile.xml";

            string haveListText = String.Empty;
            string missListText = String.Empty;

            datafile haveList = new datafile();
            datafile missList = new datafile();

            game newHaveGame = new game();
            game newMissGame = new game();

            ArrayList haveGameList = new ArrayList();
            ArrayList missGameList = new ArrayList();
            
            foreach (game g in datafile.game)
            {                
                ArrayList haveRomList = new ArrayList();
                ArrayList missRomList = new ArrayList();
                
                foreach (rom r in g.rom)
                {
                    if (checksumCache.Contains(r.crc))
                    {
                        haveRomList.Add(r);
                    }
                    else 
                    {
                        missRomList.Add(r);
                    }
                }

                if (haveRomList.Count > 0)
                {
                    newHaveGame = g.DeepCopy();
                    newHaveGame.rom = (rom[]) haveRomList.ToArray(typeof(rom));
                    haveGameList.Add(newHaveGame);
                }

                if (missRomList.Count > 0)
                {
                    newMissGame = g.DeepCopy();
                    newMissGame.rom = (rom[])missRomList.ToArray(typeof(rom));
                    missGameList.Add(newMissGame);
                }
            }

            haveList.game = (game[])haveGameList.ToArray(typeof(game));
            missList.game = (game[])missGameList.ToArray(typeof(game));

            haveListText = BuildDatafileList(haveList);
            missListText = BuildDatafileList(missList);

            haveListText = String.Format("You have {0} of {1} {2} sets,", haveGameList.Count,
                datafile.game.Length, datafile.header.description) + Environment.NewLine +
                Environment.NewLine + haveListText;
            missListText = String.Format("You are missing {0} of {1} {2} sets,", missGameList.Count,
                datafile.game.Length, datafile.header.description) + Environment.NewLine +
                Environment.NewLine + missListText;
            
            // Have List
            try
            {
                if (File.Exists(havePath))
                {
                    File.Delete(havePath);
                }

                using (StreamWriter sw = File.CreateText(havePath))
                {
                    sw.Write(haveListText);
                    sw.Close();
                }
            }
            catch(Exception ex)
            {
                throw new IOException("Error creating 'HAVE' list:" + ex.Message);
            }
            
            // Miss List
            try
            {
                if (File.Exists(missPath))
                {
                    File.Delete(missPath);
                }

                using (StreamWriter sw = File.CreateText(missPath))
                {
                    sw.Write(missListText);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Error creating 'MISS' list:" + ex.Message);
            }

            // Fix Datafile
            try
            {
                if (File.Exists(fixDatPath))
                {
                    File.Delete(fixDatPath);
                }

                if (missList.game.Length > 0)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(datafile));
                    TextWriter textWriter = new StreamWriter(fixDatPath);
                    serializer.Serialize(textWriter, missList);
                    textWriter.Close();
                    textWriter.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Error creating 'FixDAT' file:" + ex.Message);
            }

            // Unknown Files
            try
            {
                if (File.Exists(unknownPath))
                {
                    File.Delete(unknownPath);
                }

                if (this.unknownFiles.Count > 0)
                {
                    using (StreamWriter sw = File.CreateText(unknownPath))
                    {
                        sw.Write(String.Format("The following files were not found in the existing datafile for {0}", datafile.header.description)
                            + Environment.NewLine + Environment.NewLine);

                        foreach (string p in this.unknownFiles)
                        {
                            sw.Write(ROM_SPACER + p + Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Error creating 'UNKNOWNS' list:" + ex.Message);
            }
        }

        public void AddUnknownFile(string pPath)
        {
            unknownFiles.Add(pPath);
        }

        # endregion

        # region CACHE METHODS

        public void AddChecksumToCache(string pChecksum)
        {
            if (pChecksum != String.Empty && !checksumCache.Contains(pChecksum))
            {
                checksumCache.Add(pChecksum);
            }        
        }

        public void ReadChecksumHashFromFile(FileStream pFileStream)
        {
            if (pFileStream.Length > 0)
            {
                pFileStream.Seek(0, SeekOrigin.Begin);
                using (InflaterInputStream inflaterInputStream = new InflaterInputStream(pFileStream, new Inflater()))
                {
                    BinaryFormatter b = new BinaryFormatter();
                    checksumCache = new ArrayList((string[])b.Deserialize(inflaterInputStream));
                }
            }
        }

        public void WriteChecksumHashToFile(FileStream pFileStream)
        {
            pFileStream.Seek(0, SeekOrigin.Begin);
            using (DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream(pFileStream, new Deflater(Deflater.BEST_COMPRESSION)))
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(deflaterOutputStream, (string[])checksumCache.ToArray(typeof(string)));
            }
        }
        
        # endregion

        #region STATIC METHODS

        public static string ByteArrayToString(byte[] pBytes)
        {
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < pBytes.Length; i++)
            {
                sBuilder.Append(pBytes[i].ToString("X2"));
            }

            return sBuilder.ToString();
        }

        public static string GetMd5OfFullFile(FileStream pFileStream)
        {
            MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();

            pFileStream.Seek(0, SeekOrigin.Begin);
            md5Hash.ComputeHash(pFileStream);
            return ByteArrayToString(md5Hash.Hash);
        }

        public static datafile DeleteItemFromDatafile(
            datafile inputDatafile,
            game sourceGame,
            rom sourceRom)
        {
            datafile workingDatafile = inputDatafile;
            ArrayList games;
            ArrayList roms;

            if (sourceGame != null)
            {
                games = new ArrayList(workingDatafile.game);

                foreach (game g in games)
                {
                    if (g.Equals(sourceGame))
                    {
                        if (sourceRom != null)
                        {
                            roms = new ArrayList(g.rom);

                            foreach (rom r in roms)
                            {
                                if (r.Equals(sourceRom))
                                {
                                    roms.Remove(r);
                                    g.rom = (rom[])roms.ToArray(typeof(rom));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            games.Remove(g);
                            workingDatafile.game = (game[])games.ToArray(typeof(game));
                            break;
                        }                                                            
                    }
                }
            }

            return workingDatafile;
        }

        public static rom BuildRom(BuildRomStruct parameters, out string messages)
        {
            string fileDirectory = Path.GetDirectoryName(parameters.FilePath);
            string messageFormat = "Error processing <{0}> as type [{1}], falling back to full file cheksum.  Error received: {2}{3}";
            messages = String.Empty;

            Crc32 crc32Generator = new Crc32();
            Type formatType = null;
            rom romfile = new rom();

            MD5CryptoServiceProvider md5Hash = null;
            MemoryStream md5MemoryStream = null;
            CryptoStream md5CryptoStream = null;

            SHA1CryptoServiceProvider sha1Hash = null;
            MemoryStream sha1MemoryStream = null;
            CryptoStream sha1CryptoStream = null;

            using (FileStream fs = File.OpenRead(parameters.FilePath))
            {
                formatType = FormatUtil.getObjectType(fs);

                // MD5
                if (parameters.AddMd5)
                {
                    md5Hash = new MD5CryptoServiceProvider();
                    md5MemoryStream = new MemoryStream();
                    md5CryptoStream = new CryptoStream(md5MemoryStream, md5Hash, CryptoStreamMode.Write);
                }

                // SHA1
                if (parameters.AddSha1)
                {
                    sha1Hash = new SHA1CryptoServiceProvider();
                    sha1MemoryStream = new MemoryStream();
                    sha1CryptoStream = new CryptoStream(sha1MemoryStream, sha1Hash, CryptoStreamMode.Write);
                }

                fs.Seek(0, SeekOrigin.Begin); // Return to start of stream

                if (!parameters.UseNormalChecksums && (formatType != null))
                {
                    try
                    {
                        IFormat vgmData = (IFormat)Activator.CreateInstance(formatType);
                        vgmData.Initialize(fs, parameters.FilePath);

                        // vgmData.getDatFileCrc32(pFileName, ref libHash, ref crc32Generator,
                        //    ref md5CryptoStream, ref sha1CryptoStream, pUseLibHash, pStreamInput);
                        vgmData.GetDatFileCrc32(ref crc32Generator);
                        vgmData = null;
                    }
                    catch (EndOfStreamException _es)
                    {
                        messages = String.Format(messageFormat, parameters.FilePath, formatType.Name, _es.Message, Environment.NewLine);

                        crc32Generator.Reset();
                        // ParseFile.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator, 
                        //    ref md5CryptoStream, ref sha1CryptoStream);
                        ChecksumUtil.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator);
                    }
                    catch (System.OutOfMemoryException _es)
                    {
                        messages = String.Format(messageFormat, parameters.FilePath, formatType.Name, _es.Message, Environment.NewLine);

                        crc32Generator.Reset();
                        // ParseFile.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator,
                        //    ref md5CryptoStream, ref sha1CryptoStream);
                        ChecksumUtil.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator);
                    }
                    catch (IOException _es)
                    {
                        messages = String.Format(messageFormat, parameters.FilePath, formatType.Name, _es.Message, Environment.NewLine);

                        crc32Generator.Reset();
                        // ParseFile.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator,
                        //    ref md5CryptoStream, ref sha1CryptoStream);
                        ChecksumUtil.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator);
                    }
                }
                else
                {
                    // ParseFile.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator,
                    //    ref md5CryptoStream, ref sha1CryptoStream);

                    ChecksumUtil.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator);
                    romfile.size = fs.Length.ToString();
                }

                if (parameters.AddMd5)
                {
                    md5CryptoStream.FlushFinalBlock();
                    romfile.md5 = AuditingUtil.ByteArrayToString(md5Hash.Hash);

                    md5MemoryStream.Close();
                    md5MemoryStream.Dispose();
                    md5CryptoStream.Close();
                    md5CryptoStream.Dispose();
                }

                if (parameters.AddSha1)
                {
                    sha1CryptoStream.FlushFinalBlock();
                    romfile.sha1 = AuditingUtil.ByteArrayToString(sha1Hash.Hash);

                    sha1MemoryStream.Close();
                    sha1MemoryStream.Dispose();
                    sha1CryptoStream.Close();
                    sha1CryptoStream.Dispose();
                }
            } // using (FileStream fs = File.OpenRead(parameters.FilePath))

            romfile.crc = crc32Generator.Value.ToString("X8");
            romfile.name = parameters.FilePath.Substring((fileDirectory.LastIndexOf(parameters.TopLevelSetFolder) + parameters.TopLevelSetFolder.Length + 1));
            
            // Cleanup
            crc32Generator.Reset();

            return romfile;
        }

        # endregion
    }
}
