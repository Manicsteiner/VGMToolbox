using System;

namespace VGMToolbox.format
{
    class FormatEventArgs : EventArgs
    {
        private string message;

        public string Message 
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
            }
        }
    }
}
