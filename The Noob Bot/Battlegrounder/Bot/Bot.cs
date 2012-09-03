using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Battlegrounder.Profile;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace Battlegrounder.Bot
{
    class Bot
    {
        static readonly Engine Fsm = new Engine();

        internal static BattlegrounderProfile Profile = new BattlegrounderProfile();

        internal static bool Pulse()
        {
            try
            {
                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CustomClass.LoadCustomClass();

                int battlegrounderStatePriority = 6;

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new nManager.Wow.Bot.States.Pause { Priority = 11 });
                Fsm.AddState(new nManager.Wow.Bot.States.Resurrect { Priority = 10 });
                Fsm.AddState(new nManager.Wow.Bot.States.IsAttacked { Priority = 9 });
                Fsm.AddState(new nManager.Wow.Bot.States.Regeneration { Priority = 8 });
                Fsm.AddState(new nManager.Wow.Bot.States.Looting { Priority = 7 });
                Fsm.AddState(new nManager.Wow.Bot.States.Farming { Priority = 6 });
                Fsm.AddState(new BattlegrounderState { Priority = battlegrounderStatePriority });
                Fsm.AddState(new nManager.Wow.Bot.States.ProspectingState { Priority = 5 });
                Fsm.AddState(new nManager.Wow.Bot.States.ToTown { Priority = 4 });
                Fsm.AddState(new nManager.Wow.Bot.States.Talents { Priority = 3 });
                Fsm.AddState(new nManager.Wow.Bot.States.Trainers { Priority = 2 });

                //Fsm.AddState(new nManager.Wow.Bot.States.MovementLoop { Priority = 1, PathLoop = Profile.Points });
                Fsm.AddState(new nManager.Wow.Bot.States.Idle { Priority = 0 });

                Fsm.States.Sort();
                Fsm.StartEngine(6); // Fsm.StartEngine(25);

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
                CustomClass.DisposeCustomClass();
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
