using System;
using System.IO;
using CASCExplorer;
using meshDatabase;
using System.Reflection;

namespace MPQ
{
    static class Mpq
    {
        static CASCHandler _casc;
        private static CASCFolder Root;

        public static void Init(string path)
        {
            _casc = CASCHandler.OpenOnlineStorage("wow", "us");
            _casc.Root.SetFlags(LocaleFlags.enUS, ContentFlags.None);
            if (_casc.FileExists("DBFilesClient\\Map.db2"))
                _casc.OpenFile("DBFilesClient\\Map.db2");
        }

        /*public static bool ExtractFile(string from, string to)
        {
            try
            {
                CASC.SaveFileTo(from, to, locale);
                return true;
            }
            catch
            {
                return false;
            }
        }*/

        public static Stream GetFile(string from)
        {
            try
            {
                return _casc.OpenFile(from);
            }
            catch
            {
                if (!from.Contains(".adt"))
                    Console.WriteLine("Problem: " + from);
                return null;
            }
        }

        public static Stream GetFile(int fileDataId)
        {
            return _casc.OpenFile(fileDataId);
        }

        public static Stream GetFile(ulong hash)
        {
            return _casc.OpenFile(hash);
        }

        public static void Close()
        {
            //
        }
    }
}
