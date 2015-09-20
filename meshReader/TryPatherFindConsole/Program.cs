using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using meshReaderTest;
using TryPatherFindConsole.Properties;

namespace TryPatherFindConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var t = new PandariaMeshTest();
            //var t = new AzerothMeshTest();
            //var t = new KalidmorMeshTest();
            //
            //t.Initialize(Settings.Default.MeshesPath, Settings.Default.WoWPath);
            //t.TestNeo();

            //Console.WriteLine("=================================");
            //Console.WriteLine("=================================");

            var u = new VisualizerTest();
            u.Initialize(Settings.Default.MeshesPath, Settings.Default.WoWPath);
            u.TestPathImageWestern(); 
            u.TestPathImageDraenor();
            u.TestPathImageKalimdor();
            
            //var u = new NorthrendMeshTest();
            //u.Initialize(Settings.Default.MeshesPath, Settings.Default.WoWPath);
            //u.TestNeo();

            //var u = new TileCrossingTest();
            //u.CrossingTest();

            //t.TestFlightpathes();
            //t.OrcStartToCrossroads();
            //Console.Read();
        }
    }
}
