using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using meshPathVisualizer;
using System.Xml;
using meshDatabase;

namespace TheNoobViewer
{
    public partial class TheNoobViewer : Form
    {
        private List<Hop> Hops;
        private float zoom;
        private string path;
        private string continent;
        private bool ignorewater;

        enum ProfileType
        {
            None = 0,
            Gatherer = 1,
            Grinder = 2,
            Fisher = 3,
        }

        enum Zones
        {
            Azeroth,
            Kalimdor,
            Northrend,
            Expansion01,
            TolBarad,
        }

        public TheNoobViewer()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InitializeComponent();
            ZoomCombo.Items.Add("Zoom: 200%");
            ZoomCombo.Items.Add("Zoom: 100%");
            ZoomCombo.Items.Add("Zoom: 50%");
            ZoomCombo.Items.Add("Zoom: 25%");
            ZoomCombo.Items.Add("Zoom: 15%");
            ZoomCombo.SelectedIndex = 1;
            if (!MpqManager.Initialize())
                Environment.Exit(1);
            Hops = new List<Hop>();
            zoom = 1;
            path = Application.StartupPath;

            menuWebLink.Text = "The Noob Bot";
            ignorewater = false;
            textBox1.Visible = false;
            Azeroth.Checked = true;
            continent = "Azeroth";
            toolStripStatusContinent.Text = "Current continent: Azeroth";
        }

        public System.Drawing.Rectangle GetScreen()
        {
            return Screen.FromControl(this).Bounds;
        }

