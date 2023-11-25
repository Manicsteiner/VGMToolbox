using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using ICSharpCode.SharpZipLib.Checksums;

using VGMToolbox.util;

namespace VGMToolbox.format
{
    class S98V3 : S98V1, IFormat, IXsfTagFormat
    {
        private static readonly byte[] ASCII_SIGNATURE = new byte[] { 0x53, 0x39, 0x38, 0x33 }; // S983
        private const string FORMAT_ABBREVIATION = "S98V3";

        private const int V3_DEVICE_COUNT_OFFSET = 0x1C;
        private const int V3_DEVICE_COUNT_LENGTH = 0x04;

        private const int V3_DEVICE_INFO_OFFSET = 0x20;
        private const int V3_DEVICE_INFO_SIZE = 0x10;

        private byte[] deviceCount;
        private byte[] v3Tags;
        public byte[] DeviceCount { get { return this.deviceCount; } }
        public byte[] V3Tags { get { return this.v3Tags; } }

        private ArrayList s98Devices = new ArrayList();
        private Dictionary<string, string> v3TagHash = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public byte[] getDeviceCount(Stream pStream)
        {
            return ParseFile.ParseSimpleOffset(pStream, V3_DEVICE_COUNT_OFFSET, V3_DEVICE_COUNT_LENGTH);
        }
        public void getS98Devices(Stream pStream)
        {
            int offset = V3_DEVICE_INFO_OFFSET;
            int deviceCount = BitConverter.ToInt32(this.deviceCount, 0);

            for (int i = 0; i < deviceCount; i++)
            {
                S98Device s98Device = new S98Device();
                s98Device.DeviceType = ParseFile.ParseSimpleOffset(pStream, offset + S98DEVICE_TYPE_OFFSET,
                                        S98DEVICE_TYPE_LENGTH);
                s98Device.Clock = ParseFile.ParseSimpleOffset(pStream, offset + S98DEVICE_CLOCK_OFFSET,
                                        S98DEVICE_CLOCK_LENGTH);
                s98Device.Pan = ParseFile.ParseSimpleOffset(pStream, offset + S98DEVICE_PAN_OFFSET,
                                        S98DEVICE_PAN_LENGTH);
                s98Device.Reserve = ParseFile.ParseSimpleOffset(pStream, offset + S98DEVICE_RESERVED_OFFSET,
                                        S98DEVICE_RESERVED_LENGTH);

                s98Devices.Add(s98Device);

                offset += V3_DEVICE_INFO_SIZE;
            }
        }

        public override void Initialize(Stream pStream, string pFilePath)
        {
            base.Initialize(pStream, pFilePath);

            this.deviceCount = this.getDeviceCount(pStream);
            this.getS98Devices(pStream);

            Int32 tagOffset = BitConverter.ToInt32(this.songNameOffset, 0) + TAG_IDENTIFIER_LENGTH;
            this.v3Tags = ParseFile.ParseSimpleOffset(pStream, tagOffset, (int)pStream.Length - tagOffset);
            tagOffset -= TAG_IDENTIFIER_LENGTH;

            // check this
            this.data = ParseFile.ParseSimpleOffset(pStream, V3_DEVICE_INFO_OFFSET, tagOffset - V3_DEVICE_INFO_OFFSET);

            this.parseTagData();
            this.addDevicesToHash();
        }

        private void parseTagData()
        {
            string tagsString;

            if (this.v3Tags != null)
            {
                this.v3Tags = FileUtil.ReplaceNullByteWithSpace(this.v3Tags);

                tagsString = VGMToolbox.util.ByteConversion.GetEncodedText(
                    this.v3Tags, 
                    VGMToolbox.util.ByteConversion.GetPredictedCodePageForTags(this.v3Tags));

                // check for utf8 tag and reencode bytes if needed
                if (tagsString.IndexOf(TAG_UTF8_INDICATOR) > -1)
                {
                    tagsString = System.Text.Encoding.UTF8.GetString(this.v3Tags);
                }

                string[] splitTags = tagsString.Trim().Split((char)0x0A);
                string[] tag;

                foreach (string s in splitTags)
                {
                    tag = s.Split((char)0x3D);

                    if (tag.Length >= 2)
                    {
                        if (!tagHash.ContainsKey(tag[0]))
                        {
                            tagHash.Add(tag[0].Trim(), tag[1].Trim());
                            v3TagHash.Add(tag[0].Trim(), tag[1].Trim());
                        }
                        else
                        {
                            string oldTag = tagHash[tag[0]] + Environment.NewLine;
                            tagHash.Remove(tag[0]);
                            tagHash.Add(tag[0], oldTag + tag[1]);
                            v3TagHash.Remove(tag[0]);
                            v3TagHash.Add(tag[0], oldTag + tag[1]);
                        }
                    }
                }
            }
        }

