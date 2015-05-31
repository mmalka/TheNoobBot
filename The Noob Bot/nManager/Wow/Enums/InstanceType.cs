namespace nManager.Wow.Enums
{
    public enum InstanceType
    {
        // This is not "Map.InstanceType"
        Dungeon = 0, // 5-man dungeons
        Scenario = 1,
        Raid = 2, // Raids before SoO
        MythicRaid = 3, // Raids since SoO
        HeroicDungeon = 4, // Dungeon with Heroic Mode
        MythicDungeon = 5, // Dungeon with Mythic Mode
    }
}