using System.Collections.Generic;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
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

        public static bool IsLooting;

        public Looting()
        {
            if (ItemsManager.HasToy(109167))
                LootingTask.LootARangeId = 109167;
            else if (ItemsManager.HasToy(60854))
                LootingTask.LootARangeId = 60854;
            else
                LootingTask.LootARangeId = 0;
        }

        public static List<int> ForceLootCreatureList = new List<int>();

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                {
                    // no need to load that list if we activated looting.
                    if (ForceLootCreatureList.Count <= 0)
                    {
                        Logging.Write("Loading ForceLootCreatureList...");
                        string[] forceLootCreatureList = Others.ReadFileAllLines(Application.StartupPath + "\\Data\\ForceLootCreatureList.txt");
                        for (int i = 0; i <= forceLootCreatureList.Length - 1; i++)
                        {
                            int creatureId = Others.ToInt32(forceLootCreatureList[i]);
                            if (creatureId > 0 && !ForceLootCreatureList.Contains(creatureId))
                                ForceLootCreatureList.Add(creatureId);
                        }
                        if (ForceLootCreatureList.Count > 0)
                            Logging.Write("Loaded " + ForceLootCreatureList.Count + " creatures to force loot even if loot is deactivated. (high reward)");
                    }
                }

                if (!Usefuls.InGame ||
                    Usefuls.IsLoading ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.InInevitableCombat ||
                    Usefuls.IsFlying ||
                    !Products.Products.IsStarted)
                    return false;

                if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft)
                    return false; // Make ToTown a priority.

                // Get Looting
                _units = new List<WoWUnit>();
                List<WoWUnit> tUnit = ObjectManager.ObjectManager.GetWoWUnitLootable();
                if (nManagerSetting.CurrentSetting.ActivateBeastSkinning &&
                    nManagerSetting.CurrentSetting.BeastNinjaSkinning)
                    tUnit.AddRange(
                        ObjectManager.ObjectManager.GetWoWUnitSkinnable(
                            new List<UInt128>(nManagerSetting.GetListGuidBlackListed())));

                foreach (WoWUnit woWUnit in tUnit)
                {
                    if (!nManagerSetting.CurrentSetting.ActivateMonsterLooting && !ForceLootCreatureList.Contains(woWUnit.Entry))
                        continue;
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
            IsLooting = true;
            LootingTask.Pulse(_units);
            IsLooting = false;
        }
    }
}