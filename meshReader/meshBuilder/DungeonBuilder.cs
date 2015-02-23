using System;
using DetourLayer;
using meshDatabase;
using meshReader.Game;
using meshReader.Game.WMO;
using RecastLayer;

namespace meshBuilder
{
    
    public class DungeonBuilder : ProgressTracker
    {
        public string Dungeon { get; private set; }
        public Geometry Geometry { get; private set; }
        public BaseLog Log { get; private set; }
        public RecastContext Context { get; private set; }
        public RecastConfig Config { get; private set; }

        public DungeonBuilder(string world)
        {
            Dungeon = world;
            Config = RecastConfig.Dungeon;
        }

        public byte[] Build(BaseLog log)
        {
            var wdt = new WDT("World\\maps\\" + Dungeon + "\\" + Dungeon + ".wdt");
            if (!wdt.IsGlobalModel || !wdt.IsValid)
                return null;

            InitializeProgress(12);

            Geometry = new Geometry {Transform = true};
            var model = new WorldModelRoot(wdt.ModelFile);
            Geometry.AddDungeon(model, wdt.ModelDefinition);

            CompleteWorkUnit();

            if (Geometry.Vertices.Count == 0 && Geometry.Triangles.Count == 0)
                throw new InvalidOperationException("Can't build mesh with empty geometry");

            Log = log;
            Context = new RecastContext();
            Context.SetContextHandler(Log);

            // get raw geometry - lots of slowness here
            float[] vertices;
            int[] triangles;
            byte[] areas;
            Geometry.GetRawData(out vertices, out triangles, out areas);
            Geometry.Triangles.Clear();

            float[] bmin, bmax;
            Geometry.CalculateBoundingBox(out bmin, out bmax);
            Geometry.Vertices.Clear();

            // Allocate voxel heightfield where we rasterize our input data to.
            Heightfield hf;
            int width, height;
            Recast.CalcGridSize(bmin, bmax, Config.CellSize, out width, out height);
            if (!Context.CreateHeightfield(out hf, width, height, bmin, bmax, Config.CellSize, Config.CellHeight))
                throw new OutOfMemoryException("CreateHeightfield ran out of memory");

            CompleteWorkUnit();

            // Find triangles which are walkable based on their slope and rasterize them.
            Context.ClearUnwalkableTriangles(Config.WalkableSlopeAngle, ref vertices, ref triangles, areas);
            Context.RasterizeTriangles(ref vertices, ref triangles, ref areas, hf, Config.WalkableClimb);

            CompleteWorkUnit();

            // Once all geometry is rasterized, we do initial pass of filtering to
            // remove unwanted overhangs caused by the conservative rasterization
            // as well as filter spans where the character cannot possibly stand.
            Context.FilterLowHangingWalkableObstacles(Config.WalkableClimb, hf);
            Context.FilterLedgeSpans(Config.WalkableHeight, Config.WalkableClimb, hf);
            Context.FilterWalkableLowHeightSpans(Config.WalkableHeight, hf);

            CompleteWorkUnit();

            // Compact the heightfield so that it is faster to handle from now on.
            // This will result in more cache coherent data as well as the neighbours
            // between walkable cells will be calculated.
            CompactHeightfield chf;
            if (!Context.BuildCompactHeightfield(Config.WalkableHeight, Config.WalkableClimb, hf, out chf))
                throw new OutOfMemoryException("BuildCompactHeightfield ran out of memory");

            CompleteWorkUnit();

            hf.Delete();

            // Erode the walkable area by agent radius.
            if (!Context.ErodeWalkableArea(Config.WalkableRadius, chf))
                throw new OutOfMemoryException("ErodeWalkableArea ran out of memory");
            CompleteWorkUnit();

            // Prepare for region partitioning, by calculating distance field along the walkable surface.
            if (!Context.BuildDistanceField(chf))
                throw new OutOfMemoryException("BuildDistanceField ran out of memory");
            CompleteWorkUnit();

            // Partition the walkable surface into simple regions without holes.
            if (!Context.BuildRegions(chf, Config.BorderSize, Config.MinRegionArea, Config.MergeRegionArea))
                throw new OutOfMemoryException("BuildRegions ran out of memory");
            CompleteWorkUnit();

            // Create contours.
            ContourSet cset;
            if (!Context.BuildContours(chf, Config.MaxSimplificationError, Config.MaxEdgeLength, out cset))
                throw new OutOfMemoryException("BuildContours ran out of memory");
            CompleteWorkUnit();

            // Build polygon navmesh from the contours.
            PolyMesh pmesh;
            if (!Context.BuildPolyMesh(cset, Config.MaxVertsPerPoly, out pmesh))
                throw new OutOfMemoryException("BuildPolyMesh ran out of memory");
            CompleteWorkUnit();

            // Build detail mesh.
            PolyMeshDetail dmesh;
            if (!Context.BuildPolyMeshDetail(pmesh, chf, Config.DetailSampleDistance, Config.DetailSampleMaxError, out dmesh))
                throw new OutOfMemoryException("BuildPolyMeshDetail ran out of memory");
            CompleteWorkUnit();

            chf.Delete();
            cset.Delete();

            // Set flags according to area types (e.g. Swim for Water)
            pmesh.MarkAll();

            byte[] meshData;
            if (!Detour.CreateNavMeshData(out meshData, pmesh, dmesh, 0, 0, bmin, bmax, Config.WorldWalkableHeight, Config.WorldWalkableRadius, Config.WorldWalkableClimb, Config.CellSize, Config.CellHeight, Config.BuildBvTree, null))
            {
                pmesh.Delete();
                dmesh.Delete();
                return null;
            }
            
            CompleteWorkUnit();
            pmesh.Delete();
            dmesh.Delete();
            return meshData;
        }

    }

}