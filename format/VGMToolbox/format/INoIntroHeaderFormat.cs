using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VGMToolbox.format
{
    public interface INoIntroHeaderFormat
    {
        string SystemName { set; get; }

        Dictionary<string, string> FileData { set; get; }
        Dictionary<string, string> HeaderData { set; get; }
        Dictionary<string, string> TitleData { set; get; }
        Dictionary<string, string> EncryptionData { set; get; }

    }
}
