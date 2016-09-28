using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.Tasks
{
    public class FishingTask
    {
        private static bool _fishBotLaunched;
        private static Thread _worker2;

        private static Spell fishingSpell;

        private static UInt128 _guidNode;
        private static bool _precision;
        private static string _lureName = "";
        private static string _fishingPoleName = "";
        private static bool _useLure;
        private static bool _automaticallyUseDraenorSecondaryBait;
        public static int _lastSuccessfullFishing;
        private static bool _firstRun = true;

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
                    _lastSuccessfullFishing = 0;
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
        /// <param name="useLure"> </param>
        /// <param name="lureName"> </param>
        /// <param name="fishingPoleName"></param>
        /// <param name="precision"> </param>
        /// <param name="automaticallyUseDraenorSecondaryBait"></param>
        public static void LoopFish(UInt128 guidNode = default(UInt128), bool useLure = false, string lureName = "", string fishingPoleName = "", bool precision = false,
            bool automaticallyUseDraenorSecondaryBait = true)
        {
            try
            {
                lock (typeof (FishingTask))
                {
                    _guidNode = guidNode;
                    _precision = precision;
                    _useLure = useLure;
                    _lureName = lureName;
                    _fishingPoleName = fishingPoleName;
                    _automaticallyUseDraenorSecondaryBait = automaticallyUseDraenorSecondaryBait;
                    Fishing.ReCheckFishingPoleTimer.ForceReady(); // In case the user stop/start the fisher bot, 
                    // we want him to be able to equip his new fishing pole if it's the case.
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
                    "FishingTask > LoopFish(UInt128 guidNode = 0, bool useLure = false, string lureName = \"\", bool precision = false): " +
                    e);
            }
        }

        private static void LoopFishThread()
        {
            try
            {
                if (_firstRun)
                {
                    EventsListener.HookEvent(WoWEventsType.LOOT_READY, callback => FarmingTask.TakeFarmingLoots(), false, true);
                    _firstRun = false;
                }
                while (true)
                {
                    try
                    {
                        while (_fishBotLaunched)
                        {
                            if (ObjectManager.ObjectManager.Me.InCombat)
                            {
                                StopLoopFish();
                                continue;
                            }
                            Fishing.EquipFishingPoles(_fishingPoleName);
                            if (_useLure)
                                Fishing.UseLure(_lureName, _automaticallyUseDraenorSecondaryBait);

                            if (fishingSpell == null)
                                fishingSpell = new Spell("Fishing");
                            fishingSpell.Launch(false, false, true);
                            Thread.Sleep(4000);
                            WoWGameObject objBobber =
                                new WoWGameObject(Fishing.SearchBobber());

                            if (objBobber.IsValid)
                            {
                                WoWGameObject node = new WoWGameObject(ObjectManager.ObjectManager.GetObjectByGuid(_guidNode).GetBaseAddress);
                                if (node.Position.DistanceTo2D(objBobber.Position) > distanceBobber && node.IsValid &&
                                    _guidNode > 0 && _precision)
                                    continue;

                                while (_fishBotLaunched && ObjectManager.ObjectManager.Me.IsCast && objBobber.IsValid &&
                                       1 != Memory.WowMemory.Memory.ReadShort(objBobber.GetBaseAddress + (uint) Patchables.Addresses.Fishing.BobberHasMoved))
                                {
                                    /*
                                     * BobberHasMoved FINDER 
                                     uint i = 148;
                                    while (false)
                                    {
                                        short info = Memory.WowMemory.Memory.ReadShort(objBobber.GetBaseAddress + (uint) i);
                                        if (info == 1)
                                        {
                                            Logging.Write("Info is " + info + " with i=" + i);
                                        }
                                        i += 4;
                                        if (i > 350)
                                            i = 148;
                                        Thread.Sleep(30);
                                    }
                                */
                                    Thread.Sleep(250);
                                }
                                if (_fishBotLaunched && ObjectManager.ObjectManager.Me.IsCast && objBobber.IsValid)
                                {
                                    FarmingTask.CountThisLoot = true;
                                    Interact.InteractWith(objBobber.GetBaseAddress);
                                    _lastSuccessfullFishing = Environment.TickCount;
                                    Statistics.Farms++;
                                    Thread.Sleep(Usefuls.Latency + 400); // Arround 450
                                    for (int i = 0; i < 10; i++)
                                    {
                                        if (!Others.IsFrameVisible("LootFrame"))
                                            break;
                                        Thread.Sleep(150); // usually stop at i = 2 for me
                                        // it's like the old 1sec sleep, but can be faster sometimes to recast.
                                        // and this will loot 100% of the time, while if you have high latency, 1sec wont loot everything.
                                    }
                                    /*if (Others.IsFrameVisible("LootFrame"))
                                    {
                                        Logging.WriteDebug("We did not loot all items within < 2 second + latency.");
                                        // More of a debug code, no need.
                                    }*/
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