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
    internal static class Loot
    {
        private const int SearchDistance = 35;
        private static readonly Timer TimerGetLoot = new Timer(1*700);
        private static ulong _lastRet;

        public static ulong GetLoot()
        {
            if (!Config.Bot.Loot)
                return 0;

            if (!TimerGetLoot.isReady)
                return _lastRet;
            TimerGetLoot.Reset();
            _lastRet = 0;

            WoWPlayer wowPlayer = ObjectManager.GetNearestWoWPlayer(ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? ObjectManager.GetWoWUnitAllianceDead() : ObjectManager.GetWoWUnitHordeDead());

            if (wowPlayer != null)
            {
                if (wowPlayer.GetDistance < SearchDistance && !Config.Bot.BlackListLoot.Contains(wowPlayer.Guid) &&
                    wowPlayer.IsValid)
                {
                    _lastRet = wowPlayer.Guid;
                    return _lastRet;
                }
            }
            return 0;
        }

        public static void LootUnit(ulong guidWowUnit)
        {
            try
            {
                if (guidWowUnit > 0 && Config.Bot.BotStarted)
                {
                    WowManager.Navigation.MovementManager.StopMove();
                    Thread.Sleep(100);

                    var wowUnit = new WoWUnit(ObjectManager.GetObjectByGuid(guidWowUnit).GetBaseAddress);

                    if ((int) wowUnit.GetBaseAddress > 0)
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.Loot) + " " + wowUnit.Name);

                        var points = new List<Point>();
                        if (ObjectManager.Me.Position.DistanceTo(wowUnit.Position) > 4.0f)
                        {
                            points = PathFinderManager.FindPath(wowUnit.Position);
                            WowManager.Navigation.MovementManager.Go(points);
                        }
                        if (points.Count <= 0)
                        {
                            points.Add(ObjectManager.Me.Position);
                            points.Add(wowUnit.Position);
                        }
                        int timer = Others.Times + ((int) Point.SizeListPoint(points)/3*1000) + 20000;
                        Thread.Sleep(1000);
                        while (wowUnit.IsValid && !ObjectManager.Me.IsDeadMe && Config.Bot.BotStarted &&
                               !ObjectManager.Me.InCombat)
                        {
                            if (ObjectManager.Me.Position.DistanceTo(wowUnit.Position) <= 4.0f)
                            {
                                WowManager.Navigation.MovementManager.Stop();
                                Mount.DismountMount();
                                Thread.Sleep(500);
                                while (ObjectManager.Me.GetMove)
                                {
                                    Thread.Sleep(50);
                                }


                                Interact.InteractGameObject(wowUnit.GetBaseAddress);
                                if (ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Thread.Sleep(1500);
                                Config.Bot.BlackListLoot.Add(wowUnit.Guid);

                                return;
                            }

                            Thread.Sleep(30);
                        }
                        if (Others.Times > timer)
                            Config.Bot.BlackListLoot.Add(wowUnit.Guid);
                    }
                    WowManager.Navigation.MovementManager.Stop();
                    Config.Bot.BlackListLoot.Add(wowUnit.Guid);
                    Log.AddLog(Translation.GetText(Translation.Text.Loot_failed));
                }
            }
            catch
            {
            }
        }
    }
}