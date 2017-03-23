using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
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
        private static Thread _longMoveThread;

        public static bool IsLongMove
        {
            get { return _used || _usedLoop; }
        }

        public static void LongMoveByNewThread(Point point)
        {
            try
            {
                if (_longMoveThread != null && _longMoveThread.IsAlive && _pointLongMove.DistanceTo(point) < 0.0001f)
                    return;
                _pointLongMove = point;
                _longMoveThread = new Thread(LongMoveGo) {IsBackground = true, Name = "LongMove"};
                _longMoveThread.Start();
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
                MountTask.Mount(false, true);

                Point pTemps = ObjectManager.ObjectManager.Me.Position;

                Timer timerSit = new Timer(2500);
                bool flyingPathFinder = false;

                while (Products.Products.IsStarted && (ObjectManager.ObjectManager.Me.IsMounted || MountTask.GetMountCapacity() == MountCapacity.Feet) &&
                       ObjectManager.ObjectManager.Me.Position.DistanceTo(point) > 3.5f && _used && _usedLoop)
                {
                    bool forceGround = false;
                    if (Usefuls.IsFlying)
                    {
                        Point pos = ObjectManager.ObjectManager.Me.Position;
                        if (point.DistanceTo2D(pos) <= 60 || flyingPathFinder && point.DistanceTo2D(pos) <= 110)
                        {
                            Point p = new Point(ObjectManager.ObjectManager.Me.Position.LerpByDistance(point, 3f));
                            bool failed = false;
                            Point targetPoint = new Point();
                            if (p.IsValid && TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position, p, CGWorldFrameHitFlags.HitTestAllButLiquid))
                            {
                                if ((point.DistanceTo2D(pos) <= 60 || flyingPathFinder && point.DistanceTo2D(pos) <= 110) && pTemps.Z > point.Z)
                                {
                                    float degree = 0f;
                                    bool doContinue = false;
                                    while (degree < 360) //Search for safe rez point, if no safe point found, just rez and get killed again!
                                    {
                                        //Calculate position on a circle 15degrees at a time and check if we can go there
                                        float x = (float) (pos.X + 50f*System.Math.Cos(Math.DegreeToRadian(degree)));
                                        float y = (float) (pos.Y + 50f*System.Math.Sin(Math.DegreeToRadian(degree)));
                                        Point topPoint = new Point(x, y, pos.Z);
                                        Point bottomPoint = new Point(x, y, PathFinder.GetZPosition(x, y));
                                        if (!TraceLine.TraceLineGo(topPoint, new Point(topPoint.LerpByDistance(point, 3f)), CGWorldFrameHitFlags.HitTestAllButLiquid))
                                        {
                                            targetPoint = topPoint;
                                            MovementManager.MoveTo(targetPoint);
                                            Thread.Sleep(2500);
                                            flyingPathFinder = true;
                                            doContinue = true;
                                            break;
                                            // we want to go to topPoint and directly go to point.
                                        }
                                        else if (!TraceLine.TraceLineGo(topPoint, bottomPoint, CGWorldFrameHitFlags.HitTestAllButLiquid))
                                        {
                                            bool success;
                                            PathFinder.FindPath(bottomPoint, point, Usefuls.ContinentNameMpq, out success);
                                            if (success)
                                            {
                                                targetPoint = topPoint;
                                                MovementManager.MoveTo(targetPoint);
                                                Thread.Sleep(2500);
                                                flyingPathFinder = true;
                                                break;
                                                // we want to go to topPoint then dismount down to bottomPoint, then findPath to target.
                                            }
                                        }
                                        degree += 20;
                                        if (degree >= 360f)
                                            failed = true;
                                    }
                                    if (doContinue)
                                    {
                                        Thread.Sleep(1000);
                                        continue;
                                    }
                                }
                                if (!failed)
                                {
                                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(targetPoint) > 5f)
                                    {
                                        Thread.Sleep(2500);
                                    }
                                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(targetPoint) > 5f)
                                        continue;
                                    MountTask.DismountMount();
                                    _used = false;
                                    _usedLoop = false;
                                    return;
                                }
                            }
                            else
                            {
                                MovementManager.MoveTo(p);
                                flyingPathFinder = false;
                            }
                        }
                    }
                    if (MountTask.GetMountCapacity() <= MountCapacity.Ground || forceGround)
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
                            MountTask.Mount(false, true);
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