using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Plugins;
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
                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 200});
                Fsm.AddState(new FightHostileTargetDamageDealerOnly {Priority = 30});
                Fsm.AddState(new SpecializationCheck {Priority = 20});
                Fsm.AddState(new LevelupCheck {Priority = 10});
                Fsm.AddState(new Idle {Priority = 0});

                foreach (var statePlugin in Plugins.ListLoadedStatePlugins)
                {
                    Fsm.AddState(statePlugin);
                }

                Fsm.States.Sort();
                Fsm.StartEngine(12, "FSM Damage Dealer");

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