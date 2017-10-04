using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Bot.States;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace Gatherer.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();
        public static GathererProfile Profile = new GathererProfile();

        internal static bool Pulse(bool autoLoaded = false)
        {
            try
            {
                if (!autoLoaded)
                {
                    // Load Profile:
                    LoadProfile f = new LoadProfile();
                    f.ShowDialog();
                }
                if (!File.Exists(Application.StartupPath + "\\Profiles\\Gatherer\\" + GathererSetting.CurrentSetting.ProfileName))
                    return false;
                Profile = XmlSerializer.Deserialize<GathererProfile>(Application.StartupPath + "\\Profiles\\Gatherer\\" + GathererSetting.CurrentSetting.ProfileName);
                if (Profile.Points.Count <= 0)
                    return false;

                // Reverse profil
                if (GathererSetting.CurrentSetting.PathingReverseDirection)
                    Profile.Points.Reverse();

                NpcDB.AddNpcRange(Profile.Npc);
                nManagerSetting.AddRangeBlackListZone(new List<nManagerSetting.BlackListZone>(Profile.BlackListRadius));

                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 200});
                Fsm.AddState(new Resurrect {Priority = 130});
                Fsm.AddState(new IsAttacked {Priority = 120});
                Fsm.AddState(new Regeneration {Priority = 110});
                Fsm.AddState(new ToTown {Priority = 100});
                Fsm.AddState(new Looting {Priority = 90});
                Fsm.AddState(new Travel {Priority = 80});
                Fsm.AddState(new SpecializationCheck {Priority = 70});
                Fsm.AddState(new LevelupCheck {Priority = 60});
                Fsm.AddState(new Trainers {Priority = 59});
                Fsm.AddState(new AutoItemCombiner {Priority = 52});
                Fsm.AddState(new MillingState {Priority = 51});
                Fsm.AddState(new ProspectingState {Priority = 50});
                Fsm.AddState(new Farming {Priority = 20});
                Fsm.AddState(new MovementLoop {Priority = 10, PathLoop = Profile.Points});
                Fsm.AddState(new Idle {Priority = 0});

                foreach (var statePlugin in Plugins.ListLoadedStatePlugins)
                {
                    Fsm.AddState(statePlugin);
                }

                Fsm.States.Sort();
                Fsm.StartEngine(7, "FSM Gatherer");

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