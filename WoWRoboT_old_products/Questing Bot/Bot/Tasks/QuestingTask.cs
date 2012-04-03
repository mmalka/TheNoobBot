using System.Collections.Generic;
using System.Threading;
using Questing_Bot.Profile;
using WowManager;
using WowManager.MiscStructs;
using WowManager.Navigation;
using WowManager.Others;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;
using WowManager.WoW.WoWObject;
using Quest = WowManager.WoW.Interface.Quest;
using Timer = WowManager.Others.Timer;

namespace Questing_Bot.Bot.Tasks
{
    class QuestingTask
    {
        public static void SelectQuest()
        {
            Config.Bot.QuestStat = "Select Quest";
            Config.Bot.CurrentQuest = new Profile.Quest();
            _currentQuestObjectiveId = -1;
            Config.Bot.CurrentQuestObjective = null;

            foreach (var quest in Config.Bot.Profile.Quests)
            {
                if (ObjectManager.Me.Level >= quest.MinLevel && // Level
                    ObjectManager.Me.Level <= quest.MaxLevel) 
                    if (!Quest.GetQuestsCompleted(quest.Id)) // Quest not completed
                        if (Quest.GetQuestsCompleted(quest.NeedQuestCompletedId) || // Quest need completed
                            quest.NeedQuestCompletedId == 0) 
                            if (Script.Run(quest.ScriptCondition)) // Condition
                            {
                                Config.Bot.CurrentQuest = quest;
                                Log.AddLog(quest.Name + ": Lvl " + quest.MinLevel + " - " + quest.MaxLevel);
                                break;
                            }
            }
        }

        static int _currentQuestObjectiveId = -1;
        public static void SelectNextQuestObjective()
        {
            Config.Bot.CurrentQuestObjective = null;
            while (true)
            {
                _currentQuestObjectiveId++;
                if (_currentQuestObjectiveId <= Config.Bot.CurrentQuest.Objectives.Count - 1) // In array
                {
                    if (Script.Run(Config.Bot.CurrentQuest.Objectives[_currentQuestObjectiveId].ScriptCondition)) // Script condition
                    {
                        Config.Bot.CurrentQuestObjective =
                            Config.Bot.CurrentQuest.Objectives[_currentQuestObjectiveId];
                        break;
                    }
                }
                else
                    break; // Out array
            }
        }

        public static void ResetQuestObjective()
        {
            Config.Bot.QuestStat = "Reset Quest Objective";
            _currentQuestObjectiveId = -1;
            Config.Bot.CurrentQuestObjective = null;
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
                if (WowManager.WoW.ItemManager.Item.GetItemCountByIdLUA((uint)questObjective.CollectItemId) < questObjective.CollectCount)
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
                if (questObjective.IsUsedUseItem)
                {
                    questObjective.IsUsedUseItem = false;
                    return true;
                }
                return false;
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
                if (questObjective.IsUsedWaitMs)
                {
                    questObjective.IsUsedWaitMs = false;
                    return true;
                }
            }

            // INTERACT WITH
            if (questObjective.Objective == Objective.InteractWith)
            {
                if (questObjective.IsUsedInteractWith)
                {
                    questObjective.IsUsedInteractWith = false;
                    return true;
                }
            }

            // USE SPELL
            if (questObjective.Objective == Objective.UseSpell)
            {
                if (questObjective.IsUsedUseSpell)
                {
                    questObjective.IsUsedUseSpell = false;
                    return true;
                }
                return false;
            }

            // EQUIP ITEM
            if (questObjective.Objective == Objective.EquipItem)
            {
                if (questObjective.IsUsedEquipItem)
                {
                    questObjective.IsUsedEquipItem = false;
                    return true;
                }
                return false;
            }

            // PICK UP QUEST
            if (questObjective.Objective == Objective.PickUpQuest)
            {
                if (questObjective.IsUsedPickUpQuest)
                {
                    questObjective.IsUsedPickUpQuest = false;
                    return true;
                }
                return false;
            }

            // TURN IN QUEST
            if (questObjective.Objective == Objective.TurnInQuest)
            {
                if (questObjective.IsUsedTurnInQuest)
                {
                    questObjective.IsUsedTurnInQuest = false;
                    return true;
                }
                return false;
            }

