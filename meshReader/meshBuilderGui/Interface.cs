using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using meshBuilder;
using meshDatabase;
using meshReader.Game;
using Microsoft.Win32;

namespace meshBuilderGui
{
    public partial class Interface : Form
    {
        private ContinentBuilder _builder;
        private DungeonBuilder _dungeonBuilder;
        private Thread _buildThread;
        private int _lastProgressX;
        private bool _buildStarted = false;

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
            if (continentNameCB.Text == string.Empty)
                return;

            try
            {
                MpqManager.Initialize(wowDirCB.Text);

                var wdt = new WDT("World\\maps\\" + continentNameCB.Text + "\\" + continentNameCB.Text + ".wdt");
                if (!wdt.IsValid)
                    return;

                if (wdt.IsGlobalModel)
                {
                    _dungeonBuilder = new DungeonBuilder(continentNameCB.Text);
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

                    startXBox.Text = startX.ToString();
                    startXBox.ReadOnly = true;
                    startYBox.Text = startY.ToString();
                    startYBox.ReadOnly = true;
                    countXBox.Text = countX.ToString();
                    countXBox.ReadOnly = true;
                    countYBox.Text = countY.ToString();
                    countYBox.ReadOnly = true;

                    _builder = new ContinentBuilder(continentNameCB.Text, meshTB.Text, startX, startY, countX, countY);
                    _builder.OnTileEvent += OnTileEvent;
                    _dungeonBuilder = null;
                }

                _buildStarted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            continentNameCB.Enabled = false;
            wowDirCB.Enabled = false;
            meshTB.ReadOnly = true;
            button1.Enabled = false;
            button1.Text = "Building...";
            
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
                    var log = new MemoryLog();
                    var mesh = _dungeonBuilder.Build(log);

                    if (Directory.Exists(_dungeonBuilder.Dungeon))
                        Directory.Delete(_dungeonBuilder.Dungeon, true);
                    Directory.CreateDirectory(_dungeonBuilder.Dungeon);
                    log.WriteToFile(_dungeonBuilder.Dungeon + "\\Build.log");
                    if (mesh != null)
                        File.WriteAllBytes(_dungeonBuilder.Dungeon + "\\" + _dungeonBuilder.Dungeon + ".dmesh", mesh);
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
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_buildStarted)
            {
                button1.Text = ContinentBuilder.PercentProgression() + " %";
                statL.Text = ContinentBuilder.CurrentTile();
                timeLeftL.Text = "Time Left: " + ContinentBuilder.GetTimeLeft();
            }
        }
    }
}
