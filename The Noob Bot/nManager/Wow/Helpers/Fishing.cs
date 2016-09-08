using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Helpers
{
    public class Fishing
    {
        private static List<uint> _listFishingPoles;

        private static List<uint> ListFishingPoles
        {
            get
            {
                try
                {
                    if (_listFishingPoles == null)
                    {
                        _listFishingPoles = new List<uint>();
                        foreach (string i in Others.ReadFileAllLines(Application.StartupPath + "\\Data\\fishingPoles.txt"))
                        {
                            try
                            {
                                _listFishingPoles.Add(Others.ToUInt32(i));
                            }
                            catch
                            {
                            }
                        }
                    }
                    return _listFishingPoles;
                }
                catch (Exception e)
                {
                    Logging.WriteError("Fishing > ListFishingPoles : " + e);
                    return new List<uint>();
                }
            }
        }

        private static List<uint> _listLure;

        private static IEnumerable<uint> listLure
        {
            get
            {
                try
                {
                    if (_listLure == null)
                    {
                        _listLure = new List<uint>();
                        foreach (string i in Others.ReadFileAllLines(Application.StartupPath + "\\Data\\lures.txt"))
                        {
                            try
                            {
                                _listLure.Add(Others.ToUInt32(i));
                            }
                            catch
                            {
                            }
                        }
                    }
                    return _listLure;
                }
                catch (Exception e)
                {
                    Logging.WriteError("Fishing > listLure : " + e);
                    return new List<uint>();
                }
            }
        }

        public static int[] DraenicBaitList = {110290, 110291, 110274, 110293, 110294, 110289, 110292};

        public static bool HaveDraenicBaitBuff()
        {
            foreach (int baitId in DraenicBaitList)
            {
                if (SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(baitId)))
                    return true;
            }
            if (SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(116755)))
                return true; // Do not cancel Nat Pagle Lukers Lure.
            if (SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(128229)))
                return true; // Do not cancel Felmouth Frenzy Bait.
            return false;
        }

        public static int[] DraenicBaitListInInventory
        {
            get
            {
                var tmpList = new List<int>();
                foreach (int i in DraenicBaitList)
                {
                    if (ItemsManager.GetItemCount(i) > 0)
                        tmpList.Add(i);
                }
                return tmpList.ToArray();
            }
        }

        public static int GetRandomDraenicBait()
        {
            var inv = DraenicBaitListInInventory;
            var randomized = new Random().Next(0, inv.Length);
            return inv.Length <= 0 ? 0 : inv[randomized];
        }

        private static readonly List<int> DraenorSeasList = new List<int>
        {
            7233,
            7246,
            7251,
            7257,
            7233,
            7238,
            7239,
            7255,
            7258,
            7259,
            7262,
            7300,
            7407,
            7408,
            7410,
            7414,
            7428,
            7436,
            7445,
            7446,
            7448,
            6968,
            7055,
            7173
        };

        public static void UseDraenicBait()
        {
            if (Usefuls.ContinentId != 1116 && Usefuls.ContinentId != 1464 || HaveDraenicBaitBuff())
                return;
            int baitToUse = 0;
            if (Usefuls.AreaId == 7004 || Usefuls.AreaId == 7078)
            {
                // Frostwall Horde garrison or Lunarfall Alliance garrison
                baitToUse = GetRandomDraenicBait();
            }
            else if (DraenorSeasList.Contains(Usefuls.SubAreaId))
            {
                baitToUse = DraenicBaitList[6];
            }
            else if (Usefuls.AreaId == 6719)
            {
                // Shadowmoon Valley
                baitToUse = DraenicBaitList[0];
            }
            else if (Usefuls.AreaId == 6720)
            {
                // Frostfire Ridge
                baitToUse = DraenicBaitList[1];
            }
            else if (Usefuls.AreaId == 6721)
            {
                // Gorgrond
                baitToUse = DraenicBaitList[2];
            }
            else if (Usefuls.AreaId == 6722)
            {
                // Spires of Arak
                baitToUse = DraenicBaitList[3];
            }
            else if (Usefuls.AreaId == 6662)
            {
                // Talador
                baitToUse = DraenicBaitList[4];
            }
            else if (Usefuls.AreaId == 6755)
            {
                // Nagrand
                baitToUse = DraenicBaitList[5];
            }
            else if (Usefuls.ContinentId == 1464)
            {
                baitToUse = 128229; // Felmouth Frenzy Bait can't be used in Garrison.
            }

            if (baitToUse == 0 || ItemsManager.GetItemCount(baitToUse) <= 0)
                return;
            string baitToUseName = ItemsManager.GetItemNameById(baitToUse);
            if (ItemsManager.IsItemUsable(baitToUseName))
                ItemsManager.UseItem(baitToUseName);
        }

        /// <summary>
        /// Use lure.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <param name="lureName"> </param>
        /// <returns name=""></returns>
        public static bool FirstLureCheck = true;

        public static void UseLure(string lureName = "", bool automaticallyUseDraenorBait = false)
        {
            try
            {
                if (automaticallyUseDraenorBait)
                    UseDraenicBait();
                if (!IsEquipedFishingPoles())
                    return;
                if (ObjectManager.ObjectManager.Me.IsMainHandTemporaryEnchanted)
                    return; // Already lured up.

                if (lureName != string.Empty)
                {
                    if (ItemsManager.GetItemCount(ItemsManager.GetItemIdByName(lureName)) > 0)
                    {
                        ItemsManager.UseItem(lureName);
                        Thread.Sleep(1000);
                        Thread.Sleep(Usefuls.Latency);
                        while (ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(200);
                        }
                        return;
                    }
                    else
                    {
                        Spell lureSpell = new Spell(lureName);
                        if (lureSpell.KnownSpell && lureSpell.IsSpellUsable)
                        {
                            lureSpell.Launch();
                            return;
                        }
                        if (!lureSpell.KnownSpell)
                        {
                            if (FirstLureCheck)
                            {
                                Logging.Write("Lure from Product Settings is missing, try to use a lure from the list built-in TheNoobBot.");
                                FirstLureCheck = false;
                            }
                        }
                    }
                }
                foreach (int i in listLure)
                {
                    if (ItemsManager.GetItemCount(i) > 0)
                    {
                        ItemsManager.UseItem(ItemsManager.GetItemNameById(i));
                        Thread.Sleep(1000);
                        while (ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(200);
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Fishing > UseLure(string lureName = \"\"): " + e);
            }
        }

        public static string LureName()
        {
            try
            {
                foreach (int i in listLure)
                {
                    if (ItemsManager.GetItemCount(i) > 0)
                    {
                        return ItemsManager.GetItemNameById(i);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Fishing > LureName(): " + e);
            }
            return "";
        }

        /// <summary>
        /// Equip canne.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <param name="fishingPoleName"> </param>
        /// <returns name=""></returns>
        public static Timer ReCheckFishingPoleTimer = new Timer(1);

        public static void EquipFishingPoles(string fishingPoleName = "")
        {
            try
            {
                if (!ReCheckFishingPoleTimer.IsReady || IsEquipedFishingPoles())
                    return;
                Logging.WriteDebug("Parsing inventory to find a Fishing Pole to equip.");
                if (fishingPoleName != string.Empty)
                {
                    ItemsManager.EquipItemByName(fishingPoleName);
                    Thread.Sleep(500);
                    Thread.Sleep(Usefuls.Latency);
                    while (ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(200);
                    }
                }
                else
                {
                    foreach (int i in ListFishingPoles)
                    {
                        if (ItemsManager.GetItemCount(i) > 0)
                        {
                            ItemsManager.EquipItemByName(ItemsManager.GetItemNameById(i));
                            Thread.Sleep(500);
                            Thread.Sleep(Usefuls.Latency);
                            while (ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(200);
                            }
                            break;
                        }
                    }
                }
                ReCheckFishingPoleTimer = new Timer(1000*60*5);
                Logging.WriteDebug("Inventory parsed, prevent this function from being parsed for the next five minutes.");
            }
            catch (Exception e)
            {
                Logging.WriteError("Fishing > EquipFishingPoles(string fishingPoleName = \"\"): " + e);
            }
        }

        public static string FishingPolesName()
        {
            try
            {
                foreach (int i in ListFishingPoles)
                {
                    if (ItemsManager.GetItemCount(i) > 0)
                    {
                        return ItemsManager.GetItemNameById(i);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Fishing > FishingPolesName(): " + e);
            }
            return "";
        }

        public static bool IsEquipedFishingPoles(string fishingPoleName = "")
        {
            try
            {
                WoWItem idEquiped = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_FISHINGPOLE);
                if (fishingPoleName != string.Empty)
                    if (idEquiped.Name == fishingPoleName)
                        return true;

                if (ListFishingPoles.Contains((uint) idEquiped.Entry))
                    return true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Fishing >IsEquipedFishingPoles(string fishingPoleName = \"\"): " + e);
            }
            return false;
        }

        /// <summary>
        /// Return a Bobber Address.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns name="BobberAddress"></returns>
        public static uint SearchBobber()
        {
            try
            {
                var listDisplayID = new List<uint> {668, 29716, 12161, 18332}; // 29716 = big bobber, 12161 = Duck Bobber, 18332 = Murloc Bobber
                foreach (WoWGameObject t in ObjectManager.ObjectManager.GetWoWGameObjectByDisplayId(listDisplayID))
                {
                    if (t.CreatedBy == ObjectManager.ObjectManager.Me.Guid)
                    {
                        uint u = t.GetBaseAddress;
                        return u;
                    }
                }
                return 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Fishing > SearchBobber(): " + e);
            }
            return 0;
        }

        /// <summary>
        /// Find a point around the fishing hole from where we can land to fish
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <param name="fishingHole"> </param>
        /// <returns name="TheBestPoint"></returns>
        public static Point FindTheUltimatePoint(Point fishingHole)
        {
            const float MinRing = 13.0f; //13
            const float MaxRing = 20.0f; //19

            const CGWorldFrameHitFlags hitFlags = CGWorldFrameHitFlags.HitTestBoundingModels |
                                                  CGWorldFrameHitFlags.HitTestWMO |
                                                  CGWorldFrameHitFlags.HitTestUnknown |
                                                  CGWorldFrameHitFlags.HitTestGround;

            for (float diameter = MinRing; diameter <= MaxRing; diameter = diameter + 3.0f)
            {
                for (double angle = 0; angle <= System.Math.PI/2; angle = angle + System.Math.PI/10)
                {
                    float offset1 = diameter*(float) System.Math.Sin(angle);
                    float offset2 = diameter*(float) System.Math.Cos(angle);
                    Point to = new Point(fishingHole.X + offset1, fishingHole.Y - offset2, fishingHole.Z);
                    to.Z = to.Z - 0.8f;
                    Point from = new Point(to) {Z = fishingHole.Z + 20.0f};
                    bool res = TraceLine.TraceLineGo(@from, to, hitFlags);
                    if (res)
                    {
                        float nz = PathFinder.GetZPosition(to);
                        if (nz == 0)
                            continue;
                        to.Z = nz + 1.0f;
                        return to;
                    }

                    to = new Point(fishingHole.X + offset2, fishingHole.Y + offset1, fishingHole.Z);
                    to.Z = to.Z - 1.0f;
                    from = new Point(to) {Z = fishingHole.Z + 20.0f};
                    res = TraceLine.TraceLineGo(from, to, hitFlags);
                    if (res)
                    {
                        float nz = PathFinder.GetZPosition(to);
                        if (nz == 0)
                            continue;
                        to.Z = nz + 1.0f;
                        return to;
                    }
                    to = new Point(fishingHole.X - offset1, fishingHole.Y + offset2, fishingHole.Z);
                    to.Z = to.Z - 1.0f;
                    from = new Point(to) {Z = fishingHole.Z + 20.0f};
                    res = TraceLine.TraceLineGo(from, to, hitFlags);
                    if (res)
                    {
                        float nz = PathFinder.GetZPosition(to);
                        if (nz == 0)
                            continue;
                        to.Z = nz + 1.0f;
                        return to;
                    }
                    to = new Point(fishingHole.X - offset2, fishingHole.Y - offset1, fishingHole.Z);
                    to.Z = to.Z - 1.0f;
                    from = new Point(to) {Z = fishingHole.Z + 20.0f};
                    res = TraceLine.TraceLineGo(from, to, hitFlags);
                    if (res)
                    {
                        float nz = PathFinder.GetZPosition(to);
                        if (nz == 0)
                            continue;
                        to.Z = nz + 1.0f;
                        return to;
                    }
                }
            }

            Point pt = new Point(0, 0, 0, "invalid");
            return pt;
        }
    }
}