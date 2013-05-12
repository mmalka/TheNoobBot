using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Math = nManager.Helpful.Math;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.Tasks
{
    public static class LootingTask
    {
        public static void Pulse(IEnumerable<WoWUnit> woWUnits)
        {
            try
            {
                woWUnits = woWUnits.OrderBy(x => x.GetDistance).ToList();
                foreach (WoWUnit wowUnit in woWUnits)
                {
                    try
                    {
                        if (Products.Products.IsStarted)
                        {
                            if (nManagerSetting.IsBlackListed(wowUnit.Guid))
                                continue;

                            MovementManager.StopMove();
                            MovementManager.StopMove();
                            Thread.Sleep(100);

                            if ((int) wowUnit.GetBaseAddress > 0)
                            {
                                if (wowUnit.IsLootable)
                                    Logging.Write("Loot " + wowUnit.Name);
                                else if (wowUnit.IsSkinnable && nManagerSetting.CurrentSetting.ActivateBeastSkinning)
                                    Logging.Write("Skin " + wowUnit.Name);
                                else
                                    continue;

                                var points = new List<Point>();
                                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(wowUnit.Position) > 4.0f)
                                {
                                    points = PathFinder.FindPath(wowUnit.Position);
                                    MovementManager.Go(points);
                                }
                                if (points.Count <= 0)
                                {
                                    points.Add(ObjectManager.ObjectManager.Me.Position);
                                    points.Add(wowUnit.Position);
                                }
                                var timer = new Timer(((int) Math.DistanceListPoint(points)/3*1000) + 5000);
                                while (!ObjectManager.ObjectManager.Me.IsDeadMe && (int) wowUnit.GetBaseAddress > 0 &&
                                       Products.Products.IsStarted &&
                                       ObjectManager.ObjectManager.GetNumberAttackPlayer() == 0 &&
                                       !(ObjectManager.ObjectManager.Me.InCombat &&
                                         !(ObjectManager.ObjectManager.Me.IsMounted &&
                                           (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                                       !timer.IsReady)
                                {
                                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(wowUnit.Position) <= 4.0f)
                                    {
                                        MovementManager.StopMove();
                                        MovementManager.StopMove();
                                        MountTask.DismountMount();
                                        Thread.Sleep(250);
                                        while (ObjectManager.ObjectManager.Me.GetMove)
                                        {
                                            Thread.Sleep(50);
                                        }

                                        if (wowUnit.IsLootable)
                                        {
                                            Interact.InteractWith(wowUnit.GetBaseAddress);
                                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted ||
                                                    Usefuls.IsFlying))))
                                            {
                                                return;
                                            }
                                            Thread.Sleep(1250);
                                            if (nManagerSetting.CurrentSetting.ActivateBeastSkinning &&
                                                ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                                                return;
                                            Statistics.Loots++;
                                            if (nManagerSetting.CurrentSetting.MakeStackOfElementalsItems &&
                                                !ObjectManager.ObjectManager.Me.InCombat)
                                                Elemental.AutoMakeElemental();
                                            if (!nManagerSetting.CurrentSetting.ActivateBeastSkinning)
                                            {
                                                WoWUnit unit = wowUnit;
                                                foreach (var u in woWUnits.Where(u => u != unit).Where(u => u.Position.DistanceTo(unit.Position) <= 20f))
                                                {
                                                    nManagerSetting.AddBlackList(u.Guid, 2500);
                                                }
                                                nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*5);
                                                break;
                                            }
                                        }
                                        if (nManagerSetting.CurrentSetting.ActivateBeastSkinning &&
                                            ObjectManager.ObjectManager.GetNumberAttackPlayer() == 0)
                                        {
                                            Thread.Sleep(1500);
                                            if (wowUnit.IsSkinnable)
                                            {
                                                Logging.Write("Skin " + wowUnit.Name);
                                                Interact.InteractWith(wowUnit.GetBaseAddress);
                                                Thread.Sleep(500);
                                                while (ObjectManager.ObjectManager.Me.IsCast)
                                                {
                                                    Thread.Sleep(50);
                                                }
                                                if ((ObjectManager.ObjectManager.Me.InCombat &&
                                                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                                                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted ||
                                                        Usefuls.IsFlying))))
                                                {
                                                    return;
                                                }
                                                Thread.Sleep(1000);
                                                if (nManagerSetting.CurrentSetting.ActivateBeastSkinning &&
                                                    ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                                                    return;
                                                Statistics.Farms++;
                                                nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*5);
                                                break;
                                            }
                                        }
                                    }

                                    Thread.Sleep(30);
                                }
                                if (timer.IsReady)
                                    nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*20);
                            }
                            MovementManager.StopMove();
                            MovementManager.StopMove();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("LootingTask > Pulse(IEnumerable<WoWUnit> woWUnits): " + ex);
            }
        }

        public static void ConfirmOnBoPItems()
        {
            try
            {
                Thread.Sleep(Usefuls.Latency);
                Lua.LuaDoString(
                    "for slot = 1, GetNumLootItems() do " +
                    //"  if GetLootSlotType(slot) != 2 then " +
                    "    ConfirmLootSlot(slot) " +
                    //"  end " +
                    "end");
                /* TODO : We need to handle properly the event OR We need to check if the current Slot is a BoP item or not.
                 * Currently, we count how many loots we've got, and check one by one if the loot is an "item" (and not golds, etc).
                 * Note: AutoConfirmOnBoPItems is false per default.
                 */
                Thread.Sleep(Usefuls.Latency);
            }
            catch (Exception ex)
            {
                Logging.WriteError("LootingTask > ConfirmOnBoPItems(): " + ex);
            }
        }
    }
}