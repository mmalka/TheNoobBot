using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Quester.Profile;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Bot;
using nManager.Wow.Class;

namespace Quester.Bot
{
    class Bot
    {
        static readonly Engine Fsm = new Engine();

        internal static QuesterProfile Profile = new QuesterProfile();

        internal static bool Pulse()
        {
            try
            {
                Profile = new QuesterProfile();
                var f = new LoadProfile();
                f.ShowDialog();
                // Load Profile
                if (!string.IsNullOrWhiteSpace(QuesterSetting.CurrentSetting.profileName) && File.Exists(Application.StartupPath + "\\Profiles\\Quester\\" + QuesterSetting.CurrentSetting.profileName))
                {
                    Profile =
                        XmlSerializer.Deserialize<QuesterProfile>(Application.StartupPath + "\\Profiles\\Quester\\" +
                                                                  QuesterSetting.CurrentSetting.profileName);
                    
                    foreach (var include in Profile.Includes)
                    {
                        try
                        {
                            if (Tasks.Script.Run(include.ScriptCondition))
                            {
                                //Logging.Write(Translation.GetText(Translation.Text.SubProfil) + " " +
                                //            include.PathFile);
                                var profileInclude =
                                    XmlSerializer.Deserialize<QuesterProfile>(
                                        Application.StartupPath + "\\Profiles\\Quester\\" +
                                        include.PathFile);
                                if (profileInclude != null)
                                {
                                    Profile.Quests.AddRange(profileInclude.Quests);
                                }
                            }

                        }

                        catch (Exception e)
                        {
                            MessageBox.Show(Translate.Get(Translate.Id.File_not_found) + ": " + e);
                            Logging.Write(Translate.Get(Translate.Id.File_not_found));
                            return false;
                        }
                    }

                    Logging.Write("Loaded " + Profile.Quests.Count + " quests");
                    Profile.Filter();
                    Logging.Write(Profile.Quests.Count + " quests left after filtering on class/race");

                    if (Profile.Quests.Count <= 0)
                        return false;
                }
                else
                {
                    MessageBox.Show(Translate.Get(Translate.Id.Please_select_an_profile));
                    return false;
                }

                // Black List:
                var blackListDic = new Dictionary<Point, float>();
                foreach (var b in Profile.Blackspots)
                {
                    blackListDic.Add(b.Position, b.Radius);
                }
                nManager.nManagerSetting.AddRangeBlackListZone(blackListDic);

                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CustomClass.LoadCustomClass();

                // PathFinder:
                //PathFinderDroidz.AddRangeListPointForPath(Profile.PointsPathFinderDroidz);
                //PathFinderDroidz.AddRangeListPointForPath(Profile.Points);

                int QuesterStatePriority = 6;

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new nManager.Wow.Bot.States.Pause { Priority = 10 });
                Fsm.AddState(new nManager.Wow.Bot.States.Resurrect { Priority = 9 });
                Fsm.AddState(new nManager.Wow.Bot.States.IsAttacked { Priority = 8 });
                Fsm.AddState(new nManager.Wow.Bot.States.Regeneration { Priority = 7 });
                Fsm.AddState(new nManager.Wow.Bot.States.Looting { Priority = 6 });
                Fsm.AddState(new nManager.Wow.Bot.States.Farming { Priority = 5 });
                Fsm.AddState(new QuesterState { Priority = QuesterStatePriority });
                Fsm.AddState(new nManager.Wow.Bot.States.ProspectingState { Priority = 4 });
                Fsm.AddState(new nManager.Wow.Bot.States.ToTown { Priority = 3 });
                Fsm.AddState(new nManager.Wow.Bot.States.Talents { Priority = 2 });
                Fsm.AddState(new nManager.Wow.Bot.States.Trainers { Priority = 1 });

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
                Logging.WriteError("Quester > Bot > Bot  > Pulse(): " + e);
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
                Logging.WriteError("Quester > Bot > Bot  > Dispose(): " + e);
            }
        }
    }
}
