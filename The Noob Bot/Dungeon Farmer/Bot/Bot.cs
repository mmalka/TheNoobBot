using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace DungeonFarmer.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static bool Pulse()
        {
            try
            {
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
                Fsm.AddState(new Trainers {Priority = 50});
                Fsm.AddState(new MillingState {Priority = 40});
                Fsm.AddState(new ProspectingState {Priority = 30});
                Fsm.AddState(new Farming {Priority = 20});
                Fsm.AddState(new DungeonFarming {Priority = 10,});
                Fsm.AddState(new Idle {Priority = 0});

                foreach (var statePlugin in Plugins.ListLoadedStatePlugins)
                {
                    Fsm.AddState(statePlugin);
                }

                Fsm.States.Sort();
                Fsm.StartEngine(7, "FSM DungeonFarmer");
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
                Logging.WriteError("DungeonFarmer > Bot > Bot  > Pulse(): " + e);
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
                Logging.WriteError("DungeonFarmer > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}