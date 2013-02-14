namespace meshDatabase.Database
{
    
    public enum MapType
    {
        World = 0,
        Instance = 1,
        RaidInstance = 2,
        Battleground = 3,
        Arena = 4
    }

    public class MapEntry
    {
        public int Id { get; private set; }
        public string InternalName { get; private set; }
        public string Name { get; private set; }
        public MapType Type { get; private set; }
        public int PhaseParent { get; private set; }

        public bool IsPhase
        {
            get { return PhaseParent > 1; }
        }

        public MapEntry(Record rec)
        {
            Id = rec[0];
            InternalName = rec.GetString(1);
            Name = rec.GetString(5);
            Type = (MapType) rec[2];
            PhaseParent = rec[18];
        }
       
    }

}