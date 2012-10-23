using System;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Class;
using Math = nManager.Helpful.Math;

namespace nManager.Wow.Helpers
{
    public class LongMove
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
            try
            {
                _pointLongMove = point;
                var worker2 = new Thread(LongMoveGo) { IsBackground = true, Name = "LongMove" };
                worker2.Start();
                Thread.Sleep(100);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveByNewThread(Point point): " + exception);
            }
        }

        private static void LongMoveGo()
        {
            try
            {
                LongMoveGo(_pointLongMove);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveGo(): " + exception);
            }
        }

        public static void LongMoveGo(Point point)
        {
            try
            {
                //MovementManager.StopMove();
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

                //MovementManager.StopMove();
                Bot.Tasks.MountTask.Mount(false);

                Point pTemps = ObjectManager.ObjectManager.Me.Position;

                var timerSit = new Helpful.Timer(2500);
                var jump_timer = new Helpful.Timer(0);

                while (Products.Products.IsStarted && ObjectManager.ObjectManager.Me.IsMounted &&
                       ObjectManager.ObjectManager.Me.Position.DistanceTo(point) > 3.5f && _used && _usedLoop)
                {
                    if (!MovementManager.IsUnStuck)
                    {
                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(point) > 15)
                        {
                            Point meTemps = ObjectManager.ObjectManager.Me.Position;
                            meTemps.Z = meTemps.Z - 2;

                            var temps = new Point(point.X, point.Y, ObjectManager.ObjectManager.Me.Position.Z - 2.5f);
                            if (point.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 100)
                            {
                                temps = Math.GetPostion2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position, point, 100);
                                temps.Z = ObjectManager.ObjectManager.Me.Position.Z - 2.5f;
                            }
                            if (TraceLine.TraceLineGo(meTemps, temps) ||
                                (ObjectManager.ObjectManager.Me.Position.Z + 10 < point.Z &&
                                 point.DistanceTo2D(ObjectManager.ObjectManager.Me.Position) < 100))
                            {
                                Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                                Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                                timerSit = new Helpful.Timer(1000);
                                // If distance to colission < 40 stop moveto
                                temps = new Point(point.X, point.Y, ObjectManager.ObjectManager.Me.Position.Z - 2.5f);
                                if (point.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 100)
                                {
                                    temps = Math.GetPostion2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position, point, 40);
                                    temps.Z = ObjectManager.ObjectManager.Me.Position.Z - 2.5f;
                                }
                                if (TraceLine.TraceLineGo(meTemps, temps))
                                {
                                    jump_timer = new Helpful.Timer(5000);
                                    while (!jump_timer.IsReady)
                                    {
                                        MovementManager.StopMoveTo(false);
                                        Thread.Sleep(1000);
                                    }
                                    MovementManager.StopMoveTo(true);
                                }
                                // End Stop move to
                                Thread.Sleep(1300);
                                if (pTemps.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 1f)
                                {
                                    MovementManager.UnStuckFly();
                                }
                                else
                                {
                                    pTemps = ObjectManager.ObjectManager.Me.Position;
                                }
                            }
                            else
                            {
                                Keybindings.UpKeybindings(Enums.Keybindings.JUMP);

                                if (timerSit.IsReady)
                                {
                                    // If distance to ground > 200
                                    temps = new Point(ObjectManager.ObjectManager.Me.Position.X,
                                                      ObjectManager.ObjectManager.Me.Position.Y,
                                                      ObjectManager.ObjectManager.Me.Position.Z - 100f);
                                    var tempsMe = new Point(ObjectManager.ObjectManager.Me.Position.X,
                                                      ObjectManager.ObjectManager.Me.Position.Y,
                                                      ObjectManager.ObjectManager.Me.Position.Z + 5f);
                                    var temps2 =
                                        Math.GetPostion2DOfLineByDistance(tempsMe, point,
                                                                          80);
                                    temps2.Z = ObjectManager.ObjectManager.Me.Position.Z - 100f;
                                    if (!TraceLine.TraceLineGo(tempsMe, temps))
                                        if (!TraceLine.TraceLineGo(tempsMe, temps2))
                                            Keybindings.DownKeybindings(Enums.Keybindings.SITORSTAND);
                                        else
                                        {
                                            timerSit = new Helpful.Timer(1000);
                                            Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                                        }
                                    else
                                    {
                                        timerSit = new Helpful.Timer(1000);
                                        Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                                    }
                                    // End Stop move ground
                                }
                                MovementManager.MoveTo(point.X, point.Y, ObjectManager.ObjectManager.Me.Position.Z);
                                if (Others.Times > (timer + 1500))
                                {
                                    MovementManager.MoveTo(point.X, point.Y, ObjectManager.ObjectManager.Me.Position.Z);
                                    timer = Others.Times;
                                }
                            }
                        }
                        else
                        {
                            Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                            Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                            MovementManager.MoveTo(point);
                        }
                    }

                    if (ObjectManager.ObjectManager.Me.IsMounted && !Usefuls.IsFlying)
                    {
                        Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                        Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                        Bot.Tasks.MountTask.Mount(false);
                        Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                        Thread.Sleep(1300);
                        Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                    }

                    Thread.Sleep(150);//50
                }
                Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                _used = false;
                _usedLoop = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveGo(Point point): " + exception);
                _used = false;
                _usedLoop = false;
            }
        }

        public static void StopLongMove()
        {
            try
            {
                Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                _used = false;
                _usedLoop = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > StopLongMove(): " + exception);
                _used = false;
                _usedLoop = false;
            }
        }
    }
}
