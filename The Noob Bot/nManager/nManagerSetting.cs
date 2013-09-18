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

        private static readonly Dictionary<ulong, int> _blackListGuidByTime = new Dictionary<ulong, int>();

        public static bool IsBlackListed(ulong guid)
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
                Logging.WriteError("IsBlackListed(ulong guid): " + e);
                return false;
            }
        }

        public static List<ulong> GetListGuidBlackListed()
        {
            try
            {
                List<ulong> ret = new List<ulong>();
                foreach (KeyValuePair<ulong, int> i in _blackListGuidByTime)
                {
                    if (i.Value == -1 || i.Value <= Others.Times)
                        ret.Add(i.Key);
                }
                return ret;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetListGuidBlackListed(): " + e);
                return new List<ulong>();
            }
        }

        public static void AddBlackList(ulong guid, int timeInMilisec = -1)
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
                Logging.WriteError("AddBlackList(ulong guid, int timeInMilisec = -1): " + e);
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

        public string CombatClass = "Tnb_CombatClass.dll";
        public string HealerClass = "Tnb_HealerClass.dll";
        public bool AutoAssignTalents; // To be fixed.
        public bool TrainNewSkills = false; // To be fixed.
        public bool CanPullUnitsAlreadyInFight;
        public bool DontPullMonsters;
        public bool UseSpiritHealer;
        public bool UseGroundMount = true;
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
        public bool ActivateChestLooting;
        public bool ActivateBeastSkinning;
        public bool BeastNinjaSkinning;
        public bool ActivateVeinsHarvesting = true;
        public bool ActivateHerbsHarvesting = true;
        public float DontHarvestIfPlayerNearRadius = 10;
        public int DontHarvestIfMoreThanXUnitInAggroRange = 3;
        public float GatheringSearchRadius = 100;
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
        public bool MakeStackOfElementalsItems;
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
    }
}