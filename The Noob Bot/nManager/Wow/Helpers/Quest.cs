using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Patchables;
using System.Runtime.InteropServices;

namespace nManager.Wow.Helpers
{
    public class Quest
    {
        public static List<int> FinishedQuestSet = new List<int>();
        public static List<int> KilledMobsToCount = new List<int>();

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
            foreach (string strQuestId in sResult.Split(Convert.ToChar("^")))
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
            Lua.LuaDoString("AcceptQuest() ");
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

        public static void CompleteQuest()
        {
            Lua.LuaDoString("CompleteQuest()");
            Thread.Sleep(500);
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumQuestChoices()");
            int numRewards = Others.ToInt32(Lua.GetLocalizedText(randomString));
            Logging.WriteDebug("There is " + numRewards + " rewards");
            for (int i = 1; i <= numRewards; i++)
            {
                string sName = Others.GetRandomString(Others.Random(4, 10));
                string sQuality = Others.GetRandomString(Others.Random(4, 10));
                string sIlevel = Others.GetRandomString(Others.Random(4, 10));
                string sEquipSlot = Others.GetRandomString(Others.Random(4, 10));
                string sPrice = Others.GetRandomString(Others.Random(4, 10));

                string command = "link = GetQuestItemLink(\"choice\", " + i + "); ";
                command += sName + ",_," + sQuality + "," + sIlevel + ",_,_,_,_," + sEquipSlot + ",_," + sPrice + "=GetItemInfo(link);";
                //command += sPrice + " = link and select(11, GetItemInfo(link));";
                Lua.LuaDoString(command);
                int price = Others.ToInt32(Lua.GetLocalizedText(sPrice));
                int quality = Others.ToInt32(Lua.GetLocalizedText(sQuality));
                int iLevel = Others.ToInt32(Lua.GetLocalizedText(sIlevel));
                Logging.WriteDebug("Item \"" + Lua.GetLocalizedText(sName) + "\" equip \"" + Lua.GetLocalizedText(sEquipSlot) + "\" has a value of " + price);
            }
            Lua.LuaDoString("GetQuestReward(1)");
            Thread.Sleep(500);
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
                for (int index = 0; index < 50; ++index)
                {
                    Quest.PlayerQuest playerQuest =
                        (Quest.PlayerQuest)
                        Memory.WowMemory.Memory.ReadObject(
                            (uint) (addressQL + (Marshal.SizeOf(typeof (Quest.PlayerQuest))*index)),
                            typeof (Quest.PlayerQuest));
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
                // title, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily, questID, startEvent, displayQuestID = GetQuestLogTitle(questIndex)
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("questIndex = GetQuestLogIndexByID(" + questId + ");" +
                                "_, _, _, _, _, _, " + randomString + " = GetQuestLogTitle(questIndex);");
                string ret = Lua.GetLocalizedText(randomString);
                return ret == "1";
            }
            catch
            {
                return false;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PlayerQuest
        {
            public int ID;
            public Quest.PlayerQuest.StateFlag State;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public short[] ObjectiveRequiredCounts;
            public int Time;

            public enum StateFlag : uint
            {
                None = 0,
                Complete = 1,
                Failed = 2,
            }
        }
    }
}