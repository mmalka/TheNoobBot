using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace Damage_Dealer.Bot
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
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 12});
                Fsm.AddState(new IsAttacked {Priority = 10});
                Fsm.AddState(new FightHostileTarget {Priority = 9});
                Fsm.AddState(new LevelupCheck {Priority = 3});
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(6, "FSM Damage Dealer");

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
                CombatClass.DisposeCombatClass();
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