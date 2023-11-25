using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Ionic.Zlib;

using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;

using VGMToolbox.format.sdat;
using VGMToolbox.util;

namespace VGMToolbox.format.util
{
    public struct Time2sfStruct
    {
        private string mini2sfDirectory;
        private string sdatPath;
        private bool doSingleLoop;

        public string Mini2sfDirectory
        {
            set { mini2sfDirectory = value; }
            get { return mini2sfDirectory; }
        }
        public string SdatPath
        {
            set { sdatPath = value; }
            get { return sdatPath; }
        }
        public bool DoSingleLoop
        {
            set { doSingleLoop = value; }
            get { return doSingleLoop; }
        }
    }
    public struct Xsf2ExeStruct
    {
        private bool includeExtension;
        private bool stripGsfHeader;

        public bool IncludeExtension
        {
            set { includeExtension = value; }
            get { return includeExtension; }
        }
        public bool StripGsfHeader
        {
            set { stripGsfHeader = value; }
            get { return stripGsfHeader; }
        }
    }
    public struct XsfBasicTaggingStruct
    {
        private string tagArtist;
        private string tagCopyright;
        private string tagYear;
        private string tagGame;
        private string tagComment;
        private string tagXsfByTagName;
        private string tagXsfByTagValue;

        public string TagArtist
        {
            set { tagArtist = value; }
            get { return tagArtist; }
        }
        public string TagCopyright
        {
            set { tagCopyright = value; }
            get { return tagCopyright; }
        }
        public string TagYear
        {
            set { tagYear = value; }
            get { return tagYear; }
        }
        public string TagGame
        {
            set { tagGame = value; }
            get { return tagGame; }
        }
        public string TagComment
        {
            set { tagComment = value; }
            get { return tagComment; }
        }
        public string TagXsfByTagName
        {
            set { tagXsfByTagName = value; }
            get { return tagXsfByTagName; }
        }
        public string TagXsfByTagValue
        {
            set { tagXsfByTagValue = value; }
            get { return tagXsfByTagValue; }
        }
    }
    public struct XsfRecompressStruct
    {
        private int compressionLevel;

        public int CompressionLevel
        {
            set { compressionLevel = value; }
            get { return compressionLevel; }
        }
    }
    public struct XsfTagCopyStruct
    {
        private bool copyEmptyTags;
        private bool updateTitleTag;
        private bool updateArtistTag;
        private bool updateGameTag;
        private bool updateYearTag;
        private bool updateGenreTag;
        private bool updateCommentTag;
        private bool updateCopyrightTag;
        private bool updateXsfByTag;
        private bool updateVolumeTag;
        private bool updateLengthTag;
        private bool updateFadeTag;
        private bool updateSystemTag;

        public bool CopyEmptyTags
        {
            set { copyEmptyTags = value; }
            get { return copyEmptyTags; }
        }
        public bool UpdateTitleTag
        {
            set { updateTitleTag = value; }
            get { return updateTitleTag; }
        }
        public bool UpdateArtistTag
        {
            set { updateArtistTag = value; }
            get { return updateArtistTag; }
        }
        public bool UpdateGameTag
        {
            set { updateGameTag = value; }
            get { return updateGameTag; }
        }
        public bool UpdateYearTag
        {
            set { updateYearTag = value; }
            get { return updateYearTag; }
        }
        public bool UpdateGenreTag
        {
            set { updateGenreTag = value; }
            get { return updateGenreTag; }
        }
        public bool UpdateCommentTag
        {
            set { updateCommentTag = value; }
            get { return updateCommentTag; }
        }
        public bool UpdateCopyrightTag
        {
            set { updateCopyrightTag = value; }
            get { return updateCopyrightTag; }
        }
        public bool UpdateXsfByTag
        {
            set { updateXsfByTag = value; }
            get { return updateXsfByTag; }
        }
        public bool UpdateVolumeTag
        {
            set { updateVolumeTag = value; }
            get { return updateVolumeTag; }
        }
        public bool UpdateLengthTag
        {
            set { updateLengthTag = value; }
            get { return updateLengthTag; }
        }
        public bool UpdateFadeTag
        {
            set { updateFadeTag = value; }
            get { return updateFadeTag; }
        }
        public bool UpdateSystemTag
        {
            set { updateSystemTag = value; }
            get { return updateSystemTag; }
        }
    }

    public class PsfPsyQAddresses
    {
        // private
        private string psfDrvLoadAddress;
        private string driverTextString;
        private string exeFileNameCrc;
        private string jumpPatchAddress;

        private string resetCallback;
        private string ssInit;        
        private string ssSeqOpen;
        private string ssSeqPlay;
        private string ssSetMVol;
        private string ssStart;
        private string ssStart2;
        private string ssSetTableSize;
        private string ssSetTickMode;
        private string ssSeqSetVol;
        private string ssUtSetReverbType;
        private string ssUtReverbOn;
        private string ssVabOpenHead;
        private string ssVabOpenHeadSticky;
        private string ssVabTransBodyPartly;
        private string ssVabTransBody;
        private string ssVabTransCompleted;

        private string spuInit;
        private string spuIsTransferCompleted;
        
        private string spuSetReverb;
        private string spuSetReverbModeParam;
        private string spuSetReverbDepth;
        private string spuSetReverbVoice;

        // public
        public string PsfDrvLoadAddress
        {
            set { psfDrvLoadAddress = value; }
            get { return psfDrvLoadAddress; }
        }
        public string DriverTextString
        {
            set { driverTextString = value; }
            get { return driverTextString; }        
        }
        public string ExeFileNameCrc
        {
            set { exeFileNameCrc = value; }
            get { return exeFileNameCrc; }
        }
        public string JumpPatchAddress
        {
            set { jumpPatchAddress = value; }
            get { return jumpPatchAddress; }
        }
        
        public string ResetCallback
        {
            set { resetCallback = value; }
            get { return resetCallback; }
        }
        public string SsInit
        {
            set { ssInit = value; }
            get { return ssInit; }
        }
        public string SsSeqOpen
        {
            set { ssSeqOpen = value; }
            get { return ssSeqOpen; }
        }
        public string SsSeqPlay
        {
            set { ssSeqPlay = value; }
            get { return ssSeqPlay; }
        }
        public string SsSetMVol
        {
            set { ssSetMVol = value; }
            get { return ssSetMVol; }
        }
        public string SsStart
        {
            set { ssStart = value; }
            get { return ssStart; }
        }
        public string SsStart2
        {
            set { ssStart2 = value; }
            get { return ssStart2; }
        }
        public string SsSetTableSize
        {
            set { ssSetTableSize = value; }
            get { return ssSetTableSize; }
        }
        public string SsSetTickMode
        {
            set { ssSetTickMode = value; }
            get { return ssSetTickMode; }
        }
        public string SsSeqSetVol
        {
            set { ssSeqSetVol = value; }
            get { return ssSeqSetVol; }
        }
        public string SsUtSetReverbType
        {
            set { ssUtSetReverbType = value; }
            get { return ssUtSetReverbType; }
        }
        public string SsUtReverbOn
        {
            set { ssUtReverbOn = value; }
            get { return ssUtReverbOn; }
        }
        public string SsVabOpenHead
        {
            set { ssVabOpenHead = value; }
            get { return ssVabOpenHead; }
        }
        public string SsVabOpenHeadSticky
        {
            set { ssVabOpenHeadSticky = value; }
            get { return ssVabOpenHeadSticky; }
        }
        public string SsVabTransBodyPartly
        {
            set { ssVabTransBodyPartly = value; }
            get { return ssVabTransBodyPartly; }
        }
        public string SsVabTransBody
        {
            set { ssVabTransBody = value; }
            get { return ssVabTransBody; }
        }
        public string SsVabTransCompleted
        {
            set { ssVabTransCompleted = value; }
            get { return ssVabTransCompleted; }
        }

        public string SpuInit
        {
            set { spuInit = value; }
            get { return spuInit; }
        }
        public string SpuIsTransferCompleted
        {
            set { spuIsTransferCompleted = value; }
            get { return spuIsTransferCompleted; }
        }

        public string SpuSetReverb
        {
            set { spuSetReverb = value; }
            get { return spuSetReverb; }
        }
        public string SpuSetReverbModeParam
        {
            set { spuSetReverbModeParam = value; }
            get { return spuSetReverbModeParam; }
        }
        public string SpuSetReverbDepth
        {
            set { spuSetReverbDepth = value; }
            get { return spuSetReverbDepth; }
        }
        public string SpuSetReverbVoice
        {
            set { spuSetReverbVoice = value; }
            get { return spuSetReverbVoice; }
        }

        public string SsSepOpen { set; get; }
        public string SsSepPlay { set; get; }
    }

    public class XsfUtil
    {
        private delegate void XsfTagSetter(string pValue, bool flag);
        private delegate string XsfTagGetter();
        
        public const string RecompressedSubfolderName = "recompressed";
        public const int InvalidData = -1;
        
