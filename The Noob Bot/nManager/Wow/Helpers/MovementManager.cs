using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;
using CSharpMath = System.Math;
using Math = nManager.Helpful.Math;

namespace nManager.Wow.Helpers
{
    /// <summary>
    /// Management the player movement
    /// </summary>
    public static class MovementManager
    {
        #region MovementManager

        // Movement Manager:
        private static Thread _worker;
        private static bool _movement;
        private static bool _loop;
        private static List<Point> _points = new List<Point>();

        private static bool _first;
        private static int _lastNbStuck;
        private static bool _farm;
        private static Point _jumpOverAttempt = new Point();
        private static Point _distmountAttempt = new Point();

        private static int _currentTargetedPoint;
        // let's remember where we are instead of searching point and doing mess

        private static List<Point> _pointsOrigine = new List<Point>();

        /// <summary>
        /// Return true if the player is in unstuck
        /// </summary>
        public static bool IsUnStuck { get; private set; }

        /// <summary>
        /// Get number of stuck
        /// </summary>
        public static int StuckCount;

        /// <summary>
        /// Return true if the player is currently in move (Go() or GoLoop()).
        /// </summary>
        /// <value>
        ///   <c>true</c> if [in movement]; otherwise, <c>false</c>.
        /// </value>
        public static bool InMovement
        {
            get
            {
                try
                {
                    return _movement;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("InMovement: " + exception);
                }
                return false;
            }
        }

        public static int PointId
        {
            get
            {
                try
                {
                    return _currentTargetedPoint;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PointId: " + exception);
                }
                return 0;
            }
            set
            {
                try
                {
                    _currentTargetedPoint = value;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PointId: " + exception);
                    _currentTargetedPoint = 0;
                }
            }
        }

        public static List<Point> CurrentPath
        {
            get
            {
                try
                {
                    return _points;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PointId: " + exception);
                }
                return null;
            }
        }

        public static void FlyingToGroundProfilesConverter(List<Point> inputPoints, out List<Point> outputPoints, out bool conversionStatus)
        {
            try
            {
                Logging.Write(
                    "Starting the conversion process. Keep in mind that the best profiles are handmade profiles.");
                var tempPoints = new List<Point>();
                outputPoints = new List<Point>();
                uint failCounter = 0;
                foreach (var inputPoint in inputPoints)
                {
                    var curr = inputPoint.Type.ToLower() == "flying" ? PathFinder.GetZPosition(inputPoint) : inputPoint.Z;
                    if (curr == 0)
                    {
                        failCounter++;
                        continue;
                    }
                    if (tempPoints.Any())
                    {
                        bool resultSuccess;
                        tempPoints = PathFinder.FindPath(tempPoints[tempPoints.Count() - 1], new Point(inputPoint.X, inputPoint.Y, curr), Usefuls.ContinentNameMpq,
                                                         out resultSuccess);
                        if (!resultSuccess)
                        {
                            failCounter++;
                            continue;
                        }
                    }
                    else
                    {
                        tempPoints.Add(new Point(inputPoint.X, inputPoint.Y, curr));
                    }
                    outputPoints.AddRange(tempPoints);
                }
                decimal failPercent = failCounter/inputPoints.Count*100;
                if (failPercent > 0)
                {
                    failPercent = System.Math.Round(failPercent, 0);
                    if (failPercent == 0)
                        failPercent++;
                }
                else failPercent = 0;

                if (failPercent > 60)
                {
                    outputPoints = inputPoints;
                    conversionStatus = false;
                    Logging.WriteDebug("The conversion has failed, " + failPercent + "% of the points have not been converted.");
                    return;
                }
                conversionStatus = true;
                Logging.Write("The conversion has succeeded.");
                Logging.WriteDebug("Conversion stats:");
                Logging.WriteDebug(100 - failPercent + "% of the points have been succesfully converted into " + inputPoints.Count + " grounds points.");
            }
            catch (Exception exception)
            {
                Logging.WriteError("FlyingToGroundProfilesConverter(List<Point> inputPoints, out List<Point> outputPoints, out bool conversionStatus): " + exception);
                Logging.Write("The conversion has failed...");
                outputPoints = inputPoints;
                conversionStatus = false;
            }
        }

