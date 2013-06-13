using System;

namespace nManager.Wow.Enums
{
    [Flags]
    public enum UnitFlags : uint
    {
        None = 0,
        Sitting = 0x1,
        Influenced = 0x4,
        PlayerControlled = 0x8,
        Totem = 0x10,
        Preparation = 0x20,
        PlusMob = 0x40,
        NotAttackable = 0x100,
        Flag_9_0x200 = 0x200,
        Looting = 0x400,
        PetInCombat = 0x800,
        PvPFlagged = 0x1000,
        Silenced = 0x2000,
        Flag_14_0x4000 = 0x4000,
        Flag_15_0x8000 = 0x8000,
        Pacified = 0x20000,
        Stunned = 0x40000,
        CanPerformAction_Mask1 = 0x60000,
        Combat = 0x80000,
        TaxiFlight = 0x100000,
        Disarmed = 0x200000,
        Confused = 0x400000,
        Fleeing = 0x800000,
        Possessed = 0x1000000,
        NotSelectable = 0x2000000,
        Skinnable = 0x4000000,
        Mounted = 0x8000000,
        Dazed = 0x20000000,
        Sheathe = 0x40000000,
    }

    [Flags]
    public enum UnitFlags2 : int
    {
        None = 0,
        FeignDeath = 0x1,
        NoModel = 0x2,
        ForceAutoRunForward = 0x40,
    }

    [Flags]
    public enum UnitNPCFlags : int
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
        Innkeeper = 0x10000,
        Banker = 0x20000,
        Auctioneer = 0x200000,
        StableMaster = 0x400000,
        GuildBanker = 0x800000,
        MailInfo = 0x4000000,
        ForgeMaster = 0x8000000,
        Transmogrifier = 0x10000000,
        VoidStorageBanker = 0x20000000,
    }

    [Flags]
    public enum UnitDynamicFlags : int
    {
        None = 0,
        Invisible = 0x1,
        Lootable = 0x2,
        TrackUnit = 0x4,
        Tapped = 0x8,
        TappedByMe = 0x10,
        SpecialInfo = 0x20, // Unknown Flag.
        Dead = 0x40,
        ReferAFriendLinked = 0x80,
        IsTappedByAllThreatList = 0x100, // Flag OK, but Blizzard broke the function, so it's not updated.
    }

    [Flags]
    public enum UnitQuestGiverStatus : int
    {
        None = 0x0,
        Unavailable = 0x1,
        LowLevelAvailable = 0x2,
        LowLevelTurnInRepeatable = 0x3,
        LowLevelAvailableRepeatable = 0x4,
        Incomplete = 0x5,
        TurnInRepeatable = 0x6,
        AvailableRepeatable = 0x7,
        Available = 0x8,
        TurnInInvisible = 0x9, // '?' hidden on the MiniMap.
        TurnIn = 0xA,
    }

    [Flags]
    public enum BagType : int
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
}