        private void Draw(bool erase, bool autoZoom)
        {
            if (erase)
            {
                pictureBox1.Image = new System.Drawing.Bitmap(519, 411);
                toolStripStatusLabel1.Text = "Please open a file";
                textBox1.Visible = true;
                textBox1.Text = "Invalid profile";
            }
            else
            {
                try
                {
                    textBox1.Text = "Please Wait...";
                    textBox1.Visible = true;
                    this.Refresh();
                    var image = new PathImage(continent, Hops, autoZoom, zoom, ignorewater);
                    image.Generate(GetScreen().Width, GetScreen().Height, out zoom);
                    textBox1.Visible = false;
                    pictureBox1.Image = image.Result;
                    toolStripStatusLabel1.Text = Hops.Count + " nodes in the path";
                }
                catch
                {
                    pictureBox1.Image = new System.Drawing.Bitmap(519, 411);
                    toolStripStatusLabel1.Text = "Please open a file";
                    textBox1.Visible = true;
                    textBox1.Text = "An error occured, be sure to use the proper map";
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML Files (.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.InitialDirectory = path;
            openFileDialog1.RestoreDirectory = false;

            openFileDialog1.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            DialogResult userClick = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClick == System.Windows.Forms.DialogResult.OK)
            {
                ProfileType type = ProfileType.None;
                // Open the selected file to read.
                System.IO.Stream fileStream = openFileDialog1.OpenFile();
                // save path
                path = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                
                textBox1.Text = "Please Wait...";
                textBox1.Visible = true;
                this.Refresh();

                XmlTextReader reader = new XmlTextReader(fileStream);
                while (reader.Read())
                {
                    if (reader.Name == "GathererProfile")
                    {
                        type = ProfileType.Gatherer;
                        break;
                    }
                    if (reader.Name == "GrinderProfile")
                    {
                        type = ProfileType.Grinder;
                        break;
                    }
                    if (reader.Name == "FisherbotProfile")
                    {
                        type = ProfileType.Fisher;
                        break;
                    }

                }
                reader.Close();

                XmlDocument doc = new XmlDocument();
                doc.Load(openFileDialog1.FileName);

                XmlNodeList AllPoints = null;
                switch (type)
                {
                    case ProfileType.Gatherer:
                        {
                            AllPoints = doc.SelectNodes("/GathererProfile/Points/Point");
                            break;
                        }
                    case ProfileType.Grinder:
                        {
                            AllPoints = doc.SelectNodes("/GrinderProfile/GrinderZones/GrinderZone/Points/Point");
                            break;
                        }
                    case ProfileType.Fisher:
                        {
                            AllPoints = doc.SelectNodes("/FisherbotProfile/Points/Point");
                            break;
                        }

                }
                Hops.Clear();
                if (AllPoints != null)
                {
                    foreach (XmlNode OnePoint in AllPoints)
                    {
                        float X = float.Parse(OnePoint.SelectSingleNode("X").InnerText);
                        float Y = float.Parse(OnePoint.SelectSingleNode("Y").InnerText);
                        float Z = float.Parse(OnePoint.SelectSingleNode("Z").InnerText);
                        Hop h = new Hop { Location = new Vector3(X, Y, Z) };
                        Hops.Add(h);
                    }
                    Draw(false, true);
                }
                else
                {
                    Draw(true, false);
                }
                SelectZoom();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uncheckAll()
        {
            Azeroth.Checked = false;
            Kalimdor.Checked = false;
            Outland.Checked = false;
            Northrend.Checked = false;
            Pandaria.Checked = false;
            TolBarad.Checked = false;
            Vashjir.Checked = false;
            Deepholm.Checked = false;
            Darkmoon.Checked = false;
            IsleThunder.Checked = false;
            ignorewater = false;
        }

        private void Azeroth_Click(object sender, EventArgs e)
        {
            uncheckAll();
            Azeroth.Checked = true;
            continent = "Azeroth";
            toolStripStatusContinent.Text = "Current continent: Azeroth";
        }
        
        private void Kalimdor_Click(object sender, EventArgs e)
        {
            uncheckAll();
            Kalimdor.Checked = true;
            continent = "Kalimdor";
            toolStripStatusContinent.Text = "Current continent: Kalimdor";
        }

        private void Outland_Click(object sender, EventArgs e)
        {
            uncheckAll();
            Outland.Checked = true;
            continent = "Expansion01";
            toolStripStatusContinent.Text = "Current continent: Outland";
        }

        private void Northrend_Click(object sender, EventArgs e)
        {
            uncheckAll();
            Northrend.Checked = true;
            continent = "Northrend";
            toolStripStatusContinent.Text = "Current continent: Northrend";
        }

        private void Pandaria_Click(object sender, EventArgs e)
        {
            uncheckAll();
            Pandaria.Checked = true;
            continent = "HawaiiMainLand";
            toolStripStatusContinent.Text = "Current continent: Pandaria";

        }

        private void TolBarad_Click(object sender, EventArgs e)
        {
            uncheckAll();
            TolBarad.Checked = true;
            continent = "TolBarad";
            toolStripStatusContinent.Text = "Current continent: Tol Barad";
        }

        private void Vashjir_Click(object sender, EventArgs e)
        {
            uncheckAll();
            Azeroth.Checked = true;
            Vashjir.Checked = true;
            continent = "Azeroth";
            toolStripStatusContinent.Text = "Current continent: Azeroth - Vashj'ir";
            ignorewater = true;
        }
 
        private void Deepholm_Click(object sender, EventArgs e)
        {
            uncheckAll();
            Deepholm.Checked = true;
            continent = "Deephome";
            toolStripStatusContinent.Text = "Current continent: Deepholm";
        }

        private void Darkmoon_Click(object sender, EventArgs e)
        {
            uncheckAll();
            Darkmoon.Checked = true;
            continent = "Darkmoonfaire";
            toolStripStatusContinent.Text = "Current continent: Darkmoon Island";
        }

        private void IsleThunder_Click(object sender, EventArgs e)
        {
            uncheckAll();
            IsleThunder.Checked = true;
            continent = "MoguIslandDailyArea";
            toolStripStatusContinent.Text = "Current continent: Isle of Thunder";
        }

        private void menuWebLink_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(GetDefaultBrowserPath(), "http://www.thenoobbot.com");
        }

        private static string GetDefaultBrowserPath()
        {
            string key = @"http\shell\open\command";
            Microsoft.Win32.RegistryKey registryKey =
            Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(key, false);
            return ((string)registryKey.GetValue(null, null)).Split('"')[1];
        }

        private void SelectZoom()
        {
            if (zoom == 2.0f)
            {
                ZoomCombo.SelectedIndex = 0;
            }
            else if (zoom == 1.0f)
            {
                ZoomCombo.SelectedIndex = 1;
            }
            else if (zoom == 0.5f)
            {
                ZoomCombo.SelectedIndex = 2;
            }
            else if (zoom == 0.25f)
            {
                ZoomCombo.SelectedIndex = 3;
            }
            else // if (zoom == 0.15f)
            {
                ZoomCombo.SelectedIndex = 4;
            }
        }

        private void ZoomCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // need to detect a change not matching the zoom
            switch (ZoomCombo.SelectedIndex)
            {
                case 0:
                {
                    if (zoom != 2.0f)
                    {
                        zoom = 2.0f;
                        Draw(false, false);
                    }
                    break;
                }
                case 1:
                {
                    if (zoom != 1.0f)
                    {
                        zoom = 1.0f;
                        Draw(false, false);
                    }
                    break;
                }
                case 2:
                {
                    if (zoom != 0.5f)
                    {
                        zoom = 0.5f;
                        Draw(false, false);
                    }
                    break;
                }
                case 3:
                {
                    if (zoom != 0.25f)
                    {
                        zoom = 0.25f;
                        Draw(false, false);
                    }
                    break;
                }
                case 4:
                {
                    if (zoom != 0.15f)
                    {
                        zoom = 0.15f;
                        Draw(false, false);
                    }
                    break;
                }
                default:
                    break;
            }
        }
    }
}
