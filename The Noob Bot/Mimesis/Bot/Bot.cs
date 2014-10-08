using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
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

                Fsm.AddState(new Pause {Priority = 100});
                Fsm.AddState(new Resurrect {Priority = 9});
                Fsm.AddState(new IsAttacked {Priority = 8});
                Fsm.AddState(new FightHostileTarget {Priority = 7});
                Fsm.AddState(new Looting {Priority = 6});
                Fsm.AddState(new Travel {Priority = 5});
                Fsm.AddState(new Farming {Priority = 4});
                Fsm.AddState(new SpecializationCheck {Priority = 3});
                Fsm.AddState(new LevelupCheck {Priority = 2});
                Fsm.AddState(new MimesisState {Priority = 1});
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(5, "FSM Mimesis");

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