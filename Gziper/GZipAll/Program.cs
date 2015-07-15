// Type: GzipAll.Program
// Assembly: GzipAll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: G:\Meshes\GzipAll.exe

using System;
using System.IO;

namespace GzipAll
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please enter the source and destination pathes");
            }
            else
            {
                string strSource = args[0];
                string strDestination = args[1];
                Console.WriteLine(strSource + " --> " + strDestination + "\n-----");
                if (!Directory.Exists(strSource))
                    Console.WriteLine("Please enter a directory which exists!!");
                else
                    Program.WorkOnFolders(strSource, strDestination);
            }
        }

        private static void WorkOnFiles(string ipath, string opath)
        {
            foreach (string filename in Directory.EnumerateFiles(ipath))
            {
                if (filename.Contains(".none"))
                    continue;
                string add = filename.Replace(ipath, null);
                GZip.Compress(filename, opath + add + ".gz");
                Console.WriteLine(opath + add + ".gz");
            }
        }

        private static void WorkOnFolders(string ipath, string opath)
        {
            if (!Directory.Exists(opath))
                Directory.CreateDirectory(opath);
            Program.WorkOnFiles(ipath, opath);
            foreach (string ipath1 in Directory.EnumerateDirectories(ipath))
            {
                string add = ipath1.Replace(ipath, null);
                Program.WorkOnFolders(ipath1, opath + add);
            }
        }
    }
}
