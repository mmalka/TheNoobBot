using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Profiles_Converters.Converters
{
    [Serializable]
    public class turninObjectiveType : object
    {
        public turninObjectiveType()
        {
            Nav = navType.Run;
        }

        [XmlAttribute, DefaultValue(navType.Run)]
        public navType Nav { get; set; }

        [XmlAttribute]
        public float X { get; set; }

        [XmlAttribute]
        public float Y { get; set; }

        [XmlAttribute]
        public float Z { get; set; }
    }


    [Serializable]
    public class objectiveMetaType : object
    {
        [XmlElement("CollectFrom", typeof (CollectFrom)), XmlElement("Hotspots", typeof (Hotspots)), XmlElement("TargetMaxLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"),
         XmlElement("TargetMinLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"), XmlElement("TurnIn", typeof (turninObjectiveType), Form = XmlSchemaForm.Unqualified),
         XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName"), XmlIgnore]
        public ItemsChoiceType3[] ItemsElementName { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string CollectCount { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string ItemId { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string KillCount { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string MobId { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string ObjectId { get; set; }

        [XmlAttribute]
        public objectiveTypeType Type { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string UseCount { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class CollectFrom : object
    {
        [XmlElement("GameObject", typeof (gameObjectType), Form = XmlSchemaForm.Unqualified), XmlElement("Mob", typeof (mobObjectiveType), Form = XmlSchemaForm.Unqualified),
         XmlElement("Vendor", typeof (vendorObjectiveType), Form = XmlSchemaForm.Unqualified)]
        public object[] Items { get; set; }
    }

    [Serializable]
    public class gameObjectType : object
    {
        public gameObjectType()
        {
            Nav = navType.Run;
        }

        [XmlAttribute, DefaultValue(navType.Run)]
        public navType Nav { get; set; }

        [XmlAttribute]
        public string Name { get; set; }


        [XmlAttribute(DataType = "positiveInteger")]
        public string Id { get; set; }
    }


    [Serializable]
    public class mobObjectiveType : object
    {
        public mobObjectiveType()
        {
            Nav = navType.Run;
        }

        [XmlAttribute, DefaultValue(navType.Run)]
        public navType Nav { get; set; }

        [XmlAttribute]
        public string Name { get; set; }


        [XmlAttribute(DataType = "positiveInteger")]
        public string Id { get; set; }


        [XmlAttribute(DataType = "positiveInteger")]
        public string Entry { get; set; }
    }


    [Serializable]
    public class vendorObjectiveType : object
    {
        public vendorObjectiveType()
        {
            Nav = navType.Run;
        }

        [XmlAttribute, DefaultValue(navType.Run)]
        public navType Nav { get; set; }

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
    public enum objectiveTypeType
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
        [XmlElement("Hotspot", typeof (point3dType)), XmlElement("HuntingGrounds", typeof (CustomBehaviorHuntingGrounds), Form = XmlSchemaForm.Unqualified)]
        public object[] Items { get; set; }


        [XmlAttribute]
        public string AllowBotStop { get; set; }


        [XmlAttribute]
        public byte AttackButton { get; set; }

        [XmlAttribute]
        public uint BuyItemId { get; set; }


        [XmlAttribute]
        public ushort CollectionDistance { get; set; }

        [XmlAttribute]
        public byte CollectItemCount { get; set; }

        [XmlAttribute]
        public ushort CollectItemId { get; set; }

        [XmlAttribute]
        public string CollectUntil { get; set; }


        [XmlAttribute]
        public string DestName { get; set; }


        [XmlAttribute]
        public string DoRepair { get; set; }


        [XmlAttribute]
        public string DoMail { get; set; }


        [XmlAttribute]
        public string DoSell { get; set; }


        [XmlAttribute]
        public string File { get; set; }


        [XmlAttribute]
        public string GoalText { get; set; }

        [XmlAttribute]
        public byte GossipOptions { get; set; }

        [XmlAttribute]
        public booleanType GroundMountFarmingMode { get; set; }

        [XmlAttribute]
        public byte HuntingGroundRadius { get; set; }

        [XmlAttribute]
        public ushort Ids { get; set; }

        [XmlAttribute]
        public string IgnoreMobsInBlackspots { get; set; }


        [XmlAttribute]
        public ushort InteractByUsingItemId { get; set; }

        [XmlAttribute]
        public booleanType KillBetweenHotspots { get; set; }

        [XmlAttribute]
        public string LogColor { get; set; }


        [XmlAttribute]
        public string Lua { get; set; }


        [XmlAttribute]
        public string Macro { get; set; }


        [XmlAttribute]
        public ushort MaxRange { get; set; }

        [XmlAttribute]
        public ushort MaxRangeToUseItem { get; set; }

        [XmlAttribute]
        public ushort MinRange { get; set; }

        [XmlAttribute]
        public uint MobId { get; set; }

        [XmlAttribute]
        public uint MobId1 { get; set; }

        [XmlAttribute]
        public uint MobId2 { get; set; }

        [XmlAttribute]
        public uint MobId3 { get; set; }


        [XmlAttribute]
        public uint MobId4 { get; set; }

        [XmlAttribute]
        public uint MobId5 { get; set; }

        [XmlAttribute]
        public uint MobId6 { get; set; }

        [XmlAttribute]
        public string MobIds { get; set; }


        [XmlAttribute]
        public string MobState { get; set; }


        [XmlAttribute]
        public byte NonCompeteDistance { get; set; }

        [XmlAttribute]
        public byte NumOfTimes { get; set; }

        [XmlAttribute]
        public uint ObjectId { get; set; }

        [XmlAttribute]
        public string ObjectType { get; set; }


        [XmlAttribute]
        public string PreInteractMountStrategy { get; set; }

        [XmlAttribute]
        public uint QuestId { get; set; }

        [XmlAttribute]
        public byte QuestObjectiveIndex { get; set; }

        [XmlAttribute]
        public byte Range { get; set; }

        [XmlAttribute]
        public string SoundCue { get; set; }


        [XmlAttribute]
        public byte SoundCueInterval { get; set; }

        [XmlAttribute]
        public string Text { get; set; }


        [XmlAttribute]
        public string Type { get; set; }


        [XmlAttribute]
        public string UseType { get; set; }


        [XmlAttribute]
        public byte UseWhenMobHasHealthPercent { get; set; }

        [XmlAttribute]
        public ushort VehicleMountId { get; set; }

        [XmlAttribute]
        public string WaitForNpcs { get; set; }

        [XmlAttribute]
        public uint WaitTime { get; set; }

        [XmlAttribute]
        public decimal X { get; set; }

        [XmlAttribute]
        public decimal Y { get; set; }

        [XmlAttribute]
        public decimal Z { get; set; }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    public class CustomBehaviorHuntingGrounds : object
    {
        public point3dType Hotspot { get; set; }


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
            Nav = navType.Run;
        }


        [XmlAttribute]
        public string DestName { get; set; }


        [XmlAttribute]
        public uint QuestId { get; set; }


        [XmlAttribute, DefaultValue(navType.Run)]
        public navType Nav { get; set; }


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
            Nav = navType.Run;
        }


        [XmlAttribute]
        public uint GiverId { get; set; }


        [XmlAttribute]
        public string GiverName { get; set; }


        [XmlAttribute, DefaultValue(navType.Run)]
        public navType Nav { get; set; }


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


    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Quest : object
    {
        [XmlElement("Objective", typeof (objectiveMetaType), Form = XmlSchemaForm.Unqualified), XmlElement("TurnIn", typeof (turninObjectiveType), Form = XmlSchemaForm.Unqualified)]
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
            Nav = navType.Run;
        }


        [XmlAttribute]
        public string TurnInName { get; set; }


        [XmlAttribute]
        public uint TurnInId { get; set; }


        [XmlAttribute, DefaultValue(navType.Run)]
        public navType Nav { get; set; }

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