            // USE VEHICLE
            if (questObjective.Objective == Objective.UseVehicle)
            {
                return Useful.PlayerUsingVehicle;
            }

            // EJECT VEHICLE
            if (questObjective.Objective == Objective.EjectVehicle)
            {
                return !Useful.PlayerUsingVehicle;
            }

            // PRESS KEY
            if (questObjective.Objective == Objective.PressKey)
            {
                if (questObjective.IsUsedPressKey)
                {
                    questObjective.IsUsedPressKey = false;
                    return true;
                }
                return false;         
            }

            // USE SPELL AOE
            if (questObjective.Objective == Objective.UseSpellAOE)
            {
                if (questObjective.IsUsedUseSpellAOE)
                {
                    questObjective.IsUsedUseSpellAOE = false;
                    return true;
                }
                return false;
            }

            // USE ITEM AOE
            if (questObjective.Objective == Objective.UseItemAOE)
            {
                if (questObjective.IsUsedUseItemAOE)
                {
                    questObjective.IsUsedUseItemAOE = false;
                    return true;
                }
                return false;
            }

            return false;
        }
        public static bool CurrentQuestObjectiveIsFinish()
        {
            return QuestObjectiveIsFinish(ref Config.Bot.CurrentQuestObjective);
        }

        public static void CurrentQuestObjectiveExecute()
        {
            QuestObjectiveExecute(ref Config.Bot.CurrentQuestObjective);
        }
        public static void QuestObjectiveExecute(ref QuestObjective questObjective)
        {
            if (questObjective == null)
                return;

            Config.Bot.QuestStat = questObjective.Objective.ToString();

            // Create Path:
            if (questObjective.PathHotsports == null)
            {
                if (questObjective.Hotspots.Count > 0)
                {
                    questObjective.PathHotsports = new List<Point>();
                    for (var i = 0; i <= questObjective.Hotspots.Count - 1 && Config.Bot.BotIsActive; i++)
                    {
                        int iLast = i - 1;
                        if (iLast < 0)
                            iLast = questObjective.Hotspots.Count - 1;
                        Log.AddLog(Translation.GetText(Translation.Text.Create_points_HotSpot) + " " + iLast + " " + Translation.GetText(Translation.Text.to_HotSpot) + " " + i);
                        List<Point> points = PathFinderManager.FindPath(questObjective.Hotspots[iLast], questObjective.Hotspots[i]);
                        questObjective.PathHotsports.AddRange(points);
                    }
                }
                else
                {
                    questObjective.PathHotsports = new List<Point>
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

                    if (!Functions.IsInAvoidMobsList(wowUnit) && !Functions.IsInBlackListZone(wowUnit.Position) && wowUnit.GetDistance <= Config.Bot.SearchDistance && Config.Bot.CurrentQuestObjective.PathHotsports[Others.NearestPointOfListPoints(Config.Bot.CurrentQuestObjective.PathHotsports, wowUnit.Position)].DistanceTo(wowUnit.Position) <= Config.Bot.SearchDistance && !Config.Bot.BlackListGuid.Contains(wowUnit.Guid) && wowUnit.IsAlive && wowUnit.IsValid && (Config.Bot.FormConfig.AttackNpcInCombat || !wowUnit.InCombat))
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.Attack) + "  Lvl " + wowUnit.Name);
                        Fight.StartFight(wowUnit.Guid);
                        if (wowUnit.IsDead)
                        {
                            Config.Bot.Kills++;
                            questObjective.CurrentCount++;
                            Thread.Sleep(Useful.Latency + 1000);
                            while (ObjectManager.Me.InCombat && ObjectManager.GetUnitAttackPlayer().Count <= 0)
                            {
                                Thread.Sleep(10);
                            }
                        }
                    }
                    else if (!MovementManager.InMovement && questObjective.PathHotsports.Count > 0)
                    {
                        // Mounting Mount
                        MountTask.MountingMount();
                        // Need GoTo Zone:
                        if (questObjective.PathHotsports[Others.NearestPointOfListPoints(questObjective.PathHotsports, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
                        {
                            MovementManager.Go(Functions.GoToPathFind(questObjective.PathHotsports[Others.NearestPointOfListPoints(questObjective.PathHotsports, ObjectManager.Me.Position)]));
                        }
                        else
                        {
                            // Start Move
                            MovementManager.GoLoop(questObjective.PathHotsports);
                        }
                    }
            }

            // PICK UP OBJECT
            if (questObjective.Objective == Objective.PickUpObject)
            {
                var node = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectById(questObjective.Entry));

                if (!Functions.IsInBlackListZone(node.Position) && node.GetDistance < Config.Bot.SearchDistance && Config.Bot.CurrentQuestObjective.PathHotsports[Others.NearestPointOfListPoints(Config.Bot.CurrentQuestObjective.PathHotsports, node.Position)].DistanceTo(node.Position) <= Config.Bot.SearchDistance && !Config.Bot.BlackListGuid.Contains(node.Guid) && node.IsValid)
                {
                    int tNumber = Config.Bot.NumberFarm;
                    FarmingTask.Pulse(new List<WoWGameObject> {node});
                    if (Config.Bot.NumberFarm > tNumber)
                        questObjective.CurrentCount++;
                }
                else if (!MovementManager.InMovement && questObjective.PathHotsports.Count > 0)
                {
                    // Mounting Mount
                    MountTask.MountingMount();
                    // Need GoTo Zone:
                    if (questObjective.PathHotsports[Others.NearestPointOfListPoints(questObjective.PathHotsports, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position) > 5)
                    {
                        MovementManager.Go(Functions.GoToPathFind(questObjective.PathHotsports[Others.NearestPointOfListPoints(questObjective.PathHotsports, ObjectManager.Me.Position)]));
                    }
                    else
                    {
                        // Start Move
                        MovementManager.GoLoop(questObjective.PathHotsports); 
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
                        MountTask.MountingMount();
                        MovementManager.Go(Functions.GoToPathFind(questObjective.PositionUseItem));
                    }
                    else
                    {
                        MovementManager.Stop();
                        MovementManager.StopMove();
                        MountTask.DismountMount();
                        WowManager.WoW.ItemManager.Item.UseItem(WowManager.WoW.ItemManager.Item.GetNameById((uint)questObjective.UseItemId));
                        Thread.Sleep(questObjective.WaitMsUseItem);
                        questObjective.IsUsedUseItem = true;
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
                        MountTask.MountingMount();
                        MovementManager.Go(Functions.GoToPathFind(questObjective.MoveTo));
                    }
                }
            }

            // WAIT
            if (questObjective.Objective == Objective.Wait)
            {
                Thread.Sleep(questObjective.WaitMs);
                questObjective.IsUsedWaitMs = true;
            }

            // INTERACT WITH
            if (questObjective.Objective == Objective.InteractWith)
            {
                if (!MovementManager.InMovement)
                {
                    if (questObjective.PositionInteractWith.DistanceTo(ObjectManager.Me.Position) > Config.Bot.SearchDistance &&
                        questObjective.PositionInteractWith.X != 0)
                    {
                        MountTask.MountingMount();
                        MovementManager.Go(Functions.GoToPathFind(questObjective.PositionInteractWith));
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

                        MovementManager.Go(Functions.GoToPathFind(pos));
                        Thread.Sleep(1000);
                        while (MovementManager.InMovement && pos.DistanceTo(ObjectManager.Me.Position) > 3.9f)
                        {
                            if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMount))
                                return;
                            Thread.Sleep(100);
                        }
                        MovementManager.Stop();
                        MovementManager.StopMove();
                        MountTask.DismountMount();
                        Interact.InteractGameObject(baseAddress);

                        if (questObjective.GossipOptionsInteractWith != -1)
                        {
                            Thread.Sleep(Useful.Latency + 500);
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

                    if (questObjective.PositionUseSpell.DistanceTo(ObjectManager.Me.Position) > 3.5f &&
                        questObjective.PositionUseSpell.X != 0)
                    {
                        MountTask.MountingMount();
                        MovementManager.Go(Functions.GoToPathFind(questObjective.PositionUseSpell));
                    }
                    else
                    {
                        MovementManager.Stop();
                        MovementManager.StopMove();
                        MountTask.DismountMount();
                        Spell t = new Spell((uint)questObjective.UseSpellId);
                        t.Launch();
                        Thread.Sleep(questObjective.WaitMsUseSpell);
                        questObjective.IsUsedUseSpell = true;
                    }
                }
            }

            // EQUIP ITEM
            if (questObjective.Objective == Objective.EquipItem)
            {
                if (ObjectManager.Me.IsDeadMe || ObjectManager.Me.InCombat)
                    return;
                WowManager.WoW.ItemManager.Item.EquipItemByName(WowManager.WoW.ItemManager.Item.GetNameById((uint)questObjective.EquipItemId));
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
                    if (questObjective.PositionVehicle.DistanceTo(ObjectManager.Me.Position) > Config.Bot.SearchDistance &&
                        questObjective.PositionVehicle.X != 0)
                    {
                        MountTask.MountingMount();
                        MovementManager.Go(Functions.GoToPathFind(questObjective.PositionVehicle));
                    }
                    else
                    {
                        
                        var unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(new List<int>() { questObjective.EntryVehicle }), questObjective.PositionVehicle);
                        if (!unit.IsValid)
                        {
                            return;
                        }

                        MovementManager.Go(Functions.GoToPathFind(unit.Position));
                        Thread.Sleep(1000);
                        while (MovementManager.InMovement && unit.IsValid && ObjectManager.Me.Position.DistanceTo(unit.Position) > 4)
                        {
                            if (ObjectManager.Me.IsDeadMe || (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMount))
                                return;
                            Thread.Sleep(100);
                        }
                        MovementManager.Stop();
                        MovementManager.StopMove();
                        MountTask.DismountMount();
                        Interact.InteractGameObject(unit.GetBaseAddress);
                        Thread.Sleep(Useful.Latency + 500);
                    }
                }
            }

