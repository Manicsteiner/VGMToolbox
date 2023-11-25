using System;
using System.ComponentModel;
using System.IO;

using VGMToolbox.format;
using VGMToolbox.format.util;
using VGMToolbox.plugin;

namespace VGMToolbox.tools.xsf
{
    class Time2sfWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        private VGMToolbox.util.ProgressStruct progressStruct;

        public Time2sfWorker()
        {
            this.progressStruct = new VGMToolbox.util.ProgressStruct();
            
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        private void time2sfs(Time2sfStruct pTime2sfStruct, DoWorkEventArgs e)
        {
            if (!CancellationPending)
            {
                string outputMessages;
                
                Time2sfStruct t = new Time2sfStruct();
                t.Mini2sfDirectory = pTime2sfStruct.Mini2sfDirectory;
                t.SdatPath = pTime2sfStruct.SdatPath;
                t.DoSingleLoop = pTime2sfStruct.DoSingleLoop;

                try
                {
                    XsfUtil.Time2sfFolder(t, out outputMessages);

                    this.progressStruct.Clear();

                    if (!String.IsNullOrEmpty(outputMessages))
                    {
                        this.progressStruct.ErrorMessage = String.Format("ERROR: {0}",
                            outputMessages) + Environment.NewLine;
                    }

                    ReportProgress(100, this.progressStruct);
                }
                catch(Exception _e)
                {
                    this.progressStruct.Clear();
                    this.progressStruct.ErrorMessage = String.Format("ERROR: {0}",
                        _e.Message) + Environment.NewLine;
                    ReportProgress(0, this.progressStruct);
                }                
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Time2sfStruct time2sfStruct = (Time2sfStruct) e.Argument;
            time2sfStruct.Mini2sfDirectory = Path.GetFullPath(time2sfStruct.Mini2sfDirectory);
            time2sfStruct.SdatPath = Path.GetFullPath(time2sfStruct.SdatPath);
            
            this.time2sfs(time2sfStruct, e);
        }    
    }
}
