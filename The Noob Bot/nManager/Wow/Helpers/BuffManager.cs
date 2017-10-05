using System;
using System.Collections.Generic;
using System.Threading;
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
            var cachedAuraStack = AuraStack(baseAddress).Auras;
            for (int i = 0; i < cachedAuraStack.Count; i++)
            {
                Auras.UnitAura buff = cachedAuraStack[i];
                if (buffId.Contains(buff.AuraSpellId))
                {
                    if (buff.IsActive)
                        return buff.AuraCount;
                    if (!buff.Flags.HasFlag(UnitAuraFlags.Passive) || buff.Flags.HasFlag(UnitAuraFlags.Cancelable))
                        return buff.AuraCount;
                    if (buff.Flags.HasFlag(UnitAuraFlags.Duration) || buff.Flags.HasFlag(UnitAuraFlags.BasePoints))
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
                        uint auraSpellId = Memory.WowMemory.Memory.ReadUInt(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructSpellId);
                        var currUnitAura = new Auras.UnitAura
                        {
                            BaseAddress = currentAuraPtr,
                            AuraSpellId = auraSpellId,
                        };
                        /*if (auraSpellId == 158188)
                        {
                            Logging.WriteDebug("=== Stealth ===");
                            string memoryDump = "";
                            var bytesDump = Memory.WowMemory.Memory.ReadBytes(currentAuraPtr, 0x98);
                            foreach (byte b in bytesDump)
                            {
                                memoryDump = memoryDump + " " + b;
                            }
                            Logging.WriteDebug("Memory Dump: " + memoryDump);
                            Logging.WriteDebug(currUnitAura.ToString());
                            Logging.WriteDebug("Me.Guid: " + ObjectManager.ObjectManager.Me.Guid);

                            memoryDump = "";
                            foreach (byte b in ObjectManager.ObjectManager.Me.Guid.ToByteArray())
                            {
                                memoryDump = memoryDump + " " + b;
                            }
                            Logging.WriteDebug("Guid Bytes: " + memoryDump);
                        }*/
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