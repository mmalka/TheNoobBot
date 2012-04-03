using System.Threading;
using Battleground_Bot.Profile;

namespace Battleground_Bot.API
{
    public static class Useful
    {
        /// <summary>
        /// Launches create profile.
        /// </summary>
        public static void LaunchCreateProfile()
        {
            var worker = new Thread(ShowCreateProfile) {IsBackground = true, Name = "CreateProfile"};
            worker.Start();
        }

        private static void ShowCreateProfile()
        {
            var formProfil = new ProfileManager();
            formProfil.Show();
        }
    }
}