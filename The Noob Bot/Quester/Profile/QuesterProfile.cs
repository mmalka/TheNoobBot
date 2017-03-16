using System;
using System.Collections.Generic;
using System.Linq;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Math = System.Math;
using System.ComponentModel;
using System.Xml.Serialization;
using Keybindings = nManager.Wow.Enums.Keybindings;

namespace Quester.Profile
{
    [Serializable]
    public class QuesterProfile
    {
        [XmlAttribute] public string Author = ""; // serialize even empty to force authors to fill
        [XmlAttribute, DefaultValue("")] public string ExtraCredits = ""; // only serialize if set
        [XmlAttribute] public DevelopmentStatus DevelopmentStatus = DevelopmentStatus.WorkInProgress; // set default as WIP if null or new
        [XmlAttribute, DefaultValue("")] public string Description = ""; // only serialize if set
        public List<Include> Includes = new List<Include>();

        public bool ShouldSerializeIncludes()
        {
            return Includes != null && Includes.Count > 0;
        }

        public List<Npc> Questers = new List<Npc>();
        public List<Quest> Quests = new List<Quest>();
        public List<QuesterBlacklistRadius> Blackspots = new List<QuesterBlacklistRadius>();

        public bool ShouldSerializeBlackspots()
        {
            return Blackspots != null && Blackspots.Count > 0;
        }

        public List<Npc> AvoidMobs = new List<Npc>();

        public bool ShouldSerializeAvoidMobs()
        {
            return AvoidMobs != null && AvoidMobs.Count > 0;
        }

        // Remove all quests not for character class/race
        public void Filter()
        {
            int exp = (int) ObjectManager.Me.WowRace - 1;
            //exp = 1 - 1; // Human
            uint mBitRace = exp >= 0 ? (uint) Math.Pow(2, exp) : 0;
            exp = (int) ObjectManager.Me.WowClass - 1;
            //exp = 2 - 1; // Paladin
            uint mBitClass = exp >= 0 ? (uint) Math.Pow(2, exp) : 0;
            for (int i = Quests.Count - 1; i >= 0; i--)
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
        [DefaultValue("")] public string PathFile = "";
        [DefaultValue("")] public string ScriptCondition = "";
    }

    [Serializable]
    public class QuesterBlacklistRadius
    {
        public Point Position = new Point();
        [DefaultValue(5.0f)] public float Radius = 5.0f;
    }

    [Serializable]
    public class Quest : IComparable
    {
        public string Name = "";
        [DefaultValue(0)] public int ClassMask = 0;
        [DefaultValue(0)] public int RaceMask = 0;
        [DefaultValue(2)] public int Gender = 2;
        public int Id = 0;
        public int QuestLevel = 0;
        public int MinLevel = 1;
        public int MaxLevel = 110;
        public List<int> NeedQuestCompletedId = new List<int>(); // req 1 in list completed
        [DefaultValue(false)] public bool AutoAccepted = false;

        public bool ShouldSerializeNeedQuestCompletedId()
        {
            return NeedQuestCompletedId != null && NeedQuestCompletedId.Count > 0;
        }

        public List<int> NeedQuestNotCompletedId = new List<int>();

        public bool ShouldSerializeNeedQuestNotCompletedId()
        {
            return NeedQuestNotCompletedId != null && NeedQuestNotCompletedId.Count > 0;
        }

        [DefaultValue(0)] public int ItemPickUp = 0;
        [DefaultValue(0)] public int PickUp = 0;
        [DefaultValue(null)] public Point WorldQuestLocation = null;
        [DefaultValue(0)] public int TurnIn = 0;
        [DefaultValue("")] public string ScriptCondition = "";
        [DefaultValue("")] public string ScriptConditionIsFinish = "";
        public List<QuestObjective> Objectives = new List<QuestObjective>();

        // Auto Accept/complete a quest with popup
        public List<int> AutoComplete = new List<int>();

