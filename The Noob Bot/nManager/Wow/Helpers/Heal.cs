using System;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class Heal
    {
        public static bool IsHealing { get; set; }

        public static void StartHeal()
        {
            try
            {
                Logging.Write("Start healing process.");
                IsHealing = true;
                if (MovementManager.InMovement)
                {
                    MovementManager.StopMove();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartHealBot(): " + exception);
                IsHealing = true;
            }
        }

        public static void StartHealBot()
        {
            try
            {
                Logging.Write("Start healing process.");
                IsHealing = true;
                if (MovementManager.InMovement)
                {
                    MovementManager.StopMove();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartHealBot(): " + exception);
                IsHealing = true;
            }
        }

        public static void StopHeal()
        {
            try
            {
                Logging.Write("Stop healing process.");
                IsHealing = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StopHeal(): " + exception);
                IsHealing = false;
            }
        }
    }
}