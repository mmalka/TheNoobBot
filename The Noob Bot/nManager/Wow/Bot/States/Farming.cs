using System.Collections.Generic;
using System.Linq;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class Farming : State
    {
        public override string DisplayName
        {
            get { return "Farming"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        //private static List<int> _listDisplayIdFarm;
        private List<WoWGameObject> _nodes;

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                if (!nManagerSetting.CurrentSetting.ActivateHerbsHarvesting &&
                    !nManagerSetting.CurrentSetting.ActivateVeinsHarvesting &&
                    !nManagerSetting.CurrentSetting.ActivateChestLooting)
                    return false;

                if (LongMove.IsLongMove && !nManagerSetting.CurrentSetting.HarvestDuringLongDistanceMovements)
                    return false;

                _nodes = new List<WoWGameObject>();
                List<WoWGameObject> tNodes = ObjectManager.ObjectManager.GetWoWGameObjectForFarm();

                foreach (var node in tNodes)
                {
                    if (!nManagerSetting.IsBlackListedZone(node.Position) &&
                        node.GetDistance2D < nManagerSetting.CurrentSetting.GatheringSearchRadius &&
                        !nManagerSetting.IsBlackListed(node.Guid) && node.IsValid)
                        if (node.CanOpen)
                            if (!PlayerNearest(node))
                                if (!node.UnitNearest)
                                    _nodes.Add(node);
                }

                if (_nodes.Count > 0)
                    return true;

                return false;
            }
        }

        public static bool PlayerNearest(WoWGameObject node)
        {
            List<WoWPlayer> players = ObjectManager.ObjectManager.GetObjectWoWPlayer();
            if (
                players.Any(
                    p =>
                    p.Position.DistanceTo2D(node.Position) <=
                    nManagerSetting.CurrentSetting.DontHarvestIfPlayerNearRadius))
            {
                Logging.Write("Player near the node");
                nManagerSetting.AddBlackList(node.Guid, 15*1000);
                return true;
            }
            return false;
        }

        public override void Run()
        {
            Tasks.FarmingTask.Pulse(_nodes);
        }
    }
}