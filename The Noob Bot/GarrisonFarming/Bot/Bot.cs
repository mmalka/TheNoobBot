using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace GarrisonFarming.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static bool Pulse()
        {
            try
            {
                if (ObjectManager.Me.Level < 90)
                {
                    Logging.Write("You don't have the required level for this product.");
                    return false;
                }
                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 100});
                Fsm.AddState(new Resurrect {Priority = 11});
                Fsm.AddState(new IsAttacked {Priority = 10});
                Fsm.AddState(new Regeneration {Priority = 9});
                Fsm.AddState(new ToTown {Priority = 8});
                Fsm.AddState(new Looting {Priority = 7});
                Fsm.AddState(new SpecializationCheck {Priority = 6});
                Fsm.AddState(new LevelupCheck {Priority = 5});
                Fsm.AddState(new Farming {Priority = 4});
                Fsm.AddState(new GarrisonState {Priority = 3});
                /* 2
                 * SendFollowerOnDuty
                 */
                Fsm.AddState(new Idle {Priority = 0});
                Fsm.States.Sort();
                Fsm.StartEngine(5, "FSM GarrisonFarming");

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
                Logging.WriteError("GarrisonFarming > Bot > Bot  > Pulse(): " + e);
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
                GarrisonState.RestoreSettings();
            }
            catch (Exception e)
            {
                Logging.WriteError("GarrisonFarming > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}