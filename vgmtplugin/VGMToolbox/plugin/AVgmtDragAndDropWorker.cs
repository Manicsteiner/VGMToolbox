using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using VGMToolbox.util;

namespace VGMToolbox.plugin
{    
    public abstract class AVgmtDragAndDropWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        protected const int DefaultProgressCounterIncrementValue = 1;
        
        protected int fileCount;
        protected int maxFiles;        
        protected int progress;
        protected VGMToolbox.util.ProgressStruct progressStruct;

        protected int progressCounter;
        protected int progressCounterIncrementer;
        protected StringBuilder outputBuffer;

        protected int Progress
        {
            get { return progress; }
            set { progress = value; }
        }

        protected AVgmtDragAndDropWorker()
        {
            this.progressCounter = 0;
            this.progressCounterIncrementer = AVgmtDragAndDropWorker.DefaultProgressCounterIncrementValue;
            this.outputBuffer = new StringBuilder();

            this.progressStruct = new VGMToolbox.util.ProgressStruct();
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
        }
        
        protected void DoTask(IVgmtWorkerStruct pTaskStruct, DoWorkEventArgs e)
        {
            this.maxFiles = FileUtil.GetFileCount(pTaskStruct.SourcePaths);
            
            // reset progress bar if needed
            this.progressStruct.Clear();
            this.progressStruct.FileName = String.Empty;
            ReportProgress(progress, progressStruct);

            foreach (string path in pTaskStruct.SourcePaths)
            {
                if (File.Exists(path))
                {
                    if (!CancellationPending)
                    {                       
                        // perform task
                        try
                        {
                            this.DoTaskForFile(path, pTaskStruct, e);
                        }
                        catch (Exception ex)
                        {
                            this.progressStruct.Clear();
                            this.progressStruct.ErrorMessage = 
                                String.Format(CultureInfo.CurrentCulture,"Error processing <{0}>.  Error received: ", path) + ex.Message + Environment.NewLine;
                            ReportProgress(this.progress, this.progressStruct);
                        }
                        finally
                        {                                                        
                            this.DoFinally();

                            fileCount += 1;

                            // Report Progress
                            if ((fileCount == maxFiles) ||
                                (((fileCount * 100) / maxFiles) > this.progressCounter))
                            {
                                this.progressCounter += this.progressCounterIncrementer;

                                // output info
                                if (this.outputBuffer.Length > 0)
                                {
                                    this.progressStruct.Clear();
                                    progressStruct.GenericMessage = this.outputBuffer.ToString();
                                    ReportProgress(this.Progress, progressStruct);

                                    // clear out old info
                                    this.outputBuffer.Length = 0;
                                }

                                // output progress
                                this.progressStruct.Clear();
                                this.progress = (fileCount * 100) / maxFiles;
                                this.progressStruct.FileName = path;
                                ReportProgress(progress, progressStruct);
                            }
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (Directory.Exists(path))
                {
                    this.DoTaskForDirectory(path, pTaskStruct, e);

                    if (CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            try
            {
                DoFinalTask(pTaskStruct);
            }
            catch (Exception ex2)
            {
                this.progressStruct.Clear();
                this.progressStruct.ErrorMessage =
                    String.Format(CultureInfo.CurrentCulture, "Error performing final task.  Error received: {0}{1}", ex2.Message, Environment.NewLine);
                ReportProgress(this.progress, this.progressStruct);
            }

            return;
        }

        protected virtual void DoTaskForDirectory(string pPath, IVgmtWorkerStruct pTaskStruct,
            DoWorkEventArgs e)
        {            
            foreach (string d in Directory.GetDirectories(pPath))
            {
                if (!CancellationPending)
                {
                    this.DoTaskForDirectory(d, pTaskStruct, e);
                }
                else
                {
                    e.Cancel = true;
                    break;
                }
            }
            
            foreach (string f in Directory.GetFiles(pPath))
            {
                if (!CancellationPending)
                {
                    // perform task
                    try
                    {
                        this.DoTaskForFile(f, pTaskStruct, e);
                    }
                    catch (Exception ex)
                    {
                        this.progressStruct.Clear();
                        this.progressStruct.ErrorMessage = 
                            String.Format(CultureInfo.CurrentCulture, "Error processing <{0}>.  Error received: ", f) + ex.Message + Environment.NewLine;
                        ReportProgress(progress, this.progressStruct);
                    }
                    finally
                    {
                        this.DoFinally();

                        fileCount += 1;

                        // Report Progress
                        if ((fileCount == maxFiles) ||
                            (((fileCount * 100) / maxFiles) > this.progressCounter))
                        {
                            this.progressCounter += this.progressCounterIncrementer;

                            // output info
                            if (this.outputBuffer.Length > 0)
                            {
                                this.progressStruct.Clear();
                                progressStruct.GenericMessage = this.outputBuffer.ToString();
                                ReportProgress(this.Progress, progressStruct);

                                // clear out old info
                                this.outputBuffer.Length = 0;
                            }

                            // output progress
                            this.progress = (fileCount * 100) / maxFiles;
                            this.progressStruct.Clear();
                            this.progressStruct.FileName = f;
                            ReportProgress(this.progress, progressStruct);
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        // this can be overridden in implementing classes that have more to do
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            this.DoTask((IVgmtWorkerStruct)e.Argument, e);
        }
        
        /// <summary>
        /// Tasks to perform in the Finally() clause of a Try/Catch statement.
        /// </summary>
        protected virtual void DoFinally() { }

        /// <summary>
        /// Performs a final task for the worker.  Outputting a report, for example.
        /// </summary>
        protected virtual void DoFinalTask(IVgmtWorkerStruct pTaskStruct) { }

        protected void ShowOutput(string path, string message, bool isError)
        {
            this.progressStruct.Clear();
            this.progressStruct.FileName = path;

            if (isError)
            {
                this.progressStruct.ErrorMessage = String.Format("ERROR: {0}{1}", message, Environment.NewLine);
            }
            else
            {
                this.progressStruct.GenericMessage = String.Format("{0}{1}", message, Environment.NewLine);
            }
            
            ReportProgress(this.progress, progressStruct);        
        }

        // abstract methods
        protected abstract void DoTaskForFile(string pPath, IVgmtWorkerStruct pTaskStruct, DoWorkEventArgs e);
    }
}
