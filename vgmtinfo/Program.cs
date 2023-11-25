using System;
using System.Collections.Generic;
using System.IO;

using VGMToolbox.format;
using VGMToolbox.format.util;

namespace vgmtinfo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Usage();
            }
            else
            {
                string filename = Path.GetFullPath(args[0]);

                using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    Type dataType = FormatUtil.getObjectType(fs);

                    if (dataType != null)
                    {
                        string tagHashValue;
                        char[] trimNull = new char[] { '\0' };

                        // initialize data
                        IFormat vgmData = (IFormat)Activator.CreateInstance(dataType);
                        vgmData.Initialize(fs, filename);
                        Dictionary<string, string> tagHash = vgmData.GetTagHash();

                        // loop over hash and output information
                        foreach (string s in tagHash.Keys)
                        {
                            tagHashValue = tagHash[s];

                            if (!String.IsNullOrEmpty(tagHashValue))
                            {
                                tagHashValue = tagHashValue.TrimEnd(trimNull);
                            }
                            else
                            {
                                tagHashValue = String.Empty;
                            }

                            Console.WriteLine(s + ": " + tagHashValue);
                        }
                    }
                }
            }
        }

        static void Usage()
        {
            Console.WriteLine("usage: vgmtinfo.exe filename");
        }
    }
}
