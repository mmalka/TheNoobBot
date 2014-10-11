using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    [Serializable]
    public class Portals
    {
        public List<Portal> Items = new List<Portal>();
    }

    [Serializable]
    public class Portal
    {
        [XmlAttribute(AttributeName = "AContinentId")] public int AContinentId;
        public Point APoint = new Point();
        [XmlAttribute(AttributeName = "BContinentId")] public int BContinentId;
        public Point BPoint = new Point();
        [XmlAttribute(AttributeName = "Faction")] public Npc.FactionType Faction;
        [XmlAttribute(AttributeName = "Id")] public uint Id;
        [XmlAttribute(AttributeName = "Name")] public string Name;
        [DefaultValue(0)] [XmlAttribute(AttributeName = "RequireAchievementId")] public int RequireAchivementId;
        [DefaultValue(0)] [XmlAttribute(AttributeName = "RequireQuestId")] public int RequireQuestId;
    }
}