using System;
using System.Drawing;
using System.IO;
using nManager;
using nManager.Helpful;

namespace Gatherer.Bot
{
    [Serializable]
    public class GathererSetting : Settings
    {
        GathererSetting()
        {
            ConfigWinForm(new Point(400, 100), "Gatherer " + Translate.Get(Translate.Id.Settings));
            AddControlInWinForm(Translate.Get(Translate.Id.Pathing_Reverse_Direction), "pathingReverseDirection");
        }

        public static GathererSetting CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("Gatherer"));
            }
            catch (Exception e)
            {
                Logging.WriteError("GathererSetting > Save(): " + e);
                return false;
            }
        }
        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("Gatherer")))
                {
                    CurrentSetting = Load<GathererSetting>(AdviserFilePathAndName("Gatherer"));
                    return true;
                }
                CurrentSetting = new GathererSetting();
            }
            catch (Exception e)
            {
                Logging.WriteError("GathererSetting > Load(): " + e);

            }
            return false;
        }

        public string profileName = "";

        public bool pathingReverseDirection;
    }
}
