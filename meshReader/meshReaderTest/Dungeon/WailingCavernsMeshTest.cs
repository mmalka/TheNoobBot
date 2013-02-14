using meshReader.Game;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace meshReaderTest.Dungeon
{
 
    [TestFixture]
    public class WailingCavernsMeshTest : DungeonMeshTest
    {
        
        [TestFixtureSetUp]
        public void Initialize()
        {
            Initialize("S:\\meshReader\\Meshes\\WailingCaverns");
        }

        [Test]
        public void StartToFinalEvent()
        {
            TryPath(new Vector3(-135.2f, -76.9f, 130.6f).ToWoW(), new Vector3(-244.8f, -97.8f, -134.6f).ToWoW());
        }

        [Test]
        public void StartToSomeBoss()
        {
            TryPath(new Vector3(-135.2f, -76.9f, 130.6f).ToWoW(), new Vector3(57.7f, -66.2f, 11.3f).ToWoW());
        }

    }

}
