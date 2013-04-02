using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using meshDatabase.Database;
using Microsoft.Win32;
using MpqLib.Mpq;

namespace meshDatabase
{

    public static class MpqManager
    {
        private static readonly List<string> Languages = new List<string> {"enUS", "koKR", "frFR", "deDE", "zhTW", "esES", "esMX", "ruRU", "enGB", "enTW"};
        private static readonly List<string> IgnoredMPQs = new List<string> {  };

        private static bool _initialized;
        private static bool _initializedDBC;
        private static readonly Dictionary<string, CArchive> _archives = new Dictionary<string, CArchive>();
        private static CArchive _locale;

        private static string GetWoWInstallPath()
        {
            RegistryKey read = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\World of Warcraft");
            return read.GetValue("InstallPath").ToString();
        }

        public static bool Initialize(string path = null)
        {
            if (_initialized)
                return true;

            _initialized = true;

            string root = "";
            if (path == null)
                root = GetWoWInstallPath();
            else
                root = path;

            var files = Directory.GetFiles(root + "Data\\", "*.MPQ", SearchOption.TopDirectoryOnly).OrderBy(s => s);

            List<string> PatchFiles = new List<string> {};
            foreach (var file in files)
            {
                if (file.Contains("-update-"))
                {
                    PatchFiles.Add(file);
                    continue;
                }
                if (IgnoredMPQs.Contains(Path.GetFileNameWithoutExtension(file)))
                    continue;
                try
                {
                    var archive = new CArchive(file);
                    Console.WriteLine("Opened " + file + " with " + archive.FileCount + " files...");
                    _archives.Add(Path.GetFileNameWithoutExtension(file), archive);
                }
                catch (IOException ex)
                {
                    if (ex.Message.Contains("Unable to open"))
                        System.Windows.Forms.MessageBox.Show("TheNoobViewer cannot open Wow data files\nThey are probably locked by a running game instance\n\nTheNoobViewer will stop.", "Unrecoverable error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    else
                        throw;
                    return false;
                }
            }
            foreach (var file in PatchFiles)
            {
                foreach (var arc in _archives)
                {
                    CArchive mainArch = arc.Value;
                    Console.WriteLine("Patching " + arc.Key + " with " + file);
                    mainArch.Patch(file, null);
                }
            }

            return InitializeDBC(root);
        }

        public static bool InitializeDBC(string path = null)
        {
            if (_initializedDBC)
                return true;

            string root = "";
            if (path == null)
                root = GetWoWInstallPath();
            else
                root = path;

            foreach (var language in Languages)
            {
                var dir = root + "Data\\" + language;
                if (!Directory.Exists(dir))
                    continue;

                var file = dir + "\\locale-" + language + ".mpq";
                if (!File.Exists(file))
                    continue;

                _locale = new CArchive(file);

                var allfiles = Directory.GetFiles(dir + "\\", "*.MPQ", SearchOption.TopDirectoryOnly).OrderBy(s => s);

                foreach (var patchfile in allfiles)
                {
                    if (patchfile.Contains("-update-"))
                    {
                        _locale.Patch(patchfile, language);
                        Console.WriteLine("Patching " + _locale.FileName + " with " + patchfile);
                    }
                }
                Console.WriteLine("Using locale: " + file);
                break;
            }

            if (_locale == null)
                throw new FileNotFoundException("InitializeDBC()");
            _initializedDBC = true;
            return true;
        }

        public static DBC GetDBC(string name)
        {

            string path = "DBFilesClient\\" + name + ".dbc";
            if (_locale == null || !_locale.FileExists(path))
            {
                throw new FileNotFoundException("Unable to find DBC " + name + "\nPath: " + path + "\nLocale: " +
                                                _locale); //added some extra stuff for debug
            }
            return new DBC(new CFileStream(_locale, path));
        }

        public static Stream GetFile(string path)
        {
            var archive = _archives.Values.LastOrDefault(a => a.FileExists(path));
            if (archive == null)
                throw new FileNotFoundException("Unable to find " + path);
            var result = new CFileStream(archive, path);
            return result;
        }
    }

}