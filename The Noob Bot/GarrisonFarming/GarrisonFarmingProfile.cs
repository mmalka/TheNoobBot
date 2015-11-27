using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace GarrisonFarming
{
    [Serializable]
    public class GarrisonFarmingProfile
    {
        public List<GarrisonFarmingBlackListRadius> BlackListRadius = new List<GarrisonFarmingBlackListRadius>();
        public List<Npc> Npc = new List<Npc>();
        public List<Point> Points = new List<Point>();

        public bool ShouldSerializeNpc()
        {
            return Npc != null && Npc.Count > 0;
        }

        public bool ShouldSerializeBlackListRadius()
        {
            return BlackListRadius != null && BlackListRadius.Count > 0;
        }
    }

    [Serializable]
    public class GarrisonFarmingBlackListRadius
    {
        public Point Position = new Point();
        public float Radius;
    }
}