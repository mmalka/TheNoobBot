using System;
using System.Collections.Generic;
using nManager;
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
        public List<Npc> Questers = new List<Npc>();

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
        public int QuestLevel = 0;
        public int MinLevel = 0;
        public int MaxLevel = 0;
        public List<int> NeedQuestCompletedId = new List<int>(); // req 1 in list completed
        public List<int> NeedQuestNotCompletedId = new List<int>();
        public int ItemPickUp = 0;
        public int PickUp = 0;
        public int TurnIn = 0;
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

            Npc currentNpc = Quester.Bot.Bot.FindQuesterById(PickUp);
            Npc otherNpc = Quester.Bot.Bot.FindQuesterById(otherQuest.PickUp);
            if (otherNpc.Position != null)
                return currentNpc.Position.DistanceTo(ObjectManager.Me.Position).CompareTo(otherNpc.Position.DistanceTo(ObjectManager.Me.Position));
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
        public string Name = "Not defined";
        public List<uint> Factions = new List<uint>();
        public uint CollectItemId = 0;
        public int CollectCount = 0;
        public List<Point> Hotspots = new List<Point>();
        public string Script = "";
        public string ScriptCondition = "";
        public string ScriptConditionIsComplete = "";
        internal bool IsObjectiveCompleted = false;
        public bool CanPullUnitsAlreadyInFight = nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight;
        public bool IgnoreQuestCompleted = false;

        // Use Item
        public int UseItemId = 0;
        public Point Position = new Point();

        // Wait
        public int WaitMs = 0;

        // InteractWith
        public int EntryInteractWith = 0;
        public int GossipOptionsInteractWith = -1;

        // UseSpell
        public int UseSpellId = 0;

        // EquipItem
        public int EquipItemId = 0;

        // UseVehicule
        public int EntryVehicle = 0;

        // PressKey
        public Keybindings Keys = Keybindings.NONE;

        // UseItemAOE & UseSpellAOE & AOE ATTACK MOB
        public float Range = 5.0f;
        public int EntryAOE = 0;
        // Apply buff from item AOE
        public int BuffId = 0;
        public int BuffCount = 0;

        // Use taxi
        public string FlightDestinationX = "";
        public string FlightDestinationY = "";
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