using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager.Products;
using nManager.Wow.Bot.States;
using Quester.Bot;
using Quester.Profile;
using nManager;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.Class;
using nManager.Wow.ObjectManager;
using Quest = nManager.Wow.Helpers.Quest;
using Timer = nManager.Helpful.Timer;
using nManager.Wow.Bot.Tasks;
using Keybindings = nManager.Wow.Helpers.Keybindings;
using Math = nManager.Helpful.Math;

namespace Quester.Tasks
{
    internal class QuestingTask
    {
        private static string QuestStatus = "";
        public static Profile.Quest CurrentQuest;
        private static int _currentQuestObjectiveId = -1;
        public static QuestObjective CurrentQuestObjective;
        private static Timer waitTimer;
        public static bool completed = false;
        private static int EntryListRow = 0;

        public static void Cleanup()
        {
            QuestStatus = "";
            CurrentQuest = null;
            _currentQuestObjectiveId = -1;
            CurrentQuestObjective = null;
            waitTimer = null;
            completed = false;
            EntryListRow = 0;
        }

        public static void SelectQuest()
        {
            QuestStatus = "Select Quest";
            CurrentQuest = null;
            _currentQuestObjectiveId = -1;
            CurrentQuestObjective = null;

            Bot.Bot.Profile.Quests.Sort();

            for (int relax = 0; relax <= 2; relax++) // search quest with level = mine, then level = mine+1, then +2
            {
                foreach (Profile.Quest quest in Bot.Bot.Profile.Quests)
                {
                    if (Quest.GetLogQuestId().Contains(quest.Id) && quest.MinLevel <= ObjectManager.Me.Level)
                    {
                        CurrentQuest = quest;
                        Logging.Write("resuming \"" + quest.Name + "\": Lvl " + quest.QuestLevel + " (" + quest.MinLevel + " - " + quest.MaxLevel + ")");
                        return;
                    }
                }
                foreach (Profile.Quest quest in Bot.Bot.Profile.Quests)
                {
                    if (quest.AutoAccepted)
                        continue;
                    if (ObjectManager.Me.Level >= quest.MinLevel && (ObjectManager.Me.Level <= quest.MaxLevel || QuesterSettings.CurrentSettings.IgnoreQuestsMaxLevel) &&
                        (QuesterSettings.CurrentSettings.IgnoreQuestsLevel || ObjectManager.Me.Level >= quest.QuestLevel - relax)) // Level
                        if (!Quest.GetQuestCompleted(quest.Id)) // Quest not completed
                            if (!Quest.GetQuestCompleted(quest.NeedQuestNotCompletedId)) // Quest done which discalify this one
                                if (Quest.GetQuestCompleted(quest.NeedQuestCompletedId) || // One of these quest need to be completed
                                    quest.NeedQuestCompletedId.Count == 0)
                                    if (quest.ItemPickUp == 0 || (quest.ItemPickUp != 0 && ItemsManager.GetItemCount(quest.ItemPickUp) > 0))
                                        if (Script.Run(quest.ScriptCondition)) // Condition
                                        {
                                            CurrentQuest = quest;
                                            Logging.Write("\"" + quest.Name + "\": Lvl " + quest.QuestLevel + " (" + quest.MinLevel + " - " + quest.MaxLevel + ")");
                                            return;
                                        }
                }
            }
            if (!completed)
            {
                Logging.Write("There is no more quest to do.");
                completed = true;
            }
        }

        public static void SelectNextQuestObjective()
        {
            CurrentQuestObjective = null;
            while (true)
            {
                _currentQuestObjectiveId++;
                if (_currentQuestObjectiveId <= CurrentQuest.Objectives.Count - 1) // In array
                {
                    if (CurrentQuest.Objectives[_currentQuestObjectiveId].InternalIndex != 0 &&
                        Quest.IsObjectiveCompleted(CurrentQuest.Objectives[_currentQuestObjectiveId].InternalQuestId != 0 ? CurrentQuest.Objectives[_currentQuestObjectiveId].InternalQuestId : CurrentQuest.Id,
                            CurrentQuest.Objectives[_currentQuestObjectiveId].InternalIndex,
                            CurrentQuest.Objectives[_currentQuestObjectiveId].Count > 0 ? CurrentQuest.Objectives[_currentQuestObjectiveId].Count : CurrentQuest.Objectives[_currentQuestObjectiveId].CollectCount))
                        continue;
                    if (Script.Run(CurrentQuest.Objectives[_currentQuestObjectiveId].ScriptCondition))
                        // Script condition
                    {
                        CurrentQuestObjective = CurrentQuest.Objectives[_currentQuestObjectiveId];
                        break;
                    }
                }
                else
                    break; // Out array
            }
        }

        public static void ResetQuestObjective()
        {
            QuestStatus = "Reset Quest Objective";
            _currentQuestObjectiveId = -1;
            CurrentQuestObjective = null;
            foreach (QuestObjective obj in CurrentQuest.Objectives)
            {
                obj.CurrentCount = 0;
                obj.IsObjectiveCompleted = false;
            }
            SelectNextQuestObjective();
        }

        public static bool AllForcedObjectiveComplete()
        {
            foreach (QuestObjective obj in CurrentQuest.Objectives)
            {
                QuestObjective objective = obj;
                if (objective.InternalQuestId > 0 && objective.InternalQuestId != objective.QuestId && objective.IsObjectiveCompleted)
                    continue; // Hack when dealing with multiple-quest and restarting the bot.

                if (objective.IgnoreQuestCompleted && !QuestObjectiveIsFinish(ref objective))
                    return false;
            }
            return true;
        }

