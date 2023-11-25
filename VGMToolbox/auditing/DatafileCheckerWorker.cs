using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using VGMToolbox.format.auditing;
using VGMToolbox.plugin;
using VGMToolbox.util;

namespace VGMToolbox.auditing
{
    class DatafileCheckerWorker : BackgroundWorker, IVgmtBackgroundWorker
    {
        private int totalItems;
        private int maxItems;
        VGMToolbox.util.ProgressStruct progressStruct;

        public struct DatafileCheckerStruct
        {            
            public string datafilePath;
            public string outputPath;
        }
        
        public DatafileCheckerWorker()
        {
            totalItems = 0;
            maxItems = 0;
            
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        private void checkDatafile(DatafileCheckerStruct pDatafileCheckerStruct, DoWorkEventArgs e)
        {
            StreamWriter sw;
            
            datafile dataFile = new datafile();
            XmlSerializer serializer = new XmlSerializer(dataFile.GetType());
            TextReader textReader = new StreamReader(pDatafileCheckerStruct.datafilePath);
            dataFile = (datafile)serializer.Deserialize(textReader);
            textReader.Close();

            AuditingUtil auditingUtil = new AuditingUtil(dataFile);
            
            // Check for duplicates
            string dupePath = pDatafileCheckerStruct.outputPath + Path.DirectorySeparatorChar + "_DUPE.TXT";
            if (File.Exists(dupePath))
            {
                File.Delete(dupePath);
            }
            sw = File.CreateText(dupePath);

            // Update max items
            maxItems = auditingUtil.ChecksumHash.Count;

            this.checkForDupes(auditingUtil, sw);

            sw.Close();
            sw.Dispose();
        }

        private void checkForDupes(AuditingUtil pAuditingUtil, StreamWriter pStreamwriter)
        {
            ArrayList keys = new ArrayList(pAuditingUtil.ChecksumHash.Keys);
            int progress;
            
            this.progressStruct = new VGMToolbox.util.ProgressStruct();

            foreach (string k in keys)
            {
                progress = (++totalItems * 100) / maxItems;
                this.progressStruct.FileName = k;
                ReportProgress(progress, this.progressStruct);
                
                ArrayList gameList = (ArrayList)pAuditingUtil.ChecksumHash[k];
                if (gameList.Count > 1)
                {
                    pStreamwriter.Write(String.Format("Checksum: {0}", k) + Environment.NewLine);
                    
                    foreach (AuditingUtil.ChecksumStruct cs in gameList)
                    {
                        pStreamwriter.Write(AuditingUtil.ROM_SPACER + cs.game + 
                            Path.DirectorySeparatorChar + cs.rom + Environment.NewLine);
                    }
                    pStreamwriter.Write(Environment.NewLine);
                }
            }        
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            DatafileCheckerStruct datafileCheckerStruct = (DatafileCheckerStruct)e.Argument;
            this.checkDatafile(datafileCheckerStruct, e);
        }
    }
}
