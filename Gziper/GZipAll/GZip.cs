// Type: GzipAll.GZip
// Assembly: GzipAll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: G:\Meshes\GzipAll.exe

using System;
using System.IO;
using System.IO.Compression;

//using ICSharpCode.SharpZipLib.GZip;
//using ICSharpCode.SharpZipLib.Core;
 
namespace GzipAll
{
  public class GZip
  {
    public static bool Decompress(string filename)
    {
      try
      {
        using (FileStream fileStream1 = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          string str = filename;
          string path = str.Remove(str.Length - 3);
          using (FileStream fileStream2 = File.Create(path))
          {
            using (GZipStream gzipStream = new GZipStream((Stream) fileStream1, CompressionMode.Decompress))
            {
              gzipStream.CopyTo((Stream) fileStream2);
              return File.Exists(path);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Decompress(string filename): " + (object) ex);
      }
      return false;
    }

    public static bool Compress(string filename, string toFile)
    {
      try
      {
        byte[] buffer = new byte[4096];
        if (File.Exists(filename))
        {
          using (FileStream fileStream1 = File.Open(filename, FileMode.Open))
          {
            using (FileStream fileStream2 = File.Create(toFile))
            {
              using (GZipStream gzipStream = new GZipStream((Stream) fileStream2, CompressionLevel.Optimal))
              //using (GZipOutputStream gzipStream = new GZipOutputStream(fileStream2, 4096))
              {
                //gzipStream.SetLevel(9);
                int count;
                while ((count = fileStream1.Read(buffer, 0, buffer.Length)) != 0)
                  gzipStream.Write(buffer, 0, count);
              }
            }
          }
          return true;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Compress(string filename, string toFile): " + (object) ex);
        return false;
      }
      return false;
    }
  }
}
