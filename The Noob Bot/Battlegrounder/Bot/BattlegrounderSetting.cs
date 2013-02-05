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

        public bool AlteracValley;
        public string AlteracValleyProfileType;
        public string XMLProfileListAlteracValley;
        public bool WarsongGulch;
        public string WarsongGulchProfileType;
        public string XMLProfileListWarsongGulch;
        public bool ArathiBasin;
        public string ArathiBasinProfileType;
        public string XMLProfileListArathiBasin;
        public bool EyeoftheStorm;
        public string EyeoftheStormProfileType;
        public string XMLProfileListEyeoftheStorm;
        public bool StrandoftheAncients;
        public string StrandoftheAncientsProfileType;
        public string XMLProfileListStrandoftheAncients;
        public bool IsleofConquest;
        public string IsleofConquestProfileType;
        public string XMLProfileListIsleofConquest;
        public bool BattleforGilneas;
        public string BattleforGilneasProfileType;
        public string XMLProfileListBattleforGilneas;
        public bool TwinPeaks = true;
        public string TwinPeaksProfileType;
        public string XMLProfileListTwinPeaks;
        public bool TempleofKotmogu;
        public string TempleofKotmoguProfileType;
        public string XMLProfileListTempleofKotmogu;
        public bool SilvershardMines;
        public string SilvershardMinesProfileType;
        public string XMLProfileListSilvershardMines;
        public bool RequeueAfterXMinutes;
        public int RequeueAfterXMinutesTimer = 5;
        public bool RandomBattleground;

        public string profileName = "";
        //internal Point BattlegrounderPosition = new Point();
        //internal float BattlegrounderRotation = 0;
    }
}