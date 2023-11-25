using System;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.other
{
    public class ExternalListFileRenamerWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        public const string RENAME_SCRIPT_NAME = "vgmt_rename_script.bat";
        public const string UNDO_RENAME_SCRIPT_NAME = "vgmt_undo_rename_script.bat";

        VGMToolbox.util.ProgressStruct progressStruct = new VGMToolbox.util.ProgressStruct();

        public struct ExternalListFileRenamerStruct
        {
            public string SourceFolder { set; get; }
            public string SourceFileMask { set; get; }
            public bool KeepOriginalFileExtension { set; get; }

            public string ListFileLocation { set; get; }
            public string OffsetToFileNames { set; get; }
            public string FileNameEncoding { set; get; }
            public string FileNameCodePage { set; get; }
            
            public bool FileNameHasTerminator { set; get; }
            public string FileNameTerminator { set; get; }
            public bool FileNameHasStaticSize { set; get; }
            public string FileNameStaticSize { set; get; }
        }

        public ExternalListFileRenamerWorker()
        {
            this.progressStruct = new VGMToolbox.util.ProgressStruct();

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        private void RenameFilesFromExternalList(ExternalListFileRenamerStruct inputValues)
        {
            string fileMask = null;
            string[] sourceFiles;
            int sourceFileCount;

            long listFileLength;
            long fileNameOffset;

            StringBuilder renameScript;
            StringBuilder undoScript;

            string sourceFile;

            byte[] destinationFileTerminator = new byte[0];
            long destinationFileBytesSize = -1;
            byte[] destinationFileBytes;
            string destinationFile;
            string destinationFolder;

            long currentOffset;

            // verify directory exists and list file exists
            if (Directory.Exists(inputValues.SourceFolder) &&
                File.Exists(inputValues.ListFileLocation))
            {
                // set file mask
                fileMask = inputValues.SourceFileMask.Trim();
                fileMask = String.IsNullOrEmpty(fileMask) ? "*.*" : fileMask;

                // get source file count
                sourceFiles = Directory.GetFiles(inputValues.SourceFolder, fileMask, SearchOption.TopDirectoryOnly);
                sourceFileCount = sourceFiles.GetLength(0);

                // verify some source files exist
                if (sourceFileCount == 0)
                {
                    throw new Exception(String.Format("Source directory is empty."));
                }
                else
                {
                    // convert file name offset to long
                    fileNameOffset = ByteConversion.GetLongValueFromString(inputValues.OffsetToFileNames);

                    // open List File
                    using (FileStream listFs = File.Open(inputValues.ListFileLocation, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        listFileLength = listFs.Length;

                        // verify offset exists within list file
                        if (listFileLength < fileNameOffset)
                        {
                            throw new IOException(String.Format("File name offset is greater than the size of the list file."));
                        }
                        else
                        {
                            renameScript = new StringBuilder();
                            undoScript = new StringBuilder();
                            
                            // parse file name terminator, if included
                            if (inputValues.FileNameHasTerminator)
                            {
                                try
                                {
                                    destinationFileTerminator = ByteConversion.GetBytesFromHexString(inputValues.FileNameTerminator);
                                }
                                catch (Exception ex1)
                                {
                                    throw new FormatException(String.Format("Unable to convert File Name Terminator Bytes to hex: '{0}'", ex1.Message), ex1);
                                }
                            }
                            else // convert static filename size
                            {
                                try
                                {
                                    destinationFileBytesSize = ByteConversion.GetLongValueFromString(inputValues.FileNameStaticSize);
                                }
                                catch (Exception ex2)
                                {
                                    throw new FormatException(String.Format("Unable to convert File Name static size fo a number: '{0}'", ex2.Message), ex2);
                                }

                            }


                            try
                            {
                                // rename files
                                currentOffset = fileNameOffset;

                                for (int i = 0; i < sourceFileCount; i++)
                                {
                                    sourceFile = sourceFiles[i];

                                    // get name for files with a terminator
                                    if (inputValues.FileNameHasTerminator)
                                    {
                                        destinationFileBytesSize = ParseFile.GetNextOffset(listFs, currentOffset, destinationFileTerminator) - currentOffset;
                                    }

                                    // read destination file name
                                    if (destinationFileBytesSize > 0)
                                    {
                                        destinationFileBytes = ParseFile.ParseSimpleOffset(listFs, currentOffset, (int)destinationFileBytesSize);
                                        destinationFile = this.getFileNameFromEncodedBytes(inputValues.FileNameEncoding, inputValues.FileNameCodePage, destinationFileBytes);
                                        currentOffset += destinationFileBytesSize + destinationFileTerminator.Length;
                                    }
                                    else
                                    {
                                        this.progressStruct.Clear();
                                        this.progressStruct.FileName = sourceFile;
                                        this.progressStruct.GenericMessage = String.Format("Warning: End of List File reached when renaming <{0}>{1}", sourceFile, Environment.NewLine);
                                        this.ReportProgress(Constants.ProgressMessageOnly, progressStruct);
                                        break;
                                    }

                                    // rename file and build scripts
                                    destinationFile = Path.Combine(inputValues.SourceFolder, destinationFile).Trim();

                                    if (inputValues.KeepOriginalFileExtension)
                                    {
                                        if (String.IsNullOrEmpty(Path.GetExtension(destinationFile)))
                                        {
                                            destinationFile += Path.GetExtension(sourceFile);
                                        }
                                        else
                                        {
                                            Path.ChangeExtension(destinationFile, Path.GetExtension(sourceFile));
                                        }
                                    }

                                    if (sourceFile != destinationFile)
                                    {
                                        renameScript.AppendFormat("rename \"{0}\" \"{1}\" {2}", Path.GetFileName(sourceFile), Path.GetFileName(destinationFile), Environment.NewLine);
                                        undoScript.AppendFormat("rename \"{0}\" \"{1}\" {2}", Path.GetFileName(destinationFile), Path.GetFileName(sourceFile), Environment.NewLine);

                                        destinationFolder = Path.GetDirectoryName(destinationFile);

                                        if (!Directory.Exists(destinationFolder))
                                        {
                                            Directory.CreateDirectory(destinationFolder);
                                        }

                                        File.Move(sourceFile, destinationFile);
                                    }




                                    // report progress
                                    this.progressStruct.Clear();
                                    this.progressStruct.FileName = sourceFile;
                                    this.ReportProgress(((i + 1) * 100) / sourceFileCount, progressStruct);
                                }

                            }
                            finally
                            {
                                if (renameScript.Length > 0)
                                { 
                                    // write to file.
                                    FileUtil.CreateFileFromString(Path.Combine(inputValues.SourceFolder, RENAME_SCRIPT_NAME), renameScript.ToString());
                                }

                                if (undoScript.Length > 0)
                                {
                                    // write to file.
                                    FileUtil.CreateFileFromString(Path.Combine(inputValues.SourceFolder, UNDO_RENAME_SCRIPT_NAME), undoScript.ToString());
                                }
                            }

                        } // if (listFileLength < fileNameOffset)



                    } // using (FileStream listFs = File.Open(inputValues.ListFileLocation, FileMode.Open, FileAccess.Read, FileShare.Read))


                } // if (sourceFileCount == 0)

            }
            else
            {
                throw new IOException(String.Format("Directory not found: <{0}>", inputValues.SourceFolder));
            } // if (Directory.Exists(inputValues.SourceFolder))
        }

        private string getFileNameFromEncodedBytes(string EncodingString, string CodePageString, byte[] FileNameBytes)
        {
            StringBuilder fileNameBuffer = new StringBuilder();
            string fileNameChar;

            Encoding enc = null;
            long codePage = ByteConversion.CodePageUnitedStates;
            int bytesToRead;

            switch (EncodingString)
            { 
                case "ASCII":
                    enc = Encoding.ASCII;
                    bytesToRead = 1;
                    break;
                case "UTF8":
                    enc = Encoding.UTF8;
                    bytesToRead = 1;
                    break;
                case "UTF16-LE":
                    enc = Encoding.Unicode;
                    bytesToRead = 2;
                    break;
                case "UTF16-BE":
                    enc = Encoding.BigEndianUnicode;
                    bytesToRead = 2;
                    break;
                case "UTF32-LE":
                    enc = Encoding.UTF32;
                    bytesToRead = 4;
                    break;
                case "Code Page":
                    // convert code page to number
                    if (!String.IsNullOrEmpty(CodePageString))
                    {
                        codePage = ByteConversion.GetLongValueFromString(CodePageString);
                    }

                    enc = Encoding.GetEncoding((int)(codePage));
                    bytesToRead = 1;
                    break;
                default:
                    enc = Encoding.ASCII;
                    bytesToRead = 1;
                    break;            
            }

            for (int i = 0; i < FileNameBytes.Length; i += bytesToRead)
            {
                fileNameChar = enc.GetString(FileNameBytes, i, bytesToRead);

                if (fileNameChar.Equals(Constants.StringNullTerminator))
                {
                    break;
                }
                else
                {
                    fileNameBuffer.Append(fileNameChar);
                }
            }

            return fileNameBuffer.ToString();
        }
               
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            ExternalListFileRenamerStruct inputs = (ExternalListFileRenamerStruct)e.Argument;

            try
            {
                RenameFilesFromExternalList(inputs);
            }
            catch (Exception _ex)
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = String.Format("Error renaming files: {0}{1}", _ex.Message, Environment.NewLine);
                ReportProgress(0, this.progressStruct);
            }
        }
    }
}
