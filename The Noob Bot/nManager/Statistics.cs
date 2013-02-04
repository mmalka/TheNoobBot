using System;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager
{
    public static class Statistics
    {
        public static void Reset()
        {
            _loots = 0;
            _kills = 0;
            _deaths = 0;
            _farms = 0;
            _stucks = 0;
            _offSetStats = 0x57;

            _startTime = Others.TimesSec;
            _startXp = ObjectManager.Me.Experience;
            _startHonor = Usefuls.GetHonorPoint;
        }

        public static int ExperienceByHr()
        {
            try
            {
                if (ObjectManager.Me.Experience <= _startXp)
                    return 0;
                if (Others.TimesSec <= _startTime)
                    return 0;

                UInt64 t = (UInt64) (ObjectManager.Me.Experience - _startXp)*(60*60);
                int r = (int) (t/(UInt64) (Others.TimesSec - _startTime));

                return r;
            }
            catch (Exception e)
            {
                Logging.WriteError("ExperienceByHr(): " + e);
                return 0;
            }
        }

        public static int RunningTimeInSec()
        {
            try
            {
                if (Others.TimesSec <= _startTime)
                    return 0;

                return Others.TimesSec - _startTime;
            }
            catch (Exception e)
            {
                Logging.WriteError("RunningTime(): " + e);
                return 0;
            }
        }

        public static int HonorByHr()
        {
            try
            {
                if (Usefuls.GetHonorPoint <= _startHonor)
                    return 0;
                if (Others.TimesSec <= _startTime)
                    return 0;

                return (Usefuls.GetHonorPoint - _startHonor)*(60*60)/(Others.TimesSec - _startTime);
            }
            catch (Exception e)
            {
                Logging.WriteError("HonorByHr(): " + e);
                return 0;
            }
        }

        public static int LootsByHr()
        {
            try
            {
                if (_loots <= 0)
                    return 0;
                if (Others.TimesSec <= _startTime)
                    return 0;

                return (int) _loots*(60*60)/(Others.TimesSec - _startTime);
            }
            catch (Exception e)
            {
                Logging.WriteError("LootsByHr(): " + e);
                return 0;
            }
        }

        public static int KillsByHr()
        {
            try
            {
                if (_kills <= 0)
                    return 0;
                if (Others.TimesSec <= _startTime)
                    return 0;

                return (int) _kills*(60*60)/(Others.TimesSec - _startTime);
            }
            catch (Exception e)
            {
                Logging.WriteError("KillsByHr(): " + e);
                return 0;
            }
        }

        public static int FarmsByHr()
        {
            try
            {
                if (_farms <= 0)
                    return 0;
                if (Others.TimesSec <= _startTime)
                    return 0;

                return (int) _farms*(60*60)/(Others.TimesSec - _startTime);
            }
            catch (Exception e)
            {
                Logging.WriteError("FarmsByHr(): " + e);
                return 0;
            }
        }

        public static int DeathsByHr()
        {
            try
            {
                if (_deaths <= 0)
                    return 0;
                if (Others.TimesSec <= _startTime)
                    return 0;

                return (int) _deaths*(60*60)/(Others.TimesSec - _startTime);
            }
            catch (Exception e)
            {
                Logging.WriteError("DeathsByHr(): " + e);
                return 0;
            }
        }

        private static int _startTime;
        private static int _startXp;
        private static int _startHonor;

        private static uint _loots;

        public static uint Loots
        {
            get { return _loots; }
            set { _loots = value; }
        }

        private static uint _kills;

        public static uint Kills
        {
            get { return _kills; }
            set { _kills = value; }
        }

        private static int _offSetStats;

        public static int OffsetStats
        {
            get { return _offSetStats; }
            set { _offSetStats = value; }
        }

        private static uint _deaths;

        public static uint Deaths
        {
            get { return _deaths; }
            set { _deaths = value; }
        }

        private static uint _farms;

        public static uint Farms
        {
            get { return _farms; }
            set { _farms = value; }
        }

        private static uint _stucks;

        public static uint Stucks
        {
            get { return _stucks; }
            set { _stucks = value; }
        }
    }
}