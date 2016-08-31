using System;
using System.Runtime.InteropServices;

namespace meshDatabase.Database
{
    
    public class WorldMapArea
    {
        public int Id { get; private set; }
        public int Map { get; private set; }
        public int Area { get; private set; }
        public string Name { get; private set; }
        public float[] BMin { get; private set; }
        public float[] BMax { get; private set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct WorldMapAreaDb2Record
        {
            public uint Id;
            public float locLeft;
            public float locRight;
            public float locTop;
            public float locBottom;
            public int Map;
            public int Area;
            // to be continued...
        }


        public WorldMapArea(Record src)
        {
            Id = src[0];
            Map = src[1];
            Area = src[2];
            Name = src.GetString(3);
            BMin = new float[2];
            BMin[0] = src.GetFloat(7);
            BMin[1] = src.GetFloat(5);
            BMax = new float[2];
            BMax[0] = src.GetFloat(6);
            BMax[1] = src.GetFloat(4);
        }

        /*public void TransformCoordinates(float coordX, float coordY, out float worldX, out float worldY)
        {
            worldX = BMin[0] + ((BMax[0] - BMin[0])*(coordX/100));
            worldY = BMin[0] + ((BMax[0] - BMin[0])*(coordY/100));
        }*/

        /*public void GetMinTile(out int x, out int y)
        {
            const float tileSize = 533.3333f;
            x = (int)Math.Floor(32 - (BMin[0] / tileSize));
            y = (int)Math.Floor(32 - (BMin[1] / tileSize));
        }

        public void GetMaxTile(out int x, out int y)
        {
            const float tileSize = 533.3333f;
            x = (int)Math.Floor(32 - (BMax[0] / tileSize));
            y = (int)Math.Floor(32 - (BMax[1] / tileSize));
        }*/

    }

}