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

        public static ulong StartHeal()
        {
            try
            {
                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    _healLoop = true;
                    if (MovementManager.InMovement)
                    {
                        MovementManager.StopMove();
                    }
                    Thread.Sleep(500);

                    while (!ObjectManager.ObjectManager.Me.IsDeadMe && _healLoop &&
                           !ObjectManager.ObjectManager.Me.InTransport)
                    {
                        Thread.Sleep(500);
                    }
                }
                _healLoop = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartHealBot(): " + exception);
                _healLoop = false;
            }
        }

        public static void StartHealBot()
        {
            try
            {
                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    _healLoop = true;
                    if (MovementManager.InMovement)
                    {
                        MovementManager.StopMove();
                    }
                    Thread.Sleep(500);

                    while (!ObjectManager.ObjectManager.Me.IsDeadMe && _healLoop &&
                           !ObjectManager.ObjectManager.Me.InTransport)
                    {
                        Thread.Sleep(500);
                    }
                }
                _healLoop = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartHealBot(): " + exception);
                _healLoop = false;
            }
        }

        public static void StopHeal()
        {
            try
            {
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