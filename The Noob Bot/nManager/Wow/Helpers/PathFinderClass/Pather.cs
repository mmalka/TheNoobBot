using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DetourLayer;
using RecastLayer;
using nManager.Helpful;
using nManager.Wow.Class;
using Math = System.Math;

namespace nManager.Wow.Helpers.PathFinderClass
{
    internal class Danger
    {
        public Point Location { get; private set; }
        public float Radius { get; private set; }

        public Danger(Point loc, float rad)
        {
            try
            {
                Location = loc;
                Radius = rad;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Danger(Point loc, float rad): " + exception);
            }
        }

        public Danger(Point loc, int levelDifference, float factor = 1.0f)
        {
            try
            {
                Location = loc;
                Radius = levelDifference*3.5f + 10;
                Radius *= factor;
                if (levelDifference < 0)
                    Radius = -Radius;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Danger(Point loc, int levelDifference, float factor = 1.0f): " + exception);
            }
        }
    }

    internal class NavMeshException : Exception
    {
        public DetourStatus Status { get; private set; }

        public NavMeshException(DetourStatus status, string text)
            : base(text + " (" + status + ")")
        {
            try
            {
                Status = status;
            }
            catch (Exception exception)
            {
                Logging.WriteError("NavMeshException(DetourStatus status, string text): " + exception);
            }
        }
    }

