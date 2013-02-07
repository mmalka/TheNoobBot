using System;
using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using Math = nManager.Helpful.Math;

namespace nManager.Wow.Bot.States
{
    public class MovementLoop : State
    {
        public override string DisplayName
        {
            get { return "Movement Loop"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;
        private float _loopPathId = -1f;

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

                if (PathLoop == null || PathLoop.Count <= 0)
                    return false;

                if (MovementManager.InMovement)
                    return false;

                if (Products.Products.ProductName == "Battlegrounder")
                {
                    if (Battleground.IsInBattleground() && Battleground.BattlegroundIsStarted() &&
                        !Battleground.IsFinishBattleground())
                        return true;
                    return false;
                }
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
            int _currentPoint;
            float PathIdentity = Math.DistanceListPoint(PathLoop);
            if (_loopPathId != PathIdentity) // If path changed, then we need to find the nearest point
            {
                _currentPoint = Math.NearestPointOfListPoints(PathLoop, ObjectManager.ObjectManager.Me.Position);
            }
            else // If the path did not change, let's return to the last point we were using
            {
                _currentPoint = MovementManager.PointId + 1;
                if (_currentPoint > PathLoop.Count - 1)
                    _currentPoint = 0;
            }
            // Too far away, then we don't care for fly/swim but we need pathfinder to go by foot before anything else
            // This simply does not work. Grinder for low level (on ground) still go straitline in front for long distance
            if (PathLoop[_currentPoint].Type.ToLower() != "flying" &&
                PathLoop[_currentPoint].Type.ToLower() != "swimming" &&
                PathLoop[_currentPoint].DistanceTo2D(ObjectManager.ObjectManager.Me.Position) > 7/*&&
                PathLoop[_currentPoint].DistanceTo2D(ObjectManager.ObjectManager.Me.Position) <= 200*/)
            {
                var npoints = PathFinder.FindPath(PathLoop[_currentPoint]);
                MovementManager.Go(npoints);
                return;
            }
            // We are near enough or flying/swimming then restore the loop
            _loopPathId = PathIdentity;
            MovementManager.GoLoop(PathLoop);
        }
    }
}