using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Battlegrounder.Profile;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace Battlegrounder.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();
        internal static MovementLoop _movementLoop = new MovementLoop { Priority = 3 };
        internal static Battlegrounding _battlegrounding = new Battlegrounding {Priority = 7};

        internal static bool Pulse()
        {
            try
            {
                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CustomClass.LoadCustomClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 13});
                Fsm.AddState(new SelectProfileState {Priority = 12});
                Fsm.AddState(new Resurrect {Priority = 11});
                Fsm.AddState(new IsAttacked {Priority = 10});
                Fsm.AddState(new Regeneration {Priority = 9});
                Fsm.AddState(new Looting {Priority = 8});
                Fsm.AddState(_battlegrounding);
                Fsm.AddState(new ToTown {Priority = 6});
                Fsm.AddState(new Talents {Priority = 5});
                Fsm.AddState(new Trainers {Priority = 4});
                Fsm.AddState(_movementLoop);
                Fsm.AddState(new BattlegrounderCurrentProfile {Priority = 2});
                Fsm.AddState(new BattlegrounderQueueing { Priority = 1 });
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(12); // Fsm.StartEngine(25);

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