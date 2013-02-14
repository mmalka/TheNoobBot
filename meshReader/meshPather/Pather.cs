using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DetourLayer;
using meshDatabase.Database;
using Microsoft.Xna.Framework;
using RecastLayer;

namespace meshPather
{
    
    public class Danger
    {
        public Vector3 Location { get; private set; }
        public float Radius { get; private set; }

        public Danger(Vector3 loc, float rad)
        {
            Location = loc;
            Radius = rad;
        }

        public Danger(Vector3 loc, int levelDifference)
            : this(loc, levelDifference, 1.0f)
        {
            
        }

        public Danger(Vector3 loc, int levelDifference, float factor)
        {
            Location = loc;
            Radius = levelDifference*3.5f + 10;
            Radius *= factor;
            if (levelDifference < 0)
                Radius = -Radius;
        }
    }

    public class NavMeshException : Exception
    {
        public DetourStatus Status { get; private set; }

        public NavMeshException(DetourStatus status, string text)
            : base(text + " (" + status + ")")
        {
            Status = status;
        }
    }

    public class ConnectionData
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool Horde { get; set; }
        public bool Alliance { get; set; }
        public int Cost { get; set; }
    }

    public class Pather
    {
        public delegate bool ConnectionHandlerDelegate(ConnectionData data);

        private readonly NavMesh _mesh;
        private readonly NavMeshQuery _query;
        private readonly string _meshPath;
        private System.Collections.Generic.List<string> _missingTile;

        #region Memory Management

        public int MemoryPressure { get; private set; }

        private void AddMemoryPressure(int bytes)
        {
            GC.AddMemoryPressure(bytes);
            MemoryPressure += bytes;
        }

        #endregion

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
            var extents = new[] {2.5f, 2.5f, 2.5f};
            return (from danger in dangers
                    let loc = danger.Location.ToRecast().ToFloatArray()
                    let polyRef = Query.FindNearestPolygon(loc, extents, Filter)
                    where polyRef != 0
                    select Query.MarkAreaInCircle(polyRef, loc, danger.Radius, Filter, PolyArea.Danger)).Sum();
        }

        public string GetTilePath(int x, int y)
        {
            return _meshPath + "\\" + Continent + "_" + x + "_" + y + ".tile";
        }

        public void GetTileByLocation(Vector3 loc, out float x, out float y)
        {
            CheckDungeon();

            var input = loc.ToRecast().ToFloatArray();
            float fx, fy;
            GetTileByLocation(input, out fx, out fy);
            x = /*(int)Math.Floor(fx)*/ fx;
            y = /*(int)Math.Floor(fy)*/ fy;
        }

        public static void GetTileByLocation(float[] loc, out float x, out float y)
        {
            x = (loc[0] - Utility.Origin[0])/Utility.TileSize;
            y = (loc[2] - Utility.Origin[2])/Utility.TileSize;
        }

        public void LoadAllTiles()
        {
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (!File.Exists(GetTilePath(x, y)))
                        continue;

                    LoadTile(x, y);
                }
            }
        }

        public void LoadAround(Vector3 loc)
        {
            CheckDungeon();

            //int tx, ty;
            float tx, ty;
            GetTileByLocation(loc, out tx, out ty);
            int x = (int)Math.Floor(tx);
            int y = (int)Math.Floor(ty);
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

        public bool LoadTile(byte[] data)
        {
            CheckDungeon();

            MeshTile tile;
            if (_mesh.AddTile(data, out tile).HasFailed())
                return false;
            AddMemoryPressure(data.Length);
            HandleConnections(tile);
            return true;
        }

        private void CheckDungeon()
        {
            if (IsDungeon)
                throw new NavMeshException(DetourStatus.Failure, "Dungeon mesh doesn't support tiles");
        }

        public bool LoadTile(int x, int y)
        {
            CheckDungeon();

            if (_mesh.HasTileAt(x, y))
                return true;
            var path = GetTilePath(x, y);
            if (!File.Exists(path))
            {
                if (!_missingTile.Contains(path))
                {
                    _missingTile.Add(path);
                    Console.WriteLine(path + " manque !");
                }
                return false;
            }
            var data = File.ReadAllBytes(path);
            Console.WriteLine(path + " ok");
            return LoadTile(data);
        }

        public bool RemoveTile(int x, int y, out byte[] tileData)
        {
            return _mesh.RemoveTileAt(x, y, out tileData).HasSucceeded();
        }

        public bool RemoveTile(int x, int y)
        {
            return _mesh.RemoveTileAt(x, y).HasSucceeded();
        }

        public List<Hop> FindPath(Vector3 startVec, Vector3 endVec)
        {
            var extents = new Vector3(2.5f, 2.5f, 2.5f).ToFloatArray();
            var start = startVec.ToRecast().ToFloatArray();
            var end = endVec.ToRecast().ToFloatArray();

            if (!IsDungeon)
            {
                Console.WriteLine("Load start");
                LoadAround(startVec);
                Console.WriteLine("Load end");
                LoadAround(endVec);
            }

            var startRef = _query.FindNearestPolygon(start, extents, Filter);
            if (startRef == 0)
                throw new NavMeshException(DetourStatus.Failure, "No polyref found for start");

            var endRef = _query.FindNearestPolygon(end, extents, Filter);
            if (endRef == 0)
                throw new NavMeshException(DetourStatus.Failure, "No polyref found for end");

            uint[] pathCorridor;
            Console.WriteLine("Findpath...");
            var status = _query.FindPath(startRef, endRef, start, end, Filter, out pathCorridor);
            if (status.HasFailed() || pathCorridor == null)
                throw new NavMeshException(status, "FindPath failed, start: " + startRef + " end: " + endRef);

            if (status.HasFlag(DetourStatus.PartialResult))
                Console.WriteLine("Warning, partial result: " + status);

            float[] finalPath;
            StraightPathFlag[] pathFlags;
            uint[] pathRefs;
            status = _query.FindStraightPath(start, end, pathCorridor, out finalPath, out pathFlags, out pathRefs);
            if (status.HasFailed() || (finalPath == null || pathFlags == null || pathRefs == null))
                throw new NavMeshException(status, "FindStraightPath failed, refs in corridor: " + pathCorridor.Length);

            var ret = new List<Hop>(finalPath.Length/3);
            for (int i = 0; i < (finalPath.Length / 3); i++)
            {
                if (pathFlags[i].HasFlag(StraightPathFlag.OffMeshConnection))
                {
                    var polyRef = pathRefs[i];
                    MeshTile tile;
                    Poly poly;
                    if (_mesh.GetTileAndPolyByRef(polyRef, out tile, out poly).HasFailed() || (poly == null || tile == null))
                        throw new NavMeshException(DetourStatus.Failure, "FindStraightPath returned a hop with an unresolvable off-mesh connection");

                    int polyIndex = _mesh.DecodePolyIndex(polyRef);
                    int pathId = -1;
                    for (int j = 0; j < tile.Header.OffMeshConCount; j++)
                    {
                        var con = tile.GetOffMeshConnection(j);
                        if (con == null)
                            continue;

                        if (con.PolyId == polyIndex)
                        {
                            pathId = (int)con.UserID;
                            break;
                        }
                    }

                    if (pathId == -1)
                        throw new NavMeshException(DetourStatus.Failure, "FindStraightPath returned a hop with an poly that lacks a matching off-mesh connection");
                    ret.Add(BuildFlightmasterHop(pathId));
                }
                else
                {

                    var hop = new Hop
                                  {
                                      Location =
                                          new Vector3(finalPath[(i*3) + 0], finalPath[(i*3) + 1], finalPath[(i*3) + 2]).
                                          ToWoW(),
                                      Type = HopType.Waypoint
                                  };

                    ret.Add(hop);
                }
            }
            
            return ret;
        }

        private static Hop BuildFlightmasterHop(int pathId)
        {
            var path = TaxiHelper.GetPath(pathId);
            if (path == null)
                throw new NavMeshException(DetourStatus.Failure, "FindStraightPath returned a hop with an invalid path id");

            var from = TaxiHelper.GetNode(path.From);
            var to = TaxiHelper.GetNode(path.To);
            if (from == null || to == null)
                throw new NavMeshException(DetourStatus.Failure, "FindStraightPath returned a hop with unresolvable flight path");

            return new Hop
                       {
                           Location = from.Location,
                           FlightTarget = to.Name,
                           Type = HopType.Flightmaster
                       };
        }

        private string GetDungeonPath()
        {
            return _meshPath + "\\" + Continent + ".dmesh";
        }

        public Pather(string continent)
            : this(continent, DefaultConnectionHandler)
        {
            _missingTile = new System.Collections.Generic.List<string>();
        }

        public Pather(string continent, ConnectionHandlerDelegate connectionHandler)
        {
            ConnectionHandler = connectionHandler;

            Continent = continent.Substring(continent.LastIndexOf('\\') + 1);

            if (Directory.Exists(continent))
                _meshPath = continent;
            else
            {
                var assembly = Assembly.GetCallingAssembly().Location;
                var dir = Path.GetDirectoryName(assembly);
                if (Directory.Exists(dir + "\\Meshes"))
                    _meshPath = dir + "\\Meshes\\" + continent;
                else 
                    _meshPath = dir + "\\" + continent;
            }

            if (!Directory.Exists(_meshPath))
                throw new NavMeshException(DetourStatus.Failure, "No mesh for " + continent + " (Path: " + _meshPath + ")");

            _mesh = new NavMesh();
            DetourStatus status;

            // check if this is a dungeon and initialize our mesh accordingly
            string dungeonPath = GetDungeonPath();
            if (File.Exists(dungeonPath))
            {
                var data = File.ReadAllBytes(dungeonPath);
                status = _mesh.Initialize(data);
                AddMemoryPressure(data.Length);
                IsDungeon = true;
            }
            else //                       15bits 9bits
                status = _mesh.Initialize(32767, 511, Utility.Origin, Utility.TileSize, Utility.TileSize);

            if (status.HasFailed())
                throw new NavMeshException(status, "Failed to initialize the mesh");

            _query = new NavMeshQuery(new PatherCallback(this));
            _query.Initialize(_mesh, 65536);
            Filter = new QueryFilter {IncludeFlags = 0xFFFF, ExcludeFlags = 0x0};
        }

        private static bool DefaultConnectionHandler(ConnectionData data)
        {
            return false;
        }

        private void HandleConnections(MeshTile tile)
        {
            if (tile.Header.OffMeshConCount <= 0)
                return;

            var count = tile.Header.OffMeshConCount;
            for (int i = 0; i < count; i++)
            {
                var con = tile.GetOffMeshConnection(i);
                if (con == null)
                    continue;
                var path = TaxiHelper.GetPath((int)con.UserID);
                if (path == null)
                {
                    DisableConnection(tile, i);
                    continue;
                }
                var from = TaxiHelper.GetNode(path.From);
                var to = TaxiHelper.GetNode(path.To);
                if (from == null || to == null)
                {
                    DisableConnection(tile, i);
                    continue;
                }

                var data = new ConnectionData
                               {
                                   Alliance = from.IsAlliance || to.IsAlliance,
                                   Horde = from.IsHorde || to.IsHorde,
                                   Cost = path.Cost,
                                   From = from.Name,
                                   To = to.Name,
                                   Id = (int)con.UserID
                               };

                if (!ConnectionHandler(data))
                    DisableConnection(tile, i);
            }
        }

        private static void DisableConnection(MeshTile tile, int index)
        {
            var poly = tile.GetPolygon((ushort)(index + tile.Header.OffMeshBase));
            if (poly == null)
                return;
            poly.Disable();
        }

        private void HandlePathfinderUpdate(float[] curPoint)
        {
            // no dynamic tile loading with dungeon mesh
            if (IsDungeon)
                return;

            var point = curPoint.ToWoW();
            LoadAround(new Vector3(point[0], point[1], point[2]));
            return;

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

        private void LoadDynamic(int x, int y)
        {
            if (!_mesh.HasTileAt(x, y))
            {
                if (LoadTile(x, y))
                    Console.WriteLine("Load dynamically: " + x + " " + y);
            }
        }

        private static void HandleLog(string text)
        {
            Console.WriteLine("Log: " + text);
        }

        private class PatherCallback : NavMeshQueryCallback
        {
            private readonly Pather _parent;

            public PatherCallback(Pather parent)
            {
                _parent = parent;
            }

            public void PathfinderUpdate(float[] curPoint)
            {
                _parent.HandlePathfinderUpdate(curPoint);
            }

            public void Log(string text)
            {
                HandleLog(text);
            }
        }

    }

}