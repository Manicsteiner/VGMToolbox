using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.examine
{
    class ExamineChecksumGeneratorWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        const string VGMToolboxDuplicatesSubfolder = "__VGMT_Checksum_Duplicates";
        const string StandardDuplicatesSubfolder = "__Standard_Checksum_Duplicates";

        private Dictionary<string, string[]> duplicateCheckHashStandard;
        private Dictionary<string, string[]> duplicateCheckHashVgmt;
        
        public struct ExamineChecksumGeneratorStruct : IVgmtWorkerStruct
        {
            public bool DoVgmtChecksums {set; get; }
            public bool CheckForDuplicates { set; get; }
            public bool MoveStandardDuplicatesToSubfolder { set; get; }
            public bool MoveVgmtDuplicatesToSubfolder { set; get; }
            public string[] SourcePaths { set; get; }
        }

        public ExamineChecksumGeneratorWorker() : 
            base() 
        { 
            this.duplicateCheckHashStandard = new Dictionary<string,string[]>(StringComparer.InvariantCultureIgnoreCase);
            this.duplicateCheckHashVgmt = new Dictionary<string, string[]>(StringComparer.InvariantCultureIgnoreCase);

            this.progressCounterIncrementer = 10;
        }

        protected override void DoTaskForFile(string pPath,
            IVgmtWorkerStruct pExamineChecksumGeneratorStruct, DoWorkEventArgs e)
        {
            ExamineChecksumGeneratorStruct examineChecksumGeneratorStruct =
                (ExamineChecksumGeneratorStruct)pExamineChecksumGeneratorStruct;

            string crc32;
            string md5;
            string sha1;

            string vgmtCrc32 = "Not implemented for this format.";
            string vgmtMd5 = "Not implemented for this format.";
            string vgmtSha1 = "Not implemented for this format.";

            string checksumKey;

            Type formatType = null;
            IFormat vgmData = null;

            using (FileStream fs = File.OpenRead(pPath))
            {
                crc32 = ChecksumUtil.GetCrc32OfFullFile(fs);
                md5 = ChecksumUtil.GetMd5OfFullFile(fs);
                sha1 = ChecksumUtil.GetSha1OfFullFile(fs);

                if (examineChecksumGeneratorStruct.CheckForDuplicates)
                {
                    checksumKey = String.Format("{0}/{1}/{2}", crc32, md5, sha1);
                    this.addChecksumToHash(checksumKey, pPath, true);
                }

                if (examineChecksumGeneratorStruct.DoVgmtChecksums)
                {
                    formatType = FormatUtil.getObjectType(fs);
                    if (formatType != null)
                    {
                        vgmData = (IFormat)Activator.CreateInstance(formatType);
                        vgmData.Initialize(fs, pPath);
                    }
                }
            }

            if (vgmData != null)
            {
                Crc32 crc32Generator = new Crc32();

                MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();
                FileStream md5FileStream = new FileStream(Path.GetTempFileName(), FileMode.Create, FileAccess.Write);
                CryptoStream md5CryptoStream = new CryptoStream(md5FileStream, md5Hash, CryptoStreamMode.Write);

                SHA1CryptoServiceProvider sha1Hash = new SHA1CryptoServiceProvider();
                FileStream sha1FileStream = new FileStream(Path.GetTempFileName(), FileMode.Create, FileAccess.Write);
                CryptoStream sha1CryptoStream = new CryptoStream(sha1FileStream, sha1Hash, CryptoStreamMode.Write);

                vgmData.GetDatFileChecksums(ref crc32Generator, ref md5CryptoStream, 
                    ref sha1CryptoStream);
                md5CryptoStream.FlushFinalBlock();
                sha1CryptoStream.FlushFinalBlock();
                                                                
                vgmtCrc32 = crc32Generator.Value.ToString("X8");
                vgmtMd5 = ParseFile.ByteArrayToString(md5Hash.Hash);
                vgmtSha1 = ParseFile.ByteArrayToString(sha1Hash.Hash);

                if (examineChecksumGeneratorStruct.CheckForDuplicates)
                {
                    checksumKey = String.Format("{0}/{1}/{2}", vgmtCrc32, vgmtMd5, vgmtSha1);
                    this.addChecksumToHash(checksumKey, pPath, false);
                }

                md5FileStream.Close();
                md5FileStream.Dispose();
                sha1FileStream.Close();
                sha1FileStream.Dispose();

                md5CryptoStream.Close();
                md5CryptoStream.Dispose();
                sha1CryptoStream.Close();
                sha1CryptoStream.Dispose();
            }

            this.outputBuffer.AppendFormat("<{0}>{1}", pPath, Environment.NewLine);
            this.outputBuffer.AppendFormat("CRC32: {0}{1}", crc32, Environment.NewLine);
            this.outputBuffer.AppendFormat("MD5: {0}{1}", md5, Environment.NewLine);
            this.outputBuffer.AppendFormat("SHA1: {0}{1}", sha1, Environment.NewLine);

            if (examineChecksumGeneratorStruct.DoVgmtChecksums)
            {
                this.outputBuffer.AppendFormat("CRC32 (VGMT): {0}{1}", vgmtCrc32, Environment.NewLine);
                this.outputBuffer.AppendFormat("MD5 (VGMT): {0}{1}", vgmtMd5, Environment.NewLine);
                this.outputBuffer.AppendFormat("SHA1 (VGMT): {0}{1}", vgmtSha1, Environment.NewLine);
            }
            this.outputBuffer.AppendLine();
        }

        protected override void DoFinalTask(IVgmtWorkerStruct pExamineChecksumGeneratorStruct) 
        {
            ExamineChecksumGeneratorStruct examineChecksumGeneratorStruct =
                (ExamineChecksumGeneratorStruct)pExamineChecksumGeneratorStruct;

            string headerFormat = "The following duplicates were found using {0} checksums." + Environment.NewLine +
                "(CRC32/MD5/SHA1)";

            if (examineChecksumGeneratorStruct.CheckForDuplicates)
            {
                this.outputDuplicatesForDictionary(
                    this.duplicateCheckHashStandard, String.Format(headerFormat, "standard"),
                        examineChecksumGeneratorStruct.MoveStandardDuplicatesToSubfolder,
                        ExamineChecksumGeneratorWorker.StandardDuplicatesSubfolder);

                if (examineChecksumGeneratorStruct.DoVgmtChecksums)
                {
                    this.outputDuplicatesForDictionary(
                        this.duplicateCheckHashVgmt, String.Format(headerFormat, "VGMToolbox method"),
                            examineChecksumGeneratorStruct.MoveVgmtDuplicatesToSubfolder,
                            ExamineChecksumGeneratorWorker.VGMToolboxDuplicatesSubfolder);                
                }
            
            }
        }

        private void outputDuplicatesForDictionary(Dictionary<string,string[]> hashList, string hashLabel, 
            bool moveDuplicates, string duplicateDestinationFolder)
        { 
            StringBuilder duplicateList = new StringBuilder();
            string[] paths;
            string destinationPath;
            int duplicateCount;
            
            string movedTag = String.Empty;

            duplicateList.AppendFormat("{0}:{1}", hashLabel, Environment.NewLine);
            
            foreach (string key in hashList.Keys)
            {
                paths = hashList[key];

                if (paths.Length > 1)
                {
                    duplicateList.AppendFormat("{0}:{1}", key, Environment.NewLine);
                    duplicateCount = 0;
                    movedTag = String.Empty;
                    
                    foreach (string s in paths)
                    {
                        // move all files except the first instance
                        if ((moveDuplicates) && (duplicateCount > 0))
                        {
                            destinationPath = Path.Combine(
                                                Path.Combine(Path.GetDirectoryName(s), duplicateDestinationFolder),
                                                Path.GetFileName(s));
                            movedTag = moveDuplicateFile(s, destinationPath);
                        }
                        
                        duplicateList.AppendFormat("  {0}{1}{2}", s, movedTag, Environment.NewLine);
                        duplicateCount++;
                    }
                }
            }

            this.progressStruct.Clear();
            progressStruct.GenericMessage = duplicateList.ToString();
            ReportProgress(Constants.ProgressMessageOnly, progressStruct);
        }

        private void addChecksumToHash(string checksum, string path, bool isStandardChecksum)
        {
            ArrayList paths = new ArrayList();
            
            if (isStandardChecksum)
            {
                if (this.duplicateCheckHashStandard.ContainsKey(checksum))
                {
                    paths = new ArrayList(this.duplicateCheckHashStandard[checksum]);
                    
                }

                paths.Add(path);
                this.duplicateCheckHashStandard[checksum] = (string[])paths.ToArray(typeof(string));
            }
            else
            {
                // do VGMT checksums
                if (this.duplicateCheckHashVgmt.ContainsKey(checksum))
                {
                    paths = new ArrayList(this.duplicateCheckHashVgmt[checksum]);

                }

                paths.Add(path);
                this.duplicateCheckHashVgmt[checksum] = (string[])paths.ToArray(typeof(string));            
            }
        }

        private string moveDuplicateFile(string sourcePath, string destinationPath)
        {
            string moveMessage;
            string destinationFolder;

            if (File.Exists(sourcePath))
            {
                if (File.Exists(destinationPath)) // there is already a file of the same name in the destination
                {
                    moveMessage = " (DESTINATION FILE ALREADY EXISTS)";
                }
                else
                {
                    // move file
                    destinationFolder = Path.GetDirectoryName(destinationPath);

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    File.Move(sourcePath, destinationPath);
                    moveMessage = " (MOVED)";
                }
            }
            else if (File.Exists(destinationPath)) // file has already been moved
            {
                moveMessage = " (FILE ALREADY MOVED)";
            }
            else // file is not in source or destination (disappeared?)
            {
                moveMessage = " (FILE NOT FOUND)";
            }

            return moveMessage;
        }
    }
}
