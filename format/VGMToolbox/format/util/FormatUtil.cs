using System;
using System.IO;
using System.Reflection;

using ICSharpCode.SharpZipLib.GZip;

using VGMToolbox.util;

namespace VGMToolbox.format.util
{
    public static class FormatUtil
    {
        private const int MAX_SIGNATURE_LENGTH = 33;
        private const int HEADER_OFFSET = 0;

        private static readonly byte[] ZIP_SIGNATURE = new byte[] { 0x50, 0x4B, 0x03, 0x04 }; // PK..
        private static readonly byte[] GZIP_SIGNATURE = new byte[] { 0x1F, 0x8B };

        /// <summary>
        /// Loops through all classes that implement IFormat and checks the signature bytes (magic number) to determine
        /// the type
        /// </summary>
        /// <param name="pFileStream">File Stream of the file to check.</param>
        /// <returns>Type of object if it matches and exisiting format class, null otherwise</returns>
        public static Type getObjectType(FileStream pFileStream)
        {
            byte[] signatureBytes;
            
            // Grab the first few bytes, since a full file read is not needed for the header

            // Check for Gzip compressed (VGM: .vgz files)
            if (IsGzipFile(pFileStream))
            {
                signatureBytes = getGzippedSignatureBytes(pFileStream);
            }
            else
            {
                signatureBytes = ParseFile.ParseSimpleOffset(pFileStream, HEADER_OFFSET, MAX_SIGNATURE_LENGTH);
            }
            
            Type ret = null;
                        
            // Get the assembly for this application
            Assembly asm = Assembly.GetExecutingAssembly();
            
            // Get all classes being used in this application
            Type[] asmtypes = asm.GetTypes();


            // Loop through classes checking interfaces
            Object o;
            IFormat format;
            string fileExtension;
            
            foreach (Type t in asmtypes)
            {
                if (t.IsClass & t.GetInterface("IFormat") != null)
                {
                    // Create and instance of this format
                    o = asm.CreateInstance(t.FullName);
                    
                    // Set it to the Interface
                    format = o as IFormat;


                    if (format.GetAsciiSignature() != null)
                    {
                        // Check the header bytes
                        if (ParseFile.CompareSegment(signatureBytes, HEADER_OFFSET, format.GetAsciiSignature()))
                        {
                            ret = Type.GetType(t.FullName);
                            break;
                        }
                    }
                    else // fall back to extension
                    {
                        if (format.GetFileExtensions() != null)
                        {
                            fileExtension = Path.GetExtension(pFileStream.Name).ToUpper();
                            if (format.GetFileExtensions().Contains(fileExtension))
                            {
                                ret = Type.GetType(t.FullName);
                                break;                            
                            }
                        }
                        
                    }
                }
            }            
            return ret;
        }