        public bool ShouldSerializeAutoComplete()
        {
            return AutoComplete != null && AutoComplete.Count > 0;
        }

        public List<int> AutoAccept = new List<int>();

        public bool ShouldSerializeAutoAccept()
        {
            return AutoAccept != null && AutoAccept.Count > 0;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Quest))
                return -1;

            Quest otherQuest = obj as Quest;
            if (otherQuest.ItemPickUp != 0)
                return ItemPickUp != 0 ? 0 : 1;
            if (ItemPickUp != 0)
                return -1;
            Point currentQuestLocation;
            Point otherQuestLocation;
            if (WorldQuestLocation != null && WorldQuestLocation.IsValid)
            {
                currentQuestLocation = WorldQuestLocation;
            }
            else
            {
                Npc currentNpc = Bot.Bot.FindQuesterById(PickUp);
                currentQuestLocation = currentNpc.Position;
            }

            if (otherQuest.WorldQuestLocation != null && otherQuest.WorldQuestLocation.IsValid)
            {
                otherQuestLocation = otherQuest.WorldQuestLocation;
            }
            else
            {
                Npc otherNpc = Bot.Bot.FindQuesterById(otherQuest.PickUp);
                otherQuestLocation = otherNpc.Position;
            }

            if (otherQuestLocation != null && otherQuestLocation.IsValid)
            {
                // We have a valid quest location, return which one is closer.
                return currentQuestLocation.DistanceTo(ObjectManager.Me.Position).CompareTo(otherQuestLocation.DistanceTo(ObjectManager.Me.Position));
            }
            if (otherQuestLocation == null)
            {
                Logging.WriteDebug("Quest " + otherQuest.Name + " (" + otherQuest.Id + ") doesn't have a valid PickUp and cannot be compared to.");
                return -1;
            }
            if (currentQuestLocation == null)
            {
                Logging.WriteDebug("Quest " + Name + " (" + Id + ") doesn't have a valid PickUp and cannot be compared to.");
                return 1;
            }
            return 0;
        }
    }

    [Serializable]
    public class QuestObjective
    {
        public Objective Objective = Objective.None;
        [DefaultValue(0)] public uint InternalIndex = 0;
        [DefaultValue(0)] public int InternalQuestId = 0;
        [DefaultValue(false)] public bool IsBonusObjective;
        [DefaultValue(0)] public int Count = 0;
        internal int CurrentCount = 0;
        [XmlIgnore] public List<Point> PathHotspots = null;
        public List<int> Entry = new List<int>();
        [DefaultValue(false)] public bool DeactivateMount = false;

        public bool ShouldSerializeEntry()
        {
            return Entry != null && Entry.Count > 0;
        }

        public bool ShouldSerializeIsObjectiveCompleted()
        {
            return false;
        }

        [DefaultValue("NoName")] public string Name = "NoName";
        public List<uint> Factions = new List<uint>();

        public bool ShouldSerializeFactions()
        {
            return Factions != null && Factions.Count > 0;
        }

        [DefaultValue(0)] public int CollectItemId = 0;
        [DefaultValue(0)] public int CollectCount = 0;
        public List<Point> Hotspots = new List<Point>();

        public bool ShouldSerializeHotspots()
        {
            return Hotspots != null && Hotspots.Count > 0;
        }

        [DefaultValue("")] public string Script = "";
        [DefaultValue("")] public string ScriptCondition = "";
        [DefaultValue("")] public string ScriptConditionIsComplete = "";
        public bool IsObjectiveCompleted = false;
        [DefaultValue(false)] public bool CanPullUnitsAlreadyInFight = false;
        [DefaultValue(false)] public bool IgnoreQuestCompleted = false;
        [DefaultValue(false)] public bool IgnoreBlackList = false;
        [DefaultValue(false)] public bool IgnoreNotSelectable = false;
        [DefaultValue(false)] public bool AllowPlayerControlled = false;
        [DefaultValue(false)] public bool IgnoreItemNotUsable = false;
        [DefaultValue(false)] public bool ForceTravelToQuestZone = false;
        [XmlIgnore] public bool TravelToQuestZone = true;
        // Use Item
        [DefaultValue(0)] public int UseItemId = 0;
        public Point Position = new Point();

        // UseLuaMAcro
        [DefaultValue("")] public string LuaMacro;

        public bool ShouldSerializePosition()
        {
            return Position != null && Position.IsValid;
        }

        public bool ShouldSerializeExtraPoint()
        {
            return ExtraPoint != null && ExtraPoint.IsValid;
        }

        public bool ShouldSerializeExtraObject1()
        {
            return ExtraObject1 != null;
        }

        public bool ShouldSerializeExtraObject2()
        {
            return ExtraObject2 != null;
        }

        public bool ShouldSerializeExtraObject3()
        {
            return ExtraObject3 != null;
        }

        // Wait
        [DefaultValue(0)] public int WaitMs = 0;

        // InteractWith/UseSpell/UseSpellAOE
        [DefaultValue(false)] public bool IgnoreFight = false;
        [DefaultValue(false)] public bool DismissPet = false;

        // InteractWith
        [DefaultValue(0)] public int GossipOptionsInteractWith = 0;
        [DefaultValue(false)] public bool IsDead = false;

        // UseSpell
        [DefaultValue(0)] public int UseSpellId = 0;

        // EquipItem
        [DefaultValue(0)] public int EquipItemId = 0;

        // UseVehicule
        [DefaultValue(0)] public int EntryVehicle = 0;
        [DefaultValue(false)] public bool OnlyInVehicule = false;
        [DefaultValue(false)] public bool OnlyOutVehicule = false;

        // PressKey
        [DefaultValue(Keybindings.NONE)] public Keybindings Keys = Keybindings.NONE;

        // UseItemAOE & UseSpellAOE & AOE ATTACK MOB
        [DefaultValue(5.0f)] public float Range = 5.0f;

        // Apply buff from item AOE
        [DefaultValue(0)] public int BuffId = 0;
        [DefaultValue(0)] public int BuffCount = 0;

        // Use taxi
        [DefaultValue(0)] public int TaxiEntry = 0;
        [DefaultValue("")] public string FlightDestinationX = "";
        [DefaultValue("")] public string FlightDestinationY = "";

        // Pickup/Turnin quest
        [DefaultValue(0)] public int NpcEntry = 0;
        [DefaultValue("")] public string QuestName = "";
        [DefaultValue(0)] public int QuestId = 0;

        // TravelTo
        [DefaultValue(-1)] public int ContinentId = -1;

        // MessageBox
        [DefaultValue("")] public string Message = "";

        // Extra for developpers/scripters.
        [DefaultValue("")] public string ExtraString = "";
        [DefaultValue(0)] public int ExtraInt = 0;
        [DefaultValue(0f)] public float ExtraFloat = 0f;
        public Point ExtraPoint = new Point();
        public object ExtraObject1 = null; // Developpers will always need to assume they are null if they wanna use it.
        public object ExtraObject2 = null; // That's the only way to effectively prevent them from being serialized.
        public object ExtraObject3 = null;
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
        KillMobUseItem,
        MoveTo,
        PickUpObject,
        PickUpQuest,
        PressKey,
        UseItem,
        UseLuaMacro,
        TurnInQuest,
        UseFlightPath,
        UseItemAOE,
        UseActionButtonOnUnit,
        UseRuneForge,
        UseSpell,
        UseSpellAOE,
        UseVehicle,
        Wait,
        TravelTo,
        ClickOnTerrain,
        MessageBox,
        PickUpNPC,
        GarrisonHearthstone,
        CSharpScript,
    }

    [Serializable]
    public class GoTo
    {
        public List<Point> Points = new List<Point>();
    }
}