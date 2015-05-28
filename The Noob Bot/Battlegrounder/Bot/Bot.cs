using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace Battlegrounder.Bot
{
    public class Bot
    {
        private static readonly Engine Fsm = new Engine();
        public static MovementLoop MovementLoop = new MovementLoop {Priority = 1};
        internal static Battlegrounding Battlegrounding = new Battlegrounding {Priority = 4};

        internal static bool Pulse()
        {
            try
            {
                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();
                Fsm.AddState(new Pause {Priority = 18});
                Fsm.AddState(new Resurrect {Priority = 17});
                Fsm.AddState(new IsAttacked {Priority = 16});
                Fsm.AddState(new BattlegrounderQueueing {Priority = 15});
                Fsm.AddState(new BattlegrounderCurrentProfile {Priority = 14});
                Fsm.AddState(new Looting {Priority = 13});
                Fsm.AddState(new Regeneration {Priority = 12});
                Fsm.AddState(new ToTown {Priority = 11});
                Fsm.AddState(new SpecializationCheck {Priority = 10});
                Fsm.AddState(new LevelupCheck {Priority = 9});
                Fsm.AddState(new Trainers {Priority = 8});
                Fsm.AddState(new MillingState {Priority = 7});
                Fsm.AddState(new ProspectingState {Priority = 6});
                Fsm.AddState(new Farming {Priority = 5});
                Fsm.AddState(Battlegrounding);
                Fsm.AddState(new BattlegrounderCurrentProfile {Priority = 3});
                Fsm.AddState(new BattlegrounderQueueing {Priority = 2});
                Fsm.AddState(MovementLoop);
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(10, "FSM Battlegrounder");

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
                CombatClass.DisposeCombatClass();
                CustomProfile.DisposeCustomProfile();
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