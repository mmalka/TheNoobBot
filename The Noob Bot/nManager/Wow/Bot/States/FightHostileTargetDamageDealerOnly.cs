using System.Collections.Generic;
using System.Threading;
using System.Linq;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class FightHostileTargetDamageDealerOnly : State
    {
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

        private WoWUnit _unit;

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting || ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid || !Products.Products.IsStarted)
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
                Logging.Write("Please dismount as soon as is possible ! This product is passive.");
            Logging.Write("Currently attacking " + _unit.Name + " (lvl " + _unit.Level + ")");
            ulong unkillableMob = Fight.StartFightDamageDealer(_unit.Guid);
            if (!_unit.IsDead && unkillableMob != 0)
            {
                Logging.Write("Can't reach " + _unit.Name + ", blacklisting it.");
            }
            else if (_unit.IsDead)
            {
                Statistics.Kills++;
                _unit = AcquireTarger();
                if (_unit.IsValid)
                    Run();
                Fight.StopFight();
            }
        }

        public WoWUnit AcquireTarger()
        {
            if (nManagerSetting.CurrentSetting.DontPullMonsters && !ObjectManager.ObjectManager.Target.InCombat)
                return new WoWUnit(0);
            if (ObjectManager.ObjectManager.Me.Target == 0)
                return new WoWUnit(0);

            // Get unit:
            var localUnit = ObjectManager.ObjectManager.Target;

            if (localUnit.IsValid && localUnit.IsAlive && localUnit.Health > 0 && localUnit.Attackable &&
                ((localUnit is WoWPlayer && (localUnit as WoWPlayer).PlayerFaction != ObjectManager.ObjectManager.Me.PlayerFaction) || localUnit.Reaction <= Reaction.Neutral))
                return localUnit;

            // If in party, then search for the target if one member is in combat
            if (Party.IsInGroup())
            {
                List<WoWUnit> targets = new List<WoWUnit>();
                foreach (ulong playerGuid in Party.GetPartyPlayersGUID())
                {
                    if (playerGuid != ObjectManager.ObjectManager.Me.Guid)
                    {
                        WoWPlayer p = ObjectManager.ObjectManager.GetObjectWoWPlayer(playerGuid);
                        if (p != null && p.InCombat && p.Target != 0)
                        {
                            WoWObject o = ObjectManager.ObjectManager.GetObjectByGuid(p.Target);
                            if (o == null)
                                break;
                            WoWUnit u = new WoWUnit(o.GetBaseAddress);
                            if (u.IsValid && !u.IsDead && u.IsAlive && u.Health > 0 && u.Attackable &&
                                ((u is WoWPlayer && (u as WoWPlayer).PlayerFaction != ObjectManager.ObjectManager.Me.PlayerFaction) || u.Reaction <= Reaction.Neutral))
                                targets.Add(u);
                        }
                    }
                }
                // Now take the most occuring unit in the list
                if (targets.Count > 0)
                {
                    localUnit = targets.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
                    return localUnit;
                }
            }
            return new WoWUnit(0);
        }
    }
}