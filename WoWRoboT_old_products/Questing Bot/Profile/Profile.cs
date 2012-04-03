using System;
using System.Collections.Generic;
using WowManager.MiscStructs;

namespace Questing_Bot.Profile
{
    [Serializable]
    public class Profile
    {
        public string Name = "";

        public float SearchDistance = 40.0f;
        public float MinDurability = 0.3f;
        public uint MinFreeBagSlots = 3;
        public uint MinLevel = 0;
        public uint MaxLevel = 85;
        public bool MailGrey = false;
        public bool MailWhite = false;
        public bool MailGreen = true;
        public bool MailBlue = true;
        public bool MailPurple = true;
        public bool SellGrey = true;
        public bool SellWhite = true;
        public bool SellGreen = false;
        public bool SellBlue = false;
        public bool SellPurple = false;

        public List<Include> Includes = new List<Include>();

        public List<Npc> AvoidMobs = new List<Npc>();

        public List<Blackspot> Blackspots = new List<Blackspot>();

        public List<Buy> BuyItems = new List<Buy>();

        public List<KeepItem> KeepItems = new List<KeepItem>();

        public List<Mailbox> Mailboxes = new List<Mailbox>();

        public List<string> MailBoxNames = new List<string>();

        public List<Vendor> Vendors = new List<Vendor>();

        public float GoToSearchDistance = 100;
        public List<GoTo> GoToList = new List<GoTo>();

        public List<Quest> Quests = new List<Quest>();
    }

    // Class
    [Serializable]
    public class Include
    {
        public string PathFile = "";
        public string ScriptCondition = "";
    }
    [Serializable]
    public class Npc
    {
        public string Name = "";
        public int Entry = 0;
        public Point Position = new Point();
        public string Script = "";
        public int SelectGossipOption = 1;
    }
    [Serializable]
    public class Blackspot
    {
        public Point Position = new Point();
        public float Radius = 5.0f;
    }
    [Serializable]
    public class Mailbox
    {
        public Point Position = new Point();
    }
    [Serializable]
    public class Vendor
    {
        public string Name = "";
        public int Entry = 0;
        public TypeVendor Type = TypeVendor.None;
        public TrainClass TrainClass = TrainClass.None;
        public Point Position = new Point();
    }
    [Serializable]
    public class Buy
    {
        public string Name = "";
        public int Id = 0;
        public int Count = 0;
        public int MinLevel = 0;
        public int MaxLevel = 0;
        public TypeBuy TypeBuy = TypeBuy.None;
    }
    [Serializable]
    public class KeepItem
    {
        public string Name = "";
        public int Id = 0;
    }
    [Serializable]
    public class Quest
    {
        public string Name = "";
        public int Id = 0;
        public int MinLevel = 0;
        public int MaxLevel = 0;
        public int NeedQuestCompletedId = 0;
        public Npc PickUp = new Npc();
        public Npc TurnIn = new Npc();
        public string ScriptCondition = "";
        public string ScriptConditionIsFinish = "";
        public List<QuestObjective> Objectives = new List<QuestObjective>();
    }
    [Serializable]
    public class QuestObjective
    {
        public Objective Objective = Objective.None;
        public int Count = 0;
        internal int CurrentCount = 0;
        internal List<Point> PathHotsports = null;
        public List<int> Entry = new List<int>();
        public List<uint> Factions = new List<uint>();
        public int CollectItemId = 0;
        public int CollectCount = 0;
        public List<Point> Hotspots = new List<Point>();
        public string Script = "";
        public string ScriptCondition = "";
        public string ScriptConditionIsComplete = "";

        // Use Item
        internal bool IsUsedUseItem = false;
        public int WaitMsUseItem = 0;
        public int UseItemId = 0;
        public Point PositionUseItem = new Point();

        // Move To
        public Point MoveTo = new Point();

        // Wait
        public int WaitMs = 0;
        internal bool IsUsedWaitMs;

        // InteractWith
        public Point PositionInteractWith = new Point();
        public int EntryInteractWith = 0;
        public bool IsUsedInteractWith;
        public int GossipOptionsInteractWith = -1;

        // UseSpell
        internal bool IsUsedUseSpell = false;
        public int WaitMsUseSpell = 0;
        public int UseSpellId = 0;
        public Point PositionUseSpell = new Point();

        // EquipItem
        internal bool IsUsedEquipItem = false;
        public int EquipItemId = 0;

        // PickUpQuest
        internal bool IsUsedPickUpQuest = false;

        // TurnInQuest
        internal bool IsUsedTurnInQuest = false;

        // UseVehicule
        public int EntryVehicle = 0;
        public Point PositionVehicle = new Point();

        // PressKey
        internal bool IsUsedPressKey = false;
        public int WaitMsPressKey = 0;
        public WowManager.MiscEnums.Keybindings Keys = WowManager.MiscEnums.Keybindings.NONE;
        public Point PositionPressKey = new Point();

        // UseItemAOE & UseSpellAOE & AOE ATTACK MOB
        public float Range = 3.5f;
        public int EntryAOE = 0;
        // UseItemAOE
        internal bool IsUsedUseItemAOE = false;
        // UseSpellAOE
        internal bool IsUsedUseSpellAOE = false;

    }
    [Serializable]
    public class GoTo
    {
        public List<Point> Points = new List<Point>();
    }

    // Enums
    [Serializable]
    [Flags]
    public enum TypeVendor
    {
        None,
        Train,
        Repair,
        Food,
    }
    [Serializable]
    [Flags]
    public enum TrainClass
    {
        None,
        Warrior,
        Paladin,
        Hunter,
        Rogue,
        Priest,
        DeathKnight,
        Shaman,
        Mage,
        Warlock,
        Druid,
        Herbalism,
        Mining
    }
    [Serializable]
    [Flags]
    public enum TypeBuy
    {
        None,
        Water,
        Food,
    }
    [Serializable]
    public enum Objective
    {
        None,
        PickUpObject,
        KillMob,
        UseItem,
        MoveTo,
        Wait,
        InteractWith,
        UseSpell,
        EquipItem,
        PickUpQuest,
        TurnInQuest,
        UseVehicle,
        EjectVehicle,
        PressKey,
        UseItemAOE,
        UseSpellAOE,
    }

}
