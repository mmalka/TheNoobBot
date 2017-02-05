using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    [Serializable]
    public class CustomPaths
    {
        public List<CustomPath> Items = new List<CustomPath>();
    }

    [Serializable]
    public class CustomPath : Transport
    {
        [XmlAttribute(AttributeName = "AContinentId")] public new int AContinentId;
        public new Point APoint = new Point();
        [XmlAttribute(AttributeName = "BContinentId")] public new int BContinentId;
        public new Point BPoint = new Point();
        [XmlAttribute(AttributeName = "Faction")] public new Npc.FactionType Faction;
        [XmlAttribute(AttributeName = "Id")] public new uint Id;
        [XmlAttribute(AttributeName = "Name")] public new string Name;
        [DefaultValue(0)] [XmlAttribute(AttributeName = "RequireAchievementId")] public int RequireAchivementId;
        [DefaultValue(0)] [XmlAttribute(AttributeName = "RequireQuestId")] public int RequireQuestId;
        public List<Point> Points; // The path should work both ways.
        [DefaultValue(true)] public bool RoundTrip;
    }
}