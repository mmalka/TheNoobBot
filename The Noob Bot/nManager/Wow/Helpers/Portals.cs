using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    [Serializable]
    public class Portals : Transports
    {
        public new List<Portal> Items = new List<Portal>();
    }

    [Serializable]
    public class Portal : Transport
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
    }
}