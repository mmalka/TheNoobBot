using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Patchables;
using nManager.Wow.Class;
using System.Runtime.InteropServices;
using Enums = nManager.Wow.Enums;

namespace nManager.Wow.Helpers
{
    public class Quest
    {
        public static List<int> FinishedQuestSet = new List<int>();
        public static List<int> KilledMobsToCount = new List<int>();
        public static int AbandonnedId = 0;

        static Quest()
        {
            GetSetIgnoreFight = false;
        }

        public static bool GetSetIgnoreFight { get; set; }

        public static bool GetQuestCompleted(List<int> qList)
        {
            foreach (int q in qList)
            {
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
            return FinishedQuestSet.Contains(questId);
        }

        public static void SelectGossipOption(int GossipOption)
        {
            Lua.LuaDoString("SelectGossipOption(" + GossipOption + ")");
        }

        public static void SelectGossipAvailableQuest(int GossipOption)
        {
            Lua.LuaDoString("SelectGossipAvailableQuest(" + GossipOption + ")");
        }

        public static void CloseQuestWindow()
        {
            Lua.LuaDoString("CloseQuest()");
            Lua.LuaDoString("CloseGossip()");
            Thread.Sleep(150);
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

                List<int> list = new List<int>();
                for (int index = 0; index < 25; ++index)
                {
                    uint pointer = (uint) (addressQL + (Marshal.SizeOf(typeof (PlayerQuest))*index));
                    PlayerQuest playerQuest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject(pointer, typeof (PlayerQuest));
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

        public static void AbandonQuest(int questId)
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
            ObjectManager.WoWGameObject targetIsGameObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(npc.Entry), npc.Position);
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
            }
        }