        public static Type getEmbeddedTagsObjectType(FileStream pFileStream)
        {
            byte[] signatureBytes;

            // Grab the first few bytes, since a full file read is not needed for the header

            // Check for Gzip compressed (VGM: .vgz files)
            if (IsGzipFile(pFileStream))
            {
                signatureBytes = getGzippedSignatureBytes(pFileStream);
            }
            else
            {
                signatureBytes = ParseFile.ParseSimpleOffset(pFileStream, HEADER_OFFSET, MAX_SIGNATURE_LENGTH);
            }

            Type ret = null;

            // Get the assembly for this application
            Assembly asm = Assembly.GetExecutingAssembly();

            // Get all classes being used in this application
            Type[] asmtypes = asm.GetTypes();


            // Loop through classes checking interfaces
            Object o;
            IFormat format;
            string fileExtension;

            foreach (Type t in asmtypes)
            {
                if (t.IsClass & t.GetInterface("IEmbeddedTagsFormat") != null)
                {
                    // Create and instance of this format
                    o = asm.CreateInstance(t.FullName);

                    // Set it to the Interface
                    format = o as IFormat;


                    if (format.GetAsciiSignature() != null)
                    {
                        // Check the header bytes
                        if (ParseFile.CompareSegment(signatureBytes, HEADER_OFFSET, format.GetAsciiSignature()))
                        {
                            ret = Type.GetType(t.FullName);
                            break;
                        }
                    }
                    else // fall back to extension
                    {
                        if (format.GetFileExtensions() != null)
                        {
                            fileExtension = Path.GetExtension(pFileStream.Name).ToUpper();
                            if (format.GetFileExtensions().Contains(fileExtension))
                            {
                                ret = Type.GetType(t.FullName);
                                break;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public static Type getHootObjectType(FileStream pFileStream)
        {
            byte[] signatureBytes;

            // Grab the first few bytes, since a full file read is not needed for the header

            // Check for Gzip compressed (VGM: .vgz files)
            if (IsGzipFile(pFileStream))
            {
                signatureBytes = getGzippedSignatureBytes(pFileStream);
            }
            else
            {
                signatureBytes = ParseFile.ParseSimpleOffset(pFileStream, HEADER_OFFSET, MAX_SIGNATURE_LENGTH);
            }

            Type ret = null;

            // Get the assembly for this application
            Assembly asm = Assembly.GetExecutingAssembly();

            // Get all classes being used in this application
            Type[] asmtypes = asm.GetTypes();


            // Loop through classes checking interfaces
            Object o;
            IHootFormat format;
            string fileExtension;

            foreach (Type t in asmtypes)
            {
                if ((t.IsClass & t.GetInterface("IHootFormat") != null))
                {
                    // Create and instance of this format
                    o = asm.CreateInstance(t.FullName);

                    // Set it to the Interface
                    format = o as IHootFormat;


                    if (format.GetAsciiSignature() != null)
                    {
                        // Check the header bytes
                        if (ParseFile.CompareSegment(signatureBytes, HEADER_OFFSET, format.GetAsciiSignature()))
                        {
                            ret = Type.GetType(t.FullName);
                            break;
                        }
                    }
                    else // fall back to extension
                    {
                        if (format.GetFileExtensions() != null)
                        {
                            fileExtension = Path.GetExtension(pFileStream.Name).ToUpper();
                            if (format.GetFileExtensions().Contains(fileExtension))
                            {
                                ret = Type.GetType(t.FullName);
                                break;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public static bool IsZipFile(string pFilePath)
        {
            bool ret = false;
            byte[] signatureBytes = null;

            using (FileStream fs = File.OpenRead(pFilePath))
            {
                signatureBytes = ParseFile.ParseSimpleOffset(fs, HEADER_OFFSET, ZIP_SIGNATURE.Length);
            }

            if ((signatureBytes != null) && 
                (ParseFile.CompareSegment(signatureBytes, HEADER_OFFSET, ZIP_SIGNATURE)))
            {
                ret = true;
            }

            return ret;
        }

        public static bool IsGzipFile(string pFilePath)
        {
            bool ret = false;
            byte[] signatureBytes = null;
            using (FileStream fs = File.OpenRead(pFilePath))
            {
                signatureBytes = ParseFile.ParseSimpleOffset(fs, HEADER_OFFSET, GZIP_SIGNATURE.Length);
            }

            if ((signatureBytes != null) && 
                (ParseFile.CompareSegment(signatureBytes, HEADER_OFFSET, GZIP_SIGNATURE)))
            {
                ret = true;
            }

            return ret;
        }

        public static bool IsGzipFile(Stream pFileStream)
        {
            bool ret = false;
            long currentOffset = pFileStream.Position;
            
            byte[] signatureBytes = ParseFile.ParseSimpleOffset(pFileStream, HEADER_OFFSET, 
                GZIP_SIGNATURE.Length);

            if (ParseFile.CompareSegment(signatureBytes, HEADER_OFFSET, GZIP_SIGNATURE))
            {
                ret = true;
            }

            pFileStream.Position = currentOffset;
            
            return ret;
        }

        private static byte[] getGzippedSignatureBytes(Stream pFileStream)
        {
            byte[] ret = null;

            long currentPosition = pFileStream.Position;

            GZipInputStream gZipInputStream = new GZipInputStream(pFileStream);
            string tempGzipFile = Path.GetTempFileName();
            FileStream gZipFileStream = new FileStream(tempGzipFile, FileMode.Open, FileAccess.ReadWrite);

            int size = 4096;
            byte[] writeData = new byte[size];
            while (true)
            {
                size = gZipInputStream.Read(writeData, 0, size);
                if (size > 0)
                {
                    gZipFileStream.Write(writeData, 0, size);
                }
                else
                {
                    break;
                }
            }

            ret = ParseFile.ParseSimpleOffset(gZipFileStream, HEADER_OFFSET, MAX_SIGNATURE_LENGTH);
            gZipFileStream.Close();
            gZipFileStream.Dispose();

            File.Delete(tempGzipFile);               // delete temp file
            pFileStream.Position = currentPosition;  // return file to position on entry

            return ret;
        }
    }
}
