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

        public List<int> EntryTarget = new List<int>();
        public List<uint> FactionsTarget = new List<uint>();
        public uint MinTargetLevel;
        public uint MaxTargetLevel = 90;
        private WoWUnit _unit;

        public override bool NeedToRun
        {
            get
            {
                if (nManagerSetting.CurrentSetting.DontPullMonsters)
                    return false;

                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                // Get unit:
                _unit = new WoWUnit(0);
                List<WoWUnit> listUnit = new List<WoWUnit>();
                if (FactionsTarget.Count > 0)
                    listUnit.AddRange(ObjectManager.ObjectManager.GetWoWUnitByFaction(FactionsTarget));
                if (EntryTarget.Count > 0)
                    listUnit.AddRange(ObjectManager.ObjectManager.GetWoWUnitByEntry(EntryTarget));

                _unit = ObjectManager.ObjectManager.GetNearestWoWUnit(listUnit);

                if (!_unit.IsValid)
                    return false;

                if (!nManagerSetting.IsBlackListedZone(_unit.Position) &&
                    _unit.GetDistance2D < nManagerSetting.CurrentSetting.GatheringSearchRadius &&
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
            if (!_unit.IsDead && unkillableMob != 0)
            {
                Logging.Write("Can't reach " + _unit.Name + ", blacklisting it.");
                nManagerSetting.AddBlackList(unkillableMob, 2*60*1000); // 2 minutes
            }
            else if (_unit.IsDead)
            {
                Statistics.Kills++;
                Thread.Sleep(Usefuls.Latency + 1000);
                while (!ObjectManager.ObjectManager.Me.IsMounted && ObjectManager.ObjectManager.Me.InCombat &&
                       ObjectManager.ObjectManager.GetUnitAttackPlayer().Count <= 0)
                {
                    Thread.Sleep(50);
                }
                Fight.StopFight();
            }
        }
    }
}