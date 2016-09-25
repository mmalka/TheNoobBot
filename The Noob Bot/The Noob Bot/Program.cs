using System;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace The_Noob_Bot
{
    internal static class Program
    {
        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var loginForm = new Login();

            /* Begin AutoStart code */
            int sId = Others.ToInt32(ExtractArgs(args, "sessId"));
            if (sId > 0)
            {
                string productName = ExtractArgs(args, "product");
                string profileName = ExtractArgs(args, "profile");
                Logging.Write("TheNoobBot started automatically. WoW Session Id = " + sId);
                loginForm.AutoStart(sId, productName, profileName);
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