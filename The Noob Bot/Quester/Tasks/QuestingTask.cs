using System;
using System.Collections.Generic;
using System.Threading;
using Quester.Bot;
using Quester.Profile;
using nManager;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.Class;
using nManager.Wow.ObjectManager;
using Quest = nManager.Wow.Helpers.Quest;
using Timer = nManager.Helpful.Timer;
using nManager.Wow.Bot.Tasks;

namespace Quester.Tasks
{
    class QuestingTask
    {
        private static string QuestStatus = "";
        public static Profile.Quest CurrentQuest = new Profile.Quest();
        private static int _currentQuestObjectiveId = -1;
        public static Profile.QuestObjective CurrentQuestObjective;

        public static void SelectQuest()
        {
            QuestStatus = "Select Quest";
            CurrentQuest = new Profile.Quest();
            _currentQuestObjectiveId = -1;
            CurrentQuestObjective = null;
            Quester.Bot.Bot.Profile.Quests.Sort(delegate(Profile.Quest q1, Profile.Quest q2)
                {
                    return q1.PickUp.Position.DistanceTo(ObjectManager.Me.Position).CompareTo(q2.PickUp.Position.DistanceTo(ObjectManager.Me.Position));
                }
            );

            foreach (var quest in Quester.Bot.Bot.Profile.Quests)
            {
                if (ObjectManager.Me.Level >= quest.MinLevel && // Level
                    ObjectManager.Me.Level <= quest.MaxLevel)
                    if (!Quest.GetQuestCompleted(quest.Id)) // Quest not completed
                        if (Quest.GetQuestCompleted(quest.NeedQuestCompletedId) || // Quest need completed
                            quest.NeedQuestCompletedId.Count == 0)
                            if (Script.Run(quest.ScriptCondition)) // Condition
                            {
                                CurrentQuest = quest;
                                Logging.Write(quest.Name + ": Lvl " + quest.MinLevel + " - " + quest.MaxLevel);
                                break;
                            }
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
                    if (Script.Run(CurrentQuest.Objectives[_currentQuestObjectiveId].ScriptCondition)) // Script condition
                    {
                        CurrentQuestObjective =
                            CurrentQuest.Objectives[_currentQuestObjectiveId];
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
            SelectNextQuestObjective();
        }

        public static bool QuestObjectiveIsFinish(ref QuestObjective questObjective)
        {
            if (questObjective == null)
                return true;

            if (questObjective.ScriptConditionIsComplete != string.Empty)
                return Script.Run(questObjective.ScriptConditionIsComplete);

            // COLLECT ITEM)
            if (questObjective.CollectItemId > 0 && questObjective.CollectCount > 0)
                if (ItemsManager.GetItemCountByIdLUA((uint)questObjective.CollectItemId) < questObjective.CollectCount)
                    return false;

            // KILL MOB
            if (questObjective.Objective == Objective.KillMob)
            {
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
                return questObjective.IsUsedUseItem;
            }

            // MOVE TO
            if (questObjective.Objective == Objective.MoveTo)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.MoveTo.DistanceTo(ObjectManager.Me.Position) <= 4)
                    {
                        return true;
                    }
                }
                return false;
            }

            // WAIT
            if (questObjective.Objective == Objective.Wait)
            {
                return questObjective.IsUsedWaitMs;
            }

            // TRAIN ALL SPELLS
            if (questObjective.Objective == Objective.TrainSpells)
            {
                return questObjective.IsUsedTrainSpells;
            }

            // INTERACT WITH
            if (questObjective.Objective == Objective.InteractWith)
            {
                return questObjective.IsUsedInteractWith;
            }

            // USE SPELL
            if (questObjective.Objective == Objective.UseSpell)
            {
                return questObjective.IsUsedUseSpell;
            }

            // EQUIP ITEM
            if (questObjective.Objective == Objective.EquipItem)
            {
                return questObjective.IsUsedEquipItem;
            }

            // PICK UP QUEST
            if (questObjective.Objective == Objective.PickUpQuest)
            {
                return questObjective.IsUsedPickUpQuest;
            }

