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
        private static UInt128 _lastnode;
        private static bool _wasLooted;
        private static bool _countThisLoot;
        private static bool _firstRun = true;

        public static void Pulse(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                if (_firstRun)
                {
                    EventsListener.HookEvent(WoWEventsType.LOOT_READY, callback => TakeFarmingLoots(), false, true);
                    _firstRun = false;
                }
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
                    if (ObjectManager.ObjectManager.Me.Position.Z < node.Position.Z)
                        zT = node.Position.Z + 5.5f;
                    else
                        zT = node.Position.Z + 2.5f;

                    Point aboveNode = new Point(node.Position);
                    aboveNode.Z = aboveNode.Z + 2.5f;
                    Point farAboveNode = new Point(aboveNode);
                    farAboveNode.Z = farAboveNode.Z + 80;
                    if (TraceLine.TraceLineGo(farAboveNode, aboveNode, CGWorldFrameHitFlags.HitTestAllButLiquid))
                    {
                        if (TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position, aboveNode, CGWorldFrameHitFlags.HitTestAllButLiquid))
                        {
                            Logging.Write("Node stuck");
                            nManagerSetting.AddBlackList(node.Guid, 1000*60*2);
                            return;
                        }
                    }

                    MovementManager.StopMove();
                    MovementManager.MoveTo(node.Position.X, node.Position.Y, zT, true);

                    Helpful.Timer timer = new Helpful.Timer((int) (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position)/3*1000) + 5000);
                    bool toMine = false;
                    bool landing = false;

                    while (node.IsValid && Products.Products.IsStarted &&
                           !ObjectManager.ObjectManager.Me.IsDeadMe &&
                           !(ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsMounted) &&
                           !timer.IsReady)
                    {
                        if (!landing)
                        {
                            bool noDirectPath = TraceLine.TraceLineGo(aboveNode);
                            zT = noDirectPath ? ObjectManager.ObjectManager.Me.Position.Z : aboveNode.Z;

                            if (ObjectManager.ObjectManager.Me.Position.Z < aboveNode.Z)
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
                            if (!noDirectPath)
                                landing = true;
                        }

                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) < 4.0f &&
                            ObjectManager.ObjectManager.Me.Position.DistanceZ(node.Position) >= 5.0f && !toMine)
                        {
                            toMine = true;
                            zT = node.Position.Z + 1.5f;
                            MovementManager.MoveTo(node.Position.X, node.Position.Y, zT);
                            if (node.GetDistance > 3.0f && TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position, node.Position, CGWorldFrameHitFlags.HitTestAllButLiquid))
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
                            {
                                if (!(SpellManager.HasSpell(169606) && Usefuls.ContinentId == 1116)) // Passive Silver Dollar Club given by Stables.
                                    Usefuls.DisMount();
                            }
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
                            _wasLooted = false;
                            _countThisLoot = true;
                            Interact.InteractWith(node.GetBaseAddress);
                            Thread.Sleep(Usefuls.Latency + 500);
                            if (!ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Interact.InteractWith(node.GetBaseAddress);
                                Thread.Sleep(Usefuls.Latency + 500);
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
                                _countThisLoot = false;
                                return;
                            }
                            Thread.Sleep(Usefuls.Latency + 100);
                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                            {
                                Usefuls.DisMount();
                                _countThisLoot = false;
                                return;
                            }
                            nManagerSetting.AddBlackList(node.Guid, 1000*20);
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
                    if (!_wasLooted)
                        Logging.Write("Farm failed");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("FarmingTask > Fly(IEnumerable<WoWGameObject> nodes): " + ex);
            }
        }

        private static WoWGameObject _curNode;

        private static void Ground(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                nodes = nodes.OrderBy(x => x.GetDistance);
                foreach (WoWGameObject node in nodes)
                {
                    WoWGameObject inode = node;
                    if (_curNode != null && _curNode.IsValid && ! nManagerSetting.IsBlackListed(_curNode.Guid))
                        inode = _curNode;
                    if (inode.IsValid)
                    {
                        _curNode = node;
                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(inode.Position) > 5.0f)
                        {
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(inode.Position) >=
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
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(inode.Position) >=
                                nManagerSetting.CurrentSetting.MinimumDistanceToUseMount &&
                                nManagerSetting.CurrentSetting.UseGroundMount)
                            {
                                if (MountTask.GetMountCapacity() == MountCapacity.Ground && !MountTask.OnGroundMount())
                                    MountTask.Mount();
                            }
                            if (MovementManager.FindTarget(inode, 5.0f, true, nManagerSetting.CurrentSetting.GatheringSearchRadius * 4.0f) == 0)
                            {
                                nManagerSetting.AddBlackList(inode.Guid, 1000*60*5);
                                _curNode = null;
                                return;
                            }
                            if (_lastnode != inode.Guid)
                            {
                                _lastnode = inode.Guid;
                                Logging.Write("Farm " + inode.Name + " > " + inode.Position);
                            }
                            if (MovementManager.InMovement)
                                return;
                        }
                        MovementManager.StopMove();
                        while (ObjectManager.ObjectManager.Me.GetMove)
                        {
                            Thread.Sleep(250);
                        }
                        Thread.Sleep(250 + Usefuls.Latency);
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            if (!inode.IsHerb || (inode.IsHerb && !ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId())))
                                MountTask.DismountMount();
                            return;
                        }
                        _wasLooted = false;
                        _countThisLoot = true;
                        Interact.InteractWith(inode.GetBaseAddress);
                        Thread.Sleep(Usefuls.Latency + 500);
                        if (!ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Interact.InteractWith(node.GetBaseAddress);
                            Thread.Sleep(Usefuls.Latency + 500);
                        }
                        while (ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(150);
                        }
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            _countThisLoot = false;
                            return;
                        }
                        Thread.Sleep(100 + Usefuls.Latency);
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            _countThisLoot = false;
                            return;
                        }
                        nManagerSetting.AddBlackList(inode.Guid, 1000*20); // 20 sec
                        if (_wasLooted)
                            Logging.Write("Farm failed");
                        return;
                    }
                    MovementManager.StopMove();
                    nManagerSetting.AddBlackList(inode.Guid);
                    Logging.Write("Farm failed");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("FarmingTask > Ground(IEnumerable<WoWGameObject> nodes): " + ex);
            }
        }

        private static void TakeFarmingLoots()
        {
            if (_countThisLoot)
            {
                _countThisLoot = false;
                LootingTask.LootAndConfirmBoPForAllItems(nManagerSetting.CurrentSetting.AutoConfirmOnBoPItems);
                Statistics.Farms++;
                Logging.Write("Farm successful");
                _wasLooted = true;
                _curNode = null;
                if (nManagerSetting.CurrentSetting.MakeStackOfElementalsItems && ObjectManager.ObjectManager.Me.InCombat)
                    Elemental.AutoMakeElemental();
            }
        }
    }
}