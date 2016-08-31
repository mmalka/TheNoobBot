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
            //_casc = CASCHandler.OpenOnlineStorage("wow", "us");

            // read local storage instead of OnlineStorage because of DDOS targetting WOW servers.
            _casc = CASCHandler.OpenLocalStorage(@"X:\World of Warcraft");
            _casc.Root.SetFlags(LocaleFlags.enGB, ContentFlags.None);

            // test reading
            /*
            if (_casc.FileExists("DBFilesClient\\Map.db2"))
                _casc.OpenFile("DBFilesClient\\Map.db2");*/
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

        public static bool FileExists(string path)
        {
            return _casc.FileExists(path);
        }
        public static bool FileExists(uint fileId)
        {
            return _casc.FileExists(fileId);
        }
        public static bool FileExists(ulong hash)
        {
            return _casc.FileExists(hash);
        }

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

        internal static ulong GetHashByDataId(int dataId)
        {
            WowRootHandler rh = _casc.Root as WowRootHandler;

            return rh?.GetHashByFileDataId(dataId) ?? 0;
        }

        public static string GeFullNameByDataId(int dataId)
        {
            ulong hash = GetHashByDataId(dataId);
            return CASCFile.FileNames[hash];
        }

        public static Stream GetFile(uint fileDataId)
        {
            return _casc.OpenFile((int)fileDataId); // warp uint AND int so we never have issues; (else it consider it a ulong)
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
