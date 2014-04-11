using System;
using System.Collections.Generic;
using nManager.Wow.Class;
using System.ComponentModel;

namespace Gatherer
{
    [Serializable]
    public class GathererProfile
    {
        public List<Point> Points = new List<Point>();

        public List<Npc> Npc = new List<Npc>();
        public bool ShouldSerializeNpc() { return Npc != null && Npc.Count > 0; }

        public List<GathererBlackListRadius> BlackListRadius = new List<GathererBlackListRadius>();
        public bool ShouldSerializeBlackListRadius() { return BlackListRadius != null && BlackListRadius.Count > 0; }
    }

    [Serializable]
    public class GathererBlackListRadius
    {
        public Point Position = new Point();
        public float Radius;
    }
}