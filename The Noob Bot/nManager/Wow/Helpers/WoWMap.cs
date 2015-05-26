using System.Runtime.InteropServices;
using nManager.Wow.Patchables;
using System.Collections.Generic;

namespace nManager.Wow.Helpers
{
    public class WoWMap
    {
        private MapDbcRecord _rMapDBCRecord0;
        private static DBC<MapDbcRecord> _rMapDBC;
        // Small list of unused map which have nor flags, nor type able to filter them
        private static List<uint> _blacklistedMaps = new List<uint>(new uint[] {930, 995, 1187});

        private static void init()
        {
            if (_rMapDBC == null)
                _rMapDBC = new DBC<MapDbcRecord>((int) Addresses.DBC.Map);
        }

        private WoWMap(string name, bool mpq = false)
        {
            init();
            for (int id = _rMapDBC.MinIndex; id <= _rMapDBC.MaxIndex; id++)
            {
                _rMapDBCRecord0 = _rMapDBC.GetRow(id);
                if (_rMapDBCRecord0.Id == id)
                {
                    string temp = (mpq ? MapMPQName : MapName);
                    if (temp == name)
                    {
                        return;
                    }
                }
            }
            _rMapDBCRecord0 = new MapDbcRecord();
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
            for (int id = _rMapDBC.MinIndex; id <= _rMapDBC.MaxIndex; id++)
            {
                _rMapDBCRecord0 = _rMapDBC.GetRow(id);
                if (_rMapDBCRecord0.Id == id && id == reqId)
                {
                    return;
                }
            }
            _rMapDBCRecord0 = new MapDbcRecord();
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
            List<MapDbcRecord> result = new List<MapDbcRecord>();
            for (int id = _rMapDBC.MinIndex; id <= _rMapDBC.MaxIndex; id++)
            {
                MapDbcRecord one = _rMapDBC.GetRow(id);
                if (!one.IsBlacklistedMap() && one.InstanceType == iType && one.MapType == mType && !one.IsTestMap() && !one.IsGarrisonMap())
                    result.Add(one);
            }
            return result;
        }

        // data
        public enum InstanceType : uint
        {
            None = 0,
            Party = 1,
            Raid = 2,
            Battleground = 3,
            Arena = 4,
            Scenario = 5,
        }

        public enum MapType : uint
        {
            ADTType = 1,
            WDTOnlyType = 2,
            TransportType = 3,
            WMOType = 4,
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
            public InstanceType InstanceType;
            public uint Flags;
            public uint unk4;
            public MapType MapType;
            private uint MapNameOffset;
            public uint AreaTableId;
            private uint MapDescriptionHordeOffset;
            private uint MapDescriptionAllianceOffset;
            public uint LoadingScreenId;
            public float MinimapIconScale;
            public int GhostEntranceMapId;
            public float GhostEntranceX;
            public float GhostEntranceY;
            public int TimeOfDayOverride;
            public uint ExpansionId;
            public uint RaidOffset;
            public uint MaxPlayers;
            public int ParentMapId;
            public int CosmeticParentMapID;
            public uint TimeOffset;

            public string MapMPQName()
            {
                return _rMapDBC.String(_rMapDBC.GetRowOffset((int) Id) + MPQDirectoryNameOffset +
                                       (uint) Marshal.OffsetOf(typeof (MapDbcRecord), "MPQDirectoryNameOffset"));
            }

            public string MapName()
            {
                return _rMapDBC.String(_rMapDBC.GetRowOffset((int) Id) + MapNameOffset +
                                       (uint) Marshal.OffsetOf(typeof (MapDbcRecord), "MapNameOffset"));
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