using System;
using System.IO;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Quester.Bot
{
    [Serializable]
    public class QuesterSetting : Settings
    {
        public static QuesterSetting CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("Quester"));
            }
            catch (Exception e)
            {
                Logging.WriteError("QuesterSetting > Save(): " + e);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("Quester")))
                {
                    CurrentSetting = Load<QuesterSetting>(AdviserFilePathAndName("Quester"));
                    return true;
                }
                CurrentSetting = new QuesterSetting();
            }
            catch (Exception e)
            {
                Logging.WriteError("QuesterSetting > Load(): " + e);
            }
            return false;
        }

        internal Point QuesterPosition = new Point();
        internal float QuesterRotation = 0;
        public string profileName = "";
    }
}