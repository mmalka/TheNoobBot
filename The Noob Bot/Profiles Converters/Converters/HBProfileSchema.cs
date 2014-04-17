using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Profiles_Converters.Converters
{
    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class AvoidMobs : object, INotifyPropertyChanged
    {
        private ItemsChoiceType[] itemsElementNameField;
        private mobType[] itemsField;

        /// <remarks />
        [XmlElement("AvoidMob", typeof (mobType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("Mob", typeof (mobType), Form = XmlSchemaForm.Unqualified)]
        [XmlChoiceIdentifier("ItemsElementName")]
        public mobType[] Items
        {
            get { return itemsField; }
            set
            {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }

        /// <remarks />
        [XmlElement("ItemsElementName")]
        [XmlIgnore]
        public ItemsChoiceType[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set
            {
                itemsElementNameField = value;
                RaisePropertyChanged("ItemsElementName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [XmlInclude(typeof (blacklistMobType))]
    [XmlInclude(typeof (avoidmobType))]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class mobType : object, INotifyPropertyChanged
    {
        private string entryField;
        private string nameField;

        /// <remarks />
        [XmlAttribute]
        public string Name
        {
            get { return nameField; }
            set
            {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "positiveInteger")]
        public string Entry
        {
            get { return entryField; }
            set
            {
                entryField = value;
                RaisePropertyChanged("Entry");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class subProfileType : object, INotifyPropertyChanged
    {
        private ItemsChoiceType2[] itemsElementNameField;
        private object[] itemsField;

        /// <remarks />
        [XmlElement("AvoidMobs", typeof (AvoidMobs))]
        [XmlElement("Blacklist", typeof (Blacklist))]
        [XmlElement("Blackspots", typeof (Blackspots))]
        [XmlElement("ContinentId", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger")]
        [XmlElement("ForceMail", typeof (ForceMail))]
        [XmlElement("GrindArea", typeof (GrindArea))]
        [XmlElement("Hotspots", typeof (Hotspots))]
        [XmlElement("MailBlue", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("MailGreen", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("MailGrey", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("MailPurple", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("MailWhite", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("Mailboxes", typeof (Mailboxes))]
        [XmlElement("MaxLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlElement("MinDurability", typeof (float), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("MinFreeBagSlots", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger")]
        [XmlElement("MinLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlElement("Name", typeof (string), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("ProtectedItems", typeof (ProtectedItems))]
        [XmlElement("SellBlue", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("SellGreen", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("SellGrey", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("SellPurple", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("SellWhite", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("TargetElites", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("Vendors", typeof (Vendors))]
        [XmlElement("Quest", typeof(Quest))]
        [XmlElement("QuestOrder", typeof(Quest))]
        //public List<Quest> Quest { get; set; }
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items
        {
            get { return itemsField; }
            set
            {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }

        /// <remarks />
        [XmlElement("ItemsElementName")]
        [XmlIgnore]
        public ItemsChoiceType2[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set
            {
                itemsElementNameField = value;
                RaisePropertyChanged("ItemsElementName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Blacklist : object, INotifyPropertyChanged
    {
        private blacklistMobType[] mobField;

        /// <remarks />
        [XmlElement("Mob", Form = XmlSchemaForm.Unqualified)]
        public blacklistMobType[] Mob
        {
            get { return mobField; }
            set
            {
                mobField = value;
                RaisePropertyChanged("Mob");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class blacklistMobType : mobType
    {
        private blacklistFlagsType flagsField;

        /// <remarks />
        [XmlAttribute]
        public blacklistFlagsType Flags
        {
            get { return flagsField; }
            set
            {
                flagsField = value;
                RaisePropertyChanged("Flags");
            }
        }
    }

    /// <remarks />
    [Serializable]
    public enum blacklistFlagsType
    {
        /// <remarks />
        All,

        /// <remarks />
        Combat,

        /// <remarks />
        Interact,

        /// <remarks />
        Loot,

        /// <remarks />
        Node,

        /// <remarks />
        Pull,
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Blackspots : object, INotifyPropertyChanged
    {
        private blackspotType[] blackspotField;

        /// <remarks />
        [XmlElement("Blackspot", Form = XmlSchemaForm.Unqualified)]
        public blackspotType[] Blackspot
        {
            get { return blackspotField; }
            set
            {
                blackspotField = value;
                RaisePropertyChanged("Blackspot");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class blackspotType : object, INotifyPropertyChanged
    {
        private float heightField;

        private float radiusField;

        private float xField;

        private float yField;

        private float zField;

        public blackspotType()
        {
            heightField = 0F;
        }

        /// <remarks />
        [XmlAttribute]
        [DefaultValue(typeof (float), "0")]
        public float Height
        {
            get { return heightField; }
            set
            {
                heightField = value;
                RaisePropertyChanged("Height");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float Radius
        {
            get { return radiusField; }
            set
            {
                radiusField = value;
                RaisePropertyChanged("Radius");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float X
        {
            get { return xField; }
            set
            {
                xField = value;
                RaisePropertyChanged("X");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float Y
        {
            get { return yField; }
            set
            {
                yField = value;
                RaisePropertyChanged("Y");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float Z
        {
            get { return zField; }
            set
            {
                zField = value;
                RaisePropertyChanged("Z");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class ForceMail : object, INotifyPropertyChanged
    {
        private itemType[] itemsField;

        /// <remarks />
        [XmlElement("Item", Form = XmlSchemaForm.Unqualified)]
        public itemType[] Items
        {
            get { return itemsField; }
            set
            {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class itemType : object, INotifyPropertyChanged
    {
        private string entryField;
        private string nameField;

        /// <remarks />
        [XmlAttribute]
        public string Name
        {
            get { return nameField; }
            set
            {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "positiveInteger")]
        public string Entry
        {
            get { return entryField; }
            set
            {
                entryField = value;
                RaisePropertyChanged("Entry");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class GrindArea : object, INotifyPropertyChanged
    {
        private ItemsChoiceType1[] itemsElementNameField;
        private object[] itemsField;

        /// <remarks />
        [XmlElement("Blacklist", typeof (Blacklist))]
        [XmlElement("Factions", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlElement("Hotspots", typeof (Hotspots))]
        [XmlElement("LootRadius", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlElement("MaxDistance", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlElement("MaximumHotspotTime", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlElement("Name", typeof (string), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("RandomizeHotspots", typeof (booleanType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("TargetMaxLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlElement("TargetMinLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items
        {
            get { return itemsField; }
            set
            {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }

        /// <remarks />
        [XmlElement("ItemsElementName")]
        [XmlIgnore]
        public ItemsChoiceType1[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set
            {
                itemsElementNameField = value;
                RaisePropertyChanged("ItemsElementName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Hotspots : object, INotifyPropertyChanged
    {
        private point3dType[] hotspotField;

        /// <remarks />
        [XmlElement("Hotspot")]
        public point3dType[] Hotspot
        {
            get { return hotspotField; }
            set
            {
                hotspotField = value;
                RaisePropertyChanged("Hotspot");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlRoot("Hotspot", Namespace = "", IsNullable = false)]
    public class point3dType : object, INotifyPropertyChanged
    {
        private float xField;

        private float yField;

        private float zField;

        /// <remarks />
        [XmlAttribute]
        public float X
        {
            get { return xField; }
            set
            {
                xField = value;
                RaisePropertyChanged("X");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float Y
        {
            get { return yField; }
            set
            {
                yField = value;
                RaisePropertyChanged("Y");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float Z
        {
            get { return zField; }
            set
            {
                zField = value;
                RaisePropertyChanged("Z");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    public enum booleanType
    {
        True,
        False,
    }

    /// <remarks />
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

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Mailboxes : object, INotifyPropertyChanged
    {
        private mailboxType[] itemsField;

        /// <remarks />
        [XmlElement("Mailbox", Form = XmlSchemaForm.Unqualified)]
        public mailboxType[] Items
        {
            get { return itemsField; }
            set
            {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class mailboxType : object, INotifyPropertyChanged
    {
        private navType navField;

        private bool navFieldSpecified;

        private float xField;

        private float yField;

        private float zField;

        public mailboxType()
        {
            navField = navType.Run;
        }

        /// <remarks />
        [XmlAttribute]
        [DefaultValue(navType.Run)]
        public navType Nav
        {
            get { return navField; }
            set
            {
                navField = value;
                RaisePropertyChanged("Nav");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool NavSpecified
        {
            get { return navFieldSpecified; }
            set
            {
                navFieldSpecified = value;
                RaisePropertyChanged("NavSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float X
        {
            get { return xField; }
            set
            {
                xField = value;
                RaisePropertyChanged("X");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float Y
        {
            get { return yField; }
            set
            {
                yField = value;
                RaisePropertyChanged("Y");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float Z
        {
            get { return zField; }
            set
            {
                zField = value;
                RaisePropertyChanged("Z");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    public enum navType
    {
        /// <remarks />
        Fly,

        /// <remarks />
        Run,
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class ProtectedItems : object, INotifyPropertyChanged
    {
        private itemType[] itemsField;

        /// <remarks />
        [XmlElement("Item", Form = XmlSchemaForm.Unqualified)]
        public itemType[] Items
        {
            get { return itemsField; }
            set
            {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Vendors : object, INotifyPropertyChanged
    {
        private vendorType[] itemsField;

        /// <remarks />
        [XmlElement("Vendor", Form = XmlSchemaForm.Unqualified)]
        public vendorType[] Items
        {
            get { return itemsField; }
            set
            {
                itemsField = value;
                RaisePropertyChanged("Items");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class vendorType : object, INotifyPropertyChanged
    {
        private string entry2Field;
        private string entryField;

        private string nameField;

        private navType navField;

        private bool navFieldSpecified;

        private vendorTypeType typeField;

        private float xField;

        private bool xFieldSpecified;

        private float yField;

        private bool yFieldSpecified;

        private float zField;

        private bool zFieldSpecified;

        public vendorType()
        {
            navField = navType.Run;
        }

        /// <remarks />
        [XmlAttribute(DataType = "positiveInteger")]
        public string Entry
        {
            get { return entryField; }
            set
            {
                entryField = value;
                RaisePropertyChanged("Entry");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "positiveInteger")]
        public string Entry2
        {
            get { return entry2Field; }
            set
            {
                entry2Field = value;
                RaisePropertyChanged("Entry2");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string Name
        {
            get { return nameField; }
            set
            {
                nameField = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <remarks />
        [XmlAttribute]
        [DefaultValue(navType.Run)]
        public navType Nav
        {
            get { return navField; }
            set
            {
                navField = value;
                RaisePropertyChanged("Nav");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool NavSpecified
        {
            get { return navFieldSpecified; }
            set
            {
                navFieldSpecified = value;
                RaisePropertyChanged("NavSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public vendorTypeType Type
        {
            get { return typeField; }
            set
            {
                typeField = value;
                RaisePropertyChanged("Type");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float X
        {
            get { return xField; }
            set
            {
                xField = value;
                RaisePropertyChanged("X");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool XSpecified
        {
            get { return xFieldSpecified; }
            set
            {
                xFieldSpecified = value;
                RaisePropertyChanged("XSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float Y
        {
            get { return yField; }
            set
            {
                yField = value;
                RaisePropertyChanged("Y");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool YSpecified
        {
            get { return yFieldSpecified; }
            set
            {
                yFieldSpecified = value;
                RaisePropertyChanged("YSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public float Z
        {
            get { return zField; }
            set
            {
                zField = value;
                RaisePropertyChanged("Z");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool ZSpecified
        {
            get { return zFieldSpecified; }
            set
            {
                zFieldSpecified = value;
                RaisePropertyChanged("ZSpecified");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks />
    [Serializable]
    public enum vendorTypeType
    {
        Food,
        Repair,
    }

    /// <remarks />
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

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class avoidmobType : mobType
    {
    }

    /// <remarks />
    [Serializable]
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {
        /// <remarks />
        AvoidMob,

        /// <remarks />
        Mob,
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class HBProfile : subProfileType
    {
        private subProfileType[] items1Field;

        /// <remarks />
        [XmlElement("SubProfile", Form = XmlSchemaForm.Unqualified)]
        public subProfileType[] Items1
        {
            get { return items1Field; }
            set
            {
                items1Field = value;
                RaisePropertyChanged("Items1");
            }
        }
    }
}