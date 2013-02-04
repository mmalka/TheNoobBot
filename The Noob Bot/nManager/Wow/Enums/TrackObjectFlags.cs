using System;

namespace nManager.Wow.Enums
{
    [Flags]
    public enum TrackObjectFlags
    {
        All = -1,
        ArmTrap = 0x100,
        Blasting = 0x8000,
        CalcifiedElvenGems = 0x40,
        Close = 0x80,
        DisarmTrap = 8,
        Fishing = 0x40000,
        Gahzridian = 0x4000,
        Herbs = 2,
        Inscription = 0x80000,
        Lockpicking = 1,
        Minerals = 4,
        None = 0,
        Open = 0x10,
        OpenAttacking = 0x2000,
        OpenFromVehicle = 0x100000,
        OpenKneeling = 0x1000,
        OpenTinkering = 0x800,
        PvPClose = 0x20000,
        PvPOpen = 0x10000,
        QuickClose = 0x400,
        QuickOpen = 0x200,
        Treasure = 0x20
    }
}