        public static bool QuestObjectiveIsFinish(ref QuestObjective questObjective)
        {
            Quest.GetSetIgnoreFight = false;
            if (questObjective == null)
                return true;

            // shortcut since we do objective one by one, for kill it can be completed before we do them all
            if (!questObjective.IgnoreQuestCompleted && Quest.GetLogQuestIsComplete(questObjective.InternalQuestId != 0 ? questObjective.InternalQuestId : CurrentQuest.Id))
                return true;

            if (questObjective.Objective != Objective.PickUpQuest && questObjective.InternalQuestId > 0 && !Quest.GetLogQuestId().Contains(questObjective.InternalQuestId))
            {
                if (questObjective.IsBonusObjective)
                {
                    if (!Quest.GetQuestCompleted(questObjective.InternalQuestId) && !Quest.IsQuestFlaggedCompletedLUA(questObjective.InternalQuestId))
                    {
                        return false; // Force the bot to go on zone to check about the current status of the objective as the quest has not been completed.
                    }
                }
                return true; // We don't have this nested quest anymore. The first check is "just in case", but a PickUpQuest objective shouldn't contains InternalQuestId anyway.
            }

            // If we can check the objective in quest log, then rely on it
            if (questObjective.InternalIndex != 0 && (questObjective.Count > 0 || questObjective.CollectCount > 0))
                return Quest.IsObjectiveCompleted(questObjective.InternalQuestId != 0 ? questObjective.InternalQuestId : CurrentQuest.Id, questObjective.InternalIndex,
                    questObjective.Count > 0 ? questObjective.Count : questObjective.CollectCount);

            if (questObjective.ScriptConditionIsComplete != string.Empty)
                return Script.Run(questObjective.ScriptConditionIsComplete);

            if (questObjective.Objective == Objective.TravelTo)
            {
                if (questObjective.ContinentId != Usefuls.ContinentId)
                    return false;
                List<WoWUnit> p = ObjectManager.GetObjectWoWUnit();
                foreach (WoWUnit unit in p)
                {
                    foreach (int i in questObjective.Entry)
                    {
                        if (unit.Entry == i)
                        {
                            return true;
                            // We use field Entry as a "IsArrivedCheck".
                        }
                    }
                }
                return false;
            }

            // COLLECT ITEM || BUY ITEM
            if (questObjective.CollectItemId > 0 && questObjective.CollectCount > 0)
                if (ItemsManager.GetItemCount(questObjective.CollectItemId) < questObjective.CollectCount)
                    return false;

            // KILL MOB
            if (questObjective.Objective == Objective.KillMob)
            {
                if (questObjective.Count == -1)
                    return false;
                if (questObjective.CurrentCount >= questObjective.Count)
                    return true;
                return false;
            }

            // PICK UP OBJECT
            if (questObjective.Objective == Objective.PickUpObject)
            {
                if (questObjective.CurrentCount >= questObjective.CollectCount)
                    return true;
                if (questObjective.CollectItemId > 0 && questObjective.CollectCount > 0 &&
                    ItemsManager.GetItemCount(questObjective.CollectItemId) >= questObjective.CollectCount)
                    return true;
                return false;
            }

            // PICK UP NPC
            if (questObjective.Objective == Objective.PickUpNPC)
            {
                return questObjective.Count > 0 ? questObjective.CurrentCount >= questObjective.Count : questObjective.IsObjectiveCompleted;
            }

            // USE ITEM
            if (questObjective.Objective == Objective.UseItem)
            {
                return questObjective.Count > 0 ? questObjective.CurrentCount >= questObjective.Count : questObjective.IsObjectiveCompleted;
            }

            // BUY ITEM
            if (questObjective.Objective == Objective.BuyItem)
            {
                if (ItemsManager.GetItemCount(questObjective.CollectItemId) >= questObjective.CollectCount)
                    return true;
                return false;
            }

            // PICK UP QUEST
            if (questObjective.Objective == Objective.PickUpQuest)
            {
                return Quest.GetQuestCompleted(questObjective.QuestId) || Quest.GetLogQuestId().Contains(questObjective.QuestId) || Quest.IsQuestFlaggedCompletedLUA(questObjective.QuestId);
            }

            // TURN IN QUEST
            if (questObjective.Objective == Objective.TurnInQuest)
            {
                return Quest.GetQuestCompleted(questObjective.QuestId) || !Quest.GetLogQuestId().Contains(questObjective.QuestId) || Quest.IsQuestFlaggedCompletedLUA(questObjective.QuestId);
            }

            /* MOVE TO || WAIT || INTERACT WITH ||
             * USE SPELL || EQUIP ITEM || PICK UP QUEST ||
             * TURN IN QUEST || PRESS KEY || USE ITEM AOE ||
             * USE SPELL AOE || USE RUNEFORGE || USE FLIGHT PATH
             */
            if (questObjective.Objective == Objective.MoveTo || questObjective.Objective == Objective.Wait || questObjective.Objective == Objective.InteractWith ||
                questObjective.Objective == Objective.UseSpell || questObjective.Objective == Objective.EquipItem || questObjective.Objective == Objective.PickUpQuest ||
                questObjective.Objective == Objective.TurnInQuest || questObjective.Objective == Objective.PressKey || questObjective.Objective == Objective.UseItemAOE ||
                questObjective.Objective == Objective.UseSpellAOE || questObjective.Objective == Objective.UseRuneForge || questObjective.Objective == Objective.UseFlightPath ||
                questObjective.Objective == Objective.UseLuaMacro || questObjective.Objective == Objective.ClickOnTerrain || questObjective.Objective == Objective.MessageBox ||
                questObjective.Objective == Objective.GarrisonHearthstone)
            {
                return questObjective.IsObjectiveCompleted;
            }

            // USE VEHICLE
            if (questObjective.Objective == Objective.UseVehicle)
            {
                return Usefuls.PlayerUsingVehicle;
            }

            // EJECT VEHICLE
            if (questObjective.Objective == Objective.EjectVehicle)
            {
                return !Usefuls.PlayerUsingVehicle;
            }

            // APPLY BUFF
            if (questObjective.Objective == Objective.ApplyBuff)
            {
                if (questObjective.CurrentCount >= questObjective.BuffCount)
                    return true;
                return false;
            }

            return false;
        }

        public static bool CurrentQuestObjectiveIsFinish()
        {
            return QuestObjectiveIsFinish(ref CurrentQuestObjective);
        }

        public static void CurrentQuestObjectiveExecute()
        {
            QuestObjectiveExecute(ref CurrentQuestObjective);
        }

        public static bool IsInAvoidMobsList(WoWUnit woWUnit)
        {
            for (int i = 0; i < Bot.Bot.Profile.AvoidMobs.Count; i++)
            {
                Npc npc = Bot.Bot.Profile.AvoidMobs[i];
                if ((npc.Position.Z == 0f || npc.Position.DistanceTo(woWUnit.Position) <= 40) && npc.Entry == woWUnit.Entry)
                    return true;
            }

            return false;
        }

