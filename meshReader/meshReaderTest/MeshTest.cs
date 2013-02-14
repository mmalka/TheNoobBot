using System;
using meshPather;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace meshReaderTest
{
    
    public abstract class MeshTest
    {
        protected Pather Pather;

        protected void Initialize(string continent)
        {
            Pather = new Pather(continent);
            Assert.IsFalse(Pather.IsDungeon);
        }

        protected double TryPath(Vector3 start, Vector3 end)
        {
            System.Collections.Generic.List<Hop> discard;
            return TryPath(start, end, out discard, false);
        }

        protected double TryPath(Vector3 start, Vector3 end, out System.Collections.Generic.List<Hop> hops, bool acceptIncomplete)
        {
            //Pather.LoadAllTiles();
            var result = Pather.FindPath(start, end);
            Assert.IsNotNull(result);
            Assert.Greater(result.Count, 0);

            // make sure we didn't get an incomplete path
            if (!acceptIncomplete)
                Assert.Less((end - result[result.Count - 1].Location).Length(), 5f);

            double length = 0;
            for (int i = 0; i < result.Count - 1; i++)
                length += (result[i].Location - result[i + 1].Location).Length();

            Console.WriteLine("Total distance flying : " + (end - start).Length() + ", walking : " + length);
            Console.WriteLine("Distance to end : "+(end - result[result.Count - 1].Location).Length());
/*            foreach (var hop in result)
            {
                float tx, ty;
                Pather.GetTileByLocation(hop.Location.ToRecast().ToFloatArray(), out tx, out ty);
                Console.WriteLine("TX: " + tx + " TY: " + ty + " X: " + hop.Location.X + " Y: " + hop.Location.Y + " Z: " + hop.Location.Z);
            }*/
            Console.WriteLine("Number of hops : " + result.Count);
            Console.WriteLine("Memory: " + (Pather.MemoryPressure / 1024 / 1024) + "MB");
            Console.WriteLine("--------------------------------------------");

            hops = result;
            return length;
        }
    }

}