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
        private GeometryDrawer _drawer;
        private BasicEffect _effect;
        private DepthStencilState _depthState;

        public MeshDrawer(Game game, string continent, int tileX, int tileY)
            : base(game)
        {
            _continent = continent;
            _tileX = tileX;
            _tileY = tileY;
        }

        protected override void LoadContent()
        {
            _mesh = new NavMesh();
            if (File.Exists(@"G:\Meshes\" + _continent + "\\" + _continent + ".dmesh"))
            {
                _mesh.Initialize(File.ReadAllBytes(@"G:\Meshes\" + _continent + "\\" + _continent + ".dmesh"));
            }
            else
            {
                _mesh.Initialize(32768, 128, World.Origin, Constant.TileSize, Constant.TileSize);
                MeshTile discard;
                _mesh.AddTile(
                    File.ReadAllBytes(@"G:\Meshes\" + _continent + "\\" + _continent + "_" + _tileX + "_" +
                                      _tileY + ".tile"), out discard);
            }
            float[] vertices;
            int[] tris;
            _mesh.BuildRenderGeometry(_tileX, _tileY, out vertices, out tris);
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