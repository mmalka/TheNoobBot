using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace Test_Product
{
    [Serializable]
    public class BattlegrounderProfile
    {
        public List<BattlegrounderZone> BattlegrounderZones = new List<BattlegrounderZone>();
    }

    [Serializable]
    public class BattlegrounderZone
    {
        public string Name = "";
        public string BattlegroundId;
        public bool Hotspots;
        public List<Point> Points = new List<Point>();
        public List<Npc> Npc = new List<Npc>();
        public List<BattlegrounderBlackListRadius> BlackListRadius = new List<BattlegrounderBlackListRadius>();

        internal bool IsValid()
        {
            try
            {
                return Points.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }

    [Serializable]
    public class BattlegrounderBlackListRadius
    {
        public Point Position = new Point();
        public float Radius;
    }
}