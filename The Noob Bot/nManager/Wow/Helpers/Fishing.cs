using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Patchables;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Helpers
{
    public class Fishing
    {
        private static Timer _timerLure;

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
                        foreach (var i in Others.ReadFileAllLines(Application.StartupPath + "\\Data\\fishingPoles.txt"))
                        {
                            try
                            {
                                _listFishingPoles.Add(Convert.ToUInt32(i));
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

        private static List<uint> listLure
        {
            get
            {
                try
                {
                    if (_listLure == null)
                    {
                        _listLure = new List<uint>();
                        foreach (var i in Others.ReadFileAllLines(Application.StartupPath + "\\Data\\lures.txt"))
                        {
                            try
                            {
                                _listLure.Add(Convert.ToUInt32(i));
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

        /// <summary>
        /// Use lure.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <param name="lureName"> </param>
        /// <returns name=""></returns>
        public static void UseLure(string lureName = "")
        {
            try
            {
                if (_timerLure != null)
                    if (!_timerLure.IsReady)
                        return;

                if (lureName != string.Empty)
                {
                    ItemsManager.UseItem(lureName);
                    var useSpell = new Spell(lureName);
                    useSpell.Launch();
                    Thread.Sleep(1000);
                    Thread.Sleep(Usefuls.Latency);
                    while (ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(200);
                    }
                }
                else
                {
                    foreach (int i in listLure)
                    {
                        if (ItemsManager.GetItemCountByIdLUA(i) > 0)
                        {
                            ItemsManager.UseItem(ItemsManager.GetNameById(i));
                            Thread.Sleep(1000);
                            while (ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(200);
                            }
                            break;
                        }
                    }
                }
                _timerLure = new Timer(10*1000*60); // 10 min
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
                    if (ItemsManager.GetItemCountByIdLUA(i) > 0)
                    {
                        return ItemsManager.GetNameById(i);
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
        public static void EquipFishingPoles(string fishingPoleName = "")
        {
            try
            {
                if (IsEquipedFishingPoles())
                    return;

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
                        if (ItemsManager.GetItemCountByIdLUA(i) > 0)
                        {
                            ItemsManager.EquipItemByName(ItemsManager.GetNameById(i));
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
                    if (ItemsManager.GetItemCountByIdLUA(i) > 0)
                    {
                        return ItemsManager.GetNameById(i);
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
                var idEquiped =
                    ObjectManager.ObjectManager.Me.GetDescriptor<uint>(Descriptors.PlayerFields.VisibleItems + 15*2);
                if (fishingPoleName != string.Empty)
                    if (ItemsManager.GetNameById((int) idEquiped) == fishingPoleName)
                        return true;

                if (ListFishingPoles.Contains(idEquiped))
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
                return (from t in ObjectManager.ObjectManager.GetWoWGameObjectByDisplayId(668)
                        where t.CreatedBy == ObjectManager.ObjectManager.Me.Guid
                        select t.GetBaseAddress).FirstOrDefault();
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
            float MinRing = 13.0f; //13
            float MaxRing = 20.0f; //19

            Enums.CGWorldFrameHitFlags hitFlags =
                Enums.CGWorldFrameHitFlags.HitTestBoundingModels |
                Enums.CGWorldFrameHitFlags.HitTestWMO |
                Enums.CGWorldFrameHitFlags.HitTestUnknown |
                Enums.CGWorldFrameHitFlags.HitTestGround;

            bool res;
            Point from, to;
            for (float diameter = MinRing; diameter <= MaxRing; diameter = diameter + 3.0f)
            {
                for (double angle = 0; angle <= System.Math.PI/2; angle = angle + System.Math.PI/10)
                {
                    float offset1 = diameter*(float) System.Math.Sin(angle);
                    float offset2 = diameter*(float) System.Math.Cos(angle);
                    to = new Point(fishingHole.X + offset1, fishingHole.Y - offset2, fishingHole.Z);
                    to.Z = to.Z - 0.8f;
                    from = new Point(to);
                    from.Z = fishingHole.Z + 20.0f;
                    res = TraceLine.TraceLineGo(from, to, hitFlags);
                    if (res)
                    {
                        float nz = PathFinder.GetZPosition(to);
                        if (nz == 0)
                            continue;
                        else
                            to.Z = nz + 1.0f;
                        return to;
                    }

                    to = new Point(fishingHole.X + offset2, fishingHole.Y + offset1, fishingHole.Z);
                    to.Z = to.Z - 1.0f;
                    from = new Point(to);
                    from.Z = fishingHole.Z + 20.0f;
                    res = TraceLine.TraceLineGo(from, to, hitFlags);
                    if (res)
                    {
                        float nz = PathFinder.GetZPosition(to);
                        if (nz == 0)
                            continue;
                        else
                            to.Z = nz + 1.0f;
                        return to;
                    }
                    to = new Point(fishingHole.X - offset1, fishingHole.Y + offset2, fishingHole.Z);
                    to.Z = to.Z - 1.0f;
                    from = new Point(to);
                    from.Z = fishingHole.Z + 20.0f;
                    res = TraceLine.TraceLineGo(from, to, hitFlags);
                    if (res)
                    {
                        float nz = PathFinder.GetZPosition(to);
                        if (nz == 0)
                            continue;
                        else
                            to.Z = nz + 1.0f;
                        return to;
                    }
                    to = new Point(fishingHole.X - offset2, fishingHole.Y - offset1, fishingHole.Z);
                    to.Z = to.Z - 1.0f;
                    from = new Point(to);
                    from.Z = fishingHole.Z + 20.0f;
                    res = TraceLine.TraceLineGo(from, to, hitFlags);
                    if (res)
                    {
                        float nz = PathFinder.GetZPosition(to);
                        if (nz == 0)
                            continue;
                        else
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