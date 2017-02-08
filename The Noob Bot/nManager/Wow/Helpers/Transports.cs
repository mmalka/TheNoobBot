using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    [Serializable]
    public class Transports
    {
        public List<Transport> Items = new List<Transport>();
    }

    [Serializable]
    public class Transport
    {
        [XmlAttribute(AttributeName = "AContinentId"), DefaultValue(0)] public int AContinentId;
        public Point AInsidePoint = new Point();
        public Point AOutsidePoint = new Point();
        public Point APoint = new Point();
        [DefaultValue(0)] public uint ALift = 0;
        [XmlIgnore] public bool UseALift = false;
        [XmlAttribute(AttributeName = "BContinentId"), DefaultValue(0)] public int BContinentId;
        public Point BInsidePoint = new Point();
        public Point BOutsidePoint = new Point();
        public Point BPoint = new Point();
        [DefaultValue(0)] public uint BLift = 0;
        [XmlIgnore] public bool UseBLift = false;
        [XmlAttribute(AttributeName = "Faction")] public Npc.FactionType Faction;
        [XmlAttribute(AttributeName = "Id")] public uint Id;
        [XmlAttribute(AttributeName = "Name")] public string Name;
        [XmlIgnore] public bool ArrivalIsA = false;

        public bool ShouldSerializeAInsidePoint()
        {
            return AInsidePoint.IsValid;
        }

        public bool ShouldSerializeBInsidePoint()
        {
            return AOutsidePoint.IsValid;
        }

        public bool ShouldSerializeAOutsidePoint()
        {
            return AOutsidePoint.IsValid;
        }

        public bool ShouldSerializeBOutsidePoint()
        {
            return BOutsidePoint.IsValid;
        }
    }
}