        public static void QuestPickUp(ref Npc npc, string questName, int questId)
        {
            if (AbandonnedId == questId) // should happen only when we do a different quest requirement for optimization
            {
                AbandonnedId = 0;
                return;
            }
            else if (AbandonnedId != 0)
            {
                AbandonQuest(AbandonnedId);
            }
            AbandonnedId = 0;
            //Start target finding based on QuestGiver.
            uint baseAddress = MovementManager.FindTarget(ref npc, 5.0f, true, true); // can pick up quest on dead NPC.
            if (MovementManager.InMovement)
                return;
            //End target finding based on QuestGiver.
            if (npc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 6)
            {
                InteractTarget(ref npc, baseAddress);
                Logging.Write("PickUp Quest " + questName + " id: " + questId);
                int id = Quest.GetQuestID();
                if (Quest.GetNumGossipAvailableQuests() == 0 && id == questId)
                {
                    Quest.AcceptQuest();
                    Thread.Sleep(Usefuls.Latency + 500);
                }
                if (Quest.GetLogQuestId().Contains(questId))
                {
                    Quest.CloseQuestWindow();
                }
                else
                {
                    bool systemWorks = Quest.GetGossipAvailableQuestsWorks();
                    if (systemWorks) // 2 quest gossip systems = 2 different codes :(
                    {
                        for (int i = 1; i <= Quest.GetNumGossipAvailableQuests(); i++)
                        {
                            Quest.SelectGossipAvailableQuest(i);
                            Thread.Sleep(Usefuls.Latency + 500);
                            id = Quest.GetQuestID();
                            if (id == 0)
                            {
                                systemWorks = false;
                                break;
                            }
                            if (id == questId)
                            {
                                Quest.AcceptQuest();
                                Thread.Sleep(Usefuls.Latency + 500);
                                id = Quest.GetQuestID();
                                Quest.CloseQuestWindow();
                                if (id != questId)
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
                    if (!systemWorks)
                    {
                        int gossipid = 1;
                        while (Quest.GetAvailableTitle(gossipid) != "")
                        {
                            Quest.SelectAvailableQuest(gossipid);
                            Thread.Sleep(Usefuls.Latency + 500);
                            id = Quest.GetQuestID();
                            if (id == questId)
                            {
                                Quest.AcceptQuest();
                                Thread.Sleep(Usefuls.Latency + 500);
                                id = Quest.GetQuestID();
                                Quest.CloseQuestWindow();
                                if (id != questId)
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
                Thread.Sleep(Usefuls.Latency);
            }
            Lua.LuaDoString("ClearTarget()");
        }

        public static void QuestTurnIn(ref Npc npc, string questName, int questId)
        {
            //Start target finding based on QuestGiver.
            uint baseAddress = MovementManager.FindTarget(ref npc, 5.0f);
            if (MovementManager.InMovement)
                return;
            ItemInfo equip = null;
            //End target finding based on QuestGiver.
            if (npc.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 6)
            {
                InteractTarget(ref npc, baseAddress);
                Logging.Write("turnIn Quest " + questName + " id: " + questId);
                int id = Quest.GetQuestID();
                if (Quest.GetNumGossipActiveQuests() == 0 && id == questId)
                {
                    equip = Quest.CompleteQuest();
                    Thread.Sleep(Usefuls.Latency + 500);
                }
                if (!Quest.GetLogQuestId().Contains(questId)) // It's no more in the quest log, then we did turn in it sucessfuly
                {
                    id = Quest.GetQuestID();
                    Quest.FinishedQuestSet.Add(questId);
                    Quest.CloseQuestWindow();
                    AbandonnedId = id;
                }
                else
                {
                    bool systemWorks = Quest.GetGossipActiveQuestsWorks();
                    if (systemWorks) // 2 quest gossip systems = 2 different codes :(
                    {
                        for (int i = 1; i <= Quest.GetNumGossipActiveQuests(); i++)
                        {
                            Quest.SelectGossipActiveQuest(i);
                            Thread.Sleep(Usefuls.Latency + 500);
                            id = Quest.GetQuestID();
                            if (id == 0)
                            {
                                systemWorks = false;
                                break;
                            }
                            if (id == questId)
                            {
                                equip = Quest.CompleteQuest();
                                Thread.Sleep(Usefuls.Latency + 500);
                                // here it can be the next quest id presented automatically when the current one is turned in
                                id = Quest.GetQuestID();
                                Quest.CloseQuestWindow();
                                if (Quest.GetLogQuestId().Contains(questId))
                                {
                                    equip = null;
                                    Logging.WriteError("Could not turn-in quest " + questId + ": \"" + questName + "\"");
                                    break;
                                }
                                Quest.FinishedQuestSet.Add(questId);
                                AbandonnedId = id;
                                break;
                            }
                            Quest.CloseQuestWindow();
                            Thread.Sleep(Usefuls.Latency + 500);
                            Interact.InteractWith(baseAddress);
                            Thread.Sleep(Usefuls.Latency + 500);
                        }
                    }
                    if (!systemWorks)
                    {
                        int gossipid = 1;
                        while (Quest.GetActiveTitle(gossipid) != "")
                        {
                            Quest.SelectActiveQuest(gossipid);
                            Thread.Sleep(Usefuls.Latency + 500);
                            id = Quest.GetQuestID();
                            if (id == questId)
                            {
                                equip = Quest.CompleteQuest();
                                Thread.Sleep(Usefuls.Latency + 500);
                                Quest.CloseQuestWindow();
                                if (Quest.GetLogQuestId().Contains(questId))
                                {
                                    equip = null;
                                    Logging.WriteError("Could not turn-in quest " + questId + ": \"" + questName + "\"");
                                    break;
                                }
                                Quest.FinishedQuestSet.Add(questId);
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
            if (equip != null)
            {
                ItemSelection.EquipItem(equip);
                Thread.Sleep(Usefuls.Latency + 500);
            }
            Lua.LuaDoString("ClearTarget()");
        }

        public static bool IsObjectiveCompleted(int questId, uint ObjectiveInternalIndex, int count)
        {
            uint descriptorsArray =
                Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                 Descriptors.StartDescriptors);
            uint addressQL = descriptorsArray + ((uint) Descriptors.PlayerFields.QuestLog*Descriptors.Multiplicator);
            for (int index = 0; index < 25; ++index)
            {
                PlayerQuest playerQuest =
                    (PlayerQuest)
                        Memory.WowMemory.Memory.ReadObject(
                            (uint) (addressQL + (Marshal.SizeOf(typeof (PlayerQuest))*index)),
                            typeof (PlayerQuest));
                if (playerQuest.ID == questId)
                    return playerQuest.ObjectiveRequiredCounts[ObjectiveInternalIndex - 1] >= count;
            }
            return false;
        }

        public static void AutoCompleteQuest(List<int> autoComplete)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumAutoQuestPopUps();");
            int num = Others.ToInt32(Lua.GetLocalizedText(randomString));
            for (int i = 1; i <= num; i++)
            {
                string questIdRet = Others.GetRandomString(Others.Random(4, 10));
                string questStatusRet = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(questIdRet + ", " + questStatusRet + " = GetAutoQuestPopUp(" + i + ");");
                string questStatus = Lua.GetLocalizedText(questStatusRet);
                if (questStatus == "COMPLETE")
                {
                    int questId = Others.ToInt32(Lua.GetLocalizedText(questIdRet));
                    if (autoComplete.Contains(questId))
                    {
                        string questLogEntry = Others.GetRandomString(Others.Random(4, 10));
                        string luaString = questLogEntry + " = GetQuestLogIndexByID(" + questId + ") ";
                        luaString += "ShowQuestComplete(" + questLogEntry + ")";
                        Lua.LuaDoString(luaString);
                        CompleteQuest();
                        Thread.Sleep(500);
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PlayerQuest
        {
            public int ID;
            public StateFlag State;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)] public short[] ObjectiveRequiredCounts;
            public int Time;

            public enum StateFlag : uint
            {
                None = 0,
                Complete = 1,
                Failed = 2,
            }
        }

        public static bool IsQuestFlaggedCompletedLUA(int internalQuestId)
        {
            string result = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(result + " = tostring(IsQuestFlaggedCompleted(" + internalQuestId + ")");
            return Lua.GetLocalizedText(result) == "true";
        }
    }
}