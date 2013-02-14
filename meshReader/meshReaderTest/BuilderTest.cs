using meshBuilder;
using meshDatabase;
using meshReader;
using NUnit.Framework;

namespace meshReaderTest
{
    
    [TestFixture]
    public class BuilderTest
    {
        
        [Test]
        public void FlightmasterTest()
        {
            MpqManager.Initialize("S:\\WoW");

            var builder = new TileBuilder("Azeroth", 36, 49);
            builder.Build(new ConsoleLog());
        }

    }

}