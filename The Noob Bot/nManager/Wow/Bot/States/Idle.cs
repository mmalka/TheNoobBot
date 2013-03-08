using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class Idle : State
    {
        private readonly Timer _canWrite = new Timer(1000*5);

        public int TimeSleepMs = 60;
        private string _currPosition;
        private string _lastPosition;

        public override string DisplayName
        {
            get { return "Idle"; }
        }

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get { return true; }
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
            Thread.Sleep(TimeSleepMs);
            if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) return;
            _currPosition = string.Format("CurrentPositionTracker({0}, {1}, {2}, {3});", ObjectManager.ObjectManager.Me.Position.X, ObjectManager.ObjectManager.Me.Position.Y,
                                          ObjectManager.ObjectManager.Me.Position.Z, ObjectManager.ObjectManager.Me.Position.Type);
            if (_currPosition == _lastPosition) return;
            _lastPosition = _currPosition;
            if (!_canWrite.IsReady) return;
            _canWrite.Reset();
            Logging.WriteFileOnly(_currPosition);
        }
    }
}