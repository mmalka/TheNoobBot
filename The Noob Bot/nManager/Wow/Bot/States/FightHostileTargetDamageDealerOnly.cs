using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class FightHostileTargetDamageDealerOnly : State
    {
        private WoWUnit _unit;

        public override string DisplayName
        {
            get { return "FightHostileTargetDamageDealerOnly"; }
        }

        public override int Priority { get; set; }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public override bool NeedToRun
        {
            get
            {
                if (Usefuls.BadBottingConditions)
                    return false;
                _unit = AcquireTarger();
                return _unit.IsValid;
            }
        }

        public override void Run()
        {
            if (ObjectManager.ObjectManager.Me.InCombat && ObjectManager.ObjectManager.Me.IsMounted)
                MountTask.DismountMount();
            else if (ObjectManager.ObjectManager.Me.IsMounted)
            {
                Logging.Write("Please dismount as soon as is possible ! This product is passive when you are not yet in combat.");
                Thread.Sleep(500);
            }
            Logging.Write("Currently attacking " + _unit.Name + " (lvl " + _unit.Level + ")");
            UInt128 unkillableMob = Fight.StartFightDamageDealer(_unit.Guid);
            if (!_unit.IsDead && unkillableMob != 0)
            {
                Logging.Write("Can't reach " + _unit.Name + ", blacklisting it.");
                return;
            }
            if (_unit.IsDead)
            {
                Statistics.Kills++;
                _unit = AcquireTarger();
                if (_unit.IsValid)
                    Run();
                return;
            }
        }

        public WoWUnit AcquireTarger()
        {
            if (nManagerSetting.CurrentSetting.DontPullMonsters && !ObjectManager.ObjectManager.Target.InCombat)
            {
                Fight.StopFight();
                return new WoWUnit(0);
            }

            // Get unit:
            WoWUnit localUnit = ObjectManager.ObjectManager.Target;

            if (localUnit.IsValid && localUnit.IsAlive && localUnit.Health > 0 && ((localUnit.Attackable && localUnit.IsHostile) || localUnit.IsUnitBrawlerAndTappedByMe))
                return localUnit;

            Fight.StopFight();
            return new WoWUnit(0);
        }
    }
}