using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Math = nManager.Helpful.Math;

namespace nManager.Wow.Bot.Tasks
{
    public static class FarmingTask
    {
        private static Int128 _lastnode;

        public static void Pulse(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                if (Usefuls.IsFlying)
                    Fly(nodes);
                else
                    Ground(nodes);
            }
            catch (Exception ex)
            {
                Logging.WriteError("FarmingTask > Pulse(IEnumerable<WoWGameObject> nodes): " + ex);
            }
        }

        private static void Fly(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                nodes = nodes.OrderBy(x => x.GetDistance);
                foreach (WoWGameObject node in nodes.Where(node => node.IsValid))
                {
                    Logging.Write("Farm " + node.Name + " (" + node.Entry + ") > " + node.Position.X + "; " + node.Position.Y + "; " + node.Position.Z);
                    float zT;
                    Point pt = node.Position;
                    pt.Z = pt.Z + 1.5f;

                    if (ObjectManager.ObjectManager.Me.Position.Z < node.Position.Z)
                        zT = node.Position.Z + 5.5f;
                    else
                        zT = node.Position.Z + 2.5f;

                    Point n = new Point(node.Position);
                    n.Z = n.Z + 2.5f;
                    Point n2 = new Point(n);
                    n2.Z = n2.Z + 60;
                    if (TraceLine.TraceLineGo(n2, n, CGWorldFrameHitFlags.HitTestAllButLiquid))
                    {
                        Logging.Write("Node stuck");
                        nManagerSetting.AddBlackList(node.Guid, 1000*60*2);
                        return;
                    }

                    MovementManager.StopMove();
                    MovementManager.MoveTo(node.Position.X, node.Position.Y, zT, true);

                    Helpful.Timer timer = new Helpful.Timer((int) (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) / 3 * 1000) + 5000);
                    bool toMine = false;

                    while (node.IsValid && Products.Products.IsStarted &&
                           !ObjectManager.ObjectManager.Me.IsDeadMe &&
                           !(ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsMounted) &&
                           !timer.IsReady)
                    {
                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) >= 10.0f)
                        {
                            Point temps = new Point(node.Position.X, node.Position.Y, node.Position.Z + 2.5f);
                            if (temps.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 100)
                            {
                                temps = Math.GetPosition2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position, temps, 100);
                            }
                            zT = TraceLine.TraceLineGo(temps) ? ObjectManager.ObjectManager.Me.Position.Z : temps.Z;

