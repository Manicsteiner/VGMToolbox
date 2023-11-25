using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using VGMToolbox.format.hoot;

namespace hoot_vermouth_gen
{
    class Program
    {
        public static string XML_OUTPUT_NAME = "zzz_vermouth.xml";

        static void Main(string[] args)
        {
            gamelist hootGames;
            game[] vermouthGames;
            ArrayList vermouthGameList = new ArrayList();

            option midioutMixOption;
            ArrayList optionsList;

            XmlSerializer serializer;

            string xmlSourceFolder = Path.GetFullPath(args[0]);

            // get files
            string[] xmlSourceFiles = Directory.GetFiles(xmlSourceFolder, "*.xml");




            hootGames = new gamelist();
            serializer = new XmlSerializer(typeof(gamelist));

            midioutMixOption = new option();
            midioutMixOption.name = "midiout_mix";
            midioutMixOption.value = "0x80";

            // loop over xml files
            foreach (string xmlSourceFile in xmlSourceFiles)
            {
                // get game list from file
                using (FileStream xmlFs = File.OpenRead(xmlSourceFile))
                {
                    using (XmlTextReader textReader = new XmlTextReader(xmlFs))
                    {
                        hootGames = (gamelist)serializer.Deserialize(textReader);
                    }
                }

                // loop over the games
                foreach (game g in hootGames.Items)
                {
                    if (g.options != null && g.options.option != null)
                    {
                        optionsList = new ArrayList(g.options.option);

                        // check for 'midiout' option;
                        foreach (option o in optionsList)
                        {
                            // midiout_type: 4 (GS), 8 (GM)
                            if (o.name.Equals("midiout_type") &&
                                    (o.value.Equals("4") || o.value.Equals("8")))
                            {
                                // convert to vermouth type
                                o.value = "6";

                                // add midiout_mix
                                optionsList.Add(midioutMixOption);

                                // replace existing options
                                g.options.option = (option[])optionsList.ToArray(typeof(option));

                                // update title
                                g.name += " (Vermouth, Gravis Ultrasound simulation)";

                                // add to vermouth game list
                                vermouthGameList.Add(g);

                                break;
                            }
                        }
                    }
                }
            }



                int x = 1;

                // write output file


            if (vermouthGameList.Count > 0)
            {
                vermouthGames = (game[])vermouthGameList.ToArray(typeof(game));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                settings.Indent = true;
                settings.IndentChars = "\t";
                settings.NewLineChars = Environment.NewLine;
                settings.ConformanceLevel = ConformanceLevel.Document;

                // Use to suppress namespace attributes
                XmlSerializerNamespaces namespaceSerializer = new XmlSerializerNamespaces();
                namespaceSerializer.Add("", "");


                string outputPath = Path.Combine(Path.GetDirectoryName((Application.ExecutablePath)), XML_OUTPUT_NAME);

                serializer = new XmlSerializer(vermouthGames.GetType());

                using (XmlWriter xmlWriter = XmlTextWriter.Create(outputPath, settings))
                {
                    serializer.Serialize(xmlWriter, vermouthGames, namespaceSerializer);
                }

            }

                







            }
    }
}
