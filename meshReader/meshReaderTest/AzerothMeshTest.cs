using System;
using System.Linq;
using meshPather;
using SlimDX;
using NUnit.Framework;
using RecastLayer;

namespace meshReaderTest
{
    [TestFixture]
    public class AzerothMeshTest : MeshTest
    {
        static private string iMeshesPath;

        [OneTimeSetUp]
        public void Initialize(string MeshesPath, string WoWPath)
        {
            iMeshesPath = MeshesPath + "Azeroth";
            Initialize(@iMeshesPath);
        }

        [Test]
        private void WriteFile(string fName, System.Collections.Generic.List<Hop> path, Vector3 target, int limit = 0, bool reopen = false)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("a_" + fName, reopen);
            file.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            file.WriteLine("<GrinderProfile xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            file.WriteLine("  <Points>");
            for (int i = 0; i < (limit == 0 ? path.Count : limit); i++)
            {
                file.WriteLine("    <Point>");
                file.WriteLine("      <X>{0}</X>", path[i].Location.X);
                file.WriteLine("      <Y>{0}</Y>", path[i].Location.Y);
                file.WriteLine("      <Z>{0}</Z>", path[i].Location.Z);
                file.WriteLine("    </Point>");
            }
            if (limit == 0 && !target.Equals(path[path.Count - 1].Location))
            {
                file.WriteLine("    <Point>");
                file.WriteLine("      <X>{0}</X>", target.X);
                file.WriteLine("      <Y>{0}</Y>", target.Y);
                file.WriteLine("      <Z>{0}</Z>", target.Z);
                file.WriteLine("    </Point>");
            }
            file.WriteLine("  </Points>");
            file.WriteLine("</GrinderProfile>\n");
            file.Close();
        }

        [Test]
        public void TestRival()
        {
            TryPath(new Vector3(-9028.3f, -162.8f, 79.4f), new Vector3(-9103.7f, -106.5f, 80.9f));
        }

