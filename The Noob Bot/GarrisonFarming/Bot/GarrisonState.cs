using System.Collections.Generic;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Bot.States;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace GarrisonFarming.Bot
{
    public class GarrisonState : State
    {
        public static Dictionary<string, string> TaskList = new Dictionary<string, string>();
        private static readonly List<int> ListHerbsObjects = new List<int> {235376, 235387, 235388, 235389, 235390, 235391}; // Plants available in Garrison's Garden.
        private static readonly List<int> ListVeinsObjects = new List<int> {/* mine cart 232541,*/ 232542, 232543, 232544, 232545}; // Veins available in Garrison's Mine.

        private static Point _mineEntrance;
        private static Point _garden;
        private static bool oldActivateVeinsHarvesting;
        private static bool oldActivateHerbsHarvesting;

        public override string DisplayName
        {
            get { return "GarrisonState"; }
        }

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    (ObjectManager.Me.InCombat &&
                     !(ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.IsStarted)
                    return false;

                if (TaskList.Count <= 0)
                    return true;

                if (GetLastTask.Value == "OnGoing")
                {
                    switch (GetLastTask.Key)
                    {
                        case "GatherMinerals":
                            if (IsListObjectGathered(ListVeinsObjects))
                            {
                                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
                                TaskList[GetLastTask.Key] = "Done";
                            }
                            break;
                        case "GatherHerbs":
                            if (IsListObjectGathered(ListHerbsObjects))
                            {
                                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
                                TaskList[GetLastTask.Key] = "Done";
                            }
                            break;
                    }
                }

                if (GetLastTask.Value != null)
                    return GetLastTask.Value != "OnGoing";

                // Restore original parameters
                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = oldActivateVeinsHarvesting;
                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = oldActivateHerbsHarvesting;
                Logging.Write("The GarrisonFarming task is 100% complete.");
                // Cannot close the product from inside an FSM, todo: find a solution
                // Maybe add an infinite task "check missions, send followers on duty".
                return false;
            }
        }

        public static KeyValuePair<string, string> GetLastTask
        {
            get
            {
                foreach (var valuePair in TaskList)
                {
                    if (valuePair.Value == "Done")
                        continue;
                    if (valuePair.Value == "OnGoing")
                        return valuePair;
                    return valuePair;
                }

                return new KeyValuePair<string, string>();
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
            if (TaskList.Count <= 0)
            {
                // Initialize Tasks
                TaskList.Add("GatherHerbs", "NotStarted");
                //TaskList.Add("GardenWorkOrder", "NotStarted");
                //TaskList.Add("GatherMinerals", "NotStarted");
                //TaskList.Add("MineWorkOrder", "NotStarted");
                //TaskList.Add("CheckGarrisonRessourceCache", "NotStarted");
                //TaskList.Add("SendFollowersOnDuty", "NotStarted");

                // Initialize Settings
                oldActivateVeinsHarvesting = nManagerSetting.CurrentSetting.ActivateVeinsHarvesting;
                oldActivateHerbsHarvesting = nManagerSetting.CurrentSetting.ActivateHerbsHarvesting;

                // Initialize Points
                if (ObjectManager.Me.PlayerFaction == "Alliance")
                {
                    switch (Garrison.GetGarrisonLevel())
                    {
                        case 1:
                            _garden = new Point();
                            _mineEntrance = new Point();
                            break;
                        case 2:
                            _garden = new Point();
                            _mineEntrance = new Point();
                            break;
                        case 3:
                            _garden = new Point {X = 1820.335f, Y = 151.5252f, Z = 76.6043f};
                            _mineEntrance = new Point {X = 1912.648f, Y = 92.09844f, Z = 83.5269f};
                            break;
                    }
                }
                else
                {
                    switch (Garrison.GetGarrisonLevel())
                    {
                        case 1:
                            _garden = new Point();
                            _mineEntrance = new Point();
                            break;
                        case 2:
                            _garden = new Point {X = 5413.17f, Y = 4558.384f, Z = 139.1283f};
                            _mineEntrance = new Point {X = 5475.534f, Y = 4446.189f, Z = 144.5391f};
                            break;
                        case 3:
                            _garden = new Point();
                            _mineEntrance = new Point();
                            break;
                    }
                }
            }
            KeyValuePair<string, string> currentTask = GetLastTask;
            switch (currentTask.Key)
            {
                case "GatherMinerals":
                    List<Point> pathToMine = PathFinder.FindPath(_mineEntrance);
                    if (_mineEntrance.DistanceTo(ObjectManager.Me.Position) > 5)
                        if (!MovementManager.InMoveTo)
                            MovementManager.Go(pathToMine);
                        else
                        {
                            TaskList[currentTask.Key] = "OnGoing";
                            nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = true;
                            nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
                        }
                    break;
                case "MineWorkOrder":

                    break;
                case "GatherHerbs":
                    bool succes;
                    List<Point> pathToGarden = PathFinder.FindPath(_garden, out succes);
                    if (_garden.DistanceTo(ObjectManager.Me.Position) > 5)
                    {
                        if (!MovementManager.InMoveTo && succes)
                            MovementManager.Go(pathToGarden);
                    }
                    else
                        TaskList[currentTask.Key] = "OnGoing";
                    nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
                    nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = true;
                    break;
                case "GardenWorkOrder":
                    Logging.Write("awaiting work orders");

                    break;
                case "CheckGarrisonRessourceCache":

                    break;
            }
        }

        public static bool IsListObjectGathered(List<int> listObject)
        {
            List<WoWGameObject> farmableGOs = Farming.GetFarmableGameObjects;
            foreach (WoWGameObject o in farmableGOs)
            {
                foreach (int i in listObject)
                {
                    if (o.Entry == i)
                        return false;
                }
            }
            return true;
        }
    }
}