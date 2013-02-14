using meshDatabase;
using meshReader.Game.ADT;
using NUnit.Framework;

namespace meshReaderTest
{
    
    [TestFixture]
    public class ADTTest
    {
        [Test(Description = "Test loading one of the games' ADTs")]
        public void TestGameAdt()
        {
            MpqManager.Initialize("S:\\WoW");
            var test = new ADT("World\\maps\\Northrend\\Northrend_43_29.adt");
            test.Read();

            Assert.AreEqual(test.MapChunks.Length, 16*16);
            Assert.IsNotNull(test.DoodadHandler.Vertices);
            Assert.IsNotNull(test.DoodadHandler.Triangles);
        }
    }

}