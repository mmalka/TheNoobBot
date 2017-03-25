using System.Drawing.Imaging;
using meshDatabase;
using meshPather;
using meshPathVisualizer;
using SlimDX;
using NUnit.Framework;
using RecastLayer;

namespace meshReaderTest
{
    [TestFixture]
    public class VisualizerTest : MeshTest
    {
        static private string iBaseMeshesPath;

        [OneTimeSetUp]
        public void Initialize(string MeshesPath, string WowPath)
        {
            iBaseMeshesPath = MeshesPath;
            string fullPath = iBaseMeshesPath + "Draenor";
            Initialize(@fullPath);
            MpqManager.Initialize(@WowPath);
            //Pather.LoadAllTiles();
        }

        [Test]
        public void TestMinimapImage()
        {
            float x, y;
            Pather.GetWoWTileByLocation(new[] {-8020, 1515, -1.5f}.ToRecast(), out x, out y);

            var image = new MinimapImage("Azeroth", 256, 256, (int) x, (int) x, (int) y, (int) y);
            image.Generate();
            image.Result.Save("MinimapImageTest.png", ImageFormat.Png);
        }

        [Test]
        public void TestPathImageDraenor()
        {
            System.Collections.Generic.List<Hop> path;
            var pt1 = new Vector3(4069.933f, -2376.877f, 94.60396f); // porte de tenebres
            var pt2 = new Vector3(1845.853f, -4396.072f, 135.2314f);
            var pop = new Vector3(1583.356f, 106.8728f, 65.87754f);
            var TelaariNagran = new Vector3(2563.416f, 5700.69f, 102.2419f);

            string ipath = iBaseMeshesPath + "Draenor";
            Pather = new Pather(@ipath, MockConnectionHandler);
            Pather.Filter.SetAreaCost((int) PolyArea.Water, 4);
            Pather.Filter.SetAreaCost((int) PolyArea.Terrain, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Road, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Danger, 20);

            double l;
            PathImage image;

            System.Console.WriteLine("Test Draenor...");
            l = TryHugePath(pop, TelaariNagran, out path);
            image = new PathImage(Pather.Continent, 128 * 11, 128 * 4, path);
            image.Generate();
            image.Result.Save("TestDraenor.png", ImageFormat.Png);
            System.Console.WriteLine("Test Draenor Length : " + l);
            System.Console.WriteLine("--------------------------------------------");
        }

        [Test]
        public void TestPathImageKalimdor()
        {
            System.Collections.Generic.List<Hop> path;
            var bas = new Vector3(1825.286f, -4395.726f, 103.5342f);
            var haut = new Vector3(1845.853f, -4396.072f, 135.2314f);

            string ipath = iBaseMeshesPath + "Kalimdor";
            Pather = new Pather(@ipath, MockConnectionHandler);
            Pather.Filter.SetAreaCost((int) PolyArea.Water, 4);
            Pather.Filter.SetAreaCost((int) PolyArea.Terrain, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Road, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Danger, 20);

            double l;
            PathImage image;

            System.Console.WriteLine("Test Orgrimar...");
            l = TryHugePath(bas, haut, out path);
            image = new PathImage(Pather.Continent, 128 * 1, 128 * 1, path);
            image.Generate();
            image.Result.Save("TestOrgrimmar.png", ImageFormat.Png);
            System.Console.WriteLine("Test Orgrimmar Length : " + l);
            System.Console.WriteLine("--------------------------------------------");
        }

