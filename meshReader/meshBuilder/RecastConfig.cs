using System;
using meshReader.Game;

namespace meshBuilder
{
    
    public class RecastConfig
    {
        public int BorderSize { get; set; }
        public float CellSize { get; set; }
        public float CellHeight { get; set; }
        public float WalkableSlopeAngle { get; set; }
        public int WalkableHeight { get; set; }
        public float WorldWalkableHeight { get; set; }
        public float WorldWalkableRadius { get; set; }
        public float WorldWalkableClimb { get; set; }
        public int WalkableClimb { get; set; }
        public int WalkableRadius { get; set; }
        public int MaxEdgeLength { get; set; }
        public float MaxSimplificationError { get; set; }
        public int MinRegionArea { get; set; }
        public int MergeRegionArea { get; set; }
        public int MaxVertsPerPoly { get; set; }
        public float DetailSampleDistance { get; set; }
        public float DetailSampleMaxError { get; set; }
        public int TileWidth { get; set; }
        public float TileSize { get; set; }
        public bool BuildBvTree { get; set; }

        public static RecastConfig Default
        {
            get
            {
                var ret = new RecastConfig();
                const float GRID_SIZE = Constant.TileSize;
                const float BASE_UNIT_DIM = 0.2962963f;
                const int VERTEX_PER_TILE = (int)(GRID_SIZE / BASE_UNIT_DIM + 0.5f);
                const int DT_VERTS_PER_POLYGON = 6;

                ret.MaxVertsPerPoly = DT_VERTS_PER_POLYGON;
                ret.CellSize = BASE_UNIT_DIM;
                ret.CellHeight = 0.4f;
                ret.WalkableSlopeAngle = 48.0f;
                ret.TileSize = GRID_SIZE;
                ret.MaxEdgeLength = 20; // 15
                ret.MinRegionArea = 40*40; // = 1600 vs 200;
                ret.MergeRegionArea = 52*52; // 52*52 = 2704 vs 3500;
                ret.MaxSimplificationError = 1.2f;
                ret.DetailSampleDistance = 2.0f;
                ret.DetailSampleMaxError = 0.5f;
                ret.TileWidth = VERTEX_PER_TILE;

                ret.WorldWalkableRadius = 0.5f;
                ret.WorldWalkableHeight = 1.69f;
                ret.WorldWalkableClimb = 1.0f;

                ret.WalkableRadius = (int)Math.Ceiling(ret.WorldWalkableRadius / ret.CellSize);
                ret.WalkableHeight = (int)Math.Ceiling(ret.WorldWalkableHeight / ret.CellHeight);
                ret.WalkableClimb = (int)Math.Floor(ret.WorldWalkableClimb / ret.CellHeight);

                ret.BorderSize = ret.WalkableRadius + 3;
                ret.BuildBvTree = true;
                return ret;
            }
        }

        public static RecastConfig Dungeon
        {
            get
            {
                var ret = new RecastConfig();
                ret.CellSize = 0.2f;
                ret.CellHeight = 0.3f;
                ret.MinRegionArea = (int) Math.Pow(5, 2);
                ret.MergeRegionArea = (int) Math.Pow(10, 2);
                ret.WalkableSlopeAngle = 50f;
                ret.DetailSampleDistance = 3f;
                ret.DetailSampleMaxError = 1.25f;
                ret.WorldWalkableClimb = 1f;
                ret.WorldWalkableHeight = 2.1f;
                ret.WorldWalkableRadius = 0.8f; // 0.6f;<-- old value  -  updated by Steveiwonder
                ret.WalkableClimb = (int)Math.Round(ret.WorldWalkableClimb / ret.CellHeight);
                ret.WalkableHeight = (int)Math.Round(ret.WorldWalkableHeight / ret.CellHeight);
                ret.WalkableRadius = (int)Math.Round(ret.WorldWalkableRadius / ret.CellSize);
                ret.MaxEdgeLength = ret.WalkableRadius*8;
                ret.MaxVertsPerPoly = 6;
                ret.MaxSimplificationError = 1.25f;
                ret.BorderSize = 0;
                return ret;
            }
        }
    }

}