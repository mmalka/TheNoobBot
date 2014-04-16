using System;
using System.IO;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Mimesis.Bot
{
    [Serializable]
    public class MimesisSettings : Settings
    {
        public static MimesisSettings CurrentSetting { get; set; }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("Mimesis"));
            }
            catch (Exception e)
            {
                Logging.WriteError("MimesisSettings > Save(): " + e);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("Mimesis")))
                {
                    CurrentSetting = Load<MimesisSettings>(AdviserFilePathAndName("Mimesis"));
                    return true;
                }
                CurrentSetting = new MimesisSettings();
            }
            catch (Exception e)
            {
                Logging.WriteError("MimesisSettings > Load(): " + e);
            }
            return false;
        }

        public string MasterIPAddress = "127.0.0.1";
        public int MasterIPPort = 6543;
        public bool ActivatePartyMode = true;
    }
}