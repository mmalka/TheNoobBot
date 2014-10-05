using System.Collections.Generic;
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