using System;

namespace nManager.Wow.Enums
{
    [Flags]
    public enum UnitFlags : uint
    {
        None = 0x0,
        ServerControlled = 0x1,
        PlayerCannotAttack = 0x2, // .Attackable check
        Influenced = 0x4,
        PVPAttackable = 0x8,
        Rename = 0x10,
        Preparation = 0x20,
        PlusMob = 0x40,
        NoAttack = 0x80, // .Attackable check
        ImmuneToPlayer = 0x100, // .Attackable check
        ImmuneToNpc = 0x200,
        Looting = 0x400,
        PetInCombat = 0x800,
        PvPFlagged = 0x1000,
        Silenced = 0x2000,
        CannotSwim = 0x4000,
        OnlySwim = 0x8000,
        NoAttack2 = 0x10000, // .Attackable check
        Pacified = 0x20000,
        Stunned = 0x40000,
        CanPerformActionMask1 = 0x60000,
        InCombat = 0x80000,
        OnTaxi = 0x100000,
        MainHandDisarmed = 0x200000,
        Confused = 0x400000,
        Feared = 0x800000,
        PossessedByPlayer = 0x1000000,
        NotSelectable = 0x2000000, // .Attackable check
        Skinnable = 0x4000000,
        Mount = 0x8000000,
        PreventKneelingWhenLooting = 0x10000000,
        PreventEmotes = 0x20000000,
        Sheath = 0x40000000,
        Unk31 = 0x80000000,
        Sheathe = 0x40000000,
    }

    [Flags]
    public enum UnitFlags2
    {
        None = 0,
        FeignDeath = 0x1,
        NoModel = 0x2,
        ForceAutoRunForward = 0x40,
    }

    [Flags]
    public enum UnitFlags3
    {
        None = 0,
    }

    [Flags]
    public enum StateAnimID
    {
        None = 0,
    }

    [Flags]
    public enum UnitNPCFlags
    {
        None = 0,
        Gossip = 0x1,
        QuestGiver = 0x2,
        CanTrain = 0x10,
        BattlePetTrainer = 0x20,
        DailyQuests = 0x40,
        Vendor = 0x80,
        SellsFood = 0x200,
        Reagents = 0x800,
        CanRepair = 0x1000,
        Taxi = 0x2000,
        SpiritHealer = 0x4000,
        SpiritGuide = 0x8000,
        Innkeeper = 0x10000,
        Banker = 0x20000,
        BattleMaster = 0x100000,
        Auctioneer = 0x200000,
        StableMaster = 0x400000,
        GuildBanker = 0x800000,
        MailInfo = 0x4000000,
        ForgeMaster = 0x8000000,
        Transmogrifier = 0x10000000,
        VoidStorageBanker = 0x20000000,
    }

    [Flags]
    public enum UnitDynamicFlags
    {
        None = 0,
        Invisible = 0x1,
        Lootable = 0x4,
        TrackUnit = 0x4,
        Tapped = 0x8,
        TappedByMe = 0x10,
        SpecialInfo = 0x20, // Unknown Flag.
        Dead = 0x40,
        ReferAFriendLinked = 0x80,
        IsTappedByAllThreatList = 0x100, // Flag OK, but Blizzard broke the function, so it's not updated.
    }

    [Flags]
    public enum UnitAuraFlags : byte
    {
        None = 0x0,
        Passive = 0x1,
        Cancelable = 0x2,
        Active = 0x4,
        PlayerCasted = 0x8,
        Harmful = 0x10,
        Duration = 0x0020,
        BasePoints = 0x0040,
        Negative = 0x0080,
    }

    public enum QuestGiverStatus
    {
        None = 0,
        Unavailable = 1,
        LowLevelAvailable = 2,
        LowLevelTurnInRepeatable = 3,
        LowLevelAvailableRepeatable = 4,
        Incomplete = 5,
        TurnInRepeatable = 6,
        AvailableRepeatable = 7,
        Available = 8,
        TurnInInvisible = 9, // '?' hidden on the MiniMap.
        TurnIn = 10,
    }

    public enum UnitFlightMasterStatus
    {
        None = 0,
        FlightAvailable = 1,
        FlightUndiscovered = 2,
        OtherFaction = 3,
    }

    public enum UnitClassification
    {
        Normal,
        Elite,
        RareElite,
        WorldBoss,
        Rare,
        Trivial,
        Minus,
    }

    [Flags]
    public enum BagType
    {
        None = 0x1000000,
        Unspecified = 0x0,
        Quiver = 0x1,
        AmmoPouch = 0x2,
        SoulBag = 0x4,
        LeatherworkingBag = 0x8,
        InscriptionBag = 0x10,
        HerbBag = 0x20,
        EnchantingBag = 0x40,
        EngineeringBag = 0x80,
        Keyring = 0x100,
        GemBag = 0x200,
        MiningBag = 0x400,
        Unknown = 0x800,
        VanityPets = 0x1000,
        LureBag = 0x8000,
    }

    [Flags]
    public enum TypeFlag
    {
        None = 0,
        HERB_LOOT = 0x100,
        MINING_LOOT = 0x200,
        ENGENEERING_LOOT = 0x8000,
    }
}