using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using Questing_Bot.Profile;
using WowManager;
using WowManager.FiniteStateMachine;
using WowManager.Navigation;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.SpellManager;

namespace Questing_Bot.Bot
{
    public class Bot
    {
        internal readonly Engine Fsm = new Engine();
        public readonly Profile.Profile Profile = new Profile.Profile();

        public bool BotIsActive;


        public Quest CurrentQuest = new Quest();
        public QuestObjective CurrentQuestObjective;
        public string QuestStat = "";

        public ReadOnlyCollection<ulong> BlackListGuid
        {
            get
            {
                return new ReadOnlyCollection<ulong>(Tasks.Black.ListGuidValid(BlackList));
            }
        }

        public readonly List<Tasks.Black> BlackList = new List<Tasks.Black>();

        public float SearchDistance { get { return Profile.SearchDistance; } }

        public bool DisableRepairAndVendor;
        public bool DisableLoot;

        // Form config
        public readonly MainFormConfig FormConfig = new MainFormConfig();

        // Stat
        public int Kills;
        public int Deaths;
        public int NumberFarm;
        public int NumberLoot;
        public bool Pause;
        public bool ForcePause;
        public bool ForceGoToTown;
        public bool ForceGoToTrainers;

        internal Bot()
        {
        }

        internal Bot(MainFormConfig mainFormConfig, bool grinding = false)
        {
            try
            {
                // Init var:
                FormConfig = mainFormConfig;

                // Active bot
                BotIsActive = true;

                // Update spell list
                SpellManager.UpdateSpellBook();

                // Config path finder:
                PathFinderManager.UsePatherFind = mainFormConfig.UsePathFinder;

                // Launch CC
                if (File.Exists(Application.StartupPath + "\\CustomClasses\\" + mainFormConfig.CustomClassName))
                    CustomClass.LoadCustomClass(Application.StartupPath + "\\CustomClasses\\" + mainFormConfig.CustomClassName);
                else
                    Log.AddLog(Translation.GetText(Translation.Text.Custom_Class_no_found));

                // Load Profile
                try
                {
                    if (grinding)
                    {
                        // Leveling Profile
                        Log.AddLog("Grinding Profile.");
                        var profileGrinding = new Grinding.Profile();
                        if (File.Exists(Application.StartupPath + "\\Products\\Questing Bot\\Profiles Grinding\\" + mainFormConfig.ProfileGrinding))
                            profileGrinding = WowManager.Others.XmlSerializerHelper.Deserialize<Grinding.Profile>(Application.StartupPath + "\\Products\\Questing Bot\\Profiles Grinding\\" + mainFormConfig.ProfileGrinding);
                        else
                            profileGrinding = WowManager.Others.XmlSerializerHelper.Deserialize<Grinding.Profile>(Application.StartupPath + "\\Products\\Leveling Bot\\Profiles\\" + mainFormConfig.ProfileGrinding);

                        Profile = new Profile.Profile();

                        try
                        {
                            if (mainFormConfig.WaterNameGrinding.Replace(" ", "") != String.Empty)
                                Profile.BuyItems.Add(new Buy { Count = 10, Id = Convert.ToInt32(mainFormConfig.WaterNameGrinding), MaxLevel = 85, MinLevel = 0, Name = WowManager.WoW.ItemManager.Item.GetNameById(Convert.ToUInt32(mainFormConfig.WaterNameGrinding)), TypeBuy = TypeBuy.Water });
                            if (mainFormConfig.FoodNameGrinding.Replace(" ", "") != String.Empty)
                                Profile.BuyItems.Add(new Buy { Count = 10, Id = Convert.ToInt32(WowManager.WoW.ItemManager.Item.GetIdByName(mainFormConfig.FoodNameGrinding)), MaxLevel = 85, MinLevel = 0, Name = WowManager.WoW.ItemManager.Item.GetNameById(Convert.ToUInt32(mainFormConfig.FoodNameGrinding)), TypeBuy = TypeBuy.Food });
                        }
                        catch
                        {
                        }

                        foreach (var subProfile in profileGrinding.SubProfiles)
                        {
                            var qo = new QuestObjective
                            {
                                Count = 999999999,
                                Factions = subProfile.Factions,
                                Hotspots = subProfile.Locations,
                                Objective = Objective.KillMob,
                            };

                            var q = new Quest
                            {
                                Id = -1,
                                MaxLevel = subProfile.MaxLevel,
                                MinLevel = subProfile.MinLevel,
                                Name = subProfile.Name,
                            };

                            q.Objectives.Add(qo);

                            Profile.Quests.Add(q);

                            foreach (var blackListZone in subProfile.BlackListZones)
                            {
                                Profile.Blackspots.Add(new Blackspot
                                {
                                    Position = blackListZone.Point,
                                    Radius = blackListZone.Radius
                                });

                            }

                            Profile.GoToList.Add(new GoTo { Points = subProfile.GhostLocations });
                            Profile.GoToList.Add(new GoTo { Points = subProfile.ToSubProfileLocations });
                        }
                    }
                    else
                    {
                        // Questing Profile
                        Profile =
                            WowManager.Others.XmlSerializerHelper.Deserialize<Profile.Profile>(Application.StartupPath +
                                                                                               "\\Products\\Questing Bot\\Profiles\\" +
                                                                                               mainFormConfig.
                                                                                                   ProfileName);
                    }

                    if (Profile == null)
                        BotIsActive = false;
                    else
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.Load_Profile) + " " + Profile.Name);
                        if (ObjectManager.Me.Level < Profile.MinLevel || ObjectManager.Me.Level > Profile.MaxLevel)
                        {
                            MessageBox.Show(Translation.GetText(Translation.Text.Error) + ": " +
                                            Translation.GetText(Translation.Text.Profile) + " " +
                                            Translation.GetText(Translation.Text.for_) + " " +
                                            Translation.GetText(Translation.Text.Level) + " " + Profile.MinLevel + " - " +
                                            Profile.MaxLevel + ". " + Translation.GetText(Translation.Text.Stoping_bot));
                            BotIsActive = false;
                        }
                        else // Load include
                        {

                            foreach (var include in Profile.Includes)
                            {
                                try
                                {
                                    if (Tasks.Script.Run(include.ScriptCondition))
                                    {
                                        Log.AddLog(Translation.GetText(Translation.Text.SubProfil) + " " +
                                                   include.PathFile);
                                        var profileInclude =
                                            WowManager.Others.XmlSerializerHelper.Deserialize<Profile.Profile>(
                                                Application.StartupPath + "\\Products\\Questing Bot\\Profiles\\" +
                                                include.PathFile);
                                        if (profileInclude != null)
                                        {
                                            Profile.AvoidMobs.AddRange(profileInclude.AvoidMobs);
                                            Profile.Blackspots.AddRange(profileInclude.Blackspots);
                                            Profile.BuyItems.AddRange(profileInclude.BuyItems);
                                            Profile.KeepItems.AddRange(profileInclude.KeepItems);
                                            Profile.Mailboxes.AddRange(profileInclude.Mailboxes);
                                            Profile.MailBoxNames.AddRange(profileInclude.MailBoxNames);
                                            Profile.Vendors.AddRange(profileInclude.Vendors);
                                            Profile.GoToList.AddRange(profileInclude.GoToList);
                                            Profile.Quests.AddRange(profileInclude.Quests);
                                        }
                                    }

                                }

                                catch (Exception e)
                                {
                                    MessageBox.Show(Translation.GetText(Translation.Text.Profile_error) + ": " + e);
                                    Log.AddLog(Translation.GetText(Translation.Text.Profile_error));
                                    BotIsActive = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(Translation.GetText(Translation.Text.Profile_error) + ": " + e);
                    Log.AddLog(Translation.GetText(Translation.Text.Profile_error));
                    BotIsActive = false;
                }

                // FSM
                Fsm.States.Clear();

                Fsm.AddState(new States.Resurrect());
                Fsm.AddState(new States.ToTown());
                Fsm.AddState(new States.Farming());
                Fsm.AddState(new States.Looting());
                Fsm.AddState(new States.Regeneration());
                Fsm.AddState(new States.Trainers());
                Fsm.AddState(new States.IsAttacked());
                Fsm.AddState(new States.Pause());
                Fsm.AddState(new States.Reloger());
                Fsm.AddState(new States.Questing());
                Fsm.AddState(new States.Talents());

                Fsm.States.Sort();
                Fsm.StartEngine(25);

                // Log Launch bot.
                Log.AddLog(Translation.GetText(Translation.Text.Launch_Bot));


            }
            catch (Exception e)
            {
                BotIsActive = false;
                MessageBox.Show(e.ToString());
            }
            // If error:
            if (!BotIsActive)
                Dispose();
        }

        internal void Dispose()
        {
            Log.AddLog(Translation.GetText(Translation.Text.Stoping_bot));
            BotIsActive = false;
            CustomClass.DisposeCustomClass();
            Fsm.StopEngine();
            WowManager.WoW.Interface.Quest.FinishQuestForceSet = new List<int>();
            WowManager.WoW.PlayerManager.Fight.StopFight();
            MovementManager.StopMove();
        }
    }
}
