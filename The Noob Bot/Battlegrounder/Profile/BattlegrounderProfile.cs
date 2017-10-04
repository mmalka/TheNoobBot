using System;
using System.Collections.Generic;
using nManager;
using nManager.Wow.Class;

namespace Battlegrounder.Profile
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
        public List<Point> Points = new List<Point>();
        //public List<Npc> Npc = new List<Npc>();

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
    public class BattlegrounderBlackListRadius : nManagerSetting.BlackListZone
    {
    }
}