        static readonly string BIN2PSF_SOURCE_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "external"), "bin2psf.exe");
        static readonly string PSFPOINT_SOURCE_PATH =
            Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "external"), "psfpoint.exe");
        public const string PSFPOINT_BATCH_TXT = "psfpoint_batch.bat";

        // 2SF CONSTANTS
        public const string SSEQ2MID_TXT = "sseq2mid_output.txt";        
        public const string SSEQ2MID_TXT_MARKER = ".sseq:";
        public const string SSEQ2MID_TXT_END_OF_TRACK = "End of Track";
        public const string EMPTY_FILE_DIRECTORY = "Empty_Files";
        public const string NDSTO2SF_FOLDER_SUFFIX = "_NDSto2SF";

        static readonly string SSEQ2MID_SOURCE_PATH =
            Path.Combine(Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "external"), "2sf"), "sseq2mid.exe");

        static readonly byte[] MINI2SF_DATA_START = new byte[] { 0xC0, 0x0F, 0x0D, 0x00, 0x02, 0x00, 0x00, 0x00 };
        static readonly byte[] MINI2SF_SAVESTATE_ID = new byte[] { 0x53, 0x41, 0x56, 0x45 };

        // PSF
        public const uint PSFDRV_LOAD_ADDRESS = 0x80100000;
        public static readonly byte[] PlayStationExecutableSignature = new byte[] { 0x50, 0x53, 0x2D, 0x58, 0x20, 0x45, 0x58, 0x45};
        public static readonly byte[] VAB_SIGNATURE = new byte[] { 0x70, 0x42, 0x41, 0x56 }; // "pBAV"

        private XsfUtil() { }
        
        public static bool IsPythonPresentInPath()
        {
            bool ret = false;
            
            string pathVariable = Environment.GetEnvironmentVariable("PATH");
            string[] paths = pathVariable.Split(new char[] { ';' });

            foreach (string p in paths)
            {
                if (Directory.Exists(p))
                {
                    string[] files = Directory.GetFiles(p, "python.exe");

                    if (files.Length > 0)
                    {
                        ret = true;
                        break;
                    }
                }
            }

            return ret;
        }
        
        public static string ExtractCompressedDataSection(string pPath, Xsf2ExeStruct pXsf2ExeStruct)
        {
            string outputFile = null;
            string formatString = GetXsfFormatString(pPath);

            if (!String.IsNullOrEmpty(formatString))
            {
                using (FileStream fs = File.OpenRead(pPath))
                {
                    Xsf vgmData = new Xsf();
                    vgmData.Initialize(fs, pPath);

                    if (vgmData.CompressedProgramLength > 0)
                    {
                        outputFile = Path.GetDirectoryName(pPath) + Path.DirectorySeparatorChar;
                        outputFile += (pXsf2ExeStruct.IncludeExtension ? Path.GetFileName(pPath) : Path.GetFileNameWithoutExtension(pPath)) + ".data.bin";

                        using (BinaryWriter bw = new BinaryWriter(File.Create(outputFile)))
                        {
                            using (ZlibStream zs = new ZlibStream(fs, Ionic.Zlib.CompressionMode.Decompress, true))
                            {
                                fs.Seek((long)(Xsf.RESERVED_SECTION_OFFSET + vgmData.ReservedSectionLength), SeekOrigin.Begin);
                                int read;
                                byte[] data = new byte[4096];

                                while ((read = zs.Read(data, 0, data.Length)) > 0)
                                {
                                    bw.Write(data, 0, read);
                                }                                
                            }
                        }

                        // strip GSF header
                        if (pXsf2ExeStruct.StripGsfHeader)
                        {
                            string strippedOutputFileName = outputFile + ".strip";

                            using (FileStream gsfStream = File.OpenRead(outputFile))
                            {
                                long fileOffset = 0x0C;
                                int fileLength = (int)(gsfStream.Length - fileOffset) + 1;

                                ParseFile.ExtractChunkToFile(gsfStream, fileOffset, fileLength,
                                    strippedOutputFileName);
                            }

                            File.Copy(strippedOutputFileName, outputFile, true);
                            File.Delete(strippedOutputFileName);
                        }

                    } // if (vgmData.CompressedProgramLength > 0)
                } // using (FileStream fs = File.OpenRead(pPath))       
            }
            return outputFile;
        }

        public static string ExtractReservedSection(string pPath, Xsf2ExeStruct pXsf2ExeStruct)
        {
            string outputFile = null;
            string formatString = GetXsfFormatString(pPath);

            if (!String.IsNullOrEmpty(formatString))
            {
                using (FileStream fs = File.OpenRead(pPath))
                {
                    Xsf vgmData = new Xsf();
                    vgmData.Initialize(fs, pPath);

                    if (vgmData.ReservedSectionLength > 0)
                    {
                        outputFile = String.Format("{0}.reserved.bin",
                            (pXsf2ExeStruct.IncludeExtension ? Path.GetFileName(pPath) : Path.GetFileNameWithoutExtension(pPath)));
                        outputFile = Path.Combine(Path.GetDirectoryName(pPath), outputFile);

                        ParseFile.ExtractChunkToFile(fs, Xsf.RESERVED_SECTION_OFFSET,
                            (int)vgmData.ReservedSectionLength, outputFile);

                    } // if (vgmData.CompressedProgramLength > 0)
                } // using (FileStream fs = File.OpenRead(pPath))       
            }
            
            return outputFile;        
        }

        public static string ReCompressDataSection(string pPath, XsfRecompressStruct pXsfRecompressStruct)
        {
            string outputFolder = Path.Combine(Path.GetDirectoryName(pPath), RecompressedSubfolderName);
            string outputPath = null;

            Xsf2ExeStruct xsf2ExeStruct = new Xsf2ExeStruct();
            xsf2ExeStruct.IncludeExtension = true;
            xsf2ExeStruct.StripGsfHeader = false;

            string extractedDataPath = ExtractCompressedDataSection(pPath, xsf2ExeStruct);
            string deflatedCrc32String;

            if (extractedDataPath != null)
            {
                string deflatedOutputPath = null;
                
                try
                {
                    deflatedOutputPath = Path.ChangeExtension(extractedDataPath, ".deflated");

                    int read;
                    byte[] data = new byte[4096];

                    // open decompressed data section
                    using (FileStream inFs = File.OpenRead(extractedDataPath))
                    {
                        // open file for outputting deflated section
                        using (FileStream outFs = File.Open(deflatedOutputPath, FileMode.Create, FileAccess.Write))
                        {
                            using (ZlibStream zs = new ZlibStream(outFs, Ionic.Zlib.CompressionMode.Compress, (CompressionLevel)pXsfRecompressStruct.CompressionLevel, true))
                            {
                                while ((read = inFs.Read(data, 0, data.Length)) > 0)
                                {
                                    zs.Write(data, 0, read);
                                }

                                zs.Flush();
                            }
                        }
                    }

                    // create output path
                    outputPath = Path.Combine(outputFolder, Path.GetFileName(pPath));

                    if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                    }

                    using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(outputPath)))
                    {
                        using (FileStream vgmFs = File.OpenRead(pPath))
                        {
                            Xsf vgmData = new Xsf();
                            vgmData.Initialize(vgmFs, pPath);

                            bw.Write(vgmData.AsciiSignature);
                            bw.Write(vgmData.VersionByte);
                            bw.Write(vgmData.ReservedSectionLength);
                            bw.Write(BitConverter.GetBytes((uint)new FileInfo(deflatedOutputPath).Length));

                            using (FileStream outFs = File.Open(deflatedOutputPath, FileMode.Open, FileAccess.Read))
                            {
                                // crc32
                                deflatedCrc32String = "0x" + ChecksumUtil.GetCrc32OfFullFile(outFs);
                                bw.Write((uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(deflatedCrc32String));
                                // reserved section
                                bw.Write(ParseFile.ParseSimpleOffset(vgmFs, Xsf.RESERVED_SECTION_OFFSET, (int)vgmData.ReservedSectionLength));

                                // data section
                                outFs.Position = 0;
                                while ((read = outFs.Read(data, 0, data.Length)) > 0)
                                {
                                    bw.Write(data, 0, read);
                                }
                            }

                            long dataSectionEnd = Xsf.RESERVED_SECTION_OFFSET + vgmData.ReservedSectionLength + vgmData.CompressedProgramLength;
                            bw.Write(ParseFile.ParseSimpleOffset(vgmFs, dataSectionEnd, (int)(vgmFs.Length - dataSectionEnd)));
                        }
                    }
                }
                finally
                {
                    if (File.Exists(extractedDataPath))
                    {
                        File.Delete(extractedDataPath);
                    }

                    if ((deflatedOutputPath != null) && 
                        (File.Exists(deflatedOutputPath)))
                    {
                        File.Delete(deflatedOutputPath);
                    }
                }
            }
            return outputPath;
        }

        public static void Bin2Psf(string pExtension, int pVersionByte, string pPath, 
            ref string pStandardOutput, ref string pStandardError)
        {
            Process bin2PsfProcess = null;
            string filePath = Path.GetFullPath(pPath);

            // call bin2psf.exe
            string arguments = String.Format(" \"{0}\" {1} \"{2}\"", pExtension, pVersionByte.ToString(), filePath);
            bin2PsfProcess = new Process();
            bin2PsfProcess.StartInfo = new ProcessStartInfo(BIN2PSF_SOURCE_PATH, arguments);
            bin2PsfProcess.StartInfo.UseShellExecute = false;
            bin2PsfProcess.StartInfo.CreateNoWindow = true;

            bin2PsfProcess.StartInfo.RedirectStandardError = true;
            bin2PsfProcess.StartInfo.RedirectStandardOutput = true;

            bool isSuccess = bin2PsfProcess.Start();
            pStandardOutput = bin2PsfProcess.StandardOutput.ReadToEnd();
            pStandardError = bin2PsfProcess.StandardError.ReadToEnd();

            bin2PsfProcess.WaitForExit();
            bin2PsfProcess.Close();
            bin2PsfProcess.Dispose();

            // return outputDir;        
        }

        public static string GetXsfFormatString(string pPath)
        {
            string ret = null;

            using (FileStream typeFs = File.Open(pPath, FileMode.Open, FileAccess.Read))
            {
                Type dataType = FormatUtil.getObjectType(typeFs);

                if (dataType != null && 
                    (dataType.Name.Equals("Xsf") || dataType.Name.Equals("Psf")))
                {
                    Xsf xsfFile = new Xsf();
                    xsfFile.Initialize(typeFs, pPath);

                    ret = xsfFile.GetFormat();
                }
            }

            
            return ret;
        }

        public static void ExecutePsfPointBatchScript(string pFullPathToScript, bool pDeleteScriptAfterExecution)
        {
            Process batProcess;
            string folderContainingScript = Path.GetDirectoryName(pFullPathToScript);
            string scriptName = Path.GetFileName(pFullPathToScript);
            bool cleanupPsfpoint = false;

            // copy psfpoint.exe
            string psfpointDestinationPath = Path.Combine(folderContainingScript, Path.GetFileName(PSFPOINT_SOURCE_PATH));

            if (!File.Exists(psfpointDestinationPath))
            {
                File.Copy(PSFPOINT_SOURCE_PATH, psfpointDestinationPath, false);
                cleanupPsfpoint = true;
            }

            // execute script
            batProcess = new Process();
            batProcess.StartInfo = new ProcessStartInfo(PSFPOINT_BATCH_TXT);
            batProcess.StartInfo.WorkingDirectory = folderContainingScript;
            batProcess.StartInfo.CreateNoWindow = true;
            batProcess.Start();
            batProcess.WaitForExit();

            batProcess.Close();
            batProcess.Dispose();

            if (cleanupPsfpoint)
            {
                File.Delete(psfpointDestinationPath);
            }

            if (pDeleteScriptAfterExecution)
            {
                File.Delete(pFullPathToScript);
            }
        }

        public static string BuildBasicTaggingBatch(string pFullPathToScriptDestinationFolder, 
            XsfBasicTaggingStruct pXsfBasicTaggingStruct, string pFileMask)
        {            
            string batchFilePath = Path.Combine(pFullPathToScriptDestinationFolder, PSFPOINT_BATCH_TXT);
            if (File.Exists(batchFilePath))
            {
                batchFilePath += "_" + new Random().Next().ToString();
            }

            using (StreamWriter sw = new StreamWriter(File.Open(batchFilePath, FileMode.CreateNew, FileAccess.Write)))
            {
                string tagFormat = "psfpoint.exe {0}=\"{1}\" {2}";
                
                if (!String.IsNullOrEmpty(pXsfBasicTaggingStruct.TagArtist))
                {
                    sw.WriteLine(String.Format(tagFormat, "-artist", pXsfBasicTaggingStruct.TagArtist, pFileMask));
                }

                if (!String.IsNullOrEmpty(pXsfBasicTaggingStruct.TagCopyright))
                {
                    sw.WriteLine(String.Format(tagFormat, "-copyright", pXsfBasicTaggingStruct.TagCopyright, pFileMask));
                }

                if (!String.IsNullOrEmpty(pXsfBasicTaggingStruct.TagYear))
                {
                    sw.WriteLine(String.Format(tagFormat, "-year", pXsfBasicTaggingStruct.TagYear, pFileMask));
                }

                if (!String.IsNullOrEmpty(pXsfBasicTaggingStruct.TagGame))
                {
                    sw.WriteLine(String.Format(tagFormat, "-game", pXsfBasicTaggingStruct.TagGame, pFileMask));
                }

                if (!String.IsNullOrEmpty(pXsfBasicTaggingStruct.TagComment))
                {
                    sw.WriteLine(String.Format(tagFormat, "-comment", pXsfBasicTaggingStruct.TagComment, pFileMask));
                }

                if (!String.IsNullOrEmpty(pXsfBasicTaggingStruct.TagXsfByTagValue))
                {
                    sw.WriteLine(String.Format(tagFormat, pXsfBasicTaggingStruct.TagXsfByTagName, 
                        pXsfBasicTaggingStruct.TagXsfByTagValue, pFileMask));
                }
            }

            return batchFilePath;
        }

        public static string GetTitleForFileName(string pFilePath, bool pRemoveBracketInfo)
        {
            string fileName = Path.GetFileNameWithoutExtension(pFilePath);
            string[] splitFileName = fileName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string ret = fileName.Trim();

            int openBracketLoc;
            int closeBracketLoc;


            if (splitFileName.Length > 1)
            {
                string firstSegment = splitFileName[0].Trim();
                string firstSegmentNumOnly = Regex.Replace(firstSegment, "[^0-9]", String.Empty);

                float firstSegmentLength = (float) firstSegment.Length;
                float firstSegmentNumOnlyLength = (float) firstSegmentNumOnly.Length;

                if ((firstSegmentNumOnlyLength > 0) && 
                    ((firstSegmentNumOnlyLength / firstSegmentLength) > 0.5)) // more than half are numeric
                {
                    ret = fileName.Substring(firstSegment.Length).Trim();
                }
            }

            if (pRemoveBracketInfo)
            {                    
                while ((openBracketLoc = ret.IndexOf("[", 0, StringComparison.InvariantCulture)) > -1)
                {
                    closeBracketLoc = ret.IndexOf("]", openBracketLoc, StringComparison.InvariantCulture);

                    if (closeBracketLoc > -1)
                    {
                        ret = ret.Remove(openBracketLoc, closeBracketLoc - openBracketLoc + 1).Trim();
                        openBracketLoc = closeBracketLoc;
                        closeBracketLoc = -1;
                    }
                    else
                    {
                        break;
                    }                                        
                }
            }

            return ret;
        }
        
        public static void CopyTags(IXsfTagFormat pSource, IXsfTagFormat pDestination, 
            XsfTagCopyStruct pXsfTagCopyStruct)
        {
            XsfTagGetter getter;
            XsfTagSetter setter;

            getter = new XsfTagGetter(pSource.GetTitleTag);
            setter = new XsfTagSetter(pDestination.SetTitleTag);
            CopyTag(pXsfTagCopyStruct.UpdateTitleTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);

            getter = new XsfTagGetter(pSource.GetArtistTag);
            setter = new XsfTagSetter(pDestination.SetArtistTag);
            CopyTag(pXsfTagCopyStruct.UpdateArtistTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);

            getter = new XsfTagGetter(pSource.GetGameTag);
            setter = new XsfTagSetter(pDestination.SetGameTag);
            CopyTag(pXsfTagCopyStruct.UpdateGameTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);
            
            getter = new XsfTagGetter(pSource.GetYearTag);
            setter = new XsfTagSetter(pDestination.SetYearTag);
            CopyTag(pXsfTagCopyStruct.UpdateYearTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);

            getter = new XsfTagGetter(pSource.GetGenreTag);
            setter = new XsfTagSetter(pDestination.SetGenreTag);
            CopyTag(pXsfTagCopyStruct.UpdateGenreTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);
            
            getter = new XsfTagGetter(pSource.GetCommentTag);
            setter = new XsfTagSetter(pDestination.SetCommentTag);
            CopyTag(pXsfTagCopyStruct.UpdateCommentTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);


            getter = new XsfTagGetter(pSource.GetCopyrightTag);
            setter = new XsfTagSetter(pDestination.SetCopyrightTag);
            CopyTag(pXsfTagCopyStruct.UpdateCopyrightTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);            

            getter = new XsfTagGetter(pSource.GetXsfByTag);
            setter = new XsfTagSetter(pDestination.SetXsfByTag);
            CopyTag(pXsfTagCopyStruct.UpdateXsfByTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);

            getter = new XsfTagGetter(pSource.GetVolumeTag);
            setter = new XsfTagSetter(pDestination.SetVolumeTag);
            CopyTag(pXsfTagCopyStruct.UpdateVolumeTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);
            
            getter = new XsfTagGetter(pSource.GetLengthTag);
            setter = new XsfTagSetter(pDestination.SetLengthTag);
            CopyTag(pXsfTagCopyStruct.UpdateLengthTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);

            getter = new XsfTagGetter(pSource.GetFadeTag);
            setter = new XsfTagSetter(pDestination.SetFadeTag);
            CopyTag(pXsfTagCopyStruct.UpdateFadeTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);            

            getter = new XsfTagGetter(pSource.GetSystemTag);
            setter = new XsfTagSetter(pDestination.SetSystemTag);
            CopyTag(pXsfTagCopyStruct.UpdateSystemTag, getter, setter, pXsfTagCopyStruct.CopyEmptyTags);

            pDestination.UpdateTags();
        }

        private static void CopyTag(bool pDoCopy, XsfTagGetter pXsfTagGetter,
            XsfTagSetter pXsfTagSetter, bool pCopyEmptyTags)
        {                        
            if (pDoCopy)
            {
                string tagData = pXsfTagGetter();

                if ((pCopyEmptyTags) || 
                    ((!pCopyEmptyTags) && (!String.IsNullOrEmpty(tagData))))
                {
                    pXsfTagSetter(tagData, true);
                }
            }
        }

        // PSF2
        public static Ps2SequenceData.Ps2SqTimingStruct GetTimeForPsf2File(string pSqPath, int pSequenceId)
        {
            Ps2SequenceData.Ps2SqTimingStruct time = new Ps2SequenceData.Ps2SqTimingStruct();

            try
            {
                using (FileStream fs = File.Open(pSqPath, FileMode.Open, FileAccess.Read))
                {                    
                    Ps2SequenceData ps2SequenceData = new Ps2SequenceData(fs);

                    time = ps2SequenceData.getTimeInSecondsForSequenceNumber(fs, pSequenceId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

            return time;
        }

        public static string UnpackPsf2(string pPath)
        { 
            return UnpackPsf2(pPath, null);
        }

        public static string UnpackPsf2(string pPath, string pDestinationFolder)
        {
            string outputDir = null;
            string formatString = GetXsfFormatString(pPath);

            if (!String.IsNullOrEmpty(formatString) && formatString.Equals(Xsf.FormatNamePsf2))
            {
                Psf2 vgmData = new Psf2();
                
                using (FileStream fs = File.OpenRead(pPath))
                {
                    vgmData.Initialize(fs, pPath);                    
                }

                // prepare output folder
                if (String.IsNullOrEmpty(pDestinationFolder))
                {
                    outputDir = Path.Combine(Path.GetDirectoryName(pPath),
                        Path.GetFileNameWithoutExtension(pPath));
                }
                else
                {
                    outputDir = pDestinationFolder;
                }

                vgmData.Unpack(outputDir);
            }
            return outputDir;
        }

        // PSF        
        public static string ExtractPsxSequenceForTiming(string pPath, bool forceSep, string sepSeqIndexOffset,
            string sepSeqIndexParameterLength, int sepSeqIndexFromMinipsf)
        {
            string tempOutputPath = null;
            string outputPath = null;

            string sepOutputPath = null;
            string fileToExtractFrom = null;
            string decompressedDataSection = null;

            int sepSeqIndex;
            string[] libFiles = new string[0];

            // extract PSF data section if needed
            using (FileStream typeFs = File.Open(pPath, FileMode.Open, FileAccess.Read))
            {
                Type dataType = FormatUtil.getObjectType(typeFs);

                if (dataType != null && dataType.Name.Equals("Xsf"))
                {
                    Xsf xsfFile = new Xsf();
                    xsfFile.Initialize(typeFs, pPath);
                    libFiles = xsfFile.GetLibPathArray();

                    if (xsfFile.GetFormat().Equals(Xsf.FormatNamePsf))
                    {
                        typeFs.Close();
                        typeFs.Dispose();

                        Xsf2ExeStruct xsf2ExeStruct = new Xsf2ExeStruct();
                        xsf2ExeStruct.IncludeExtension = true;
                        xsf2ExeStruct.StripGsfHeader = false;
                        decompressedDataSection = ExtractCompressedDataSection(pPath, xsf2ExeStruct);
                        fileToExtractFrom = decompressedDataSection;
                    }
                }
                else
                {
                    fileToExtractFrom = pPath;
                }
            }

            using (FileStream fs = File.Open(fileToExtractFrom, FileMode.Open, FileAccess.Read))
            {
                // build output path
                tempOutputPath = Path.Combine(Path.GetDirectoryName(pPath), Path.ChangeExtension(pPath, PsxSequence.FILE_EXTENSION_SEQ));

                if (tempOutputPath.Equals(fileToExtractFrom))
                {
                    tempOutputPath = Path.Combine(Path.GetDirectoryName(pPath),
                        (Path.GetFileNameWithoutExtension(pPath) + "extract" + PsxSequence.FILE_EXTENSION_SEQ));
                }

                //extract the SEQ we need
                try
                {
                    // check for SEQ first
                    if (!ExtractPsxSeq(fs, tempOutputPath))
                    {
                        if ((!forceSep) ||
                            (String.IsNullOrEmpty(sepSeqIndexOffset.Trim())))
                        {
                            throw new PsxSeqFormatException("No SEQ Data found, and SEP inputs are not set.  Did you forget to add the SEP parameters in the options?");
                        }
                        else
                        {
                            //try for SEP
                            sepOutputPath = Path.ChangeExtension(tempOutputPath, PsxSequence.FILE_EXTENSION_SEP);

                            if (sepSeqIndexFromMinipsf == -1)
                            {
                                sepSeqIndex = GetSepSeqIndex(fs, sepSeqIndexOffset, sepSeqIndexParameterLength);
                            }
                            else
                            {
                                sepSeqIndex = sepSeqIndexFromMinipsf;
                            }

                            if (sepSeqIndex > -1)
                            {
                                if (ExtractPsxSep(fs, sepOutputPath))
                                {
                                    tempOutputPath = PsxSequence.ExtractSeqFromSep(sepOutputPath, (uint)sepSeqIndex);
                                    File.Delete(sepOutputPath);  // delete extracted SEP
                                }
                                else
                                {
                                    // minipsf pointing to SEP
                                    foreach (string lib in libFiles)
                                    {
                                        tempOutputPath = ExtractPsxSequenceForTiming(lib, forceSep, sepSeqIndexOffset,
                                            sepSeqIndexParameterLength, sepSeqIndex);

                                        if (!String.IsNullOrEmpty(tempOutputPath))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new PsxSeqFormatException("SEQ Index cannot be found for input SEP parameters.");
                            }
                        }                       
                    }
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                    
                    if (!String.IsNullOrEmpty(decompressedDataSection))
                    {
                        File.Delete(decompressedDataSection);
                    }

                    if (File.Exists(tempOutputPath))
                    {
                        outputPath = tempOutputPath;
                    }
                }
            }

            return outputPath;
        }

        private static bool ExtractPsxSeq(Stream data, string outputPath)
        {
            bool ret = false;
            
            long offsetLocation = -1;
            long seqEndLocation = -1;
            long seqEndLocationType2 = -1;
            long seqEndLocationType3 = -1;

            if ((offsetLocation = ParseFile.GetNextOffset(data, 0, PsxSequence.ASCII_SIGNATURE_SEQ)) > -1)
            {
                seqEndLocation = ParseFile.GetNextOffset(data, offsetLocation, PsxSequence.END_SEQUENCE);
                seqEndLocationType2 = ParseFile.GetNextOffset(data, offsetLocation, PsxSequence.END_SEQUENCE_TYPE2);
                seqEndLocationType3 = ParseFile.GetNextOffset(data, offsetLocation, PsxSequence.END_SEQUENCE_TYPE2);

                if  (((seqEndLocation == -1) && (seqEndLocationType2 > -1)) ||
                    ((seqEndLocationType2 != -1) && (seqEndLocationType2 < seqEndLocation)))
                {
                    seqEndLocation = seqEndLocationType2 + PsxSequence.END_SEQUENCE_TYPE2.Length;
                }
                else if (((seqEndLocation == -1) && (seqEndLocationType3 > -1)) ||
                    ((seqEndLocationType3 != -1) && (seqEndLocationType3 < seqEndLocation)))
                {
                    seqEndLocation = seqEndLocationType3 + PsxSequence.END_SEQUENCE_TYPE3.Length;
                }


                if (seqEndLocation > -1)
                {
                    seqEndLocation += PsxSequence.END_SEQUENCE.Length;  // add length to include the end bytes

                    // extract SEQ                    
                    ParseFile.ExtractChunkToFile(data, offsetLocation, (int)(seqEndLocation - offsetLocation),
                        outputPath);
                    ret = true;
                }
                else
                {                                        
                    throw new PsxSeqFormatException("SEQ begin found, but terminator bytes were not found.");
                }
            }

            return ret;
        }

        private static bool ExtractPsxSep(Stream data, string outputPath)
        {
            bool ret = false;

            long offsetLocation = -1;
            long sepEndLocation = -1;
            long sepStartingOffset = -1;
            
            int sepSeqCount;
            long seqEof;

            byte[] nextSepCheckBytes;

            if ((offsetLocation = ParseFile.GetNextOffset(data, 0, PsxSequence.ASCII_SIGNATURE_SEP)) > -1)
            {
                sepStartingOffset = offsetLocation;
                sepSeqCount = 1;
                seqEof = offsetLocation;

                while ((seqEof = ParseFile.GetNextOffset(data, seqEof, PsxSequence.END_SEQUENCE)) > -1)
                {
                    nextSepCheckBytes = ParseFile.ParseSimpleOffset(data, (long)(seqEof + PsxSequence.END_SEQUENCE.Length), 2);

                    if (nextSepCheckBytes.Length > 0)
                    {
                        Array.Reverse(nextSepCheckBytes);

                        if (BitConverter.ToUInt16(nextSepCheckBytes, 0) == (UInt16)sepSeqCount)
                        {
                            sepSeqCount++;
                        }
                        else
                        {
                            sepEndLocation = seqEof + PsxSequence.END_SEQUENCE.Length;
                            break;
                        }
                    }
                    else
                    {
                        sepEndLocation = seqEof + PsxSequence.END_SEQUENCE.Length;
                        break;
                    }

                    seqEof += 1;                    
                }

                // if we found the end
                if (sepEndLocation > -1)
                {
                    sepEndLocation += PsxSequence.END_SEQUENCE.Length;  // add length to include the end bytes

                    // extract SEP                    
                    ParseFile.ExtractChunkToFile(data, offsetLocation, (int)(sepEndLocation - sepStartingOffset),
                        outputPath);
                    ret = true;
                }
                else
                {
                    throw new PsxSeqFormatException("SEP begin found, but terminator bytes were not found.");
                }
            }

            return ret;
        }

        private static int GetSepSeqIndex(Stream data, string seqIndexOffset, string seqIndexLength)
        {
            int ret = -1;
            
            byte[] textSectionOffset;
            long textSectionOffsetValue;

            long pcOffsetSepSeqIndex;
            int seqOffsetLength;

            byte[] indexValueBytes;
            uint tempIndexValue;

            // get text section offset
            textSectionOffset = ParseFile.ParseSimpleOffset(data, 0x18, 4);
            textSectionOffsetValue = BitConverter.ToUInt32(textSectionOffset, 0);

            // get offset of index
            pcOffsetSepSeqIndex = VGMToolbox.util.ByteConversion.GetLongValueFromString(seqIndexOffset) -
                textSectionOffsetValue + Psf.PC_OFFSET_CORRECTION;

            // get index value
            seqOffsetLength = (int)VGMToolbox.util.ByteConversion.GetLongValueFromString(seqIndexLength);
            indexValueBytes = ParseFile.ParseSimpleOffset(data, pcOffsetSepSeqIndex, seqOffsetLength);

            switch (seqOffsetLength)
            { 
                case 1:
                    tempIndexValue = (uint)indexValueBytes[0];
                    break;
                case 2:
                    tempIndexValue = (uint)BitConverter.ToUInt16(indexValueBytes, 0);
                    break;
                case 4:
                    tempIndexValue = BitConverter.ToUInt32(indexValueBytes, 0);
                    break;
                default:
                    throw new PsxSeqFormatException("Invalid SEQ index parameter entered");
            }

            ret = (int)tempIndexValue;

            return ret;        
        }

        public static PsfPsyQAddresses GetSigFindItems(Stream sigFindOutputStream, bool relaxLoadAddressRestriction)
        {
            return GetSigFindItems(sigFindOutputStream, relaxLoadAddressRestriction, null);
        }
        
        public static PsfPsyQAddresses GetSigFindItems(Stream sigFindOutputStream, bool relaxLoadAddressRestriction, PsfPsyQAddresses addressesToUpdate)
        {
            PsfPsyQAddresses ret;

            if (addressesToUpdate == null)
            {
                ret = new PsfPsyQAddresses();
            }
            else
            {
                ret = addressesToUpdate;
            }
            
            string inputLine;
            string[] splitInput;
            string firstChunk;
            char[] splitDelimeters = new char[] { ' ' };
            ArrayList psyQFunctions;
            string addressValue;
            PropertyInfo psyQValue;

            psyQFunctions = XsfUtil.GetPsyQFunctionList();

            using (StreamReader logFileReader = new StreamReader(sigFindOutputStream))
            {
                while ((inputLine = logFileReader.ReadLine()) != null)
                {
                    if ((inputLine.Contains("Text Start =")) && (addressesToUpdate == null))
                    {
                        ret.PsfDrvLoadAddress = XsfUtil.getSigFindAddress(inputLine, true);

                        if ((!relaxLoadAddressRestriction) && 
                            ((uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(ret.PsfDrvLoadAddress) > PSFDRV_LOAD_ADDRESS))
                        {
                            throw new ArgumentOutOfRangeException("PSFDRV_LOAD", ret.PsfDrvLoadAddress, String.Format("Text Area + Size is greater than 0x{0}, this is not supported.", PSFDRV_LOAD_ADDRESS.ToString("X8")));
                        }
                        else
                        {
                            ret.PsfDrvLoadAddress = String.Format("0x{0}", PSFDRV_LOAD_ADDRESS.ToString("X8"));
                        }
                    }
                    else
                    {
                        splitInput = inputLine.Split(splitDelimeters, StringSplitOptions.RemoveEmptyEntries);

                        if (splitInput.Length > 0)
                        {
                            firstChunk = splitInput[0];

                            if (psyQFunctions.Contains(firstChunk))
                            {
                                addressValue = XsfUtil.getSigFindAddress(inputLine, false);
                                psyQValue = ret.GetType().GetProperty(firstChunk);

                                if ((addressesToUpdate == null) || (psyQValue.GetValue(ret, null) == null))
                                {
                                    psyQValue.SetValue(ret, addressValue, null);
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private static string getSigFindAddress(string inputLine, bool addValues)
        {
            string ret = null;
            int firstHexSpecifier = -1;
            int secondHexSpecifier = -1;
            uint firstHexValue;
            uint secondHexValue;

            firstHexSpecifier = inputLine.IndexOf("0x");
            secondHexSpecifier = inputLine.LastIndexOf("0x");

            if (addValues)
            {
                if ((firstHexSpecifier != -1) && (secondHexSpecifier != -1))
                {
                    firstHexValue = (uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(inputLine.Substring(firstHexSpecifier, 10));
                    secondHexValue = (uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(inputLine.Substring(secondHexSpecifier, 10));

                    ret = String.Format("0x{0}", (firstHexValue + secondHexValue).ToString("X8"));
                }
            }
            else
            {
                ret = inputLine.Substring(firstHexSpecifier, 10);
            }

            return ret;
        }

        public static ArrayList GetPsyQFunctionList()
        { 
            ArrayList psyQFunctionList = new ArrayList();    
            
            psyQFunctionList.Add("ResetCallback");
            psyQFunctionList.Add("SsInit");
            psyQFunctionList.Add("SsSeqOpen");
            psyQFunctionList.Add("SsSeqPlay");
            psyQFunctionList.Add("SsSetMVol");
            psyQFunctionList.Add("SsStart");
            psyQFunctionList.Add("SsSetTableSize");
            psyQFunctionList.Add("SsSetTickMode");
            psyQFunctionList.Add("SsSeqSetVol");
            psyQFunctionList.Add("SsUtSetReverbType");
            psyQFunctionList.Add("SsUtReverbOn");
            psyQFunctionList.Add("SsVabOpenHead");
            psyQFunctionList.Add("SsVabTransBodyPartly");
            psyQFunctionList.Add("SsVabTransCompleted");
            psyQFunctionList.Add("SpuSetReverb");
            psyQFunctionList.Add("SpuSetReverbModeParam");
            psyQFunctionList.Add("SpuSetReverbDepth");
            psyQFunctionList.Add("SpuSetReverbVoice");

            // alternatives
            psyQFunctionList.Add("SsVabOpenHeadSticky");
            psyQFunctionList.Add("SsVabTransBody");
            psyQFunctionList.Add("SpuIsTransferCompleted");
            psyQFunctionList.Add("SpuInit");
            psyQFunctionList.Add("SsStart2");

            // SEP
            psyQFunctionList.Add("SsSepOpen");
            psyQFunctionList.Add("SsSepPlay");

            return psyQFunctionList;
        }

        public static Dictionary<string, string> getPsyQSourceCodeList()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("PsfDrvLoadAddress", "#define PSFDRV_LOAD       ({0})");
            list.Add("DriverTextString", "  (int)\"{0}\",");
            list.Add("ExeFileNameCrc", "{0}"); // "  (int)\"{0}\", {1}," used in PsfStubMakerWorker
            list.Add("JumpPatchAddress", "  {0},");

            list.Add("ResetCallback", "  #define ResetCallback                          F0({0})");
            list.Add("SsInit", "  #define SsInit                                 F0({0})");
            list.Add("SsSeqOpen", "  #define SsSeqOpen(a,b)               ((short)( F2({0}) ((int)(a),(int)(b)) ))");
            list.Add("SsSeqPlay", "  #define SsSeqPlay(a,b,c)                       F3({0}) ((int)(a),(int)(b),(int)(c))");
            list.Add("SsSetMVol", "  #define SsSetMVol(a,b)                         F2({0}) ((int)(a),(int)(b))");
            list.Add("SsStart", "  #define SsStart                                F0({0})");
            list.Add("SsSetTableSize", "  #define SsSetTableSize(a,b,c)                  F3({0}) ((int)(a),(int)(b),(int)(c))");
            list.Add("SsSetTickMode", "  #define SsSetTickMode(a)                       F1({0}) ((int)(a))");
            list.Add("SsSeqSetVol", "  #define SsSeqSetVol(a,b,c)                     F3({0}) ((int)(a),(int)(b),(int)(c))");
            list.Add("SsUtSetReverbType", "  #define SsUtSetReverbType(a)         ((short)( F1({0}) ((int)(a)) ))");
            list.Add("SsUtReverbOn", "  #define SsUtReverbOn                           F0({0})");
            list.Add("SsVabOpenHead", "  #define SsVabOpenHead(a,b)           ((short)( F2({0}) ((int)(a),(int)(b)) ))");
            list.Add("SsVabTransBodyPartly", "  #define SsVabTransBodyPartly(a,b,c)  ((short)( F3({0}) ((int)(a),(int)(b),(int)(c)) ))");
            list.Add("SsVabTransCompleted", "  #define SsVabTransCompleted(a)       ((short)( F1({0}) ((int)(a)) ))");

            list.Add("SpuSetReverb", "  #define SpuSetReverb(a)                        F1({0}) ((int)(a))");
            list.Add("SpuSetReverbModeParam", "  #define SpuSetReverbModeParam(a)               F1({0}) ((int)(a))");
            list.Add("SpuSetReverbDepth", "  #define SpuSetReverbDepth(a)                   F1({0}) ((int)(a))");
            list.Add("SpuSetReverbVoice", "  #define SpuSetReverbVoice(a,b)                 F2({0}) ((int)(a),(int)(b))");

            // alternatives
            list.Add("SsVabOpenHeadSticky", "  #define SsVabOpenHeadSticky(a,b,c)   ((short)( F3({0}) ((int)(a),(int)(b),(int)(c)) ))");
            list.Add("SsVabTransBody", "  #define SsVabTransBody(a,b)          ((short)( F2({0}) ((int)(a),(int)(b)) ))");
            list.Add("SpuIsTransferCompleted", "  #define SpuIsTransferCompleted(a)     ((short)( F5({0}) ((long)(a)) ))");
            list.Add("SpuInit", "  #define SpuInit                                F0({0})");
            list.Add("SsStart2", "  #define SsStart2                               F0({0})");

            // SEP
            list.Add("SsSepOpen", "  #define SsSepOpen(a,b,c)             ((short)( F3({0}) ((int)(a),(int)(b),(int)(c)) ))");
            list.Add("SsSepPlay", "  #define SsSepPlay(a,b,c,d)                     F4({0}) ((int)(a),(int)(b),(int)(c),(int)(d))");

            return list;
        }

        public static Dictionary<int, string> getPsyQSourceCodeLineNumberList()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();

            list.Add(18, "PsfDrvLoadAddress");
            list.Add(101, "DriverTextString");
            list.Add(112, "ExeFileNameCrc");
            list.Add(118, "JumpPatchAddress");

            list.Add(193, "ResetCallback");            
            list.Add(195, "SsInit");
            list.Add(196, "SsSeqOpen");
            list.Add(197, "SsSeqPlay");
            list.Add(198, "SsSetMVol");
            list.Add(199, "SsStart");
            list.Add(200, "SsSetTableSize");
            list.Add(201, "SsSetTickMode");
            list.Add(202, "SsSeqSetVol");
            list.Add(203, "SsUtSetReverbType");
            list.Add(204, "SsUtReverbOn");
            list.Add(205, "SsVabOpenHead");
            list.Add(206, "SsVabTransBodyPartly");
            list.Add(207, "SsVabTransCompleted");
            
            list.Add(209, "SpuSetReverb");
            list.Add(210, "SpuSetReverbModeParam");
            list.Add(211, "SpuSetReverbDepth");
            list.Add(212, "SpuSetReverbVoice");

            // alternatives
            list.Add(215, "SsVabOpenHeadSticky");
            list.Add(216, "SsVabTransBody");
            list.Add(217, "SpuIsTransferCompleted");
            list.Add(218, "SpuInit");
            list.Add(219, "SsStart2");

            // SEP
            list.Add(223, "SsSepOpen");
            list.Add(224, "SsSepPlay");

            return list;
        }

        public static bool IsPsyQSdkPresent()
        {
            string[] files;
            bool isCcpsxPresent = false;
            bool isPsyLinkPresent = false;

            string pathVariable = Environment.GetEnvironmentVariable("PATH");
            string[] paths = pathVariable.Split(new char[] { ';' });

            foreach (string p in paths)
            {
                if (Directory.Exists(p))
                {
                    // CCPSX.EXE
                    files = Directory.GetFiles(p, "CCPSX.EXE", SearchOption.TopDirectoryOnly);

                    if (files.Length > 0)
                    {
                        isCcpsxPresent = true;
                    }

                    // PSYLINK.EXE
                    files = Directory.GetFiles(p, "PSYLINK.EXE", SearchOption.TopDirectoryOnly);

                    if (files.Length > 0)
                    {
                        isPsyLinkPresent = true;
                    }
                }
            }

            return (isCcpsxPresent && isPsyLinkPresent);        
        }

        public static bool IsPsyQPathVariablePresent()
        {
            bool ret = false;
            string pathVariable = Environment.GetEnvironmentVariable("PSYQ_PATH");

            if (!String.IsNullOrEmpty(pathVariable))
            {
                ret = true;
            }

            return ret;
        }

        public static bool IsPsxExe(string pPath)
        { 
            bool ret = false;
            byte[] checkArray;

            using (FileStream fs = File.OpenRead(pPath))
            {
                checkArray = ParseFile.ParseSimpleOffset(fs, 0, XsfUtil.PlayStationExecutableSignature.Length);
                ret = ParseFile.CompareSegment(checkArray, 0, XsfUtil.PlayStationExecutableSignature);
            }
                        
            return ret;
        }

        public static void SplitVab(string sourceFilePath, bool outputLogFile, bool outputBatchFile)
        {
            using (FileStream fs = File.OpenRead(sourceFilePath))
            { 
                byte[] checkBytes;
                UInt16 programCount;
                int vabHeaderSize;
                checkBytes = ParseFile.ParseSimpleOffset(fs, 0, 4);

                if (ParseFile.CompareSegment(checkBytes, 0, XsfUtil.VAB_SIGNATURE))
                {
                    programCount = 
                        BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(fs, 0x12, 2), 0);
                    vabHeaderSize = 2592 + (512 * programCount);

                    ParseFile.ExtractChunkToFile(fs, 0, vabHeaderSize, Path.ChangeExtension(sourceFilePath, ".vh"), outputLogFile, outputBatchFile);
                    ParseFile.ExtractChunkToFile(fs, (long)vabHeaderSize, (int)(fs.Length - (long)vabHeaderSize), Path.ChangeExtension(sourceFilePath, ".vb"), outputLogFile, outputBatchFile);
                }
            }            
        }

        // 2SF
        public static int GetSongNumberForMini2sf(string pPath)
        {
            int ret = InvalidData;
            
            string formatString = GetXsfFormatString(pPath);
            Xsf2ExeStruct xsf2ExeStruct;
            string decompressedDataSectionPath;

            if (!String.IsNullOrEmpty(formatString) && formatString.Equals(Xsf.FormatName2sf))
            {
                // extract data section
                xsf2ExeStruct = new Xsf2ExeStruct();
                xsf2ExeStruct.IncludeExtension = true;
                xsf2ExeStruct.StripGsfHeader = false;
                decompressedDataSectionPath = ExtractCompressedDataSection(pPath, xsf2ExeStruct);

                if (!String.IsNullOrEmpty(decompressedDataSectionPath))
                {
                    using (FileStream fs = File.OpenRead(decompressedDataSectionPath))
                    {
                        if (fs.Length == 0x0A) // make sure it is a mini2sf
                        {
                            ret = (int)BitConverter.ToUInt16(ParseFile.ParseSimpleOffset(fs, 8, 2), 0);
                        }
                    }

                    File.Delete(decompressedDataSectionPath);                
                }                
            }

            return ret;
        }

        public static int GetSongNumberForYoshiIslandMini2sf(string pPath)
        {
            int ret = InvalidData;

            string formatString = GetXsfFormatString(pPath);
            Xsf2ExeStruct xsf2ExeStruct;
            string decompressedReservedSectionPath;
            string decompressedSaveStatePath;

            if (!String.IsNullOrEmpty(formatString) && formatString.Equals(Xsf.FormatName2sf))
            {
                // extract reserved section
                xsf2ExeStruct = new Xsf2ExeStruct();
                xsf2ExeStruct.IncludeExtension = true;
                xsf2ExeStruct.StripGsfHeader = false;
                decompressedReservedSectionPath = ExtractReservedSection(pPath, xsf2ExeStruct);

                if (!String.IsNullOrEmpty(decompressedReservedSectionPath))
                {
                    using (FileStream fs = File.OpenRead(decompressedReservedSectionPath))
                    {
                        byte[] savestateCheckbytes = new byte[4];
                        fs.Read(savestateCheckbytes, 0, 4);

                        // check for SAVE bytes to indicate this is a V1 set.
                        if (ParseFile.CompareSegment(savestateCheckbytes, 0, MINI2SF_SAVESTATE_ID))
                        {                            
                            decompressedSaveStatePath =
                                Path.ChangeExtension(decompressedReservedSectionPath, CompressionUtil.ZlibDecompressOutputExtension);

                            // decompress save state
                            CompressionUtil.DecompressZlibStreamToFile(fs, decompressedSaveStatePath, 0x0C);

                            using (FileStream ssFs = File.OpenRead(decompressedSaveStatePath))
                            {
                                if (ssFs.Length == 0x0C) // verify save state length
                                {
                                    ret = (int)BitConverter.ToUInt32(ParseFile.ParseSimpleOffset(ssFs, 8, 4), 0);
                                }
                            }
                            File.Delete(decompressedSaveStatePath);
                        }
                    }
                    File.Delete(decompressedReservedSectionPath);
                }
            }

            return ret;
        }

        public static void Time2sfFolder(Time2sfStruct pTime2sfStruct, out string pMessages)
        {
            Hashtable mini2sfSongNumbers = new Hashtable();
            string emptyFolderFileName;
            bool processSuccess;
            
            pMessages = String.Empty;

            // Verify paths
            if (!Directory.Exists(pTime2sfStruct.Mini2sfDirectory))
            {
                throw new DirectoryNotFoundException(String.Format("Cannot find directory <{0}>", pTime2sfStruct.Mini2sfDirectory));
            }
            
            if (!File.Exists(pTime2sfStruct.SdatPath))
            {
                throw new FileNotFoundException(String.Format("Cannot find file <{0}>", pTime2sfStruct.SdatPath));
            }

            if (Sdat.IsSdat(pTime2sfStruct.SdatPath))
            {

                // delete existing batch file
                string psfpointBatchFilePath = Path.Combine(Path.Combine(pTime2sfStruct.Mini2sfDirectory, "text"), PSFPOINT_BATCH_TXT);

                if (File.Exists(psfpointBatchFilePath))
                {
                    File.Delete(psfpointBatchFilePath);
                }

                // extract SDAT
                string extractedSdatPath = Path.Combine(Path.GetDirectoryName(pTime2sfStruct.SdatPath), Path.GetFileNameWithoutExtension(pTime2sfStruct.SdatPath));
                if (Directory.Exists(extractedSdatPath))
                {
                    extractedSdatPath += String.Format("_temp_{0}", new Random().Next().ToString());
                }

                string extractedSseqPath = Path.Combine(extractedSdatPath, "Seq");

                Sdat sdat = new Sdat();
                using (FileStream fs = File.Open(pTime2sfStruct.SdatPath, FileMode.Open, FileAccess.Read))
                {
                    sdat.Initialize(fs, pTime2sfStruct.SdatPath);
                    sdat.ExtractSseqs(fs, extractedSdatPath);
                }

                // Make SMAP
                Smap smap = new Smap(sdat);

                // Build Hashtable for mini2sfs
                int songNumber;
                foreach (string mini2sfFile in Directory.GetFiles(pTime2sfStruct.Mini2sfDirectory, "*.mini2sf"))
                {
                    songNumber = GetSongNumberForMini2sf(mini2sfFile);
                    
                    if (!mini2sfSongNumbers.ContainsKey(songNumber))
                    {
                        mini2sfSongNumbers.Add(songNumber, mini2sfFile);
                    }
                }

                // Loop through SMAP and build timing script
                string emptyFileDir = Path.Combine(pTime2sfStruct.Mini2sfDirectory, EMPTY_FILE_DIRECTORY);

                int totalSequences = smap.SseqSection.Length;
                int i = 1;

                // Initialize Bass            
                if (smap.SseqSection.Length > 0)
                {
                    Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero, null);
                }

                string rippedFileName;
                string rippedFilePath;
                string outputMessages;
                string sseqFilePath;

                foreach (Smap.SmapSeqStruct s in smap.SseqSection)
                {
                    if (mini2sfSongNumbers.ContainsKey(s.number))
                    {
                        rippedFileName = Path.GetFileName((string)mini2sfSongNumbers[s.number]);
                        rippedFilePath = (string)mini2sfSongNumbers[s.number];

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
                            sseqFilePath = Path.Combine(extractedSseqPath, s.name);
                            
                            // convert sseq file to midi
                            processSuccess = convertSseqFile(SSEQ2MID_SOURCE_PATH, pTime2sfStruct.Mini2sfDirectory,
                                sseqFilePath, pTime2sfStruct.DoSingleLoop, out outputMessages);
                            pMessages += outputMessages;

                            // time file
                            if (processSuccess)
                            {
                                processSuccess = buildFileTimingBatch(pTime2sfStruct.Mini2sfDirectory,
                                    rippedFilePath, sseqFilePath, out outputMessages);
                                pMessages += outputMessages;
                            }
                        }                                        
                    }
                    i++;
                }
                
                // copy script
                string psfpointBatchFileDestinationPath = Path.Combine(pTime2sfStruct.Mini2sfDirectory, PSFPOINT_BATCH_TXT);
                File.Copy(psfpointBatchFilePath, psfpointBatchFileDestinationPath, true);

                // run timing script
                ExecutePsfPointBatchScript(psfpointBatchFileDestinationPath, true);

                // delete extracted SDAT
                Directory.Delete(extractedSdatPath, true);
            }
            else
            {
                throw new FormatException(String.Format("{0} is not an SDAT file.", pTime2sfStruct.SdatPath));
            } // if (Sdat.IsSdat(pTime2sfStruct.SdatPath))

        }

        private static bool convertSseqFile(string pSseq2MidToolPath, string pMini2sfPath,
            string pSseqFilePath, bool pDoSingleLoop, out string pErrorMessage)
        {
            Process ndsProcess;
            bool isSuccess;
            pErrorMessage = String.Empty;

            // convert existing sseq to mid            
            string sseqPath = Path.GetDirectoryName(pSseqFilePath);
            string sseq2MidDestinationPath = Path.Combine(sseqPath, Path.GetFileName(pSseq2MidToolPath));

            try
            {
                if (!File.Exists(sseq2MidDestinationPath))
                {
                    File.Copy(pSseq2MidToolPath, sseq2MidDestinationPath, false);
                }
                
                // pDoSingleLoop
                string arguments;

                if (pDoSingleLoop)
                {
                    arguments = String.Format(" -1 -l {0}", Path.GetFileName(pSseqFilePath));
                }
                else
                {
                    arguments = String.Format(" -2 -l {0}", Path.GetFileName(pSseqFilePath));
                }

                ndsProcess = new Process();

                ndsProcess.StartInfo = new ProcessStartInfo(sseq2MidDestinationPath, arguments);
                ndsProcess.StartInfo.WorkingDirectory = sseqPath;
                ndsProcess.StartInfo.UseShellExecute = false;
                ndsProcess.StartInfo.CreateNoWindow = true;
                ndsProcess.StartInfo.RedirectStandardOutput = true;
                isSuccess = ndsProcess.Start();
                string sseqOutputFile = ndsProcess.StandardOutput.ReadToEnd();
                ndsProcess.WaitForExit();
                ndsProcess.Close();
                ndsProcess.Dispose();

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
                pErrorMessage = _e.Message + Environment.NewLine;
            }

            return isSuccess;
        }

        private static bool buildFileTimingBatch(string pMini2sfPath,
            string p2sfFilePath, string pSseqFilePath, out string pErrorMessage)
        {
            bool isSuccess = false;
            string arguments;
            pErrorMessage = String.Empty;

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
                        pErrorMessage = Environment.NewLine + String.Format("Stream error: {0}", Bass.BASS_ErrorGetCode()) +
                            Environment.NewLine;                       
                        return false;
                    }


                    // Do Fade
                    if (isLoopingTrack(pMini2sfPath, Path.GetFileName(pSseqFilePath)))
                    {
                        arguments = " -fade=\"10\" \"" + _2sfFileName + "\"";

                        if (minutes == 0 && seconds == 0)
                        {
                            seconds = 1;
                        }
                    }
                    else
                    {
                        arguments = " -fade=\"1\" \"" + _2sfFileName + "\"";
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
                    arguments = " -length=\"" + minutes + ":" + seconds.ToString().PadLeft(2, '0') + "\" \"" + _2sfFileName + "\"";
                    sw.WriteLine("psfpoint.exe" + arguments);
                }

                isSuccess = true;
            }
            catch (Exception e)
            {
                pErrorMessage = String.Format("Error timing {0}: {1}", p2sfFilePath, e.Message) + Environment.NewLine;
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }

            return isSuccess;
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

        public static void Make2sfSet(string pRomPath, string pSdatPath, 
            int pMinIndex, int pMaxIndex, string pOutputFolder)
        {
            int read;
            byte[] data = new byte[Constants.FileReadChunkSize];
            
            string sdatPrefix = Path.GetFileNameWithoutExtension(pSdatPath);
            string libOutputPath = Path.Combine(pOutputFolder, sdatPrefix + ".2sflib");
            string compressedDataSectionPath = Path.Combine(pOutputFolder, sdatPrefix + ".data.bin");

            string dataCrc32 = String.Empty;
            UInt32 dataLength = 0;
            long totalRomSize;
            FileInfo fi;

            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            byte[] tagData;

            // write compressed section to temp file
            using (FileStream dataFs = File.Open(compressedDataSectionPath, FileMode.Create, FileAccess.ReadWrite))
            {                                
                using (ZlibStream zs = new ZlibStream(dataFs, Ionic.Zlib.CompressionMode.Compress, CompressionLevel.BestCompression, true))
                {
                    fi = new FileInfo(pRomPath);
                    totalRomSize = fi.Length;
                    fi = new FileInfo(pSdatPath);
                    totalRomSize += fi.Length;

                    zs.Write(new byte[] { 0, 0, 0, 0}, 0, 4);
                    zs.Write(BitConverter.GetBytes((UInt32) totalRomSize), 0, 4);
                    
                    // write rom file
                    using (FileStream romStream = File.OpenRead(pRomPath))
                    {
                        while ((read = romStream.Read(data, 0, data.Length)) > 0)
                        {
                            zs.Write(data, 0, read);
                        }
                    }
                    
                    // write SDAT                                        
                    using (FileStream sdatStream = File.OpenRead(pSdatPath))
                    {
                        while ((read = sdatStream.Read(data, 0, data.Length)) > 0)
                        {
                            zs.Write(data, 0, read);
                        }
                        zs.Flush();                        
                    }
                }

                // output 2sflib file
                dataFs.Seek(0, SeekOrigin.Begin);
                dataLength = (UInt32)dataFs.Length;
                dataCrc32 = ChecksumUtil.GetCrc32OfFullFile(dataFs);

                using (FileStream libStream = File.OpenWrite(libOutputPath))
                {
                    using (BinaryWriter bw = new BinaryWriter(libStream))
                    {
                        bw.Write('P');
                        bw.Write('S');
                        bw.Write('F');
                        bw.Write(new byte[] { 0x24 });
                        bw.Write(BitConverter.GetBytes((UInt32)0));          // reserved size
                        bw.Write(BitConverter.GetBytes(dataLength));         // data length
                        bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString("0x" + dataCrc32)); // data crc32

                        // data
                        dataFs.Seek(0, SeekOrigin.Begin);
                        while ((read = dataFs.Read(data, 0, data.Length)) > 0)
                        {
                            bw.Write(data, 0, read);
                        }
                    }
                }                                
            }

            // delete compressed data section
            if (File.Exists(compressedDataSectionPath))
            {
                File.Delete(compressedDataSectionPath);
            }

            // write .mini2sfs
            string mini2sfFileName;
            int mini2sfCrc32;
            
            byte[] mini2sfData = new byte[MINI2SF_DATA_START.Length + 2];
            Array.ConstrainedCopy(MINI2SF_DATA_START, 0, mini2sfData, 0, MINI2SF_DATA_START.Length);


            for (int i = pMinIndex; i <= pMaxIndex; i++)
            {
                using (MemoryStream mini2sfMs = new MemoryStream(mini2sfData, true))
                {
                    // build data section
                    Array.ConstrainedCopy(BitConverter.GetBytes((UInt16)i), 0, mini2sfData, mini2sfData.Length - 2, 2);

                    // create compressed Stream
                    using (MemoryStream compressedMs = new MemoryStream())
                    {
                        using (ZlibStream zs = new ZlibStream(compressedMs, Ionic.Zlib.CompressionMode.Compress, CompressionLevel.None, true))
                        {
                            zs.Write(mini2sfMs.ToArray(), 0, (int)mini2sfMs.Length);
                            zs.Flush();
                        }

                        // get CRC32
                        CRC32 crc32Calc = new CRC32();
                        mini2sfCrc32 = crc32Calc.GetCrc32(new System.IO.MemoryStream(compressedMs.ToArray()));

                        // write mini2sf to disk
                        mini2sfFileName = Path.Combine(pOutputFolder, String.Format("{0}-{1}.mini2sf", sdatPrefix, i.ToString("x4")));

                        using (FileStream mini2sfFs = File.Open(mini2sfFileName, FileMode.Create, FileAccess.Write))
                        {
                            using (BinaryWriter bw = new BinaryWriter(mini2sfFs))
                            {
                                bw.Write('P');
                                bw.Write('S');
                                bw.Write('F');
                                bw.Write(new byte[] { 0x24 });
                                bw.Write(BitConverter.GetBytes((UInt32)0));                  // reserved size
                                bw.Write(BitConverter.GetBytes((UInt32)compressedMs.Length)); // data length                        
                                bw.Write(BitConverter.GetBytes((UInt32)mini2sfCrc32));       // data crc32    
                                bw.Write(compressedMs.ToArray());                            // data

                                // add [TAG]
                                tagData = enc.GetBytes(Xsf.ASCII_TAG);
                                bw.Write(tagData, 0, tagData.Length);

                                tagData = enc.GetBytes(String.Format("_lib={0}", Path.GetFileName(libOutputPath)));
                                bw.Write(tagData, 0, tagData.Length);
                                bw.Write(0x0A);

                            }
                        }
                    }
                }
            }    
        }

        public static void Make2sfSet(string pRomPath, string pSdatPath,
            int[] allowedSequences, string pOutputFolder, bool useSmapNames)
        {
            int read;
            byte[] data = new byte[Constants.FileReadChunkSize];

            string sdatPrefix = Path.GetFileNameWithoutExtension(pSdatPath);
            string libOutputPath = Path.Combine(pOutputFolder, sdatPrefix + ".2sflib");
            string compressedDataSectionPath = Path.Combine(pOutputFolder, sdatPrefix + ".data.bin");

            string dataCrc32 = String.Empty;
            UInt32 dataLength = 0;
            long totalRomSize;
            FileInfo fi;

            Smap smap = new Smap();

            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            byte[] tagData;

            // write compressed section to temp file
            using (FileStream dataFs = File.Open(compressedDataSectionPath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZlibStream zs = new ZlibStream(dataFs, Ionic.Zlib.CompressionMode.Compress, CompressionLevel.BestCompression, true))
                {
                    fi = new FileInfo(pRomPath);
                    totalRomSize = fi.Length;
                    fi = new FileInfo(pSdatPath);
                    totalRomSize += fi.Length;

                    zs.Write(new byte[] { 0, 0, 0, 0 }, 0, 4);
                    zs.Write(BitConverter.GetBytes((UInt32)totalRomSize), 0, 4);

                    // write rom file
                    using (FileStream romStream = File.OpenRead(pRomPath))
                    {
                        while ((read = romStream.Read(data, 0, data.Length)) > 0)
                        {
                            zs.Write(data, 0, read);
                        }
                    }

                    // write SDAT                                        
                    using (FileStream sdatStream = File.OpenRead(pSdatPath))
                    {                        
                        while ((read = sdatStream.Read(data, 0, data.Length)) > 0)
                        {
                            zs.Write(data, 0, read);
                        }
                        zs.Flush();                        
                    }
                }

                // output 2sflib file
                dataFs.Seek(0, SeekOrigin.Begin);
                dataLength = (UInt32)dataFs.Length;
                dataCrc32 = ChecksumUtil.GetCrc32OfFullFile(dataFs);

                using (FileStream libStream = File.OpenWrite(libOutputPath))
                {
                    using (BinaryWriter bw = new BinaryWriter(libStream))
                    {
                        bw.Write('P');
                        bw.Write('S');
                        bw.Write('F');
                        bw.Write(new byte[] { 0x24 });
                        bw.Write(BitConverter.GetBytes((UInt32)0));          // reserved size
                        bw.Write(BitConverter.GetBytes(dataLength));         // data length
                        bw.Write((UInt32)VGMToolbox.util.ByteConversion.GetLongValueFromString("0x" + dataCrc32)); // data crc32

                        // data
                        dataFs.Seek(0, SeekOrigin.Begin);
                        while ((read = dataFs.Read(data, 0, data.Length)) > 0)
                        {
                            bw.Write(data, 0, read);
                        }
                    }
                }
            }

            // delete compressed data section
            if (File.Exists(compressedDataSectionPath))
            {
                File.Delete(compressedDataSectionPath);
            }

            // write .mini2sfs
            string mini2sfFileName;
            int mini2sfCrc32;

            byte[] mini2sfData = new byte[MINI2SF_DATA_START.Length + 2];
            Array.ConstrainedCopy(MINI2SF_DATA_START, 0, mini2sfData, 0, MINI2SF_DATA_START.Length);

            // Get SMAP for File Names
            if (useSmapNames)
            {                
                smap = SdatUtil.GetSmapFromSdat(pSdatPath);            
            }

            foreach (int i in allowedSequences)
            {
                using (MemoryStream mini2sfMs = new MemoryStream(mini2sfData, true))
                {
                    // build data section
                    Array.ConstrainedCopy(BitConverter.GetBytes((UInt16)i), 0, mini2sfData, mini2sfData.Length - 2, 2);

                    // create compressed Stream
                    using (MemoryStream compressedMs = new MemoryStream())
                    {
                        using (ZlibStream zs = new ZlibStream(compressedMs, Ionic.Zlib.CompressionMode.Compress, CompressionLevel.None, true))
                        {
                            zs.Write(mini2sfMs.ToArray(), 0, (int)mini2sfMs.Length);
                            zs.Flush();
                        }

                        // get CRC32
                        CRC32 crc32Calc = new CRC32();
                        mini2sfCrc32 = crc32Calc.GetCrc32(new System.IO.MemoryStream(compressedMs.ToArray()));

                        // build file names for mini2sfs
                        if (useSmapNames)
                        {
                            mini2sfFileName = Path.Combine(pOutputFolder, String.Format("{0} - {1}.mini2sf", i.ToString("x4"), smap.SseqSection[i].label));                           
                        }
                        else
                        {                            
                            mini2sfFileName = Path.Combine(pOutputFolder, String.Format("{0}-{1}.mini2sf", sdatPrefix, i.ToString("x4")));
                        }

                        // write mini2sf to disk
                        using (FileStream mini2sfFs = File.Open(mini2sfFileName, FileMode.Create, FileAccess.Write))
                        {
                            using (BinaryWriter bw = new BinaryWriter(mini2sfFs))
                            {
                                bw.Write('P');
                                bw.Write('S');
                                bw.Write('F');
                                bw.Write(new byte[] { 0x24 });
                                bw.Write(BitConverter.GetBytes((UInt32)0));                  // reserved size
                                bw.Write(BitConverter.GetBytes((UInt32)compressedMs.Length)); // data length                        
                                bw.Write(BitConverter.GetBytes((UInt32)mini2sfCrc32));       // data crc32    
                                bw.Write(compressedMs.ToArray());                            // data

                                // add [TAG]
                                tagData = enc.GetBytes(Xsf.ASCII_TAG);
                                bw.Write(tagData, 0, tagData.Length);

                                tagData = enc.GetBytes(String.Format("_lib={0}", Path.GetFileName(libOutputPath)));
                                bw.Write(tagData, 0, tagData.Length);
                                bw.Write((byte)0x0A);

                                tagData = enc.GetBytes(String.Format("2sfby={0}", "VGMToolbox - NDSto2SF"));
                                bw.Write(tagData, 0, tagData.Length);
                                bw.Write((byte)0x0A);
                            }
                        }
                    }
                }
            }
        }

        public static bool NdsTo2sf(string ndsPath, string testPackPath, bool useSmapNames)
        {
            bool filesWereRipped = false;
            
            if (Path.GetExtension(ndsPath).ToUpper().Equals(Nds.FileExtension))
            {                
                Nds ndsFile = new Nds();

                string[] sdatPaths;
                int validSdatCount;
                int currentSdatNumber = 0;

                Sdat sdatToRip;

                string sdatPrefix;
                string ripOutputPath;
                string sdatDestinationPath;
                string testpackDestinationPath;

                string waveArcOutputPath;
                string swavOutputPath;
                Swar swar = new Swar();
                Type dataType;

                ArrayList optimizationList;
                Time2sfStruct timerStruct;
                string outputTimerMessages;

                string topMostFolder;

                // Build Nds object
                using (FileStream ndsStream = File.OpenRead(ndsPath))
                {
                    ndsFile.Initialize(ndsStream, ndsPath);
                }

                // Get the Sdat Paths
                sdatPaths = SdatUtil.ExtractSdatsFromFile(ndsPath, NDSTO2SF_FOLDER_SUFFIX);                

                if (sdatPaths != null)
                {                    
                    // Get count of valid Sdats
                    validSdatCount = GetCountOfValidSdats(sdatPaths);

                    // Loop over the extracted Sdats
                    foreach (string extractedSdatPath in sdatPaths)
                    {
                        // assume rip goes ok, set to false in directory check
                        filesWereRipped = true;

                        // rename sdat file
                        sdatPrefix = GetSdatPrefix(ndsPath, validSdatCount, ndsFile.GameSerial, currentSdatNumber);
                        sdatDestinationPath = Path.Combine(Path.GetDirectoryName(extractedSdatPath), Path.ChangeExtension(sdatPrefix, Sdat.SDAT_FILE_EXTENSION));
                        File.Move(extractedSdatPath, sdatDestinationPath);

                        // build sdat object
                        if (Sdat.TryParse(sdatDestinationPath, out sdatToRip))
                        {
                            ripOutputPath = Path.Combine(
                                Path.GetDirectoryName(sdatDestinationPath),
                                Path.GetFileNameWithoutExtension(sdatDestinationPath));

                            if (!Directory.Exists(ripOutputPath))
                            {
                                Directory.CreateDirectory(ripOutputPath);
                            }

                            using (Stream sdatStream = File.OpenRead(sdatDestinationPath))
                            {
                                // extract Strms
                                sdatToRip.ExtractStrms(sdatStream, ripOutputPath);

                                // extract SWARs => SWAVs
                                waveArcOutputPath = sdatToRip.ExtractWaveArcs(sdatStream, ripOutputPath);

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

                                                SdatUtil.ExtractAndWriteSwavFromSwar(swarFs, swar, swavOutputPath, true);
                                            }
                                        }
                                    }

                                    // copy all SWAV to top level folder
                                    swavOutputPath = Path.Combine(ripOutputPath, "SWAVs from SWARs");

                                    if (!Directory.Exists(swavOutputPath))
                                    {
                                        Directory.CreateDirectory(swavOutputPath);
                                    }

                                    foreach (string f in Directory.GetFiles(waveArcOutputPath, "*" + Swav.FILE_EXTENSION, SearchOption.AllDirectories))
                                    { 
                                        File.Move(f, Path.Combine(swavOutputPath, Path.GetFileName(f)));
                                    }

                                    // delete SWARs
                                    Directory.Delete(waveArcOutputPath, true);
                                }
                            }


                            // build optimization list
                            optimizationList = GetDefaultOptimizationList(sdatDestinationPath);

                            if (optimizationList.Count > 0)
                            {
                                // optimize Sdat
                                sdatToRip.OptimizeForZlib(optimizationList);

                                // Copy testpack.nds
                                testpackDestinationPath = Path.Combine(Path.GetDirectoryName(sdatDestinationPath), Path.GetFileName(testPackPath));
                                File.Copy(testPackPath, testpackDestinationPath, true);

                                // make 2SFs
                                Make2sfSet(
                                    testpackDestinationPath,
                                    sdatDestinationPath,
                                    (int[])optimizationList.ToArray(typeof(int)),
                                    ripOutputPath,
                                    useSmapNames);

                                // delete testpack
                                File.Delete(testpackDestinationPath);

                                // time files
                                timerStruct = new Time2sfStruct();
                                timerStruct.DoSingleLoop = false;
                                timerStruct.Mini2sfDirectory = ripOutputPath;
                                timerStruct.SdatPath = sdatDestinationPath;
                                Time2sfFolder(timerStruct, out outputTimerMessages);

                                // cleanup timer folder
                                if (Directory.Exists(Path.Combine(ripOutputPath, "text")))
                                {
                                    Directory.Delete(Path.Combine(ripOutputPath, "text"), true);
                                }
                            }
                            else
                            {
                                if ((Directory.Exists(ripOutputPath)) &&
                                    (Directory.GetFiles(ripOutputPath, "*.*", SearchOption.AllDirectories).Length == 0))
                                {
                                    Directory.Delete(ripOutputPath, true);
                                }

                            } // if (optimizationList.Count > 0)
                        } // if (Sdat.TryParse(sdatDestinationPath, out sdatToRip))

                        // delete sdat
                        File.Delete(sdatDestinationPath);

                        // delete top most folder if nothing was found
                        topMostFolder = Path.GetDirectoryName(extractedSdatPath);

                        if ((Directory.Exists(topMostFolder)) &&
                            (Directory.GetFiles(topMostFolder, "*.*", SearchOption.AllDirectories).Length == 0))
                        {
                            filesWereRipped = false;
                            Directory.Delete(topMostFolder, true);
                        }

                        // increment naming counter
                        currentSdatNumber++;
                    } // foreach (string extractedSdatPath in sdatPaths)
                } // if (sdatPaths != null)
            }

            return filesWereRipped;
        }

        private static int GetCountOfValidSdats(string[] sdatPaths)
        {
            int ret = 0;
            Sdat temporarySdat;

            foreach (string path in sdatPaths)
            {
                if (Sdat.TryParse(path, out temporarySdat))
                {
                    ret++;
                }
            }

            return ret;
        }

        private static string GetSdatPrefix(string ndsPath, int validSdatCount, string gameSerial, int currentIndexNumber)
        {
            string sdatPrefix = String.Empty;

            if (String.IsNullOrEmpty(gameSerial))
            {
                sdatPrefix = Path.GetFileNameWithoutExtension(ndsPath);
            }
            else
            {
                sdatPrefix = gameSerial;
            }

            if (validSdatCount > 1)
            {
                sdatPrefix = String.Format("{0}_{1}", sdatPrefix, currentIndexNumber.ToString("X2"));
            }

            return sdatPrefix;
        }

        private static ArrayList GetDefaultOptimizationList(string sdatPath)
        {
            ArrayList sequencesToAllow = new ArrayList();
            Smap.SmapSeqStruct s;

            // get Smap
            Smap smap = SdatUtil.GetSmapFromSdat(sdatPath);
            
            // get List of duplicates
            ArrayList duplicatesList = SdatUtil.GetDuplicateSseqsList(sdatPath);
            
            // loop through smap, removing duplicates
            for (int i = 0; i < smap.SseqSection.Length; i++)
            {
                s = smap.SseqSection[i];

                if (!duplicatesList.Contains(i) &&
                    !String.IsNullOrEmpty(s.name))
                {
                    sequencesToAllow.Add(i);
                }                    
            }
            
            return sequencesToAllow;
        }
    }
}
