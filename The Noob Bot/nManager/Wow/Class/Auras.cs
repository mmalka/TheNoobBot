using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;

namespace nManager.Wow.Class
{
    public class Auras
    {
        public class UnitAura
        {
            public UInt128 AuraCreatorGUID { get; set; }

            public bool IsValid
            {
                get { return (AuraCount != -1 && AuraCreatorGUID > 0); }
            }

            public uint AuraSpellId { get; set; }
            public int AuraCount { get; set; }
            public byte AuraCasterLevel { get; set; }
            public byte AuraFlag { get; set; }
            public uint AuraMask { get; set; }
            public byte AuraUnk1 { get; set; }
            public int AuraDuration { get; set; }
            public int AuraSpellEndTime { get; set; }
            public uint AuraUnk2 { get; set; }

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
                if (Cancellable)
                {
                    Lua.LuaDoString(string.Format("for i = 1,40 do local spellId = select(11, UnitAura('player', i)) if spellId == {0} then CancelUnitBuff('player', i) end end", AuraSpellId));
                }
            }

            public override string ToString()
            {
                return string.Format(
                    "AuraCasterLevel: {1}{0}" +
                    "AuraCount: {2}{0}" +
                    "AuraCreatorGUID: {3}{0}" +
                    "AuraDuration: {4}{0}" +
                    "AuraFlags: {5}{0}" +
                    "AuraSpellEndTime: {6}{0}" +
                    "AuraSpellId: {7}{0}" +
                    "AuraTimeLeftInMs: {8}{0}" +
                    "AuraMask: {9}{0}" +
                    "AuraUnk1: {10}{0}" +
                    "AuraUnk2: {11}{0}", Environment.NewLine, AuraCasterLevel, AuraCount, AuraCreatorGUID, AuraDuration, AuraFlag, AuraSpellEndTime, AuraSpellId, AuraTimeLeftInMs, AuraMask.ToString("X8"), AuraUnk1, AuraUnk2);
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