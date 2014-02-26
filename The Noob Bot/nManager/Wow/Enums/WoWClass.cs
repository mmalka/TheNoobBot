namespace nManager.Wow.Enums
{
    public enum WoWClass : uint
    {
        None = 0,
        Warrior = 1,
        Paladin = 2,
        Hunter = 3,
        Rogue = 4,
        Priest = 5,
        DeathKnight = 6,
        Shaman = 7,
        Mage = 8,
        Warlock = 9,
        Monk = 10,
        Druid = 11,
    }

    public enum WoWClassMask : uint
    {
        None = 0x0,
        Warrior = 0x1,
        Paladin = 0x2,
        Hunter = 0x4,
        Rogue = 0x8,
        Priest = 0x16,
        DeathKnight = 0x32,
        Shaman = 0x64,
        Mage = 0x128,
        Warlock = 0x256,
        Monk = 0x512,
        Druid = 0x1024,
    }
}