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

        public static void CloseQuestWindow() // ToDo: Fix
        {
            Lua.RunMacroText("/click SpellbookMicroButton");
            Thread.Sleep(150);
            Lua.RunMacroText("/click SpellbookMicroButton");
        }

        public static void AcceptQuest()
        {
            Lua.RunMacroText("/script AcceptQuest() ");
        }

        public static int GetNumGossipAvailableQuests()
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = GetNumGossipAvailableQuests()");
                int res = Convert.ToInt32(Lua.GetLocalizedText(randomString));
                return res;
            }
            catch
            {
                return 0;
            }
        }

        public static int GetNumGossipActiveQuests()
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = GetNumGossipActiveQuests()");
                return Convert.ToInt32(Lua.GetLocalizedText(randomString));
            }
            catch
            {
                return 0;
            }
        }

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
            Lua.RunMacroText("/script AcceptQuest() ");
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
                        Console.WriteLine("q: " + playerQuest.ID);
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

        public static void AbandonLastQuest()
        {
            Lua.LuaDoString("SetAbandonQuest() AbandonQuest()");
        }

        public static bool GetLogQuestIsComplete(int questId)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                //Lua.LuaDoString("SelectQuestLogEntry(" + questId + "); " + randomString + " = IsQuestCompletable();"); // ToDo: SelectQuestLogEntry probably wrong
                Lua.LuaDoString("_, _, _, " + randomString + " = GetGossipActiveQuests()");
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