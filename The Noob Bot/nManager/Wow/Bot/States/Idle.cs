using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;

namespace nManager.Wow.Bot.States
{
    public class Idle : State
    {

        public override string DisplayName
        {
            get { return "Idle"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public int TimeSleepMs = 60;

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
        }
    }
}
