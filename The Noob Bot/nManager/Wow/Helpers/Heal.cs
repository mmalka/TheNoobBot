using System;
using System.Threading;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class Heal
    {
        private static bool _healLoop;

        public static bool IsHealing
        {
            get { return _healLoop; }
            set { _healLoop = value; }
        }

        public static void StartHeal()
        {
            try
            {
                Logging.Write("Start healing process.");
                _healLoop = true;
                if (MovementManager.InMovement)
                {
                    MovementManager.StopMove();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartHealBot(): " + exception);
                _healLoop = true;
            }
        }

        public static void StartHealBot()
        {
            try
            {
                Logging.Write("Start healing process.");
                _healLoop = true;
                if (MovementManager.InMovement)
                {
                    MovementManager.StopMove();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartHealBot(): " + exception);
                _healLoop = true;
            }
        }

        public static void StopHeal()
        {
            try
            {
                Logging.Write("Stop healing process.");
                _healLoop = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StopHeal(): " + exception);
                _healLoop = false;
            }
        }
    }
}