using System.Collections.Generic;
using nManager.FiniteStateMachine;

namespace GarrisonFarming.Bot
{
    public class MineState : State
    {
        public override string DisplayName
        {
            get { return "MineState"; }
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
        }
    }
}