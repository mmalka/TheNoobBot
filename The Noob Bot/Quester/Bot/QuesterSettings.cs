using System;
using System.IO;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Quester.Bot
{
    [Serializable]
    public class QuesterSettings : Settings
    {
        public static QuesterSettings CurrentSettings { get; set; }

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
                    CurrentSettings = Load<QuesterSettings>(AdviserFilePathAndName("Quester"));
                    return true;
                }
                CurrentSettings = new QuesterSettings();
            }
            catch (Exception e)
            {
                Logging.WriteError("QuesterSetting > Load(): " + e);
            }
            return false;
        }

        internal Point QuesterPosition = new Point();
        internal float QuesterRotation = 0;
        public string LastProfile = "";
        public bool LastProfileSimple = true;
    }
}