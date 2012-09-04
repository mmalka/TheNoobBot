using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace Gatherer.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();
        public static GathererProfile Profile = new GathererProfile();

        internal static bool Pulse()
        {
            try
            {
                // Load Profile:
                var f = new LoadProfile();
                f.ShowDialog();
                if (
                    !File.Exists(Application.StartupPath + "\\Profiles\\Gatherer\\" +
                                 GathererSetting.CurrentSetting.profileName))
                    return false;
                Profile =
                    XmlSerializer.Deserialize<GathererProfile>(Application.StartupPath + "\\Profiles\\Gatherer\\" +
                                                               GathererSetting.CurrentSetting.profileName);
                if (Profile.Points.Count <= 0)
                    return false;

                // Reverse profil
                if (GathererSetting.CurrentSetting.pathingReverseDirection)
                    Profile.Points.Reverse();

                NpcDB.AddNpcRange(Profile.Npc);
                var blackListDic = Profile.BlackListRadius.ToDictionary(b => b.Position, b => b.Radius);
                nManager.nManagerSetting.AddRangeBlackListZone(blackListDic);

                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CustomClass.LoadCustomClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new nManager.Wow.Bot.States.Pause {Priority = 12});
                Fsm.AddState(new nManager.Wow.Bot.States.Resurrect {Priority = 11});
                Fsm.AddState(new nManager.Wow.Bot.States.IsAttacked {Priority = 10});
                Fsm.AddState(new nManager.Wow.Bot.States.Regeneration {Priority = 9});
                Fsm.AddState(new nManager.Wow.Bot.States.Looting {Priority = 8});
                Fsm.AddState(new nManager.Wow.Bot.States.Farming {Priority = 7});
                Fsm.AddState(new nManager.Wow.Bot.States.MillingState { Priority = 6 });
                Fsm.AddState(new nManager.Wow.Bot.States.ProspectingState { Priority = 5 });
                Fsm.AddState(new nManager.Wow.Bot.States.ToTown {Priority = 4});
                Fsm.AddState(new nManager.Wow.Bot.States.Talents {Priority = 3});
                Fsm.AddState(new nManager.Wow.Bot.States.Trainers {Priority = 2});

                Fsm.AddState(new nManager.Wow.Bot.States.MovementLoop {Priority = 1, PathLoop = Profile.Points});
                Fsm.AddState(new nManager.Wow.Bot.States.Idle {Priority = 0});

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
                Logging.WriteError("Gatherer > Bot > Bot  > Pulse(): " + e);
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
                Logging.WriteError("Gatherer > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}
