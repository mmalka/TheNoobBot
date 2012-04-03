using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace Fisherbot.Profile
{
    [Serializable]
    public class FisherbotProfile
    {
        public List<Point> Points = new List<Point>();

        public List<FisherbotBlackListRadius> BlackListRadius = new List<FisherbotBlackListRadius>();
    }
    [Serializable]
    public class FisherbotBlackListRadius
    {
        public Point Position = new Point();
        public float Radius;
    }
}
