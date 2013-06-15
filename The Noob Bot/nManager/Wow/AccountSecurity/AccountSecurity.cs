using System;
using System.Diagnostics;
using System.Windows.Forms;
using nManager.Helpful;

namespace nManager.Wow.AccountSecurity
{
    internal class AccountSecurity
    {
        internal static void DialogNewScan()
        {
            try
            {
                Memory.WowProcess.KillWowProcess();
                MessageBox.Show(
                    Translate.Get(
                        Translate.Id.Suspect_activity_of_the_game_which_haven_t_verified_yet__Closing_game_and_tnb));
                Logging.Write(
                    Translate.Get(
                        Translate.Id.Suspect_activity_of_the_game_which_haven_t_verified_yet__Closing_game_and_tnb));
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception exception)
            {
                Logging.WriteError("AccountSecurity > DialogNewScan(): " + exception);
            }
        }

        public static void Pulse()
        {
            try
            {
                HookAccountSecurity.Pulse();
            }
            catch (Exception exception)
            {
                Logging.WriteError("AccountSecurity > Pulse(): " + exception);
            }
        }

        public static void Dispose()
        {
            try
            {
                HookAccountSecurity.DisposeHook();
            }
            catch (Exception exception)
            {
                Logging.WriteError("AccountSecurity > Dispose(): " + exception);
            }
        }
    }
}