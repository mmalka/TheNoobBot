using meshReader.Game;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace meshReaderTest.Dungeon
{

    [TestFixture]
    public class GnomeragonMeshTest : DungeonMeshTest
    {

        [TestFixtureSetUp]
        public void Initialize()
        {
            Initialize("S:\\meshReader\\Meshes\\GnomeragonInstance");
        }

        [Test]
        public void StartToBoss()
        {
            TryPath(new Vector3(-504f, -216.4f, 551.1f).ToWoW(), new Vector3(-61.2f, -156.4f, 384.6f).ToWoW());
        }

    }

}