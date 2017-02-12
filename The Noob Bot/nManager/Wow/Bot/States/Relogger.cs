using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;

namespace nManager.Wow.Bot.States
{
    internal class Relogger : State
    {
        public override string DisplayName
        {
            get { return "relogger"; }
        }

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get
            {
                if (!Products.Products.IsStarted)
                    return false;

                if (_reloggerTimer != null && _reloggerTimer.IsReady)
                {
                    if (!_reloggerOff)
                    {
                        Logging.Write("We have tryed to relog for 5minutes without success, stopping Relogger system.");
                        _reloggerOff = true;
                    }
                    return false;
                }

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
        private bool _reloggerOff = false;
        private Helpful.Timer _reloggerTimer;

        public override void Run()
        {
            if (Usefuls.InGame)
                return;

            if (!_relogger)
            {
                Logging.Write("Initiate player relogging.");
                _reloggerTimer = new Helpful.Timer(1000*60*5);
                _reloggerTimer.Reset();
            }

            while (Products.Products.IsStarted)
            {
                Logging.Status = "relogger";

                Login.SettingsLogin s = new Login.SettingsLogin
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
                if (_relogger && Usefuls.InGame && !Usefuls.IsLoading)
                {
                    Thread.Sleep(5000);
                    if (Usefuls.InGame && !Usefuls.IsLoading)
                    {
                        Logging.Write("Ending player relogging with success.");
                        _reloggerTimer = null;
                        _relogger = false;
                        ConfigWowForThisBot.ConfigWow();
                        if (Products.Products.ProductName == "Damage Dealer" && !nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer)
                            ConfigWowForThisBot.StartStopClickToMove(false);
                        if (Products.Products.ProductName == "Heal Bot" && nManagerSetting.CurrentSetting.ActivateMovementsHealerBot)
                            ConfigWowForThisBot.StartStopClickToMove(false);
                        SpellManager.UpdateSpellBook();
                        //Products.Products.ProductRestart();
                        break;
                    }
                }
            }
        }
    }
}