using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace The_Noob_Bot
{
    using System.IO;
    using System.Threading;

    internal static class Program
    {
        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var processName = Process.GetCurrentProcess().ProcessName.ToLower();
            /*if (!processName.Contains("vshost"))
            {
                bool doConfuse = false;
                if (File.Exists(Application.StartupPath + "\\Settings\\security.txt"))
                {
                    string res = Others.ReadFile(Application.StartupPath + "\\Settings\\security.txt");
                    var newProc = Process.GetCurrentProcess().ProcessName + ".exe";
                    if (res.ToLower() != newProc.ToLower())
                    {
                        doConfuse = true;
                    }
                }
                else
                    doConfuse = true;
                if (doConfuse)
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Settings\\");
                    int random = Others.Random(10, 1000);
                    string tnbExe = Process.GetCurrentProcess().ProcessName + random + ".exe";
                    File.Copy(Process.GetCurrentProcess().ProcessName + ".exe", tnbExe, true);
                    Thread.Sleep(50);
                    var sw = new StreamWriter(Application.StartupPath + "\\Settings\\security.crproj");
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    sw.WriteLine("<project outputDir=\"" + Application.StartupPath + "\\\" snKey=\"\" preset=\"minimum\" xmlns=\"http://confuser.codeplex.com\">");
                    sw.WriteLine("  <assembly path=\"" + Application.StartupPath + "\\" + tnbExe + "\" />");
                    sw.WriteLine("</project>");
                    sw.Close();

                    sw = new StreamWriter(Application.StartupPath + "\\Settings\\security.txt");
                    sw.WriteLine(tnbExe);
                    sw.Close();
                    Thread.Sleep(50);
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = Application.StartupPath + "\\bin\\Confuser.Console.exe ",
                        Arguments = Application.StartupPath + "\\Settings\\security.crproj"
                    };
                    process.StartInfo = startInfo;
                    process.Start();
                    Thread.Sleep(50);

                    Process.Start(Application.StartupPath + "\\" + tnbExe);
                    Thread.Sleep(50);
                    File.Move(Process.GetCurrentProcess().ProcessName + ".exe", "./bin/old");
                    Process.GetCurrentProcess().Kill();
                }
            }*/

            if (!processName.Contains("vshost") && (processName.Contains("bot") || processName.Contains("noob") || processName.Contains("tnb")))
            {
                MessageBox.Show(
                    "You must rename " + Process.GetCurrentProcess().ProcessName + ".exe to something else prior to launch." + Environment.NewLine +
                    "The new name should not contains the words: \"bot, noob, tnb\"", "Security Warning - Follow instructions to avoid automatic detections.");
                Process.GetCurrentProcess().Kill();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var loginForm = new Login();

            /* Begin AutoStart code */
            int sId = Others.ToInt32(ExtractArgs(args, "sessId"));
            if (sId > 0)
            {
                string productName = ExtractArgs(args, "product");
                string profileName = ExtractArgs(args, "profile");
                string battleNet = Others.DecryptString(ExtractArgs(args, "account"));
                string wowEmail = Others.DecryptString(ExtractArgs(args, "email"));
                string wowPassword = Others.DecryptString(ExtractArgs(args, "password"));
                string realmName = Others.DecryptString(ExtractArgs(args, "realm"));
                string character = Others.DecryptString(ExtractArgs(args, "character"));
                bool loginInfoProvided = false;
                Logging.Write("TheNoobBot started automatically. WoW Session Id = " + sId);
                if (!string.IsNullOrEmpty(battleNet) && !string.IsNullOrEmpty(wowEmail) && !string.IsNullOrEmpty(wowPassword) && !string.IsNullOrEmpty(realmName) &&
                    !string.IsNullOrEmpty(character))
                {
                    Logging.Write("The game will be connected automatically with player " + character + " if not yet connected.");

                    ScriptOnlineManager.LoadScript();
                    loginInfoProvided = true;
                }
                loginForm.AutoStart(sId, productName, profileName, battleNet, wowEmail, wowPassword, realmName, character, loginInfoProvided);
            }
            /* End AutoStart code */

            Usefuls.DisableFIPS();
            Application.Run(loginForm);
        }

        private static string ExtractArgs(string[] args, string target)
        {
            // "The Noob Bot.exe" sessId=1337 product=Gatherer profile=TheProfile.xml
            string first = "";
            foreach (string s in args)
            {
                if (s.Contains(target) && s.Contains("="))
                {
                    string ret = s.Split('=')[1];
                    first = ret;
                    break;
                }
            }
            return first;
        }
    }
}