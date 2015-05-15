using System;
using System.IO;
using nManager;
using nManager.Helpful;

namespace Damage_Dealer.Bot
{
    [Serializable]
    public class DamageDealerSettings : Settings
    {
        public bool ActivateMovements = false;
        public static DamageDealerSettings CurrentSetting { get; set; }

        public bool ActivateAutoFacing
        {
            get { return nManagerSetting.CurrentSetting.ActivateAutoFacingDamageDealer; }
            set
            {
                nManagerSetting.CurrentSetting.ActivateAutoFacingDamageDealer = value;
                nManagerSetting.CurrentSetting.Save();
            }
        }

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("DamageDealer"));
            }
            catch (Exception e)
            {
                Logging.WriteError("DamageDealerSettings > Save(): " + e);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("DamageDealer")))
                {
                    CurrentSetting = Load<DamageDealerSettings>(AdviserFilePathAndName("DamageDealer"));
                    return true;
                }
                CurrentSetting = new DamageDealerSettings();
            }
            catch (Exception e)
            {
                Logging.WriteError("Damage Dealer > Load(): " + e);
            }
            return false;
        }
    }
}