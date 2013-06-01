using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;
using nManager.Wow.Class;
using System.Collections.Generic;

namespace nManager.Wow.Helpers
{
    class WoWQuestPOIPoint
    {
        private List<Point> _SetPoints;
        private Point _MiddlePoint = new Point(0, 0, 0);

        private static DBC<QuestPOIPointDbcRecord> qpPointDbc;

        private WoWQuestPOIPoint(uint setId)
        {
            _SetPoints = new List<Point>();
            QuestPOIPointDbcRecord qpPointDbcRecord;
            if (qpPointDbc == null)
                qpPointDbc = new DBC<QuestPOIPointDbcRecord>((int)Addresses.DBC.QuestPOIPoint);
            bool flag = false;
            for (int id = qpPointDbc.MinIndex; id <= qpPointDbc.MaxIndex; id++)
            {
                qpPointDbcRecord = qpPointDbc.GetRow((int)id);
                if (qpPointDbcRecord.Id == id)
                {
                    if (qpPointDbcRecord.SetId == setId)
                    {
                        _SetPoints.Add(new Point(qpPointDbcRecord.X, qpPointDbcRecord.Y, 0));
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
            get { return _SetPoints; }
        }

        public Point Center
        {
            get
            {
                if (_SetPoints.Count == 0)
                    return new Point(0, 0, 0, "invalid");
                int middleX = 0;
                int middleY = 0;
                for (int i = 0; i < _SetPoints.Count; i++)
                {
                    middleX += (int)_SetPoints[i].X;
                    middleY += (int)_SetPoints[i].Y;
                }
                return new Point(middleX / _SetPoints.Count, middleY / _SetPoints.Count, 0);
            }
        }

        public Point MiddlePoint
        {
            get
            {
                // We have it already, then return it
                if (_MiddlePoint.X != 0 || _MiddlePoint.Y != 0)
                    return _MiddlePoint;

                // We don't have it, then compute it
                _MiddlePoint = Center;
                float curZ = PathFinder.GetZPosition(_MiddlePoint.X, _MiddlePoint.Y, true);
                float anotherZ = PathFinder.GetZPosition(_MiddlePoint.X + 8, _MiddlePoint.Y + 8, true);
                if (!IsInside(_MiddlePoint) || curZ == 0 || (uint)(anotherZ - curZ) >= 11 ||
                    TraceLine.TraceLineGo(new Point(_MiddlePoint.X, _MiddlePoint.Y, curZ),
                    new Point(_MiddlePoint.X, _MiddlePoint.Y, curZ + 50), CGWorldFrameHitFlags.HitTestAll))
                {
                    bool found = false;
                    int delta = 0;
                    // special case when the polygon is not convexe or/and to find a usable point
                    do
                    {
                        delta += 5;
                        if (IsInside(new Point(_MiddlePoint.X + delta, _MiddlePoint.Y, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_MiddlePoint.X + delta, _MiddlePoint.Y, true);
                            anotherZ = PathFinder.GetZPosition(_MiddlePoint.X + delta + 8, _MiddlePoint.Y, true);
                            if (curZ != 0 && (uint)(anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_MiddlePoint.X + delta, _MiddlePoint.Y, curZ),
                                new Point(_MiddlePoint.X + delta, _MiddlePoint.Y, curZ + 50), CGWorldFrameHitFlags.HitTestAll))
                            {
                                _MiddlePoint = new Point(_MiddlePoint.X + delta, _MiddlePoint.Y, curZ + 2.0f);
                                found = true;
                                continue;
                            }
                        }
                        if (IsInside(new Point(_MiddlePoint.X - delta, _MiddlePoint.Y, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_MiddlePoint.X - delta, _MiddlePoint.Y, true);
                            anotherZ = PathFinder.GetZPosition(_MiddlePoint.X - delta - 8, _MiddlePoint.Y, true);
                            if (curZ != 0 && (uint)(anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_MiddlePoint.X - delta, _MiddlePoint.Y, curZ),
                                new Point(_MiddlePoint.X - delta, _MiddlePoint.Y, curZ + 50), CGWorldFrameHitFlags.HitTestAll))
                            {
                                _MiddlePoint = new Point(_MiddlePoint.X - delta, _MiddlePoint.Y, curZ + 2.0f);
                                found = true;
                                continue;
                            }
                        }
                        if (IsInside(new Point(_MiddlePoint.X, _MiddlePoint.Y + delta, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_MiddlePoint.X, _MiddlePoint.Y + delta, true);
                            anotherZ = PathFinder.GetZPosition(_MiddlePoint.X, _MiddlePoint.Y + delta + 8, true);
                            if (curZ != 0 && (uint)(anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_MiddlePoint.X, _MiddlePoint.Y + delta, curZ),
                                new Point(_MiddlePoint.X, _MiddlePoint.Y + delta, curZ + 50), CGWorldFrameHitFlags.HitTestAll))
                            {
                                _MiddlePoint = new Point(_MiddlePoint.X, _MiddlePoint.Y + delta, curZ + 2.0f);
                                found = true;
                                continue;
                            }
                        }
                        if (IsInside(new Point(_MiddlePoint.X, _MiddlePoint.Y - delta, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_MiddlePoint.X, _MiddlePoint.Y - delta, true);
                            anotherZ = PathFinder.GetZPosition(_MiddlePoint.X, _MiddlePoint.Y - delta - 8, true);
                            if (curZ != 0 && (uint)(anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_MiddlePoint.X, _MiddlePoint.Y - delta, curZ),
                                new Point(_MiddlePoint.X, _MiddlePoint.Y - delta, curZ + 50), CGWorldFrameHitFlags.HitTestAll))
                            {
                                _MiddlePoint = new Point(_MiddlePoint.X, _MiddlePoint.Y - delta, curZ + 2.0f);
                                found = true;
                            }
                        }
                    } while (!found);
                    //nManager.Helpful.Logging.Write("Not convexe or invalid: delta " + delta + " used.");
                }
                else
                {
                    _MiddlePoint = new Point(_MiddlePoint.X, _MiddlePoint.Y, curZ + 2.0f);
                }
                return _MiddlePoint;
            }
        }

        // An implementation of the shortest ray-trace algorythm for PiP problem I could make
        public bool IsInside(Point p)
        {
            int c = _SetPoints.Count;
            int hits = 0;
            int px = (int)p.X;
            int py = (int)p.Y;
            for (int i = 0; i < c; i++)
            {
                int x1 = (int)_SetPoints[i].X;
                int y1 = (int)_SetPoints[i].Y;
                int x2 = (int)_SetPoints[(i + 1) % c].X;
                int y2 = (int)_SetPoints[(i + 1) % c].Y;
                if ((y1 - py) * (y2 - py) < 0)
                {
                    int ix = x1 + (x2 - x1) * (py - y1) / (y2 - y1);
                    if (ix < px)
                        hits++;
                }
            }
            if ((hits % 2) != 0)
                return true;

            return false;
        }
   
        [StructLayout(LayoutKind.Sequential)]
        private struct QuestPOIPointDbcRecord
        {
            public uint Id;
            public int X;
            public int Y;
            public uint SetId;
        }

    }
}
