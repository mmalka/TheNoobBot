using System.Collections.Generic;
using System.Threading;
using System.Linq;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
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
                if (nManagerSetting.CurrentSetting.DontPullMonsters)
                    return false;

                if (!Usefuls.InGame || Usefuls.IsLoading || ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid || !Products.Products.IsStarted)
                    return false;

                // Get unit:
                _unit = ObjectManager.ObjectManager.Target;

                if (_unit.IsValid && !_unit.IsDead && _unit.IsAlive && _unit.Health > 0 && _unit.Attackable)
                    return true;

                // If in party, then search for the target if one member is in combat
                if (Party.IsInGroup())
                {
                    List<WoWUnit> targets = new List<WoWUnit>();
                    foreach (UInt128 playerGuid in Party.GetPartyPlayersGUID())
                    {
                        if (playerGuid != ObjectManager.ObjectManager.Me.Guid)
                        {
                            WoWPlayer p = ObjectManager.ObjectManager.GetObjectWoWPlayer(playerGuid);
                            if (p != null && p.Target != 0 && p.InCombatBlizzard && p.GetDistance < 40)
                            {
                                WoWObject o = ObjectManager.ObjectManager.GetObjectByGuid(p.Target);
                                if (o == null)
                                    break;
                                WoWUnit u = new WoWUnit(o.GetBaseAddress);
                                if (u.IsValid && !u.IsDead && u.IsAlive && u.Health > 0 && u.Attackable)
                                    targets.Add(u);
                            }
                        }
                    }
                    // Now take the most occuring unit in the list
                    if (targets.Count > 0)
                    {
                        _unit = targets.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
                        return true;
                    }
                }
                _unit = new WoWUnit(0);
                return false;
            }
        }

        public override void Run()
        {
            MountTask.DismountMount();
            if (ObjectManager.ObjectManager.Me.IsMounted)
                return;
            Logging.Write("Player Attack " + _unit.Name + " (lvl " + _unit.Level + ")");
            UInt128 unkillableMob = Fight.StartFight(_unit.Guid);
            if (!_unit.IsDead && unkillableMob != 0 && _unit.HealthPercent == 100.0f)
            {
                Logging.Write("Can't reach " + _unit.Name + ", blacklisting it.");
                nManagerSetting.AddBlackList(unkillableMob, 2*60*1000); // 2 minutes
            }
            else if (_unit.IsDead)
            {
                Statistics.Kills++;
                Thread.Sleep(Usefuls.Latency + 1000);
                while (ObjectManager.ObjectManager.Me.InCombat &&
                       ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                {
                    Thread.Sleep(50);
                }
                Fight.StopFight();
            }
        }
    }
}