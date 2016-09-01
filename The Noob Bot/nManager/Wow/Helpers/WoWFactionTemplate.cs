using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWFactionTemplate
    {
        [CompilerGenerated] private FactionTemplateDbcRecord factionTemplateDbcRecord_0;
        [CompilerGenerated] private uint uint_0;
        private static DB5Reader factionTemplateDB2;
        private static BinaryReader[] _cachedFactionTemplateRows;
        private static FactionTemplateDbcRecord[] _cachedRecords;

        private static void init()
        {
            if (factionTemplateDB2 == null)
            {
                var m_definitions = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                var definitions = m_definitions.Tables.Where(t => t.Name == "FactionTemplate");

                if (!definitions.Any())
                {
                    definitions = m_definitions.Tables.Where(t => t.Name == Path.GetFileName("FactionTemplate"));
                }
                if (definitions.Count() == 1)
                {
                    var table = definitions.First();
                    factionTemplateDB2 = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\FactionTemplate.db2", table) as DB5Reader;
                    if (_cachedFactionTemplateRows == null)
                    {
                        if (factionTemplateDB2 != null)
                        {
                            _cachedFactionTemplateRows = factionTemplateDB2.Rows.ToArray();
                            _cachedRecords = new FactionTemplateDbcRecord[_cachedFactionTemplateRows.Length];
                            for (int i = 0; i < _cachedFactionTemplateRows.Length - 1; i++)
                            {
                                _cachedRecords[i] = DB5Reader.ByteToType<FactionTemplateDbcRecord>(_cachedFactionTemplateRows[i]);
                            }
                        }
                    }
                    Logging.Write(factionTemplateDB2.FileName + " loaded with " + factionTemplateDB2.RecordsCount + " entries.");
                }
                else
                {
                    Logging.Write("DB2 FactionTemplate not read-able.");
                }
            }
        }

        private WoWFactionTemplate(uint reqId)
        {
            init();
            FactionTemplateDbcRecord tempfactionTemplateDBC = new FactionTemplateDbcRecord();
            bool found = false;
            for (int i = 0; i < factionTemplateDB2.RecordsCount - 1; i++)
            {
                tempfactionTemplateDBC = _cachedRecords[i];
                if (tempfactionTemplateDBC.Id == reqId)
                {
                    found = true;
                    break;
                }
            }
            Record = found ? tempfactionTemplateDBC : new FactionTemplateDbcRecord();
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
            if (record.EnemyFactions != null)
            {
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
            }
            if ((record2.FightSupport & record.FriendlyMask) != 0)
            {
                return Reaction.Friendly;
            }
            if (record.FriendlyFactions != null)
            {
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
            }
            if ((record.FightSupport & record2.FriendlyMask) != 0)
            {
                return Reaction.Friendly;
            }
            if (record2.FriendlyFactions != null)
            {
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
            }
            uint num4 = (uint) ((~(record.FactionFlags >> 12) & 2) | 1);
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
            public ushort FactionId;
            public ushort FactionFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public ushort[] EnemyFactions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public ushort[] FriendlyFactions;
            public byte FightSupport;
            public byte FriendlyMask;
            public byte HostileMask;
            public byte Unknown;
        }
    }
}