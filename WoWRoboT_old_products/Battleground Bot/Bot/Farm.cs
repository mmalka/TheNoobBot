using System.Collections.Generic;
using System.Threading;
using WowManager;
using WowManager.MiscStructs;
using WowManager.Navigation;
using WowManager.Others;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;
using WowManager.WoW.WoWObject;
using Timer = WowManager.Others.Timer;

namespace Battleground_Bot.Bot
{
    internal static class Farm
    {
        private static List<int> _listDisplayIdFarm;
        private static readonly Timer TimerGetFarm = new Timer(1*600);
        private static ulong _lastRet;

        public static ulong GetFarm()
        {
            if (!TimerGetFarm.isReady)
                return _lastRet;
            TimerGetFarm.Reset();
            _lastRet = 0;

            LoadListFarm();

            List<WoWGameObject> tl =
                ObjectManager.GetWoWGameObjectById(_listDisplayIdFarm);
            for (int i = 0; i <= tl.Count - 1; i++)
            {
                if (!NobesList.GetListId("Battleground", (int) tl[i].GetDistance).Contains(tl[i].Entry))
                {
                    tl.RemoveAt(i);
                    i--;
                }
            }
            WoWGameObject nobe = ObjectManager.GetNearestWoWGameObject(tl);

            if (nobe != null)
            {
                if (nobe.GetDistance < 40)
                {
                    _lastRet = nobe.Guid;
                    return _lastRet;
                }
            }
            return 0;
        }

        public static void FarmNobe(ulong guidNobe)
        {
            try
            {
                if (guidNobe > 0 && Config.Bot.BotStarted)
                {
                    WowManager.Navigation.MovementManager.StopMove();
                    Thread.Sleep(700);

                    var nobe = new WoWGameObject(ObjectManager.GetObjectByGuid(guidNobe).GetBaseAddress);

                    if ((int) nobe.GetBaseAddress > 0)
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.Use_node) + " " + nobe.Name + " > " + nobe.Position);
                        var points = new List<Point>();
                        if (ObjectManager.Me.Position.DistanceTo(nobe.Position) > 4.0f)
                        {
                            points = PathFinderManager.FindPath(nobe.Position);
                            WowManager.Navigation.MovementManager.Go(points);
                        }
                        if (points.Count <= 0)
                        {
                            points.Add(ObjectManager.Me.Position);
                            points.Add(nobe.Position);
                        }
                        int timer = Others.Times + ((int) Point.SizeListPoint(points)/3*1000) + 5000;

                        while ((int) nobe.GetBaseAddress > 0 && Config.Bot.BotStarted &&
                               !ObjectManager.Me.IsDeadMe && !ObjectManager.Me.InCombat && Others.Times < timer)
                        {
                            if (ObjectManager.Me.Position.DistanceTo(nobe.Position) <= 4.0f)
                            {
                                WowManager.Navigation.MovementManager.Stop();
                                Mount.DismountMount();
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
                                Interact.InteractGameObject(nobe.GetBaseAddress);
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
                                Log.AddLog(Translation.GetText(Translation.Text.Node_used_succes));
                                return;
                            }

                            Thread.Sleep(30);
                        }
                        if (ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                    }
                    WowManager.Navigation.MovementManager.Stop();
                    Log.AddLog(Translation.GetText(Translation.Text.Use_node_failed));
                }
            }
            catch
            {
            }
        }

        private static void LoadListFarm()
        {
            if (_listDisplayIdFarm == null)
            {
                Log.AddLog("Load nobe list.");
                _listDisplayIdFarm = new List<int>();
                _listDisplayIdFarm.AddRange(NobesList.GetListId("Battleground", 999999));
            }
        }
    }
}