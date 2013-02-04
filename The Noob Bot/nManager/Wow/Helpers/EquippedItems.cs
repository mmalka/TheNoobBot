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
                                         Descriptors.PlayerFields.VisibleItems + 0*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 1*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 2*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 3*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 4*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 5*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 6*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 7*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 8*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 9*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 10*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 11*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 12*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 13*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 14*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 15*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 16*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 17*2),
                                     ObjectManager.ObjectManager.Me.GetDescriptor<uint>(
                                         Descriptors.PlayerFields.VisibleItems + 18*2)
                                 };

                if (itemId.Count > 0)
                {
                    List<WoWItem> objects = ObjectManager.ObjectManager.GetObjectWoWItem();

                    foreach (var o in objects)
                    {
                        var itemIdTemp = ObjectManager.ObjectManager.Me.GetDescriptor<uint>(o.GetBaseAddress,
                                                                                            (uint)
                                                                                            Descriptors.ObjectFields.
                                                                                                        Entry);
                        var itemGuidOwner = ObjectManager.ObjectManager.Me.GetDescriptor<ulong>(o.GetBaseAddress,
                                                                                                (uint)
                                                                                                Descriptors.ItemFields.
                                                                                                            Owner);

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

        public static WoWItem GetEquippedItem(WoWInventorySlot inventorySlot, uint resultNb = 1)
        {
            uint nb = 1;
            try
            {
                var itemsTemps = GetEquippedItems();

                foreach (var itemsTemp in itemsTemps)
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