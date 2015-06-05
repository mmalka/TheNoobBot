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
using InteractGame = System.Threading.Thread;
using HookInfoz = System.Diagnostics.Process;
using Timer = nManager.Helpful.Timer;

namespace The_Noob_Bot
{
    internal static class LoginServer
    {
        private static readonly string[] LocalStatusList =
        {
            "NOKConnect",
            "SNVConnect",
            "OKConnect",
            "PEConnect",
            "LEConnect"
        };

        internal static int StartTime;

        private static string _ip;
        internal static bool IsConnected { get; set; }

        internal static bool IsFreeVersion { get; private set; }

        internal static string Login = "";
        internal static string Password = "";
        internal static string HardwareKey = "";
        private const string Secret = "0e8897c8c73772e72d81dd28ebf57ac3";
        private static string TrueResultLoop = "";

        internal static bool IsOnlineserver;
        private static readonly Thread LoginThread = new Thread(LoopThreads) {Name = "LoopLogon"};

        internal static void Connect(string login, string password)
        {
            try
            {
                if (HardwareKey == "")
                    HardwareKey = Others.GetRandomString(20);
                if (login == "" || (password == "" && login.Length != 20))
                {
                    MessageBox.Show(Translate.Get(Translate.Id.User_name_or_Password_error) + ".",
                        Translate.Get(Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.GetCurrentProcess().Kill();
                    return;
                }
                Login = login.ToLower();
                Password = password;

                Thread connectThreadLaunch = new Thread(ConnectThread) {Name = "ConnectThread"};
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
                    _ip = GetReqWithAuthHeader(Others.GetClientIPScriptLink, Login, Password)[1];
                    List<string> resultConnectReq = GetReqWithAuthHeader(Others.GetAuthScriptLink + "?create=true&HardwareKey=" + HardwareKey, Login, Password);
                    string goodResultConnectReq = Others.EncrypterMD5(Secret + _ip + Login + HardwareKey);
                    repC = resultConnectReq[1];

                    int randomKey = Others.Random(1, 9999);
                    List<string> resultRandom = GetReqWithAuthHeader(Others.GetAuthScriptLink + "?random=true",
                        randomKey.ToString(CultureInfo.InvariantCulture),
                        randomKey.ToString(CultureInfo.InvariantCulture));
                    string goodResultRandomTry = Others.EncrypterMD5((randomKey*4) + Secret);

                    if (resultRandom[0] == goodResultRandomTry && resultConnectReq[0] == goodResultConnectReq)
                    {
                        TrueResultLoop = goodResultConnectReq;
                        Thread connectThreadLaunch = new Thread(LoopThread) {Name = "LoopLogin"};
                        connectThreadLaunch.Start();
                        return;
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("DFiosdfosfIDFsDIODJFsios#1: " + e);
                }

                // Error
                if (repC == LocalStatusList[1])
                {
                    MessageBox.Show(
                        Translate.Get(
                            Translate.Id.
                                Subscription_finished__renew_it_if_you_want_use_no_limited_version_of_the_tnb_again_here) +
                        ": http://thenoobbot.com/.",
                        Translate.Get(Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(
                        Translate.Get(
                            Translate.Id.You_starting_trial_version__the_tnb_will_automatically_stopped_after____min),
                        Translate.Get(Translate.Id.Trial_version), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    IsFreeVersion = true;
                    Trial();
                    return;
                }

                if (repC == LocalStatusList[3])
                    MessageBox.Show(
                        Translate.Get(
                            Translate.Id.Incorrect_password__go_to_this_address_if_you_have_forget_your_password) +
                        ": http://thenoobbot.com/login/?action=lostpassword", Translate.Get(Translate.Id.Error),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (repC == LocalStatusList[4])
                    MessageBox.Show(
                        Translate.Get(
                            Translate.Id.Incorrect_user_name__go_here_if_you_want_create_an_account_and_buy_The_Noob_Bot) +
                        ": http://thenoobbot.com/", Translate.Get(Translate.Id.Error), MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                else
                    MessageBox.Show(
                        Translate.Get(
                            Translate.Id.Login_error__try_to_disable_your_antivirus__go_to_the_website_if_you_need_help) +
                        ": http://thenoobbot.com/", Translate.Get(Translate.Id.Error), MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                EndInformation();
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
                Thread connectThreadLaunch = new Thread(LoopThread) {Name = "LoopLogin"};
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
// ReSharper disable ConvertToConstant.Local
            bool lalala = true;
// ReSharper restore ConvertToConstant.Local
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
                                if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting && Usefuls.GetHonorPoint >= 0)
                                    _honnorStat = Usefuls.GetHonorPoint;
                                else _honnorStat = -1;
                                _expStat = nManager.Wow.ObjectManager.ObjectManager.Me.Experience;
                                _farmStat = (int) Statistics.Farms;
                                _killStat = (int) Statistics.Kills;
                                _levelStat = (int) nManager.Wow.ObjectManager.ObjectManager.Me.Level;
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
                            if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting || Usefuls.GetHonorPoint < 0)
                            {
                                reqStatistique += "&honnor=0";
                            }
                            else if (Usefuls.GetHonorPoint >= 0)
                            {
                                if (_honnorStat >= 0)
                                {
                                    if (Usefuls.GetHonorPoint < _honnorStat)
                                    {
                                        _honnorStat = Usefuls.GetHonorPoint;
                                        reqStatistique += "&honnor=0";
                                    }
                                    else if (Usefuls.GetHonorPoint - _honnorStat > 0)
                                    {
                                        reqStatistique += "&honnor=" + (Usefuls.GetHonorPoint - _honnorStat);
                                        _honnorStat = Usefuls.GetHonorPoint;
                                    }
                                }
                                else if (_honnorStat < 0)
                                {
                                    _honnorStat = Usefuls.GetHonorPoint;
                                    reqStatistique += "&honnor=0";
                                }
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
                        }
                        catch
                        {
                            reqStatistique = "";
                        }

                        if (!LoginThread.IsAlive)
                            LoginThread.Start();

                        // End Statistique

                        string resultReqLoop =
                            GetReqWithAuthHeader(Others.GetAuthScriptLink + reqStatistique + "&HardwareKey=" + HardwareKey, Login, Password)[0];
                        if (TrueResultLoop != resultReqLoop)
                        {
                            if (!lastResult)
                                break;
                            if (_ip != GetReqWithAuthHeader(Others.GetClientIPScriptLink, Login, Password)[1])
                            {
                                while (!ServerIsOnline())
                                {
                                    InteractGame.Sleep(10000);
                                }
                                Connect(Login, Password);
                                return;
                            }
                            lastResult = false;
                        }
                        else
                            lastResult = true;
                        Others.LUAGlobalVarDestructor();
                        InteractGame.Sleep(55000);
                    }
                }
                else
                {
                    DoLoginCheck();

                    int i = 0;
                    do
                    {
                        InteractGame.Sleep(0x3E8);
                        i++;
                        if (!LoginThread.IsAlive)
                            LoginThread.Start();
                    } while (i < (0x98B >> 1));
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("DSfi^sdfDSOfijfze#1" + e);
            }
            IsConnected = false;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (lalala && IsFreeVersion)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                Others.OpenWebBrowserOrApplication("http://goo.gl/Fzdgc");
            }
            else
                Logging.WriteError("#tuk51t6#Connection error, close The Noob Bot. #sdffzFsd");

            EndInformation();
        }

        private static void LoopThreads()
        {
            while (true)
            {
                while (Usefuls.InGame)
                {
                    Statistics.OffsetStats = 0xB5;
                    Thread.Sleep(500);
                }
                Thread.Sleep(500);
            }
            //LoginThread.Abort();
        }

        internal static void EndInformation()
        {
            HookWowQ();
            if (!isProcessKilled())
                KillApplication();

            try
            {
                Application.Exit();
            }
            catch
            {
            }
            Reboot();
            Overload();
        }

        public static bool isProcessKilled()
        {
            return false;
        }

        public static void KillProcess()
        {
        }

        public static void KillApplication()
        {
            KillProcess();
        }

        private static void Reboot()
        {
            try
            {
                Application.ExitThread();
            }
            catch
            {
            }
        }

        private static void Overload()
        {
            try
            {
                Environment.Exit(0);
            }
            catch
            {
            }
        }

        private static void HookWowQ()
        {
            if (true)
            {
                try
                {
                    Pulsator.Dispose(true);
                }
                catch
                {
                }
                HookInfoz.GetCurrentProcess().Kill();
            }
        }

        private static void DoLoginCheck()
        {
        }

        internal static void CheckUpdate()
        {
            try
            {
                Thread checkUpdateThreadLaunch = new Thread(CheckUpdateThread) {Name = "CheckUpdate"};
                checkUpdateThreadLaunch.Start();
            }
            catch /*(Exception e)*/
            {
                //Logging.WriteError("LoginServer > CheckUpdate(): " + e);
            }
        }

        private static void CheckUpdateThread()
        {
            try
            {
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (Others.EncrypterMD5(Information.Version) == "5006678f64f53edfaa26f2587c559d1e")
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                    return;
#pragma warning disable 162
// ReSharper disable HeuristicUnreachableCode
                string resultReq = Others.GetRequest(Others.GetUpdateScriptLink, "null=null");
                if (resultReq != null)
                {
                    if (resultReq.Count() < 100 && resultReq.Any())
                    {
                        if (resultReq != Information.Version)
                        {
                            string resultDesc = Others.GetRequest(Others.GetUpdateScriptLink, "show=desc");
                            string resultLog = Others.GetRequest(Others.GetUpdateScriptLink, "show=changelog");
                            DialogResult dr =
                                MessageBox.Show(
                                    string.Format("{0}{1}{4}{4}{2}{4}{3}{4}{5}", Translate.Get(Translate.Id.LatestUpdateVersion), resultReq,
                                        Translate.Get(Translate.Id.LatestUpdateDescription), resultDesc, Environment.NewLine, Translate.Get(Translate.Id.ConfirmUpdate)),
                                    Translate.Get(Translate.Id.LatestUpdateTitle), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    Others.OpenWebBrowserOrApplication(resultLog);
                                    Others.OpenWebBrowserOrApplication("http://thenoobbot.com/downloads/latest.php");

                                    try
                                    {
                                        foreach (
                                            Process process in
                                                Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
                                        {
                                            if (process.Id != Process.GetCurrentProcess().Id)
                                                process.Kill();
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    EndInformation();
                                    break;
                                case DialogResult.No:
                                    break;
                            }
                        }
                    }
                }
// ReSharper restore HeuristicUnreachableCode
#pragma warning restore 162
            }
            catch /*(Exception e)*/
            {
                //Logging.WriteError("LoginServer > CheckUpdateThread(): " + e);
            }
        }

        internal static void CheckServerIsOnline()
        {
            try
            {
                Thread checkUpdateThreadLaunch = new Thread(CheckServerIsOnlineThread) {Name = "CheckIsOnline"};
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
                string resultReq = Others.GetRequest(Others.GetMonitoringScriptLink, "null=null");
                if (resultReq != null)
                {
                    if (resultReq.Count() < 10)
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
            MessageBox.Show(Translate.Get(Translate.Id.TheNoobBotServerIsOffline), Translate.Get(Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Error);
            EndInformation();
        }

        private static List<string> GetReqWithAuthHeader(string url, String userName, String userPassword)
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