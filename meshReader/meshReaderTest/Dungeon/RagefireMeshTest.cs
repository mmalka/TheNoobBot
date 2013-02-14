using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace meshReaderTest.Dungeon
{
    
    [TestFixture]
    public class RagefireMeshTest : DungeonMeshTest
    {
        
        [TestFixtureSetUp]
        public void Initialize()
        {
            Initialize("S:\\meshReader\\Meshes\\OrgrimmarInstance");
        }

        [Test]
        public void StartToBoss()
        {
            TryPath(new Vector3(2.6f, -8.2f, -15f), new Vector3(-242.6f, 156.5f, -18.3f));
        }

    }

}