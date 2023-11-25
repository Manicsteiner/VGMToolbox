using System;
using System.Collections.Generic;
using System.Text;

namespace VGMToolbox.tools
{
    public interface ISerializablePreset
    {
        Header Header { set; get; }
        string NotesOrWarnings { set; get; }
    }
}
