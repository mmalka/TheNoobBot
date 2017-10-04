using System;
using System.Collections.Generic;
using nManager;
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
    public class FisherbotBlackListRadius : nManagerSetting.BlackListZone
    {
    }
}