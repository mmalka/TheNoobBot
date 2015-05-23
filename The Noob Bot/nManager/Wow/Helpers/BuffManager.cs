using System;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
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
                var buffIdT = new List<UInt32> {buffId};
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
            foreach (Auras.UnitAura buff in AuraStack(baseAddress).Auras)
            {
                if (buffId.Contains(buff.AuraSpellId))
                {
                    if (buff.IsActive)
                        return buff.AuraCount;
                    if (buff.Flags.HasFlag(UnitAuraFlags.Passive) && !buff.Flags.HasFlag(UnitAuraFlags.Cancelable))
                        return -1;           
                    return buff.AuraCount;
                }
            }
            return -1;
        }

        public static Auras.UnitAuras AuraStack(uint baseAddress)
        {
            try
            {
                var unitAuras = new Auras.UnitAuras(baseAddress);
                uint auraTableBase = baseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraTable1;

                int auraCount = Memory.WowMemory.Memory.ReadInt(auraTableBase + (uint) Addresses.UnitBaseGetUnitAura.AuraTable2);

                if (auraCount == -1)
                    auraCount = Memory.WowMemory.Memory.ReadInt(auraTableBase);
                for (uint currentIndex = 0; currentIndex < auraCount; currentIndex++)
                {
                    uint currentAuraPtr = GetAuraPtrByIndex(auraTableBase, currentIndex);

                    if (currentAuraPtr > 0)
                    {
                        UInt128 auraCreatorGuid = Memory.WowMemory.Memory.ReadUInt128(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructCreatorGuid);
                        uint auraSpellId = Memory.WowMemory.Memory.ReadUInt(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructSpellId);
                        byte auraFlags = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructFlags);
                        byte auraStackCount = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructCount);
                        byte auraCasterLevel = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructCasterLevel);
                        byte auraUnk2 = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructUnk2);
                        int auraDuration = Memory.WowMemory.Memory.ReadInt(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructDuration);
                        int auraSpellEndTime = Memory.WowMemory.Memory.ReadInt(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructSpellEndTime);
                        byte auraUnk3 = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructUnk3);
                        var currUnitAura = new Auras.UnitAura
                        {
                            AuraCreatorGUID = auraCreatorGuid,
                            AuraSpellId = auraSpellId,
                            AuraFlags = auraFlags,
                            AuraCount = auraStackCount,
                            AuraCasterLevel = auraCasterLevel,
                            AuraUnk2 = auraUnk2,
                            AuraDuration = auraDuration,
                            AuraSpellEndTime = auraSpellEndTime,
                            AuraUnk3 = auraUnk3
                        };
                        unitAuras.Auras.Add(currUnitAura);
                    }
                }
                return unitAuras;
            }
            catch (Exception e)
            {
                Logging.WriteError("AuraStack(uint baseAddress, List<UInt32> buffId)" + e);
            }
            return new Auras.UnitAuras(baseAddress);
        }

        private static uint GetAuraPtrByIndex(uint auraBase, uint currentIndex)
        {
            try
            {
                uint result;

                uint currentAura = (uint) Addresses.UnitBaseGetUnitAura.AuraSize*currentIndex;
                if (Memory.WowMemory.Memory.ReadInt(auraBase + (uint) Addresses.UnitBaseGetUnitAura.AuraTable2) == -1)
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