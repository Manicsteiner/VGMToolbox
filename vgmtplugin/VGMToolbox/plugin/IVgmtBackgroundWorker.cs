using System;
using System.ComponentModel;

namespace VGMToolbox.plugin
{    
    public interface IVgmtBackgroundWorker
    {
        event ProgressChangedEventHandler ProgressChanged;

        event RunWorkerCompletedEventHandler RunWorkerCompleted;
        
        bool IsBusy { get; }
        
        void RunWorkerAsync(object argument);
        
        void CancelAsync();
    }
}
