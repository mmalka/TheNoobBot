using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace HealBot.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static bool Pulse()
        {
            try
            {
                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                HealerClass.LoadHealerClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 3});
                Fsm.AddState(new HealFriendlyTarget {Priority = 2});
                Fsm.AddState(new Talents {Priority = 1});
                Fsm.AddState(new Idle {Priority = 0});

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
                Logging.WriteError("Heal Bot >  Bot  > Pulse(): " + e);
                return false;
            }
        }

        internal static void Dispose()
        {
            try
            {
                HealerClass.DisposeHealerClass();
                Fsm.StopEngine();
                Heal.StopHeal();
            }
            catch (Exception e)
            {
                Logging.WriteError("Heal Bot > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}