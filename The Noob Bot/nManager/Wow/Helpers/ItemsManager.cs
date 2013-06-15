using System;
using System.Collections.Generic;
using System.Linq;
using nManager.Helpful;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class ItemsManager
    {
        public static int GetItemCountByNameLUA(string Name)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = GetItemCount(\"" + Name + "\");");
                    return Others.ToInt32(Lua.GetLocalizedText(randomString));
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemCountByNameLUA(string itemName): " + exception);
            }
            return 0;
        }

        public static int GetItemCountByIdLUA(int Entry)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = GetItemCount(" + Entry + ");");
                    try
                    {
                        return Others.ToInt32(Lua.GetLocalizedText(randomString));
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemCountByIdLUA(uint itemId): " + exception);
            }
            return 0;
        }

        public static void EquipItemByName(string Name)
        {
            try
            {
                Lua.LuaDoString("EquipItemByName(\"" + Name + "\")");
                Logging.Write("Equip item " + Name);
            }
            catch (Exception exception)
            {
                Logging.WriteError("EquipItemByName(string itemName): " + exception);
            }
        }

        public static void UseItem(int Entry)
        {
            try
            {
                string itemName = GetNameById(Entry);
                Lua.RunMacroText("/use " + itemName);
                Logging.WriteFight("Use item " + itemName + " (" + GetItemSpellByItemName(itemName) + ")");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UseItem(uint itemId): " + exception);
            }
        }

        public static void UseItem(string Name)
        {
            try
            {
                Lua.RunMacroText("/use " + Name);
                Logging.WriteFight("Use item " + Name + " (" + GetItemSpellByItemName(Name) + ")");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UseItem(string itemName): " + exception);
            }
        }

        public static void UseItem(int Entry, Class.Point point)
        {
            try
            {
                string itemName = GetNameById(Entry);
                ClickOnTerrain.Item(Entry, point);
                Logging.WriteFight("Use item " + itemName + " (" + GetItemSpellByItemName(itemName) + ") at position: " + point);
            }
            catch (Exception exception)
            {
                Logging.WriteError("UseItem(uint itemId, Class.Point point): " + exception);
            }
        }

        public static string GetNameById(int Entry)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + ", _, _, _, _, _, _, _ = GetItemInfo(" + Entry + ")");
                    return Lua.GetLocalizedText(randomString);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNameById(uint idItem): " + exception);
            }
            return "";
        }

        public static int GetIdByName(string Name)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    if (GetItemCountByNameLUA(Name) > 0)
                    {
                        Lua.LuaDoString(
                            "local nameItem = \"" + Name + "\" " +
                            "_, itemLink = GetItemInfo(nameItem); " +
                            "_,_," + randomString + " = string.find(itemLink, \".*|Hitem:(%d+):.*\"); "
                            );
                        return Others.ToInt32(Lua.GetLocalizedText(randomString));
                    }
                    return 0;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetIdByName(string nameItem): " + exception);
            }
            return 0;
        }

        public static bool IsItemOnCooldown(int Entry)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("startTime, duration, enable = GetItemCooldown(" + Entry + "); " + randomString +
                                " = startTime .. \"^\" .. duration .. \"^\" .. enable");
                string[] itemInfoArray = Lua.GetLocalizedText(randomString).Split('^');

                float itemStartTime = Others.ToSingle(itemInfoArray[0]);
                float itemDuration = Others.ToSingle(itemInfoArray[1]);
                uint itemEnable = Others.ToUInt32(itemInfoArray[2]);

                if (itemStartTime == 0 && itemDuration == 0)
                    return false;
                if (itemEnable == 0 || (itemEnable == 1 && itemStartTime < itemDuration))
                    return true;
                if (itemEnable == 1)
                    return false;
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsItemOnCooldown(uint itemId): " + exception);
            }
            return true;
        }

        public static bool IsUsableItemById(int Entry)
        {
            try
            {
                if (string.IsNullOrEmpty(GetItemSpellByItemId(Entry)))
                    return false;
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + ",_ = IsUsableItem(" + Entry + ")");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult != "nil" && sResult == "1")
                    return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsUsableItemById(uint itemId): " + exception);
            }
            return false;
        }

        public static bool IsUsableItemByName(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(GetItemSpellByItemName(Name)))
                    return false;
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + ",_ = IsUsableItem(" + Name + ")");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult != "nil" && sResult == "1")
                    return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsUsableItemByName(string itemName): " + exception);
            }
            return false;
        }

        public static string GetItemSpellByItemId(int Entry)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + ",_ = GetItemSpell(" + Entry + ")");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult != string.Empty && sResult != "nil")
                    return sResult;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemSpellByItemId(uint itemId): " + exception);
            }
            return "";
        }

        public static string GetItemSpellByItemName(string itemName)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + ",_ = GetItemSpell(" + itemName + ")");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult != string.Empty && sResult != "nil")
                    return sResult;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemSpellByItemName(string itemName): " + exception);
            }
            return "";
        }

        // Return best ILevel item
        private static WoWItem BestItemLevel(List<WoWItem> listItem)
        {
            try
            {
                WoWItem bestItem = new WoWItem(0);
                int best = 0;

                foreach (WoWItem iT in listItem)
                {
                    if (iT.GetItemInfo.ItemLevel > best)
                    {
                        best = iT.GetItemInfo.ItemLevel;
                        bestItem = iT;
                    }
                }
                return bestItem;
            }
            catch (Exception exception)
            {
                Logging.WriteError("BestItemLevel(List<WoWItem> listItem): " + exception);
            }
            return new WoWItem(0);
        }

        // Get item subtype (mail, ..)
        private static List<WoWItem> GetItemSubType(List<WoWItem> listItem, Enums.WoWItemTradeGoodsClass subType)
        {
            try
            {
                return listItem.Where(iT => iT.GetItemInfo.ItemSubType == subType.ToString()).ToList();
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemSubType(List<WoWItem> listItem, Enums.WoWItemTradeGoodsClass subType): " +
                                   exception);
            }
            return new List<WoWItem>();
        }

        // Get item type (amor...)
        private static List<WoWItem> GetItemType(List<WoWItem> listItem, Enums.WoWItemClass type)
        {
            try
            {
                return listItem.Where(iT => iT.GetItemInfo.ItemType == type.ToString()).ToList();
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemType(List<WoWItem> listItem, Enums.WoWItemClass type): " + exception);
            }
            return new List<WoWItem>();
        }
    }
}