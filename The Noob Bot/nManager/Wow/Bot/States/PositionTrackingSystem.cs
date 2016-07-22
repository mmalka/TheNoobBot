using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class PositionTrackingSystem : State
    {
        private readonly Timer _canWrite = new Timer(1000*5);
        private string _currPosition;
        private string _lastPosition;

        public override string DisplayName
        {
            get { return "PositionTrackingSystem"; }
        }

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame || Usefuls.IsLoading) return false;
                _currPosition = string.Format("CurrentPositionTracker({0}, {1}, {2}, {3});", ObjectManager.ObjectManager.Me.Position.X, ObjectManager.ObjectManager.Me.Position.Y,
                    ObjectManager.ObjectManager.Me.Position.Z, ObjectManager.ObjectManager.Me.Position.Type);
                if (_currPosition == _lastPosition) return false;
                _lastPosition = _currPosition;
                return _canWrite.IsReady;
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
            Logging.WriteFileOnly(_currPosition);
            _canWrite.Reset();
        }
    }
}