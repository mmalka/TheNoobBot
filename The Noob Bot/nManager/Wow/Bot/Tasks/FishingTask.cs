using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.Tasks
{
    public class FishingTask
    {
        private static bool _fishBotLaunched;
        private static Thread _worker2;

        private static Spell fishingSpell;

        private static ulong _guidNode;
        private static bool _precision;
        private static string _lureName = "";
        private static bool _useLure;

        private const float distanceBobber = 4.0f;

        public static bool IsLaunched
        {
            get { return _fishBotLaunched; }
        }

        /// <summary>
        /// Stop fish bot.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns name="processHandle"></returns>
        public static void StopLoopFish()
        {
            try
            {
                lock (typeof (FishingTask))
                {
                    _guidNode = 0;
                    _fishBotLaunched = false;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("FishingTask > StopLoopFish(): " + e);
            }
        }

        /// <summary>
        /// Loop of the fish bot.
        /// </summary>
        /// <param name="guidNode">The node GUID.</param>
        /// <param name="lureName"> </param>
        /// <param name="precision"> </param>
        /// <param name="useLure"> </param>
        public static void LoopFish(ulong guidNode = 0, bool useLure = false, string lureName = "",
                                    bool precision = false)
        {
            try
            {
                lock (typeof (FishingTask))
                {
                    _guidNode = guidNode;
                    _precision = precision;
                    _useLure = useLure;
                    _lureName = lureName;
                    if (_worker2 == null)
                    {
                        _worker2 = new Thread(LoopFishThread) {Name = "Fish"};
                        _worker2.Start();
                    }
                    _fishBotLaunched = true;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "FishingTask > LoopFish(ulong guidNode = 0, bool useLure = false, string lureName = \"\", bool precision = false): " +
                    e);
            }
        }

        private static void LoopFishThread()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        while (_fishBotLaunched)
                        {
                            Fishing.EquipFishingPoles();
                            if (_useLure)
                                Fishing.UseLure(_lureName);

                            if (fishingSpell == null)
                                fishingSpell = new Spell("Fishing");
                            fishingSpell.Launch(false, false, true);
                            Others.Wait(2000);
                            WoWGameObject objBobber =
                                new WoWGameObject(Fishing.SearchBobber());

                            if (objBobber.IsValid)
                            {
                                if (objBobber.GetBaseAddress > 0)
                                {
                                    WoWGameObject node =
                                        new WoWGameObject(
                                            ObjectManager.ObjectManager.GetObjectByGuid(_guidNode).GetBaseAddress);
                                    if (node.Position.DistanceTo2D(objBobber.Position) > distanceBobber && node.IsValid &&
                                        _guidNode > 0 && _precision)
                                        continue;

                                    while (_fishBotLaunched && ObjectManager.ObjectManager.Me.IsCast &&
                                           (int) objBobber.GetBaseAddress > 0 &&
                                           1 !=
                                           Memory.WowMemory.Memory.ReadShort(objBobber.GetBaseAddress + (uint) nManager.Wow.Patchables.Addresses.Fishing.BobberHasMoved))
                                    {
                                        Thread.Sleep(50);
                                        Application.DoEvents();
                                    }
                                    if (_fishBotLaunched && ObjectManager.ObjectManager.Me.IsCast &&
                                        (int) objBobber.GetBaseAddress > 0)
                                    {
                                        Interact.InteractWith(objBobber.GetBaseAddress);
                                        Statistics.Farms++;
                                        Others.Wait(1000);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    Thread.Sleep(400);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("FishingTask > LoopFishThread(): " + e);
            }
        }
    }
}