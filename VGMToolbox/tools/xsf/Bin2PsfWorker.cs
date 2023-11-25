using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.xsf
{
    public enum PsfMakerTask
    { 
        SeqPsf,
        SeqMiniPsf,
        SeqPsfLib,
        SepPsf,
        SepMiniPsf,
        SepMiniPsfWithVhVbLib,
        SepPsfLib,
        SepPsfLibWithVhVbLib,
        SepPsfWithVhVbLib
    }

    public enum PsfDriverNames
    {
        StandardStub,
        MarkGrassGenericV25
    }

    class Bin2PsfWorker : BackgroundWorker, IVgmtBackgroundWorker
    {                
        private static readonly string WORKING_FOLDER =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "working_psf"));
        private static readonly string PROGRAMS_FOLDER =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "external"));
        private static readonly string PSF_PROGRAMS_FOLDER = Path.Combine(PROGRAMS_FOLDER, "psf");
        private static readonly string OUTPUT_FOLDER =
            Path.GetFullPath(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "rips"), "psfs"));

        public const string STUB_BUILDER = "PSF Stub Builder Defaults";
        public const string GENERIC_DRIVER_MGRASS = "Mark Grass Generic Driver v2.5";
        public const string GENERIC_DRIVER_MGRASS_300 = "Mark Grass Generic Driver v3.0.0";
        public const string GENERIC_DRIVER_MGRASS_422 = "Mark Grass Generic Driver v4.2.2";
        public const string GENERIC_DRIVER_DAVIRONICA = "Davironica's Generic Driver v1.0.4";

        public static readonly string MGRASS422_EXE_PATH = Path.Combine(PSF_PROGRAMS_FOLDER, "MG_DRIVER_V422.SEQ.INFINITE.EXE");
        public static readonly string MGRASS300_EXE_PATH = Path.Combine(PSF_PROGRAMS_FOLDER, "MG_DRIVER_V300.SEQ.INFINITE.EXE");
        public static readonly string MGRASS_EXE_PATH = Path.Combine(PSF_PROGRAMS_FOLDER, "MG_DRIVER_V25.SEQ.INFINITE.EXE");
        public static readonly string EZPSF_EXE_PATH = Path.Combine(PSF_PROGRAMS_FOLDER, "EZPSF_V104.EXE");
        public static readonly string GENERIC_MINIPSF_EXE_PATH = Path.Combine(PSF_PROGRAMS_FOLDER, "minipsf.exe");

        public const string PSFLIB_FILE_EXTENSION = ".psflib";

        public const int PARAM_SEQNUM_TICKMODE = 0x0C;
        public const int PARAM_MAXSEQ_LOOPOFF = 0x10;
        public const int PARAM_SEQNUM_OFFSET = 0x14;
        public const int PARAM_MAXSEQ_OFFSET = 0x18;

        private int progress = 0;
        private int fileCount = 0;
        private int maxFiles = 0;
        VGMToolbox.util.ProgressStruct progressStruct;

        public struct Bin2PsfStruct
        {
            public string sourcePath;
            public string exePath;
            public string seqOffset;
            public string SeqSize { set; get; }
            public string vhOffset;
            public string vbOffset;

            public string ParamOffset { set; get; }

            public string outputFolder;
            public bool MakePsfLib { set; get; }
            public string psflibName;

            public bool TryCombinations;
            public string DriverName;

            public bool ForceSepTrackNo { set; get; }
            public int SepTrackNo { set; get; }
        }

        public Bin2PsfWorker()
        {
            progressStruct = new VGMToolbox.util.ProgressStruct();
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        private void makePsfs(Bin2PsfStruct pBin2PsfStruct, DoWorkEventArgs e)
        {
            string[] sqFiles;
            string[] vhFiles;
            string[] sepFiles;
            string[] uniqueSequences;
            uint seqCount;

            if (!CancellationPending)
            {
                // get list of unique files
                sqFiles = this.getUniqueFileNames(pBin2PsfStruct.sourcePath, "*.SEQ");
                vhFiles = this.getUniqueFileNames(pBin2PsfStruct.sourcePath, "*.VH");
                sepFiles = this.getUniqueFileNames(pBin2PsfStruct.sourcePath, "*.SEP");

                uniqueSequences = new string[sqFiles.Length + sepFiles.Length];
                Array.ConstrainedCopy(sqFiles, 0, uniqueSequences, 0, sqFiles.Length);
                Array.ConstrainedCopy(sepFiles, 0, uniqueSequences, sqFiles.Length, sepFiles.Length);

                if (uniqueSequences != null)
                {                    
                    // calculate max file                   
                    if (pBin2PsfStruct.TryCombinations)
                    {
                        foreach (string sequence in uniqueSequences)
                        {
                            if (pBin2PsfStruct.ForceSepTrackNo && PsxSequence.IsSepTypeSequence(sequence))
                            {
                                seqCount = 1;
                            }
                            else
                            {
                                seqCount = PsxSequence.GetSeqCount(sequence);
                            }

                            this.maxFiles += (int)seqCount * vhFiles.Length;
                        }
                    }
                    else
                    {                        
                        foreach (string sequence in uniqueSequences)
                        {
                            if (pBin2PsfStruct.ForceSepTrackNo && PsxSequence.IsSepTypeSequence(sequence))
                            {
                                seqCount = 1;
                            }
                            else
                            {
                                seqCount = PsxSequence.GetSeqCount(sequence);
                            }

                            if (seqCount > 1)
                            {
                                this.maxFiles += (int)seqCount + 1;
                            }
                            else
                            {
                                this.maxFiles += (int)seqCount;
                            }
                        }
                    }
                    
                    // check psflib counts
                    if (pBin2PsfStruct.MakePsfLib)
                    {
                        this.maxFiles++;
                        
                        if (vhFiles.Length > 1)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.ErrorMessage = String.Format("ERROR: More than 1 VH/VB pair detected, please only include 1 VH/VB pair when making .psflib files{0}", Environment.NewLine);
                            ReportProgress(this.progress, this.progressStruct);
                            return;
                        }
                    }                                        
                }
                                
                // build PSFs
                this.buildPsfs(uniqueSequences, vhFiles, pBin2PsfStruct, e);
            }
            else
            {
                e.Cancel = true;
                return;
            }

            return;
        }

        private string[] getUniqueFileNames(string pSourceDirectory, string mask)
        {
            int fileCount = 0;
            int i = 0;
            string[] ret = new string[0];
            
            if (!Directory.Exists(pSourceDirectory))
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = String.Format("ERROR: Directory {0} not found.", pSourceDirectory);
                ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
            }
            else
            {
                fileCount = Directory.GetFiles(pSourceDirectory, mask, SearchOption.TopDirectoryOnly).Length;

                if (fileCount > 0)
                {
                    ret = new string[fileCount];
                }

                foreach (string f in Directory.GetFiles(pSourceDirectory, mask))
                {
                    ret[i] = f;
                    i++;
                }
            }

            return ret;
        }

        private void buildPsfs(string[] pUniqueSequences, string[] pVhFiles, Bin2PsfStruct pBin2PsfStruct,
            DoWorkEventArgs e)
        {
            string ripOutputFolder = Path.Combine(OUTPUT_FOLDER, pBin2PsfStruct.outputFolder);
            
            string bin2PsfSourcePath = Path.Combine(PROGRAMS_FOLDER, "bin2psf.exe");
            string bin2PsfDestinationPath = Path.Combine(WORKING_FOLDER, "bin2psf.exe");

            string vhName;
            string vbName;

            string sepPsfLibExePath;
            string vhVbMiniPsfLibExePath;
            string exePath;
            string psfLibName;
            string filePrefix;

            PsxSequenceType sequenceType;
            int trackId;
            string originalExe;
            uint seqCount = 0;
            uint totalSequences = 1;
            PsfMakerTask task;
            
            try
            {
                // create working directory
                Directory.CreateDirectory(WORKING_FOLDER);

                // copy program
                File.Copy(bin2PsfSourcePath, bin2PsfDestinationPath, true);
            }
            catch (Exception ex)
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = ex.Message;
                ReportProgress(0, this.progressStruct);

                return;
            }

            // modify minipsf .exe for VH/VB lib and make .psflib
            if (pBin2PsfStruct.MakePsfLib)
            {
                // vhVbMiniPsfLibExePath = setMiniPsfValues(GENERIC_MINIPSF_EXE_PATH, pBin2PsfStruct, false);

                this.makePsfFile(
                    PsfMakerTask.SeqPsfLib,
                    pBin2PsfStruct,
                    pBin2PsfStruct.exePath,
                    null,
                    pVhFiles[0],
                    Path.ChangeExtension(pVhFiles[0], ".vb"),
                    bin2PsfDestinationPath,
                    ripOutputFolder,
                    null,
                    null,
                    Path.GetFileNameWithoutExtension(pBin2PsfStruct.psflibName),
                    -1,
                    -1);
            }

            #region MAIN LOOP

            foreach (string sequenceFile in pUniqueSequences)
            {                                
                if (!CancellationPending)
                {
                    originalExe = pBin2PsfStruct.exePath;
                    sequenceType = this.getPsxSequenceType(sequenceFile);

                    // get sequence count
                    if (pBin2PsfStruct.ForceSepTrackNo && PsxSequence.IsSepTypeSequence(sequenceFile))
                    {
                        seqCount = 1;
                        
                        // get actual number of sequences for SEP functions
                        totalSequences = PsxSequence.GetSeqCount(sequenceFile);
                    }
                    else
                    {
                        seqCount = PsxSequence.GetSeqCount(sequenceFile);
                        totalSequences = seqCount;
                    }
                                        
                    // Try combinations                    
                    if (pBin2PsfStruct.TryCombinations)
                    #region COMBINATIONS
                    {
                        // loop over VH/VB files
                        foreach (string vhFile in pVhFiles)
                        {
                            try
                            {
                                // single sequence
                                if (seqCount == 1)
                                {
                                    if (sequenceType == PsxSequenceType.SeqType)
                                    {
                                        task = PsfMakerTask.SeqPsf;
                                        filePrefix = String.Format(
                                            "SEQ[{0}]_VAB[{1}]",
                                            Path.GetFileNameWithoutExtension(sequenceFile),
                                            Path.GetFileNameWithoutExtension(vhFile));
                                        trackId = 0;
                                    }
                                    else // sequenceType == PsxSequenceType.SepType
                                    {
                                        task = PsfMakerTask.SepPsf;
                                        filePrefix = String.Format(
                                            "SEP[{0}]_VAB[{1}]",
                                            Path.GetFileNameWithoutExtension(sequenceFile),
                                            Path.GetFileNameWithoutExtension(vhFile));

                                        if (pBin2PsfStruct.ForceSepTrackNo)
                                        {
                                            trackId = pBin2PsfStruct.SepTrackNo;
                                        }
                                        else
                                        {
                                            trackId = 0;
                                        }
                                    }

                                    this.makePsfFile(
                                        task,
                                        pBin2PsfStruct,
                                        pBin2PsfStruct.exePath,
                                        sequenceFile,
                                        vhFile,
                                        Path.ChangeExtension(vhFile, ".vb"),
                                        bin2PsfDestinationPath,
                                        ripOutputFolder,
                                        null,
                                        null,
                                        filePrefix,
                                        trackId,
                                        (int)totalSequences);
                                }
                                else if (seqCount > 1) // SEP only
                                {
                                    task = PsfMakerTask.SepPsf;

                                    for (int i = 0; i < seqCount; i++)
                                    {
                                        filePrefix = String.Format(
                                            "SEP[{0}_{1}]_VAB[{2}]",
                                            Path.GetFileNameWithoutExtension(sequenceFile),
                                            i.ToString("X2"),
                                            Path.GetFileNameWithoutExtension(vhFile));

                                        this.makePsfFile(
                                            task,
                                            pBin2PsfStruct,
                                            pBin2PsfStruct.exePath,
                                            sequenceFile,
                                            vhFile,
                                            Path.ChangeExtension(vhFile, ".vb"),
                                            bin2PsfDestinationPath,
                                            ripOutputFolder,
                                            null,
                                            null,
                                            filePrefix,
                                            i,
                                            (int)totalSequences);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                this.progressStruct.Clear();
                                this.progressStruct.FileName = sequenceFile;
                                this.progressStruct.ErrorMessage = String.Format("{0}{1}", ex.Message, Environment.NewLine);
                                ReportProgress(this.progress, this.progressStruct);
                            }
                        }
                    }
                    #endregion
                    else // no combinations
                    #region NO COMBINATIONS
                    {
                        // single sequence
                        if (seqCount == 1)
                        {
                            filePrefix = Path.GetFileNameWithoutExtension(sequenceFile);
                            
                            if (sequenceType == PsxSequenceType.SeqType)
                            {
                                task = PsfMakerTask.SeqPsf;
                                trackId = 0;
                            }
                            else // sequenceType == PsxSequenceType.SepType
                            {
                                task = PsfMakerTask.SepPsf;

                                if (pBin2PsfStruct.ForceSepTrackNo)
                                {
                                    trackId = pBin2PsfStruct.SepTrackNo;
                                }
                                else
                                {
                                    trackId = 0;
                                }
                            }
                            
                            if (pBin2PsfStruct.MakePsfLib)
                            {
                                vhName = null;
                                vbName = null;                                

                                if (sequenceType == PsxSequenceType.SeqType)
                                {
                                    task = PsfMakerTask.SeqMiniPsf;
                                    vhVbMiniPsfLibExePath = setMiniPsfValues(GENERIC_MINIPSF_EXE_PATH, pBin2PsfStruct, false);
                                }
                                else // sequenceType == PsxSequenceType.SepType
                                {
                                    task = PsfMakerTask.SepPsfWithVhVbLib;
                                    vhVbMiniPsfLibExePath = setMiniPsfValues(GENERIC_MINIPSF_EXE_PATH, pBin2PsfStruct, true);
                                }

                                exePath = vhVbMiniPsfLibExePath;
                            }
                            else
                            {
                                vhName = Path.ChangeExtension(sequenceFile, ".vh");
                                vbName = Path.ChangeExtension(sequenceFile, ".vb");
                                exePath = pBin2PsfStruct.exePath;

                                if (sequenceType == PsxSequenceType.SeqType)
                                {
                                    task = PsfMakerTask.SeqPsf;
                                }
                                else // sequenceType == PsxSequenceType.SepType
                                {
                                    task = PsfMakerTask.SepPsf;
                                }
                            }

                            try
                            {
                                this.makePsfFile(
                                    task,
                                    pBin2PsfStruct,
                                    exePath,
                                    sequenceFile,
                                    vhName,
                                    vbName,
                                    bin2PsfDestinationPath,
                                    ripOutputFolder,
                                    pBin2PsfStruct.psflibName,
                                    null,
                                    filePrefix,
                                    trackId,
                                    (int)totalSequences);
                            }
                            catch (Exception ex)
                            { 
                                this.progressStruct.Clear();
                                this.progressStruct.FileName = sequenceFile;
                                this.progressStruct.ErrorMessage = String.Format("{0}{1}", ex.Message, Environment.NewLine);
                                ReportProgress(this.progress, this.progressStruct);
                            }
                        }
                        else if (seqCount > 1) // SEP only
                        {
                            // make SEP psflib
                            psfLibName = Path.GetFileName(Path.ChangeExtension(sequenceFile, PSFLIB_FILE_EXTENSION));                            

                            if (pBin2PsfStruct.MakePsfLib)
                            {
                                vhName = null;
                                vbName = null;
                                task = PsfMakerTask.SepMiniPsfWithVhVbLib;
                                // sepPsfLibExePath = setMiniPsfValues(GENERIC_MINIPSF_EXE_PATH, pBin2PsfStruct, true);
                                sepPsfLibExePath = setMiniPsfValues(GENERIC_MINIPSF_EXE_PATH, pBin2PsfStruct, false);
                            }
                            else
                            {
                                vhName = Path.ChangeExtension(sequenceFile, ".vh");
                                vbName = Path.ChangeExtension(sequenceFile, ".vb");
                                task = PsfMakerTask.SepMiniPsf;
                                sepPsfLibExePath = pBin2PsfStruct.exePath;
                            }

                            this.makePsfFile(
                                PsfMakerTask.SepPsfLib,
                                pBin2PsfStruct,
                                sepPsfLibExePath,
                                sequenceFile,
                                vhName,
                                vbName,
                                bin2PsfDestinationPath,
                                ripOutputFolder,
                                null,
                                null,
                                Path.GetFileNameWithoutExtension(psfLibName),
                                -1,
                                -1);


                            // create minipsfs
                            sepPsfLibExePath = setMiniPsfValues(GENERIC_MINIPSF_EXE_PATH, pBin2PsfStruct, true);
                            
                            for (int i = 0; i < seqCount; i++)
                            {
                                if (!CancellationPending)
                                {
                                    filePrefix = String.Format(
                                        "{0}_{1}",
                                        Path.GetFileNameWithoutExtension(sequenceFile),
                                        i.ToString("X2"));

                                    this.makePsfFile(
                                        task,
                                        pBin2PsfStruct,
                                        sepPsfLibExePath,
                                        null,
                                        null,
                                        null,
                                        bin2PsfDestinationPath,
                                        ripOutputFolder,
                                        pBin2PsfStruct.psflibName,
                                        psfLibName,
                                        filePrefix,
                                        i,
                                        (int)totalSequences);
                                }
                                else
                                {
                                    e.Cancel = true;
                                    return;
                                } // if (!CancellationPending)
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    e.Cancel = true;
                    return;                
                }
            }

            #endregion

            // delete working folder
            try
            {
                Directory.Delete(WORKING_FOLDER, true);
            }
            catch (Exception ex2)
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage = ex2.Message;
                ReportProgress(100, this.progressStruct);
            }
        }

        private void makePsfFile(
            PsfMakerTask task,
            Bin2PsfStruct pBin2PsfStruct,
            string sourceExePath,
            string seqFile,
            string vhFile,
            string vbFile,
            string bin2PsfDestinationPath,
            string ripOutputFolder,
            string vhVhPsfLibFileName,
            string sepPsfLibFileName,
            string filePrefix,
            int sepCount,
            int sepTotalSeqs)
        {

            long pcOffsetSeq;
            long pcOffsetVh;
            long pcOffsetVb;
            long pcOffsetSepParams = 0;
            long textSectionOffsetValue;

            bool isSeqPresent = !String.IsNullOrEmpty(seqFile);
            bool isVbPresent = !String.IsNullOrEmpty(vbFile);
            bool isVhPresent = !String.IsNullOrEmpty(vhFile);

            string destinationFile;
            FileInfo fi = null;

            // report progress
            this.progress = (++this.fileCount * 100) / maxFiles;
            this.progressStruct.Clear();
            this.progressStruct.FileName = seqFile;
            ReportProgress(this.progress, this.progressStruct);

            // copy data files to working directory                    
            string sourceDirectory = pBin2PsfStruct.sourcePath;
            string destinationExeFile = Path.Combine(WORKING_FOLDER, filePrefix + ".BIN");
            string builtFilePath;

            // build output file name
            switch (task)
            {
                case PsfMakerTask.SepMiniPsf:
                case PsfMakerTask.SeqMiniPsf:
                case PsfMakerTask.SepPsfWithVhVbLib:
                case PsfMakerTask.SepMiniPsfWithVhVbLib:
                    builtFilePath = String.Format("{0}.{1}", filePrefix, "minipsf");
                    break;                
                case PsfMakerTask.SepPsf:
                case PsfMakerTask.SeqPsf:
                    builtFilePath = String.Format("{0}.{1}", filePrefix, "psf");
                    break;
                case PsfMakerTask.SepPsfLib:
                case PsfMakerTask.SeqPsfLib:
                    builtFilePath = String.Format("{0}.{1}", filePrefix, "psflib");
                    break;
                default:
                    builtFilePath = String.Format("{0}.{1}", filePrefix, "xsf");
                    break;
            }

            builtFilePath = Path.Combine(WORKING_FOLDER, builtFilePath);

            // copy exe to destination folder
            File.Copy(sourceExePath, destinationExeFile, true);

            // determine offsets
            using (FileStream fs = File.OpenRead(destinationExeFile))
            {
                // get offset of text section
                byte[] textSectionOffset = ParseFile.ParseSimpleOffset(fs, 0x18, 4);
                textSectionOffsetValue = BitConverter.ToUInt32(textSectionOffset, 0);

                // calculate pc offsets
                pcOffsetSeq = VGMToolbox.util.ByteConversion.GetLongValueFromString(pBin2PsfStruct.seqOffset) -
                    textSectionOffsetValue + Psf.PC_OFFSET_CORRECTION;
                pcOffsetVb = VGMToolbox.util.ByteConversion.GetLongValueFromString(pBin2PsfStruct.vbOffset) -
                    textSectionOffsetValue + Psf.PC_OFFSET_CORRECTION;
                pcOffsetVh = VGMToolbox.util.ByteConversion.GetLongValueFromString(pBin2PsfStruct.vhOffset) -
                    textSectionOffsetValue + Psf.PC_OFFSET_CORRECTION;
                pcOffsetSepParams = VGMToolbox.util.ByteConversion.GetLongValueFromString(pBin2PsfStruct.ParamOffset) -
                    textSectionOffsetValue + Psf.PC_OFFSET_CORRECTION;
            }

            // copy and insert the data
            if ((task == PsfMakerTask.SepPsf) ||
                (task == PsfMakerTask.SepPsfLib) ||
                (task == PsfMakerTask.SepPsfWithVhVbLib) ||
                (task == PsfMakerTask.SeqMiniPsf) ||
                (task == PsfMakerTask.SeqPsf))
            {                                                       
                if (isSeqPresent)
                {
                    destinationFile = Path.Combine(WORKING_FOLDER, Path.GetFileName(seqFile));
                    File.Copy(seqFile, destinationFile, true);
                    fi = new FileInfo(destinationFile);
                    FileUtil.ReplaceFileChunk(destinationFile, 0, fi.Length,
                        destinationExeFile, pcOffsetSeq);
                    File.Delete(destinationFile);
                }
            }

            if ((task == PsfMakerTask.SepPsf) ||
                (task == PsfMakerTask.SepPsfLib) ||
                (task == PsfMakerTask.SeqPsf) || 
                (task == PsfMakerTask.SeqPsfLib))
            {

                if (isVbPresent)
                {
                    destinationFile = Path.Combine(WORKING_FOLDER, Path.GetFileName(vbFile));
                    File.Copy(vbFile, destinationFile, true);
                    fi = new FileInfo(destinationFile);
                    FileUtil.ReplaceFileChunk(destinationFile, 0, fi.Length,
                        destinationExeFile, pcOffsetVb);
                    File.Delete(destinationFile);
                }

                if (isVhPresent)
                {
                    destinationFile = Path.Combine(WORKING_FOLDER, Path.GetFileName(vhFile));
                    File.Copy(vhFile, destinationFile, true);
                    fi = new FileInfo(destinationFile);
                    FileUtil.ReplaceFileChunk(destinationFile, 0, fi.Length,
                        destinationExeFile, pcOffsetVh);
                    File.Delete(destinationFile);
                }
            }

            if ((task == PsfMakerTask.SepMiniPsf) ||
                (task == PsfMakerTask.SepMiniPsfWithVhVbLib) ||
                (task == PsfMakerTask.SepPsf) ||
                (task == PsfMakerTask.SepPsfWithVhVbLib))
            {
                    byte[] tickModeBytes = BitConverter.GetBytes((uint)1);
                    byte[] loopOffBytes = BitConverter.GetBytes((uint)1);
                    byte[] sepCountBytes = BitConverter.GetBytes((uint)sepCount);
                    byte[] sepTotalSeqsBytes = BitConverter.GetBytes((uint)sepTotalSeqs);

                    FileUtil.UpdateChunk(destinationExeFile, (int)(pcOffsetSepParams + PARAM_SEQNUM_TICKMODE), tickModeBytes);
                    FileUtil.UpdateChunk(destinationExeFile, (int)(pcOffsetSepParams + PARAM_MAXSEQ_LOOPOFF), loopOffBytes);
                    FileUtil.UpdateChunk(destinationExeFile, (int)(pcOffsetSepParams + PARAM_SEQNUM_OFFSET), sepCountBytes);
                    FileUtil.UpdateChunk(destinationExeFile, (int)(pcOffsetSepParams + PARAM_MAXSEQ_OFFSET), sepTotalSeqsBytes);

                    if ((task == PsfMakerTask.SepMiniPsf) ||
                        (task == PsfMakerTask.SepMiniPsfWithVhVbLib))
                    {
                        uint totalFileSize = (uint)(pcOffsetSepParams + PARAM_MAXSEQ_OFFSET + 4);
                        byte[] totalFileSizeBytes = BitConverter.GetBytes(totalFileSize);
                        
                        FileUtil.TrimFileToLength(destinationExeFile, (int)totalFileSize);
                    }
            }
                
            // build bin2psf arguments                    
            StringBuilder bin2PsfArguments = new StringBuilder();
            bin2PsfArguments.Append(String.Format(" {0} 1 {1}.bin", Path.GetExtension(builtFilePath).Substring(1), filePrefix));

            // run bin2psf                
            Process bin2PsfProcess = new Process();
            bin2PsfProcess.StartInfo = new ProcessStartInfo(bin2PsfDestinationPath, bin2PsfArguments.ToString());
            bin2PsfProcess.StartInfo.WorkingDirectory = WORKING_FOLDER;
            bin2PsfProcess.StartInfo.UseShellExecute = false;
            bin2PsfProcess.StartInfo.CreateNoWindow = true;
            bool isSuccess = bin2PsfProcess.Start();
            bin2PsfProcess.WaitForExit();

            if (isSuccess)
            {
                // add lib tag(s)
                if ((task == PsfMakerTask.SepMiniPsf) || 
                    (task == PsfMakerTask.SeqMiniPsf) ||
                    (task == PsfMakerTask.SepPsfWithVhVbLib) ||
                    (task == PsfMakerTask.SepMiniPsfWithVhVbLib))
                {
                    using (FileStream ofs = File.Open(builtFilePath, FileMode.Open, FileAccess.Write))
                    {
                        ofs.Seek(0, SeekOrigin.End);
                        using (BinaryWriter bw = new BinaryWriter(ofs))
                        {
                            System.Text.Encoding enc = System.Text.Encoding.ASCII;
                            bw.Write(enc.GetBytes(Xsf.ASCII_TAG)); // [TAG]

                            if (!String.IsNullOrEmpty(vhVhPsfLibFileName))
                            {
                                bw.Write(enc.GetBytes(String.Format("_lib={0}", vhVhPsfLibFileName)));
                                bw.Write(new byte[] { 0x0A });
                            }

                            if (!String.IsNullOrEmpty(sepPsfLibFileName))
                            {
                                if (!String.IsNullOrEmpty(vhVhPsfLibFileName))
                                {
                                    bw.Write(enc.GetBytes(String.Format("_lib2={0}", sepPsfLibFileName)));
                                    bw.Write(new byte[] { 0x0A });
                                }
                                else
                                {
                                    bw.Write(enc.GetBytes(String.Format("_lib={0}", sepPsfLibFileName)));
                                    bw.Write(new byte[] { 0x0A });                                
                                }
                            }
                        }
                    }
                }

                // copy file to output folder
                if (!Directory.Exists(ripOutputFolder))
                {
                    Directory.CreateDirectory(Path.Combine(OUTPUT_FOLDER, pBin2PsfStruct.outputFolder));
                }
                
                File.Move(builtFilePath, Path.Combine(ripOutputFolder, Path.GetFileName(builtFilePath)));

                // report to user
                this.progressStruct.Clear();
                this.progressStruct.GenericMessage = String.Format("{0} created.{1}", Path.GetFileName(builtFilePath), Environment.NewLine);                       
                ReportProgress(Constants.ProgressMessageOnly, this.progressStruct);
            }
        }

        private string setMiniPsfValues(string templateMiniPsfPath, Bin2PsfStruct pBin2PsfStruct, bool useSepParameters)
        {
            string modifiedMiniPsfPath = Path.Combine(WORKING_FOLDER, Path.GetFileName(templateMiniPsfPath));
            long seqSize = VGMToolbox.util.ByteConversion.GetLongValueFromString(pBin2PsfStruct.SeqSize);
            int totalFileSize;

            // copy file
            File.Copy(templateMiniPsfPath, modifiedMiniPsfPath, true);

            // edit values            
            if (!useSepParameters)
            {
                byte[] seqOffsetBytes = BitConverter.GetBytes((uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(pBin2PsfStruct.seqOffset));
                byte[] seqSizeBytes = BitConverter.GetBytes((uint)seqSize);

                FileUtil.UpdateChunk(modifiedMiniPsfPath, Psf.MINIPSF_INITIAL_PC_OFFSET, seqOffsetBytes);
                FileUtil.UpdateChunk(modifiedMiniPsfPath, Psf.MINIPSF_TEXT_SECTION_OFFSET, seqOffsetBytes);
                FileUtil.UpdateChunk(modifiedMiniPsfPath, Psf.MINIPSF_TEXT_SECTION_SIZE_OFFSET, seqSizeBytes);

                // trim end of file
                totalFileSize = (int)(seqSize + Psf.PC_OFFSET_CORRECTION);
            }
            else
            {
                uint paramOffet = (uint)VGMToolbox.util.ByteConversion.GetLongValueFromString(pBin2PsfStruct.ParamOffset);
                byte[] paramOffsetBytes = BitConverter.GetBytes(paramOffet);

                totalFileSize = (int)(Psf.PC_OFFSET_CORRECTION + PARAM_MAXSEQ_OFFSET + 4);
                byte[] textSectionSizeBytes = BitConverter.GetBytes((uint)(totalFileSize - Psf.PC_OFFSET_CORRECTION));

                FileUtil.UpdateChunk(modifiedMiniPsfPath, Psf.MINIPSF_INITIAL_PC_OFFSET, paramOffsetBytes);
                FileUtil.UpdateChunk(modifiedMiniPsfPath, Psf.MINIPSF_TEXT_SECTION_OFFSET, paramOffsetBytes);
                FileUtil.UpdateChunk(modifiedMiniPsfPath, Psf.MINIPSF_TEXT_SECTION_SIZE_OFFSET, textSectionSizeBytes);
            }
            
            FileUtil.TrimFileToLength(modifiedMiniPsfPath, totalFileSize);

            return modifiedMiniPsfPath;
        }

        private PsxSequenceType getPsxSequenceType(string filePath)
        {
            PsxSequenceType ret = PsxSequenceType.None;

            if (PsxSequence.IsSeqTypeSequence(filePath))
            {
                ret = PsxSequenceType.SeqType;
            }
            else if (PsxSequence.IsSepTypeSequence(filePath))
            {
                ret = PsxSequenceType.SepType;
            }
            else
            {
                this.progressStruct.Clear();
                this.progressStruct.FileName = filePath;
                this.progressStruct.ErrorMessage = String.Format("ERROR: Cannot parse <{0}> as SEQ or SEP type sequence file.{1}", filePath, Environment.NewLine);
                ReportProgress(this.progress, this.progressStruct);                                
            }
                        
            return ret;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Bin2PsfStruct bin2PsfStruct = (Bin2PsfStruct)e.Argument;            
            this.makePsfs(bin2PsfStruct, e);
        }            
    }
}
