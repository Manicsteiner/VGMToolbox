using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using ICSharpCode.SharpZipLib.Checksums;

namespace VGMToolbox.format
{
    public interface IFormat
    {
        string FilePath { get; set; }
        
        byte[] GetAsciiSignature();
        string GetFileExtensions();  // Should return values only if getAsciiSignature returns NULL

        void Initialize(Stream pStream, string pFilePath);
               
        string GetFormatAbbreviation();
        bool IsFileLibrary();
        bool HasMultipleFileExtensions();

        bool UsesLibraries();
        bool IsLibraryPresent();

        Dictionary<string, string> GetTagHash();

        void GetDatFileCrc32(ref Crc32 pChecksum);
        void GetDatFileChecksums(ref Crc32 pChecksum, ref CryptoStream pMd5CryptoStream,
            ref CryptoStream pSha1CryptoStream);
    }
}