            // EJECT VEHICLE
            if (questObjective.Objective == Objective.EjectVehicle)
            {
                Useful.EjectVehicle();
                Thread.Sleep(Useful.Latency + 500);
            }

            // PRESS KEY
            if (questObjective.Objective == Objective.PressKey)
            {
                if (!MovementManager.InMovement || ObjectManager.Me.Position.DistanceTo(questObjective.PositionPressKey) < 3.5f)
                {
                    if (questObjective.PositionPressKey.DistanceTo(ObjectManager.Me.Position) > 3.5f &&
                        questObjective.PositionPressKey.X != 0)
                    {
                        MountTask.MountingMount();
                        MovementManager.Go(Functions.GoToPathFind(questObjective.PositionPressKey));
                    }
                    else
                    {
                        MovementManager.Stop();
                        MovementManager.StopMove();
                        MountTask.DismountMount();
                        WowManager.WoW.Useful.Keybindings.PressKeybindings(questObjective.Keys);
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
                        MountTask.MountingMount();
                        MovementManager.Go(Functions.GoToPathFind(questObjective.PositionUseSpell));
                    }
                    else
                    {
                        MountTask.DismountMount();
                        MovementManager.Stop();
                        MovementManager.StopMove();
                        WowManager.WoW.SpellManager.SpellManager.CastSpellByIDAndPosition((uint)questObjective.UseSpellId, questObjective.PositionUseSpell);
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
                        MountTask.MountingMount();
                        MovementManager.Go(Functions.GoToPathFind(questObjective.PositionUseItem));
                    }
                    else
                    {
                        MountTask.DismountMount();
                        MovementManager.Stop();
                        MovementManager.StopMove();
                        WowManager.WoW.ItemManager.Item.UseItem((uint)questObjective.UseItemId, questObjective.PositionUseItem);
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
                npc = Config.Bot.CurrentQuest.PickUp;
                Config.Bot.QuestStat = "Pick-Up Quest";
            }
            if (turnIn)
            {
                Config.Bot.QuestStat = "Turn-In Quest";
                npc = Config.Bot.CurrentQuest.TurnIn;
            }

            if (npc == null)
                return;

            // Go To NPC:
            MovementManager.StopMove();
            Log.AddLog(Translation.GetText(Translation.Text.GoTo) + " " + npc.Name);
            // Launch script
            Script.Run(npc.Script);
            // Mounting Mount
            MountTask.MountingMount();

            // Find path
             
            if (npc.Position.DistanceTo(ObjectManager.Me.Position) < Config.Bot.Profile.SearchDistance)
             {
                 WoWUnit tNpc = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(npc.Entry), npc.Position);
                 WoWGameObject tGameObj = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByEntry(npc.Entry), npc.Position);
                 if (tNpc.IsValid)
                     npc.Position = tNpc.Position;
                 else if (tGameObj.IsValid)
                     npc.Position = tGameObj.Position;
             }
            List<Point> points = Functions.GoToPathFind(npc.Position);

