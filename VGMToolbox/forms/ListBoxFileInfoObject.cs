using System;
using System.IO;

namespace VGMToolbox.forms
{
    public class ListBoxFileInfoObject
    {
        public ListBoxFileInfoObject(string path)
        {
            this.FilePath = path;
        }

        public string FilePath { set; get; }

        public override string ToString()
        {
            string ret;

            if (!String.IsNullOrEmpty(this.FilePath))
            {
                ret = Path.GetFileName(this.FilePath);
            }
            else
            {
                ret = String.Empty;
            }

            return ret;
        }
    }
}