        [Test]
        public void TestNeo()
        {
            System.Collections.Generic.List<Hop> walkHops;

            var NorthShire = new Vector3(-8913.293f, -137.431f, 80.78545f); // redone
            var Goldshire = new Vector3(-9465.505f, 73.8429f, 56.57315f); // redone
            var Westfall = new Vector3(-10507.8f, 1044.024f, 60.51819f); // redone
            var LakeShire = new Vector3(-9245.273f, -2211.097f, 66.18014f); // redone

            var IronForgeFly = new Vector3(-4821.13f, -1152.4f, 502.295f);
            var LocModanCityFly = new Vector3(-5424.85f, -2929.87f, 347.645f);

            var Dustwood = new Vector3(-10557.85f, -1147.258f, 28.03498f); // redone
            var BootyBay = new Vector3(-14449.93f, 472.5713f, 15.21084f); // redone
            var inBootyBay = new Vector3(-14420.53f, 479.2f, 15.21084f); // computed !!
            var PassPointStrong = new Vector3(-13050.81f, -363.3668f, 34.68807f); // redone
            var BootyTunnel = new Vector3(-14247.34f, 328.0565f, 24.18314f);

            var StormwindDwarf = new Vector3(-8373.929f, 622.0675f, 95.19361f);
            var etage = new Vector3(-9456.81f, 29.361f, 63.9038f);
            var Stormgriffon = new Vector3(-8835.36f, 490.4935f, 109.9794f);
            var HarrisonJonesSTW = new Vector3(-8295.137f, 232.502f, 155.3478f);

            var AzoraDown = new Vector3(-9517.778f, -680.7117f, 63.0045f);
            var AzoraTop = new Vector3(-9551.597f, -721.6193f, 99.12933f);

            Pather = new Pather(@iMeshesPath, MockConnectionHandler);
            // costs settings
            Pather.Filter.SetAreaCost((int) PolyArea.Water, 4);
            Pather.Filter.SetAreaCost((int) PolyArea.Terrain, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Road, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Danger, 20);
            //Pather.LoadAllTiles();

            Console.WriteLine("Northshire -> Goldshire");
            TryPath(NorthShire, Goldshire, out walkHops, true);
            WriteFile("Northshire-Goldshire.xml", walkHops, Goldshire);

            // works
            /*
            Console.WriteLine("Northshire -> Westfall");
            TryPath(NorthShire, Westfall, out walkHops, true);
            WriteFile("Northshire-Westfall.xml", walkHops, Westfall);
            */
            // works
            /*
            Console.WriteLine("Goldshire -> Stormwind Dwarf");
            TryPath(Goldshire, StormwindDwarf, out walkHops, true);
            WriteFile("Goldshire-StormwindDwarf.xml", walkHops, StormwindDwarf);
            */
            // very small to test height & radius
            /*
            Console.WriteLine("Goldshire -> Lion Pride upstair");
            TryPath(Goldshire, etage, out walkHops, true);
            WriteFile("Goldshire-LionPride.xml", walkHops, etage);
            */
            // very small to test height & radius
            /*
            Console.WriteLine("Goldshire -> Stormgriffon");
            TryPath(Goldshire, Stormgriffon, out walkHops, true);
            WriteFile("Goldshire-Stormgriffon.xml", walkHops, Stormgriffon);
            */
            // another stormwind test
            /*
            Console.WriteLine("Goldshire -> Harrison Jones STW");
            TryPath(Goldshire, HarrisonJonesSTW, out walkHops, true);
            WriteFile("Goldshire-HarrisonJonesSTW.xml", walkHops, HarrisonJonesSTW);
            */
            // Azora tower pb on tile 33/50 it does not go to the last stair (missing 7.5 in height)
            /*
            Console.WriteLine("AzoraDown -> Azora tower top");
            TryPath(AzoraDown, AzoraTop, out walkHops, true);
            WriteFile("AzoraDown-AzoraTop.xml", walkHops, AzoraTop);
            */
            // works
            /*
            Console.WriteLine("Goldshire -> Lakeshire");
            TryPath(Goldshire, LakeShire, out walkHops, true);
            WriteFile("Goldshire-Lakeshire.xml", walkHops, LakeShire);
            */
            // works
            /*
            Console.WriteLine("Northshire -> Lakeshire");
            TryPath(NorthShire, LakeShire, out walkHops, true);
            WriteFile("Northshire-Lakeshire.xml", walkHops, LakeShire);
            */
            // works
            /*
            Console.WriteLine("Goldshire -> Sombre Conté");
            TryPath(Goldshire, Dustwood, out walkHops, true);
            WriteFile("Goldshire-Dustwood.xml", walkHops, Dustwood);
            */
            // ??
            /*
            Console.WriteLine("Sombre Conté -> Booty Bay");
            TryPath(Dustwood, BootyBay, out walkHops, true);
            WriteFile("Dustwood-BootyBay.xml", walkHops, BootyBay);
            */
            // works
            /*
            Console.WriteLine("Sombre Conté -> Passage 2 strong");
            TryPath(Dustwood, PassPointStrong, out walkHops, true);
            WriteFile("Dustwood-PassPointStrong.xml", walkHops, PassPointStrong);
            */
            //
            /*
            Console.WriteLine("Passage 2 strong -> Booty Tunnel");
            TryPath(PassPointStrong, BootyTunnel, out walkHops, true);
            WriteFile("PassPointStrong-BootyTunnel.xml", walkHops, BootyTunnel);
            */
            // fails because cannot enter the tunnel
            /*
            // pb tile not 31,59
            Console.WriteLine("Booty Tunnel -> Booty bay");
            TryPath(BootyTunnel, BootyBay, out walkHops, true);
            WriteFile("Booty Tunnel-BootyBay.xml", walkHops, BootyBay);
            */
            // works
            /*
            Console.WriteLine("Goldshire -> Booty Tunnel");
            TryPath(Goldshire, BootyTunnel, out walkHops, true);
            WriteFile("Goldshire-BootyTunnel.xml", walkHops, BootyTunnel);
            */
            // works
            /*
            Console.WriteLine("Goldshire -> PassPointStrong");
            TryPath(Goldshire, PassPointStrong, out walkHops, true);
            WriteFile("Goldshire-PassPointStrong.xml", walkHops, PassPointStrong);
            */
            // unsure coords

            Console.WriteLine("Ironforge (Fly-Master) -> Loc Modan City (Fly-Master Thorgrum Borrelson)");
            TryPath(IronForgeFly, LocModanCityFly, out walkHops, true);
            WriteFile("Ironforge-Loc Modan City.xml", walkHops, LocModanCityFly);

            Console.WriteLine("Goldshire -> Ironforge");
            TryPath(Goldshire, IronForgeFly, out walkHops, true);
            if ((IronForgeFly - walkHops[walkHops.Count - 1].Location).Length() > 5f)
            {
                int limit = (int) (walkHops.Count * 0.75f);
                Console.WriteLine("Incomplete result of {0} node, restarting at node {1}", walkHops.Count, limit);
                WriteFile("Goldshire-Ironforge.xml", walkHops, IronForgeFly); //, limit);
                TryPath(walkHops[limit].Location, IronForgeFly, out walkHops, true);
                WriteFile("Goldshire-Ironforge.xml", walkHops, IronForgeFly, 0, true);
            }

            //Pather.Filter.ExcludeFlags = (ushort)(PolyFlag.FlightMaster);
            /*Pather = new Pather("X:\\Meshes\\Azeroth", MockConnectionHandler); // accept alliance fly
            Console.WriteLine("Goldshire -> Ironforge by fly");
            TryPath(Goldshire, IronForgeFly, out walkHops, true);
            WriteFile("Goldshire-Ironforge_byFly.xml", walkHops, IronForgeFly);*/
        }


