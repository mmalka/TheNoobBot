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
        Flag_15_0x8000 = 0x8000, // To me, it looks like "ServerControlled" in the mean of "patrol NPC", 
                                 // but I let you do it, I'm definitly not good at guessing flags.
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
        Invisible = 0x1,                 // Maybe something intended to detect botter, we may need to don't touch a monster having this flag for security purpose.
                                         // A Game Master can easily add this flag to a creature near a Grinder bot, and the bot will begin to fight against that creature.
                                         // Even if the creature is totally invisible and not possible to target with "Tab" for example.
                                         // Write memory |1 and you will see the monster disapear, but the objectmanager still handle it.
        Lootable = 0x2,
        TrackUnit = 0x4,
        Tapped = 0x8,
        TappedByMe = 0x10,
        SpecialInfo = 0x20,              // Unknown Flag.
        Dead = 0x40,                     // It's a visual Dead status (Gray, 0 HP...) but the creature lives and can interact.
                                         // Some quester are like this, you interact with the corpse if your are neutral at least
                                         // Some mobs are like this, you then neither attack them, nor skin them
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

}