        [Test]
        public void TestPathImageWestern()
        {
            System.Collections.Generic.List<Hop> path;
            // Azeroth
            var IronForgeFly = new Vector3(-4821.13f, -1152.4f, 502.295f);
            var LocModanCityFly = new Vector3(-5424.85f, -2929.87f, 347.645f);
            var BootyTunnel = new Vector3(-14247.34f, 328.0565f, 24.18314f);
            var MalTerreEstNord = new Vector3(3362.56f, -4446.57f, 127.746f);
            var BootyBay = new Vector3(-14374.42f, 382.3631f, 22.77349f);
            var Goldshire = new Vector3(-9465.505f, 73.8429f, 56.57315f);

            var AgentKearnenInWestFall = new Vector3(-11042.13f, 625.1042f, 36.90501f);
            var TopOfTowerInWestfall = new Vector3(-11132.36f, 555.1247f, 70.38676f);

            // Kalimdor
            string ipath = iBaseMeshesPath + "Azeroth";
            Pather = new Pather(@ipath, MockConnectionHandler);
            Pather.Filter.SetAreaCost((int) PolyArea.Water, 4);
            Pather.Filter.SetAreaCost((int) PolyArea.Terrain, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Road, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Danger, 20);

            double l;
            PathImage image;

            /*TryPath(IronForgeFly, LocModanCityFly, out path, true);

            Assert.NotNull(path);

            image = new PathImage(Pather.Continent, 128*5, 128*2, path); // 5 * 2
            image.Generate();
            image.Result.Save("IronForgeLocModan.png", ImageFormat.Png);
            */

            System.Console.WriteLine("AgentKearnen-Tower...");
            l = TryHugePath(AgentKearnenInWestFall, TopOfTowerInWestfall, out path);
            image = new PathImage(Pather.Continent, 128 * 1, 128 * 1, path);
            image.Generate();
            image.Result.Save("AgentKearnenToTower.png", ImageFormat.Png);
            System.Console.WriteLine("AgentKearnen-Tower Length : " + l);
            System.Console.WriteLine("--------------------------------------------");

            /*System.Console.WriteLine("BootyTunnel-BootyBay...");
            l = TryHugePath(BootyTunnel, BootyBay, out path);
            image = new PathImage(Pather.Continent, 128 * 5, 128 * 2, path);
            image.Generate();
            image.Result.Save("BootyTunnelBootyBay.png", ImageFormat.Png);
            System.Console.WriteLine("BootyTunnel-BootyBay Length : " + l);
            System.Console.WriteLine("--------------------------------------------");*/

            System.Console.WriteLine("IronForge-LocModanCity...");
            l = TryPath(IronForgeFly, LocModanCityFly, out path, true);
            image = new PathImage(Pather.Continent, 128 * 5, 128 * 2, path);
            image.Generate();
            image.Result.Save("IronForgeLocModanCity.png", ImageFormat.Png);
            System.Console.WriteLine("IronForge-LocModanCity Length : " + l);
            System.Console.WriteLine("--------------------------------------------");

            System.Console.WriteLine("Goldshire-IronForge...");
            l = TryHugePath(Goldshire, IronForgeFly, out path);
            image = new PathImage(Pather.Continent, 128 * 6, 128 * 10, path);
            image.Generate();
            image.Result.Save("GoldshireIronForge.png", ImageFormat.Png);
            System.Console.WriteLine("Goldshire-IronForge Length : " + l);
            System.Console.WriteLine("--------------------------------------------");

            System.Console.WriteLine("IronForge-Goldshire...");
            l = TryHugePath(IronForgeFly, Goldshire, out path);
            image = new PathImage(Pather.Continent, 128 * 6, 128 * 10, path);
            image.Generate();
            image.Result.Save("IronForgeGoldshire.png", ImageFormat.Png);
            System.Console.WriteLine("IronForge-Goldshire Length : " + l);
            System.Console.WriteLine("--------------------------------------------");

            /*System.Console.WriteLine("Goldshire-BootyTunnel...");
            l = TryHugePath(Goldshire, BootyTunnel, out path);
            image = new PathImage(Pather.Continent, 128 * 2, 128 * 10, path);
            image.Generate();
            image.Result.Save("GoldshireBootyTunnel.png", ImageFormat.Png);
            System.Console.WriteLine("Goldshire-BootyTunnel Length : " + l);
            System.Console.WriteLine("--------------------------------------------");*/

            System.Console.WriteLine("Goldshire-BootyTown...");
            l = TryHugePath(Goldshire, BootyBay, out path);
            image = new PathImage(Pather.Continent, 128 * 2, 128 * 10, path);
            image.Generate();
            image.Result.Save("GoldshireBootyTown.png", ImageFormat.Png);
            System.Console.WriteLine("Goldshire-BootyTown Length : " + l);
            System.Console.WriteLine("--------------------------------------------");

            System.Console.WriteLine("IronForge-Booty...");
            l = TryHugePath(IronForgeFly, BootyBay, out path);
            image = new PathImage(Pather.Continent, 128 * 6, 128 * 18, path);
            image.Generate();
            image.Result.Save("IronForgeBooty.png", ImageFormat.Png);
            System.Console.WriteLine("IronForge-Booty Length : " + l);
            System.Console.WriteLine("--------------------------------------------");

            System.Console.WriteLine("IronForge-MalTerre...");
            l = TryHugePath(IronForgeFly, MalTerreEstNord, out path);
            image = new PathImage(Pather.Continent, 128 * 8, 128 * 18, path);
            image.Generate();
            image.Result.Save("IronForgeMalTerre.png", ImageFormat.Png);
            System.Console.WriteLine("IronForge-MalTerre Length : " + l);
            System.Console.WriteLine("--------------------------------------------");

            // And the final one, the biggest, the best, the .... I am the best ^^

            System.Console.WriteLine("Booty-MalTerre...");
            l = TryHugePath(BootyBay, MalTerreEstNord, out path);
            image = new PathImage(Pather.Continent, 128 * 10, 128 * 34, path);
            image.Generate();
            image.Result.Save("BootyMalTerre.png", ImageFormat.Png);
            System.Console.WriteLine("Booty-MalTerre Length : " + l);
        }

        private static bool MockConnectionHandler(ConnectionData data)
        {
            if (!data.Alliance)
                return false;
            return true;
        }
    }
}