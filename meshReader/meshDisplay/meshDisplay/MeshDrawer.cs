using System.IO;
using DetourLayer;
using meshReader.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace meshDisplay
{
    
    public class MeshDrawer : DrawableGameComponent
    {
        private NavMesh _mesh;
        private readonly string _continent;
        private readonly int _tileX;
        private readonly int _tileY;
        private readonly int _i;
        private readonly int _j;
        private GeometryDrawer _drawer;
        private BasicEffect _effect;
        private DepthStencilState _depthState;

        public MeshDrawer(Game game, string continent, int tileX, int tileY, int i=0, int j=0)
            : base(game)
        {
            _continent = continent;
            _tileX = tileX;
            _tileY = tileY;
            _i = i;
            _j = j;
        }

        protected override void LoadContent()
        {
            string Folder = @"G:\Meshes\6.0\";
            _mesh = new NavMesh();
            if (File.Exists(Folder + _continent + "\\" + _continent + ".dmesh"))
            {
                _mesh.Initialize(File.ReadAllBytes(Folder + _continent + "\\" + _continent + ".dmesh"));
            }
            else
            {
                MeshTile discard;
                if (Constant.Division == 1)
                {
                    _mesh.Initialize(150000, 4096, World.Origin, Constant.BaseTileSize, Constant.BaseTileSize);
                    _mesh.AddTile(File.ReadAllBytes(Folder + _continent + "\\" + _continent + "_" + _tileX + "_" + _tileY + ".tile"), out discard);
                }
                else
                {
                    _mesh.Initialize(150000, 4096, World.Origin, Constant.TileSize, Constant.TileSize);
                    string name = _continent + "\\" + _continent + "_" + _tileX + "_" + _tileY + "_" + _i + _j + ".tile";
                    _mesh.AddTile(File.ReadAllBytes(Folder + name), out discard);
                    System.Console.WriteLine(name);
                }
            }
            float[] vertices;
            int[] tris;
            if (Constant.Division == 1)
                _mesh.BuildRenderGeometry(_tileX, _tileY, out vertices, out tris);
            else
                _mesh.BuildRenderGeometry(_tileX * Constant.Division + _i, _tileY * Constant.Division + _j, out vertices, out tris);
            _drawer = new GeometryDrawer();
            _drawer.Initialize(Game, new Color(0.1f, 0.1f, 0.9f, 0.5f), vertices, tris);
            _effect = new BasicEffect(GraphicsDevice);
            _depthState = new DepthStencilState { DepthBufferEnable = true };
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = _depthState;
            GraphicsDevice.RasterizerState = meshDisplay.Game.GeneralDialog.IsWireframeEnabled ? meshDisplay.Game.WireframeMode : RasterizerState.CullNone;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _effect.View = meshDisplay.Game.Camera.Camera.View;
            _effect.Projection = meshDisplay.Game.Camera.Camera.Projection;
            _effect.World = Matrix.Identity;
            _effect.TextureEnabled = false;
            _effect.VertexColorEnabled = true;
            _effect.Alpha = 0.6f;
            _effect.EnableDefaultLighting();
            _effect.CurrentTechnique.Passes[0].Apply();

            _drawer.Draw();
            base.Draw(gameTime);
        }
    }

}