        private void addDevicesToHash()
        {
            string deviceString = String.Empty;

            foreach (S98Device s in s98Devices)
            {
                deviceString += "[" + deviceHash[BitConverter.ToInt32(s.DeviceType, 0)] + "]";
            }

            if (s98Devices.Count > 0)
            {
                tagHash.Add("Devices Used", deviceString);
            }
        }

        public override void GetDatFileCrc32(ref Crc32 pChecksum)
        {
            pChecksum.Reset();

            pChecksum.Update(this.version);
            pChecksum.Update(this.timerInfo);
            pChecksum.Update(this.timerInfo2);                
            pChecksum.Update(this.compressing);
            pChecksum.Update(this.deviceCount);
            pChecksum.Update(this.data);
            
            // ADD LOOP POINT AS LOOP OFFSET - DATA OFFSET?
        }

        public override void GetDatFileChecksums(ref Crc32 pChecksum,
            ref CryptoStream pMd5CryptoStream, ref CryptoStream pSha1CryptoStream)
        {
            pChecksum.Reset();

            pChecksum.Update(this.version);
            pChecksum.Update(this.timerInfo);
            pChecksum.Update(this.timerInfo2);
            pChecksum.Update(this.compressing);

            pMd5CryptoStream.Write(this.version, 0, this.version.Length);
            pMd5CryptoStream.Write(this.timerInfo, 0, this.timerInfo.Length);
            pMd5CryptoStream.Write(this.timerInfo2, 0, this.timerInfo2.Length);
            pMd5CryptoStream.Write(this.compressing, 0, this.compressing.Length);

            pSha1CryptoStream.Write(this.version, 0, this.version.Length);
            pSha1CryptoStream.Write(this.timerInfo, 0, this.timerInfo.Length);
            pSha1CryptoStream.Write(this.timerInfo2, 0, this.timerInfo2.Length);
            pSha1CryptoStream.Write(this.compressing, 0, this.compressing.Length);

            pChecksum.Update(this.deviceCount);
            pMd5CryptoStream.Write(this.deviceCount, 0, this.deviceCount.Length);
            pSha1CryptoStream.Write(this.deviceCount, 0, this.deviceCount.Length);

            pChecksum.Update(this.data);
            pMd5CryptoStream.Write(this.data, 0, this.data.Length);
            pSha1CryptoStream.Write(this.data, 0, this.data.Length);

            // ADD LOOP POINT AS LOOP OFFSET - DATA OFFSET?
        }

        public override byte[] GetAsciiSignature()
        {
            return S98V3.ASCII_SIGNATURE;
        }

        public override string GetFormatAbbreviation()
        {
            return S98V3.FORMAT_ABBREVIATION;
        }

        #region IXsfTagFormat FUNCTIONS

        private string GetSimpleTag(string pTagKey)
        {
            string ret = String.Empty;

            if (this.tagHash.ContainsKey(pTagKey))
            {
                ret = tagHash[pTagKey];
            }

            return ret;
        }
        public string GetTitleTag() { return GetSimpleTag("title"); }
        public string GetArtistTag() { return GetSimpleTag("artist"); }
        public string GetGameTag() { return GetSimpleTag("game"); }
        public string GetYearTag() { return GetSimpleTag("year"); }
        public string GetGenreTag() { return GetSimpleTag("genre"); }
        public string GetCommentTag() { return GetSimpleTag("comment"); }
        public string GetCopyrightTag() { return GetSimpleTag("copyright"); }
        public string GetVolumeTag() { return GetSimpleTag("volume"); }
        public string GetLengthTag() { return GetSimpleTag("length"); }
        public string GetFadeTag() { return GetSimpleTag("fade"); }
        public string GetXsfByTag() { return GetSimpleTag("s98by"); }
        public string GetSystemTag() { return GetSimpleTag("system"); }
        public string GetTaggerTag() { return GetSimpleTag("tagger"); }

