using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace DamageDealer.Bot
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
                CustomClass.LoadCustomClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new nManager.Wow.Bot.States.Pause {Priority = 12});
                Fsm.AddState(new nManager.Wow.Bot.States.IsAttacked {Priority = 10});
                Fsm.AddState(new nManager.Wow.Bot.States.FightHostileTarget {Priority = 9});
                Fsm.AddState(new nManager.Wow.Bot.States.Talents {Priority = 3});
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
                Logging.WriteError("Damage Dealer > Bot > Bot  > Pulse(): " + e);
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
            }
            catch (Exception e)
            {
                Logging.WriteError("Damage Dealer > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}