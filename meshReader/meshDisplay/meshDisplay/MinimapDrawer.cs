using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace meshDisplay
{
    
    public class MinimapDrawer : DrawableGameComponent
    {
        public bool ShouldDraw { get; set; }

        private void GenerateTexture()
        {
            // generate a Texture2D from the raw pixel data. SurfaceType Color
        }

        public MinimapDrawer(Game game)
            : base(game)
        {
            
        }
    }

}