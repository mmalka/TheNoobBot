using System;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.ObjectManager
{
    public class WoWContainer : WoWItem
    {
        public WoWContainer(uint address)
            : base(address)
        {
        }

        public int NumberSlot
        {
            get
            {
                try
                {
                    return GetDescriptor<int>(Descriptors.ContainerFields.numSlots);
                }
                catch (Exception e)
                {
                    Logging.WriteError("NumberSlot: " + e);
                    return 0;
                }
            }
        }

        public int GetSlot(int slot)
        {
                try
                {
                    slot -= 1;
                    if (slot < 0 || slot > NumberSlot)
                        return 0;
                    return GetDescriptor<int>((uint) (((int)Descriptors.ContainerFields.slots) + (slot * 0x8)));
                }
                catch (Exception e)
                {
                    Logging.WriteError("GetSlot(int slot): " + e);
                    return 0;
                }
        }

        public T GetDescriptor<T>(Descriptors.ContainerFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint)field);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.ContainerFields field): " + e);
                return default(T);
            }
        }
    }
}
