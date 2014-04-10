using System;
using System.Collections.Generic;
using System.Linq;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;
using Math = System.Math;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Quester.Profile
{
    [Serializable]
    public class QuesterProfile
    {
        public List<Include> Includes = new List<Include>();
        public bool ShouldSerializeIncludes() { return Includes != null && Includes.Count > 0; }
        public List<Npc> Questers = new List<Npc>();
        public List<Quest> Quests = new List<Quest>();
        public List<QuesterBlacklistRadius> Blackspots = new List<QuesterBlacklistRadius>();
        public bool ShouldSerializeBlackspots() { return Blackspots != null && Blackspots.Count > 0; }
        public List<Npc> AvoidMobs = new List<Npc>();
        public bool ShouldSerializeAvoidMobs() { return AvoidMobs != null && AvoidMobs.Count > 0; }

        // Remove all quests not for character class/race
        public void Filter()
        {
            int exp = (int) ObjectManager.Me.WowRace - 1;
            //exp = 1 - 1; // Human
            uint mBitRace = exp >= 0 ? (uint) Math.Pow(2, exp) : 0;
            exp = (int) ObjectManager.Me.WowClass - 1;
            //exp = 2 - 1; // Paladin
            uint mBitClass = exp >= 0 ? (uint) Math.Pow(2, exp) : 0;
            for (int i = Quests.Count-1; i >=0 ; i--)
            {
                Quest quest = Quests[i];
                if (quest.ClassMask > 0 && (quest.ClassMask & mBitClass) == 0 ||
                    (quest.RaceMask > 0 && (quest.RaceMask & mBitRace) == 0) ||
                    quest.Gender != (uint) WoWGender.Both && quest.Gender != (uint) ObjectManager.Me.WowGender)
                    Quests.Remove(quest);
            }
        }

        // ToDo: Add another filtering function to remove done quests after requesting them to the server
    }

    [Serializable]
    public class Include
    {
        [DefaultValue("")]
        public string PathFile = "";
        [DefaultValue("")]
        public string ScriptCondition = "";
    }

    [Serializable]
    public class QuesterBlacklistRadius
    {
        public Point Position = new Point();
        [DefaultValue(5.0f)]
        public float Radius = 5.0f;
    }

    [Serializable]
    public class Quest : IComparable
    {
        public string Name = "";
        [DefaultValue(0)]
        public int ClassMask = 0;
        [DefaultValue(0)]
        public int RaceMask = 0;
        [DefaultValue(2)]
        public int Gender = 2;
        public int Id = 0;
        public int QuestLevel = 0;
        public int MinLevel = 0;
        public int MaxLevel = 0;
        public List<int> NeedQuestCompletedId = new List<int>(); // req 1 in list completed
        public bool ShouldSerializeNeedQuestCompletedId() { return NeedQuestCompletedId != null && NeedQuestCompletedId.Count > 0; }
        public List<int> NeedQuestNotCompletedId = new List<int>();
        public bool ShouldSerializeNeedQuestNotCompletedId() { return NeedQuestNotCompletedId != null && NeedQuestNotCompletedId.Count > 0; }
        [DefaultValue(0)]
        public int ItemPickUp = 0;
        [DefaultValue(0)]
        public int PickUp = 0;
        public int TurnIn = 0;
        [DefaultValue("")]
        public string ScriptCondition = "";
        [DefaultValue("")]
        public string ScriptConditionIsFinish = "";
        public List<QuestObjective> Objectives = new List<QuestObjective>();

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Quest otherQuest = obj as Quest;
            if (otherQuest == null)
                return -1;
            if (otherQuest.ItemPickUp != 0)
                return ItemPickUp != 0 ? 0 : 1;
            if (ItemPickUp != 0)
                return -1;

            Npc currentNpc = Bot.Bot.FindQuesterById(PickUp);
            Npc otherNpc = Bot.Bot.FindQuesterById(otherQuest.PickUp);
            if (otherNpc.Position != null)
                return currentNpc.Position.DistanceTo(ObjectManager.Me.Position).CompareTo(otherNpc.Position.DistanceTo(ObjectManager.Me.Position));
            throw new ArgumentException("Object is not a Quest");
        }
    }

    [Serializable]
    public class QuestObjective
    {
        public Objective Objective = Objective.None;
        [DefaultValue(0)]
        public int Count = 0;
        internal int CurrentCount = 0;
        internal List<Point> PathHotspots = null;
        public List<int> Entry = new List<int>();
        public bool ShouldSerializeEntry() { return Entry != null && Entry.Count > 0; }
        [DefaultValue("NoName")]
        public string Name = "NoName";
        public List<uint> Factions = new List<uint>();
        public bool ShouldSerializeFactions() { return Factions != null && Factions.Count > 0; }
        public int CollectItemId = 0;
        public int CollectCount = 0;
        public List<Point> Hotspots = new List<Point>();
        public bool ShouldSerializeHotspots() { return Hotspots != null && Hotspots.Count > 0; }
        [DefaultValue("")]
        public string Script = "";
        [DefaultValue("")]
        public string ScriptCondition = "";
        [DefaultValue("")]
        public string ScriptConditionIsComplete = "";
        internal bool IsObjectiveCompleted = false;
        [DefaultValue(false)]
        public bool CanPullUnitsAlreadyInFight = nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight;
        [DefaultValue(false)]
        public bool IgnoreQuestCompleted = false;

        // Use Item
        [DefaultValue(0)]
        public int UseItemId = 0;
        public Point Position = new Point();
        public bool ShouldSerializePosition() { return Position != null && Position.IsValid; }

        // Wait
        [DefaultValue(0)]
        public int WaitMs = 0;

        // InteractWith/UseSpell/UseSpellAOE
        [DefaultValue(false)]
        public bool IgnoreFight = false;

        // InteractWith
        [DefaultValue(-1)]
        public int GossipOptionsInteractWith = -1;

        // UseSpell
        [DefaultValue(0)]
        public int UseSpellId = 0;

        // EquipItem
        [DefaultValue(0)]
        public int EquipItemId = 0;

        // UseVehicule
        [DefaultValue(0)]
        public int EntryVehicle = 0;

        // PressKey
        [DefaultValue(Keybindings.NONE)]
        public Keybindings Keys = Keybindings.NONE;

        // UseItemAOE & UseSpellAOE & AOE ATTACK MOB
        [DefaultValue(5.0f)]
        public float Range = 5.0f;

        // Apply buff from item AOE
        [DefaultValue(0)]
        public int BuffId = 0;
        [DefaultValue(0)]
        public int BuffCount = 0;

        // Use taxi
        [DefaultValue(0)]
        public int TaxiEntry = 0;
        [DefaultValue("")]
        public string FlightDestinationX = "";
        [DefaultValue("")]
        public string FlightDestinationY = "";

        // Pickup/Turnin quest
        [DefaultValue(0)]
        public int NpcEntry = 0;
        [DefaultValue("")]
        public string QuestName = "";
        [DefaultValue(0)]
        public int QuestId = 0;
    }

    [Serializable]
    public enum Objective
    {
        None,
        ApplyBuff,
        BuyItem,
        EjectVehicle,
        EquipItem,
        InteractWith,
        KillMob,
        MoveTo,
        PickUpObject,
        PickUpQuest,
        PressKey,
        UseItem,
        TurnInQuest,
        UseFlightPath,
        UseItemAOE,
        UseRuneForge,
        UseSpell,
        UseSpellAOE,
        UseVehicle,
        Wait,
    }

    [Serializable]
    public class GoTo
    {
        public List<Point> Points = new List<Point>();
    }
}