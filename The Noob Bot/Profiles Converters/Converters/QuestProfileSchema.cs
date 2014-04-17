using System;
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
    public class turninObjectiveType : object, INotifyPropertyChanged
    {
        private navType navField;

        private bool navFieldSpecified;

        private float xField;

        private float yField;

        private float zField;

        public turninObjectiveType()
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
    public class objectiveMetaType : object, INotifyPropertyChanged
    {
        private string collectCountField;

        private string itemIdField;
        private ItemsChoiceType3[] itemsElementNameField;
        private object[] itemsField;

        private string killCountField;

        private string mobIdField;

        private string nameField;

        private string objectIdField;

        private objectiveTypeType typeField;

        private string useCountField;

        /// <remarks />
        [XmlElement("CollectFrom", typeof (CollectFrom))]
        [XmlElement("Hotspots", typeof (Hotspots))]
        [XmlElement("TargetMaxLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlElement("TargetMinLevel", typeof (string), Form = XmlSchemaForm.Unqualified, DataType = "positiveInteger")]
        [XmlElement("TurnIn", typeof (turninObjectiveType), Form = XmlSchemaForm.Unqualified)]
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
        public ItemsChoiceType3[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set
            {
                itemsElementNameField = value;
                RaisePropertyChanged("ItemsElementName");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "positiveInteger")]
        public string CollectCount
        {
            get { return collectCountField; }
            set
            {
                collectCountField = value;
                RaisePropertyChanged("CollectCount");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "positiveInteger")]
        public string ItemId
        {
            get { return itemIdField; }
            set
            {
                itemIdField = value;
                RaisePropertyChanged("ItemId");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "positiveInteger")]
        public string KillCount
        {
            get { return killCountField; }
            set
            {
                killCountField = value;
                RaisePropertyChanged("KillCount");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "positiveInteger")]
        public string MobId
        {
            get { return mobIdField; }
            set
            {
                mobIdField = value;
                RaisePropertyChanged("MobId");
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
        [XmlAttribute(DataType = "positiveInteger")]
        public string ObjectId
        {
            get { return objectIdField; }
            set
            {
                objectIdField = value;
                RaisePropertyChanged("ObjectId");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public objectiveTypeType Type
        {
            get { return typeField; }
            set
            {
                typeField = value;
                RaisePropertyChanged("Type");
            }
        }

        /// <remarks />
        [XmlAttribute(DataType = "positiveInteger")]
        public string UseCount
        {
            get { return useCountField; }
            set
            {
                useCountField = value;
                RaisePropertyChanged("UseCount");
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
    public class CollectFrom : object, INotifyPropertyChanged
    {
        private object[] itemsField;

        /// <remarks />
        [XmlElement("GameObject", typeof (gameObjectType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("Mob", typeof (mobObjectiveType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("Vendor", typeof (vendorObjectiveType), Form = XmlSchemaForm.Unqualified)]
        public object[] Items
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
    public class gameObjectType : object, INotifyPropertyChanged
    {
        private string idField;
        private string nameField;
        private navType navField;

        private bool navFieldSpecified;

        public gameObjectType()
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
        public string Id
        {
            get { return idField; }
            set
            {
                idField = value;
                RaisePropertyChanged("Id");
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
    public class mobObjectiveType : object, INotifyPropertyChanged
    {
        private string entryField;
        private string idField;
        private string nameField;
        private navType navField;

        private bool navFieldSpecified;

        public mobObjectiveType()
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
        public string Id
        {
            get { return idField; }
            set
            {
                idField = value;
                RaisePropertyChanged("Id");
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
    public class vendorObjectiveType : object, INotifyPropertyChanged
    {
        private string idField;
        private string nameField;
        private navType navField;

        private bool navFieldSpecified;

        public vendorObjectiveType()
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
        public string Id
        {
            get { return idField; }
            set
            {
                idField = value;
                RaisePropertyChanged("Id");
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
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType3
    {
        /// <remarks />
        CollectFrom,

        /// <remarks />
        Hotspots,

        /// <remarks />
        TargetMaxLevel,

        /// <remarks />
        TargetMinLevel,

        /// <remarks />
        TurnIn,
    }

    /// <remarks />
    [Serializable]
    public enum objectiveTypeType
    {
        /// <remarks />
        TurnIn,

        /// <remarks />
        CollectItem,

        /// <remarks />
        KillMob,

        /// <remarks />
        UseObject,
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
    public enum booleanType
    {
        /// <remarks />
        True,

        /// <remarks />
        False,
    }

    /// <remarks />
    [Serializable]
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType1
    {
        /// <remarks />
        Blacklist,

        /// <remarks />
        Factions,

        /// <remarks />
        Hotspots,

        /// <remarks />
        LootRadius,

        /// <remarks />
        MaxDistance,

        /// <remarks />
        MaximumHotspotTime,

        /// <remarks />
        Name,

        /// <remarks />
        RandomizeHotspots,

        /// <remarks />
        TargetMaxLevel,

        /// <remarks />
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
        /// <remarks />
        Food,

        /// <remarks />
        Repair,
    }

    /// <remarks />
    [Serializable]
    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType2
    {
        /// <remarks />
        AvoidMobs,

        /// <remarks />
        Blacklist,

        /// <remarks />
        Blackspots,

        /// <remarks />
        ContinentId,

        /// <remarks />
        ForceMail,

        /// <remarks />
        GrindArea,

        /// <remarks />
        Hotspots,

        /// <remarks />
        MailBlue,

        /// <remarks />
        MailGreen,

        /// <remarks />
        MailGrey,

        /// <remarks />
        MailPurple,

        /// <remarks />
        MailWhite,

        /// <remarks />
        Mailboxes,

        /// <remarks />
        MaxLevel,

        /// <remarks />
        MinDurability,

        /// <remarks />
        MinFreeBagSlots,

        /// <remarks />
        MinLevel,

        /// <remarks />
        Name,

        /// <remarks />
        ProtectedItems,

        /// <remarks />
        SellBlue,

        /// <remarks />
        SellGreen,

        /// <remarks />
        SellGrey,

        /// <remarks />
        SellPurple,

        /// <remarks />
        SellWhite,

        /// <remarks />
        TargetElites,

        /// <remarks />
        Vendors,
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

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class CustomBehavior : object, INotifyPropertyChanged
    {
        private string allowBotStopField;

        private byte attackButtonField;

        private bool attackButtonFieldSpecified;

        private uint buyItemIdField;

        private bool buyItemIdFieldSpecified;

        private byte collectItemCountField;

        private bool collectItemCountFieldSpecified;

        private ushort collectItemIdField;

        private bool collectItemIdFieldSpecified;

        private string collectUntilField;
        private ushort collectionDistanceField;

        private bool collectionDistanceFieldSpecified;

        private string destNameField;

        private string doMailField;
        private string doRepairField;

        private string doSellField;

        private string fileField;

        private string goalTextField;

        private byte gossipOptionsField;

        private bool gossipOptionsFieldSpecified;

        private booleanType groundMountFarmingModeField;

        private bool groundMountFarmingModeFieldSpecified;

        private byte huntingGroundRadiusField;

        private bool huntingGroundRadiusFieldSpecified;

        private ushort idsField;

        private bool idsFieldSpecified;

        private string ignoreMobsInBlackspotsField;

        private ushort interactByUsingItemIdField;

        private bool interactByUsingItemIdFieldSpecified;
        private object[] itemsField;

        private booleanType killBetweenHotspotsField;

        private bool killBetweenHotspotsFieldSpecified;

        private string logColorField;

        private string luaField;

        private string macroField;

        private ushort maxRangeField;

        private bool maxRangeFieldSpecified;

        private ushort maxRangeToUseItemField;

        private bool maxRangeToUseItemFieldSpecified;

        private ushort minRangeField;

        private bool minRangeFieldSpecified;

        private uint mobId1Field;

        private bool mobId1FieldSpecified;

        private uint mobId2Field;

        private bool mobId2FieldSpecified;

        private uint mobId3Field;

        private bool mobId3FieldSpecified;

        private uint mobId4Field;

        private bool mobId4FieldSpecified;

        private uint mobId5Field;

        private bool mobId5FieldSpecified;

        private uint mobId6Field;

        private bool mobId6FieldSpecified;
        private uint mobIdField;

        private bool mobIdFieldSpecified;

        private string mobIdsField;

        private string mobStateField;

        private byte nonCompeteDistanceField;

        private bool nonCompeteDistanceFieldSpecified;

        private byte numOfTimesField;

        private bool numOfTimesFieldSpecified;

        private uint objectIdField;

        private bool objectIdFieldSpecified;

        private string objectTypeField;

        private string preInteractMountStrategyField;

        private uint questIdField;

        private bool questIdFieldSpecified;

        private byte questObjectiveIndexField;

        private bool questObjectiveIndexFieldSpecified;

        private byte rangeField;

        private bool rangeFieldSpecified;

        private string soundCueField;

        private byte soundCueIntervalField;

        private bool soundCueIntervalFieldSpecified;

        private string textField;

        private string typeField;

        private string useTypeField;

        private byte useWhenMobHasHealthPercentField;

        private bool useWhenMobHasHealthPercentFieldSpecified;

        private ushort vehicleMountIdField;

        private bool vehicleMountIdFieldSpecified;

        private string waitForNpcsField;

        private uint waitTimeField;

        private bool waitTimeFieldSpecified;

        private decimal xField;

        private bool xFieldSpecified;

        private decimal yField;

        private bool yFieldSpecified;

        private decimal zField;

        private bool zFieldSpecified;

        /// <remarks />
        [XmlElement("Hotspot", typeof (point3dType))]
        [XmlElement("HuntingGrounds", typeof (CustomBehaviorHuntingGrounds), Form = XmlSchemaForm.Unqualified)]
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
        [XmlAttribute]
        public string AllowBotStop
        {
            get { return allowBotStopField; }
            set
            {
                allowBotStopField = value;
                RaisePropertyChanged("AllowBotStop");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte AttackButton
        {
            get { return attackButtonField; }
            set
            {
                attackButtonField = value;
                RaisePropertyChanged("AttackButton");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool AttackButtonSpecified
        {
            get { return attackButtonFieldSpecified; }
            set
            {
                attackButtonFieldSpecified = value;
                RaisePropertyChanged("AttackButtonSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint BuyItemId
        {
            get { return buyItemIdField; }
            set
            {
                buyItemIdField = value;
                RaisePropertyChanged("BuyItemId");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool BuyItemIdSpecified
        {
            get { return buyItemIdFieldSpecified; }
            set
            {
                buyItemIdFieldSpecified = value;
                RaisePropertyChanged("BuyItemIdSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public ushort CollectionDistance
        {
            get { return collectionDistanceField; }
            set
            {
                collectionDistanceField = value;
                RaisePropertyChanged("CollectionDistance");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool CollectionDistanceSpecified
        {
            get { return collectionDistanceFieldSpecified; }
            set
            {
                collectionDistanceFieldSpecified = value;
                RaisePropertyChanged("CollectionDistanceSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte CollectItemCount
        {
            get { return collectItemCountField; }
            set
            {
                collectItemCountField = value;
                RaisePropertyChanged("CollectItemCount");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool CollectItemCountSpecified
        {
            get { return collectItemCountFieldSpecified; }
            set
            {
                collectItemCountFieldSpecified = value;
                RaisePropertyChanged("CollectItemCountSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public ushort CollectItemId
        {
            get { return collectItemIdField; }
            set
            {
                collectItemIdField = value;
                RaisePropertyChanged("CollectItemId");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool CollectItemIdSpecified
        {
            get { return collectItemIdFieldSpecified; }
            set
            {
                collectItemIdFieldSpecified = value;
                RaisePropertyChanged("CollectItemIdSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string CollectUntil
        {
            get { return collectUntilField; }
            set
            {
                collectUntilField = value;
                RaisePropertyChanged("CollectUntil");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string DestName
        {
            get { return destNameField; }
            set
            {
                destNameField = value;
                RaisePropertyChanged("DestName");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string DoRepair
        {
            get { return doRepairField; }
            set
            {
                doRepairField = value;
                RaisePropertyChanged("DoRepair");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string DoMail
        {
            get { return doMailField; }
            set
            {
                doMailField = value;
                RaisePropertyChanged("DoMail");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string DoSell
        {
            get { return doSellField; }
            set
            {
                doSellField = value;
                RaisePropertyChanged("DoSell");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string File
        {
            get { return fileField; }
            set
            {
                fileField = value;
                RaisePropertyChanged("File");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string GoalText
        {
            get { return goalTextField; }
            set
            {
                goalTextField = value;
                RaisePropertyChanged("GoalText");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte GossipOptions
        {
            get { return gossipOptionsField; }
            set
            {
                gossipOptionsField = value;
                RaisePropertyChanged("GossipOptions");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool GossipOptionsSpecified
        {
            get { return gossipOptionsFieldSpecified; }
            set
            {
                gossipOptionsFieldSpecified = value;
                RaisePropertyChanged("GossipOptionsSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public booleanType GroundMountFarmingMode
        {
            get { return groundMountFarmingModeField; }
            set
            {
                groundMountFarmingModeField = value;
                RaisePropertyChanged("GroundMountFarmingMode");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool GroundMountFarmingModeSpecified
        {
            get { return groundMountFarmingModeFieldSpecified; }
            set
            {
                groundMountFarmingModeFieldSpecified = value;
                RaisePropertyChanged("GroundMountFarmingModeSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte HuntingGroundRadius
        {
            get { return huntingGroundRadiusField; }
            set
            {
                huntingGroundRadiusField = value;
                RaisePropertyChanged("HuntingGroundRadius");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool HuntingGroundRadiusSpecified
        {
            get { return huntingGroundRadiusFieldSpecified; }
            set
            {
                huntingGroundRadiusFieldSpecified = value;
                RaisePropertyChanged("HuntingGroundRadiusSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public ushort Ids
        {
            get { return idsField; }
            set
            {
                idsField = value;
                RaisePropertyChanged("Ids");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool IdsSpecified
        {
            get { return idsFieldSpecified; }
            set
            {
                idsFieldSpecified = value;
                RaisePropertyChanged("IdsSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string IgnoreMobsInBlackspots
        {
            get { return ignoreMobsInBlackspotsField; }
            set
            {
                ignoreMobsInBlackspotsField = value;
                RaisePropertyChanged("IgnoreMobsInBlackspots");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public ushort InteractByUsingItemId
        {
            get { return interactByUsingItemIdField; }
            set
            {
                interactByUsingItemIdField = value;
                RaisePropertyChanged("InteractByUsingItemId");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool InteractByUsingItemIdSpecified
        {
            get { return interactByUsingItemIdFieldSpecified; }
            set
            {
                interactByUsingItemIdFieldSpecified = value;
                RaisePropertyChanged("InteractByUsingItemIdSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public booleanType KillBetweenHotspots
        {
            get { return killBetweenHotspotsField; }
            set
            {
                killBetweenHotspotsField = value;
                RaisePropertyChanged("KillBetweenHotspots");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool KillBetweenHotspotsSpecified
        {
            get { return killBetweenHotspotsFieldSpecified; }
            set
            {
                killBetweenHotspotsFieldSpecified = value;
                RaisePropertyChanged("KillBetweenHotspotsSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string LogColor
        {
            get { return logColorField; }
            set
            {
                logColorField = value;
                RaisePropertyChanged("LogColor");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string Lua
        {
            get { return luaField; }
            set
            {
                luaField = value;
                RaisePropertyChanged("Lua");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string Macro
        {
            get { return macroField; }
            set
            {
                macroField = value;
                RaisePropertyChanged("Macro");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public ushort MaxRange
        {
            get { return maxRangeField; }
            set
            {
                maxRangeField = value;
                RaisePropertyChanged("MaxRange");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MaxRangeSpecified
        {
            get { return maxRangeFieldSpecified; }
            set
            {
                maxRangeFieldSpecified = value;
                RaisePropertyChanged("MaxRangeSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public ushort MaxRangeToUseItem
        {
            get { return maxRangeToUseItemField; }
            set
            {
                maxRangeToUseItemField = value;
                RaisePropertyChanged("MaxRangeToUseItem");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MaxRangeToUseItemSpecified
        {
            get { return maxRangeToUseItemFieldSpecified; }
            set
            {
                maxRangeToUseItemFieldSpecified = value;
                RaisePropertyChanged("MaxRangeToUseItemSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public ushort MinRange
        {
            get { return minRangeField; }
            set
            {
                minRangeField = value;
                RaisePropertyChanged("MinRange");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MinRangeSpecified
        {
            get { return minRangeFieldSpecified; }
            set
            {
                minRangeFieldSpecified = value;
                RaisePropertyChanged("MinRangeSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint MobId
        {
            get { return mobIdField; }
            set
            {
                mobIdField = value;
                RaisePropertyChanged("MobId");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MobIdSpecified
        {
            get { return mobIdFieldSpecified; }
            set
            {
                mobIdFieldSpecified = value;
                RaisePropertyChanged("MobIdSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint MobId1
        {
            get { return mobId1Field; }
            set
            {
                mobId1Field = value;
                RaisePropertyChanged("MobId1");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MobId1Specified
        {
            get { return mobId1FieldSpecified; }
            set
            {
                mobId1FieldSpecified = value;
                RaisePropertyChanged("MobId1Specified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint MobId2
        {
            get { return mobId2Field; }
            set
            {
                mobId2Field = value;
                RaisePropertyChanged("MobId2");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MobId2Specified
        {
            get { return mobId2FieldSpecified; }
            set
            {
                mobId2FieldSpecified = value;
                RaisePropertyChanged("MobId2Specified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint MobId3
        {
            get { return mobId3Field; }
            set
            {
                mobId3Field = value;
                RaisePropertyChanged("MobId3");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MobId3Specified
        {
            get { return mobId3FieldSpecified; }
            set
            {
                mobId3FieldSpecified = value;
                RaisePropertyChanged("MobId3Specified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint MobId4
        {
            get { return mobId4Field; }
            set
            {
                mobId4Field = value;
                RaisePropertyChanged("MobId4");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MobId4Specified
        {
            get { return mobId4FieldSpecified; }
            set
            {
                mobId4FieldSpecified = value;
                RaisePropertyChanged("MobId4Specified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint MobId5
        {
            get { return mobId5Field; }
            set
            {
                mobId5Field = value;
                RaisePropertyChanged("MobId5");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MobId5Specified
        {
            get { return mobId5FieldSpecified; }
            set
            {
                mobId5FieldSpecified = value;
                RaisePropertyChanged("MobId5Specified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint MobId6
        {
            get { return mobId6Field; }
            set
            {
                mobId6Field = value;
                RaisePropertyChanged("MobId6");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool MobId6Specified
        {
            get { return mobId6FieldSpecified; }
            set
            {
                mobId6FieldSpecified = value;
                RaisePropertyChanged("MobId6Specified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string MobIds
        {
            get { return mobIdsField; }
            set
            {
                mobIdsField = value;
                RaisePropertyChanged("MobIds");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string MobState
        {
            get { return mobStateField; }
            set
            {
                mobStateField = value;
                RaisePropertyChanged("MobState");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte NonCompeteDistance
        {
            get { return nonCompeteDistanceField; }
            set
            {
                nonCompeteDistanceField = value;
                RaisePropertyChanged("NonCompeteDistance");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool NonCompeteDistanceSpecified
        {
            get { return nonCompeteDistanceFieldSpecified; }
            set
            {
                nonCompeteDistanceFieldSpecified = value;
                RaisePropertyChanged("NonCompeteDistanceSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte NumOfTimes
        {
            get { return numOfTimesField; }
            set
            {
                numOfTimesField = value;
                RaisePropertyChanged("NumOfTimes");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool NumOfTimesSpecified
        {
            get { return numOfTimesFieldSpecified; }
            set
            {
                numOfTimesFieldSpecified = value;
                RaisePropertyChanged("NumOfTimesSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint ObjectId
        {
            get { return objectIdField; }
            set
            {
                objectIdField = value;
                RaisePropertyChanged("ObjectId");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool ObjectIdSpecified
        {
            get { return objectIdFieldSpecified; }
            set
            {
                objectIdFieldSpecified = value;
                RaisePropertyChanged("ObjectIdSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string ObjectType
        {
            get { return objectTypeField; }
            set
            {
                objectTypeField = value;
                RaisePropertyChanged("ObjectType");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string PreInteractMountStrategy
        {
            get { return preInteractMountStrategyField; }
            set
            {
                preInteractMountStrategyField = value;
                RaisePropertyChanged("PreInteractMountStrategy");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint QuestId
        {
            get { return questIdField; }
            set
            {
                questIdField = value;
                RaisePropertyChanged("QuestId");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool QuestIdSpecified
        {
            get { return questIdFieldSpecified; }
            set
            {
                questIdFieldSpecified = value;
                RaisePropertyChanged("QuestIdSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte QuestObjectiveIndex
        {
            get { return questObjectiveIndexField; }
            set
            {
                questObjectiveIndexField = value;
                RaisePropertyChanged("QuestObjectiveIndex");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool QuestObjectiveIndexSpecified
        {
            get { return questObjectiveIndexFieldSpecified; }
            set
            {
                questObjectiveIndexFieldSpecified = value;
                RaisePropertyChanged("QuestObjectiveIndexSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte Range
        {
            get { return rangeField; }
            set
            {
                rangeField = value;
                RaisePropertyChanged("Range");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool RangeSpecified
        {
            get { return rangeFieldSpecified; }
            set
            {
                rangeFieldSpecified = value;
                RaisePropertyChanged("RangeSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string SoundCue
        {
            get { return soundCueField; }
            set
            {
                soundCueField = value;
                RaisePropertyChanged("SoundCue");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte SoundCueInterval
        {
            get { return soundCueIntervalField; }
            set
            {
                soundCueIntervalField = value;
                RaisePropertyChanged("SoundCueInterval");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool SoundCueIntervalSpecified
        {
            get { return soundCueIntervalFieldSpecified; }
            set
            {
                soundCueIntervalFieldSpecified = value;
                RaisePropertyChanged("SoundCueIntervalSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string Text
        {
            get { return textField; }
            set
            {
                textField = value;
                RaisePropertyChanged("Text");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string Type
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
        public string UseType
        {
            get { return useTypeField; }
            set
            {
                useTypeField = value;
                RaisePropertyChanged("UseType");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public byte UseWhenMobHasHealthPercent
        {
            get { return useWhenMobHasHealthPercentField; }
            set
            {
                useWhenMobHasHealthPercentField = value;
                RaisePropertyChanged("UseWhenMobHasHealthPercent");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool UseWhenMobHasHealthPercentSpecified
        {
            get { return useWhenMobHasHealthPercentFieldSpecified; }
            set
            {
                useWhenMobHasHealthPercentFieldSpecified = value;
                RaisePropertyChanged("UseWhenMobHasHealthPercentSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public ushort VehicleMountId
        {
            get { return vehicleMountIdField; }
            set
            {
                vehicleMountIdField = value;
                RaisePropertyChanged("VehicleMountId");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool VehicleMountIdSpecified
        {
            get { return vehicleMountIdFieldSpecified; }
            set
            {
                vehicleMountIdFieldSpecified = value;
                RaisePropertyChanged("VehicleMountIdSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string WaitForNpcs
        {
            get { return waitForNpcsField; }
            set
            {
                waitForNpcsField = value;
                RaisePropertyChanged("WaitForNpcs");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint WaitTime
        {
            get { return waitTimeField; }
            set
            {
                waitTimeField = value;
                RaisePropertyChanged("WaitTime");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool WaitTimeSpecified
        {
            get { return waitTimeFieldSpecified; }
            set
            {
                waitTimeFieldSpecified = value;
                RaisePropertyChanged("WaitTimeSpecified");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public decimal X
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
        public decimal Y
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
        public decimal Z
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
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class CustomBehaviorHuntingGrounds : object, INotifyPropertyChanged
    {
        private point3dType hotspotField;

        private string waypointVisitStrategyField;

        /// <remarks />
        public point3dType Hotspot
        {
            get { return hotspotField; }
            set
            {
                hotspotField = value;
                RaisePropertyChanged("Hotspot");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string WaypointVisitStrategy
        {
            get { return waypointVisitStrategyField; }
            set
            {
                waypointVisitStrategyField = value;
                RaisePropertyChanged("WaypointVisitStrategy");
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
    public class MoveTo : object, INotifyPropertyChanged
    {
        private string destNameField;

        private navType navField;

        private bool navFieldSpecified;
        private uint questIdField;

        private bool questIdFieldSpecified;

        private float xField;

        private float yField;

        private float zField;

        public MoveTo()
        {
            navField = navType.Run;
        }

        /// <remarks />
        [XmlAttribute]
        public string DestName
        {
            get { return destNameField; }
            set
            {
                destNameField = value;
                RaisePropertyChanged("DestName");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint QuestId
        {
            get { return questIdField; }
            set
            {
                questIdField = value;
                RaisePropertyChanged("QuestId");
            }
        }

        /// <remarks />
        [XmlIgnore]
        public bool QuestIdSpecified
        {
            get { return questIdFieldSpecified; }
            set
            {
                questIdFieldSpecified = value;
                RaisePropertyChanged("QuestIdSpecified");
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
    public class PickUp : object, INotifyPropertyChanged
    {
        private uint giverIdField;

        private string giverNameField;

        private navType navField;

        private bool navFieldSpecified;

        private uint questIdField;
        private string questNameField;

        private float xField;

        private float yField;

        private float zField;

        public PickUp()
        {
            navField = navType.Run;
        }

        /// <remarks />
        [XmlAttribute]
        public uint GiverId
        {
            get { return giverIdField; }
            set
            {
                giverIdField = value;
                RaisePropertyChanged("GiverId");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public string GiverName
        {
            get { return giverNameField; }
            set
            {
                giverNameField = value;
                RaisePropertyChanged("GiverName");
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
        public string QuestName
        {
            get { return questNameField; }
            set
            {
                questNameField = value;
                RaisePropertyChanged("QuestName");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint QuestId
        {
            get { return questIdField; }
            set
            {
                questIdField = value;
                RaisePropertyChanged("QuestId");
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
    public class Quest : object, INotifyPropertyChanged
    {
        private string idField;
        private object[] itemsField;

        private string nameField;

        /// <remarks />
        [XmlElement("Objective", typeof (objectiveMetaType), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("TurnIn", typeof (turninObjectiveType), Form = XmlSchemaForm.Unqualified)]
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
        [XmlAttribute(DataType = "positiveInteger")]
        public string Id
        {
            get { return idField; }
            set
            {
                idField = value;
                RaisePropertyChanged("Id");
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
    public class TurnIn : object, INotifyPropertyChanged
    {
        private navType navField;

        private bool navFieldSpecified;

        private uint questIdField;
        private string questNameField;
        private uint turnInIdField;
        private string turnInNameField;

        private float xField;

        private float yField;

        private float zField;

        public TurnIn()
        {
            navField = navType.Run;
        }

        /// <remarks />
        [XmlAttribute]
        public string TurnInName
        {
            get { return turnInNameField; }
            set
            {
                turnInNameField = value;
                RaisePropertyChanged("TurnInName");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint TurnInId
        {
            get { return turnInIdField; }
            set
            {
                turnInIdField = value;
                RaisePropertyChanged("TurnInId");
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
        public string QuestName
        {
            get { return questNameField; }
            set
            {
                questNameField = value;
                RaisePropertyChanged("QuestName");
            }
        }

        /// <remarks />
        [XmlAttribute]
        public uint QuestId
        {
            get { return questIdField; }
            set
            {
                questIdField = value;
                RaisePropertyChanged("QuestId");
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
}