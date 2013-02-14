using System.Collections.Generic;
using System.Linq;

namespace meshDatabase.Database
{
    
    public static class PhaseHelper
    {
        private static DBC _map;
        private static bool _initialized;
        private static IEnumerable<MapEntry> _entries;

        public static void Initialize()
        {
            if (_initialized)
                return;

            _map = MpqManager.GetDBC("Map");
            _entries = _map.Records.Select(r => new MapEntry(r));
            _initialized = true;
        }

        public static int GetMapIdByName(string search)
        {
            Initialize();

            var entry = _entries.Where(e => e.Name == search || e.InternalName == search).FirstOrDefault();
            if (entry == null)
                return -1;
            return entry.Id;
        }

        public static List<MapEntry> GetPhasesByMap(string internalMapName)
        {
            Initialize();

            MapEntry root = _entries.FirstOrDefault(entry => entry.InternalName == internalMapName);

            if (root == null)
                return null;

            return _entries.Where(entry => entry.IsPhase && entry.PhaseParent == root.Id).ToList();
        }
    }

}