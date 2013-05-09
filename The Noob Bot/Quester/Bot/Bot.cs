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
using nManager.Wow.ObjectManager;
using nManager.Wow.Bot;
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
                var f = new LoadProfile();
                f.ShowDialog();
                // Load Profile
                if (!string.IsNullOrWhiteSpace(QuesterSetting.CurrentSetting.profileName) &&
                    File.Exists(Application.StartupPath + "\\Profiles\\Quester\\" +
                                QuesterSetting.CurrentSetting.profileName))
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
                    // Now check the integrity by checking we have all NPC required
                    foreach (Profile.Quest q in Profile.Quests)
                    {
                        if (q.ItemPickUp == 0 && FindQuesterById(q.PickUp).Entry == 0)
                        {
                            MessageBox.Show("Your profile is missing the definition of NPC entry " + q.PickUp +
                                "\nThe quest is " +  q.Name + ". Cannot continues!");
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

                    Quester.Tasks.QuestingTask.completed = false;

                    nManager.Wow.Helpers.Quest.ConsumeQuestsCompletedRequest();
                    Logging.Write("received " + nManager.Wow.Helpers.Quest.FinishedQuestSet.Count + " quests.");
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
                CombatClass.LoadCombatClass();

                int QuesterStatePriority = 2;

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new Pause {Priority = 13});
                Fsm.AddState(new Resurrect {Priority = 12});
                Fsm.AddState(new IsAttacked {Priority = 11});
                Fsm.AddState(new Looting {Priority = 10});
                Fsm.AddState(new Regeneration {Priority = 9});
                Fsm.AddState(new ToTown {Priority = 8});
                Fsm.AddState(new LevelupCheck {Priority = 7});
                Fsm.AddState(new Trainers {Priority = 6});
                Fsm.AddState(new MillingState {Priority = 5});
                Fsm.AddState(new ProspectingState {Priority = 4});
                Fsm.AddState(new Farming {Priority = 3});
                Fsm.AddState(new QuesterState {Priority = QuesterStatePriority});
                Fsm.AddState(new MovementLoop {Priority = 1});
                Fsm.AddState(new Idle {Priority = 0});

                Fsm.States.Sort();
                Fsm.StartEngine(6);

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
                CombatClass.DisposeCombatClass();
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