using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.ObjectManager;

namespace Battlegrounder.Bot
{
    internal class SelectProfileState : State
    {
        public override string DisplayName
        {
            get { return "SelectProfileState"; }
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
                //return true;
                // if current action need to change, true (example, flag have been taken)
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
            // nothing to do right now
        }
    }
}