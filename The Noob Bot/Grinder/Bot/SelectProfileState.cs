using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.ObjectManager;

namespace Grinder.Bot
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

        private uint _lastLevel;

        public override bool NeedToRun
        {
            get
            {
                if (_lastLevel != ObjectManager.Me.Level)
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
            Bot.SelectZone();
            _lastLevel = ObjectManager.Me.Level;
            Logging.Write("Select zone: " + Bot.Profile.GrinderZones[Bot.ZoneIdProfile].Name);
        }
    }
}
