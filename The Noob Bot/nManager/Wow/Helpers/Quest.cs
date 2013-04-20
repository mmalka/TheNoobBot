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

        public static bool GetQuestCompleted(List<int> qList)
        {
            foreach (int q in qList)
            {
                if (GetQuestCompleted(q))
                    return true;
            }
            return false;
        }

        public static void RequestQuestsCompleted()
        {
            Lua.LuaDoString("QueryQuestsCompleted()");
        }

        public static void ConsumeQuestsCompletedRequest()
        {
            string luaTable = Others.GetRandomString(Others.Random(4, 10));
            string luaResultStr = Others.GetRandomString(Others.Random(4, 10));
            string command = "";
            command += "local " + luaTable + " = GetQuestsCompleted() ";
            command += luaResultStr + " = \"\" ";
            command += "for key,value in pairs(" + luaTable + ") do "; // value is "true" always
            command += luaResultStr + " = " + luaResultStr + " .. \"^\" .. key ";
            command += "end";
            Lua.LuaDoString(command);
            string sResult = Lua.GetLocalizedText(luaResultStr);

            foreach (string toto in sResult.Split(Convert.ToChar("^")))
            {
                if (toto != string.Empty)
                    FinishedQuestSet.Add(Convert.ToInt32(toto));
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
            Lua.RunMacroText("/script CloseQuest()");
            Thread.Sleep(150);
        }

        public static void AcceptQuest()
        {
            Lua.RunMacroText("/script AcceptQuest() ");
        }

        /*public static List<GossipQuest> GetAllAvailableQuestIdInGossip()
        {
            List<GossipQuest> result = new List<GossipQuest>();

            //uint availableQuests = ObjectManager.ObjectManager.Me.GetBaseAddress + (uint)Addresses.Quests.GossipQuests;
            uint availableQuests = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.Quests.GossipQuests);
            //uint availableQuests = Memory.WowProcess.WowModule + (uint)Addresses.Quests.GossipQuests;
            for (int i = 0; i < 40; i++)
            {
                int questId = Memory.WowMemory.Memory.ReadInt(availableQuests);
                if (questId == 0)
                    continue;
                GossipQuest one = new GossipQuest();
                one.ID = questId;
                one.Title = Memory.WowMemory.Memory.ReadUTF8String(availableQuests + (uint)Addresses.Quests.TitleText);
                result.Add(one);
                availableQuests += (uint)Addresses.Quests.GossipQuestNext;
            }
            return result;
        }*/

        public static int GetQuestID()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetQuestID()");
            return Convert.ToInt32(Lua.GetLocalizedText(randomString));
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

        /*public static List<String> GetAllTitleOfAvailableQuestsInGossip()
        {
            List<String> result = new List<String>();
            const string separator = "^";

            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString("numNewQuests = GetNumGossipAvailableQuests();" +
                randomString + " = ''; " +
                "_ = { GetGossipAvailableQuests() }; " +
                "index = 0; " +
                "while (index < numNewQuests) do" +
                    randomString + " = " + randomString + " .. _[(1+(index*5))]; .. '" + separator + "';" +
                "end");
            string resultLua = Lua.GetLocalizedText(randomString);
            if (resultLua.Replace(" ", "").Length > 0)
            {
                string[] sQuestTitles = resultLua.Split(Convert.ToChar(separator));
                foreach (string s in sQuestTitles)
                {
                    if (s.Replace(" ", "").Length > 0)
                    {
                        result.Add(s);
                    }
                }
            }
            return result;
        }*/

        /*public static int GetNumGossipAvailableQuests()
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = GetNumGossipAvailableQuests();");
                string titi = Lua.GetLocalizedText(randomString);
                int res = Convert.ToInt32(titi);
                return res;
            }
            catch
            {
                return 0;
            }
        }*/

        /*public static int GetNumGossipActiveQuests()
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = GetNumGossipActiveQuests();");
                return Convert.ToInt32(Lua.GetLocalizedText(randomString));
            }
            catch
            {
                return 0;
            }
        }*/

        public static void SelectGossipActiveQuest(int GossipOption)
        {
            Lua.LuaDoString("SelectGossipActiveQuest(" + GossipOption + ")");
        }

        public static void CompleteQuest()
        {
            Lua.RunMacroText("/script CompleteQuest()");
            Thread.Sleep(500);
            Lua.RunMacroText("/script GetQuestReward(1)"); // or /script SelectGossipOption(1)?
            Thread.Sleep(500);
            //Lua.RunMacroText("/script AcceptQuest() ");
        }

        public static List<int> GetLogQuestId()
        {
            try
            {
                uint descriptorsArray =
                    Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                     Descriptors.startDescriptors);
                uint addressQL = descriptorsArray + ((uint) Descriptors.PlayerFields.QuestLog*Descriptors.multiplicator);

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

        public static void AbandonLastQuest()
        {
            Lua.LuaDoString("SetAbandonQuest() AbandonQuest()");
        }

        public static bool GetLogQuestIsComplete(int questId) // we could also use the PlayerQuest struct from memory
        {
            try
            { // title, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily, questID, startEvent, displayQuestID = GetQuestLogTitle(questIndex)
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

        /*public struct GossipQuest
        {
            public int ID;
            public String Title;
        }*/
    }
}