using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;

namespace nManager.Wow.Class
{
    public class Auras
    {
        public class UnitAura
        {
            public uint BaseAddress { get; set; }

            public bool IsValid
            {
                get { return BaseAddress > 0 && (AuraCount != -1 && AuraCreatorGUID > 0); }
            }

            public UInt128 AuraCreatorGUID
            {
                get { return Memory.WowMemory.Memory.ReadUInt128(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructCreatorGuid); }
            }

            public UInt128 AuraUnknownGUID
            {
                get { return Memory.WowMemory.Memory.ReadUInt128(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructUnkwnonGuid); }
            }

            public uint AuraSpellId { get; set; }

            public int AuraCount
            {
                get { return Memory.WowMemory.Memory.ReadByte(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructCount); }
            }

            public byte AuraCasterLevel
            {
                get { return Memory.WowMemory.Memory.ReadByte(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructCasterLevel); }
            }

            public byte AuraFlag
            {
                get { return Memory.WowMemory.Memory.ReadByte(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructFlag); }
            }

            public uint AuraMask
            {
                get { return Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructMask); }
            }

            public byte AuraUnk1
            {
                get { return Memory.WowMemory.Memory.ReadByte(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructUnk1); }
            }

            public int AuraDuration
            {
                get { return Memory.WowMemory.Memory.ReadInt(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructDuration); }
            }

            public int AuraSpellEndTime
            {
                get { return Memory.WowMemory.Memory.ReadInt(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructSpellEndTime); }
            }

            public uint AuraUnk2
            {
                get { return Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.UnitBaseGetUnitAura.AuraStructUnk2); }
            }

            public int AuraTimeLeftInMs
            {
                get
                {
                    if (AuraSpellEndTime == 0)
                        return 0;
                    return AuraSpellEndTime - Usefuls.GetWoWTime;
                }
            }

            public bool Cancellable
            {
                get { return Flags.HasFlag(UnitAuraFlags.Cancelable); }
            }

            public UnitAuraFlags Flags
            {
                get { return (UnitAuraFlags) AuraFlag; }
            }

            public bool IsActive
            {
                get { return Flags.HasFlag(UnitAuraFlags.Active); }
            }

            public bool IsHarmful
            {
                get { return Flags.HasFlag(UnitAuraFlags.Harmful); }
            }

            public bool IsPassive
            {
                get { return Flags.HasFlag(UnitAuraFlags.Passive) || Flags.HasFlag(UnitAuraFlags.None); }
            }

            public void TryCancel()
            {
                Lua.LuaDoString(string.Format("for i = 1,40 do local spellId = select(11, UnitAura('player', i)) if spellId == {0} then CancelUnitBuff('player', i) end end", AuraSpellId));
            }

            public override string ToString()
            {
                return string.Format(
                    "AuraCasterLevel: {1}{0}" +
                    "AuraCount: {2}{0}" +
                    "AuraCreatorGUID: {3}{0}" +
                    "AuraCreatorGUID: {4}{0}" +
                    "AuraDuration: {5}{0}" +
                    "AuraFlags: {6}{0}" +
                    "AuraSpellEndTime: {7}{0}" +
                    "AuraSpellId: {8}{0}" +
                    "AuraSpellName: {9}{0}" +
                    "AuraTimeLeftInMs: {10}{0}" +
                    "AuraMask: {11}{0}" +
                    "AuraUnk1: {12}{0}" +
                    "AuraUnk2: {13}{0}",
                    Environment.NewLine, AuraCasterLevel, AuraCount, AuraCreatorGUID, AuraUnknownGUID,
                    AuraDuration, AuraFlag, AuraSpellEndTime, AuraSpellId, new Spell(AuraSpellId).NameInGame,
                    AuraTimeLeftInMs, AuraMask.ToString("X8"), AuraUnk1, AuraUnk2);
            }
        }

        public class UnitAuras
        {
            public UnitAuras(uint baseAddress)
            {
                UnitBaseAddress = baseAddress;
                Auras = new List<UnitAura>();
            }

            private uint UnitBaseAddress { set; get; }
            public List<UnitAura> Auras { set; get; }
        }
    }
}