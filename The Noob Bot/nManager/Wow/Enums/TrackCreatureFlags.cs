using System;

namespace nManager.Wow.Enums
{
    [Flags]
    public enum TrackCreatureFlags
    {
        All = -1,
        Beasts = 1,
        Critters = 0x80,
        Demons = 4,
        Dragons = 2,
        Elementals = 8,
        GasCloud = 0x1000,
        Giants = 0x10,
        Humanoids = 0x40,
        Machines = 0x100,
        NonCombatPet = 0x800,
        Slimes = 0x200,
        Totem = 0x400,
        Undead = 0x20
    }
}

