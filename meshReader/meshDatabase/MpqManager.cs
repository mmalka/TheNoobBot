using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using meshDatabase.Database;
using Microsoft.Win32;
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
            Mpq.Init(path);
        }


        public static DB5Reader GetDB5(string name)
        {
            string path = "DBFilesClient\\" + name + ".db2";
            return new DB5Reader(Mpq.GetFile(path));

        }

        public static DBC GetDBC(string name)
        {
            string path = "DBFilesClient\\" + name + ".db2";
            return new DBC(Mpq.GetFile(path));

        }

        public static ulong GetHashByDataId(int dataId)
        {
            return Mpq.GetHashByDataId(dataId);
        }

        public static string GetFullNameByDataId(int dataId)
        {
            return Mpq.GeFullNameByDataId(dataId);
        }

        public static bool FileExists(string path)
        {
            return Mpq.FileExists(path);
        }

        public static bool FileExists(uint fileId)
        {
            return Mpq.FileExists(fileId);
        }

        public static bool FileExists(ulong hash)
        {
            return Mpq.FileExists(hash);
        }

        public static string GetFileExtension(int fileId)
        {
            return Mpq.GetFileExtension(fileId);
        }

        public static Stream GetFile(string path)
        {
            Stream file = Mpq.GetFile(path);
            if (file == null)
                throw new FileNotFoundException("Unable to find " + path);
            return file;
        }

        public static Stream GetFile(uint fileId)
        {
            Stream file = Mpq.GetFile(fileId);
            if (file == null)
                throw new FileNotFoundException("Unable to find " + fileId);
            return file;
        }

        public static Stream GetFile(ulong hash)
        {
            Stream file = Mpq.GetFile(hash);
            if (file == null)
                throw new FileNotFoundException("Unable to find " + hash);
            return file;
        }

        public static void Close()
        {
            Mpq.Close();
        }

        public static void DumpSpellList()
        {
            Mpq.DumpSpellList();
        }
    }
}
