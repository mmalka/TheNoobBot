using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using meshDatabase.Database;

namespace meshDatabase
{
    public class WoWMap
    {
        private MapDbcRecord _rMapDBCRecord0;
        private static MapDbcRecord[] _cachedRecords;
        private static BinaryReader[] _cachedMapRows;

        // Small list of unused map which have nor flags, nor type able to filter them
        private static List<uint> _blacklistedMaps = new List<uint>(new uint[] {930, 995, 1187});

        private static DB6Reader mapDB2;

        private static void init()
        {
            if (mapDB2 == null)
            {
                var m_definitions = DBFilesClient.Load(@"E:\Développement\TheNoobBot\TheNoobBot\docs\updatebase_directory\Data\DBFilesClient\dblayout.xml");
                var definitions = m_definitions.Tables.Where(t => t.Name == "Map");

                if (!definitions.Any())
                {
                    definitions = m_definitions.Tables.Where(t => t.Name == Path.GetFileName("Map"));
                }
                if (definitions.Count() == 1)
                {
                    var table = definitions.First();
                    mapDB2 = DBReaderFactory.GetReader(@"E:\Développement\TheNoobBot\TheNoobBot\docs\updatebase_directory\Data\DBFilesClient\Map.db2", table) as DB6Reader;
                    if (_cachedMapRows == null || _cachedRecords == null)
                    {
                        if (mapDB2 != null)
                        {
                            _cachedMapRows = mapDB2.Rows.ToArray();
                            _cachedRecords = new MapDbcRecord[_cachedMapRows.Length];
                            for (int i = 0; i < _cachedMapRows.Length - 1; i++)
                            {
                                _cachedRecords[i] = DB6Reader.ByteToType<MapDbcRecord>(_cachedMapRows[i]);
                            }
                        }
                    }
                    //Logging.Write(mapDB2.FileName + " loaded with " + mapDB2.RecordsCount + " entries.");
                }
                else
                {
                    //Logging.Write("DB2 Map not read-able.");
                }
            }
        }

        private WoWMap(string name, bool mpq = false)
        {
            init();
            MapDbcRecord tempMapDbcRecord = new MapDbcRecord();
            bool found = false;
            for (int i = 0; i < mapDB2.RecordsCount - 1; i++)
            {
                tempMapDbcRecord = _cachedRecords[i];
                string temp = (mpq ? tempMapDbcRecord.MapMPQName() : tempMapDbcRecord.MapName());
                if (temp == name)
                {
                    found = true;
                    break;
                }
            }
            _rMapDBCRecord0 = found ? tempMapDbcRecord : new MapDbcRecord();
        }

        public string MapName
        {
            get { return _rMapDBCRecord0.MapName(); }
        }

        public string MapMPQName
        {
            get { return _rMapDBCRecord0.MapMPQName(); }
        }

        public bool IsTestMap
        {
            get { return _rMapDBCRecord0.IsTestMap(); }
        }

        public bool IsGarrisonMap
        {
            get { return _rMapDBCRecord0.IsGarrisonMap(); }
        }

        private WoWMap(int reqId)
        {
            init();
            MapDbcRecord tempMapDbcRecord = new MapDbcRecord();
            bool found = false;
            for (int i = 0; i < mapDB2.RecordsCount - 1; i++)
            {
                tempMapDbcRecord = _cachedRecords[i];
                if (tempMapDbcRecord.Id == reqId)
                {
                    found = true;
                    break;
                }
            }
            _rMapDBCRecord0 = found ? tempMapDbcRecord : new MapDbcRecord();
        }

        public MapDbcRecord Record
        {
            get { return _rMapDBCRecord0; }
        }

        // Factory functions
        public static WoWMap FromId(int id)
        {
            return new WoWMap(id);
        }

        public static WoWMap FromName(string name)
        {
            return new WoWMap(name, false);
        }

        public static WoWMap FromMPQName(string name)
        {
            return new WoWMap(name, true);
        }

        public static List<MapDbcRecord> WoWMaps(InstanceType iType, MapType mType)
        {
            init();

            MapDbcRecord tempMapDbcRecord = new MapDbcRecord();
            List<MapDbcRecord> result = new List<MapDbcRecord>();
            for (int i = 0; i < mapDB2.RecordsCount - 1; i++)
            {
                tempMapDbcRecord = _cachedRecords[i];
                if (!tempMapDbcRecord.IsBlacklistedMap() && tempMapDbcRecord.InstanceType == iType && tempMapDbcRecord.MapType == mType && !tempMapDbcRecord.IsTestMap() && !tempMapDbcRecord.IsGarrisonMap())
                    result.Add(tempMapDbcRecord);
            }
            return result;
        }

        // data

        public enum ExtensionId : byte
        {
            Vanilla = 0,
            TBC = 1,
            WoTLK = 2,
            Cataclysm = 3,
            MoP = 4,
            WoD = 5,
            Legion = 6,
        }

        private enum MapFlags : uint
        {
            MAP_FLAG_TEST_MAP = 0x002,
            MAP_FLAG_NOT_EXISTING = 0x080, // This returns 2 maps not in CASC (CraigTest (597) and Deephomeceiling (660))
            MAP_FLAG_DYNAMIC_DIFFICULTY = 0x100,
            // 0x20000 = Phased world map with adt
            MAP_FLAG_GARRISON = 0x4000000,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MapDbcRecord
        {
            public uint Id;
            private uint MPQDirectoryNameOffset;
            public int Flags;
            public int Flags2;
            private int field10; // looks like the pointer we used to use for strings
            public int field14_0;
            public int field14_1;
            private int MapNameOffset;
            private int MapDescriptionHordeOffset;
            private int MapDescriptionAllianceOffset;
            public ushort field28;
            public ushort field2A;
            public ushort field2C;
            public ushort field2E;
            public ushort field30;
            public ushort field32;
            public ushort field34;
            public InstanceType InstanceType;
            public MapType MapType;
            public ExtensionId ExtensionId;
            public byte PhaseParent;
            public byte field3A_0;
            public byte field3A_1;

            public bool IsPhase
            {
                get { return PhaseParent > 1; }
            }

            public string MapMPQName()
            {
                string strValue = "";
                if (PhaseHelper._map.StringTable != null && PhaseHelper._map.StringTable.TryGetValue((int) MPQDirectoryNameOffset, out strValue))
                {
                    return strValue;
                }
                return "";
                /*return _rMapDBC.String(_rMapDBC.GetRowOffset((int) Id) + MPQDirectoryNameOffset +
                                       (uint) Marshal.OffsetOf(typeof (MapDbcRecord), "MPQDirectoryNameOffset"));*/
            }

            public string MapName()
            {
                string strValue = "";
                if (PhaseHelper._map.StringTable != null && PhaseHelper._map.StringTable.TryGetValue((int) MapNameOffset, out strValue))
                {
                    return strValue;
                }
                return "";
                /*return _rMapDBC.String(_rMapDBC.GetRowOffset((int) Id) + MapNameOffset +
                                       (uint) Marshal.OffsetOf(typeof (MapDbcRecord), "MapNameOffset"));*/
            }

            public bool IsTestMap()
            {
                return (Flags & (uint) MapFlags.MAP_FLAG_TEST_MAP) != 0 ||
                       (Flags & (uint) MapFlags.MAP_FLAG_NOT_EXISTING) != 0;
            }

            public bool IsGarrisonMap()
            {
                return (Flags & (uint) MapFlags.MAP_FLAG_GARRISON) != 0;
            }

            public bool IsBlacklistedMap()
            {
                return _blacklistedMaps.Contains(Id);
            }
        }
    }
}