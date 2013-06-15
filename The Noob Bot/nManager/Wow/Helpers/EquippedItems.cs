using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class EquippedItems
    {
        public static List<WoWItem> GetEquippedItems()
        {
            try
            {
                List<WoWItem> listItems = new List<WoWItem>();

                List<uint> itemId = new List<uint>
                    {
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_AMMO).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_HEAD).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_NECK).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_SHOULDER).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_BODY).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_CHEST).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_WAIST).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_LEGS).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_FEET).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_WRIST).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_HAND).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_FINGER).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_FINGER + 1).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_TRINKET).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_TRINKET + 1).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_CLOAK).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_WEAPON).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_SHIELD).Entry,
                        (uint) GetEquippedItem((int) WoWInventorySlot.INVTYPE_RANGED).Entry
                    };

                if (itemId.Count > 0)
                {
                    List<WoWItem> objects = ObjectManager.ObjectManager.GetObjectWoWItem();

                    listItems.AddRange(from o in objects
                                       let itemIdTemp = ObjectManager.ObjectManager.Me.GetDescriptor<uint>(o.GetBaseAddress, (uint) Descriptors.ObjectFields.Entry)
                                       let itemGuidOwner = ObjectManager.ObjectManager.Me.GetDescriptor<ulong>(o.GetBaseAddress, (uint) Descriptors.ItemFields.Owner)
                                       where itemId.Contains(itemIdTemp) && itemGuidOwner == ObjectManager.ObjectManager.Me.Guid
                                       select o);
                }

                return listItems;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetEquippedItems(): " + exception);
            }
            return new List<WoWItem>();
        }

        public static WoWItem GetEquippedItem(int invSlot)
        {
            ulong guid = ObjectManager.ObjectManager.Me.GetDescriptor<ulong>((Descriptors.PlayerFields) (uint) Descriptors.PlayerFields.InvSlots + (invSlot*2));
            List<WoWItem> items = ObjectManager.ObjectManager.GetObjectWoWItem();
            WoWItem first = items.FirstOrDefault(x => x.Guid == guid);
            WoWItem item = first ?? new WoWItem(0);
            return item;
        }

        public static bool IsEquippedItemByGuid(ulong guid)
        {
            int slot;
            ulong tmpguid = 0;
            bool success = false;
            for (slot = 0; slot < 19; slot++)
            {
                tmpguid = ObjectManager.ObjectManager.Me.GetDescriptor<ulong>((Descriptors.PlayerFields) (uint) Descriptors.PlayerFields.InvSlots + (slot*2));
                if (tmpguid != guid) continue;
                success = true;
                break;
            }
            if (success)
            {
                WoWItem item = tmpguid == guid ? new WoWItem(ObjectManager.ObjectManager.GetObjectByGuid(tmpguid).GetBaseAddress) : new WoWItem(0);
                return item.IsValid;
            }
            return false;
        }

        public static WoWItem GetEquippedItem(WoWInventorySlot inventorySlot, uint resultNb = 1)
        {
            uint nb = 1;
            try
            {
                List<WoWItem> itemsTemps = GetEquippedItems();

                foreach (WoWItem itemsTemp in itemsTemps)
                {
                    if (itemsTemp.GetItemInfo.ItemEquipLoc == inventorySlot.ToString() && nb == resultNb)
                        return itemsTemp;
                    if (itemsTemp.GetItemInfo.ItemEquipLoc == inventorySlot.ToString())
                        nb++;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetEquippedItem(WoWInventorySlot inventorySlot): " + exception);
            }
            return new WoWItem(0);
        }
    }
}