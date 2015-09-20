using System;
using System.IO;
using meshDatabase;
using meshReader.Game;
using System.Collections.Generic;

namespace meshBuilder
{
    
    public enum TileEventType
    {
        StartedBuild,
        CompletedBuild,
        FailedBuild,
        AlreadyBuilt,
        BuiltByOther,
        ToBuild,
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
        private string current = "";

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

        private string GetTileNameAndPath(int x, int y, int i=0, int j=0)
        {
#pragma warning disable 162
            if (Constant.Division != 1)
                return _meshDir + Continent + "\\" + Continent + "_" + x + "_" + y + "_" + i + j + ".tile";
            else
                return _meshDir + Continent + "\\" + Continent + "_" + x + "_" + y + ".tile";
#pragma warning restore 162
        }

        private string GetTileLockAndPath(int x, int y)
        {
            return _meshDir + Continent + "\\" + Continent + "_" + x + "_" + y + ".lock";
        }

        private string GetTileNoneAndPath(int x, int y)
        {
            return _meshDir + Continent + "\\" + Continent + "_" + x + "_" + y + ".none";
        }

        private void SaveTile(int x, int y, byte[] data, int i=0, int j=0)
        {
#pragma warning disable 162
            if (Constant.Division != 1)
                File.WriteAllBytes(_meshDir + Continent + "\\" + Continent + "_" + x + "_" + y + "_" + i + j + ".tile", data);
            else
                File.WriteAllBytes(_meshDir + Continent + "\\" + Continent + "_" + x + "_" + y + ".tile", data);
#pragma warning restore 162
        }

        private void Report(int x, int y, TileEventType type)
        {
            if (OnTileEvent != null)
                OnTileEvent(this, new TileEvent(Continent, x, y, type));
        }

        static int _totalMeshes = 0;
        static int _meshesDone = 0;

        public static int PercentProgression()
        {
            if (_totalMeshes > 0)
                return (_meshesDone * 100) / _totalMeshes;
            return 0;
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

        static int _timeOneMesh = 0;
        static int _oldMeshBuildCount = 0;

        public static string GetTimeLeft()
        {
            if (_meshBuild > 0)
            {
                if (_oldMeshBuildCount < _meshBuild)
                {
                    _timeOneMesh = (Environment.TickCount - _timeStart) / _meshBuild;
                    _oldMeshBuildCount = _meshBuild;
                }
                int timeRemainingMeshes = _timeOneMesh * _nbMeshToCreate;
                TimeSpan t = TimeSpan.FromSeconds(timeRemainingMeshes / 1000);
                return (t.Days * 24 + t.Hours) + " H " + t.Minutes + " m";
            }
            else
            {
                return "-";
            }
        }

        static int _nbMeshToCreate = 0;
        static int _meshBuild = 0;
        static int _timeStart = 0;
        private static string _currentTile = "";
        private static byte[,] _doneTiles = new byte[64, 64];

        public static void Reset()
        {
            _doneTiles = new byte[64, 64];
            _nbMeshToCreate = 0;
            _meshBuild = 0;
            _timeStart = 0;
            _currentTile = "";
            _timeOneMesh = 0;
            _oldMeshBuildCount = 0;
        }

        public void ScanFolderAndReport()
        {
            int nbMeshToCreate = 0;
            int totalMeshes = 0;
            int meshesDone = 0;
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    // Get if tile is alreay Build
                    if (File.Exists(GetTileNameAndPath(x, y)))
                    {
                        if (_doneTiles[x, y] != 1)
                            Report(x, y, TileEventType.AlreadyBuilt);
                        totalMeshes++;
                        meshesDone++;
                        continue;
                    }
                    else if (File.Exists(GetTileLockAndPath(x, y)))
                    {
                        if (_doneTiles[x, y] != 1 && current != GetTileLockAndPath(x, y))
                            Report(x, y, TileEventType.BuiltByOther);
                        totalMeshes++;
                        meshesDone++;
                        continue;
                    }
                    else
                    {
                        // If tile does not exist in wow
                        if (!TileMap.HasTile(x, y))
                            continue;
                    }
                    Report(x, y, TileEventType.ToBuild);
                    nbMeshToCreate++;
                    totalMeshes++;
                }
            }
            _nbMeshToCreate = nbMeshToCreate;
            _totalMeshes = totalMeshes;
            _meshesDone = meshesDone;
        }

        public void Build()
        {
            if (!Directory.Exists(_meshDir + Continent))
                Directory.CreateDirectory(_meshDir + Continent);

            _timeStart = (int)Environment.TickCount;
            // Start mesh creation
            for (int y = StartY; y < (StartY+CountY); y++)
            {
                for (int x = StartX; x < (StartX+CountX); x++)
                {
                    _doneTiles[x, y] = 0;
                    // Get if tile is alreay Build
                    if (File.Exists(GetTileNameAndPath(x, y)) || File.Exists(GetTileLockAndPath(x, y)) || File.Exists(GetTileNoneAndPath(x, y)))
                    {
                        continue;
                    }
                    else
                    {
                        // If the tile does not exist in wow
                        if (!TileMap.HasTile(x, y))
                            continue;
                    }
                    try
                    {
                        string tlock = GetTileLockAndPath(x, y);
                        var sw = File.CreateText(tlock);
                        current = tlock;
                        sw.Close();
                    }
                    catch (Exception)
                    {
                        // In case 2 builder are trying to create the same file
                        continue;
                    }
                    ScanFolderAndReport();
                    Report(x, y, TileEventType.StartedBuild);
                    _currentTile = Continent + "_" + x + "_" + y;
                    Console.WriteLine(_currentTile);
                    var builder = new TileBuilder(Continent, x, y);
                    byte[] data = null;
                    bool failed = false;

                    try
                    {
                        builder.PrepareData(new MemoryLog());
                        for (int i = 0; i < Constant.Division; i++)
                        {
                            for (int j = 0; j < Constant.Division; j++)
                            {
                                data = null;
                                data = builder.Build(i, j);
                                if (data == null)
                                {
                                    failed = true;
                                    var none = File.CreateText(GetTileNoneAndPath(x, y));
                                    none.Close();
                                    break;
                                }
                                SaveTile(x, y, data, i, j);
                            }
                            if (failed)
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        File.Delete(GetTileLockAndPath(x, y));
                        throw;
                    }

                    _meshBuild++;

                    if (failed)
                        Report(x, y, TileEventType.FailedBuild);
                    else
                    {
                        _doneTiles[x, y] = 1;
                        Report(x, y, TileEventType.CompletedBuild);
                    }
                    File.Delete(GetTileLockAndPath(x, y));
                }
            }
            ScanFolderAndReport();
            _currentTile = "Finish";
            MpqManager.Close();
        }

        public void CleanupLastLock()
        {
            try
            {
                File.Delete(current);
            }
            catch (Exception)
            {
            }
        }
    }
}