using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.MemoryClass;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class Quest
    {
        public static List<int> FinishedQuestSet = new List<int>();
        public static List<int> KilledMobsToCount = new List<int>();
        public static int AbandonnedId = 0;
        private static Point _travelLocation = null;
        private static bool _travelDisabled = false;

        static Quest()
        {
            GetSetIgnoreFight = false;
            GetSetIgnoreAllFight = false;
            GetSetDismissPet = false;
            MountTask.AllowMounting = true;
        }

        public static bool GetSetIgnoreFight { get; set; }
        public static bool GetSetIgnoreAllFight { get; set; }
        public static bool GetSetDismissPet { get; set; }

        public static bool GetQuestCompleted(List<int> qList)
        {
            foreach (int q in qList)
            {
                if (q == -1)
                    return false;
                if (GetQuestCompleted(q))
                    return true;
            }
            return false;
        }

        public static void ConsumeQuestsCompletedRequest()
        {
            FinishedQuestSet.Clear();
            string luaTable = Others.GetRandomString(Others.Random(4, 10));
            string luaResultStr = Others.GetRandomString(Others.Random(4, 10));
            string command = "";
            command += "local " + luaTable + " = GetQuestsCompleted() ";
            command += "if " + luaTable + " == nil then " + luaResultStr + " = \"NIL\" else ";
            command += luaResultStr + " = \"\" ";
            command += "for key,value in pairs(" + luaTable + ") do "; // value is "true" always
            command += luaResultStr + " = " + luaResultStr + " .. \"^\" .. key ";
            command += "end end";
            Lua.LuaDoString(command);
            string sResult = Lua.GetLocalizedText(luaResultStr);

            if (sResult == "NIL")
                return;
            foreach (string strQuestId in sResult.Split('^'))
            {
                if (strQuestId != string.Empty)
                    FinishedQuestSet.Add(Others.ToInt32(strQuestId));
            }
        }

        public static bool GetQuestCompleted(int questId)
        {
            if (questId == -1)
                return false;
            if (FinishedQuestSet.Contains(questId))
                return true;
            if (!IsQuestFlaggedCompletedLUA(questId)) return false;
            Logging.WriteDebug("GetQuestCompleted but not in FinishedQuestSet yet. qId: " + questId);
            FinishedQuestSet.Add(questId);
            return true;
        }

        public static void SelectGossipOption(int GossipOption)
        {
            Lua.LuaDoString("SelectGossipOption(" + GossipOption + ")");
        }

        public static void SelectGossipAvailableQuest(int GossipOption)
        {
            Lua.LuaDoString("SelectGossipAvailableQuest(" + GossipOption + ")");
        }

        public static void CloseWindow()
        {
            try
            {
                Memory.WowMemory.GameFrameLock();
                Lua.LuaDoString("CloseQuest()");
                Lua.LuaDoString("CloseGossip()");
                Lua.LuaDoString("CloseBankFrame()");
                Lua.LuaDoString("CloseMail()");
                Lua.LuaDoString("CloseMerchant()");
                Lua.LuaDoString("ClosePetStables()");
                Lua.LuaDoString("CloseTaxiMap()");
                Lua.LuaDoString("CloseTrainer()");
                Lua.LuaDoString("CloseAuctionHouse()");
                Lua.LuaDoString("CloseGuildBankFrame()");
                Lua.RunMacroText("/Click QuestFrameCloseButton");
                Lua.LuaDoString("ClearTarget()");
                Thread.Sleep(150);
            }
            catch (Exception e)
            {
                Logging.WriteError("public static void CloseWindow(): " + e);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public static void AcceptQuest()
        {
            Lua.LuaDoString("AcceptQuest()");
            if (Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
                Lua.RunMacroText("/click QuestFrameCompleteQuestButton");
            // hack for SelfComplete quests
        }

        public static int GetQuestID()
        {
            int questId = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.Quests.QuestId);
            if (questId > 0)
                return questId;
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetQuestID()");
            return Others.ToInt32(Lua.GetLocalizedText(randomString));
        }

        public static bool GetGossipAvailableQuestsWorks()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + ", _ = GetGossipAvailableQuests()");
            string result = Lua.GetLocalizedText(randomString);
            return (result != "");
        }

        public static bool GetGossipActiveQuestsWorks()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + ", _ = GetGossipActiveQuests()");
            string result = Lua.GetLocalizedText(randomString);
            return (result != "");
        }

        public static int GetNumGossipAvailableQuests()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumGossipAvailableQuests()");
            return Others.ToInt32(Lua.GetLocalizedText(randomString));
        }

        public static int GetNumGossipActiveQuests()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumGossipActiveQuests()");
            return Others.ToInt32(Lua.GetLocalizedText(randomString));
        }

        public static int GetNumGossipOptions()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumGossipOptions()");
            return Others.ToInt32(Lua.GetLocalizedText(randomString));
        }

        public static String GetAvailableTitle(int index)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetAvailableTitle(" + index + ")");
            return Lua.GetLocalizedText(randomString);
        }

        public static void SelectAvailableQuest(int index)
        {
            Lua.LuaDoString("SelectAvailableQuest(" + index + ")");
        }

        public static String GetActiveTitle(int index)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetActiveTitle(" + index + ")");
            return Lua.GetLocalizedText(randomString);
        }

        public static void SelectActiveQuest(int index)
        {
            Lua.LuaDoString("SelectActiveQuest(" + index + ")");
        }

        public static void SelectGossipActiveQuest(int index)
        {
            Lua.LuaDoString("SelectGossipActiveQuest(" + index + ")");
        }

        public static ItemInfo CompleteQuest()
        {
            Lua.LuaDoString("CompleteQuest()");
            Thread.Sleep(300);
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumQuestChoices()");
            int numRewards = Others.ToInt32(Lua.GetLocalizedText(randomString));
            Logging.WriteDebug("There is " + numRewards + " rewards");
            ItemInfo itemToEquip = null, tempItem;

            uint valuePriceChoice = 1;
            uint highestPriceValue = 0;
            uint valueEquipementChoice = 0;
#pragma warning disable 219
            uint highestEquipementValue = 0; // if 2 items are usefull, then we need to select one, but how?
#pragma warning restore 219
            for (uint i = 1; i <= numRewards; i++)
            {
                string sLink = Others.GetRandomString(Others.Random(4, 10));
                string command = sLink + " = GetQuestItemLink(\"choice\", " + i + "); ";
                Lua.LuaDoString(command);
                string link = Lua.GetLocalizedText(sLink);

                int value = ItemSelection.EvaluateItemStatsVsEquiped(link, out tempItem);
                if (value > 0)
                {
                    if (highestPriceValue < value)
                    {
                        highestPriceValue = (uint) value;
                        valuePriceChoice = i;
                    }
                }
                else
                {
                    valueEquipementChoice = i;
                    itemToEquip = tempItem;
                    break; // Let's stop at the first found for now, later we will give them a weight a select the best upgrade
                }
            }

            if (itemToEquip == null) // We have nothing to equip from rewards choice, let's take a look at reward with no choice
            {
                randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = GetNumQuestRewards()");
                numRewards = Others.ToInt32(Lua.GetLocalizedText(randomString));
                for (uint i = 1; i <= numRewards; i++)
                {
                    string sLink = Others.GetRandomString(Others.Random(4, 10));
                    string command = sLink + " = GetQuestItemLink(\"reward\", " + i + "); ";
                    Lua.LuaDoString(command);
                    string link = Lua.GetLocalizedText(sLink);
                    if (ItemSelection.EvaluateItemStatsVsEquiped(link, out tempItem) <= 0)
                    {
                        itemToEquip = tempItem;
                    }
                }
            }

            if (valueEquipementChoice > 0)
            {
                Logging.WriteDebug("Going to select reward " + valueEquipementChoice + " for item stats");
                Lua.LuaDoString("GetQuestReward(" + valueEquipementChoice + ")");
            }
            else
            {
                Logging.WriteDebug("Going to select reward " + valuePriceChoice + " for its money value");
                Lua.LuaDoString("GetQuestReward(" + valuePriceChoice + ")");
            }
            Thread.Sleep(500);
            return itemToEquip;
        }

        public static List<int> GetLogQuestId()
        {
            try
            {
                uint descriptorsArray =
                    Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                     Descriptors.StartDescriptors);
                uint addressQL = descriptorsArray + ((uint) Descriptors.PlayerFields.QuestLog*Descriptors.Multiplicator);

                var list = new List<int>();
                for (int index = 0; index < 50; ++index)
                {
                    var pointer = (uint) (addressQL + (Marshal.SizeOf(typeof (PlayerQuest))*index));
                    var playerQuest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject(pointer, typeof (PlayerQuest));
                    if (playerQuest.ID > 0)
                    {
                        list.Add(playerQuest.ID);
                    }
                }
                return list;
            }
            catch
            {
                return new List<int>();
            }
        }

        public static String GetLogQuestTitle(int questId)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString("questIndex = GetQuestLogIndexByID(" + questId + ");" +
                            randomString + ", _ = GetQuestLogTitle(questIndex);");
            string ret = Lua.GetLocalizedText(randomString);
            return ret;
        }

        public static bool RequestResetObjectives = false;

        public static void AbandonQuest(int questId, bool forceResetObjectives = false)
        {
            if (questId == 0)
                return;
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetQuestLogIndexByID(" + questId + ")");
            int index = Others.ToInt32(Lua.GetLocalizedText(randomString));
            if (index > 0)
            {
                Lua.LuaDoString("SelectQuestLogEntry(" + index + ") SetAbandonQuest() AbandonQuest()");
                Thread.Sleep(Usefuls.Latency + 500);
            }
            if (forceResetObjectives)
                RequestResetObjectives = true;
        }

        public static bool GetLogQuestIsComplete(int questId) // we could also use the PlayerQuest struct from memory
        {
            try
            {
                // title, level, suggestedGroup, isHeader, isCollapsed, isComplete, frequency, questID, startEvent, displayQuestID, isOnMap, hasLocalPOI, isTask, isStory = GetQuestLogTitle(questIndex)
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("questIndex = GetQuestLogIndexByID(" + questId + ");" +
                                "_, _, _, _, _, " + randomString + " = GetQuestLogTitle(questIndex);" +
                                randomString + " = tostring(" + randomString + ")");
                string ret = Lua.GetLocalizedText(randomString);
                return ret == "1"; // 1-nil was supposed to have been removed by patch-note...
            }
            catch
            {
                return false;
            }
        }

        public static void InteractTarget(ref Npc npc, uint baseAddress)
        {
            MovementManager.StopMove();
            WoWGameObject targetIsGameObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(npc.Entry), npc.Position);
            Interact.InteractWith(baseAddress);
            if (targetIsGameObject.IsValid)
            {
                Thread.Sleep(3100);
                return;
            }
            Thread.Sleep(Usefuls.Latency + 500);
            if (ObjectManager.ObjectManager.Target.GetBaseAddress == 0 || ObjectManager.ObjectManager.Target.GetBaseAddress != baseAddress)
            {
                Logging.WriteDebug("Using LUA to target " + npc.Name);
                Lua.LuaDoString("TargetUnit(\"" + npc.Name + "\")");
                Thread.Sleep(Usefuls.Latency + 500);
                Interact.InteractWith(ObjectManager.ObjectManager.Target.GetBaseAddress);
                Thread.Sleep(Usefuls.Latency + 500);
            }
        }

        public static void QuestPickUp(ref Npc npc, string questName, int questId, bool ignoreBlacklist = false, bool forceTravel = false)
        {
            bool cancelPickUp;
            QuestPickUp(ref npc, questName, questId, out cancelPickUp, ignoreBlacklist, forceTravel);
        }

        public static bool IsNearQuestGiver(Point p)
        {
            return ObjectManager.ObjectManager.Me.Position.DistanceTo(p) <= 5f;
        }

        public static void QuestPickUp(ref Npc npc, string questName, int questId, out bool cancelPickUp, bool ignoreBlacklist = false, bool forceTravel = false)
        {
            if (npc.ForceTravel)
                forceTravel = true;
            cancelPickUp = false;
            if (AbandonnedId == questId) // should happen only when we do a different quest requirement for optimization
            {
                AbandonnedId = 0;
                return;
            }
            if (AbandonnedId != 0)
            {
                AbandonQuest(AbandonnedId);
            }
            AbandonnedId = 0;
            Point me = ObjectManager.ObjectManager.Me.Position;
            WoWUnit mNpc = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry(npc.Entry, true), false, ignoreBlacklist, true);
            // We have the NPC in memory and he is closer than the QuesterDB entry. (some Npc moves)
            if (mNpc.IsValid && mNpc.HasQuests)
                npc.Position = mNpc.Position;
            WoWGameObject mObj = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(npc.Entry), ignoreBlacklist);
            if (mObj.IsValid && mObj.HasQuests)
                npc.Position = mObj.Position;
            bool bypassTravel = false;
            if (me.DistanceTo(npc.Position) <= 800f)
                PathFinder.FindPath(npc.Position, out bypassTravel);
            if (Usefuls.IsFlying && npc.ContinentIdInt == Usefuls.ContinentId)
                bypassTravel = true;
            else if (Usefuls.IsFlying)
                MountTask.DismountMount();
            if (!bypassTravel && (_travelLocation == null || _travelLocation.DistanceTo(me) > 0.1f) && !_travelDisabled)
            {
                MovementManager.StopMove();
                Logging.Write("Calling travel system for PickUpQuest " + questName + "(" + questId + ") from " + npc.Name + " (" + npc.Entry + ")...");
                Products.Products.TravelToContinentId = npc.ContinentIdInt;
                Products.Products.TravelTo = npc.Position;
                Products.Products.TravelFromContinentId = Usefuls.ContinentId;
                Products.Products.TravelFrom = ObjectManager.ObjectManager.Me.Position;
                Products.Products.ForceTravel = forceTravel;
                // Pass the check for valid destination as a lambda
                Products.Products.TargetValidationFct = IsNearQuestGiver;
                _travelLocation = me;
                return;
            }
            if (_travelLocation != null && _travelLocation.DistanceTo(me) <= 0.1f)
                _travelDisabled = true;
            //Start target finding based on QuestGiver.
            uint baseAddress = MovementManager.FindTarget(ref npc, 4.5f, true, true, 0f, ignoreBlacklist); // can pick up quest on dead NPC.
            if (MovementManager.InMovement)
                return;
            _travelDisabled = false; // reset travel
            if (baseAddress > 0)
            {
                var tmpNpc = ObjectManager.ObjectManager.GetObjectByGuid(npc.Guid);
                if (tmpNpc is WoWUnit)
                {
                    var unitTest = tmpNpc as WoWUnit;
                    if (unitTest.IsValid && unitTest.GetDistance < 20f && !unitTest.HasQuests)
                    {
                        _travelDisabled = false; // reset travel
                        nManagerSetting.AddBlackList(unitTest.Guid, 30000);
                        Logging.Write("Npc QuestGiver " + unitTest.Name + " (" + unitTest.Entry + ", distance: " + unitTest.GetDistance +
                                      ") does not have any available quest for the moment. Blacklisting it for 30 seconds.");
                        cancelPickUp = true;
                        return;
                    }
                }
            }
            //End target finding based on QuestGiver.
            if (npc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 5f)
            {
                if (baseAddress <= 0)
                {
                    cancelPickUp = true; // We are there but no NPC waiting for us.
                    // This code is there for Mimesis and I'm not currently working on Quester, so I'm not going to "return;" there as quester will be even more lost.
                }
                if (Usefuls.IsFlying)
                    MountTask.Land();
                InteractTarget(ref npc, baseAddress);
                Logging.Write("PickUpQuest " + questName + " (" + questId + ") from " + npc.Name + " (" + npc.Entry + ")");
                int id = GetQuestID();
                if (GetNumGossipAvailableQuests() == 0 && id == questId)
                {
                    AcceptQuest();
                    Thread.Sleep(Usefuls.Latency + 500);
                }
                if (GetLogQuestId().Contains(questId))
                {
                    CloseWindow();
                }
                else
                {
                    bool systemWorks = GetGossipAvailableQuestsWorks();
                    if (systemWorks) // 2 quest gossip systems = 2 different codes :(
                    {
                        for (int i = 1; i <= GetNumGossipAvailableQuests(); i++)
                        {
                            SelectGossipAvailableQuest(i);
                            Thread.Sleep(Usefuls.Latency + 500);
                            id = GetQuestID();
                            if (id == 0)
                            {
                                systemWorks = false;
                                break;
                            }
                            if (id == questId)
                            {
                                AcceptQuest();
                                Thread.Sleep(Usefuls.Latency + 500);
                                id = GetQuestID();
                                CloseWindow();
                                if (id != questId)
                                    AbandonQuest(id);
                                break;
                            }
                            CloseWindow();
                            Thread.Sleep(Usefuls.Latency + 500);
                            AbandonQuest(id);
                            Interact.InteractWith(baseAddress);
                            Thread.Sleep(Usefuls.Latency + 500);
                        }
                    }
                    if (!systemWorks)
                    {
                        int gossipid = 1;
                        while (GetAvailableTitle(gossipid) != "")
                        {
                            SelectAvailableQuest(gossipid);
                            Thread.Sleep(Usefuls.Latency + 500);
                            id = GetQuestID();
                            if (id == questId)
                            {
                                AcceptQuest();
                                Thread.Sleep(Usefuls.Latency + 500);
                                id = GetQuestID();
                                CloseWindow();
                                if (id != questId)
                                    AbandonQuest(id);
                                break;
                            }
                            CloseWindow();
                            Thread.Sleep(Usefuls.Latency + 500);
                            AbandonQuest(id);
                            Interact.InteractWith(baseAddress);
                            Thread.Sleep(Usefuls.Latency + 500);
                            gossipid++;
                        }
                    }
                }
                KilledMobsToCount.Clear();
                Thread.Sleep(Usefuls.Latency);
            }
            CloseWindow();
        }

        public static void QuestTurnIn(ref Npc npc, string questName, int questId, bool ignoreBlacklist = false, bool forceTravel = false)
        {
            if (npc.ForceTravel)
                forceTravel = true;
            Point me = ObjectManager.ObjectManager.Me.Position;
            WoWUnit mNpc = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry(npc.Entry, true), false, ignoreBlacklist, true);
            if (mNpc.IsValid && mNpc.CanTurnIn)
                npc.Position = mNpc.Position;
            WoWGameObject mObj = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(npc.Entry), ignoreBlacklist);
            if (mObj.IsValid && mObj.CanTurnIn)
                npc.Position = mObj.Position;
            bool bypassTravel = false;
            if (me.DistanceTo(npc.Position) <= 800f)
                PathFinder.FindPath(npc.Position, out bypassTravel);
            if (Usefuls.IsFlying && npc.ContinentIdInt == Usefuls.ContinentId)
                bypassTravel = true;
            else if (Usefuls.IsFlying)
                MountTask.DismountMount();
            if (!bypassTravel && (_travelLocation == null || _travelLocation.DistanceTo(me) > 0.1f) && !_travelDisabled)
            {
                MovementManager.StopMove();
                Logging.Write("Calling travel system for TurnInQuest " + questName + "(" + questId + ") from " + npc.Name + " (" + npc.Entry + ")...");
                Products.Products.TravelToContinentId = npc.ContinentIdInt;
                Products.Products.TravelTo = npc.Position;
                Products.Products.TravelFromContinentId = Usefuls.ContinentId;
                Products.Products.TravelFrom = ObjectManager.ObjectManager.Me.Position;
                Products.Products.ForceTravel = forceTravel;
                // Pass the check for valid destination as a lambda
                Products.Products.TargetValidationFct = IsNearQuestGiver;
                _travelLocation = me;
                return;
            }
            if (_travelLocation != null && _travelLocation.DistanceTo(me) <= 0.1f)
                _travelDisabled = true;
            //Start target finding based on QuestGiver.
            uint baseAddress = MovementManager.FindTarget(ref npc, 4.5f, true, true, 0f, ignoreBlacklist);
            if (MovementManager.InMovement)
                return;

            if (baseAddress > 0)
            {
                var tmpNpc = ObjectManager.ObjectManager.GetObjectByGuid(npc.Guid);
                if (tmpNpc is WoWUnit)
                {
                    var unitTest = tmpNpc as WoWUnit;
                    if (unitTest.IsValid && unitTest.GetDistance < 20f && !unitTest.CanTurnIn)
                    {
                        _travelDisabled = false; // reset travel
                        nManagerSetting.AddBlackList(unitTest.Guid, 30000);
                        Logging.Write("Npc QuestGiver " + unitTest.Name + " (" + unitTest.Entry + ", distance: " + unitTest.GetDistance +
                                      ") cannot TurnIn any quest right now. Blacklisting it for 30 seconds.");
                        return;
                    }
                }
            }
            _travelDisabled = false; // reset travel
            ItemInfo equip = null;
            //End target finding based on QuestGiver.
            if (npc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 5f)
            {
                if (Usefuls.IsFlying)
                    MountTask.Land();
                InteractTarget(ref npc, baseAddress);
                Logging.Write("QuestTurnIn " + questName + " (" + questId + ") to " + npc.Name + " (" + npc.Entry + ")");
                int id = GetQuestID();
                if (GetNumGossipActiveQuests() == 0 && id == questId)
                {
                    if (Others.IsFrameVisible("QuestFrameCompleteButton") && !Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
                    {
                        Lua.RunMacroText("/click QuestFrameCompleteButton");
                        Thread.Sleep(300);
                    }
                    equip = CompleteQuest();
                    Thread.Sleep(Usefuls.Latency + 500);
                }
                if (!GetLogQuestId().Contains(questId)) // It's no more in the quest log, then we did turn in it sucessfuly
                {
                    id = GetQuestID();
                    FinishedQuestSet.Add(questId);
                    CloseWindow();
                    AbandonnedId = id;
                }
                else
                {
                    bool systemWorks = GetGossipActiveQuestsWorks();
                    if (systemWorks) // 2 quest gossip systems = 2 different codes :(
                    {
                        for (int i = 1; i <= GetNumGossipActiveQuests(); i++)
                        {
                            SelectGossipActiveQuest(i);
                            Thread.Sleep(Usefuls.Latency + 500);
                            id = GetQuestID();
                            if (id == 0)
                            {
                                systemWorks = false;
                                break;
                            }
                            if (id == questId)
                            {
                                if (Others.IsFrameVisible("QuestFrameCompleteButton") && !Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
                                {
                                    Lua.RunMacroText("/click QuestFrameCompleteButton");
                                    Thread.Sleep(300);
                                }
                                equip = CompleteQuest();
                                Thread.Sleep(Usefuls.Latency + 500);
                                // here it can be the next quest id presented automatically when the current one is turned in
                                id = GetQuestID();
                                CloseWindow();
                                if (GetLogQuestId().Contains(questId))
                                {
                                    equip = null;
                                    Logging.WriteError("Could not turn-in quest " + questId + ": \"" + questName + "\"");
                                    break;
                                }
                                FinishedQuestSet.Add(questId);
                                AbandonnedId = id;
                                break;
                            }
                            CloseWindow();
                            Thread.Sleep(Usefuls.Latency + 500);
                            Interact.InteractWith(baseAddress);
                            Thread.Sleep(Usefuls.Latency + 500);
                        }
                    }
                    if (!systemWorks)
                    {
                        int gossipid = 1;
                        while (GetActiveTitle(gossipid) != "")
                        {
                            SelectActiveQuest(gossipid);
                            Thread.Sleep(Usefuls.Latency + 500);
                            id = GetQuestID();
                            if (id == questId)
                            {
                                if (Others.IsFrameVisible("QuestFrameCompleteButton") && !Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
                                {
                                    Lua.RunMacroText("/click QuestFrameCompleteButton");
                                    Thread.Sleep(300);
                                }
                                equip = CompleteQuest();
                                Thread.Sleep(Usefuls.Latency + 500);
                                CloseWindow();
                                if (GetLogQuestId().Contains(questId))
                                {
                                    equip = null;
                                    Logging.WriteError("Could not turn-in quest " + questId + ": \"" + questName + "\"");
                                    break;
                                }
                                FinishedQuestSet.Add(questId);
                                break;
                            }
                            CloseWindow();
                            Thread.Sleep(Usefuls.Latency + 500);
                            Interact.InteractWith(baseAddress);
                            Thread.Sleep(Usefuls.Latency + 500);
                            gossipid++;
                        }
                    }
                }
            }
            Thread.Sleep(Usefuls.Latency);
            if (equip != null)
            {
                ItemSelection.EquipItem(equip);
                Thread.Sleep(Usefuls.Latency + 500);
            }
            CloseWindow();
            if (!GetLogQuestId().Contains(questId))
            {
                FinishedQuestSet.Add(questId);
            }
        }

        public static void DumpInternalIndexForQuestId(int questId)
        {
            Logging.Write("Dumping current InternalIndexes for QuestId: " + questId);
            uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors);
            uint addressQL = descriptorsArray + ((uint) Descriptors.PlayerFields.QuestLog*Descriptors.Multiplicator);
            for (int index = 0; index < 50; ++index)
            {
                var playerQuest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject((uint) (addressQL + (Marshal.SizeOf(typeof (PlayerQuest))*index)), typeof (PlayerQuest));
                if (playerQuest.ID == questId)
                {
                    for (int i = 0; i <= playerQuest.ObjectiveRequiredCounts.Length - 1; i++)
                    {
                        if (playerQuest.ObjectiveRequiredCounts[i] > 0)
                            Logging.Write("InternalIndex: " + (i + 1) + ", Count: " + playerQuest.ObjectiveRequiredCounts[i]);
                    }
                }
            }
        }

        public static bool IsObjectiveCompleted(int questId, uint ObjectiveInternalIndex, int count)
        {
            uint descriptorsArray =
                Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                 Descriptors.StartDescriptors);
            uint addressQL = descriptorsArray + ((uint) Descriptors.PlayerFields.QuestLog*Descriptors.Multiplicator);
            for (int index = 0; index < 50; ++index)
            {
                var playerQuest =
                    (PlayerQuest)
                        Memory.WowMemory.Memory.ReadObject(
                            (uint) (addressQL + (Marshal.SizeOf(typeof (PlayerQuest))*index)),
                            typeof (PlayerQuest));
                if (playerQuest.ID == questId)
                    return playerQuest.ObjectiveRequiredCounts[ObjectiveInternalIndex - 1] >= count;
            }
            return false;
        }

        public static int GetCurrentInternalIndexCount(int questId, uint ObjectiveInternalIndex)
        {
            uint descriptorsArray =
                Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                 Descriptors.StartDescriptors);
            uint addressQL = descriptorsArray + ((uint) Descriptors.PlayerFields.QuestLog*Descriptors.Multiplicator);
            for (int index = 0; index < 50; ++index)
            {
                var playerQuest =
                    (PlayerQuest)
                        Memory.WowMemory.Memory.ReadObject(
                            (uint) (addressQL + (Marshal.SizeOf(typeof (PlayerQuest))*index)),
                            typeof (PlayerQuest));
                if (playerQuest.ID == questId)
                    return playerQuest.ObjectiveRequiredCounts[ObjectiveInternalIndex - 1];
            }
            return 0;
        }

        public static bool IsQuestFailed(int questId)
        {
            uint descriptorsArray =
                Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                 Descriptors.StartDescriptors);
            uint addressQL = descriptorsArray + ((uint) Descriptors.PlayerFields.QuestLog*Descriptors.Multiplicator);
            for (int index = 0; index < 50; ++index)
            {
                var playerQuest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject((uint) (addressQL + (Marshal.SizeOf(typeof (PlayerQuest))*index)), typeof (PlayerQuest));
                if (playerQuest.ID == questId)
                {
                    return playerQuest.State == PlayerQuest.StateFlag.Failed;
                }
            }
            return false;
        }

        private static object _threadLock = new object();

        public static void AutoCompleteQuest()
        {
            lock (_threadLock)
            {
                for (int i = 1; i < 20; i++)
                {
                    string questIdRet = Others.GetRandomString(Others.Random(4, 10));
                    string questStatusRet = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(questIdRet + ", " + questStatusRet + " = GetAutoQuestPopUp(" + i + ");");
                    string questStatus = Lua.GetLocalizedText(questStatusRet);
                    int questId = Others.ToInt32(Lua.GetLocalizedText(questIdRet));
                    if (questId == 0 && string.IsNullOrEmpty(questStatus) && i > 10)
                        return;
                    if (questId == 0 && string.IsNullOrEmpty(questStatus))
                        continue;
                    if (questStatus == "COMPLETE")
                    {
                        if (!GetLogQuestId().Contains(questId))
                            continue;
                        string questLogEntry = Others.GetRandomString(Others.Random(4, 10));
                        string luaString = questLogEntry + " = GetQuestLogIndexByID(" + questId + "); ";
                        luaString += "ShowQuestComplete(" + questLogEntry + ");";
                        Lua.LuaDoString(luaString);
                        Thread.Sleep(300);
                        if (Others.IsFrameVisible("QuestFrameCompleteButton") && !Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
                        {
                            Lua.RunMacroText("/click QuestFrameCompleteButton");
                            Thread.Sleep(300);
                        }
                        CompleteQuest();
                        Thread.Sleep(500);
                        if (!GetLogQuestId().Contains(questId)) // It's no more in the quest log, then we did turn in it sucessfuly
                        {
                            FinishedQuestSet.Add(questId);
                        }
                    }
                    else if (questStatus == "OFFER")
                    {
                        if (GetLogQuestId().Contains(questId) || GetLogQuestIsComplete(questId) || IsQuestFlaggedCompletedLUA(questId))
                            continue;
                        string questLogEntry = Others.GetRandomString(Others.Random(4, 10));
                        string luaString = questLogEntry + " = GetQuestLogIndexByID(" + questId + "); ";
                        luaString += "ShowQuestOffer(" + questLogEntry + ");";
                        Lua.LuaDoString(luaString);
                        Thread.Sleep(300);
                        if (Others.IsFrameVisible("QuestFrameAcceptButton") && !Others.IsFrameVisible("QuestFrameAcceptQuestButton"))
                        {
                            Lua.RunMacroText("/click QuestFrameAcceptButton");
                            Thread.Sleep(300);
                        }
                        AcceptQuest();
                        Thread.Sleep(500);
                    }
                }
            }
        }

        public static bool IsQuestFlaggedCompletedLUA(int internalQuestId)
        {
            if (internalQuestId <= 0)
                return true;
            string result = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(result + " = tostring(IsQuestFlaggedCompleted(" + internalQuestId + "))");
            return Lua.GetLocalizedText(result) == "true";
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PlayerQuest
        {
            public int ID;
            public StateFlag State;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)] public short[] ObjectiveRequiredCounts;
            public int Time;

            public enum StateFlag : uint
            {
                None = 0,
                Complete = 1,
                Failed = 2,
            }
        }

        public static bool TravelToQuestZone(Point destination, ref bool travelToQuestZone, int continentId = -1, bool forceTravel = false, string reason = "TravelToQuestZone")
        {
            bool doTravel = forceTravel;
            if (continentId == -1)
                continentId = Usefuls.ContinentId;
            if (continentId == Usefuls.ContinentId && Usefuls.IsFlying && !forceTravel)
                return false;
            if (continentId == Usefuls.ContinentId && Usefuls.IsFlying && forceTravel)
            {
                MountTask.DismountMount();
                // land before generating paths...
            }
            if (continentId != Usefuls.ContinentId)
                doTravel = true;
            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(destination) > 800f)
                doTravel = true;
            else if (!doTravel)
            {
                // We are closer, check for a valid path.
                PathFinder.FindPath(destination, out doTravel);
                doTravel = !doTravel; // reverse result
            }
            if (!travelToQuestZone && !forceTravel)
                doTravel = false;
            if (doTravel && (_travelLocation == null || _travelLocation.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 0.1f) && !_travelDisabled)
            {
                MovementManager.StopMove();
                Logging.Write("Calling travel system for " + reason + "...");
                travelToQuestZone = false;
                Products.Products.TravelToContinentId = continentId;
                Products.Products.TravelTo = destination;
                Products.Products.TravelFromContinentId = Usefuls.ContinentId;
                Products.Products.TravelFrom = ObjectManager.ObjectManager.Me.Position;
                Products.Products.ForceTravel = forceTravel;
                // Pass the check for valid destination as a lambda
                Products.Products.TargetValidationFct = Quest.IsNearQuestGiver; // compare me.Pos to dest.Pos
                _travelLocation = ObjectManager.ObjectManager.Me.Position;
                return true;
            }
            if (_travelLocation != null && _travelLocation.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 0.1f)
                _travelDisabled = true; // release travel once arrived.
            return false;
        }
    }
}