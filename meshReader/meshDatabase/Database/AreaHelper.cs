using System.Collections.Generic;
using System.Linq;

namespace meshDatabase.Database
{
    
    public static class AreaHelper
    {
        private static bool _initialized;
        private static List<WorldMapArea> _data;

        /// <summary>
        /// Initialize the helper. Will be automatically called by all other helper functions that require the data.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized)
                return;

            var dbc = MpqManager.GetDBC("WorldMapArea");
            _data = dbc.Records.Select(r => new WorldMapArea(r)).ToList();
            _initialized = true;
        }

        public static WorldMapArea GetAreaByName(string name)
        {
            Initialize();
            return _data.FirstOrDefault(d => d.Name == name);
        }

        /// <summary>
        /// These are the zone ids from WoWHead for example
        /// </summary>
        /// <param name="id">zone id (e.g. 14 for Durotar)</param>
        /// <returns></returns>
        public static WorldMapArea GetAreaByZoneId(int id)
        {
            Initialize();
            return _data.FirstOrDefault(d => d.Area == id);
        }
    }

}