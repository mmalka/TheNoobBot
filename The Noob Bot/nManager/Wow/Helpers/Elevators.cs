using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    [Serializable]
    public class Elevators
    {
        public List<Elevator> Items = new List<Elevator>();
    }

    [Serializable]
    public class Elevator
    {
        [XmlAttribute(AttributeName = "Id")]
        public uint Id;
        [XmlAttribute(AttributeName = "Name")]
        public string Name;
        [XmlAttribute(AttributeName = "Faction")]
        public Npc.FactionType Faction;
        [XmlAttribute(AttributeName = "Continent")]
        public int ContinentId;
        public Point BottomPoint = new Point();
        public Point OutsideBottomPoint = new Point();
        public Point TopPoint = new Point();
        public Point OutsideTopPoint = new Point();
    }
}
