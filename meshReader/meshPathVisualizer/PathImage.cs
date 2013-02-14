using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using meshPather;

namespace meshPathVisualizer
{
    
    public class PathImage
    {
        public MinimapImage Background { get; private set; }
        public List<Hop> Hops { get; private set; }
        public string World { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Bitmap Result { get; set; }

        public PathImage(string world, int width, int height, List<Hop> hops)
        {
            World = world;
            Width = width;
            Height = height;
            Hops = hops;
        }

        public void Generate()
        {
            // first, parse the hops data to determine the touched tiles
            int minX = 64, maxX = 0, minY = 64, maxY = 0;
            foreach (var hop in Hops)
            {
                var recastLoc = hop.Location.ToRecast().ToFloatArray();
                float tX, tY;
                Pather.GetTileByLocation(recastLoc, out tX, out tY);

                if (tX < minX)
                    minX = (int) tX;
                if (tY < minY)
                    minY = (int) tY;
                if (tX > maxX)
                    maxX = (int) tX;
                if (tY > maxY)
                    maxY = (int) tY;
            }

            // initialize and generate the background
            Background = new MinimapImage(World, Width, Height, minX, maxX, minY, maxY);
            Background.Generate();

            // draw the path
            var graphics = Graphics.FromImage(Background.Result);
            var points = new PointF[Hops.Count];
            for (int i = 0; i < Hops.Count; i++)
            {
                var hop = Hops[i];
                var recastLoc = hop.Location.ToRecast().ToFloatArray();
                float tX, tY;
                Pather.GetTileByLocation(recastLoc, out tX, out tY);

                tX -= minX;
                tY -= minY;
                points[i] = new PointF(tX*Background.TileWidth, tY*Background.TileHeight);
            }
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawLines(new Pen(Color.Red, 4f), points);
            foreach (var point in points)
                graphics.DrawEllipse(new Pen(Color.Black, 1f), point.X - (6f/2), point.Y - (6f/2), 6, 6);
            graphics.Dispose();

            // and wrap up the result
            Result = Background.Result;
        }

        public void DrawHeatMap(List<KeyValuePair<float, float>> pts)
        {
            var graphics = Graphics.FromImage(Result);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            foreach (var kvp in pts)
            {
                graphics.DrawRectangle(new Pen(Color.Blue, 1f), ((kvp.Key - Background.StartTileX) * Background.TileWidth) - 2, ((kvp.Value - Background.StartTileY) * Background.TileHeight) - 2, 4, 4);
            }
            graphics.Dispose();
        }

    }

}