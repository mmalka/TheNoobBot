using System;
using System.IO;
using nManager;
using nManager.Helpful;

namespace Heal_Bot.Bot
{
    [Serializable]
    public class HealBotSettings : Settings
    {
        public static HealBotSettings CurrentSetting { get; set; }

        public bool ActivateAutoFacing
        {
            get { return nManagerSetting.CurrentSetting.ActivateAutoFacingDamageDealer; }
            set
            {
                nManagerSetting.CurrentSetting.ActivateAutoFacingDamageDealer = value;
                nManagerSetting.CurrentSetting.Save();
            }
        }

        public bool ActivateMovements
        {
            get { return nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer; }
            set
            {
                nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer = value;
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
                    CurrentSetting = Load<HealBotSettings>(AdviserFilePathAndName("DamageDealer"));
                    return true;
                }
                CurrentSetting = new HealBotSettings();
            }
            catch (Exception e)
            {
                Logging.WriteError("Damage Dealer > Load(): " + e);
            }
            return false;
        }
    }
}