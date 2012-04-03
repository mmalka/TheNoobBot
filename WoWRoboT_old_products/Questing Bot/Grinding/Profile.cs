using System;
using System.Collections.Generic;
using System.Linq;
using WowManager.MiscStructs;

namespace Questing_Bot.Grinding
{
    [Serializable]
    public class Profile
    {
        public string Name = "";

        public List<SubProfile> SubProfiles = new List<SubProfile>();
    }

    [Serializable]
    public class SubProfile
    {
        public List<BlackListZone> BlackListZones = new List<BlackListZone>();
        public List<uint> Factions = new List<uint>();

        public List<Point> GhostLocations = new List<Point>();
        public bool Hotspots;
        public List<Point> Locations = new List<Point>();
        public List<Point> Mailboxes = new List<Point>();
        public int MaxLevel = 85;
        public int MinLevel = 1;
        public string Name = "";
        public int TargetMaxLevel = 85;
        public int TargetMinLevel = 1;
        public List<Point> ToSubProfileLocations = new List<Point>();

        public List<Trainers> Trainers = new List<Trainers>();
        public List<Point> Vendors = new List<Point>();
        public bool random;

        public SubProfile()
        {
        }

        public SubProfile(string name)
        {
            Name = name;
        }

        internal List<Point> TrainersByClass(string classWow)
        {
            return (from t in Trainers where t.WowClass == classWow select t.Point).ToList();
        }
    }

    [Serializable]
    public class Trainers
    {
        public Trainers(WowManager.MiscStructs.Point trainer, string wowClass)
        {
            Point = trainer;
            WowClass = wowClass;
        }
        public Trainers()
        { }
        public WowManager.MiscStructs.Point Point = new WowManager.MiscStructs.Point();
        public string WowClass = "";
    }

    [Serializable]
    public class Buy
    {
        public string Name = "";
        public int Quantity;
    }

    [Serializable]
    public class BlackListZone
    {
        public Point Point = new Point(0, 0, 0);
        public float Radius = 30;
    }
}
