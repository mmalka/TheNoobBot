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
                    _node = new WoWGameObject(0);
                    _node =
                        ObjectManager.GetNearestWoWGameObject(
                            ObjectManager.GetWoWGameObjectOfType(nManager.Wow.Enums.WoWGameObjectType.FishingHole));

                    if (_node.IsValid && _node.GetBaseAddress > 0)
                        if (!nManagerSetting.IsBlackListedZone(_node.Position) &&
                            _node.GetDistance2D < nManagerSetting.CurrentSetting.GatheringSearchRadius &&
                            !nManagerSetting.IsBlackListed(_node.Guid) && _node.IsValid)
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
            nManager.Helpful.Timer timer;
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
                    Logging.Write("Fishing failed - No found nearby point (distance near position = " +
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
            {
                ObjectManager.Me.Rotation = FisherbotSetting.CurrentSetting.FisherbotRotation;
                //MovementsAction.StrafeLeft(true, true);
                //MovementsAction.StrafeRight(true, true);
            }

            // Ce met a la bonne distance du banc de poisson
            if (_node.IsValid)
            {
                int nbIsSwimming = 0;
                timer = new nManager.Helpful.Timer(1000*8);
                while ((Usefuls.IsSwimming || _node.GetDistance > DistanceMax || _node.GetDistance < DistanceMin) &&
                       (int) _node.GetBaseAddress > 0 && Products.IsStarted && !ObjectManager.Me.IsDeadMe &&
                       !ObjectManager.Me.InCombat && !timer.IsReady)
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
            if (FisherbotSetting.CurrentSetting.FishSchool)
                FishingTask.LoopFish(_node.Guid, FisherbotSetting.CurrentSetting.UseLure,
                                     FisherbotSetting.CurrentSetting.LureName,
                                     FisherbotSetting.CurrentSetting.PrecisionMode);
            else
                FishingTask.LoopFish(0, FisherbotSetting.CurrentSetting.UseLure,
                                     FisherbotSetting.CurrentSetting.LureName);

            timer = FisherbotSetting.CurrentSetting.FishSchool ? new nManager.Helpful.Timer(2*60*1000) : new nManager.Helpful.Timer(1000*120);
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