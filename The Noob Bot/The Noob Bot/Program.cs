using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace The_Noob_Bot
{
    using System.Security.AccessControl;

    internal static class Program
    {
        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var processName = Process.GetCurrentProcess().ProcessName.ToLower();
            if (!processName.Contains("vshost") && (processName.Contains("bot") || processName.Contains("noob") || processName.Contains("tnb")))
            {
                MessageBox.Show(
                    "You must rename " + Process.GetCurrentProcess().ProcessName + ".exe to something else prior to launch." + Environment.NewLine +
                    "The new name should not contains the words: \"bot, noob, tnb\"", "Security Warning - Follow instructions to avoid automatic detections.");
                Process.GetCurrentProcess().Kill();
            }
            if (!processName.Contains("vshost"))
            {
                var account = Others.GetProcessOwner(Process.GetCurrentProcess().Id);
                Others.RemoveFileSecurity(Process.GetCurrentProcess().ProcessName + ".exe", account, FileSystemRights.ReadData, AccessControlType.Allow);
                Others.AddFileSecurity(Process.GetCurrentProcess().ProcessName + ".exe", account, FileSystemRights.ReadData, AccessControlType.Deny);
                DialogResult resulMb = MessageBox.Show(
                    "World of Warcraft 7.3 is able to detect any hacks or bots, including passives tools. (pixels bot, etc) " +
                    Environment.NewLine +
                    "Do you want to ignore this warning and run the bot anyway ?",
                    @"WARNING / ATTENTION / ARTUNG / внимание / 注意",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (resulMb != DialogResult.Yes)
                {
                    MessageBox.Show("You probably made the right decision. We'll notify you if we find a way to effectively protect the bot against detections.");
                    Process.GetCurrentProcess().Kill();
                }
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