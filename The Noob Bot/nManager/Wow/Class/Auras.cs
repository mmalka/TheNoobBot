using System.Collections.Generic;
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
                get { return (AuraCount != -1); }
            }

            public uint AuraSpellId { get; set; }
            public byte AuraFlags { get; set; }
            public int AuraCount { get; set; }
            public byte AuraCasterLevel { get; set; }
            public byte AuraUnk2 { get; set; }
            public uint AuraDuration { get; set; }
            public uint AuraSpellEndTime { get; set; }
            public byte AuraUnk3 { get; set; }

            public uint AuraTimeLeftInMs
            {
                get { return AuraSpellEndTime - Usefuls.GetWoWTime; }
            }

            public bool Cancellable
            {
                get { return Flags.HasFlag(UnitAuraFlags.Cancelable); }
            }

            public UnitAuraFlags Flags
            {
                get { return (UnitAuraFlags) AuraFlags; }
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
        }

        public class UnitAuras
        {
            public UnitAuras(uint baseAddress)
            {
                UnitBaseAddress = baseAddress;
            }

            private uint UnitBaseAddress { set; get; }
            public List<UnitAura> Auras { set; get; }
        }
    }
}