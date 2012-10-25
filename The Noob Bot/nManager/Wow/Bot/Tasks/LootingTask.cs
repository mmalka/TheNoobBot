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

                            if ((int)wowUnit.GetBaseAddress > 0)
                            {
                                if (wowUnit.IsLootable)
                                    Logging.Write("Loot " + wowUnit.Name);
                                else if (wowUnit.Skinnable && nManagerSetting.CurrentSetting.skinMobs)
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
                                var timer = new Timer(((int)Math.DistanceListPoint(points) / 3 * 1000) + 5000);
                                while (!ObjectManager.ObjectManager.Me.IsDeadMe && (int)wowUnit.GetBaseAddress > 0 &&
                                       Products.Products.IsStarted &&
                                       ObjectManager.ObjectManager.GetNumberAttackPlayer() == 0 &&
                                       !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) &&
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
                                            Interact.InteractGameObject(wowUnit.GetBaseAddress);
                                            if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                                            {
                                                return;
                                            }
                                            Thread.Sleep(1250);
                                            if (nManagerSetting.CurrentSetting.skinMobs && ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                                                return;
                                            Statistics.Loots++;
                                            if (nManagerSetting.CurrentSetting.autoMakeElemental && !ObjectManager.ObjectManager.Me.InCombat)
                                                Elemental.AutoMakeElemental();
                                            if (!nManagerSetting.CurrentSetting.skinMobs)
                                            {
                                                nManagerSetting.AddBlackList(wowUnit.Guid, 1000 * 60 * 5);
                                                break;
                                            }
                                        }
                                        if (nManagerSetting.CurrentSetting.skinMobs && ObjectManager.ObjectManager.GetNumberAttackPlayer() == 0)
                                        {
                                            Thread.Sleep(1500);
                                            if (wowUnit.Skinnable)
                                            {
                                                Logging.Write("Skin " + wowUnit.Name);
                                                Interact.InteractGameObject(wowUnit.GetBaseAddress);
                                                Thread.Sleep(500);
                                                while (ObjectManager.ObjectManager.Me.IsCast)
                                                {
                                                    Thread.Sleep(50);
                                                }
                                                if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                                                {
                                                    return;
                                                }
                                                Thread.Sleep(1000);
                                                if (nManagerSetting.CurrentSetting.skinMobs &&
                                                    ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                                                    return;
                                                Statistics.Farms++;
                                                nManagerSetting.AddBlackList(wowUnit.Guid, 1000 * 60 * 5);
                                                break;
                                            }
                                        }
                                    }

                                    Thread.Sleep(30);
                                }
                                if (timer.IsReady)
                                    nManagerSetting.AddBlackList(wowUnit.Guid, 1000 * 60 * 20);
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

        public static void ConfirmBoP()
        {
            try
            {
                Thread.Sleep(Usefuls.Latency + 500);
                Lua.LuaDoString(
                    "for slot = 1, GetNumLootItems() do" +
                    "  if GetLootSlotType(slot) == LOOT_SLOT_ITEM then" +
                    "    ConfirmLootSlot(slot)" +
                    "  end" +
                    "end");
                /* TODO : We need to handle properly the event OR We need to check if the current Slot is a BoP item or not.
                 * Currently, we count how many loots we've got, and check one by one if the loot is an "item" (and not golds, etc).
                 * Note: AutoConfirmBoPItems is false per default.
                 */
                Thread.Sleep(Usefuls.Latency);
            }
            catch (Exception ex)
            {
                Logging.WriteError("LootingTask > ConfirmBoP(): " + ex);
            }
        }
    }
}
