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

        public static RecastConfig Default
        {
            get
            {
                var ret = new RecastConfig();
                const float tileSize = Constant.TileSize; // 80 dans mangos
                const int tileVoxelSize = 2000; //1800;
                ret.CellSize = tileSize / tileVoxelSize; // -> 0.2666666666
                ret.CellHeight = 0.3f; //0.4f
                ret.MinRegionArea = 1600;// 3600;//64; // 3600; //20;
                ret.MergeRegionArea = 2500;// 400; // 2500; //40;
                ret.WalkableSlopeAngle = 45.0f; // 50.0 juste previously
                ret.DetailSampleDistance = ret.CellSize * 64; // = 17.xx //3f;
                ret.DetailSampleMaxError = ret.CellHeight * 2; // = 0.6 //1.25f;
                //Toon Config
                // working set
//                ret.WorldWalkableClimb = 1.6f; //1f; *           // less than agent height
//                ret.WorldWalkableHeight = 2.4f; //1.7f;         // agent height
//                ret.WorldWalkableRadius = 0.7f; //0.6f; *       // agent radius
                // test set
                ret.WorldWalkableClimb = 1.0f;
                ret.WorldWalkableHeight = 1.7f; // 1.8f and 7 bellow
                ret.WorldWalkableRadius = 0.6f;
                // end test

                ret.WalkableClimb = 4; // (int)Math.Round(ret.WorldWalkableClimb / ret.CellHeight); // 4
                ret.WalkableHeight = 6; // (int)Math.Round(ret.WorldWalkableHeight / ret.CellHeight); // 6
                ret.WalkableRadius = 2; // (int)Math.Round(ret.WorldWalkableRadius / ret.CellSize); // 2
                //end of Toon Config
                ret.MaxEdgeLength = 200; // ret.WalkableRadius * 8;
                ret.BorderSize = ret.WalkableRadius + 3; //4
                ret.TileWidth = tileVoxelSize; // +(ret.BorderSize * 2); //* 2
                ret.MaxVertsPerPoly = 6;
                ret.MaxSimplificationError = 2.0f; //1.3f;
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