                            if (ObjectManager.ObjectManager.Me.Position.Z < node.Position.Z + 2.5f)
                            {
                                // elevate in a 45° angle instead of 90°
                                Point direction = Math.GetPosition2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position,
                                    node.Position,
                                    (node.Position.Z + 2.5f) - ObjectManager.ObjectManager.Me.Position.Z);
                                // if there is an obstacle, then go mostly vertical but not 90° to prevent spinning around
                                if (TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position,
                                    direction,
                                    CGWorldFrameHitFlags.HitTestAllButLiquid))
                                    direction = Math.GetPosition2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position,
                                        node.Position, 1.0f);
                                MovementManager.MoveTo(direction.X, direction.Y, node.Position.Z + 5.0f);
                            }
                            else
                            {
                                MovementManager.MoveTo(node.Position.X, node.Position.Y, zT);
                            }
                        }

                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) < 5.0f &&
                            ObjectManager.ObjectManager.Me.Position.DistanceZ(node.Position) >= 6 && !toMine)
                        {
                            toMine = true;
                            zT = node.Position.Z + 1.5f;
                            MovementManager.MoveTo(node.Position.X, node.Position.Y, zT);
                            if (node.GetDistance > 4.0f && TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position, node.Position, CGWorldFrameHitFlags.HitTestAllButLiquid))
                            {
                                Logging.Write("Node outside view");
                                nManagerSetting.AddBlackList(node.Guid, 1000*120);
                                break;
                            }
                        }
                        else if ((ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) < 1.1f ||
                                  (!Usefuls.IsFlying &&
                                   ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) < 3.0f)) &&
                                 ObjectManager.ObjectManager.Me.Position.DistanceZ(node.Position) < 6)
                        {
                            Thread.Sleep(150);
                            MovementManager.StopMove();
                            if (Usefuls.IsFlying)
                            {
                                MountTask.Land();
                            }
                            if (ObjectManager.ObjectManager.Me.GetMove)
                            {
                                MovementManager.StopMove();
                            }
                            while (ObjectManager.ObjectManager.Me.GetMove)
                            {
                                Thread.Sleep(50);
                            }
                            if (!node.IsHerb || node.IsHerb && !ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                Usefuls.DisMount();
                            else if (node.IsHerb)
                            {
                                Logging.WriteDebug("Druid IsFlying ? " + Usefuls.IsFlying);
                                if (Usefuls.IsFlying)
                                {
                                    MountTask.Land();
                                    MovementManager.StopMove();
                                    if (Usefuls.IsFlying)
                                    {
                                        Logging.Write("You are still flying after two attemps of Landing.");
                                        Logging.Write("Make sure you have binded the action \"SitOrStand\" on a keyboard key and not any mouse button or special button.");
                                        Logging.Write("If you still have this message, please try a \"Reset Keybindings\" before posting on the forum.");
                                        Logging.Write("A work arround have been used, it may let you actually farm or not. Because it's random, please fix your keybinding issue.");
                                        MountTask.Land(true);
                                    }
                                }
                            }
                            Thread.Sleep(Usefuls.Latency + 200);
                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                            {
                                Usefuls.DisMount();
                                return;
                            }
                            Interact.InteractWith(node.GetBaseAddress);
                            Thread.Sleep(Usefuls.Latency + 400);
                            if (!ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Interact.InteractWith(node.GetBaseAddress);
                                Thread.Sleep(Usefuls.Latency + 400);
                            }
                            while (ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(100);
                            }
                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                            {
                                Usefuls.DisMount();
                                return;
                            }
                            Thread.Sleep(Usefuls.Latency + 100);
                            if (nManagerSetting.CurrentSetting.AutoConfirmOnBoPItems)
                                LootingTask.ConfirmOnBoPItems();
                            Statistics.Farms++;
                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                            {
                                Usefuls.DisMount();
                                return;
                            }
                            nManagerSetting.AddBlackList(node.Guid, 1000*20);
                            Logging.Write("Farm successful");
                            if (nManagerSetting.CurrentSetting.MakeStackOfElementalsItems &&
                                !ObjectManager.ObjectManager.Me.InCombat)
                                Elemental.AutoMakeElemental();

                            return;
                        }
                        else if (!ObjectManager.ObjectManager.Me.GetMove)
                        {
                            Thread.Sleep(50);
                            MovementManager.MoveTo(node.Position.X, node.Position.Y, zT);
                        }
                        if (States.Farming.PlayerNearest(node))
                        {
                            Logging.Write("Player near the node, farm canceled");
                            nManagerSetting.AddBlackList(node.Guid, 15*1000);
                            return;
                        }
                    }
                    if (timer.IsReady)
                        nManagerSetting.AddBlackList(node.Guid);
                    MovementManager.StopMove();
                    Logging.Write("Farm failed");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("FarmingTask > Fly(IEnumerable<WoWGameObject> nodes): " + ex);
            }
        }

        private static void Ground(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                nodes = nodes.OrderBy(x => x.GetDistance);
                foreach (WoWGameObject node in nodes)
                {
                    if (node.IsValid)
                    {
                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) > 5.0f)
                        {
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) >=
                                nManagerSetting.CurrentSetting.MinimumDistanceToUseMount ||
                                !nManagerSetting.CurrentSetting.UseGroundMount)
                            {
                                if (MountTask.GetMountCapacity() == MountCapacity.Fly)
                                {
                                    if (!MountTask.OnFlyMount())
                                        MountTask.Mount();
                                    else
                                        MountTask.Takeoff();
                                    Fly(nodes);
                                    return;
                                }
                                if (MountTask.GetMountCapacity() == MountCapacity.Swimm)
                                {
                                    if (!MountTask.OnAquaticMount())
                                        MountTask.Mount();
                                    Fly(nodes);
                                    return;
                                }
                            }
                            // fallback to ground mount or feet
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) >=
                                nManagerSetting.CurrentSetting.MinimumDistanceToUseMount &&
                                nManagerSetting.CurrentSetting.UseGroundMount)
                            {
                                if (MountTask.GetMountCapacity() == MountCapacity.Ground && !MountTask.OnGroundMount())
                                    MountTask.Mount();
                            }
                            MovementManager.FindTarget(node, 5.0f);
                            if (_lastnode != node.Guid)
                            {
                                _lastnode = node.Guid;
                                Logging.Write("Farm " + node.Name + " > " + node.Position);
                            }
                            if (MovementManager.InMovement)
                                return;
                        }
                        Thread.Sleep(250 + Usefuls.Latency);
                        while (ObjectManager.ObjectManager.Me.GetMove)
                        {
                            Thread.Sleep(250);
                        }
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            if (!node.IsHerb || (node.IsHerb && !ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId())))
                                MountTask.DismountMount();
                            return;
                        }
                        Interact.InteractWith(node.GetBaseAddress);
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Thread.Sleep(250 + Usefuls.Latency);
                        while (ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(250);
                        }
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Thread.Sleep(250 + Usefuls.Latency);
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        if (nManagerSetting.CurrentSetting.AutoConfirmOnBoPItems)
                            LootingTask.ConfirmOnBoPItems();
                        Statistics.Farms++;
                        nManagerSetting.AddBlackList(node.Guid, 1000*20); //60 * 5); // 20 sec instead of 5 min
                        Logging.Write("Farm successful");
                        if (nManagerSetting.CurrentSetting.MakeStackOfElementalsItems &&
                            !ObjectManager.ObjectManager.Me.InCombat)
                            Elemental.AutoMakeElemental();
                        return;
                    }
                    MovementManager.StopMove();
                    nManagerSetting.AddBlackList(node.Guid);
                    Logging.Write("Farm failed");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("FarmingTask > Ground(IEnumerable<WoWGameObject> nodes): " + ex);
            }
        }
    }
}