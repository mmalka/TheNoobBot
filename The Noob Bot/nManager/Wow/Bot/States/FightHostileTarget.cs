using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class FightHostileTarget : State
    {
        public override string DisplayName
        {
            get { return "FightHostileTarget"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        private WoWUnit _unit;

        public override bool NeedToRun
        {
            get
            {
                if (nManagerSetting.CurrentSetting.DontPullMonsters)
                    return false;

                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting || ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid || !Products.Products.IsStarted)
                    return false;

                // Get unit:
                _unit = ObjectManager.ObjectManager.Target;

                if (_unit.IsValid && !_unit.IsDead && _unit.IsAlive && _unit.Health > 0)
                    if (_unit.Reaction == Reaction.Hostile || _unit.Reaction == Reaction.Hated || _unit.Reaction == Reaction.Neutral)
                        return true;
                _unit = new WoWUnit(0);
                return false;
            }
        }

        public override void Run()
        {
            MovementManager.StopMove();
            Logging.Write("Player Attack " + _unit.Name + " (lvl " + _unit.Level + ")");
            ulong unkillableMob = Fight.StartFightDamageDealer(_unit.Guid);
            if (!_unit.IsDead && unkillableMob != 0)
            {
                Logging.Write("Can't reach " + _unit.Name + ", blacklisting it.");
            }
            else if (_unit.IsDead)
            {
                Statistics.Kills++;
                Thread.Sleep(Usefuls.Latency + 1000);
                while (ObjectManager.ObjectManager.Me.InCombat &&
                       ObjectManager.ObjectManager.GetUnitAttackPlayer().Count > 0)
                {
                    Thread.Sleep(50);
                }
                Fight.StopFight();
            }
        }
    }
}