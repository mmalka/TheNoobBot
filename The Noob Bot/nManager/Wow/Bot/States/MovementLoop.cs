using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class MovementLoop : State
    {
        private readonly Timer _outOfBattlegroundAntiAfk = new Timer(1000*60);
        private int _battleground;
        private float _loopPathId = -1f;
        public bool IsProfileCSharp;

        public override string DisplayName
        {
            get { return "Movement Loop"; }
        }

        public override int Priority { get; set; }

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

                if (MovementManager.InMovement)
                    return false;

                if (Products.Products.ProductName == "Battlegrounder")
                {
                    if (Battleground.IsInBattleground() && !Battleground.IsFinishBattleground() &&
                        Battleground.BattlegroundIsStarted())
                    {
                        _battleground = 0;
                        if (PathLoop != null && PathLoop.Count > 0 && !IsProfileCSharp)
                            return true;
                    }
                    if (Battleground.IsInBattleground() && !Battleground.IsFinishBattleground() &&
                        !Battleground.BattlegroundIsStarted() && _battleground == 0)
                    {
                        MovementsAction.MoveForward(true);
                        Thread.Sleep(1000);
                        MovementsAction.MoveForward(false);
                        _battleground++;
                    }
                    else if (!Battleground.IsInBattleground() && _outOfBattlegroundAntiAfk.IsReady)
                    {
                        _battleground = 0;
                        _outOfBattlegroundAntiAfk.Reset();
                        MovementsAction.Ascend(true);
                        Thread.Sleep(300);
                        MovementsAction.Ascend(false);
                    }
                    return false;
                }
                if (PathLoop == null || PathLoop.Count <= 0)
                    return false;
                return true;
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

        public List<Point> PathLoop { get; set; }

        public override void Run()
        {
            int currentPoint;
            float pathIdentity = Math.DistanceListPoint(PathLoop);
            if (_loopPathId != pathIdentity) // If path changed, then we need to find the nearest point
            {
                currentPoint = Math.NearestPointOfListPoints(PathLoop, ObjectManager.ObjectManager.Me.Position);
                MovementManager.PointId = currentPoint;
            }
            // If the path did not change, let's find a good next point
            else if (PathLoop[MovementManager.PointId].DistanceTo(ObjectManager.ObjectManager.Me.Position) > 2.0f)
            {
                currentPoint = MovementManager.PointId + 1;
                if (currentPoint > PathLoop.Count - 1)
                    currentPoint = 0;
                int point = currentPoint;
                int lookup = 0;
                Vector3 me = new Vector3(ObjectManager.ObjectManager.Me.Position);
                do
                {
                    int pointNext = (point + 1 <= PathLoop.Count - 1 ? point + 1 : 0);
                    Vector3 v0 = new Vector3(PathLoop[point]);
                    Vector3 v1 = new Vector3(PathLoop[pointNext]);
                    float angle = (v1 - v0).Angle2D(v0 - me);
                    if (System.Math.Abs(angle) <= (System.Math.PI / 3f))
                    {
                        Logging.WriteNavigator("Next Point is " + (point - currentPoint) + " ahead, his angle is " + System.Math.Round(angle * 180 / System.Math.PI, 2) + "°");
                        currentPoint = point;
                        break;
                    }
                    point = pointNext;
                    lookup++;
                } while (lookup <= 10);
                MovementManager.PointId = currentPoint;
            }
            else
            {
                currentPoint = MovementManager.PointId;
            }
            if (_loopPathId == -1f)
                _loopPathId = pathIdentity;
            // Too far away, then we don't care for fly/swim but we need pathfinder to go by foot before anything else
            // This simply does not work. Grinder for low level (on ground) still go straitline in front for long distance
            if (PathLoop[currentPoint].Type.ToLower() != "flying" &&
                PathLoop[currentPoint].Type.ToLower() != "swimming" &&
                PathLoop[currentPoint].DistanceTo2D(ObjectManager.ObjectManager.Me.Position) > 7 /*&&
                PathLoop[_currentPoint].DistanceTo2D(ObjectManager.ObjectManager.Me.Position) <= 200*/)
            {
                bool bResult;
                List<Point> npoints = PathFinder.FindPath(PathLoop[currentPoint], out bResult);
                if (!bResult)
                    npoints.Add(new Point(PathLoop[currentPoint]));
                MovementManager.Go(npoints);
                return;
            }
            // We are near enough or flying/swimming then restore the loop
            MovementManager.GoLoop(PathLoop);
        }
    }
}