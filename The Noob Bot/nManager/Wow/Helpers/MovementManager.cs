using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;
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
        private static int _currentTargetedPoint; // let's remember where we are instead of searching point and doing mess
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
        }

        public static void LaunchThreadMovementManager()
        {
            try
            {
                lock (typeof(MovementManager))
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
                    lock (typeof(MovementManager))
                    {
                        while ((_loop || _first) && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                        {
                            if(Statistics.OffsetStats == 0xB5)
                            {
                                _first = false;

                                if (_movement && _points.Count > 0)
                                {
                                    if (_points[_currentTargetedPoint].DistanceTo2D(ObjectManager.ObjectManager.Me.Position) < 0.5f)
                                    {
                                        _currentTargetedPoint++;
                                    }

                                    if (_currentTargetedPoint >= _points.Count - 1)
                                    {
                                        _currentTargetedPoint = 0;
                                    }

                                    if (_points[_currentTargetedPoint].Type.ToLower() == "flying")
                                        FlyMouvementManager(_currentTargetedPoint);
                                    else if (_points[_currentTargetedPoint].Type.ToLower() == "swimming")
                                        AquaticMouvementManager(_currentTargetedPoint);
                                    else
                                        GroundMouvementManager(_currentTargetedPoint);

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
            // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns

        static void GroundMouvementManager(int firstIdPoint)
        {
            try
            {
                if (_movement && _points.Count > 0)
                {
                    if (_points[firstIdPoint].Type.ToLower() == "flying" || _points[firstIdPoint].Type.ToLower() == "swimming")
                    {
                        return;
                    }

                    if (_points[firstIdPoint].DistanceTo2D(ObjectManager.ObjectManager.Me.Position) >= 200 && _loop && !String.IsNullOrWhiteSpace(nManagerSetting.CurrentSetting.FlyingMountName))
                    {
                        bool result;
                        var path = PathFinderDroidz.FindPath(_points[firstIdPoint], out result, "Flying");
                        if (result)
                        {
                            for (int i = 0; i <= path.Count-1; i++)
                            {
                                path[i].Type = "Flying";
                            }
                            _points = path;
                            _loop = false;
                            firstIdPoint = 0;
                            FlyMouvementManager(firstIdPoint);
                            return;
                        }
                        else
                        {
                            Logging.WriteNavigator("Long Move distance: " + ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[firstIdPoint]));
                            LongMove.LongMoveByNewThread(_points[firstIdPoint]);
                            Thread.Sleep(100);
                            while (LongMove.IsLongMove && _movement)
                            {
                                Thread.Sleep(50);
                            }
                            LongMove.StopLongMove();
                        }
                        if (!_movement)
                            return;
                    }

                    if (nManagerSetting.CurrentSetting.useGroundMount)
                    {
                        if (Math.DistanceListPoint(_points) >= nManagerSetting.CurrentSetting.mountDistance || _loop)
                            Bot.Tasks.MountTask.MountingGroundMount(false);
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
                            if (_points[firstIdPoint].Type.ToLower() == "flying" || _points[firstIdPoint].Type.ToLower() == "swimming")
                            {
                                return;
                            }
                            // GoTo next Point
                            if ((((ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_points[idPoint]) <= 2.0f &&
                                   ObjectManager.ObjectManager.Me.IsMounted) ||
                                  ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_points[idPoint]) <= 2.0f) &&
                                 ObjectManager.ObjectManager.Me.Position.DistanceZ(_points[idPoint]) <= 10.5f) && _movement)
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
                                    _points.AddRange(PathFinder.FindPath(_pointsOrigine[_points.Count - 1]));
                                    //idPoint = 0; // no: do not reset the id after a stuck
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
                                Keybindings.PressKeybindings(Enums.Keybindings.JUMP);
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

        static void FlyMouvementManager(int firstIdPoint)
        {
            try
            {
                if (_movement && _points.Count > 0)
                {
                    Bot.Tasks.MountTask.MountingFlyingMount(false);
                    if (!_movement)
                        return;
                    int idPoint = firstIdPoint;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_points[idPoint]) > 200 || _points.Count == 1)
                    {
                        bool result;
                        var path = PathFinderDroidz.FindPath(_points[idPoint], out result, "Flying");
                        if (result)
                        {
                            _points = path;
                            _loop = false;
                            idPoint = 0;
                        }
                        else
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
                            Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                            Thread.Sleep(300);
                            Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                        }

                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[idPoint]) < 10 && _movement)
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

        static void AquaticMouvementManager(int firstIdPoint)
        {
            try
            {
                if (_movement && _points.Count > 0)
                {
                    Bot.Tasks.MountTask.MountingAquaticMount(false);
                    if (!_movement)
                        return;
                    int idPoint = firstIdPoint;
                    if (_points[idPoint].DistanceTo(ObjectManager.ObjectManager.Me.Position) > 30)
                    {
                        bool result;
                        var path = PathFinderDroidz.FindPath(_points[idPoint], out result, "Swimming");
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
                        while (!Usefuls.IsSwimming && ObjectManager.ObjectManager.Me.IsMounted)
                        {
                            Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                            Thread.Sleep(300);
                            Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
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
                Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);

                if (ObjectManager.ObjectManager.Me.IsMounted)
                {
                    Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                    Thread.Sleep(Others.Random(1300, 2000));
                    Keybindings.UpKeybindings(Enums.Keybindings.JUMP);

                    // if fly mode:
                    if (Usefuls.IsFlying)
                    {
                        UnStuckFly();
                        IsUnStuck = false;
                        StuckCount++;
                        return;
                    }
                }
                Statistics.Stucks++;
                Logging.WriteNavigator("Unstuck character.");

                var lastPost = new Point(ObjectManager.ObjectManager.Me.Position);
                for (int i = 0; i < 8; i++)
                {
                    int j = Others.Random(1, 8);

                    float disX = System.Math.Abs(lastPost.X - ObjectManager.ObjectManager.Me.Position.X);
                    float disY = System.Math.Abs(lastPost.Y - ObjectManager.ObjectManager.Me.Position.Y);

                    if (disX < 2.5f || disY < 2.5f)
                    //we likely don't need to run this if we are 5+ units from from our stuck point
                    {
                        if (j == 1)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 10,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0, (int)ClickToMoveType.Move,
                                                                0.5f); // try 'backward'
                        else if (j == 2)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0, (int)ClickToMoveType.Move,
                                                                0.5f); // try 'back left'
                        else if (j == 3)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 10,
                                                                ObjectManager.ObjectManager.Me.Position.Y, ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int)ClickToMoveType.Move, 0.5f); // try 'left'
                        else if (j == 4)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0, (int)ClickToMoveType.Move,
                                                                0.5f); // try 'foward left'
                        else if (j == 5)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 10,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0, (int)ClickToMoveType.Move,
                                                                0.5f); // try 'forward'
                        else if (j == 6)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 14,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0, (int)ClickToMoveType.Move,
                                                                0.5f); // try 'forward right'
                        else if (j == 7)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 10,
                                                                ObjectManager.ObjectManager.Me.Position.Y, ObjectManager.ObjectManager.Me.Position.Z, 0,
                                                                (int)ClickToMoveType.Move, 0.5f); // try 'right'
                        else if (j == 8)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 14,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z, 0, (int)ClickToMoveType.Move,
                                                                0.5f); // try 'back right'

                        int k = Others.Random(1, 3);
                        if (k == 2)
                            Keybindings.PressKeybindings(Enums.Keybindings.JUMP); // Jump

                        Thread.Sleep(Others.Random(1000, 2500));
                    }
                    else
                        break;
                }


                IsUnStuck = false;
                StuckCount++;
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
                Statistics.Stucks++;
                Logging.WriteNavigator("Unstuck character > flying mode.");

                Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                Thread.Sleep(Others.Random(300, 900));
                Keybindings.UpKeybindings(Enums.Keybindings.JUMP);

                var lastPost = new Point(ObjectManager.ObjectManager.Me.Position);
                var iR = Others.Random(2, 3);
                for (int i = 0; i < iR; i++)
                {
                    int j = Others.Random(1, 8);
                    int z = Others.Random(-15, 15);

                    float disX = System.Math.Abs(lastPost.X - ObjectManager.ObjectManager.Me.Position.X);
                    float disY = System.Math.Abs(lastPost.Y - ObjectManager.ObjectManager.Me.Position.Y);

                    if (disX < 5 || disY < 5)
                    {
                        if (j == 1)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 15,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int)ClickToMoveType.Move, 0.5f); // try 'backward'
                        else if (j == 2)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 17,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 17,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int)ClickToMoveType.Move, 0.5f); // try 'back left'
                        else if (j == 3)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 15,
                                                                ObjectManager.ObjectManager.Me.Position.Y, ObjectManager.ObjectManager.Me.Position.Z + z,
                                                                0, (int)ClickToMoveType.Move, 0.5f); // try 'left'
                        else if (j == 4)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X - 17,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 17,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int)ClickToMoveType.Move, 0.5f); // try 'foward left'
                        else if (j == 5)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 15,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int)ClickToMoveType.Move, 0.5f); // try 'forward'
                        else if (j == 6)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 15,
                                                                ObjectManager.ObjectManager.Me.Position.Y + 17,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int)ClickToMoveType.Move, 0.5f); // try 'forward right'
                        else if (j == 7)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 15,
                                                                ObjectManager.ObjectManager.Me.Position.Y, ObjectManager.ObjectManager.Me.Position.Z + z,
                                                                0, (int)ClickToMoveType.Move, 0.5f); // try 'right'
                        else if (j == 8)
                            ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X + 17,
                                                                ObjectManager.ObjectManager.Me.Position.Y - 14,
                                                                ObjectManager.ObjectManager.Me.Position.Z + z, 0,
                                                                (int)ClickToMoveType.Move, 0.5f); // try 'back right'

                        Thread.Sleep(100);
                        var tus = new Timer(3000);
                        while (ClickToMove.GetClickToMoveTypePush() == ClickToMoveType.Move && !tus.IsReady)
                            Thread.Sleep(50);

                        if (i == 7)
                        {
                            int k = Others.Random(1, 4);
                            if (k == 2)
                            {
                                Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                                Thread.Sleep(Others.Random(350, 800));
                                Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                            }
                            if (k == 3)
                            {
                                Keybindings.DownKeybindings(Enums.Keybindings.SITORSTAND);
                                Thread.Sleep(Others.Random(350, 800));
                                Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                            }
                        }
                    }
                    else
                        break;
                }
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
                    lock (typeof(MovementManager))
                    {
                        if (_worker == null)
                            LaunchThreadMovementManager();

                        _pointsOrigine = new List<Point>();
                        _pointsOrigine.AddRange(points);
                        _points = new List<Point>();
                        _points.AddRange(points);
                        _currentTargetedPoint = 0;

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
                    lock (typeof(MovementManager))
                    {
                        if (_worker == null)
                            LaunchThreadMovementManager();

                        _pointsOrigine = new List<Point>();
                        _pointsOrigine.AddRange(points);
                        _points = new List<Point>();
                        _points.AddRange(points);
                        _currentTargetedPoint = Math.NearestPointOfListPoints(_points, ObjectManager.ObjectManager.Me.Position);
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
                lock (typeof(MovementManager))
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
                _workerMoveTo = new Thread(ThreadMoveTo) { IsBackground = true, Name = "MoveTo" };
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
            // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns

        private static bool MoveToLocation(Point position)
        {
            try
            {
                while (ObjectManager.ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(100);
                }

                var timer = new Timer(1 * 1000 * 2);
                var timerWaypoint = new Timer(1 * 1000 * (30 / 3));
                double distance = (double)position.DistanceTo(ObjectManager.ObjectManager.Me.Position) - 1;
                if (distance > 45)
                    timerWaypoint = new Timer(1 * 1000 * (distance / 3));

                if (distance < 45 && distance > 40)
                    timerWaypoint = new Timer(1 * 1000 * (45 / 3));

                if (distance < 40 && distance > 35)
                    timerWaypoint = new Timer(1 * 1000 * (40 / 3));

                if (distance < 35 && distance > 30)
                    timerWaypoint = new Timer(1 * 1000 * (35 / 3));

                Point oldPos = ObjectManager.ObjectManager.Me.Position;
                bool alerted = false;

                while (!timerWaypoint.IsReady && Usefuls.InGame)
                {
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(position) <= 0.6)
                        return true;

                    Point posP = ObjectManager.ObjectManager.Me.Position;

                    while (ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(10);
                    }
                    if (ClickToMove.GetClickToMovePosition().DistanceTo(position) > 1 ||
                        ClickToMove.GetClickToMoveTypePush() != ClickToMoveType.Move)
                        ClickToMove.CGPlayer_C__ClickToMove(position.X, position.Y, position.Z, 0,
                                                            (int)ClickToMoveType.Move, 0.5f);

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
                        //releaseKeys();
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
        public static void MoveTo(Point point)
        {
            try
            {
                if (point.X != 0 && point.Y != 0 && point.Z != 0)
                {
                    if (_workerMoveTo == null)
                        LaunchThreadMoveTo();

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
        public static void MoveTo(float x, float y, float z)
        {
            try
            {
                MoveTo(new Point(x, y, z));
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
        public static void MoveTo(WoWGameObject obj)
        {
            try
            {
                MoveTo(obj.Position);
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveTo(WoWGameObject obj): " + exception);
            }
        }

        /// <summary>
        /// Stop Player.
        /// </summary>
        public static void StopMoveTo(bool UpKey = true)
        {
            try
            {
                _loopMoveTo = false;
                if (ObjectManager.ObjectManager.Me.GetMove)
                {
                    if (UpKey)
                    {
                        Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                        Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                    }
                    Keybindings.PressKeybindings(Enums.Keybindings.MOVEFORWARD);
                    Keybindings.PressKeybindings(Enums.Keybindings.MOVEBACKWARD);

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
                        System.Math.Atan2((obj.Position.Y - localPlayerPosition.Y), (obj.Position.X - localPlayerPosition.X)));
                float dif = wowFacing - ObjectManager.ObjectManager.Me.Rotation;
                if (dif < 0)
                    dif = -dif;
                if (dif <= 0.10f)
                    return;

                ClickToMove.CGPlayer_C__ClickToMove(obj.Position.X, obj.Position.Y, obj.Position.Z, obj.Guid,
                                                    (Int32)ClickToMoveType.FaceTarget, 0.5f);
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
                        System.Math.Atan2((obj.Position.Y - localPlayerPosition.Y), (obj.Position.X - localPlayerPosition.X)));
                float dif = wowFacing - ObjectManager.ObjectManager.Me.Rotation;
                if (dif < 0)
                    dif = -dif;
                if (dif <= 0.10f)
                    return;

                ClickToMove.CGPlayer_C__ClickToMove(obj.Position.X, obj.Position.Y, obj.Position.Z, obj.Guid,
                                                    (Int32)ClickToMoveType.FaceTarget, 0.5f);
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
                        System.Math.Atan2((obj.Position.Y - localPlayerPosition.Y), (obj.Position.X - localPlayerPosition.X)));
                float dif = wowFacing - ObjectManager.ObjectManager.Me.Rotation;
                if (dif < 0)
                    dif = -dif;
                if (dif <= 0.10f)
                    return;

                ClickToMove.CGPlayer_C__ClickToMove(obj.Position.X, obj.Position.Y, obj.Position.Z, obj.Guid,
                                                    (Int32)ClickToMoveType.FaceTarget, 0.5f);
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
                        (float)System.Math.Atan2((position.Y - localPlayerPosition.Y), (position.X - localPlayerPosition.X)));
                float dif = wowFacing - ObjectManager.ObjectManager.Me.Rotation;
                if (dif < 0)
                    dif = -dif;
                if (dif <= 0.10f)
                    return;

                ClickToMove.CGPlayer_C__ClickToMove(position.X, position.Y, position.Z, 0, (Int32)ClickToMoveType.Face,
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
                    angle += (float)(System.Math.PI * 2);
                return angle;
            }
            catch (Exception exception)
            {
                Logging.WriteError("NegativeAngle(float angle): " + exception);
            }
            return 0;
        }

        #endregion

        #region Pitch

        /// <summary>
        /// Pitches position.
        /// </summary>
        /// <param name="point">The point.</param>
        public static void PitchWrite(Point point)
        {
            try
            {
                int angle =
                    Convert.ToInt32(Math.RadianToDegree(ObjectManager.ObjectManager.Me.Pitch) -
                                    Math.RadianToDegree(Math.PitchAngle(ObjectManager.ObjectManager.Me.Position, point)));
                if (angle < 0)
                {
                    angle = -angle;
                }
                if (angle > 5)
                {
                    ObjectManager.ObjectManager.Me.Pitch = Math.PitchAngle(ObjectManager.ObjectManager.Me.Position, point);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("PitchWrite(Point point): " + exception);
            }
        }

        #endregion Pitch
    }
}
