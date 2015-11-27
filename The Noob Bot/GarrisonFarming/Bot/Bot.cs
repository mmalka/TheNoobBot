using System;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;

namespace GarrisonFarming.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();
        public static GarrisonFarmingProfile Profile = new GarrisonFarmingProfile();

        internal static bool Pulse()
        {
            try
            {
                /*
                Dictionary<Point, float> blackListDic = Profile.BlackListRadius.ToDictionary(b => b.Position, b => b.Radius);
                nManager.nManagerSetting.AddRangeBlackListZone(blackListDic);
                */

                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 100});
                Fsm.AddState(new Resurrect {Priority = 13});
                Fsm.AddState(new IsAttacked {Priority = 12});
                //Fsm.AddState(new Regeneration {Priority = 11});
                Fsm.AddState(new ToTown {Priority = 10});
                Fsm.AddState(new Looting {Priority = 9});
                Fsm.AddState(new SpecializationCheck {Priority = 7});
                Fsm.AddState(new LevelupCheck {Priority = 6});
                Fsm.AddState(new GarrisonState {Priority = 5});
                /* 3-4
                 * EmptyGarrisonCache
                 * SendWorkOrders
                 */
                Fsm.AddState(new Farming {Priority = 2});

                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(10, "FSM GarrisonFarming");

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
            }
            catch (Exception e)
            {
                Logging.WriteError("GarrisonFarming > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}