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

        internal static BattlegrounderProfile Profile = new BattlegrounderProfile();
        internal static int ZoneIdProfile;

        internal static MovementLoop _movementLoop = new MovementLoop {Priority = 1};
        internal static Battlegrounding _battlegrounding = new Battlegrounding {Priority = 5};

        internal static bool Pulse()
        {
            try
            {
                Profile = new BattlegrounderProfile();
                var f = new LoadProfile();
                f.ShowDialog();
                // TODO: If Battlegrounder.Profile Load Profile else Action
                if (!string.IsNullOrWhiteSpace(BattlegrounderSetting.CurrentSetting.profileName) &&
                    File.Exists(Application.StartupPath + "\\Profiles\\Battlegrounder\\" +
                                BattlegrounderSetting.CurrentSetting.profileName))
                {
                    Profile =
                        XmlSerializer.Deserialize<BattlegrounderProfile>(Application.StartupPath + "\\Profiles\\Battlegrounder\\" +
                                                                  BattlegrounderSetting.CurrentSetting.profileName);
                    if (Profile.BattlegrounderZones.Count <= 0)
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
                foreach (var zone in Profile.BattlegrounderZones)
                {
                    foreach (var b in zone.BlackListRadius)
                    {
                        blackListDic.Add(b.Position, b.Radius);
                    }
                }
                nManager.nManagerSetting.AddRangeBlackListZone(blackListDic);

                // Add Npc
                foreach (var zone in Profile.BattlegrounderZones)
                {
                    NpcDB.AddNpcRange(zone.Npc);
                }

                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CustomClass.LoadCustomClass();

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 11});
                Fsm.AddState(new SelectProfileState {Priority = 10});
                Fsm.AddState(new Resurrect {Priority = 9});
                Fsm.AddState(new IsAttacked {Priority = 8});
                Fsm.AddState(new Regeneration {Priority = 7});
                Fsm.AddState(new Looting {Priority = 6});
                Fsm.AddState(_battlegrounding);
                Fsm.AddState(new ToTown {Priority = 4});
                Fsm.AddState(new Talents {Priority = 3});
                Fsm.AddState(new Trainers {Priority = 2});
                //Fsm.AddState(new nManager.Wow.Bot.States.MovementLoop { Priority = 1, PathLoop = Profile.Points });
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
        internal static void SelectZone()
        {
            for (int i = 0; i <= Profile.BattlegrounderZones.Count - 1; i++)
            {
                /*if (Profile.BattlegrounderZones[i].MaxLevel >= ObjectManager.Me.Level &&
                    Profile.BattlegrounderZones[i].MinLevel <= ObjectManager.Me.Level &&
                    Profile.BattlegrounderZones[i].IsValid())
                {*/
                    ZoneIdProfile = i;
                    break;
                /*}*/
            }

            if (Profile.BattlegrounderZones[ZoneIdProfile].Hotspots)
            {
                var pointsTemps = new List<Point>();
                for (int i = 0; i <= Profile.BattlegrounderZones[ZoneIdProfile].Points.Count - 1; i++)
                {
                    if (i + 1 > Profile.BattlegrounderZones[ZoneIdProfile].Points.Count - 1)
                        pointsTemps.AddRange(PathFinder.FindPath(Profile.BattlegrounderZones[ZoneIdProfile].Points[i],
                                                                 Profile.BattlegrounderZones[ZoneIdProfile].Points[0]));
                    else
                        pointsTemps.AddRange(PathFinder.FindPath(Profile.BattlegrounderZones[ZoneIdProfile].Points[i],
                                                                 Profile.BattlegrounderZones[ZoneIdProfile].Points[i + 1]));
                }
                Profile.BattlegrounderZones[ZoneIdProfile].Hotspots = false;
                Profile.BattlegrounderZones[ZoneIdProfile].Points.Clear();
                Profile.BattlegrounderZones[ZoneIdProfile].Points.AddRange(pointsTemps);
            }

            _battlegrounding.EntryTarget = Profile.BattlegrounderZones[ZoneIdProfile].TargetEntry;
            _battlegrounding.FactionsTarget = Profile.BattlegrounderZones[ZoneIdProfile].TargetFactions;
            _battlegrounding.MaxTargetLevel = Profile.BattlegrounderZones[ZoneIdProfile].MaxTargetLevel;
            _battlegrounding.MinTargetLevel = Profile.BattlegrounderZones[ZoneIdProfile].MinTargetLevel;

            _movementLoop.PathLoop = Profile.BattlegrounderZones[ZoneIdProfile].Points;
        }
    }
}