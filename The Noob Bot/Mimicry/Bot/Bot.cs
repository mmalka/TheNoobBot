using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace MimicryBot.Bot
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
                MimicryClientCom.Connect();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause { Priority = 8 });
                Fsm.AddState(new Looting { Priority = 7 });
                Fsm.AddState(new IsAttacked { Priority = 6 });
                Fsm.AddState(new FightHostileTarget { Priority = 5 });
                Fsm.AddState(new Farming { Priority = 4 });
                Fsm.AddState(new SpecializationCheck { Priority = 3 });
                Fsm.AddState(new LevelupCheck { Priority = 2});
                Fsm.AddState(new MimicryState { Priority = 1});
                Fsm.AddState(new Idle { Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(6, "FSM Mimicry");

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
                Logging.WriteError("Mimicry > Bot > Bot  > Pulse(): " + e);
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
                MimicryClientCom.Disconnect();
            }
            catch (Exception e)
            {
                Logging.WriteError("Mimicry > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}