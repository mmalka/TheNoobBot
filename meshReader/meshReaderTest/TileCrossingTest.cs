using meshBuilder;
using meshDatabase;
using meshPather;
using meshReader;
using SlimDX;
using NUnit.Framework;

namespace meshReaderTest
{
    
    [TestFixture]
    public class TileCrossingTest
    {

        [Ignore("Should be explicitly tested only - takes 4+ minutes since it generates tile navmesh data")]
        public void CrossingTest()
        {
            //MpqManager.Initialize("I:\\WoW");
            byte[] dataA, dataB;

            // Build tile A
            {
                var builder = new TileBuilder("Azeroth", 31, 49);
                builder.PrepareData(new ConsoleLog());
                dataA = builder.Build();
                Assert.IsNotNull(dataA);
            }

            // Build tile B
            {
                var builder = new TileBuilder("Azeroth", 32, 49);
                builder.PrepareData(new ConsoleLog());
                dataB = builder.Build();
                Assert.IsNotNull(dataB);
            }

            // Load into mesh
            var pather = new Pather("G:\\Meshes\\Azeroth");
            Assert.IsTrue(pather.LoadTile(dataA));
            Assert.IsTrue(pather.LoadTile(dataB));
            
            // and try pathing, coords from AzerothMeshTest -> TileCrossing which is a non-building version of this
            var start = new Vector3(-9467.8f, 64.2f, 55.9f);
            var end = new Vector3(-9248.9f, -93.35f, 70.3f);
            var path = pather.FindPath(start, end);

            // check result
            Assert.IsNotNull(path);
            Assert.Greater(path.Count, 0);
            Assert.Less((end - path[path.Count-1].Location).Length(), 3f);
        }

    }

}