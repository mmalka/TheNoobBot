using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace Battlegrounder.Profile
{
    [Serializable]
    public class BattlegrounderProfile
    {
        public List<Point> Points = new List<Point>();

        public List<BattlegrounderBlackListRadius> BlackListRadius = new List<BattlegrounderBlackListRadius>();
    }

    [Serializable]
    public class BattlegrounderBlackListRadius
    {
        public Point Position = new Point();
        public float Radius;
    }
}