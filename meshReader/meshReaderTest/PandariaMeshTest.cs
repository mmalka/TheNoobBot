using System;
using System.Linq;
using meshPather;
using SlimDX;
using NUnit.Framework;
using RecastLayer;
using System.IO;

namespace meshReaderTest
{

    [TestFixture]
    public class PandariaMeshTest : MeshTest
    {
        private static string iMeshesPath;

        [TestFixtureSetUp]
        public void Initialize(string MeshesPath, string WowPath)
        {
            iMeshesPath = MeshesPath + "HawaiiMainLand";
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

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        public void TestNeo()
        {
            System.Collections.Generic.List<Hop> walkHops;

            var Place01 = new Vector3(1044.314f, -1238.926f, 179.1343f); // Triger the load of tile 34_30
            var Place02 = new Vector3(1044.677f, -1049.89f, 210.6527f);  // Trigger the load of tile 33_30

            Pather = new Pather(@iMeshesPath, MockConnectionHandler);
            // costs settings
            Pather.Filter.SetAreaCost((int)PolyArea.Water, 4);
            Pather.Filter.SetAreaCost((int)PolyArea.Terrain, 1);
            Pather.Filter.SetAreaCost((int)PolyArea.Road, 1);
            Pather.Filter.SetAreaCost((int)PolyArea.Danger, 20);
            Console.WriteLine("Place01 -> Place02");
            TryPath(Place01, Place02, out walkHops, true);
            WriteFile("Place01-Place02.xml", walkHops, Place02);

            //meshDatabase.MpqManager.Initialize();
            Stream OutFileStream = System.IO.File.OpenWrite("AreaTable.dbc");
            Stream input = meshDatabase.MpqManager.GetFile("DBFilesClient\\AreaTable.dbc");
            CopyStream(input, OutFileStream);
        }


        private static bool MockConnectionHandler(ConnectionData data)
        {
            if (!data.Alliance)
                return false;
            return true;
        }
    
    }
}

