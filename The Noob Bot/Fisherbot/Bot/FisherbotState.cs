using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace Fisherbot.Bot
{
    internal class FisherbotState : State
    {
        public override string DisplayName
        {
            get { return "FisherbotState"; }
        }

        public override int Priority { get; set; }

        private WoWGameObject _node;
        private const float DistanceMin = 13f;
        private const float DistanceMax = 19f;
        private const int TimeTryFindGoodPos = 7000;
        private nManager.Helpful.Timer timer;

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

                if (FisherbotSetting.CurrentSetting.FishSchool)
                {
                    // Get farm:
                    _node =
                        ObjectManager.GetNearestWoWGameObject(
                            ObjectManager.GetWoWGameObjectOfType(nManager.Wow.Enums.WoWGameObjectType.FishingHole));

                    if (_node.IsValid && !nManagerSetting.IsBlackListedZone(_node.Position) &&
                        _node.GetDistance2D < nManagerSetting.CurrentSetting.GatheringSearchRadius &&
                        !nManagerSetting.IsBlackListed(_node.Guid))
                        return true;
                }
                else
                {
                    _node = new WoWGameObject(0);
                    return true;
                }
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
            if (!Products.IsStarted || ObjectManager.Me.IsDeadMe || ObjectManager.Me.InCombat)
                return;
            if (!FisherbotSetting.CurrentSetting.FishSchool)
            {
                // If we have a saved position and we don't fish, then go to position
                if (FisherbotSetting.CurrentSetting.FisherbotPosition.IsValid &&
                    !FishingTask.IsLaunched)
                {
                    LongMove.LongMoveGo(FisherbotSetting.CurrentSetting.FisherbotPosition);
                    MovementManager.Face(FisherbotSetting.CurrentSetting.FisherbotRotation);
                }
                // If we don't fish or the timer is null, then start a new timer, save position and fish
                if (timer == null || !FishingTask.IsLaunched)
                {
                    timer = new nManager.Helpful.Timer(10*60*1000 + 10*1000);
                    FisherbotSetting.CurrentSetting.FisherbotPosition = ObjectManager.Me.Position;
                    FisherbotSetting.CurrentSetting.FisherbotRotation = ObjectManager.Me.Rotation;
                    Fishing.EquipFishingPoles(FisherbotSetting.CurrentSetting.FishingPoleName);
                    FishingTask.LoopFish(0, FisherbotSetting.CurrentSetting.UseLure,
                        FisherbotSetting.CurrentSetting.LureName);
                }
                    // If the timer ended, stop fishing, equip weapon, null the timer
                else if (timer.IsReady)
                {
                    FishingTask.StopLoopFish();
                    ItemsManager.EquipItemByName(FisherbotSetting.CurrentSetting.WeaponName);
                    timer = null;
                }
                    // We are in timer, we fish, then save the position
                else
                {
                    FisherbotSetting.CurrentSetting.FisherbotPosition = ObjectManager.Me.Position;
                    FisherbotSetting.CurrentSetting.FisherbotRotation = ObjectManager.Me.Rotation;
                    Thread.Sleep(500);
                }
                // No more while, we test what we need and return
                return;
            }

            // Nodes fishing code
            if (_node.IsValid)
            {
                FisherbotSetting.CurrentSetting.FisherbotPosition =
                    Bot.Profile.Points[Math.NearestPointOfListPoints(Bot.Profile.Points, _node.Position)];
                Logging.Write("Fish " + _node.Name + " > " + _node.Position);
            }

            if (FisherbotSetting.CurrentSetting.FishSchool)
            {
                Point whereToGo = Fishing.FindTheUltimatePoint(_node.Position);
                if (whereToGo.Type == "invalid")
                {
                    Logging.Write("No valid point found");
                    nManagerSetting.AddBlackList(_node.Guid);
                    return;
                }
                bool r;
                List<Point> points = PathFinder.FindPath(whereToGo, out r);
                if (points.Count <= 1 || points.Count >= 20)
                {
                    points.Clear();
                    points.Add(ObjectManager.Me.Position);
                    points.Add(whereToGo);
                }
                else
                {
                    for (int i = 0; i < points.Count; i++)
                        if (points[i].Z < whereToGo.Z)
                            points[i].Z = whereToGo.Z;
                }
                Logging.Write("Going to point > " + whereToGo.X + " ; " + whereToGo.Y + " ; " + whereToGo.Z + " ; " +
                              points[0].Type);
                MovementManager.Go(points);

                timer = new nManager.Helpful.Timer(((int) Math.DistanceListPoint(points)/3*1000) + 4000);
                while ((_node.IsValid || !FisherbotSetting.CurrentSetting.FishSchool) && Products.IsStarted &&
                       !ObjectManager.Me.IsDeadMe &&
                       !(ObjectManager.Me.InCombat &&
                         !(ObjectManager.Me.IsMounted &&
                           (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                       !timer.IsReady && MovementManager.InMovement)
                {
                    if (ObjectManager.Me.Position.DistanceTo2D(whereToGo) <= 0.2f)
                    {
                        MovementManager.StopMove();
                        break;
                    }
                    Thread.Sleep(50);
                }

                if (timer.IsReady && _node.GetDistance2D > DistanceMax)
                {
                    Logging.Write("Fishing failed - No nearby point found (distance near position = " +
                                  ObjectManager.Me.Position.DistanceTo2D(
                                      FisherbotSetting.CurrentSetting.FisherbotPosition).ToString(CultureInfo.InvariantCulture) + ")");
                    MovementManager.StopMove();
                    nManagerSetting.AddBlackList(_node.Guid);
                    return;
                }
            }

            // Stop move
            MovementManager.StopMove();
            MountTask.DismountMount();

            // Face
            if (_node.IsValid)
                MovementManager.Face(_node);
            else
                MovementManager.Face(FisherbotSetting.CurrentSetting.FisherbotRotation);

            // Ce met a la bonne distance du banc de poisson
            if (_node.IsValid)
            {
                int nbIsSwimming = 0;
                timer = new nManager.Helpful.Timer(1000*8);
                while ((Usefuls.IsSwimming || _node.GetDistance > DistanceMax || _node.GetDistance < DistanceMin) &&
                       Products.IsStarted && !ObjectManager.Me.IsDeadMe && !ObjectManager.Me.InCombat && !timer.IsReady)
                {
                    if (nbIsSwimming*100 > TimeTryFindGoodPos)
                    {
                        FishingTask.StopLoopFish();
                        MovementsAction.MoveBackward(false);
                        MovementsAction.MoveForward(false);
                        Logging.Write("BlackList " + _node.Name);
                        nManagerSetting.AddBlackList(_node.Guid);
                        return;
                    }
                    FishingTask.StopLoopFish();
                    MovementManager.Face(_node);

                    if (Usefuls.IsSwimming || _node.GetDistance < DistanceMin)
                    {
                        MovementsAction.MoveForward(false);
                        MovementsAction.MoveBackward(true);
                    }
                    if (_node.GetDistance > DistanceMax)
                    {
                        MovementsAction.MoveBackward(false);
                        MovementsAction.MoveForward(true);
                    }

                    nbIsSwimming++;
                    Thread.Sleep(100);
                }
                MovementsAction.MoveBackward(false);
                MovementsAction.MoveForward(false);
                if (timer.IsReady)
                {
                    Logging.Write("Fishing failed - Out of range" + timer.IsReady.ToString());
                    MovementManager.StopMove();
                    nManagerSetting.AddBlackList(_node.Guid);
                    return;
                }
            }

            // Fish
            Fishing.EquipFishingPoles(FisherbotSetting.CurrentSetting.FishingPoleName);
            FishingTask.LoopFish(_node.Guid, FisherbotSetting.CurrentSetting.UseLure,
                FisherbotSetting.CurrentSetting.LureName,
                FisherbotSetting.CurrentSetting.PrecisionMode);

            timer = new nManager.Helpful.Timer(2*60*1000);
            while ((_node.IsValid || !FisherbotSetting.CurrentSetting.FishSchool) && Products.IsStarted &&
                   !ObjectManager.Me.IsDeadMe &&
                   !ObjectManager.Me.InCombat && !timer.IsReady &&
                   FishingTask.IsLaunched)
            {
                if (ObjectManager.Me.Position.DistanceTo2D(FisherbotSetting.CurrentSetting.FisherbotPosition) > 3.5f &&
                    !FisherbotSetting.CurrentSetting.FishSchool)
                {
                    break;
                }
                Thread.Sleep(300);
            }
            FishingTask.StopLoopFish();
            ItemsManager.EquipItemByName(FisherbotSetting.CurrentSetting.WeaponName);
        }
    }
}