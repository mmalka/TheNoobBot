using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Profiles_Converters.Converters
{
    public class HBProfile
    {
        public string Name = "ProfileTest1";
        public int MinLevel = 1;
        public int MaxLevel = 5;
        public double MinDurability = 0.3;
        public int MinFreeBagSlots = 3;

        public bool SellGrey = true;
        public bool SellWhite = false;
        public bool SellGreen = false;
        public bool SellBlue = false;
        public bool SellPurple = false;

        public bool MailGrey = false;
        public bool MailWhite = false;
        public bool MailGreen = false;
        public bool MailBlue = false;
        public bool MailPurple = false;

        public List<Vendor> Vendors = new List<Vendor>();

        public List<Mailbox> Mailboxes = new List<Mailbox>();

        public List<Blackspot> Blackspots = new List<Blackspot>();

        public List<Mob> AvoidMobs = new List<Mob>();

        [XmlElement("Quest")]
        //[XmlElement(IsNullable = false)] // Breaks serialize
        public List<QuestTemplate> Quest { get; set; }

        [Serializable]
        public class Vendor
        {
            //[DefaultValue("")]
            [XmlAttribute(AttributeName = "Name")] public string Name = "";
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Id")] public int Id = 0;
            //[DefaultValue("")]
            [XmlAttribute(AttributeName = "Type")] public string Type = "";
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "X")] public double X = 0;
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Y")] public double Y = 0;
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Z")] public double Z = 0;
        }

        [Serializable]
        public class Mailbox
        {
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "X")] public double X = 0;
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Y")] public double Y = 0;
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Z")] public double Z = 0;
        }

        [Serializable]
        public class Blackspot
        {
            //[DefaultValue("")]
            [XmlAttribute(AttributeName = "Name")] public string Name = "";
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "X")] public double X = 0;
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Y")] public double Y = 0;
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Z")] public double Z = 0;
            //[DefaultValue(1)]
            [XmlAttribute(AttributeName = "Radius")] public int Radius = 1;
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Height")] public double Height = 5;
        }

        [Serializable]
        public class Hotspot
        {
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "X")] public double X = 0;
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Y")] public double Y = 0;
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Z")] public double Z = 0;
        }

        [Serializable]
        public class Mob
        {
            //[DefaultValue("")]
            [XmlAttribute(AttributeName = "Name")] public string Name = "";
            //[DefaultValue(0)]
            [XmlAttribute(AttributeName = "Id")] public int Id = 0;
        }

        [Serializable]
        public class QuestTemplate
        {
            [XmlAttribute(AttributeName = "Id")] public int Id = 0;
            [XmlAttribute(AttributeName = "Name")] public string Name = "";

            [XmlElement("Objective")]
            //[XmlElement(IsNullable = false)] // Breaks serialize
            public List<QuestObjective> Objective { get; set; }
        }

        [Serializable]
        public class QuestObjective
        {
            public List<Hotspot> Hotspots = new List<Hotspot>();
            [XmlAttribute(AttributeName = "Type")] public ObjectiveType Type = ObjectiveType.None;
            [XmlAttribute(AttributeName = "MobId")] public int MobId = 0;
            [XmlAttribute(AttributeName = "Name")] public int KillCount = 0;
        }

        [Serializable]
        public enum ObjectiveType
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
    }
}