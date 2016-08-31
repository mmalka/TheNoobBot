using System;
using System.Linq;
using meshPather;
using SlimDX;
using NUnit.Framework;
using RecastLayer;

namespace meshReaderTest
{

    [TestFixture]
    public class NorthrendMeshTest : MeshTest
    {

        [OneTimeSetUp]
        public void Initialize(string MeshesPath, string WowPath)
        {
            string iMeshesPath = MeshesPath + "Northrend";
            Initialize(@iMeshesPath);
        }

        [Test]
        private void WriteFile(string fName, System.Collections.Generic.List<Hop> path, Vector3 target)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("n_" + fName);
            file.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            file.WriteLine("<GrinderProfile xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            file.WriteLine("  <Points>");
            foreach (var hop in path)
            {
                file.WriteLine("    <Point>");
                file.WriteLine("      <X>{0}</X>", hop.Location.X);
                file.WriteLine("      <Y>{0}</Y>", hop.Location.Y);
                file.WriteLine("      <Z>{0}</Z>", hop.Location.Z);
                file.WriteLine("    </Point>");
            }
            if (!target.Equals(path[path.Count - 1].Location))
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
        public void TestNeo()
        {
            System.Collections.Generic.List<Hop> walkHops;

            // Brom Brewbaster <Cooking Trainer> inside the inn / Valgard / Howling Fjord
            var Valgard = new Vector3(615.058f, -4916.97f, 18.7611f);
            // Prospector Belvar / Fort Wildervar / Howling Fjord
            var Wildervar = new Vector3(2421.57f, -5163.51f, 277.272f);
            // Captain Gryan Stoutmantle <Commander of the Westfall Brigade> / Westfall Brigade / Grizzly Hills
            var WestfallBrigade = new Vector3(4603.29f, -4226.95f, 178.645f);
            // Maaka <Flight Master> / Zim'Torga / Zul'Drak
            var ZimTorga = new Vector3(5708.84f, -3598.16f, 387.238f);
            // Roxi Ramrocket <Flying Trainer> / K3 / The Storm Peaks
            var K3 = new Vector3(6178.93f, -1053.07f, 411.088f);
            // Cid Flounderfix <Flight Master> / Moaki Harbor/ Dragonblight
            var MoakiHarbor = new Vector3(2793.19f, 906.36f, 22.5437f);


            Console.WriteLine("Valgard -> Fort Wildervar");
            TryPath(Valgard, Wildervar, out walkHops, true);
            WriteFile("Valgard-Wildervar.xml", walkHops, Wildervar);

            Console.WriteLine("Fort Wildervar -> Westfall Brigade");
            TryPath(Wildervar, WestfallBrigade, out walkHops, true);
            WriteFile("Wildervar-WestfallBrigade.xml", walkHops, WestfallBrigade);

            Console.WriteLine("Westfall Brigade -> Zim'Torga");
            TryPath(WestfallBrigade, ZimTorga, out walkHops, true);
            WriteFile("WestfallBrigade-ZimTorga.xml", walkHops, ZimTorga);

            Console.WriteLine("Zim'Torga -> K3");  // fails
            TryPath(ZimTorga, K3, out walkHops, true);
            WriteFile("ZimTorga-K3.xml", walkHops, K3);

            Console.WriteLine("Big: Valgard -> Zim'Torga"); // fails
            TryPath(Valgard, ZimTorga, out walkHops, true);
            WriteFile("Valgard-ZimTorga.xml", walkHops, ZimTorga);

            Console.WriteLine("Zim'Torga -> Moaki Harbor");
            TryPath(ZimTorga, MoakiHarbor, out walkHops, true);
            WriteFile("ZimTorga-MoakiHarbor.xml", walkHops, MoakiHarbor);

            Console.WriteLine("Valgard -> Moaki Harbor");
            TryPath(Valgard, MoakiHarbor, out walkHops, true);
            WriteFile("Valgard-MoakiHarbor.xml", walkHops, MoakiHarbor);
        }

    }
}