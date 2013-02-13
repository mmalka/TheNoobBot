using System;
using System.Collections.Generic;
using System.Linq;
using nManager.Helpful;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class ItemsManager
    {
        public static int GetItemCountByNameLUA(string itemName)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = GetItemCount(\"" + itemName + "\");");
                    return Convert.ToInt32(Lua.GetLocalizedText(randomString));
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemCountByNameLUA(string itemName): " + exception);
            }
            return 0;
        }

        public static int GetItemCountByIdLUA(uint itemId)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = GetItemCount(" + itemId + ");");
                    try
                    {
                        return Convert.ToInt32(Lua.GetLocalizedText(randomString));
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

        public static void EquipItemByName(string itemName)
        {
            try
            {
                Lua.LuaDoString("EquipItemByName(\"" + itemName + "\")");
            }
            catch (Exception exception)
            {
                Logging.WriteError("EquipItemByName(string itemName): " + exception);
            }
        }

        public static void UseItem(string itemName)
        {
            try
            {
                Lua.RunMacroText("/use " + itemName);
            }
            catch (Exception exception)
            {
                Logging.WriteError("UseItem(string itemName): " + exception);
            }
        }

        public static void UseItem(uint itemId, Class.Point point)
        {
            try
            {
                ClickOnTerrain.Item(itemId, point);
            }
            catch (Exception exception)
            {
                Logging.WriteError("UseItem(uint itemId, Class.Point point): " + exception);
            }
        }

        public static string GetNameById(uint idItem)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + ", _, _, _, _, _, _, _ = GetItemInfo(" + idItem + ")");
                    return Lua.GetLocalizedText(randomString);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNameById(uint idItem): " + exception);
            }
            return "";
        }

        public static int GetIdByName(string nameItem)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    if (GetItemCountByNameLUA(nameItem) > 0)
                    {
                        Lua.LuaDoString(
                            "local nameItem = \"" + nameItem + "\" " +
                            "_, itemLink = GetItemInfo(nameItem); " +
                            "print(itemLink)  _,_," + randomString + " = string.find(itemLink, \".*|Hitem:(%d+):.*\"); "
                            );
                        string ret = Lua.GetLocalizedText(randomString);
                        return Convert.ToInt32(ret);
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

        public static bool IsItemOnCooldown(uint itemId)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("startTime, duration, enable = GetItemCooldown(" + itemId + "); " + randomString +
                                " = startTime .. \"^\" .. duration .. \"^\" .. enable");
                string[] itemInfoArray = Lua.GetLocalizedText(randomString).Split(Convert.ToChar("^"));

                uint itemStartTime = Convert.ToUInt32(itemInfoArray[0]);
                uint itemDuration = Convert.ToUInt32(itemInfoArray[1]);
                uint itemEnable = Convert.ToUInt32(itemInfoArray[2]);

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

        public static bool IsUsableItemById(uint itemId)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + ",_ = IsUsableItem(" + itemId + ")");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult != "nil" && sResult == "1")
                    return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsUsableItem(uint itemId): " + exception);
            }
            return false;
        }

        public static bool IsUsableItemByName(string itemName)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + ",_ = IsUsableItem(" + itemName + ")");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult != "nil" && sResult == "1")
                    return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsUsableItem(string itemName): " + exception);
            }
            return false;
        }

        public static string GetItemSpellByItemId(uint itemId)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + ",_ = GetItemSpell(" + itemId + ")");
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
                var bestItem = new WoWItem(0);
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