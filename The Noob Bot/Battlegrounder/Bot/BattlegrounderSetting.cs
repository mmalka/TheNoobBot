using System;
using System.IO;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Battlegrounder.Bot
{
    [Serializable]
    public class BattlegrounderSetting : Settings
    {
        public static BattlegrounderSetting CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("Battlegrounder"));
            }
            catch (Exception e)
            {
                Logging.WriteError("BattlegrounderSetting > Save(): " + e);
                return false;
            }
        }
        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("Battlegrounder")))
                {
                    CurrentSetting = Load<BattlegrounderSetting>(AdviserFilePathAndName("Battlegrounder"));
                    return true;
                }
                CurrentSetting = new BattlegrounderSetting();
            }
            catch (Exception e)
            {
                Logging.WriteError("BattlegrounderSetting > Load(): " + e);

            }
            return false;
        }

        public bool WarsongGulch = true;
        public bool ArathiBasin = true;
        public bool AlteracValley = true;
        public bool EyeoftheStorm = true;
        public bool StrandoftheAncients = true;
        public bool IsleofConquest = true;
        public bool BattleforGilneas = true;
        public bool TwinPeaks = true;
        public bool RandomBattleground = false;

        internal Point BattlegrounderPosition = new Point();
        internal float BattlegrounderRotation;
    }
}
