using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using ObjMgr = nManager.Wow.ObjectManager.ObjectManager;
using nManager.Wow.Bot.Tasks;

namespace nManager.Wow.Bot.States
{
    public class GarrisonState : State
    {
        private const int GarrisonHearthstone = 110560;
        private const int MinerCoffee = 118897;
        private const uint MinerCoffeeBuff = 176049;
        private const int PreservedMiningPick = 118903;
        private const uint PreservedMiningPickBuff = 176061;
        private static Stack<Task> tList = null;
        private static Task previousTask = Task.None;
        private static Point _mineEntrance;
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
        private static readonly Farming FarmingState = new Farming();

        public GarrisonState()
        {
            // Save settings and disable farming at instance creation
            _oldActivateVeinsHarvesting = nManagerSetting.CurrentSetting.ActivateVeinsHarvesting;
            _oldActivateHerbsHarvesting = nManagerSetting.CurrentSetting.ActivateHerbsHarvesting;
            _oldGatheringSearchRadius = nManagerSetting.CurrentSetting.GatheringSearchRadius;
            nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
            nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
        }

        public override string DisplayName
        {
            get { return "GarrisonState"; }
        }

        public override int Priority { get; set; }

        private enum Task : int
        {
            None = 0,
            GoToGarrison = 1,
            GatherHerbs = 2,
            GardenWorkOrder = 3,
            GatherMinerals = 4,
            MineWorkOrder = 5,
            CheckGarrisonRessourceCache = 6
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting || ObjMgr.Me.IsDeadMe || !ObjMgr.Me.IsValid ||
                    (ObjMgr.Me.InCombat && !(ObjMgr.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted ||
                    Usefuls.IsFlying))) || !Products.Products.IsStarted)
                    return false;
                // Not initialized, then run
                if (tList == null)
                    return true;
                // Task list empty, then we did finish
                if (tList.Count == 0)
                    return false;
                return true;
            }
        }

        public override void Run()
        {
            if (MovementManager.InMovement)
                return;
            // Now configure if not already done
            if (tList == null)
            {
                // Fill the task list
                tList = new Stack<Task>();
                tList.Push(Task.CheckGarrisonRessourceCache);
                if (Garrison.GetGarrisonLevel() > 1)
                {
                    tList.Push(Task.MineWorkOrder);
                    tList.Push(Task.GatherMinerals);
                    tList.Push(Task.GardenWorkOrder);
                    tList.Push(Task.GatherHerbs);
                }
                tList.Push(Task.GoToGarrison);
                // And fill the variables
                _cacheGarrison = new List<int> { 237722, 236916, 237191, 237724, 237720, 237723 };
                if (ObjMgr.Me.PlayerFaction == "Alliance")
                {
                    _npcGarden = 85514;
                    _cacheGarden = 235885;
                    _npcMine = 77730;
                    _cacheMine = 235886;
                    _garden = new Point { X = 1833.85f, Y = 154.7408f, Z = 76.66339f };
                    _mineEntrance = new Point { X = 1886.021f, Y = 83.23455f, Z = 84.31888f };
                    _cacheGarrisonPoint = new Point { X = 1914.361f, Y = 290.3863f, Z = 88.96407f };
                }
                else
                {
                    _npcGarden = 85783;
                    _cacheGarden = 239238;
                    _npcMine = 81688;
                    _cacheMine = 239237;
                    _garden = new Point { X = 5413.795f, Y = 4548.928f, Z = 139.1232f };
                    _mineEntrance = new Point { X = 5465.796f, Y = 4430.045f, Z = 145.4595f };
                    _cacheGarrisonPoint = new Point { X = 5592.229f, Y = 4569.476f, Z = 136.1069f };
                }
                _cacheGardenGathered = false;
                _cacheMineGathered = false;
            }
            bool success, display = false;
            Point me = ObjMgr.Me.Position;
            Task currentTask = tList.Peek();
            if (currentTask != previousTask)
            {
                previousTask = currentTask;
                display = true;
            }
            switch (currentTask)
            {
                case Task.GoToGarrison:
                    if (display)
                        Logging.Write("Task: go to garrison");
                    if (!Garrison.GarrisonMapIdList.Contains(Usefuls.RealContinentId))
                    {
                        // We are in Draenor
                        if (Usefuls.ContinentId == Usefuls.ContinentIdByContinentName("Draenor"))
                        {
                            // We can fly, then fly
                            if (MountTask.GetMountCapacity() == MountCapacity.Fly)
                            {
                                LongMove.LongMoveGo(_cacheGarrisonPoint);
                                return;
                            }
                            // else if not too far go by foot
                            else if (_cacheGarrisonPoint.DistanceTo(me) < 100)
                            {
                                List<Point> pathToGarrison = PathFinder.FindPath(_cacheGarrisonPoint, out success);
                                if (success)
                                {
                                    MovementManager.Go(pathToGarrison);
                                    return;
                                }
                            }
                        }
                        // We have to use the garrison hearthstone
                        if (ItemsManager.GetItemCount(GarrisonHearthstone) > 0 && !ItemsManager.IsItemOnCooldown(GarrisonHearthstone))
                        {
                            Logging.Write("Using garrison Hearthstone");
                            ItemsManager.UseItem(GarrisonHearthstone);
                        }
                        else
                        {
                            Logging.Write("Run aborted, you are not in Draenor or too far away and don't known how to fly and don't have a Garrison Hearthstone or it's on Cooldown.");
                            tList.Clear();
                            break; // prevent poping an empry stack, jump to the end
                        }
                    }
                    tList.Pop();
                    break;
                case Task.CheckGarrisonRessourceCache:
                    if (display)
                        Logging.Write("Task: gather garrison cache");
                    if (_cacheGarrisonPoint.DistanceTo(me) > 75.0f)
                    {
                        List<Point> pathToGCache = PathFinder.FindPath(_cacheGarrisonPoint);
                        MovementManager.Go(pathToGCache);
                        return;
                    }
                    WoWGameObject cache = ObjMgr.GetNearestWoWGameObject(ObjMgr.GetWoWGameObjectById(_cacheGarrison));
                    if (cache.GetBaseAddress != 0)
                    {
                        if (cache.Position.DistanceTo(me) > 5.0f)
                        {
                            _targetNpc = new Npc { Entry = cache.Entry, Position = cache.Position };
                            MovementManager.FindTarget(ref _targetNpc, 5f);
                            return;
                        }
                        Interact.InteractWith(cache.GetBaseAddress, true);
                    }
                    tList.Pop();
                    break;
                case Task.GatherHerbs:
                    if (display)
                        Logging.Write("Task: gather plants in garrison garden");
                    if (_garden.DistanceTo(me) > 15.0f)
                    {
                        List<Point> pathToGarden = PathFinder.FindPath(_garden, out success); // assume success
                        MovementManager.Go(pathToGarden);
                        return;
                    }
                    nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = true;
                    nManagerSetting.CurrentSetting.GatheringSearchRadius = 30f;
                    if (!FarmingState.NeedToRun) // Nothing anymore to farm, then next task
                    {
                        Logging.Write("Finished to farm garrison garden");
                        nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
                        tList.Pop();
                    }
                    break;
                case Task.GatherMinerals:
                    if (display)
                        Logging.Write("Task: gather ores and carts in garrison mine");
                    if (_mineEntrance.DistanceTo(me) > 15.0f)
                    {
                        List<Point> pathToMine = PathFinder.FindPath(_mineEntrance, out success); // assume success
                        MovementManager.Go(pathToMine);
                        return;
                    }
                    nManagerSetting.CurrentSetting.GatheringSearchRadius = 120f;
                    nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = true;
                    if (FarmingState.NeedToRun)
                    {
                        Logging.Write("Take coffee and Mining Pick buffs");
                        if (ItemsManager.GetItemCount(PreservedMiningPick) > 0 && !ItemsManager.IsItemOnCooldown(PreservedMiningPick) &&
                            ItemsManager.IsItemUsable(PreservedMiningPick) && !ObjMgr.Me.HaveBuff(PreservedMiningPickBuff))
                        {
                            ItemsManager.UseItem(PreservedMiningPick);
                            Thread.Sleep(150 + Usefuls.Latency);
                            while (ObjMgr.Me.IsCast)
                            {
                                Thread.Sleep(150);
                            }
                            Thread.Sleep(1000);
                        }
                        if (ItemsManager.GetItemCount(MinerCoffee) > 0 && !ItemsManager.IsItemOnCooldown(MinerCoffee) &&
                            ItemsManager.IsItemUsable(MinerCoffee) && ObjMgr.Me.BuffStack(MinerCoffeeBuff) < 2)
                        {
                            ItemsManager.UseItem(MinerCoffee);
                            Thread.Sleep(150 + Usefuls.Latency);
                            while (ObjMgr.Me.IsCast)
                            {
                                Thread.Sleep(150);
                            }
                        }
                    }
                    else // Nothing anymore to farm, then next task
                    {
                        Logging.Write("Finished to farm garrison mine");
                        nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
                        tList.Pop();
                    }
                    break;
                case Task.GardenWorkOrder:
                    if (display)
                        Logging.Write("Task: collect garden cache and send work order");
                    if (!_cacheGardenGathered)
                    {
                        WoWGameObject gardenCache = ObjMgr.GetNearestWoWGameObject(ObjMgr.GetWoWGameObjectById(_cacheGarden));
                        if (gardenCache.GetBaseAddress != 0)
                        {
                            if (gardenCache.Position.DistanceTo(me) > 5.0f)
                            {
                                _targetNpc = new Npc { Entry = gardenCache.Entry, Position = gardenCache.Position };
                                MovementManager.FindTarget(ref _targetNpc, 5f);
                                return;
                            }
                            else
                            {
                                Thread.Sleep(Usefuls.Latency + 250);
                                Interact.InteractWith(gardenCache.GetBaseAddress, true);
                                _cacheGardenGathered = true;
                                Thread.Sleep(Usefuls.Latency + 1750);
                            }
                        }
                    }
                    WoWUnit gardenNpc = ObjMgr.GetNearestWoWUnit(ObjMgr.GetWoWUnitByEntry(_npcGarden));
                    if (gardenNpc.GetBaseAddress != 0)
                    {
                        if (gardenNpc.Position.DistanceTo(me) > 5.0f)
                        {
                            _targetNpc = new Npc { Entry = gardenNpc.Entry, Position = gardenNpc.Position };
                            MovementManager.FindTarget(ref _targetNpc, 5f);
                            return;
                        }
                        else
                        {
                            Interact.InteractWith(gardenNpc.GetBaseAddress, true);
                            Thread.Sleep(Usefuls.Latency + 1000);
                            Interact.InteractWith(gardenNpc.GetBaseAddress, true);
                            Thread.Sleep(Usefuls.Latency + 500);
                            Lua.LuaDoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()");
                            Thread.Sleep(Usefuls.Latency + 1000);
                        }
                    }
                    tList.Pop();
                    break;
                case Task.MineWorkOrder:
                    if (display)
                        Logging.Write("Task: collect mine cache and send work order");
                    if (!_cacheMineGathered)
                    {
                        WoWGameObject mineCache = ObjMgr.GetNearestWoWGameObject(ObjMgr.GetWoWGameObjectById(_cacheMine));
                        if (mineCache.GetBaseAddress != 0)
                        {
                            if (mineCache.Position.DistanceTo(me) > 5.0f)
                            {
                                _targetNpc = new Npc { Entry = mineCache.Entry, Position = mineCache.Position };
                                MovementManager.FindTarget(ref _targetNpc, 5f);
                                return;
                            }
                            else
                            {
                                Thread.Sleep(Usefuls.Latency + 250);
                                Interact.InteractWith(mineCache.GetBaseAddress, true);
                                _cacheMineGathered = true;
                                Thread.Sleep(Usefuls.Latency + 1750);
                            }
                        }
                    }
                    WoWUnit mineNpc = ObjMgr.GetNearestWoWUnit(ObjMgr.GetWoWUnitByEntry(_npcMine));
                    if (mineNpc.GetBaseAddress != 0)
                    {
                        if (mineNpc.Position.DistanceTo(me) > 5.0f)
                        {
                            _targetNpc = new Npc { Entry = mineNpc.Entry, Position = mineNpc.Position };
                            MovementManager.FindTarget(ref _targetNpc, 5f);
                            return;
                        }
                        else
                        {
                            Interact.InteractWith(mineNpc.GetBaseAddress, true);
                            Thread.Sleep(Usefuls.Latency + 1000);
                            Interact.InteractWith(mineNpc.GetBaseAddress, true);
                            Thread.Sleep(Usefuls.Latency + 500);
                            Lua.LuaDoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()");
                            Thread.Sleep(Usefuls.Latency + 1000);
                        }
                    }
                    tList.Pop();
                    break;
            }
            if (tList.Count == 0)
            {
                Logging.Write("Garrison Farming completed");
                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = _oldActivateVeinsHarvesting;
                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = _oldActivateHerbsHarvesting;
                nManagerSetting.CurrentSetting.GatheringSearchRadius = _oldGatheringSearchRadius;
                CloseProduct();
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
    }
}