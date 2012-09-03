using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace Gatherer
{
    [Serializable]
    public class GathererProfile
    {
        public List<Point> Points = new List<Point>();

        public List<Npc> Npc = new List<Npc>();

        public List<GathererBlackListRadius> BlackListRadius = new List<GathererBlackListRadius>();
    }
    [Serializable]
    public class GathererBlackListRadius
    {
        public Point Position = new Point();
        public float Radius;
    }
}
