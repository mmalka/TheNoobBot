using System;
using System.Linq;
using System.Windows.Forms;
using nManager.Helpful;

namespace The_Noob_Bot
{
    internal static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var loginForm = new Login();

            /* Begin AutoStart code */
            int sId = ExtractSessId(args);
            if (sId > 0)
            {
                Logging.Write("TheNoobBot started automatically. WoW Session Id = " + sId);

                loginForm.AutoStart(sId);
            }
            /* End AutoStart code */

            Application.Run(loginForm);
        }

        private static int ExtractSessId(string[] args)
        {
            // "The Noob Bot.exe" sessId=1337 product=Gatherer profile=TheProfile.xml
            return args.Length <= 0 ? 0 : (from s in args where s.Contains("sessId") && s.Contains("=") select s.Split('=')[1] into sessId select Others.ToInt32(sessId)).FirstOrDefault();
        }
    }
}