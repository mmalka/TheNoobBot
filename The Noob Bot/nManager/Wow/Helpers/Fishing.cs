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
        static List<uint> ListFishingPoles
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
        static List<uint> listLure
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
                    foreach (uint i in listLure)
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
                _timerLure = new Timer(10 * 1000 * 60); // 10 min
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
                    if (ItemsManager.GetItemCountByIdLUA((uint)i) > 0)
                    {
                        return ItemsManager.GetNameById((uint)i);
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
                    foreach (uint i in ListFishingPoles)
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
                foreach (uint i in ListFishingPoles)
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
                var idEquiped = ObjectManager.ObjectManager.Me.GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_16_ENTRYID);
                if (fishingPoleName != string.Empty)
                    if (ItemsManager.GetNameById(idEquiped) == fishingPoleName)
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
    }
}
