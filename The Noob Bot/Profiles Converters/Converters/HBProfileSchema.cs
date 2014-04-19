using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Profiles_Converters.Converters
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class AvoidMobs : object
    {
        [XmlElement("AvoidMob", typeof (mobType), Form = XmlSchemaForm.Unqualified), XmlElement("Mob", typeof (mobType), Form = XmlSchemaForm.Unqualified), XmlChoiceIdentifier("ItemsElementName")]
        public mobType[] Items { get; set; }

        [XmlElement("ItemsElementName"), XmlIgnore]
        public ItemsChoiceType[] ItemsElementName { get; set; }
    }


    [XmlInclude(typeof (blacklistMobType))]
    [XmlInclude(typeof (avoidmobType))]
    [Serializable]
    public class mobType : object
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public uint Id { get; set; }
    }

    [Serializable]
    public class subProfileType : object
    {
        [XmlElement("AvoidMobs", typeof (AvoidMobs)), XmlElement("Blacklist", typeof (Blacklist)), XmlElement("Blackspots", typeof (Blackspots)),
         XmlElement("ContinentId", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger"), XmlElement("ForceMail", typeof (ForceMail)), XmlElement("GrindArea", typeof (GrindArea)),
         XmlElement("Hotspots", typeof(Hotspots)), XmlElement("MailBlue", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("MailGreen", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("MailGrey", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("MailPurple", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("MailWhite", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("Mailboxes", typeof(Mailboxes)),
         XmlElement("MaxLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"), XmlElement("MinDurability", typeof (float), Form = XmlSchemaForm.Unqualified),
         XmlElement("MinFreeBagSlots", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger"),
         XmlElement("MinLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"), XmlElement("Name", typeof (string), Form = XmlSchemaForm.Unqualified),
         XmlElement("ProtectedItems", typeof(ProtectedItems)), XmlElement("SellBlue", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("SellGreen", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("SellGrey", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("SellPurple", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("SellWhite", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("TargetElites", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("Vendors", typeof(Vendors)), XmlElement("Quest", typeof(Quest)), XmlElement("QuestOrder", typeof(questOrderType)),
         XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName"), XmlIgnore]
        public ItemsChoiceType2[] ItemsElementName { get; set; }
    }
    [Serializable]
    public class questOrderType : object
    {
        [XmlElement("AvoidMobs", typeof(AvoidMobs)), XmlElement("Blacklist", typeof(Blacklist)), XmlElement("Blackspots", typeof(Blackspots)),
         XmlElement("ContinentId", typeof(string), Form = XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger"), XmlElement("ForceMail", typeof(ForceMail)), XmlElement("GrindArea", typeof(GrindArea)),
         XmlElement("Hotspots", typeof(Hotspots)), XmlElement("MailBlue", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("MailGreen", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("MailGrey", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("MailPurple", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("MailWhite", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("Mailboxes", typeof(Mailboxes)),
         XmlElement("MaxLevel", typeof(string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"), XmlElement("MinDurability", typeof(float), Form = XmlSchemaForm.Unqualified),
         XmlElement("MinFreeBagSlots", typeof(string), Form = XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger"),
         XmlElement("MinLevel", typeof(string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"), XmlElement("Name", typeof(string), Form = XmlSchemaForm.Unqualified),
         XmlElement("ProtectedItems", typeof(ProtectedItems)), XmlElement("SellBlue", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("SellGreen", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("SellGrey", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("SellPurple", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("SellWhite", typeof(Boolean), Form = XmlSchemaForm.Unqualified),
         XmlElement("TargetElites", typeof(Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("Vendors", typeof(Vendors)), XmlElement("Quest", typeof(Quest)), XmlElement("QuestOrder", typeof(Quest)),
         XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName"), XmlIgnore]
        public ItemsChoiceType2[] ItemsElementName { get; set; }
    }
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Blacklist : object
    {
        [XmlElement("Mob", Form = XmlSchemaForm.Unqualified)]
        public blacklistMobType[] Mob { get; set; }
    }


    [Serializable]
    public class blacklistMobType : mobType
    {
        [XmlAttribute]
        public blacklistFlagsType Flags { get; set; }
    }

    [Serializable]
    public enum blacklistFlagsType
    {
        All,
        Combat,
        Interact,
        Loot,
        Node,
        Pull,
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Blackspots : object
    {
        [XmlElement("Blackspot", Form = XmlSchemaForm.Unqualified)]
        public blackspotType[] Blackspot { get; set; }
    }

    [Serializable]
    public class blackspotType : object
    {
        public blackspotType()
        {
            Height = 0F;
        }

        [XmlAttribute, DefaultValue(typeof (float), "0")]
        public float Height { get; set; }

        [XmlAttribute]
        public float Radius { get; set; }

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
    public class ForceMail : object
    {
        [XmlElement("Item", Form = XmlSchemaForm.Unqualified)]
        public itemType[] Items { get; set; }
    }

    [Serializable]
    public class itemType : object
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string Entry { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class GrindArea : object
    {
        [XmlElement("Blacklist", typeof (Blacklist)), XmlElement("Factions", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"), XmlElement("Hotspots", typeof (Hotspots)),
         XmlElement("LootRadius", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"),
         XmlElement("MaxDistance", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"),
         XmlElement("MaximumHotspotTime", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"), XmlElement("Name", typeof (string), Form = XmlSchemaForm.Unqualified),
         XmlElement("RandomizeHotspots", typeof (Boolean), Form = XmlSchemaForm.Unqualified), XmlElement("TargetMaxLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"),
         XmlElement("TargetMinLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger"), XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items { get; set; }

        [XmlElement("ItemsElementName"), XmlIgnore]
        public ItemsChoiceType1[] ItemsElementName { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Hotspots : object
    {
        [XmlElement("Hotspot")]
        public point3dType[] Hotspot { get; set; }
    }

    [Serializable]
    [XmlRoot("Hotspot", Namespace = "", IsNullable = false)]
    public class point3dType : object
    {
        [XmlAttribute]
        public float X { get; set; }

        [XmlAttribute]
        public float Y { get; set; }

        [XmlAttribute]
        public float Z { get; set; }
    }

    [Serializable]
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType1
    {
        Blacklist,
        Factions,
        Hotspots,
        LootRadius,
        MaxDistance,
        MaximumHotspotTime,
        Name,
        RandomizeHotspots,
        TargetMaxLevel,
        TargetMinLevel,
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Mailboxes : object
    {
        [XmlElement("Mailbox", Form = XmlSchemaForm.Unqualified)]
        public mailboxType[] Items { get; set; }
    }


    [Serializable]
    public class mailboxType : object
    {
        public mailboxType()
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
    public enum navType
    {
        Fly,
        Run,
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class ProtectedItems : object
    {
        private itemType[] itemsField;

        [XmlElement("Item", Form = XmlSchemaForm.Unqualified)]
        public itemType[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }
    }


    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Vendors : object
    {
        private vendorType[] itemsField;


        [XmlElement("Vendor", Form = XmlSchemaForm.Unqualified)]
        public vendorType[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }
    }


    [Serializable]
    public class vendorType : object
    {
        public vendorType()
        {
            Nav = navType.Run;
        }

        [XmlAttribute(DataType = "positiveInteger")]
        public string Entry { get; set; }

        [XmlAttribute(DataType = "positiveInteger")]
        public string Entry2 { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute, DefaultValue(navType.Run)]
        public navType Nav { get; set; }

        [XmlAttribute]
        public vendorTypeType Type { get; set; }

        [XmlAttribute]
        public float X { get; set; }

        [XmlAttribute]
        public float Y { get; set; }

        [XmlAttribute]
        public float Z { get; set; }
    }


    [Serializable]
    public enum vendorTypeType
    {
        Food,
        Repair,
    }


    [Serializable]
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType2
    {
        AvoidMobs,
        Blacklist,
        Blackspots,
        ContinentId,
        ForceMail,
        GrindArea,
        Hotspots,
        MailBlue,
        MailGreen,
        MailGrey,
        MailPurple,
        MailWhite,
        Mailboxes,
        MaxLevel,
        MinDurability,
        MinFreeBagSlots,
        MinLevel,
        Name,
        ProtectedItems,
        SellBlue,
        SellGreen,
        SellGrey,
        SellPurple,
        SellWhite,
        TargetElites,
        Vendors,
        Quest,
        QuestOrder
    }


    [Serializable]
    public class avoidmobType : mobType
    {
    }


    [Serializable]
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {
        AvoidMob,
        Mob,
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class HBProfile : subProfileType
    {
        [XmlElement("SubProfile", Form = XmlSchemaForm.Unqualified)]
        public subProfileType[] Items1 { get; set; }
    }
}