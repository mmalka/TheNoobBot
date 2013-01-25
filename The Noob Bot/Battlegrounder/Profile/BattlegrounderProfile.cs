using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace Battlegrounder.Profile
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
        public bool Hotspots;
        public uint MinLevel = 0;
        public uint MaxLevel = 90;
        public uint MinTargetLevel = 0;
        public uint MaxTargetLevel = 90;
        public List<int> TargetEntry = new List<int>();
        public List<uint> TargetFactions = new List<uint>();
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
