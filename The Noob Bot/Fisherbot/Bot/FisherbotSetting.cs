using System;
using System.IO;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Fisherbot.Bot
{
    [Serializable]
    public class FisherbotSetting : Settings
    {
        public static FisherbotSetting CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("Fisherbot"));
            }
            catch (Exception e)
            {
                Logging.WriteError("FisherbotSetting > Save(): " + e);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("Fisherbot")))
                {
                    CurrentSetting = Load<FisherbotSetting>(AdviserFilePathAndName("Fisherbot"));
                    return true;
                }
                CurrentSetting = new FisherbotSetting();
            }
            catch (Exception e)
            {
                Logging.WriteError("FisherbotSetting > Load(): " + e);
            }
            return false;
        }

        public bool useLure = true;
        public bool fishSchool;
        public string fishSchoolProfil = "";
        public string lureName = "";
        public string FishingPoleName = "";
        public string weaponName = "";
        public bool precisionMode = true;

        internal Point FisherbotPosition = new Point();
        internal float FisherbotRotation;
    }
}