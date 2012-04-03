using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WowManager;
using WowManager.MiscStructs;
using WowManager.Navigation;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;
using WowManager.WoW.WoWObject;
using Timer = WowManager.Others.Timer;

namespace Questing_Bot.Bot.Tasks
{
    internal static class LootingTask
    {
        public static void Pulse(IEnumerable<WoWUnit> woWUnits)
        {
            woWUnits = woWUnits.OrderBy(x => x.GetDistance).ToList();
            foreach (WoWUnit wowUnit in woWUnits)
            {
                try
                {
                    if (Config.Bot.BotIsActive)
                    {
                        if (Config.Bot.BlackListGuid.Contains(wowUnit.Guid))
                            continue;

                        MovementManager.Stop();
                        MovementManager.StopMove();
                        Thread.Sleep(100);

                        if ((int) wowUnit.GetBaseAddress > 0)
                        {
                            if (wowUnit.IsLootable)
                                Log.AddLog(Translation.GetText(Translation.Text.Loot) + " " + wowUnit.Name);
                            else if (wowUnit.Skinnable && Config.Bot.FormConfig.Skinning)
                                Log.AddLog(Translation.GetText(Translation.Text.Skin) + " " + wowUnit.Name);
                            else
                                continue;

                            var points = new List<Point>();
                            if (ObjectManager.Me.Position.DistanceTo(wowUnit.Position) > 4.0f)
                            {
                                points = Functions.GoToPathFind(wowUnit.Position);
                                MovementManager.Go(points);
                            }
                            if (points.Count <= 0)
                            {
                                points.Add(ObjectManager.Me.Position);
                                points.Add(wowUnit.Position);
                            }
                            var timer = new Timer(((int) Point.SizeListPoint(points)/3*1000) + 5000);
                            while (!ObjectManager.Me.IsDeadMe && (int) wowUnit.GetBaseAddress > 0 &&
                                   Config.Bot.BotIsActive &&
                                   ObjectManager.GetNumberAttackPlayer() == 0 && !ObjectManager.Me.InCombat &&
                                   !timer.isReady)
                            {
                                if (ObjectManager.Me.Position.DistanceTo(wowUnit.Position) <= 4.0f)
                                {
                                    MovementManager.Stop();
                                    MovementManager.StopMove();
                                    MountTask.DismountMount();
                                    Thread.Sleep(500);
                                    while (ObjectManager.Me.GetMove)
                                    {
                                        Thread.Sleep(50);
                                    }

                                    if (wowUnit.IsLootable)
                                    {
                                        Interact.InteractGameObject(wowUnit.GetBaseAddress);
                                        if (ObjectManager.Me.InCombat)
                                        {
                                            return;
                                        }
                                        Thread.Sleep(1500);
                                        if (Config.Bot.FormConfig.Skinning && ObjectManager.GetNumberAttackPlayer() > 0)
                                            return;
                                        Config.Bot.NumberLoot++;
                                        if (!Config.Bot.FormConfig.Skinning)
                                        {
                                            Config.Bot.BlackList.Add(new Black(wowUnit.Guid, 1000 * 60 * 5));
                                            break;
                                        }
                                    }
                                    if (Config.Bot.FormConfig.Skinning && ObjectManager.GetNumberAttackPlayer() == 0)
                                    {
                                        Thread.Sleep(2000);
                                        if (wowUnit.Skinnable)
                                        {
                                            Log.AddLog(Translation.GetText(Translation.Text.Go_skin) + " " +
                                                       wowUnit.Name);
                                            Interact.InteractGameObject(wowUnit.GetBaseAddress);
                                            Thread.Sleep(500);
                                            while (ObjectManager.Me.IsCast)
                                            {
                                                Thread.Sleep(50);
                                            }
                                            if (ObjectManager.Me.InCombat)
                                            {
                                                return;
                                            }
                                            Thread.Sleep(1000);
                                            if (Config.Bot.FormConfig.Skinning &&
                                                ObjectManager.GetNumberAttackPlayer() > 0)
                                                return;
                                            Config.Bot.NumberFarm++;
                                            Config.Bot.BlackList.Add(new Black(wowUnit.Guid, 1000 * 60 * 5));
                                            break;
                                        }
                                    }
                                }

                                Thread.Sleep(30);
                            }
                            if (timer.isReady)
                                Config.Bot.BlackList.Add(new Black(wowUnit.Guid, 1000*60*20));
                        }
                        MovementManager.Stop();
                        MovementManager.StopMove();
                    }
                }
                catch
                {
                }
            }
        }
    }
}