using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WowManager;
using WowManager.FiniteStateMachine;
using WowManager.Others;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;

namespace Questing_Bot.Bot.States
{
    class Reloger : State
    {
        public override string DisplayName
        {
            get { return "Reloger"; }
        }

        public override int Priority
        {
            get { return (int)States.Priority.Reloger; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Config.Bot.BotIsActive)
                    return false;

                // Need Reloger
                if (Config.Bot.FormConfig.Relog != string.Empty && Config.Bot.FormConfig.ReLogActive)
                    if (!Useful.InGame)
                        return true;

                return false;
            }
        }

        private bool _reloger;
        public override void Run()
        {
            if (Useful.InGame)
                return;

            if (!_reloger)
                Log.AddLog(Translation.GetText(Translation.Text.Relog_Player));

            while (Config.Bot.BotIsActive)
            {
                string fileLog = Config.Bot.FormConfig.Relog;
                string[] fileLogArray = fileLog.Split(Convert.ToChar("#"));
                var s = new Login.SettingsLogin
                            {
                                Username = Others.EncryptStringToString(fileLogArray[0]),
                                Password = Others.EncryptStringToString(fileLogArray[1]),
                                Realm = Others.EncryptStringToString(fileLogArray[3]),
                                Character = Others.EncryptStringToString(fileLogArray[2]),
                            };

                Login.Pulse(s);
                _reloger = true;
                if (_reloger && Useful.InGame && !Useful.isLoadingOrConnecting)
                {
                    Thread.Sleep(5000);
                    if (Useful.InGame && !Useful.isLoadingOrConnecting)
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.Relog_player_finish_restart_bot));
                        _reloger = false;
                        ConfigWowForWowManager.ConfigWow();
                        break;
                    }
                }
            }
        }
    }
}
