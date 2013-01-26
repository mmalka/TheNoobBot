using System.Collections.Generic;
using nManager.FiniteStateMachine;

namespace Battlegrounder.Bot
{
    internal class BattlegrounderState : State
    {
        public override string DisplayName
        {
            get { return "BattlegrounderState"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public override bool NeedToRun
        {
            get { return false; }
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