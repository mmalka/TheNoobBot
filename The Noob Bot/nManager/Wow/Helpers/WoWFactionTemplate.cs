using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWFactionTemplate
    {
        [CompilerGenerated]
        private FactionTemplateDbcRecord factionTemplateDbcRecord_0;
        [CompilerGenerated]
        private uint uint_0;

        private WoWFactionTemplate(uint id, uint row)
        {
            Id = id;
            Record = (FactionTemplateDbcRecord)Memory.WowMemory.Memory.ReadObject(row, typeof(FactionTemplateDbcRecord));
        }

        public static WoWFactionTemplate FromId(uint id)
        {
            uint row = DBCReading.GetAddressByIndex((int)id, DBCReading.GetWoWClientDBByAddress((uint)Addresses.UnitRelation.FACTION_POINTER));
            if (row == 0)
            {
                return null;
            }
            return new WoWFactionTemplate(id, row);
        }

        public Reaction GetReactionTowards(WoWFactionTemplate otherFaction)
        {
            FactionTemplateDbcRecord record = Record;
            FactionTemplateDbcRecord record2 = otherFaction.Record;
            if ((record2.FightSupport & record.HostileMask) != 0)
            {
                return Reaction.Hostile;
            }
            for (int i = 0; i < 4; i++)
            {
                if (record.EnemyFactions[i] == 0)
                {
                    break;
                }
                if (record.EnemyFactions[i] == record2.FactionId)
                {
                    return Reaction.Hostile;
                }
            }
            if ((record2.FightSupport & record.FriendlyMask) != 0)
            {
                return Reaction.Friendly;
            }
            for (int j = 0; j < 4; j++)
            {
                if (record.FriendlyFactions[j] == 0)
                {
                    break;
                }
                if (record.FriendlyFactions[j] == record2.FactionId)
                {
                    return Reaction.Friendly;
                }
            }
            if ((record.FightSupport & record2.FriendlyMask) != 0)
            {
                return Reaction.Friendly;
            }
            for (int k = 0; k < 4; k++)
            {
                if (record2.FriendlyFactions[k] == 0)
                {
                    break;
                }
                if (record2.FriendlyFactions[k] == record.FactionId)
                {
                    return Reaction.Friendly;
                }
            }
            uint num4 = (~(record.FactionFlags >> 12) & 2) | 1;
            return (Reaction)num4;
        }
/*
        public Reaction GetReactionTowards(WoWUnit unit)
        {
            WoWFactionTemplate factionTemplate = unit.FactionTemplate;
            if (factionTemplate != null)
            {
                WoWPlayer player;
                if ((unit.PlayerControlled && ((player = unit.ControllingPlayer) != null)) && player.IsMe)
                {
                    Reaction reaction;
                    FactionStanding standing;
                    if (((this.Record.FactionFlags & 0x1000) != 0) && player.ContestedPvPFlagged)
                    {
                        return Reaction.Hostile;
                    }
                    if (ObjectManager.Me.method_10(this.Record.FactionId, out reaction))
                    {
                        return reaction;
                    }
                    if (!unit.HasFlag(Enum10.flag_2) && ObjectManager.Me.GetFactionStanding(this.Faction, out standing))
                    {
                        return this.method_0(standing);
                    }
                }
                return this.GetReactionTowards(factionTemplate);
            }
            return Reaction.Neutral;
        }

        private Reaction method_0(FactionStanding standing)
        {
            int totalReputation = standing.TotalReputation;
            if (totalReputation >= 0xa410)
            {
                return Reaction.Exalted;
            }
            if (totalReputation >= 0x5208)
            {
                return Reaction.Revered;
            }
            if (totalReputation >= 0x2328)
            {
                return Reaction.Honored;
            }
            if (totalReputation >= 0xbb8)
            {
                return Reaction.Friendly;
            }
            if (totalReputation >= 0)
            {
                return Reaction.Neutral;
            }
            if (totalReputation >= -3000)
            {
                return Reaction.Unfriendly;
            }
            if (totalReputation >= -6000)
            {
                return Reaction.Hostile;
            }
            return Reaction.Hated;
        }

        public WoWFaction Faction
        {
            get
            {
                return WoWFaction.FromId(this.Record.FactionId);
            }
        }
        */
        public uint Id
        {
            [CompilerGenerated]
            get
            {
                return this.uint_0;
            }
            [CompilerGenerated]
            private set
            {
                this.uint_0 = value;
            }
        }

        public FactionTemplateDbcRecord Record
        {
            [CompilerGenerated]
            get
            {
                return this.factionTemplateDbcRecord_0;
            }
            [CompilerGenerated]
            private set
            {
                this.factionTemplateDbcRecord_0 = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FactionTemplateDbcRecord
        {
            public uint Id;
            public uint FactionId;
            public uint FactionFlags;
            public uint FightSupport;
            public uint FriendlyMask;
            public uint HostileMask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] EnemyFactions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] FriendlyFactions;
        }
    }
}
