using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class SpecializationCheck : State
    {
        public override string DisplayName
        {
            get { return "SpecializationCheck"; }
        }

        public override int Priority { get; set; }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        private WoWSpecialization _lastSpecialization = ObjectManager.ObjectManager.Me.WowSpecialization();

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

                uint lastLevel = LevelupCheck.GetLastLevel;
                if (lastLevel <= 0 || lastLevel != ObjectManager.ObjectManager.Me.Level)
                    return false;
                // It's the job of the state LevelupCheck.

                // Update the SpellBook and the Talents on Specialization changes.
                return _lastSpecialization != ObjectManager.ObjectManager.Me.WowSpecialization();
            }
        }

        public override void Run()
        {
            Logging.Write("We have detected that your Wow Specialization has changed, reloading it.");
            _lastSpecialization = ObjectManager.ObjectManager.Me.WowSpecialization();
            if (ObjectManager.ObjectManager.Me.Level >= 15 && nManagerSetting.CurrentSetting.AutoAssignTalents)
                Talent.DoTalents(); // First talent at level 15. Level check kept as additional security, should be useless here.
            SpellManager.UpdateSpellBook(); // It also reset Combat/Healer classes.
        }
    }
}