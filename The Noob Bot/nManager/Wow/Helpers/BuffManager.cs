using System;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    internal class BuffManager
    {
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
                List<uint> buffIdT = new List<UInt32> {buffId};
                return HaveBuff(objBase, buffIdT);
            }
            catch (Exception e)
            {
                Logging.WriteError("HaveBuff(uint objBase, UInt32 buffId)" + e);
                return false;
            }
        }

        public static int AuraStack(uint baseAddress, List<UInt32> buffId)
        {
            try
            {
                if (buffId == null || buffId.Count <= 0)
                    return 0;

                uint auraTableBase = baseAddress + (uint)Addresses.UnitBaseGetUnitAura.AuraTable1;

                int auraCount = Memory.WowMemory.Memory.ReadInt(auraTableBase + (uint)Addresses.UnitBaseGetUnitAura.AuraTable2);

                if (auraCount == -1)
                    auraCount = Memory.WowMemory.Memory.ReadInt(auraTableBase);
                for (uint currentIndex = 0; currentIndex < auraCount; currentIndex++)
                {
                    uint currentAuraPtr = GetAuraPtrByIndex(auraTableBase, currentIndex);
                    
                    if (currentAuraPtr > 0)
                    {
                        int spellId = Memory.WowMemory.Memory.ReadInt(currentAuraPtr + (uint)Addresses.UnitBaseGetUnitAura.AuraSpellId);
                        int stack = Memory.WowMemory.Memory.ReadInt(currentAuraPtr + (uint)Addresses.UnitBaseGetUnitAura.AuraStack);
                        //Logging.WriteDebug("Aura for spellid(" + spellId + "), stack(" + stack + "): " + SpellManager.GetSpellInfo((uint)spellId).Name);
                        if (spellId > 0 && buffId.Contains((uint)spellId))
                            return stack;
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

        static uint GetAuraPtrByIndex(uint auraBase, uint currentIndex)
        {
            try
            {
                uint result;

                uint currentAura = (uint)Addresses.UnitBaseGetUnitAura.AuraSize * currentIndex;
                if (Memory.WowMemory.Memory.ReadUInt(auraBase + (uint)Addresses.UnitBaseGetUnitAura.AuraTable2) == -1)
                {
                    result = Memory.WowMemory.Memory.ReadUInt(auraBase + 4) + currentAura;
                }
                else
                {
                    result = auraBase + currentAura;
                }
                return result;
            }
            catch (Exception e)
            {
                Logging.WriteError("static uint GetAura(uint auraBase, uint currentIndex)" + e);
                return 0;
            }
        }
    }
}