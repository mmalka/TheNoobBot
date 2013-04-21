using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Keybindings = nManager.Wow.Helpers.Keybindings;
using Math = nManager.Helpful.Math;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.Tasks
{
    public static class FarmingTask
    {
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
                nodes = nodes.OrderBy(x => x.GetDistance).ToList();
                foreach (var node in nodes)
                {
                    if (node.IsValid)
                    {
                        MovementManager.StopMove();

                        Logging.Write("Farm " + node.Name + " > " + node.Position);
                        float zT;
                        Point pt = node.Position;
                        pt.Z = pt.Z + 1.5f;

                        if (ObjectManager.ObjectManager.Me.Position.Z < node.Position.Z)
                            zT = node.Position.Z + 5.5f;
                        else
                            zT = node.Position.Z + 2.5f;

                        var n = new Point(node.Position);
                        n.Z = n.Z + 2.5f;
                        var n2 = new Point(n);
                        n2.Z = n2.Z + 100;
                        if (TraceLine.TraceLineGo(n2, n, Enums.CGWorldFrameHitFlags.HitTestAllButLiquid))
                        {
                            Logging.Write("Node stuck");
                            nManagerSetting.AddBlackList(node.Guid, 1000*60*2);
                            return;
                        }

                        MovementManager.MoveTo(node.Position.X, node.Position.Y, zT, true);

                        int timer = Others.Times +
                                    ((int) ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position)/3*1000) +
                                    4000;
                        bool toMine = false;

                        while (node.IsValid && Products.Products.IsStarted &&
                               !ObjectManager.ObjectManager.Me.IsDeadMe &&
                               !(ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsMounted) &&
                               Others.Times < timer)
                        {
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) >= 10.0f)
                            {
                                var temps = new Point(node.Position.X, node.Position.Y, node.Position.Z + 2.5f);
                                if (temps.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 100)
                                {
                                    temps = Math.GetPostion2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position, temps, 100);
                                }
                                if (TraceLine.TraceLineGo(temps))
                                    zT = ObjectManager.ObjectManager.Me.Position.Z;
                                else
                                    zT = temps.Z;

                                if (ObjectManager.ObjectManager.Me.Position.Z < node.Position.Z + 2.5f)
                                {
                                    // elevate in a 45° angle instead of 90°
                                    Point direction = Math.GetPostion2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position,
                                                                                        node.Position,
                                                                                        (node.Position.Z + 2.5f) - ObjectManager.ObjectManager.Me.Position.Z);
                                    // if there is an obstacle, then go mostly vertical but not 90° to prevent spinning around
                                    if (TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position,
                                                              direction,
                                                              Enums.CGWorldFrameHitFlags.HitTestAllButLiquid))
                                        direction = Math.GetPostion2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position,
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
                                if (node.GetDistance > 4.0f &&  TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position, node.Position, CGWorldFrameHitFlags.HitTestAllButLiquid))
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
                                if (!ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                    Usefuls.DisMount();
                                Thread.Sleep(Usefuls.Latency + 300);
                                Interact.InteractGameObject(node.GetBaseAddress);
                                Thread.Sleep(Usefuls.Latency + 200);
                                if (!ObjectManager.ObjectManager.Me.IsCast)
                                {
                                    Interact.InteractGameObject(node.GetBaseAddress);
                                    Thread.Sleep(Usefuls.Latency + 200);
                                }
                                while (ObjectManager.ObjectManager.Me.IsCast)
                                {
                                    Thread.Sleep(50);
                                }
                                if ((ObjectManager.ObjectManager.Me.InCombat &&
                                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                                {
                                    if (ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                        Lua.RunMacroText("/cancelform");
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
                                    if (ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                        Lua.RunMacroText("/cancelform");
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
                        if (Others.Times > timer)
                            nManagerSetting.AddBlackList(node.Guid);
                        MovementManager.StopMove();
                        Logging.Write("Farm failed");
                    }
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
                nodes = nodes.OrderBy(x => x.GetDistance).ToList();
                foreach (var node in nodes)
                {
                    MovementManager.StopMove();
                    if ((int) node.GetBaseAddress > 0)
                    {
                        var points = new List<Point>();
                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) > 4.0f)
                        {
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) >=
                                nManagerSetting.CurrentSetting.MinimumDistanceToUseMount ||
                                !nManagerSetting.CurrentSetting.UseGroundMount)
                            {
                                if (MountTask.GetMountCapacity() == MountCapacity.Fly)
                                {
                                    if (!MountTask.onFlyMount())
                                        MountTask.Mount();
                                    else
                                        MountTask.Takeoff();
                                    Fly(nodes);
                                    return;
                                }
                                else if (MountTask.GetMountCapacity() == MountCapacity.Swimm)
                                {
                                    if (!MountTask.onAquaticMount())
                                        MountTask.Mount();
                                    Fly(nodes);
                                    return;
                                }
                            }
                            // fallback to ground mount or feet
                            bool r;
                            points = PathFinder.FindPath(node.Position, out r);
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) >=
                                nManagerSetting.CurrentSetting.MinimumDistanceToUseMount &&
                                nManagerSetting.CurrentSetting.UseGroundMount)
                            {
                                if (MountTask.GetMountCapacity() == MountCapacity.Ground && !MountTask.onGroundMount())
                                    MountTask.Mount();
                            }
                            MovementManager.Go(points);
                        }

                        Logging.Write("Farm " + node.Name + " > " + node.Position);
                        var timer = new Timer(((int) Math.DistanceListPoint(points)/3*1000) + 4000);
                        while ((int) node.GetBaseAddress > 0 && Products.Products.IsStarted &&
                               !ObjectManager.ObjectManager.Me.IsDeadMe &&
                               !(ObjectManager.ObjectManager.Me.InCombat &&
                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                               !timer.IsReady)
                        {
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) <= 4.0f)
                            {
                                MovementManager.StopMove();

                                if (!ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                    MountTask.DismountMount();

                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Thread.Sleep(800);
                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                while (ObjectManager.ObjectManager.Me.GetMove)
                                {
                                    Thread.Sleep(50);
                                }
                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Interact.InteractGameObject(node.GetBaseAddress);
                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Thread.Sleep(500);
                                while (ObjectManager.ObjectManager.Me.IsCast)
                                {
                                    Thread.Sleep(50);
                                }
                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Thread.Sleep(Usefuls.Latency + 1250);
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

                            Thread.Sleep(30);
                        }
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            MountTask.DismountMount();
                            return;
                        }
                        if (timer.IsReady)
                            nManagerSetting.AddBlackList(node.Guid);
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