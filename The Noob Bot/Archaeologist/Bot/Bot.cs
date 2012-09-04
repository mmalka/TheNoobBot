using System;
using System.Windows.Forms;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
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

                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CustomClass.LoadCustomClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new nManager.Wow.Bot.States.Pause {Priority = 11});
                Fsm.AddState(new nManager.Wow.Bot.States.Resurrect {Priority = 10});
                Fsm.AddState(new nManager.Wow.Bot.States.IsAttacked {Priority = 9});
                Fsm.AddState(new nManager.Wow.Bot.States.Regeneration {Priority = 8});
                Fsm.AddState(new nManager.Wow.Bot.States.Looting {Priority = 7});
                Fsm.AddState(new nManager.Wow.Bot.States.Farming {Priority = 6});
                Fsm.AddState(new nManager.Wow.Bot.States.ProspectingState {Priority = 5});
                Fsm.AddState(new nManager.Wow.Bot.States.ToTown {Priority = 4});
                Fsm.AddState(new nManager.Wow.Bot.States.Talents {Priority = 3});
                Fsm.AddState(new nManager.Wow.Bot.States.Trainers {Priority = 2});
                Fsm.AddState(new nManager.Wow.Bot.States.ArchaeologyStates
                                 {
                                     Priority = 1,
                                     SolvingEveryXMin = ArchaeologistSetting.CurrentSetting.solvingEveryXMin,
                                     MaxTryByDigsite = ArchaeologistSetting.CurrentSetting.maxTryByDigsite
                                 });
                Fsm.AddState(new nManager.Wow.Bot.States.Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(6); // Fsm.StartEngine(25);

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
                CustomClass.DisposeCustomClass();
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