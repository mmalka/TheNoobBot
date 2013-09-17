using System;
using System.IO;
using StormLib;
using meshDatabase;

namespace MPQ
{
    static class Mpq
    {
        static readonly string[] archiveNames = {
                    "expansion4.mpq",
                    "expansion3.mpq",
                    "expansion2.mpq",
                    "expansion1.mpq",
                    "texture.mpq" };

        static readonly MpqArchiveSet archive = new MpqArchiveSet();
        static readonly string regGameDir = MpqArchiveSet.GetGameDirFromReg();

        static Mpq()
        {
            string dir;
            if (MpqManager.gameDir == null)
                dir = Path.Combine(regGameDir, "Data\\");
            else
                dir = Path.Combine(MpqManager.gameDir, "Data\\");
            archive.SetGameDir(dir);

            Console.WriteLine("Game dir is {0}", dir);
            if (archive.AddArchives(archiveNames) == 0)
            {
                System.Windows.Forms.MessageBox.Show("TheNoobViewer cannot open Wow data files\nThey are probably locked by a running game instance\n\nTheNoobViewer will stop.", "Unrecoverable error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            foreach (var loc in MpqLocale.Locales)
            {
                var strDirLocale = dir + loc + "\\";
                if (Directory.Exists(strDirLocale))
                {
                    var strLocaleMPQ = "locale-" + loc + ".MPQ";
                    if (File.Exists(strDirLocale + strLocaleMPQ))
                        archive.AddArchive(loc + "\\" + strLocaleMPQ);
                }
            }
        }

        public static bool ExtractFile(string from, string to, OpenFile dwSearchScope)
        {
            return archive.ExtractFile(from, to, dwSearchScope);
        }

        public static Stream GetFile(string from)
        {
            Stream strm;
            bool res = archive.ReadFile(from, out strm);
            if (res)
                return strm;
            return null;
        }

        public static void Close()
        {
            archive.Close();
        }
    }
}
