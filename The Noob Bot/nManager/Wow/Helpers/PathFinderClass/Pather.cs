using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DetourLayer;
using nManager.Wow.Enums;
using RecastLayer;
using nManager.Helpful;
using nManager.Wow.Class;
using Math = System.Math;
using dtPolyRef = System.UInt64;

namespace nManager.Wow.Helpers.PathFinderClass
{
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
        private static object _threadLocker = new object();
        private const int Division = 2;

        public delegate bool ConnectionHandlerDelegate(ConnectionData data);

        private readonly NavMesh _mesh;
        private readonly NavMeshQuery _query;
        private readonly string _meshPath;
        private Dictionary<Tuple<int, int>, int> _loadedTiles;
        private Dictionary<Tuple<int, int>, int> _failedTiles;
        private Helpful.Timer _loadTileCheck;

        #region Memory Management

        public long MemoryPressure { get; private set; }

        private void AddMemoryPressure(long bytes)
        {
            try
            {
                if (bytes > 0)
                {
                    GC.AddMemoryPressure(bytes);
                    MemoryPressure += bytes;
                }
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
                //UnloadAllTiles();
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
                if (MemoryPressure > 0)
                {
                    GC.RemoveMemoryPressure(MemoryPressure);
                }
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

        public int ReportDanger(List<nManagerSetting.DangerousZone> dangers, bool doLoad = false)
        {
            lock (_threadLocker)
            {
                try
                {
                    float[] extents = new[] {2.5f, 2.5f, 2.5f};
                    int sum = 0;
                    foreach (var danger in dangers)
                    {
                        if (danger.ContinentId != Continent)
                            continue;
                        if (danger.ContinentId != Usefuls.ContinentNameMpq)
                            continue; // only load blacklist for the actual continent, even if pather is checking remotely.
                        if (doLoad)
                            LoadAround(danger.Position, false);
                        float[] loc = danger.Position.ToRecast().ToFloatArray();
                        ulong polyRef = _query.FindNearestPolygon(loc, extents, Filter);
                        if (polyRef != 0) sum += _query.MarkAreaInCircle(polyRef, loc, danger.Radius, Filter, PolyArea.Danger);
                    }
                    return sum;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("ReportDanger(IEnumerable<Danger> dangers): " + exception);
                    return 0;
                }
            }
        }

        public string GetTilePath(int x, int y)
        {
            lock (_threadLocker)
            {
                try
                {
                    return _meshPath + "\\" + GetTileName(x, y);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetTilePath(int x, int y): " + exception);
                    return "";
                }
            }
        }

        public string GetTileName(int x, int y, bool onlyName = false)
        {
            lock (_threadLocker)
            {
                try
                {
                    string cont = Continent;
                    int baseX = (int) (x/Division);
                    int baseY = (int) (y/Division);
                    if (Continent == "Draenor")
                    {
                        if (baseX == 23 && baseY == 21 && ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde")
                        {
                            switch (Garrison.GetGarrisonLevel())
                            {
                                case 1:
                                    cont = "FWHordeGarrisonLevel1";
                                    break;
                                case 2:
                                    cont = "FWHordeGarrisonLeve2new";
                                    break;
                                case 3:
                                    cont = "FWHordeGarrisonLevel2";
                                    break;
                            }
                        }
                        else if (baseX == 31 && baseY == 28 && ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() != "horde")
                        {
                            switch (Garrison.GetGarrisonLevel())
                            {
                                case 1:
                                    cont = "SMVAllianceGarrisonLevel1";
                                    break;
                                case 2:
                                    cont = "SMVAllianceGarrisonLevel2new";
                                    break;
                                case 3:
                                    cont = "SMVAllianceGarrisonLevel2";
                                    break;
                            }
                        }
                    }
#pragma warning disable 162
                    if (Division == 1)
                        return (onlyName ? "" : cont + "\\") + cont + "_" + x + "_" + y + ".tile";
                    else
                    {
                        float offsetI = (x*(Utility.TileSize/Division)) - (baseX*Utility.TileSize);
                        float offsetJ = (y*(Utility.TileSize/Division)) - (baseY*Utility.TileSize);
                        int i = (int) Math.Round(offsetI/(Utility.TileSize/Division));
                        int j = (int) Math.Round(offsetJ/(Utility.TileSize/Division));
                        return (onlyName ? "" : cont + "\\") + cont + "_" + baseX + "_" + baseY + "_" + i + j + ".tile";
                    }
#pragma warning restore 162
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetTileName(int x, int y, bool onlyName): " + exception);
                    return "";
                }
            }
        }

        public void GetTileByLocation(Point loc, out float x, out float y)
        {
            lock (_threadLocker)
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
        }

        public static void GetWoWTileByLocation(float[] loc, out float x, out float y)
        {
            x = (loc[0] - Utility.Origin[0])/Utility.TileSize;
            y = (loc[2] - Utility.Origin[2])/Utility.TileSize;
        }

        public static void GetTileByLocation(float[] loc, out float x, out float y)
        {
            x = (loc[0] - Utility.Origin[0])/(Utility.TileSize/Division);
            y = (loc[2] - Utility.Origin[2])/(Utility.TileSize/Division);
        }

        public void LoadAllTiles()
        {
            lock (_threadLocker)
            {
                try
                {
                    for (int y = 0; y < 64*Division; y++)
                    {
                        for (int x = 0; x < 64*Division; x++)
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
        }

        public void LoadAround(Point loc, bool applyDanger = true)
        {
            lock (_threadLocker)
            {
                try
                {
                    if (CheckDungeon())
                        return;

                    float tx, ty;
                    GetTileByLocation(loc, out tx, out ty);
                    int x = (int) Math.Floor(tx);
                    int y = (int) Math.Floor(ty);

#pragma warning disable 162
                    if (Division == 1)
                    {
                        // Depends on our settings, we may load a single mesh with no division.
                        int thirdx, thirdy;
                        LoadTile(x, y);
                        if (tx < x + 0.5f)
                            thirdx = x - 1;
                        else
                            thirdx = x + 1;
                        if (
                            ty < y + 0.5f)
                            thirdy = y - 1;
                        else
                            thirdy = y + 1;
                        LoadTile(thirdx, y);
                        LoadTile(x, thirdy);
                        LoadTile(thirdx, thirdy);
                    }
                    else
                    {
                        for (int i = -1; i < 2; i++)
                            for (int j = -1; j < 2; j++)
                                LoadTile(x + i, y + j, applyDanger);
                    }
#pragma warning restore 162
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadAround(Point loc): " + exception);
                }
            }
        }

        public bool LoadTile(byte[] data)
        {
            lock (_threadLocker)
            {
                try
                {
                    if (CheckDungeon())
                        return false;

                    MeshTile tile;
                    DetourStatus ret = _mesh.AddTile(data, out tile);
                    if (ret.IsWrongVersion())
                    {
                        Logging.WriteNavigator("This mesh tile is outdated.");
                        return false;
                    }
                    if (ret.HasFailed())
                    {
                        Logging.WriteNavigator("This mesh tile is corrupted.");
                        return false;
                    }
                    AddMemoryPressure(data.LongLength);
                    // HandleConnections(tile);
                    return true;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadTile(byte[] data): " + exception);
                    return false;
                }
            }
        }

        private bool CheckDungeon()
        {
            lock (_threadLocker)
            {
                try
                {
                    if (IsDungeon)
                        Logging.WriteError("Dungeon mesh doesn't support tiles");
                    return IsDungeon;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("CheckDungeon(): " + exception);
                    return false;
                }
            }
        }

        private readonly List<string> _loadedTilesString = new List<string>();

        public bool LoadTile(int x, int y, bool applyDanger = true)
        {
            lock (_threadLocker)
            {
                try
                {
                    if (CheckDungeon())
                        return false;

                    // To check every 1 minute minimum and unload tiles loaded more than 15 minutes ago
                    if (_loadTileCheck.IsReady)
                    {
                        //Logging.Write("Timer ready, checking loaded tile's age");
                        _loadTileCheck.Reset();
                        CheckTilesAgeAndUnload();
                        CheckFailedTilesAgeAndUnload();
                    }

                    Tuple<int, int> coords = new Tuple<int, int>(x, y);
                    if (_mesh.HasTileAt(x, y))
                    {
                        _loadedTiles[coords] = Others.TimesSec;
                        return true;
                    }
                    if (_failedTiles.ContainsKey(coords))
                    {
                        _failedTiles[coords] = Others.TimesSec; // don't retry to download that tile for another XX minutes.
                        return false;
                    }
                    string path = GetTilePath(x, y);

                    string fName = GetTileName(x, y);
                    if (!downloadTile(fName))
                        return false;
                    if (!File.Exists(path))
                        return false;
                    byte[] data = File.ReadAllBytes(path);
                    if (!LoadTile(data))
                    {
                        Others.DeleteFile(_meshPath + "\\" + fName);
                        if (!forceDownloadTile(fName))
                            return false;
                        data = File.ReadAllBytes(path);
                        if (!LoadTile(data))
                        {
                            Logging.WriteError("Problem with Meshes tile " + fName + " , cannot load it.");
                            if (!_loadedTiles.ContainsKey(coords))
                                _failedTiles[coords] = Others.TimesSec;
                            return false;
                        }
                    }
                    // loaded
                    if (applyDanger)
                    {
                        var dangers = new List<nManagerSetting.DangerousZone>();
                        foreach (var zone in nManagerSetting.DangerousZones)
                        {
                            if (Continent != zone.ContinentId)
                                continue;
                            if ((int) Math.Floor(zone.TileX) != x || (int) Math.Floor(zone.TileY) != y)
                                continue;
                            dangers.Add(zone);
                        }
                        if (dangers.Count > 0)
                        {
                            int addedDangers = ReportDanger(dangers);
                            if (addedDangers > 0)
                                Logging.WriteNavigator(addedDangers + " dangers added.");
                        }
                    }
                    _loadedTilesString.Add(GetTileName(x, y, true));
                    if (_loadedTilesString.Count >= 10)
                    {
                        string output = "";
                        foreach (string s in _loadedTilesString)
                        {
                            output = s + ", " + output;
                        }
                        _loadedTilesString.Clear();

                        Logging.WriteNavigator(output + " loaded.");
                    }
                    if (!_loadedTiles.ContainsKey(coords)) // multi thread on the same path can cause a duplicate here
                        _loadedTiles.Add(coords, Others.TimesSec);
                    return true;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadTile(int x, int y): " + exception);
                    return false;
                }
            }
        }

        private static readonly List<string> blackListMaptitle = new List<string>();

        private bool downloadTile(string fileName)
        {
            lock (_threadLocker)
            {
                if (blackListMaptitle.Contains(fileName))
                    return true;

                blackListMaptitle.Add(fileName);
                return forceDownloadTile(fileName);
            }
        }

        private bool forceDownloadTile(string fileName)
        {
            lock (_threadLocker)
            {
                try
                {
                    const string stringHttpMapBaseAddress = "http://meshes.thenoobbot.com/";

                    string stringHttpMap = stringHttpMapBaseAddress + Utility.GetDetourSupportedVersion() + "/";
                    var continentDir = fileName.Split('\\');
                    Directory.CreateDirectory(_meshPath + "\\" + continentDir[0] + "\\");

                    if (!Others.ExistFile(_meshPath + "\\" + fileName))
                    {
                        Logging.Write("Downloading \"" + fileName + "\"...");
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
        }

        private void CheckTilesAgeAndUnload()
        {
            lock (_threadLocker)
            {
                List<Tuple<int, int>> toRemove = new List<Tuple<int, int>>();
                foreach (KeyValuePair<Tuple<int, int>, int> entry in _loadedTiles)
                {
                    //Logging.Write("Found " + entry.Key.Item1 + "," + entry.Key.Item2 + " time " + entry.Value);
                    if (entry.Value < Others.TimesSec - (15*60)) // 15 * 60 = 15 mins
                    {
                        RemoveTile(entry.Key.Item1, entry.Key.Item2);
                        Logging.WriteNavigator("Unloading old tile (" + GetTileName(entry.Key.Item1, entry.Key.Item2, true) + ")");
                        toRemove.Add(entry.Key);
                    }
                }
                foreach (Tuple<int, int> entry in toRemove)
                    _loadedTiles.Remove(entry);
            }
        }

        private void UnloadAllTiles()
        {
            lock (_threadLocker)
            {
                Logging.WriteNavigator("Unloading all tiles...");

                List<Tuple<int, int>> toRemove = new List<Tuple<int, int>>();
                foreach (KeyValuePair<Tuple<int, int>, int> entry in _loadedTiles)
                {
                    RemoveTile(entry.Key.Item1, entry.Key.Item2);
                    toRemove.Add(entry.Key);
                }
                foreach (Tuple<int, int> entry in toRemove)
                    _loadedTiles.Remove(entry);
            }
        }

        private void CheckFailedTilesAgeAndUnload()
        {
            lock (_threadLocker)
            {
                List<Tuple<int, int>> toRemove = new List<Tuple<int, int>>();
                foreach (KeyValuePair<Tuple<int, int>, int> entry in _failedTiles)
                {
                    //Logging.Write("Found " + entry.Key.Item1 + "," + entry.Key.Item2 + " time " + entry.Value);
                    if (entry.Value < Others.TimesSec - (15*60)) // 15 * 60 = 15 mins
                    {
                        Logging.WriteNavigator("Allowing to retry failed tile (" + GetTileName(entry.Key.Item1, entry.Key.Item2, true) + ")");
                        toRemove.Add(entry.Key);
                    }
                }
                foreach (Tuple<int, int> entry in toRemove)
                    _failedTiles.Remove(entry);
            }
        }

        public bool RemoveTile(int x, int y, out byte[] tileData)
        {
            lock (_threadLocker)
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
        }

        public bool RemoveTile(int x, int y)
        {
            lock (_threadLocker)
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
        }

        public List<Point> FindLocalPath(Point startVec, Point endVec)
        {
            lock (_threadLocker)
            {
                try
                {
                    bool b;
                    bool p;
                    return FindPathSimple(startVec, endVec, out b, out p, true);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("FindLocalpath(Point startVec, Point endVec): " + exception);
                    return new List<Point>();
                }
            }
        }

        public List<Point> FindPath(Point startVec, Point endVec)
        {
            lock (_threadLocker)
            {
                try
                {
                    bool b;
                    bool p;
                    return FindPath(startVec, endVec, out b, out p);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("FindPath(Point startVec, Point endVec): " + exception);
                    return new List<Point>();
                }
            }
        }

        public List<Point> FindPath(Point startVec, Point endVec, out bool resultSuccess, out bool failpolyref)
        {
            lock (_threadLocker)
            {
                List<Point> path = FindPathSimple(startVec, endVec, out resultSuccess, out failpolyref);
                if (path == null || path.Count < 2)
                {
                    resultSuccess = false;
                    return new List<Point>();
                }
                if ((endVec - startVec).Magnitude < 3000) // 5000 is about the distance from Ironforge to Lockmodan Flying
                    return path; // The path finder is able to find this kind of path easiely in one run, so if < 3000, then no need to search more, we won't find better
                float diff = (endVec - path[path.Count - 1]).Magnitude;
                float ndiff = 5f;
                while (ndiff < diff)
                {
                    int limit = (int) (path.Count*0.80f);
                    List<Point> path2;
                    path2 = FindPathSimple(path[limit], endVec, out resultSuccess, out failpolyref);
                    if (path2 == null)
                    {
                        resultSuccess = false;
                        return new List<Point>();
                    }
                    ndiff = (endVec - path2[path2.Count - 1]).Magnitude;
                    if (ndiff < diff)
                    {
                        for (int j = path.Count - 1; j > limit; j--)
                        {
                            path.RemoveAt(j);
                        }
                        foreach (Point p in path2)
                            path.Add(p);
                        diff = ndiff;
                        ndiff = 5f;
                    }
                }
                return path;
            }
        }

        public List<Point> FindPathSimple(Point startVec, Point endVec, out bool resultSuccess, out bool failedPolyref, bool ShortPath = false)
        {
            lock (_threadLocker)
            {
                try
                {
                    failedPolyref = false;
                    resultSuccess = true;
                    float[] extents = new Point(4.5f, 200.0f, 4.5f).ToFloatArray();
                    float[] start = startVec.ToRecast().ToFloatArray();
                    float[] end = endVec.ToRecast().ToFloatArray();

                    if (!IsDungeon)
                    {
                        LoadAround(startVec);
                        LoadAround(endVec);
                    }

                    dtPolyRef startRef = _query.FindNearestPolygon(start, extents, Filter);
                    if (startRef == 0)
                    {
                        var startVec2 = startVec;
                        startVec2.Z = GetZ(new Point(startVec));
                        var st2 = startVec2.ToRecast().ToFloatArray();
                        startRef = _query.FindNearestPolygon(st2, extents, Filter);
                        if (startRef > 0)
                        {
                            startVec = startVec2;
                            start = st2;
                        }
                        else
                        {
                            failedPolyref = true;
                            Logging.WriteNavigator(DetourStatus.Failure + " No polyref found for start (" + startVec + ")");
                        }
                    }

                    dtPolyRef endRef = _query.FindNearestPolygon(end, extents, Filter);
                    if (endRef == 0)
                    {
                        failedPolyref = true;
                        Logging.WriteNavigator(DetourStatus.Failure + " No polyref found for end (" + endVec + ")");
                    }

                    if (startRef == 0 || endRef == 0)
                        return new List<Point>();

                    dtPolyRef[] pathCorridor;
                    DetourStatus status;
                    if (ShortPath)
                        status = _query.FindLocalPath(startRef, endRef, start, end, Filter, out pathCorridor);
                    else
                        status = _query.FindPath(startRef, endRef, start, end, Filter, out pathCorridor);
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
                    dtPolyRef[] pathRefs;
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
                    failedPolyref = true;
                }
                return new List<Point>();
            }
        }

        public Point GetClosestPointOnTile(Point position, out bool success)
        {
            lock (_threadLocker)
            {
                float[] extents = new Point(20.0f, 2000.0f, 20.0f).ToFloatArray();
                float[] center = position.ToRecast().ToFloatArray();

                float tx, ty;
                GetTileByLocation(position, out tx, out ty);
                int x = (int) Math.Floor(tx);
                int y = (int) Math.Floor(ty);
                LoadTile(x, y);

                dtPolyRef startRef = _query.FindNearestPolygon(center, extents, Filter);
                if (startRef == 0)
                {
                    success = false;
                    return new Point();
                }
                float[] result;
                DetourStatus status = _query.closestPointOnPolyBoundary(startRef, center, out result);
                if (status.HasFailed())
                {
                    success = false;
                    return new Point();
                }
                success = true;
                return new Point(result.ToWoW());
            }
        }

        public float GetZ(Point position, bool strict = false)
        {
            lock (_threadLocker)
            {
                float[] extents = strict ? new Point(0.5f, 2000.0f, 0.5f).ToFloatArray() : new Point(1.5f, 2000.0f, 1.5f).ToFloatArray();
                float[] center = position.ToRecast().ToFloatArray();

                float tx, ty;
                GetTileByLocation(position, out tx, out ty);
                int x = (int) Math.Floor(tx);
                int y = (int) Math.Floor(ty);
                LoadTile(x, y);

                dtPolyRef startRef = _query.FindNearestPolygon(center, extents, Filter);
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
        }

        private string GetDungeonPath()
        {
            try
            {
                return "Dungeons\\" + Continent + ".dmesh";
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
            lock (_threadLocker)
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
                    _loadedTiles = new Dictionary<Tuple<int, int>, int>();
                    _failedTiles = new Dictionary<Tuple<int, int>, int>();
                    if (_loadTileCheck == null)
                        _loadTileCheck = new Helpful.Timer(60*1000); // 1 min
                    DetourStatus status;

                    // check if this is a dungeon and initialize our mesh accordingly
                    WoWMap map = WoWMap.FromMPQName(continent);
                    if (map.Record.MapType == WoWMap.MapType.WDTOnlyType || continent == "AllianceGunship")
                    {
                        string dungeonPath = GetDungeonPath();
                        if (!File.Exists(_meshPath + "\\" + dungeonPath))
                            downloadTile(dungeonPath);
                        byte[] data = File.ReadAllBytes(_meshPath + "\\" + dungeonPath);
                        status = _mesh.Initialize(data);
                        AddMemoryPressure(data.LongLength);
                        IsDungeon = true;
                    }
                    else //                       20bits 28bits
                        status = _mesh.Initialize(1048576, 2048*Division*Division, Utility.Origin, Utility.TileSize/Division, Utility.TileSize/Division);
                    // maxPolys = 1 << polyBits (20) = 1048576
                    // maxTiles = is 8192 (was 4096), Travel loads tons of tile in quester.
                    // I have logs with over 6000 .tile files loaded.

                    if (status.HasFailed())
                        Logging.WriteNavigator(status + " Failed to initialize the mesh");

                    _query = new NavMeshQuery(new PatherCallback(this));
                    DetourStatus t = _query.Initialize(_mesh, 524287); // 20bits - 1
                    //Logging.WriteDebug("NavMeshQuery initialized with status: " + t);
                    Filter = new QueryFilter {IncludeFlags = 0xFFFF, ExcludeFlags = 0x0};
                    // Add the costs
                    Filter.SetAreaCost((int) PolyArea.Water, 4);
                    Filter.SetAreaCost((int) PolyArea.Terrain, 1);
                    Filter.SetAreaCost((int) PolyArea.Road, 1); // This is the Taxi system, not in tiles yet
                    Filter.SetAreaCost((int) PolyArea.Danger, 25);
                    if (nManagerSetting.DangerousZones.Count > 0)
                    {
                        int addedDangers = ReportDanger(nManagerSetting.DangerousZones, true);
                        if (addedDangers > 0)
                            Logging.WriteNavigator(addedDangers + " dangers added.");
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Pather(string continent, ConnectionHandlerDelegate connectionHandler): " + exception);
                }
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
            lock (_threadLocker)
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
        }

        private void HandlePathfinderUpdate(float[] best)
        {
            lock (_threadLocker)
            {
                try
                {
                    // no dynamic tile loading with dungeon mesh
                    if (IsDungeon)
                        return;

                    LoadAround(new Point(best.ToWoW()));

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
        }

// ReSharper disable UnusedMember.Local
        private void LoadDynamic(int x, int y)
// ReSharper restore UnusedMember.Local
        {
            lock (_threadLocker)
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