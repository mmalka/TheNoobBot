using System;
using System.IO;
using System.IO.Compression;

namespace nManager.Helpful
{
    public class GZip
    {
        /// <summary>
        /// Decompresses the specified file.
        /// </summary>
        /// <param name="filename">The file path.</param>
        /// <returns></returns>
        public static bool Decompress(string filename)
        {
            try
            {
                // Get the stream of the source file.
                using (FileStream inFile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Get original file extension, for example
                    // "doc" from report.doc.gz.
                    string curFile = filename;
                    string origName = curFile.Remove(curFile.Length - 3);

                    //Create the decompressed file.
                    using (FileStream outFile = File.Create(origName))
                    {
                        using (GZipStream decompress = new GZipStream(inFile,
                                                                      CompressionMode.Decompress))
                        {
                            // Copy the decompression stream 
                            // into the output file.
                            decompress.CopyTo(outFile);
                            return File.Exists(origName);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Decompress(string filename): " + exception);
            }
            return false;
        }

        /// <summary>
        /// Compress the specified file.
        /// </summary>
        /// <param name="filename">The file path.</param>
        /// <param name="toFile">To file.</param>
        /// <returns></returns>
        public static bool Compress(string filename, string toFile)
        {
            try
            {
                byte[] buffer = new byte[4096];
                if (File.Exists(filename))
                {
                    using (FileStream inputFile = File.Open(filename, FileMode.Open), outputFile = File.Create(toFile))
                    {
                        using (GZipStream gzip = new GZipStream(outputFile, CompressionMode.Compress))
                        {
                            int n;
                            while ((n = inputFile.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                gzip.Write(buffer, 0, n);
                            }
                        }
                    }
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Compress(string filename, string toFile): " + exception);
                return false;
            }
            return false;
        }
    }
}