using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace Grinder.Profile
{
    [Serializable]
    public class GrinderProfile
    {
        public List<GrinderZone> GrinderZones = new List<GrinderZone>();
        public List<Point> PointsPathFinderDroidz = new List<Point>();
    }
    [Serializable]
    public class GrinderZone
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
        public List<GrinderBlackListRadius> BlackListRadius = new List<GrinderBlackListRadius>();

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
    public class GrinderBlackListRadius
    {
        public Point Position = new Point();
        public float Radius;
    }
}
