using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Quester.Profile;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;
using nManager.Wow.Class;

namespace Quester.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static QuesterProfile Profile = new QuesterProfile();

        internal static bool Pulse()
        {
            try
            {
                Profile = new QuesterProfile();
                LoadQuesterProfile f = new LoadQuesterProfile();
                f.ShowDialog();
                if (!string.IsNullOrWhiteSpace(QuesterSettings.CurrentSettings.LastProfile) &&
                    ((QuesterSettings.CurrentSettings.LastProfileSimple &&
                      File.Exists(Application.StartupPath + "\\Profiles\\Quester\\" + QuesterSettings.CurrentSettings.LastProfile)) ||
                     (!QuesterSettings.CurrentSettings.LastProfileSimple &&
                      File.Exists(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\" + QuesterSettings.CurrentSettings.LastProfile))))
                {
                    Profile = QuesterSettings.CurrentSettings.LastProfileSimple
                                  ? XmlSerializer.Deserialize<QuesterProfile>(Application.StartupPath + "\\Profiles\\Quester\\" + QuesterSettings.CurrentSettings.LastProfile)
                                  : XmlSerializer.Deserialize<QuesterProfile>(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\" +
                                                                              QuesterSettings.CurrentSettings.LastProfile);

                    foreach (Include include in Profile.Includes)
                    {
                        try
                        {
                            if (!Tasks.Script.Run(include.ScriptCondition)) continue;
                            //Logging.Write(Translation.GetText(Translation.Text.SubProfil) + " " + include.PathFile);
                            QuesterProfile profileInclude = XmlSerializer.Deserialize<QuesterProfile>(Application.StartupPath + "\\Profiles\\Quester\\" + include.PathFile);
                            if (profileInclude != null)
                            {
                                Profile.Questers.AddRange(profileInclude.Questers);
                                Profile.Blackspots.AddRange(profileInclude.Blackspots);
                                Profile.AvoidMobs.AddRange(profileInclude.AvoidMobs);
                                Profile.Quests.AddRange(profileInclude.Quests);
                            }
                        }

                        catch (Exception e)
                        {
                            MessageBox.Show(Translate.Get(Translate.Id.File_not_found) + ": " + e);
                            Logging.Write(Translate.Get(Translate.Id.File_not_found));
                            return false;
                        }
                    }
                    // Now check the integrity by checking we have all NPC required
                    foreach (Profile.Quest q in Profile.Quests)
                    {
                        if (q.ItemPickUp == 0 && FindQuesterById(q.PickUp).Entry == 0)
                        {
                            MessageBox.Show("Your profile is missing the definition of NPC entry " + q.PickUp +
                                            "\nThe quest is " + q.Name + ". Cannot continues!");
                            return false;
                        }
                        if (FindQuesterById(q.TurnIn).Entry == 0)
                        {
                            MessageBox.Show("Your profile is missing the definition of NPC entry " + q.TurnIn +
                                            "\nThe quest is " + q.Name + ". Cannot continues!");
                            return false;
                        }
                    }
                    Logging.Write("Loaded " + Profile.Quests.Count + " quests");
                    Profile.Filter();
                    Logging.Write(Profile.Quests.Count + " quests left after filtering on class/race");

                    Tasks.QuestingTask.completed = false;

                    nManager.Wow.Helpers.Quest.ConsumeQuestsCompletedRequest();
                    Logging.Write("received " + nManager.Wow.Helpers.Quest.FinishedQuestSet.Count + " quests.");
                }
                else
                    return false;

                // Black List:
                Dictionary<Point, float> blackListDic = new Dictionary<Point, float>();
                foreach (QuesterBlacklistRadius b in Profile.Blackspots)
                {
                    blackListDic.Add(b.Position, b.Radius);
                }
                nManagerSetting.AddRangeBlackListZone(blackListDic);

                // Load CC:
                CombatClass.LoadCombatClass();

                const int QuesterStatePriority = 2;

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 14});
                Fsm.AddState(new Resurrect {Priority = 13});
                Fsm.AddState(new IsAttacked {Priority = 12});
                Fsm.AddState(new Looting {Priority = 11});
                Fsm.AddState(new Regeneration {Priority = 10});
                Fsm.AddState(new ToTown {Priority = 9});
                Fsm.AddState(new SpecializationCheck {Priority = 8});
                Fsm.AddState(new LevelupCheck {Priority = 7});
                Fsm.AddState(new Trainers {Priority = 6});
                Fsm.AddState(new MillingState {Priority = 5});
                Fsm.AddState(new ProspectingState {Priority = 4});
                Fsm.AddState(new Farming {Priority = 3});
                Fsm.AddState(new QuesterState {Priority = QuesterStatePriority});
                Fsm.AddState(new MovementLoop {Priority = 1});
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(6, "FSM Quester");

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
                Fsm.StopEngine();
                Fight.StopFight();
                MovementManager.StopMove();
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Bot > Bot  > Dispose(): " + e);
            }
        }

        public static Npc FindQuesterById(int entry)
        {
            foreach (Npc npc in Profile.Questers)
            {
                if (npc.Entry == entry)
                    return npc;
            }
            return NpcDB.GetNpcByEntry(entry);
        }
    }
}