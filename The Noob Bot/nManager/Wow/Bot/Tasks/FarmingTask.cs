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

        static void Fly(IEnumerable<WoWGameObject> nodes)
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

                        /*
                        if (ObjectManager.Me.Position.Z > node.Position.Z)
                            zT = ObjectManager.Me.Position.Z + 5.0f;
                        else
                            zT = node.Position.Z + 10f;

                        
                        
                        if (!TraceLine.TraceLineGo(pt) && ObjectManager.Me.Position.DistanceTo2D(node.Position) < 100)
                            zT = node.Position.Z + 1.5f;
                        */
                        // Replace code:
                        if (ObjectManager.ObjectManager.Me.Position.Z < node.Position.Z)
                            zT = ObjectManager.ObjectManager.Me.Position.Z + 5.5f;
                        else
                            zT = node.Position.Z + 2.5f;



                        // Try way
                        /*
                        if (Bot.ConfigBot.Skipnode)
                        {
                            pt = node.Position;
                            pt.Z = zT;
                            if (TraceLine.TraceLineGo(pt) || TraceLine.TraceLineGo(pt, node.Position))
                            {
                                pt.Z = node.Position.Z + 1.5f;
                                pt.X = node.Position.X + 1.0f;
                                zT = node.Position.Z + 1.5f;
                                if (TraceLine.TraceLineGo(pt) || TraceLine.TraceLineGo(pt, node.Position))
                                {
                                    Log.AddLog(Translation.GetText(Translation.Text.Node_stuck));
                                    Bot.ConfigBot.BlackListnode.Add(node.Guid);
                                    return;
                                }
                            }
                        }
                        */
                        var n = new Point(node.Position);
                        n.Z = n.Z + 2.5f;
                        var n2 = new Point(n);
                        n2.Z = n2.Z + 100;
                        if (TraceLine.TraceLineGo(n2, n))
                        {
                            Logging.Write("Node stuck");
                            nManagerSetting.AddBlackList(node.Guid, 1000*120);
                            return;
                        }


                        MovementManager.MoveTo(node.Position.X, node.Position.Y, zT);

                        int timer = Others.Times + ((int)ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) / 3 * 1000) +
                                    5000;
                        bool toMine = false;

                        while (node.IsValid && Products.Products.IsStarted &&
                               !ObjectManager.ObjectManager.Me.IsDeadMe && !(ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsMounted) &&
                               Others.Times < timer)
                        {
                            //pt.Z = pt.Z + 1.5f;
                            /*if (!TraceLine.TraceLineGo(pt) &&
                                ObjectManager.Me.Position.DistanceTo2D(node.Position) < 100 &&
                                node.Position.Z + 1.6f > zT)
                                zT = node.Position.Z + 1.5f;
                            */
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) >= 10.0f)
                            {
                                var temps = new Point(node.Position.X, node.Position.Y, node.Position.Z + 2.5f);
                                if (temps.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 100)
                                {
                                    temps = Math.GetPostion2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position, temps, 100);
                                    temps.Z = ObjectManager.ObjectManager.Me.Position.Z + 1.5f;
                                }
                                if (TraceLine.TraceLineGo(temps))
                                    zT = ObjectManager.ObjectManager.Me.Position.Z;
                                else
                                    zT = node.Position.Z + 2.5f;


                                if (ObjectManager.ObjectManager.Me.Position.Z < node.Position.Z + 2.5f)
                                    MovementManager.MoveTo(ObjectManager.ObjectManager.Me.Position.X, ObjectManager.ObjectManager.Me.Position.Y, node.Position.Z + 5.0f);
                                else
                                {
                                    var temps1 = Math.GetPostion2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position, node.Position, ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) + 0.9f);
                                    MovementManager.MoveTo(temps1.X, temps1.Y, zT);
                                }
                            }

                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) < 10.0f &&
                                ObjectManager.ObjectManager.Me.Position.DistanceZ(node.Position) >= 6 && !toMine)
                            {
                                toMine = true;
                                zT = node.Position.Z + 1.5f;
                                var temps = Math.GetPostion2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position, node.Position, ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) + 0.9f);
                                MovementManager.MoveTo(temps.X, temps.Y, zT);
                                //Thread.Sleep(700);
                                if (TraceLine.TraceLineGo(node.Position) && node.GetDistance > 4.0f)
                                {
                                    Logging.Write("Node outside view");
                                    nManagerSetting.AddBlackList(node.Guid, 1000*120);
                                    break;
                                }
                            }
                            else if ((ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) < 1.1f ||
                                      (!ObjectManager.ObjectManager.Me.IsMounted &&
                                       ObjectManager.ObjectManager.Me.Position.DistanceTo2D(node.Position) < 3.0f)) &&
                                     ObjectManager.ObjectManager.Me.Position.DistanceZ(node.Position) < 6)
                            {
                                Thread.Sleep(150);
                                MovementManager.StopMove();
                                if (Usefuls.IsFlying || ObjectManager.ObjectManager.Me.IsMounted)
                                {
                                    /*if (NodesList.GetListId("Herb", Skill.GetValue(SkillLine.Herbalism)).Contains(node.Entry) && ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                    {
                                        Thread.Sleep(100);
                                        Keybindings.DownKeybindings(Enums.Keybindings.SITORSTAND);
                                        var t = new Timer(5500);
                                        while (Usefuls.IsFlying && !t.IsReady)
                                        {
                                            Thread.Sleep(50);
                                        }
                                        Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                                        Thread.Sleep(10);
                                    }
                                    else
                                    {
                                    */
                                        Keybindings.DownKeybindings(Enums.Keybindings.SITORSTAND);
                                        var t = new Timer(850);
                                        while (Usefuls.IsFlying && !t.IsReady)
                                        {
                                            Thread.Sleep(3);
                                        }
                                        Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                                        Thread.Sleep(10);
                                    /*
                                        if (Usefuls.IsFlying || ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                        {
                                            MountTask.DismountMount();
                                            Thread.Sleep(500);
                                        }
                                    }
                                     */
                                }

                                while (ObjectManager.ObjectManager.Me.GetMove)
                                {
                                    Thread.Sleep(50);
                                }
                                if (!(ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()) && NodesList.GetListId("Herb", Skill.GetValue(SkillLine.Herbalism)).Contains(node.Entry)))
                                    Usefuls.DisMount();
                                Thread.Sleep(Usefuls.Latency + 400);
                                Interact.InteractGameObjectBeta23(node.GetBaseAddress);
                                Thread.Sleep(Usefuls.Latency + 200); // tauren cast plus vite
                                if (!ObjectManager.ObjectManager.Me.IsCast)
                                {
                                    Interact.InteractGameObjectBeta23(node.GetBaseAddress);
                                    Thread.Sleep(750);
                                }
                                while (ObjectManager.ObjectManager.Me.IsCast)
                                {
                                    Thread.Sleep(50);
                                }
                                if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                                {
                                    if (ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                        Lua.RunMacroText("/cancelform");
                                    return;
                                }
                                Thread.Sleep(Usefuls.Latency);
                                Thread.Sleep(100);
                                Statistics.Farms++;
                                if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                                {
                                    if (ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                        Lua.RunMacroText("/cancelform");
                                    return;
                                }
                                nManagerSetting.AddBlackList(node.Guid);
                                Logging.Write("Farm successful");
                                if (nManagerSetting.CurrentSetting.autoMakeElemental && !ObjectManager.ObjectManager.Me.InCombat)
                                    Elemental.AutoMakeElemental();

                                /*if (ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                                {
                                    Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                                    Thread.Sleep(500);
                                    Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                                }*/

                                return;
                            }
                            else if (!ObjectManager.ObjectManager.Me.GetMove)
                            {
                                Thread.Sleep(50);
                                MovementManager.MoveTo(node.Position.X, node.Position.Y, zT);
                            }

                            if (States.Farming.PlayerNearest(node))
                            {
                                Logging.Write("Player Nearest of the node, farm failed");
                                nManagerSetting.AddBlackList(node.Guid, 15 * 1000);
                                return;
                            }

                            Thread.Sleep(10);
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

        static void Ground(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                nodes = nodes.OrderBy(x => x.GetDistance).ToList();
                foreach (var node in nodes)
                {
                    MovementManager.StopMove();
                    if ((int)node.GetBaseAddress > 0)
                    {
                        var points = new List<Point>();
                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) > 4.0f)
                        {
                            bool r;
                            points = PathFinder.FindPath(node.Position, out r);
                            if (nManagerSetting.CurrentSetting.FlyingMountName != string.Empty && (!r || ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) >= 15.0f))
                            {
                                MountTask.MountingFlyingMount();
                                if (Usefuls.IsFlying)
                                {
                                    Fly(nodes);
                                    return;
                                }
                            }
                            MovementManager.Go(points);
                        }
                        if (points.Count <= 0)
                        {
                            points.Add(ObjectManager.ObjectManager.Me.Position);
                            points.Add(node.Position);
                        }
                        Logging.Write("Farm " + node.Name + " > " + node.Position);
                        var timer = new Timer(((int)Math.DistanceListPoint(points) / 3 * 1000) + 5000);
                        while ((int)node.GetBaseAddress > 0 && Products.Products.IsStarted && !ObjectManager.ObjectManager.Me.IsDeadMe &&
                               !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) && !timer.IsReady)
                        {
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(node.Position) <= 4.5f)
                            {
                                MovementManager.StopMove();

                                if (!(ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()) && NodesList.GetListId("Herb", Skill.GetValue(SkillLine.Herbalism)).Contains(node.Entry)))
                                    MountTask.DismountMount();

                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Thread.Sleep(1000);
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
                                Interact.InteractGameObjectBeta23(node.GetBaseAddress);
                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Thread.Sleep(500);
                                while (ObjectManager.ObjectManager.Me.IsCast && !ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    Thread.Sleep(50);
                                }
                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Thread.Sleep(Usefuls.Latency);
                                Thread.Sleep(1500);
                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Statistics.Farms++;
                                nManagerSetting.AddBlackList(node.Guid, 1000 * 60 * 5);
                                Logging.Write("Farm successful");
                                if (nManagerSetting.CurrentSetting.autoMakeElemental && !ObjectManager.ObjectManager.Me.InCombat)
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
