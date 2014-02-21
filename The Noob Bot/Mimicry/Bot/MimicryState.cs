using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;
using Math = nManager.Helpful.Math;

namespace MimicryBot.Bot
{
    public class MimicryState : State
    {
        public override string DisplayName
        {
            get { return "Mimicry"; }
        }

        public override int Priority { get; set; }

        private Timer _positionCheckTimer;
        private Point _lastPoint;

        public override bool NeedToRun
        {
            get
            {
                if (_positionCheckTimer == null)
                    _positionCheckTimer = new Timer(1 * 1000);
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    (ObjectManager.Me.InCombat &&
                     !(ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    Usefuls.IsFlying ||
                    !Products.IsStarted)
                    return false;

                if (_positionCheckTimer.IsReady)
                {
                    _positionCheckTimer.Reset();
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
            Point target = MimicryClientCom.GetMasterPosition();
            if (_lastPoint == target)
                return;

            _lastPoint = target;
            bool r;
            if (target.Type.ToLower() == "flying" || target.Type.ToLower() == "swimming")
            {
                MovementManager.MoveTo(target);
                return;
            }

            List<Point> points = PathFinder.FindPath(target, out r);
            if (points.Count <= 1 || points.Count >= 40)
            {
                points.Clear();
                points.Add(ObjectManager.Me.Position);
                points.Add(target);
            }
            MovementManager.Go(points);

            timer = new nManager.Helpful.Timer(((int) Math.DistanceListPoint(points)/3*1000) + 4000);
            while (Products.IsStarted &&
                    !ObjectManager.Me.IsDeadMe &&
                    !(ObjectManager.Me.InCombat &&
                        !(ObjectManager.Me.IsMounted &&
                        (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                    !timer.IsReady && MovementManager.InMovement)
            {
                if (ObjectManager.Me.Position.DistanceTo2D(target) <= 2)
                {
                    MovementManager.StopMove();
                    break;
                }
                Thread.Sleep(50);
            }

            if (timer.IsReady && target.DistanceTo2D(ObjectManager.Me.Position) > 20)
            {
                MovementManager.StopMove();
                return;
            }

            // Stop move
            MovementManager.StopMove();

            // Face
            //MovementManager.Face(target);

        }


    }
}