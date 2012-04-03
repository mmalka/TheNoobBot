using System;
using System.IO;
using nManager.Helpful;

namespace Grinder.Bot
{
    [Serializable]
    public class GrinderSetting : Settings
    {
        public static GrinderSetting CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("Grinder"));
            }
            catch (Exception e)
            {
                Logging.WriteError("GrinderSetting > Save(): " + e);
                return false;
            }
        }
        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("Grinder")))
                {
                    CurrentSetting = Load<GrinderSetting>(AdviserFilePathAndName("Grinder"));
                    return true;
                }
                CurrentSetting = new GrinderSetting();
            }
            catch (Exception e)
            {
                Logging.WriteError("GrinderSetting > Load(): " + e);

            }
            return false;
        }

        public string profileName = "";
    }
}
