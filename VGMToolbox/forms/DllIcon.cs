using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace VGMToolbox.forms
{
    // courtesy of http://social.msdn.microsoft.com/Forums/en/csharplanguage/thread/04297272-0564-4401-9688-4e393406e56a
    public class DllIcon : IDisposable
    {
        [DllImport("shell32.dll")]
        static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        [DllImport("user32.dll")]
        static extern bool DestroyIcon(IntPtr hInst);

        IntPtr handler;
        Icon icon;

        public static implicit operator Icon(DllIcon icon)
        {
            return icon.icon;
        }

        public DllIcon(string path, int index)
        {
            IntPtr processHandle = System.Diagnostics.Process.GetCurrentProcess().Handle;
            handler = ExtractIcon(processHandle, path, index);
            if(handler!=IntPtr.Zero)
                icon = Icon.FromHandle((IntPtr)handler);
        }

        public void Dispose()
        {
            if (handler != IntPtr.Zero)
                DestroyIcon(handler);
        }

        ~DllIcon()
        {
            Dispose();
        }
    }

}
