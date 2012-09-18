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
        private WoWUnit _unit;

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

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.IsMounted ||
                    !Products.Products.IsStarted)
                    return false;

                // Check if we have a target, and if this target is hostile or not:
                _unit = null;
                /*if (UnitRelation.GetReaction(ObjectManager.ObjectManager.Me.Faction, ObjectManager.ObjectManager.Target.Faction) == Reaction.Hostile)
                {
                    _unit = ObjectManager.ObjectManager.Me.Target;
                    return true;
                }*/
                if (_unit != null)
                    if (_unit.IsValid)
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

        public override void Run()
        {
            MovementManager.StopMove();
            MovementManager.StopMove();
            Logging.Write("Currently fighting against " + _unit.Name + " (lvl " + _unit.Level + ")");
            Fight.StartFight(_unit.Guid);
            if (_unit.IsDead)
            {
                Statistics.Kills++;
                Thread.Sleep(Usefuls.Latency + 800);
                while (!ObjectManager.ObjectManager.Me.IsMounted && ObjectManager.ObjectManager.Me.InCombat && ObjectManager.ObjectManager.GetUnitAttackPlayer().Count <= 0)
                {
                    Thread.Sleep(10);
                }
                Fight.StopFight();
            }
        }
    }
}
