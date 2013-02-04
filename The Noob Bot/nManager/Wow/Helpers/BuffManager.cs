using System;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    internal class BuffManager
    {
        public static int AuraStack(uint baseAddress, List<UInt32> buffId)
        {
            try
            {
                int auraCount =
                    Memory.WowMemory.Memory.ReadInt(baseAddress + (uint) Addresses.UnitBaseGetUnitAura.AURA_COUNT_1);
                uint auraTable = baseAddress + (uint) Addresses.UnitBaseGetUnitAura.AURA_TABLE_1;
                if (auraCount == -1)
                {
                    auraCount =
                        Memory.WowMemory.Memory.ReadInt(baseAddress + (uint) Addresses.UnitBaseGetUnitAura.AURA_COUNT_2);
                    auraTable =
                        Memory.WowMemory.Memory.ReadUInt(baseAddress + (uint) Addresses.UnitBaseGetUnitAura.AURA_TABLE_2);
                }
                for (uint i = 0; i < auraCount && i < 200; i++)
                {
                    int spellID =
                        Memory.WowMemory.Memory.ReadInt(auraTable + ((uint) Addresses.UnitBaseGetUnitAura.AURA_SIZE*i) +
                                                        (uint) Addresses.UnitBaseGetUnitAura.AURA_SPELL_ID);
                    if (spellID > 0)
                    {
                        if (buffId.Contains((uint) spellID))
                            return
                                Memory.WowMemory.Memory.ReadByte(auraTable +
                                                                 ((uint) Addresses.UnitBaseGetUnitAura.AURA_SIZE*i) +
                                                                 (uint) Addresses.UnitBaseGetUnitAura.AURA_STACK);
                    }
                }
                return -1;
            }
            catch (Exception e)
            {
                Logging.WriteError("AuraStack(uint baseAddress, List<UInt32> buffId)" + e);
                return -1;
            }
        }

        public static bool HaveBuff(uint baseAddress, List<UInt32> buffId)
        {
            try
            {
                return (AuraStack(baseAddress, buffId) != -1);
            }
            catch (Exception e)
            {
                Logging.WriteError("HaveBuff(uint baseAddress, List<UInt32> buffId)" + e);
                return false;
            }
        }

        public static bool HaveBuff(uint objBase, UInt32 buffId)
        {
            try
            {
                var buffIdT = new List<UInt32> {buffId};
                return HaveBuff(objBase, buffIdT);
            }
            catch (Exception e)
            {
                Logging.WriteError("HaveBuff(uint objBase, UInt32 buffId)" + e);
                return false;
            }
        }
    }
}