using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using meshBuilder;
using meshDatabase;
using meshReader.Game;
using Microsoft.Win32;
using meshDatabase.Database;
using System.Collections.Generic;

namespace meshBuilderGui
{
    public partial class Interface : Form
    {
        private ContinentBuilder _builder;
        private DungeonBuilder _dungeonBuilder;
        private Thread _buildThread;
        private int _lastProgressX;
        private bool _buildStarted = false;
        private bool _initialized = false;
        private int _buildIndex = -1;
        private bool _onlyOneCategory = false;

        public Interface()
        {
            InitializeComponent();

            try
            {
                RegistryKey read = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\World of Warcraft");
                wowDirCB.Text = read.GetValue("InstallPath").ToString();
                wowDirCB.Items.Add(read.GetValue("InstallPath").ToString());
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _onlyOneCategory = false;
            if (wowDirCB.Text == string.Empty)
                return;
            if (continentNameCB.Text == "BUILD ALL")
            {
                _buildIndex = 2;
                continentNameCB.SelectedIndex = _buildIndex;
            }
            if (continentNameCB.Text.Contains("------"))
            {
                _onlyOneCategory = true;
                _buildIndex = continentNameCB.SelectedIndex + 1;
                continentNameCB.SelectedIndex = _buildIndex;
            }
            if (!_initialized)
            {
                MpqManager.Initialize(wowDirCB.Text);
                _initialized = true;
            }
            builderLauncher(continentNameCB.Text);
        }

        private void builderLauncher(string mapname)
        {
            if (mapname.Contains("------"))
                return;
            try
            {
                string continentInternalName = mapname;
                var wdt = new WDT("World\\Maps\\" + continentInternalName + "\\" + continentInternalName + ".wdt");
                if (!wdt.IsValid)
                    return;

                if (wdt.IsGlobalModel)
                {
                    _dungeonBuilder = new DungeonBuilder(continentInternalName, meshTB.Text);
                    _dungeonBuilder.OnProgress += OnProgress;
                    _builder = null;
                    _lastProgressX = 0;
                }
                else
                {
                    int startX = startXBox.Text.Length > 0 ? int.Parse(startXBox.Text) : 0;
                    int startY = startYBox.Text.Length > 0 ? int.Parse(startYBox.Text) : 0;
                    int countX = countXBox.Text.Length > 0 ? int.Parse(countXBox.Text) : (64 - startX);
                    int countY = countYBox.Text.Length > 0 ? int.Parse(countYBox.Text) : (64 - startY);
                    if (countX > (64 - startX))
                        countX = 64 - startX;
                    if (countY > (64 - startY))
                        countY = 64 - startY;

                    if (_buildIndex != -1) // We are building everything, then no limits
                    {
                        startX = 0;
                        startY = 0;
                        countX = 64;
                        countY = 64;
                    }

                    startXBox.Text = startX.ToString();
                    startXBox.ReadOnly = true;
                    startYBox.Text = startY.ToString();
                    startYBox.ReadOnly = true;
                    countXBox.Text = countX.ToString();
                    countXBox.ReadOnly = true;
                    countYBox.Text = countY.ToString();
                    countYBox.ReadOnly = true;

                    _builder = new ContinentBuilder(continentInternalName, meshTB.Text, startX, startY, countX, countY);
                    _builder.OnTileEvent += OnTileEvent;
                    _dungeonBuilder = null;
                }

                _buildStarted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            this.buildDisplay1.Clear();
            continentNameCB.Enabled = false;
            wowDirCB.Enabled = false;
            meshTB.ReadOnly = true;
            button1.Enabled = false;
            button1.Text = "Building...";
            populate.Enabled = false;
            
            _buildThread = new Thread(RunBuild) {IsBackground = true}; 
            _buildThread.Start();
        }

        void OnProgress(object sender, ProgressEvent e)
        {
            var bars = (int)Math.Floor(64*e.Completion);
            for (int i = _lastProgressX; i < bars; i++)
            {
                for (int y = 0; y < 64; y++)
                {
                    buildDisplay1.MarkCompleted(i, y);
                }
            }
            _lastProgressX = bars;
        }

        // Start Build
        private void RunBuild()
        {
            try
            {
                if (_builder != null)
                {
                    _builder.Build(); 
                }
                else if (_dungeonBuilder != null)  
                {
                    var mesh = _dungeonBuilder.Build();
                    if (mesh != null)
                        _dungeonBuilder.SaveTile(mesh);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Mesh Builder Interface - Exception");
            }
        }

        // Paint tile
        void OnTileEvent(object sender, TileEvent e)
        {
            switch (e.Type)
            {
                case TileEventType.CompletedBuild:
                    buildDisplay1.MarkCompleted(e.X, e.Y);
                    break;

                case TileEventType.StartedBuild:
                    buildDisplay1.MarkRunning(e.X, e.Y);
                    break;

                case TileEventType.FailedBuild:
                    buildDisplay1.MarkFailed(e.X, e.Y);
                    break;
                    //I added this
                case TileEventType.AlreadyBuilt:
                    buildDisplay1.MarkAlreadyBuilt(e.X, e.Y);
                    break;
                case TileEventType.ToBuild:
                    buildDisplay1.MarkToBeDone(e.X, e.Y);
                    break;
                case TileEventType.BuiltByOther:
                    buildDisplay1.MarkDoneByother(e.X, e.Y);
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_buildStarted)
            {
                button1.Text = ContinentBuilder.PercentProgression() + " %";
                statL.Text = ContinentBuilder.CurrentTile();
                timeLeftL.Text = "Time Left: " + ContinentBuilder.GetTimeLeft();
                if (!_buildThread.IsAlive)
                {
                    if (_buildIndex != -1)
                        _buildIndex++;
                    if (_buildIndex == -1 || _buildIndex > continentNameCB.Items.Count - 1)
                    {
                        _buildStarted = false;
                        continentNameCB.Enabled = true;
                        wowDirCB.Enabled = false; // let this locked
                        meshTB.ReadOnly = false;
                        button1.Enabled = true;
                        startXBox.ReadOnly = false;
                        startYBox.ReadOnly = false;
                        countXBox.ReadOnly = false;
                        countYBox.ReadOnly = false;
                        ContinentBuilder.Reset();
                        button1.Text = "Start Build";
                    }
                    else
                    {
                        if (_onlyOneCategory && continentNameCB.Text.Contains("------"))
                        {
                            _buildStarted = false;
                            continentNameCB.Enabled = true;
                            wowDirCB.Enabled = false; // let this locked
                            meshTB.ReadOnly = false;
                            button1.Enabled = true;
                            startXBox.ReadOnly = false;
                            startYBox.ReadOnly = false;
                            countXBox.ReadOnly = false;
                            countYBox.ReadOnly = false;
                            ContinentBuilder.Reset();
                            button1.Text = "Start Build";
                        }
                        else
                        {
                            continentNameCB.SelectedIndex = _buildIndex;
                            ContinentBuilder.Reset();
                            builderLauncher(continentNameCB.Text);
                        }
                    }
                }
            }
        }

        private void Interface_FormClosing(object sender, FormClosingEventArgs e)
        {
            MpqManager.Close();
            if (_builder != null)
                _builder.CleanupLastLock();
        }

        private void populate_Click(object sender, EventArgs e)
        {
            if (wowDirCB.Text == string.Empty)
                return;
            continentNameCB.Enabled = false;
            wowDirCB.Enabled = false;
            meshTB.ReadOnly = true;
            button1.Enabled = false;
            populate.Enabled = false;
            if (!_initialized)
            {
                MpqManager.Initialize(wowDirCB.Text);
                _initialized = true;
            }

            // SpellList dumper (keep commented to avoid fileException when opening severals builder at once, but it's very fast)
            
            //MpqManager.DumpSpellList(); // dumps all spells into directory/spell.txt
            
            // End of SpellListDumper

            // Doing them one by one to be able to add some separators
            List<WoWMap.MapDbcRecord> l0 = PhaseHelper.GetAllMapOfInstanceType(InstanceType.World, MapType.ADTType);
            List<string> strl = new List<string>();
            strl.Add("BUILD ALL");
            strl.Add("------- Big tiled world maps -------");
            foreach (WoWMap.MapDbcRecord m in l0)
                strl.Add(m.MapMPQName());
            strl.Add("------- BG -------");
            l0 = PhaseHelper.GetAllMapOfInstanceType(InstanceType.Battleground, MapType.ADTType);
            foreach (WoWMap.MapDbcRecord m in l0)
                strl.Add(m.MapMPQName());
            strl.Add("------- Big tiled Instances-------");
            l0 = PhaseHelper.GetAllMapOfInstanceType(InstanceType.Instance, MapType.ADTType);
            foreach (WoWMap.MapDbcRecord m in l0)
                strl.Add(m.MapMPQName());
            strl.Add("------- Small Instances-------");
            l0 = PhaseHelper.GetAllMapOfInstanceType(InstanceType.Instance, MapType.WDTOnlyType);
            foreach (WoWMap.MapDbcRecord m in l0)
                strl.Add(m.MapMPQName());
            strl.Add("------- Big tiled Raids -------");
            l0 = PhaseHelper.GetAllMapOfInstanceType(InstanceType.RaidInstance, MapType.ADTType);
            foreach (WoWMap.MapDbcRecord m in l0)
                strl.Add(m.MapMPQName());
            strl.Add("------- Small raids -------");
            l0 = PhaseHelper.GetAllMapOfInstanceType(InstanceType.RaidInstance, MapType.WDTOnlyType);
            foreach (WoWMap.MapDbcRecord m in l0)
                strl.Add(m.MapMPQName());
            strl.Add("------- Scenarios -------");
            l0 = PhaseHelper.GetAllMapOfInstanceType(InstanceType.Scenario, MapType.ADTType);
            foreach (WoWMap.MapDbcRecord m in l0)
                strl.Add(m.MapMPQName());
            /*strl.Add("------- Small Scenarios -------");
            l0 = PhaseHelper.GetAllMapOfInstanceType(InstanceType.Scenario, MapType.WDTOnlyType);
            foreach (MapEntry m in l0)
                strl.Add(m.InternalName);*/
            strl.Add("------- Garrison maps -------");
            l0 = PhaseHelper.GetGarrisonMaps();
            foreach (WoWMap.MapDbcRecord m in l0)
                strl.Add(m.MapMPQName());
            //foreach (string s in strl)
            //    Console.WriteLine(s);
            this.continentNameCB.AutoCompleteCustomSource.Clear();
            this.continentNameCB.AutoCompleteCustomSource.AddRange(strl.ToArray());
            this.continentNameCB.Items.Clear();
            this.continentNameCB.Items.AddRange(strl.ToArray());
            continentNameCB.Enabled = true;
            wowDirCB.Enabled = true;
            meshTB.ReadOnly = false;
            button1.Enabled = true;
            continentNameCB.SelectedIndex = 2;
        }

    }
}
