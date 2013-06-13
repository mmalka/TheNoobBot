using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Wow.Bot.Tasks;
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

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                    return false;

                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    Usefuls.IsFlying ||
                    !Products.Products.IsStarted)
                    return false;

                // Get Looting
                _units = new List<WoWUnit>();
                var tUnit = ObjectManager.ObjectManager.GetWoWUnitLootable();
                if (nManagerSetting.CurrentSetting.ActivateBeastSkinning &&
                    nManagerSetting.CurrentSetting.BeastNinjaSkinning)
                    tUnit.AddRange(
                        ObjectManager.ObjectManager.GetWoWUnitSkinnable(
                            new List<ulong>(nManagerSetting.GetListGuidBlackListed())));

                foreach (var woWUnit in tUnit)
                {
                    if (woWUnit.GetDistance2D <= nManagerSetting.CurrentSetting.GatheringSearchRadius &&
                        !nManagerSetting.IsBlackListed(woWUnit.Guid) &&
                        woWUnit.IsValid &&
                        (!woWUnit.UnitNearest || woWUnit.GetDistance2D < 15))
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

        public override void Run()
        {
            LootingTask.Pulse(_units);
        }
    }
}