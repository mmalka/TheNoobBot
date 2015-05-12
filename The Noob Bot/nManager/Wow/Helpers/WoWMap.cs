using System.Runtime.InteropServices;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWMap
    {
        private MapDbcRecord _rMapDBCRecord0;
        private static DBC<MapDbcRecord> _rMapDBC;

        private void init()
        {
            if (_rMapDBC == null)
                _rMapDBC = new DBC<MapDbcRecord>((int)Addresses.DBC.Map);
        }

        private WoWMap(string name, bool mpq = false)
        {
            init();
            for (int id = _rMapDBC.MinIndex; id <= _rMapDBC.MaxIndex; id++)
            {
                _rMapDBCRecord0 = _rMapDBC.GetRow(id);
                if (_rMapDBCRecord0.Id == id)
                {
                    string temp = (mpq ? MapName : MapMPQName);
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
            get
            {
                return _rMapDBC.String(_rMapDBC.GetRowOffset((int)_rMapDBCRecord0.Id) +
                                        _rMapDBCRecord0.MapNameOffset +
                                        (uint)Marshal.OffsetOf(typeof(MapDbcRecord), "MapNameOffset"));
            }
        }

        public string MapMPQName
        {
            get
            {
                return _rMapDBC.String(_rMapDBC.GetRowOffset((int)_rMapDBCRecord0.Id) +
                                        _rMapDBCRecord0.MPQDirectoryNameOffset +
                                        (uint)Marshal.OffsetOf(typeof(MapDbcRecord), "MPQDirectoryNameOffset"));
            }
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

        // Factory function
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

        public MapDbcRecord Record
        {
            get { return _rMapDBCRecord0; }
        }

        // data
        public enum InstanceType : uint
        {
            None = 0,
            Party = 1,
            Raid = 2,
            PvP = 3,
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

        [StructLayout(LayoutKind.Sequential)]
        public struct MapDbcRecord
        {
            public uint Id;
            public uint MPQDirectoryNameOffset;
            public InstanceType InstanceType;
            public uint Flags;
            public uint unk4;
            public MapType MapType;
            public uint MapNameOffset;
            public uint AreaTableId;
            public uint MapDescriptionHordeOffset;
            public uint MapDescriptionAllianceOffset;
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
        }
    }
}
