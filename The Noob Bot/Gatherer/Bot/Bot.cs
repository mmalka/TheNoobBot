using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Bot.Tasks;
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
                                 GathererSetting.CurrentSetting.ProfileName))
                    return false;
                Profile =
                    XmlSerializer.Deserialize<GathererProfile>(Application.StartupPath + "\\Profiles\\Gatherer\\" +
                                                               GathererSetting.CurrentSetting.ProfileName);
                if (Profile.Points.Count <= 0)
                    return false;

                // Reverse profil
                if (GathererSetting.CurrentSetting.PathingReverseDirection)
                    Profile.Points.Reverse();

                NpcDB.AddNpcRange(Profile.Npc);
                var blackListDic = Profile.BlackListRadius.ToDictionary(b => b.Position, b => b.Radius);
                nManager.nManagerSetting.AddRangeBlackListZone(blackListDic);

                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 12});
                Fsm.AddState(new Resurrect {Priority = 11});
                Fsm.AddState(new IsAttacked {Priority = 10});
                Fsm.AddState(new Looting {Priority = 9});
                Fsm.AddState(new Regeneration {Priority = 8});
                Fsm.AddState(new ToTown {Priority = 7});
                Fsm.AddState(new LevelupCheck {Priority = 6});
                Fsm.AddState(new Trainers {Priority = 5});
                Fsm.AddState(new MillingState {Priority = 4});
                Fsm.AddState(new ProspectingState {Priority = 3});
                Fsm.AddState(new Farming {Priority = 2});
                Fsm.AddState(new MovementLoop {Priority = 1, PathLoop = Profile.Points});
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(13);

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
                CombatClass.DisposeCombatClass();
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