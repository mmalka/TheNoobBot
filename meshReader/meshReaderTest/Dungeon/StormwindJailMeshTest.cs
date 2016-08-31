using meshReader.Game;
using SlimDX;
using NUnit.Framework;

namespace meshReaderTest.Dungeon
{

    [TestFixture]
    public class StormwindJailMeshTest : DungeonMeshTest
    {

        [OneTimeSetUp]
        public void Initialize()
        {
            Initialize("S:\\meshReader\\Meshes\\StormwindJail");
        }

        [Test]
        public void StartToLeftWing()
        {
            TryPath(new Vector3(-0.8f, -25.2f, -74.4f).ToWoW(), new Vector3(-134f, -33.1f, -167.2f).ToWoW());
        }

        [Test]
        public void StartToRightWing()
        {
            TryPath(new Vector3(-0.8f, -25.2f, -74.4f).ToWoW(), new Vector3(132.7f, -32.8f, -91.5f).ToWoW());
        }

        [Test]
        public void StartToFinalRoom()
        {
            TryPath(new Vector3(-0.8f, -25.2f, -74.4f).ToWoW(), new Vector3(-1.2f, -24.9f, -165.2f).ToWoW());
        }

    }

}