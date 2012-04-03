using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;

namespace nManager.Wow.Bot.States
{
    public class Pause : State
    {

        public override string DisplayName
        {
            get { return "Pause"; }
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
                if (Products.Products.IsStarted && Products.Products.InPause)
                    return true;
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
            Logging.Write("Pause started");
            Helpers.MovementManager.StopMove();
            while (Products.Products.IsStarted && Products.Products.InPause)
            {
                Thread.Sleep(300);
            }
            Logging.Write("Pause stoped");
        }
    }
}
