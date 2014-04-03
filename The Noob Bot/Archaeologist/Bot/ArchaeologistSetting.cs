using System;
using System.IO;
using nManager.Helpful;

namespace Archaeologist.Bot
{
    [Serializable]
    public class ArchaeologistSetting : Settings
    {
        public static ArchaeologistSetting CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("Archaeologist"));
            }
            catch (Exception e)
            {
                Logging.WriteError("ArchaeologistSetting > Save(): " + e);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("Archaeologist")))
                {
                    CurrentSetting = Load<ArchaeologistSetting>(AdviserFilePathAndName("Archaeologist"));
                    return true;
                }
                CurrentSetting = new ArchaeologistSetting();
            }
            catch (Exception e)
            {
                Logging.WriteError("ArchaeologistSetting > Load(): " + e);
            }
            return false;
        }

        public int SolvingEveryXMin = 20;
        public int MaxTryByDigsite = 50;
        public bool UseKeystones = true;
    }
}