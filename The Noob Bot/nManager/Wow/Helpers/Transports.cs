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
        [XmlAttribute(AttributeName = "Id")] public uint Id;
        [XmlAttribute(AttributeName = "Name")] public string Name;
        [XmlAttribute(AttributeName = "Faction")] public Npc.FactionType Faction;
        [XmlAttribute(AttributeName = "AContinentId"), DefaultValue(-1)] public int AContinentId = -1;
        [XmlAttribute(AttributeName = "BContinentId"), DefaultValue(-1)] public int BContinentId = -1;
        public Point AOutsidePoint = new Point();
        [DefaultValue(0)] public uint ALift = 0;
        [XmlIgnore] public bool UseALift = false;
        public Point BOutsidePoint = new Point();
        [XmlIgnore] public bool UseBLift = false;
        [DefaultValue(0)] public uint BLift = 0;
        public Point AInsidePoint = new Point();
        public Point BInsidePoint = new Point();
        public Point APoint = new Point();
        public Point BPoint = new Point();
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