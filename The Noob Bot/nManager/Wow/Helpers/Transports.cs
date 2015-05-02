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
        [XmlAttribute(AttributeName = "BContinentId"), DefaultValue(0)] public int BContinentId;
        public Point BInsidePoint = new Point();
        public Point BOutsidePoint = new Point();
        public Point BPoint = new Point();
        [XmlAttribute(AttributeName = "Faction")] public Npc.FactionType Faction;
        [XmlAttribute(AttributeName = "Id")] public uint Id;
        [XmlAttribute(AttributeName = "Name")] public string Name;
        [XmlIgnore] public bool ArrivalIsA = false;
    }
}