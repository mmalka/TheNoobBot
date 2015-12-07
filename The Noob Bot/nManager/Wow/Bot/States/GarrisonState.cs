using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class GarrisonState : State
    {
        private const int GarrisonHearthstone = 110560;
        private const int MinerCoffee = 118897;
        private const uint MinerCoffeeBuff = 176049;
        private const int PreservedMiningPick = 118903;
        private const uint PreservedMiningPickBuff = 176061;
        public static Dictionary<string, string> TaskList = new Dictionary<string, string>();

        private static Point _mineEntrance;
        private static GathererProfile _mineProfile;
        private static Point _garden;
        private static int _npcGarden;
        private static int _npcMine;
        private static int _cacheMine;
        private static int _cacheGarden;
        private static List<int> _cacheGarrison;
        private static bool _cacheGardenGathered;
        private static bool _cacheMineGathered;
        private static Point _cacheGarrisonPoint;
        private static bool _oldActivateVeinsHarvesting;
        private static bool _oldActivateHerbsHarvesting;
        private static float _oldGatheringSearchRadius;
        private static Npc _targetNpc = new Npc();
        private static uint _targetBaseAddress;
        private static bool _isCompleted;
        private static MovementLoop _movementLoopState;
        private static readonly Farming FarmingState = new Farming();

        public override string DisplayName
        {
            get { return "GarrisonState"; }
        }

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting || ObjectManager.ObjectManager.Me.IsDeadMe || !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                if (TaskList.Count <= 0)
                    return true;

                if (GetLastTask.Value == "OnGoing")
                {
                    switch (GetLastTask.Key)
                    {
                        case "GatherHerbs":
                            if (FarmingState.NeedToRun)
                            {
                                FarmingState.Run();
                            }
                            else
                            {
                                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
                                Logging.Write(GetLastTask.Key + " terminated.");
                                TaskList[GetLastTask.Key] = "Done";
                            }
                            /*if (IsListObjectGathered(ListHerbsObjects))
                            {
                                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
                                Logging.Write(GetLastTask.Key + " terminated.");
                                TaskList[GetLastTask.Key] = "Done";
                            }*/
                            break;
                        case "GatherMinerals":
                            if (_mineProfile != null && _movementLoopState != null)
                            {
                                if (MovementManager.PointId == _mineProfile.Points.Count - 1)
                                {
                                    nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
                                    Logging.Write(GetLastTask.Key + " terminated.");
                                    TaskList[GetLastTask.Key] = "Done";
                                    nManagerSetting.CurrentSetting.GatheringSearchRadius = _oldGatheringSearchRadius;
                                    return false;
                                }
                                if (ItemsManager.GetItemCount(PreservedMiningPick) > 0 && !ItemsManager.IsItemOnCooldown(PreservedMiningPick) &&
                                    ItemsManager.IsItemUsable(PreservedMiningPick) && !ObjectManager.ObjectManager.Me.HaveBuff(PreservedMiningPickBuff))
                                {
                                    ItemsManager.UseItem(PreservedMiningPick);
                                    Thread.Sleep(50 + Usefuls.Latency);
                                    while (ObjectManager.ObjectManager.Me.IsCast)
                                    {
                                        Thread.Sleep(200);
                                    }
                                }
                                if (ItemsManager.GetItemCount(MinerCoffee) > 0 && !ItemsManager.IsItemOnCooldown(MinerCoffee) &&
                                    ItemsManager.IsItemUsable(MinerCoffee) && ObjectManager.ObjectManager.Me.BuffStack(MinerCoffeeBuff) < 2)
                                {
                                    ItemsManager.UseItem(MinerCoffee);
                                    Thread.Sleep(50 + Usefuls.Latency);
                                    while (ObjectManager.ObjectManager.Me.IsCast)
                                    {
                                        Thread.Sleep(200);
                                    }
                                }
                                if (FarmingState.NeedToRun)
                                    FarmingState.Run();
                                else if (_movementLoopState.NeedToRun)
                                    _movementLoopState.Run();
                                return false;
                            }
                            Logging.Write(GetLastTask.Key + " aborted, did not find profile informations.");
                            TaskList[GetLastTask.Key] = "Done";
                            nManagerSetting.CurrentSetting.GatheringSearchRadius = _oldGatheringSearchRadius;
                            nManagerSetting.CurrentSetting.ActivatePathFindingFeature = true;
                            return false;
                        case "GardenWorkOrder":
                        case "MineWorkOrder":
                        case "CheckGarrisonRessourceCache":
                        case "GoTogarrison":
                            return true; // force run if OnGoing
                    }
                }

                if (GetLastTask.Value != null)
                    return GetLastTask.Value != "OnGoing";

                if (_isCompleted)
                    return false;
                _isCompleted = true;
                // Restore original parameters
                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = _oldActivateVeinsHarvesting;
                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = _oldActivateHerbsHarvesting;
                Logging.Write("The GarrisonFarming task is 100% complete.");
                CloseProduct();
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

        private static void CloseProduct()
        {
            var threadProductStop = new Thread(Products.Products.ProductStopFromFSM);
            threadProductStop.Start();
        }

        public override void Run()
        {
            if (TaskList.Count <= 0)
            {
                // Initialize Tasks
                TaskList.Add("GoToGarrison", "NotStarted");
                Logging.Write("Task GoToGarrison added to the TaskList.");
                TaskList.Add("GatherHerbs", "NotStarted");
                Logging.Write("Task GatherHerbs added to the TaskList.");
                TaskList.Add("GardenWorkOrder", "NotStarted");
                Logging.Write("Task GardenWorkOrder added to the TaskList.");
                TaskList.Add("GatherMinerals", "NotStarted");
                Logging.Write("Task GatherMinerals added to the TaskList.");
                TaskList.Add("MineWorkOrder", "NotStarted");
                Logging.Write("Task MineWorkOrder added to the TaskList.");
                TaskList.Add("CheckGarrisonRessourceCache", "NotStarted");
                Logging.Write("Task CheckGarrisonRessourceCache added to the TaskList.");
                //TaskList.Add("SendFollowersOnDuty", "NotStarted");
                //Logging.Write("Task SendFollowersOnDuty added to the TaskList.");

                // Initialize Settings
                _oldActivateVeinsHarvesting = nManagerSetting.CurrentSetting.ActivateVeinsHarvesting;
                _oldActivateHerbsHarvesting = nManagerSetting.CurrentSetting.ActivateHerbsHarvesting;

                // Initialize Points
                _cacheGarrison = new List<int> {237722, 236916, 237191, 237724, 237720, 237723};
                // All garrison cache ids.
                string playerFaction = ObjectManager.ObjectManager.Me.PlayerFaction;
                if (Garrison.GetGarrisonGardenLevel <= 0)
                {
                    TaskList.Remove("GatherHerbs");
                    TaskList.Remove("GardenWorkOrder");
                    Logging.Write("Task GatherHerbs/GardenWorkOrder removed from to the TaskList, reason: There is no garden in your garrison.");
                }
                if (playerFaction == "Alliance")
                {
                    _npcGarden = 85514;
                    _cacheGarden = 235885;
                    _npcMine = 77730;
                    _cacheMine = 235886;
                    switch (Garrison.GetGarrisonLevel())
                    {
                        case 1:
                            _garden = new Point();
                            _mineEntrance = new Point();
                            _cacheGarrisonPoint = new Point {X = 1899.979f, Y = 93.62897f, Z = 83.52692f};
                            break;
                        case 2:
                            _garden = new Point {X = 1820.335f, Y = 151.5252f, Z = 76.6043f};
                            _mineEntrance = new Point {X = 1912.648f, Y = 92.09844f, Z = 83.5269f};
                            _cacheGarrisonPoint = new Point {X = 1914.361f, Y = 290.3863f, Z = 88.96407f};
                            break;
                        case 3:
                            _garden = new Point {X = 1820.335f, Y = 151.5252f, Z = 76.6043f};
                            _mineEntrance = new Point {X = 1912.648f, Y = 92.09844f, Z = 83.5269f};
                            _cacheGarrisonPoint = new Point {X = 1914.361f, Y = 290.3863f, Z = 88.96407f};
                            break;
                    }
                }
                else
                {
                    _npcGarden = 85783;
                    _cacheGarden = 239238;
                    _npcMine = 81688;
                    _cacheMine = 239237;
                    switch (Garrison.GetGarrisonLevel())
                    {
                        case 1:
                            _garden = new Point();
                            _mineEntrance = new Point();
                            _cacheGarrisonPoint = new Point {X = 5592.229f, Y = 4569.476f, Z = 136.1069f};
                            break;
                        case 2:
                            _garden = new Point {X = 5413.17f, Y = 4558.384f, Z = 139.1283f};
                            _mineEntrance = new Point {X = 5475.534f, Y = 4446.189f, Z = 144.5391f};
                            _cacheGarrisonPoint = new Point {X = 5592.229f, Y = 4569.476f, Z = 136.1069f};
                            break;
                        case 3:
                            _garden = new Point {X = 5413.17f, Y = 4558.384f, Z = 139.1283f};
                            _mineEntrance = new Point {X = 5475.534f, Y = 4446.189f, Z = 144.5391f};
                            _cacheGarrisonPoint = new Point {X = 5592.229f, Y = 4569.476f, Z = 136.1069f};
                            break;
                    }
                }

                if (Garrison.GetGarrisonLevel() == 1)
                {
                    TaskList.Remove("GatherMinerals");
                    TaskList.Remove("MineWorkOrder");

                    TaskList.Remove("GatherHerbs");
                    TaskList.Remove("GardenWorkOrder");
                    Logging.Write("Task GatherMinerals, MineWorkOrder, GatherHerbs, GardenWorkOrder are removed from to the TaskList, reason: Garrison level 1 does not have Mine nor Garden.");
                    return;
                }
                int mineLevel = Garrison.GetGarrisonMineLevel;
                if (_movementLoopState == null || _mineProfile == null)
                {
                    _mineProfile = XmlSerializer.Deserialize<GathererProfile>(Application.StartupPath + "\\Profiles\\GarrisonFarming\\" + playerFaction + "Mine" + mineLevel + ".xml");
                    if (_mineProfile != null && _mineProfile.Points.Count > 0)
                    {
                        _movementLoopState = new MovementLoop {PathLoop = _mineProfile.Points};
                    }
                    else
                    {
                        TaskList.Remove("GatherMinerals");
                        if (mineLevel > 0)
                            Logging.Write("Task GatherMinerals removed from to the TaskList, reason: " + playerFaction + " Mine Profile not found for Mine level: " + mineLevel + ".");
                        else
                        {
                            TaskList.Remove("MineWorkOrder");
                            Logging.Write("Task GatherMinerals/MineWorkOrder removed from to the TaskList, reason: There is no mine in your garrison.");
                        }
                    }
                }
            }
            KeyValuePair<string, string> currentTask = GetLastTask;
            bool success;
            switch (currentTask.Key)
            {
                case "GoToGarrison":
                    if (currentTask.Value == "Not Started")
                    {
                        Logging.Write(currentTask.Key + " started.");
                        TaskList[currentTask.Key] = "OnGoing";
                    }
                    if (Garrison.GarrisonMapIdList.Contains(Usefuls.RealContinentId))
                    {
                        Logging.Write(currentTask.Key + " terminated.");
                        TaskList[currentTask.Key] = "Done";
                        break;
                    }
                    if (ItemsManager.GetItemCount(GarrisonHearthstone) <= 0 || ItemsManager.IsItemOnCooldown(GarrisonHearthstone))
                    {
                        if (Usefuls.ContinentIdByContinentName("Draenor") == Usefuls.ContinentId)
                        {
                            List<Point> pathToGarrison = PathFinder.FindPath(_cacheGarrisonPoint, out success);
                            if (_cacheGarrisonPoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 100)
                            {
                                if (!MovementManager.InMoveTo && success)
                                    MovementManager.Go(pathToGarrison);
                                else if (!success)
                                {
                                    Logging.Write(currentTask.Key + " aborted, no paths.");
                                    TaskList[currentTask.Key] = "Done";
                                    CloseProduct();
                                }
                            }
                            else
                            {
                                Logging.Write(currentTask.Key + " terminated.");
                                TaskList[currentTask.Key] = "Done";
                            }
                        }
                        else
                        {
                            Logging.Write(currentTask.Key + " aborted, you are not in Draenor and don't have a Garrison Hearthstone or it's on Cooldown.");
                            TaskList[currentTask.Key] = "Done";
                            CloseProduct();
                        }
                    }
                    else
                    {
                        ItemsManager.UseItem(GarrisonHearthstone);
                        Logging.Write("Using Garrison Hearthstone to go back to Garrison.");
                        Thread.Sleep(200);
                        while (ObjectManager.ObjectManager.Me.IsCast || Usefuls.IsLoadingOrConnecting)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    break;
                case "GatherMinerals":
                    if (currentTask.Value == "NotStarted")
                    {
                        List<Point> pathToMine = PathFinder.FindPath(_mineEntrance, out success);
                        if (_mineEntrance.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 5)
                        {
                            if (!MovementManager.InMoveTo && success)
                                MovementManager.Go(pathToMine);
                            else if (!success)
                            {
                                Logging.Write(currentTask.Key + " aborted, no paths.");
                                TaskList[currentTask.Key] = "Done";
                            }
                        }
                        else
                        {
                            Logging.Write(currentTask.Key + " started.");
                            TaskList[currentTask.Key] = "OnGoing";
                            _oldGatheringSearchRadius = nManagerSetting.CurrentSetting.GatheringSearchRadius;
                            nManagerSetting.CurrentSetting.GatheringSearchRadius = 13f;
                            nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = true;
                            nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
                        }
                    }
                    break;
                case "MineWorkOrder":
                    if (currentTask.Value == "NotStarted")
                    {
                        Logging.Write(currentTask.Key + " started.");
                        TaskList[currentTask.Key] = "OnGoing";
                    }
                    if (!_cacheMineGathered)
                    {
                        // Gather Mine's Cache
                        if (_targetNpc.Entry != _cacheMine)
                            _targetNpc = new Npc {Entry = _cacheMine, Position = _mineEntrance};
                        _targetBaseAddress = MovementManager.FindTarget(ref _targetNpc);
                        if (MovementManager.InMovement)
                            return;
                        if (_targetBaseAddress > 0 && _targetNpc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 5f)
                        {
                            Interact.InteractWith(_targetBaseAddress, true);
                            Thread.Sleep(Usefuls.Latency + 3500);
                            nManagerSetting.AddBlackList(_targetNpc.Guid, 1000*60*60);
                            _cacheMineGathered = true;
                        }
                        else if (_targetNpc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 5f)
                            _cacheMineGathered = true;
                    }
                    if (!_cacheMineGathered)
                        return;
                    // Send Mine's Work Orders
                    if (_targetNpc.Entry != _npcMine)
                        _targetNpc = new Npc {Entry = _npcMine, Position = _mineEntrance};
                    _targetBaseAddress = MovementManager.FindTarget(ref _targetNpc);
                    if (MovementManager.InMovement)
                        return;
                    if (_targetBaseAddress > 0 && _targetNpc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 5f)
                    {
                        Interact.InteractWith(_targetBaseAddress, true);
                        Thread.Sleep(Usefuls.Latency + 500);
                        Interact.InteractWith(_targetBaseAddress, true);
                        Thread.Sleep(Usefuls.Latency + 1000);
                        Lua.LuaDoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()");
                        Thread.Sleep(Usefuls.Latency + 5000);
                        Logging.Write(currentTask.Key + " terminated.");
                        TaskList[currentTask.Key] = "Done";
                    }
                    break;
                case "GatherHerbs":
                    List<Point> pathToGarden = PathFinder.FindPath(_garden, out success);
                    if (_garden.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 5f)
                    {
                        if (!MovementManager.InMoveTo && success)
                            MovementManager.Go(pathToGarden);
                        else if (!success)
                        {
                            Logging.Write(currentTask.Key + " aborted, no paths.");
                            TaskList[currentTask.Key] = "Done";
                        }
                    }
                    else
                    {
                        Logging.Write(currentTask.Key + " started.");
                        TaskList[currentTask.Key] = "OnGoing";
                        nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
                        nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = true;
                    }
                    break;
                case "GardenWorkOrder":
                    if (currentTask.Value == "NotStarted")
                    {
                        Logging.Write(currentTask.Key + " started.");
                        TaskList[currentTask.Key] = "OnGoing";
                    }
                    if (!_cacheGardenGathered)
                    {
                        // Gather Garden's Cache
                        if (_targetNpc.Entry != _cacheGarden)
                            _targetNpc = new Npc {Entry = _cacheGarden, Position = _garden};
                        _targetBaseAddress = MovementManager.FindTarget(ref _targetNpc);
                        if (MovementManager.InMovement)
                            return;
                        if (_targetBaseAddress > 0 && _targetNpc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 5f)
                        {
                            Interact.InteractWith(_targetBaseAddress, true);
                            Thread.Sleep(Usefuls.Latency + 3500);
                            nManagerSetting.AddBlackList(_targetNpc.Guid, 1000*60*60);
                            _cacheGardenGathered = true;
                        }
                        else if (_targetNpc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 5f)
                            _cacheGardenGathered = true;
                    }
                    if (!_cacheGardenGathered)
                        return;
                    // Send Garden's Work Orders

                    if (_targetNpc.Entry != _npcGarden)
                        _targetNpc = new Npc {Entry = _npcGarden, Position = _garden};
                    _targetBaseAddress = MovementManager.FindTarget(ref _targetNpc);
                    if (MovementManager.InMovement)
                        return;
                    if (_targetBaseAddress > 0 && _targetNpc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 5f)
                    {
                        Interact.InteractWith(_targetBaseAddress, true);
                        Thread.Sleep(Usefuls.Latency + 500);
                        Interact.InteractWith(_targetBaseAddress, true);
                        Thread.Sleep(Usefuls.Latency + 1000);
                        Lua.LuaDoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()");
                        Thread.Sleep(Usefuls.Latency + 5000);
                        Logging.Write(currentTask.Key + " terminated.");
                        TaskList[currentTask.Key] = "Done";
                    }
                    break;
                case "CheckGarrisonRessourceCache":
                    if (currentTask.Value == "NotStarted")
                    {
                        List<Point> pathToGCache = PathFinder.FindPath(_cacheGarrisonPoint);
                        if (_cacheGarrisonPoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 5f)
                        {
                            if (!MovementManager.InMoveTo)
                                MovementManager.Go(pathToGCache);
                        }
                        else if (_cacheGarrisonPoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 5f)
                        {
                            Logging.Write(currentTask.Key + " started.");
                            TaskList[currentTask.Key] = "OnGoing";
                        }
                    }
                    else if (currentTask.Value == "OnGoing")
                    {
                        WoWGameObject o = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectById(_cacheGarrison));
                        if (o.GetBaseAddress <= 0)
                        {
                            Logging.Write(GetLastTask.Key + " terminated.");
                            TaskList[GetLastTask.Key] = "Done";
                        }
                        if (_targetNpc.Entry != o.Entry)
                            _targetNpc = new Npc {Entry = o.Entry, Position = _cacheGarrisonPoint};
                        _targetBaseAddress = MovementManager.FindTarget(ref _targetNpc);
                        if (MovementManager.InMovement)
                            return;
                        if (_targetBaseAddress > 0 && _targetNpc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 5f)
                        {
                            Interact.InteractWith(_targetBaseAddress, true);
                            Thread.Sleep(Usefuls.Latency + 2500);
                            nManagerSetting.AddBlackList(_targetNpc.Guid, 1000*20);
                            Logging.Write(GetLastTask.Key + " terminated.");
                            TaskList[GetLastTask.Key] = "Done";
                        }
                        else if (_targetNpc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 5f)
                        {
                            Logging.Write(GetLastTask.Key + " terminated.");
                            TaskList[GetLastTask.Key] = "Done";
                        }
                    }
                    break;
            }
        }
    }
}