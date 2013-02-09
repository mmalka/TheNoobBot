using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace Battlegrounder.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();
        internal static MovementLoop MovementLoop = new MovementLoop {Priority = 5};
        internal static Battlegrounding Battlegrounding = new Battlegrounding {Priority = 3};

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

                Fsm.AddState(new Pause {Priority = 12});
                Fsm.AddState(new ToTown {Priority = 1});
                Fsm.AddState(new Talents {Priority = 10});
                Fsm.AddState(new Trainers {Priority = 9});
                Fsm.AddState(new Resurrect {Priority = 8});
                Fsm.AddState(new Looting {Priority = 7});
                Fsm.AddState(new Regeneration {Priority = 6});
                Fsm.AddState(Battlegrounding);
                Fsm.AddState(new IsAttacked {Priority = 4});
                Fsm.AddState(MovementLoop);
                Fsm.AddState(new BattlegrounderCurrentProfile {Priority = 2});
                Fsm.AddState(new BattlegrounderQueueing {Priority = 1});
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(12); // Fsm.StartEngine(25);

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
                Logging.WriteError("Battlegrounder > Bot > Bot  > Pulse(): " + e);
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
                Logging.WriteError("Battlegrounder > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}