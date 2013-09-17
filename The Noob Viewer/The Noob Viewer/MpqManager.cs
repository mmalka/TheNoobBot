using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using meshDatabase.Database;
using Microsoft.Win32;
using StormLib;
using MPQ;
using System.Windows.Forms;

namespace meshDatabase
{

    public static class MpqManager
    {
        public static string gameDir = null;

        public static void Initialize(string path = null)
        {
            gameDir = path;
        }

        public static DBC GetDBC(string name)
        {
            string path = "DBFilesClient\\" + name + ".dbc";
            return new DBC(Mpq.GetFile(path));
        }

        public static Stream GetFile(string path)
        {
            Stream file = Mpq.GetFile(path);
            if (file == null)
                throw new FileNotFoundException("Unable to find " + path);
            return file;
        }

        public static void Close()
        {
            Mpq.Close();
        }
    }
}
