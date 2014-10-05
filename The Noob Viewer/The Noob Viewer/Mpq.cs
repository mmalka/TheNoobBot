using System;
using System.IO;
using meshDatabase;
using TheNoobViewer;
using System.Windows.Forms;
using System.Reflection;

namespace MPQ
{
    static class Mpq
    {
        static CASCHandler CASC;
        static CASCFolder Root;

        public static void Init(string path)
        {
            CASC = CASCHandler.OpenLocalStorage(path);

            byte[] filelistbytes = TheNoobViewer.Properties.Resources.listfile;
            Stream stream = new MemoryStream(filelistbytes);

            CASC.LoadListFile(stream);
            Root = CASC.CreateStorageTree(LocaleFlags.All);
        }

        public static bool ExtractFile(string from, string to)
        {
            try
            {
                CASC.SaveFileTo(from, to, LocaleFlags.All);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Stream GetFile(string from)
        {
            try
            {
                return CASC.OpenFile(from, LocaleFlags.All);
            }
            catch
            {
                Console.WriteLine("Problem: " + from);
                return null;
            }
        }

        public static void Close()
        {
            //
        }
    }
}
