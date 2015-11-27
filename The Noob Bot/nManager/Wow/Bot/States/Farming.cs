using System.Collections.Generic;
using System.Linq;
using nManager.FiniteStateMachine;
using nManager.Helpful;
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

        public override int Priority { get; set; }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        //private static List<int> _listDisplayIdFarm;
        private static List<WoWGameObject> _nodes = new List<WoWGameObject>();

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

                _nodes = GetFarmableGameObjects;
                return _nodes.Count > 0;
            }
        }

        public static List<WoWGameObject> GetFarmableGameObjects
        {
            get
            {
                var nodes = new List<WoWGameObject>();
                List<WoWGameObject> tNodes = ObjectManager.ObjectManager.GetWoWGameObjectForFarm();

                for (int i = 0; i < tNodes.Count; i++)
                {
                    WoWGameObject node = tNodes[i];
                    if (!node.IsValid)
                        continue;
                    if (nManagerSetting.IsBlackListed(node.Guid) || nManagerSetting.IsBlackListedZone(node.Position))
                        continue;
                    if (node.GetDistance2D > nManagerSetting.CurrentSetting.GatheringSearchRadius)
                        continue;
                    if (!node.CanOpen) continue;
                    if (PlayerNearest(node)) continue;
                    if (node.UnitNearest) continue;
                    nodes.Add(node);
                }
                return tNodes;
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