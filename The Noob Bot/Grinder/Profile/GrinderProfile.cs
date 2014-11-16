using System;
using System.Collections.Generic;
using System.ComponentModel;
using nManager.Wow.Class;

namespace Grinder.Profile
{
    [Serializable]
    public class GrinderProfile
    {
        public List<GrinderZone> GrinderZones = new List<GrinderZone>();
    }

    [Serializable]
    public class GrinderZone
    {
        public List<GrinderBlackListRadius> BlackListRadius = new List<GrinderBlackListRadius>();
        public bool Hotspots;
        [DefaultValue(100)] public uint MaxLevel = 100;
        [DefaultValue(103)] public uint MaxTargetLevel = 103;
        [DefaultValue(1)] public uint MinLevel = 1;
        [DefaultValue(1)] public uint MinTargetLevel = 1;
        public string Name = "";
        public List<Npc> Npc = new List<Npc>();
        public List<Point> Points = new List<Point>();
        public List<int> TargetEntry = new List<int>();
        public List<uint> TargetFactions = new List<uint>();

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