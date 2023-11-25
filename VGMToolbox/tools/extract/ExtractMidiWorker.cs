using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using VGMToolbox.format;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.tools.extract
{
    public class ExtractMidiWorker : AVgmtDragAndDropWorker, IVgmtBackgroundWorker
    {
        public struct ExtractMidiStruct : IVgmtWorkerStruct
        {
            private string[] sourcePaths;
            public string[] SourcePaths
            {
                get { return sourcePaths; }
                set { sourcePaths = value; }
            }
        }

        public ExtractMidiWorker() :
            base() { }

        protected override void DoTaskForFile(string pPath, IVgmtWorkerStruct pExtractMidiStruct, DoWorkEventArgs e)
        {
            long headerOffset = 0;

            using (FileStream fs = File.OpenRead(pPath))
            {
                while ((headerOffset = ParseFile.GetNextOffset(fs, headerOffset, Midi.ASCII_SIGNATURE_MTHD)) > -1)
                {
                    Midi midiFile = new Midi();
                    midiFile.Initialize(fs, pPath, headerOffset);
                    midiFile.ExtractToFile(fs, Path.Combine(Path.GetDirectoryName(pPath), Path.GetFileNameWithoutExtension(pPath)));
                    headerOffset += 1;
                }
            }
        }           
    }
}
