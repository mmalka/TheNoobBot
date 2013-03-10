using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Grinder.Profile;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
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

        internal static MovementLoop _movementLoop = new MovementLoop {Priority = 1};
        internal static Grinding _grinding = new Grinding {Priority = 2};

        internal static bool Pulse()
        {
            try
            {
                Profile = new GrinderProfile();
                var f = new LoadProfile();
                f.ShowDialog();
                // If grinder School Load Profile
                if (!string.IsNullOrWhiteSpace(GrinderSetting.CurrentSetting.profileName) &&
                    File.Exists(Application.StartupPath + "\\Profiles\\Grinder\\" +
                                GrinderSetting.CurrentSetting.profileName))
                {
                    Profile =
                        XmlSerializer.Deserialize<GrinderProfile>(Application.StartupPath + "\\Profiles\\Grinder\\" +
                                                                  GrinderSetting.CurrentSetting.profileName);
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
                var blackListDic = new Dictionary<Point, float>();
                foreach (var zone in Profile.GrinderZones)
                {
                    foreach (var b in zone.BlackListRadius)
                    {
                        blackListDic.Add(b.Position, b.Radius);
                    }
                }
                nManager.nManagerSetting.AddRangeBlackListZone(blackListDic);

                // Add Npc
                foreach (var zone in Profile.GrinderZones)
                {
                    NpcDB.AddNpcRange(zone.Npc);
                }

                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CombatClass.LoadCombatClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 14});
                Fsm.AddState(new SelectProfileState {Priority = 13});
                Fsm.AddState(new Resurrect {Priority = 12});
                Fsm.AddState(new IsAttacked {Priority = 11});
                Fsm.AddState(new Looting {Priority = 10});
                Fsm.AddState(new Regeneration {Priority = 9});
                Fsm.AddState(new ToTown {Priority = 8});
                Fsm.AddState(new Talents {Priority = 7});
                Fsm.AddState(new Trainers {Priority = 6});
                Fsm.AddState(new MillingState {Priority = 5});
                Fsm.AddState(new ProspectingState {Priority = 4});
                Fsm.AddState(new Farming {Priority = 3});
                Fsm.AddState(_grinding);
                Fsm.AddState(_movementLoop);
                Fsm.AddState(new Idle {Priority = 0});

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
                var pointsTemps = new List<Point>();
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

            _grinding.EntryTarget = Profile.GrinderZones[ZoneIdProfile].TargetEntry;
            _grinding.FactionsTarget = Profile.GrinderZones[ZoneIdProfile].TargetFactions;
            _grinding.MaxTargetLevel = Profile.GrinderZones[ZoneIdProfile].MaxTargetLevel;
            _grinding.MinTargetLevel = Profile.GrinderZones[ZoneIdProfile].MinTargetLevel;

            _movementLoop.PathLoop = Profile.GrinderZones[ZoneIdProfile].Points;
        }
    }
}