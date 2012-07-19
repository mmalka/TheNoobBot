using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using meshReader.Game.Miscellaneous;

namespace meshPathVisualizer
{
    
    public class MinimapImage
    {
        public Bitmap Result { get; private set; }
        public string World { get; private set; }
        public bool UnderWater { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int TilesX { get; private set; }
        public int TilesY { get; private set; }
        public int StartTileX { get; private set; }
        public int StartTileY { get; private set; }

        public MinimapImage(string world, int width, int height, int startX, int endX, int startY, int endY, bool underwater = false)
        {
            World = world;
            Result = new Bitmap(width, height);
            TilesX = endX - startX + 1;
            TilesY = endY - startY + 1;
            TileWidth = width/TilesX;
            TileHeight = height/TilesY;
            StartTileX = startX;
            StartTileY = startY;
            UnderWater = underwater;
        }

        public void Generate()
        {
            for (int y = 0; y < TilesY; y++)
            {
                for (int x = 0; x < TilesX; x++)
                {
                    var file = GetMinimapFileByCoords(World, StartTileX + x, StartTileY + y, UnderWater);
                    Image tile;
                    try
                    {
                        var blp = new Blp(file);
                        tile = blp.GetImage(0);
                    }
                    catch (FileNotFoundException)
                    {
                        continue;
                    }

                    int posX = x * TileWidth;
                    int posY = y * TileHeight;

                    var resized = ResizeImage(tile);
                    for (int iy = 0; iy < TileHeight; iy++)
                    {
                        for (int ix = 0; ix < TileWidth; ix++)
                        {
                            Result.SetPixel(ix + posX, iy + posY, resized.GetPixel(ix, iy));
                        }
                    }
                }
            }
        }

        private static string GetMinimapFileByCoords(string world, int x, int y, bool underWater)
        {
            return "World\\Minimaps\\" + world + "\\" + (underWater ? "noLiquid_": "") + "map" + x + "_" + y + ".blp";
        }

        private Bitmap ResizeImage(Image imgToResize)
        {
            int destWidth = TileWidth;
            int destHeight = TileHeight;

            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

    }

}