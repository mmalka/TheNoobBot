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

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                if (PathLoop == null || PathLoop.Count <= 0)
                    return false;

                if (MovementManager.InMovement)
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
            var nearPointId = Math.NearestPointOfListPoints(PathLoop, ObjectManager.ObjectManager.Me.Position);
            if (PathLoop[nearPointId].Type.ToLower() != "flying" && PathLoop[nearPointId].Type.ToLower() != "swimming" && PathLoop[nearPointId].DistanceTo2D(ObjectManager.ObjectManager.Me.Position) > 7 && PathLoop[nearPointId].DistanceTo2D(ObjectManager.ObjectManager.Me.Position) <= 200)
            {
                var points = PathFinder.FindPath(PathLoop[nearPointId]);
                MovementManager.Go(points);
                return;
            }
            MovementManager.GoLoop(PathLoop);
        }
    }
}
