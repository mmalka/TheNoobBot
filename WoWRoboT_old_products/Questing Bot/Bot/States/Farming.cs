using System.Collections.Generic;
using Questing_Bot.Bot.Tasks;
using WowManager.FiniteStateMachine;
using WowManager.MiscEnums;
using WowManager.MiscStructs;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;
using WowManager.WoW.WoWObject;

namespace Questing_Bot.Bot.States
{
    class Farming : State
    {
        public override string DisplayName
        {
            get { return "Farming"; }
        }

        public override int Priority
        {
            get { return (int)States.Priority.Farming; }
        }

        private static List<int> _listDisplayIdFarm;
        private List<WoWGameObject> _nodes;

        public override bool NeedToRun
        {
            get
            {
                if (!Useful.InGame ||
                    Useful.isLoadingOrConnecting ||
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMount) ||
                    !Config.Bot.BotIsActive)
                    return false;

                // Get farm:
                _listDisplayIdFarm = new List<int>();
                if (Config.Bot.FormConfig.FarmHerb)
                {
                    _listDisplayIdFarm.AddRange(NobesList.GetListId("Herb", Skill.GetValue(SkillLine.Herbalism)));
                }
                if (Config.Bot.FormConfig.FarmMine)
                {
                    _listDisplayIdFarm.AddRange(NobesList.GetListId("Mineral", Skill.GetValue(SkillLine.Mining)));
                }
                _nodes = new List<WoWGameObject>();
                List<WoWGameObject> tNodes = ObjectManager.GetWoWGameObjectById(_listDisplayIdFarm);

                foreach (var node in tNodes)
                {
                    if (!Functions.IsInBlackListZone(node.Position) && node.GetDistance < Config.Bot.SearchDistance && !Config.Bot.BlackListGuid.Contains(node.Guid) && node.IsValid)
                        _nodes.Add(node);
                }

                if (_nodes.Count > 0)
                    return true;

                return false;
            }
        }

        public override void Run()
        {
            Tasks.FarmingTask.Pulse(_nodes);
        }
    }
}
