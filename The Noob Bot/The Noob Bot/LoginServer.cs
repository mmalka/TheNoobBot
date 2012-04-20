using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Wow;
using Process = System.Diagnostics.Process;
using Usefuls = nManager.Wow.Helpers.Usefuls;

namespace The_Noob_Bot
{
    internal static class LoginServer
    {
        private const string UrlWebServer = "http://tech.thenoobbot.com/";
        private const string ScripLogintUrl = UrlWebServer + "auth.php";
        private const string ScripUpdate = UrlWebServer + "update.php";
        private const string ScripServerIsOnline = UrlWebServer + "isOnline.php";
        private const string AccountSecurityLog = UrlWebServer + "AccountSecurity.log";
        private const string ScripServerMyIp = UrlWebServer + "myIp.php";
        internal static int StartTime;

        private static string _ip;
        internal static bool IsConnected { get; set; }

        internal static bool IsFreeVersion { get; private set; }
        internal static string Login = "";
        internal static string Password = "";
        private const string Secret = "vsdGDFDSFF4578sGDSD65csds89df4sfcds65vF";
        private static string TrueResultLoop = "";

        internal static bool IsOnlineserver;

        internal static void Connect(string login, string password)
        {
            try
            {
                if (login == "" || password == "")
                {
                    MessageBox.Show(Translate.Get(Translate.Id.User_name_or_Password_error) + ".", Translate.Get(Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.GetCurrentProcess().Kill();
                    return;
                }
                Login = login.ToLower();
                Password = password;

                var connectThreadLaunch = new Thread(ConnectThread) { Name = "ConnectThread" };
                connectThreadLaunch.Start();
            }
            catch (Exception e)
            {
                Logging.WriteError("FDsojfOFDSiojfzeosqodifjksdfjsij: " + e);
            }
        }

        private static void ConnectThread()
        {
            string repC = "";
            try
            {
                try
                {
                    _ip = GetReqWithAuthHeader(ScripServerMyIp, "", "")[1];
                    var resultConnectReq = GetReqWithAuthHeader(ScripLogintUrl + "?create=true", Login, Password);
                    var goodResultConnectReq = Others.EncrypterMD5(Secret + _ip + Login);
                    repC = resultConnectReq[1];

                    var randomKey = Others.Random(1, 9999);
                    var resultRandom = GetReqWithAuthHeader(ScripLogintUrl + "?random=true", randomKey.ToString(CultureInfo.InvariantCulture), randomKey.ToString(CultureInfo.InvariantCulture));
                    string goodResultRandomTry = Others.EncrypterMD5((randomKey * 4) + Secret);

                    if (resultRandom[0] == goodResultRandomTry && resultConnectReq[0] == goodResultConnectReq)
                    {
                        TrueResultLoop = goodResultConnectReq;
                        var connectThreadLaunch = new Thread(LoopThread) { Name = "LoopLogin" };
                        connectThreadLaunch.Start();
                        return;
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("DFiosdfosfIDFsDIODJFsios#1: " + e);
                }

                // Error
                if (repC == "SubscriptionNotValide")
                {
                    MessageBox.Show(
                        Translate.Get(Translate.Id.Subscription_finished__renew_it_if_you_want_use_no_limited_version_of_the_tnb_again_here) + ": http://thenoobbot.com/.",
                        Translate.Get(Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(
                        Translate.Get(Translate.Id.You_starting_trial_version__the_tnb_will_automatically_stopped_after____min),
                        Translate.Get(Translate.Id.Trial_version), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    IsFreeVersion = true;
                    Trial();
                    return;
                }

                if (repC == "PasswordError")
                    MessageBox.Show(Translate.Get(Translate.Id.Incorrect_password__go_to_this_address_if_you_have_forget_your_password) + ": http://thenoobbot.com/login/?action=lostpassword", Translate.Get(Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (repC == "LoginError")
                    MessageBox.Show(Translate.Get(Translate.Id.Incorrect_user_name__go_here_if_you_want_create_an_account_and_buy_The_Noob_Bot) + ": http://thenoobbot.com/", Translate.Get(Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(Translate.Get(Translate.Id.Login_error__try_to_disable_your_antivirus__go_to_the_website_if_you_need_help) + ": http://thenoobbot.com/", Translate.Get(Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Error);

                Process.GetCurrentProcess().Kill();
            }
            catch (Exception e)
            {
                Logging.WriteError("DFiosdfosfIDFsDIODJFsios#2: " + e);
            }
        }

        private static bool _trial;
        internal static void Trial()
        {
            try
            {
                _trial = true;
                var connectThreadLaunch = new Thread(LoopThread) { Name = "LoopLogin" };
                connectThreadLaunch.Start();
            }
            catch (Exception e)
            {
                Logging.WriteError("DSFskljfIODdfsdttrSFDfsfsd: " + e);
            }
        }

        private static int _levelStat;
        private static int _honnorStat;
        private static int _expStat;
        private static int _farmStat;
        private static int _killStat;

        private static void LoopThread()
        {
            try
            {
                bool lastResult = true;
                IsConnected = true;
                StartTime = Others.Times;
                
                if (!_trial)
                {
                    Thread.Sleep(10000);
                    while (true)
                    {
                        while (!Memory.WowMemory.ThreadHooked)
                        {
                            Thread.Sleep(3000);
                            lastResult = true;
                        }
                        while (!Usefuls.InGame)
                        {
                            Thread.Sleep(3000);
                            lastResult = true;
                        }

                        // Statistique
                        string reqStatistique = "?";
                        try
                        {
                            if (_levelStat == 0)
                            {
                                _honnorStat = Usefuls.GetHonorPoint;
                                _expStat = nManager.Wow.ObjectManager.ObjectManager.Me.Experience;
                                _farmStat = (int)Statistics.Farms;
                                _killStat = (int)Statistics.Kills;
                                _levelStat = (int)nManager.Wow.ObjectManager.ObjectManager.Me.Level;
                            }
                            // Level
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.Level - _levelStat > 0)
                            {
                                reqStatistique += "level=" +
                                                  (nManager.Wow.ObjectManager.ObjectManager.Me.Level - _levelStat);
                                _levelStat = (int) nManager.Wow.ObjectManager.ObjectManager.Me.Level;
                            }
                            else
                            {
                                reqStatistique += "level=0";
                            }
                            // Honnor
                            if (Usefuls.GetHonorPoint - _honnorStat > 0)
                            {
                                reqStatistique += "&honnor=" + (Usefuls.GetHonorPoint - _honnorStat);
                                _honnorStat = Usefuls.GetHonorPoint;
                            }
                            else
                            {
                                if (Usefuls.GetHonorPoint < _honnorStat)
                                    _honnorStat = Usefuls.GetHonorPoint;

                                reqStatistique += "&honnor=0";
                            }
                            // Exp
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.Experience - _expStat > 0)
                            {
                                reqStatistique += "&exp=" +
                                                  (nManager.Wow.ObjectManager.ObjectManager.Me.Experience - _expStat);
                                _expStat = nManager.Wow.ObjectManager.ObjectManager.Me.Experience;
                            }
                            else
                            {
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.Experience < _expStat)
                                    _expStat = nManager.Wow.ObjectManager.ObjectManager.Me.Experience;

                                reqStatistique += "&exp=0";
                            }
                            // Farm
                            if (Statistics.Farms - _farmStat > 0)
                            {
                                reqStatistique += "&farm=" + (Statistics.Farms - _farmStat);
                                _farmStat = (int) Statistics.Farms;
                            }
                            else
                            {
                                if (Statistics.Farms < _farmStat)
                                    _farmStat = (int) Statistics.Farms;

                                reqStatistique += "&farm=0";
                            }
                            // Kill
                            if (Statistics.Kills - _killStat > 0)
                            {
                                reqStatistique += "&kill=" + (Statistics.Kills - _killStat);
                                _killStat = (int) Statistics.Kills;
                            }
                            else
                            {
                                if (Statistics.Kills < _killStat)
                                    _killStat = (int) Statistics.Kills;

                                reqStatistique += "&kill=0";
                            }
                            
                        } catch
                        {
                            reqStatistique = "";
                        }
                        // End Statistique

                        string resultReqLoop = GetReqWithAuthHeader(ScripLogintUrl + reqStatistique, Login, Password)[0];
                        if (TrueResultLoop != resultReqLoop)
                        {
                            if (!lastResult)
                                break;
                            if (_ip != GetReqWithAuthHeader(ScripServerMyIp, "", "")[1])
                            {
                                while (!ServerIsOnline())
                                {
                                    Thread.Sleep(10000);
                                }
                                Connect(Login, Password);
                                return;
                            }
                            lastResult = false;
                        }
                        else
                            lastResult = true;
                        Thread.Sleep(55000);
                    }
                }
                else
                {
                    int msToSec = 1;
                    msToSec = msToSec*1000;
                    int secToMin = 2;
                    secToMin = secToMin*30;
                    int i = 0;
                    while (i < secToMin * 20)
                    {
                        Thread.Sleep(msToSec);
                        i++;
                    } 
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("DSfi^sdfDSOfijfze#1" + e);
            }
            IsConnected = false;
            if (IsFreeVersion)
            {
                Others.OpenWebBrowserOrApplication("http://thenoobbot.com/get-a-bg-bot-wow/");
                Logging.WriteFileOnly("Trial period finished.");
            }
            else
                Logging.WriteError("Connection error , close The Noob Bot. #sdfezFsd");
            try
            {
                Pulsator.Dispose(true);
            }
            catch
            {
            }
            Process.GetCurrentProcess().Kill();
        }

        internal static void CheckUpdate()
        {
            try
            {
                var checkUpdateThreadLaunch = new Thread(CheckUpdateThread) { Name = "CheckUpdate" };
                checkUpdateThreadLaunch.Start();
            }
            catch (Exception e)
            {
                Logging.WriteError("LoginServer > CheckUpdate(): " + e);
            }
        }

        private static void CheckUpdateThread()
        {
            try
            {
                string resultReq = Others.GetRequest(ScripUpdate, "null=null");
                if (resultReq != null)
                {
                    if (resultReq.Count() < 100 && resultReq.Any())
                    {
                        if (resultReq != Information.Version)
                        {
                            DialogResult dr =
                                MessageBox.Show(
                                    string.Format(Translate.Get(Translate.Id.New_update) + ": \"{0}\". " + Translate.Get(Translate.Id.Do_you_want_to_update_now) + "? {1}", resultReq, Environment.NewLine), " " + Translate.Get(Translate.Id.Update) + "", MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);

                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    Others.OpenWebBrowserOrApplication("http://thenoobbot.com/");
                                    Others.OpenWebBrowserOrApplication("http://thenoobbot.com/downloads/The_Noob_Bot-" + resultReq + ".rar");

                                    try
                                    {
                                        foreach (var process in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
                                        {
                                            if (process.Id != Process.GetCurrentProcess().Id)
                                                process.Kill();
                                        }

                                    }
                                    catch
                                    {
                                    }
                                    Process.GetCurrentProcess().Kill();
                                    break;
                                case DialogResult.No:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("LoginServer > CheckUpdateThread(): " + e);
            }
        }


        internal static void CheckAccountSecurity()
        {
            try
            {
                var checkAccountSecurityThreadLaunch = new Thread(CheckAccountSecurityThread) { Name = "CheckAccountSecurity" };
                checkAccountSecurityThreadLaunch.Start();
            }
            catch (Exception e)
            {
                Logging.WriteError("LoginServer > CheckAccountSecurity(): " + e);
            }
        }

        private static void CheckAccountSecurityThread()
        {
            try
            {
                string resultReq = Others.GetRequest(AccountSecurityLog, "");
                if (resultReq != null)
                {
                    if (resultReq.Any())
                    {
                        if (resultReq != Information.Version)
                        {
                            var dr = MessageBox.Show(Translate.Get(Translate.Id.The_game_has_an_suspect_activity_it_is_recommended_to_closing_the_game_and_tnb_for_your_account_security_Click_on__Yes__to_close_tnb) + ". ", "/!\\ " + Translate.Get(Translate.Id.Suspect_Activity) + " /!\\", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    Memory.WowProcess.KillWowProcess();
                                    Process.GetCurrentProcess().Kill();
                                    break;
                                case DialogResult.No:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("LoginServer > CheckAccountSecurityThread(): " + e);
            }
        }

        internal static void CheckServerIsOnline()
        {
            try
            {
                var checkUpdateThreadLaunch = new Thread(CheckServerIsOnlineThread) { Name = "CheckIsOnline" };
                checkUpdateThreadLaunch.Start();
            }
            catch (Exception e)
            {
                Logging.WriteError("FDsdfezfDFezfzefe: " + e);
            }
        }

        private static bool ServerIsOnline()
        {
            try
            {
                string resultReq = Others.GetRequest(ScripServerIsOnline, "null=null");
                if (resultReq != null)
                {
                    if (resultReq.Count() < 100)
                    {
                        if (resultReq == "true")
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("FDSfzefDSVSVFEFsdfsfvzs: " + e);
            }
            return false;
        }
        private static void CheckServerIsOnlineThread()
        {
            try
            {
                if (ServerIsOnline())
                {
                    IsOnlineserver = true;
                    return;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("LoginServer > CheckServerIsOnlineThread(): " + e);
            }
            MessageBox.Show(Translate.Get(Translate.Id.TheNoobBot_s_server_seems_to_be_down__you_may_try_to_disable_your_Anti_virus_or_Firewall_and_try_again__Note__This_version_may_have_been_blocked_from_our_servers_due_to_a_Suspect_Activity_or_crack_attempt__you_can_check_if_a_new_version_is_available_on_our_Website_or_check_our_forum_in_News_cat), Translate.Get(Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Error);
            Process.GetCurrentProcess().Kill();
        }

        static List<string> GetReqWithAuthHeader(string url, String userName, String userPassword)
        {
            try
            {
                return Others.GetReqWithAuthHeader(url, userName, userPassword);
            }
            catch (Exception e)
            {
                Logging.WriteError("DVSVsVrfvgrerfgd'(gtedf;: " + e);
                return new List<string>();
            }
        }
    }

}
