using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Grinder.Profile;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Bot.States;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace Grinder.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static GrinderProfile Profile = new GrinderProfile();
        internal static int ZoneIdProfile;

        internal static MovementLoop MovementLoop = new MovementLoop {Priority = 10};
        internal static Grinding Grinding = new Grinding {Priority = 30};

        internal static bool Pulse()
        {
            try
            {
                Profile = new GrinderProfile();
                LoadProfile f = new LoadProfile();
                f.ShowDialog();
                // If grinder School Load Profile
                if (!string.IsNullOrWhiteSpace(GrinderSetting.CurrentSetting.ProfileName) &&
                    File.Exists(Application.StartupPath + "\\Profiles\\Grinder\\" +
                                GrinderSetting.CurrentSetting.ProfileName))
                {
                    Profile =
                        XmlSerializer.Deserialize<GrinderProfile>(Application.StartupPath + "\\Profiles\\Grinder\\" +
                                                                  GrinderSetting.CurrentSetting.ProfileName);
                    if (Profile.GrinderZones.Count <= 0)
                        return false;
                }
                else
                {
                    MessageBox.Show(Translate.Get(Translate.Id.Please_select_an_profile));
                    return false;
                }

                SelectZone();


                // Black List:
                Dictionary<Point, float> blackListDic = new Dictionary<Point, float>();
                foreach (GrinderZone zone in Profile.GrinderZones)
                {
                    foreach (GrinderBlackListRadius b in zone.BlackListRadius)
                    {
                        if (!blackListDic.ContainsKey(b.Position))
                            blackListDic.Add(b.Position, b.Radius);
                    }
                }
                nManagerSetting.AddRangeBlackListZone(blackListDic);

                // Add Npc
                foreach (GrinderZone zone in Profile.GrinderZones)
                {
                    NpcDB.AddNpcRange(zone.Npc);
                }

                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 200});
                Fsm.AddState(new SelectProfileState {Priority = 150});
                Fsm.AddState(new Resurrect {Priority = 140});
                Fsm.AddState(new IsAttacked {Priority = 130});
                Fsm.AddState(new Regeneration {Priority = 120});
                Fsm.AddState(new ToTown {Priority = 110});
                Fsm.AddState(new Looting {Priority = 100});
                Fsm.AddState(new Travel {Priority = 90});
                Fsm.AddState(new SpecializationCheck {Priority = 80});
                Fsm.AddState(new LevelupCheck {Priority = 70});
                Fsm.AddState(new Trainers {Priority = 60});
                Fsm.AddState(new MillingState {Priority = 50});
                Fsm.AddState(new ProspectingState {Priority = 40});
                Fsm.AddState(Grinding);
                Fsm.AddState(new Farming {Priority = 20});
                Fsm.AddState(MovementLoop);
                Fsm.AddState(new Idle {Priority = 0});

                foreach (var statePlugin in Plugins.ListLoadedStatePlugins)
                {
                    Fsm.AddState(statePlugin);
                }

                Fsm.States.Sort();
                Fsm.StartEngine(7, "FSM Grinder");

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
                Logging.WriteError("Grinder > Bot > Bot  > Pulse(): " + e);
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
                Logging.WriteError("Grinder > Bot > Bot  > Dispose(): " + e);
            }
        }

        internal static void SelectZone()
        {
            for (int i = 0; i <= Profile.GrinderZones.Count - 1; i++)
            {
                if (Profile.GrinderZones[i].MaxLevel >= ObjectManager.Me.Level &&
                    Profile.GrinderZones[i].MinLevel <= ObjectManager.Me.Level &&
                    Profile.GrinderZones[i].IsValid())
                {
                    ZoneIdProfile = i;
                    break;
                }
            }

            if (Profile.GrinderZones[ZoneIdProfile].Hotspots)
            {
                List<Point> pointsTemps = new List<Point>();
                for (int i = 0; i <= Profile.GrinderZones[ZoneIdProfile].Points.Count - 1; i++)
                {
                    if (i + 1 > Profile.GrinderZones[ZoneIdProfile].Points.Count - 1)
                        pointsTemps.AddRange(PathFinder.FindPath(Profile.GrinderZones[ZoneIdProfile].Points[i],
                            Profile.GrinderZones[ZoneIdProfile].Points[0]));
                    else
                        pointsTemps.AddRange(PathFinder.FindPath(Profile.GrinderZones[ZoneIdProfile].Points[i],
                            Profile.GrinderZones[ZoneIdProfile].Points[i + 1]));
                }
                Profile.GrinderZones[ZoneIdProfile].Hotspots = false;
                Profile.GrinderZones[ZoneIdProfile].Points.Clear();
                Profile.GrinderZones[ZoneIdProfile].Points.AddRange(pointsTemps);
            }

            Grinding.EntryTarget = Profile.GrinderZones[ZoneIdProfile].TargetEntry;
            Grinding.FactionsTarget = Profile.GrinderZones[ZoneIdProfile].TargetFactions;
            Grinding.MaxTargetLevel = Profile.GrinderZones[ZoneIdProfile].MaxTargetLevel;
            Grinding.MinTargetLevel = Profile.GrinderZones[ZoneIdProfile].MinTargetLevel;

            MovementLoop.PathLoop = Profile.GrinderZones[ZoneIdProfile].Points;
        }
    }
}