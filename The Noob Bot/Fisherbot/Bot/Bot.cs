using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Fisherbot.Profile;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Bot.States;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace Fisherbot.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static FisherbotProfile Profile = new FisherbotProfile();

        internal static bool Pulse()
        {
            try
            {
                Profile = new FisherbotProfile();

                // If Fish School Load Profile
                if (FisherbotSetting.CurrentSetting.FishSchool)
                {
                    if (File.Exists(Application.StartupPath + "\\Profiles\\Fisherbot\\" + FisherbotSetting.CurrentSetting.FishSchoolProfil))
                    {
                        Profile = XmlSerializer.Deserialize<FisherbotProfile>(Application.StartupPath + "\\Profiles\\Fisherbot\\" + FisherbotSetting.CurrentSetting.FishSchoolProfil);
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
                Dictionary<Point, float> blackListDic = Profile.BlackListRadius.ToDictionary(b => b.Position, b => b.Radius);
                nManager.nManagerSetting.AddRangeBlackListZone(blackListDic);

                // Load CC:
                CombatClass.LoadCombatClass();

                if (!FisherbotSetting.CurrentSetting.FishSchool)
                {
                    FisherbotSetting.CurrentSetting.FisherbotPosition = ObjectManager.Me.Position;
                    FisherbotSetting.CurrentSetting.FisherbotRotation = ObjectManager.Me.Rotation;
                }

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 200});
                Fsm.AddState(new Resurrect {Priority = 140});
                Fsm.AddState(new IsAttacked {Priority = 130});
                Fsm.AddState(new ToTown {Priority = 120});
                Fsm.AddState(new Looting {Priority = 110});
                Fsm.AddState(new Travel {Priority = 100});
                Fsm.AddState(new Regeneration {Priority = 90});
                Fsm.AddState(new SpecializationCheck {Priority = 80});
                Fsm.AddState(new LevelupCheck {Priority = 70});
                Fsm.AddState(new Trainers {Priority = 60});
                Fsm.AddState(new AutoItemCombiner {Priority = 52});
                Fsm.AddState(new MillingState {Priority = 51});
                Fsm.AddState(new ProspectingState {Priority = 50});
                Fsm.AddState(new Farming {Priority = 30});
                Fsm.AddState(new FisherbotState {Priority = 20});
                Fsm.AddState(new MovementLoop {Priority = 10, PathLoop = Profile.Points});
                Fsm.AddState(new Idle {Priority = 0});

                foreach (var statePlugin in Plugins.ListLoadedStatePlugins)
                {
                    Fsm.AddState(statePlugin);
                }

                Fsm.States.Sort();
                Fsm.StartEngine(7, "FSM Fisherbot");

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
                FishingTask.StopLoopFish();
                CombatClass.DisposeCombatClass();
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