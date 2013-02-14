using System;
using System.IO;
using meshDatabase;
using meshReader.Game;

namespace meshBuilder
{
    
    public enum TileEventType
    {
        StartedBuild,
        CompletedBuild,
        FailedBuild,
        AlreadyBuilt, // I added this
    }

    public class TileEvent : EventArgs
    {
        public string Continent { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public TileEventType Type { get; private set; }

        public TileEvent(string continent, int x, int y, TileEventType type)
        {
            Continent = continent;
            X = x;
            Y = y;
            Type = type;
        }
    }

    public class ContinentBuilder
    {
        public string Continent { get; private set; }
        public WDT TileMap { get; private set; }

        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int CountX { get; private set; }
        public int CountY { get; private set; }
        private string _meshDir = "";

        public event EventHandler<TileEvent> OnTileEvent;

        public ContinentBuilder(string continent, string meshDir)
            : this(continent, meshDir, 0, 0, 64, 64)
        {
        }

        public ContinentBuilder(string continent, string meshDir, int startX, int startY, int countX, int countY)
        {
            StartX = startX;
            StartY = startY;
            CountX = countX;
            CountY = countY;
            _meshDir = meshDir;

            Continent = continent;
            TileMap = new WDT("World\\Maps\\" + continent + "\\" + continent + ".wdt");
        }

        private string GetTilePath(int x, int y, int phaseId)
        {
            return _meshDir + Continent + "\\" + Continent + "_" + x + "_" + y + "_" + phaseId + ".tile";
        }

        private string GetTilePath(int x, int y)
        {
            return _meshDir + Continent + "\\" + Continent + "_" + x + "_" + y + ".tile";
        }
        
        private void SaveTile(int x, int y, byte[] data)
        {
            File.WriteAllBytes(_meshDir + Continent + "\\" + Continent + "_" + x + "_" + y + ".tile", data);
        }

        private void Report(int x, int y, TileEventType type)
        {
            if (OnTileEvent != null)
                OnTileEvent(this, new TileEvent(Continent, x, y, type));
        }

        public static int PercentProgression()
        {
            try
            {
                return _meshBuild * 100 / _nbMeshAtCreate;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static string CurrentTile()
        {
            try
            {
                return _currentTile;
            }
            catch (Exception)
            {
                return "-";
            }
        }
        public static string GetTimeLeft()
        {
            try
            {
                TimeSpan t = TimeSpan.FromSeconds(((((int)Environment.TickCount - _timeStart) * _nbMeshAtCreate / _meshBuild)) / 1000);
                return t.Days + " Day(s) " + t.Hours + " H " + t.Minutes + " m";
                
            }
            catch (Exception)
            {
                return "-";
            }
        }

        static int _nbMeshAtCreate = 0;
        static int _meshBuild = 0;
        static int _timeStart = 0;
        private static string _currentTile = "";
        public void Build()
        {
            if (!Directory.Exists(_meshDir + Continent))
                Directory.CreateDirectory(_meshDir + Continent);

            // Count nb mesh at create
            for (int y = StartY; y < (StartY + CountY); y++)
            {
                for (int x = StartX; x < (StartX + CountX); x++)
                {
                    if (File.Exists(GetTilePath(x, y)))
                    {
                        Report(x, y, TileEventType.AlreadyBuilt);
                        continue;
                    }
                    else
                    {
                        // If tile not existe in wow
                        if (!TileMap.HasTile(x, y))
                            continue;
                    }
                    _nbMeshAtCreate++;
                }
            }

            _timeStart = (int)Environment.TickCount;
            // Start mesh creation
            for (int y = StartY; y < (StartY+CountY); y++)
            {
                for (int x = StartX; x < (StartX+CountX); x++)
                {
                    // Get if tile is alreay Build
                    if (File.Exists(GetTilePath(x, y)))
                    {
                        Report(x, y, TileEventType.AlreadyBuilt); 
                        continue;
                    }
                    else
                    {
                        // If tile not existe in wow
                        if (!TileMap.HasTile(x, y))
                            continue;
                    }

                    Report(x, y, TileEventType.StartedBuild);
                    _currentTile = Continent + "_" + x + "_" + y;
                    var builder = new TileBuilder(Continent, x, y);
                    byte[] data = null;

                    try
                    {
                        data = builder.Build(new MemoryLog());
                    }
                    catch (Exception)
                    {
                    }

                    _meshBuild++;

                    if (data == null)
                        Report(x, y, TileEventType.FailedBuild);
                    else
                    {
                        SaveTile(x, y, data);
                        Report(x, y, TileEventType.CompletedBuild);
                    }

                   // if (builder.Log is MemoryLog)
                    //    (builder.Log as MemoryLog).WriteToFile(Continent + "\\" + Continent + "_" + x + "_" + y + ".log");
                }
            }

            _currentTile = "Finish";
        }
    }

}