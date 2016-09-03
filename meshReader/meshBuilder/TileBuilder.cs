// #define TIMETHIS

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DetourLayer;
using meshDatabase;
using meshDatabase.Database;
using meshReader;
using meshReader.Game;
using meshReader.Game.ADT;
using meshReader.Game.Caching;
using RecastLayer;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

namespace meshBuilder
{
    public class TileBuilder
    {
        public string World { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int MapId { get; private set; }

        public RecastConfig Config { get; private set; }
        public Geometry Geometry { get; private set; }
        public RecastContext Context { get; private set; }
        public BaseLog Log { get; private set; }

        private static string GetAdtPath(string world, int x, int y)
        {
            return "World\\Maps\\" + world + "\\" + world + "_" + x + "_" + y + ".adt";
        }

        private static Mutex _mutex = null;

        public TileBuilder(string world, int x, int y)
        {
            World = world;
            X = x;
            Y = y;
            Config = RecastConfig.Default;
            MapId = PhaseHelper.GetMapIdByName(World);

            // Now create a global mutex
            if (_mutex == null)
            {
                string appGuid = ((GuidAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
                string mutexId = string.Format("Global\\{{{0}}}", appGuid);
                _mutex = new Mutex(false, mutexId);
                MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
                MutexSecurity securitySettings = new MutexSecurity();
                securitySettings.AddAccessRule(allowEveryoneRule);
                _mutex.SetAccessControl(securitySettings);
            }
        }

        private void CalculateTileBounds2(out float[] bmin, out float[] bmax, int i = 0, int j = 0)
        {
            var origin = meshReader.Game.World.Origin;
            bmin = new float[3];
            bmax = new float[3];
            bmin[0] = origin[0] + (Constant.TileSize*i*Constant.Division);
            bmin[2] = origin[2] + (Constant.TileSize*j*Constant.Division);
            bmax[0] = origin[0] + (Constant.TileSize*(i + 1)*Constant.Division);
            bmax[2] = origin[2] + (Constant.TileSize*(j + 1)*Constant.Division);
        }

        private void CalculateTileBounds(out float[] bmin, out float[] bmax, bool forBaseTile = false, int i = 0, int j = 0)
        {
            var origin = meshReader.Game.World.Origin;
            bmin = new float[3];
            bmax = new float[3];
            if (forBaseTile || Constant.Division == 1)
            {
                bmin[0] = origin[0] + (Constant.TileSize*X);
                bmin[2] = origin[2] + (Constant.TileSize*Y);
                bmax[0] = origin[0] + (Constant.TileSize*(X + 1));
                bmax[2] = origin[2] + (Constant.TileSize*(Y + 1));
            }
            else
            {
                bmin[0] = origin[0] + (Constant.TileSize*X*Constant.Division) + (Constant.TileSize*i);
                bmin[2] = origin[2] + (Constant.TileSize*Y*Constant.Division) + (Constant.TileSize*j);
                bmax[0] = origin[0] + (Constant.TileSize*X*Constant.Division) + (Constant.TileSize*(i + 1));
                bmax[2] = origin[2] + (Constant.TileSize*Y*Constant.Division) + (Constant.TileSize*(j + 1));
            }
        }

        public static ADT GetAdt(string world, int x, int y)
        {
            //if (Cache.Adt.TryGetValue(new Tuple<int, int>(x, y), out adt))
            //    return adt;
            ADT adt = new ADT(GetAdtPath(world, x, y));
            if (adt.HasObjectData)
                adt.Read();
            //Cache.Adt.Add(new Tuple<int, int>(x, y), adt);
            return adt;
        }

        public void InsertAllGameobjectGeometry(int x, int y, int map)
        {
            float[] bbMin, bbMax;
            CalculateTileBounds2(out bbMin, out bbMax, x, y);
            foreach (GameObject go in GameObjectHelper.GetAllGameobjectInBoundingBox(bbMin, bbMax, map))
            {
                Geometry.AddGameObject(go);

                /*if (!IsGeometryFine(Geometry))
                {
                    Console.WriteLine("Broken after adding GameObject " + go.Model);
                }*/
            }
        }

        public bool IsGeometryFine(Geometry geo)
        {
            try
            {
                float sum = 0;
                foreach (var t in geo.Triangles)
                {
                    sum += geo.Vertices[(int)t.V0].X + geo.Vertices[(int)t.V0].Y + geo.Vertices[(int)t.V0].Z;
                    sum += geo.Vertices[(int)t.V1].X + geo.Vertices[(int)t.V1].Y + geo.Vertices[(int)t.V1].Z;
                    sum += geo.Vertices[(int)t.V2].X + geo.Vertices[(int)t.V2].Y + geo.Vertices[(int)t.V2].Z;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void PrepareData(BaseLog log)
        {
            if (log == null) throw new ArgumentNullException("log");

            Log = log;
            Cache.Clear();

            Geometry = new Geometry {Transform = true};
            //List<ADT> adtList = new List<ADT>();

            bool hasHandle = false;
            try
            {
                try
                {
                    hasHandle = _mutex.WaitOne(Timeout.Infinite, false);
                    if (hasHandle == false)
                        throw new TimeoutException("Timeout waiting for exclusive access");
                }
                catch (AbandonedMutexException)
                {
                    // The mutex was abandoned in another process, it will still get aquired
                    hasHandle = true;
                }

                // Do the work which require this mutex locking (useless with CACS)
                //{
                ADT main = GetAdt(World, X, Y);
                Geometry.AddAdt(main);
                InsertAllGameobjectGeometry(X, Y, MapId);
                //}

                if (Geometry.Vertices.Count == 0 && Geometry.Triangles.Count == 0)
                    throw new InvalidOperationException("Can't build tile with empty geometry");

                // again, we load everything - wasteful but who cares
                for (int ty = Y - 1; ty <= Y + 1; ty++)
                {
                    for (int tx = X - 1; tx <= X + 1; tx++)
                    {
                        // don't load main tile again
                        if (tx == X && ty == Y)
                            continue;

                        ADT adt = GetAdt(World, tx, ty);
                        if (adt.HasObjectData)
                        {
                            //Console.WriteLine("-> " + World + "_" + tx + "_" + ty);
                            Geometry.AddAdt(adt);
                            InsertAllGameobjectGeometry(tx, ty, MapId);
                        }
                        else
                        {
                            string parentMap = PhaseHelper.GetParentMap(World);
                            if (parentMap != string.Empty)
                            {
                                ADT adtParent = GetAdt(parentMap, tx, ty);
                                if (adtParent.HasObjectData)
                                {
                                    Console.WriteLine("-> " + parentMap + "_" + tx + "_" + ty);
                                    Geometry.AddAdt(adtParent);
                                    InsertAllGameobjectGeometry(tx, ty, PhaseHelper.GetMapIdByName(parentMap));
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (hasHandle)
                    _mutex.ReleaseMutex();
            }

            Context = new RecastContext();
            Context.SetContextHandler(Log);
        }

        public byte[] Build(int i = 0, int j = 0)
        {
            float[] bbMin, bbMax;
            CalculateTileBounds(out bbMin, out bbMax, false, i, j);

#if (TIMETHIS)
                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();
                long cur = 0;
            #endif
            // add border
            bbMin[0] -= Config.BorderSize*Config.CellSize;
            bbMin[2] -= Config.BorderSize*Config.CellSize;
            bbMax[0] += Config.BorderSize*Config.CellSize;
            bbMax[2] += Config.BorderSize*Config.CellSize;

            // get raw geometry - lots of slowness here
            float[] vertices;
            int[] triangles;
            byte[] areas;

            Geometry.GetRawData(out vertices, out triangles, out areas);
#if (TIMETHIS)
                Console.WriteLine("GetRawData: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
#endif
            // Following code would check if we have Index out of range error while computing the sum. the result itself is useless.
            /*float sum = 0;
            foreach (int i2 in triangles)
                sum += vertices[i2*3+0] + vertices[i2*3+1] + vertices[i2*3+2];
            Console.WriteLine(sum);*/

            // now we can find the min/max height for THIS tile
            float MinHeight, MaxHeight;
            Geometry.CalculateMinMaxHeight(out MinHeight, out MaxHeight, bbMin, bbMax);
            //Geometry.Triangles.Clear(); // We should not clear triangles and stuff because we have severals files for one tile.
            //Geometry.Vertices.Clear(); // It will be reset at the next tile anyway, no memory issues.
            bbMin[1] = MinHeight;
            bbMax[1] = MaxHeight;

            Heightfield hf;
            int width = Config.TileWidth + (Config.BorderSize*2);
            if (!Context.CreateHeightfield(out hf, width, width, bbMin, bbMax, Config.CellSize, Config.CellHeight))
                throw new OutOfMemoryException("CreateHeightfield ran out of memory");

#if (TIMETHIS)
                Console.WriteLine("CreateHeightfield: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif

            if (triangles.Count() > 0)
            {
                /*Console.WriteLine("Context.ClearUnwalkableTriangles: verticles: " + vertices.Length + ", triangles: " + triangles.Length + ", areas: " + areas.Length);
                Console.WriteLine("Memory allocated GC: " + GC.GetTotalMemory(true));/*/
                Context.ClearUnwalkableTriangles(Config.WalkableSlopeAngle, ref vertices, ref triangles, areas);
#if (TIMETHIS)
                    Console.WriteLine("ClearUnwalkableTriangles: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                    cur = stopWatch.ElapsedMilliseconds;
                #endif
                Context.RasterizeTriangles(ref vertices, ref triangles, ref areas, hf, Config.WalkableClimb);
#if (TIMETHIS)
                    Console.WriteLine("RasterizeTriangles: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                    cur = stopWatch.ElapsedMilliseconds;
                #endif

                GC.KeepAlive(vertices); // force C# to keep vertices, triangles and areas alive while it's in unamanaged code.
                GC.KeepAlive(triangles);
                GC.KeepAlive(areas);
            }
            vertices = null;
            triangles = null;
            areas = null;
            GC.Collect();

            // Once all geometry is rasterized, we do initial pass of filtering to
            // remove unwanted overhangs caused by the conservative rasterization
            // as well as filter spans where the character cannot possibly stand.
            Context.FilterLowHangingWalkableObstacles(Config.WalkableClimb, hf);
#if (TIMETHIS)
                Console.WriteLine("FilterLowHangingWalkableObstacles: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif
            Context.FilterLedgeSpans(Config.WalkableHeight, Config.WalkableClimb, hf);
#if (TIMETHIS)
                Console.WriteLine("FilterLedgeSpans: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif
            Context.FilterWalkableLowHeightSpans(Config.WalkableHeight, hf);
#if (TIMETHIS)
                Console.WriteLine("FilterWalkableLowHeightSpans: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif

            // Rasterize once again after the cleanup we did
            //Context.RasterizeTriangles(ref vertices, ref triangles, ref areas, hf, Config.WalkableClimb);
            //#if (TIMETHIS)
            //    Console.WriteLine("RasterizeTriangles: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
            //    cur = stopWatch.ElapsedMilliseconds;
            //#endif

            // Compact the heightfield so that it is faster to handle from now on.
            // This will result in more cache coherent data as well as the neighbours
            // between walkable cells will be calculated.
            CompactHeightfield chf;
            if (!Context.BuildCompactHeightfield(Config.WalkableHeight, Config.WalkableClimb, hf, out chf))
                throw new OutOfMemoryException("BuildCompactHeightfield ran out of memory");
#if (TIMETHIS)
                Console.WriteLine("BuildCompactHeightfield: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif
            hf.Delete();

            // Erode the walkable area by agent radius.
            if (!Context.ErodeWalkableArea(Config.WalkableRadius, chf))
                throw new OutOfMemoryException("ErodeWalkableArea ran out of memory");
#if (TIMETHIS)
                Console.WriteLine("ErodeWalkableArea: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif

            // Prepare for region partitioning, by calculating distance field along the walkable surface.
            if (!Context.BuildDistanceField(chf))
                throw new OutOfMemoryException("BuildDistanceField ran out of memory");
#if (TIMETHIS)
                Console.WriteLine("BuildDistanceField: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif

            // Partition the walkable surface into simple regions without holes.
            if (!Context.BuildRegions(chf, Config.BorderSize, Config.MinRegionArea, Config.MergeRegionArea))
                throw new OutOfMemoryException("BuildRegions ran out of memory");
#if (TIMETHIS)
                Console.WriteLine("BuildRegions: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif

            // Create contours.
            ContourSet cset;
            if (!Context.BuildContours(chf, Config.MaxSimplificationError, Config.MaxEdgeLength, out cset))
                throw new OutOfMemoryException("BuildContours ran out of memory");
#if (TIMETHIS)
                Console.WriteLine("BuildContours: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif

            // Build polygon navmesh from the contours.
            PolyMesh pmesh;
            if (!Context.BuildPolyMesh(cset, Config.MaxVertsPerPoly, out pmesh))
                throw new OutOfMemoryException("BuildPolyMesh ran out of memory");
#if (TIMETHIS)
                Console.WriteLine("BuildPolyMesh: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
                cur = stopWatch.ElapsedMilliseconds;
            #endif

            // Build detail mesh.
            PolyMeshDetail dmesh = null;
            //if (!Context.BuildPolyMeshDetail(pmesh, chf, Config.DetailSampleDistance, Config.DetailSampleMaxError,out dmesh))
            //    throw new OutOfMemoryException("BuildPolyMeshDetail ran out of memory");
            //#if (TIMETHIS)
            //    Console.WriteLine("BuildPolyMeshDetail: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
            //   cur = stopWatch.ElapsedMilliseconds;
            //#endif

            chf.Delete();
            cset.Delete();

            // Set flags according to area types (e.g. Swim for Water)
            pmesh.MarkAll();

            // get original bounds
            float[] tilebMin, tilebMax;
            CalculateTileBounds(out tilebMin, out tilebMax, false, i, j);
            tilebMin[1] = bbMin[1];
            tilebMax[1] = bbMax[1];

            // build off mesh connections for flightmasters
            // bMax and bMin are switched here because of the coordinate system transformation

            var connections = new List<OffMeshConnection>();

            /*if (MapId == 0 && X == 31 && Y == 58)
            {
                //<X>-14250.71</X>
                //<Y>329.7578</Y>
                //<Z>24.17678</Z>

                //<X>-14254.86</X>
                //<Y>336.1039</Y>
                //<Z>26.79213</Z>
                
                SlimDX.Vector3 from = new SlimDX.Vector3(1.0f, 1.0f, 1.0f);
                SlimDX.Vector3 to   = new SlimDX.Vector3(1.0f, 1.0f, 1.0f);
                OffMeshConnection conn = new OffMeshConnection
                        { 
                            From = from.ToRecast().ToFloatArray(),
                            To = to.ToRecast().ToFloatArray(),
                            Radius = 2.5f,
                            Type = ConnectionType.BiDirectional,
                            UserID = 88,
                        };
                connections.Add(conn);
            }*/

            /* // Disable mesh connexion RIVAL
            var taxis = TaxiHelper.GetNodesInBBox(MapId, tilebMax.ToWoW(), tilebMin.ToWoW());
            foreach (var taxi in taxis)
            {
                Log.Log(LogCategory.Warning,
                        "Flightmaster \"" + taxi.Name + "\", Id: " + taxi.Id + " Horde: " + taxi.IsHorde + " Alliance: " +
                        taxi.IsAlliance);

                var data = TaxiHelper.GetTaxiData(taxi);
                var from = taxi.Location.ToRecast().ToFloatArray();
                connections.AddRange(data.To.Select(to => new OffMeshConnection
                                                              {
                                                                  AreaId = PolyArea.Road,
                                                                  Flags = PolyFlag.FlightMaster,
                                                                  From = from,
                                                                  To = to.Value.Location.ToRecast().ToFloatArray(),
                                                                  Radius = Config.WorldWalkableRadius,
                                                                  Type = ConnectionType.OneWay,
                                                                  UserID = (uint) to.Key
                                                              }));

                foreach (var target in data.To)
                    Log.Log(LogCategory.Warning,
                            "\tPath to: \"" + target.Value.Name + "\" Id: " + target.Value.Id + " Path Id: " +
                            target.Key);
            }*/


            byte[] tileData;
            if (!Detour.CreateNavMeshData(out tileData, pmesh, dmesh,
                X*Constant.Division + i, Y*Constant.Division + j, tilebMin, tilebMax,
                Config.WorldWalkableHeight, Config.WorldWalkableRadius,
                Config.WorldWalkableClimb, Config.CellSize,
                Config.CellHeight, Config.BuildBvTree,
                connections.ToArray()))
            {
                pmesh.Delete();
                //dmesh.Delete();
                return null;
            }
#if (TIMETHIS)
                Console.WriteLine("CreateNavMeshData: " + (stopWatch.ElapsedMilliseconds - cur) + ", total: " + stopWatch.ElapsedMilliseconds);
            #endif
            pmesh.Delete();
            //dmesh.Delete();
            GC.Collect();

            return tileData;
        }
    }
}