            MovementManager.Go(points);
            var timer = new Timer(((int)Point.SizeListPoint(points) / 3 * 1000) + 5000);
            while (MovementManager.InMovement && Config.Bot.BotIsActive && Useful.InGame &&
                   !(ObjectManager.Me.InCombat && !ObjectManager.Me.IsMount) && !ObjectManager.Me.IsDeadMe)
            {
                if (timer.isReady)
                    MovementManager.StopMove();
                if (npc.Position.DistanceTo(ObjectManager.Me.Position) <= 3.8f)
                    MovementManager.StopMove();
                Thread.Sleep(100);
            }

            if (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMount)
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
                    points = Functions.GoToPathFind(position);
                    MovementManager.Go(points);
                    timer = new Timer(Others.Times + ((int)Point.SizeListPoint(points) / 3 * 1000) + 5000);
                    while (MovementManager.InMovement && Config.Bot.BotIsActive && Useful.InGame &&
                           !(ObjectManager.Me.InCombat && !ObjectManager.Me.IsMount) && !ObjectManager.Me.IsDeadMe)
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
                        if (timer.isReady)
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

                while (position.DistanceTo(ObjectManager.Me.Position) > 5 && !timerGoToNpc.isReady)
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
                    Thread.Sleep(Useful.Latency + 200);
                    Interact.InteractGameObject(baseAddress);
                    if (pickUp)
                    {
                        Log.AddLog("PickUp Quest " + Config.Bot.CurrentQuest.Name + " id: " + Config.Bot.CurrentQuest.Id);
                        Quest.AcceptQuest();
                        for (var i = Quest.GetNumGossipAvailableQuests(); i >= 1 && !Quest.GetLogQuestId().Contains(Config.Bot.CurrentQuest.Id); i--)
                        {
                            if (i <= 0)
                                i = 1;

                            int countQuestInLog = Quest.GetLogQuestId().Count;

                            Interact.InteractGameObject(baseAddress);
                            Thread.Sleep(Useful.Latency + 200);
                            Quest.SelectGossipAvailableQuest(i);
                            Thread.Sleep(Useful.Latency + 200);
                            Quest.AcceptQuest();
                            Thread.Sleep(Useful.Latency + 200);
                            Quest.CloseQuestWindow();
                            Thread.Sleep(Useful.Latency + 200);
                            if (!Quest.GetLogQuestId().Contains(Config.Bot.CurrentQuest.Id) && Quest.GetLogQuestId().Count > countQuestInLog)
                                Quest.AbandonLastQuest();
                        }
                    }
                    if (turnIn)
                    {
                        Log.AddLog("turnIn Quest " + Config.Bot.CurrentQuest.Name + " id: " + Config.Bot.CurrentQuest.Id);
                        Quest.CompleteQuest();
                        for (var i = Quest.GetNumGossipActiveQuests(); i >= 1 && Quest.GetLogQuestId().Contains(Config.Bot.CurrentQuest.Id); i--)
                        {
                            if (i <= 0)
                                i = 1;

                            Interact.InteractGameObject(baseAddress);
                            Thread.Sleep(Useful.Latency + 200);
                            Quest.SelectGossipActiveQuest(i);
                            Thread.Sleep(Useful.Latency + 200);
                            Quest.SelectGossipOption(npc.SelectGossipOption);
                            Thread.Sleep(Useful.Latency + 200);
                            Quest.CompleteQuest();
                            Thread.Sleep(Useful.Latency + 200);
                            Quest.CloseQuestWindow();
                            Thread.Sleep(Useful.Latency + 200);
                        }
                        Quest.FinishQuestForceSet.Add(Config.Bot.CurrentQuest.Id);
                    }
                    Thread.Sleep(Useful.Latency);
                }
            }
            else
            {
                Log.AddLog(npc.Name + " " + Translation.GetText(Translation.Text.no_found));
            }
        }
    }
}
