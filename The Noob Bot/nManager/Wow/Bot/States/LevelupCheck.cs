using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class LevelupCheck : State
    {
        public override string DisplayName
        {
            get { return "LevelupCheck"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        private uint _lastLevel;

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.InCombat ||
                    !Products.Products.IsStarted)
                    return false;

                // Collect our initial Level at product start.
                if (_lastLevel <= 0)
                    _lastLevel = ObjectManager.ObjectManager.Me.Level;

                // Update the SpellBook and eventually the talents on level-ups.
                return _lastLevel != ObjectManager.ObjectManager.Me.Level;
            }
        }

        public override void Run()
        {
            _lastLevel = ObjectManager.ObjectManager.Me.Level;
            if (ObjectManager.ObjectManager.Me.Level >= 15 && nManagerSetting.CurrentSetting.AutoAssignTalents)
                Talent.DoTalents(); // First talent at level 15.
            SpellManager.UpdateSpellBook(); // It also reset Combat/Healer classes.
        }
    }
}