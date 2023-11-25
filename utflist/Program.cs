using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using VGMToolbox.format;
using VGMToolbox.util;

namespace utflist
{
    class Program
    {
        static void Main(string[] args)
        {
            string inFile = Path.GetFullPath(args[0]);
            string outFile = Path.GetFullPath(args[1]);
            long startOffset = long.Parse(args[2]);

            CriUtfTable topUtf = new CriUtfTable();

            using (FileStream fs = File.Open(Path.GetFullPath(inFile), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                topUtf.Initialize(fs, startOffset);
                //CriField field;

                //for (int i = 0; i < topUtf.Rows.Length; i++)
                //{
                //    var d = topUtf.Rows[i];
                //    //field = d.

                //    foreach (var key in d.Keys)
                //    {
                //        field = d[key];
                //        Console.WriteLine(field.ToString());
                //    }
                //}

                //Console.WriteLine(topUtf.GetTableAsString(true));

                File.WriteAllText(outFile, topUtf.GetTableAsString(0, true));

            }

            Console.WriteLine("Complete.  Press any key.");
            Console.ReadKey();
        }

        private static void usage()
        {


        }
    }
}
