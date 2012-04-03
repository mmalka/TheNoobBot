using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WowManager.MiscStructs;
using WowManager.Others;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.Useful;
using Keybindings = WowManager.MiscEnums.Keybindings;

namespace Questing_Bot.Bot.Tasks
{
    internal class LongMove
    {
        private static bool _used;
        private static bool _usedLoop;

        private static Point _pointLongMove = new Point();

        public static bool IsLongMove
        {
            get { return _used || _usedLoop; }
        }

        public static void LongMoveByNewThread(Point point)
        {
            _pointLongMove = point;
            var worker2 = new Thread(LongMoveGo) { IsBackground = true, Name = "LongMove" };
            worker2.Start();
            Thread.Sleep(100);
        }

        private static void LongMoveGo()
        {
            LongMoveGo(_pointLongMove);
        }

        public static void LongMoveGo(Point point)
        {
            WowManager.Navigation.MovementManager.StopMove();
            int timer = Others.Times;

            if (_used)
            {
                _used = false;
                while (_usedLoop)
                {
                    Thread.Sleep(5);
                }
            }
            _used = true;
            _usedLoop = true;

            WowManager.Navigation.MovementManager.Stop();
            WowManager.Navigation.MovementManager.StopMove();

            while (Config.Bot.BotIsActive && ObjectManager.Me.IsMount && ObjectManager.Me.Position.DistanceTo(point) > 3.5f &&
                   _used && _usedLoop)
            {
                if (!WowManager.Navigation.MovementManager.IsUnStuck)
                {
                    if (ObjectManager.Me.Position.DistanceTo2D(point) > 15)
                    {
                        Point meTemps = ObjectManager.Me.Position;
                        meTemps.Z = meTemps.Z - 2;

                        var temps = new Point(point.X, point.Y, ObjectManager.Me.Position.Z - 2.5f);
                        if (point.DistanceTo(ObjectManager.Me.Position) > 100)
                        {
                            temps = Others.GetPostion2dOfLineByDistance(ObjectManager.Me.Position, point, 100);
                            temps.Z = ObjectManager.Me.Position.Z - 2.5f;
                        }
                        if (TraceLine.TraceLineGo(meTemps, temps) ||
                            (ObjectManager.Me.Position.Z + 10 < point.Z &&
                             point.DistanceTo2D(ObjectManager.Me.Position) < 100))
                        {
                            WowManager.WoW.Useful.Keybindings.UpKeybindings(Keybindings.SITORSTAND);
                            WowManager.Navigation.MovementManager.MoveTo(ObjectManager.Me.Position.X,
                                                                         ObjectManager.Me.Position.Y,
                                                                         ObjectManager.Me.Position.Z + 20);
                        }
                        else
                        {
                            WowManager.WoW.Useful.Keybindings.UpKeybindings(Keybindings.JUMP);
                            temps = new Point(point.X, point.Y, ObjectManager.Me.Position.Z - 50.0f);
                            if (point.DistanceTo(ObjectManager.Me.Position) > 100)
                            {
                                temps = Others.GetPostion2dOfLineByDistance(ObjectManager.Me.Position, point, 100);
                                temps.Z = ObjectManager.Me.Position.Z - 50.0f;
                            }
                            var temps2 = new Point(ObjectManager.Me.Position.X, ObjectManager.Me.Position.Y,
                                                   ObjectManager.Me.Position.Z - 40f);
                            if (!TraceLine.TraceLineGo(meTemps, temps) && !TraceLine.TraceLineGo(temps2) &&
                                point.DistanceTo2D(ObjectManager.Me.Position) > 100)
                            {
                                WowManager.WoW.Useful.Keybindings.DownKeybindings(Keybindings.SITORSTAND);
                            }
                            else
                            {
                                WowManager.WoW.Useful.Keybindings.UpKeybindings(Keybindings.SITORSTAND);
                            }

                            WowManager.Navigation.MovementManager.MoveTo(point.X, point.Y, ObjectManager.Me.Position.Z);
                            if (Others.Times > (timer + 1500))
                            {
                                WowManager.Navigation.MovementManager.MoveTo(point.X, point.Y,
                                                                             ObjectManager.Me.Position.Z);
                                timer = Others.Times;
                            }
                        }
                    }
                    else
                    {
                        WowManager.WoW.Useful.Keybindings.UpKeybindings(Keybindings.JUMP);
                        WowManager.WoW.Useful.Keybindings.UpKeybindings(Keybindings.SITORSTAND);
                        WowManager.Navigation.MovementManager.MoveTo(point);
                    }
                }
                Thread.Sleep(50);
            }
            WowManager.WoW.Useful.Keybindings.UpKeybindings(Keybindings.JUMP);
            WowManager.WoW.Useful.Keybindings.UpKeybindings(Keybindings.SITORSTAND);
            _used = false;
            _usedLoop = false;
        }

        public static void StopLongMove()
        {
            _used = false;
            _usedLoop = false;
        }
    }
}
