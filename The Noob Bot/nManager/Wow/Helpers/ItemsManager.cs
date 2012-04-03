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
                lock (typeof(ItemsManager))
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
                lock (typeof(ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = GetItemCount(" + itemId + ");");
                    return Convert.ToInt32(Lua.GetLocalizedText(randomString));
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
                lock (typeof(ItemsManager))
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
                lock (typeof(ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(
                        "local nameItem = \"" + nameItem + "\" " +
                        "_, itemLink = GetItemInfo(nameItem); " +
                        "print(itemLink)  _,_," + randomString + " = string.find(itemLink, \".*|Hitem:(%d+):.*\"); "
                        );
                    string ret = Lua.GetLocalizedText(randomString);
                    return Convert.ToInt32(ret);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetIdByName(string nameItem): " + exception);
            }
            return 0;
        }

        // Return best ILevel item
        static WoWItem BestItemLevel(List<WoWItem> listItem)
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
        static List<WoWItem> GetItemSubType(List<WoWItem> listItem, Enums.WoWItemTradeGoodsClass subType)
        {
            try
            {
                return listItem.Where(iT => iT.GetItemInfo.ItemSubType == subType.ToString()).ToList();
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemSubType(List<WoWItem> listItem, Enums.WoWItemTradeGoodsClass subType): " + exception);
            }
            return new List<WoWItem>();
        }

        // Get item type (amor...)
        static List<WoWItem> GetItemType(List<WoWItem> listItem, Enums.WoWItemClass type)
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
