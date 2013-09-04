using System;
using System.Collections.Generic;
using System.Linq;
using nManager.Helpful;
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

        public static string GetItemNameById(int entry)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + ",_,_,_,_,_,_,_,_,_,_ = GetItemInfo(" + entry + ")");
                    return Lua.GetLocalizedText(randomString);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNameById(int entry): " + exception);
            }
            return "";
        }

        public static int GetItemIdByName(string name)
        {
            try
            {
                lock (typeof (ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(
                        "local nameItem = \"" + name + "\" " +
                        "_,itemLink,_,_,_,_,_,_,_,_,_  = GetItemInfo(nameItem); " +
                        "_,_," + randomString + " = string.find(itemLink, \".*|Hitem:(%d+):.*\"); "
                        );
                    return Others.ToInt32(Lua.GetLocalizedText(randomString));
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
                Lua.LuaDoString(randomString + ",_ = IsUsableItem(" + entry + ")");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult == "1")
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
                Lua.LuaDoString(randomString + " = IsHarmfulItem(" + entry + ")");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult == "1")
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
                Lua.LuaDoString(randomString + " = IsHelpfulItem(" + entry + ")");
                string sResult = Lua.GetLocalizedText(randomString);
                if (sResult == "1")
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

        public static string GetItemSpell(int entry)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + ",_ = GetItemSpell(" + entry + ")");
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
// ReSharper disable UnusedMember.Local
        private static List<WoWItem> GetItemType(IEnumerable<WoWItem> listItem, Enums.WoWItemClass type)
// ReSharper restore UnusedMember.Local
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