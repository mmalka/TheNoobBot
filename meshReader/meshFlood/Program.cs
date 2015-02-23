using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DetourLayer;
using meshBuilder;
using meshDatabase;
using meshDatabase.Database;
using meshPather;
using Microsoft.Xna.Framework;

namespace meshFlood
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please supply continent name");
                return;
            }

            var continent = args[0];
            var path = "S:\\meshReader\\Meshes\\" + continent;
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Can't find mesh directory: " + path);
                return;
            }

            Console.Write("Setting up data storage.. ");
            string floodDir = "C:\\Meshes\\Floodfill\\";
            if (!Directory.Exists(floodDir))
                Directory.CreateDirectory(floodDir);
            floodDir += continent;
            if (Directory.Exists(floodDir))
                Directory.Delete(floodDir, true);
            Directory.CreateDirectory(floodDir);
            floodDir += "\\";
            Console.WriteLine("done");

            var files = Directory.GetFiles(path).Where(f => f.EndsWith(".tile"));
            Console.WriteLine("Total amount of tiles: " + files.Count());
            if (files.Count() > 4096)
            {
                Console.WriteLine("Too many tiles. Increase maxTiles.");
                return;
            }

            Console.WriteLine("Initializing mesh..");
            var mesh = new NavMesh();
            if ((mesh.Initialize(/*32768*/ 8192, /*4096*/ 384, Utility.Origin, Utility.TileSize, Utility.TileSize) & DetourStatus.Failure) != 0)
            {
                Console.WriteLine("Failed to initialize mesh.");
                return;
            }

            Console.WriteLine("Loading all tiles..");
            var tiles = new List<MeshTile>(files.Count());
            foreach (var file in files)
            {
                var data = File.ReadAllBytes(file);
                MeshTile tile;
                if ((mesh.AddTile(data, out tile) & DetourStatus.Failure) != 0)
                {
                    Console.WriteLine("Failed to load tile: " + file);
                    return;
                }
                tiles.Add(tile);
            }

            Console.WriteLine("Initializing DBC backend...");
            //MpqManager.InitializeDBC();

            Console.Write("Identifiying map id.. ");
            int mapId = PhaseHelper.GetMapIdByName(continent);
            if (mapId < 0)
            {
                Console.WriteLine("failed");
                return;
            }
            Console.WriteLine(mapId);
            Console.Write("Identifying source points.. ");
            var sourcePoints = new List<Vector3>(100);
            sourcePoints.AddRange(from record in TaxiHelper.TaxiNodesDBC.Records
                                  select new TaxiNode(record)
                                  into node where node.IsValid && node.MapId == mapId select node.Location.ToRecast());
            Console.WriteLine(sourcePoints.Count);

            Console.WriteLine("Initializing flood fill..");
            var floodFill = new FloodFill(mesh);
            Console.WriteLine("Flooding.. ");
            foreach (var source in sourcePoints)
                floodFill.ExecuteFrom(source);
            Console.WriteLine("Finished, visited " + floodFill.Marked + " polygons");

            Console.WriteLine("Rebuilding tiles...");
            var config = RecastConfig.Default;
            long sizeBefore = 0;
            long sizeAfter = 0;
            foreach (var tile in tiles)
            {
                sizeBefore += tile.DataSize;
                byte[] rebuiltData;
                
                if (!tile.Rebuild(floodFill.Visited, floodFill.VisitedMask, config.CellHeight, config.MaxVertsPerPoly, out rebuiltData))
                {
                    Console.WriteLine("Failed to rebuild tile " + tile.Header.X + " " + tile.Header.Y);
                    continue;
                }

                if (rebuiltData == null)
                {
                    Console.WriteLine("Tile " + tile.Header.X + " " + tile.Header.Y + " ceases to exist.");
                    continue;
                }

                sizeAfter += rebuiltData.Length;
                File.WriteAllBytes(floodDir + continent + "_" + tile.Header.X + "_" + tile.Header.Y + ".tile", rebuiltData);
            }

            Console.WriteLine("All done, size before: " + (sizeBefore / 1024 / 1024) + "MiB after: " + (sizeAfter / 1024 / 1024) + "MiB");
            Console.ReadKey(true);
        }
    }
}
