using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using VGMToolbox.format.auditing;

namespace VGMToolbox.format.hoot
{
    public class HootCsvEntry
    {
        public static readonly string[] SEPARATOR = new string[] {"\","};

        public const int ENGLISH_NAME_INDEX = 0;
        public const int JAPANESE_NAME_INDEX = 1;
        public const int CHIP_INDEX = 2;
        public const int COMPANY_INDEX = 3;
        public const int SYSTEM_INDEX = 4;
        public const int COMPOSER_INDEX = 5;
        public const int RELEASE_YEAR_INDEX = 6;
        public const int FILE_NAME_INDEX = 7;
        public const int FILE_ID_INDEX = 8;
                        
        private string englishName;
        private string japaneseName;
        private string chip;
        private string company;
        private string system;
        private string composer;
        private string releaseYear;
        private string fileName;
        private int fileId;

        public string EnglishName
        {
            get { return englishName; }
            set { englishName = value; }        
        }
        public string JapaneseName
        {
            get { return japaneseName; }
            set { japaneseName = value; }
        }
        public string Chip
        {
            get { return chip; }
            set { chip = value; }
        }
        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        public string System
        {
            get { return system; }
            set { system = value; }
        }
        public string Composer
        {
            get { return composer; }
            set { composer = value; }
        }
        public string ReleaseYear
        {
            get { return releaseYear; }
            set { releaseYear = value; }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public int FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        public HootCsvEntry() { }

    }

    public class HootCsv
    {
        public HootCsv() { }

        private HootCsvEntry[] csvEntries;
        public HootCsvEntry[] CsvEntries { get { return csvEntries; } }
                                
        public void LoadFromFile(string pFilePath)
        {
            int lineCount;
            Encoding fileEncoding = Encoding.ASCII;
            TextReader textReader = new StreamReader(pFilePath, fileEncoding);
            string csvLine;

            lineCount = getLineCount(textReader);
            
            textReader.Close();
            textReader.Dispose();

            if (lineCount > 0)
            {
                this.csvEntries = new HootCsvEntry[lineCount - 1];

                // Grab first header line and ignore
                textReader = new StreamReader(pFilePath, fileEncoding);
                csvLine = textReader.ReadLine();

                for (int i = 0; i < lineCount - 1; i++)
                {
                    csvLine = textReader.ReadLine();
                    parseCsvEntry(csvLine, i);
                }

                textReader.Close();
                textReader.Dispose();
            }
        }

        private int getLineCount(TextReader pTextReader)
        { 
            int lineCount = 0;
            
            while (!String.IsNullOrEmpty(pTextReader.ReadLine()))
            {
                lineCount++;
            }

            return lineCount;
        }

        private void parseCsvEntry(string pLine, int pIndex)
        {
            string[] csvArray = pLine.Split(HootCsvEntry.SEPARATOR, StringSplitOptions.None);

            csvEntries[pIndex] = new HootCsvEntry();

            csvEntries[pIndex].EnglishName = csvArray[HootCsvEntry.ENGLISH_NAME_INDEX].Replace("\"", String.Empty).Trim();
            csvEntries[pIndex].JapaneseName = csvArray[HootCsvEntry.JAPANESE_NAME_INDEX].Replace("\"", String.Empty).Trim();
            csvEntries[pIndex].Chip = csvArray[HootCsvEntry.CHIP_INDEX].Replace("\"", String.Empty).Trim();
            csvEntries[pIndex].Company = csvArray[HootCsvEntry.COMPANY_INDEX].Replace("\"", String.Empty).Trim();
            csvEntries[pIndex].System = csvArray[HootCsvEntry.SYSTEM_INDEX].Replace("\"", String.Empty).Trim();
            csvEntries[pIndex].Composer = csvArray[HootCsvEntry.COMPOSER_INDEX].Replace("\"", String.Empty).Trim();
            csvEntries[pIndex].ReleaseYear = csvArray[HootCsvEntry.RELEASE_YEAR_INDEX].Replace("\"", String.Empty).Trim();
            csvEntries[pIndex].FileName = csvArray[HootCsvEntry.FILE_NAME_INDEX].Replace("\"", String.Empty).Trim();
            csvEntries[pIndex].FileId = Int16.Parse(csvArray[HootCsvEntry.FILE_ID_INDEX].Replace("\"", String.Empty).Trim());
        }
    }

    public class HootCsvTools
    {
        public static void AddCsvInformationToDataFile(string pCsvPath, 
            string pSourceDataFilePath, string pDestinationDataFilePath)
        {
            // Initialize CSV            
            HootCsv hootCsvFile = new HootCsv();
            hootCsvFile.LoadFromFile(pCsvPath);

            // Initialize Source DAT
            datafile dataFile = new datafile();
            XmlSerializer serializer = new XmlSerializer(typeof(datafile));
            TextReader textReader = new StreamReader(pSourceDataFilePath);
            dataFile = (datafile)serializer.Deserialize(textReader);
            textReader.Close();
            textReader.Dispose();

            // Get Csv HashTable
            Hashtable csvHashtable = getCsvHashTable(hootCsvFile);
            
            // Add Data to DAT File
            foreach (VGMToolbox.format.auditing.game g in dataFile.game)
            {
                if (csvHashtable.ContainsKey(g.name))
                { 
                    HootCsvEntry csvEntry = (HootCsvEntry) csvHashtable[g.name];
                    g.description = csvEntry.EnglishName;
                    g.board = csvEntry.System + " [" + csvEntry.Chip + "]";
                    g.manufacturer = csvEntry.Company;
                    g.year = csvEntry.ReleaseYear;
                    
                    g.comment = new string[1];
                    g.comment[0] = csvEntry.Composer;
                }
            }

            // Output DAT File
            TextWriter textWriter = new StreamWriter(pDestinationDataFilePath);
            serializer.Serialize(textWriter, dataFile);
            textWriter.Close();
            textWriter.Dispose();
        }

        private static Hashtable getCsvHashTable(HootCsv pHootCsvFile)
        {
            Hashtable csvHashTable = new Hashtable();
            
            foreach (HootCsvEntry he in pHootCsvFile.CsvEntries)
            {
                if (!csvHashTable.ContainsKey(he.FileName))
                {
                    csvHashTable.Add(he.FileName, he);
                }
            }

            return csvHashTable;
        }
    }
}
