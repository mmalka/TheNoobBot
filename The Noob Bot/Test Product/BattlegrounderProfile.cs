using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace Test_Product
{
    [Serializable]
    public class BattlegrounderProfile
    {
        public List<BattlegrounderZone> BattlegrounderZones = new List<BattlegrounderZone>();

        internal bool IsValid()
        {
            try
            {
                return BattlegrounderZones.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }

    [Serializable]
    public class BattlegrounderZone
    {
        public string BattlegroundId;
        public List<BattlegrounderBlackListRadius> BlackListRadius = new List<BattlegrounderBlackListRadius>();
        public bool Hotspots;
        public string Name = "";
        public List<Npc> Npc = new List<Npc>();
        public List<Point> Points = new List<Point>();

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