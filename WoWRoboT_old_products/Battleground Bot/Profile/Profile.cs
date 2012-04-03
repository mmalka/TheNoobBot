using System;
using System.Collections.Generic;
using WowManager.MiscStructs;

namespace Battleground_Bot.Profile
{
    [Serializable]
    public class Profile
    {
        public List<SubProfile> SubProfiles = new List<SubProfile>();
    }

    [Serializable]
    public class SubProfile
    {
        public List<ulong> BlackListGuid = new List<ulong>();
        public bool Hotspots;
        public List<Point> Locations = new List<Point>();

        public string PlayerFaction = "All"; // "All" or "Alliance" or "Horde"

        public List<uint> TargetFaction = new List<uint>();
    }
}