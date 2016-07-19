using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Patchables;
using nManager.Wow.Class;
using System.Collections.Generic;

namespace nManager.Wow.Helpers
{
    public class WoWQuestPOIPoint
    {
        private readonly List<Point> _setPoints;
        private Point _middlePoint = new Point(0, 0, 0);
        private Point _center = new Point(0, 0, 0);
        private bool _middlePointSet;

        private static DB5Reader _questPOIPointDB2;

        private static void Init() // todo make DBC loading shared accross all others reads with a better loading class.
        {
            if (_questPOIPointDB2 == null)
            {
                var mDefinitions = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                var definitions = mDefinitions.Tables.Where(t => t.Name == "QuestPOIPoint");

                if (!definitions.Any())
                {
                    definitions = mDefinitions.Tables.Where(t => t.Name == Path.GetFileName("QuestPOIPoint"));
                }
                if (definitions.Count() == 1)
                {
                    var table = definitions.First();
                    _questPOIPointDB2 = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\QuestPOIPoint.db2", table) as DB5Reader;
                    if (_cachedQuestPOIPointRows == null)
                    {
                        if (_questPOIPointDB2 != null)
                            _cachedQuestPOIPointRows = _questPOIPointDB2.Rows.ToArray();
                    }
                    if (_questPOIPointDB2 != null)
                        Logging.Write(_questPOIPointDB2.FileName + " loaded with " + _questPOIPointDB2.RecordsCount + " entries.");
                }
                else
                {
                    Logging.Write("DB2 QuestPOIPoint not read-able.");
                }
            }
        }

        private static BinaryReader[] _cachedQuestPOIPointRows;

        private WoWQuestPOIPoint(uint setId)
        {
            Init();
            _setPoints = new List<Point>();
            bool flag = false;
            QuestPOIPointDb2Record qpPointDb2Record = new QuestPOIPointDb2Record();

            for (int i = 0; i < _questPOIPointDB2.RecordsCount - 1; i++)
            {
                qpPointDb2Record = DB5Reader.ByteToType<QuestPOIPointDb2Record>(_cachedQuestPOIPointRows[i]);
                if (qpPointDb2Record.SetId == setId)
                {
                    _setPoints.Add(new Point(qpPointDb2Record.X, qpPointDb2Record.Y, 0));
                    flag = true;
                }
                else if (flag)
                    break;
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
                if (_center.IsValid)
                    return _center;

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
                while (z == 0 && IsInside(new Point(x + 5, y + 5, 0)))
                {
                    x += 5;
                    y += 5;
                    z = PathFinder.GetZPosition(x, y, true);
                }
                float z2 = PathFinder.GetZPosition(x, y, z + 100f);
                if (z2 - z > 5f)
                    z = z2;
                _center = new Point(x, y, z);
                return _center;
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
                if (_middlePoint.IsValid)
                    return _middlePoint;

                // We don't have it, then compute it
                _middlePoint = new Point(Center);
                _middlePoint.Z += 40.0f;
                float curZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y, _middlePoint.Z, true);
                float anotherZ = PathFinder.GetZPosition(_middlePoint.X + 8, _middlePoint.Y + 8, _middlePoint.Z, true);
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
                            curZ = PathFinder.GetZPosition(_middlePoint.X + delta, _middlePoint.Y, _middlePoint.Z, true);
                            anotherZ = PathFinder.GetZPosition(_middlePoint.X + delta + 8, _middlePoint.Y, _middlePoint.Z, true);
                            if (curZ != 0 && (uint) (anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_middlePoint.X + delta, _middlePoint.Y, curZ),
                                    new Point(_middlePoint.X + delta, _middlePoint.Y, curZ + 50)))
                            {
                                _middlePoint = new Point(_middlePoint.X + delta, _middlePoint.Y, curZ + 5f);
                                found = true;
                                continue;
                            }
                        }
                        if (IsInside(new Point(_middlePoint.X - delta, _middlePoint.Y, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_middlePoint.X - delta, _middlePoint.Y, _middlePoint.Z, true);
                            anotherZ = PathFinder.GetZPosition(_middlePoint.X - delta - 8, _middlePoint.Y, _middlePoint.Z, true);
                            if (curZ != 0 && (uint) (anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_middlePoint.X - delta, _middlePoint.Y, curZ),
                                    new Point(_middlePoint.X - delta, _middlePoint.Y, curZ + 50)))
                            {
                                _middlePoint = new Point(_middlePoint.X - delta, _middlePoint.Y, curZ + 5f);
                                found = true;
                                continue;
                            }
                        }
                        if (IsInside(new Point(_middlePoint.X, _middlePoint.Y + delta, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y + delta, _middlePoint.Z, true);
                            anotherZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y + delta + 8, _middlePoint.Z, true);
                            if (curZ != 0 && (uint) (anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_middlePoint.X, _middlePoint.Y + delta, curZ),
                                    new Point(_middlePoint.X, _middlePoint.Y + delta, curZ + 50)))
                            {
                                _middlePoint = new Point(_middlePoint.X, _middlePoint.Y + delta, curZ + 5f);
                                found = true;
                                continue;
                            }
                        }
                        if (IsInside(new Point(_middlePoint.X, _middlePoint.Y - delta, 0)))
                        {
                            curZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y - delta, _middlePoint.Z, true);
                            anotherZ = PathFinder.GetZPosition(_middlePoint.X, _middlePoint.Y - delta - 8, _middlePoint.Z, true);
                            if (curZ != 0 && (uint) (anotherZ - curZ) < 8 &&
                                !TraceLine.TraceLineGo(new Point(_middlePoint.X, _middlePoint.Y - delta, curZ),
                                    new Point(_middlePoint.X, _middlePoint.Y - delta, curZ + 50)))
                            {
                                _middlePoint = new Point(_middlePoint.X, _middlePoint.Y - delta, curZ + 5f);
                                found = true;
                            }
                        }
                    } while (!found);
                    //nManager.Helpful.Logging.Write("Not convexe or invalid: delta " + delta + " used.");
                }
                else
                {
                    _middlePoint = new Point(_middlePoint.X, _middlePoint.Y, curZ + 5f);
                }
                _middlePointSet = true;
                return _middlePoint;
            }
        }

        // An implementation of the shortest ray-trace algorythm for PiP problem I could make
        public bool IsInside(Point p)
        {
            int i, j = _setPoints.Count - 1;
            int x = (int) p.X;
            int y = (int) p.Y;
            bool oddNodes = false;
            for (i = 0; i < _setPoints.Count; i++)
            {
                if ((_setPoints[i].Y < y && _setPoints[j].Y >= y || _setPoints[j].Y < y && _setPoints[i].Y >= y)
                    && (_setPoints[i].X <= x || _setPoints[j].X <= x))
                {
                    if (_setPoints[i].X + (y - _setPoints[i].Y)/(_setPoints[j].Y - _setPoints[i].Y)*(_setPoints[j].X - _setPoints[i].X) < x)
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }
            return oddNodes;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct QuestPOIPointDb2Record
        {
            public readonly uint SetId;
            public readonly ushort X;
            public readonly ushort Y;
            public readonly uint Id;
        }
    }
}