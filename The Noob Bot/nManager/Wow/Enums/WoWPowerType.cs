namespace nManager.Wow.Enums
{
    public enum PowerType
    {
        Health = -0x2, // only LUA
        Mana = 0x0,
        Rage = 0x1,
        Focus = 0x2,
        Energy = 0x3,
        ComboPoint = 0x4,
        Runes = 0x5,
        RunicPower = 0x6,
        SoulShards = 0x7,
        Eclipse = 0x8,
        HolyPower = 0x9,
        Alternate = 0xA, // Alternate bar. (Example: Atramedes Song bar)
        Maelstrom = 0xB,
        LightForce = 0xC, // Previous PowerType of the Monk. Now called Chi.
        Chi = 0xC,
        ShadowOrbs = 0xD,
        BurningEmbers = 0xE,
        DemonicFury = 0xF,
        ArcaneCharges = 0x10, // Not proven to really exist, need testing on Mage.
        Fury = 0x11,
    };
}