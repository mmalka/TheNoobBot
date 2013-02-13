using System;
using System.IO;
using nManager.Helpful;

namespace Battlegrounder.Bot
{
    [Serializable]
    public class BattlegrounderSetting : Settings
    {
        public bool AlteracValley;
        public string AlteracValleyProfileType = "XMLProfile";
        public string AlteracValleyXmlProfile;
        public bool ArathiBasin;
        public string ArathiBasinProfileType = "XMLProfile";
        public string ArathiBasinXmlProfile;
        public bool BattleforGilneas;
        public string BattleforGilneasProfileType = "XMLProfile";
        public string BattleforGilneasXmlProfile;
        public bool EyeoftheStorm;
        public string EyeoftheStormProfileType = "XMLProfile";
        public string EyeoftheStormXmlProfile;
        public bool IsleofConquest;
        public string IsleofConquestProfileType = "XMLProfile";
        public string IsleofConquestXmlProfile;
        public bool RandomBattleground;
        public bool RequeueAfterXMinutes;
        public int RequeueAfterXMinutesTimer = 5;
        public bool SilvershardMines;
        public string SilvershardMinesProfileType = "XMLProfile";
        public string SilvershardMinesXmlProfile;
        public bool StrandoftheAncients;
        public string StrandoftheAncientsProfileType = "XMLProfile";
        public string StrandoftheAncientsXmlProfile;
        public bool TempleofKotmogu;
        public string TempleofKotmoguProfileType = "XMLProfile";
        public string TempleofKotmoguXmlProfile;
        public bool TwinPeaks;
        public string TwinPeaksProfileType = "XMLProfile";
        public string TwinPeaksXmlProfile;
        public bool WarsongGulch = true;
        public string WarsongGulchProfileType = "CSharpProfile";
        public string WarsongGulchXmlProfile;
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
    }
}