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

        public bool UseLure = true;
        public bool FishSchool;
        public string FishSchoolProfil = "";
        public string LureName = "";
        public string FishingPoleName = "";
        public string WeaponName = "";
        public string ShieldName = "";
        public bool PrecisionMode = true;

        internal Point FisherbotPosition = new Point();
        internal float FisherbotRotation;
    }
}