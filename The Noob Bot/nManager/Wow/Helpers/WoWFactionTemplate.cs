using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWFactionTemplate
    {
        [CompilerGenerated] private FactionTemplateDbcRecord factionTemplateDbcRecord_0;
        [CompilerGenerated] private uint uint_0;

        private static DBC<FactionTemplateDbcRecord> factionTemplateDBC;

        private WoWFactionTemplate(uint id)
        {
            Id = id;
            if (factionTemplateDBC == null)
                factionTemplateDBC = new DBC<FactionTemplateDbcRecord>((int) Addresses.DBC.FactionTemplate);
            Record = factionTemplateDBC.GetRow((int) id);
        }

        public static WoWFactionTemplate FromId(uint id)
        {
            return new WoWFactionTemplate(id);
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
            return (Reaction) num4;
        }

        public uint Id
        {
            [CompilerGenerated] get { return uint_0; }
            [CompilerGenerated] private set { uint_0 = value; }
        }

        public FactionTemplateDbcRecord Record
        {
            [CompilerGenerated] get { return factionTemplateDbcRecord_0; }
            [CompilerGenerated] private set { factionTemplateDbcRecord_0 = value; }
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public uint[] EnemyFactions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public uint[] FriendlyFactions;
        }
    }
}