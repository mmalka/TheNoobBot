using System;
using System.Windows.Forms;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Bot.States;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace Archaeologist.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static Spell SurveySpell;

        internal static bool Pulse()
        {
            try
            {
                SurveySpell = new Spell("Survey");
                if (!SurveySpell.KnownSpell)
                {
                    MessageBox.Show(Translate.Get(Translate.Id.Survey_spell_not_found__stopping_tnb));
                    Logging.Write("Survey spell not found, stopping bot.");
                    return false;
                }

                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 200});
                Fsm.AddState(new Resurrect {Priority = 130});
                Fsm.AddState(new IsAttacked {Priority = 120});
                Fsm.AddState(new ToTown {Priority = 110});
                Fsm.AddState(new Looting {Priority = 100});
                Fsm.AddState(new Travel {Priority = 90});
                Fsm.AddState(new Regeneration {Priority = 80});
                Fsm.AddState(new SpecializationCheck {Priority = 70});
                Fsm.AddState(new LevelupCheck {Priority = 60});
                Fsm.AddState(new Trainers {Priority = 59});
                Fsm.AddState(new AutoItemCombiner {Priority = 52});
                Fsm.AddState(new MillingState {Priority = 51});
                Fsm.AddState(new ProspectingState {Priority = 50});
                Fsm.AddState(new Farming {Priority = 20});
                Fsm.AddState(new ArchaeologyStates
                {
                    Priority = 10,
                    SolvingEveryXMin = ArchaeologistSetting.CurrentSetting.SolvingEveryXMin,
                    MaxTryByDigsite = ArchaeologistSetting.CurrentSetting.MaxTryByDigsite,
                    UseKeystones = ArchaeologistSetting.CurrentSetting.UseKeystones,
                    CrateRestored = ArchaeologistSetting.CurrentSetting.CrateRestored
                });
                Fsm.AddState(new Idle {Priority = 0});

                foreach (var statePlugin in Plugins.ListLoadedStatePlugins)
                {
                    Fsm.AddState(statePlugin);
                }

                Fsm.States.Sort();
                Fsm.StartEngine(7, "FSM Archaeologist");
                Archaeology.Initialize();
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    Dispose();
                }
                catch
                {
                }
                Logging.WriteError("Archaeologist > Bot > Bot  > Pulse(): " + e);
                return false;
            }
        }

        internal static void Dispose()
        {
            try
            {
                CombatClass.DisposeCombatClass();
                Fsm.StopEngine();
                Fight.StopFight();
                MovementManager.StopMove();
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeologist > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}