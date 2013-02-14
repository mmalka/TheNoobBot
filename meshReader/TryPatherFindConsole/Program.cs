using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using meshReaderTest;

namespace TryPatherFindConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new AzerothMeshTest();
            //var t = new KalidmorMeshTest();
            //
            t.Initialize();
            t.TestNeo();

            Console.WriteLine("=================================");
            Console.WriteLine("=================================");

            var u = new NorthrendMeshTest();
            u.Initialize();
            u.TestNeo();

            //var u = new TileCrossingTest();
            //u.CrossingTest();

            //t.TestFlightpathes();
            //t.OrcStartToCrossroads();
            //Console.Read();
        }
    }
}
