using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Profiles_Converters.Converters
{
    [Serializable]
    public class TurninObjectiveType : object
    {
        public TurninObjectiveType()
        {
            Nav = NavType.Run;
        }

        [XmlAttribute, DefaultValue(NavType.Run)]
        public NavType Nav { get; set; }

        [XmlAttribute]
        public float X { get; set; }

        [XmlAttribute]
        public float Y { get; set; }

        [XmlAttribute]
        public float Z { get; set; }
    }


    [Serializable]
    public class ObjectiveMetaType : object
    {
        [XmlElement("CollectFrom", typeof (CollectFrom)), XmlElement("Hotspots", typeof (Hotspots)), XmlElement("TargetMaxLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"),
         XmlElement("TargetMinLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"), XmlElement("TurnIn", typeof (TurninObjectiveType), Form = XmlSchemaForm.Unqualified),
         XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName"), XmlIgnore]
        public ItemsChoiceType3[] ItemsElementName { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string CollectCount { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string ItemId { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string QuestId { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string KillCount { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string MobId { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string ObjectId { get; set; }

        [XmlAttribute]
        public ObjectiveTypeType Type { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string UseCount { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class CollectFrom : object
    {
        [XmlElement("Object", typeof (GameObjectType), Form = XmlSchemaForm.Unqualified),
         XmlElement("Mob", typeof (MobObjectiveType), Form = XmlSchemaForm.Unqualified),
         XmlElement("Vendor", typeof (VendorObjectiveType), Form = XmlSchemaForm.Unqualified)]
        public object[] Items { get; set; }
    }

    [Serializable]
    public class GameObjectType : object
    {
        public GameObjectType()
        {
            Nav = NavType.Run;
        }

        [XmlAttribute, DefaultValue(NavType.Run)]
        public NavType Nav { get; set; }

        [XmlAttribute]
        public string Name { get; set; }


        [XmlAttribute(DataType = "positiveInteger")]
        public string Id { get; set; }
    }


    [Serializable]
    public class MobObjectiveType : object
    {
        public MobObjectiveType()
        {
            Nav = NavType.Run;
        }

        [XmlAttribute, DefaultValue(NavType.Run)]
        public NavType Nav { get; set; }

        [XmlAttribute]
        public string Name { get; set; }


        [XmlAttribute(DataType = "positiveInteger")]
        public string Id { get; set; }


        [XmlAttribute(DataType = "positiveInteger")]
        public string Entry { get; set; }
    }


    [Serializable]
    public class VendorObjectiveType : object
    {
        public VendorObjectiveType()
        {
            Nav = NavType.Run;
        }

        [XmlAttribute, DefaultValue(NavType.Run)]
        public NavType Nav { get; set; }

        [XmlAttribute]
        public string Name { get; set; }


        [XmlAttribute(DataType = "positiveInteger")]
        public string Id { get; set; }
    }

    [Serializable]
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType3
    {
        CollectFrom,
        Hotspots,
        TargetMaxLevel,
        TargetMinLevel,
        TurnIn,
    }


    [Serializable]
    public enum ObjectiveTypeType
    {
        TurnIn,
        CollectItem,
        KillMob,
        UseObject,
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class CustomBehavior : object
    {
        [XmlElement("Hotspot", typeof (Point3DType)), XmlElement("HuntingGrounds", typeof (CustomBehaviorHuntingGrounds), Form = XmlSchemaForm.Unqualified)]
        public object[] Items { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string File { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string AllowBotStop { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string Names { get; set; }


        [DefaultValue("")]
        [XmlAttribute]
        public string ProfileName { get; set; }


        [DefaultValue(0)]
        [XmlAttribute]
        public byte AttackButton { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint BuyItemId { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public ushort CollectionDistance { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public byte CollectItemCount { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public ushort CollectItemId { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string CollectUntil { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string DestName { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string DoRepair { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string DoMail { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string DoSell { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string GoalText { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public byte GossipOptions { get; set; }

        [DefaultValue(false)]
        [XmlAttribute]
        public bool GroundMountFarmingMode { get; set; }

        [DefaultValue(false)]
        [XmlAttribute]
        public bool AllowBrokenPlugIns { get; set; }

        [DefaultValue(false)]
        [XmlAttribute]
        public bool AllowBrokenAddOns { get; set; }

        [DefaultValue(false)]
        [XmlAttribute]
        public bool AutoEquip { get; set; }

        [DefaultValue(false)]
        [XmlAttribute(AttributeName = "state")]
        public bool State { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public byte HuntingGroundRadius { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string Ids { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string IgnoreMobsInBlackspots { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public ushort InteractByUsingItemId { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string InteractByGossipOptions { get; set; }

        [DefaultValue(false)]
        [XmlAttribute]
        public bool KillBetweenHotspots { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string LogColor { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string Lua { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string Macro { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public ushort MaxRange { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public ushort MaxRangeToUseItem { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public ushort MinRange { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint MobId { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint MobId1 { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint MobId2 { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint MobId3 { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint MobId4 { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint MobId5 { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint MobId6 { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string MobIds { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string MobState { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public byte NonCompeteDistance { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint NumOfTimes { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint ObjectId { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string ObjectType { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string PreInteractMountStrategy { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint QuestId { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public byte QuestObjectiveIndex { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public float Range { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string SoundCue { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public byte SoundCueInterval { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string Text { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string Type { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string UseType { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public byte UseWhenMobHasHealthPercent { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public ushort VehicleMountId { get; set; }

        [DefaultValue("")]
        [XmlAttribute]
        public string WaitForNpcs { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public uint WaitTime { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public decimal X { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public decimal Y { get; set; }

        [DefaultValue(0)]
        [XmlAttribute]
        public decimal Z { get; set; }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    public class CustomBehaviorHuntingGrounds : object
    {
        public Point3DType Hotspot { get; set; }


        [XmlAttribute]
        public string WaypointVisitStrategy { get; set; }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class MoveTo : object
    {
        public MoveTo()
        {
            Nav = NavType.Run;
        }


        [XmlAttribute]
        public string DestName { get; set; }


        [XmlAttribute]
        public uint QuestId { get; set; }


        [XmlAttribute, DefaultValue(NavType.Run)]
        public NavType Nav { get; set; }


        [XmlAttribute]
        public float X { get; set; }


        [XmlAttribute]
        public float Y { get; set; }


        [XmlAttribute]
        public float Z { get; set; }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class PickUp : object
    {
        public PickUp()
        {
            Nav = NavType.Run;
        }

        [XmlAttribute]
        public uint GiverId { get; set; }

        [XmlAttribute]
        public string GiverName { get; set; }

        [XmlAttribute, DefaultValue(NavType.Run)]
        public NavType Nav { get; set; }

        [XmlAttribute]
        public string QuestName { get; set; }

        [XmlAttribute]
        public uint QuestId { get; set; }

        [XmlAttribute]
        public float X { get; set; }

        [XmlAttribute]
        public float Y { get; set; }

        [XmlAttribute]
        public float Z { get; set; }

        [XmlAttribute]
        public string GiverType { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class While : object
    {
        [XmlAttribute]
        public string Condition { get; set; }

        [XmlElement("Quest", typeof (Quest)), XmlChoiceIdentifier("ItemsElementName"), XmlElement("PickUp", typeof (PickUp)), XmlElement("TurnIn", typeof (TurnIn)),
         XmlElement("CustomBehavior", typeof(CustomBehavior)), XmlElement("If", typeof(If)), XmlElement("While", typeof(While)), XmlElement("Objective", typeof(ObjectiveMetaType)), XmlElement("MoveTo", typeof(MoveTo))]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName"), XmlIgnore]
        public ItemsChoiceType4[] ItemsElementName { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class If : object
    {
        [XmlAttribute]
        public string Condition { get; set; }

        [XmlElement("Quest", typeof (Quest)), XmlChoiceIdentifier("ItemsElementName"), XmlElement("PickUp", typeof (PickUp)), XmlElement("TurnIn", typeof (TurnIn)),
         XmlElement("CustomBehavior", typeof(CustomBehavior)), XmlElement("If", typeof(If)), XmlElement("Objective", typeof (ObjectiveMetaType)), XmlElement("While", typeof(While)), XmlElement("MoveTo", typeof(MoveTo))]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName"), XmlIgnore]
        public ItemsChoiceType4[] ItemsElementName { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Quest : object
    {
        [XmlElement("Objective", typeof (ObjectiveMetaType), Form = XmlSchemaForm.Unqualified), XmlElement("TurnIn", typeof (TurninObjectiveType), Form = XmlSchemaForm.Unqualified)]
        public object[] Items { get; set; }


        [XmlAttribute(DataType = "positiveInteger")]
        public string Id { get; set; }


        [XmlAttribute]
        public string Name { get; set; }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class TurnIn : object
    {
        public TurnIn()
        {
            Nav = NavType.Run;
        }


        [XmlAttribute]
        public string TurnInName { get; set; }


        [XmlAttribute]
        public uint TurnInId { get; set; }


        [XmlAttribute, DefaultValue(NavType.Run)]
        public NavType Nav { get; set; }

        [XmlAttribute]
        public string QuestName { get; set; }


        [XmlAttribute]
        public uint QuestId { get; set; }


        [XmlAttribute]
        public float X { get; set; }


        [XmlAttribute]
        public float Y { get; set; }


        [XmlAttribute]
        public float Z { get; set; }
    }
}