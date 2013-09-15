using System.Runtime.InteropServices;
using nManager.Wow.Patchables;
using nManager.Wow.Class;
using System.Collections.Generic;

namespace nManager.Wow.Helpers
{
    public class WoWQuestPOIPoint
    {
        private readonly List<Point> _setPoints;
        private Point _middlePoint = new Point(0, 0, 0);
        private bool _middlePointSet;

        private static DBC<QuestPOIPointDbcRecord> _qpPointDBC;

        private WoWQuestPOIPoint(uint setId)
        {
            _setPoints = new List<Point>();
            if (_qpPointDBC == null)
                _qpPointDBC = new DBC<QuestPOIPointDbcRecord>((int) Addresses.DBC.QuestPOIPoint);
            bool flag = false;
            for (int id = _qpPointDBC.MinIndex; id <= _qpPointDBC.MaxIndex; id++)
            {
                QuestPOIPointDbcRecord qpPointDbcRecord = _qpPointDBC.GetRow(id);
                if (qpPointDbcRecord.Id == id)
                {
                    if (qpPointDbcRecord.SetId == setId)
                    {
                        _setPoints.Add(new Point(qpPointDbcRecord.X, qpPointDbcRecord.Y, 0));
                        flag = true;
                    }
                    else if (flag)
                        break;
                }
            }
        }

        // Factory function
        public static WoWQuestPOIPoint FromSetId(uint setId)
        {
            return new WoWQuestPOIPoint(setId);
        }

        public List<Point> PointList
        {
            get { return _setPoints; }
        }

        public Point Center
        {
            get
            {
                if (_setPoints.Count == 0)
                    return new Point(0, 0, 0, "invalid");
                int middleX = 0;
                int middleY = 0;
                for (int i = 0; i < _setPoints.Count; i++)
                {
                    middleX += (int) _setPoints[i].X;
                    middleY += (int) _setPoints[i].Y;
                }
                float x = middleX/_setPoints.Count;
                float y = middleY/_setPoints.Count;
                float z = PathFinder.GetZPosition(x, y);
                return new Point(x, y, z + 80.0f);
            }
        }

        public bool ValidPoint
        {
            get { return _middlePointSet; }
        }

        public Point MiddlePoint
        {
            get
            {
                // We have it already, then return it
                if (_middlePoint.X != 0 || _middlePoint.Y != 0)
                    return _middlePoint;

                // We don't have it, then compute it
                _middlePoint = Center;
                float curZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y, true);
                float anotherZ = PathFinder.GetZPosition(_middlePoint.X + 8, _middlePoint.Y + 8, true);
                if (!IsInside(_middlePoint) || curZ == 0 || (uint) (anotherZ - curZ) >= 11 ||
                    TraceLine.TraceLineGo(new Point(_middlePoint.X, _middlePoint.Y, curZ),
                                          new Point(_middlePoint.X, _middlePoint.Y, curZ + 50)))
                {
                    bool found = false;
                    int delta = 0;
                    // special case when the polygon is not convexe or/and to find a usable point
                    do
                    {
                        delta += 5;
                        if (IsInside(new Point(_middlePoint.X + delta, _middlePoint.Y, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_middlePoint.X + delta, _middlePoint.Y, true);
                            anotherZ = PathFinder.GetZPosition(_middlePoint.X + delta + 8, _middlePoint.Y, true);
                            if (curZ != 0 && (uint) (anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_middlePoint.X + delta, _middlePoint.Y, curZ),
                                                       new Point(_middlePoint.X + delta, _middlePoint.Y, curZ + 50)))
                            {
                                _middlePoint = new Point(_middlePoint.X + delta, _middlePoint.Y, curZ + 2.0f);
                                found = true;
                                continue;
                            }
                        }
                        if (IsInside(new Point(_middlePoint.X - delta, _middlePoint.Y, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_middlePoint.X - delta, _middlePoint.Y, true);
                            anotherZ = PathFinder.GetZPosition(_middlePoint.X - delta - 8, _middlePoint.Y, true);
                            if (curZ != 0 && (uint) (anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_middlePoint.X - delta, _middlePoint.Y, curZ),
                                                       new Point(_middlePoint.X - delta, _middlePoint.Y, curZ + 50)))
                            {
                                _middlePoint = new Point(_middlePoint.X - delta, _middlePoint.Y, curZ + 2.0f);
                                found = true;
                                continue;
                            }
                        }
                        if (IsInside(new Point(_middlePoint.X, _middlePoint.Y + delta, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y + delta, true);
                            anotherZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y + delta + 8, true);
                            if (curZ != 0 && (uint) (anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_middlePoint.X, _middlePoint.Y + delta, curZ),
                                                       new Point(_middlePoint.X, _middlePoint.Y + delta, curZ + 50)))
                            {
                                _middlePoint = new Point(_middlePoint.X, _middlePoint.Y + delta, curZ + 2.0f);
                                found = true;
                                continue;
                            }
                        }
                        if (IsInside(new Point(_middlePoint.X, _middlePoint.Y - delta, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y - delta, true);
                            anotherZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y - delta - 8, true);
                            if (curZ != 0 && (uint) (anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_middlePoint.X, _middlePoint.Y - delta, curZ),
                                                       new Point(_middlePoint.X, _middlePoint.Y - delta, curZ + 50)))
                            {
                                _middlePoint = new Point(_middlePoint.X, _middlePoint.Y - delta, curZ + 2.0f);
                                found = true;
                            }
                        }
                    } while (!found);
                    //nManager.Helpful.Logging.Write("Not convexe or invalid: delta " + delta + " used.");
                }
                else
                {
                    _middlePoint = new Point(_middlePoint.X, _middlePoint.Y, curZ + 2.0f);
                }
                _middlePointSet = true;
                return _middlePoint;
            }
        }

        // An implementation of the shortest ray-trace algorythm for PiP problem I could make
        public bool IsInside(Point p)
        {
            int c = _setPoints.Count;
            int hits = 0;
            int px = (int) p.X;
            int py = (int) p.Y;
            for (int i = 0; i < c; i++)
            {
                int x1 = (int) _setPoints[i].X;
                int y1 = (int) _setPoints[i].Y;
                int x2 = (int) _setPoints[(i + 1)%c].X;
                int y2 = (int) _setPoints[(i + 1)%c].Y;
                if ((y1 - py)*(y2 - py) < 0)
                {
                    int ix = x1 + (x2 - x1)*(py - y1)/(y2 - y1);
                    if (ix < px)
                        hits++;
                }
            }
            if ((hits%2) != 0)
                return true;

            return false;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct QuestPOIPointDbcRecord
        {
            public readonly uint Id;
            public readonly int X;
            public readonly int Y;
            public readonly uint SetId;
        }
    }
}