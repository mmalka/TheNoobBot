using System;
using System.IO;
using nManager.Helpful;

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

        public bool WarsongGulch;
        public bool ArathiBasin;
        public bool AlteracValley;
        public bool EyeoftheStorm;
        public bool StrandoftheAncients;
        public bool IsleofConquest;
        public bool BattleforGilneas;
        public bool TwinPeaks = true;
        public bool TempleofKotmogu;
        public bool SilvershardMines;
        public bool RequeueAfterTerminaison = true;
        public bool RandomBattleground;

        public string profileName = "";
        //internal Point BattlegrounderPosition = new Point();
        //internal float BattlegrounderRotation = 0;
    }
}