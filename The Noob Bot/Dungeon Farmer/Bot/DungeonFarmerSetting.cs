using System;
using System.IO;
using nManager.Helpful;

namespace DungeonFarmer.Bot
{
    [Serializable]
    public class DungeonFarmerSetting : Settings
    {
        public static DungeonFarmerSetting CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("DungeonFarmer"));
            }
            catch (Exception e)
            {
                Logging.WriteError("DungeonFarmerSetting > Save(): " + e);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("DungeonFarmer")))
                {
                    CurrentSetting = Load<DungeonFarmerSetting>(AdviserFilePathAndName("DungeonFarmer"));
                    return true;
                }
                CurrentSetting = new DungeonFarmerSetting();
            }
            catch (Exception e)
            {
                Logging.WriteError("DungeonFarmerSetting > Load(): " + e);
            }
            return false;
        }
    }
}