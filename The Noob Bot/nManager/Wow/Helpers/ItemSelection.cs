using System;
using nManager.Helpful;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    public class ItemSelection
    {
        public static int EvaluateItemStatsVsEquiped(string link, out ItemInfo equipment)
        {
            int itemValueForMe = 0;
            string slot = "";
            ItemInfo questRewardItem = new ItemInfo(link);

            // Now get the class and subclass of the item by searching in DBCs with localized names
            WoWItemClass TheItemClassRec = WoWItemClass.FromName(questRewardItem.ItemType);
            uint classId = TheItemClassRec.Record.ClassId;
            WoWItemSubClass TheItemSubClassRec = WoWItemSubClass.FromNameAndClass(questRewardItem.ItemSubType, classId);
            uint subClassId = TheItemSubClassRec.Record.SubClassId;

            Logging.WriteDebug("Item \"" + questRewardItem.ItemName + "\" equip \"" + questRewardItem.ItemEquipLoc + "\" class " + (Enums.WoWItemClass) classId + " subclass " + subClassId +
                               " has a value of " + questRewardItem.ItemSellPrice);

            if ((Enums.WoWItemClass) classId == Enums.WoWItemClass.Armor)
                if ((Enums.WowItemSubClassArmor) subClassId == EquipmentAndStats.EquipableArmorItemType
                    || questRewardItem.ItemEquipLoc == "INVTYPE_CLOAK" || questRewardItem.ItemEquipLoc == "INVTYPE_NECK"
                    || questRewardItem.ItemEquipLoc == "INVTYPE_FINGER" || questRewardItem.ItemEquipLoc == "INVTYPE_TRINKET"
                    || (questRewardItem.ItemEquipLoc == "INVTYPE_SHIELD" && EquipmentAndStats.HasShield))
                    if (CheckItemStats(link))
                        itemValueForMe = -1; // Just to trigger the next checks
            if ((Enums.WoWItemClass) classId == Enums.WoWItemClass.Weapon)
                if (EquipmentAndStats.EquipableWeapons.Contains((Enums.WowItemSubClassWeapon) subClassId))
                    if (CheckItemStats(link))
                        itemValueForMe = -1;
            if (itemValueForMe != 0) // Item interesting
            {
                itemValueForMe = 0;
                string sLink = Others.GetRandomString(Others.Random(4, 10));
                switch (questRewardItem.ItemEquipLoc)
                {
                    case "INVTYPE_HEAD":
                        slot = "INVSLOT_HEAD";
                        break;
                    case "INVTYPE_SHOULDER":
                        slot = "INVSLOT_SHOULDER";
                        break;
                    case "INVTYPE_CHEST":
                    case "INVTYPE_ROBE":
                        slot = "INVSLOT_CHEST";
                        break;
                    case "INVTYPE_WAIST":
                        slot = "INVSLOT_WAIST";
                        break;
                    case "INVTYPE_LEGS":
                        slot = "INVSLOT_LEGS";
                        break;
                    case "INVTYPE_FEET":
                        slot = "INVSLOT_FEET";
                        break;
                    case "INVTYPE_WRIST":
                        slot = "INVSLOT_WRIST";
                        break;
                    case "INVTYPE_HAND":
                        slot = "INVSLOT_HAND";
                        break;
                    case "INVTYPE_CLOAK":
                        slot = "INVSLOT_BACK";
                        break;
                    case "INVTYPE_NECK":
                        slot = "INVSLOT_NECK";
                        break;
                    case "INVTYPE_FINGER":
                        slot = "INVSLOT_FINGER1";
                        break;
                    case "INVTYPE_WEAPON": // all this weapon code is not perfect at all
                    case "INVTYPE_WEAPONMAINHAND":
                    case "INVTYPE_2HWEAPON":
                    case "INVTYPE_RANGED":
                        slot = "INVSLOT_MAINHAND";
                        break;
                    case "INVTYPE_WEAPONOFFHAND":
                    case "INVTYPE_HOLDABLE":
                    case "INVTYPE_SHIELD":
                        slot = "INVSLOT_OFFHAND";
                        break;
                }
                Lua.LuaDoString(sLink + "= GetInventoryItemLink(\"player\", " + slot + ")");
                string linkEquip = Lua.GetLocalizedText(sLink);
                ItemInfo equipedItem = new ItemInfo(linkEquip);
                if (questRewardItem.ItemEquipLoc == "INVTYPE_FINGER" || questRewardItem.ItemEquipLoc == "INVTYPE_TRINKET") // Then check the second one too
                {
                    if (questRewardItem.ItemEquipLoc == "INVTYPE_FINGER")
                        Lua.LuaDoString(sLink + "= GetInventoryItemLink(\"player\", INVSLOT_FINGER2)");
                    else // TRINKET
                        Lua.LuaDoString(sLink + "= GetInventoryItemLink(\"player\", INVSLOT_TRINKET2)");
                    string linkEquip2 = Lua.GetLocalizedText(sLink);
                    ItemInfo equipedItem2 = new ItemInfo(linkEquip2);

                    if ((questRewardItem.ItemRarity > equipedItem2.ItemRarity && questRewardItem.ItemLevel > (equipedItem2.ItemLevel - (5*equipedItem2.ItemLevel/100)))
                        || (questRewardItem.ItemRarity == equipedItem2.ItemRarity && questRewardItem.ItemLevel > equipedItem2.ItemLevel)
                        || (questRewardItem.ItemRarity < equipedItem2.ItemRarity && questRewardItem.ItemLevel > (equipedItem2.ItemLevel + (5*equipedItem2.ItemLevel/100))))
                    {
                        itemValueForMe = -1;
                        slot = (questRewardItem.ItemEquipLoc == "INVTYPE_FINGER" ? "INVSLOT_FINGER2" : "INVSLOT_TRINKET2");
                    }
                }
                if ((questRewardItem.ItemRarity > equipedItem.ItemRarity && questRewardItem.ItemLevel > (equipedItem.ItemLevel - (5*equipedItem.ItemLevel/100)))
                    || (questRewardItem.ItemRarity == equipedItem.ItemRarity && questRewardItem.ItemLevel > equipedItem.ItemLevel)
                    || (questRewardItem.ItemRarity < equipedItem.ItemRarity && questRewardItem.ItemLevel > (equipedItem.ItemLevel + (5*equipedItem.ItemLevel/100))))
                    itemValueForMe = -1;
            }
            if (itemValueForMe == 0)
                itemValueForMe = (int) questRewardItem.ItemSellPrice;

            equipment = questRewardItem;
            equipment.xItemEquipSlot = slot;
            return itemValueForMe;
        }

        public static bool CheckItemStats(string link)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            string command = randomString + "='' ";
            command += "stats=GetItemStats(" + link + "); ";
            command += "for key,value in pairs(stats) do ";
            command += randomString + "=" + randomString + " .. key .. '^' .. value .. '^' ";
            command += "end;";
            Lua.LuaDoString(command);
            string sResult = Lua.GetLocalizedText(randomString);

            string[] itemStatsArray = sResult.Split('^');
            int index = 0;
            bool valid = true;
            while (index < itemStatsArray.Length - 1)
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

        private static Enums.WoWStatistic ConvertToStatistic(string InterfaceName)
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

        public static void EquipItem(ItemInfo item)
        {
            Logging.Write("Equiping item \"" + item.ItemName + "\"");
            Lua.LuaDoString("EquipItemByName(\"" + item.ItemLink + "\", " + item.xItemEquipSlot + ")");
        }

        /* May be of iterest :
         * GetItemStatDelta(itemlnk1, itemlnk2)
         */
    }
}