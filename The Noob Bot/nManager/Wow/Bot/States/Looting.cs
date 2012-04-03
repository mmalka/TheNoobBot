using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class Looting : State
    {
        private List<WoWUnit> _units;

        public override string DisplayName
        {
            get { return "Looting"; }
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
                if (!nManagerSetting.CurrentSetting.lootMobs)
                    return false;

                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) ||
                    Usefuls.IsFlying ||
                    !Products.Products.IsStarted)
                    return false;

                // Get Looting
                _units = new List<WoWUnit>();
                var tUnit = ObjectManager.ObjectManager.GetWoWUnitLootable();
                if (nManagerSetting.CurrentSetting.skinMobs && nManagerSetting.CurrentSetting.skinNinja)
                    tUnit.AddRange(ObjectManager.ObjectManager.GetWoWUnitSkinnable(new List<ulong>(nManagerSetting.GetListGuidBlackListed())));

                foreach (var woWUnit in tUnit)
                {
                    if (woWUnit.GetDistance2D <= nManagerSetting.CurrentSetting.searchRadius &&
                            !nManagerSetting.IsBlackListed(woWUnit.Guid) &&
                            woWUnit.IsValid &&
                            (!UnitNearest(woWUnit) || woWUnit.GetDistance2D < 15))
                        _units.Add(woWUnit);
                }

                if (_units.Count > 0)
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

        public static bool UnitNearest(WoWUnit unit)
        {
            List<WoWUnit> units = ObjectManager.ObjectManager.GetObjectWoWUnit();
            var i = 0;
            foreach (var woWUnit in units)
            {
                if (woWUnit.Position.DistanceTo2D(unit.Position) <= woWUnit.AggroDistance && UnitRelation.GetReaction(ObjectManager.ObjectManager.Me, unit) == Reaction.Hostile)
                    i++;
            }
            var r = i > nManagerSetting.CurrentSetting.maxUnitsNear;
            if (r)
            {
                nManagerSetting.AddBlackList(unit.Guid, 50 * 1000);
                Logging.Write(i + " Units hostile Near " + unit.Name);
            }
            return r;
        }

        public override void Run()
        {
            LootingTask.Pulse(_units);
        }
    }
}
