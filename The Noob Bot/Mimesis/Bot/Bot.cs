using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace Mimesis.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static bool Pulse()
        {
            try
            {
                if (!MimesisClientCom.Connect())
                    return false;
                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 200});
                Fsm.AddState(new Resurrect {Priority = 90});
                Fsm.AddState(new IsAttacked {Priority = 80});
                Fsm.AddState(new FightHostileTarget {Priority = 70});
                Fsm.AddState(new Looting {Priority = 60});
                Fsm.AddState(new Travel {Priority = 50});
                Fsm.AddState(new Farming {Priority = 40});
                Fsm.AddState(new SpecializationCheck {Priority = 30});
                Fsm.AddState(new LevelupCheck {Priority = 20});
                Fsm.AddState(new MimesisState {Priority = 10});
                Fsm.AddState(new Idle {Priority = 0});

                foreach (var statePlugin in Plugins.ListLoadedStatePlugins)
                {
                    Fsm.AddState(statePlugin);
                }

                Fsm.States.Sort();
                Fsm.StartEngine(7, "FSM Mimesis");

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
                Logging.WriteError("Mimesis > Bot > Bot  > Pulse(): " + e);
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
                MimesisClientCom.Disconnect();
            }
            catch (Exception e)
            {
                Logging.WriteError("Mimesis > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}