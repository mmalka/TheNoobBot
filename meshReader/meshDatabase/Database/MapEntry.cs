namespace meshDatabase.Database
{
    public enum InstanceType : byte
    {
        World = 0,
        Instance = 1,
        RaidInstance = 2,
        Battleground = 3,
        Arena = 4,
        Scenario = 5
    }

    public enum MapType : byte
    {
        ADTType = 1,
        WDTOnlyType = 2,
        TransportType = 3,
        WMOType = 4,
    }

    public enum MapFlag : uint
    {
        MAP_FLAG_TEST_MAP = 0x002,
        MAP_FLAG_NOT_EXISTING = 0x080, // This returns 2 maps not in CASC (CraigTest (597) and Deephomeceiling (660))
        MAP_FLAG_DYNAMIC_DIFFICULTY = 0x100,
        // 0x8000 = only about half of raids
        // 0x20000 = Phased world map with adt
        // 0x40000 = new race zones of cataclysm
        // 0x1000000 = World map only, but missing Draenor
        // 0x04 or 0x08 or 0x40 or 0x800 = seams to flag all non ADT map
        MAP_FLAG_GARRISON = 0x4000000,
    }

    public class MapEntry
    {
        public int Id { get; private set; }
        public string InternalName { get; private set; }
        public string Name { get; private set; }
        public InstanceType InstanceType { get; private set; }
        public MapType MapType { get; private set; }
        public int PhaseParent { get; private set; }
        public uint Flags { get; private set; }

        public bool IsPhase
        {
            get { return PhaseParent > 1; }
        }

        public bool IsGarrisonMap
        {
            get { return (Flags & (uint)MapFlag.MAP_FLAG_GARRISON) != 0; }
        }
        public bool IsTestMap
        {
            get { return (Flags & (uint)MapFlag.MAP_FLAG_TEST_MAP) != 0 || (Flags & (uint)MapFlag.MAP_FLAG_NOT_EXISTING) != 0; }
        }

        public MapEntry(Record rec)
        {
            Id = rec[0];
            InternalName = rec.GetString(1);
            InstanceType = (InstanceType)rec[2];
            Flags = (uint)rec[3];
            MapType = (MapType)rec[5];
            Name = rec.GetString(6);
            PhaseParent = rec[19];
        }
       
    }
}