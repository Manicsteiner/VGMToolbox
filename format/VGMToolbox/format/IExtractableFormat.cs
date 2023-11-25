using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VGMToolbox.format
{
    public interface IExtractableFormat
    {
        string FilePath { get; set; }
        long FileStartOffset { get; set; }
        long TotalFileLength { get; set; }

        void Initialize(Stream pStream, string pFilePath, long pFileOffset);
        void ExtractToFile(Stream pStream, string pOutputDirectory);
        void ExtractToFile(Stream pStream, string pOutputDirectory, object Options);
    }
}
