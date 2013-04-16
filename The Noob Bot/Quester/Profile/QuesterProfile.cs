using System;
using System.Collections.Generic;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;

namespace Quester.Profile
{
    [Serializable]
    public class QuesterProfile
    {
        public List<Include> Includes = new List<Include>();
        public List<QuesterBlacklistRadius> Blackspots = new List<QuesterBlacklistRadius>();
        public List<Npc> AvoidMobs = new List<Npc>();
        public List<Quest> Quests = new List<Quest>();

        // Remove all quests not for character class/race
        public void Filter()
        {
            int exp = (int) ObjectManager.Me.WowRace - 1;
            //exp = 1 - 1; // Human
            uint mBitRace = exp >= 0 ? (uint) System.Math.Pow(2, exp) : 0;
            exp = (int) ObjectManager.Me.WowClass - 1;
            //exp = 2 - 1; // Paladin
            uint mBitClass = exp >= 0 ? (uint) System.Math.Pow(2, exp) : 0;

            Quests.RemoveAll(delegate(Quest quest)
                {
                    return ((quest.ClassMask > 0 && (quest.ClassMask & mBitClass) == 0) ||
                            (quest.RaceMask > 0 && (quest.RaceMask & mBitRace) == 0)
                           );
                }
                );
        }

        // ToDo: Add another filtering function to remove done quests after requesting them to the server
    }

    [Serializable]
    public class Include
    {
        public string PathFile = "";
        public string ScriptCondition = "";
    }

    [Serializable]
    public class QuesterBlacklistRadius
    {
        public Point Position = new Point();
        public float Radius = 5.0f;
    }

    [Serializable]
    public class Quest : IComparable
    {
        public string Name = "";
        public int ClassMask = 0;
        public int RaceMask = 0;
        public int Id = 0;
        public int MinLevel = 0;
        public int MaxLevel = 0;
        public List<int> NeedQuestCompletedId = new List<int>(); // req 1 in list completed
        public int ItemPickUp = 0;
        public Npc PickUp = new Npc();
        public Npc TurnIn = new Npc();
        public string ScriptCondition = "";
        public string ScriptConditionIsFinish = "";
        public List<QuestObjective> Objectives = new List<QuestObjective>();

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Quest otherQuest = obj as Quest;
            if (otherQuest.ItemPickUp != 0)
                if (this.ItemPickUp != 0)
                    return 0;
                else
                    return 1;
            else if (this.ItemPickUp != 0)
                return -1;

            if (otherQuest.PickUp.Position != null)
                return this.PickUp.Position.DistanceTo(ObjectManager.Me.Position).CompareTo(otherQuest.PickUp.Position.DistanceTo(ObjectManager.Me.Position));
            else
                throw new ArgumentException("Object is not a Quest");
        }

    }

    [Serializable]
    public class QuestObjective
    {
        public Objective Objective = Objective.None;
        public int Count = 0;
        internal int CurrentCount = 0;
        internal List<Point> PathHotspots = null;
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
        internal bool IsUsedWaitMs = false;

        // InteractWith
        public Point PositionInteractWith = new Point();
        public int EntryInteractWith = 0;
        public bool IsUsedInteractWith;
        public int GossipOptionsInteractWith = -1;

        // Train all Spells
        internal bool IsUsedTrainSpells = false;

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
        public Keybindings Keys = Keybindings.NONE;
        public Point PositionPressKey = new Point();

        // UseItemAOE & UseSpellAOE & AOE ATTACK MOB
        public float Range = 5.0f;
        public int EntryAOE = 0;
        // UseItemAOE
        internal bool IsUsedUseItemAOE = false;
        // UseSpellAOE
        internal bool IsUsedUseSpellAOE = false;
        // Use RuneForge
        internal bool IsUsedUseRuneForge = false;
        public int WaitMsUseRuneForge = 0;
        public Point PositionUseRuneForge = new Point();
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
        TrainSpells,
        UseSpell,
        EquipItem,
        PickUpQuest,
        TurnInQuest,
        UseVehicle,
        EjectVehicle,
        PressKey,
        UseItemAOE,
        UseSpellAOE,
        UseRuneForge,
    }

    [Serializable]
    public class GoTo
    {
        public List<Point> Points = new List<Point>();
    }
}