        public static void LaunchThreadMovementManager()
        {
            try
            {
                lock (typeof (MovementManager))
                {
                    if (_worker == null)
                    {
                        _worker = new Thread(ThreadMovementManager) {IsBackground = true, Name = "MovementManager"};
                        _worker.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LaunchThreadMovementManager(): " + exception);
            }
        }

        private static void ThreadMovementManager()
        {
            try
            {
                while (true)
                {
                    lock (typeof (MovementManager))
                    {
                        while ((_loop || _first) && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                        {
                            if (Statistics.OffsetStats == 0xB5)
                            {
                                _first = false;

                                if (_movement && _points.Count > 0)
                                {
                                    int targetPoint = 0;
                                    if (_loop)
                                        targetPoint = _currentTargetedPoint;
                                    if (
                                        _points[targetPoint].DistanceTo2D(
                                            ObjectManager.ObjectManager.Me.Position) < 0.5f)
                                    {
                                        targetPoint++;
                                    }
                                    if (targetPoint >= _points.Count - 1)
                                    {
                                        targetPoint = 0;
                                    }
                                    if (_loop)
                                        _currentTargetedPoint = targetPoint;
                                    if (_points[targetPoint].Type.ToLower() == "flying")
                                        FlyMovementManager(targetPoint);
                                    else if (_points[targetPoint].Type.ToLower() == "swimming")
                                        AquaticMovementManager(targetPoint);
                                    else
                                        GroundMovementManager(targetPoint);

                                    Statistics.OffsetStats = 0x53;
                                }
                            }
                            Thread.Sleep(80);
                        }
                        if (!_loop && !_first)
                            _movement = false;
                    }
                    Thread.Sleep(150);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ThreadMovementManager()#3: " + exception);
            }
        }

        private static void GroundMovementManager(int firstIdPoint)
        {
            try
            {
                if (_movement && _points.Count > 0)
                {
                    if (_points[firstIdPoint].Type.ToLower() == "swimming")
                    {
                        return;
                    }

                    if (_loop && _points[firstIdPoint].DistanceTo2D(ObjectManager.ObjectManager.Me.Position) >= 200 &&
                        !String.IsNullOrWhiteSpace(nManagerSetting.CurrentSetting.FlyingMountName))
                    {
                        Logging.WriteNavigator("Long Move distance: " +
                                               ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[firstIdPoint]));
                        LongMove.LongMoveByNewThread(_points[firstIdPoint]);
                        Thread.Sleep(100);
                        while (LongMove.IsLongMove && _movement)
                        {
                            Thread.Sleep(50);
                        }
                        LongMove.StopLongMove();

                        if (!_movement)
                            return;
                    }

                    if (_loop ||
                        Math.DistanceListPoint(_points) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount)
                    {
                        if (nManagerSetting.CurrentSetting.UseGroundMount)
                            Bot.Tasks.MountTask.MountingGroundMount(false);
                        else
                            Bot.Tasks.MountTask.Mount(false);
                        if (Usefuls.IsFlying)
                        {
                            List<Point> tmpList = new List<Point>();
                            for (var i = 0; i < _points.Count; i++)
                            {
                                Point pt = new Point(_points[i].X, _points[i].Y, _points[i].Z + 2.0f, "flying");
                                tmpList.Add(pt);
                            }
                            _points = tmpList;
                        }
                    }
                    _lastNbStuck = StuckCount;
                    int idPoint = firstIdPoint;

                    while (ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(10);
                    }

                    MoveTo(_points[idPoint]);

                    bool end = false;
                    while ((_movement && !end) && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                    {
                        try
                        {
                            if (_points[idPoint].Type.ToLower() == "swimming")
                            {
                                return;
                            }
                            // GoTo next Point
                            if ((((ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_points[idPoint]) <= 3.0f &&
                                   ObjectManager.ObjectManager.Me.IsMounted) ||
                                  ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_points[idPoint]) <= 3.0f) &&
                                 ObjectManager.ObjectManager.Me.Position.DistanceZ(_points[idPoint]) <= 10.5f) &&
                                _movement)
                            {
                                idPoint++;
                                if (idPoint > _points.Count - 1)
                                {
                                    idPoint = _points.Count - 1;
                                    end = true;
                                    if (_loop)
                                    {
                                        end = false;
                                        _points = new List<Point>();
                                        _points.AddRange(_pointsOrigine);
                                        idPoint = 0;
                                    }
                                }
                            }

                            // Generate new path
                            if (_lastMoveToResult == false || _lastNbStuck != StuckCount)
                            {
                                try
                                {
                                    _lastNbStuck = StuckCount;

                                    StopMoveTo();
                                    _points = PathFinder.FindPath(_pointsOrigine[_pointsOrigine.Count - 1]);
                                    idPoint = 0;
                                }
                                catch (Exception exception)
                                {
                                    Logging.WriteError("ThreadMovementManager()#1: " + exception);
                                }
                                _lastMoveToResult = true;
                            }

                            // Move to point
                            if (_loop)
                                _currentTargetedPoint = idPoint;
                            MoveTo(_points[idPoint]);

                            Thread.Sleep(50);

                            int rJump = Others.Random(1, 5000);
                            if (rJump == 5)
                                MovementsAction.Jump();
                        }
                        catch (Exception exception)
                        {
                            Logging.WriteError("ThreadMovementManager()#2: " + exception);
                            idPoint = Math.NearestPointOfListPoints(_points, ObjectManager.ObjectManager.Me.Position);
                        }
                    }
                    if (!_loop)
                        StopMove();
                    if (!ObjectManager.ObjectManager.Me.IsCast)
                        StopMoveTo();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MovementManager > GroundMouvementManager(int firstIdPoint): " + ex);
                StopMove();
            }
        }

        private static void FlyMovementManager(int firstIdPoint)
        {
            try
            {
                if (_movement && _points.Count > 0)
                {
                    Bot.Tasks.MountTask.Mount(false);
                    if (!_movement)
                        return;
                    int idPoint = firstIdPoint;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_points[idPoint]) > 200 ||
                        _points.Count == 1)
                    {
                        Logging.WriteNavigator("Long Move distance: " +
                                               ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[idPoint]));
                        LongMove.LongMoveByNewThread(_points[idPoint]);
                        Thread.Sleep(100);
                        while (LongMove.IsLongMove && _movement)
                        {
                            Thread.Sleep(50);
                        }
                        LongMove.StopLongMove();
                    }

                    bool end = false;
                    while ((_movement && !end) && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                    {
                        if (_points[idPoint].Type.ToLower() != "flying")
                        {
                            return;
                        }
                        if (!ObjectManager.ObjectManager.Me.IsMounted)
                            return;
                        while (!Usefuls.IsFlying && ObjectManager.ObjectManager.Me.IsMounted)
                        {
                            MovementsAction.Ascend(true);
                            Thread.Sleep(300);
                            MovementsAction.Ascend(false);
                        }

                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[idPoint]) < 15 && _movement)
                        {
                            idPoint++;
                            if (idPoint > _points.Count - 1)
                            {
                                idPoint = _points.Count - 1;
                                end = true;
                                if (_loop)
                                {
                                    end = false;
                                    _points = new List<Point>();
                                    _points.AddRange(_pointsOrigine);
                                    idPoint = 0;
                                }
                            }
                            if (_loop)
                                _currentTargetedPoint = idPoint;
                            MoveTo(_points[idPoint]);
                        }
                        MoveTo(_points[idPoint]);

                        Thread.Sleep(70);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MovementManager > FlyMouvementManager(int firstIdPoint): " + ex);
                StopMove();
            }
        }

        private static void AquaticMovementManager(int firstIdPoint)
        {
            try
            {
                if (_movement && _points.Count > 0)
                {
                    Bot.Tasks.MountTask.Mount(false);
                    if (!_movement)
                        return;
                    int idPoint = firstIdPoint;
                    if (_points[idPoint].DistanceTo(ObjectManager.ObjectManager.Me.Position) > 30)
                    {
                        bool result;
                        var path = PathFinder.FindPath(_points[idPoint], out result);
                        if (result)
                        {
                            for (int i = 0; i <= path.Count - 1; i++)
                            {
                                path[i].Type = "Swimming";
                            }
                            _points = path;
                            _loop = false;
                            idPoint = 0;
                        }
                    }

                    bool end = false;
                    while ((_movement && !end) && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                    {
                        if (_points[idPoint].Type.ToLower() != "swimming")
                        {
                            return;
                        }
                        while (!Usefuls.IsSwimming && !Usefuls.IsFlying && ObjectManager.ObjectManager.Me.IsMounted)
                        {
                            MovementsAction.Ascend(true);
                            Thread.Sleep(300);
                            MovementsAction.Ascend(false);
                        }

                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[idPoint]) < 7 && _movement)
                        {
                            idPoint++;
                            if (idPoint > _points.Count - 1)
                            {
                                idPoint = _points.Count - 1;
                                end = true;
                                if (_loop)
                                {
                                    end = false;
                                    _points = new List<Point>();
                                    _points.AddRange(_pointsOrigine);
                                    idPoint = 0;
                                }
                            }
                            _currentTargetedPoint = idPoint;
                            MoveTo(_points[idPoint]);
                        }
                        MoveTo(_points[idPoint]);

                        Thread.Sleep(70);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MovementManager > AquaticMouvementManager(int firstIdPoint): " + ex);
                StopMove();
            }
        }

        /// <summary>
        /// UnStuck the player.
        /// </summary>
        public static void UnStuck()
        {
            try
            {
                IsUnStuck = true;
                Logging.WriteDebug("UnStuck() started.");
                MovementsAction.Ascend(false);
                MovementsAction.Descend(false);
                Logging.WriteDebug("Jump / Down released.");
                if (ObjectManager.ObjectManager.Me.IsMounted)
                {
                    Logging.WriteDebug("UnStuck - We are currently mounted.");
                    MovementsAction.Ascend(true);
                    Thread.Sleep(Others.Random(500, 1000));
                    MovementsAction.Ascend(false);
                    Logging.WriteDebug("UnStuck - Jump attempt done.");

                    // if fly mode:
                    if (Usefuls.IsFlying)
                    {
                        Logging.WriteDebug("UnStuck - We are currently Flying.");
                        UnStuckFly();
                        IsUnStuck = false;
                        StuckCount++;
                        Logging.WriteDebug("UnStuck - StuckCount updated, new value: " + StuckCount + ".");
                        return;
                    }
                    Logging.WriteDebug("UnStuck - We are currently mounted.");
                    MovementsAction.Ascend(true);
                    Thread.Sleep(Others.Random(500, 1000));
                    MovementsAction.Ascend(false);
                    Logging.WriteDebug("UnStuck - Jump attempt done.");
                }
                Statistics.Stucks++;
                Logging.WriteNavigator("UnStuck - Non-flying UnStuck in progress.");

                // Simply try to jump over or dismount if needed
                if (_distmountAttempt.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 3)
                {
                    float dx = 1.5f*(float) CSharpMath.Cos(ObjectManager.ObjectManager.Me.Rotation);
                    float dy = 1.5f*(float) CSharpMath.Sin(ObjectManager.ObjectManager.Me.Rotation);
                    Point inFront = new Point(ObjectManager.ObjectManager.Me.Position.X + dx, ObjectManager.ObjectManager.Me.Position.Y + dy,
                                              ObjectManager.ObjectManager.Me.Position.Z + 0.8f);
                    _distmountAttempt = new Point(ObjectManager.ObjectManager.Me.Position.X, ObjectManager.ObjectManager.Me.Position.Y,
                                                  ObjectManager.ObjectManager.Me.Position.Z + 0.8f);
                    if (ObjectManager.ObjectManager.Me.IsMounted && !nManager.Wow.Helpers.TraceLine.TraceLineGo(_distmountAttempt, inFront, CGWorldFrameHitFlags.HitTestAll))
                    {
                        Logging.WriteNavigator("UnStuck - Dismounting.");
                        Usefuls.DisMount();
                        IsUnStuck = false;
                        StuckCount++;
                        return;
                    }
                    _jumpOverAttempt = new Point(ObjectManager.ObjectManager.Me.Position.X, ObjectManager.ObjectManager.Me.Position.Y,
                                                 ObjectManager.ObjectManager.Me.Position.Z + 1.5f);
                    inFront = new Point(ObjectManager.ObjectManager.Me.Position.X + dx, ObjectManager.ObjectManager.Me.Position.Y + dy,
                                        ObjectManager.ObjectManager.Me.Position.Z + 1.5f);
                    if (!nManager.Wow.Helpers.TraceLine.TraceLineGo(_jumpOverAttempt, inFront, CGWorldFrameHitFlags.HitTestAll))
                    {
                        Logging.WriteNavigator("UnStuck - Jumping over.");
                        MovementsAction.Ascend(true);
                        MovementsAction.MoveForward(true);
                        Thread.Sleep(75);
                        MovementsAction.Ascend(false);
                        MovementsAction.MoveForward(false);
                        Thread.Sleep(350);
                        MovementsAction.Ascend(true);
                        MovementsAction.MoveForward(true);
                        Thread.Sleep(75);
                        MovementsAction.Ascend(false);
                        MovementsAction.MoveForward(false);
                        IsUnStuck = false;
                        StuckCount++;
                        return;
                    }
                }

                var lastPost = new Point(ObjectManager.ObjectManager.Me.Position);
                Logging.WriteDebug("UnStuck - lastPost = " + lastPost);
                for (int i = 0; i < 8; i++)
                {
                    Logging.WriteDebug("UnStuck - UnStuck attempt " + i + " started.");
                    int j = Others.Random(1, 8);

                    float disX = System.Math.Abs(lastPost.X - ObjectManager.ObjectManager.Me.Position.X);
                    float disY = System.Math.Abs(lastPost.Y - ObjectManager.ObjectManager.Me.Position.Y);
                    Logging.WriteDebug("UnStuck - disX = " + disX);
                    Logging.WriteDebug("UnStuck - disY = " + disY);

                    if (disX < 2.5f || disY < 2.5f)
                        //we likely don't need to run this if we are 5+ units from our stuck point
                    {
                        if (j == 1)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 10,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int) ClickToMoveType.Move,
                                                                0.5f); // try 'backward'
                            Logging.WriteDebug("UnStuck - Backward done.");
                        }
                        else if (j == 2)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int) ClickToMoveType.Move,
                                                                0.5f); // try 'back left'
                            Logging.WriteDebug("UnStuck - Backward Left done.");
                        }
                        else if (j == 3)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 10,
                                                                ObjectManager.ObjectManager.Me.Position.Y,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int) ClickToMoveType.Move, 0.5f); // try 'left'
                            Logging.WriteDebug("UnStuck - Left done.");
                        }
                        else if (j == 4)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int) ClickToMoveType.Move,
                                                                0.5f); // try 'forward left'
                            Logging.WriteDebug("UnStuck - Forward Left done.");
                        }
                        else if (j == 5)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 10,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int) ClickToMoveType.Move,
                                                                0.5f); // try 'forward'
                            Logging.WriteDebug("UnStuck - Forward done.");
                        }
                        else if (j == 6)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 14,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int) ClickToMoveType.Move,
                                                                0.5f); // try 'forward right'
                            Logging.WriteDebug("UnStuck - Forward Right done.");
                        }
                        else if (j == 7)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 10,
                                                                ObjectManager.ObjectManager.Me.Position.Y,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int) ClickToMoveType.Move, 0.5f); // try 'right'
                            Logging.WriteDebug("UnStuck - Right done.");
                        }
                        else if (j == 8)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 14,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int) ClickToMoveType.Move,
                                                                0.5f); // try 'back right'
                            Logging.WriteDebug("UnStuck - Backward Right done.");
                        }

                        int k = Others.Random(1, 3);
                        if (k == 2)
                        {
                            MovementsAction.Ascend(true);
                            Thread.Sleep(Others.Random(500, 1000));
                            MovementsAction.Ascend(false);
                            Logging.WriteDebug("UnStuck - k = 2, Jump attempt done.");
                        }

                        Thread.Sleep(Others.Random(700, 3000));
                        Logging.WriteDebug("UnStuck - UnStuck attempt " + i + " finished.");
                    }
                    else
                    {
                        Logging.WriteDebug("UnStuck - UnStuck attempt " + i + " finished by breaking.");
                        break;
                    }
                }

                IsUnStuck = false;
                StuckCount++;
                Logging.WriteDebug("UnStuck - StuckCount updated, new value: " + StuckCount + ".");
                Logging.WriteDebug("UnStuck() done.");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UnStuck(): " + exception);
            }
        }

        /// <summary>
        /// UnStuck the player if fly.
        /// </summary>
        public static void UnStuckFly()
        {
            try
            {
                Logging.WriteDebug("UnStuckFly() started.");
                Statistics.Stucks++;
                Logging.WriteDebug("Flying UnStuck - Statistics.Stucks updated, new value: " + Statistics.Stucks + ".");
                Logging.WriteNavigator("UnStuck character > flying mode.");
                if (ClickToMove.GetClickToMoveTypePush() != ClickToMoveType.None)
                {
                    ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                        ObjectManager.ObjectManager.Me.Position.Y,
                                                        ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                        (int) ClickToMoveType.Move, 0.5f);
                    Logging.WriteDebug("Flying UnStuck - Reset position to our current location and StopMove.");
                    // Reset position to our current location and StopMove.
                    StopMove();
                }
                MovementsAction.Ascend(true);
                Thread.Sleep(Others.Random(500, 1000));
                MovementsAction.Ascend(false);
                Logging.WriteDebug("Flying UnStuck - Jump attempt done.");

                var lastPost = new Point(ObjectManager.ObjectManager.Me.Position);
                Logging.WriteDebug("Flying UnStuck - lastPost = " + lastPost);
                var iR = Others.Random(2, 3);
                for (int i = 0; i < iR; i++)
                {
                    Logging.WriteDebug("Flying UnStuck - UnStuck attempt " + i + "started.");
                    int j = Others.Random(1, 8);
                    int z = Others.Random(-15, 15);

                    float disX = System.Math.Abs(lastPost.X - ObjectManager.ObjectManager.Me.Position.X);
                    float disY = System.Math.Abs(lastPost.Y - ObjectManager.ObjectManager.Me.Position.Y);
                    Logging.WriteDebug("Flying UnStuck - disX = " + disX);
                    Logging.WriteDebug("Flying UnStuck - disY = " + disY);
                    if (disX < 5 || disY < 5)
                    {
                        if (j == 1)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 15,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int) ClickToMoveType.Move, 0.5f); // try 'backward'
                            Logging.WriteDebug("Flying UnStuck - Backward done.");
                        }
                        else if (j == 2)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 17,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 17,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int) ClickToMoveType.Move, 0.5f); // try 'back left'
                            Logging.WriteDebug("Flying UnStuck - Backward Left done.");
                        }
                        else if (j == 3)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 15,
                                                                ObjectManager.ObjectManager.Me.Position.Y,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z,
                                                                0, (int) ClickToMoveType.Move, 0.5f); // try 'left'
                            Logging.WriteDebug("Flying UnStuck - Left done.");
                        }
                        else if (j == 4)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 17,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 17,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int) ClickToMoveType.Move, 0.5f); // try 'forward left'
                            Logging.WriteDebug("Flying UnStuck - Forward Left done.");
                        }
                        else if (j == 5)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 15,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int) ClickToMoveType.Move, 0.5f); // try 'forward'
                            Logging.WriteDebug("Flying UnStuck - Forward done.");
                        }
                        else if (j == 6)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 15,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 17,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int) ClickToMoveType.Move, 0.5f);
                            // try 'forward right'
                            Logging.WriteDebug("Flying UnStuck - Forward Right done.");
                        }
                        else if (j == 7)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 15,
                                                                ObjectManager.ObjectManager.Me.Position.Y,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z,
                                                                0, (int) ClickToMoveType.Move, 0.5f); // try 'right'
                            Logging.WriteDebug("Flying UnStuck - Right done.");
                        }
                        else if (j == 8)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 17,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int) ClickToMoveType.Move, 0.5f); // try 'back right'
                            Logging.WriteDebug("Flying UnStuck - Backward Right done.");
                        }
                        Thread.Sleep(100);
                        var tus = new Timer(3000);
                        while (ClickToMove.GetClickToMoveTypePush() == ClickToMoveType.Move && !tus.IsReady)
                            Thread.Sleep(50);
                        if (ClickToMove.GetClickToMoveTypePush() != ClickToMoveType.None)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                                ObjectManager.ObjectManager.Me.Position.Y,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int) ClickToMoveType.Move, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Reset position to our current location and StopMove.");
                            // Reset position to our current location and StopMove.
                            StopMove();
                        }

                        if (i == 7)
                        {
                            int k = Others.Random(1, 4);
                            if (k == 2)
                            {
                                MovementsAction.Ascend(true);
                                Thread.Sleep(Others.Random(500, 1000));
                                MovementsAction.Ascend(false);
                                Logging.WriteDebug("Flying UnStuck - Jump attempt done.");
                            }
                            if (k == 3)
                            {
                                MovementsAction.Descend(true);
                                Thread.Sleep(Others.Random(500, 1000));
                                MovementsAction.Descend(false);
                                Logging.WriteDebug("Flying UnStuck - Down attempt done.");
                            }
                        }
                        Logging.WriteDebug("Flying UnStuck - UnStuck attempt " + i + " finished.");
                    }
                    else
                    {
                        Logging.WriteDebug("Flying UnStuck - UnStuck attempt " + i + " finished by direct breaking.");
                        break;
                    }
                }
                Logging.WriteDebug("UnStuckFly() done.");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UnStuckFly(): " + exception);
            }
        }

        /// <summary>
        /// Go to with this list of points.
        /// </summary>
        /// <param name="points">The _points.</param>
        public static void Go(List<Point> points)
        {
            try
            {
                if (Math.DistanceListPoint(_points) != Math.DistanceListPoint(points))
                {
                    _loop = false;
                    _movement = false;
                    lock (typeof (MovementManager))
                    {
                        if (_worker == null)
                        {
                            _currentTargetedPoint = 0;
                            LaunchThreadMovementManager();
                        }
                        _pointsOrigine = new List<Point>();
                        _pointsOrigine.AddRange(points);
                        _points = new List<Point>();
                        _points.AddRange(points);

                        _movement = true;
                        _first = true;
                    }
                }
                else if (_loop || !_movement || !_first)
                {
                    _first = true;
                    _loop = false;
                    _movement = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Go(List<Point> points): " + exception);
            }
        }

        /// <summary>
        /// move with this list of point  loop.
        /// </summary>
        /// <param name="points">The points.</param>
        public static void GoLoop(List<Point> points)
        {
            try
            {
                // We are returning here. We had a current point, then we return to it, or the current point is the nearest one if we just start the GoLoop on a new points list
                if (Math.DistanceListPoint(_points) != Math.DistanceListPoint(points))
                {
                    _loop = false;
                    _movement = false;
                    lock (typeof (MovementManager))
                    {
                        if (_worker == null)
                            LaunchThreadMovementManager();

                        _pointsOrigine = new List<Point>();
                        _pointsOrigine.AddRange(points);
                        _points = new List<Point>();
                        _points.AddRange(points);
                        _currentTargetedPoint = Math.NearestPointOfListPoints(_points,
                                                                              ObjectManager.ObjectManager.Me.Position);
                        _loop = true;
                        _movement = true;
                    }
                }
                else if (!_loop || !_movement)
                {
                    _loop = true;
                    _movement = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GoLoop(List<Point> points): " + exception);
            }
        }

        /// <summary>
        /// Stops the Go() and GoLoop().
        /// </summary>
        public static void StopMove()
        {
            try
            {
                _loop = false;
                _movement = false;
                lock (typeof (MovementManager))
                {
                    StopMoveTo();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("StopMove(): " + exception);
            }
        }

        #endregion MovementManager

        #region MoveTo

        // Move To:
        private static Thread _workerMoveTo;
        private static Point _pointTo = new Point();
        private static bool _loopMoveTo;
        private static bool _lastMoveToResult = true;

        /// <summary>
        /// Get if player use MoveTo().
        /// </summary>
        /// <value>
        ///   <c>true</c> if [the player is in MoveTo()]; otherwise, <c>false</c>.
        /// </value>
        public static bool InMoveTo
        {
            get
            {
                try
                {
                    return _loopMoveTo;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("InMoveTo: " + exception);
                }
                return false;
            }
        }

        private static void LaunchThreadMoveTo()
        {
            try
            {
                if (_workerMoveTo != null)
                {
                    try
                    {
                        _workerMoveTo.Interrupt();
                        _workerMoveTo.Abort();
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("LaunchThreadMoveTo()#1: " + exception);
                        _workerMoveTo = null;
                    }
                }
                _workerMoveTo = new Thread(ThreadMoveTo) {IsBackground = true, Name = "MoveTo"};
                _workerMoveTo.Start();
            }
            catch (Exception exception)
            {
                Logging.WriteError("LaunchThreadMoveTo()#2: " + exception);
                _loopMoveTo = false;
            }
        }

        private static void ThreadMoveTo()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (_loopMoveTo)
                        {
                            _lastMoveToResult = MoveToLocation(_pointTo);

                            _loopMoveTo = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("ThreadMoveTo()#1: " + exception);
                        _loopMoveTo = false;
                    }
                    Thread.Sleep(80); // 50
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ThreadMoveTo()#2: " + exception);
            }
        }

        private static bool MoveToLocation(Point position)
        {
            try
            {
                while (ObjectManager.ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(100);
                }

                var timer = new Timer(1*1000*2);
                var timerWaypoint = new Timer(1*1000*(30/3));
                double distance = (double) position.DistanceTo(ObjectManager.ObjectManager.Me.Position) - 1;
                if (distance > 45)
                    timerWaypoint = new Timer(1*1000*(distance/3));

                if (distance < 45 && distance > 40)
                    timerWaypoint = new Timer(1*1000*(45/3));

                if (distance < 40 && distance > 35)
                    timerWaypoint = new Timer(1*1000*(40/3));

                if (distance < 35 && distance > 30)
                    timerWaypoint = new Timer(1*1000*(35/3));

                Point oldPos = ObjectManager.ObjectManager.Me.Position;
                bool alerted = false;

                while (!timerWaypoint.IsReady && Usefuls.InGame)
                {
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(position) <= 0.6)
                        return true;

                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(position) <= 3.0 &&
                        ObjectManager.ObjectManager.Me.Position.DistanceZ(position) < 6 && _farm)
                        return true;

                    Point posP = ObjectManager.ObjectManager.Me.Position;

                    while (ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(100);
                    }
                    if (ClickToMove.GetClickToMovePosition().DistanceTo(position) > 1 ||
                        ClickToMove.GetClickToMoveTypePush() != ClickToMoveType.Move)
                        ClickToMove.CGPlayer_C__ClickToMove(position.X, position.Y, position.Z, 0,
                                                            (int) ClickToMoveType.Move, 0.5f);

                    if (!_loopMoveTo || _pointTo.DistanceTo(position) > 0.5f)
                        break;

                    if (posP.DistanceTo(oldPos) > 2)
                    {
                        oldPos = ObjectManager.ObjectManager.Me.Position;
                        timer.Reset();
                    }
                    if (!_loopMoveTo || _pointTo.DistanceTo(position) > 0.5f)
                        break;

                    if (posP.DistanceTo(oldPos) < 2 && timer.IsReady)
                    {
                        if (alerted)
                        {
                            UnStuck();
                            break;
                        }
                        Logging.WriteNavigator("Think we are stuck");
                        Logging.WriteNavigator("Trying something funny, hang on");
                        alerted = true;
                        UnStuck();
                        timer.Reset();
                        //timerWaypoint.Reset();
                    }
                    if (!_loopMoveTo || _pointTo.DistanceTo(position) > 0.5f)
                        break;

                    Thread.Sleep(35);
                }

                if (_loopMoveTo && _pointTo.DistanceTo(position) < 0.5f)
                {
                    Logging.WriteNavigator("Waypoint timed out");
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveToLocation(Point position): " + exception);
                return false;
            }
        }

        /// <summary>
        /// Moves to Position.
        /// </summary>
        /// <param name="point">The point.</param>
        public static void MoveTo(Point point, bool Farm = false)
        {
            try
            {
                if (point.X != 0 && point.Y != 0 && point.Z != 0)
                {
                    if (_workerMoveTo == null)
                        LaunchThreadMoveTo();

                    _farm = Farm;
                    _pointTo = new Point(point.X, point.Y, point.Z);
                    _loopMoveTo = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveTo(Point point): " + exception);
            }
        }

        /// <summary>
        /// Moves to Position.
        /// </summary>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        /// <param name="z">Z.</param>
        public static void MoveTo(float x, float y, float z, bool Farm = false)
        {
            try
            {
                MoveTo(new Point(x, y, z), Farm);
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveTo(float x, float y, float z): " + exception);
            }
        }

        /// <summary>
        /// Moves to.
        /// </summary>
        /// <param name="obj">The Unit.</param>
        public static void MoveTo(WoWUnit obj)
        {
            try
            {
                MoveTo(obj.Position);
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveTo(WoWUnit obj): " + exception);
            }
        }

        /// <summary>
        /// Moves to.
        /// </summary>
        /// <param name="obj">The GameObject.</param>
        public static void MoveTo(WoWGameObject obj, bool Farm = false)
        {
            try
            {
                MoveTo(obj.Position, Farm);
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveTo(WoWGameObject obj): " + exception);
            }
        }

        /// <summary>
        /// Stop Player.
        /// </summary>
        public static void StopMoveTo(bool stabilizeZAxis = true, bool stabilizeSides = false)
        {
            try
            {
                _loopMoveTo = false;
                if (ObjectManager.ObjectManager.Me.GetMove)
                {
                    if (stabilizeZAxis)
                    {
                        MovementsAction.Ascend(false);
                        MovementsAction.Descend(false);
                    }
                    MovementsAction.MoveForward(true, true);
                    MovementsAction.MoveBackward(true, true);
                    if (stabilizeSides)
                    {
                        MovementsAction.StrafeLeft(true, true);
                        MovementsAction.StrafeRight(true, true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MovementManager > StopMoveTo(): " + exception);
            }
        }

        #endregion MoveTo

        #region Facing code

        /// <summary>
        /// Faces the Unit.
        /// </summary>
        /// <param name="obj">The Unit.</param>
        public static void Face(WoWUnit obj)
        {
            try
            {
                Point localPlayerPosition = ObjectManager.ObjectManager.Me.Position;
                float wowFacing =
                    NegativeAngle(
                        (float)
                        System.Math.Atan2((obj.Position.Y - localPlayerPosition.Y),
                                          (obj.Position.X - localPlayerPosition.X)));
                float dif = wowFacing - ObjectManager.ObjectManager.Me.Rotation;
                if (dif < 0)
                    dif = -dif;
                if (dif <= 0.10f)
                    return;

                ClickToMove.CGPlayer_C__ClickToMove(obj.Position.X, obj.Position.Y, obj.Position.Z, obj.Guid,
                                                    (Int32) ClickToMoveType.FaceTarget, 0.5f);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Face(WoWUnit obj): " + exception);
            }
        }

        /// <summary>
        /// Faces the specified Player.
        /// </summary>
        /// <param name="obj">The Player.</param>
        public static void Face(WoWPlayer obj)
        {
            try
            {
                Point localPlayerPosition = ObjectManager.ObjectManager.Me.Position;
                float wowFacing =
                    NegativeAngle(
                        (float)
                        System.Math.Atan2((obj.Position.Y - localPlayerPosition.Y),
                                          (obj.Position.X - localPlayerPosition.X)));
                float dif = wowFacing - ObjectManager.ObjectManager.Me.Rotation;
                if (dif < 0)
                    dif = -dif;
                if (dif <= 0.10f)
                    return;

                ClickToMove.CGPlayer_C__ClickToMove(obj.Position.X, obj.Position.Y, obj.Position.Z, obj.Guid,
                                                    (Int32) ClickToMoveType.FaceTarget, 0.5f);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Face(WoWPlayer obj): " + exception);
            }
        }

        /// <summary>
        /// Faces the specified GameObject.
        /// </summary>
        /// <param name="obj">The GameObject.</param>
        public static void Face(WoWGameObject obj)
        {
            try
            {
                Point localPlayerPosition = ObjectManager.ObjectManager.Me.Position;
                float wowFacing =
                    NegativeAngle(
                        (float)
                        System.Math.Atan2((obj.Position.Y - localPlayerPosition.Y),
                                          (obj.Position.X - localPlayerPosition.X)));
                float dif = wowFacing - ObjectManager.ObjectManager.Me.Rotation;
                if (dif < 0)
                    dif = -dif;
                if (dif <= 0.10f)
                    return;

                ClickToMove.CGPlayer_C__ClickToMove(obj.Position.X, obj.Position.Y, obj.Position.Z, obj.Guid,
                                                    (Int32) ClickToMoveType.FaceTarget, 0.5f);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Face(WoWGameObject obj): " + exception);
            }
        }

        /// <summary>
        /// Faces the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        public static void Face(Point position)
        {
            try
            {
                Point localPlayerPosition = ObjectManager.ObjectManager.Me.Position;
                float wowFacing =
                    NegativeAngle(
                        (float)
                        System.Math.Atan2((position.Y - localPlayerPosition.Y), (position.X - localPlayerPosition.X)));
                float dif = wowFacing - ObjectManager.ObjectManager.Me.Rotation;
                if (dif < 0)
                    dif = -dif;
                if (dif <= 0.10f)
                    return;

                ClickToMove.CGPlayer_C__ClickToMove(position.X, position.Y, position.Z, 0, (Int32) ClickToMoveType.Face,
                                                    0.5f);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Face(Point position): " + exception);
            }
        }

        private static float NegativeAngle(float angle)
        {
            try
            {
                //if the turning angle is negative
                if (angle < 0)
                    //add the maximum possible angle (PI x 2) to normalize the negative angle
                    angle += (float) (System.Math.PI*2);
                return angle;
            }
            catch (Exception exception)
            {
                Logging.WriteError("NegativeAngle(float angle): " + exception);
            }
            return 0;
        }

        #endregion

        #region NPC/Object Finder

        private static string FoundType;
        private static bool LastChance;

        public static Npc FindTarget(Npc Target, out WoWUnit TargetIsNPC, out WoWObject TargetIsObject)
        {
            Logging.Write("Initiate target finding, currently looking for: " + Target.Name + " (" + Target.Entry + ").");
            LastChance = true;
            FoundType = "none";
            Random rand = new Random();
            if (Target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 5f &&
                Target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount)
                MountTask.Mount();
            // We can't stop the FindTarget if we have not found the Target from the Memory, that's why we don't check for <= 5f yet.
            TargetIsNPC = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry(Target.Entry), Target.Position);
            TargetIsObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Target.Entry), Target.Position);
            if (TargetIsNPC.IsValid)
            {
                Target.Position = TargetIsNPC.Position;
                Target.Name = TargetIsNPC.Name;
                FoundType = "NPC";
            }
            else if (TargetIsObject.IsValid)
            {
                Target.Position = TargetIsObject.Position;
                Target.Name = TargetIsObject.Name;
                FoundType = "OBJ";
            }

            var timerPathFinder = new Timer(0);
            GeneratePath:
            if (Target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 3.5f && FoundType != "none")
                goto End;
            if (ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsMounted)
                return Target;
            List<Point> points = PathFinder.FindPath(Target.Position);
            Go(points);
            // Check < 3.5 yards instead of 5 yards to avoid losing few seconds while approaching a Patrolling Target.
            // Also being more Human by not standing at the farthest distance from the Target.
            var timer = new Timer(((int) Math.DistanceListPoint(points)/3*1000) + 5000);
            while (InMovement && Usefuls.InGame && !(ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsMounted) &&
                   !ObjectManager.ObjectManager.Me.IsDeadMe)
            {
                if (timer.IsReady)
                    goto GeneratePath;
                if (Target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= (rand.NextDouble()*2f+2.5f) && FoundType != "none")
                    StopMove();
                /* The following code check 2 things.
                 * 1: Does the real Target (Memory) is at a different place than the Target Position in the Profile ?
                 * 2: Did the real Target (Memory) mooved ?
                 */
                switch (FoundType)
                {
                    case "NPC":
                        TargetIsNPC = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry(Target.Entry), Target.Position);
                        if (TargetIsNPC.IsValid && TargetIsNPC.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 50 &&
                            Target.Position.DistanceTo(TargetIsNPC.Position) > 1 &&
                            timerPathFinder.IsReady)
                        {
                            Target.Position = TargetIsNPC.Position;
                            Target.Name = TargetIsNPC.Name;
                            timerPathFinder = new Timer(5000);
                            goto GeneratePath;
                        }
                        if (!TargetIsNPC.IsValid)
                            FoundType = "none";
                        break;
                    case "OBJ":
                        TargetIsObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Target.Entry), Target.Position);
                        if (TargetIsObject.IsValid && TargetIsObject.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 50 &&
                            Target.Position.DistanceTo(TargetIsObject.Position) > 1 &&
                            timerPathFinder.IsReady)
                        {
                            Target.Position = TargetIsObject.Position;
                            Target.Name = TargetIsObject.Name;
                            timerPathFinder = new Timer(5000);
                            goto GeneratePath;
                        }
                        if (!TargetIsObject.IsValid)
                            FoundType = "none";
                        break;
                    default:
                        if (Target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 50)
                        {
                            TargetIsNPC = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry(Target.Entry), Target.Position);
                            TargetIsObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Target.Entry), Target.Position);
                            if (TargetIsNPC.IsValid && Target.Position.DistanceTo(TargetIsNPC.Position) > 1 && timerPathFinder.IsReady)
                            {
                                Target.Position = TargetIsNPC.Position;
                                Target.Name = TargetIsNPC.Name;
                                timerPathFinder = new Timer(5000);
                                FoundType = "NPC";
                                goto GeneratePath;
                            }
                            if (TargetIsObject.IsValid && Target.Position.DistanceTo(TargetIsObject.Position) > 1 && timerPathFinder.IsReady)
                            {
                                Target.Position = TargetIsObject.Position;
                                Target.Name = TargetIsObject.Name;
                                timerPathFinder = new Timer(5000);
                                FoundType = "OBJ";
                                goto GeneratePath;
                            }
                        }
                        break;
                }
            }

            while (InMovement && Target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 3.5f &&
                   Target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 15)
            {
                switch (FoundType)
                {
                    case "NPC":
                        TargetIsNPC = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry(Target.Entry), Target.Position);
                        if (TargetIsNPC.IsValid && TargetIsNPC.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 15)
                        {
                            Target.Position = TargetIsNPC.Position;
                            Target.Name = TargetIsNPC.Name;
                            _points[_points.Count - 1] = Target.Position;
                        }
                        break;
                    case "OBJ":
                        TargetIsObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Target.Entry), Target.Position);
                        if (TargetIsObject.IsValid && TargetIsObject.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 15)
                        {
                            Target.Position = TargetIsObject.Position;
                            Target.Name = TargetIsObject.Name;
                            _points[_points.Count - 1] = Target.Position;
                        }
                        break;
                    default:
                        goto End;
                }
                if (ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsMounted)
                    return Target;
            }
            End:
            if (FoundType == "none" && Target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 15)
            {
                if (LastChance)
                {
                    Logging.WriteDebug("Last chance used when searching for Target " + Target.Name + " (" + Target.Entry + ").");
                    LastChance = false;
                    TargetIsNPC = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry(Target.Entry), Target.Position);
                    TargetIsObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Target.Entry), Target.Position);
                    FoundType = TargetIsNPC.IsValid ? "NPC" : TargetIsObject.IsValid ? "OBJ" : "none";
                    goto End;
                }
                Logging.Write("Aborting FindTarget, it seems the Target " + Target.Name + " (" + Target.Entry + ") is not spawn at coordonate (" + Target.Position.X + ";" +
                              Target.Position.Y + ";" + Target.Position.Z + ").");
                return Target;
            }
            if (Target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 3.5f)
                goto GeneratePath;
            Logging.Write("Terminate target finding, found: " + Target.Name + " (" + Target.Entry + ").");
            return Target;
        }

        #endregion

        #region Melee Control System

        private static uint _currFightMeleeControl;
        private static readonly Timer MeleeControlTimer = new Timer(5000);
        private static readonly Timer MeleeControlMovementTimer = new Timer(3000);

        private static void MeleeControl(uint minDist, uint maxDist, bool resetCount)
        {
            if (_currFightMeleeControl > 5 || (_currFightMeleeControl != 0 && !MeleeControlTimer.IsReady))
                return;
            MeleeControlTimer.Reset(); // No reasons to spam the check, and less than 5 seconds of inactivity is not 'dramatical'.
            if (_currFightMeleeControl > 0 && resetCount)
                _currFightMeleeControl = 0;
            if (ObjectManager.ObjectManager.Target.GetDistance < minDist && ObjectManager.ObjectManager.Target.InCombatWithMe)
            {
                Logging.WriteFight("Under the minimal distance from the target, move closer.");
                _currFightMeleeControl++;
                GetToMelee(maxDist, maxDist);
            }
            if (ObjectManager.ObjectManager.Target.GetDistance > maxDist && ObjectManager.ObjectManager.Target.InCombatWithMe)
            {
                Logging.WriteFight("Over the maximal distance from the target, move closer.");
                _currFightMeleeControl++;
                AvoidMelee(minDist, maxDist);
            }
            /*
             * resetCount must be set true after each target switching (pull part) of the CombatClass.
             * TODO: Check if there is no ravines in front/back of us using the GetZ function if not too greedy for perfs.
             */
        }

        private static void AvoidMelee(uint minDist, uint maxDist)
        {
            MeleeControlMovementTimer.Reset();
            MovementsAction.MoveBackward(true);
            while (ObjectManager.ObjectManager.Target.GetDistance < minDist && ObjectManager.ObjectManager.Target.GetDistance < maxDist &&
                   ObjectManager.ObjectManager.Target.InCombatWithMe && !MeleeControlMovementTimer.IsReady)
                Thread.Sleep(50);
            MovementsAction.MoveBackward(false);
        }

        private static void GetToMelee(uint minDist, uint maxDist)
        {
            MeleeControlMovementTimer.Reset();
            MovementsAction.MoveForward(true);
            while (ObjectManager.ObjectManager.Target.GetDistance > maxDist && ObjectManager.ObjectManager.Target.GetDistance > minDist &&
                   ObjectManager.ObjectManager.Target.InCombatWithMe && !MeleeControlMovementTimer.IsReady)
                Thread.Sleep(50);
            MovementsAction.MoveForward(false);
        }

        #endregion
    }
}