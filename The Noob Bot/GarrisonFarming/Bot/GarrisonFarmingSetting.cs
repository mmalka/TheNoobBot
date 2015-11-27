using System;
using System.IO;
using nManager;
using nManager.Helpful;

namespace GarrisonFarming.Bot
{
    [Serializable]
    public class GarrisonFarmingSetting : Settings
    {
        public bool PathingReverseDirection;
        public string ProfileName = "";

        private GarrisonFarmingSetting()
        {
            ConfigWinForm("GarrisonFarming " + Translate.Get(Translate.Id.Settings));
            AddControlInWinForm(Translate.Get(Translate.Id.Pathing_Reverse_Direction), "PathingReverseDirection", "Path Settings");
        }

        public static GarrisonFarmingSetting CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("GarrisonFarming"));
            }
            catch (Exception e)
            {
                Logging.WriteError("GarrisonFarmingSetting > Save(): " + e);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("GarrisonFarming")))
                {
                    CurrentSetting = Load<GarrisonFarmingSetting>(AdviserFilePathAndName("GarrisonFarming"));
                    return true;
                }
                CurrentSetting = new GarrisonFarmingSetting();
            }
            catch (Exception e)
            {
                Logging.WriteError("GarrisonFarmingSetting > Load(): " + e);
            }
            return false;
        }
    }
}