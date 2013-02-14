using System.Collections.Generic;
using System.Globalization;
using System.IO;
using meshReader.Game;
using meshReader.Game.ADT;
using meshReader.Game.WMO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace meshDisplay
{
    
    public class WmoDrawer : DrawableGameComponent
    {
        private readonly string _path;
        private readonly WorldModelHandler.WorldModelDefinition _def;
        private GeometryDrawer _drawer;
        private BasicEffect _effect;
        private DepthStencilState _depthState;

        public WmoDrawer(Microsoft.Xna.Framework.Game game, string path,  WorldModelHandler.WorldModelDefinition def) : base(game)
        {
            _path = path;
            _def = def;
        }

        private static void SaveMeshToDisk(IEnumerable<Vector3> verts, IEnumerable<Triangle<uint>> tris)
        {
            using (var s = new StreamWriter("S:\\recast\\Meshes\\wmoDrawer.obj", false))
            {
                foreach (var vert in verts)
                {
                    s.WriteLine("v " + vert.Y.ToString(CultureInfo.InvariantCulture) + " " +
                                vert.Z.ToString(CultureInfo.InvariantCulture) + " " +
                                vert.X.ToString(CultureInfo.InvariantCulture));
                }

                foreach (var tri in tris)
                {
                    s.WriteLine("f " + (tri.V0 + 1) + " " + (tri.V1 + 1) + " " + (tri.V2 + 1));
                }

                s.Flush();
            }
        }

        protected override void LoadContent()
        {
            var model = new WorldModelRoot(_path);
            var verts = new List<Vector3>();
            var tris = new List<Triangle<uint>>();
            WorldModelHandler.InsertModelGeometry(verts, tris, _def, model);
            SaveMeshToDisk(verts, tris);
            meshDisplay.Game.Camera.Camera.Position = new Vector3(verts[0].Y, verts[0].Z, verts[0].X);

            _drawer = new GeometryDrawer();
            _drawer.Initialize(Game, Color.Red, verts, tris);

            _effect = new BasicEffect(GraphicsDevice);
            _depthState = new DepthStencilState { DepthBufferEnable = true };
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

            _drawer.Draw();
            base.Draw(gameTime);
        }
    }

}