        [Test]
        public void LongTest()
        {
            TryPath(new Vector3(-22, -918, 54), new Vector3(1699, 1706, 135));
        }

        [Test]
        public void HumanStartingArea01()
        {
            TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8929f, -195f, 80f));
        }

        [Test]
        public void HumanStartingArea02()
        {
            TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8903f, -159f, 81.9f));
        }

        [Test]
        public void HumanStartingArea03()
        {
            TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8898f, -173f, 81.5f));
        }

        [Test]
        public void HumanStartingArea04()
        {
            TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8916f, -214f, 82.1f));
        }

        [Test]
        public void HumanStartingArea05()
        {
            TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8909f, -216f, 89.1f));
        }

        [Test]
        public void HumanStartingArea06()
        {
            TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8900f, -181f, 113.1f));
        }

        [Test]
        public void HumanStartingArea07()
        {
            TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8897f, -174f, 115.7f));
        }

        [Test]
        public void TileCrossing()
        {
            TryPath(new Vector3(-9467.8f, 64.2f, 55.9f), new Vector3(-9248.9f, -93.35f, 70.3f));
        }

        [Test]
        public void GoldhainToStormwind()
        {
            TryPath(new Vector3(-9447.5f, 55.4f, 56.2f), new Vector3(-8957.4f, 517.3f, 96.3f));
        }

        [Test(Description = "A longer path to test dynamic tile loading")]
        public void HumanStartToStormwind()
        {
            TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8957.4f, 517.3f, 96.3f));
        }

        [Test(Description = "Test the difference between ground pathing and using flightpathes")]
        public void TestFlightpathes()
        {
            System.Collections.Generic.List<Hop> roadHops;
            Pather.Filter.ExcludeFlags = (ushort) (PolyFlag.FlightMaster);
            TryPath(new Vector3(-9447.5f, 55.4f, 56.2f), new Vector3(-8957.4f, 517.3f, 96.3f), out roadHops, false);

            Pather = new Pather(@iMeshesPath, MockConnectionHandler);
            System.Collections.Generic.List<Hop> flightHops;
            TryPath(new Vector3(-9447.5f, 55.4f, 56.2f), new Vector3(-8957.4f, 517.3f, 96.3f), out flightHops, false);

            Console.WriteLine("Ground path: " + roadHops.Count + " hops, Flight path: " + flightHops.Count + " hops");
            Assert.Less(flightHops.Count, roadHops.Count);
            Assert.IsTrue(flightHops.Any(hop => hop.Type == HopType.Flightmaster && hop.FlightTarget != null));
        }

        private static bool MockConnectionHandler(ConnectionData data)
        {
            if (!data.Alliance)
                return false;
            return true;
        }

        [Test]
        public void TestRoadPriorization()
        {
            Pather.Filter.SetAreaCost((int) PolyArea.Water, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Terrain, 10);
            Pather.Filter.SetAreaCost((int) PolyArea.Road, 1);
            var roadLength = TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8957.4f, 517.3f, 96.3f));
            Console.WriteLine("Path length on road: " + roadLength);

            Pather.Filter.SetAreaCost((int) PolyArea.Water, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Terrain, 1);
            Pather.Filter.SetAreaCost((int) PolyArea.Road, 1);
            var normalLength = TryPath(new Vector3(-8949.918f, -133.572f, 83.589f), new Vector3(-8957.4f, 517.3f, 96.3f));
            Console.WriteLine("Shortest possible path: " + normalLength);

            Assert.IsTrue(roadLength > normalLength);
        }

        [Test]
        public void UndeadStartingArea01()
        {
            TryPath(new Vector3(1676.7f, 1678.1f, 121.6f), new Vector3(1680.3f, 1664.9f, 135.2f));
        }

        [Test]
        public void UndeadStartingArea02()
        {
            TryPath(new Vector3(1676.7f, 1678.1f, 121.6f), new Vector3(1843.5f, 1641.2f, 97.6f));
        }
    }
}