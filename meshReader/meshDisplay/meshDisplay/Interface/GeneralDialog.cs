using meshDatabase;
using meshReader;
using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;

namespace meshDisplay.Interface
{
    
    public class GeneralDialog
    {
        private readonly Manager _manager;
        private readonly Window _window;
        private readonly CheckBox _wireframe;
        private readonly CheckBox _terrain;
        private readonly CheckBox _doodad;
        private readonly CheckBox _wmo;
        private readonly CheckBox _water;
        private readonly Console _console;

        public GeneralDialog(Manager manager)
        {
            _manager = manager;

            _window = new Window(_manager);
            _window.Init();
            _window.Text = "General";
            _window.Width = 200;
            _window.Height = 200;
            _window.Left = 809;
            _window.Top = 15;
            _window.Visible = true;
            _window.Movable = false;
            _window.CloseButtonVisible = false;

            _wireframe = new CheckBox(_manager);
            _wireframe.Init();
            _wireframe.Text = "Wireframe";
            _wireframe.Width = 100;
            _wireframe.Height = 24;
            _wireframe.Anchor = Anchors.Bottom;
            _wireframe.Left = 15;
            _wireframe.Top = 15;
            _wireframe.Visible = true;
            _wireframe.Parent = _window;

            _terrain = new CheckBox(_manager);
            _terrain.Init();
            _terrain.Text = "Terrain";
            _terrain.Width = 100;
            _terrain.Height = 24;
            _terrain.Anchor = Anchors.Bottom;
            _terrain.Left = 15;
            _terrain.Top = 45;
            _terrain.Visible = true;
            _terrain.Parent = _window;

            _doodad = new CheckBox(_manager);
            _doodad.Init();
            _doodad.Text = "Doodads";
            _doodad.Width = 100;
            _doodad.Height = 24;
            _doodad.Anchor = Anchors.Bottom;
            _doodad.Left = 15;
            _doodad.Top = 75;
            _doodad.Visible = true;
            _doodad.Parent = _window;

            _wmo = new CheckBox(_manager);
            _wmo.Init();
            _wmo.Text = "WMOs";
            _wmo.Width = 100;
            _wmo.Height = 24;
            _wmo.Anchor = Anchors.Bottom;
            _wmo.Left = 15;
            _wmo.Top = 105;
            _wmo.Visible = true;
            _wmo.Parent = _window;

            _water = new CheckBox(_manager);
            _water.Init();
            _water.Text = "Water";
            _water.Width = 100;
            _water.Height = 24;
            _water.Anchor = Anchors.Bottom;
            _water.Left = 15;
            _water.Top = 135;
            _water.Visible = true;
            _water.Parent = _window;

            _console = new Console(_manager);
            _console.Init();
            _console.Left = 15;
            _console.Top = 15;
            _console.Width = 779;
            _console.Height = 200;
            _console.Visible = true;
            _console.Text = "meshDisplay Console";
            _console.Visible = true;
            _console.Movable = false;
            _console.Channels.Add(new ConsoleChannel(1, "Default", Color.White));
            _console.MessageSent += HandleMessage;

            _manager.Add(_console);
            _manager.Add(_window);
        }

        static void HandleMessage(object sender, ConsoleMessageEventArgs e)
        {
            if (e.Message.Text.StartsWith("load "))
            {
                var path = e.Message.Text.Substring(5).Replace("/", "\\");
                Program.Game.AddAdt(path);
            }
            else if (e.Message.Text.StartsWith("loadInstance "))
            {
                var path = e.Message.Text.Substring(13).Replace("/", "\\");
                Program.Game.AddInstance(path);
            }
            else if (e.Message.Text.StartsWith("loadMesh "))
            {
                var args = e.Message.Text.Substring(9).Split(' ');
                if (args.Length == 3)
                    Program.Game.AddMesh(args[0], int.Parse(args[1]), int.Parse(args[2]));
            }
        }
        
        public bool DrawTerrain
        {
            get
            {
                return _terrain.Checked;
            }
        }

        public bool DrawDoodads
        {
            get
            {
                return _doodad.Checked;
            }
        }

        public bool DrawWmos
        {
            get
            {
                return _wmo.Checked;
            }
        }

        public bool DrawWater
        {
            get
            {
                return _water.Checked;
            }
        }

        public bool IsWireframeEnabled
        {
            get
            {
                return _wireframe.Checked;
            }
        }

    }

}