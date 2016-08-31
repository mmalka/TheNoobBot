using System.Collections.Generic;
using System.Linq;

namespace meshDatabase.Database
{
    
    public static class PhaseHelper
    {
        public static DB5Reader _map;
        private static bool _initialized;
        private static IEnumerable<WoWMap.MapDbcRecord> _entries;
        private static List<int> _blacklistedMaps = new List<int>(new int[] { 930, 995, 1187 });

        public static void Initialize()
        {
            if (_initialized)
                return;


            _map = MpqManager.GetDB5("Map");
            _entries = _map.Rows.Select(DB5Reader.ByteToType<WoWMap.MapDbcRecord>);
            _initialized = true;
        }

        public static List<WoWMap.MapDbcRecord> GetAllMaps()
        {
            Initialize();
            return _entries.ToList();
        }

        public static int GetMapIdByName(string search)
        {
            Initialize();

            var entry = _entries.FirstOrDefault(e => e.MapName() == search || e.MapMPQName() == search);
            return (int) entry.Id;
        }

        public static string GetParentMap(string internalMapName)
        {
            Initialize();

            WoWMap.MapDbcRecord root = _entries.FirstOrDefault(entry => entry.MapMPQName() == internalMapName);

            if (root.PhaseParent == 0)
                return string.Empty;

            return _entries.FirstOrDefault(entry => entry.Id == root.PhaseParent).MapMPQName();
        }

        public static List<WoWMap.MapDbcRecord> GetPhasesByMap(string internalMapName)
        {
            Initialize();

            WoWMap.MapDbcRecord root = _entries.FirstOrDefault(entry => entry.MapMPQName() == internalMapName);
            
            return _entries.Where(entry => entry.IsPhase && entry.PhaseParent == root.Id).ToList();
        }

        public static List<WoWMap.MapDbcRecord> GetGarrisonMaps()
        {
            Initialize();

            return _entries.Where(entry => entry.IsGarrisonMap()).ToList();
        }

        public static List<WoWMap.MapDbcRecord> GetAllMapOfInstanceType(InstanceType iType, MapType mType)
        {
            Initialize();
            var list = new List<WoWMap.MapDbcRecord>();
            foreach (WoWMap.MapDbcRecord entry in _entries)
            {
                if (_blacklistedMaps.Contains((int) entry.Id))
                    continue;
                if (entry.InstanceType != iType)
                    continue;
                if (entry.MapType != mType)
                    continue;
                if (entry.IsTestMap() || entry.IsGarrisonMap())
                    continue;
                list.Add(entry);
            }
            return list;
        }

        public static string GetMapInternalNameFromName(string name)
        {
            var entry = _entries.FirstOrDefault(e => e.MapName() == name);
            return entry.MapMPQName();
        }
    }

}