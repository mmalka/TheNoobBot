using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Fisherbot.Profile;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace Fisherbot.Bot
{
    class Bot
    {
        static readonly Engine Fsm = new Engine();

        internal static FisherbotProfile Profile = new FisherbotProfile();

        internal static bool Pulse()
        {
            try
            {
                Profile = new FisherbotProfile();

                // If Fish School Load Profile
                if (FisherbotSetting.CurrentSetting.fishSchool)
                {
                    if (!string.IsNullOrWhiteSpace(FisherbotSetting.CurrentSetting.FisherbotPoolName) && File.Exists(Application.StartupPath + "\\Profiles\\Fisherbot\\"+ FisherbotSetting.CurrentSetting.fishSchoolProfil))
                    {
                        Profile =
                            XmlSerializer.Deserialize<FisherbotProfile>(Application.StartupPath + "\\Profiles\\Fisherbot\\" +
                                                                      FisherbotSetting.CurrentSetting.fishSchoolProfil);
                        if (Profile.Points.Count <= 0)
                            return false;
                    }
                    else
                    {
                        MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Please_select_an_profile_or_disable_School_Fish_option));
                        return false;
                    }
                }

                // Black List:
                var blackListDic = Profile.BlackListRadius.ToDictionary(b => b.Position, b => b.Radius);
                nManager.nManagerSetting.AddRangeBlackListZone(blackListDic);

                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CustomClass.LoadCustomClass();

                if (!FisherbotSetting.CurrentSetting.fishSchool)
                {
                    FisherbotSetting.CurrentSetting.FisherbotPosition = ObjectManager.Me.Position;
                    FisherbotSetting.CurrentSetting.FisherbotRotation = ObjectManager.Me.Rotation;
                }

                int fisherbotStatePriority;
                if (FisherbotSetting.CurrentSetting.fishSchool)
                    fisherbotStatePriority = 6;
                else
                    fisherbotStatePriority = 1;

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new nManager.Wow.Bot.States.Pause { Priority = 11 });
                Fsm.AddState(new nManager.Wow.Bot.States.Resurrect { Priority = 10 });
                Fsm.AddState(new nManager.Wow.Bot.States.IsAttacked { Priority = 9 });
                Fsm.AddState(new nManager.Wow.Bot.States.Regeneration { Priority = 8 });
                Fsm.AddState(new nManager.Wow.Bot.States.Looting { Priority = 7 });
                Fsm.AddState(new nManager.Wow.Bot.States.Farming { Priority = 6 });
                Fsm.AddState(new FisherbotState { Priority = fisherbotStatePriority });
                Fsm.AddState(new nManager.Wow.Bot.States.ProspectingState { Priority = 5 });
                Fsm.AddState(new nManager.Wow.Bot.States.ToTown { Priority = 4 });
                Fsm.AddState(new nManager.Wow.Bot.States.Talents { Priority = 3 });
                Fsm.AddState(new nManager.Wow.Bot.States.Trainers { Priority = 2 });

                Fsm.AddState(new nManager.Wow.Bot.States.MovementLoop { Priority = 1, PathLoop = Profile.Points });
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
                Logging.WriteError("Fisherbot > Bot > Bot  > Pulse(): " + e);
                return false;
            }
        }

        internal static void Dispose()
        {
            try
            {
                nManager.Wow.Bot.Tasks.FishingTask.StopLoopFish();
                CustomClass.DisposeCustomClass();
                Fsm.StopEngine();
                Fight.StopFight();
                MovementManager.StopMove();
            }
            catch (Exception e)
            {
                Logging.WriteError("Fisherbot > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}
