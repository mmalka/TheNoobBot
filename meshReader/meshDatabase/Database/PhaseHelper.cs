using System.Collections.Generic;
using System.Linq;

namespace meshDatabase.Database
{
    
    public static class PhaseHelper
    {
        private static DBC _map;
        private static bool _initialized;
        private static IEnumerable<MapEntry> _entries;
        private static List<int> _blacklistedMaps = new List<int>(new int[] { 930, 995, 1187 });

        public static void Initialize()
        {
            if (_initialized)
                return;

            _map = MpqManager.GetDBC("Map");
            _entries = _map.Records.Select(r => new MapEntry(r));
            _initialized = true;
        }

        public static List<MapEntry> GetAllMaps()
        {
            Initialize();
            return _entries.ToList();
        }

        public static int GetMapIdByName(string search)
        {
            Initialize();

            var entry = _entries.Where(e => e.Name == search || e.InternalName == search).FirstOrDefault();
            if (entry == null)
                return -1;
            return entry.Id;
        }

        public static string GetParentMap(string internalMapName)
        {
            Initialize();

            MapEntry root = _entries.FirstOrDefault(entry => entry.InternalName == internalMapName);

            if (root == null || root.PhaseParent == -1)
                return string.Empty;

            return _entries.Where(entry => entry.Id == root.PhaseParent).FirstOrDefault().InternalName;
        }

        public static List<MapEntry> GetPhasesByMap(string internalMapName)
        {
            Initialize();

            MapEntry root = _entries.FirstOrDefault(entry => entry.InternalName == internalMapName);

            if (root == null)
                return null;

            return _entries.Where(entry => entry.IsPhase && entry.PhaseParent == root.Id).ToList();
        }

        public static List<MapEntry> GetGarrisonMaps()
        {
            Initialize();

            return _entries.Where(entry => entry.IsGarrisonMap).ToList();
        }

        public static List<MapEntry> GetAllMapOfInstanceType(InstanceType iType, MapType mType)
        {
            Initialize();

            return _entries.Where(entry => !_blacklistedMaps.Contains(entry.Id) && entry.InstanceType == iType && entry.MapType == mType && !entry.IsTestMap && !entry.IsGarrisonMap).ToList();
        }

        public static string GetMapInternalNameFromName(string name)
        {
            var entry = _entries.Where(e => e.Name == name).FirstOrDefault();
            if (entry == null)
                return string.Empty;
            return entry.InternalName;
        }
    }

}