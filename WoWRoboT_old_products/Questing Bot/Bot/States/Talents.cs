using WowManager.FiniteStateMachine;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;

namespace Questing_Bot.Bot.States
{
    internal class Talents : State
    {
        public override string DisplayName
        {
            get { return "Talents"; }
        }

        public override int Priority
        {
            get { return (int)States.Priority.Talents; }
        }

        private uint _lastLevel;
        public override bool NeedToRun
        {
            get
            {
                if (!Useful.InGame ||
                    Useful.isLoadingOrConnecting ||
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    ObjectManager.Me.InCombat ||
                    !Config.Bot.BotIsActive)
                    return false;

                // Firts:
                if (_lastLevel <= 0)
                    _lastLevel = ObjectManager.Me.Level;

                // Need Talents
                if (Config.Bot.FormConfig.Talents)
                    if (ObjectManager.Me.Level >= 10 && _lastLevel != ObjectManager.Me.Level)
                        return true;

                return false;
            }
        }

        public override void Run()
        {
            _lastLevel = ObjectManager.Me.Level;
            Talent.DoTalents();
            WowManager.WoW.SpellManager.SpellManager.UpdateSpellBook();
        }
    }
}