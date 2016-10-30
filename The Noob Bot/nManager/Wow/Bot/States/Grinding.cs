using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class Grinding : State
    {
        public List<int> EntryTarget = new List<int>();
        public List<uint> FactionsTarget = new List<uint>();
        public uint MaxTargetLevel = 113;
        public uint MinTargetLevel;
        private WoWUnit _unit;

        public override string DisplayName
        {
            get { return "Grinding"; }
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
                if (nManagerSetting.CurrentSetting.DontPullMonsters)
                    return false;

                if (!Usefuls.InGame ||
                    Usefuls.IsLoading ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.InInevitableCombat ||
                    !Products.Products.IsStarted)
                    return false;

                // Get unit:
                _unit = new WoWUnit(0);
                var listUnit = new List<WoWUnit>();
                if (FactionsTarget.Count > 0)
                    listUnit.AddRange(ObjectManager.ObjectManager.GetWoWUnitByFaction(FactionsTarget));
                if (EntryTarget.Count > 0)
                    listUnit.AddRange(ObjectManager.ObjectManager.GetWoWUnitByEntry(EntryTarget));

                _unit = ObjectManager.ObjectManager.GetNearestWoWUnit(listUnit);

                if (!_unit.IsValid)
                    return false;

                if (!_unit.Attackable)
                {
                    Logging.Write("Unit " + _unit.Name + " is non-attackable, blacklisting it.");
                    nManagerSetting.AddBlackList(_unit.Guid, 10*60*1000); // 10 minutes
                    return false;
                    // This may cause issues or not, I'm not sure why it never been there in the first case.
                    // Note: This code handle Grinder's "target selection".
                }

                if (_unit.IsTapped && !_unit.IsTappedByMe)
                    return false;
                if (!nManagerSetting.IsBlackListedZone(_unit.Position) &&
                    _unit.GetDistance < nManagerSetting.CurrentSetting.GatheringSearchRadius &&
                    !nManagerSetting.IsBlackListed(_unit.Guid) && _unit.IsValid)
                    if (_unit.Target == ObjectManager.ObjectManager.Me.Target ||
                        _unit.Target == ObjectManager.ObjectManager.Pet.Target || _unit.Target == 0 ||
                        nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight)
                        if (!_unit.UnitNearest)
                            if (_unit.Level <= MaxTargetLevel && _unit.Level >= MinTargetLevel)
                                return true;

                _unit = new WoWUnit(0);
                return false;
            }
        }

        public override void Run()
        {
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
                if (ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0)
                    Thread.Sleep(Usefuls.Latency + 500);
                // Only sleep that long if we killed the last target.

                while (!ObjectManager.ObjectManager.Me.IsMounted && ObjectManager.ObjectManager.Me.InCombat && ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0)
                {
                    Thread.Sleep(50);
                }
                Fight.StopFight();
            }
        }
    }
}