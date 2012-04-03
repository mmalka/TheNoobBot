using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class Talents : State
    {
        public override string DisplayName
        {
            get { return "Talents"; }
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

                // Firts:
                if (_lastLevel <= 0)
                    _lastLevel = ObjectManager.ObjectManager.Me.Level;

                // Need Talents
                if (nManagerSetting.CurrentSetting.assignTalents)
                    if (ObjectManager.ObjectManager.Me.Level >= 10 && _lastLevel != ObjectManager.ObjectManager.Me.Level)
                        return true;

                return false;
            }
        }

        public override void Run()
        {
            _lastLevel = ObjectManager.ObjectManager.Me.Level;
            Talent.DoTalents();
            SpellManager.UpdateSpellBook();
        }
    }
}
