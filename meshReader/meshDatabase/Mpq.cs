using System;
using System.IO;
using meshDatabase;
using TheNoobViewer;
using System.Reflection;

namespace MPQ
{
    static class Mpq
    {
        static CASCHandler CASC;
        static CASCFolder Root;
        static LocaleFlags locale;

        public static void Init(string path)
        {
            CASC = CASCHandler.OpenLocalStorage(path);
            Root = CASC.CreateStorageTree(LocaleFlags.All);

            // Now get locale to be able to extract any file
            try
            {
                CASC.OpenFile("DBFilesClient\\Map.db2", LocaleFlags.enUS);
                locale = LocaleFlags.enUS;
                return;
            } catch {}
            try
            {
                CASC.OpenFile("DBFilesClient\\Map.db2", LocaleFlags.frFR);
                locale = LocaleFlags.frFR;
                return;
            } catch {}
            try
            {
                CASC.OpenFile("DBFilesClient\\Map.db2", LocaleFlags.deDE);
                locale = LocaleFlags.deDE;
                return;
            } catch {}
            try
            {
                CASC.OpenFile("DBFilesClient\\Map.db2", LocaleFlags.enGB);
                locale = LocaleFlags.enGB;
                return;
            } catch
            {
                locale = LocaleFlags.All;
            }
        }

        public static bool ExtractFile(string from, string to)
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
        }

        public static Stream GetFile(string from)
        {
            try
            {
                //Console.WriteLine(from);
                return CASC.OpenFile(from, locale, ContentFlags.None);
            } catch {}
            try
            {
                //Console.WriteLine(from);
                return CASC.OpenFile(from, locale, ContentFlags.LowViolence);
            }
            catch
            {
                if (!from.Contains(".adt"))
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
