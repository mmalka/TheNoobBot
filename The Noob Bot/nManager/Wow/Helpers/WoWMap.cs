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

        /*private int MinIndex
        {
            get
            {
                init();
                return _rMapDBC.MinIndex;
            }
        }

        private int MaxIndex
        {
            get
            {
                init();
                return _rMapDBC.MaxIndex;
            }
        }*/

        [StructLayout(LayoutKind.Sequential)]
        public struct MapDbcRecord
        {
            public uint Id;
            public uint MPQDirectoryNameOffset;
            public uint InstanceType;
            public uint Flags;
            public uint MapType;
            public uint unk5;
            public uint MapNameOffset;
            public uint AreaTableId;
            public uint MapDescriptionHordeOffset;
            public uint MapDescriptionAllianceOffset;
            public uint LoadingScreenId;
            public float MinimapIconScale;
            public uint CorpseMapId;
            public float CorpseX;
            public float CorpseY;
            public uint TimeOfDayOverride;
            public uint ExpansionId;
            public uint RaidOffset;
            public uint MaxPlayers;
            public uint ParentMapId;
            public uint CosmeticParentMapID;
            public uint TimeOffset;
        }
    }
}