        private void SetSimpleTag(string pKey, string pNewValue, bool AddActionToBatchFile)
        {            
            if (!String.IsNullOrEmpty(pNewValue) && !String.IsNullOrEmpty(pNewValue.Trim()))
            {
                this.tagHash[pKey] = pNewValue.Trim();
                this.v3TagHash[pKey] = pNewValue.Trim();
            }
            else if (tagHash.ContainsKey(pKey))
            {
                tagHash.Remove(pKey);
                v3TagHash.Remove(pKey);
            }


        }
        public void SetTitleTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("title", pNewValue, AddActionToBatchFile); }
        public void SetArtistTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("artist", pNewValue, AddActionToBatchFile); }
        public void SetGameTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("game", pNewValue, AddActionToBatchFile); }
        public void SetYearTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("year", pNewValue, AddActionToBatchFile); }
        public void SetGenreTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("genre", pNewValue, AddActionToBatchFile); }
        public void SetCommentTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("comment", pNewValue, AddActionToBatchFile); }
        public void SetCopyrightTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("copyright", pNewValue, AddActionToBatchFile); }
        public void SetVolumeTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("volume", pNewValue, AddActionToBatchFile); }
        public void SetLengthTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("length", pNewValue, AddActionToBatchFile); }
        public void SetFadeTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("fade", pNewValue, AddActionToBatchFile); }
        public void SetXsfByTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("s98by", pNewValue, AddActionToBatchFile); }
        public void SetSystemTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("system", pNewValue, AddActionToBatchFile); }
        public void SetTaggerTag(string pNewValue, bool AddActionToBatchFile) { SetSimpleTag("tagger", pNewValue, AddActionToBatchFile); }

        public void UpdateTags()
        {
            int actualFileEnd;
            string retaggingFilePath;
            string[] splitValue;
            string[] splitParam = new string[] { Environment.NewLine };

            try
            {
                actualFileEnd = BitConverter.ToInt32(this.songNameOffset, 0);
                retaggingFilePath = Path.Combine(Path.GetDirectoryName(this.filePath),
                    String.Format("{0}_RETAG_{1}{2}", Path.GetFileNameWithoutExtension(this.filePath), new Random().Next().ToString(), Path.GetExtension(this.filePath)));

                // extract file without tags
                using (FileStream fs = File.OpenRead(this.filePath))
                {
                    ParseFile.ExtractChunkToFile(fs, 0, actualFileEnd, retaggingFilePath);
                }

                // add new tags
                using (FileStream fs = File.Open(retaggingFilePath, FileMode.Append, FileAccess.Write))
                {
                    byte[] dataToWrite;

                    // write [S98]
                    dataToWrite = System.Text.Encoding.ASCII.GetBytes(ASCII_TAG);
                    fs.Write(dataToWrite, 0, dataToWrite.Length);

                    // add or update utf8=1 tag
                    this.tagHash["utf8"] = "1";
                    this.v3TagHash["utf8"] = "1";

                    foreach (string key in v3TagHash.Keys)
                    {
                        splitValue = v3TagHash[key].Split(splitParam, StringSplitOptions.None);

                        foreach (string valueItem in splitValue)
                        {
                            dataToWrite = System.Text.Encoding.UTF8.GetBytes(String.Format("{0}={1}", key, valueItem));
                            fs.Write(dataToWrite, 0, dataToWrite.Length);
                            fs.WriteByte(0x0A);
                        }
                    } // foreach (string key in tagHash.Keys)
                } // using (FileStream fs = File.Open(this.filePath, FileMode.Append, FileAccess.Write))

                File.Delete(this.filePath);
                File.Move(retaggingFilePath, this.filePath);
            }
            catch (Exception _ex)
            {
                throw new Exception(String.Format("Error updating tags for <{0}>", this.filePath), _ex);
            }
        }

        #endregion
    }
}
