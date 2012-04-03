using System.Collections.Generic;
using Questing_Bot.Bot.Tasks;
using WowManager.FiniteStateMachine;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.WoWObject;
using System.Linq;


namespace Questing_Bot.Bot.States
{
    internal class Looting : State
    {
        private List<WoWUnit> _units;

        public override string DisplayName
        {
            get { return "Looting"; }
        }

        public override int Priority
        {
            get { return (int) States.Priority.Looting; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (Config.Bot.DisableLoot)
                    return false;

                if (!Useful.InGame ||
                    Useful.isLoadingOrConnecting || 
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMount) ||
                    !Config.Bot.BotIsActive)
                    return false;

                // Get Looting
                _units = new List<WoWUnit>();
                var tUnit = ObjectManager.GetWoWUnitLootable();
                if (Config.Bot.FormConfig.Skinning)
                    tUnit.AddRange(ObjectManager.GetWoWUnitSkinnable(new List<ulong>(Config.Bot.BlackListGuid)));

                foreach (var woWUnit in tUnit)
                {
                    if (!Functions.IsInBlackListZone(woWUnit.Position) && woWUnit.GetDistance < Config.Bot.SearchDistance &&
                            !Config.Bot.BlackListGuid.Contains(woWUnit.Guid) &&
                            woWUnit.IsValid)
                        _units.Add(woWUnit);
                }

                if (_units.Count > 0)
                    return true;

                return false;
            }
        }

        public override void Run()
        {
            LootingTask.Pulse(_units);
        }
    }
}