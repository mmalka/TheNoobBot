using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace Battlegrounder.Bot
{
    public class Bot
    {
        private static readonly Engine Fsm = new Engine();
        public static MovementLoop MovementLoop = new MovementLoop {Priority = 10};
        internal static Battlegrounding Battlegrounding = new Battlegrounding {Priority = 40};

        internal static bool Pulse()
        {
            try
            {
                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();
                Fsm.AddState(new Pause {Priority = 200});
                Fsm.AddState(new Resurrect {Priority = 170});
                Fsm.AddState(new IsAttacked {Priority = 160});
                Fsm.AddState(new BattlegrounderQueueing {Priority = 150});
                Fsm.AddState(new BattlegrounderCurrentProfile {Priority = 140});
                Fsm.AddState(new Looting {Priority = 130});
                Fsm.AddState(new Regeneration {Priority = 120});
                Fsm.AddState(new ToTown {Priority = 110});
                Fsm.AddState(new SpecializationCheck {Priority = 100});
                Fsm.AddState(new LevelupCheck {Priority = 90});
                Fsm.AddState(new Trainers {Priority = 80});
                Fsm.AddState(new MillingState {Priority = 70});
                Fsm.AddState(new ProspectingState {Priority = 60});
                Fsm.AddState(new Farming {Priority = 50});
                Fsm.AddState(Battlegrounding);
                Fsm.AddState(new BattlegrounderCurrentProfile {Priority = 30});
                Fsm.AddState(new BattlegrounderQueueing {Priority = 20});
                Fsm.AddState(MovementLoop);
                Fsm.AddState(new Idle {Priority = 0});

                foreach (var statePlugin in Plugins.ListLoadedStatePlugins)
                {
                    Fsm.AddState(statePlugin);
                }

                Fsm.States.Sort();
                Fsm.StartEngine(7, "FSM Battlegrounder");

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