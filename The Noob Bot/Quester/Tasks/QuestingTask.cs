using System.Collections.Generic;
using System.Threading;
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

// ReSharper disable ConvertToConstant.Local
        // todo: Add thoses to Product Settings of the Quester.
        private static bool _HARDMODE_ = false; // if (true) { Ignore QuestLevel }
        private static bool _DEVMODE_ = true; // On error: if (true) { Dispose Quester } else { doIgnoreCurrentQuest + continue }
// ReSharper restore ConvertToConstant.Local

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
                    if (Quest.GetLogQuestId().Contains(quest.Id))
                    {
                        CurrentQuest = quest;
                        Logging.Write("resuming \"" + quest.Name + "\": Lvl " + quest.QuestLevel + " (" + quest.MinLevel + " - " + quest.MaxLevel + ")");
                        return;
                    }
                }
                foreach (Profile.Quest quest in Bot.Bot.Profile.Quests)
                {
                    if (ObjectManager.Me.Level >= quest.MinLevel && ObjectManager.Me.Level <= quest.MaxLevel &&
                        (_HARDMODE_ || ObjectManager.Me.Level >= quest.QuestLevel - relax)) // Level
                        if (!Quest.GetQuestCompleted(quest.Id)) // Quest not completed
                            if (!Quest.GetQuestCompleted(quest.NeedQuestNotCompletedId)) // Quest done which discalify this one
                                if (Quest.GetQuestCompleted(quest.NeedQuestCompletedId) || // Quest need completed
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
                if (objective.IgnoreQuestCompleted && !QuestObjectiveIsFinish(ref objective))
                    return false;
            }
            return true;
        }

        public static bool QuestObjectiveIsFinish(ref QuestObjective questObjective)
        {
            if (questObjective == null)
                return true;

            // shortcut since we do objective one by one, for kill it can be completed before we do them all
            if (!questObjective.IgnoreQuestCompleted && Quest.GetLogQuestIsComplete(CurrentQuest.Id))
                return true;

            if (questObjective.ScriptConditionIsComplete != string.Empty)
                return Script.Run(questObjective.ScriptConditionIsComplete);

            CheckMandatoryFieldsByObjective(questObjective);

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

            // PICK UP
            if (questObjective.Objective == Objective.PickUpObject)
            {
                if (questObjective.CurrentCount >= questObjective.Count)
                    return true;
                return false;
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

            /* MOVE TO || WAIT || INTERACT WITH ||
             * USE SPELL || EQUIP ITEM || PICK UP QUEST ||
             * TURN IN QUEST || PRESS KEY || USE ITEM AOE ||
             * USE SPELL AOE || USE RUNEFORGE || USE FLIGHT PATH
             */
            if (questObjective.Objective == Objective.MoveTo || questObjective.Objective == Objective.Wait || questObjective.Objective == Objective.InteractWith ||
                questObjective.Objective == Objective.UseSpell || questObjective.Objective == Objective.EquipItem || questObjective.Objective == Objective.PickUpQuest ||
                questObjective.Objective == Objective.TurnInQuest || questObjective.Objective == Objective.PressKey || questObjective.Objective == Objective.UseItemAOE ||
                questObjective.Objective == Objective.UseSpellAOE || questObjective.Objective == Objective.UseRuneForge || questObjective.Objective == Objective.UseFlightPath)
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
            foreach (Npc npc in Bot.Bot.Profile.AvoidMobs)
            {
                if ((npc.Position == new Point() || npc.Position.DistanceTo(woWUnit.Position) <= 40) && npc.Entry == woWUnit.Entry)
                    return true;
            }

            return false;
        }

        public static void QuestObjectiveExecute(ref QuestObjective questObjective)
        {
            if (questObjective == null)
                return;

            QuestStatus = questObjective.Objective.ToString();

            // Create Path:
            if (questObjective.PathHotspots == null)
            {
                if (questObjective.Hotspots.Count > 0)
                {
                    questObjective.PathHotspots = new List<Point>();
                    for (int i = 0; i <= questObjective.Hotspots.Count - 1; i++)
                    {
                        int iLast = i - 1;
                        if (iLast < 0)
                            iLast = questObjective.Hotspots.Count - 1;
                        Logging.Write( /*Translate.Get(...)*/ "Create_points_HotSpot " + iLast + " to_HotSpot " + i);
                        List<Point> points = PathFinder.FindPath(questObjective.Hotspots[iLast],
                                                                 questObjective.Hotspots[i]);
                        questObjective.PathHotspots.AddRange(points);
                    }
                }
                else
                {
                    questObjective.PathHotspots = new List<Point>
                        {
                            ObjectManager.Me.Position,
                            ObjectManager.Me.Position
                        };
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

                WoWUnit wowUnit =
                    ObjectManager.GetNearestWoWUnit(
                        ObjectManager.GetWoWUnitByEntry(questObjective.Entry));

                if (!wowUnit.IsValid && questObjective.Factions.Count > 0)
                    wowUnit =
                        ObjectManager.GetNearestWoWUnit(
                            ObjectManager.GetWoWUnitByFaction(questObjective.Factions));

                if (!IsInAvoidMobsList(wowUnit) && !nManagerSetting.IsBlackListedZone(wowUnit.Position) &&
                    !nManagerSetting.IsBlackListed(wowUnit.Guid) && wowUnit.IsAlive && wowUnit.IsValid &&
                    (questObjective.CanPullUnitsAlreadyInFight || !wowUnit.InCombat))
                {
                    MovementManager.FindTarget(wowUnit, wowUnit.AggroDistance*1.1f);
                    if (MovementManager.InMovement)
                        return;
                    Logging.Write("Attacking Lvl " + wowUnit.Level + " " + wowUnit.Name);
                    ulong Unkillable = Fight.StartFight(wowUnit.Guid);
                    if (!wowUnit.IsDead && Unkillable != 0)
                    {
                        nManagerSetting.AddBlackList(Unkillable, 3*60*1000);
                    }
                    else if (wowUnit.IsDead)
                    {
                        Statistics.Kills++;
                        if (!wowUnit.IsTapped || (wowUnit.IsTapped && wowUnit.IsTappedByMe))
                        {
                            questObjective.CurrentCount++;
                        }
                        Thread.Sleep(1000 + Usefuls.Latency);
                        while (!ObjectManager.Me.IsMounted && ObjectManager.Me.InCombat &&
                               ObjectManager.GetUnitAttackPlayer().Count <= 0)
                        {
                            Thread.Sleep(100);
                        }
                        Fight.StopFight();
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

            // PICK UP OBJECT
            if (questObjective.Objective == Objective.PickUpObject)
            {
                WoWGameObject node =
                    ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));

                if (!nManagerSetting.IsBlackListedZone(node.Position) &&
                    !nManagerSetting.IsBlackListed(node.Guid) && node.IsValid)
                {
                    uint tNumber = Statistics.Farms;
                    FarmingTask.Pulse(new List<WoWGameObject> {node});
                    if (Statistics.Farms > tNumber)
                        questObjective.CurrentCount++;
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

            // USE ITEM
            if (questObjective.Objective == Objective.UseItem)
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
                                ObjectManager.GetWoWUnitByEntry(questObjective.Entry));
                        if (node.IsValid)
                        {
                            questObjective.Position = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.Position = new Point(unit.Position);
                        }
                    }

                    if (questObjective.Position != new Point() && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        MountTask.DismountMount();
                        MovementManager.StopMove();
                        if (questObjective.Entry.Count > 0)
                        {
                            WoWGameObject node =
                                ObjectManager.GetNearestWoWGameObject(
                                    ObjectManager.GetWoWGameObjectById(questObjective.Entry));
                            WoWUnit unit =
                                ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.GetWoWUnitByEntry(questObjective.Entry));
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
                        ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
                        if (questObjective.Count > 0)
                            questObjective.CurrentCount++;
                        else
                            questObjective.IsObjectiveCompleted = true;
                        Thread.Sleep(questObjective.WaitMs);
                    }
                }
            }

            // MOVE TO
            if (questObjective.Objective == Objective.MoveTo)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.Position != new Point() && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                        questObjective.IsObjectiveCompleted = true;
                }
            }

            // WAIT
            if (questObjective.Objective == Objective.Wait)
            {
                if (waitTimer == null)
                    waitTimer = new Timer(questObjective.WaitMs);
                if (waitTimer.IsReady)
                {
                    questObjective.IsObjectiveCompleted = true;
                    waitTimer = null;
                }
            }

            // INTERACT WITH
            if (questObjective.Objective == Objective.InteractWith)
            {
                Thread.Sleep(500);
                if (!MovementManager.InMovement)
                {
                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) < questObjective.Range)
                    {
                        WoWGameObject node =
                            ObjectManager.GetNearestWoWGameObject(
                                ObjectManager.GetWoWGameObjectById(new List<int> {questObjective.EntryInteractWith}));
                        WoWUnit unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(new List<int> {questObjective.EntryInteractWith}));
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
                            return;
                        }

                        MovementManager.Go(PathFinder.FindPath(pos));
                        Thread.Sleep(1000);
                        while (MovementManager.InMovement && pos.DistanceTo(ObjectManager.Me.Position) > 3.9f)
                        {
                            if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
                                return;
                            Thread.Sleep(100);
                        }
                        MountTask.DismountMount();
                        MovementManager.StopMove();
                        Interact.InteractWith(baseAddress);

                        if (questObjective.GossipOptionsInteractWith != -1)
                        {
                            Thread.Sleep(Usefuls.Latency + 500);
                            Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
                        }
                        questObjective.IsObjectiveCompleted = true;
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
                                ObjectManager.GetWoWUnitByEntry(questObjective.Entry));
                        if (node.IsValid)
                        {
                            questObjective.Position = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.Position = new Point(unit.Position);
                        }
                    }

                    if (questObjective.Position != new Point() && questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
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
                                    ObjectManager.GetWoWUnitByEntry(questObjective.Entry));
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
                        Spell t = new Spell((uint) questObjective.UseSpellId);
                        for (int i = 0; i < questObjective.Count; i++)
                        {
                            while (!t.IsSpellUsable)
                                Thread.Sleep(50);
                            t.Launch();
                            Thread.Sleep(questObjective.WaitMs);
                        }
                        questObjective.IsObjectiveCompleted = true;
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
                PickUpQuest();
                questObjective.IsObjectiveCompleted = true;
            }

            // TURN IN QUEST
            if (questObjective.Objective == Objective.TurnInQuest)
            {
                TurnInQuest();
                questObjective.IsObjectiveCompleted = true;
            }

            // USE VEHICLE
            if (questObjective.Objective == Objective.UseVehicle)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.Position != new Point() &&
                        questObjective.Position.DistanceTo(ObjectManager.Me.Position) > nManagerSetting.CurrentSetting.GatheringSearchRadius)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        WoWUnit unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(new List<int> {questObjective.EntryVehicle}),
                                questObjective.Position);
                        if (!unit.IsValid)
                        {
                            return;
                        }

                        MovementManager.Go(PathFinder.FindPath(unit.Position));
                        Thread.Sleep(1000);
                        while (MovementManager.InMovement && unit.IsValid &&
                               ObjectManager.Me.Position.DistanceTo(unit.Position) > 4)
                        {
                            if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
                                return;
                            Thread.Sleep(100);
                        }
                        MountTask.DismountMount();
                        Interact.InteractWith(unit.GetBaseAddress);
                        Thread.Sleep(Usefuls.Latency + 500);
                    }
                }
            }

            // EJECT VEHICLE
            if (questObjective.Objective == Objective.EjectVehicle)
            {
                Usefuls.EjectVehicle();
                Thread.Sleep(Usefuls.Latency + 500);
            }

            // PRESS KEY
            if (questObjective.Objective == Objective.PressKey)
            {
                if (!MovementManager.InMovement ||
                    ObjectManager.Me.Position.DistanceTo(questObjective.Position) < 5.0f)
                {
                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) > 5.0f && questObjective.Position != new Point())
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        MountTask.DismountMount();
                        Keybindings.DownKeybindings(questObjective.Keys);
                        Thread.Sleep(questObjective.WaitMs);
                        Keybindings.UpKeybindings(questObjective.Keys);
                        questObjective.IsObjectiveCompleted = true;
                    }
                }
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
                                ObjectManager.GetWoWUnitByEntry(questObjective.Entry));
                        if (node.IsValid)
                        {
                            questObjective.Position = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.Position = new Point(unit.Position);
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        MountTask.DismountMount();
                        SpellManager.CastSpellByIDAndPosition((uint) questObjective.UseSpellId,
                                                              questObjective.Position);
                        Thread.Sleep(questObjective.WaitMs);
                        questObjective.IsObjectiveCompleted = true;
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
                            ContinentId = (ContinentId) Usefuls.ContinentId,
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
                    ItemsManager.UseItem(questObjective.UseItemId, questObjective.Position);
                    Thread.Sleep(questObjective.WaitMs);
                    questObjective.IsObjectiveCompleted = true;
                    ResetEntryListRow();
                }
                else if (baseAddress != 0)
                {
                    Interact.InteractWith(baseAddress);
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
                        Logging.Write("UseItemAOE objective: An empty have been provided in the profile, but not valid nor spawned at the position. UseItemAOE by position instead.");
                    }
                    return; // target not found, try next Entry
                }
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
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Faction = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
                        SelectGossipOption = questObjective.GossipOptionsInteractWith
                    };
                uint baseAddress = MovementManager.FindTarget(ref target, questObjective.Range > 5f ? questObjective.Range : 0);
                if (MovementManager.InMovement)
                    return;
                if (baseAddress != 0)
                {
                    Interact.InteractWith(baseAddress);
                    Thread.Sleep(500 + Usefuls.Latency);
                    Vendor.BuyItem(ItemsManager.GetItemNameById(questObjective.CollectItemId), questObjective.CollectCount);
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
                        Thread.Sleep(500);
                        Lua.RunMacroText("/script DoTradeSkill(GetTradeSkillSelectionIndex())");
                        Lua.LuaDoString("DoTradeSkill(GetTradeSkillSelectionIndex())"); // bug
                        Thread.Sleep(500);
                        // selectionne le premier dans la liste, donc OK
                        Lua.RunMacroText("/click CharacterMainHandSlot");
                        Thread.Sleep(500);
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
                List<WoWUnit> allUnits = ObjectManager.GetWoWUnitByEntry(questObjective.Entry);
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
                        MountTask.DismountMount();
                        Logging.WriteDebug("Buffing " + wowUnit.Name + "(" + wowUnit.GetBaseAddress + ")");
                        ItemsManager.UseItem(ItemsManager.GetItemNameById(questObjective.UseItemId));
                        Thread.Sleep(questObjective.WaitMs);
                        questObjective.CurrentCount++; // This is not correct
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
                Npc taxiMan = Bot.Bot.FindQuesterById(questObjective.EntryInteractWith);

                uint baseAddress = MovementManager.FindTarget(ref taxiMan);
                if (MovementManager.InMovement)
                    return;

                if (baseAddress != 0)
                {
                    Interact.InteractWith(baseAddress);
                    Thread.Sleep(500 + Usefuls.Latency);
                    if (!Taxi.IsTaxiWindowOpen())
                    {
                        Taxi.FindAndOpenTaxiGossip();
                        Thread.Sleep(500 + Usefuls.Latency);
                    }
                    if (!Taxi.IsTaxiWindowOpen())
                    {
                        Logging.Write("There is a big problem !!!!!!");
                        return;
                    }
                    Taxi.TakeTaxi(questObjective.FlightDestinationX, questObjective.FlightDestinationY);
                    Thread.Sleep(questObjective.WaitMs);
                    questObjective.IsObjectiveCompleted = true;
                }
                // target not found
            }
        }

        public static void PickUpQuest()
        {
            PickUpTurnInQuest(true, false);
        }

        public static void TurnInQuest()
        {
            if (CurrentQuest.Objectives.Count > 0 && !Quest.GetLogQuestIsComplete(CurrentQuest.Id))
            {
                ResetQuestObjective();
                return;
            }
            PickUpTurnInQuest(false, true);
        }

        private static void PickUpTurnInQuest(bool pickUp, bool turnIn)
        {
            Npc npc = null;
            int item = CurrentQuest.ItemPickUp;

            if (pickUp)
            {
                QuestStatus = "Pick-Up Quest";
                npc = Bot.Bot.FindQuesterById(CurrentQuest.PickUp);
            }
            if (turnIn)
            {
                QuestStatus = "Turn-In Quest";
                npc = Bot.Bot.FindQuesterById(CurrentQuest.TurnIn);
            }

            if (pickUp && item != 0)
            {
                ItemsManager.UseItem(ItemsManager.GetItemNameById(item));
                Thread.Sleep(250);
                Quest.AcceptQuest();
                return;
            }

            if (npc == null)
                return;

            // Go To QuestGiver:
            // Launch script
            //Script.Run(npc.Script); ToDo: Support scripts for special case quests.

            //Start target finding based on QuestGiver.
            uint baseAddress = MovementManager.FindTarget(ref npc);
            if (MovementManager.InMovement)
                return;
            //End target finding based on QuestGiver.

            if (npc.Position.DistanceTo(ObjectManager.Me.Position) < 6)
            {
                Quest.CloseQuestWindow();
                Interact.InteractWith(baseAddress);
                Thread.Sleep(Usefuls.Latency + 600);
                WoWObject targetIsObject = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByEntry(npc.Entry), npc.Position);
                if (targetIsObject.IsValid)
                    Thread.Sleep(2500); // to let the Gameobject open
                if (pickUp)
                {
                    Logging.Write("PickUp Quest " + CurrentQuest.Name + " id: " + CurrentQuest.Id);
                    int id = Quest.GetQuestID();
                    // GetNumGossipActiveQuests() == 1 because of auto accepted quests
                    if (!(Quest.GetNumGossipAvailableQuests() == 1 && Quest.GetNumGossipActiveQuests() == 1) && id == CurrentQuest.Id)
                    {
                        Quest.AcceptQuest();
                        Thread.Sleep(Usefuls.Latency + 500);
                    }
                    if (Quest.GetLogQuestId().Contains(CurrentQuest.Id))
                    {
                        Quest.CloseQuestWindow();
                    }
                    else
                    {
                        if (Quest.GetGossipAvailableQuestsWorks()) // 2 quest gossip systems = 2 different codes :(
                        {
                            for (int i = 1; i <= Quest.GetNumGossipAvailableQuests(); i++)
                            {
                                Quest.SelectGossipAvailableQuest(i);
                                Thread.Sleep(Usefuls.Latency + 500);
                                id = Quest.GetQuestID();
                                if (id == CurrentQuest.Id)
                                {
                                    Quest.AcceptQuest();
                                    Thread.Sleep(Usefuls.Latency + 500);
                                    id = Quest.GetQuestID();
                                    Quest.CloseQuestWindow();
                                    if (id != CurrentQuest.Id)
                                        Quest.AbandonQuest(id);
                                    break;
                                }
                                Quest.CloseQuestWindow();
                                Thread.Sleep(Usefuls.Latency + 500);
                                Quest.AbandonQuest(id);
                                Interact.InteractWith(baseAddress);
                                Thread.Sleep(Usefuls.Latency + 500);
                            }
                        }
                        else
                        {
                            int gossipid = 1;
                            while (Quest.GetAvailableTitle(gossipid) != "")
                            {
                                Quest.SelectAvailableQuest(gossipid);
                                Thread.Sleep(Usefuls.Latency + 500);
                                id = Quest.GetQuestID();
                                if (id == CurrentQuest.Id)
                                {
                                    Quest.AcceptQuest();
                                    Thread.Sleep(Usefuls.Latency + 500);
                                    id = Quest.GetQuestID();
                                    Quest.CloseQuestWindow();
                                    if (id != CurrentQuest.Id)
                                        Quest.AbandonQuest(id);
                                    break;
                                }
                                Quest.CloseQuestWindow();
                                Thread.Sleep(Usefuls.Latency + 500);
                                Quest.AbandonQuest(id);
                                Interact.InteractWith(baseAddress);
                                Thread.Sleep(Usefuls.Latency + 500);
                                gossipid++;
                            }
                        }
                    }
                    Quest.KilledMobsToCount.Clear();
                }
                if (turnIn)
                {
                    Logging.Write("turnIn Quest " + CurrentQuest.Name + " id: " + CurrentQuest.Id);
                    int id = Quest.GetQuestID();
                    if (id == CurrentQuest.Id) // this may fail
                    {
                        Quest.CompleteQuest();
                        Thread.Sleep(Usefuls.Latency + 500);
                    }
                    if (!Quest.GetLogQuestId().Contains(CurrentQuest.Id)) // It's no more in the quest log, then we did turn in it sucessfuly
                    {
                        id = Quest.GetQuestID();
                        Quest.FinishedQuestSet.Add(CurrentQuest.Id);
                        Quest.CloseQuestWindow();
                        Quest.AbandonQuest(id);
                    }
                    else
                    {
                        if (Quest.GetGossipActiveQuestsWorks()) // 2 quest gossip systems = 2 different codes :(
                        {
                            for (int i = 1; i <= Quest.GetNumGossipActiveQuests(); i++)
                            {
                                Quest.SelectGossipActiveQuest(i);
                                Thread.Sleep(Usefuls.Latency + 500);
                                id = Quest.GetQuestID();
                                if (id == CurrentQuest.Id)
                                {
                                    Quest.CompleteQuest();
                                    Thread.Sleep(Usefuls.Latency + 500);
                                    // here it can be the next quest id presented automatically when the current one is turned in
                                    id = Quest.GetQuestID();
                                    Quest.CloseQuestWindow();
                                    Quest.FinishedQuestSet.Add(CurrentQuest.Id);
                                    // If it was auto-accepted, then abandon it. I'll make this better later.
                                    Quest.AbandonQuest(id);
                                    break;
                                }
                                Quest.CloseQuestWindow();
                                Thread.Sleep(Usefuls.Latency + 500);
                                Interact.InteractWith(baseAddress);
                                Thread.Sleep(Usefuls.Latency + 500);
                            }
                        }
                        else
                        {
                            int gossipid = 1;
                            while (Quest.GetActiveTitle(gossipid) != "")
                            {
                                Quest.SelectActiveQuest(gossipid);
                                Thread.Sleep(Usefuls.Latency + 500);
                                id = Quest.GetQuestID();
                                if (id == CurrentQuest.Id)
                                {
                                    Quest.CompleteQuest();
                                    Thread.Sleep(Usefuls.Latency + 500);
                                    Quest.CloseQuestWindow();
                                    Quest.FinishedQuestSet.Add(CurrentQuest.Id);
                                    break;
                                }
                                Quest.CloseQuestWindow();
                                Thread.Sleep(Usefuls.Latency + 500);
                                Interact.InteractWith(baseAddress);
                                Thread.Sleep(Usefuls.Latency + 500);
                                gossipid++;
                            }
                        }
                    }
                }
                Thread.Sleep(Usefuls.Latency);
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
            // Todo make this functions till the end.
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
            }
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
            if (cPosition && questObjective.Position == new Point())
            {
                Logging.WriteError("The Position(Point(X, Y, Z)) of your " + questObjective.Objective + " objective is missing or invalid. Can't continue.");
                errors++;
            }
            if (cCollectItemId && questObjective.CollectItemId <= 0)
            {
                Logging.WriteError("The CollectItemId(int) of your " + questObjective.Objective + " objective is missing or invalid. Can't continue.");
                errors++;
            }
            if (cCountItemId && questObjective.CollectCount != -1 || questObjective.CollectCount <= 0)
            {
                Logging.WriteError("The CollectCount(int) of your " + questObjective.Objective + " objective is missing or invalid. Should be set to '-1' or '> 0'. Can't continue.");
                errors++;
            }

            if (errors == 0)
                return;
            if (_DEVMODE_)
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