        public static void QuestObjectiveExecute(ref QuestObjective questObjective)
        {
            if (questObjective == null)
                return;

            QuestStatus = questObjective.Objective.ToString();
            CheckMandatoryFieldsByObjective(questObjective);
            if (questObjective.OnlyInVehicule && !ObjectManager.Me.InTransport)
            {
                questObjective.IsObjectiveCompleted = true;
                // we cannot do it now, do it later
                return;
            }
            if (questObjective.OnlyOutVehicule && ObjectManager.Me.InTransport)
            {
                questObjective.IsObjectiveCompleted = true;
                // we cannot do it now, do it later
                return;
            }

            // Create Path:
            if (questObjective.PathHotspots == null)
            {
                questObjective.PathHotspots = new List<Point>();
                if (questObjective.Hotspots.Count > 0)
                {
                    for (int i = 0; i <= questObjective.Hotspots.Count - 1; i++)
                    {
                        int iLast = i - 1;
                        if (iLast < 0)
                            iLast = questObjective.Hotspots.Count - 1;
                        List<Point> points = new List<Point>();
                        if (iLast != i)
                        {
                            Logging.Write( /*Translate.Get(...)*/ "Create path from Hotspot #" + (iLast + 1) + " to Hotspot #" + (i + 1));
                            points = PathFinder.FindPath(questObjective.Hotspots[iLast], questObjective.Hotspots[i]);
                            questObjective.PathHotspots.AddRange(points);
                        }
                        else
                        {
                            Logging.Write("Create path to Hotspot #" + (i + 1));
                            questObjective.PathHotspots.Add(questObjective.Hotspots[i]);
                            questObjective.PathHotspots.Add(questObjective.Hotspots[i]);
                        }
                    }
                }
                else if (questObjective.Position.IsValid)
                {
                    List<Point> points = PathFinder.FindPath(questObjective.Position);
                    questObjective.PathHotspots.AddRange(points);
                }
                else
                {
                    questObjective.PathHotspots.Add(ObjectManager.Me.Position);
                    questObjective.PathHotspots.Add(ObjectManager.Me.Position);
                    Logging.Write("There is no specific position defined for the next QuestObjective, wont move. (this is not necessarily an issue)");
                }
            }

            // KILL MOB
            if (questObjective.Objective == Objective.KillMob)
            {
                // Count mobs killed by the defense system
                List<int> entryList = questObjective.Entry;
                int alreadyKilled = Quest.KilledMobsToCount.FindAll(entryList.Contains).Count;
                if (alreadyKilled > 0)
                {
                    questObjective.CurrentCount += alreadyKilled;
                    Quest.KilledMobsToCount.RemoveAll(entryList.Contains);
                    return;
                }

                WoWUnit wowUnit = new WoWUnit(0);
                if (questObjective.CollectItemId != 0)
                    wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByQuestLoot(questObjective.CollectItemId), questObjective.IgnoreBlackList);

                if (!wowUnit.IsValid)
                    wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), questObjective.IgnoreBlackList);

