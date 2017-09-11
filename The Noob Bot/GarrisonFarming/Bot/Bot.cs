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

                Fsm.AddState(new Pause {Priority = 200});
                Fsm.AddState(new Resurrect {Priority = 110});
                Fsm.AddState(new IsAttacked {Priority = 100});
                Fsm.AddState(new Regeneration {Priority = 90});
                Fsm.AddState(new ToTown {Priority = 80});
                Fsm.AddState(new Looting {Priority = 70});
                Fsm.AddState(new SpecializationCheck {Priority = 60});
                Fsm.AddState(new LevelupCheck {Priority = 50});
                Fsm.AddState(new Farming {Priority = 40});
                Fsm.AddState(new GarrisonState {Priority = 30});
                /* 2
                 * SendFollowerOnDuty - Plugins?
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