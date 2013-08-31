using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers.PathFinderClass;

namespace nManager.Wow.Helpers
{
    /// <summary>
    /// Path Generator Class
    /// </summary>
    public class PathFinder
    {
        /// <summary>
        /// Enable or disable path finder.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use pather finder]; otherwise, <c>false</c>.
        /// </value>
        public static bool UsePatherFind
        {
            get { return nManagerSetting.CurrentSetting.ActivatePathFindingFeature; }
        }

        private static Pather _pather;

        /// <summary>
        /// Finds the path.
        /// </summary>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static List<Point> FindPath(Point to)
        {
            try
            {
                if (ObjectManager.ObjectManager.Me.Position.Type.ToLower() == "swimming")
                {
                    if (TraceLine.TraceLineGo(new Point(to.X, to.Y, to.Z + 1000), to,
                                              Enums.CGWorldFrameHitFlags.HitTestLiquid))
                    {
                        // The destination is in water
                        if (
                            !TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position, to))
                        {
                            Logging.WriteNavigator("Swimmming right to the destination");
                            return new List<Point> {to};
                        }
                        Logging.WriteNavigator("Swimming to the destination using the PathFinder");
                    }
                    else
                        Logging.WriteNavigator("Using the PathFinder to destination out of water");
                }
                return FindPath(ObjectManager.ObjectManager.Me.Position, to);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point to): " + exception);
            }
            return new List<Point>();
        }

        /// <summary>
        /// Finds the path.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static List<Point> FindPath(Point from, Point to)
        {
            try
            {
                return FindPath(from, to, Usefuls.ContinentNameMpq);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point from, Point to): " + exception);
            }
            return new List<Point>();
        }

        /// <summary>
        /// Finds the path.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="resultSuccess"> </param>
        /// <returns></returns>
        public static List<Point> FindPath(Point to, out bool resultSuccess)
        {
            try
            {
                if (ObjectManager.ObjectManager.Me.Position.Type.ToLower() == "swimming")
                {
                    if (TraceLine.TraceLineGo(new Point(to.X, to.Y, to.Z + 1000), to,
                                              Enums.CGWorldFrameHitFlags.HitTestLiquid))
                    {
                        // The destination is in water
                        if (
                            !TraceLine.TraceLineGo(ObjectManager.ObjectManager.Me.Position, to))
                        {
                            Logging.WriteNavigator("Swimmming right to the destination");
                            resultSuccess = true;
                            return new List<Point> {to};
                        }
                        Logging.WriteNavigator("Swimming to the destination using the PathFinder");
                    }
                    else
                        Logging.WriteNavigator("Using the PathFinder to destination out of water");
                }
                return FindPath(ObjectManager.ObjectManager.Me.Position, to, Usefuls.ContinentNameMpq, out resultSuccess);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point to, out bool resultSuccess): " + exception);
            }
            resultSuccess = false;
            return new List<Point>();
        }

        /// <summary>
        /// Finds the path.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="continentNameMpq">The continent name MPQ.</param>
        /// <returns></returns>
        public static List<Point> FindPath(Point from, Point to, string continentNameMpq)
        {
            try
            {
                bool b;
                return FindPath(from, to, continentNameMpq, out b);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point from, Point to, string continentNameMpq): " + exception);
            }
            return new List<Point>();
        }

        /// <summary>
        /// Finds the path.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="continentNameMpq">The continent name MPQ.</param>
        /// <param name="resultSuccess">if set to <c>true</c> [result success].</param>
        /// <param name="addFromAndStart">if set to <c>true</c> [add from and start].</param>
        /// <param name="loadAllTile"></param>
        /// <returns></returns>
        public static List<Point> FindPath(Point from, Point to, string continentNameMpq, out bool resultSuccess,
                                           bool addFromAndStart = true, bool loadAllTile = false)
        {
            List<Point> locList = new List<Point>();
            resultSuccess = true;
            try
            {
                if (!UsePatherFind || continentNameMpq == "None")
                {
                    locList.Add(from);
                    locList.Add(to);
                    return locList;
                }

                if (_pather == null)
                    _pather = new Pather(continentNameMpq);
                if (_pather.Continent != continentNameMpq)
                {
                    _pather.Dispose();
                    _pather = new Pather(continentNameMpq);
                }

                if (addFromAndStart)
                    locList.Add(from);

                if (loadAllTile)
                    _pather.LoadAllTiles();

                locList = _pather.FindPath(from, to, out resultSuccess);
                if (addFromAndStart && resultSuccess)
                    locList.Add(to);

                // Clean list:
                for (int i = 0; i <= locList.Count - 2; i++)
                {
                    if (locList[i].DistanceTo(locList[i + 1]) < 0.5 ||
                        (locList[i + 1].X == 0.0f || locList[i + 1].Y == 0.0f || locList[i + 1].Z == 0.0f))
                    {
                        locList.RemoveAt(i + 1);
                        i--;
                    }
                }
                // Offset all points except origin and end to pass around obstacles by some distance.
                // We stop at 2.0 before each point in MovementManager and the meshes are done for a player of 0.6 in diameter.
                // So 2.0-0.6=1.4 is the strick minimum offset needed. Since 'click to point' has a precision of 0.5, 
                // 1.4+0.5=1.9 is a pretty good value here.
                for (int i = locList.Count - 2; i > 0; i--)
                {
                    Point offset = Helpful.Math.GetPositionOffsetBy3DDistance(locList[i - 1], locList[i], 1.9f);
                    locList[i] = offset;
                }

                Logging.WriteNavigator("Path Count: " + locList.Count());

                return locList;
            }
            catch (Exception e)
            {
                Logging.WriteError("ToRecast(this Point v): PATH FIND ERROR: " + e);
                Console.WriteLine("Path find ERROR.");

                resultSuccess = false;
                locList = new List<Point>();

                if (addFromAndStart)
                {
                    if (from != null)
                        if (from.X != 0 || from.Y != 0 || from.Z != 0)
                            locList.Add(from);

                    if (to != null)
                        if (to.X != 0 || to.Y != 0 || to.Z != 0)
                            locList.Add(to);
                }

                return locList;
            }
        }

        public static float GetZPosition(float x, float y, bool strict = false)
        {
            return GetZPosition(new Point(x, y, 0), strict);
        }

        /// <summary>
        /// </summary>
        /// <param name="point"></param>
        /// <param name="strict"></param>
        /// <returns></returns>
        public static float GetZPosition(Point point, bool strict = false)
        {
            try
            {
                if (_pather == null)
                    _pather = new Pather(Usefuls.ContinentNameMpq);
                if (_pather.Continent != Usefuls.ContinentNameMpq)
                {
                    _pather.Dispose();
                    _pather = new Pather(Usefuls.ContinentNameMpq);
                }
                return _pather.GetZ(point, strict);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetZPosition(Point point): " + exception);
            }
            return 0;
        }

        public static List<Point> FindPathUnstuck(Point to)
        {
            try
            {
                bool result;
                List<Point> points = FindPath(to, out result);
                if (!result && points.Count <= 2)
                {
                    Thread.Sleep(100);
                    Logging.WriteNavigator("FindPathUnstuck : FindPath failed. From: " +
                                           ObjectManager.ObjectManager.Me.Position + " To: " + to);
                    int trys = 0;
                    while (!result && trys <= 2)
                    {
                        MovementsAction.MoveForward(true);
                        Thread.Sleep(1000);
                        MovementsAction.Ascend(true);
                        Thread.Sleep(100);
                        MovementsAction.Ascend(false);
                        points = FindPath(to, out result);
                        Thread.Sleep(200);
                        MovementsAction.MoveForward(false);
                        trys++;
                    }
                }
                return points;
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPathUnstuck(Point to): " + exception);
            }
            return new List<Point>();
        }
    }
}