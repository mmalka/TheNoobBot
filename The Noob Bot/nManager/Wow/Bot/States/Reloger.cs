using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;

namespace nManager.Wow.Bot.States
{
    class Reloger : State
    {
        public override string DisplayName
        {
            get { return "Reloger"; }
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

                // Need Reloger
                if (nManagerSetting.CurrentSetting.reloger && nManagerSetting.CurrentSetting.accountEmail != string.Empty && nManagerSetting.CurrentSetting.accountPassword != string.Empty)
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

        private bool _reloger;
        public override void Run()
        {
            if (Usefuls.InGame)
                return;

            if (!_reloger)
                Logging.Write("Relog Player");

            while (Products.Products.IsStarted)
            {
                if (Logging.Status != "Reloger")
                    Logging.Status = "Reloger";

                var s = new Login.SettingsLogin
                { 
                    Login = nManagerSetting.CurrentSetting.accountEmail,
                    Password = nManagerSetting.CurrentSetting.accountPassword,
                    Realm = Usefuls.RealmName,
                    Character = Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule +
                                                                            (uint)Addresses.Player.playerName),
                    BNetName = nManagerSetting.CurrentSetting.bNetName,
                };

                Login.Pulse(s);
                _reloger = true;
                if (_reloger && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    Thread.Sleep(5000);
                    if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                    {
                        Logging.Write("Relog Player Finished, Restarting bot");
                        _reloger = false;
                        ConfigWowForThisBot.ConfigWow();
                        //Products.Products.ProductRestart();
                        break;
                    }
                }
            }
        }
    }
}
