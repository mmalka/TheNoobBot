using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Class;
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
            try
            {
                if (buffId == null || buffId.Count <= 0)
                    return 0;

                uint auraTableBase = baseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraTable1;

                int auraCount = Memory.WowMemory.Memory.ReadInt(auraTableBase + (uint) Addresses.UnitBaseGetUnitAura.AuraTable2);

                if (auraCount == -1)
                    auraCount = Memory.WowMemory.Memory.ReadInt(auraTableBase);
                for (uint currentIndex = 0; currentIndex < auraCount; currentIndex++)
                {
                    uint currentAuraPtr = GetAuraPtrByIndex(auraTableBase, currentIndex);

                    if (currentAuraPtr > 0)
                    {
                        // ReSharper disable UnusedVariable
                        UInt128 auraCreatorGuid = Memory.WowMemory.Memory.ReadUInt128(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructCreatorGuid);
                        uint auraSpellId = Memory.WowMemory.Memory.ReadUInt(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructSpellId);
                        //string auraSpellName = SpellManager.GetSpellInfo(Memory.WowMemory.Memory.ReadUInt(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructSpellId)).Name;
                        byte auraFlags = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint)Addresses.UnitBaseGetUnitAura.AuraStructFlags);
                        byte auraStackCount = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructCount);
                        byte auraCasterLevel = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructCasterLevel);
                        byte auraUnk2 = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructUnk2);
                        uint auraDuration = Memory.WowMemory.Memory.ReadUInt(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructDuration);
                        uint auraSpellEndTime = Memory.WowMemory.Memory.ReadUInt(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructSpellEndTime);
                        uint auraTimeLeftInMs = auraSpellEndTime - Usefuls.GetWoWTime;
                        byte auraUnk3 = Memory.WowMemory.Memory.ReadByte(currentAuraPtr + (uint) Addresses.UnitBaseGetUnitAura.AuraStructUnk3); // ReSharper restore UnusedVariable

                        /*if (auraSpellId == 324)
                        {
                            Logging.WriteDebug("AuraCreatorGuid: " + auraCreatorGuid);
                            Logging.WriteDebug("AuraSpellId: " + auraSpellId);
                            //Logging.WriteDebug("AuraName: " + auraSpellName);
                            Logging.WriteDebug("AuraFlags: " + auraFlags);
                            Logging.WriteDebug("AuraCount: " + auraStackCount);
                            Logging.WriteDebug("AuraCasterLevel: " + auraCasterLevel);
                            Logging.WriteDebug("AuraUnk2: " + auraUnk2);
                            Logging.WriteDebug("AuraDuration: " + auraDuration);
                            Logging.WriteDebug("AuraSpellEndTime: " + auraSpellEndTime);
                            Logging.WriteDebug("GetTime: " + Usefuls.GetWoWTime);
                            Logging.WriteDebug("AuraTimeLeftInMs: " + auraTimeLeftInMs);
                            Logging.WriteDebug("AuraStructUnk3: " + auraUnk3);
                            Thread.Sleep(200);
                        }*/
                        if (auraSpellId > 0 && buffId.Contains(auraSpellId))
                            return auraStackCount;
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

        private static uint GetAuraPtrByIndex(uint auraBase, uint currentIndex)
        {
            try
            {
                uint result;

                uint currentAura = (uint) Addresses.UnitBaseGetUnitAura.AuraSize*currentIndex;
                if (Memory.WowMemory.Memory.ReadUInt(auraBase + (uint) Addresses.UnitBaseGetUnitAura.AuraTable2) == -1)
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