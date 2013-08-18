using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Patchables;
using System.Runtime.InteropServices;
using Enums = nManager.Wow.Enums;

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

            uint valuePriceChoice = 1;
            uint highestPriceValue = 0;
            uint valueEquipementChoice = 0;
#pragma warning disable 219
            uint highestEquipementValue = 0; // if 2 items are usefull, then we need to select one, but how?
#pragma warning restore 219

            for (uint i = 1; i <= numRewards; i++)
            {
                string sName = Others.GetRandomString(Others.Random(4, 10));
                string sQuality = Others.GetRandomString(Others.Random(4, 10));
                string sIlevel = Others.GetRandomString(Others.Random(4, 10));
                string sInventorySlot = Others.GetRandomString(Others.Random(4, 10));
                string sPrice = Others.GetRandomString(Others.Random(4, 10));
                string sClass = Others.GetRandomString(Others.Random(4, 10));
                string sSubClass = Others.GetRandomString(Others.Random(4, 10));

                string command = "link = GetQuestItemLink(\"choice\", " + i + "); ";
                command += sName + ",_," + sQuality + "," + sIlevel + ",_," + sClass + "," + sSubClass + ",_," + sInventorySlot + ",_," + sPrice + "=GetItemInfo(link);";

                Lua.LuaDoString(command);
                uint price = Others.ToUInt32(Lua.GetLocalizedText(sPrice));
                if (price > highestPriceValue)
                {
                    highestPriceValue = price;
                    valuePriceChoice = i;
                }
#pragma warning disable 168
                int quality = Others.ToInt32(Lua.GetLocalizedText(sQuality));
                int iLevel = Others.ToInt32(Lua.GetLocalizedText(sIlevel));
#pragma warning restore 168
                string locClass = Lua.GetLocalizedText(sClass);
                string locSubClass = Lua.GetLocalizedText(sSubClass);

                // Now get the class and subclass of the item by searching in DBCs with localized names
                WoWItemClass TheItemClassRec = WoWItemClass.FromName(locClass);
                uint classId = TheItemClassRec.Record.ClassId;
                WoWItemSubClass TheItemSubClassRec = WoWItemSubClass.FromNameAndClass(locSubClass, classId);
                uint subClassId = TheItemSubClassRec.Record.SubClassId;
                string strInvSlot = Lua.GetLocalizedText(sInventorySlot);

                Logging.WriteDebug("Item \"" + Lua.GetLocalizedText(sName) + "\" equip \"" + strInvSlot + "\" class " + (Enums.WoWItemClass)classId + " subclass " + subClassId + " has a value of " + price);
                if ((Enums.WoWItemClass)classId == Enums.WoWItemClass.Armor)
                    if ((Enums.WowItemSubClassArmor)subClassId ==  EquipmentAndStats.InternalEquipableArmorItemType
                        || strInvSlot == "INVTYPE_CLOAK" || strInvSlot == "INVTYPE_NECK"
                        || strInvSlot == "INVTYPE_FINGER" || strInvSlot == "INVTYPE_TRINKET"
                        || (strInvSlot == "INVTYPE_SHIELD" && EquipmentAndStats.HasShield))
                        if (CheckItemStats(i))
                            valueEquipementChoice = i;
                if ((Enums.WoWItemClass)classId == Enums.WoWItemClass.Weapon)
                {
                    if (EquipmentAndStats.EquipableWeapons.Contains((Enums.WowItemSubClassWeapon)subClassId))
                        if (CheckItemStats(i))
                            valueEquipementChoice = i;
                }
                // FUTUR in case we can select more than 1 item reward, then we need to compare to what we have
                // FUTUR and maybe also equip the items

                // Don't forget Cloaks which are all Class 4 SubClass 1 (cloth armor) and inventory type 16: Cloak
                // Rings are Class 4 Subclass 0 and intype 11: Finger
                /*switch (strInvSlot)
                {
                    case "INVTYPE_HEAD":
                    case "INVTYPE_SHOULDER":
                    case "INVTYPE_CHEST":
                    case "INVTYPE_WAIST":
                    case "INVTYPE_LEGS":
                    case "INVTYPE_FEET":
                    case "INVTYPE_HAND":
                        break;
                    case "INVTYPE_CLOAK":
                        break;
                    case "INVTYPE_NECK":
                    case "INVTYPE_FINGER":
                    case "INVTYPE_TRINKET":
                        break;
                    case "INVTYPE_2HWEAPON": // Two-Hand
                    case "INVTYPE_WEAPON": // One-Hand
                    case "INVTYPE_WEAPONMAINHAND": // Main Hand
                    case "INVTYPE_WEAPONOFFHAND": // Off Hand
                    case "INVTYPE_SHIELD":
                    case "INVTYPE_HOLDABLE": // Held In Off-hand
                        break;
                    default:
                        break;
                }*/
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
        }

        public static bool CheckItemStats(uint slot)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            string command = randomString + "='' ";
            command += "link=GetQuestItemLink(\"choice\"," + slot + "); ";
            command += "stats=GetItemStats(link); ";
            command += "for key,value in pairs(stats) do ";
            command += randomString + "=" + randomString + " .. key .. '^' .. value .. '^' ";
            command += "end;";
            Lua.LuaDoString(command);
            string sResult = Lua.GetLocalizedText(randomString);

            string[] itemStatsArray = sResult.Split('^');
            int index = 0;
            bool valid = true;
            while (index < itemStatsArray.Length -1)
            {
                string name = itemStatsArray[index];
                //string value = itemStatsArray[index+1];
                Enums.WoWStatistic Stat = ConvertToStatistic(name);
                if (Stat != Enums.WoWStatistic.None && !EquipmentAndStats.InternalEquipementStats.Contains(Stat))
                    valid = false;
                //Logging.WriteDebug(name + "=" + value);
                index += 2;
            }
            return valid;
        }

        static Enums.WoWStatistic ConvertToStatistic(string InterfaceName)
        {
            switch (InterfaceName)
            {
                case "ITEM_MOD_AGILITY_SHORT":
                    return Enums.WoWStatistic.AGILITY;
                case "ITEM_MOD_ATTACK_POWER_SHORT":
                    return Enums.WoWStatistic.ATTACK_POWER;
                case "ITEM_MOD_CRIT_RATING_SHORT":
                    return Enums.WoWStatistic.CRIT_RATING;
                case "ITEM_MOD_DODGE_RATING_SHORT":
                    return Enums.WoWStatistic.DODGE_RATING;
                case "ITEM_MOD_EXPERTISE_RATING_SHORT":
                    return Enums.WoWStatistic.EXPERTISE_RATING;
                case "ITEM_MOD_HASTE_RATING_SHORT":
                    return Enums.WoWStatistic.HASTE_RATING;
                case "ITEM_MOD_HIT_RATING_SHORT":
                    return Enums.WoWStatistic.HIT_RATING;
                case "ITEM_MOD_INTELLECT_SHORT":
                    return Enums.WoWStatistic.INTELLECT;
                case "ITEM_MOD_MASTERY_RATING_SHORT":
                    return Enums.WoWStatistic.MASTERY_RATING;
                case "ITEM_MOD_PARRY_RATING_SHORT":
                    return Enums.WoWStatistic.PARRY_RATING;
                case "not known":
                    return Enums.WoWStatistic.RESILIENCE_RATING;
                case "ITEM_MOD_SPELL_POWER_SHORT":
                    return Enums.WoWStatistic.SPELL_POWER;
                case "ITEM_MOD_SPIRIT_SHORT":
                    return Enums.WoWStatistic.SPIRIT;
                case "ITEM_MOD_STAMINA_SHORT":
                    return Enums.WoWStatistic.STAMINA;
                case "ITEM_MOD_STRENGTH_SHORT":
                    return Enums.WoWStatistic.STRENGTH;
                default: // To ignore ITEM_MOD_DAMAGE_PER_SECOND_SHORT for exemple
                    return Enums.WoWStatistic.None;
            }
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
                    PlayerQuest playerQuest =
                        (PlayerQuest)
                        Memory.WowMemory.Memory.ReadObject(
                            (uint) (addressQL + (Marshal.SizeOf(typeof (PlayerQuest))*index)),
                            typeof (PlayerQuest));
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
            public StateFlag State;
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