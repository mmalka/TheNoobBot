using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    using System.Diagnostics;
    using System.IO;
    using System.Management;
    using System.Security.AccessControl;
    using nManager.Helpful;

    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var account = Others.GetProcessOwner(Process.GetCurrentProcess().Id);
            var botFile = Others.ReadFileAllLines(Application.StartupPath + "\\Data\\Launcher.txt")[0].Trim();
            if (File.Exists(botFile))
            {
                string random = Others.GetRandomString(Others.Random(4, 10));
                string tnbExe = random + ".exe";
                var sw = new StreamWriter(Application.StartupPath + "\\Data\\Launcher.txt");
                sw.WriteLine(tnbExe);
                sw.Close();
                File.Move(botFile, tnbExe);
                if (File.Exists(botFile + ".config"))
                    File.Move(botFile + ".config", tnbExe + ".config");
                Others.AddFileSecurity(tnbExe, account, FileSystemRights.ReadData, AccessControlType.Allow);
                Others.RemoveFileSecurity(tnbExe, account, FileSystemRights.ReadData, AccessControlType.Deny);
                Process.Start(tnbExe);
                Process.GetCurrentProcess().Kill();
            }
            MessageBox.Show("Please do not modify the name of the executable by yourself. Restore the original name or re-download the software.");
            Process.GetCurrentProcess().Kill();
        }
    }
}