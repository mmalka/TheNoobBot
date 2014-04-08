using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace meshPathVisualizer
{
    
    public class PathImage
    {
        public MinimapImage Background { get; private set; }
        public List<Hop> Hops { get; private set; }
        public List<Hop> NpcDB { get; private set; }
        public string World { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Zoom { get; set; }
        public bool IgnoreWater { get; set; }
        public Bitmap Result { get; set; }

        public PathImage(string world, List<Hop> hops, List<Hop> npcdb, bool autoZoom = false, float zoom = 1.0f, bool ignoreWater = false)
        {
            World = world;
            Width = 0; // width;
            Height = 0; // height;
            Hops = hops;
            NpcDB = npcdb;
            IgnoreWater = ignoreWater;
            Zoom = (autoZoom ? 0.0f : zoom);
        }

        public void Generate(int screenW, int screenH, out float proposedZoom)
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
            // Prevent only 1 tile width or height and extend to 3 tiles in this case
            if (minX == maxX)
            {
                minX = System.Math.Max(0, minX - 1);
                maxX = System.Math.Min(64, maxX + 1);
            }
            if (minY == maxY)
            {
                minY = System.Math.Max(0, minY - 1);
                maxY = System.Math.Min(64, maxY + 1);
            }

            int baseWidth = (maxX - minX + 1) * 128;
            int baseHeight = (maxY - minY + 1) * 128;
            if (Zoom == 0) // autozoom
            {   // screenH-100 to allow header and footer of form to fit on screen
                Zoom = System.Math.Min((float)(screenW) / baseWidth, (float)(screenH-100) / baseHeight);
                if (Zoom < 0.25f)
                    Zoom = 0.15f;
                else if (Zoom < 0.50f)
                    Zoom = 0.25f;
                else if (Zoom < 1.0f)
                    Zoom = 0.5f;
                else if (Zoom < 2.0f)
                    Zoom = 1.0f;
                else
                    Zoom = 2.0f;
            }
            
            Width = (int)(baseWidth * Zoom);
            Height = (int)(baseHeight * Zoom);
            proposedZoom = Zoom;
            
            // initialize and generate the background
            Background = new MinimapImage(World, Width, Height, minX, maxX, minY, maxY, IgnoreWater);
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
            graphics.DrawLines(new Pen(NpcDB == null ? Color.Red : Color.Orange, 4f), points);

            foreach (var point in points)
                graphics.DrawEllipse(new Pen(Color.Black, 1f), point.X - (6f/2), point.Y - (6f/2), 6, 6);

            if (NpcDB != null)
            {
                for (int i = 0; i < NpcDB.Count; i++)
                {
                    var hop = NpcDB[i];
                    string iContinent = hop.Continent;
                    if (iContinent == "Pandaria")
                        iContinent = "HawaiiMainLand";
                    if (iContinent == World)
                    {
                        var recastLoc = hop.Location.ToRecast().ToFloatArray();
                        float tX, tY;
                        Pather.GetTileByLocation(recastLoc, out tX, out tY);
                        if (tX >= minX && tX <= maxX && tY >= minY && tY <= maxY)
                        {
                            tX -= minX;
                            tY -= minY;
                            switch (hop.Type)
                            {
                                case HopType.Alliance:
                                    graphics.DrawEllipse(new Pen(Color.White, 1f), tX * Background.TileWidth - (6f / 2), tY * Background.TileHeight - (6f / 2), 6, 6);
                                    graphics.FillEllipse(new SolidBrush(Color.Blue), tX * Background.TileWidth - (6f / 2), tY * Background.TileHeight - (6f / 2), 6, 6);
                                    break;
                                case HopType.Horde:
                                    graphics.DrawEllipse(new Pen(Color.White, 1f), tX * Background.TileWidth - (6f / 2), tY * Background.TileHeight - (6f / 2), 6, 6);
                                    graphics.FillEllipse(new SolidBrush(Color.Red), tX * Background.TileWidth - (6f / 2), tY * Background.TileHeight - (6f / 2), 6, 6);
                                    break;
                                case HopType.Neutral:
                                    graphics.DrawEllipse(new Pen(Color.White, 1f), tX * Background.TileWidth - (6f / 2), tY * Background.TileHeight - (6f / 2), 6, 6);
                                    graphics.FillEllipse(new SolidBrush(Color.Yellow), tX * Background.TileWidth - (6f / 2), tY * Background.TileHeight - (6f / 2), 6, 6);
                                    break;
                            }
                        }
                    }
                }
            }
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