                if (!wowUnit.IsValid && questObjective.Factions.Count > 0)
                    wowUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByFaction(questObjective.Factions), true, questObjective.IgnoreBlackList);

                if (!IsInAvoidMobsList(wowUnit) && !nManagerSetting.IsBlackListedZone(wowUnit.Position) &&
                    !nManagerSetting.IsBlackListed(wowUnit.Guid) && wowUnit.IsAlive && wowUnit.IsValid &&
                    (questObjective.CanPullUnitsAlreadyInFight || !wowUnit.InCombat))
                {
                    MovementManager.FindTarget(wowUnit, wowUnit.AggroDistance*1.1f);
                    if (MovementManager.InMovement)
                        return;
                    Logging.Write("Attacking Lvl " + wowUnit.Level + " " + wowUnit.Name);
                    UInt128 Unkillable = Fight.StartFight(wowUnit.Guid);
                    if (!wowUnit.IsDead && Unkillable != 0 && wowUnit.HealthPercent == 100.0f)
                    {
                        nManagerSetting.AddBlackList(Unkillable, 3*60*1000);
                        Logging.Write("Can't reach " + wowUnit.Name + ", blacklisting it.");
                    }
                    else if (wowUnit.IsDead)
                    {
                        Statistics.Kills++;
                        if (!wowUnit.IsTapped || (wowUnit.IsTapped && wowUnit.IsTappedByMe))
                        {
                            questObjective.CurrentCount++;
                        }
                        Thread.Sleep(50 + Usefuls.Latency);
                        while (!ObjectManager.Me.IsMounted && ObjectManager.Me.InCombat &&
                               ObjectManager.GetNumberAttackPlayer() <= 0)
                        {
                            Thread.Sleep(Usefuls.Latency);
                        }
                        Fight.StopFight();
                    }
                }
                else if (!MovementManager.InMovement && questObjective.PathHotspots.Count > 0)
                {
                    // Mounting Mount
                    MountTask.Mount();
                    // Need GoTo Zone:
                    if (questObjective.PathHotspots[Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
                    }
                    else
                    {
                        // Start Move
                        MovementManager.GoLoop(questObjective.PathHotspots);
                    }
                }
            }

            // PICK UP OBJECT
            if (questObjective.Objective == Objective.PickUpObject)
            {
                if (questObjective.CurrentCount >= questObjective.CollectCount && questObjective.CollectCount > 0)
                    return;
                WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                if (!nManagerSetting.IsBlackListedZone(node.Position) && !nManagerSetting.IsBlackListed(node.Guid) && node.IsValid)
                {
                    uint tNumber = Statistics.Farms;
                    FarmingTask.Pulse(new List<WoWGameObject> {node});
                    if (Statistics.Farms > tNumber)
                        questObjective.CurrentCount++;
                    else if (node.GetDistance < 5)
                    {
                        Thread.Sleep(300);
                        nManagerSetting.AddBlackList(node.Guid, 1000*60*3);
                    }
                }
                else if (!MovementManager.InMovement && questObjective.PathHotspots.Count > 0)
                {
                    // Mounting Mount
                    MountTask.Mount();
                    // Need GoTo Zone:
                    if (
                        questObjective.PathHotspots[Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5f)
                    {
                        MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
                    }
                    else
                    {
                        // Start Move
                        MovementManager.GoLoop(questObjective.PathHotspots);
                    }
                }
            }
            // PICK UP NPC
            if (questObjective.Objective == Objective.PickUpNPC)
            {
                WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));
                Point pos;
                uint baseAddress;
                if (!nManagerSetting.IsBlackListedZone(unit.Position) && !nManagerSetting.IsBlackListed(unit.Guid) && unit.IsValid)
                {
                    if (unit.IsValid)
                    {
                        pos = new Point(unit.Position);
                        baseAddress = unit.GetBaseAddress;
                    }
                    else
                    {
                        if (questObjective.InternalQuestId > 0)
                        {
                            if (!Quest.GetLogQuestId().Contains(questObjective.InternalQuestId))
                                questObjective.IsObjectiveCompleted = true;
                        }
                        return;
                    }
                    MovementManager.Go(PathFinder.FindPath(pos));
                    Thread.Sleep(500 + Usefuls.Latency);
                    while (MovementManager.InMovement && pos.DistanceTo(ObjectManager.Me.Position) > 3.9f)
                    {
                        if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
                            return;
                        Thread.Sleep(100);
                    }
                    if (questObjective.IgnoreFight)
                        Quest.GetSetIgnoreFight = true;
                    MountTask.DismountMount();
                    MovementManager.StopMove();
                    Interact.InteractWith(baseAddress);
                    Thread.Sleep(Usefuls.Latency);
                    while (ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(Usefuls.Latency);
                    }

                    if (questObjective.GossipOptionsInteractWith != 0)
                    {
                        Thread.Sleep(250 + Usefuls.Latency);
                        Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
                    }
                    if (ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
                        return;
                    Thread.Sleep(questObjective.WaitMs);
                    questObjective.CurrentCount++;
                    nManagerSetting.AddBlackList(unit.Guid, 5000);
                    Quest.GetSetIgnoreFight = false;
                }
                else if (!MovementManager.InMovement && questObjective.PathHotspots.Count > 0)
                {
                    // Mounting Mount
                    MountTask.Mount();
                    // Need GoTo Zone:
                    if (
                        questObjective.PathHotspots[Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5f)
                    {
                        MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
                    }
                    else
                    {
                        // Start Move
                        MovementManager.GoLoop(questObjective.PathHotspots);
                    }
                }
            }

            // USE ITEM
            if (questObjective.Objective == Objective.UseItem)
            {
                if (!MovementManager.InMovement ||
                    ObjectManager.Me.Position.DistanceTo(questObjective.Position) < questObjective.Range)
                {
                    if (questObjective.Entry.Count > 0)
                    {
                        WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                        WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), true, questObjective.IgnoreBlackList);
                        if (node.IsValid)
                        {
                            questObjective.Position = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.Position = new Point(unit.Position);
                        }
                    }

                    if (questObjective.Position.Z != 0f && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        if (questObjective.IgnoreFight)
                            Quest.GetSetIgnoreFight = true;
                        MountTask.DismountMount();
                        MovementManager.StopMove();
                        if (questObjective.Entry.Count > 0)
                        {
                            WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                            WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), true, questObjective.IgnoreBlackList);
                            if (node.IsValid)
                            {
                                MovementManager.Face(node);
                                Interact.InteractWith(node.GetBaseAddress);
                                nManagerSetting.AddBlackList(node.Guid, 30*1000);
                            }
                            else if (unit.IsValid)
                            {
                                MovementManager.Face(unit);
                                Interact.InteractWith(unit.GetBaseAddress);
                                nManagerSetting.AddBlackList(unit.Guid, 30*1000);
                            }
                        }
                        if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
                            return;
                        ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
                        if (questObjective.Count > 0)
                            questObjective.CurrentCount++;
                        else if (questObjective.Count == 0) // Then -1 is making the objectif never ending
                            questObjective.IsObjectiveCompleted = true;
                        Thread.Sleep(questObjective.WaitMs);
                        Quest.GetSetIgnoreFight = false;
                    }
                }
            }


            // Lua Macro
            if (questObjective.Objective == Objective.UseLuaMacro)
            {
                if (!MovementManager.InMovement ||
                    ObjectManager.Me.Position.DistanceTo(questObjective.Position) < questObjective.Range)
                {
                    if (questObjective.Entry.Count > 0)
                    {
                        WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                        WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), true, questObjective.IgnoreBlackList);
                        if (node.IsValid)
                        {
                            questObjective.Position = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.Position = new Point(unit.Position);
                        }
                    }

                    if (questObjective.Position.Z != 0f && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        if (questObjective.IgnoreFight)
                            Quest.GetSetIgnoreFight = true;
                        MountTask.DismountMount();
                        MovementManager.StopMove();
                        if (questObjective.Entry.Count > 0)
                        {
                            WoWGameObject node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                            WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead), true, questObjective.IgnoreBlackList);
                            if (node.IsValid)
                            {
                                MovementManager.Face(node);
                                Interact.InteractWith(node.GetBaseAddress);
                                nManagerSetting.AddBlackList(node.Guid, 30*1000);
                            }
                            else if (unit.IsValid)
                            {
                                MovementManager.Face(unit);
                                Interact.InteractWith(unit.GetBaseAddress);
                                nManagerSetting.AddBlackList(unit.Guid, 30*1000);
                            }
                        }
                        Thread.Sleep(Usefuls.Latency);
                        Lua.RunMacroText(questObjective.LuaMacro);
                        if (questObjective.Count > 0)
                            questObjective.CurrentCount++;
                        else if (questObjective.Count == 0) // Then -1 is making the objectif never ending
                            questObjective.IsObjectiveCompleted = true;
                        Thread.Sleep(questObjective.WaitMs);
                        Quest.GetSetIgnoreFight = false;
                    }
                }
            }

            // MOVE TO
            if (questObjective.Objective == Objective.MoveTo)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.Position.Z != 0f && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        if (questObjective.WaitMs > 0)
                            Thread.Sleep(questObjective.WaitMs);
                        questObjective.IsObjectiveCompleted = true;
                    }
                }
            }

            // WAIT
            if (questObjective.Objective == Objective.Wait)
            {
                if (questObjective.IgnoreFight)
                    Quest.GetSetIgnoreFight = true;
                if (waitTimer == null)
                    waitTimer = new Timer(questObjective.WaitMs);
                if (waitTimer.IsReady)
                {
                    questObjective.IsObjectiveCompleted = true;
                    waitTimer = null;
                    Quest.GetSetIgnoreFight = false;
                }
            }

            if (questObjective.Objective == Objective.GarrisonHearthstone)
            {
                if (ObjectManager.Me.InCombat)
                    return;
                if (ItemsManager.GetItemCount(110560) > 0 && !ItemsManager.IsItemOnCooldown(110560) && ItemsManager.IsItemUsable(110560))
                {
                    ItemsManager.UseItem(ItemsManager.GetItemNameById(110560));
                    Thread.Sleep(6000);
                    Logging.WriteFight("Use Garrison Hearthstone");
                }
                questObjective.IsObjectiveCompleted = true;
            }

            // INTERACT WITH
            if (questObjective.Objective == Objective.InteractWith)
            {
                CheckMandatoryFieldsByType(questObjective, true, true);
                Thread.Sleep(250 + Usefuls.Latency);
                if (!MovementManager.InMovement)
                {
                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) < questObjective.Range)
                    {
                        WoWGameObject node =
                            ObjectManager.GetNearestWoWGameObject(
                                ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                        WoWUnit unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));
                        Point pos;
                        uint baseAddress;
                        if (node.IsValid)
                        {
                            pos = new Point(node.Position);
                            baseAddress = node.GetBaseAddress;
                        }
                        else if (unit.IsValid)
                        {
                            pos = new Point(unit.Position);
                            baseAddress = unit.GetBaseAddress;
                        }
                        else
                        {
                            if (questObjective.InternalQuestId > 0)
                            {
                                if (!Quest.GetLogQuestId().Contains(questObjective.InternalQuestId))
                                    questObjective.IsObjectiveCompleted = true;
                            }
                            return;
                        }

                        MovementManager.Go(PathFinder.FindPath(pos));
                        Thread.Sleep(500 + Usefuls.Latency);
                        while (MovementManager.InMovement && pos.DistanceTo(ObjectManager.Me.Position) > 3.9f)
                        {
                            if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
                                return;
                            Thread.Sleep(100);
                        }
                        if (questObjective.IgnoreFight)
                            Quest.GetSetIgnoreFight = true;
                        MountTask.DismountMount();
                        MovementManager.StopMove();
                        Interact.InteractWith(baseAddress);
                        Thread.Sleep(Usefuls.Latency);
                        while (ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(Usefuls.Latency);
                        }

                        if (questObjective.GossipOptionsInteractWith != 0)
                        {
                            Thread.Sleep(250 + Usefuls.Latency);
                            Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
                        }
                        if (Others.IsFrameVisible("StaticPopup1Button1"))
                            Lua.RunMacroText("/click StaticPopup1Button1");
                        if (ObjectManager.Me.InCombat && !questObjective.IgnoreFight)
                            return;
                        Thread.Sleep(questObjective.WaitMs);
                        questObjective.IsObjectiveCompleted = true;
                        Quest.GetSetIgnoreFight = false;
                    }
                    else
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                }
            }

            // USE SPELL
            if (questObjective.Objective == Objective.UseSpell)
            {
                if (!MovementManager.InMovement ||
                    ObjectManager.Me.Position.DistanceTo(questObjective.Position) < questObjective.Range)
                {
                    if (questObjective.Entry.Count > 0)
                    {
                        WoWGameObject node =
                            ObjectManager.GetNearestWoWGameObject(
                                ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                        WoWUnit unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));
                        if (node.IsValid)
                        {
                            questObjective.Position = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.Position = new Point(unit.Position);
                        }
                    }

                    if (questObjective.Position.Z != 0f && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        MountTask.DismountMount();
                        if (questObjective.Entry.Count > 0)
                        {
                            WoWGameObject node =
                                ObjectManager.GetNearestWoWGameObject(
                                    ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                            WoWUnit unit =
                                ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));
                            if (node.IsValid)
                            {
                                MovementManager.Face(node);
                                Interact.InteractWith(node.GetBaseAddress);
                                MovementManager.StopMove(); // because interact will make the character go to the target due to CTM
                            }
                            else if (unit.IsValid)
                            {
                                MovementManager.Face(unit);
                                Interact.InteractWith(unit.GetBaseAddress);
                                MovementManager.StopMove(); // because interact will make the character go to the target due to CTM
                            }
                        }
                        if (questObjective.IgnoreFight)
                            Quest.GetSetIgnoreFight = true;
                        Spell t = new Spell((uint) questObjective.UseSpellId);
                        for (int i = 0; i < questObjective.Count; i++)
                        {
                            while (!t.IsSpellUsable)
                                Thread.Sleep(Usefuls.Latency);
                            t.Launch();
                            Thread.Sleep(questObjective.WaitMs);
                        }
                        questObjective.IsObjectiveCompleted = true;
                        Quest.GetSetIgnoreFight = false;
                    }
                }
                else if (!MovementManager.InMovement && questObjective.PathHotspots.Count > 0)
                {
                    // Mounting Mount
                    MountTask.Mount();
                    // Need GoTo Zone:
                    if (
                        questObjective.PathHotspots[
                            Math.NearestPointOfListPoints(questObjective.PathHotspots,
                                ObjectManager.Me.Position)].DistanceTo(
                                    ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(
                            PathFinder.FindPath(
                                questObjective.PathHotspots[
                                    Math.NearestPointOfListPoints(questObjective.PathHotspots,
                                        ObjectManager.Me.Position)]));
                    }
                    else
                    {
                        // Start Move
                        MovementManager.GoLoop(questObjective.PathHotspots);
                    }
                }
            }

            // EQUIP ITEM
            if (questObjective.Objective == Objective.EquipItem)
            {
                if (ObjectManager.Me.IsDeadMe || ObjectManager.Me.InCombat)
                    return;
                ItemsManager.EquipItemByName(ItemsManager.GetItemNameById(questObjective.EquipItemId));
                questObjective.IsObjectiveCompleted = true;
            }

            // PICK UP QUEST
            if (questObjective.Objective == Objective.PickUpQuest)
            {
                if (!MovementManager.InMovement)
                {
                    Npc Quester = Bot.Bot.FindQuesterById(questObjective.NpcEntry);
                    Quest.QuestPickUp(ref Quester, questObjective.QuestName, questObjective.QuestId);
                }
                if (Quest.GetLogQuestId().Contains(questObjective.QuestId) || Quest.GetLogQuestIsComplete(questObjective.QuestId) || Quest.IsQuestFlaggedCompletedLUA(questObjective.QuestId))
                    questObjective.IsObjectiveCompleted = true;
            }

            // TURN IN QUEST
            if (questObjective.Objective == Objective.TurnInQuest)
            {
                if (!MovementManager.InMovement)
                {
                    Npc Quester = Bot.Bot.FindQuesterById(questObjective.NpcEntry);
                    Quest.QuestTurnIn(ref Quester, questObjective.QuestName, questObjective.QuestId);
                }
                if (!Quest.GetLogQuestId().Contains(questObjective.QuestId))
                    questObjective.IsObjectiveCompleted = true;
            }

            // USE VEHICLE
            if (questObjective.Objective == Objective.UseVehicle)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.Position.Z != 0f &&
                        questObjective.Position.DistanceTo(ObjectManager.Me.Position) > nManagerSetting.CurrentSetting.GatheringSearchRadius)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        WoWUnit unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(new List<int> {questObjective.EntryVehicle}, questObjective.IsDead),
                                questObjective.Position);
                        if (!unit.IsValid)
                        {
                            return;
                        }

                        MovementManager.Go(PathFinder.FindPath(unit.Position));
                        Thread.Sleep(500 + Usefuls.Latency);
                        while (MovementManager.InMovement && unit.IsValid &&
                               ObjectManager.Me.Position.DistanceTo(unit.Position) > 4)
                        {
                            if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
                                return;
                            Thread.Sleep(Usefuls.Latency);
                        }
                        MountTask.DismountMount();
                        Interact.InteractWith(unit.GetBaseAddress);
                        Thread.Sleep(250 + Usefuls.Latency);
                    }
                }
            }

            // EJECT VEHICLE
            if (questObjective.Objective == Objective.EjectVehicle)
            {
                Usefuls.EjectVehicle();
                Thread.Sleep(250 + Usefuls.Latency);
            }

            // PRESS KEY
            if (questObjective.Objective == Objective.PressKey)
            {
                if (!MovementManager.InMovement ||
                    ObjectManager.Me.Position.DistanceTo(questObjective.Position) < 5.0f)
                {
                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) > 5.0f && questObjective.Position.Z != 0f)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        if (questObjective.IgnoreFight)
                            Quest.GetSetIgnoreFight = true;
                        MountTask.DismountMount();
                        Keybindings.DownKeybindings(questObjective.Keys);
                        Thread.Sleep(questObjective.WaitMs);
                        Keybindings.UpKeybindings(questObjective.Keys);
                        questObjective.IsObjectiveCompleted = true;
                        Quest.GetSetIgnoreFight = false;
                    }
                }
            }

            // Click On terrain
            if (questObjective.Objective == Objective.ClickOnTerrain)
            {
                ClickOnTerrain.ClickOnly(questObjective.Position);
                if (questObjective.WaitMs > 0)
                    Thread.Sleep(questObjective.WaitMs);
                questObjective.IsObjectiveCompleted = true;
                Quest.GetSetIgnoreFight = false;
            }

            // MessageBox
            if (questObjective.Objective == Objective.MessageBox)
            {
                MessageBox.Show(questObjective.Message);
                questObjective.IsObjectiveCompleted = true;
                Quest.GetSetIgnoreFight = false;
            }

            // USE SPELL AOE
            if (questObjective.Objective == Objective.UseSpellAOE)
            {
                if (!MovementManager.InMovement ||
                    questObjective.Position.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
                {
                    if (questObjective.Entry.Count > 0)
                    {
                        WoWGameObject node =
                            ObjectManager.GetNearestWoWGameObject(
                                ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                        WoWUnit unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead));
                        if (node.IsValid)
                        {
                            questObjective.Position = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.Position = new Point(unit.Position);
                        }
                    }
                    if (questObjective.Position.IsValid && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        if (questObjective.IgnoreFight)
                            Quest.GetSetIgnoreFight = true;
                        MountTask.DismountMount();
                        SpellManager.CastSpellByIDAndPosition((uint) questObjective.UseSpellId,
                            questObjective.Position);
                        Thread.Sleep(questObjective.WaitMs);
                        questObjective.IsObjectiveCompleted = true;
                        Quest.GetSetIgnoreFight = false;
                    }
                }
            }

            // USE ITEM AOE
            if (questObjective.Objective == Objective.UseItemAOE)
            {
                uint baseAddress = 0;
                Npc target = new Npc();
                int localEntry = GetEntryListRow(questObjective);
                if (questObjective.Entry.Count > 0 && localEntry > 0)
                {
                    target = new Npc
                    {
                        Entry = localEntry,
                        Position = questObjective.Position,
                        Name = questObjective.Name,
                        ContinentIdInt = Usefuls.ContinentId,
                        Faction = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
                        SelectGossipOption = questObjective.GossipOptionsInteractWith
                    };
                    baseAddress = MovementManager.FindTarget(ref target, questObjective.Range > 5f ? questObjective.Range : 0);
                    if (MovementManager.InMovement)
                        return;
                    //End target finding based on Entry.
                }
                if ((questObjective.Entry.Count <= 0 || localEntry <= 0) && baseAddress == 0)
                {
                    if (questObjective.IgnoreFight)
                        Quest.GetSetIgnoreFight = true;
                    if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
                        return;
                    ItemsManager.UseItem(questObjective.UseItemId, questObjective.Position);
                    Thread.Sleep(questObjective.WaitMs);
                    questObjective.IsObjectiveCompleted = true;
                    ResetEntryListRow();
                }
                else if (baseAddress > 0)
                {
                    if (questObjective.IgnoreFight)
                        Quest.GetSetIgnoreFight = true;
                    Interact.InteractWith(baseAddress);
                    if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
                        return;
                    ItemsManager.UseItem(questObjective.UseItemId, target.Position);
                    Thread.Sleep(questObjective.WaitMs);
                    questObjective.IsObjectiveCompleted = true;
                    ResetEntryListRow();
                }
                else
                {
                    UpdateEntryListRow(questObjective);
                    if (EntryListRow == -1)
                    {
                        Logging.Write("UseItemAOE objective: An entry has been provided in the profile, but is not valid nor spawned at the position. Use UseItemAOE by position instead.");
                    }
                    return; // target not found, try next Entry
                }
                Quest.GetSetIgnoreFight = false;
            }

            // BUY ITEM
            if (questObjective.Objective == Objective.BuyItem)
            {
                CheckMandatoryFieldsByType(questObjective, true, true, true, true);
                int localEntry = GetEntryListRow(questObjective);
                Npc target = new Npc
                {
                    Entry = localEntry,
                    Position = questObjective.Position,
                    Name = questObjective.Name,
                    ContinentIdInt = Usefuls.ContinentId,
                    Faction = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
                    SelectGossipOption = questObjective.GossipOptionsInteractWith
                };
                uint baseAddress = MovementManager.FindTarget(ref target);
                if (MovementManager.InMovement)
                    return;
                if (baseAddress > 0)
                {
                    Interact.InteractWith(baseAddress);
                    Thread.Sleep(250 + Usefuls.Latency);
                    if (target.SelectGossipOption != 0)
                    {
                        Lua.LuaDoString("SelectGossipOption(" + target.SelectGossipOption + ")");
                        Thread.Sleep(250 + Usefuls.Latency);
                    }
                    else if (target.Type == Npc.NpcType.Vendor)
                    {
                        Gossip.SelectGossip(Gossip.GossipOption.Vendor);
                    }
                    int amount = questObjective.CollectCount - ItemsManager.GetItemCount(questObjective.CollectItemId);
                    if (amount <= 0)
                    {
                        nManagerSetting.CurrentSetting.DontSellTheseItems.Add(ItemsManager.GetItemNameById(questObjective.CollectItemId));
                        questObjective.IsObjectiveCompleted = true;
                    }
                    Vendor.BuyItem(ItemsManager.GetItemNameById(questObjective.CollectItemId), amount);
                    Thread.Sleep(questObjective.WaitMs == 0 ? 2000 + Usefuls.Latency : questObjective.WaitMs);
                    if (ItemsManager.GetItemCount(questObjective.CollectItemId) >= questObjective.CollectCount)
                    {
                        nManagerSetting.CurrentSetting.DontSellTheseItems.Add(ItemsManager.GetItemNameById(questObjective.CollectItemId));
                        questObjective.IsObjectiveCompleted = true;
                    }
                }
                else
                {
                    UpdateEntryListRow(questObjective);
                    if (EntryListRow == -1)
                    {
                        questObjective.Entry = new List<int>();
                        CheckMandatoryFieldsByType(questObjective, true);
                    }
                    return; // target not found
                }
            }

            // USE RUNEFORGE
            if (questObjective.Objective == Objective.UseRuneForge)
            {
                CheckMandatoryFieldsByType(questObjective, false, true);
                if (!MovementManager.InMovement ||
                    questObjective.Position.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
                {
                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        MountTask.DismountMount();
                        Spell runeforging = new Spell("Runeforging");
                        Lua.RunMacroText("/cast " + runeforging.NameInGame);
                        Thread.Sleep(250 + Usefuls.Latency);
                        Lua.RunMacroText("/script DoTradeSkill(GetTradeSkillSelectionIndex())");
                        Lua.LuaDoString("DoTradeSkill(GetTradeSkillSelectionIndex())"); // bug
                        Thread.Sleep(250 + Usefuls.Latency);
                        // selectionne le premier dans la liste, donc OK
                        Lua.RunMacroText("/click CharacterMainHandSlot");
                        Thread.Sleep(250 + Usefuls.Latency);
                        Lua.LuaDoString("ReplaceEnchant()");
                        Thread.Sleep(questObjective.WaitMs);
                        Lua.LuaDoString("CloseTradeSkill()");
                        questObjective.IsObjectiveCompleted = true;
                    }
                }
            }

            // APPLY BUFF
            if (questObjective.Objective == Objective.ApplyBuff)
            {
                List<WoWUnit> allUnits = ObjectManager.GetWoWUnitByEntry(questObjective.Entry, questObjective.IsDead);
                List<WoWUnit> allProperUnits = new List<WoWUnit>();
                foreach (WoWUnit unit in allUnits)
                {
                    if (!unit.HaveBuff((uint) questObjective.BuffId))
                        allProperUnits.Add(unit);
                }
                WoWUnit wowUnit = ObjectManager.GetNearestWoWUnit(allProperUnits);

                if (wowUnit.IsValid && !MovementManager.InMovement)
                {
                    if (wowUnit.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(wowUnit.Position));
                    }
                    else
                    {
                        if (questObjective.IgnoreFight)
                            Quest.GetSetIgnoreFight = true;
                        MountTask.DismountMount();
                        if (ItemsManager.GetItemCount(questObjective.UseItemId) <= 0 || ItemsManager.IsItemOnCooldown(questObjective.UseItemId) || !ItemsManager.IsItemUsable(questObjective.UseItemId))
                            return;
                        Logging.WriteDebug("Buffing " + wowUnit.Name + "(" + wowUnit.GetBaseAddress + ")");
                        ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
                        Thread.Sleep(questObjective.WaitMs);
                        questObjective.CurrentCount++; // This is not correct
                        Quest.GetSetIgnoreFight = false;
                    }
                }
                else if (!MovementManager.InMovement && questObjective.PathHotspots.Count > 0)
                {
                    // Mounting Mount
                    MountTask.Mount();
                    // Need GoTo Zone:
                    if (
                        questObjective.PathHotspots[
                            Math.NearestPointOfListPoints(questObjective.PathHotspots,
                                ObjectManager.Me.Position)].DistanceTo(
                                    ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(
                            PathFinder.FindPath(
                                questObjective.PathHotspots[
                                    Math.NearestPointOfListPoints(questObjective.PathHotspots,
                                        ObjectManager.Me.Position)]));
                    }
                    else
                    {
                        // Start Move
                        MovementManager.GoLoop(questObjective.PathHotspots);
                    }
                }
            }

            // USE TAXI
            if (questObjective.Objective == Objective.UseFlightPath)
            {
                Npc taxiMan = Bot.Bot.FindQuesterById(questObjective.TaxiEntry);

                uint baseAddress = MovementManager.FindTarget(ref taxiMan);
                if (MovementManager.InMovement)
                    return;

                if (baseAddress > 0)
                {
                    Interact.InteractWith(baseAddress);
                    Thread.Sleep(250 + Usefuls.Latency);
                    if (!Gossip.IsTaxiWindowOpen())
                    {
                        Gossip.SelectGossip(Gossip.GossipOption.Taxi);
                    }
                    if (!Gossip.IsTaxiWindowOpen())
                    {
                        Logging.Write("There is a problem with taxi master " + taxiMan.Name);
                        return;
                    }
                    Gossip.TakeTaxi(questObjective.FlightDestinationX, questObjective.FlightDestinationY);
                    Thread.Sleep(questObjective.WaitMs);
                    questObjective.IsObjectiveCompleted = true;
                }
                // target not found
            }

            if (questObjective.Objective == Objective.TravelTo)
            {
                Products.TravelTo = questObjective.Position;
                Products.TravelToContinentId = questObjective.ContinentId;
                if (ObjectManager.Me.Position.DistanceTo(questObjective.Position) > 100)
                {
                    return;
                }
                questObjective.IsObjectiveCompleted = true;
            }
        }

        public static void PickUpQuest()
        {
            QuestStatus = "Pick-Up Quest";
            Npc npc = Bot.Bot.FindQuesterById(CurrentQuest.PickUp);
            int item = CurrentQuest.ItemPickUp;
            if (item != 0)
            {
                ItemsManager.UseItem(ItemsManager.GetItemNameById(item));
                Thread.Sleep(250 + Usefuls.Latency);
                Quest.AcceptQuest();
                return;
            }
            if (npc == null)
                return;
            Quest.QuestPickUp(ref npc, CurrentQuest.Name, CurrentQuest.Id);
        }

        public static void TurnInQuest()
        {
            if (CurrentQuest.Objectives.Count > 0 && !Quest.GetLogQuestIsComplete(CurrentQuest.Id))
            {
                ResetQuestObjective();
                return;
            }
            QuestStatus = "Turn-In Quest";
            Npc npc = Bot.Bot.FindQuesterById(CurrentQuest.TurnIn);
            if (npc == null)
                return;
            Quest.QuestTurnIn(ref npc, CurrentQuest.Name, CurrentQuest.Id);
            if (CurrentQuest.AutoComplete != null && QuestingTask.CurrentQuest.AutoComplete.Count > 0)
            {
                EventsListener.UnHookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_AUTOCOMPLETE, callback => Quest.AutoCompleteQuest(CurrentQuest.AutoComplete), false);
                Quest.AutoCompleteQuest(CurrentQuest.AutoComplete); // make sure to recall it as long as the quest is not yet completed in case it failed the first time.
            }
        }

        // end PickUpTurnInQuest

        #region EntryListRow Management

        private static void UpdateEntryListRow(QuestObjective questObjective)
        {
            if (questObjective.Entry.Count >= EntryListRow)
            {
                EntryListRow++;
            }
            else
            {
                EntryListRow = -1;
            }
        }

        private static void ResetEntryListRow()
        {
            EntryListRow = 0;
        }

        private static int GetEntryListRow(QuestObjective questObjective)
        {
            if (questObjective.Entry.Count == 1)
                return questObjective.Entry[0];
            if (EntryListRow == -1)
            {
                ResetEntryListRow();
                return 0;
            }
            if (questObjective.Entry.Count >= EntryListRow + 1)
            {
                return questObjective.Entry[EntryListRow];
            }
            ResetEntryListRow();
            return questObjective.Entry[0];
        }

        #endregion

        #region Debug for Questing developers

        private static void CheckMandatoryFieldsByObjective(QuestObjective questObjective)
        {
            return;
            /* Todo make this functions till the end.
            switch (questObjective.Objective)
            {
                case Objective.None:
                case Objective.ApplyBuff:
                case Objective.BuyItem:
                case Objective.EjectVehicle:
                case Objective.EquipItem:
                case Objective.InteractWith:
                case Objective.KillMob:
                case Objective.MoveTo:
                case Objective.PickUpObject:
                case Objective.PickUpQuest:
                case Objective.PressKey:
                case Objective.UseItem:
                case Objective.TurnInQuest:
                case Objective.UseFlightPath:
                case Objective.UseItemAOE:
                case Objective.UseRuneForge:
                case Objective.UseSpell:
                case Objective.UseSpellAOE:
                case Objective.UseVehicle:
                case Objective.Wait:
                    break;
            }*/
        }

        private static void CheckMandatoryFieldsByType(QuestObjective questObjective, bool cEntry, bool cPosition = false, bool cCollectItemId = false, bool cCountItemId = false)
        {
            // Call this function from any objective you want to check. Setting bool true to any of the parameter means this parameter is Mandatory in the objective.
            // Todo: Add support for all type of fields that can be mandatory for an objective.
            uint errors = 0;
            if (cEntry && questObjective.Entry.Count <= 0)
            {
                Logging.WriteError("The Entry (List<int>) of your " + questObjective.Objective + " objective is missing or invalid.");
                errors++;
            }
            if (cPosition && questObjective.Position.Z == 0f)
            {
                Logging.WriteError("The Position(Point(X, Y, Z)) of your " + questObjective.Objective + " objective is missing or invalid. Can't continue.");
                errors++;
            }
            if (cCollectItemId && questObjective.CollectItemId <= 0)
            {
                Logging.WriteError("The CollectItemId(int) of your " + questObjective.Objective + " objective is missing or invalid. Can't continue.");
                errors++;
            }
            if (cCountItemId && questObjective.CollectCount != -1 && questObjective.CollectCount <= 0)
            {
                Logging.WriteError("The CollectCount(int) of your " + questObjective.Objective + " objective is missing or invalid. Should be set to '-1' or '> 0'. Can't continue.");
                errors++;
            }

            if (errors == 0)
                return;
            if (QuesterSettings.CurrentSettings.ActivateDevMode)
            {
                Bot.Bot.Dispose();
                return;
            }
            //Logging.WriteError("This quest is now blacklisted. You may report the issue to the profile's developper.");
            //questObjective.DoIgnoreCurrentQuest();
        }

        #endregion
    }
}

// some maybe useful stuffs
// GetNumQuestPOIWorldEffects May we find hotspots with this?