using meshDatabase;
using meshReader.Game.ADT;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using System.Linq;

namespace meshDisplay
{

    public class AdtDrawer : DrawableGameComponent
    {
        private string _path;
        private ADT _adt;
        private BasicEffect _effect;
        private DepthStencilState _depthState;

        private GeometryDrawer _terrain;
        private GeometryDrawer _doodads;
        private GeometryDrawer _wmos;
        private GeometryDrawer _liquid;

        public AdtDrawer(Microsoft.Xna.Framework.Game game, string path) : base(game)
        {
            _path = path;
        }

        protected override void LoadContent()
        {
            _adt = new ADT(_path);
            _adt.Read();

            _terrain = new GeometryDrawer();
            _terrain.Initialize(Game, Color.Green, _adt.MapChunks.Select(mc => mc.Vertices),
                                _adt.MapChunks.Select(mc => mc.Triangles));

            var firstVert = _adt.MapChunks[0].Vertices[0];
            meshDisplay.Game.Camera.Camera.Position = new Vector3(firstVert.Y, firstVert.Z, firstVert.X);

            if (_adt.DoodadHandler.Triangles != null)
            {
                _doodads = new GeometryDrawer();
                _doodads.Initialize(Game, Color.Yellow, _adt.DoodadHandler.Vertices, _adt.DoodadHandler.Triangles);
            }

            if (_adt.WorldModelHandler.Triangles != null)
            {
                _wmos = new GeometryDrawer();
                _wmos.Initialize(Game, Color.Red, _adt.WorldModelHandler.Vertices, _adt.WorldModelHandler.Triangles);
            }

            if (_adt.LiquidHandler.Triangles != null)
            {
                _liquid = new GeometryDrawer();
                _liquid.Initialize(Game, Color.Blue, _adt.LiquidHandler.Vertices, _adt.LiquidHandler.Triangles);
            }

            _effect = new BasicEffect(GraphicsDevice);
            _depthState = new DepthStencilState {DepthBufferEnable = true};
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = _depthState;
            GraphicsDevice.RasterizerState = meshDisplay.Game.GeneralDialog.IsWireframeEnabled ? meshDisplay.Game.WireframeMode : RasterizerState.CullNone;
            _effect.View = meshDisplay.Game.Camera.Camera.View;
            _effect.Projection = meshDisplay.Game.Camera.Camera.Projection;
            _effect.World = Matrix.Identity;
            _effect.TextureEnabled = false;
            _effect.VertexColorEnabled = true;
            _effect.EnableDefaultLighting();
            _effect.CurrentTechnique.Passes[0].Apply();

            if (meshDisplay.Game.GeneralDialog.DrawTerrain)
                _terrain.Draw();

            if (meshDisplay.Game.GeneralDialog.DrawDoodads && _doodads != null)
                _doodads.Draw();

            if (meshDisplay.Game.GeneralDialog.DrawWmos && _wmos != null)
                _wmos.Draw();
            
            if (meshDisplay.Game.GeneralDialog.DrawWater && _liquid != null)
                _liquid.Draw();

            base.Draw(gameTime);
        }

    }

}