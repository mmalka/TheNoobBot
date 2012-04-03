using System;
using System.Collections.Generic;
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
                var listItems = new List<WoWItem>();

                var itemId = new List<uint>
                             {
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_1_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_2_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_3_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_4_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_5_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_6_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_7_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_8_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_9_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_10_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_11_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_12_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_13_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_14_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_15_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_16_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_17_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_18_ENTRYID),
                                 ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                     Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_19_ENTRYID)
                             };

                if (itemId.Count > 0)
                {
                    List<WoWItem> objects = ObjectManager.ObjectManager.GetObjectWoWItem();

                    foreach (var o in objects)
                    {
                        var itemIdTemp = ObjectManager.ObjectManager.Me.GetDescriptor<uint>(o.GetBaseAddress, (uint)Descriptors.ObjectFields.OBJECT_FIELD_ENTRY);
                        var itemGuidOwner = ObjectManager.ObjectManager.Me.GetDescriptor<ulong>(o.GetBaseAddress, (uint)Descriptors.ItemFields.ITEM_FIELD_OWNER);

                        if (itemId.Contains(itemIdTemp) && itemGuidOwner == ObjectManager.ObjectManager.Me.Guid)
                        {
                            listItems.Add(o);
                        }
                    }
                }

                return listItems;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetEquippedItems(): " + exception);
            }
            return new List<WoWItem>();
        }

        public static WoWItem GetEquippedItem(WoWInventorySlot inventorySlot)
        {
            try
            {
                var itemsTemps = GetEquippedItems();

                foreach (var itemsTemp in itemsTemps)
                {
                    if (itemsTemp.GetItemInfo.ItemSubType == inventorySlot.ToString())
                        return itemsTemp;
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
