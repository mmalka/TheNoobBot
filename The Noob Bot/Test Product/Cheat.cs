using System;
using System.Linq;
using nManager.Helpful;
using nManager.Wow;

namespace Test_Product
{
    public class Cheat
    {
        #region AntiAfk

        private static uint afk = 0x4C3C1B;

        private static readonly byte[] AfkOrigine = new byte[] {0x78, 0x42};
        private static readonly byte[] AfkAfter = new byte[] {0xEB, 0x40};

        public static void AntiAfkPulse()
        {
            try
            {
                Memory.WowMemory.Memory.WriteBytes(Memory.WowProcess.WowModule + afk, AfkAfter);
            }
            catch (Exception exception)
            {
                Logging.WriteError("AntiAfkPulse(): " + exception);
            }
        }

        public static void AntiAfkDispose()
        {
            try
            {
                Memory.WowMemory.Memory.WriteBytes(Memory.WowProcess.WowModule + afk, AfkOrigine);
            }
            catch (Exception exception)
            {
                Logging.WriteError("AntiAfkDispose(): " + exception);
            }
        }

        public static bool AntiAfkIsActive()
        {
            try
            {
                if (
                    Memory.WowMemory.Memory.ReadBytes(Memory.WowProcess.WowModule + afk,
                                                      AfkOrigine.Count())[0] == AfkOrigine[0])
                    return false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("AntiAfkIsActive(): " + exception);
            }
            return true;
        }

        #endregion AntiAfk
    }
}