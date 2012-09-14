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
    public class nManagerSetting : Settings
    {
        private static nManagerSetting _currentSetting;
        static string _lastName = "";
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
        static Dictionary<ulong, int> _blackListGuidByTime = new Dictionary<ulong, int>();

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
                var ret = new List<ulong>();
                foreach (var i in _blackListGuidByTime)
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
        static Dictionary<Point, float> _blackListZone = new Dictionary<Point, float>();

        public static bool IsBlackListedZone(Point position)
        {
            try
            {
                foreach (var f in _blackListZone)
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
                foreach (var f in listBlackZone)
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

        public string customClass = "TheNoobBot.cs";
        public bool assignTalents;
        public bool trainNewSkills = true;
        public bool trainNewSpells = true;
        public bool canAttackUnitsAlreadyInFight;
        public bool dontStartFighting;
        public bool useSpiritHealer;
        public bool useGroundMount = true;
        public string GroundMountName = "";
        public float mountDistance = 80;
        public bool ignoreFightGoundMount = true;
        public string FlyingMountName = "";
        public string AquaticMountName = "";
        public string foodName = "";
        public int foodPercent = 35;
        public string drinkName = "";
        public int drinkPercent = 35;
        public bool restingMana;
        public bool lootMobs = true;
        public bool lootChests;
        public bool skinMobs;
        public bool skinNinja;
        public bool harvestMinerals = true;
        public bool harvestHerbs = true;
        public float harvestAvoidPlayersRadius = 10;
        public int maxUnitsNear = 3;
        public float searchRadius = 80;
        public bool harvestDuringLongMove;
        public bool smelting;
        public bool prospecting;
        public bool prospectingInTown;
        public int prospectingTime = 15;
        public List<string> prospectingList = new List<string>();
        public bool milling;
        public bool millingInTown;
        public int millingTime = 15;
        public List<string> millingList = new List<string>();
        public List<int> blackListHarvest = new List<int>();
        public bool autoMakeElemental;
        public bool reloger;
        public string accountEmail = "";
        public string accountPassword = "";
        public string bNetName = "";
        //public string characterName = "";
        //public string realmServer = "";
        public int foodAmount;
        public int drinkAmount;
        public bool repair = true;
        public bool selling;
        public bool sellGray = true;
        public bool sellWhite;
        public bool sellGreen;
        public bool sellBlue;
        public bool sellPurple;
        public List<string> doNotSellList = new List<string>();
        public List<string> forceSellList = new List<string>();
        public bool useMail;
        public string mailRecipient = "";
        public string mailSubject = "Hey";
        public bool mailGray;
        public bool mailWhite = true;
        public bool mailGreen = true;
        public bool mailBlue = true;
        public bool mailPurple = true;
        public List<string> doNotMailList = new List<string>();
        public List<string> forceMailList = new List<string>();
        public bool closeIfFullBag;
        public bool closeIfReached4000HonorPoints;
        public bool closeIfPlayerTeleported = true;
        public int closeAfterXLevel = 90;
        public int closeIfWhisperBiggerOrEgalAt = 10;
        public int closeAfterXBlockages = 80;
        public int closeAfterXMin = 1440;
        public bool securityPauseBotIfNerbyPlayer;
        public bool securityRecordWhisperInLogFile = true;
        public bool securitySongIfNewWhisper = false;
        public bool usePathsFinder = true;
        public bool MaxFPSSwitch = true;
        public float npcMailboxSearchRadius = 600;
    }
}
