using System;
using System.Collections;
using System.ComponentModel;
using System.IO;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format;
using VGMToolbox.format.auditing;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.auditing
{
    class RebuilderWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        private const string CACHE_DB_FILENAME = "_cache.db";
        private ArrayList libFilesForDeletion = new ArrayList();
        private ArrayList tempDirsForDeletion = new ArrayList();
        private int fileCount = 0;
        private int maxFiles = 0;
        private VGMToolbox.util.ProgressStruct progressStruct;

        public struct RebuildSetsStruct
        { 
            public string pSourceDir; 
            public string pDestinationDir; 
            public datafile pDatFile;
            public bool pRemoveSource; 
            public bool pOverwriteExisting;
            public bool ScanOnly;
            public bool pCompressOutput;
            public int totalFiles;
        }

        public RebuilderWorker()
        {
            progressStruct = new VGMToolbox.util.ProgressStruct();
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        private string buildFilePath(string pSetName, string pFilePath)
        {
            return (pSetName + Path.DirectorySeparatorChar + pFilePath);
        }

        private void deleteQueuedLibFiles()
        {
            foreach (string f in libFilesForDeletion.ToArray(typeof(string)))
            {
                File.Delete(f);
                libFilesForDeletion.Remove(f);
            }
        }

        private void deleteQueuedTempDirs()
        {
            foreach (string d in tempDirsForDeletion.ToArray(typeof(string)))
            {
                Directory.Delete(d, true);
                tempDirsForDeletion.Remove(d);
            }
        }

        private bool moveFile(string pDestination, ArrayList pDestinationFiles, FileStream pSourceFile,
            string pSourceName, bool pOverwriteExisting, bool pCompressOutput,
            bool hasMultipleExtensions)
        {
            int readSize = 0;
            byte[] data = new byte[4096];
            string pZipFilePath = String.Empty;

            string filePath;
            string path;
            string destinationPath = String.Empty;
            string searchPattern;

            bool isSuccess = false;

            try
            {
                foreach (AuditingUtil.ChecksumStruct cs in pDestinationFiles)
                {
                    filePath = buildFilePath(cs.game, Path.ChangeExtension(cs.rom, Path.GetExtension(pSourceFile.Name)));
                    path = pDestination + filePath.Substring(0, filePath.LastIndexOf(Path.DirectorySeparatorChar));

                    if (hasMultipleExtensions)
                    {
                        searchPattern = Path.GetFileNameWithoutExtension(cs.rom) + ".*";
                    }
                    else
                    {
                        searchPattern = Path.GetFileName(cs.rom);
                    }
                    
                    if (pCompressOutput)
                    {
                        pZipFilePath = pDestination + cs.game + ".zip";                        
                        destinationPath = Path.ChangeExtension(cs.rom, Path.GetExtension(pSourceFile.Name));
                        CompressionUtil.AddFileToZipFile(pZipFilePath, pSourceName, destinationPath);
                    }
                    else
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (pOverwriteExisting || Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).Length == 0)
                        {
                            File.Delete(pDestination + filePath);
                            FileStream streamWriter = File.Create(pDestination + filePath);
                            pSourceFile.Seek(0, SeekOrigin.Begin);

                            while (true)
                            {
                                readSize = pSourceFile.Read(data, 0, data.Length);
                                if (readSize > 0)
                                {
                                    streamWriter.Write(data, 0, readSize);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            streamWriter.Close();
                            streamWriter.Dispose();
                        }
                    }
                } // foreach (AuditingUtil.ChecksumStruct cs in pDestinationFiles)


                isSuccess = true;
            }
            catch (Exception exception)
            {
                this.progressStruct.Clear();
                this.progressStruct.FileName = pSourceName;
                this.progressStruct.ErrorMessage = "Error rebuilding <" + pSourceName + "> to destination: " + exception.Message + Environment.NewLine;
                ReportProgress(Constants.IgnoreProgress, this.progressStruct);
                isSuccess = false;
            }

            return isSuccess;
        }

        private void rebuildFile(string pFilePath, RebuildSetsStruct pRebuildSetsStruct, AuditingUtil pAuditingUtil, 
            DoWorkEventArgs ea)
        {
            ArrayList pDestinationFiles = new ArrayList();
            bool isFileLibrary = false;
            bool hasMultipleExtensions = false;
            bool fileMovedOrScannedOk = true;
            Type formatType = null;

            if (FormatUtil.IsZipFile(pFilePath))
            {
                string tempDir = System.IO.Path.GetTempPath() + Path.GetFileNameWithoutExtension(pFilePath);
                CompressionUtil.ExtractAllFilesFromZipFile(pFilePath, tempDir);

                tempDirsForDeletion.Add(tempDir);           // Add in case of cancel button
                
                RebuildSetsStruct zipRebuildSetsStruct = pRebuildSetsStruct;
                zipRebuildSetsStruct.pSourceDir = tempDir;
                zipRebuildSetsStruct.totalFiles = Directory.GetFiles(tempDir, "*.*", SearchOption.AllDirectories).Length;

                maxFiles += zipRebuildSetsStruct.totalFiles;
                this.rebuildSets(zipRebuildSetsStruct, pAuditingUtil, 0, ea);

                if (!CancellationPending)
                {
                    Directory.Delete(tempDir, true);
                    tempDirsForDeletion.Remove(tempDir);
                }
            }
            else
            {
                using (FileStream fs = File.OpenRead(pFilePath))
                {
                    formatType = FormatUtil.getObjectType(fs);
                    fs.Seek(0, SeekOrigin.Begin);

                    // CRC32
                    string crc32Value = String.Empty;
                    Crc32 crc32Generator = new Crc32();

                    // @TODO - Change to file streams?  Out of memory errors on repeat runs

                    /*
                    // MD5
                    string md5FileName = Path.GetTempFileName();
                    MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();
                    //MemoryStream md5MemoryStream = new MemoryStream();
                    FileStream md5MemoryStream = new FileStream(md5FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    CryptoStream md5CryptoStream = new CryptoStream(md5MemoryStream, md5Hash, CryptoStreamMode.Write);

                    // SHA1
                    string sha1FileName = Path.GetTempFileName();
                    SHA1CryptoServiceProvider sha1Hash = new SHA1CryptoServiceProvider();
                    //MemoryStream sha1MemoryStream = new MemoryStream();
                    FileStream sha1MemoryStream = new FileStream(sha1FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    CryptoStream sha1CryptoStream = new CryptoStream(sha1MemoryStream, sha1Hash, CryptoStreamMode.Write);
                    */


                    if ((pAuditingUtil.HasVgmtSizeFlag) && (formatType != null))
                    {
                        try
                        {
                            IFormat vgmData = (IFormat)Activator.CreateInstance(formatType);
                            vgmData.Initialize(fs, pFilePath);

                            isFileLibrary = vgmData.IsFileLibrary();
                            hasMultipleExtensions = vgmData.HasMultipleFileExtensions();
                            // vgmData.getDatFileCrc32(pFilePath, ref libHash, ref crc32Generator,
                            //    ref md5CryptoStream, ref sha1CryptoStream, false, pStreamInput);                    
                            vgmData.GetDatFileCrc32(ref crc32Generator);
                            vgmData = null;
                        }
                        catch (EndOfStreamException e)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.FileName = pFilePath;
                            this.progressStruct.ErrorMessage = String.Format("Error processing <{0}> as type [{1}], falling back to full file cheksum.  Error received: {2}", pFilePath, formatType.Name, e.Message) + Environment.NewLine + Environment.NewLine;
                            ReportProgress(Constants.IgnoreProgress, this.progressStruct);

                            crc32Generator.Reset();
                            // ParseFile.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator,
                            //    ref md5CryptoStream, ref sha1CryptoStream);
                            ChecksumUtil.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator);
                        }
                        catch (System.OutOfMemoryException e)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.FileName = pFilePath;
                            this.progressStruct.ErrorMessage = String.Format("Error processing <{0}> as type [{1}], falling back to full file cheksum.  Error received: {2}", pFilePath, formatType.Name, e.Message) + Environment.NewLine + Environment.NewLine;
                            ReportProgress(Constants.IgnoreProgress, this.progressStruct);

                            crc32Generator.Reset();
                            // ParseFile.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator,
                            //    ref md5CryptoStream, ref sha1CryptoStream);
                            ChecksumUtil.AddChunkToChecksum(fs, 0, (int)fs.Length, ref crc32Generator);
                        }
                        catch (IOException e)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.FileName = pFilePath;
                            this.progressStruct.ErrorMessage = String.Format("Error processing <{0}> as type [{1}], falling back to full file cheksum.  Error received: {2}", pFilePath, formatType.Name, e.Message) + Environment.NewLine + Environment.NewLine;
                            ReportProgress(Constants.IgnoreProgress, this.progressStruct);

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
                    }

                    // @TODO Add MD5/SHA1 to make checksum hash correct String(CRC32 + MD5 + SHA1)
                    crc32Value = crc32Generator.Value.ToString("X8");

                    pDestinationFiles = (ArrayList)pAuditingUtil.ChecksumHash[crc32Value];
                    if (pDestinationFiles != null)
                    {
                        if (!pRebuildSetsStruct.ScanOnly)
                        {
                            fileMovedOrScannedOk = this.moveFile(pRebuildSetsStruct.pDestinationDir, pDestinationFiles, fs, pFilePath, pRebuildSetsStruct.pOverwriteExisting,
                                pRebuildSetsStruct.pCompressOutput, hasMultipleExtensions);
                        }
                        pAuditingUtil.AddChecksumToCache(crc32Value);
                    }
                    else
                    {
                        pAuditingUtil.AddUnknownFile(pFilePath);
                    }
                }

                /*
                md5CryptoStream.Close();
                md5CryptoStream.Dispose();
                sha1CryptoStream.Close();
                sha1CryptoStream.Dispose();
            
                md5MemoryStream.Close();
                md5MemoryStream.Dispose();
                sha1MemoryStream.Close();
                sha1MemoryStream.Dispose();
           
                File.Delete(md5FileName);
                File.Delete(sha1FileName);
                */

                // Remove Source only if copied
                if (fileMovedOrScannedOk && pDestinationFiles != null && 
                    pRebuildSetsStruct.pRemoveSource && File.Exists(pFilePath))
                {
                    if (!isFileLibrary)
                    {
                        File.Delete(pFilePath);
                    }
                    else // Add to List for deletion later
                    {
                        libFilesForDeletion.Add(pFilePath);
                    }
                }
            } // (FormatUtil.IsZipFile(pFilePath))
        }

        private void rebuildSets(RebuildSetsStruct pRebuildSetsStruct, AuditingUtil pAuditingUtil,
            uint pDepth, DoWorkEventArgs e)
        {
            int progress;
            RebuildSetsStruct subdirRebuildSetsStruct;
            
            try
            {
                if (pDepth++ == 0)
                {
                    // Set Max sets
                    //maxFiles += pRebuildSetsStruct.totalFiles;

                    // process top level files
                    libFilesForDeletion.Clear();
                    foreach (string f in Directory.GetFiles(pRebuildSetsStruct.pSourceDir))
                    {
                        if (!CancellationPending)
                        {
                            this.rebuildFile(f, pRebuildSetsStruct, pAuditingUtil, e);
                            
                            progress = (++fileCount * 100) / maxFiles;
                            this.progressStruct.Clear();
                            this.progressStruct.FileName = f;
                            ReportProgress(progress, this.progressStruct);
                        }
                        else
                        {
                            e.Cancel = true;
                            doCancelCleanup(pAuditingUtil, pRebuildSetsStruct.pDestinationDir);
                            return;
                        }
                    } // foreach (string f in Directory.GetFiles(pRebuildSetsStruct.pSourceDir))
                    if (!CancellationPending)
                    {
                        this.deleteQueuedLibFiles();
                    }
                }

                // process subdirs                                
                if (!CancellationPending)
                {
                    foreach (string d in Directory.GetDirectories(pRebuildSetsStruct.pSourceDir))
                    {
                        libFilesForDeletion.Clear();

                        foreach (string f in Directory.GetFiles(d))
                        {
                            if (!CancellationPending)
                            {
                                try
                                {
                                    this.rebuildFile(f, pRebuildSetsStruct, pAuditingUtil, e);                                    
                                    
                                    progress = (++fileCount * 100) / maxFiles;
                                    this.progressStruct.Clear();
                                    this.progressStruct.FileName = f;
                                    ReportProgress(progress, this.progressStruct);
                                }
                                catch (Exception ex)
                                {
                                    this.progressStruct.Clear();
                                    this.progressStruct.FileName = f;
                                    this.progressStruct.ErrorMessage = "[" + f + "] " + ex.Message;
                                    ReportProgress(Constants.IgnoreProgress, this.progressStruct);
                                }
                            }
                            else
                            {
                                e.Cancel = true;
                                doCancelCleanup(pAuditingUtil, pRebuildSetsStruct.pDestinationDir);
                                return;
                            }
                        }

                        if (!CancellationPending)
                        {

                            // RebuildSetsStruct subdirRebuildSetsStruct = pRebuildSetsStruct;
                            subdirRebuildSetsStruct = pRebuildSetsStruct;                            
                            subdirRebuildSetsStruct.pSourceDir = d;
                            this.rebuildSets(subdirRebuildSetsStruct, pAuditingUtil, pDepth, e);

                            // Remove Queued Lib Files
                            this.deleteQueuedLibFiles();

                            // remove empty directory
                            if (pRebuildSetsStruct.pRemoveSource &&
                                Directory.GetFiles(d, "*.*", SearchOption.AllDirectories).Length == 0)
                            {
                                Directory.Delete(d);
                            }                                                
                        }
                        else 
                        {
                            e.Cancel = true;
                            doCancelCleanup(pAuditingUtil, pRebuildSetsStruct.pDestinationDir);
                            return;
                        }                        
                    } // (string d in Directory.GetDirectories(pRebuildSetsStruct.pSourceDir))
                } //if (!CancellationPending)
                else
                {
                    e.Cancel = true;
                    doCancelCleanup(pAuditingUtil, pRebuildSetsStruct.pDestinationDir);
                    return;
                }
            }
            catch (Exception exception2)
            {
                this.progressStruct.Clear();
                this.progressStruct.FileName = null;
                this.progressStruct.ErrorMessage = exception2.Message;
                ReportProgress(Constants.IgnoreProgress, this.progressStruct);
            }
        }

        public void rebuildSets(RebuildSetsStruct pRebuildSetsStruct, DoWorkEventArgs e)
        {
            // Set Max sets
            maxFiles += pRebuildSetsStruct.totalFiles;
            
            // Setup output directory
            pRebuildSetsStruct.pDestinationDir += Path.DirectorySeparatorChar;

            // Setup AuditingUtil
            AuditingUtil auditingUtil = new AuditingUtil(pRebuildSetsStruct.pDatFile);

            // Load/Create cache db
            try
            {
                string cacheDbPath = pRebuildSetsStruct.pDestinationDir + CACHE_DB_FILENAME;
                using (FileStream cacheFileStream = new FileStream(cacheDbPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    auditingUtil.ReadChecksumHashFromFile(cacheFileStream);
                }

                // Rebuild
                this.rebuildSets(pRebuildSetsStruct, auditingUtil, 0, e);

                // Finish rebuilding
                using (FileStream cacheFileStream = new FileStream(cacheDbPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    auditingUtil.WriteChecksumHashToFile(cacheFileStream);
                    auditingUtil.WriteHaveMissLists(pRebuildSetsStruct.pDestinationDir);
                }
            }
            catch (Exception ex)
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = String.Format("Error while rebuilding sets: {0}{1}", ex.Message, Environment.NewLine);
                ReportProgress(Constants.IgnoreProgress, this.progressStruct);
            }
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            RebuildSetsStruct vRebuildSetsStruct = (RebuildSetsStruct)e.Argument;
            this.rebuildSets(vRebuildSetsStruct, e);
        }

        private void doCancelCleanup(AuditingUtil pAuditingUtil, string pDestinationDir)
        {
            this.deleteQueuedTempDirs();            
        }
    }
}
