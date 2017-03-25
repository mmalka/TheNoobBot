using meshDatabase;
using meshReader.Game;
using meshReader.Game.ADT;
using meshReader.Game.WMO;
using SlimDX;
using NUnit.Framework;
using System.Linq;

namespace meshReaderTest
{
    [TestFixture]
    public class WMOTest
    {
        [Test(Description = "Test reading a game wmo")]
        public void TestGameWmo()
        {
            //MpqManager.Initialize("S:\\WoW");
            var root = new WorldModelRoot("world\\wmo\\Northrend\\Battleground\\ND_BG_Keep01.wmo");
        }

        [Test]
        public void TestDungeonWithWater()
        {
            //MpqManager.Initialize("S:\\World of Warcraft");
            var wdt = new WDT("world\\maps\\orgrimmarinstance\\orgrimmarinstance.wdt");
            Assert.IsTrue(wdt.IsValid && wdt.IsGlobalModel);
            var file = wdt.ModelFile;
            var model = new WorldModelRoot(file);
            var verts = new System.Collections.Generic.List<Vector3>();
            var tris = new System.Collections.Generic.List<Triangle<uint>>();
            WorldModelHandler.InsertModelGeometry(verts, tris, wdt.ModelDefinition, model);
            Assert.IsFalse(verts.Any(v => float.IsNaN(v.X) || float.IsNaN(v.Y) || float.IsNaN(v.Z)));
        }
    }
}