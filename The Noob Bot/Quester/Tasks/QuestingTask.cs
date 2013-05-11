using System;
using System.Collections.Generic;
using System.Threading;
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

namespace Quester.Tasks
{
    internal class QuestingTask
    {
        private static string QuestStatus = "";
        public static Profile.Quest CurrentQuest = new Profile.Quest();
        private static int _currentQuestObjectiveId = -1;
        public static Profile.QuestObjective CurrentQuestObjective;
        private static Timer waitTimer;
        public static bool completed = false;

        private static bool _HARDMODE_ = false;

        public static void SelectQuest()
        {
            QuestStatus = "Select Quest";
            CurrentQuest = new Profile.Quest();
            _currentQuestObjectiveId = -1;
            CurrentQuestObjective = null;

            Quester.Bot.Bot.Profile.Quests.Sort();

            for (int relax = 0; relax <= 2; relax++) // search quest with level = mine, then level = mine+1, then +2
            {
                foreach (var quest in Quester.Bot.Bot.Profile.Quests)
                {
                    if (Quest.GetLogQuestId().Contains(quest.Id))
                    {
                        CurrentQuest = quest;
                        Logging.Write("resuming \"" + quest.Name + "\": Lvl " + quest.QuestLevel + " (" + quest.MinLevel + " - " + quest.MaxLevel + ")");
                        return;
                    }
                }
                foreach (var quest in Quester.Bot.Bot.Profile.Quests)
                {
                    if (ObjectManager.Me.Level >= quest.MinLevel && ObjectManager.Me.Level <= quest.MaxLevel &&
                        (_HARDMODE_ || ObjectManager.Me.Level >= quest.QuestLevel - relax)) // Level
                        if (!Quest.GetQuestCompleted(quest.Id)) // Quest not completed
                            if (!Quest.GetQuestCompleted(quest.NeedQuestNotCompletedId)) // Quest done which discalify this one
                                if (Quest.GetQuestCompleted(quest.NeedQuestCompletedId) || // Quest need completed
                                    quest.NeedQuestCompletedId.Count == 0)
                                    if (quest.ItemPickUp == 0 || (quest.ItemPickUp != 0 && ItemsManager.GetItemCountByIdLUA((uint) quest.ItemPickUp) > 0))
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
            foreach (QuestObjective obj in CurrentQuest.Objectives)
            {
                obj.CurrentCount = 0;
                obj.IsObjectiveCompleted = false;
            }
            SelectNextQuestObjective();
        }

        public static bool QuestObjectiveIsFinish(ref QuestObjective questObjective)
        {
            if (questObjective == null)
                return true;

            // shortcut since we do objective one by one, for kill it can be completed before we do them all
            if (Quest.GetLogQuestIsComplete(CurrentQuest.Id))
                return true;

            if (questObjective.ScriptConditionIsComplete != string.Empty)
                return Script.Run(questObjective.ScriptConditionIsComplete);

            // COLLECT ITEM || BUY ITEM
            if (questObjective.CollectItemId > 0 && questObjective.CollectCount > 0)
                if (ItemsManager.GetItemCountByIdLUA(questObjective.CollectItemId) < questObjective.CollectCount)
                    return false;

            // KILL MOB
            if (questObjective.Objective == Objective.KillMob)
            {
                if (questObjective.Count == -1)
                    return false;
                else if (questObjective.CurrentCount >= questObjective.Count)
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
                if (ItemsManager.GetItemCountByIdLUA(questObjective.CollectItemId) >= questObjective.CollectCount)
                    return true;
                return false;
            }

            /* MOVE TO || WAIT || TRAIN ALL SPELLS || 
             * INTERACT WITH || USE SPELL || EQUIP ITEM || 
             * PICK UP QUEST || TURN IN QUEST || PRESS KEY || 
             * USE SPELL AOE || USE ITEM AOE || USE RUNEFORGE
             */
            if (questObjective.Objective == Objective.MoveTo || questObjective.Objective == Objective.Wait || questObjective.Objective == Objective.TrainSpells ||
                questObjective.Objective == Objective.InteractWith || questObjective.Objective == Objective.UseSpell || questObjective.Objective == Objective.EquipItem ||
                questObjective.Objective == Objective.PickUpQuest || questObjective.Objective == Objective.TurnInQuest || questObjective.Objective == Objective.PressKey ||
                questObjective.Objective == Objective.UseItemAOE || questObjective.Objective == Objective.UseSpellAOE || questObjective.Objective == Objective.UseRuneForge)
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
            foreach (var npc in Quester.Bot.Bot.Profile.AvoidMobs)
            {
                if ((npc.Position.X == 0.0f || npc.Position.DistanceTo(woWUnit.Position) <= 40) &&
                    npc.Entry == woWUnit.Entry)
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
                WoWUnit wowUnit =
                    ObjectManager.GetNearestWoWUnit(
                        ObjectManager.GetWoWUnitByEntry(questObjective.Entry));

                if (!wowUnit.IsValid && questObjective.Factions.Count > 0)
                    wowUnit =
                        ObjectManager.GetNearestWoWUnit(
                            ObjectManager.GetWoWUnitByFaction(questObjective.Factions));

                if (!IsInAvoidMobsList(wowUnit) && !nManagerSetting.IsBlackListedZone(wowUnit.Position) &&
                    !nManagerSetting.IsBlackListed(wowUnit.Guid) && wowUnit.IsAlive && wowUnit.IsValid &&
                    (nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight || !wowUnit.InCombat))
                {
                    Logging.Write("Attacking Lvl " + wowUnit.Name);
                    ulong Unkillable = Fight.StartFight(wowUnit.Guid);
                    if (!wowUnit.IsDead && Unkillable != 0)
                    {
                        nManagerSetting.AddBlackList(Unkillable, 3 * 60 * 1000);
                    }
                    else if (wowUnit.IsDead)
                    {
                        Statistics.Kills++;
                        questObjective.CurrentCount++;
                        Thread.Sleep(Usefuls.Latency + 1000);
                        while (!ObjectManager.Me.IsMounted && ObjectManager.Me.InCombat &&
                               ObjectManager.GetUnitAttackPlayer().Count <= 0)
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
                    if (
                        questObjective.PathHotspots[
                            nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots,
                                                                           ObjectManager.Me.Position)].DistanceTo(
                                                                               ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(
                            PathFinder.FindPath(
                                questObjective.PathHotspots[
                                    nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots,
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
                var node =
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
                            nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots,
                                                                           ObjectManager.Me.Position)].DistanceTo(
                                                                               ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(
                            PathFinder.FindPath(
                                questObjective.PathHotspots[
                                    nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots,
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
                    if (questObjective.EntryAOE > 0)
                    {
                        var node =
                            ObjectManager.GetNearestWoWGameObject(
                                ObjectManager.GetWoWGameObjectById(new List<int>() {questObjective.EntryAOE}));
                        var unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(new List<int>() {questObjective.EntryAOE}));
                        Point pos = new Point();
                        if (node.IsValid)
                        {
                            questObjective.Position = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.Position = new Point(unit.Position);
                        }
                    }

                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range &&
                        questObjective.Position.X != 0)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        MountTask.DismountMount(true);
                        MovementManager.StopMove();
                        if (questObjective.EntryAOE > 0)
                        {
                            var node =
                                ObjectManager.GetNearestWoWGameObject(
                                    ObjectManager.GetWoWGameObjectById(new List<int>() {questObjective.EntryAOE}));
                            var unit =
                                ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.GetWoWUnitByEntry(new List<int>() {questObjective.EntryAOE}));
                            if (node.IsValid)
                            {
                                MovementManager.Face(node);
                                Interact.InteractGameObject(node.GetBaseAddress);
                                nManagerSetting.AddBlackList(node.Guid, 30*1000);
                            }
                            else if (unit.IsValid)
                            {
                                MovementManager.Face(unit);
                                Interact.InteractGameObject(unit.GetBaseAddress);
                                nManagerSetting.AddBlackList(unit.Guid, 30*1000);
                            }
                        }
                        ItemsManager.UseItem(ItemsManager.GetNameById((uint) questObjective.UseItemId));
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
                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range &&
                        questObjective.Position.X != 0)
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

            // TRAIN ALL SPELLS
            if (questObjective.Objective == Objective.TrainSpells)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) < questObjective.Range)
                    {
                        var unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(new List<int>() {questObjective.EntryInteractWith}));
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
                        questObjective.IsObjectiveCompleted = true;
                    }
                    else
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
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
                        var node =
                            ObjectManager.GetNearestWoWGameObject(
                                ObjectManager.GetWoWGameObjectById(new List<int>() {questObjective.EntryInteractWith}));
                        var unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(new List<int>() {questObjective.EntryInteractWith}));
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
                    if (questObjective.EntryAOE > 0)
                    {
                        var node =
                            ObjectManager.GetNearestWoWGameObject(
                                ObjectManager.GetWoWGameObjectById(new List<int>() {questObjective.EntryAOE}));
                        var unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(new List<int>() {questObjective.EntryAOE}));
                        Point pos = new Point();
                        if (node.IsValid)
                        {
                            questObjective.Position = new Point(node.Position);
                        }
                        else if (unit.IsValid)
                        {
                            questObjective.Position = new Point(unit.Position);
                        }
                    }

                    if (questObjective.Position.X != 0 &&
                        questObjective.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        MountTask.DismountMount(true);
                        if (questObjective.EntryAOE > 0)
                        {
                            var node =
                                ObjectManager.GetNearestWoWGameObject(
                                    ObjectManager.GetWoWGameObjectById(new List<int>() {questObjective.EntryAOE}));
                            var unit =
                                ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.GetWoWUnitByEntry(new List<int>() {questObjective.EntryAOE}));
                            if (node.IsValid)
                            {
                                MovementManager.Face(node);
                                Interact.InteractGameObject(node.GetBaseAddress);
                                MovementManager.StopMove(); // because interact will make the character go to the target due to CTM
                            }
                            else if (unit.IsValid)
                            {
                                MovementManager.Face(unit);
                                Interact.InteractGameObject(unit.GetBaseAddress);
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
                            nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots,
                                                                           ObjectManager.Me.Position)].DistanceTo(
                                                                               ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(
                            PathFinder.FindPath(
                                questObjective.PathHotspots[
                                    nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots,
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
                ItemsManager.EquipItemByName(ItemsManager.GetNameById((uint) questObjective.EquipItemId));
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
                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) >
                        nManagerSetting.CurrentSetting.GatheringSearchRadius &&
                        questObjective.Position.X != 0)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        var unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(new List<int>() {questObjective.EntryVehicle}),
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
                if (!MovementManager.InMovement ||
                    ObjectManager.Me.Position.DistanceTo(questObjective.Position) < 5.0f)
                {
                    if (questObjective.Position.DistanceTo(ObjectManager.Me.Position) > 5.0f &&
                        questObjective.Position.X != 0)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                    }
                    else
                    {
                        MountTask.DismountMount(true);
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
                    if (questObjective.EntryAOE > 0)
                    {
                        var node =
                            ObjectManager.GetNearestWoWGameObject(
                                ObjectManager.GetWoWGameObjectById(new List<int>() {questObjective.EntryAOE}));
                        var unit =
                            ObjectManager.GetNearestWoWUnit(
                                ObjectManager.GetWoWUnitByEntry(new List<int>() {questObjective.EntryAOE}));
                        Point pos = new Point();
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
                        MountTask.DismountMount(true);
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
                if (!MovementManager.InMovement || questObjective.Position.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
                {
                    var Target = new Npc
                        {
                            Entry = questObjective.EntryAOE,
                            Position = questObjective.Position,
                            Name = "Unknown",
                            ContinentId = (ContinentId) Usefuls.ContinentId,
                            Faction = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
                            SelectGossipOption = questObjective.GossipOptionsInteractWith
                        };
                    WoWUnit TargetIsNPC;
                    WoWObject TargetIsObject;
                    Target = questObjective.Range > 5f
                                 ? MovementManager.FindTarget(Target, out TargetIsNPC, out TargetIsObject, questObjective.Range)
                                 : MovementManager.FindTarget(Target, out TargetIsNPC, out TargetIsObject);
                    //End target finding based on EntryAOE.
                    if (questObjective.EntryAOE <= 0 && !TargetIsNPC.IsValid && !TargetIsObject.IsValid)
                    {
                        ItemsManager.UseItem((uint) questObjective.UseItemId, Target.Position);
                        Thread.Sleep(questObjective.WaitMs);
                        questObjective.IsObjectiveCompleted = true;
                    }
                    else if (TargetIsNPC.IsValid || TargetIsObject.IsValid)
                    {
                        uint baseAddress = TargetIsNPC.IsValid ? TargetIsNPC.GetBaseAddress : TargetIsObject.GetBaseAddress;
                        Interact.InteractGameObject(baseAddress);
                        ItemsManager.UseItem((uint) questObjective.UseItemId, Target.Position);
                        Thread.Sleep(questObjective.WaitMs);
                        questObjective.IsObjectiveCompleted = true;
                    }
                    else
                    {
                        return; // target not found
                    }
                }
            }

            // BUY ITEM
            if (questObjective.Objective == Objective.BuyItem)
            {
                if (!MovementManager.InMovement || questObjective.Position.DistanceTo(ObjectManager.Me.Position) <= questObjective.Range)
                {
                    var Target = new Npc
                        {
                            Entry = questObjective.EntryAOE,
                            Position = questObjective.Position,
                            Name = questObjective.Name,
                            ContinentId = (ContinentId) Usefuls.ContinentId,
                            Faction = ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
                            SelectGossipOption = questObjective.GossipOptionsInteractWith
                        };
                    WoWUnit TargetIsNPC;
                    WoWObject TargetIsObject;
                    Target = questObjective.Range > 5f
                                 ? MovementManager.FindTarget(Target, out TargetIsNPC, out TargetIsObject, questObjective.Range)
                                 : MovementManager.FindTarget(Target, out TargetIsNPC, out TargetIsObject);
                    if (TargetIsNPC.IsValid || TargetIsObject.IsValid)
                    {
                        uint baseAddress = TargetIsNPC.IsValid ? TargetIsNPC.GetBaseAddress : TargetIsObject.GetBaseAddress;
                        Interact.InteractGameObject(baseAddress);
                        Thread.Sleep(500 + Usefuls.Latency);
                        Vendor.BuyItem(ItemsManager.GetNameById(questObjective.CollectItemId), questObjective.CollectCount);
                        Thread.Sleep(questObjective.WaitMs == 0 ? 1000 + Usefuls.Latency : questObjective.WaitMs);
                        if (ItemsManager.GetItemCountByIdLUA(questObjective.CollectItemId) >= questObjective.CollectCount)
                        {
                            nManagerSetting.CurrentSetting.DontSellTheseItems.Add(ItemsManager.GetNameById(questObjective.CollectItemId));
                            questObjective.IsObjectiveCompleted = true;
                        }
                    }
                    else
                    {
                        return; // target not found
                    }
                }
                else
                {
                    MountTask.Mount();
                    MovementManager.Go(PathFinder.FindPath(questObjective.Position));
                }
            }

            // USE RUNEFORGE
            if (questObjective.Objective == Objective.UseRuneForge)
            {
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
                        MountTask.DismountMount(true);
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
                WoWUnit wowUnit;
                List<WoWUnit> allProperUnits = new List<WoWUnit>();
                foreach (WoWUnit unit in allUnits)
                {
                    if (!unit.HaveBuff((uint) questObjective.BuffId))
                        allProperUnits.Add(unit);
                }
                wowUnit = ObjectManager.GetNearestWoWUnit(allProperUnits);

                if (wowUnit.IsValid && !MovementManager.InMovement)
                {
                    if (wowUnit.Position.DistanceTo(ObjectManager.Me.Position) > questObjective.Range)
                    {
                        MountTask.Mount();
                        MovementManager.Go(PathFinder.FindPath(wowUnit.Position));
                    }
                    else
                    {
                        MountTask.DismountMount(true);
                        Logging.WriteDebug("Buffing " + wowUnit.Name + "(" + wowUnit.GetBaseAddress + ")");
                        ItemsManager.UseItem(ItemsManager.GetNameById((uint) questObjective.UseItemId));
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
                            nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots,
                                                                           ObjectManager.Me.Position)].DistanceTo(
                                                                               ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(
                            PathFinder.FindPath(
                                questObjective.PathHotspots[
                                    nManager.Helpful.Math.NearestPointOfListPoints(questObjective.PathHotspots,
                                                                                   ObjectManager.Me.Position)]));
                    }
                    else
                    {
                        // Start Move
                        MovementManager.GoLoop(questObjective.PathHotspots);
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
                npc = Quester.Bot.Bot.FindQuesterById(CurrentQuest.PickUp);
            }
            if (turnIn)
            {
                QuestStatus = "Turn-In Quest";
                npc = Quester.Bot.Bot.FindQuesterById(CurrentQuest.TurnIn);
            }

            if (pickUp && item != 0)
            {
                ItemsManager.UseItem(ItemsManager.GetNameById((uint) item));
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
            WoWUnit TargetIsNPC;
            WoWObject TargetIsObject;
            uint baseAddress;
            Npc Target = MovementManager.FindTarget(npc, out TargetIsNPC, out TargetIsObject);
            if (TargetIsNPC.IsValid)
                baseAddress = TargetIsNPC.GetBaseAddress;
            else if (TargetIsObject.IsValid)
                baseAddress = TargetIsObject.GetBaseAddress;
            else
            {
                baseAddress = 0;
                //NpcDB.DelNpc(npc); // I don't particularly recommand it for Quester, as we can simply abort this quest and log it.
                // ToDo: Stop working on that quest.
            }
            //End target finding based on QuestGiver.

            if (Target.Position.DistanceTo(ObjectManager.Me.Position) < 6)
            {
                Quest.CloseQuestWindow();
                Interact.InteractGameObject(baseAddress);
                Thread.Sleep(Usefuls.Latency + 600);
                if (TargetIsObject.IsValid)
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
                                else
                                {
                                    Quest.CloseQuestWindow();
                                    Thread.Sleep(Usefuls.Latency + 500);
                                    Quest.AbandonQuest(id);
                                    Interact.InteractGameObject(baseAddress);
                                    Thread.Sleep(Usefuls.Latency + 500);
                                }
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
                                else
                                {
                                    Quest.CloseQuestWindow();
                                    Thread.Sleep(Usefuls.Latency + 500);
                                    Quest.AbandonQuest(id);
                                    Interact.InteractGameObject(baseAddress);
                                    Thread.Sleep(Usefuls.Latency + 500);
                                }
                                gossipid++;
                            }
                        }
                    }
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
                                else
                                {
                                    Quest.CloseQuestWindow();
                                    Thread.Sleep(Usefuls.Latency + 500);
                                    Interact.InteractGameObject(baseAddress);
                                    Thread.Sleep(Usefuls.Latency + 500);
                                }
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
                                else
                                {
                                    Quest.CloseQuestWindow();
                                    Thread.Sleep(Usefuls.Latency + 500);
                                    Interact.InteractGameObject(baseAddress);
                                    Thread.Sleep(Usefuls.Latency + 500);
                                }
                                gossipid++;
                            }
                        }
                    }
                }
                Thread.Sleep(Usefuls.Latency);
            }
        }
    }

    // end PickUpTurnInQuest
}

// some maybe useful stuffs
// GetNumQuestPOIWorldEffects May we find hotspots with this?