    internal class ConnectionData
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool Horde { get; set; }
        public bool Alliance { get; set; }
        public int Cost { get; set; }
    }

    internal class Pather
    {
        public delegate bool ConnectionHandlerDelegate(ConnectionData data);

        private readonly NavMesh _mesh;
        private readonly NavMeshQuery _query;
        private readonly string _meshPath;

        private const uint MESH_TILES_VERSION = 702;

        #region Memory Management

        public int MemoryPressure { get; private set; }

        private void AddMemoryPressure(int bytes)
        {
            try
            {
                GC.AddMemoryPressure(bytes);
                MemoryPressure += bytes;
            }
            catch (Exception exception)
            {
                Logging.WriteError("AddMemoryPressure(int bytes): " + exception);
            }
        }

        #endregion

        public void Dispose()
        {
            try
            {
                // _mesh.Dispose();
            }
            catch (Exception)
            {
            }
            try
            {
                _query.Dispose();
            }
            catch (Exception)
            {
            }
            try
            {
                GC.RemoveMemoryPressure(MemoryPressure);
                MemoryPressure = 0;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Pather > Dispose(): " + exception);
            }
        }

        public readonly bool IsDungeon;
        public QueryFilter Filter { get; private set; }
        public string Continent { get; private set; }

        public ConnectionHandlerDelegate ConnectionHandler { get; set; }

        public NavMesh Mesh
        {
            get { return _mesh; }
        }

        public NavMeshQuery Query
        {
            get { return _query; }
        }

        public int ReportDanger(IEnumerable<Danger> dangers)
        {
            try
            {
                float[] extents = new[] {2.5f, 2.5f, 2.5f};
                return (from danger in dangers
                        let loc = danger.Location.ToRecast().ToFloatArray()
                        let polyRef = Query.FindNearestPolygon(loc, extents, Filter)
                        where polyRef != 0
                        select Query.MarkAreaInCircle(polyRef, loc, danger.Radius, Filter, PolyArea.Danger)).Sum();
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReportDanger(IEnumerable<Danger> dangers): " + exception);
                return 0;
            }
        }

        public string GetTilePath(int x, int y)
        {
            try
            {
                return _meshPath + "\\" + Continent + "\\" + Continent + "_" + x + "_" + y + ".tile";
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetTilePath(int x, int y): " + exception);
                return "";
            }
        }

        public string GetTileName(int x, int y)
        {
            try
            {
                return Continent + "\\" + Continent + "_" + x + "_" + y + ".tile";
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetTileName(int x, int y): " + exception);
                return "";
            }
        }

        public void GetTileByLocation(Point loc, out float x, out float y)
        {
            try
            {
                CheckDungeon();

                float[] input = loc.ToRecast().ToFloatArray();
                float fx, fy;
                GetTileByLocation(input, out fx, out fy);
                x = fx;
                y = fy;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetTileByLocation(Point loc, out int x, out int y): " + exception);
                x = 0;
                y = 0;
            }
        }

        public static void GetTileByLocation(float[] loc, out float x, out float y)
        {
            try
            {
                x = (loc[0] - Utility.Origin[0])/Utility.TileSize;
                y = (loc[2] - Utility.Origin[2])/Utility.TileSize;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetTileByLocation(float[] loc, out float x, out float y): " + exception);
                x = 0;
                y = 0;
            }
        }

        public void LoadAllTiles()
        {
            try
            {
                for (int y = 0; y < 64; y++)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        downloadTile(GetTileName(x, y));

                        if (!File.Exists(GetTilePath(x, y)))
                            continue;

                        LoadTile(x, y);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadAllTiles(): " + exception);
            }
        }

        public void LoadAround(Point loc)
        {
            try
            {
                CheckDungeon();

                float tx, ty;
                GetTileByLocation(loc, out tx, out ty);
                int x = (int) Math.Floor(tx);
                int y = (int) Math.Floor(ty);
                int thirdx, thirdy;

                LoadTile(x, y);
                if (tx < x + 0.5f)
                    thirdx = x - 1;
                else
                    thirdx = x + 1;
                if (ty < y + 0.5f)
                    thirdy = y - 1;
                else
                    thirdy = y + 1;
                LoadTile(thirdx, y);
                LoadTile(x, thirdy);
                LoadTile(thirdx, thirdy);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadAround(Point loc): " + exception);
            }
        }

        public bool LoadTile(byte[] data)
        {
            try
            {
                CheckDungeon();

                MeshTile tile;
                if (_mesh.AddTile(data, out tile).HasFailed() || tile.Header.Version < MESH_TILES_VERSION)
                {
                    Logging.WriteNavigator("Out of date mesh tile");
                    return false;
                }
                AddMemoryPressure(data.Length);
                // HandleConnections(tile);
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadTile(byte[] data): " + exception);
                return false;
            }
        }

        private void CheckDungeon()
        {
            try
            {
                if (IsDungeon)
                    Logging.WriteError("Dungeon mesh doesn't support tiles");
            }
            catch (Exception exception)
            {
                Logging.WriteError("CheckDungeon(): " + exception);
            }
        }

        public bool LoadTile(int x, int y)
        {
            try
            {
                CheckDungeon();

                if (_mesh.HasTileAt(x, y))
                    return true;
                string path = GetTilePath(x, y);

                string fName = GetTileName(x, y);
                if (!downloadTile(fName))
                    return false;
                if (!File.Exists(path))
                    return false;
                byte[] data = File.ReadAllBytes(path);
                Logging.WriteNavigator("Load finish: " + Continent + "_" + x + "_" + y + ".tile");
                if (!LoadTile(data))
                {
                    Others.DeleteFile(_meshPath + "\\" + fName);
                    if (!forceDownloadTile(fName))
                        return false;
                    data = File.ReadAllBytes(path);
                    if (!LoadTile(data))
                    {
                        Logging.WriteError("Problem with Meshes tile " + fName + " , cannot load it.");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadTile(int x, int y): " + exception);
                return false;
            }
        }

        private static readonly List<string> blackListMaptitle = new List<string>();

        private bool downloadTile(string fileName)
        {
            if (blackListMaptitle.Contains(fileName))
                return true;

            blackListMaptitle.Add(fileName);
            return forceDownloadTile(fileName);
        }

        private bool forceDownloadTile(string fileName)
        {
            try
            {
                const string stringHttpMap = "http://meshes.thenoobbot.com/";

                Directory.CreateDirectory(_meshPath + "\\" + Continent + "\\");

                if (!Others.ExistFile(_meshPath + "\\" + fileName))
                {
                    Logging.WriteNavigator("Downloading mesh tile \"" + fileName + "\"");
                    if (!Others.DownloadFile(stringHttpMap + fileName.Replace("\\", "/") + ".gz",
                                             _meshPath + "\\" + fileName + ".gz"))
                        return false;
                    if (!GZip.Decompress(_meshPath + "\\" + fileName + ".gz"))
                        return false;
                    if (Others.ExistFile(_meshPath + "\\" + fileName + ".gz"))
                        File.Delete(_meshPath + "\\" + fileName + ".gz");
                    if (Others.ExistFile(_meshPath + "\\" + fileName))
                        return true;
                    return false;
                }

                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("forceDownloadTile(string fileName): " + exception);
                return false;
            }
        }

        public bool RemoveTile(int x, int y, out byte[] tileData)
        {
            try
            {
                return _mesh.RemoveTileAt(x, y, out tileData).HasSucceeded();
            }
            catch (Exception exception)
            {
                Logging.WriteError("RemoveTile(int x, int y, out byte[] tileData): " + exception);
                tileData = new byte[0];
                return false;
            }
        }

        public bool RemoveTile(int x, int y)
        {
            try
            {
                return _mesh.RemoveTileAt(x, y).HasSucceeded();
            }
            catch (Exception exception)
            {
                Logging.WriteError("RemoveTile(int x, int y): " + exception);
                return false;
            }
        }

        public List<Point> FindPath(Point startVec, Point endVec)
        {
            try
            {
                bool b;
                return FindPath(startVec, endVec, out b);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point startVec, Point endVec): " + exception);
                return new List<Point>();
            }
        }

        public List<Point> FindPath(Point startVec, Point endVec, out bool resultSuccess)
        {
            try
            {
                resultSuccess = true;
                float[] extents = new Point(4.5f, 200.0f, 4.5f).ToFloatArray();
                float[] start = startVec.ToRecast().ToFloatArray();
                float[] end = endVec.ToRecast().ToFloatArray();

                if (!IsDungeon)
                {
                    LoadAround(startVec);
                    LoadAround(endVec);
                }

                uint startRef = _query.FindNearestPolygon(start, extents, Filter);
                if (startRef == 0)
                    Logging.WriteNavigator(DetourStatus.Failure + " No polyref found for start (" + startVec + ")");

                uint endRef = _query.FindNearestPolygon(end, extents, Filter);
                if (endRef == 0)
                    Logging.WriteNavigator(DetourStatus.Failure + " No polyref found for end (" + endVec + ")");

                if (startRef == 0 || endRef == 0)
                    return new List<Point>();

                uint[] pathCorridor;
                DetourStatus status = _query.FindPath(startRef, endRef, start, end, Filter, out pathCorridor);
                if (status.HasFailed() || pathCorridor == null)
                {
                    Logging.WriteNavigator(status + " FindPath failed, start: " + startRef + " end: " + endRef);
                    return new List<Point>();
                }

                if (status.HasFlag(DetourStatus.PartialResult))
                {
                    Logging.WriteNavigator("Warning, partial result: " + status);
                    resultSuccess = false;
                }

                float[] finalPath;
                StraightPathFlag[] pathFlags;
                uint[] pathRefs;
                status = _query.FindStraightPath(start, end, pathCorridor, out finalPath, out pathFlags, out pathRefs);
                if (status.HasFailed() || (finalPath == null || pathFlags == null || pathRefs == null))
                    Logging.WriteNavigator(status + "FindStraightPath failed, refs in corridor: " + pathCorridor.Length);

                if (finalPath != null)
                {
                    List<Point> resultPath = new List<Point>(finalPath.Length/3);
                    for (int i = 0; i < (finalPath.Length/3); i++)
                    {
                        resultPath.Add(
                            new Point(finalPath[(i*3) + 0], finalPath[(i*3) + 1], finalPath[(i*3) + 2]).ToWoW());
                    }

                    return resultPath;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point startVec, Point endVec, out bool resultSuccess): " + exception);
                resultSuccess = false;
            }
            return new List<Point>();
        }

        public float GetZ(Point position, bool strict = false)
        {
            float[] extents = strict ? new Point(0.5f, 2000.0f, 0.5f).ToFloatArray() : new Point(1.5f, 2000.0f, 1.5f).ToFloatArray();
            float[] center = position.ToRecast().ToFloatArray();

            float tx, ty;
            GetTileByLocation(position, out tx, out ty);
            int x = (int) Math.Floor(tx);
            int y = (int) Math.Floor(ty);
            LoadTile(x, y);

            uint startRef = _query.FindNearestPolygon(center, extents, Filter);
            if (startRef == 0)
            {
                Logging.WriteDebug("There is no polygon in this location (Tile " + x + "," + y + "), coord: X:" +
                                   position.X + ", Y:" + position.Y);
                return 0;
            }
            float z = _query.GetPolyHeight(startRef, center);
            if (z == 0 && !strict) // it failed but we are not strict, then search around
            {
                float[] result;
                DetourStatus status = _query.closestPointOnPolyBoundary(startRef, center, out result);
                z = status.HasFailed() ? 0 : result[1];
            }
            return z;
        }

        private string GetDungeonPath()
        {
            try
            {
                return _meshPath + "\\" + Continent + ".dmesh";
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetDungeonPath(): " + exception);
                return "";
            }
        }

        public Pather(string continent)
            : this(continent, DefaultConnectionHandler)
        {
        }

        public Pather(string continent, ConnectionHandlerDelegate connectionHandler)
        {
            try
            {
                ConnectionHandler = connectionHandler;

                Continent = continent.Substring(continent.LastIndexOf('\\') + 1);

                string dir = Application.StartupPath;
                _meshPath = dir + "\\Meshes"; // + continent;


                if (!Directory.Exists(_meshPath))
                    Logging.WriteNavigator(DetourStatus.Failure + " No mesh for " + continent + " (Path: " + _meshPath +
                                           ")");

                _mesh = new NavMesh();
                DetourStatus status;

                // check if this is a dungeon and initialize our mesh accordingly
                string dungeonPath = GetDungeonPath();
                if (File.Exists(dungeonPath))
                {
                    byte[] data = File.ReadAllBytes(dungeonPath);
                    status = _mesh.Initialize(data);
                    AddMemoryPressure(data.Length);
                    IsDungeon = true;
                }
                else //                       15bits 9bits
                    status = _mesh.Initialize(32767, 511, Utility.Origin, Utility.TileSize, Utility.TileSize);

                if (status.HasFailed())
                    Logging.WriteNavigator(status + " Failed to initialize the mesh");

                _query = new NavMeshQuery(new PatherCallback(this));
                _query.Initialize(_mesh, 65536);
                Filter = new QueryFilter {IncludeFlags = 0xFFFF, ExcludeFlags = 0x0};
                // Add the costs
                Filter.SetAreaCost((int) PolyArea.Water, 4);
                Filter.SetAreaCost((int) PolyArea.Terrain, 1);
                Filter.SetAreaCost((int) PolyArea.Road, 1); // This is the Taxi system, not in tiles yet
                Filter.SetAreaCost((int) PolyArea.Danger, 20);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Pather(string continent, ConnectionHandlerDelegate connectionHandler): " + exception);
            }
        }

        private static bool DefaultConnectionHandler(ConnectionData data)
        {
            try
            {
                return false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DefaultConnectionHandler(ConnectionData data): " + exception);
                return false;
            }
        }

// ReSharper disable UnusedMember.Local
        private static void DisableConnection(MeshTile tile, int index)
// ReSharper restore UnusedMember.Local
        {
            try
            {
                Poly poly = tile.GetPolygon((ushort) (index + tile.Header.OffMeshBase));
                if (poly == null)
                    return;
                poly.Disable();
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisableConnection(MeshTile tile, int index): " + exception);
            }
        }

        private void HandlePathfinderUpdate(float[] best)
        {
            try
            {
                // no dynamic tile loading with dungeon mesh
                if (IsDungeon)
                    return;

                float[] point = best.ToWoW();
                LoadAround(new Point(point[0], point[1], point[2]));

                /*float tx, ty;
                GetTileByLocation(best, out tx, out ty);
                var currentX = (int) Math.Floor(tx);
                var currentY = (int) Math.Floor(ty);
                var diffX = Math.Abs((currentX + 1) - tx);
                var diffY = Math.Abs((currentY + 1) - ty);

                Console.WriteLine("DynamicTileLoading: " + tx + " " + ty);
                HeatMap.Add(new KeyValuePair<float, float>(tx, ty));

                const float threshold = 0.7f;

                int addX = 0;
                int addY = 0;
                if (diffX < threshold)
                    addX = 1;
                else if (diffX > (1 - threshold))
                    addX = -1;
                if (diffY < threshold)
                    addY = 1;
                else if (diffY > (1 - threshold))
                    addY = -1;

                if (addX != 0 || addY != 0)
                {
                    LoadDynamic(currentX + addX, currentY);
                    LoadDynamic(currentX, currentY + addY);
                    LoadDynamic(currentX + addX, currentY + addY);
                }*/
            }
            catch (Exception exception)
            {
                Logging.WriteError("HandlePathfinderUpdate(float[] best): " + exception);
            }
        }

// ReSharper disable UnusedMember.Local
        private void LoadDynamic(int x, int y)
// ReSharper restore UnusedMember.Local
        {
            try
            {
                if (!_mesh.HasTileAt(x, y))
                {
                    if (LoadTile(x, y))
                        Logging.WriteNavigator("Load dynamically: " + x + " " + y);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadDynamic(int x, int y): " + exception);
            }
        }

        private static void HandleLog(string text)
        {
            try
            {
                Logging.WriteNavigator(text);
            }
            catch (Exception exception)
            {
                Logging.WriteError("HandleLog(string text): " + exception);
            }
        }

        private class PatherCallback : NavMeshQueryCallback
        {
            private readonly Pather _parent;

            public PatherCallback(Pather parent)
            {
                try
                {
                    _parent = parent;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PatherCallback(Pather parent): " + exception);
                }
            }

            public void PathfinderUpdate(float[] best)
            {
                try
                {
                    _parent.HandlePathfinderUpdate(best);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PathfinderUpdate(float[] best): " + exception);
                }
            }

            public void Log(string text)
            {
                try
                {
                    HandleLog(text);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Log(string text): " + exception);
                }
            }
        }
    }
}