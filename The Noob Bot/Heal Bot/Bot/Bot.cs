using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace Heal_Bot.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static bool Pulse()
        {
            try
            {
                // Load CC:
                HealerClass.LoadHealerClass();
                Heal.StartHealBot();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 40});
                Fsm.AddState(new SpecializationCheck {Priority = 30});
                Fsm.AddState(new LevelupCheck {Priority = 20});
                /*Fsm.AddState(new HealFriendlyTarget { Priority = 1 });*/
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(6, "FSM HealBot");

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
                Logging.WriteError("Heal Bot >  Bot  > Pulse(): " + e);
                return false;
            }
        }

        internal static void Dispose()
        {
            try
            {
                HealerClass.DisposeHealerClass();
                Fsm.StopEngine();
                Heal.StopHeal();
            }
            catch (Exception e)
            {
                Logging.WriteError("Heal Bot > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}