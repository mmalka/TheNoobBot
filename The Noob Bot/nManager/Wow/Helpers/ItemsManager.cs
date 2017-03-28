using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Ink;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class ItemsManager
    {
        public static int GetItemCount(string name)
        {
            return GetItemCount(Others.ToInt32(name) > 0 ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static int GetItemCount(int entry)
        {
            lock (typeof (ItemsManager))
            {
                try
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = GetItemCount(" + entry + ");");
                    return Others.ToInt32(Lua.GetLocalizedText(randomString));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetItemCountByIdLUA(uint itemId): " + exception);
                }
                return 0;
            }
        }

        public static bool HasToy(int entry)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = tostring(PlayerHasToy(" + entry + "));");
            return Others.ToBoolean(Lua.GetLocalizedText(randomString));
        }

        public static void UseToy(int entry)
        {
            Lua.LuaDoString("UseToy(" + entry + ");");
        }

        public static void EquipItemByName(string name)
        {
            try
            {
                Lua.LuaDoString("EquipItemByName(\"" + name + "\")");
                Logging.Write("Equip item " + name);
            }
            catch (Exception exception)
            {
                Logging.WriteError("EquipItemByName(string name): " + exception);
            }
        }

        public static void UseItem(string name)
        {
            try
            {
                Lua.RunMacroText("/use " + name);
                Logging.WriteFight("Use item " + name + " (SpellName: " + GetItemSpell(name) + ")");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UseItem(string name): " + exception);
            }
        }

        public static void UseItem(int entry)
        {
            UseItem(GetItemNameById(entry));
        }

        public static void UseItem(int entry, Class.Point point)
        {
            try
            {
                string itemName = GetItemNameById(entry);
                ClickOnTerrain.Item(entry, point);
                Logging.WriteFight("Use item " + itemName + " (SpellName: " + GetItemSpell(itemName) + ")");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UseItem(int entry, Class.Point point): " + exception);
            }
        }

        public static void UseItem(string name, Class.Point point)
        {
            UseItem(Others.ToInt32(name) > 0 ? Others.ToInt32(name) : GetItemIdByName(name), point);
        }

        public static Dictionary<int, string> ItemNameCache = new Dictionary<int, string>();

        public static string GetItemNameById(uint entry)
        {
            return GetItemNameById((int) entry);
            // Entry should be uint, but there must be some code calling it with int, so this is just an alias.
        }

        public static string GetItemNameById(int entry)
        {
            try
            {
                if (ItemNameCache.ContainsKey(entry))
                    return ItemNameCache[entry];
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + ",_,_,_,_,_,_,_,_,_,_ = GetItemInfo(" + entry + ")");
                    string itemName = Lua.GetLocalizedText(randomString);
                    if (!string.IsNullOrWhiteSpace(itemName))
                        ItemNameCache.Add(entry, itemName);
                    return itemName;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNameById(int entry): " + exception);
            }
            return "";
        }

        public static Dictionary<string, int> ItemIdCache = new Dictionary<string, int>();

        public static int GetItemIdByName(string name)
        {
            if (name == "")
                return 0;
            try
            {
                if (ItemIdCache.ContainsKey(name))
                    return ItemIdCache[name];
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(
                        "local nameItem = \"" + name + "\" " +
                        "_,itemLink,_,_,_,_,_,_,_,_,_  = GetItemInfo(nameItem); " +
                        "if itemLink == nil then " + randomString + " = 0 else " +
                        "_,_," + randomString + " = string.find(itemLink, 'item:(%d+)') end");
                    int itemEntry = Others.ToInt32(Lua.GetLocalizedText(randomString));
                    if (itemEntry > 0)
                        ItemIdCache.Add(name, itemEntry);
                    return itemEntry;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetIdByName(string name): " + exception);
            }
            return 0;
        }

        public static bool IsItemOnCooldown(int entry)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("startTime, duration, enable = GetItemCooldown(" + entry + ") " +
                                randomString + " = startTime .. \"^\" .. duration .. \"^\" .. time() .. \"^\" .. enable");
                string[] itemInfoArray = Lua.GetLocalizedText(randomString).Split('^');
                if (itemInfoArray.Count() != 4)
                    return true;

                float itemStartTime = Others.ToSingle(itemInfoArray[0]);
                float itemDuration = Others.ToSingle(itemInfoArray[1]);
                float currTime = Others.ToSingle(itemInfoArray[2]);
                uint itemEnable = Others.ToUInt32(itemInfoArray[3]);

                if (itemStartTime <= 0 && itemDuration <= 0)
                    return false;
                if (itemEnable == 0 || (itemEnable == 1 && itemStartTime + itemDuration < currTime))
                    return true;
                if (itemEnable == 1)
                    return false;
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsItemOnCooldown(int entry): " + exception);
            }
            return true;
        }

        public static bool IsItemOnCooldown(string name)
        {
            return IsItemOnCooldown(Others.ToInt32(name) > 0 ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static bool IsItemUsable(int entry)
        {
            try
            {
                if (string.IsNullOrEmpty(GetItemSpell(entry)))
                    return false;
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + ",_ = tostring(IsUsableItem(" + entry + "))");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult == "true")
                    return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsUsableItemById(uint itemId): " + exception);
            }
            return false;
        }

        public static bool IsItemUsable(string name)
        {
            return IsItemUsable(Others.ToInt32(name) > 0 ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static bool IsHarmfulItem(int entry)
        {
            try
            {
                if (string.IsNullOrEmpty(GetItemSpell(entry)))
                    return false;
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = tostring(IsHarmfulItem(" + entry + "))");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult == "true")
                    return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsHarmfulItem(string itemName): " + exception);
            }
            return false;
        }

        public static bool IsHarmfulItem(string name)
        {
            return IsHarmfulItem(Others.ToInt32(name) > 0 ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static bool IsHelpfulItem(int entry)
        {
            try
            {
                if (string.IsNullOrEmpty(GetItemSpell(entry)))
                    return false;
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = tostring(IsHelpfulItem(" + entry + "))");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult == "true")
                    return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsHarmfulItem(string itemName): " + exception);
            }
            return false;
        }

        public static bool IsHelpfulItem(string name)
        {
            return IsHelpfulItem(Others.ToInt32(name) > 0 ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static Dictionary<int, string> ItemSpellCache = new Dictionary<int, string>();

        public static string GetItemSpell(int entry)
        {
            try
            {
                lock (ItemSpellCache)
                {
                    if (ItemSpellCache.ContainsKey(entry))
                        return ItemSpellCache[entry];
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + ",_ = GetItemSpell(" + entry + ")");
                    string sResult = Lua.GetLocalizedText(randomString);
                    if (sResult != string.Empty && sResult != "nil")
                    {
                        ItemSpellCache.Add(entry, sResult);
                        return sResult;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemSpellByItemId(uint itemId): " + exception);
            }
            return "";
        }

        public static string GetItemSpell(string name)
        {
            return GetItemSpell(Others.ToInt32(name) > 0 ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        // Return best ILevel item
// ReSharper disable UnusedMember.Local
        private static WoWItem BestItemLevel(IEnumerable<WoWItem> listItem)
// ReSharper restore UnusedMember.Local
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
// ReSharper disable UnusedMember.Local
        private static List<WoWItem> GetItemSubType(IEnumerable<WoWItem> listItem, Enums.WoWItemTradeGoodsClass subType)
// ReSharper restore UnusedMember.Local
        {
            try
            {
                List<WoWItem> list = new List<WoWItem>();
                foreach (WoWItem iT in listItem)
                {
                    if (iT.GetItemInfo.ItemSubType == subType.ToString()) list.Add(iT);
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemSubType(List<WoWItem> listItem, Enums.WoWItemTradeGoodsClass subType): " +
                                   exception);
            }
            return new List<WoWItem>();
        }

        // Get item type (amor...)
// ReSharper disable UnusedMember.Local
        private static List<WoWItem> GetItemType(IEnumerable<WoWItem> listItem, Enums.WoWItemClass type)
// ReSharper restore UnusedMember.Local
        {
            try
            {
                List<WoWItem> list = new List<WoWItem>();
                foreach (WoWItem iT in listItem)
                {
                    if (iT.GetItemInfo.ItemType == type.ToString()) list.Add(iT);
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemType(List<WoWItem> listItem, Enums.WoWItemClass type): " + exception);
            }
            return new List<WoWItem>();
        }

        public static List<string> GetAllReagentsItems()
        {
            List<string> reagentsItems = new List<string>();
            var listItems = ObjectManager.ObjectManager.GetObjectWoWItem();
            try
            {
                Memory.WowMemory.GameFrameLock();
                foreach (var item in listItems)
                {
                    if (item.GetItemInfo.ItemType == "Tradeskill" && !reagentsItems.Contains(item.Name) && !item.Name.Contains("\""))
                        reagentsItems.Add(item.Name);
                }
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
            return reagentsItems;
        }

        public static List<WoWItem> GetAllFoodItems()
        {
            List<WoWItem> reagentsItems = new List<WoWItem>();
            var listItems = ObjectManager.ObjectManager.GetObjectWoWItem();
            try
            {
                Memory.WowMemory.GameFrameLock();
                foreach (var item in listItems)
                {
                    if (item.GetItemInfo.ItemType == "Consumable" && item.GetItemInfo.ItemSubType == "Food & Drink" && !reagentsItems.Contains(item) && !item.Name.Contains("\""))
                        reagentsItems.Add(item);
                }
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
            return reagentsItems;
        }

        public static List<string> GetAllQuestsItems()
        {
            List<string> questsItems = new List<string>();
            var listItems = ObjectManager.ObjectManager.GetObjectWoWItem();
            try
            {
                Memory.WowMemory.GameFrameLock();
                foreach (var item in listItems)
                {
                    if (item.GetItemInfo.ItemType == "Quest" && !questsItems.Contains(item.Name) && item.GetItemInfo.ItemRarity == (uint) WoWItemQuality.Common && !item.Name.Contains("\""))
                        questsItems.Add(item.Name);
                }
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
            return questsItems;
        }
    }
}