using System.Collections.Generic;
using System.Threading;
using WowManager;
using WowManager.MiscStructs;
using WowManager.Navigation;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;
using WowManager.WoW.WoWObject;
using Timer = WowManager.Others.Timer;
using System.Linq;
namespace Questing_Bot.Bot.Tasks
{
    static class FarmingTask
    {
        public static void Pulse(IEnumerable<WoWGameObject> nodes)
        {
            nodes = nodes.OrderBy(x => x.GetDistance).ToList();
            foreach (var node in nodes)
            {
                MovementManager.StopMove();
                MovementManager.Stop();
                if ((int) node.GetBaseAddress > 0)
                {
                    Log.AddLog(Translation.GetText(Translation.Text.Farm) + " " + node.Name + " > " + node.Position);
                    var points = new List<Point>();
                    if (ObjectManager.Me.Position.DistanceTo(node.Position) > 4.0f)
                    {
                        points = Functions.GoToPathFind(node.Position);
                        MovementManager.Go(points);
                    }
                    if (points.Count <= 0)
                    {
                        points.Add(ObjectManager.Me.Position);
                        points.Add(node.Position);
                    }
                    var timer = new Timer(((int) Point.SizeListPoint(points)/3*1000) + 5000);
                    while ((int) node.GetBaseAddress > 0 && Config.Bot.BotIsActive && !ObjectManager.Me.IsDeadMe &&
                           !ObjectManager.Me.InCombat && !timer.isReady)
                    {
                        if (ObjectManager.Me.Position.DistanceTo(node.Position) <= 4.5f)
                        {
                            MovementManager.Stop();
                            MovementManager.StopMove();
                            MountTask.DismountMount();
                            if (ObjectManager.Me.InCombat)
                            {
                                return;
                            }
                            Thread.Sleep(1000);
                            if (ObjectManager.Me.InCombat)
                            {
                                return;
                            }
                            while (ObjectManager.Me.GetMove)
                            {
                                Thread.Sleep(50);
                            }
                            if (ObjectManager.Me.InCombat)
                            {
                                return;
                            }
                            Interact.InteractGameObject(node.GetBaseAddress);
                            if (ObjectManager.Me.InCombat)
                            {
                                return;
                            }
                            Thread.Sleep(500);
                            while (ObjectManager.Me.IsCast && !ObjectManager.Me.InCombat)
                            {
                                Thread.Sleep(50);
                            }
                            if (ObjectManager.Me.InCombat)
                            {
                                return;
                            }
                            Thread.Sleep(1500);
                            if (ObjectManager.Me.InCombat)
                            {
                                return;
                            }
                            Config.Bot.NumberFarm++;
                            Config.Bot.BlackList.Add(new Black(node.Guid, 1000*60*10));
                            Log.AddLog(Translation.GetText(Translation.Text.Farm_successful));
                            return;
                        }

                        Thread.Sleep(30);
                    }
                    if (ObjectManager.Me.InCombat)
                    {
                        MountTask.DismountMount();
                        return;
                    }
                    if (timer.isReady)
                        Config.Bot.BlackList.Add(new Black(node.Guid));
                }
                MovementManager.Stop();
                MovementManager.StopMove();
                Config.Bot.BlackList.Add(new Black(node.Guid));
                Log.AddLog(Translation.GetText(Translation.Text.Farm_failed));
            }
        }
    }
}