            // TURN IN QUEST
            if (questObjective.Objective == Objective.TurnInQuest)
            {
                return questObjective.IsUsedTurnInQuest;
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

            // PRESS KEY
            if (questObjective.Objective == Objective.PressKey)
            {
                return questObjective.IsUsedPressKey;
            }

            // USE SPELL AOE
            if (questObjective.Objective == Objective.UseSpellAOE)
            {
                return questObjective.IsUsedUseSpellAOE;
            }

            // USE ITEM AOE
            if (questObjective.Objective == Objective.UseItemAOE)
            {
                return questObjective.IsUsedUseItemAOE;
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
            foreach (var npc in Quester.Bot.Bot.Profile.AvoidMobs)
            {
                if ((npc.Position.X == 0.0f || npc.Position.DistanceTo(woWUnit.Position) <= 40) && npc.Entry == woWUnit.Entry)
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
                    for (var i = 0; i <= questObjective.Hotspots.Count - 1; i++)
                    {
                        int iLast = i - 1;
                        if (iLast < 0)
                            iLast = questObjective.Hotspots.Count - 1;
                        Logging.Write(/*Translate.Get(...)*/ "Create_points_HotSpot " + iLast + " to_HotSpot " + i);
                        List<Point> points = PathFinder.FindPath(questObjective.Hotspots[iLast], questObjective.Hotspots[i]);
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
                WoWUnit wowUnit =
                    ObjectManager.GetNearestWoWUnit(
                        ObjectManager.GetWoWUnitByEntry(questObjective.Entry));

                if (!wowUnit.IsValid && questObjective.Factions.Count > 0)
                    wowUnit =
                    ObjectManager.GetNearestWoWUnit(
                        ObjectManager.GetWoWUnitByFaction(questObjective.Factions));

                if (!IsInAvoidMobsList(wowUnit) && !nManagerSetting.IsBlackListedZone(wowUnit.Position) && wowUnit.GetDistance <= nManager.nManagerSetting.CurrentSetting.searchRadius && CurrentQuestObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(CurrentQuestObjective.PathHotspots, wowUnit.Position)].DistanceTo(wowUnit.Position) <= nManagerSetting.CurrentSetting.searchRadius && !nManagerSetting.IsBlackListed(wowUnit.Guid) && wowUnit.IsAlive && wowUnit.IsValid && (nManagerSetting.CurrentSetting.canAttackUnitsAlreadyInFight || !wowUnit.InCombat))
                {
                    Logging.Write("Attacking Lvl " + wowUnit.Name);
                    Fight.StartFight(wowUnit.Guid);
                    if (wowUnit.IsDead)
                    {
                        Statistics.Kills++;
                        questObjective.CurrentCount++;
                        Thread.Sleep(Usefuls.Latency + 1000);
                        while (!ObjectManager.Me.IsMounted && ObjectManager.Me.InCombat && ObjectManager.GetUnitAttackPlayer().Count <= 0)
                        {
                            Thread.Sleep(10);
                        }
                        Fight.StopFight();
                    }
                }
                else if (!MovementManager.InMovement && questObjective.PathHotspots.Count > 0)
                {
                    // Mounting Mount
                    MountTask.Mount();
                    // Need GoTo Zone:
                    if (questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
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
                var node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));

                if (!nManagerSetting.IsBlackListedZone(node.Position) && node.GetDistance < nManagerSetting.CurrentSetting.searchRadius && CurrentQuestObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(CurrentQuestObjective.PathHotspots, node.Position)].DistanceTo(node.Position) <= nManagerSetting.CurrentSetting.searchRadius && !nManagerSetting.IsBlackListed(node.Guid) && node.IsValid)
                {
                    uint tNumber = Statistics.Farms;
                    FarmingTask.Pulse(new List<WoWGameObject> { node });
                    if (Statistics.Farms > tNumber)
                        questObjective.CurrentCount++;
                }
                else if (!MovementManager.InMovement && questObjective.PathHotspots.Count > 0)
                {
                    // Mounting Mount
                    MountTask.Mount();
                    // Need GoTo Zone:
                    if (questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
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
                if (!MovementManager.InMovement || ObjectManager.Me.Position.DistanceTo(questObjective.PositionUseItem) < 3.5f)
                {
                    if (questObjective.EntryAOE > 0)
                    {
                        var node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(new List<int>() { questObjective.EntryAOE }));
                        var unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(new List<int>() { questObjective.EntryAOE }));
                        Point pos = new Point();
                        if (node.IsValid)
                        {
                            questObjective.PositionUseItem = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.PositionUseItem = new Point(unit.Position);
                            Interact.InteractGameObject(unit.GetBaseAddress);
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (questObjective.PositionUseItem.DistanceTo(ObjectManager.Me.Position) > 3.5f &&
                        questObjective.PositionUseItem.X != 0)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.PositionUseItem));
                    }
                    else
                    {
                        MountTask.DismountMount(true);
                        MovementManager.StopMove();
                        ItemsManager.UseItem(ItemsManager.GetNameById((uint)questObjective.UseItemId));
                        questObjective.IsUsedUseItem = true;
                        Thread.Sleep(questObjective.WaitMsUseItem);
                    }
                }
            }

