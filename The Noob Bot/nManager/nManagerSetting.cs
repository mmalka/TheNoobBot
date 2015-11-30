using System;
using System.Collections.Generic;
using System.IO;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager
{
    [Serializable]
// ReSharper disable InconsistentNaming
    public class nManagerSetting : Settings
// ReSharper restore InconsistentNaming
    {
        private static nManagerSetting _currentSetting;
        private static string _lastName = "";
        public static bool ActivateProductTipOff = true;
        public static bool AutoStartProduct = false;
        public static string AutoStartProductName = "";
        public static string AutoStartProfileName = "";

        public static nManagerSetting CurrentSetting
        {
            get
            {
                if (_currentSetting == null || ObjectManager.Me.Name != _lastName && Usefuls.InGame)
                {
                    Load();
                    _lastName = ObjectManager.Me.Name;
                }
                return _currentSetting;
            }
            set { _currentSetting = value; }
        }

        #region BlackListGuid

        private static readonly Dictionary<UInt128, int> _blackListGuidByTime = new Dictionary<UInt128, int>();

        public static bool IsBlackListed(UInt128 guid)
        {
            try
            {
                if (_blackListGuidByTime.ContainsKey(guid))
                {
                    return ((_blackListGuidByTime[guid] >= Others.Times) || _blackListGuidByTime[guid] == -1);
                }
                return false;
            }
            catch (Exception e)
            {
                Logging.WriteError("IsBlackListed(UInt128 guid): " + e);
                return false;
            }
        }

        public static List<UInt128> GetListGuidBlackListed()
        {
            try
            {
                List<UInt128> ret = new List<UInt128>();
                foreach (KeyValuePair<UInt128, int> i in _blackListGuidByTime)
                {
                    if (i.Value == -1 || i.Value <= Others.Times)
                        ret.Add(i.Key);
                }
                return ret;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetListGuidBlackListed(): " + e);
                return new List<UInt128>();
            }
        }

        public static void AddBlackList(UInt128 guid, int timeInMilisec = -1)
        {
            try
            {
                if (_blackListGuidByTime.ContainsKey(guid))
                    _blackListGuidByTime.Remove(guid);

                if (timeInMilisec >= 0)
                    timeInMilisec = timeInMilisec + Others.Times;

                _blackListGuidByTime.Add(guid, timeInMilisec);
            }
            catch (Exception e)
            {
                Logging.WriteError("AddBlackList(UInt128 guid, int timeInMilisec = -1): " + e);
            }
        }

        #endregion

        #region BlackListZone

        private static readonly Dictionary<Point, float> _blackListZone = new Dictionary<Point, float>();

        public static bool IsBlackListedZone(Point position)
        {
            try
            {
                foreach (KeyValuePair<Point, float> f in _blackListZone)
                {
                    if (f.Key.DistanceTo(position) <= f.Value)
                        return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Logging.WriteError("IsBlackListedZone(Point position): " + e);
                return false;
            }
        }

        public static Dictionary<Point, float> GetListZoneBlackListed()
        {
            try
            {
                return _blackListZone;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetListZoneBlackListed(): " + e);
                return new Dictionary<Point, float>();
            }
        }

        public static void AddBlackListZone(Point position, float radius)
        {
            try
            {
                if (!_blackListZone.ContainsKey(position))
                    _blackListZone.Add(position, radius);
            }
            catch (Exception e)
            {
                Logging.WriteError("AddBlackListZone(Point position, float radius): " + e);
            }
        }

        public static void AddRangeBlackListZone(Dictionary<Point, float> listBlackZone)
        {
            try
            {
                foreach (KeyValuePair<Point, float> f in listBlackZone)
                {
                    if (!_blackListZone.ContainsKey(f.Key))
                        _blackListZone.Add(f.Key, f.Value);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("AddRangeBlackListZone(Dictionary<Point, float> listBlackZone): " + e);
            }
        }

        #endregion

        // ----------------------

        public bool Save()
        {
            try
            {
                return Save(AdviserFilePathAndName("General"));
            }
            catch (Exception e)
            {
                Logging.WriteError("nManagerSetting > Save(): " + e);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(AdviserFilePathAndName("General")))
                {
                    CurrentSetting = Load<nManagerSetting>(AdviserFilePathAndName("General"));
                    return true;
                }
                CurrentSetting = new nManagerSetting();
            }
            catch (Exception e)
            {
                Logging.WriteError("nManagerSetting > Load(): " + e);
            }
            return false;
        }

        // Special DamageDealer:
        public bool ActivateAutoFacingDamageDealer = false;
        public bool ActivateMovementsDamageDealer = false;
        // Special HealerBot:
        public bool ActivateAutoFacingHealerBot = false;
        public bool ActivateMovementsHealerBot = false;

        // Global Settings:
        public string LastProductLoaded;
        public string CombatClass = "Tnb_CombatClass.dll";
        public string HealerClass = "Tnb_HealerClass.dll";
        public bool AutoAssignTalents; // To be fixed.
        public bool ActivateSkillsAutoTraining = true;
        public bool OnlyTrainCurrentlyUsedSkills = true;
        public bool TrainMountingCapacity = true;
        public bool OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum = true;
        public bool BecomeApprenticeIfNeededByProduct = false;
        public bool BecomeApprenticeOfSecondarySkillsWhileQuesting = false;
        public bool CanPullUnitsAlreadyInFight;
        public bool DontPullMonsters;
        public bool UseSpiritHealer;
        public bool UseGroundMount = false;
        public string GroundMountName = "";
        public uint MinimumDistanceToUseMount = 15;
        public bool IgnoreFightIfMounted = true;
        public string FlyingMountName = "";
        public string AquaticMountName = "";
        public string FoodName = "";
        public int EatFoodWhenHealthIsUnderXPercent = 35;
        public string BeverageName = "";
        public int DrinkBeverageWhenManaIsUnderXPercent = 35;
        public bool DoRegenManaIfLow;
        public bool ActivateMonsterLooting = true;
        public bool ActivateLootStatistics = true;
        public bool ActivateChestLooting;
        public bool ActivateBeastSkinning;
        public bool BeastNinjaSkinning;
        public bool ActivateVeinsHarvesting = true;
        public bool ActivateHerbsHarvesting = true;
        public float DontHarvestIfPlayerNearRadius = 10;
        public int DontHarvestIfMoreThanXUnitInAggroRange = 3;
        public float GatheringSearchRadius = 80;
        public bool HarvestDuringLongDistanceMovements;
        public bool ActivateAutoSmelting;
        public bool ActivateAutoProspecting;
        public bool OnlyUseProspectingInTown;
        public int TimeBetweenEachProspectingAttempt = 15;
        public List<string> MineralsToProspect = new List<string>();
        public bool ActivateAutoMilling;
        public bool OnlyUseMillingInTown;
        public int TimeBetweenEachMillingAttempt = 15;
        public List<string> HerbsToBeMilled = new List<string>();
        public List<string> DontHarvestTheFollowingObjects = new List<string>();
        public bool MakeStackOfElementalsItems = true;
        public bool ActivateReloggerFeature;
        public string EmailOfTheBattleNetAccount = "";
        public string PasswordOfTheBattleNetAccount = "";
        public string BattleNetSubAccount = "";
        public int NumberOfFoodsWeGot; // TODO Count the items instead (!?)
        public int NumberOfBeverageWeGot; // TODO Count the items instead (!?)
        public bool ActivateAutoRepairFeature = true;
        public bool ActivateAutoSellingFeature;
        public bool SellGray = true;
        public bool SellWhite;
        public bool SellGreen;
        public bool SellBlue;
        public bool SellPurple;
        public List<string> DontSellTheseItems = new List<string>();
        public List<string> ForceToSellTheseItems = new List<string>();
        public bool ActivateAutoMaillingFeature;
        public string MaillingFeatureRecipient = "";
        public string MaillingFeatureSubject = "Hey";
        public bool MailGray;
        public bool MailWhite = true;
        public bool MailGreen = true;
        public bool MailBlue = true;
        public bool MailPurple = true;
        public List<string> DontMailTheseItems = new List<string>();
        public List<string> ForceToMailTheseItems = new List<string>();
        public bool StopTNBIfBagAreFull;
        public bool StopTNBIfHonorPointsLimitReached;
        public bool StopTNBIfPlayerHaveBeenTeleported;
        public int StopTNBAfterXLevelup = 90;
        public int StopTNBIfReceivedAtMostXWhispers = 10;
        public int StopTNBAfterXStucks = 80;
        public int StopTNBAfterXMinutes = 1440;
        public bool PauseTNBIfNearByPlayer;
        public bool RecordWhispsInLogFiles = true;
        public bool PlayASongIfNewWhispReceived;
        public bool ActivatePathFindingFeature = true;
        public bool AllowTNBToSetYourMaxFps = true;
        public float MaxDistanceToGoToMailboxesOrNPCs = 1000;
        public bool AutoConfirmOnBoPItems = true;
        public bool ActivateAlwaysOnTopFeature;
        public int RepairWhenDurabilityIsUnderPercent = 35;
        public int SendMailWhenLessThanXSlotLeft = 2;
        public int SellItemsWhenLessThanXSlotLeft = 2;
        public bool UseHearthstone;
        public bool ActiveStopTNBAfterXLevelup;
        public bool ActiveStopTNBAfterXMinutes;
        public bool ActiveStopTNBAfterXStucks;
        public bool ActiveStopTNBIfReceivedAtMostXWhispers;
        public bool UseMollE;
        public bool UseRobot;
        public bool AutoCloseChatFrame = true;
        public bool ActivateBroadcastingMimesis = false;
        public int BroadcastingPort = 6543;
        public List<string> ActivatedPluginsList = new List<string>();
        public bool ActivatePluginsSystem = true;
        public bool LaunchExpiredPlugins = false;
        public bool HideSdkFiles = true;
        public bool UseFrameLock = false;
        public bool UseLootARange = true;
    }
}