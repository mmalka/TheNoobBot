using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace meshPathVisualizer
{
    class Pather
    {
        public static void GetTileByLocation(float[] loc, out float x, out float y)
        {
            x = (loc[0] - Utility.Origin[0]) / Utility.TileSize;
            y = (loc[2] - Utility.Origin[2]) / Utility.TileSize;
        }

    }
}