            // MOVE TO
            if (questObjective.Objective == Objective.MoveTo)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.MoveTo.DistanceTo(ObjectManager.Me.Position) > 3.5f &&
                        questObjective.MoveTo.X != 0)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.MoveTo));
                    }
                }
            }

            // WAIT
            if (questObjective.Objective == Objective.Wait)
            {
                Thread.Sleep(questObjective.WaitMs);
                questObjective.IsUsedWaitMs = true;
            }

            // TRAIN ALL SPELLS
            if (questObjective.Objective == Objective.TrainSpells)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.PositionInteractWith.DistanceTo(ObjectManager.Me.Position) > nManagerSetting.CurrentSetting.searchRadius &&
                        questObjective.PositionInteractWith.X != 0)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.PositionInteractWith));
                    }
                    else
                    {
                        var unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(new List<int>() { questObjective.EntryInteractWith }));
                        Point pos = new Point();
                        uint baseAddress;
                        if (unit.IsValid)
                        {
                            pos = new Point(unit.Position);
                            baseAddress = unit.GetBaseAddress;
                        }
                        else
                        {
                            return;
                        }

                        Thread.Sleep(500);
                        MovementManager.Go(PathFinder.FindPath(pos));
                        Thread.Sleep(1000);
                        while (MovementManager.InMovement && pos.DistanceTo(ObjectManager.Me.Position) > 3.9f)
                        {
                            if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
                                return;
                            Thread.Sleep(100);
                        }
                        MountTask.DismountMount(true);
                        Interact.InteractGameObject(baseAddress);

                        Trainer.TrainingSpell();
                        SpellManager.UpdateSpellBook();
                        questObjective.IsUsedTrainSpells = true;
                    }
                }
            }

            // INTERACT WITH
            if (questObjective.Objective == Objective.InteractWith)
            {
                Thread.Sleep(500);
                if (!MovementManager.InMovement)
                {
                    if (questObjective.PositionInteractWith.DistanceTo(ObjectManager.Me.Position) > nManagerSetting.CurrentSetting.searchRadius &&
                        questObjective.PositionInteractWith.X != 0)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.PositionInteractWith));
                    }
                    else
                    {
                        var node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(new List<int>() { questObjective.EntryInteractWith }));
                        var unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(new List<int>() { questObjective.EntryInteractWith }));
                        Point pos = new Point();
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
                        MountTask.DismountMount(true);
                        MovementManager.StopMove();
                        Interact.InteractGameObject(baseAddress);

                        if (questObjective.GossipOptionsInteractWith != -1)
                        {
                            Thread.Sleep(Usefuls.Latency + 500);
                            Quest.SelectGossipOption(questObjective.GossipOptionsInteractWith);
                        }
                        questObjective.IsUsedInteractWith = true;
                    }
                }
            }

            // USE SPELL
            if (questObjective.Objective == Objective.UseSpell)
            {
                if (!MovementManager.InMovement || ObjectManager.Me.Position.DistanceTo(questObjective.PositionUseSpell) < 3.5f)
                {
                    if (questObjective.EntryAOE > 0)
                    {
                        var node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(new List<int>() { questObjective.EntryAOE }));
                        var unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(new List<int>() { questObjective.EntryAOE }));
                        Point pos = new Point();
                        if (node.IsValid)
                        {
                            questObjective.PositionUseSpell = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            Interact.InteractGameObject(unit.GetBaseAddress);
                            questObjective.PositionUseSpell = new Point(unit.Position);
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (questObjective.PositionUseSpell.X != 0 &&
                        questObjective.PositionUseSpell.DistanceTo(ObjectManager.Me.Position) > 3.5f)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.PositionUseSpell));
                    }
                    else
                    {
                        MountTask.DismountMount(true);
                        Spell t = new Spell((uint)questObjective.UseSpellId);
                        for (int i = 0; i < questObjective.Count; i++)
                        {
                            t.Launch();
                            Thread.Sleep(questObjective.WaitMsUseSpell);
                        }
                        questObjective.IsUsedUseSpell = true;
                    }
                }
                else if (!MovementManager.InMovement && questObjective.PathHotspots.Count > 0)
                {
                    // Mounting Mount
                    MountTask.Mount(); // not yet
                    // Need GoTo Zone:
                    if (questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(PathFinder.FindPath(questObjective.PathHotspots[nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots, ObjectManager.Me.Position)]));
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
                ItemsManager.EquipItemByName(ItemsManager.GetNameById((uint)questObjective.EquipItemId));
                questObjective.IsUsedEquipItem = true;
            }

            // PICK UP QUEST
            if (questObjective.Objective == Objective.PickUpQuest)
            {
                PickUpQuest();
                questObjective.IsUsedPickUpQuest = true;
            }

            // TURN IN QUEST
            if (questObjective.Objective == Objective.TurnInQuest)
            {
                TurnInQuest();
                questObjective.IsUsedTurnInQuest = true;
            }

            // USE VEHICLE
            if (questObjective.Objective == Objective.UseVehicle)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.PositionVehicle.DistanceTo(ObjectManager.Me.Position) > nManagerSetting.CurrentSetting.searchRadius &&
                        questObjective.PositionVehicle.X != 0)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.PositionVehicle));
                    }
                    else
                    {

                        var unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(new List<int>() { questObjective.EntryVehicle }), questObjective.PositionVehicle);
                        if (!unit.IsValid)
                        {
                            return;
                        }

                        MovementManager.Go(PathFinder.FindPath(unit.Position));
                        Thread.Sleep(1000);
                        while (MovementManager.InMovement && unit.IsValid && ObjectManager.Me.Position.DistanceTo(unit.Position) > 4)
                        {
                            if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted))
                                return;
                            Thread.Sleep(100);
                        }
                        MountTask.DismountMount(true);
                        Interact.InteractGameObject(unit.GetBaseAddress);
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
                if (!MovementManager.InMovement || ObjectManager.Me.Position.DistanceTo(questObjective.PositionPressKey) < 3.5f)
                {
                    if (questObjective.PositionPressKey.DistanceTo(ObjectManager.Me.Position) > 3.5f &&
                        questObjective.PositionPressKey.X != 0)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.PositionPressKey));
                    }
                    else
                    {
                        MountTask.DismountMount(true);
                        Keybindings.PressKeybindings(questObjective.Keys);
                        Thread.Sleep(questObjective.WaitMsPressKey);
                        questObjective.IsUsedPressKey = true;
                    }
                }
            }

            // USE SPELL AOE
            if (questObjective.Objective == Objective.UseSpellAOE)
            {

                if (!MovementManager.InMovement || questObjective.PositionUseSpell.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
                {
                    if (questObjective.EntryAOE > 0)
                    {
                        var node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(new List<int>() { questObjective.EntryAOE }));
                        var unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(new List<int>() { questObjective.EntryAOE }));
                        Point pos = new Point();
                        if (node.IsValid)
                        {
                            questObjective.PositionUseSpell = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.PositionUseSpell = new Point(unit.Position);
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (questObjective.PositionUseSpell.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.PositionUseSpell));
                    }
                    else
                    {
                        MountTask.DismountMount(true);
                        SpellManager.CastSpellByIDAndPosition((uint)questObjective.UseSpellId, questObjective.PositionUseSpell);
                        Thread.Sleep(questObjective.WaitMsUseSpell);
                        questObjective.IsUsedUseSpellAOE = true;
                    }
                }
            }

            // USE ITEM AOE
            if (questObjective.Objective == Objective.UseItemAOE)
            {
                if (!MovementManager.InMovement || questObjective.PositionUseItem.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
                {
                    if (questObjective.EntryAOE > 0)
                    {
                        var node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(new List<int>() { questObjective.EntryAOE }));
                        var unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(new List<int>() { questObjective.EntryAOE }));
                        Point pos = new Point();
                        if (node.IsValid)
                        {
                            questObjective.PositionUseItem = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.PositionUseItem = new Point(unit.Position);
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (questObjective.PositionUseItem.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.PositionUseItem));
                    }
                    else
                    {
                        MountTask.DismountMount(true);
                        ItemsManager.UseItem((uint)questObjective.UseItemId, questObjective.PositionUseItem);
                        Thread.Sleep(questObjective.WaitMsUseItem);
                        questObjective.IsUsedUseItemAOE = true;
                    }
                }
            }
        }

        public static void PickUpQuest()
        {
            PickUpTurnInQuest(true, false);
        }

        public static void TurnInQuest()
        {
            PickUpTurnInQuest(false, true);
        }

        private static void PickUpTurnInQuest(bool pickUp, bool turnIn)
        {
            Npc npc = null;
            if (pickUp)
            {
                QuestStatus = "Pick-Up Quest";
                npc = CurrentQuest.PickUp;
            }
            if (turnIn)
            {
                QuestStatus = "Turn-In Quest";
                npc = CurrentQuest.TurnIn;
            }

            if (npc == null)
                return;

            // Go To NPC:
            MovementManager.StopMove();
            Logging.Write("Goto " + npc.Name);
            // Launch script
            //Script.Run(npc.Script); ToDo: probably add this
            // Mounting Mount
            MountTask.Mount(); // not good yet

            // Find path
            if (npc.Position.DistanceTo(ObjectManager.Me.Position) < nManagerSetting.CurrentSetting.searchRadius)
            {
                WoWUnit tNpc = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(npc.Entry), npc.Position);
                WoWGameObject tGameObj = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByEntry(npc.Entry), npc.Position);
                if (tNpc.IsValid)
                    npc.Position = tNpc.Position;
                else if (tGameObj.IsValid)
                    npc.Position = tGameObj.Position;
            }
            List<Point> points = PathFinder.FindPath(npc.Position);

            MovementManager.Go(points);
            var timer = new Timer(((int)nManager.Helpful.Math.DistanceListPoint(points) / 3 * 1000) + 5000);
            var timerNpc = new Timer(1000);
            while (MovementManager.InMovement && Usefuls.InGame &&
                   !(ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted) && !ObjectManager.Me.IsDeadMe)
            {
                if (timer.IsReady)
                    MovementManager.StopMove();
                if (npc.Position.DistanceTo(ObjectManager.Me.Position) <= 3.8f)
                    MovementManager.StopMove();

                if (npc.Position.DistanceTo(ObjectManager.Me.Position) <= 80 && timerNpc.IsReady)
                {
                    var listUnit = ObjectManager.GetWoWUnitByEntry(npc.Entry);
                    if (listUnit.Count > 0)
                    {
                        WoWUnit nNpc = ObjectManager.GetNearestWoWUnit(listUnit, npc.Position);
                        if (nNpc.IsValid)
                        {
                            MovementManager.StopMove();
                        }
                    }
                    timerNpc.Reset();
                }

                Thread.Sleep(100);
            }

            if (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted)
                return;

            // GoTo Npc:
            List<WoWUnit> tListUnit = ObjectManager.GetWoWUnitByEntry(npc.Entry);
            List<WoWGameObject> tListGameObj = ObjectManager.GetWoWGameObjectByEntry(npc.Entry);
            if (tListUnit.Count > 0 || tListGameObj.Count > 0)
            {
                WoWUnit nNpc = ObjectManager.GetNearestWoWUnit(tListUnit, npc.Position);
                WoWGameObject nGameObj = ObjectManager.GetNearestWoWGameObject(tListGameObj, npc.Position);

                var position = new Point();
                uint baseAddress = 0;
                if (nNpc.IsValid)
                {
                    position = nNpc.Position;
                    baseAddress = nNpc.GetBaseAddress;
                }
                else if (nGameObj.IsValid)
                {
                    position = nGameObj.Position;
                    baseAddress = nGameObj.GetBaseAddress;
                }
                else
                    return;

                if (position.DistanceTo(ObjectManager.Me.Position) > 4)
                {
                    points = PathFinder.FindPath(position);
                    MovementManager.Go(points);
                    timer = new Timer(Others.Times + ((int)nManager.Helpful.Math.DistanceListPoint(points) / 3 * 1000) + 5000);
                    while (MovementManager.InMovement && Usefuls.InGame &&
                           !(ObjectManager.Me.InCombat && !ObjectManager.Me.IsMounted) && !ObjectManager.Me.IsDeadMe)
                    {
                        // Update position
                        if (nNpc.IsValid)
                            position = nNpc.Position;
                        else if (nGameObj.IsValid)
                            position = nGameObj.Position;
                        else
                            return;

                        if (position.DistanceTo(ObjectManager.Me.Position) < 5)
                            MovementManager.StopMove();
                        if (timer.IsReady)
                            MovementManager.StopMove();
                        Thread.Sleep(100);
                    }
                }
                var timerGoToNpc = new Timer(1000*15);

                // Update position
                if (nNpc.IsValid)
                    position = nNpc.Position;
                else if (nGameObj.IsValid)
                    position = nGameObj.Position;
                else
                    return;

                while (position.DistanceTo(ObjectManager.Me.Position) > 5 && !timerGoToNpc.IsReady)
                {
                    // Update position
                    if (nNpc.IsValid)
                        position = nNpc.Position;
                    else if (nGameObj.IsValid)
                        position = nGameObj.Position;
                    else
                        return;

                    MovementManager.MoveTo(position);
                    if (position.DistanceTo(ObjectManager.Me.Position) < 7)
                        Interact.InteractGameObject(baseAddress);
                    Thread.Sleep(300);
                }
                MovementManager.StopMove();

                // Update position
                if (nNpc.IsValid)
                    position = nNpc.Position;
                else if (nGameObj.IsValid)
                    position = nGameObj.Position;
                else
                    return;

                if (position.DistanceTo(ObjectManager.Me.Position) < 6)
                {
                    Quest.CloseQuestWindow();
                    Interact.InteractGameObject(baseAddress);
                    Thread.Sleep(Usefuls.Latency + 200);
                    Interact.InteractGameObject(baseAddress);
                    if (pickUp)
                    {
                        Logging.Write("PickUp Quest " + CurrentQuest.Name + " id: " + CurrentQuest.Id);
                        Quest.AcceptQuest();
                        for (var i = Quest.GetNumGossipAvailableQuests(); i >= 1 && !Quest.GetLogQuestId().Contains(CurrentQuest.Id); i--)
                        {
                            if (i <= 0)
                                i = 1;

                            //int countQuestInLog = Quest.GetLogQuestId().Count;

                            Interact.InteractGameObject(baseAddress);
                            Thread.Sleep(Usefuls.Latency + 1000);
                            Quest.SelectGossipAvailableQuest(i);
                            Thread.Sleep(Usefuls.Latency + 1000);
                            Quest.AcceptQuest();
                            Thread.Sleep(Usefuls.Latency + 1000);
                            //Quest.CloseQuestWindow(); // no need to do this
                            //Thread.Sleep(Usefuls.Latency + 1000);
                            
                            //if (!Quest.GetLogQuestId().Contains(CurrentQuest.Id) && Quest.GetLogQuestId().Count > countQuestInLog)
                            //    Quest.AbandonLastQuest();
                        }
                    }
                    if (turnIn)
                    {
                        Logging.Write("turnIn Quest " + CurrentQuest.Name + " id: " + CurrentQuest.Id);
                        Quest.CompleteQuest();
                        for (var i = Quest.GetNumGossipActiveQuests(); i >= 1 && Quest.GetLogQuestId().Contains(CurrentQuest.Id); i--)
                        {
                            if (i <= 0)
                                i = 1;

                            Interact.InteractGameObject(baseAddress);
                            Thread.Sleep(Usefuls.Latency + 200);
                            Quest.SelectGossipActiveQuest(i);
                            Thread.Sleep(Usefuls.Latency + 200);
                            Quest.SelectGossipOption(npc.SelectGossipOption);
                            Thread.Sleep(Usefuls.Latency + 200);
                            Quest.CompleteQuest();
                            Thread.Sleep(Usefuls.Latency + 200);
                            //Quest.CloseQuestWindow(); // no need to do this
                            //Thread.Sleep(Usefuls.Latency + 200);
                        }
                        Quest.FinishedQuestSet.Add(CurrentQuest.Id);
                    }
                    Thread.Sleep(Usefuls.Latency);
                }
            }
            else
            {
                Logging.Write(npc.Name + " Not found");
            }
        } // end PickUpTurnInQuest
    }
}
