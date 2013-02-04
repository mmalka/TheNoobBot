using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;

namespace nManager.Wow.Bot.States
{
    internal class relogger : State
    {
        public override string DisplayName
        {
            get { return "relogger"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public override bool NeedToRun
        {
            get
            {
                if (!Products.Products.IsStarted)
                    return false;

                // Need relogger
                if (nManagerSetting.CurrentSetting.ActivateReloggerFeature &&
                    nManagerSetting.CurrentSetting.EmailOfTheBattleNetAccount != string.Empty &&
                    nManagerSetting.CurrentSetting.PasswordOfTheBattleNetAccount != string.Empty)
                    if (!Usefuls.InGame)
                        return true;

                return false;
            }
        }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        private bool _relogger;

        public override void Run()
        {
            if (Usefuls.InGame)
                return;

            if (!_relogger)
                Logging.Write("Relog Player");

            while (Products.Products.IsStarted)
            {
                if (Logging.Status != "relogger")
                    Logging.Status = "relogger";

                var s = new Login.SettingsLogin
                            {
                                Login = nManagerSetting.CurrentSetting.EmailOfTheBattleNetAccount,
                                Password = nManagerSetting.CurrentSetting.PasswordOfTheBattleNetAccount,
                                Realm = Usefuls.RealmName,
                                Character = Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule +
                                                                                   (uint) Addresses.Player.playerName),
                                BNetName = nManagerSetting.CurrentSetting.BattleNetSubAccount,
                            };

                Login.Pulse(s);
                _relogger = true;
                if (_relogger && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    Thread.Sleep(5000);
                    if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                    {
                        Logging.Write("Relog Player Finished, Restarting bot");
                        _relogger = false;
                        ConfigWowForThisBot.ConfigWow();
                        //Products.Products.ProductRestart();
                        break;
                    }
                }
            }
        }
    }
}