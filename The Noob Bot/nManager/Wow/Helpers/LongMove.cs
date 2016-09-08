using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using Math = nManager.Helpful.Math;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Helpers
{
    public class LongMove
    {
        private const float altitude = 150f;
        private static bool _used;
        private static bool _usedLoop;

        private static Point _pointLongMove = new Point();
        private static Timer RegenPath = new Timer(0);

        public static bool IsLongMove
        {
            get { return _used || _usedLoop; }
        }

        public static void LongMoveByNewThread(Point point)
        {
            try
            {
                _pointLongMove = point;
                Thread worker2 = new Thread(LongMoveGo) {IsBackground = true, Name = "LongMove"};
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
                MountTask.Mount(false);

                Point pTemps = ObjectManager.ObjectManager.Me.Position;

                Timer timerSit = new Timer(2500);

                while (Products.Products.IsStarted && (ObjectManager.ObjectManager.Me.IsMounted || MountTask.GetMountCapacity() == MountCapacity.Feet) &&
                       ObjectManager.ObjectManager.Me.Position.DistanceTo(point) > 3.5f && _used && _usedLoop)
                {
                    if (MountTask.GetMountCapacity() <= MountCapacity.Ground)
                    {
                        if (RegenPath.IsReady && ObjectManager.ObjectManager.Me.Position.DistanceTo(point) > 3.5f)
                        {
                            RegenPath = new Timer(1000*60);
                            List<Point> getFullPath = PathFinder.FindPath(point);
                            MovementManager.Go(getFullPath);
                            RegenPath.Reset();
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        if (!MovementManager.IsUnStuck)
                        {
                            const int checkInFront = 130; // was 100
                            const int checkCollision = 60; // was 40
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(point) > 15)
                            {
                                Point meTemps = ObjectManager.ObjectManager.Me.Position;
                                meTemps.Z = meTemps.Z - 2;

                                Point temps = new Point(point.X, point.Y, ObjectManager.ObjectManager.Me.Position.Z - 2.5f);
                                if (point.DistanceTo(ObjectManager.ObjectManager.Me.Position) > checkInFront)
                                {
                                    temps = Math.GetPosition2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position, point,
                                        checkInFront);
                                    temps.Z = ObjectManager.ObjectManager.Me.Position.Z - 2.5f;
                                }
                                if (TraceLine.TraceLineGo(meTemps, temps) ||
                                    (ObjectManager.ObjectManager.Me.Position.Z + 10 < point.Z &&
                                     point.DistanceTo2D(ObjectManager.ObjectManager.Me.Position) < 100))
                                {
                                    MovementsAction.Descend(false);
                                    MovementsAction.Ascend(true);
                                    timerSit = new Timer(1000);
                                    // If distance to colission < checkCollision stop moveto
                                    temps = new Point(point.X, point.Y, ObjectManager.ObjectManager.Me.Position.Z - 2.5f);
                                    if (point.DistanceTo(ObjectManager.ObjectManager.Me.Position) > checkInFront)
                                    {
                                        temps = Math.GetPosition2DOfLineByDistance(ObjectManager.ObjectManager.Me.Position,
                                            point, checkCollision);
                                        temps.Z = ObjectManager.ObjectManager.Me.Position.Z - 2.5f;
                                    }
                                    if (TraceLine.TraceLineGo(meTemps, temps))
                                    {
                                        MovementManager.StopMoveTo(false);
                                    }
                                    // End Stop move to
                                    Thread.Sleep(800);
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
                                    MovementsAction.Ascend(false);

                                    if (timerSit.IsReady)
                                    {
                                        // If distance to ground > 100
                                        temps = new Point(ObjectManager.ObjectManager.Me.Position.X,
                                            ObjectManager.ObjectManager.Me.Position.Y,
                                            ObjectManager.ObjectManager.Me.Position.Z - altitude);
                                        Point tempsMe = new Point(ObjectManager.ObjectManager.Me.Position.X,
                                            ObjectManager.ObjectManager.Me.Position.Y,
                                            ObjectManager.ObjectManager.Me.Position.Z + 5f);
                                        Point temps2 = Math.GetPosition2DOfLineByDistance(tempsMe, point, 80);
                                        temps2.Z = ObjectManager.ObjectManager.Me.Position.Z - altitude;
                                        if (!TraceLine.TraceLineGo(tempsMe, temps))
                                            if (!TraceLine.TraceLineGo(tempsMe, temps2))
                                                MovementsAction.Descend(true);
                                            else
                                            {
                                                timerSit = new Timer(1000);
                                                MovementsAction.Descend(false);
                                            }
                                        else
                                        {
                                            timerSit = new Timer(1000);
                                            MovementsAction.Descend(false);
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
                                MovementsAction.Descend(false);
                                MovementsAction.Ascend(false);
                                MovementManager.MoveTo(point);
                            }
                        }

                        if (ObjectManager.ObjectManager.Me.IsMounted && !Usefuls.IsFlying)
                        {
                            MovementsAction.Descend(false);
                            MovementsAction.Ascend(false);
                            MountTask.Mount(false);
                            MovementsAction.Ascend(true);
                            Thread.Sleep(1300);
                            MovementsAction.Ascend(false);
                        }
                    }
                    Thread.Sleep(150); //50
                }
                MovementsAction.Descend(false);
                MovementsAction.Ascend(false);
                _used = false;
                _usedLoop = false;
                RegenPath.ForceReady();
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveGo(Point point): " + exception);
                _used = false;
                _usedLoop = false;
                RegenPath.ForceReady();
            }
        }

        public static void StopLongMove()
        {
            try
            {
                MovementsAction.Ascend(false);
                MovementsAction.Descend(false);
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