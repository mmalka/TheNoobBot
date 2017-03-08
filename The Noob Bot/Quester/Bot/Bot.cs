using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using nManager.Wow.Bot.Tasks;
using Quester.Profile;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Helpers;
using nManager.Wow.Class;
using Quester.Tasks;
using Quest = nManager.Wow.Helpers.Quest;

namespace Quester.Bot
{
    internal class Bot
    {
        private static readonly Engine Fsm = new Engine();

        internal static QuesterProfile Profile;

        internal static bool Pulse()
        {
            try
            {
                MountTask.AllowMounting = true;
                Quest.GetSetIgnoreFight = false;
                Quest.GetSetDismissPet = false;
                Profile = new QuesterProfile();
                QuesterProfileLoader f = new QuesterProfileLoader();
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
                                // Profile.Includes.AddRange(profileInclude.Includes);
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
                        bool isWorldQuest = q.WorldQuestLocation != null && q.WorldQuestLocation.IsValid;
                        if (!isWorldQuest && q.ItemPickUp == 0 && FindQuesterById(q.PickUp).Entry == 0 && !q.AutoAccepted)
                        {
                            MessageBox.Show("Your profile is missing the definition of NPC entry " + q.PickUp +
                                            "\nThe quest is '" + q.Name + "' (" + q.Id + "). Cannot continues!");
                            return false;
                        }
                        if (!isWorldQuest && FindQuesterById(q.TurnIn).Entry == 0)
                        {
                            MessageBox.Show("Your profile is missing the definition of NPC entry " + q.TurnIn +
                                            "\nThe quest is '" + q.Name + "' (" + q.Id + "). Cannot continues!");
                            return false;
                        }
                        foreach (Profile.QuestObjective o in q.Objectives)
                        {
                            if (o.NpcEntry != 0 && FindQuesterById(o.NpcEntry).Entry == 0)
                            {
                                MessageBox.Show("Your profile is missing the definition of NPC entry " + o.NpcEntry +
                                                "\nThe quest is '" + q.Name + "' (" + q.Id + "). Cannot continues!");
                                return false;
                            }
                            if (o.InternalIndex != 0 && o.Count <= 0 && o.CollectCount <= 0)
                            {
                                MessageBox.Show("Your profile has an objective with an InternalIndex but without proper Count or CollectCount values" +
                                                "\nThe quest is '" + q.Name + "' (" + q.Id + "). Cannot continues!");
                                return false;
                            }
                            if (o.InternalIndex > 23)
                            {
                                MessageBox.Show("Your profile has an objective with an InternalIndex > 23, which is not possible." +
                                                "\nThe quest is '" + q.Name + "' (" + q.Id + "). Cannot continues!");
                                return false;
                            }
                        }
                    }
                    Logging.Write("Loaded " + Profile.Quests.Count + " quests");
                    Profile.Filter();
                    Logging.Write(Profile.Quests.Count + " quests left after filtering on class/race");

                    Tasks.QuestingTask.completed = false;

                    Quest.ConsumeQuestsCompletedRequest();
                    Logging.Write("received " + Quest.FinishedQuestSet.Count + " quests.");
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

                const int questerStatePriority = 2;
                ImportedQuesters = false;

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 100});
                Fsm.AddState(new Resurrect {Priority = 15});
                Fsm.AddState(new IsAttacked {Priority = 14});
                Fsm.AddState(new Regeneration {Priority = 13});
                Fsm.AddState(new FlightMasterDiscovery {Priority = 12});
                Fsm.AddState(new Looting {Priority = 11});
                Fsm.AddState(new Travel {Priority = 10});
                Fsm.AddState(new ToTown {Priority = 9});
                Fsm.AddState(new SpecializationCheck {Priority = 8});
                Fsm.AddState(new LevelupCheck {Priority = 7});
                Fsm.AddState(new Trainers {Priority = 6});
                Fsm.AddState(new MillingState {Priority = 5});
                Fsm.AddState(new ProspectingState {Priority = 4});
                Fsm.AddState(new Farming {Priority = 3});
                Fsm.AddState(new QuesterState {Priority = questerStatePriority});
                Fsm.AddState(new MovementLoop {Priority = 1});
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(10, "FSM Quester");
                EventsListener.HookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_DETAIL, callback => Quest.AutoCompleteQuest());
                EventsListener.HookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_AUTOCOMPLETE, callback => Quest.AutoCompleteQuest());

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
                Script.CachedScripts = new Dictionary<string, IScript>();
                // clear cache on Stop.
                Fsm.StopEngine();
                Fight.StopFight();
                MovementManager.StopMove();
                MountTask.AllowMounting = true;
                Quest.GetSetIgnoreFight = false;
                Quest.GetSetDismissPet = false;
                Profile = null;
                Tasks.QuestingTask.Cleanup();
                EventsListener.UnHookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_DETAIL, callback => Quest.AutoCompleteQuest());
                EventsListener.UnHookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_AUTOCOMPLETE, callback => Quest.AutoCompleteQuest());
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Bot > Bot  > Dispose(): " + e);
            }
        }

        public static bool ImportedQuesters = false;

        public static Npc FindQuesterById(int entry)
        {
            if (!ImportedQuesters)
            {
                if (Profile.Questers.Count > 0)
                    QuestersDB.AddNpcRange(Profile.Questers);
                ImportedQuesters = true;
            }
            return QuestersDB.GetNpcByEntry(entry);
        }

        public static Npc FindNearestQuesterById(int entry)
        {
            if (!ImportedQuesters)
            {
                if (Profile.Questers.Count > 0)
                    QuestersDB.AddNpcRange(Profile.Questers);
                ImportedQuesters = true;
            }
            return QuestersDB.GetNpcNearbyByEntry(entry);
        }
    }
}