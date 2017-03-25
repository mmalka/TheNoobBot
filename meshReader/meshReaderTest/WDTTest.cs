using meshDatabase;
using meshReader;
using meshReader.Game;
using NUnit.Framework;

namespace meshReaderTest
{
    [TestFixture]
    public class WDTTest
    {
        static WDTTest()
        {
            //MpqManager.Initialize("S:\\WoW");
        }

        [Test]
        public void TestAzeroth()
        {
            var wdt = new WDT(@"World\maps\Azeroth\Azeroth.wdt");
            Assert.IsTrue(wdt.IsValid);
        }

        [Test]
        public void TestKalimdor()
        {
            var wdt = new WDT(@"World\maps\Kalimdor\Kalimdor.wdt");
            Assert.IsTrue(wdt.IsValid);
        }

        [Test]
        public void TestExpansion01()
        {
            var wdt = new WDT(@"World\maps\Expansion01\Expansion01.wdt");
            Assert.IsTrue(wdt.IsValid);
        }

        [Test]
        public void TestNorthrend()
        {
            var wdt = new WDT(@"World\maps\Northrend\Northrend.wdt");
            Assert.IsTrue(wdt.IsValid);
        }

        [Test]
        public void TestWorldModelContinent()
        {
            var wdt = new WDT(@"World\maps\OrgrimmarInstance\OrgrimmarInstance.wdt");
            Assert.IsTrue(wdt.IsValid);
            Assert.IsTrue(wdt.IsGlobalModel);
            Assert.That(wdt.ModelFile, Is.Not.Null.Or.Empty);
            Assert.IsNotNull(wdt.ModelDefinition);
        }
    }
}