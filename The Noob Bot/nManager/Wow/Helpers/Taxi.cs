using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    [Serializable]
    public class Taxi : Transport
    {
        public Point Position = new Point();
        [DefaultValue("")] public string Xcoord;
        [DefaultValue("")] public string Ycoord;
        [XmlAttribute(AttributeName = "ContinentId")] public int ContinentId;
        [DefaultValue(0)] public uint EndOfPath;

        [DefaultValue(0)] [XmlAttribute(AttributeName = "RequireAchievementId")] public int RequireAchivementId;
        [DefaultValue(0)] [XmlAttribute(AttributeName = "RequireQuestId")] public int RequireQuestId;
    }

    [Serializable]
    public class TaxiLink
    {
        public uint PointA;
        public uint PointB;
        [DefaultValue("")] public string PointB_XY; // temporary field if we don't know PointB ID yet.
    }
}